<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OF101.aspx.cs" Inherits="Forms_OF101"
    StylesheetTheme="ASVT" %>

<%@ Register Src="../UserControl/ucJQCalendar.ascx" TagName="ucJQCalendar" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"
        name="viewport" />

    <title>一般申購作業</title>    

    <!-- Google Fonts -->
    <link href="<%=ResolveClientUrl("~/Css/google_material_icons.css")%>" rel="stylesheet"
        type="text/css" />

    <!-- Bootstrap Core Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap/css/bootstrap.css")%>" rel="stylesheet" />

    <!-- Waves Effect Css -->
    <link href="<%=ResolveClientUrl("~/plugins/node-waves/waves.css")%>" rel="stylesheet" />

    <!-- Animation Css -->
    <link href="<%=ResolveClientUrl("~/plugins/animate-css/animate.css")%>" rel="stylesheet" />

    <!-- Colorpicker Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css")%>"
        rel="stylesheet" />

    <!-- Dropzone Css -->
    <link href="<%=ResolveClientUrl("~/plugins/dropzone/dropzone.css")%>" rel="stylesheet" />

    <!-- Multi Select Css -->
    <link href="<%=ResolveClientUrl("~/plugins/multi-select/css/multi-select.css")%>"
        rel="stylesheet" />

    <!-- Bootstrap Spinner Css -->
    <link href="<%=ResolveClientUrl("~/plugins/jquery-spinner/css/bootstrap-spinner.css")%>"
        rel="stylesheet" />

    <!-- Bootstrap Tagsinput Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css")%>"
        rel="stylesheet" />

    <!-- Bootstrap Select Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-select/css/bootstrap-select.csss")%>"
        rel="stylesheet" />

    <!-- noUISlider Css -->
    <link href="<%=ResolveClientUrl("~/plugins/nouislider/nouislider.min.css")%>" rel="stylesheet" />

    <!-- Custom Css -->
    <link href="<%=ResolveClientUrl("~/css/style.css")%>" rel="stylesheet" />

    <!-- AdminBSB Themes. You can choose a theme from css/themes instead of get all themes -->
    <link href="<%=ResolveClientUrl("~/css/themes/all-themes.css")%>" rel="stylesheet" />




    <!-- Jquery Core Js -->    
    <script src="<%=ResolveClientUrl("~/plugins/jquery/jquery.js")%>"></script>

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

    <script src="<%=ResolveClientUrl("~/js/jquery-1.12.4.js")%>"></script>
    <link href="<%=ResolveClientUrl("~/Css/datepicker.css")%>" rel="stylesheet" type="text/css" />    
    <script src="<%=ResolveClientUrl("~/js/datepicker.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/js/datepicker_main.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/js/jquery.inputmask.bundle.js")%>"></script>

</head>
<body class="theme-cyan" style="background: #efeded">



    <form id="form1" runat="server" style="width: 99%">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="block-header">
            <h2 style="font-family: 'Microsoft JhengHei'">
                <asp:Label ID="lbHeader" runat="server" Text="一般申購作業"></asp:Label>
            </h2>
        </div>

        <div class="row clearfix">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                <asp:Panel ID="Panel2" runat="server" CssClass="card">
                    <div class="header">
                        <h2>查詢條件
                        </h2>
                    </div>
                    <div class="body">
                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>*下單日</b>
                                <div class="form-group">
                                    <uc1:ucJQCalendar ID="ucJQCalendar2" runat="server" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <b><asp:Label ID="Label1" runat="server" Text="*申購書編號"></asp:Label></b>
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="申購書編號" data-dtp="dtp_VF2Nr" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>客戶代號</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="客戶代號" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="button-demo">
                            <asp:Button ID="Button4" runat="server" UseSubmitBehavior="false" Text="查詢" />
                            <asp:Button ID="Button3" runat="server" UseSubmitBehavior="false" Text="新增" 
                                OnClick="Button3_Click" />
                        </div>
                    </div>
                </asp:Panel>


                <asp:Panel ID="Panel3" runat="server" CssClass="card">
                    <div class="header">
                        <h2>查詢清單
                        </h2>
                    </div>


                    <div class="body">
                        <div class="table-responsive">
                            <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                     
                                <div class="row">
                                    <div class="col-sm-12">
                                        <table class="table table-bordered table-striped table-hover js-basic-example dataTable"
                                            id="DataTables_Table_0" role="grid" aria-describedby="DataTables_Table_0_info">
                                            <thead>
                                                <tr>
                                                    <th rowspan="1" colspan="1">申購書編號</th>
                                                    <th rowspan="1" colspan="1">交易編號</th>
                                                    <th rowspan="1" colspan="1">交易日</th>
                                                    <th rowspan="1" colspan="1">客戶代號</th>
                                                    <th rowspan="1" colspan="1">申購基金</th>
                                                    <th rowspan="1" colspan="1">幣別</th>
                                                    <th rowspan="1" colspan="1">投資金額</th>
                                                    <th rowspan="1" colspan="1">公司手續費率</th>
                                                    <th rowspan="1" colspan="1">代銷手續費率</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr role="row" class="even">
                                                    <td class="sorting_1">測試1</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>2019/01/01</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>TWD</td>
                                                    <td>10000</td>
                                                    <td>1%</td>
                                                    <td>1%</td>
                                                </tr>
                                                <tr role="row" class="odd">
                                                      <td class="sorting_1">XXXXXXXX</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>2019/01/01</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>TWD</td>
                                                    <td>10000</td>
                                                    <td>1%</td>
                                                    <td>1%</td>
                                                </tr>
                                                <tr role="row" class="odd">
                                                     <td>測試3</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>2019/01/01</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>XXXXXXXX</td>
                                                    <td>TWD</td>
                                                    <td>10000</td>
                                                    <td>1%</td>
                                                    <td>1%</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <div class="dataTables_info" id="DataTables_Table_0_info" role="status" aria-live="polite">
                                             第1/1頁，共3筆
                                        </div>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_0_paginate">
                                            <asp:Button ID="Button5" runat="server" UseSubmitBehavior="false" Text="第一頁" Enabled="false" />
                                            <asp:Button ID="Button6" runat="server" UseSubmitBehavior="false" Text="上一頁" Enabled="false" />
                                            <asp:Button ID="Button7" runat="server" UseSubmitBehavior="false" Text="下一頁" Enabled="false" />
                                            <asp:Button ID="Button8" runat="server" UseSubmitBehavior="false" Text="最末頁" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="Panel1" runat="server" CssClass="card" Visible="false">
                    <div class="header">
                        <h2>申購資料
                        </h2>
                    </div>
                    <div class="body">
                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>*下單日</b>
                                <div class="form-group">
                                    <uc1:ucJQCalendar ID="ucJQCalendar1" runat="server" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>*申購書編號</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="申購書編號" data-dtp="dtp_VF2Nr" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>客戶代號</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="客戶代號" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>客戶姓名</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="客戶姓名" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>英文姓</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <asp:TextBox ID="TextBox1" runat="server" placeholder="英文姓" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>英文名</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="英文名" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <b>客戶類別</b>
                                <div class="btn-group bootstrap-select form-control show-tick" style="padding: unset">
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem Value="" Text="-- Please select --" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>*Agent ID</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="Agent ID" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row clearfix">

                            <div class="col-sm-3">
                                <b>Agent 姓名</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="Agent 姓名" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <b>佣金比率</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="佣金比率" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <b>Agent 單位</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="Agent 單位" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>*投顧人員</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="投顧人員" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>投顧姓名</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="投顧姓名" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>佣金比率</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="佣金比率" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>代銷機構</b>
                                    <select class="form-control show-tick">
                                        <option value="">-- Please select --</option>
                                        <option value="10">10</option>
                                        <option value="20">20</option>
                                        <option value="30">30</option>
                                        <option value="40">40</option>
                                        <option value="50">50</option>
                                    </select>
                            </div>

                            <div class="col-sm-3">
                                <b>交易日</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="交易日" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>交易編號</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="交易編號" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>申購基金：ISINCode</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="申購基金：ISINCode" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <b>Fund Code</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="Fund Code" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>幣別</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="幣別" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>投資金額</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="投資金額" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <b>公司手續費率</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="公司手續費率" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>公司手續費</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="公司手續費" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>代銷手續費率</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="代銷手續費率" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row clearfix">
                            <div class="col-sm-3">
                                <b>停損點</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="停損點" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>停益點</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="停益點" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <b>配息轉申購基金</b>
                                <div class="form-group">
                                    <div class="form-line">
                                        <input type="text" class="form-control" placeholder="配息轉申購基金" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="button-demo">
                            <asp:Button ID="Button2" runat="server" UseSubmitBehavior="false" Text="新增" />
                            <asp:Button ID="Button1" runat="server" UseSubmitBehavior="false" Text="取消" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

    </form>

</body>
</html>
