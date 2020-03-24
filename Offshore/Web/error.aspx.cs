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

public partial class error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool HasInner = Utilities.LastError.InnerException == null ? false : true;

        // 取得錯誤訊息
        string strErrMsg;

        // 取得錯誤的NameSpace
        string strErrNameSpace;

        // 取得錯誤的詳細資料
        string strErrDetail;

        // 取得錯誤的頁面URL
        string strErrURL;

        if (Utilities.LastError.InnerException != null)
        {
            // 取得錯誤訊息
            strErrMsg = Utilities.LastError.InnerException.Message;

            // 取得錯誤的NameSpace
            strErrNameSpace = Utilities.LastError.InnerException.Source;

            // 取得錯誤的詳細資料
            strErrDetail = Utilities.LastError.InnerException.StackTrace;
        }
        else
        {
            // 取得錯誤訊息
            strErrMsg = Utilities.LastError.Message;

            // 取得錯誤的NameSpace
            strErrNameSpace = Utilities.LastError.Source;

            // 取得錯誤的詳細資料
            strErrDetail = Utilities.LastError.StackTrace;
        }

        // 取得錯誤的頁面URL
        strErrURL = Request["aspxerrorpath"].ToString();

        lbErrMsg.Text = strErrMsg;
        lbErrNameSpace.Text = strErrNameSpace;
        lbErrDetail.Text = strErrDetail;
        lbErrURL.Text = strErrURL;
    }
}
