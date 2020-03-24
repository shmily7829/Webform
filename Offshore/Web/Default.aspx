<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background: #efeded">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <title></title>
    <!-- Favicon-->
    <link rel="icon" href="favicon.ico" type="image/x-icon" />

    <!-- Google Fonts -->
    <link href="<%=ResolveClientUrl("~/Css/google_material_icons.css")%>" rel="stylesheet" type="text/css" />


    <!-- Bootstrap Core Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap/css/bootstrap.css")%>" rel="stylesheet" />


    <!-- Waves Effect Css -->
    <link href="<%=ResolveClientUrl("~/plugins/node-waves/waves.css")%>" rel="stylesheet" />

    <!-- Animation Css -->
    <link href="<%=ResolveClientUrl("~/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css")%>" rel="stylesheet" />

    <!-- Morris Chart Css-->

    <!-- Wait Me Css -->

    <!-- Bootstrap Select Css -->
    <!-- Custom Css -->
    <link href="<%=ResolveClientUrl("~/css/style.css")%>" rel="stylesheet" />

    <!-- AdminBSB Themes. You can choose a theme from css/themes instead of get all themes -->
    <link href="<%=ResolveClientUrl("~/css/themes/all-themes.css")%>" rel="stylesheet" />

    <!-- SweetAlert Css -->

    <!-- Jquery Core Js -->
    <script src="<%=ResolveClientUrl("~/plugins/jquery/jquery.min.js")%>"></script>

    <!-- Select Plugin Js -->

    <!-- Slimscroll Plugin Js -->

    <!-- Waves Effect Plugin Js -->

    <!-- Autosize Plugin Js -->

    <!-- Morris Plugin Js -->
    <script src="<%=ResolveClientUrl("~/plugins/momentjs/moment.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/plugins/node-waves/waves.js")%>"></script>

    <!-- Jquery CountTo Plugin Js -->

    <!-- Custom Js -->
    <script src="<%=ResolveClientUrl("~/js/admin.js")%>"></script>

    <!-- sweetalert Js -->

    <!-- Bootstrap Core Js -->
    <script src="<%=ResolveClientUrl("~/plugins/bootstrap/js/bootstrap.js")%>"></script>

    <script src="<%=ResolveClientUrl("~/js/pages/ui/tooltips-popovers.js")%>"></script>

    <script type="text/javascript">
        function shows(val) {
            document.getElementById('I2').src = val;
            return false;
        }
    </script>

    <script type="text/javascript">
        function SetCwinHeight() {

            var iframeid = document.getElementById("I2"); //iframe id  

            //取得子頁高度
            var ChildContentHeight = document.body.scrollHeight;

            //調整最外面那層Iframe的高度
            iframeid.height = iframeid.contentWindow.document.body.scrollHeight;
        }
    </script>
</head>
<body class="theme-cyan" style="background: #efeded">
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <!-- Page Loader -->
        <div class="page-loader-wrapper">
            <div class="loader">
                <div class="preloader">
                    <div class="spinner-layer pl-red">
                        <div class="circle-clipper left">
                            <div class="circle"></div>
                        </div>
                        <div class="circle-clipper right">
                            <div class="circle"></div>
                        </div>
                    </div>
                </div>
                <p>Please wait...</p>
            </div>
        </div>
        <!-- #END# Page Loader -->
        <!-- Overlay For Sidebars -->
        <div class="overlay" style="display: none;"></div>
        <!-- #END# Overlay For Sidebars -->
        <!-- Search Bar -->
        <div class="search-bar">
        </div>
        <!-- #END# Search Bar -->
        <!-- Top Bar -->
        <nav class="navbar">
            <div class="container-fluid" id="container_fluid" style="background-image: url('<%=ResolveClientUrl("~/images/background-blue.png")%>')">
                <div class="navbar-header">
                    <a href="javascript:void(0);" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar-collapse"
                        aria-expanded="false"></a>
                    <a href="javascript:void(0);" class="bars"></a>
                    <asp:ImageButton ID="ImageButton1" CssClass="navbar-brand" runat="server"
                        OnClientClick="return shows('blank.aspx')" ImageUrl="~/Images/ASVTlogo.png" Visible="true" />
                    &nbsp;
                    <a href="javascript:void(0);" class="navbar-brand" style="display: inline-block" onclick="return shows('blank.aspx')">ePolicy System</a>
                </div>
                <div class="collapse navbar-collapse" id="navbar-collapse">
                </div>
            </div>
        </nav>
        <!-- #Top Bar -->
        <section>
            <!-- Left Sidebar -->
            <aside id="leftsidebar" class="sidebar">
                <!-- User Info -->
                <div class="user-info">
                    <div class="image" style="height: 20px;"></div>
                    <div class="info-container">
                        <div class="name" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <asp:Label ID="lbUser" runat="server"></asp:Label>
                        </div>
                        <div class="email">
                            <asp:Label ID="lbEmail" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <!-- #User Info -->
                <!-- Menu -->
                <div class="menu">
                    <ul class="list">
                        <li class="header">主選單</li>
                        <li class='active'>
                            <a href="javascript:void(0)" onclick="return shows('blank.aspx')">
                                <i class='material-icons'>home</i>
                                <span>主頁</span>
                            </a>
                        </li>

                        <asp:Repeater ID="Tree1" runat="server" OnItemDataBound="Tree1_ItemDataBound">
                            <ItemTemplate>
                                <li class='view_list'>
                                    <a href='javascript:void(0);' class='menu-toggle'>
                                        <i class='material-icons'>view_list</i>
                                        <span><%# Eval("FunctionName").ToString()%></span>
                                    </a>
                                    <asp:Repeater ID="Tree2" runat="server">
                                        <HeaderTemplate>
                                            <ul class='ml-menu'>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <a href="javascript:void(0)" onclick="return shows('<%# Eval("ProgramPath").ToString()%>')">
                                                    <%# Eval("FunctionName").ToString()%></a>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <!-- #Menu -->
                <!-- Footer -->
                <div class="legal">
                    <div class="copyright">
                    </div>
                    <div class="version">
                        <b>Version: </b>1.0.0
                    </div>
                </div>
                <!-- #Footer -->
            </aside>
            <!-- #END# Left Sidebar -->
            <!-- Right Sidebar -->

            <!-- #END# Right Sidebar -->
        </section>

        <section class="content">
            <div class="container-fluid">
                <%--    <div class="block-header">
                    <h2 style="font-family: 'Microsoft JhengHei'">
                        <asp:Label ID="lbHeader" runat="server"></asp:Label>
                    </h2>
                </div>
              <div class="card">
                    <div class="body">--%>
                <iframe id="I2" runat="server" frameborder="0" border="0" cellspacing="0" src="/Forms/OF101.aspx"
                    class="iframe" />
                <%--   </div>
                </div>--%>
            </div>
        </section>
    </form>
</body>
</html>
