<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="IRNDefault.aspx.cs"
    Inherits="Masters_ADD_IRNDefault" Title="IRN" %>

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

    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
        <ContentTemplate>
            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
            <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                OnOkScript="oknumber()" CancelControlID="Button7" DynamicServicePath="" Enabled="True"
                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
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
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Masters</a>
                                    <span class="after"></span></li>
                                <li id="StageMaster" runat="server"><a id="A2" href="#" onserverclick="btnRejection_click"
                                    runat="server"><i class="fa fa-tasks"></i>Rejection Stage Master </a></li>
                                <li id="Reason" runat="server"><a href="#" runat="server" id="l1" onserverclick="btnReason_click">
                                    <i class="fa fa-tasks"></i>Reason Master </a></li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transaction</a>
                                    <span class="after"></span></li>
                                <li id="Prod" runat="server"><a id="A4" href="#" onserverclick="btnProd_click" runat="server">
                                    <i class="fa fa-tasks"></i>Production Entry </a></li>
                                <li id="Foundry" runat="server"><a id="A1" href="#" onserverclick="btnFoundry_click"
                                    runat="server"><i class="fa fa-tasks"></i>Foundry IRN Entry </a></li>
                                <li id="Other" runat="server"><a href="#" runat="server" id="A3" onserverclick="btnOther_click">
                                    <i class="fa fa-tasks"></i>Other IRN Entry </a></li>
                                <li id="StageChange" runat="server"><a href="#" runat="server" id="A16" onserverclick="btnStageChange_Click">
                                    <i class="fa fa-tasks"></i>Component Stages Change </a></li>
                                <li id="LineChange" runat="server"><a href="#" runat="server" id="A17" onserverclick="btnLineChange_Click">
                                    <i class="fa fa-tasks"></i>Component Line Change</a></li>
                                <li id="CustRejInward" runat="server"><a href="#" runat="server" id="A26" onserverclick="btnCustRejInward_Click">
                                    <i class="fa fa-tasks"></i>Customer Rejection Inward</a></li>
                                <li id="liCustRej" runat="server"><a href="#" runat="server" id="A22" onserverclick="btnCustRej_Click">
                                    <i class="fa fa-tasks"></i>Customer Rejection</a></li>
                                <li id="liIntrRejCAPA" runat="server"><a href="#" runat="server" id="A23" onserverclick="btnIntrRejCAPA_Click">
                                    <i class="fa fa-tasks"></i>Internal Rejection CAPA</a></li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                    <li id="liProEntryReg" runat="server"><a id="A27" href="#" onserverclick="btnProEntryReg_click"
                                    runat="server"><i class="fa fa-tasks"></i>Production Entry Register</a></li>
                                <li id="DailyIRN" runat="server"><a id="A5" href="#" onserverclick="btnDailyIRN_click"
                                    runat="server"><i class="fa fa-tasks"></i>Daily rejection report</a></li>
                                <li id="RejSummaryR" runat="server"><a id="A6" href="#" onserverclick="btnRejSummary_click"
                                    runat="server"><i class="fa fa-tasks"></i>Cumm rejection summary </a></li>
                                <li id="RejDetailR" runat="server"><a id="A7" href="#" onserverclick="btnRejDetails_click"
                                    runat="server"><i class="fa fa-tasks"></i>Stage wise rejection details</a></li>
                                <li id="DefRejDetilsR" runat="server"><a id="A8" href="#" onserverclick="btnDefectWiseRej_click"
                                    runat="server"><i class="fa fa-tasks"></i>Monthly defect specific rejection details</a>
                                </li>
                                <li id="FdryRejYearlyR" runat="server"><a id="A9" href="#" onserverclick="btnfoundryRejper_click"
                                    runat="server"><i class="fa fa-tasks"></i>foundry rejection month wise</a> </li>
                                <li id="MechRejYearlyR" runat="server"><a id="A10" href="#" onserverclick="btnMecRejPerf"
                                    runat="server"><i class="fa fa-tasks"></i>Machining rejection month wise</a></li>
                                <li id="DefFodryRejYearlyR" runat="server"><a id="A11" href="#" onserverclick="btnDefectWiseFoundRej_click"
                                    runat="server"><i class="fa fa-tasks"></i>Component specific defect specific month
                                    wise foundry rejection details </a></li>
                                <li id="DefMechRejYearlyR" runat="server"><a id="A12" href="#" onserverclick="btnDefectWiseMechRej_click"
                                    runat="server"><i class="fa fa-tasks"></i>Component specific defect specific month
                                    wise machining rejection details</a></li>
                                <li id="DeptWiseR" runat="server"><a id="A13" href="#" onserverclick="btnDeptWise_click"
                                    runat="server"><i class="fa fa-tasks"></i>Company rejection trend process wise</a></li>
                                <li id="RejPerfR" runat="server"><a id="A14" href="#" onserverclick="btnRejPerf_click"
                                    runat="server"><i class="fa fa-tasks"></i>Rejection in ascending order </a></li>
                                <li id="VendorR" runat="server"><a id="A15" href="#" onserverclick="btnVendorRejYrly_click"
                                    runat="server"><i class="fa fa-tasks"></i>Vendor Wise Rejection</a></li>
                                <li id="StageHistoryR" runat="server"><a id="A18" href="#" onserverclick="btnStageWiseHistory_click"
                                    runat="server"><i class="fa fa-tasks"></i>Stage Wise History Report</a></li>
                                <li id="LineHistoryR" runat="server"><a id="A19" href="#" onserverclick="btnLineWiseHistory_click"
                                    runat="server"><i class="fa fa-tasks"></i>Line Wise History Report</a></li>
                                <li id="CompR" runat="server"><a id="A20" href="#" onserverclick="btnComp_click"
                                    runat="server"><i class="fa fa-tasks"></i>Component Stage Wise Report</a></li>
                                <li id="RejSummStageR" runat="server"><a id="A21" href="#" onserverclick="btnRejSummStage_click"
                                    runat="server"><i class="fa fa-tasks"></i>Rejection Summary Stage Wise</a></li>
                                <li id="liEffectiveCAPA" runat="server"><a id="A24" href="#" onserverclick="btnliEffectiveCAPA_click"
                                    runat="server"><i class="fa fa-tasks"></i>Effectiveness Of CAPA</a></li>
                                <li id="InternalCAPA" runat="server"><a id="A25" href="#" onserverclick="btnInternalCAPA_click"
                                    runat="server"><i class="fa fa-tasks"></i>Internal Rejection CAPA Report</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
