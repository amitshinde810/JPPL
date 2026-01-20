<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CustomerPO.aspx.cs"
    Inherits="Transactions_ADD_CustomerPO" Title="Customer PO" EnableEventValidation="true"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

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
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (charCode == 46 && el.value.indexOf(".") !== -1) {
                return false;
            }
            if (el.value.indexOf(".") !== -1) {
                var range = document.selection.createRange();
                if (range.text != "") {
                }
                else {
                    var number = el.value.split('.');
                    if (number.length == 2 && number[1].length > 1)
                        return false;
                }
            }
            return true;
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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
            jQuery("#<%=ddlPOType.ClientID %>").select2();
            jQuery("#<%=ddlProjectCode.ClientID %>").select2();
            jQuery("#<%=ddlUnit.ClientID %>").select2();
            jQuery("#<%=ddlTaxCategory.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function Showalert1() {
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
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
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
                                <i class="fa fa-reorder"></i>Customer PO
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
                                                    Order No.</label>
                                                <div class="col-md-3 ">
                                                    <asp:TextBox ID="txtOrderNo" runat="server" placeholder="Order No." CssClass="form-control"
                                                        ValidationGroup="Save" TabIndex="1" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Entry Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Entry Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtPODate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtPODate" PopupButtonID="txtPODate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtPODocNo" runat="server" placeholder="Number" CssClass="form-control"
                                                        ValidationGroup="Save" Visible="false" TabIndex="4444" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> PO Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlPOType_SelectedIndexChanged" ID="ddlPOType"
                                                                CssClass="select2" Width="100%" runat="server" TabIndex="3" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Sales Order Type">
                                                                <asp:ListItem Selected="True" Value="0">Select PO Type</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <font color="red">*</font> Project Code</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="upProCode">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlProjectCode" MsgObrigatorio="Project Code" AutoPostBack="true"
                                                                CssClass="select2" Width="100%" TabIndex="4" runat="server">
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
                                                    <font color="red">*</font> Customer Name</label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomer" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="5" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                AutoPostBack="True" Visible="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                    <%--Is Verbal--%>
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkIsVerbal" runat="server" CssClass="checker" TabIndex="6" Checked="false"
                                                                    OnCheckedChanged="chkIsVerbal_CheckedChanged" Visible="false" AutoPostBack="true" />
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> PO No.</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtPONumber" runat="server" placeholder="Customer PO Number" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="6" MsgObrigatorio="Customer PO Number" MaxLength="20"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right ">
                                                    <font color="red">*</font> PO Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCustPoDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                            ValidationGroup="Save" TabIndex="7" MsgObrigatorio="Sales Order Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtCustPoDate" PopupButtonID="txtCustPoDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlEnquiryNo" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="4" AutoPostBack="True" MsgObrigatorio="Enquiry No" placeholder="Enquiry No"
                                                                OnSelectedIndexChanged="ddlEnquiryNo_SelectedIndexChanged" Visible="false">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <%--Project Name--%></label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control" placeholder="Project Name" Style="text-transform: uppercase"
                                                        ID="txtProjName" TabIndex="4" runat="server" Visible="false" TextMode="SingleLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Code</label>
                                                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="8" AutoPostBack="True" MsgObrigatorio="Item Code" Visible="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <div class="col-md-6">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                    runat="server" TabIndex="9" MsgObrigatorio="Item Name" AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Unit</label>
                                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlUnit" CssClass="select2" Width="100%" runat="server" TabIndex="10"
                                                                    AutoPostBack="True" MsgObrigatorio="Unit" Visible="True">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lblUnit" Visible="false" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Quantity</label>
                                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtOrderQty"
                                                                    Text="0.000" TabIndex="11" MaxLength="20" OnTextChanged="txtOrderQty_OnTextChanged"
                                                                    MsgObrigatorio="Order Qty." onkeypress="return validateFloatKeyPress(this,event);"
                                                                    placeholder="Order Qty" runat="server"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtOrderQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font>Gross Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanelG37" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" OnTextChanged="txtRate_TextChanged"
                                                                    ID="txtGrossRate" Text="0.00" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    TabIndex="12" runat="server" MaxLength="20" MsgObrigatorio="Rate" placeholder="Rate"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtGrossRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Discount Per</label>
                                                        <asp:UpdatePanel ID="UpdatePanelD38" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" OnTextChanged="txtRate_TextChanged"
                                                                    ID="txtDiscPer" Text="0.00" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    TabIndex="13" runat="server" MaxLength="20" MsgObrigatorio="Rate" placeholder="Rate"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtDiscPer"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" OnTextChanged="txtRate_TextChanged"
                                                                    ID="txtRate" Text="0.00" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    TabIndex="14" ReadOnly="true" runat="server" MaxLength="20" MsgObrigatorio="Rate"
                                                                    placeholder="Rate" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Amount</label>
                                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtAmount" Text="0.00" TabIndex="15"
                                                                    runat="server" placeholder="Rate" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    AutoPostBack="True" ReadOnly="true" MaxLength="20"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtAmount"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtOrderQty" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtRate" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Attach Document
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                                            <ContentTemplate>
                                                                <asp:FileUpload ID="imgUpload" ClientIDMode="Static" onchange="this.form.submit()"
                                                                    TabIndex="16" runat="server" />
                                                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" Style="display: none" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Mod. No.</label>
                                                        <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtModNo"
                                                                    Text="0.000" TabIndex="12" MaxLength="20" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    placeholder="Mod. No." runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Mod Date</label>
                                                        <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtModDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                        ValidationGroup="Save" TabIndex="8" ReadOnly="true"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                        TargetControlID="txtModDate" PopupButtonID="txtModDate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Amort Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtAmortRate" Text="0.00" TabIndex="14"
                                                                    runat="server" placeholder="Rate" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    MaxLength="20" OnTextChanged="txtAmortRate_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtAmortRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Tax Category</label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTaxCategory" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="15" AutoPostBack="True" Visible="True" MsgObrigatorio="Tax Name">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Customer Item Code</label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtCustItemCode" placeholder="Customer Item Code"
                                                                    TabIndex="17" runat="server" MaxLength="200"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Customer Item Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtCustItemName" placeholder="Customer Item Name"
                                                                    TabIndex="18" runat="server" MaxLength="200"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Status</label>
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                            <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbtStatus" runat="server" AutoPostBack="True" TabIndex="19"
                                                                    RepeatDirection="Horizontal" CssClass="checker" CellPadding="10">
                                                                    <asp:ListItem Value="0" Selected="True"> &nbsp;Active</asp:ListItem>
                                                                    <asp:ListItem Value="1"> &nbsp;Short Close</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Trasport Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtTransRate" Text="0.00" TabIndex="20"
                                                                    runat="server" placeholder="Rate" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    MaxLength="20" OnTextChanged="txtTransRate_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtTransRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Die Amortisation Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtDieAmorRate" Text="0.00"
                                                                    TabIndex="21" runat="server" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    MaxLength="20" OnTextChanged="txtDieAmorRate_OnTextChanged" placeholder="Rate"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtDieAmorRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Die Amortisation Qty</label>
                                                        <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtDieAmortQty" Text="0.00"
                                                                    TabIndex="22" runat="server" onkeypress="return validateFloatKeyPress(this,event);"
                                                                    MaxLength="20" placeholder="Qty" OnTextChanged="txtDieAmortQty_OnTextChanged"
                                                                    AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtDieAmortQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Cast Weight</label>
                                                        <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtCastWeight" OnTextChanged="txtCastWeight_OnTextChanged" Text="0.00" TabIndex="23"
                                                                    runat="server" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="20"
                                                                    placeholder="Qty" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtCastWeight"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Finish Weight</label>
                                                        <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtFinishwgt" OnTextChanged="txtFinishwgt_OnTextChanged" Text="0.00" TabIndex="24"
                                                                    runat="server" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="20"
                                                                    placeholder="Qty" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtFinishwgt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Turning Weight</label>
                                                        <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtTurningwgt" OnTextChanged="txtTurningwgt_OnTextChanged" Text="0.00" TabIndex="25"
                                                                    runat="server" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="20"
                                                                    placeholder="Qty" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtTurningwgt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label label-sm">
                                                            &nbsp</label><br />
                                                        <asp:LinkButton ID="btnInsert" CssClass="btn blue" TabIndex="26" runat="server" OnClick="btnInsert_Click"
                                                            OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-o-down"> </i>  Insert</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row" style="overflow-x: auto;">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2566">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgMainPO" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            AutoGenerateColumns="False" CellPadding="4" TabIndex="27" ShowHeaderWhenEmpty="true"
                                                            OnRowCommand="dgMainPO_RowCommand" OnRowDeleting="dgMainPO_Deleting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSelect" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                            CausesValidation="False" CommandName="Select" Text="Modify" CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                            CausesValidation="False" CommandName="Delete" CommandArgument='<%#Container.DataItemIndex%>'
                                                                            Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="Item Code" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblShortName" runat="server" Width="180px" Text='<%#Eval("ShortName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemName" runat="server" Width="180px" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Width="50px" Text='<%# Eval("Unit") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Order Qty." SortExpression="OrderQty" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOrderQty" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("OrderQty")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dispatch Qty." SortExpression="CPOD_DISPACH" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_DISPACH" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_DISPACH")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Gross Rate" SortExpression="CPOD_GROSS_RATE" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_GROSS_RATE" CssClass=" Control-labelt" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_GROSS_RATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discount Per." SortExpression="CPOD_DISC_PER" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_DISC_PER" CssClass=" Control-labelt" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_DISC_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="P_CODE" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRate" CssClass=" Control-labelt" runat="server" Width="100px" Text='<%# Eval("Rate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%#Eval("Amount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Item Code" Visible="false" SortExpression="Customer Item Name"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustItemCode" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CustItemCode")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Item Name" Visible="false" SortExpression="Customer Item Name"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustItemName" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CustItemName")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mod No." Visible="false" SortExpression="CPOD_MODNO"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblModNo" CssClass=" Control-label" runat="server" Width="100px" Text='<%# Eval("ModNo")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mod Date" Visible="false" SortExpression="CPOD_MODDATE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblModDate" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("ModDate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amort Rate" Visible="false" SortExpression="CPOD_AMORTRATE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmortRate" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("AmortRate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Category" Visible="false" SortExpression="Tax Category"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxCategory" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("TaxCategory") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="taxCatCode" SortExpression="taxCatCode" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxCatCode" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("TaxCatCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StatusInd" SortExpression="StatusInd" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatusInd" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("StatusInd") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" CssClass="Control-label" runat="server" Width="100px" Text='<%# Eval("Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Document View" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                            CommandName="ViewPDF" Text='<%# Eval("DocName") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Die Amortisation Rate" SortExpression="CPOD_AMORTRATE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDieAmortRate" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_DIEAMORTRATE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Die Amortisation QTY" SortExpression="CPOD_DIEAMORTQTY"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDieAmortQTY" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_DIEAMORTQTY")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Transport Rate" SortExpression="CPOD_TRANSPORT_RATE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_TRANSPORT_RATE" CssClass="Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_TRANSPORT_RATE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cast Weight" SortExpression="CPOD_CAST_WEIGHT" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_CAST_WEIGHT" CssClass="Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_CAST_WEIGHT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Finish Weight" SortExpression="CPOD_FINISH_WEIGHT"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_FINISH_WEIGHT" CssClass="Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_FINISH_WEIGHT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Turning Weight" SortExpression="CPOD_TURNING_WEIGHT"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_TURNING_WEIGHT" CssClass="Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CPOD_TURNING_WEIGHT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit Code" SortExpression="I_UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_UOM_CODE" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("I_UOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Payment Term</label>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtPayTerm" placeholder="Payment Term" TabIndex="28"
                                                                runat="server" MaxLength="260"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <asp:Label Text="Basic Amount" runat="server" ID="lblBasicAmt" class="col-md-2 control-label label-sm" />
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control  input-sm text-right" ID="txtBasicAmount" placeholder="0.00"
                                                                TabIndex="29" runat="server" OnTextChanged="txtBasicAmount_TextChanged" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtBasicAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="divExport" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlExport">
                                            <div class="form-horizontal">
                                                <div class="form-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Buyer Name</label>
                                                                <div class="col-md-5">
                                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtBuyerName" placeholder="Buyer Name" TabIndex="30"
                                                                                runat="server" MaxLength="50"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <label class="col-md-2 control-label label-sm">
                                                                    <font color="red">*</font> Currency
                                                                </label>
                                                                <div class="col-md-3">
                                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="ddlCurrancy" AutoPostBack="true" CssClass="select2_category form-control"
                                                                                runat="server" TabIndex="31">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Buyer Address
                                                                </label>
                                                                <div class="col-md-5">
                                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtBuyerAdd" TabIndex="32" runat="server"
                                                                                placeholder="Buyer Address" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Port Loading
                                                                </label>
                                                                <div class="col-md-3">
                                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtPortLoad" TabIndex="33" runat="server"
                                                                                placeholder="Port Loading"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Final Destination
                                                                </label>
                                                                <div class="col-md-5">
                                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtFinalD" TabIndex="34" runat="server"
                                                                                placeholder="Final Destination"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Port Discharge
                                                                </label>
                                                                <div class="col-md-3">
                                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtPortDis" TabIndex="35" runat="server"
                                                                                placeholder="Port Discharge"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group">
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Pre Carriage By
                                                                </label>
                                                                <div class="col-md-5">
                                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtPreCarr" TabIndex="36" runat="server"
                                                                                placeholder="Pre Carriage By"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <label class="col-md-2 control-label label-sm">
                                                                    Place Of Delivery
                                                                </label>
                                                                <div class="col-md-3">
                                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox CssClass="form-control" ID="txtPlace" TabIndex="37" runat="server" placeholder="Place Of Delivery"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPOType" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="38" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="39" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                            <ContentTemplate>
                                <asp:LinkButton ID="LnkDoc" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                <cc1:ModalPopupExtender runat="server" ID="ModalPopDocument" BackgroundCssClass="modalBackground"
                                    OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                    PopupControlID="PopDocument" TargetControlID="LnkDoc">
                                </cc1:ModalPopupExtender>
                                <asp:Panel class="col-md-6" ID="PopDocument" runat="server" Style="display: none;"
                                    ScrollBars="Both" Height="70%" Width="80%">
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="captionPopup">
                                                Document View
                                            </div>
                                        </div>
                                        <div class="portlet-body form" style="height: 400px">
                                            <div class="form-horizontal">
                                                <div class="form-body">
                                                    <div class="row">
                                                        <label class="col-md-12 control-label">
                                                        </label>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                                                <ContentTemplate>
                                                                    <iframe runat="server" id="IframeViewPDF" width="100%" height="100%"></iframe>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="dgMainPO" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-offset-5 col-md-9">
                                                            <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
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
    <%-- </div>--%>
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

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
    function VerificaCamposObrigatorios() 
    { try { if (VerificaValorCombo('#<%=ddlPOType.ClientID%>','#Avisos') == false)
     { $("#Avisos").fadeOut(6000); return false; }
    else if (VerificaObrigatorio('#<%=txtPONumber.ClientID%>','#Avisos') == false)
     { $("#Avisos").fadeOut(6000); return false; }
     else if (VerificaObrigatorio('#<%=txtPODate.ClientID%>','#Avisos') == false)
     { $("#Avisos").fadeOut(6000); return false; } 
     else if (VerificaObrigatorio('#<%=ddlEnquiryNo.ClientID%>','#Avisos') == false)
     {$("#Avisos").fadeOut(6000); // return false; // } else
    if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false)
     { $("#Avisos").fadeOut(6000);return false; }
     else if (VerificaValorCombo('#<%=ddlItemCode.ClientID%>', '#Avisos') == false)
      { $("#Avisos").fadeOut(6000); return false; }
     else if (VerificaValorCombo('#<%=ddlItemName.ClientID%>','#Avisos') == false)
      { $("#Avisos").fadeOut(6000); return false; }
     else if (VerificaObrigatorio('#<%=txtRate.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
     else if (VerificaValorCombo('#<%=ddlTaxCategory.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; }
     else { $("#Avisos").fadeOut(6000);return true;
      } } catch (err) { alert('Erro in Required Fields: ' + err.description);
    return false;
     } }
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
    </label>
</asp:Content>
