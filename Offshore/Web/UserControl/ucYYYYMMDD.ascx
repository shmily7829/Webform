<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucYYYYMMDD.ascx.cs" Inherits="UserControl_ucYYYYMMDD" %>


<script>
    $(function () {
        $("#<%=txtCalendar.ClientID%>").inputmask('yyyy/mm/dd', { placeholder: '____/__/__' });
    });
</script>


<div class="input-group">
    <span class="input-group-addon">
        <i class="material-icons">date_range</i>
    </span>
    <div class="form-line">
        <asp:TextBox ID="txtCalendar" runat="server" class="form-control date" placeholder="Ex: 2016/07/31"></asp:TextBox>
    </div>
</div>
