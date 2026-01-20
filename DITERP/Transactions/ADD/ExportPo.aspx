<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ExportPo.aspx.cs" Inherits="Transactions_ADD_ExportPo1" %>

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
            $find('ModalPopupExtenderConfirm').hide();
            __doPostBack('btnOk1', e);
        }
        
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
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
                                <i class="fa fa-reorder"></i>Export Po1 Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a><a href="javascript:;" class="remove">
                                </a>
                            </div>
                        </div>
                        <!-- Start Tabs Setting -->
                        <!-- End Tabs Setting -->
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span>PO No.
                                                </label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control" placeholder="PO No." ID="txtPONo" TabIndex="2"
                                                            runat="server" MsgObrigatorio="Please Enter PO No." TextMode="SingleLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> PO Date
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtPODate" TabIndex="2"
                                                            runat="server" MsgObrigatorio="Please Enter PO Date" TextMode="SingleLine"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtPODate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label label-sm">
                                                <span class="required">*</span> Customer
                                            </label>
                                            <div class="col-md-6">
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" ID="ddlCustomer"
                                                            AutoPostBack="true" CssClass="select2_category form-control" runat="server" MsgObrigatorio="Please Select Customer"
                                                            TabIndex="3">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label">
                                                Credit Days:
                                            </label>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <asp:TextBox CssClass="form-control" placeholder="0" ID="TextBox1" TabIndex="2" runat="server"
                                                        TextMode="SingleLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <!--/row-->
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" MsgObrigatorio="Please Select Po Type" CssClass="select2_category form-control input-sm"
                                                                runat="server" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Name</label>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" MsgObrigatorio="Please Select Po Type" CssClass="select2_category form-control input-sm"
                                                                runat="server" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" placeholder="0" ID="txtUOM" TabIndex="2" runat="server"
                                                                TextMode="SingleLine" ReadOnly="True"></asp:TextBox>
                                                            <asp:Label runat="server" ID="lblUnit" Visible="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Order Qty</label>
                                                    <asp:TextBox CssClass="form-control" placeholder="0.000" ID="txtOrderQty" TabIndex="2"
                                                        runat="server" MsgObrigatorio="Please Enter Order Qty" TextMode="SingleLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <!--/span-->
                                        </div>
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Rate (In USD)</label>
                                                    <asp:TextBox CssClass="form-control" placeholder="0.000" ID="txtRate" TabIndex="2"
                                                        runat="server" TextMode="SingleLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Currency</label>
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCurrancy" AutoPostBack="true" CssClass="select2_category form-control"
                                                                runat="server" TabIndex="3">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Cust.Item Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCustomerItemCode"
                                                                placeholder="Customer Item Code" TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Cust.Item Name
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCustomerItemName"
                                                                placeholder="Customer Item Name" TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-1">
                                                <label class="control-label label-sm">
                                                    Cust.Wgt.</label>
                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCustWeight" placeholder="Cust Weight"
                                                    TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Store Loc.</label>
                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtStoreLoc" placeholder=" Store Loc"
                                                    TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Tax Category
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlTaxCategory" AutoPostBack="true" CssClass="select2_category form-control"
                                                                runat="server" MsgObrigatorio="Please Select Tax Category" TabIndex="3">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Excise%</label>
                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtExcise" placeholder="0"
                                                        TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label  label-sm">
                                                        Payment Term</label>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPayTerms" placeholder="0"
                                                                TabIndex="10" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Active
                                                    </label>
                                                    <label class="checkbox">
                                                        <asp:CheckBox ID="chlActive" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                            TabIndex="20" />
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label><br />
                                                    <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" TabIndex="12" runat="server">
                                                    <i class="fa fa-arrow-circle-down">Insert </i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="control-group">
                                                    <div class="col-md-12" style="overflow-x: auto;">
                                                        <asp:GridView ID="dgMainPO" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            AutoGenerateColumns="False" CellPadding="4" TabIndex="16" ShowHeaderWhenEmpty="true"
                                                            OnRowCommand="dgMainPO_RowCommand" OnRowDeleting="dgMainPO_Deleting">
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
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="ShortName" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblShortName" runat="server" Width="180px" Text='<%# Eval("ShortName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="ItemName" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemName" runat="server" Width="180px" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="Unit" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Width="50px" Text='<%# Eval("Unit") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Order Qty." SortExpression="OrderQty" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOrderQty" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("OrderQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="P_CODE" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRate" CssClass=" Control-label" runat="server" Width="100px" Text='<%# Eval("Rate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CurrCode" SortExpression="CurrCode" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrCode" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CurrCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Curr Name" SortExpression="CurrName" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrName" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CurrName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Item Code" SortExpression="Customer Item Name"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustItemCode" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CustItemCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Item Name" SortExpression="Customer Item Name"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustItemName" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CustItemName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Weight" SortExpression="Customer Weght" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustWght" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("CustWght") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Category" SortExpression="Tax Category" HeaderStyle-HorizontalAlign="Left">
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
                                                                        <asp:Label ID="lblStatus" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="27" runat="server"
                                        OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="28" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div> </div>
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

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtPODate.ClientID%>', '#Avisos') == false) {
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
