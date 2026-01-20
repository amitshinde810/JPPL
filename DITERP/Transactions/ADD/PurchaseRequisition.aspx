<%@ Page Title="Purchase Requisition" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="PurchaseRequisition.aspx.cs" Inherits="Transactions_ADD_PurchaseRequisition" %>

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
        function Showalert1() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div id="MSG" class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; margin-bottom: 10px; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Purchase Requisition
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
                                                <label class="col-md-2 control-label label-sm">
                                                    Pur. Req. No.</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtPerReqNo" placeholder="Pur. Req. No"
                                                                TabIndex="1" runat="server" Enabled="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Date</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="2" msgObrigatorio="Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtDate" PopupButtonID="txtDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" CssClass="select2_category form-control" runat="server"
                                                                MsgObrigatorio="Type" TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="Select">Select Type</asp:ListItem>
                                                                <asp:ListItem Value="1" Text="AsPerMateReq">As Per Material Req.</asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Direct">Direct</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Mat. Req. No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlMatReqNo" CssClass="select2_category form-control" runat="server"
                                                                MsgObrigatorio="Mat. Req. No" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="ddlMatReqNo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Required For</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel444" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" CssClass="select2_category form-control" runat="server"
                                                                MsgObrigatorio="Item" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Department</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtDepartment" msgObrigatorio="Department"
                                                                placeholder="Department" TabIndex="6" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Raw Material Code</label>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlRawComponentCode" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Raw Material Code" OnSelectedIndexChanged="ddlRawComponentCode_SelectedIndexChanged"
                                                            TabIndex="7" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentName" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Raw Material Name</label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlRawComponentName" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Raw Material Name" OnSelectedIndexChanged="ddlRawComponentName_SelectedIndexChanged"
                                                            TabIndex="8" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlRawComponentCode" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm ">
                                                    <span class="required">*</span> Requested Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtReqQty" AutoPostBack="true"
                                                            placeholder="0.000" TabIndex="9" runat="server" MsgObrigatorio="Requested Qty"
                                                            OnTextChanged="txtReqQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtReqQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <!--/span-->
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm ">
                                                    <font color="red">*</font> Order Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOrdQty" placeholder="0.000"
                                                            TabIndex="10" runat="server" MsgObrigatorio="Order Qty" AutoPostBack="true" OnTextChanged="txtOrdQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtOrdQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="control-label label-sm ">
                                                    <font color="red">*</font> Remark
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control  input-sm" ID="txtRemark" placeholder="Remark"
                                                            TabIndex="11" runat="server" MsgObrigatorio="Remark"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    &nbsp</label>
                                                <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" TabIndex="12" runat="server"
                                                    OnClick="btnInsert_Click"><i class="fa fa-arrow-circle-down" > </i>  Insert </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="MainPanel" runat="server" Visible="false">
                                        <div class="row">
                                            <!--/span-->
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="control-label label-sm ">
                                                        Old Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control  input-sm" ID="txtoldQty" placeholder="0.000"
                                                                TabIndex="11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="row" style="overflow: auto; width: 100%">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgvPurReqDet" runat="server" TabIndex="12" Style="width: 100%;"
                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        CellPadding="4" GridLines="Both" OnRowDeleting="dgvPurReqDet_RowDeleting" OnSelectedIndexChanged="dgvPurReqDet_SelectedIndexChanged"
                                                        OnRowCommand="dgvPurReqDet_RowCommand">
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
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                        CausesValidation="False" CommandName="Delete" Text="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RawMaterialCode" SortExpression="SubComponentCode"
                                                                Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPRD_I_CODE" runat="server" Text='<%# Bind("PRD_I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Raw Material Code" SortExpression="Raw Material Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Raw Material Name" SortExpression="Raw Material Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_NAME" runat="server" Text='<%# Bind("I_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Requested Qty" SortExpression="Requested Qty" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPRD_REQ_QTY" runat="server" Text='<%# Eval("PRD_REQ_QTY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Old Qty" SortExpression="Old Qty" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPRD_OLD_QTY" runat="server" Text='<%# Eval("PRD_OLD_QTY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Qty" SortExpression="Order Qty" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" ControlStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPRD_ORD_QTY" runat="server" CssClass="form-control text-right"
                                                                        placeholder="" Text='<%# Eval("PRD_ORD_QTY") %>'></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtPRD_ORD_QTY"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock Qty" SortExpression="Stock Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CURRENT_BAL" runat="server" Text='<%# Eval("I_CURRENT_BAL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Minimum Qty" SortExpression="Minimum Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_MIN_LEVEL" runat="server" Text='<%# Eval("I_MIN_LEVEL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remark" SortExpression="Quantity" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPRD_REMARK" runat="server" Text='<%# Eval("PRD_REMARK") %>'></asp:Label>
                                                                </ItemTemplate>
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
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="13" runat="server"
                                        OnClientClick="return VerificaCamposObrigatorios();" OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="14" runat="server"
                                        OnClick="btnCancel_Click"></i> Cancel</asp:LinkButton>
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
    <!-- END PAGE CONTENT-->
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

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <script type="text/javascript" src="assets/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {

                if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtDepartment.ClientID%>', '#Avisos') == false) {
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
