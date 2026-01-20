<%@ Page Title="DITERP | Masters" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="MasterDefault.aspx.cs" Inherits="Masters_ADD_MasterDefault" %>

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
                                    <div class="col-md-9">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-9">
                                    </div>
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
                                <li id="Item" runat="server"><a id="A2" href="#" onserverclick="btnItemMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>Item Master </a></li>
                                <li id="ItemCategory" runat="server"><a href="#" runat="server" id="l1" onserverclick="btnItemCategory_click">
                                    <i class="fa fa-tasks"></i>Item Category Master </a></li>
                                <li id="ItemSubCat" runat="server" visible="false"><a id="A1" href="#" runat="server"
                                    onserverclick="btnItemSubCategoryMaster_click"><i class="fa fa-tasks"></i>Item Sub
                                    Category Master </a></li>
                                <li id="Tally" runat="server"><a href="#" id="comp10" runat="server" onserverclick="btnTallyMaster_click">
                                    <i class="fa fa-tasks"></i>Tally Master</a> </li>
                                <li id="Excise" runat="server"><a id="A3" href="#" runat="server" onserverclick="btnExciseTariffDetails_click">
                                    <i class="fa fa-tasks"></i>Excise Tariff Heading Master</a> </li>
                                <li id="HSNSAC" runat="server"><a id="A8" href="#" runat="server" onserverclick="btnGST_click">
                                    <i class="fa fa-tasks"></i>GST HSN/SAC Master</a> </li>
                                <li id="SalesTax" runat="server"><a id="A6" href="" runat="server" onserverclick="btnSalesTaxMaster_click">
                                    <i class="fa fa-tasks"></i>Sales Tax Master</a> </li>
                                <li id="BOMMASTER" runat="server"><a id="A4" href="" runat="server" onserverclick="btnBOMMaster_click">
                                    <i class="fa fa-tasks"></i>Bill OF Material Master</a> </li>
                                <li id="StoreMaster" runat="server"><a id="A9" href="" runat="server" onserverclick="StoreMaster_click">
                                    <i class="fa fa-tasks"></i>Store Master</a> </li>
                                <li id="ItemTranMaster" runat="server"><a id="A10" href="" runat="server" onserverclick="ItemTranMaster_click">
                                    <i class="fa fa-tasks"></i>Item Transfer Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="BOMReg" runat="server"><a id="A5" href="" runat="server" onserverclick="btnBOMRegister_click">
                                    <i class="fa fa-tasks"></i>Bill Of Material Register</a> </li>
                                <li id="MasterRep" runat="server"><a id="A7" href="" runat="server" onserverclick="btnMasterReport_click">
                                    <i class="fa fa-tasks"></i>Master Report</a> </li>
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
