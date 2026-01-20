<%@ Page Title="Product Master" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ProductMaster.aspx.cs" Inherits="Masters_ProductMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlGroup.ClientID %>").select2();
            jQuery("#<%=ddlInventory.ClientID %>").select2();
            jQuery("#<%=ddlRaw.ClientID %>").select2();
            jQuery("#<%=ddlMachining.ClientID %>").select2();
            jQuery("#<%=ddlSand.ClientID %>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Product Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Part Name & No.
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="upDIthem">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" runat="server"
                                                            MsgObrigatorio="Owner" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Group Name
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlGroup" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Owner"
                                                            TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Inventory Type
                                            </label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlInventory" CssClass="select2" Width="100%" runat="server"
                                                    MsgObrigatorio="Owner" TabIndex="1" AutoPostBack="false">
                                                    <asp:ListItem Text="Select Inventory Type" Value="0" />
                                                    <asp:ListItem Text="RUNNER" Value="1" />
                                                    <asp:ListItem Text="REPEATER" Value="2" />
                                                    <asp:ListItem Text="STRAINGER" Value="3" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="dvVisible" runat="server" visible="false">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%--RFM STORE DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtRfmStore" placeholder="RfmStore" Enabled="true"
                                                        TabIndex="2" Visible="false" Text="0" runat="server" MaxLength="14" MsgObrigatorio="RfmStore"
                                                        onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtRfmStore"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%--VENDOR DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtVendor" placeholder="Vendor" Enabled="true"
                                                        TabIndex="2" Visible="false" Text="0" runat="server" MaxLength="14" MsgObrigatorio="Vendor"
                                                        onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtVendor"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%-- MAIN STORE DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtMainStore" placeholder="Main Store"
                                                        Enabled="true" Visible="false" TabIndex="2" Text="0" runat="server" MaxLength="14"
                                                        MsgObrigatorio="Main Store" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtMainStore"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%-- MACHINE SHOP DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtMachineShop" placeholder="Machine Shop"
                                                        Enabled="true" Visible="false" TabIndex="2" Text="0" runat="server" MaxLength="14"
                                                        MsgObrigatorio="Machine Shop" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtMachineShop"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%-- RFI STORE DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtRFIStore" placeholder="RFIStore" Enabled="true"
                                                        TabIndex="2" Visible="false" Text="0" runat="server" MaxLength="14" MsgObrigatorio="RFIStore"
                                                        onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtRFIStore"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <%-- RFD STORE DAYS--%>
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control " ID="txtRFDStore" placeholder="RFDStore" Enabled="true"
                                                        TabIndex="2" Text="0" Visible="false" runat="server" MaxLength="14" MsgObrigatorio="RFDStore"
                                                        onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtRFDStore"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Core Wt
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtCoreWt" placeholder="Core Wt" Enabled="true"
                                                    TabIndex="2" Text="0" runat="server" MaxLength="30" MsgObrigatorio="Core Wt"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Cast Wt
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtCastWt" placeholder="Cast Wt" Enabled="false"
                                                            TabIndex="2" Text="0" runat="server" MaxLength="30" MsgObrigatorio="Cast Wt"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Finish Wt
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtFinishWt" placeholder="Finish Wt" Enabled="false"
                                                            TabIndex="2" Text="0" runat="server" MaxLength="30" MsgObrigatorio="Finish Wt"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Raw Material Type
                                            </label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlRaw" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Raw Material"
                                                    TabIndex="1" AutoPostBack="false">
                                                    <asp:ListItem Text="Select Raw Type" Value="0" />
                                                    <asp:ListItem Text="LM24" Value="LM24" />
                                                    <asp:ListItem Text="LM25" Value="LM25" />
                                                    <asp:ListItem Text="LM26" Value="LM26" />
                                                    <asp:ListItem Text="LM28" Value="LM28" />
                                                    <asp:ListItem Text="AC4B" Value="AC4B" />
                                                    <asp:ListItem Text="AC4CH Alloy*" Value="AC4CH Alloy*" />
                                                    <asp:ListItem Text="KS 1275" Value="KS 1275" />
                                                    <asp:ListItem Text="ADC 12" Value="ADC 12" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Sand
                                            </label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlSand" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Raw Material"
                                                    TabIndex="1" AutoPostBack="false">
                                                    <asp:ListItem Text="Select Sand" Value="0" />
                                                    <asp:ListItem Text="Sand" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Machining Location
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlMachining" CssClass="select2" Width="100%" runat="server"
                                                            MsgObrigatorio="Machining" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlMachining_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select Machining Type" Value="0" />
                                                            <asp:ListItem Text="INHOUSE" Value="1" />
                                                            <asp:ListItem Text="OFFLOADED" Value="2" />
                                                            <asp:ListItem Text="BOTH" Value="3" />
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                </div>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" Visible="false"
                                                            RepeatDirection="Horizontal" CssClass="checker" CellPadding="10">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp;SOB</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp;Partially</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Line
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlLine" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Owner"
                                                            TabIndex="1" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlLine_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="6" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="7" OnClick="btnCancel_Click"
                                        runat="server"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                        <ContentTemplate>
                            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="captionPopup">
                                            Alert
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <label class="col-md-12 control-label">
                                                        Do you want to cancel record ?
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-3 col-md-9">
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                            OnClick="btnOk_Click"> Yes </asp:LinkButton>
                                                        <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                            OnClick="btnCancel1_Click"> No</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!-- END PAGE CONTENT-->
        </div>
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

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
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=ddlItemName.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; }
                if (VerificaObrigatorio('#<%=ddlGroup.ClientID%>', '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
                if (VerificaObrigatorio('#<%=ddlInventory.ClientID%>', '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
                if (VerificaObrigatorio('#<%=ddlRaw.ClientID%>', '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
                if (VerificaObrigatorio('#<%=ddlMachining.ClientID%>', '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
        //-->
    </script>

</asp:Content>
