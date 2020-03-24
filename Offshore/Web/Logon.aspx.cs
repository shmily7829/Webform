using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;

public partial class Logon : basePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!base.IsDEVEnvironment)
            {
                //非開發環境進入此頁直接導入無權限畫面
                Server.Transfer(Request.ApplicationPath + "/LoginFailed.aspx");
                return;
            }
            //    Vista.SEC.Information.UserInfo userInfo = new Vista.SEC.Information.UserInfo();
            //    userInfo.UserID = "1250913";
            //    userInfo.Load();

            //    base.SetSessionInfo("1250913");
            //    ScriptManager.RegisterStartupScript(this, GetType(), "LogonLog", "location.href='Default.aspx';", true);
            //}
            //else
            this.BindControl();
        }
    }

    protected override void BindControl()
    {
        if (base.IsDEVEnvironment)
        {
            lblEnvironmentDescription.Text = "【UAT Environment(測試環境)】";
        }
        else
        {
            lblEnvironmentDescription.Text = "<p>&nbsp;</p>";
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            this.DoLogon();
        }
        catch (Exception ex)
        {
            lblStatus.Text = String.Format("登入錯誤 [{0}]", ex.Message);
        }
    }

    /// <summary>
    /// 登入檢查
    /// </summary>
    private bool DoLogon()
    {
        string strUserID = txtUserID.Text.Trim();
        string strPassWord = txtPassword.Text.Trim();

        bool blnLogonResult = false;

        Vista.SEC.Common.Common cmn = new Vista.SEC.Common.Common();
        string strIsADMode = cmn.GetParamValue("ActiveADValid"); //DB於Parameter建INDEX

        if (strIsADMode == "N" || base.IsDEVEnvironment)
        {
            blnLogonResult = true;

            //需加一段判斷User是否存在的程式
            Vista.SEC.Information.UserInfo userInfo = new Vista.SEC.Information.UserInfo();
            userInfo.UserID = strUserID;
            userInfo.Load();

            blnLogonResult = userInfo.ErrFlag;
        }
        else
        {
            blnLogonResult = this.ADAuthenticate(strUserID, strPassWord);
        }

        //檢查登入的帳戶是否已被鎖住
        if (CheckUserIDIsLock(strUserID))
            return false;

        if (blnLogonResult)
        {
            base.SetSessionInfo(strUserID);

            #region 寫入登入資料
            Vista.SEC.Information.UserLoginLogInfo Info = new Vista.SEC.Information.UserLoginLogInfo();
            Info.UserID = strUserID;
            Info.SystemID = GetSystemID(strUserID);
            Info.SessionID = Session.SessionID;
            Info.IPAddress = Request.UserHostAddress;
            Info.IsSuccess = "Y";
            Info.LoginDate = DateTime.Now;
            // Info.LogoutDate = DateTime.Now.AddMinutes(Session.Timeout);
            Info.ModifiedDate = DateTime.Now;
            Info.Insert();
            #endregion

            //2010.12.21 清空記錄帳密錯誤的Session
            //Session.Remove("LogonFailedUserID");
            //Session.Remove("LogonFailedCount");

            //2010.12.21 將要進入的系統寫至Session
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["SystemID"])))
            {
                Session["APPortalSelectedSystemID"] = Request.QueryString["SystemID"].ToString();
            }

            #region 登入成功後的告知訊息  *上次成功登入的日期與時間 *自上次成功登入後是否有任何登入失敗的紀錄
            DataTable dtLog = Info.GetLastSuccessLogin();
            string StrLogMsg = string.Empty;

            foreach (DataRow dr in dtLog.Rows)
            {
                StrLogMsg += string.Format("上次成功登入時間為{0}，IP為{1}\\n", dr["LoginDate"], dr["IPAddress"]);
            }

            DataTable failRecord = Info.GetFailRecord();

            foreach (DataRow dr in failRecord.Rows)
            {
                StrLogMsg += string.Format("最近一次登入失敗，時間為{0}，IP為{1}", dr["LoginDate"], dr["IPAddress"]);
            }

            if (string.IsNullOrEmpty(StrLogMsg))
            {
                StrLogMsg += "這是您第一次登入本站!";
            }

            #endregion

            ScriptManager.RegisterStartupScript(this, GetType(), "LogonLog", "alert('" + StrLogMsg + "');location.href='Default.aspx';", true);
        }
        else
        {
            lblStatus.Text = "如果忘記密碼，請通知IT協助重設您的LAN ACCOUNT密碼";

            this.LogonFailedCountCheck();

            #region 寫入登入錯誤資料
            Vista.SEC.Information.UserLoginLogInfo InfoFailLog = new Vista.SEC.Information.UserLoginLogInfo();
            InfoFailLog.UserID = strUserID;
            InfoFailLog.SystemID = GetSystemID(strUserID);
            InfoFailLog.SessionID = Session.SessionID;
            InfoFailLog.IPAddress = Request.UserHostAddress;
            InfoFailLog.IsSuccess = "N";
            InfoFailLog.LoginDate = DateTime.Now;
            InfoFailLog.Insert();
            #endregion
        }

        return blnLogonResult;
    }

    /// <summary>
    /// AD帳號驗證
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    public bool ADAuthenticate(string UserID, string Password)
    {
        GetKeysFromIni();

        bool blnRtn = false;
        //string strLdapHost = "";
        //Vista.SEC.Common.Common cmn = new Vista.SEC.Common.Common();
        //取得AD主機參數
        //strLdapHost = cmn.GetParamValue("ADServer"); //DB於Parameter建INDEX

        try
        {
            Vista.SEC.Information.SecPassword myPassword = new Vista.SEC.Information.SecPassword();
            myPassword.UserID = UserID;
            myPassword.Load();

            if (myPassword.ErrFlag)
            {
                string strValue = DecryptStringWithCurrentKeys(myPassword.UserPassword);
                if (Password == strValue)
                {
                    Vista.SEC.Information.UserInfo userInfo = new Vista.SEC.Information.UserInfo();
                    userInfo.UserID = UserID;
                    userInfo.Load();

                    blnRtn = userInfo.ErrFlag;
                }
                else
                    blnRtn = false;
            }
            else
            {
                blnRtn = false;
            }

            //OleDbConnection cn = new OleDbConnection(String.Format("Provider=ADSDSOObject;User ID={0};Password={1};Encrypt Password=True;ADSI Flag=1", UserID, Password));
            //cn.Open();
            //OleDbCommand cmd = new OleDbCommand(String.Format("<LDAP://{0}>;(&(objectClass=user)sAMAccountName={1});distinguishedName;subtree", strLdapHost, UserID), cn);
            //OleDbDataReader dr = cmd.ExecuteReader();
            //blnRtn = dr.HasRows;
            //AD驗證通過，檢查徵審系統SEC_USER是否存在
            //if (blnRtn)
            //{
            //    //需加一段判斷User是否存在的程式
            //    Vista.SEC.Information.UserInfo userInfo = new Vista.SEC.Information.UserInfo();
            //    userInfo.UserID = UserID;
            //    userInfo.Load();
            //    blnRtn = userInfo.ErrFlag;
            //}
            //dr.Close();
            //cn.Close();
        }
        catch
        {
            return false;
            //throw new Exception("進行AD驗證時發生錯誤!!");
        }

        return blnRtn;
    }

    /// <summary>
    /// 2010.12.21 判斷帳密輸入錯誤次數，若錯誤第二次則提示訊息
    /// 2012.3月 檢核資料庫錯誤紀錄，錯誤四次鎖定帳號
    /// </summary>
    private void LogonFailedCountCheck()
    {
        Vista.SEC.Information.UserLoginStatusInfo uls = new Vista.SEC.Information.UserLoginStatusInfo();
        uls.UserID = txtUserID.Text.Trim();
        uls.Load();

        //取得UserLoginStatus資料表內是否有目前登入的使用者登入失敗紀錄
        if (uls.ErrFlag) //有目前使用者登入失敗的紀錄
        {
            //判斷該使用者的帳戶是否被鎖住或是登入失敗紀錄已達3次(本次登入失敗是第4次紀錄，登入失敗第4次將會鎖住帳戶)
            if (uls.IsLock || uls.ErrorTimes == 3)
            {
                //判斷帳戶鎖住的IsLock是否已被更新為True；否則更新IsLock為True
                if (!uls.IsLock)
                {
                    uls.UserID = txtUserID.Text.Trim();
                    uls.ErrorTimes = 4;
                    uls.IsLock = true;
                    uls.Update();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "LockAlert", "alert('您的帳號已被鎖定，請到解鎖功能進行解鎖');", true);
            }
            else
            {
                //更新登入失敗次數的紀錄
                uls.UserID = txtUserID.Text.Trim();
                uls.ErrorTimes = uls.ErrorTimes + 1;
                uls.IsLock = false;
                uls.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "LockAlert", "alert('使用者ID或密碼無效，請在試一次。\\n(Invalid Bank ID or Password. Please try again)');", true);
            }
        }
        else //目前登入的使用者帳戶為第1次登入失敗
        {
            //插入登入失敗的紀錄
            uls.UserID = txtUserID.Text.Trim();
            uls.ErrorTimes = 1;
            uls.IsLock = false;
            uls.Insert();
            ScriptManager.RegisterStartupScript(this, GetType(), "LockAlert", "alert('使用者ID或密碼無效，請在試一次。\\n(Invalid Bank ID or Password. Please try again)');", true);
            //if (Session["LogonFailedUserID"] != null)
            //{
            //    if (Session["LogonFailedUserID"].ToString() == txtUserID.Text.Trim()) //同一錯誤帳號
            //    {
            //        Session["LogonFailedCount"] = Convert.ToInt32(Session["LogonFailedCount"]) + 1;
            //        uls.UserID = Session["LogonFailedUserID"].ToString();
            //        uls.ErrorTimes = Convert.ToInt32(Session["LogonFailedCount"]);
            //        uls.IsLock = false;
            //        uls.Update();
            //    }
            //    else //不同錯誤帳號，重計
            //    {
            //        Session["LogonFailedUserID"] = txtUserID.Text.Trim();
            //        Session["LogonFailedCount"] = 1;

            //        uls.UserID = Session["LogonFailedUserID"].ToString();
            //        uls.ErrorTimes = 1;
            //        uls.IsLock = false;
            //        uls.Insert();
            //    }

            //    //提示
            //    if (Convert.ToInt32(Session["LogonFailedCount"]) == 4)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "LockAlert", "alert('帳密輸入錯誤已達三次，再輸入錯誤則可能導致此帳戶被鎖住！請注意！');", true);
            //    }
            //}
            //else //第一次登入失敗
            //{
            //    Session["LogonFailedUserID"] = txtUserID.Text.Trim();
            //    Session["LogonFailedCount"] = 1;

            //    uls.UserID = Session["LogonFailedUserID"].ToString();
            //    uls.ErrorTimes = 1;
            //    uls.IsLock = false;
            //    uls.Insert();
            //}
        }
    }

    /// <summary>
    /// 檢核目前的UserID是否已被鎖住
    /// </summary>
    /// <returns>True:帳戶被鎖住；False:帳戶沒有被鎖住</returns>
    private bool CheckUserIDIsLock(string userID)
    {
        Vista.SEC.Information.UserLoginStatusInfo uls = new Vista.SEC.Information.UserLoginStatusInfo();
        uls.UserID = userID;
        uls.Load();

        if (uls.ErrFlag)
        {
            if (uls.IsLock)
            {
                #region 寫入登入錯誤資料
                Vista.SEC.Information.UserLoginLogInfo InfoFailLog = new Vista.SEC.Information.UserLoginLogInfo();
                InfoFailLog.UserID = userID;
                InfoFailLog.SystemID = GetSystemID(userID);
                InfoFailLog.SessionID = Session.SessionID;
                InfoFailLog.IPAddress = Request.UserHostAddress;
                InfoFailLog.IsSuccess = "N";
                InfoFailLog.LoginDate = DateTime.Now;
                InfoFailLog.Insert();
                #endregion

                ScriptManager.RegisterStartupScript(this, GetType(), "LockAlert", "alert('您的帳號已被鎖定，請到解鎖功能進行解鎖！');", true);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 取得進入系統的SystemID
    /// </summary>
    /// <returns></returns>
    private string GetSystemID(string userID)
    {
        /*
        //取得第一筆有權限的SystemID
        Vista.SEC.Business.Security secBiz = new Vista.SEC.Business.Security();
        DataTable dtSystemID = secBiz.GetSystemList(userID);
        foreach (DataRow dr in dtSystemID.Rows)
        {
            if (secBiz.GetAuthList(userID, 1, dr["SystemID"].ToString().Trim()) as System.Collections.Specialized.StringCollection != null)
            {
                if ((secBiz.GetAuthList(userID, 1, dr["SystemID"].ToString().Trim()) as System.Collections.Specialized.StringCollection).Count == 0)
                {
                    dr.Delete();
                }
            }
        }
        dtSystemID.AcceptChanges();
        string strSystemID = dtSystemID.Rows.Count > 0 ? dtSystemID.Rows[0]["SystemID"].ToString() : string.Empty;

        return strSystemID;
        */
        return ConfigurationManager.AppSettings["SystemID"].ToString(); 
    }

    private void GetKeysFromIni()
    {
        string strIniPath = ConfigurationManager.AppSettings["SecIniPath"].ToString();
        Vista.SEC.Common.INIProcessor iPro = new Vista.SEC.Common.INIProcessor(strIniPath);

        //取值
        Vista.SEC.Coder coder = new Vista.SEC.Coder(); //無引數，表示採用系統預設的Key，兩個Key皆為固定值
        hfKey1.Value = coder.Decrypt(iPro.ReadValue("Main", "Key1"));
        hfKey2.Value = coder.Decrypt(iPro.ReadValue("Main", "Key2"));
    }

    //加密字串
    private string EncryptStringWithCurrentKeys(string strStringToEncrypt)
    {
        Vista.SEC.Coder coder = new Vista.SEC.Coder(hfKey1.Value, hfKey2.Value);

        return coder.Encrypt(strStringToEncrypt);
    }

    //解密字串
    private string DecryptStringWithCurrentKeys(string strStringToDecrypt)
    {
        Vista.SEC.Coder coder = new Vista.SEC.Coder(hfKey1.Value, hfKey2.Value);

        return coder.Decrypt(strStringToDecrypt);
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        txtUserID.Text = ((Button)sender).Text;
        txtPassword.Text = "zaq1xsw@";
        DoLogon();
    }
}
