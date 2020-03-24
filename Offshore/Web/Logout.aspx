<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="Logout" %>


<html>
<head>
    <title>SKB</title>
    <link href="css/style.css" type="text/css" rel="stylesheet">
    <meta http-equiv="Content-Type" content="text/html; charset=big5">
</head>
<body>
    <!-- ImageReady Slices (UP.psd) -->
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <!-- fwtable fwsrc="未命名" fwbase="UP_02.gif" fwstyle="Dreamweaver" fwdocid = "1188643084" fwnested="0" -->
        <tr>
            <td colspan="2">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr class="bg">
                        <td width="27%">
                            <%--<img src="images/logo_rbs_2007.gif" border="0" alt="">--%>
                        </td>
                        <td width="73%" valign="bottom">
                            <table width="600" border="0" align="right" cellpadding="0" cellspacing="0">
                                <!-- fwtable fwsrc="未命名" fwbase="ICON.jpg" fwstyle="Dreamweaver" fwdocid = "337333541" fwnested="0" -->
                                <tr>
                                    <td style="width: 300px;" class ="text01"> </td>
                                    <td valign="bottom">
                                         
                                    </td>
                                    <td valign="bottom">
                                        <div align="right">
                                                <%--<img src="images/arrow02.gif" class="img" alt="" />--%><asp:Label ID="lblLogout" runat="server"
                                                    Text="系統已登出" CssClass="text01"></asp:Label> </div>
                                    </td>
                                    <td valign="bottom">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!-- End ImageReady Slices -->
    
  
  <br /><br /><br /><br /><br /><br /><br /> <div align="center"><font class="text02"><asp:Label ID ="lblMSG" runat="server"></asp:Label></font></div>
    
</body>
</html>
