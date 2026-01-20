<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="DCReturn.aspx.cs"
    Inherits="Transactions_ADD_DCReturn" Title="DC Return" %>

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
            jQuery("#<%=ddlchallanNo.ClientID %>").select2();
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
                                <i class="fa fa-reorder"></i>Delivery Challan Return
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
                                                    <font color="red"></font>Delivery Challan Return For.</label><div class="col-md-3 ">
                                                        <asp:UpdatePanel ID="UpdatePanele7" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="ddlType" Enabled="false" CssClass="select2" Width="100%" runat="server"
                                                                        TabIndex="7" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                                        Visible="True">
                                                                        <asp:ListItem Value="DCTIN">Tray Delivary Challan</asp:ListItem>
                                                                        <asp:ListItem Selected="True" Value="DCIN">Delivary Challan</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                <label class="col-md-2 control-label text-right ">
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Inward No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtChallanNumber" runat="server" placeholder="Inward Number" CssClass="form-control"
                                                            ValidationGroup="Save" Enabled="false" TabIndex="2" MsgObrigatorio="Inward Number"
                                                            MaxLength="100"></asp:TextBox>
                                                    </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Inward Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtChallanDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Inward Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtChallanDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtChallanDate" PopupButtonID="txtChallanDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" visible="false" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    Order No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtOrderNo" runat="server" placeholder="Order Number" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                <label class=" col-md-2 control-label text-right ">
                                                    Invoice No.</label><div class="col-md-2 ">
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" placeholder="Invoice Number" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" visible="false" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    Vehicle No.</label><div class="col-md-3 ">
                                                        <asp:TextBox ID="txtVehicleNo" runat="server" placeholder="Vehicle Number" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                <label class=" col-md-2 control-label text-right ">
                                                    L.R. No.</label><div class="col-md-2 ">
                                                        <asp:TextBox ID="txtLRno" runat="server" placeholder="L.R Number" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Our Challan No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlchallanNo" CssClass="select2" Width="250px" runat="server"
                                                                    TabIndex="7" MsgObrigatorio="Challan No." OnSelectedIndexChanged="ddlchallanNo_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    ValidationGroup="Save" TabIndex="5" MsgObrigatorio="Date" ReadOnly="false" Enabled="false"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtOrderDate" PopupButtonID="txtOrderDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1 " visible="false" runat="server">
                                                    <asp:TextBox ID="txtThrough" runat="server" placeholder="Through" CssClass="form-control"
                                                        ValidationGroup="Save" TabIndex="6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Party Challan No.
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPartyChNo" runat="server" OnTextChanged="txtPartyChNo_TextChanged"
                                                                    AutoPostBack="true" placeholder="Party Challan Number" CssClass="form-control"
                                                                    ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtPchallanDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="5" MsgObrigatorio="Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtPchallanDate" PopupButtonID="txtPchallanDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                                <div id="Div1" class="col-md-1 " visible="false" runat="server">
                                                    <asp:TextBox ID="TextBox2" runat="server" placeholder="Through" CssClass="form-control"
                                                        ValidationGroup="Save" TabIndex="6"></asp:TextBox>
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
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2" Width="650px" runat="server"
                                                                    TabIndex="7" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlchallanNo" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
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
                                                <div class="col-md-5">
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
                                                                <asp:AsyncPostBackTrigger ControlID="ddlchallanNo" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-6">
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
                                                                <asp:AsyncPostBackTrigger ControlID="ddlchallanNo" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-1">
                                                    <label class="control-label label-sm">
                                                        Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                Width="100%" runat="server" Enabled="false" MsgObrigatorio="Item Name" AutoPostBack="True"
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
                                                <!--/span-->
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Pending Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass=" form-control text-right" ID="txtStock" TabIndex="12" runat="server"
                                                                MsgObrigatorio="Stock" placeholder="Stock" AutoPostBack="True" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtStock"
                                                                ValidChars="0123456789" runat="server" />
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
                                                        <font color="red">*</font> Quantity</label>
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtOrderQty"
                                                                Text="0.000" TabIndex="11" MsgObrigatorio="Order Qty." placeholder="Order Qty"
                                                                runat="server" OnTextChanged="txtOrderQty_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtOrderQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainDC" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Scrap Quantity</label>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtScrapQty"
                                                                Text="0.000" TabIndex="11" MsgObrigatorio="Scrap Qty." placeholder="Scrap Qty"
                                                                runat="server" OnTextChanged="txtScrapQty_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtScrapQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
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
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Store Name</label><asp:UpdatePanel ID="UpdatePanel7" runat="server"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlStore" CssClass="select2" Width="100%" runat="server" TabIndex="9"
                                                                    MsgObrigatorio="Item Name" AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
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
                                                                            <asp:TemplateField HeaderText="Scrap" SortExpression="Scrap" HeaderStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScrap" CssClass=" Control-labelt" runat="server" Width="100px"
                                                                                        Text='<%# Eval("Scrap") %>'></asp:Label>
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
                                                                            <asp:TemplateField HeaderText="StoreCode" SortExpression="Remark" Visible="false"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreCode" CssClass=" Control-label" runat="server" Width="100px"
                                                                                        Text='<%# Eval("StoreCode")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Store Name" SortExpression="StoreName" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreName" CssClass=" Control-label" runat="server" Width="100px"
                                                                                        Text='<%# Eval("StoreName")%>'></asp:Label>
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

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

</asp:Content>
