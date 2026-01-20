<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="MaterialRequisition.aspx.cs"
    Inherits="Transactions_ADD_MaterialRequisition" Title="Material Requisition" %>

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
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
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
                                <i class="fa fa-reorder"></i>Material Requisition
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <asp:Panel ID="MainPanel" runat="server">
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Department</label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtDepartment" msgObrigatorio="Enter Department Name"
                                                                    placeholder="Department" ReadOnly="true" Text="Production" TabIndex="1" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Batch No
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlbatchNo" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Batch No" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlbatchNo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:TextBox CssClass="form-control" ID="txtBatchNo" Visible="false" runat="server"></asp:TextBox>
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
                                                        <span class="required">* </span>Date</label>
                                                    <div class="col-md-4">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                ValidationGroup="Save" TabIndex="3" msgObrigatorio="Please Select Date" ReadOnly="false"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtDate" PopupButtonID="txtDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Formula
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlFormualCode" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Formula Code" TabIndex="4" Enabled="false" OnSelectedIndexChanged="ddlFormualCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
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
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Type</label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlType" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Type" TabIndex="5" Enabled="false" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Text="Select">Select Type</asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="AsPerOrder">As Per Batch</asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Direct">Direct</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Order No.
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlOrderNo" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Order No." TabIndex="6" Enabled="false" OnSelectedIndexChanged="ddlOrderNo_SelectedIndexChanged">
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
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">*</span> Required For</label>
                                                    <div class="col-md-6">
                                                        <asp:UpdatePanel ID="UpdatePanel444" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemName" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Required For" Enabled="false" TabIndex="7" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                                </asp:DropDownList>
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
                                            <div class="col-md-3">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Process</label>
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlProcess" CssClass="select2_category form-control" runat="server"
                                                            TabIndex="8" AutoPostBack="True" MsgObrigatorio="Process Name" Visible="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Material Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlRawComponentCode" CssClass="select2_category form-control input-sm"
                                                                runat="server" MsgObrigatorio="Material Code" TabIndex="8" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlRawComponentCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRawComponentName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Material Name</label>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlRawComponentName" CssClass="select2_category form-control input-sm"
                                                                runat="server" MsgObrigatorio="Material Name" TabIndex="9" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlRawComponentName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRawComponentCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm ">
                                                        <span class="required"></span>Material Batch No.</label>
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItembatchno" CssClass="select2_category form-control input-sm"
                                                                runat="server" MsgObrigatorio="Material Batch No" TabIndex="10" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRawComponentCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRawComponentName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm ">
                                                        <span class="required">*</span> Required Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtVQty" placeholder="0.000"
                                                                TabIndex="11" runat="server" MsgObrigatorio="Please Enter Qty."></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtVQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm ">
                                                        <span class="required">*</span> Add In</label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAddIn" placeholder="0.000"
                                                                TabIndex="12" runat="server" MsgObrigatorio="Please Add In Qty."></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtAddIn"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm ">
                                                        <span class="required">*</span> Steps No.</label>
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSteps" AutoPostBack="true"
                                                                placeholder="0" TabIndex="13" runat="server" MsgObrigatorio="Please Enter Steps Number."></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtSteps"
                                                                ValidChars="0123456789" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgvBOMaterialDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label>
                                                    <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" OnClick="btnInsert_Click"
                                                        TabIndex="14" runat="server"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="overflow: auto; width: 100%">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgvBOMaterialDetails" runat="server" TabIndex="15" Style="width: 100%;"
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
                                                                <asp:TemplateField HeaderText="RawMaterialCode" SortExpression="SubComponentCode"
                                                                    Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBD_I_CODE" runat="server" Text='<%# Bind("BD_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Step No" SortExpression="STEP_NO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTEP_NO" runat="server" Text='<%# Bind("STEP_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Process Code" SortExpression="PROCESS_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPROCESS_CODE" runat="server" Text='<%# Bind("PROCESS_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Process Name" SortExpression="PROCESS_NAME" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPROCESS_NAME" runat="server" Text='<%# Bind("PROCESS_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Material Code" SortExpression="Sub Component Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Material Name" SortExpression="Sub Component Name"
                                                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch Code" SortExpression="Batch Code" Visible="false"
                                                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBT_CODE" runat="server" Text='<%# Eval("BT_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch No" SortExpression="Batch No" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBT_NO" runat="server" Text='<%# Eval("BT_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Req.Qty ltr" SortExpression="Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBD_VQTY" runat="server" Text='<%# Eval("BD_VQTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty In Kg" SortExpression="WEIGHT_IN_KG" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblWEIGHT_IN_KG" runat="server" Width="100px" Text='<%# Eval("WEIGHT_IN_KG") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Stock Qty" SortExpression="Stock Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CURRENT_BAL" runat="server" Text='<%# Eval("I_CURRENT_BAL") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Minimum Qty" SortExpression="Minimum Quantity" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_MIN_LEVEL" runat="server" Text='<%# Eval("I_MIN_LEVEL") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Balance Qty" SortExpression="Balance Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMR_BALANCE_QTY" runat="server" Text='<%# Eval("MR_BALANCE_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Add In" SortExpression="Order Qty" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" ControlStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtADD_IN_QTY" runat="server" CssClass="form-control text-right"
                                                                            placeholder="" Text='<%# Eval("ADD_IN_QTY") %>' TabIndex="16" AutoPostBack="false"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtADD_IN_QTY"
                                                                            ValidChars="0123456789." runat="server" />
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
                                                        <asp:AsyncPostBackTrigger ControlID="ddlbatchNo" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btn_Calculate" CssClass="btn blue  btn-sm" OnClick="btn_Calculate_Click"
                                                                TabIndex="17" runat="server"><i class="fa fa-arrow-circle-down"> </i>  Calculate </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Add In Total
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtAddInTotal" placeholder="0.00"
                                                                runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtAddInTotal"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btn_Calculate" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="17" runat="server"
                                        OnClick="btnSubmit_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="18" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                if (VerificaObrigatorio('#<%=txtDepartment.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlbatchNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlItemName.ClientID%>', '#Avisos') == false) {
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
