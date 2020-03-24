<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucJQCalendar.ascx.cs" Inherits="ucJQCalendar" %>

<!--以下需搬去aspx或直接將這邊的註解拿掉-->
<%--<link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/css/datepicker.css")%>" />
<script type="text/javascript" src="<%=ResolveClientUrl("~/js/jquery-1.12.4.js")%>"></script>
<script type="text/javascript" src="<%=ResolveClientUrl("~/js/datepicker_main.js")%>"></script>
<script type="text/javascript" src="<%=ResolveClientUrl("~/js/datepicker.js")%>"></script>
<script type="text/javascript" src="<%=ResolveClientUrl("~/js/jquery.inputmask.bundle.js")%>"></script>--%>

<!--若無UpdatePanel則需把下面程式的註解拿掉;反之則需要從Page_Load註冊JS-->
<%--<script type="text/javascript">

    $(function () {

        $("#<%=txtCalendar.ClientID%>").datepicker({
            language: $("#<%=hdCultureName.ClientID%>").val(),
            format: $("#<%=hdDateFormat.ClientID%>").val(),
            autoHide: true
        });

        $("#<%=txtCalendar.ClientID%>").inputmask("datetime", {
            inputFormat: $("#<%=hdDateFormat.ClientID%>").val(),
            outputFormat: $("#<%=hdDateFormat.ClientID%>").val(),
            inputEventOnly: true,
            showMaskOnFocus: false,
            showMaskOnHover: false,
            clearIncomplete: true
        });
    });
</script>--%>


<div class='<%= DivCssClass %>'>
    <asp:TextBox ID="txtCalendar" runat="server" class='<%= TextCssClass %>'></asp:TextBox>
</div>

<asp:HiddenField ID="hdCultureName" runat="server" Value="en-US" />
<asp:HiddenField ID="hdDateFormat" runat="server" Value="yyyy/mm/dd" />
