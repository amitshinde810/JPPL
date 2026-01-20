<%@ Page Title="Export Invoice" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ExportInvoice.aspx.cs" Inherits="Transactions_ADD_ExportInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
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
            no: ffobackground-color:#8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">

        function oknumber1(sender, e) {
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
            $("#up").click();
        }
    </script>
    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
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
            <div id="MSG" class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
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
            <br />
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Export Invoice
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <br />
                            <!-- Start Tabs Setting -->
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab_1_1" data-toggle="tab">Export Invoice </a></li>
                                <li class=""><a href="#tab_2_2" data-toggle="tab">Export Invoice Details</a> </li>
                                <li class=""><a href="#tab_3_3" data-toggle="tab">ARE 1 Details</a> </li>
                                <li class=""><a href="#tab_4_4" data-toggle="tab">Other Detail</a> </li>
                                <li class=""><a href="#tab_5_5" data-toggle="tab">Annexure C-I Detail</a> </li>
                                <li class=""><a href="#tab_6_6" data-toggle="tab">IMO Dang.Goods Declaration</a>

                                
                                
                                
                                
                                
                                </li>
                            </ul>
                            <!-- End Tabs Setting -->
                            <!-- Start Tabs-->
                            <div class="tab-content">
                                <!-- T1 Info Tab -->
                                <div class="tab-pane fade active in" id="tab_1_1">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label text-right">
                                                            <font color="red">*</font> Invoice Type</label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlInvoiceType" CssClass="select2_category form-control" runat="server"
                                                                        TabIndex="1" Visible="True" MsgObrigatorio="Invoice Type">
                                                                        <asp:ListItem Selected="True" Value="0">Select Invoice Type</asp:ListItem>
                                                                        <asp:ListItem Value="2">Export Invoice</asp:ListItem>
                                                                        <asp:ListItem Value="3">Proforma Invoice</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label">
                                                            <span class="required">*</span>Invoice Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtDate" TabIndex="2"
                                                                    runat="server" MsgObrigatorio="Invoice Date" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender7" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            <span class="required">*</span> Customer name
                                                        </label>
                                                        <div class="col-md-6">
                                                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlCustomer" AutoPostBack="true" CssClass="select2_category form-control"
                                                                        OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" MsgObrigatorio="Customer name"
                                                                        runat="server" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Invoice No
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtInvoiceNo" placeholder="No"
                                                                TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            <span class="required">*</span> Buyer Name
                                                        </label>
                                                        <div class="col-md-6">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtBuyerName" placeholder="Buyer Name"
                                                                        TabIndex="4" runat="server" MsgObrigatorio="Buyer Name"></asp:TextBox>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label">
                                                            Is Suppliementory
                                                        </label>
                                                        <div class="col-md-1">
                                                            <asp:UpdatePanel ID="UpdatePanel59" runat="server">
                                                                <ContentTemplate>
                                                                    <label class="checkbox">
                                                                        <asp:CheckBox ID="chkIsSuppliement" runat="server" CssClass="checker" Checked="false" />
                                                                    </label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            <span class="required">*</span> Buyer Address
                                                        </label>
                                                        <div class="col-md-6">
                                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtBuyerAddress" placeholder="Buyer Address"
                                                                        TabIndex="5" runat="server" TextMode="MultiLine" MsgObrigatorio="Buyer Address"></asp:TextBox>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                            <%--   row end--%>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged"
                                                                CssClass="select2_category form-control input-sm" runat="server" TabIndex="6"
                                                                MsgObrigatorio="Item Code" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <%-- <!--/span-->--%>
                                                <div class="col-md-5">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Name</label>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                CssClass="select2_category form-control input-sm" runat="server" TabIndex="7"
                                                                MsgObrigatorio="Item Name" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-1">
                                                    <label class="control-label label-sm">
                                                        Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtUOM" placeholder=""
                                                                TabIndex="8" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <asp:Label runat="server" ID="lblUOM" Visible="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> PO NO</label>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlPONO" CssClass="select2_category form-control input-sm"
                                                                runat="server" TabIndex="9" OnSelectedIndexChanged="ddlPONO_SelectedIndexChanged"
                                                                MsgObrigatorio="PO NO" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Stock Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtStockQty" placeholder="0.000"
                                                                TabIndex="10" runat="server" AutoPostBack="true" ReadOnly="true" MsgObrigatorio="Stock Qty">
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtStockQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Pending Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingQty" Text="0.000"
                                                                placeholder="0.00" TabIndex="11" runat="server" AutoPostBack="true" MsgObrigatorio="Pending Qty"
                                                                ReadOnly="true">
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtPendingQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Invoice Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtInvoiceQty" placeholder="0.000"
                                                                TabIndex="12" runat="server" MsgObrigatorio="Invoice Qty" AutoPostBack="true"
                                                                OnTextChanged="txtInvoiceQty_OnTextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtInvoiceQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Rate(in USD)
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" placeholder="0.00"
                                                                TabIndex="13" runat="server" ReadOnly="true" MsgObrigatorio="Rate in USD" AutoPostBack="true"
                                                                OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtRate"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Amount</label>
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAmount" placeholder="0.00"
                                                                ReadOnly="true" TabIndex="14" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtInvoiceQty" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtRate" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Net Wght(In Kg)
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtNetWght" placeholder="0.000"
                                                                TabIndex="16" runat="server" AutoPostBack="true" MsgObrigatorio="Net Wght" OnTextChanged="txtNetWght_OnTextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtNetWght"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Gross.Wght(In Kg)
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGrossWght" placeholder="0.00"
                                                                TabIndex="15" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtGrossWght"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Size of Box</label>
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm text-right" ID="txtSizeofBox" placeholder="0"
                                                                TabIndex="17" runat="server" AutoPostBack="true" MsgObrigatorio="Size of Box"
                                                                OnTextChanged="txtSizeofBox_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtSizeofBox"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label  label-sm">
                                                        <span class="required">*</span> No of Barrels</label>
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtNoofBarrets" placeholder="0"
                                                                TabIndex="18" runat="server" MsgObrigatorio="No of Barrels"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtNoofBarrets"
                                                                ValidChars="0123456789" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        No of Pack
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm text-right" ID="txtNoofPackDesc" placeholder="0"
                                                                TabIndex="19" runat="server" MsgObrigatorio="No Of Pack"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-4">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Pack Description
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel34" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtPackDesc" MsgObrigatorio="Pack Description"
                                                                placeholder="Pack Description" TabIndex="19" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Container No.
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtContainerNo" MsgObrigatorio="Container No"
                                                                placeholder=" Container No" TabIndex="20" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Batch No.
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel58" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtbatchno" MsgObrigatorio="Batch No"
                                                                placeholder="Batch No" TabIndex="20" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONO" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label><br />
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnInsert" OnClick="btnInsert_Click" CssClass="btn blue  btn-sm"
                                                                TabIndex="21" runat="server" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                </div>
                                            </div>
                                            <%-- row start--%>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="control-group">
                                                        <div class="col-md-12" style="overflow-x: auto;">
                                                            <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgInvoiceAddDetail" runat="server" TabIndex="22" Style="width: 100%;"
                                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                        CellPadding="4" GridLines="Both" OnRowCommand="dgInvoiceAddDetail_RowCommand"
                                                                        OnRowDeleting="dgInvoiceAddDetail_Deleting">
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
                                                                                        CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                        Text="Delete"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="ItemCode" SortExpression="temCode" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_I_CODE" runat="server" Text='<%# Bind("IND_I_CODE") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCode" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_I_CODENO" runat="server" Text='<%# Bind("IND_I_CODENO") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Name" SortExpression="ItemCode" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_I_NAME" runat="server" Text='<%# Bind("IND_I_NAME") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Unit" SortExpression="UOM" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("UOM") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="UOM_CODE" SortExpression="UOM_CODE" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUOM_CODE" runat="server" Text='<%# Bind("UOM_CODE") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="POCODE" SortExpression="POCODE" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPO_CODE" runat="server" Text='<%# Bind("PO_CODE") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="PO No" SortExpression="PO No" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPO_NO" runat="server" Text='<%# Bind("PO_NO") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Stock Qty" SortExpression="StockQty" Visible="True"
                                                                                HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSTOCK_QTY" runat="server" Text='<%# Bind("STOCK_QTY") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Pending Qty" SortExpression="PendingQty" Visible="True"
                                                                                HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPEND_QTY" runat="server" Text='<%# Bind("PEND_QTY") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Inv. Qty" SortExpression="Inv.Qty" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblINV_QTY" runat="server" Text='<%# Bind("INV_QTY") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Right"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRATE" runat="server" Text='<%# Eval("RATE") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAMT" runat="server" Text='<%# Eval("AMT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="GROSS Wght." SortExpression="GROSS Wght." HeaderStyle-HorizontalAlign="Right"
                                                                                Visible="true">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGROSS_WGHT" runat="server" Text='<%# Bind("GROSS_WGHT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Net Wght." SortExpression="NET WGHT" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNET_WGHT" runat="server" Text='<%# Bind("NET_WGHT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Box Size" SortExpression="BOX_SIZE">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBOX_SIZE" runat="server" Text='<%# Bind("BOX_SIZE") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="No of Barrels" SortExpression="NOOF_BARRELS">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNOOF_BARRELS" runat="server" Text='<%# Bind("NOOF_BARRELS") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="No of Pack" SortExpression="NOOF_PACK_DESC">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNOOF_PACK_DESC" runat="server" Text='<%# Bind("NO_OF_PACK") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Pack.Desc" SortExpression="NOOF_PACK_DESC">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPACK_DESC" runat="server" Text='<%# Bind("PACK_DESC") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Container No" SortExpression="CONT_NO">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCONT_NO" runat="server" Text='<%# Bind("CONT_NO") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IND_EX_AMT" SortExpression="IND_EX_AMT" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_EX_AMT" runat="server" Text='<%# Bind("IND_EX_AMT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IND_E_CESS_AMT" SortExpression="IND_E_CESS_AMT" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_E_CESS_AMT" runat="server" Text='<%# Bind("IND_E_CESS_AMT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IND_SH_CESS_AMT" SortExpression="IND_SH_CESS_AMT"
                                                                                Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_SH_CESS_AMT" runat="server" Text='<%# Bind("IND_SH_CESS_AMT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Batch No" SortExpression="IND_BACHNO" Visible="true">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIND_BACHNO" runat="server" Text='<%# Bind("IND_BACHNO") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <PagerStyle CssClass="pgr" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <asp:Panel ID="Panel1" runat="server">
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- End T1 --%>
                                <!-- T2 Info Tab -->
                                <div class="tab-pane fade" id="tab_2_2">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                        </label>
                                                        <div class="col-md-4">
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            <span class="required">*</span> Tax Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel57" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlTaxName" CssClass="select2_category form-control input-sm"
                                                                        Enabled="false" runat="server" TabIndex="23" AutoPostBack="True" OnSelectedIndexChanged="ddlTaxName_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--Basic--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Basic Amount</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtNetAmount" TabIndex="24"
                                                                        placeholder="0.00" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtNetAmount"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Sales tax %
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel51" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSalesTaxPer" placeholder="0.00"
                                                                        TabIndex="34" runat="server" Text="0.00" Enabled="false" AutoPostBack="true"
                                                                        OnTextChanged="txtSalesTaxPer_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" TargetControlID="txtSalesTaxPer"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel52" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSalesTaxAmount" placeholder="0.00"
                                                                        TabIndex="25" runat="server" Enabled="false"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" TargetControlID="txtSalesTaxAmount"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAdvDuty" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Discount-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Discount%
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDiscPer" placeholder="0.00"
                                                                        AutoPostBack="true" OnTextChanged="txtDiscPer_TextChanged" TabIndex="26" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtDiscPer"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDiscount" placeholder="0.00"
                                                                        TabIndex="25" Enabled="false" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtDiscount"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Other Charges
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel53" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOtherCharges" Text="0.00"
                                                                        placeholder="0.00" TabIndex="35" runat="server" AutoPostBack="true" OnTextChanged="txtOtherCharges_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" TargetControlID="txtOtherCharges"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--Packaging--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Packaging Amount</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel42" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPackagingAmt" TabIndex="27"
                                                                        placeholder="0.00" runat="server" AutoPostBack="true" OnTextChanged="txtPackagingAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txtPackagingAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Freight
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtFreight" placeholder="0.00"
                                                                        AutoPostBack="true" Text="0.00" OnTextChanged="txtFreight_TextChanged" TabIndex="36"
                                                                        runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtFreight"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--Accessable--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Accessable Amount</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel43" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAccessable" TabIndex="28"
                                                                        placeholder="0.00" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtAccessable"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Insurance
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtInsurance" placeholder="0.00"
                                                                        AutoPostBack="true" OnTextChanged="txtInsurance_TextChanged" Text="0.00" TabIndex="37"
                                                                        runat="server">  </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtInsurance"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Basic Exceise Duty-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Basic Excise Duty
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel44" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtBasicExcPer"
                                                                        placeholder="0.00" TabIndex="29" runat="server" OnTextChanged="txtBasicExcPer_TextChanged"
                                                                        AutoPostBack="true" Text="12.00"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" TargetControlID="txtBasicExcPer"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel45" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtBasicExcAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="29" runat="server" Enabled="false" AutoPostBack="true"
                                                                        OnTextChanged="txtBasicExcAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" TargetControlID="txtBasicExcAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Transport
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel153" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTransportAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="38" runat="server" AutoPostBack="true" OnTextChanged="txtTransportAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender127" TargetControlID="txtTransportAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Education-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Edu. Cess
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel46" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtducexcper"
                                                                        placeholder="0.00" TabIndex="30" runat="server" OnTextChanged="txtducexcper_TextChanged"
                                                                        Text="2.00" AutoPostBack="true"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" TargetControlID="txtducexcper"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel47" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtEdueceAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="32" runat="server" Enabled="false" OnTextChanged="txtEdueceAmt_TextChanged"
                                                                        AutoPostBack="true"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" TargetControlID="txtEdueceAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Octri
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel54" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOctri" Text="0.00"
                                                                        placeholder="0.00" TabIndex="39" runat="server" AutoPostBack="true" OnTextChanged="txtOctri_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" TargetControlID="txtOctri"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--HigherEdu-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            S & H E. Cess
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtSHEExcPer"
                                                                        placeholder="0.00" TabIndex="31" AutoPostBack="true" OnTextChanged="txtSHEExcPer_TextChanged"
                                                                        runat="server" Text="1.00"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" TargetControlID="txtSHEExcPer"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel49" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSHEExcAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="35" runat="server" Enabled="false"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" TargetControlID="txtSHEExcAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscount" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            TCS
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel55" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTCSAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="40" runat="server" AutoPostBack="true" OnTextChanged="txtTCSAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" TargetControlID="txtTCSAmt"
                                                                        ValidChars="0123456789." runat="server" />
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
                                                            Advance Duty
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel152" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAdvDuty" Text="0.00"
                                                                        placeholder="0.00" TabIndex="32" runat="server" AutoPostBack="true" OnTextChanged="txtAdvDuty_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender126" TargetControlID="txtAdvDuty"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Rounding(+/-)
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel56" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRoundingAmt" Text="0.00"
                                                                        placeholder="0.00" TabIndex="41" runat="server" AutoPostBack="true" OnTextChanged="txtRoundingAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" TargetControlID="txtRoundingAmt"
                                                                        ValidChars="0123456789.-" runat="server" />
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
                                                            Taxable Amount
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel50" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTaxableAmt" ReadOnly="true"
                                                                        Text="0.00" placeholder="0.00" TabIndex="33" runat="server" AutoPostBack="true"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" TargetControlID="txtTaxableAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAdvDuty" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAccessable" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Grand Total
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGrandTotal" placeholder="0.00"
                                                                        TabIndex="42" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtGrandTotal"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtInsurance" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtPackagingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtRoundingAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAdvDuty" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAccessable" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtAccessable" EventName="TextChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                            <!--row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Final Destination
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control  input-sm" ID="txtFinalDestination" placeholder="Final Destination"
                                                                        TabIndex="43" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Prepration Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control  input-sm" placeholder="dd MMM yyyy" ID="txtIssueDate"
                                                                    TabIndex="44" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtIssueDate">
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
                                                        <label class="col-md-3 control-label label-sm">
                                                            Pre Carriage By
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPreCarriageBy" placeholder="Pre Carriage By"
                                                                        TabIndex="45" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Prepration Time
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtIssuetime" TabIndex="46" runat="server"
                                                                    MsgObrigatorio="Please Enter Prepration Time" TextMode="SingleLine" ReadOnly="false"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <Ajaxified:TimePicker ID="TimePicker1" runat="server" TargetControlID="txtIssuetime">
                                                                </Ajaxified:TimePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Port Of Loading
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control  input-sm" ID="txtPortOfLoading" placeholder="Port Of Loading"
                                                                        TabIndex="47" runat="server" TextMode="SingleLine">
                                                                    </asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Removal Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control  input-sm" placeholder="dd MMM yyyy" ID="txtRemovalDate"
                                                                    TabIndex="48" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtRemovalDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Port Of Discharge
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control  input-sm" ID="txtPortOfDischarge" placeholder="Port Of Discharge"
                                                                        TabIndex="50" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Removal Time
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtRemoveltime" TabIndex="51" runat="server"
                                                                    MsgObrigatorio="Please Enter Removel Time" TextMode="SingleLine" ReadOnly="false"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <Ajaxified:TimePicker ID="TimePicker2" runat="server" TargetControlID="txtRemoveltime">
                                                                </Ajaxified:TimePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Place of Delivery
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPlaceofDelivery" placeholder="Place of Delivery"
                                                                        TabIndex="52" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Flight No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtFlightNo" placeholder="Flight No"
                                                                TabIndex="53" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            <span class="required">*</span> Currency
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlCurrency" MsgObrigatorio="Currancy" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" CssClass="select2_category form-control"
                                                                        runat="server" TabIndex="54">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Currency Rate
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCurrencyRate" placeholder="0.00"
                                                                        TabIndex="55" AutoPostBack="true" runat="server" OnTextChanged="txtCurrencyRate_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtCurrencyRate"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrency" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Clearance Under Form
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtClearance" placeholder="Form"
                                                                        TabIndex="56" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            MFG Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" placeholder="dd MMM yyyy" ID="txtMFGDate"
                                                                    TabIndex="57" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtMFGDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Label runat="server" ID="w2" Visible="false" class="col-md-1 control-label label-sm">
                                                            Licence No & Date
                                                        </asp:Label>
                                                        <div visible="false" class="col-md-2">
                                                            <asp:Button Visible="false" ID="ntnClick" TabIndex="58" runat="server" Text="Click" />
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Processing Charges Only
                                                        </label>
                                                        <div class="col-md-2">
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="ChkProcessingChargesOnly" runat="server" Text="" CssClass="checker"
                                                                    TabIndex="59" />
                                                            </label>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Exp Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" placeholder="dd MMM yyyy" ID="txtExpDate"
                                                                    TabIndex="60" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtExpDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Transport Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control  input-sm" ID="txtTransportBy" placeholder="Transport Name"
                                                                TabIndex="61" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Authorised Sign Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtAuthSign" placeholder="Sign"
                                                                        TabIndex="62" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            L.R. No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" placeholder="L R No" ID="txtLRNo" TabIndex="63"
                                                                MaxLength="50" runat="server" MsgObrigatorio="LR No" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            L.R. Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtLRDate" TabIndex="64"
                                                                    ReadOnly="false" runat="server" MsgObrigatorio="Please L.R Date" TextMode="SingleLine"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtender9" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtLRDate">
                                                                </cc1:CalendarExtender>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            L.C. No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" placeholder="L C No" ID="txtLCNo" TabIndex="65"
                                                                MaxLength="50" runat="server" MsgObrigatorio="LC No" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            L.C. Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtLCDate" TabIndex="67"
                                                                    ReadOnly="false" runat="server" MsgObrigatorio="Please L.R Date" TextMode="SingleLine"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtender10" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtLCDate">
                                                                </cc1:CalendarExtender>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Transporter Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" placeholder="Transporter Name" ID="txtTransporterName"
                                                                TabIndex="68" MaxLength="50" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Transport Address
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" placeholder="Transport Address" ID="txtTransportAdd"
                                                                TabIndex="69" MaxLength="200" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <!--/row-->
                                        </div>
                                    </div>
                                    <%--Div tag end of first tab--%>
                                </div>
                                <!-- T3 Info Tab -->
                                <div class="tab-pane fade" id="tab_3_3">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            ARE1 Form No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtARE1FormNo" placeholder="ARE1 Form No"
                                                                TabIndex="46" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            ARE1 Form Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtARE1FormDate"
                                                                    TabIndex="47" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtARE1FormDate">
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
                                                        <label class="col-md-3 control-label label-sm">
                                                            Shipment
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control" ID="txtShipment" placeholder="Shipment" TabIndex="48"
                                                                runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Cenvat Account Entry No
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control" ID="txtCenvatAccountEntryNo" placeholder="Cenvat Account Entry No"
                                                                TabIndex="49" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Bond No./UT1 No.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtBondNo" placeholder="Bond No/UT1 No"
                                                                TabIndex="50" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Bond Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtBondDate" TabIndex="51"
                                                                    runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtBondDate">
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
                                                        <label class="col-md-3 control-label label-sm">
                                                            UT1 File No.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtUT1FileNo" placeholder="UT1 File No"
                                                                TabIndex="52" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Export Under Duty Claim
                                                        </label>
                                                        <div class="col-md-3">
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="ChkExportUnderDutyClaim" runat="server" Text="" CssClass="checker"
                                                                    TabIndex="53" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Sr No Of UT-1
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtFileNo" placeholder="Sr No Of UT-1"
                                                                TabIndex="54" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Valid Up To
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtValidUpTo"
                                                                    TabIndex="55" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtValidUpTo">
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
                                                        <label class="col-md-3 control-label label-sm">
                                                            Examined Boxes/Pallets No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtExaminedBoxes" placeholder="Examined Boxes"
                                                                TabIndex="56" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Remark
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtARERemark" MaxLength="250" placeholder="Remark"
                                                                TabIndex="57" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- End T3 --%>
                                <!-- T4 Info Tab -->
                                <div class="tab-pane fade" id="tab_4_4">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            <span class="required">*</span> Country Of Origin
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlCOuntryOrigin" AutoPostBack="true" CssClass="select2_category form-control"
                                                                        MsgObrigatorio="Please Enter Country Of Origin" runat="server" TabIndex="58">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            <span class="required">*</span> Country Of Destination
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlCountryOfDest" AutoPostBack="true" CssClass="select2_category form-control"
                                                                        MsgObrigatorio="Please Enter Country Of Destination" OnSelectedIndexChanged="ddlCountryOfDest_SelectedIndexChanged"
                                                                        runat="server" TabIndex="59">
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
                                                            Place Of Pre Carriage
                                                        </label>
                                                        <div class="col-md-9">
                                                            <asp:UpdatePanel ID="UpdatePanel60" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtPlaceOfPreCarriage" placeholder="Place Of Receipt By Pre Carriage"
                                                                        TabIndex="60" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-3">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Voyage No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtVoyageNo" placeholder="Voyage No" TabIndex="61"
                                                                runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Marks & Nos
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtMarksNos" placeholder="Marks & Nos"
                                                                TabIndex="62" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            No. & Kind Of Pkg.
                                                        </label>
                                                        <div class="col-md-9">
                                                            <asp:UpdatePanel ID="UpdatePanel61" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNoKindPkg" placeholder="No. & Kind of Package"
                                                                        TabIndex="63" runat="server"></asp:TextBox>
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
                                                            UN No
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel62" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtUnNo" placeholder="UN No" TabIndex="64"
                                                                        runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-1 control-label label-sm">
                                                            HAZ Class
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel63" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtHASClass" placeholder="HAZ Class"
                                                                        TabIndex="65" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            HS Code No
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel64" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtHSCodeNo" placeholder="HS Code No"
                                                                        TabIndex="66" runat="server"></asp:TextBox>
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
                                                            Terms Of Delivery
                                                        </label>
                                                        <div class="col-md-9">
                                                            <asp:UpdatePanel ID="UpdatePanel65" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtTrmOfDel" placeholder="Term Of Delivery"
                                                                        TabIndex="67" runat="server"></asp:TextBox>
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
                                                            Terms Of Payment
                                                        </label>
                                                        <div class="col-md-9">
                                                            <asp:UpdatePanel ID="UpdatePanel66" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" placeholder="Terms Of Payment" ID="txtTrmOfPay"
                                                                        TabIndex="68" runat="server" TextMode="SingleLine"></asp:TextBox>
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
                                                            Container No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtContainer" placeholder="Container No"
                                                                TabIndex="69" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Bottle Seal No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtBottleSealNo" placeholder="Bottle Seal No"
                                                                TabIndex="70" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            OTS No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtOTSNo" placeholder="OTS No"
                                                                TabIndex="71" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- End T4 --%>
                                <!-- T5 Info Tab -->
                                <div class="tab-pane fade" id="tab_5_5">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Shipping Bill No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtShippingBillno" placeholder="Shipping Bill No"
                                                                TabIndex="62" MaxLength="30" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            IEC No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtIECNo" placeholder="IEC No"
                                                                TabIndex="62" MaxLength="20" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            IEC Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtIECDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    ValidationGroup="Save" TabIndex="3" MsgObrigatorio="IEC Date" ReadOnly="false"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtIECDate_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtIECDate" PopupButtonID="txtIECDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Examination Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtExamDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Examiniation Date" ReadOnly="false"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender8" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtExamDate" PopupButtonID="txtExamDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Custom Seal No
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCustSealNo" placeholder="Custom Seal No"
                                                                TabIndex="62" runat="server" MaxLength="30"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            C.Excise Regn
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCentExcRegNo" placeholder="C.Excise Reg.No"
                                                                TabIndex="62" MaxLength="20" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Inspector Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtInspectorName" placeholder="Inspector Name"
                                                                TabIndex="62" MaxLength="30" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Superintendent Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtSuperintendentName" placeholder="Superintendent Name"
                                                                TabIndex="62" runat="server" MaxLength="30"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Stuffing Per.No.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtStuffingPerNo" placeholder="Stuffing Per. No"
                                                                TabIndex="62" runat="server" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Cargo No. of Pack
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCargoNoOfPack" Text="Not Applicable"
                                                                TabIndex="62" MaxLength="50" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- End T5 --%>
                                <!-- T5 Info Tab -->
                                <div class="tab-pane fade" id="tab_6_6">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Carrier Name
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCarrierName" placeholder="Carrier Name"
                                                                TabIndex="62" MaxLength="100" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Carrier Booking No.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCarrierBookNo" placeholder="Carrier Book No"
                                                                TabIndex="62" MaxLength="100" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Technical Name
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtTechnicalName" placeholder="Technical Name"
                                                                TabIndex="62" MaxLength="150" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Outer Packages
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtOuterPack" placeholder="Outer Packages"
                                                                TabIndex="62" MaxLength="50" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Inner Packing
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtInnerPack" placeholder="Inner Packing"
                                                                TabIndex="62" runat="server" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Sub Class
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtSubClass" Text="N/A" TabIndex="62"
                                                                MaxLength="50" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            UN Packing Group
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel67" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtUnPackingGroup" placeholder="Packing Group"
                                                                        TabIndex="62" runat="server" MaxLength="50"></asp:TextBox>
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
                                                            UN Packing Code
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel68" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtUnPackingCode" placeholder="UN Packing Code"
                                                                        TabIndex="62" MaxLength="50" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            EMS NO.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEMSNo" placeholder="EMS No"
                                                                TabIndex="62" runat="server" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Flash Point
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtFlashPoint" placeholder="Flash Point"
                                                                TabIndex="62" MaxLength="50" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-3 control-label label-sm">
                                                            Marrine Pollutanat
                                                        </label>
                                                        <div class="col-md-2">
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkMarrinePollutent" runat="server" Text="" CssClass="checker"
                                                                    TabIndex="44" Checked="true" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- End T5 --%>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="72" runat="server"
                                            OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="73" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <!-- End Tabs-->
                            <asp:UpdatePanel runat="server" ID="UpdatePanel33">
                                <ContentTemplate>
                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
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
                                                                Do you want to cancel record?
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
        </div>
    </div>
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {

                if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtBuyerName.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtBuyerAddress.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }

                else if (VerificaValorCombo('#<%=ddlItemCode.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }

                else if (VerificaValorCombo('#<%=ddlItemName.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%= ddlItemName.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlPONO.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtPendingQty.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtInvoiceQty.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtRate.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtNetWght.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtNetWght.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
              
                else if (VerificaObrigatorio('#<%=txtNoofBarrets.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }               
                else if (VerificaObrigatorio('#<%=txtPackDesc.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }                
                else if (VerificaObrigatorio('#<%=ddlCurrency.ClientID%>', '#Avisos') == false) {
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
        //-->
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
