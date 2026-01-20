<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Admin_Default" Title="Administration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupMsg').hide();
            __doPostBack('Button5', e);
        }
        
    </script>

    <!-- BEGIN CONTENT -->
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="tiles">
                <div class="col-md-1">
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePane22">
                    <ContentTemplate>
                        <a href="#" id="comp1" runat="server" onserverclick="btnCompany_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-home"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Company Information
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="A1" runat="server" onserverclick="btnSysConfig_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-home"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        System Configration
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp2" runat="server" onserverclick="btnUserMaster_click">
                            <div class="tile bg-red">
                                <div class="corner">
                                </div>
                                <div class="tile-body">
                                    <i class="fa fa-user"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        User Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp8" runat="server" onserverclick="btnUserRights_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        User Rights
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp3" runat="server" onserverclick="btnUnlockRecords_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-unlock"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Unlock Records
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp12" runat="server" onserverclick="btnDatabaseBackup_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Database Backup
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp4" runat="server" onserverclick="btnCountryMaster_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Country Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp5" runat="server" onserverclick="btnStateMaster_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        State Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp6" runat="server" onserverclick="btnCityMaster_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        City Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp7" runat="server" onserverclick="btnCurrencyMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Currency Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp10" runat="server" onserverclick="btnUnitMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Unit Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="compArea" runat="server" onserverclick="btnAreaMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Area Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp11" runat="server" onserverclick="btnLogMaster_click" visible="true">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Log Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp13" runat="server" onserverclick="btnISOMaster_click" visible="true">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        ISO Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="A2" runat="server" onserverclick="btnRejectionMaster_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Rejection Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp9" runat="server" onserverclick="btnUserMasterReport_click">
                            <div class="tile bg-dark">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        User Master Report
                                    </div>
                                </div>
                            </div>
                        </a>
                        <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                        <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                            OnOkScript="oknumber()" CancelControlID="Button7" Enabled="True" PopupControlID="popUpPanel5"
                            TargetControlID="CheckCondition">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                            <div class="col-md-12">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div style="font-size: medium;" class="captionPopup">
                                            Warning
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <label style="font-size: medium;" class="col-md-12 control-label">
                                                        You Have No Right To View
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-2 col-md-9">
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END CONTENT -->
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

    <script src="../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
