<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ANZ - Error Page</title>
    <link href="Css/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="98%" align="left" cellpadding="0" cellspacing="10" class="SystemTable">
            <tr>
                <td class="BlueBg">
                    <p>
                        &nbsp;</p>
                </td>
            </tr>
          
            <tr>
                <td style="text-align: center;" height="20px">
                    <img src="images/ASVTlogo.png" alt="" />
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblEnvironmentDescription" runat="server" Font-Bold="true" Font-Size="16pt"
                        ForeColor="RED"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size:large;">
                    Error：
                </td>
              </tr>
            <tr>
                <td>
                    錯誤訊息：
                    <asp:Label runat="server" ID="lbErrMsg"></asp:Label>
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    錯誤範圍：
                    <asp:Label runat="server" ID="lbErrNameSpace"></asp:Label>
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    詳細資料：
                    <asp:Label runat="server" ID="lbErrDetail"></asp:Label>
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    頁面URL：
                    <asp:Label runat="server" ID="lbErrURL"></asp:Label>
                    <br /><br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
