<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Issue.aspx.cs"
    Inherits="Transactions_ADD_Issue" Title="Store To Store Casting Movement" %>

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
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=txtReqPerson.ClientID %>").select2();
            jQuery("#<%=ddlTostore.ClientID %>").select2();
            jQuery("#<%=ddlStoreType.ClientID %>").select2();
        });
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
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Store To Store Casting Movement
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" CssClass="remove" OnClick="btnCancel_Click" TabIndex="29"
                                    runat="server"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Issue No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtIssueNo" runat="server" CssClass="form-control" placeholder="Issue To Prod. No"
                                                        ReadOnly="true" Enabled="false" TabIndex="1"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Issue Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    TabIndex="2" MsgObrigatorio="Please Enter Date" AutoPostBack="true" OnTextChanged="txtIssueDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy hh:mm"
                                                                    TargetControlID="txtIssueDate" PopupButtonID="txtIssueDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Issue Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlIssueType" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="3" AutoPostBack="true" MsgObrigatorio="Issue Type" OnSelectedIndexChanged="ddlIssueType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Select Issue Type</asp:ListItem>
                                                                <asp:ListItem Value="1">As Per Material Requirment</asp:ListItem>
                                                                <asp:ListItem Value="2" Selected="True">Direct</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlMaterialReq" AutoPostBack="true" CssClass="select2_category form-control"
                                                                runat="server" TabIndex="4" OnSelectedIndexChanged="ddlMaterialReq_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Issue By
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="txtIssueBy" runat="server" CssClass="form-control" placeholder="Issue by"
                                                                TabIndex="4" MsgObrigatorio="Please Enter Issue By"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label">
                                                Requested By
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="txtReqPerson" CssClass="select2" Width="400px" runat="server"
                                                                TabIndex="3" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> From Store
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlStoreType" runat="server" Width="100%" TabIndex="1" CssClass="select2"
                                                                AutoPostBack="True" MsgObrigatorio="From Store" OnSelectedIndexChanged="ddlStoreType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> To Store</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlTostore" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Item Name" TabIndex="7" placeholder="Select To Store" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlTostore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
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
                                                                <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="95%" runat="server"
                                                                    TabIndex="6" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged" MsgObrigatorio="Item Code"
                                                                    placeholder="Select Item Code" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlIssueType" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
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
                                                                <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="95%" runat="server"
                                                                    MsgObrigatorio="Item Name" TabIndex="7" placeholder="Select Item Name" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
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
                                                            Unit</label>
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlUOM" placeholder="Unit" CssClass="select2_category form-control input-sm"
                                                                    runat="server" TabIndex="8" Enabled="false">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
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
                                                            Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" Width="90%" ID="txtrate"
                                                                    placeholder="0.000" TabIndex="8" Enabled="false" ReadOnly="true" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Current Stock</label>
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" Width="90%" ID="txtCurrStock"
                                                                    placeholder="0.000" TabIndex="8" Enabled="false" ReadOnly="true" runat="server">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> transfer Qty</label>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" Width="90%" ID="txtRequiredQty"
                                                                    MsgObrigatorio="Required Qty" placeholder="0.000" TabIndex="9" AutoPostBack="true"
                                                                    runat="server" OnTextChanged="txtRequiredQty_TextChanged">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtRequiredQty"
                                                                    ValidChars="0123456789" runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtIssueQty" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" Width="90%" ID="txtIssueQty"
                                                                    placeholder="0.000" Visible="false" TabIndex="9" runat="server" MsgObrigatorio="Please Enter Issue Qty"
                                                                    AutoPostBack="true" OnTextChanged="txtIssueQty_TextChanged"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                        ID="FilteredTextBoxExtender1" TargetControlID="txtIssueQty" ValidChars="0123456789."
                                                                        runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Amount</label>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" Width="90%" ID="txtAmount"
                                                                    placeholder="0.000" TabIndex="10" runat="server" MsgObrigatorio="Amount" AutoPostBack="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtAmount"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtIssueQty" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgIssueTo" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Remark</label>
                                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtRemark" Width="90%" placeholder=" Remark"
                                                                    TabIndex="10" runat="server"></asp:TextBox></ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-5 text-right">
                                                    <label class="control-label label-sm">
                                                    </label>
                                                    <br />
                                                    <asp:Button ID="btnInsert" CssClass="btn green" runat="server" Text="Insert" OnClick="btnInsert_Click"
                                                        TabIndex="11" />
                                                </div>
                                            </div>
                                            <div class="row" style="overflow: auto; width: 100%">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgIssueTo" runat="server" TabIndex="13" Style="width: 100%;" AutoGenerateColumns="False"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" CellPadding="4"
                                                                GridLines="Both" OnRowCommand="dgIssueTo_RowCommand" OnRowDeleting="dgIssueTo_RowDeleting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkModify" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                                CausesValidation="False" CommandName="Modify" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton></ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                                CausesValidation="False" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                Text="Delete"></asp:LinkButton></ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="temCode" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCode" Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_I_CODENO" runat="server" Text='<%# Bind("ItemCodeNo") %>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name" SortExpression="ItemCode" Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_I_NAME" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Stock Unit" SortExpression="UOM" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUOM_CODE" runat="server" Text='<%# Bind("UOM_CODE") %>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit" SortExpression="UOM" Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("StockUOM") %>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Current Stock" SortExpression="PO No" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCurrStock" CssClass=" Control-label pull-right" runat="server"
                                                                                Text='<%# Bind("CurrStock") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Required Qty" SortExpression="POCODE" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQtyRequirment" CssClass=" Control-label pull-right" runat="server"
                                                                                Text='<%# Bind("IMD_REQ_QTY") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Transfer Qty" SortExpression="StockQty" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtIssueQty" runat="server" CssClass="form-control text-right" placeholder="0.00"
                                                                                MsgObrigatorio="Qty" AutoPostBack="true" OnTextChanged="txtIssueQty1_TextChanged"
                                                                                Text='<%# Eval("IssueQty") %>' MaxLength="50" TabIndex="14" Enabled="false"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtIssueQty"
                                                                                ValidChars="0123456789." runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remark" SortExpression="Remark" Visible="True" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" placeholder="Remark"
                                                                                Text='<%# Eval("Remark") %>' MaxLength="50" TabIndex="14" Enabled="false"></asp:TextBox></ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIMD_RATE" CssClass=" Control-label pull-right" runat="server" Text='<%# Bind("IMD_RATE") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIMD_AMOUNT" CssClass=" Control-label pull-right" runat="server"
                                                                                Text='<%# Bind("IMD_AMOUNT") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Store" SortExpression="IMD_To_STORE" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIMD_To_STORE" CssClass=" Control-label pull-right" runat="server"
                                                                                Text='<%# Bind("IMD_To_STORE") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Store" SortExpression="IMD_To_STORE" Visible="false"
                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIMD_To_STORE_CODE" CssClass=" Control-label pull-right" runat="server"
                                                                                Text='<%# Bind("IMD_To_STORE_CODE") %>'></asp:Label></ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
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
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="15" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                                <ContentTemplate>
                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton><cc1:ModalPopupExtender
                                        runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
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
                                                                    OnClick="btnOk_Click">&nbsp;Yes </asp:LinkButton>
                                                                <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                    OnClick="btnCancel1_Click"> No</asp:LinkButton></div>
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
</asp:Content>
