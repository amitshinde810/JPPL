<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="PPCDefault.aspx.cs"
    Inherits="Masters_ADD_PPCDefault" Title="DITERP | CP" %>

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
                                <li id="LiGroup" runat="server"><a href="../../PPC/VIEW/ViewGroupMaster.aspx" onserverclick="btnGroup_click">
                                    <i class="fa fa-tasks"></i>Group Master</a> </li>
                                <li id="LiProd" runat="server"><a href="../../PPC/VIEW/ViewProductMaster.aspx" onserverclick="btnProduct_click">
                                    <i class="fa fa-tasks"></i>Product Master</a> </li>
                                <li id="LiStandInv" runat="server"><a href="../../PPC/VIEW/ViewStandardInventoryMaster.aspx"
                                    onserverclick="btnStandard_click"><i class="fa fa-tasks"></i>Standard Inventory
                                    Master</a> </li>
                                <li id="LiReason" runat="server"><a href="../../PPC/VIEW/ViewReasonMaster.aspx" onserverclick="btnReason_click">
                                    <i class="fa fa-tasks"></i>Reason Master</a> </li>
                                <li id="LiProductivity" runat="server"><a href="../../PPC/VIEW/ViewProductivityMaster.aspx"
                                    onserverclick="btnProductivity_click"><i class="fa fa-tasks"></i>Productivity Master</a></li>
                                <li id="LiMachine" runat="server"><a href="../../PPC/VIEW/ViewMachineMaster.aspx"
                                    onserverclick="btnMachine_click"><i class="fa fa-tasks"></i>Machine Master</a></li>
                                <li id="LiProcessMachine" runat="server"><a href="../../PPC/VIEW/ViewProcessMachineMaster.aspx"
                                    onserverclick="btnProcessMachine_click"><i class="fa fa-tasks"></i>Machine Quantity
                                    Master</a> </li>
                                <li id="LiVendorMachine" runat="server"><a href="../../PPC/VIEW/ViewVendorMachineMaster.aspx"
                                    onserverclick="btnVendorMachine_click"><i class="fa fa-tasks"></i>Vendor Machine
                                    Quantity Master</a> </li>
                                <li id="LiBoughtout" runat="server"><a href="../../PPC/VIEW/ViewBoughtoutMaster.aspx"
                                    onserverclick="btnBoughtout_click"><i class="fa fa-tasks"></i>BOM For Boughtout
                                    Master</a> </li>
                                <li id="LiProcessBom" runat="server"><a href="../../PPC/VIEW/ViewProcessBomMaster.aspx"
                                    onserverclick="btnProcessBom_click"><i class="fa fa-tasks"></i>Process Routing Master</a>
                                </li>
                                <li id="LiPallet" runat="server"><a href="../../PPC/VIEW/ViewPalletMaster.aspx" onserverclick="btnPallet_click">
                                    <i class="fa fa-tasks"></i>Pallet Master</a> </li>
                                <li id="LiBox" runat="server"><a href="../../PPC/VIEW/ViewBoxMaster.aspx" onserverclick="btnBox_click">
                                    <i class="fa fa-tasks"></i>Box Master</a> </li>
                                <li id="LiMachiningToolCost" runat="server"><a href="../../PPC/VIEW/ViewMachiningToolCostMaster.aspx"
                                    onserverclick="btnMachiningToolCost_click"><i class="fa fa-tasks"></i>Machining
                                    Tool Cost Master</a> </li>
                                <li id="LiMachineBooking" runat="server"><a href="../../PPC/VIEW/ViewMachineBookingMaster.aspx"
                                    onserverclick="btnMachineBooking_click"><i class="fa fa-tasks"></i>Machine Booking
                                    Master</a> </li>
                                <li id="LiSandMinimumStandardMaster" runat="server"><a href="../../PPC/VIEW/ViewSandMinimumStandardMaster.aspx"
                                    onserverclick="btnSandMinMaster_click"><i class="fa fa-tasks"></i>Sand Minimum Standard
                                    Master</a> </li>
                                <li id="liBOM" runat="server"><a href="../../PPC/VIEW/CPBOM_Master.aspx" onserverclick="btnBOMMaster_click">
                                    <i class="fa fa-tasks"></i>BOM Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="LiCoreInv" runat="server"><a href="../../PPC/VIEW/ViewCoreInventoryTransaction.aspx"
                                    onserverclick="btnCoreInventory_click"><i class="fa fa-tasks"></i>Core Inventory</a>
                                </li>
                                <li id="LiCustomerSchedule" runat="server"><a href="../../PPC/VIEW/ViewCustomerSchedule.aspx"
                                    onserverclick="btnCustomerSchedule_click"><i class="fa fa-tasks"></i>Customer Schedule</a>
                                </li>
                                <li id="LiVendorSchedule" runat="server"><a href="../../PPC/VIEW/ViewVendorSchedule.aspx"
                                    onserverclick="btnVendorSchedule_click"><i class="fa fa-tasks"></i>Vendor Schedule</a>
                                </li>
                                <li id="LiCustomerWeeklyPlan" runat="server"><a href="../../PPC/VIEW/ViewCustomerWeeklyPlan.aspx"
                                    onserverclick="btnCustomerWeeklyPlan_click"><i class="fa fa-tasks"></i>Firm Customer
                                    Weekly Plan</a> </li>
                                <li id="LiVendorWeeklyPlan" runat="server"><a href="../../PPC/VIEW/ViewVendorWeeklyPlan.aspx"
                                    onserverclick="btnVendorWeeklyPlan_click"><i class="fa fa-tasks"></i>Firm Vendor
                                    Weekly Plan</a> </li>
                                <li id="LiDailySaleEntry" runat="server"><a href="../../PPC/VIEW/DailySaleEntryView.aspx"
                                    onserverclick="btnDailySaleEntry_click"><i class="fa fa-tasks"></i>Daily Sale Plan
                                    Entry</a></li>
                                <li id="LiShortSaleEntry" runat="server"><a href="../../PPC/VIEW/ShortSaleEntryView.aspx"
                                    onserverclick="btnShortSaleEntry_click"><i class="fa fa-tasks"></i>Short Sale Entry</a>
                                </li>
                                <li id="LiUnplan" runat="server"><a href="../../PPC/VIEW/ViewunplanSalesSchedule.aspx"
                                    onserverclick="btnunplanSalesSchedule_click"><i class="fa fa-tasks"></i>Unplan Sale
                                    Plan Entry</a></li>
                                <li id="Li2" visible="false" runat="server"><a href="../../PPC/VIEW/ViewUnplanVendorSchedule.aspx"
                                    onserverclick="btnUnplanVendorSchedule_click"><i class="fa fa-tasks"></i>Unplan
                                    Vendor Schedule</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content " id="dvReports" runat="server">
                            <ul class="ver-inline-menu tabbable margin-bottom-25" id="ulReports" runat="server">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="LiMonth" runat="server" visible="true"><a id="A13" href="#" onserverclick="btnMonth_click"
                                    runat="server"><i class="fa fa-tasks"></i>Monthly Report</a>
                                    <ul class="ver-inline-menu tabbable" id="ul1" runat="server">
                                        <asp:Panel ID="pnlMonth" runat="server" Visible="false">
                                            <li id="LiSalePlanningReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewSalePlanningReport.aspx">
                                                <i class="fa fa-tasks"></i>Sale Planning Report</a> </li>
                                            <li id="LiSaleSummaryCustomerwise" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewSaleSummaryCustomerwise.aspx">
                                                <i class="fa fa-tasks"></i>Sale Summary Customerwise Report</a> </li>
                                            <li id="LiSaleSummaryOwnerwise" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewSaleSummaryOwnerwise.aspx">
                                                <i class="fa fa-tasks"></i>Sale Summary Ownerwise Report</a> </li>
                                            <li id="CoreInvReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCoreInventoryRegister.aspx">
                                                <i class="fa fa-tasks"></i>Core Inventory</a> </li>
                                            <li id="LiVendorScheduleReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewVendorScheduleForMachine.aspx">
                                                <i class="fa fa-tasks"></i>Vendor Schedule Register</a> </li>
                                            <li id="LiCustomerScheduleReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCustomerSchedule.aspx">
                                                <i class="fa fa-tasks"></i>Customer Schedule Register</a> </li>
                                            <li id="LiCastingtobeInspectReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingtobeInspectReport.aspx">
                                                <i class="fa fa-tasks"></i>Casting To Be Inspected Report</a> </li>
                                            <li id="LiCastingtobeOffloadedReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingtobeOffloaded.aspx">
                                                <i class="fa fa-tasks"></i>Casting To Be Offloaded Report</a> </li>
                                            <li id="LiCastingtobeCast" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingtobeCast.aspx">
                                                <i class="fa fa-tasks"></i>Casting To Be Cast Report</a> </li>
                                            <li id="LiCastingtobeMachined" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingtobeMachined.aspx">
                                                <i class="fa fa-tasks"></i>Casting To Be Machined Report</a> </li>
                                            <li id="LiCastingInventoryReport" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingInventory.aspx">
                                                <i class="fa fa-tasks"></i>Casting Inventory Report</a> </li>
                                            <li id="LiCorestobeMade" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCorestobeMade.aspx">
                                                <i class="fa fa-tasks"></i>Cores To Be Made Report</a> </li>
                                            <li id="LiPurchaseRequirement" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewPurchaseRequirement.aspx">
                                                <i class="fa fa-tasks"></i>Purchase Requirement Report</a> </li>
                                            <li id="LiRMnSandRequire" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewRMnSandRequire.aspx">
                                                <i class="fa fa-tasks"></i>RM & SAND Requirement </a></li>
                                            <li id="LiRMnSandPurchase" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewRMnSandPurchase.aspx">
                                                <i class="fa fa-tasks"></i>RM & SAND Purchase Requirement </a></li>
                                        </asp:Panel>
                                    </ul>
                                </li>
                                <li id="LiCapcity" runat="server" visible="true"><a id="A1" href="#" onserverclick="btnCapacity_click"
                                    runat="server"><i class="fa fa-tasks"></i>Capacity Report</a>
                                    <ul class="ver-inline-menu tabbable" id="ul2" runat="server">
                                        <asp:Panel ID="pnlCapacity" runat="server" Visible="false">
                                            <li id="LiCoreShopCapacityPlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCoreShopCapacityPlan.aspx">
                                                <i class="fa fa-tasks"></i>Core Shop Capacity Planning Report </a></li>
                                            <li id="LiCastingCapacityPlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingCapacityPlan.aspx">
                                                <i class="fa fa-tasks"></i>Casting Capacity Planning Report </a></li>
                                            <li id="LiInhouseMachineShopCapacityPlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewInhouseMachineShopCapacityPlan.aspx">
                                                <i class="fa fa-tasks"></i>Inhouse Machine Shop Planning Report </a></li>
                                            <li id="LiVendorCapacityPlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewVendorCapacityPlan.aspx">
                                                <i class="fa fa-tasks"></i>Vendor Capacity Planning Report </a></li>
                                            <li id="LiLeakageTestCapacityPlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewLeakageTestCapacityPlan.aspx">
                                                <i class="fa fa-tasks"></i>Leakage Testing Capacity Planning Report </a></li>
                                            <li id="LiFoundryCapBookSummary" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewFoundryCapBookSummary.aspx">
                                                <i class="fa fa-tasks"></i>Foundry Capacity Booking Summary</a></li>
                                            <li id="LiTotMachineCapBookSummary" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewTotMachineCapBookSummary.aspx">
                                                <i class="fa fa-tasks"></i>Total Machining Capacity Booking Summary</a></li>
                                            <li id="LiLeakageBookCapSummary" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewLeakageBookCapSummary.aspx">
                                                <i class="fa fa-tasks"></i>Leakage Capacity Booking Summary</a></li>
                                            <li id="LiWeeklysalePlan" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewWeeklySalePlanningReport.aspx">
                                            </li>
                                        </asp:Panel>
                                    </ul>
                                </li>
                                <li id="LiWeekly" runat="server" visible="true"><a id="A2" href="#" onserverclick="btnWeekly_click"
                                    runat="server"><i class="fa fa-tasks"></i>Weekly Report</a>
                                    <ul class="ver-inline-menu tabbable" id="ul3" runat="server">
                                        <asp:Panel ID="pnlWeekly" runat="server" Visible="false">
                                            <li id="Li1" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewWeeklySalePlanningReport.aspx">
                                                <i class="fa fa-tasks"></i>Weekly Sale Planning</a></li>
                                            <li id="LiCastReqForInsps" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/VewCastingrequiredfrominspectionReport.aspx">
                                                <i class="fa fa-tasks"></i>Castings required from inspection</a></li>
                                            <li id="LiCastReqForMachShop" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingsrequriedformmachineshopReport.aspx">
                                                <i class="fa fa-tasks"></i>Castings requried from machine shop</a></li>
                                            <li id="LiCastReqForVendor" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewcastingReqFromVendor.aspx">
                                                <i class="fa fa-tasks"></i>Castings requried from Vendor</a></li>
                                            <li id="LiCastReqForRFMToMS" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingsrequriedfromRFMformachineshopReport.aspx">
                                                <i class="fa fa-tasks"></i>Castings requried from RFM store for machine shop</a></li>
                                            <li id="LiCastReqForRFMToVndor" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingsrequriedfromrfmforvendorReport.aspx">
                                                <i class="fa fa-tasks"></i>Castings requried from RFM store for vendor</a></li>
                                            <li id="LiCastToBeDisFromFoundry" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCastingstobedispatchedfromfoundryReport.aspx">
                                                <i class="fa fa-tasks"></i>Castings to be dispatched from foundry</a></li>
                                            <li id="LiCastToBeDisFromCoreShop" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/ViewCorestobedispatchedfromcoreshopReport.aspx">
                                                <i class="fa fa-tasks"></i>Cores to be dispatched from core shop</a></li>
                                        </asp:Panel>
                                    </ul>
                                </li>
                                <li id="LiDaily" runat="server" visible="true"><a id="A3" href="#" onserverclick="btnDaily_click"
                                    runat="server"><i class="fa fa-tasks"></i>Daily Report</a>
                                    <ul class="ver-inline-menu tabbable" id="ul4" runat="server">
                                        <asp:Panel ID="pnlDaily" runat="server" Visible="false">
                                            <li id="LiDailySalePerfrpt" runat="server" visible="true"><a href="../../PPC/ReportForms/VIEW/DailySalePerfView.aspx">
                                                <i class="fa fa-tasks"></i>Daily Sales Performance Report</a></li>
                                            <li id="Li4" runat="server" visible="false"><a href="../../PPC/ReportForms/VIEW/DailyFoundryDispVsActualView.aspx">
                                                <i class="fa fa-tasks"></i>Foundry Dispatch Plan Vs Actual Report</a></li>
                                            <li id="Li5" runat="server" visible="false"><a href="../../PPC/ReportForms/VIEW/DailyMachinePerfView.aspx">
                                                <i class="fa fa-tasks"></i>Daily Machine Shop Performance Report</a></li>
                                            <li id="Li6" runat="server" visible="false"><a href="../../PPC/ReportForms/VIEW/DailyFinalInspDispPerfView.aspx">
                                                <i class="fa fa-tasks"></i>Daily Final Inspection Dispatch Performance Report</a></li>
                                            <li id="Li7" runat="server" visible="false"><a href="../../PPC/ReportForms/VIEW/DailyVendorInwardPerfView.aspx">
                                                <i class="fa fa-tasks"></i>Daily Vendor Inwarding Performance Report</a></li>
                                        </asp:Panel>
                                    </ul>
                                </li>
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
