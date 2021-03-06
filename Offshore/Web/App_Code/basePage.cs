﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections.Specialized;

using System.Data.OleDb;
using System.Threading;
using System.Security.Principal;


using System.Collections.Generic;


/// <summary>
/// 網頁基底類別
/// </summary>
public partial class basePage : System.Web.UI.Page
{
    /// <summary>
    /// 列舉狀態
    /// </summary>
    public enum Mode { Nomal, AddNew, Modify, View, Delete, Print, Default, Save, Send, Referral, Reply }

    /// <summary>
    /// 20120420：新增預設 Mode 變數 
    /// </summary>
    protected Mode enumModeStatus
    {
        get
        {
            if (ViewState["enumModeStatus"] == null)
                return Mode.Default;
            else
                return (Mode)ViewState["enumModeStatus"];
        }
        set
        {
            ViewState["enumModeStatus"] = value;
        }
    }

    protected string clientIP
    {
        get
        {
            //判所client端是否有設定代理伺服器
            if (Request.ServerVariables["HTTP_VIA"] == null)
                return Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
    }

    #region  Maker-Checker 使用函式

    #region 狀態宣告

    protected string[] MakerCheckerCode = { "", "A", "D", "U", "R", "A", "R", "P" };  //8 cells
    //改為由 DB 的 DataDictionary 控制 protected string[] MakerCheckerWord = { "Defalut", "新增", "刪除", "修改", "復原", "申請", "退件", "准許" }; //8 cells

    /// <summary>
    /// 20120420: Maker-Checker 狀態列
    /// Default：無狀態
    /// 資料動作：AddNew(A-新增)、Delete(D-刪除)、Update(U-修改)、Recovery(R-還原)
    /// 資料狀態：Apply(A-申請)、Return(R-退件)、Permit(P-准許)
    /// Cell 9  ：Remove 註銷，針對尚未更新到 Production 的資訊進行註銷
    /// </summary>
    protected enum MakerCheckerMode { Default = 0, AddNew = 1, Delete = 2, Update = 3, Recovery = 4, Apply = 5, Return = 6, Permit = 7, Remove = 8 } //9 cells

    #endregion

    /// <summary>
    /// 20120420：新增 Maker - Checker Mode 
    /// </summary>
    protected MakerCheckerMode enumMakerCheckerStatus
    {
        get
        {
            if (ViewState["MakerCheckerStatus"] == null)
                return MakerCheckerMode.Default;
            else
                return (MakerCheckerMode)ViewState["MakerCheckerStatus"];
        }
        set
        {
            ViewState["MakerCheckerStatus"] = value;
        }
    }

    /// <summary>
    /// 20120420：新增 Maker - Checker Mode 
    /// </summary>
    protected MakerCheckerMode enumActionStatus
    {
        get
        {
            if (ViewState["ActionStatus"] == null)
                return MakerCheckerMode.Default;
            else
                return (MakerCheckerMode)ViewState["ActionStatus"];
        }
        set
        {
            ViewState["ActionStatus"] = value;
        }
    }

    /// <summary>
    /// 20120420：新增 Maker - Checker Mode 
    /// </summary>
    protected MakerCheckerMode enumCheckingStatus
    {
        get
        {
            if (ViewState["CheckingStatus"] == null)
                return MakerCheckerMode.Default;
            else
                return (MakerCheckerMode)ViewState["CheckingStatus"];
        }
        set
        {
            ViewState["CheckingStatus"] = value;
        }
    }

    /// <summary>
    /// 傳入代碼得到 Checker 列舉確認狀態，共有四種。
    /// "A":申請(Apply)、"R":退件(Return)、"P":准許(Permit)，如都不符合則回傳 Default
    /// </summary>
    /// <param name="MakerCheckerStatus">A,R,P,D</param>
    /// <returns></returns>
    protected MakerCheckerMode GetCheckeStatus(string MakerCheckerStatus)
    {
        switch (MakerCheckerStatus)
        {
            case "A": //申請
                return MakerCheckerMode.Apply;
            case "R": //退件
                return MakerCheckerMode.Return;
            case "P": //准許
                return MakerCheckerMode.Permit;
            default:  //nothing
                return MakerCheckerMode.Default;
        }
    }

    /// <summary>
    /// 傳入代碼得到 Maker 列舉確認狀態，共有五種。
    /// "A": 新增(AddNew)、"D": 刪除(Delete)、"U": 修改(Update)、"R": 復原(Recovery)，如都不符合則回傳 Default
    /// </summary>
    /// <param name="MakerCheckerStatus"></param>
    /// <returns></returns>
    protected MakerCheckerMode GetActionStatus(string MakerCheckerStatus)
    {
        switch (MakerCheckerStatus)
        {
            case "A": //新增
                return MakerCheckerMode.AddNew;
            case "D": //刪除
                return MakerCheckerMode.Delete;
            case "U": //修改
                return MakerCheckerMode.Update;
            case "R": //復原
                return MakerCheckerMode.Recovery;
            default:  //nothing
                return MakerCheckerMode.Default;
        }
    }

    protected void SwitchMakerCheckerMethod(MakerCheckerMode Status)
    {
        //20120425: 請注意千萬不要把 DoDefault() 改成 this.DoDefault(); ，改了會有大error 
        //下列函式皆為 overwrite Method 
        switch (Status)
        {
            case MakerCheckerMode.Default:
                DoDefault();
                break;
            case MakerCheckerMode.AddNew:
                DoAddNew();
                break;
            case MakerCheckerMode.Delete:
                DoDelete();
                break;
            case MakerCheckerMode.Update:
                DoUpdate();
                break;
            case MakerCheckerMode.Recovery:
                DoRecovery();
                break;
            case MakerCheckerMode.Apply:
                DoApply();
                break;
            case MakerCheckerMode.Return:
                DoReturn();
                break;
            case MakerCheckerMode.Permit:
                DoPermit();
                break;
            case MakerCheckerMode.Remove:
                DoRemove();
                break;
            default:
                DoDefault();
                break;
        }
    }

    #region Maker-Checker 宣告提供子類別 overwrite 函數定義

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoDefault() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoAddNew() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoDelete() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoUpdate() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoRecovery() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoApply() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoReturn() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoPermit() { }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DoRemove() { }

    #endregion

    #endregion

    #region 標準宣告函數定義

    /// <summary>
    /// //設定頁面資訊標準宣告函數
    /// </summary>
    protected virtual void SetFormInfo() { }

    /// <summary>
    /// //載入基本Control標準宣告函數
    /// </summary>
    protected virtual void LoadControl() { }

    /// <summary>
    /// 畫面控制標準宣告函數
    /// </summary>
    protected virtual void ShowControl() { }

    /// <summary>
    /// 資料Binding至控制項標準函數
    /// </summary>
    protected virtual void BindData() { }

    /// <summary>
    /// 載入資料至dataSet標準函數
    /// </summary>
    protected virtual void LoadData() { }

    /// <summary>
    /// 載入表單控制項初始值標準函數

    /// </summary>
    protected virtual void LoadDefaultData() { }

    /// <summary>
    /// 儲存資料標準函數
    /// </summary>
    protected virtual void SaveData() { }

    /// <summary>
    /// 畫面控制項Binding標準宣告函數
    /// </summary>
    protected virtual void BindControl() { }

    /// <summary>
    /// 畫面載入Report標準宣告函數
    /// </summary>
    protected virtual void LoadReport() { }

    #endregion

    #region 共用變數區

    protected string strFunctionParaValue = "strFunctionParaValue";

    public string strErr = "";

    /// <summary>
    /// 來源位址
    /// </summary>
    protected string strFromIP
    {
        get
        {
            return Request.UserHostAddress;
        }
    }

    /// <summary>
    /// 來源電腦
    /// </summary>
    protected string strFromHostName
    {
        get
        {
            System.Net.IPHostEntry ine = System.Net.Dns.GetHostByAddress(this.strFromIP);
            return ine.HostName;
        }
    }

    /// <summary>
    /// 使用者編號
    /// </summary>
    protected string strUserID
    {
        get
        {
            if (Session["UserID"] != null)
                return Session["UserID"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 使用者英文名稱
    /// </summary>
    protected string strUserName
    {
        get
        {
            if (Session["UserName"] != null)
                return Session["UserName"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 使用者中文名稱
    /// </summary>
    protected string strUserCName
    {
        get
        {
            if (Session["UserCName"] != null)
                return Session["UserCName"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 部門中文名稱
    /// </summary>
    protected string strDeptCName
    {
        get
        {
            if (Session["UserDeptCName"] != null)
                return Session["UserDeptCName"].ToString().Trim();
            else
                return string.Empty;
        }

    }

    /// <summary>
    ///  程式執行Sec FunctionID
    /// </summary>
    protected string strSecFunctionID
    {
        get
        {
            if (Session["SecFunctionID"] != null)
                return Session["SecFunctionID"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected string strRecentVolumeID
    {
        set { Session["strRecentVolumeID"] = value; }
        get
        {
            if (Session["strRecentVolumeID"] != null)
                return Session["strRecentVolumeID"].ToString();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 網址列加密之Key1(Key)
    /// </summary>
    protected string strKey1
    {
        get
        {
            if (Session["strKey1"] != null)
                return Session["strKey1"].ToString();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 網址列加密之Key2(IV)
    /// </summary>
    protected string strKey2
    {
        get
        {
            if (Session["strKey2"] != null)
                return Session["strKey2"].ToString();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 部門編號
    /// </summary>
    protected string strUserDeptID
    {
        get
        {
            if (Session["UserDeptID"] != null)
                return Session["UserDeptID"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 部門名稱
    /// </summary>
    protected string strUserDeptName
    {
        get
        {
            if (Session["UserDeptName"] != null)
                return Session["UserDeptName"].ToString().Trim();
            else
                return string.Empty;
        }
    }

    /// <summary>
    /// 功能權限表(SECW0001, CTIW1110, CTIW1115...etc)
    /// </summary>
    protected StringCollection scAuthList
    {
        get
        {
            if (Session["AuthList"] != null)
                return Session["AuthList"] as StringCollection;
            else
                return new StringCollection();
        }
    }

    /// <summary>
    /// 解密模式
    /// 2012.05.28: Jimull
    /// </summary>
    protected string DecryptMode
    {
        get
        {
            if (Request.QueryString.GetValues("Type") != null)
            {
                return Request.QueryString.GetValues("Type")[0];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// [ 可覆寫 ] 取得網頁FormID
    /// 2012.05.28: Jimull
    /// 注意：覆寫此變數的時機為"該網頁為提供給外部廠商使用"，不然請盡量不要覆寫
    /// </summary>
    protected virtual string FormID
    {
        get
        {
            if (ViewState["FormID"] != null)
                return ViewState["FormID"].ToString();
            else
                return string.Empty;
        }

    }

    /// <summary>
    /// 取得 Querystring 參數字串
    /// 2012.05.28: Jimull
    /// </summary>
    protected Dictionary<string, string> QueryStringParameter
    {
        get
        {
            if (ViewState["QueryStringParameter"] != null)
                return (Dictionary<string, string>)ViewState["QueryStringParameter"];
            else
                return new Dictionary<string, string>();
        }
    }

    private Dictionary<string, string> GetQueryStringParameter(string strValue)
    {
        string[] paramenterSet = strValue.Split('&');

        Dictionary<string, string> collection = new Dictionary<string, string>();
        string[] Cell;

        foreach (string item in paramenterSet)
        {
            Cell = item.Split('=');
            if (Cell.Length == 2)
            {
                if (!collection.ContainsKey(Cell[0]))
                {
                    collection.Add(Cell[0].Trim(), Cell[1].Trim());
                }
            }
        }

        return collection;
    }

    /// <summary>
    /// 如果在下列開發環境則不進行 windows 驗證 [True: 不進行 windows, false: 使用 windows]
    /// </summary>
    protected bool IsDEVEnvironment
    {
        get
        {
            return (true);
        }
    }

    #endregion

    #region 基本函數

    /// <summary>
    /// 是否為例外網頁。
    /// </summary>
    private bool IsSpecialPage
    {
        get
        {
            string RequestPage = Request.FilePath;
            return (VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("default.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("itask.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("logon.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("sysmenu.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("systembar.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("ciadlink.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("download.aspx") // 20120710: 因為 download 有繼承 basepage，但不適合將此頁面加入權限控制
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("scform.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("ctiw1162.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("ctiw1114.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("jcicreport.aspx")
                || VirtualPathUtility.GetFileName(RequestPage).ToLower().Equals("DARW0502.aspx")
                );
        }
    }

    private void SettingQuerystringSession()
    {
        ViewState["FormID"] = string.Empty; //20120726 取消此註解。 每次進來都先做清除已便強制繼承此 basepage 的頁面進行權限驗證

        if (Request.QueryString.GetValues(strFunctionParaValue) != null)
        {
            ViewState["QueryStringParameter"] = GetQueryStringParameter(this.GetUrlParamValueMode(Request.QueryString.GetValues(strFunctionParaValue)[0], string.Empty));

            if (QueryStringParameter.ContainsKey("strFunctionID"))
            {
                ViewState["FormID"] = QueryStringParameter["strFunctionID"];
            }
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.SettingQuerystringSession();

        if (VirtualPathUtility.GetFileName(Request.FilePath).ToLower().Equals("default.aspx") && !IsDEVEnvironment)
        {
            SetSessionInfo(string.Empty);
        }

        if (!this.IsSpecialPage)
        {
            if (string.IsNullOrEmpty(this.strUserID))
            {
                string strScript = string.Empty;

                if (IsDEVEnvironment)
                {
                    strScript = "alert('連線逾時，請重新登入系統！');window.top.location.href='" + Request.ApplicationPath + "/Logon.aspx';";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Home", "javascript:" + strScript, true);
                    return;
                }
                else
                {
                    if (Request.UrlReferrer == null || !Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/default.aspx"))
                    {
                        //ex. 直接 po 網址，如果是開新視窗/分頁 會常事先抓 UserID 的 session 
                        SetSessionInfo(string.Empty);
                    }
                    else if (Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/default.aspx"))
                    {
                        // 如果在 ASVT 系統內已無 UserID 則進行重新刷新的動作
                        strScript = "parent.document.location.reload();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Home", "javascript:" + strScript, true);
                        return;
                    }
                    else
                    {
                        //目前其餘狀況都先導入無權限畫面
                        Server.Transfer(Request.ApplicationPath + "/LoginFailed.aspx");
                        return;
                    }
                }
            }

            #region 判斷是否 Session Time Out

            if (string.IsNullOrEmpty(this.FormID.Trim()))
            {
                if (Request.UrlReferrer == null || !Request.UrlReferrer.AbsoluteUri.ToLower().Contains("/default.aspx"))
                {
                    //直接開新視窗/分頁 PO 網址 or CIAD(理論上ciad 不會發生) 系統連結進入 ASVT 系統時，出現無權限畫面
                    //目前其餘狀況都先導入無權限畫面
                    ScriptManager.RegisterStartupScript(this, GetType(), "Home", "javascript:alert('Session Timeout! 請關閉視窗後重新登入系統! 謝謝!');", true);
                    Server.Transfer(Request.ApplicationPath + "/LoginFailed.aspx");
                    return;
                }
                else
                {
                    //ASVT 系統內框架
                    string strScript = string.Empty;
                    //  strScript += "parent.document.getElementsByTagName('iframe')[0].src = 'SystemBar.aspx';";
                    strScript += "parent.document.location.reload();";

                    ScriptManager.RegisterStartupScript(this, GetType(), "ReSetSystem", strScript, true);
                    return;
                }
            }

            #endregion

            #region 判斷是否具有權限

            string strErrMsg = string.Empty;

            if (!this.HasFunctionAuthorized(out strErrMsg))
            {
                //Response.Redirect會由Client送出兩次的Request，Server.Transfer則是一次
                //20120720: 在 CTI open dialog 時會再多開出一頁，所以先改用 Server.Transfer 代替 ScriptManager.RegisterStartupScript(this, GetType(), "logon", "javascript:window.location.href='" + Request.ApplicationPath + "/LoginFailed.aspx';", true);
                Server.Transfer(Request.ApplicationPath + "/LoginFailed.aspx");
                return;
            }

            #endregion
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    /// <summary>
    /// 頁面載入處理
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        //設定網頁過期時間
        Response.Cache.SetExpires(DateTime.Now);

        #region 20120726_在ANZ環境的 SSL 憑證下，取消禁止快取。防止 CTI 的 GridView 匯出 EXCEL or CrystalReports  error

        //這只是暫時性處裡，日後註解必須取消
        //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

        #endregion

        base.OnLoad(e);
    }

    /// <summary>
    /// Page_Error 事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Page_Error(object sender, EventArgs e)
    {
    }

    #endregion

    #region 登入&權限

    private void ReSetPageBySessionTimeOut()
    {

    }

    /// <summary>
    /// 設定使用者資料
    /// </summary>
    /// <param name="UserID"></param>
    protected void SetSessionInfo(string UserID)
    {
        #region 20120418: Jimull, ANZ 環境不可使用 Cookie, 如有必要請先向 Account Manager 確認
        //HttpCookie SessionCookie = new HttpCookie("UserID");
        //SessionCookie.Value = UserID;
        //SessionCookie.Expires = DateTime.Now.AddDays(1);
        //Response.Cookies.Add(SessionCookie);
        #endregion
        Vista.SEC.Business.SystemPageBiz mySysBiz = new Vista.SEC.Business.SystemPageBiz();
        Vista.SEC.Information.UserInfo userInfo = new Vista.SEC.Information.UserInfo();
        Vista.SEC.Information.Department UserDept = new Vista.SEC.Information.Department();
        userInfo.UserID = UserID;
        userInfo.Load();

        // 用於需使用 Window 驗證時須進行判斷
        // 如果 Session UserID 遺失
        // 則須重新取得 window 帳戶名稱進行驗證
        // 如果 使用環境為 公司開發環境 DEV_01 將不進行 Window 驗證
        if (!IsDEVEnvironment)
        {
            if (!GetUserInfo(out userInfo))
            {
                //UserInfo 無此人員 window 帳號資訊時會倒入登入失敗畫面
                //ScriptManager.RegisterStartupScript(this, GetType(), "logon", "javascript:window.top.location.href='" + Request.ApplicationPath + "/LoginFailed.aspx';", true);
                Server.Transfer(Request.ApplicationPath + "/LoginFailed.aspx");
                return;
            }
        }

        //20120914: 當 user dpetid = call center 十 把dpetid 改為 Tid (客服中心)
        string callCenterDeptID = string.Empty;
        DataTable dtPara = mySysBiz.GetSystemParameterList("CALL_CENTER_DEPTID", string.Empty);
        if (dtPara.Rows.Count == 1)
        {
            callCenterDeptID = dtPara.Rows[0]["ParameterValue"].ToString();
        }

        //設定User資訊
        Session["UserID"] = userInfo.UserID;
        Session["UserName"] = userInfo.Name;
        Session["UserCName"] = userInfo.CName;
        Session["UserDeptID"] = userInfo.DeptID;

        //取得User的部門
        UserDept.DeptID =  userInfo.DeptID;
        UserDept.Load();
        Session["UserDeptCName"] = UserDept.DeptCName;

        //取得權限清單(使用StringCollection的方式儲存)
        Vista.SEC.Business.Security secBiz = new Vista.SEC.Business.Security();
        StringCollection scFunList = secBiz.GetAuthList(userInfo.UserID, 1) as StringCollection;
        Session["AuthList"] = (object)scFunList;



        //20120418: 每次登入 Key1, Key2 皆會重新產生        
        //設定網址列參數的加密金鑰
        string strTempKey1 = "";
        string strTempKey2 = "";
        Vista.SEC.Coder coder = new Vista.SEC.Coder(out strTempKey1, out strTempKey2);
        Session["strKey1"] = strTempKey1;
        Session["strKey2"] = strTempKey2;

        Session.Timeout = 30; //雖然在這有設定，但是還有其他因素會影響 Session Timeout
    }

    /// <summary>
    /// 權限驗證
    /// </summary>
    /// <param name="ProjectID"></param>
    /// <returns></returns>
    protected bool VerifyRight(string ProjectID)
    {
        //return true;

        bool result = false;

        try
        {
            if (ProjectID.Length > 0 && scAuthList.Contains(ProjectID))
            {
                result = true;
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }

    /// <summary>
    /// 取得 Window 登入User的電腦名稱/帳號資訊驗證 UserInfo 是否有此人員可登入 
    /// </summary>
    /// <returns></returns>
    public bool GetUserInfo(out Vista.SEC.Information.UserInfo userInfo)
    {
        bool result = false;

        userInfo = new Vista.SEC.Information.UserInfo();

        DataTable dtResult = new DataTable();
        AppDomain myDomain = Thread.GetDomain();
        myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
        //ex. JIMULL\Administrator
        if (string.IsNullOrEmpty(myPrincipal.Identity.Name.ToString()) == false)
        {
            string[] Identity = myPrincipal.Identity.Name.ToString().Split('\\');

            userInfo.UserID = Identity[1];
            result = userInfo.Load();
        }

        return result;
    }

    /// <summary>
    /// [可override] 判斷登入者身分層級是否具有指定網頁之權限
    /// P.S. 網頁加密概略分三種
    /// 1. 左側功能清單的項目都會自動加密 formID 於 Quserystring
    /// 2. 當網頁非左側功能清單進入方式時，如果是公司內部系統繼承 basepage ，須手動將導向網頁加上 Querystring(FormID) ， 的加密字串
    /// 3. 當網頁是由外部廠商進入時，也須先把網頁加上 Querystring(FormID) 的加密字串
    /// 如上列方式皆不正確也可覆寫此 method
    /// </summary>
    /// <param name="strErrMsg">例外訊息</param>
    /// <returns>是否具有權限</returns>
    protected bool HasFunctionAuthorized(out string strErrMsg)
    {
        strErrMsg = Request.QueryString[strFunctionParaValue];

        bool result = false;

        try
        {
            if (this.scAuthList.Contains(FormID)) //20120514[Jimull]: 請勿把 FormID 改成 this.FormID ，會導致 override FormID 的頁面失效
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            //防止連加解密都失敗的狀況
            strErrMsg = ex.Message;
        }

        return result;
    }

    #endregion

    #region Script Function


    /// <summary>
    /// 註冊指令碼(檢查框架是否為最上層,非框架頁)
    /// </summary>
    protected void scriptTopLoc()
    {
        string strScript = "if (top.location == self.location) {top.location = '" +
            Request.ApplicationPath + "/Default.aspx';}";

        ClientScriptManager cs = Page.ClientScript;

        if (!cs.IsClientScriptBlockRegistered(this.GetType(), "scriptTopLoc"))
        {
            cs.RegisterClientScriptBlock(this.GetType(), "scriptTopLoc", strScript, true);
        }

    }

    /// <summary>
    /// 註冊指令碼(框架設定)
    /// </summary>
    protected void scriptFramSet()
    {
        ClientScriptManager cs = Page.ClientScript;

        if (!cs.IsClientScriptIncludeRegistered(this.GetType(), "scriptFramSet"))
        {
            cs.RegisterClientScriptInclude("scriptFramSet", Request.ApplicationPath + "/js/Common/FrameSet.js");
        }

    }


    /// <summary>
    /// 提供訊息,For AJAX ,只能在aspx.cs中使用

    /// </summary>
    /// <param name="webPage">傳 this</param>
    /// <param name="key">Key</param>
    /// <param name="AlertMessage">Message</param>
    protected void DoAlertinAjax(System.Web.UI.Page webPage, string key, string AlertMessage)
    {
        ScriptManager.RegisterStartupScript(webPage, GetType(), key, "alert('" + AlertMessage.Replace("'", "\\'").Replace("\r\n", "") + "');", true);
    }

    /// <summary>
    /// 提示訊息
    /// </summary>
    /// <param name="key">鍵值</param>
    /// <param name="AlertMessage">訊息內容</param>
    protected void DoAlertMsg(string key, string AlertMessage)
    {
        ClientScript.RegisterStartupScript(this.GetType(), key, "alert('" + AlertMessage + "');", true);
    }


    /// <summary>
    /// 註冊Javascript到Client
    /// </summary>
    /// <param name="key"></param>
    /// <param name="strScript"></param>
    protected void DoRegisterJS(string strScript)
    {
        string key = "K" + DateTime.Now.ToString("yyyyMMddHHmmss");

        ClientScript.RegisterStartupScript(this.GetType(), key, strScript, true);
    }


    /// <summary>
    /// Reload主頁面

    /// </summary>
    protected void ReloadMainPage()
    {
        if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "ReloadPage"))
        {
            string js = "window.opener.location.reload(true);";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadPage", js, true);
        }
    }

    #endregion

    #region Other Function

    /// <summary>
    /// 設定表格上面的標題列
    /// </summary>
    /// <param name="strTitle">標題名稱</param>
    /// <param name="strSize">標題列大小</param>
    /// <returns>HTML Code</returns>
    protected string getTableTitle(string strTitle, string strSize, string strPath)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(" <br /><table align='center' width='" + strSize + "' border='0' cellpadding='0' cellspacing='0' height='22' >");
        sb.Append("<tr>");
        sb.Append("<td class='Tab'>");
        sb.Append(strTitle + "</td>");
        /*sb.Append("<td valign='top'>");
        sb.Append("<img src='" + strPath + "Images/TabRight.gif' width='25' height='22' alt='' /></td>");
        sb.Append("<td width='100%' align='right' background='" + strPath + "Images/TabBg.gif'>");
        sb.Append(" &nbsp;");
        sb.Append("</td>");*/
        sb.Append("</tr>");
        sb.Append("</table>");

        return sb.ToString();
    }


    /// <summary>
    /// 設定檔案上傳變數
    /// </summary>
    /// <param name="strClientID">UniKey用識別多個檔案上傳UC</param>
    /// <param name="strFileDesc">選取檔案時的說明</param>
    /// <param name="strFileExt">副檔名 *.*, *.???</param>
    /// <param name="blnIsFileMultiUpload">是否為多檔</param>
    protected void SetFileUploadVar(string strClientID, string strFileDesc, string strFileExt, bool blnIsFileMultiUpload)
    {
        //Vista.SEC.Common.Common myFunc = new Vista.SEC.Common.Common();

        //Session[strClientID + "Folder"] = myFunc.GetAppConfig("FileUpload.TmpRootPath") + strUserID + "/" + System.Guid.NewGuid().ToString();
        //Session[strClientID + "FileExt"] = strFileExt;
        //Session[strClientID + "FileDesc"] = strFileDesc;
        //Session[strClientID + "FileMultiUpload"] = blnIsFileMultiUpload;
    }

    #region 下載檔案

    public Dictionary<string, string> DownloadWileList
    {
        get
        {
            if (Session["DownloadWileList"] != null)
                return (Dictionary<string, string>)Session["DownloadWileList"];
            else
                return new Dictionary<string, string>();
        }

        set
        {
            Session["DownloadWileList"] = value;

        }
    }

    /// <summary>
    /// 下載檔案 呼叫 UserControl 之 Download.aspx
    /// ，如中文檔名請勿超過15個中文字，否則將會被系統自動替代檔名
    /// ；下載檔案路徑(相對路徑)，ex. ~/Forms/Report/Excel/xyz.xls ，在此 method 會轉為 Server Side 絕對路徑 D:\ANZSEC\Web\Forms\Report\Excel\xyz.xls
    /// </summary>
    /// <param name="strSN"></param>
    public void DownloadFile(string strFileName)
    {
        string strFullFileName;
        try
        {
            if (strFileName.StartsWith("\\\\") || strFileName.StartsWith("file:"))
                strFullFileName = strFileName;
            else
                strFullFileName = Server.MapPath(strFileName);
        }
        catch
        {
            strFullFileName = strFileName;
        }
        if (System.IO.File.Exists(strFullFileName))
        {
            string WebAddress = GetWebAddress(strFullFileName);

            //CodeRevie新增白名單驗證
            if (DownloadWileList.ContainsValue(WebAddress))
                Response.Redirect(WebAddress, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "illegalURL", "alert('非系統合法路徑，無法下載檔案！');", true);

        }
        else
        {
            //ScriptManager.RegisterStartupScript(this.Page, GetType(), "FileNotExist", "alert('指定的檔案(" + strFullFileName.Replace("\\", "\\\\") + ")不存在！');", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "FileNotExist", "alert('指定的檔案不存在！');", true);
        }

    }

    public string GetWebAddress(string strFullFileName)
    {
        return Microsoft.Security.Application.Encoder.HtmlEncode("~/UserControl/Download.aspx?FN=" + Server.UrlEncode(strFullFileName));
    }

    public void AddDownloadWileList(string Key, String Value)
    {
        Dictionary<string, string> tempDownloadWileList = new Dictionary<string, string>();
        if (DownloadWileList.ContainsKey(Key))
        {
            tempDownloadWileList = DownloadWileList;
            tempDownloadWileList[Key] = Value;
            DownloadWileList = tempDownloadWileList;
        }
        else
        {
            tempDownloadWileList = DownloadWileList;
            tempDownloadWileList.Add(Key, Value);
            DownloadWileList = tempDownloadWileList;
        }
    }

    public static void DownLoadFile(string strFullFileName, string _remoteUri)
    {

    }

    #endregion

    #endregion

    #region 20120418: Jimull 增加對各網址functionID 加密FUNCTION

    /// <summary>
    /// 傳入 Function ID, 依照 FunctionID 以及其網址所附帶的 Querystring 作加密
    /// EX. strFunctionParaValue=加密資訊(strFunctionID=SEC0001)
    /// </summary>
    /// <param name="strFunctionID"></param>
    /// <returns></returns>
    protected string GetEncryptFunctionIDString(string strFunctionID)
    {
        return (strFunctionParaValue + "=" + this.GetEncryptString("strFunctionID=" + strFunctionID));
    }

    /// <summary>
    /// 傳入 Function ID, 完整連結網址，會依照 FunctionID 以及其網址所附帶的 Querystring 作加密
    /// EX. strFunctionParaValue=加密資訊((strFunctionID=SEC0001) & (OpenKeyA=AAA&OpenKeyB=BBB....))
    /// </summary>
    /// <param name="strFunctionID">功能代碼</param>
    /// <param name="strProgramPathQueryString">網址附帶的Querystring</param>
    /// <returns></returns>
    protected string GetEncryptURLParamenterString(string strFunctionID, string strProgramPathQueryString)
    {
        return (strFunctionParaValue + "=" + this.GetEncryptString("strFunctionID=" + strFunctionID + "&" + strProgramPathQueryString));
    }

    #endregion

    # region 網址列參數加解密Function

    /// <summary>
    /// 將傳入之字串加密並回傳加密後之文字(用於將網址後之參數加密)
    /// </summary>
    /// <param name="strQueryList">以「&」符號分隔之參數字串，例："a=abc&b=def&c=ghi"</param>
    /// <returns></returns>
    protected string GetEncryptString(string strQueryList)
    {
        Vista.SEC.Coder coder = new Vista.SEC.Coder(strKey1, strKey2); //用預設的Key加密
        //return coder.Encrypt(strQueryList));

        return Server.UrlEncode(coder.Encrypt(strQueryList));
    }

    /// <summary>
    /// 將傳入之字串解密並回傳解密後之文字(用於將網址後之參數解密)
    /// </summary>
    /// <param name="strQueryStringList">加密後之字串</param>
    /// <returns></returns>
    protected string GetDecryptString(string strEncryptString)
    {
        Vista.SEC.Coder coder = new Vista.SEC.Coder(strKey1, strKey2);
        return coder.Decrypt(strEncryptString);
    }

    /// <summary>
    /// 20120726: 安全的取出加密參數字串，防止直接取出不存在的Key 值而 Error
    /// </summary>
    /// <param name="ParamKey"></param>
    /// <returns></returns>
    protected string GetSafeQueryString(string ParamKey)
    {
        if (this.QueryStringParameter.ContainsKey(ParamKey) &&
            !string.IsNullOrEmpty(this.QueryStringParameter[ParamKey]))
        {
            return this.QueryStringParameter[ParamKey];
        }
        return string.Empty;
    }


    /// <summary>
    /// 取得網址列參數之值
    /// </summary>
    /// <param name="strEncryptString">加密後之字串</param>
    /// <param name="strParamName">網址列參數名稱，(20120528)如果 strParamName 為空值則回傳整段解密後字串</param>
    /// <returns>參數值</returns>
    protected string GetUrlParamValue(string strEncryptString, string strParamName)
    {
        //Vista.SEC.Coder coder = new Vista.SEC.Coder(strKey1, strKey2);        
        //string strUrlParams = coder.Decrypt(strEncryptString);
        string strUrlParams = this.GetDecryptString(strEncryptString);

        if (string.IsNullOrEmpty(strUrlParams))
        {
            #region Exception Log 記錄
            //取得目前MethodName
            System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
            System.Reflection.MethodBase myMethodBase = stackFrame.GetMethod();

            Vista.SEC.Information.LogExpInfo myLogExpInfo = new Vista.SEC.Information.LogExpInfo();
            myLogExpInfo.ClassName = this.GetType().FullName.ToString();
            myLogExpInfo.MethodName = "GetUrlParamValue";
            myLogExpInfo.ErrMsg = "Key1:" + strKey1 + "  Key2:" + strKey2 + " EnString:" + strEncryptString;
            myLogExpInfo.Insert();
            #endregion
        }

        //20120528: 如果 strParamName 為空值則回傳整段解密後字串
        if (string.IsNullOrEmpty(strParamName))
        {
            return strUrlParams;
        }

        bool blnIsParamExist = false;
        string strParamValue = "";
        foreach (string strParam in strUrlParams.Split(new char[] { '&' }))
        {
            if (strParam.Length > 0 && strParam.Contains("="))
            {
                if (strParam.Substring(0, strParam.IndexOf("=")) == strParamName)
                {
                    blnIsParamExist = true;
                    strParamValue = strParam.Substring(strParam.IndexOf("=") + 1, strParam.Length - (strParam.IndexOf("=") + 1));
                }
            }
        }

        if (!blnIsParamExist)
        {
            throw new Exception("\"" + strParamName + "\"參數不存在");
        }

        return strParamValue;
    }

    /// <summary>
    /// [針對由外部廠商加密後 Querystring 進行解密]
    /// 20120522: Jimull 解密由外部廠商系統加密後網址列參數值
    /// 
    /// 原由： 在 ANZ 開會時討論到。外部廠商的 Win Form 系統會有我們的連結頁面網址
    ///       ，在傳遞網址時會夾帶部份 QueryString ，而且該字串必須加密
    ///       。可是外部廠商只有固定金鑰進行加解密，但與本公司網站所產生的金鑰不同(每次登入就會重新產生一組)
    ///       造成由外部廠商加密 QueryString 導進網址列 會在本站台無法解密使用
    ///
    /// 解法：新增此 method 把外部廠商的加密字串進行解密，此金鑰與外部廠商的為一致，可利用此方式進行解密
    /// 
    /// </summary>
    /// <param name="strEncryptString">加密後之字串</param>
    /// <param name="strParamName">網址列參數名稱，(20120528)如果 strParamName 為空值則回傳整段解密後字串</param>
    /// <returns>參數值</returns>
    protected string GetExternalUrlParamValue(string strEncryptString, string strParamName)
    {
        string strUrlParams = string.Empty;
        string strParamValue = string.Empty;

        #region 固定金鑰解密方法

        // 金鑰實體路徑需要自行調整
        string strIniPath = ConfigurationManager.AppSettings["SecIniPath"].ToString();
        Vista.SEC.Coder coderA = new Vista.SEC.Coder();
        Vista.SEC.IniUtil iPro = new Vista.SEC.IniUtil(strIniPath);
        Vista.SEC.Coder coderB = new Vista.SEC.Coder(coderA.Decrypt(iPro.ReadValue("Main", "Key1")), coderA.Decrypt(iPro.ReadValue("Main", "Key2")));

        #endregion

        strUrlParams = coderB.Decrypt(strEncryptString);

        if (string.IsNullOrEmpty(strParamName))
        {
            return strUrlParams;
        }

        bool blnIsParamExist = false;
        foreach (string strParam in strUrlParams.Split(new char[] { '&' }))
        {
            if (strParam.Length > 0 && strParam.Contains("="))
            {
                if (strParam.Substring(0, strParam.IndexOf("=")) == strParamName)
                {
                    blnIsParamExist = true;
                    strParamValue = strParam.Substring(strParam.IndexOf("=") + 1, strParam.Length - (strParam.IndexOf("=") + 1));
                }
            }
        }

        if (!blnIsParamExist)
        {
            throw new Exception("\"" + strParamName + "\"參數不存在");
        }

        return strParamValue;
    }

    /// <summary>
    /// 針對由內部系統、外部系統連結選擇不同的解密方式
    /// 
    /// 使用範例 GetUrlParamValueMode("asdjlasjdiqwlkznlasd", "test")
    /// 會決定是系列兩種其一
    /// 1. GetExternalUrlParamValue(string strEncryptString, string strParamName)
    /// 2. GetUrlParamValue(string strEncryptString, string strParamName)
    /// 
    /// </summary>
    /// <param name="strEncryptString">被加密的字串</param>
    /// <param name="strParamName">要從加密字串取出的參數</param>
    /// <param name="strMode">解祕方式(1: 針對外部廠商連結進來的頁面作解密，其餘皆使用網站的解密方式)</param>
    /// <returns></returns>
    protected string GetUrlParamValueMode(string strEncryptString, string strParamName)
    {
        if (this.DecryptMode.Equals("ExternalA"))
        {
            return GetExternalUrlParamValue(strEncryptString, strParamName);
        }
        else
        {
            return GetUrlParamValue(strEncryptString, strParamName);
        }
    }

    # endregion

    #region GirdView 匯出excel 必要override 的method
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    #endregion
}

/// <summary>
/// [for error page] 在Web程式中自行定義Exception物件
/// </summary>
public class Utilities
{
    public static Exception LastError;
}
