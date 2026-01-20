<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Delivery_Challan.aspx.cs"
    Inherits="Transactions_ADD_Delivery_Challan" Title="Delivery Challan" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlStore.ClientID %>").select2();
        });
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

    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
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
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Delivery Challan
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Challan No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtChallanNumber" runat="server" placeholder="Challan Number" CssClass="form-control"
                                                            ValidationGroup="Save" Enabled="false" TabIndex="2" MsgObrigatorio="Challan Number"
                                                            MaxLength="100"></asp:TextBox>
                                                    </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Challan Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtChallanDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Challan Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtChallanDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtChallanDate" PopupButtonID="txtChallanDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    Order No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtOrderNo" runat="server" MaxLength="20" placeholder="Order Number"
                                                            CssClass="form-control" ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                <label class=" col-md-2 control-label text-right ">
                                                    Invoice No.</label><div class="col-md-2 ">
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" placeholder="Invoice Number"
                                                            CssClass="form-control" ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    Vehicle No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="20" placeholder="Vehicle Number"
                                                            CssClass="form-control" ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                <label class=" col-md-2 control-label text-right ">
                                                    L.R. No.</label><div class="col-md-2 ">
                                                        <asp:TextBox ID="txtLRno" runat="server" MaxLength="20" placeholder="L.R Number"
                                                            CssClass="form-control" ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="5" MsgObrigatorio="Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtOrderDate" PopupButtonID="txtOrderDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                                <label class=" col-md-2 control-label text-right ">
                                                    Through</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtThrough" MaxLength="20" runat="server" placeholder="Through"
                                                            CssClass="form-control" ValidationGroup="Save" TabIndex="6"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Customer Name</label><div class="col-md-8">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="7" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font></label><div class="col-md-8">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkIsReturn" Checked="true" runat="server" Text="Is Returnable"
                                                                    CssClass="checkbox" />
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
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Item Code</label><asp:UpdatePanel ID="UpdatePanel17" runat="server"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="8" AutoPostBack="True" MsgObrigatorio="Item Code" Visible="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-5">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Item Name</label><asp:UpdatePanel ID="UpdatePanel18" runat="server"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                    runat="server" TabIndex="9" MsgObrigatorio="Item Name" AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                runat="server" Enabled="false" MsgObrigatorio="Item Name" AutoPostBack="True"
                                                                ReadOnly="true">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Store</label>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlStore" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged"
                                                                runat="server" MsgObrigatorio="Store" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Stock Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass=" form-control text-right" ID="txtStock" TabIndex="12" runat="server"
                                                                MsgObrigatorio="Stock" placeholder="Stock" AutoPostBack="True" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtStock"
                                                                ValidChars="0123456789.-" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlStore" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Quantity</label>
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtOrderQty"
                                                                Text="0.000" TabIndex="11" MaxLength="20" MsgObrigatorio="Order Qty." placeholder="Order Qty"
                                                                runat="server" OnTextChanged="txtOrderQty_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtOrderQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        Remark</label>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtRemark" placeholder="Remark" TabIndex="13"
                                                                runat="server" MaxLength="50"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label><br />
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnInsert" CssClass="btn blue" TabIndex="16" runat="server" OnClick="btnInsert_Click"
                                                                OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-Load"> </i>  Insert</asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="control-group">
                                                        <div class="col-md-12" style="overflow-x: auto;">
                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel26">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgMainDC" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                        AutoGenerateColumns="False" CellPadding="4" TabIndex="17" ShowHeaderWhenEmpty="true"
                                                                        OnRowCommand="dgMainDC_RowCommand" OnRowDeleting="dgMainDC_Deleting">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkSelect" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                                        CausesValidation="False" CommandName="Select" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                                        CausesValidation="False" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                        Text="Delete"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="Item Code" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="Item Code" Visible="true">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemCodeNo" runat="server" Text='<%# Bind("ItemCodeNo") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Name" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemName" runat="server" Width="180px" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Unit1" SortExpression="CPOM_CODE" Visible="false"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnit1" runat="server" Width="50px" Text='<%# Eval("Unit1") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Unit" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnit" runat="server" Width="50px" Text='<%# Eval("Unit") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Store" SortExpression="StoreCode" Visible="false"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreCode" runat="server" Width="50px" Text='<%# Eval("StoreCode") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Store" SortExpression="StoreName" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreName" runat="server" Width="50px" Text='<%# Eval("StoreName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Qty." SortExpression="OrderQty" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOrderQty" CssClass=" Control-label" runat="server" Width="100px"
                                                                                        Text='<%# Eval("OrderQty")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Stock" SortExpression="Stock" HeaderStyle-HorizontalAlign="Right"
                                                                                Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStock" CssClass=" Control-labelt" runat="server" Width="100px"
                                                                                        Text='<%# Eval("Stock") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Remark" SortExpression="Remark" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRemark" CssClass=" Control-label" runat="server" Width="100px"
                                                                                        Text='<%# Eval("Remark")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <PagerStyle CssClass="pgr" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="28" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="29" runat="server"
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts
    at bottom, this will reduce page load time) -->
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

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false) {
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
                else if (VerificaObrigatorio('#<%=txtOrderQty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    $("#Avisos").fadeOut(6000);
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
