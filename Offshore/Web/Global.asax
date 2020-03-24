<%@ Application Language="C#" %>

<script RunAt="server">

    private static System.Timers.Timer timer;

    void Application_Start(object sender, EventArgs e)
    {
        // 應用程式啟動時執行的程式碼 

        string SecIniPath = ConfigurationManager.AppSettings["SecIniPath"].ToString();
        string SystemID = ConfigurationManager.AppSettings["SystemID"].ToString();
        #region 設定Key1 and Key2
        Vista.SEC.Coder coder = new Vista.SEC.Coder();
        Vista.SEC.IniUtil INI = new Vista.SEC.IniUtil(SecIniPath);
        Application.Add("SECKey1", coder.Decrypt(INI.ReadValue("Main", "Key1")));
        Application.Add("SECKey2", coder.Decrypt(INI.ReadValue("Main", "Key2")));
        #endregion

        #region 設定Connection Pool
        Vista.DBSSEC.ConnectionPool CP;
        CP = new Vista.DBSSEC.ConnectionPool(SecIniPath);
        CP.SetConnection("RES", "CONNPIPA");
        Application.Add("CONNSEC", Vista.DBSSEC.ConnectionPool.GetConnection("CONNSEC"));
        Application.Add("CONNPIPA", Vista.DBSSEC.ConnectionPool.GetConnection("CONNPIPA"));
        #endregion

    }

    void Application_End(object sender, EventArgs e)
    {
        //  應用程式關閉時執行的程式碼

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 發生未處理錯誤時執行的程式碼
        Utilities.LastError = Server.GetLastError();
    }

    void Session_Start(object sender, EventArgs e)
    {
        // 啟動新工作階段時執行的程式碼
        //Response.Cookies["ASP.NET_SessionId"].Path = "/DARWeb/";
    }

    void Session_End(object sender, EventArgs e)
    {
        // 工作階段結束時執行的程式碼。 
        // 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        // 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        // 或 SQLServer，就不會引發這個事件。

        #region 寫入登出時間
        Vista.SEC.Information.UserLoginLogInfo Info = new Vista.SEC.Information.UserLoginLogInfo();
        Info.UpdateLogoutDate(Convert.ToString(Session["UserID"]), Session.SessionID);
        #endregion

    }


    protected void Application_EndRequest(object sender, EventArgs e)
    {
        /*
        if (Response.Cookies.Count > 0)
        {
            var sessionState = ConfigurationManager.GetSection("system.web/sessionState") as System.Web.Configuration.SessionStateSection;
            var cookieName = sessionState != null && !string.IsNullOrEmpty(sessionState.CookieName) ? sessionState.CookieName : "ASP.NET_SessionId";
            foreach (var key in Response.Cookies.AllKeys)
            {
                if (key == cookieName)
                {
                    Response.Cookies[key].Path = Request.ApplicationPath;
                    break;
                }
            }
        }
        */

        if (Response.Headers.Count > 0)
        {
            System.Collections.Generic.List<string> hkeys = new System.Collections.Generic.List<string>() { "Server", "X-AspNet-Version", "X-Powered-By" };
            foreach (var key in Response.Headers.AllKeys)
            {
                if (hkeys.Contains(key))
                    Response.Headers.Remove(key);
            }
        }
    }

</script>

