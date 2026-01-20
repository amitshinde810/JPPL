<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProductionDefault.aspx.cs"
    Inherits="Masters_ProductionDefault" Title="DITERP | Store" %>

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
            $find('ModalPopupMsg').hide();vendor wise
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
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Inward
                                    Transactions</a> <span class="after"></span></li>
                                <li id="Inwrd" runat="server"><a id="A13" href="#" onserverclick="btnMaterialInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inward</a> </li>
                                <li id="liSubconInwrd" runat="server"><a id="A7" href="#" onserverclick="btnSubContractorInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Sub Contractor Inward</a> </li>
                                <li id="LiCashiwrd" runat="server"><a id="A10" href="#" onserverclick="btnCashInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Cash Inward</a> </li>
                                <li id="LiForProInwrd" runat="server"><a id="A11" href="#" onserverclick="btnForProcessInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>For Process Inward</a> </li>
                                <li id="CustRej" runat="server"><a id="A2" href="#" runat="server" onserverclick="btnCustomerRejection_click">
                                    <i class="fa fa-tasks"></i>Customer Rejection Inward</a> </li>
                                <li id="TurIWD" runat="server"><a id="A23" href="#" runat="server" onserverclick="btnTurningIWD_click">
                                    <i class="fa fa-tasks"></i>Turning Inward</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Other
                                    Transactions</a> <span class="after"></span></li>
                                <li id="MatReq" runat="server"><a id="A3" href="#" visible="false" onserverclick="btnMaterialRequisition_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition</a> </li>
                                <li id="FillOffSheet" runat="server"><a id="A5" visible="false" href="#" runat="server"
                                    onserverclick="btnFillOffSheet_click"><i class="fa fa-tasks"></i>Fill Off Sheet</a></li>
                                <li id="IssueProd" runat="server"><a href="#" onserverclick="btnIssueToProduction_click"
                                    runat="server"><i class="fa fa-tasks"></i>Issue From Main Store</a> </li>
                                <li id="main2main" runat="server"><a id="A39" href="#" onserverclick="btnmain2main_click"
                                    runat="server"><i class="fa fa-tasks"></i>Main store To Main store Movement</a></li>
                                <li id="IssueProdA" runat="server"><a id="A27" href="#" onserverclick="btnIssueToProductionA_click"
                                    runat="server"><i class="fa fa-tasks"></i>Store To Store Casting Movement</a></li>
                                <li id="ProdStore" runat="server"><a id="A8" href="#" onserverclick="btnProductionToStore_click"
                                    runat="server"><i class="fa fa-tasks"></i>Production to Store</a> </li>
                                <li id="Inspection" runat="server"><a id="A14" href="#" onserverclick="btnMaterialInspection_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inspection</a> </li>
                                <li id="liDistToSubCon" runat="server"><a id="A9" href="#" runat="server" onserverclick="btnDispatchToSub_click">
                                    <i class="fa fa-tasks"></i>Dispatch To Sub Contractor</a> </li>
                                <li id="IssueFillOffSheet" visible="false" runat="server"><a id="A6" href="#" runat="server"
                                    onserverclick="btnIssueFillOffSheet_click"><i class="fa fa-tasks"></i>Issue Fill
                                    Off Sheet</a> </li>
                                <li id="StockAdj" runat="server"><a href="#" onserverclick="btnStockAdjustment_click"
                                    runat="server"><i class="fa fa-tasks"></i>Stock Adjustment</a> </li>
                                <li id="LiDCReturn" runat="server"><a id="A16" href="#" runat="server" onserverclick="btnDCRetrun_click">
                                    <i class="fa fa-tasks"></i>Delivery Challan Return</a> </li>
                                <li id="LiTDCReturn" runat="server"><a id="A24" href="#" runat="server" onserverclick="btnTrayDCRetrun_click">
                                    <i class="fa fa-tasks"></i>Tray Delivery Challan Return</a> </li>
                                <li id="LiMaterialAcceptance" runat="server"><a id="A26" href="#" runat="server"
                                    onserverclick="btnMaterialAcceptance_click"><i class="fa fa-tasks"></i>Material
                                    Acceptance</a> </li>
                                <li id="RejConvT" runat="server"><a id="A28" href="#" runat="server" onserverclick="btnRejToFondryCov_click">
                                    <i class="fa fa-tasks"></i>Casting Conversion</a> </li>
                                <li id="StockCompateT" runat="server"><a id="A29" href="#" runat="server" onserverclick="btnUploadStock_click">
                                    <i class="fa fa-tasks"></i>Stock Error Correction Utility - Upload Physical Stock</a></li>
                                <li id="StockTran" runat="server"><a id="A33" href="#" runat="server" onserverclick="btnStockTran_click">
                                    <i class="fa fa-tasks"></i>Stock Transfer</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="BOMReg" runat="server"><a href="#" onserverclick="btnBillOfMaterialRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Bill Of Material Register</a> </li>
                                <li id="MatReqReg" visible="false" runat="server"><a href="#" onserverclick="btnMaterialRequisitionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition Register</a></li>
                                <li id="IssueProdReg" runat="server"><a href="#" onserverclick="btnIssueToProductionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Issue To Production Register</a> </li>
                                <li id="ProdReg" runat="server"><a href="#" onserverclick="btnProdcutionToStoreRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Production To Store Register</a> </li>
                                <li id="MatreqMisReg" visible="false" runat="server"><a id="A1" href="#" onserverclick="btnMaterialRequisitionMISReport_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition MIS Report</a></li>
                                <li id="liDistToSubReg" runat="server"><a id="A12" href="#" runat="server" onserverclick="btnDispatchToSubReport_click">
                                    <i class="fa fa-tasks"></i>Dispatch To Sub Contractor Report</a> </li>
                                <li id="InwdReg" runat="server"><a id="A20" href="#" onserverclick="btnInwardSuppWise_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inward Register</a> </li>
                                <li id="Inspreg" runat="server"><a id="A21" href="#" onserverclick="btnInspectionRegisterReport_click"
                                    runat="server"><i class="fa fa-tasks"></i>Inspection Register</a> </li>
                                <li id="StockAsjreg" runat="server"><a href="#" onserverclick="btnStockAdjustmentRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Stock Adjustment Register</a> </li>
                                <li id="CustRejReg" runat="server"><a id="A4" href="#" runat="server" onserverclick="btnCustomerRejectionRegister_click">
                                    <i class="fa fa-tasks"></i>Customer Rejection Register</a></li>
                                <li id="LiStockRegReport" runat="server"><a id="A15ty" href="#" runat="server" onserverclick="btnStockLedger_click">
                                    <i class="fa fa-tasks"></i>Stock Register</a></li>
                                <li id="liDcRReg" runat="server"><a id="A17" href="#" onserverclick="btnDCRetrunRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Delivery Challan Return Register</a></li>
                                <li id="LiTDCRReg" runat="server"><a id="A25" href="#" onserverclick="btnDCTRetrunRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Tray Delivery Challan Return Register</a></li>
                                <li id="LiTSReg" runat="server"><a id="A18" href="#" runat="server" onserverclick="btnTrayStockLedger_click">
                                    <i class="fa fa-tasks"></i>Tray Stock Register</a></li>
                                <li id="LiTSMISRpt" runat="server"><a id="A15" href="#" runat="server" onserverclick="btnTrayStockMIS_click">
                                    <i class="fa fa-tasks"></i>Tray Stock MIS Report</a></li>
                                <li id="liVenSReg" runat="server"><a id="A19" href="#" runat="server" onserverclick="btnVendorStockLedger_click">
                                    <i class="fa fa-tasks"></i>Vendor Stock Register</a></li>
                                <li id="LiCustStockReg" runat="server"><a id="A38" href="#" runat="server" onserverclick="btnCustStockLedger_click">
                                    <i class="fa fa-tasks"></i>Customer Stock Register</a></li>
                                <li id="TurningRegister" runat="server"><a id="A22" href="#" runat="server" onserverclick="btnTurning_click">
                                    <i class="fa fa-tasks"></i>Turning Register</a></li>
                                <li id="LiCastConReg" runat="server"><a id="A31" href="#" runat="server" onserverclick="btnRejToFound_click">
                                    <i class="fa fa-tasks"></i>Casting Conversion Register</a></li>
                                <li id="LiStockReport" runat="server"><a id="A30" href="#" runat="server" onserverclick="btnStockReport_click">
                                    <i class="fa fa-tasks"></i>Store Wise Stock Report</a></li>
                                <li id="LiStoreRegister" runat="server"><a id="A3r1" href="#" runat="server" onserverclick="btnStoreRegister_click">
                                    <i class="fa fa-tasks"></i>Store To Store Casting Movement Register</a></li>
                                <li id="StockErrorT" runat="server"><a id="A32" href="#" runat="server" onserverclick="btnStockError_click">
                                    <i class="fa fa-tasks"></i>Stock Error Correction Report</a></li>
                                <li id="StockTranReg" runat="server"><a id="A34" href="#" runat="server" onserverclick="btnStockTranReg_click">
                                    <i class="fa fa-tasks"></i>Stock Transfer Register</a></li>
                                <li id="StlLvlRpt" runat="server"><a id="A35" href="#" runat="server" onserverclick="btnStockLevel_click">
                                    <i class="fa fa-tasks"></i>Stock Level Report</a> </li>
                                <li id="InwMISRpt" runat="server"><a id="A37" href="#" runat="server" onserverclick="btnInwMISRpt_click">
                                    <i class="fa fa-tasks"></i>Inward MIS report</a></li>
                                <li id="trunreg" runat="server"><a id="A36" href="#" runat="server" onserverclick="btnTurningReg_click">
                                    <i class="fa fa-tasks"></i>Turning Inward Report</a></li>
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
