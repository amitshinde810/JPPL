<%@ Page Title="Tools" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ToolsDefault.aspx.cs" Inherits="Masters_ADD_ToolsDefault" %>

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
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Master
                                </a><span class="after"></span></li>
                                <li id="Toolmstr" runat="server"><a id="A13" href="#" onserverclick="btnToolingMstr_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tooling Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="MonthlyPlan" runat="server"><a id="A1" href="#" onserverclick="btnMonthlyPlan_click"
                                    runat="server"><i class="fa fa-tasks"></i>Monthly Planned Preventive Maintenance
                                    </a> </li>
                                <li id="WeeklyPlan" runat="server"><a id="A7" href="#" onserverclick="btnWeeklyPlan_click"
                                    runat="server"><i class="fa fa-tasks"></i>Weekly Preventive Maintenance</a>                               
                                </li>
                                <li id="BreakDownReg" runat="server"><a id="A8" href="#" onserverclick="btnBrkEntry_click"
                                    runat="server"><i class="fa fa-tasks"></i>Breakdown Entry</a> </li>
                                <li id="ImpromentReg" runat="server"><a id="A9" href="#" onserverclick="btnInproentry_click"
                                    runat="server"><i class="fa fa-tasks"></i>Improvement Entry</a> </li>
                                <li id="TDModel" runat="server"><a id="A10" href="#" onserverclick="btn3DModelnRevision_click"
                                    runat="server"><i class="fa fa-tasks"></i>3D Model storage & request for revision</a>
                                </li>
                                 <li id="PMPending" runat="server"><a id="A15" href="#" onserverclick="btnPMPending_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tools Pending for Preventive Maintenance</a>
                                </li>
                                <li id="TRRefurbish" runat="server"><a id="A16" href="#" onserverclick="btnTRRefurbish_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tools For Refurbish</a>
                                </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="toolingMstrrpt" runat="server"><a id="A11" href="#" onserverclick="btntoolMasterrpt_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tooling Master Report </a></li>
                                <li id="ToolLife" runat="server"><a id="A2" href="#" onserverclick="btntoolLifeMonitering_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tool Life Monitering Sheet</a></li>
                                <li id="PMTrigger" runat="server"><a id="A3" href="#" onserverclick="btnPreMaintenanceTrigger_click"
                                    runat="server"><i class="fa fa-tasks"></i>Monthly Preventive Maintenance Plan Report</a>
                                </li>
                                <li id="PMPerformance" runat="server"><a id="A4" href="#" onserverclick="btnWeeklyPremenPerformance_click"
                                    runat="server"><i class="fa fa-tasks"></i>Weekly Preventive Maintenance Report</a></li>
                                <li id="BrkDwnChart" runat="server"><a id="A5" href="#" onserverclick="btnBreakOccChart_click"
                                    runat="server"><i class="fa fa-tasks"></i>Breakdown Register</a></li>
                                <li id="ImpReg" runat="server"><a id="A12" href="#" onserverclick="btnImproReg_click"
                                    runat="server"><i class="fa fa-tasks"></i>Improvement Register</a></li>
                                <li id="ToolHistoryCard" runat="server"><a id="A6" href="#" onserverclick="btntoolHistoryCard_click"
                                    runat="server"><i class="fa fa-tasks"></i>Tool History Card</a> </li>
                                <li id="ToolsPM" runat="server"><a id="A14" href="#" onserverclick="btntoolForPM_click"
                                    runat="server"><i class="fa fa-tasks"></i>Trigger For Preventive Maintenance</a>                                </li>
                                <li id="PMPerform" runat="server"><a id="A17" href="#" onserverclick="btnPMPerform_click"
                                    runat="server"><i class="fa fa-tasks"></i>Preventive Maintenance Performance Report</a>                                </li>
                                <li id="BreakdownChart" runat="server"><a id="A18" href="#" onserverclick="btnBreakdownChart_click"
                                    runat="server"><i class="fa fa-tasks"></i>Breakdown Occurence chart</a>                                </li>
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

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
</asp:Content>
