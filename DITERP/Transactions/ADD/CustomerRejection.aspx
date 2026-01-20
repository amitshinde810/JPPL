<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CustomerRejection.aspx.cs"
    Inherits="Transactions_ADD_CustomerRejection" Title="Customer Rejection" EnableEventValidation="true" %>

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
    </style>

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtInvoiceNo.ClientID %>', '');
        };
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
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
        }
    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; margin-bottom: 10px; width: 100%; border: 1px solid #9f6000">
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
                                <i class="fa fa-reorder"></i>Customer Rejection
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
                                        <div class="col-md-25">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel ID="UpdatePane16" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="optLstType_SelectedIndexChanged"
                                                                ID="optLstType" runat="server" CssClass="checker" TabIndex="1">
                                                                <asp:ListItem Value="1" Text="&nbsp With Invoice Reference &nbsp&nbsp" Selected="True" />
                                                                <asp:ListItem Text="&nbsp Without Invoice Reference" Value="2" />
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right ">
                                                        GIN No.</label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox CssClass="form-control" ID="txtGINNo" placeholder="GIN No. " TabIndex="2"
                                                            runat="server" ReadOnly="true">
                                                        </asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <span class="required">*</span> GIN Date</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" ID="txtGINDate" placeholder="dd MMM yyyy" TabIndex="3"
                                                                runat="server">
                                                            </asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtGINDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right ">
                                                        <span class="required">*</span> Challan No.</label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox CssClass="form-control" ID="txtChallanNo" placeholder="Challan No. "
                                                            TabIndex="4" runat="server">
                                                        </asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <span class="required">*</span> Challan Date</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" ID="txtChallanDate" placeholder="dd MMM yyyy"
                                                                TabIndex="5" runat="server">
                                                            </asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                TargetControlID="txtChallanDate" PopupButtonID="txtChallanDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right ">
                                                        <span class="required">*</span> Customer name</label>
                                                    <div class="col-md-8">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2" Width="500px" runat="server"
                                                                    TabIndex="6" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="True"
                                                                    Visible="True">
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
                                                        Invoice No.</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtInvoiceNo" AutoPostBack="true" OnTextChanged="txtInvoiceNo_TextChanged"
                                                                    placeholder="Invoice No. " TabIndex="7" runat="server">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        Invoice Date</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" ID="txtInvDate" placeholder="Invoice Date" TabIndex="8"
                                                                        runat="server" AutoPostBack="true" OnTextChanged="txtInvDate_TextChanged">
                                                                    </asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                        TargetControlID="txtInvDate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:ListBox ID="lstview" runat="server" Visible="false" Width="200px"></asp:ListBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtInvoiceNo" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Item Code</label>
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="400px" runat="server"
                                                            TabIndex="9" AutoPostBack="True" Visible="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged1"
                                                            MsgObrigatorio="Item Code">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtInvDate" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtInvoiceNo" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-5">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Item Name</label>
                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="500px" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                            runat="server" TabIndex="10" AutoPostBack="True" Visible="True" MsgObrigatorio="Item Name">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtInvDate" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtInvoiceNo" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-1">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span>Unit</label>
                                                <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control" ID="txtUnit" placeholder="Eg. Kg " TabIndex="11"
                                                            runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblUnit" Visible="false" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> PO No.</label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlPONo" CssClass="select2_category form-control" runat="server"
                                                            TabIndex="12" AutoPostBack="True" Visible="True" MsgObrigatorio="Item Name" OnSelectedIndexChanged="ddlPONo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Rate</label>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass=" form-control text-right" OnTextChanged="txtRate_TextChanged"
                                                            Enabled="false" ID="txtRate" Text="0.00" TabIndex="13" runat="server" placeholder="Rate"
                                                            AutoPostBack="True"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtRate"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Max. Returnable Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass=" form-control text-right" ID="txtOrigionalQty" placeholder="0.000"
                                                            runat="server" ReadOnly="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Challan Qty</label>
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass=" form-control text-right" ID="txtChallanQty" Text="0.000"
                                                            TabIndex="15" runat="server" placeholder="Rate" AutoPostBack="True" OnTextChanged="txtChallanQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtChallanQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="control-label label-sm">
                                                    <span class="required">*</span> Received Quantity</label>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass=" form-control text-right" ID="txtReceivedQty" ToolTip="Received Quantity"
                                                            Text="0.000" TabIndex="16" AutoPostBack="True" runat="server" OnTextChanged="txtReceivedQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtReceivedQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label><br />
                                                    <asp:LinkButton ID="btnInsert" CssClass="btn blue" TabIndex="17" runat="server" OnClick="btnInsert_Click"
                                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-Load"> </i>  Insert</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="control-group">
                                                <div class="col-md-12" style="overflow-x: auto;">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgCustomerRejection" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                AutoGenerateColumns="False" CellPadding="4" ShowHeaderWhenEmpty="true" OnRowCommand="dgCustomerRejection_RowCommand"
                                                                OnRowDeleting="dgCustomerRejection_Deleting">
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
                                                                                CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                                CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
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
                                                                    <asp:TemplateField HeaderText="Unit" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnit" runat="server" Width="50px" Text='<%# Eval("Unit") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UOM_CODE" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnitCode" runat="server" Width="50px" Text='<%# Eval("UnitCode") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO_Code" SortExpression="PO_Code" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPO_Code" CssClass=" control-label" runat="server" Width="100px"
                                                                                Text='<%# Eval("PO_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO No" SortExpression="PO_No" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPO_No" CssClass=" Control-label" runat="server" Width="100px" Text='<%# Eval("PO_No") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRate" CssClass=" Control-label" runat="server" Width="100px" Text='<%# Eval("Rate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Original Qty" SortExpression="Original Qty" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOriginalQty" CssClass=" Control-label" runat="server" Width="100px"
                                                                                Text='<%# Eval("OriginalQty")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Challan Qty" SortExpression="Challan Qty" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblChallanQty" CssClass="Control-text text-right" runat="server" Width="100px"
                                                                                Text='<%# Eval("ChallanQty")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass=" Control-text text-right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Received Qty" SortExpression="Received Qty" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReceivedQty" CssClass="Control-text text-right" runat="server"
                                                                                Width="100px" Text='<%# Eval("ReceivedQty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass=" Control-text text-right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" CssClass="Control-text text-right" runat="server" Width="100px"
                                                                                Text='<%# Eval("Amount") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass=" Control-text text-right" />
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
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-7 control-label label-sm">
                                                    Net Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtNetAmount" placeholder="0.00"
                                                                TabIndex="29" runat="server" OnTextChanged="txtNetAmount_TextChanged" AutoPostBack="true"
                                                                Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtNetAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-7 control-label label-sm">
                                                    CGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right" ID="txtBasicExcPer"
                                                                AutoPostBack="true" placeholder="0.00" TabIndex="28" runat="server" Text="0.00"
                                                                OnTextChanged="txtBasicExcPer_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtBasicExcPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ReadOnly="true" ID="txtBasicExcise"
                                                                placeholder="0.00" TabIndex="30" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--raw-->
                                    <!--raw-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-7 control-label label-sm">
                                                    SGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right" ID="txtducexcper"
                                                                placeholder="0.00" TabIndex="31" runat="server" Text="0.00" OnTextChanged="txtducexcper_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtducexcper"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ReadOnly="true" ID="txtEduCess" placeholder="0.00"
                                                                TabIndex="30" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--raw-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-7 control-label label-sm">
                                                    IGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right" ID="txtSHEExcPer"
                                                                placeholder="0.00" TabIndex="34" runat="server" Text="0.00" OnTextChanged="txtSHEExcPer_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtSHEExcPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ReadOnly="true" ID="txtHigherEduCess"
                                                                placeholder="0.00" TabIndex="31" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <!--raw-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-7 control-label label-sm">
                                                        <span class="required">*</span> Tax Name
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTaxName" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="36" AutoPostBack="True" OnSelectedIndexChanged="ddlTaxName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="optLstType" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--raw-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-7 control-label label-sm">
                                                        Sales tax %
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtSalesTaxPer" placeholder="0.00"
                                                                    TabIndex="38" runat="server" Text="0.00" OnTextChanged="txtSalesTaxPer_TextChanged"
                                                                    Enabled="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" TargetControlID="txtSalesTaxPer"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="selectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ReadOnly="true" ID="txtSalesTax"
                                                                    placeholder="0.00" TabIndex="31" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="selectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--raw-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-7 control-label label-sm">
                                                        Grand Amount
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel31" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtGrandAmt" placeholder="0.00"
                                                                    TabIndex="40" runat="server" Enabled="false" OnTextChanged="txtGrandAmt_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" TargetControlID="txtGrandAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtNetAmount" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtHigherEduCess" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtEduCess" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtBasicExcise" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgCustomerRejection" EventName="RowCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                                UseSubmitBehavior="false" CssClass="btn green" TabIndex="18" runat="server" Text="Save"
                                                OnClick="btnSubmit_Click1" />
                                            <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="19" runat="server"
                                                OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
