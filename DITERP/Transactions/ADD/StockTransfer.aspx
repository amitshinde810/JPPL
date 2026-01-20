<%@ Page Title="Stock Transfer" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="StockTransfer.aspx.cs" Inherits="Transactions_ADD_StockTransfer" %>

<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlItemTranCode.ClientID %>").select2();
            jQuery("#<%=ddlItemTranName.ClientID %>").select2();
            jQuery("#<%=ddlStoreType.ClientID %>").select2();
        });
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
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Stock Transfer
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" CssClass="remove" OnClick="btnCancel_Click" TabIndex="29"
                                    runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Doc No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtIssueNo" runat="server" CssClass="form-control" placeholder="Doc. No."
                                                        ReadOnly="true" Enabled="false" TabIndex="1"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Stock Adjustment Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtStockAdjustmentDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    TabIndex="2" MsgObrigatorio="Stock Adjustment Date" OnTextChanged="txtStockAdjustmentDate_TextChanged"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtStockAdjustmentDate" PopupButtonID="txtIssueDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Store
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlStoreType" runat="server" TabIndex="1" Width="100%" CssClass="select2"
                                                                AutoPostBack="True" MsgObrigatorio=" Store Name" OnSelectedIndexChanged="ddlStoreType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Item Code</label>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="400px" runat="server"
                                                            TabIndex="3" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged" MsgObrigatorio="Select Item Code"
                                                            placeholder="Select Item Code" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgStockAdjustment" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <%-- <!--/span-->--%>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Item Name</label>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="400px" runat="server"
                                                            MsgObrigatorio="Select Item Name" TabIndex="4" placeholder="Select Item Name"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    Current Stock</label>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCurrStock" Enabled="false"
                                                            placeholder="0.000" TabIndex="5" runat="server">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtCurrStock"
                                                            ValidChars="0123456789.-" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                         <asp:AsyncPostBackTrigger ControlID="ddlStoreType" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgStockAdjustment" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <!--/span-->
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Transfer Item Code</label>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemTranCode" CssClass="select2" Width="400px" runat="server"
                                                            TabIndex="3" OnSelectedIndexChanged="ddlItemTranCode_SelectedIndexChanged" MsgObrigatorio="Select Item Code"
                                                            placeholder="Select Item Code" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemTranName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgStockAdjustment" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <%-- <!--/span-->--%>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Transfer Item Name</label>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemTranName" CssClass="select2" Width="400px" runat="server"
                                                            MsgObrigatorio="Select Item Name" TabIndex="4" placeholder="Select Item Name"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlItemTranName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemTranCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    Transfer Qty.</label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" AutoPostBack="true" OnTextChanged="txtTranQty_TextChanged" ID="txtTranQty" Enabled="true"
                                                            placeholder="0.000" TabIndex="5" runat="server">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtTranQty"
                                                            ValidChars="0123456789.-" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemTranCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemTranName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgStockAdjustment" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <!--/span-->
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label label-sm" runat="server" visible="false">
                                                    <span class="required">*</span> Stock Adjustment Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" Visible="false" MsgObrigatorio="Select Stock Adjustment Qty"
                                                            ID="txtStockAdjustmentQty" MaxLength="15" AutoPostBack="true" OnTextChanged="txtStockAdjustmentQty_TextChanged"
                                                            placeholder="0.000" TabIndex="6" runat="server">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtStockAdjustmentQty"
                                                            ValidChars="0123456789.-" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgStockAdjustment" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="control-label label-sm" runat="server" visible="false">
                                                    Remark</label>
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control" Visible="false" ID="txtRemark" placeholder="Remark"
                                                            TabIndex="7" MaxLength="50" runat="server">
                                                        </asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <!--/span-->
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                </label>
                                                <asp:Button ID="btnInsert" CssClass="btn green" runat="server" Text="Insert" OnClick="btnInsert_Click"
                                                    TabIndex="8" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="overflow: auto; width: 100%">
                                        <div class="col-md-18">
                                            <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgStockAdjustment" runat="server" TabIndex="9" Style="width: 100%;"
                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        CellPadding="4" GridLines="Both" OnRowCommand="dgStockAdjustment_RowCommand"
                                                        OnRowDeleting="dgStockAdjustment_RowDeleting">
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
                                                                        CausesValidation="False" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="temCode" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%#Bind("STD_I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCodeNo" Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Name" SortExpression="ItemCode" Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIND_I_NAME" runat="server" Text='<%# Bind("ItemName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Curr. Stock" SortExpression="SAD_ADJUSTMENT_QTY">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSTD_CURR_STOCK" runat="server" Text='<%# Bind("STD_CURR_STOCK") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TRan. Item Code" SortExpression="NoOfPackes" Visible="false"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSTD_TRAN_I_CODE" runat="server" Text='<%# Eval("STD_TRAN_I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transfer Item Code" SortExpression="ItemCodeNo" Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO_1" runat="server" Text='<%# Bind("I_CODENO_1")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transfer Item Name" SortExpression="ItemCode" Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemName_1" runat="server" Text='<%# Bind("ItemName_1")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transfer Qty" SortExpression="NoOfPackes" Visible="True"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSTD_TRAN_QTY" runat="server" Text='<%# Eval("STD_TRAN_QTY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
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
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="11" runat="server"
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix
    bootstrap tooltip conflict with jquery ui tooltip -->

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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtStockAdjustmentDate.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else {
                    return
                    true;
                }
            } catch (err) {
                alert('Erro in Required Fields: ' + err.description); return
                false;
            }
        } </script>

</asp:Content>
