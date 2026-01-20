<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs"
    Inherits="Master_ADD_Welcome" Title="Welcome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <!-- BEGIN CONTAINER -->
    <div class="page-container">
        <!-- BEGIN SIDEBAR -->
        <div class="page-sidebar-wrapper">
            <div class="page-sidebar navbar-collapse collapse">
                <!-- BEGIN SIDEBAR MENU -->
                <ul class="page-sidebar-menu">
                    <li class="sidebar-toggler-wrapper">
                        <!-- BEGIN SIDEBAR TOGGLER BUTTON -->
                        <div class="sidebar-toggler hidden-phone">
                            <%--<a href="#portlet-config" data-toggle="modal" class="config"></a>--%>
                        </div>
                        <!-- BEGIN SIDEBAR TOGGLER BUTTON -->
                    </li>
                    <li class="sidebar-search-wrapper"></li>
                    <li class=""><a href="javascript:;"><i class="fa fa-bookmark-o"></i><span class="title">
                        Masters </span><span class="arrow "></span></a>
                        <ul class="sub-menu">
                            <li><a href="">Employee Master</a> </li>
                            <li><a href="">Loan Master</a> </li>
                        </ul>
                    </li>
                    <li class="sidebar-search-wrapper"></li>
                    <li class=""><a href="javascript:;"><i class="fa fa-bookmark-o"></i><span class="title">
                        Transaction</span><span class="arrow "></span></a>
                        <ul class="sub-menu">
                            <li><a href="">Monthly Deduction</a> </li>
                            <li><a href="">Genrate Salary</a> </li>
                        </ul>
                    </li>
                    <li class="sidebar-search-wrapper"></li>
                    <li class=""><a href="javascript:;"><i class="fa fa-bookmark-o"></i><span class="title">
                        Reports</span><span class="arrow "></span></a>
                        <ul class="sub-menu">
                            <li><a href="">Salary Slip</a> </li>
                            <li><a href="">Monthlly Register</a> </li>
                        </ul>
                    </li>
                    <li class=""><a href="../VIEW/ViewLoan.aspx"><i class="fa fa-bookmark-o"></i><span
                        class="title">Loan Master</span><span class="selected "></span><span class="arrow open"></span></a>
                    </li>
                    <li class="active"><a href="../VIEW/ViewBank.aspx"><i class="fa fa-bookmark-o"></i><span
                        class="title">Bank Master</span><span class="selected "></span><span class="arrow open"></span></a>
                    </li>
                </ul>
                <!-- END SIDEBAR MENU -->
            </div>
        </div>
        <!-- END SIDEBAR -->
        <div class="page-content-wrapper">
            <div class="page-content">
            </div>
        </div>
    </div>
    <!-- END CONTAINER -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->

    <script src="../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- /.modal -->
    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- END JAVASCRIPTS -->
</asp:Content>
