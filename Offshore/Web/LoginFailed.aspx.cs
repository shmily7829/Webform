using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Security.Principal;

public partial class Form_LoginFailed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = GetUserInfo();
    }


    public string GetUserInfo()
    {
        bool result = false;

        DataTable dtResult = new DataTable();
        AppDomain myDomain = Thread.GetDomain();
        myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
        //ex. JIMULL\Administrator
        if (string.IsNullOrEmpty(myPrincipal.Identity.Name.ToString()) == false)
        {
            string[] Identity = myPrincipal.Identity.Name.ToString().Split('\\');

            return Identity[1];
        }

        return "";
    }
}
