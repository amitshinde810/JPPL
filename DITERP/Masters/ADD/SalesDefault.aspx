<%@ Page Title="DITERP | Sales" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="SalesDefault.aspx.cs" Inherits="Masters_ADD_SalesDefault" %>

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
                                <li id="SoType" runat="server"><a id="A1" href="#" onserverclick="btnSOTypeMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>Sale Order Type Master</a> </li>
                                <li id="CustType" runat="server"><a id="A2" href="#" onserverclick="btnCustomerTypeMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>Customer Type Master</a> </li>
                                <li id="Customer" runat="server"><a href="#" runat="server" onserverclick="btnCustomerMaster_click">
                                    <i class="fa fa-tasks"></i>Customer Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="CustomerEnquiry" visible="false" runat="server"><a href="#" runat="server"
                                    onserverclick="btnCustomerEnquiry_click"><i class="fa fa-tasks"></i>Shade Development
                                    Requisition</a> </li>
                                <li id="Saleorder" runat="server"><a href="#" runat="server" onserverclick="btnCustomerPO_click">
                                    <i class="fa fa-tasks"></i>Sales Order</a> </li>
                                <li id="TaxInvoice" runat="server"><a href="#" runat="server" onserverclick="btnTaxInvoice_click">
                                    <i class="fa fa-tasks"></i>Tax Invoice</a> </li>
                                <li id="Li1" runat="server"><a id="A16" href="#" runat="server" onserverclick="btnLabourChargeInvoiceInvoice_click">
                                    <i class="fa fa-tasks"></i>Labour Charge Invoice/Delivery Challan </a> </li>
                                <li id="IssueProd" runat="server"><a id="A15" href="#" onserverclick="btnIssueToProduction_click"
                                    runat="server" visible="false"><i class="fa fa-tasks"></i>Issue to Production</a>
                                </li>
                                <li id="ProdStore" runat="server"><a id="A14" href="#" onserverclick="btnProductionToStore_click"
                                    runat="server" visible="false"><i class="fa fa-tasks"></i>Production to Store</a>
                                </li>
                                <li id="ExportInvoice" runat="server" visible="false"><a href="#" runat="server"
                                    onserverclick="btnExportInvoice_click"><i class="fa fa-tasks"></i>Export Invoice</a>
                                </li>
                                <li id="PurchaseRejInvoice" runat="server"  visible="false"><a href="#" runat="server" onserverclick="btnPurchaseRejInvoice_click">
                                    <i class="fa fa-tasks"></i>Purchase Rejection Invoice</a> </li>
                                <li id="DelChallan" runat="server"><a id="A5" href="#" runat="server" onserverclick="btnDeliveryChallan_click">
                                    <i class="fa fa-tasks"></i>Delivery Challan</a> </li>
                                <li id="Li3" runat="server"><a id="A18" href="#" runat="server" onserverclick="btnTrayDeliveryChallan_click">
                                    <i class="fa fa-tasks"></i>Tray Delivery Challan</a> </li>
                                <li id="workorder" runat="server" visible="false"><a id="A10" href="#" runat="server"
                                    onserverclick="btnWorkOrder_click"><i class="fa fa-tasks"></i>Work Order</a>
                                </li>
                                
                                 <li id="Li5" runat="server"><a id="A20" href="#" runat="server" onserverclick="btnLabourChargeInvoiceNew_click">
                                    <i class="fa fa-tasks"></i>Labour Charge Invoice</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="MasterRpt" runat="server"><a href="#" runat="server" onserverclick="btnMasterReport_click">
                                    <i class="fa fa-tasks"></i>Master Reports</a> </li>
                                <li id="SaleOrdReg" visible="false" runat="server"><a href="#" runat="server" onserverclick="btnCustomerOrderRegister_click">
                                    <i class="fa fa-tasks"></i>Sales Order Register</a> </li>
                                <li id="SalesOrderReport" runat="server"><a id="A6" href="#" runat="server" onserverclick="btnCustomerOrderReport_click">
                                    <i class="fa fa-tasks"></i>Sales Order Report</a> </li>
                                <li id="SalesOrderPendingStatus" visible="false" runat="server"><a id="A13" href="#"
                                    runat="server" onserverclick="btnSalesOrderPendingStatus_click"><i class="fa fa-tasks">
                                    </i>Sales Order Pending Status</a> </li>
                                <li id="TaxInvReg" runat="server"><a href="#" runat="server" onserverclick="btnTaxInvoiceRegister_click">
                                    <i class="fa fa-tasks"></i>Tax Invoice Register</a> </li>
                                <li id="Li2" runat="server"><a id="A17" href="#" runat="server" onserverclick="btnSalesSummaryRegister_click">
                                    <i class="fa fa-tasks"></i>Sales Summary Register</a> </li>
                                <li id="ExpInvReg" runat="server" visible="false"><a href="#" runat="server" onserverclick="btnExportInvoiceRegister_click">
                                    <i class="fa fa-tasks"></i>Export Invoice Register</a></li>
                                <li id="StockReg" runat="server" visible="false"><a id="A3" href="#" runat="server"
                                    onserverclick="btnStockLedgerRegister_click"><i class="fa fa-tasks"></i>Stock Ledger
                                    Register</a></li>
                                <li id="StockReport" runat="server" visible="false"><a id="A7" href="#" runat="server"
                                    onserverclick="btnStockLedger_click"><i class="fa fa-tasks"></i>Stock Report</a></li>
                                <li id="StoctLedgerType" visible="false" runat="server"><a id="A12" href="#" runat="server"
                                    onserverclick="btnStoctLedgerType_click"><i class="fa fa-tasks"></i>Stock Ledger
                                    Type Wise Report</a></li>
                                <li id="DelChaReg" runat="server"><a id="A4" href="#" runat="server" onserverclick="btnDeliveryChallanRegister">
                                    <i class="fa fa-tasks"></i>Delivery Challan Register</a></li>
                                <li id="Li4" runat="server"><a id="A19" href="#" runat="server" onserverclick="btnTrayDeliveryChallanRegister">
                                    <i class="fa fa-tasks"></i>Tray Delivery Challan Register</a></li>
                                <li id="UnderDrwaback" runat="server" visible="false"><a id="A8" href="#" runat="server"
                                    onserverclick="btnUnderDrwaback_click"><i class="fa fa-tasks"></i>Under Drawback
                                    Report</a></li>
                                <li id="SelfSealingCertificate" runat="server" visible="false"><a id="A9" href="#"
                                    runat="server" onserverclick="btnSelfSealingCertificate"><i class="fa fa-tasks">
                                    </i>Self Sealing Certificate Report</a></li>
                                <li id="EnquiryReg" runat="server" visible="false"><a id="A11" href="#" runat="server"
                                    onserverclick="btnEnquiryRegister_click"><i class="fa fa-tasks"></i>Shade Development
                                    Requisition Register</a></li>
                                <li id="ItemSale" runat="server"><a id="A25" href="#" runat="server" onserverclick="btnItemSale_click">
                                    <i class="fa fa-tasks"></i>Item Sale Date Wise</a></li>
                                <li id="WeigtVaration" runat="server"><a id="A2tyt" href="#" runat="server" onserverclick="btnWeightMIS_click">
                                    <i class="fa fa-tasks"></i>Weight variation Report</a></li>
                                <li id="FactoryStcok" runat="server"><a id="A2thh" href="#" runat="server" onserverclick="btnInhouserpt_click">
                                    <i class="fa fa-tasks"></i>Factory Stock Report</a></li>
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
