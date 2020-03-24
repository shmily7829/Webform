using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Default : basePage
{
    Vista.SEC.Business.Security myBiz = new Vista.SEC.Business.Security();
    Vista.SEC.Business.SystemPageBiz mySysBiz = new Vista.SEC.Business.SystemPageBiz();

    /// <summary>
    /// 取得選擇的系統
    /// </summary>
    private string GetSystemID
    {
        get { return ConfigurationManager.AppSettings["SystemID"].ToString(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(base.strUserID.ToString().Trim()) && base.IsDEVEnvironment)
        {
            Response.Redirect("Logon.aspx");
        }

        if (!IsPostBack)
        {
            lbUser.Text = " " + strUserCName + " " + strUserName;

            DataSet dsTemp;

            if (string.IsNullOrEmpty(GetSystemID))
            {
                dsTemp = myBiz.GetAuthList(base.strUserID, 0) as DataSet;
            }
            else
            {
                dsTemp = myBiz.GetAuthList(base.strUserID, 0, GetSystemID) as DataSet;
            }

            this.BindTree(dsTemp);
        }
    }

    #region TreeView

    DataSet dsMenu;

    /// <summary>
    /// 建立樹狀結構
    /// </summary>
    private void BindTree(DataSet dsTemp)
    {
        dsMenu = dsTemp;

        //根目錄層
        DataTable dt2 = dsMenu.Tables[0].Clone();

        foreach (DataRow dr in dsMenu.Tables[0].Select("OrderBy='00' "))
        {
            dt2.ImportRow(dr);
        }

        Tree1.DataSource = dt2;
        Tree1.DataBind();
    }

    protected void Tree1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string GroupBy = ((DataRowView)e.Item.DataItem)["GroupBy"].ToString();
        Repeater Tree2 = (Repeater)e.Item.FindControl("Tree2");

        //目錄層
        DataTable dt2 = dsMenu.Tables[0].Clone();
        foreach (DataRow dr in dsMenu.Tables[0].Select(" GroupBy ='" + GroupBy + "' AND OrderBy<>'00'"))
        {
            dt2.ImportRow(dr);
        }

        foreach (DataRow dr in dt2.Rows)
        {
            string strTempQ;       // URL QueryString 加密字串
            string strFunctionID = dr["FunctionID"].ToString().Trim();    //取得功能代碼
            string strProgramPath = dr["ProgramPath"].ToString().Trim();  //先取得路徑
            string strProgramPathQueryString = ""; //網址後端所帶的 Querystring

            if (strProgramPath.IndexOf("?") != -1) //如果該網址不具有 Querystring 則不進行 Querystring 加密動作
            {
                strProgramPathQueryString = strProgramPath.Substring(strProgramPath.IndexOf("?") + 1);  //取得 Querystring (不含"?"符號)
                strProgramPath = strProgramPath.Substring(0, strProgramPath.IndexOf("?"));      //再替換成只取網址到 Querystring 前(不含"?"符號)
            }
            strTempQ = base.GetEncryptURLParamenterString(strFunctionID, strProgramPathQueryString);//取代 base.GetEncryptString("strFunctionID=" + Rows[i]["FunctionID"].ToString().Trim());
            dr["ProgramPath"] = strProgramPath + (string.IsNullOrEmpty(strTempQ) ? "" : ("?" + strTempQ));
        }
        dt2.AcceptChanges();

        Tree2.DataSource = dt2;
        Tree2.DataBind();
    }

    #endregion

    protected void btnResetLocked_Click(object sender, EventArgs e)
    {
    }

}
