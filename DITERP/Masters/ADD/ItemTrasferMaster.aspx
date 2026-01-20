<%@ Page Title="Item Transfer Master" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ItemTrasferMaster.aspx.cs" Inherits="Masters_ADD_ItemTrasferMaster" %>

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlSItemCodeno.ClientID %>").select2();
            jQuery("#<%=ddlSItemName.ClientID %>").select2();
            jQuery("#<%=ddlMaterialFrom.ClientID %>").select2();
            jQuery("#<%=ddlRawComponentCode.ClientID %>").select2();
            jQuery("#<%=ddlRawComponentName.ClientID %>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Item Transfer Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Item Code</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSItemCodeno" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Item Code" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlSItemCodeno_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label label-sm">
                                                    <span class="required">* </span>Unit
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel43" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtSinUOM" placeholder="Unit" TabIndex="2"
                                                                ReadOnly="true" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSItemCodeno" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Item Name</label>
                                                <div class="col-md-6">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSItemName" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Item Name" TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="ddlSItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSItemCodeno" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label label-sm" runat="server" visible="false">
                                                    Copy Finished Product Material From</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlMaterialFrom" Visible="false" CssClass="select2" Width="100%"
                                                        runat="server" MsgObrigatorio="Material From" TabIndex="4" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlMaterialFrom_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Item Code</label>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlRawComponentCode" CssClass="select2" Width="100%" runat="server"
                                                            MsgObrigatorio="Item Code" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlRawComponentCode_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentName" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Item Name</label>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlRawComponentName" CssClass="select2" Width="100%" runat="server"
                                                            MsgObrigatorio="Item Name" TabIndex="6" AutoPostBack="True" OnSelectedIndexChanged="ddlRawComponentName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentCode" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    Unit</label>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtUOM" placeholder="Unit" TabIndex="7"
                                                            ReadOnly="true" runat="server"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentName" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm" runat="server" visible="false">
                                                    Quantity</label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox AutoPostBack="true" MaxLength="10" Visible="false" Text="1" CssClass="form-control text-right input-sm"
                                                            ID="txtVQty" placeholder="0.000" TabIndex="8" runat="server" MsgObrigatorio="Quantity"
                                                            OnTextChanged="txtVQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtVQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm" runat="server" visible="false">
                                                    Scrap Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox AutoPostBack="true" Visible="false" Text="0" MaxLength="10" CssClass="form-control text-right input-sm"
                                                            ID="txtScraQty" placeholder="0.000" TabIndex="8" runat="server" MsgObrigatorio="Scrap Qty"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtScraQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm" runat="server">
                                                    &nbsp</label>
                                                <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" OnClick="btnInsert_Click"
                                                    TabIndex="9" runat="server" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                            </div>
                                        </div>
                                        <!--/span-->
                                    </div>
                                    <div class="row" style="overflow: auto; width: 100%">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgvBOMaterialDetails" runat="server" TabIndex="10" Style="width: 100%;"
                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        CellPadding="4" GridLines="Both" OnRowDeleting="dgvBOMaterialDetails_RowDeleting"
                                                        OnSelectedIndexChanged="dgvBOMaterialDetails_SelectedIndexChanged" OnRowCommand="dgvBOMaterialDetails_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkModify" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                        CausesValidation="False" CommandName="Modify" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                        CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                        CommandName="Delete" Text="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SubComponentCode" SortExpression="SubComponentCode"
                                                                Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBD_I_CODE" runat="server" Text='<%# Bind("TD_I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="Sub Component Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Name" SortExpression="Sub Component Name" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit" SortExpression="UOM" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("UOM") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantity" Visible="false" SortExpression="Quantity"
                                                                HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBD_VQTY" runat="server" Text='<%# Eval("TD_VQTY") %>' CssClass="control-label pull-right"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Scrap Quantity" Visible="false" SortExpression="BD_SCRAPQTY"
                                                                HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBD_SCRAPQTY" runat="server" Text='<%# Eval("TD_SCRAPQTY") %>' CssClass="control-label pull-right"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:Panel ID="Panel1" runat="server">
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="3" runat="server" OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="4" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                                                        Do you Want to Cancel Record?
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-offset-3 col-md-9">
                                                                <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                    OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                                <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                    OnClick="btnCancel1_Click"> No</asp:LinkButton>
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
        </div>
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)-->
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
    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlSItemCodeno.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaValorCombo('#<%=ddlSItemName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
