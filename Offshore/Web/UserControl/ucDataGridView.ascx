<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucDataGridView.ascx.cs" Inherits="ucDataGridView" %>

<div class="row">
    <div class="col-sm-12">
        <div class="dataTables_paginate paging_simple_numbers" style="text-align: center">
            <asp:Button ID="First" runat="server" CommandArgument="First" CommandName="Page" Text="第一頁" OnClick="First_Click" UseSubmitBehavior="false" />
            <asp:Button ID="Prev" runat="server" CommandArgument="Prev" CommandName="Page" Text="上一頁" OnClick="Prev_Click" UseSubmitBehavior="false" />
            <asp:Button ID="Next" runat="server" CommandArgument="Next" CommandName="Page" Text="下一頁" OnClick="Next_Click" UseSubmitBehavior="false" />
            <asp:Button ID="Last" runat="server" CommandArgument="Last" CommandName="Page" Text="最末頁" OnClick="Last_Click" UseSubmitBehavior="false" />
        </div>
    </div>
    <div class="col-sm-12 align-center">
        <div class="dataTables_info" role="status" aria-live="polite">
            第
        <asp:HiddenField ID="hdPage" runat="server" />
            <asp:TextBox ID="txtPage" runat="server" Style="font-size: 12px; text-align: center;"
                OnTextChanged="txtPage_TextChanged" AutoPostBack="true" ToolTip="請輸入數字" BorderStyle="Solid" BorderWidth="1px" Width="46px" Height="30px" />
            頁/共
        <asp:Literal ID="ltPageText" runat="server"></asp:Literal>
            頁，共
        <asp:Literal ID="ltRecordText" runat="server"></asp:Literal>
            筆，每頁
        <asp:HiddenField ID="hdRecordCount" runat="server" Value="15" />
            <asp:DropDownList ID="ddlRecordCount" runat="server" OnSelectedIndexChanged="ddlRecordCount_SelectedIndexChanged"
                AutoPostBack="True">
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem Selected="True">15</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
            </asp:DropDownList>
            筆
        </div>
    </div>
</div>
