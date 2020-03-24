<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forms_CASA7100.aspx.cs" Inherits="Forms_Sys_Forms_CASA7100" StylesheetTheme="ASVT"%>

<%--@ Register Src="~/UserControl/ucCalendar.ascx" TagName="ucCalendar" TagPrefix="uc1" --%>
<%--@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" --%>
<%@ Register Src="~/UserControl/ucDataGridView.ascx" TagPrefix="uc1" TagName="ucDataGridView" %>
<%@ Register Src="~/UserControl/ucJQCalendar.ascx" TagPrefix="uc1" TagName="ucJQCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上傳客戶名單測試</title>
    <link href="../../../Css/DemoStyle.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" src="../../../js/jquery-1.4.2.min.js"></script>

    <style type="text/css">
        .gvPage table, .gvPage tr, .gvPage td {
            border-width: 0px;
        }
    </style>
</head>
<body>
    <form runat="server" id="form1">
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Label ID="Label7" runat="server" Text="上傳Excel檔："></asp:Label>
                <asp:FileUpload ID="fuExcel" runat="server" Width="70%" />
                <asp:Button ID="btnUploadExcel" runat="server" Text="Excel上傳" ToolTip="Excel上傳" OnClick="btnUploadExcel_Click" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" UpdateMode="Conditional">
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadExcel"></asp:PostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </form>
</body>
</html>