<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logon.aspx.cs" Inherits="Logon"
    StylesheetTheme="ASVT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ANZ</title>
    <link href="Css/Logon.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
    
    //帳號密碼重設
    function btnreset_Click()
    {
        document.getElementById("txtUserID").value = "";
        document.getElementById("txtPassword").value = "";
    }

    //檢核帳號或密碼是否有輸入
    function CheckIDorPassword() {
        var id = document.getElementById("txtUserID").value;
        var pw = document.getElementById("txtPassword").value;

        if (id == "" || pw == "") {
            alert("請輸入帳號密碼");
            return false;
        }
        return true;
    }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <br />
    <br />
    <table width="60%" align="center" cellpadding="0" cellspacing="0" class="SystemTable">
        <tr>
            <td align="center">
                <asp:Button ID="Button1" runat="server" Text="1002281" OnClick="Button1_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button2" runat="server" Text="1101386" OnClick="Button2_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button3" runat="server" Text="1101442" OnClick="Button3_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button4" runat="server" Text="1105888" OnClick="Button4_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button5" runat="server" Text="1119533" OnClick="Button5_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button6" runat="server" Text="1119557" OnClick="Button6_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button7" runat="server" Text="1250913" OnClick="Button7_Click" SkinID="btnLogon" />&nbsp;
                <asp:Button ID="Button8" runat="server" Text="1114618" OnClick="Button7_Click" SkinID="btnLogon" />&nbsp;
            </td>
        </tr>
        <tr>
            <td class="BlueBg">
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;" height="20px" class="titlebg">
                <img src="images/logo.png" alt="">
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
        <tr valign="bottom">
            <td class="LineImg" valign="bottom">
                <img src="images/topLine_400.gif" border="0" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <div style="width: 400px;" class="LeftRightLine">
                    <table>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblPromptDescription" runat="server" Text="請輸入您的帳號密碼，系統將開啟工作平台" SkinID="skTextBold"></asp:Label>
                                <img src="images/Line.gif" border="0" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblUserID" runat="server" Text="帳號：" SkinID="skTextBold"></asp:Label>
                                <asp:TextBox runat="server" ID="txtUserID" MaxLength="20" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblPassword" runat="server" Text="密碼：" SkinID="skTextBold"></asp:Label>
                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnLogin" runat="server" Text="登入平台" OnClick="btnLogin_Click" SkinID="btnLogon"
                                    OnClientClick="return CheckIDorPassword();" />
                                <asp:Button ID="btnReset" runat="server" Text="重新輸入" OnClientClick="btnreset_Click(); return false;"
                                    SkinID="btnLogon" />
                                <asp:Button ID="btnUnlock" runat="server" Text="帳號解鎖" SkinID="btnLogon" OnClientClick="window.showModalDialog('Forms/SystemPage/SEC0026.aspx','','dialogWidth:700px; dialogHeight:300px; center=yes; scrollbars=yes')"
                                    Visible="false" />
                                <asp:HiddenField runat="server" ID="hfKey1" />
                                <asp:HiddenField runat="server" ID="hfKey2" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr valign="top">
            <td class="LineImg" valign="top">
                <img src="images/underLine_400.gif" border="0" />
            </td>
        </tr>
        <tr>
            <td>
                <p>
                    &nbsp;</p>
                <p>
                    &nbsp;</p>
            </td>
        </tr>
        <tr>
            <td class="BlueBg">
                <asp:Label runat="server" ID="lblStatus" Text="《 目前尚未登入 》" SkinID="null" CssClass="text01"></asp:Label>
                <br />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="60%" align="center" cellpadding="0" cellspacing="10">
        <tr>
            <td>
                <div style="font-size: 8pt;">
                    This computer system, its network and data contained therein in proprietary. Access
                    to this computer and network is restricted to persons and programs authorised by
                    the business owner only. Access by others is prohibited and unauthorised, and is
                    wrongful under law. Do not proceed if You are not authorised. Any unauthorised access
                    will be prosecuted to the fullest extent of the law.
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>

<script type="text/javascript" language="JavaScript">
<!--
   if (document.all) {
      document.all.txtUserID.focus();
   }
//-->
</script>

</html>
