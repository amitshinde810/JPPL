<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SubContractorInward.aspx.cs"
    Inherits="Transactions_ADD_SubContractorInward" Title="Sub Contractor Inward" %>

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
        .modalBackground
        {
            background-color: #8B8B8B;
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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlSupplier.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlInwardType.ClientID %>").select2();
            jQuery("#<%=ddlPoNumber.ClientID %>").select2();
            jQuery("#<%=ddlProcess.ClientID %>").select2();

        });
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
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sub Contractor Inward
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="18" runat="server" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                     <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label label-sm">
                                                  Select Plant
                                                </label>
                                                <div class="col-md-3">
                                                        <asp:RadioButtonList ID="rbtWithAmt"   runat="server" AutoPostBack="True"
                                                                TabIndex="2" RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtWithAmt_SelectedIndexChanged">
                                                                <asp:ListItem Value="-2147483648" Selected="True">&nbsp; Plant 1&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="-2147483647" >&nbsp;Plant 2</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    
                                                </label>
                                                <div class="col-md-3">
                                                    
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> Inward Type</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlInwardType" runat="server" AutoPostBack="true" TabIndex="1"
                                                                OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged" CssClass="select2"
                                                                Width="100%">
                                                                <asp:ListItem Value="0">As Per BOM</asp:ListItem>
                                                                <asp:ListItem Value="1">One To One</asp:ListItem>
                                                                <asp:ListItem Value="2">Online Rejection</asp:ListItem>
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
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> Supplier Name</label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplier" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"
                                                                MsgObrigatorio=" Supplier Name">
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
                                                <label class="col-md-3 control-label text-right">
                                                    GRN No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGRNno" placeholder="0"
                                                                TabIndex="3" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtGRNno"
                                                                FilterType="Numbers" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> GRN Date</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtGRNDate" runat="server" CssClass="form-control input-sm" placeholder=""
                                                            TabIndex="4" msgObrigatorio="Please Select GRN Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtGRNDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtGRNDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> Challan/Invoice No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control text-right input-sm"
                                                                placeholder="0" TabIndex="5" AutoPostBack="true" MaxLength="50" MsgObrigatorio=" Enter Challan No"
                                                                OnTextChanged="txtChallanNo_TextChanged"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Challan Date</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtChallanDate"
                                                            TabIndex="6" runat="server" MsgObrigatorio=" Select Challan Date" TextMode="SingleLine"
                                                            OnTextChanged="txtChallanDate_TextChanged"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtChallanDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div1" class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    Gate Pass No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control text-right input-sm"
                                                                placeholder="0" TabIndex="7" AutoPostBack="true" MaxLength="50" MsgObrigatorio=" Enter Gate Pass No"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                </label>
                                                <div class="col-md-3" runat="server" visible="false">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtInvoiceDate"
                                                            TabIndex="8" runat="server" MsgObrigatorio=" Select Invoice Date" TextMode="SingleLine"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtInvoiceDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label label-sm">
                                                    Exit Gate Pass No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtEgpNo" placeholder="0"
                                                                TabIndex="9" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Lr No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtLrNo" placeholder="0"
                                                                TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">
                                                    Vehicle No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtVehno" placeholder="0"
                                                                TabIndex="12" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Transporter Name
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtTransporterName" placeholder="Transporter Name"
                                                                TabIndex="13" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div2" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label label-sm">
                                                    Octroi Memo No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOctNo" placeholder="0"
                                                                TabIndex="11" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font> Item Code
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="11" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlInwardType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font> Item Name
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlInwardType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        PO No.
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlPoNumber" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="14" AutoPostBack="True" OnSelectedIndexChanged="ddlPoNumber_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-1">
                                                    <label class=" control-label label-sm">
                                                        PendingSch
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingSchQty" placeholder="0.000"
                                                                Enabled="false" Text="0.00" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <label class=" control-label label-sm">
                                                        Sch Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSchdeuleQTy" placeholder="0.000"
                                                                Enabled="false" Text="0.00" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class=" control-label label-sm">
                                                        Pending Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingQty" placeholder="0.000"
                                                                Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtPendingQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font>Challan Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtChallanQty" placeholder="0.000"
                                                                TabIndex="15" runat="server" AutoPostBack="true" MsgObrigatorio="Please Enter Challan Qty"
                                                                OnTextChanged="txtChallanQty_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtChallanQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font>Recd Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRecdQty" placeholder="0.000"
                                                                TabIndex="16" runat="server" AutoPostBack="true" MsgObrigatorio="Please Enter Recd Qty"
                                                                OnTextChanged="txtRecdQty_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtRecdQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class=" control-label label-sm">
                                                        Rate
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" placeholder="0.00"
                                                                Enabled="false" runat="server" AutoPostBack="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtRate"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged">
                                                            </asp:AsyncPostBackTrigger>
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class=" control-label label-sm">
                                                        Unit
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUom" CssClass="select2_category form-control" runat="server"
                                                                Enabled="false" TabIndex="17" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-1">
                                                </div>
                                                <div id="Div3" class="col-md-3">
                                                    <label class=" control-label label-sm">
                                                        Process Type
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlProcess" CssClass="select2" runat="server"
                                                                TabIndex="11" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3" runat="server" visible="false">
                                                    <label class=" control-label label-sm">
                                                        Remark
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-left input-sm" ID="txtRemark" placeholder="Remark"
                                                                TabIndex="18" runat="server" AutoPostBack="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3" runat="server" visible="false">
                                                    <label class=" control-label label-sm">
                                                        Lot/Batch No.
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control " ID="txtBatchNo" placeholder="Batch No" TabIndex="19"
                                                                runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div id="Div4" class="col-md-3" runat="server">
                                                    <label class=" control-label label-sm">
                                                        Turning Weight
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox Enabled="false" CssClass="form-control " ID="txtTurningWeight" Text="0.000"
                                                                placeholder="Tuning weight" TabIndex="19" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div id="Div5" class="col-md-3" runat="server">
                                                    <label class=" control-label label-sm">
                                                        Turning Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox Enabled="false" CssClass="form-control " ID="txtTurningQty" placeholder="Turning Qty"
                                                                TabIndex="19" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <label class=" control-label label-sm">
                                                    </label>
                                                    <br />
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnInsert" CssClass="btn blue" TabIndex="16" runat="server" OnClick="BtnInsert_Click">Insert</asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12" style="overflow: auto;">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgInwardMaster" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" OnRowCommand="dgInwardMaster_RowCommand"
                                                                OnRowDeleting="dgInwardMaster_Deleting">
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
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="IWD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_I_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IWD_I_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_Name" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO Code" SortExpression="IWD_CPOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_CPOM_CODE" CssClass="" runat="server" Visible="true" Text='<%# Eval("IWD_CPOM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO No" SortExpression="SPOM_PO_NO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSPOM_PO_NO" CssClass="" runat="server" Text='<%# Eval("SPOM_PO_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Recd. Qty" SortExpression="IWD_REV_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_REV_QTY" CssClass="" runat="server" Text='<%# Eval("IWD_REV_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Challan Qty" SortExpression="IWD_CH_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_CH_QTY" CssClass="" runat="server" Text='<%# Eval("IWD_CH_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Rate" SortExpression="IWD_RATE" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_RATE" CssClass="text-left" runat="server" Text='<%# Eval("IWD_RATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit" SortExpression="I_UOM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_UOM_NAME" CssClass="" runat="server" Text='<%# Eval("I_UOM_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UOMCODE" SortExpression="UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUOM_CODE" CssClass="" runat="server" Text='<%# Eval("IWD_UOM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remark" SortExpression="IWD_REMARK" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_REMARK" CssClass="" runat="server" Text='<%# Eval("IWD_REMARK") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Batch No" SortExpression="IWD_BATCH_NO" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_BATCH_NO" CssClass="" runat="server" Text='<%# Eval("IWD_BATCH_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PROCESS_CODE" SortExpression="IWD_PROCESS_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_PROCESS_CODE" CssClass="" runat="server" Text='<%# Eval("IWD_PROCESS_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Process Type" SortExpression="IWD_PROCESS_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPROCESS_NAME" CssClass="" runat="server" Text='<%# Eval("IWD_PROCESS_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IWD_TUR_QTY" SortExpression="IWD_TUR_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_TUR_QTY" CssClass="" runat="server" Text='<%# Eval("IWD_TUR_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IWD_TUR_WEIGHT" SortExpression="IWD_TUR_WEIGHT" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWD_TUR_WEIGHT" CssClass="" runat="server" Text='<%# Eval("IWD_TUR_WEIGHT") %>'></asp:Label>
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
                                   <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-7">
                                            </div>
                                            <label class="col-md-2 control-label">
                                                Total Amount
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTotalAmount" placeholder=""
                                                            TabIndex="23" Enabled="false" runat="server"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-7">
                                            </div>
                                            <label class="col-md-2 control-label">
                                                CGST
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtcgst" placeholder="0.00"
                                                            Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-7">
                                            </div>
                                            <label class="col-md-2 control-label">
                                                SGST
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel30" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtsgst" placeholder="0.00"
                                                            Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-7">
                                            </div>
                                            <label class="col-md-2 control-label">
                                                IGST
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel31" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtigst" placeholder="0.00"
                                                            Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-7">
                                            </div>
                                            <label class="col-md-2 control-label">
                                                Final Amount
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel32" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtFinalAmount" placeholder="0.00"
                                                            Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
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
                                                                        Do you Want to Cancel Record?
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
                                            <div class="form-actions fluid">
                                                <div class="col-md-offset-4 col-md-9">
                                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Save"
                                                        OnClick="btnSubmit_Click" />
                                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="18" runat="server"
                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                                </div>
                                            </div>
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

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

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlSupplier.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtGRNDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtChallanNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtChallanDate.ClientID%>', '#Avisos') == false) {
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

</asp:Content>
