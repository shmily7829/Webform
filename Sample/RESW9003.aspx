<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RESW9003.aspx.cs" Inherits="RESW9003" StylesheetTheme="ASVT" %>

<%@ Register Src="~/UserControl/ucDataGridView.ascx" TagPrefix="uc1" TagName="ucDataGridView" %>
<%@ Register Src="~/UserControl/ucJQCalendar.ascx" TagPrefix="uc1" TagName="ucJQCalendar" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <title>代碼選單維護(Maker)</title>

    <!-- Google Fonts -->
    <link href="<%=ResolveClientUrl("~/Css/google_material_icons.css")%>" rel="stylesheet" type="text/css" />

    <!-- Bootstrap Core Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap/css/bootstrap.css")%>" rel="stylesheet" />

    <!-- Waves Effect Css -->
    <link href="<%=ResolveClientUrl("~/plugins/node-waves/waves.css")%>" rel="stylesheet" />

    <!-- Animation Css -->
    <link href="<%=ResolveClientUrl("~/plugins/animate-css/animate.css")%>" rel="stylesheet" />

    <!-- Colorpicker Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css")%>" rel="stylesheet" />

    <!-- Dropzone Css -->
    <link href="<%=ResolveClientUrl("~/plugins/dropzone/dropzone.css")%>" rel="stylesheet" />

    <!-- Multi Select Css -->
    <link href="<%=ResolveClientUrl("~/plugins/multi-select/css/multi-select.css")%>" rel="stylesheet" />

    <!-- Bootstrap Spinner Css -->
    <link href="<%=ResolveClientUrl("~/plugins/jquery-spinner/css/bootstrap-spinner.css")%>" rel="stylesheet" />

    <!-- Bootstrap Tagsinput Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css")%>" rel="stylesheet" />

    <!-- Bootstrap Select Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-select/css/bootstrap-select.csss")%>" rel="stylesheet" />

    <!-- noUISlider Css -->
    <link href="<%=ResolveClientUrl("~/plugins/nouislider/nouislider.min.css")%>" rel="stylesheet" />

    <!-- JQuery Datatable Css -->
    <link href="<%=ResolveClientUrl("~/plugins/jquery-datatable/skin/bootstrap/css/dataTables.bootstrap.css")%>" rel="stylesheet" />

    <!-- Custom Css -->
    <link href="<%=ResolveClientUrl("~/css/style.css")%>" rel="stylesheet" />

    <!-- AdminBSB Themes. You can choose a theme from css/themes instead of get all themes -->
    <link href="<%=ResolveClientUrl("~/css/themes/all-themes.css")%>" rel="stylesheet" />

    <!-- Jquery Core Js -->
    <script src="<%=ResolveClientUrl("~/plugins/jquery/jquery.min.js")%>"></script>

    <!-- Bootstrap Core Js -->
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap/js/bootstrap.js")%>"></script>

    <!-- Select Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap-select/js/bootstrap-select.js")%>"></script>

    <!-- Slimscroll Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/jquery-slimscroll/jquery.slimscroll.js")%>"></script>

    <!-- Bootstrap Colorpicker Js -->
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.js")%>"></script>

    <!-- Dropzone Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/dropzone/dropzone.js")%>"></script>

    <!-- Input Mask Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/jquery-inputmask/jquery.inputmask.bundle.js")%>"></script>

    <!-- Multi Select Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/multi-select/js/jquery.multi-select.js")%>"></script>

    <!-- Jquery Spinner Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/jquery-spinner/js/jquery.spinner.js")%>"></script>

    <!-- Bootstrap Tags Input Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap-tagsinput/bootstrap-tagsinput.js")%>"></script>

    <!-- noUISlider Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/nouislider/nouislider.js")%>"></script>

    <!-- Waves Effect Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/node-waves/waves.js")%>"></script>

    <!-- Custom Js -->
    <script src="<%=ResolveClientUrl("~/js/admin.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/js/pages/forms/advanced-form-elements.js")%>"></script>

    <!-- Demo Js -->
    <script src="<%=ResolveClientUrl("~/js/demo.js")%>"></script>

    <%--以下日期元件所需--%>
    <link rel="stylesheet" type="text/css" href="../../Css/datepicker.css" />

    <%--不一定要這版本的JQuery--%>
    <script type="text/javascript" src="../../js/jquery-1.12.4.js"></script>

    <script type="text/javascript" src="../../js/datepicker.js"></script>
    <script type="text/javascript" src="../../js/datepicker_main.js"></script>
    <script type="text/javascript" src="../../js/jquery.inputmask.bundle.js"></script>

    <%--以下可依語系選擇載入，若使用預設則都無需載入--%>
    <%--<script type="text/javascript" src="../../js/i18n/datepicker.zh-CN.js"></script>--%>
</head>
<body class="theme-cyan" style="background: #efeded">
    <form id="form1" runat="server" style="width: 98%">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel">
            <ContentTemplate>
                <div class="block-header">
                    <h2 style="font-family: 'Microsoft JhengHei'">
                        <asp:Label ID="lbHeader" runat="server" Text="代碼選單維護(Maker)"></asp:Label>
                    </h2>
                </div>

                <!--查詢區塊-->
                <asp:Panel ID="plQuery" runat="server" CssClass="row clearfix">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="card">
                            <div class="header">
                                <h2>查詢條件</h2>
                            </div>
                            <div class="body">
                                <div class="row clearfix">
                                    <div class="col-sm-3">
                                        <b><asp:Label ID="Label2" runat="server" Text="選單類別"></asp:Label></b>
                                        <asp:DropDownList ID="ddlqType" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>代碼值</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtqValue" runat="server" placeholder="代碼" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>代碼名稱</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtqText" runat="server" placeholder="代碼名稱" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>測試日期</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <uc1:ucJQCalendar runat="server" ID="ucJQCalendar" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="button-demo">
                                    <asp:Button ID="btnQry" runat="server" UseSubmitBehavior="false" Text="查詢" OnClick="btnQry_Click" />
                                    <asp:Button ID="btnAdd" runat="server" UseSubmitBehavior="false" Text="新增" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!--清單區塊-->
                <asp:Panel ID="plList" runat="server" CssClass="row clearfix">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="card">
                            <div class="header">
                                <h2>查詢清單</h2>
                            </div>
                            <div class="body">
                                <div class="table-responsive">
                                    <asp:Panel ID="plShowEmpty" runat="server" CssClass="dataTables_wrapper form-inline dt-bootstrap" Visible="false" Width="98%">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <table class="table table-bordered table-striped table-hover" role="grid" aria-describedby="DataTables_Table_0_info">
                                                    <thead>
                                                        <tr>
                                                            <th rowspan="1" colspan="1">無資料可顯示</th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="plShowList" runat="server" CssClass="dataTables_wrapper form-inline dt-bootstrap" Visible="false" Width="98%">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <table class="table table-bordered table-striped table-hover" role="grid" aria-describedby="DataTables_Table_0_info">
                                                    <thead>
                                                        <tr role="row">
                                                            <th rowspan="1" colspan="1" style="width:40px">編輯</th>
                                                            <th rowspan="1" colspan="1">選單類別</th>
                                                            <th rowspan="1" colspan="1">代碼值</th>
                                                            <th rowspan="1" colspan="1">代碼名稱</th>
                                                            <th rowspan="1" colspan="1" style="width:80px">顯示順序</th>
                                                            <th rowspan="1" colspan="1">Value2</th>
                                                            <th rowspan="1" colspan="1">備註</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="repDataList" runat="server" OnItemDataBound="repDataList_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr role="row">
                                                                    <td>
                                                                        <asp:Button ID="rplbtnEdit" runat="server" Text="編輯" UseSubmitBehavior="false" OnClick="rplbtnEdit_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbType" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("Type").ToString())%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbValue" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("Value").ToString())%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbText" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("Text").ToString())%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbDisplayOrder" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("DisplayOrder").ToString())%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbValue2" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("Value2").ToString())%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="rplbRemark" Text='<%# Microsoft.Security.Application.Encoder.HtmlEncode(Eval("REMARKS").ToString())%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <uc1:ucDataGridView runat="server" ID="ucDataGridView1" OnChangePageClick="ucDataGridView1_ChangePageClick" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!--明細區塊-->
                <asp:Panel ID="plDetails" runat="server" CssClass="row clearfix" Visible="false">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="card">
                            <div class="header">
                                <h2>代碼資料</h2>
                            </div>
                            <div class="body">
                                <div class="row clearfix">
                                    <div class="col-sm-12">
                                        <b>資料狀態</b>
                                        <div class="form-group">
                                            <asp:Label ID="lbModItem" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-sm-3">
                                        <b>選單類別</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiType" runat="server" placeholder="選單類別" MaxLength="50" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>代碼值</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiValue" runat="server" placeholder="代碼值" MaxLength="10" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>顯示名稱</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiText" runat="server" placeholder="顯示名稱" MaxLength="800" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <b>顯示順序</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiDisplayOrder" runat="server" placeholder="顯示順序" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-sm-3">
                                        <b>Value2</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiValue2" runat="server" placeholder="Value2" MaxLength="50" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                        <b>備註</b>
                                        <div class="form-group">
                                            <div class="form-line">
                                                <asp:TextBox ID="txtiRemarks" runat="server" placeholder="備註" MaxLength="80" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row clearfix">
                                    <div class="col-sm-12">
                                        <b>測試日期2</b>
                                        <div class="form-group">
                                            <uc1:ucJQCalendar runat="server" ID="ucJQCalendar1" />
                                        </div>
                                    </div>
                                </div>
                                <div class="button-demo">
                                    <asp:Button ID="btnSave" runat="server" UseSubmitBehavior="false" Text="暫存" OnClick="btnSave_Click" OnClientClick="if (!confirm('確定暫存此筆資料?')) return;" />
                                    <asp:Button ID="btnSubmit" runat="server" UseSubmitBehavior="false" Text="送審" OnClick="btnSubmit_Click" OnClientClick="if (!confirm('確定送審此筆資料?')) return;" />
                                    <asp:Button ID="btnDel" runat="server" UseSubmitBehavior="false" Text="刪除" OnClick="btnDel_Click" OnClientClick="if (!confirm('確定申請刪除此筆資料?')) return ;" />
                                    <asp:Button ID="btnDrop" runat="server" UseSubmitBehavior="false" Text="捨棄" OnClick="btnDrop_Click" OnClientClick="if (!confirm('確定捨棄此筆資料?')) return;" />
                                    <asp:Button ID="btnCancel" runat="server" UseSubmitBehavior="false" Text="取消" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
