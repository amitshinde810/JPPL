<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SendErrorReportPage.aspx.cs"
    Inherits="Masters_ADD_SendErrorReportPage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Send Error report
                            </div>
                            <div class="tools">
                                <asp:LinkButton ID="btnCancel" CssClass="remove" TabIndex="29" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <label class="col-md-3 control-label text-right">
                                            Module Name
                                        </label>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblModuleName" runat="server" Style="padding-left: 10px;"></asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <label class="col-md-3 control-label text-right">
                                            Function Name
                                        </label>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblFunctionName" runat="server" Style="padding-left: 10px;"></asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <label class="col-md-3 control-label text-right">
                                            Error Disc
                                        </label>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblError" runat="server" Style="padding-left: 10px;"></asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <label class="col-md-3 control-label text-right">
                                            Date & Time
                                        </label>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblDateNtime" runat="server" Style="padding-left: 10px;"></asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                        </div>
                                        <label class="col-md-3 control-label text-right">
                                            IP Address
                                        </label>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblIP" runat="server" Style="padding-left: 10px;"></asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div class="form-actions fluid">
                                        <div class="col-md-offset-4 col-md-9">
                                            <asp:LinkButton ID="btnYES" CssClass="btn dark" TabIndex="4" Visible="false" runat="server">Yes</asp:LinkButton>
                                            <asp:LinkButton ID="btnNO" CssClass="btn dark" Visible="false" TabIndex="5" runat="server">No</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END CONTAINER -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

</asp:Content>
