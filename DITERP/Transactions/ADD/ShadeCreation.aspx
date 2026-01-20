<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ShadeCreation.aspx.cs"
    Inherits="Transactions_ADD_ShadeCreation" Title="Shade Creation" %>

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
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
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
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
                                <i class="fa fa-reorder"></i>Shade Creation
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
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Formula Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlFormulaType_SelectedIndexChanged" ID="ddlFormulaType"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="1" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Formula Type">
                                                                <asp:ListItem Selected="True" Value="0">Select Formula Type</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Location</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddllocation_SelectedIndexChanged" ID="ddllocation"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="2" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Location">
                                                                <asp:ListItem Selected="True" Value="0">Select Location</asp:ListItem>
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
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Gloss %</label>
                                                <div class="col-md-3 ">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtGloss" runat="server" placeholder="Gloss %" CssClass="form-control text-right"
                                                                    ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Gloss %" MaxLength="10" AutoPostBack="true"
                                                                    OnTextChanged="txtGloss_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon">+-5</span>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtGloss"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <label class="col-md-2 control-label text-right">
                                                        <font color="red">*</font> Color Int.</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList OnSelectedIndexChanged="ddlColorini_SelectedIndexChanged" ID="ddlColorini"
                                                                    CssClass="select2_category form-control" runat="server" TabIndex="4" AutoPostBack="True"
                                                                    Visible="True" MsgObrigatorio="Color">
                                                                    <asp:ListItem Selected="True" Value="0">Select Color</asp:ListItem>
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
                                                    <label class=" col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Formula Code</label>
                                                    <div class="col-md-3 ">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtFormulaCode" runat="server" placeholder="formula Code" CssClass="form-control"
                                                                    ValidationGroup="Save" TabIndex="5" MsgObrigatorio="Formula Code" MaxLength="100"
                                                                    ReadOnly="false"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class=" col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Formula Name</label>
                                                    <div class="col-md-5 ">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtFormulaName" runat="server" placeholder="Formula Name" CssClass="form-control"
                                                                    ValidationGroup="Save" TabIndex="6" MsgObrigatorio="Formula Name" MaxLength="100"></asp:TextBox>
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
                                                        <font color="red">*</font> Remarks/Notes</label>
                                                    <div class="col-md-3 ">
                                                        <asp:TextBox ID="txtRemark" runat="server" placeholder="Remark/Notes" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="7" MsgObrigatorio="Remark/Notes" MaxLength="200"></asp:TextBox>
                                                    </div>
                                                    <label class=" col-md-2 control-label text-right ">
                                                        Base Item</label>
                                                    <div class="col-md-3 ">
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlBaseItem" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="8" Visible="True" MsgObrigatorio="Base Item">
                                                                    <asp:ListItem Selected="True" Value="0">Select Base Item</asp:ListItem>
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
                                                    <label class=" col-md-2 control-label text-right ">
                                                        Project No</label>
                                                    <div class="col-md-3 ">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlProjectNo" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="9" Visible="True" MsgObrigatorio="Project No" AutoPostBack="true" OnSelectedIndexChanged="ddlProjectNo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right">
                                                        Customer Name
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel1663" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="10" MsgObrigatorio="Customer Name" Enabled="false">
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
                                                    <label class=" col-md-2 control-label text-right ">
                                                        Shade Name</label>
                                                    <div class="col-md-5 ">
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtShadeName" runat="server" placeholder="Shade Name" CssClass="form-control"
                                                                    ValidationGroup="Save" TabIndex="745" MaxLength="200" ReadOnly="true"></asp:TextBox>
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
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Process Steps</label>
                                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtProcessStep" TabIndex="11"
                                                                    runat="server" placeholder="Steps No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtProcessStep"
                                                                    ValidChars="0123456789" runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Process</label>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlProcess" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="12" AutoPostBack="True" MsgObrigatorio="Process Name" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Code</label>
                                                        <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemCode" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="13" AutoPostBack="True" MsgObrigatorio="Item Code" Visible="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <div class="col-md-4">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItemName" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="14" MsgObrigatorio="Item Name" AutoPostBack="True" Visible="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <!--/span-->
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Unit</label>
                                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="15" AutoPostBack="True" MsgObrigatorio="Unit" Visible="True">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lblUnit" Visible="false" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Quantity(In KG)</label>
                                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox AutoPostBack="true" CssClass="form-control text-right" ID="txtQtyInKG"
                                                                    Text="0.000" TabIndex="16" MsgObrigatorio="Qty (In KG)" placeholder="Qty (In KG)"
                                                                    runat="server" OnTextChanged="txtQtyInKG_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtQtyInKG"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtRate" Text="0.00" TabIndex="17"
                                                                    runat="server" ReadOnly="true" MsgObrigatorio="Rate" placeholder="Rate"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            Quantity (In LTR)</label>
                                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtQtyinltr" Text="0.00" TabIndex="18"
                                                                    runat="server" placeholder="Rate" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtQtyinltr"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtQtyInKG" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
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
                                                            Density</label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass=" form-control text-right" ID="txtDensity" Text="0.00" TabIndex="19"
                                                                    runat="server" placeholder="Density" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtDensity"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgMainShade" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label label-sm">
                                                            &nbsp</label><br />
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnInsert" CssClass="btn blue" TabIndex="20" runat="server" OnClick="btnInsert_Click"
                                                                    OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-Load"> </i>  Insert</asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12" style="overflow-x: auto;">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel26">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgMainShade" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            AutoGenerateColumns="False" CellPadding="4" TabIndex="21" ShowHeaderWhenEmpty="true"
                                                            OnRowCommand="dgMainShade_RowCommand" OnRowDeleting="dgMainShade_Deleting">
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
                                                                <asp:TemplateField HeaderText="Process Code" SortExpression="Process_code" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProcessCode" runat="server" Text='<%# Bind("Process_code") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Steps" SortExpression="ProcessSteps" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProSteps" CssClass=" Control-label" runat="server" Width="50px"
                                                                            Text='<%#Eval("ProcessSteps") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Process Name" SortExpression="Process Name" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProcessName" runat="server" Text='<%# Bind("Process_name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCode" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCodeNo">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemCodeNo" runat="server" Text='<%# Bind("ItemCodeNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="ITEM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemName" runat="server" Width="180px" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="I_UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnitCode" runat="server" Width="50px" Text='<%# Eval("I_UOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="Unit" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Width="50px" Text='<%# Eval("Unit") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Wght.(In KG)" SortExpression="QtyinKG" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQtyInKg" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("QtyinKG")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty.(In LTR)" SortExpression="QtyInLTR" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQtyInLTR" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%# Eval("QtyInLTR")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRate" CssClass=" Control-labelt" runat="server" Width="100px" Text='<%# Eval("Rate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" CssClass=" Control-labelt" runat="server" Width="100px"
                                                                            Text='<%# Eval("Amount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Density" SortExpression="Density" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDensity" CssClass=" Control-label" runat="server" Width="100px"
                                                                            Text='<%#Eval("Density") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
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
                                        <br />
                                    </div>
                                </div>
                                <br />
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Total(In Kg).
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtTotalInKg" placeholder="0.00"
                                                                TabIndex="22" runat="server" MsgObrigatorio="Total (In Kg)" OnTextChanged="txtTotalInKG_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtTotalInKg"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Total(In Ltr)
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtToatlLtr" placeholder="0.00"
                                                                TabIndex="23" runat="server" MaxLength="50" MsgObrigatorio="Density"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtToatlLtr"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Density
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtAvgDensity" placeholder="0.00"
                                                                TabIndex="24" runat="server" MaxLength="50" MsgObrigatorio="Density"></asp:TextBox>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtVolumeSolids" placeholder="0.00"
                                                                TabIndex="25" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtAvgDensity"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="26" runat="server"
                                            OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="27" runat="server"
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

        <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js
    before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

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

        <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
        </script>

        <!-- END JAVASCRIPTS -->

        <script type="text/javascript">
            function VerificaCamposObrigatorios() {
                try {
                    if (VerificaValorCombo('#<%=ddlFormulaType.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }

                    else if (VerificaValorCombo('#<%=ddlColorini.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaObrigatorio('#<%=txtFormulaCode.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaObrigatorio('#<%=txtFormulaName.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaObrigatorio('#<%=txtRemark.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaObrigatorio('#<%=txtProcessStep.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaValorCombo('#<%=ddlProcess.ClientID%>', '#Avisos') == false) {
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
                    else if (VerificaObrigatorio('#<%=txtQtyInKG.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                    else if (VerificaObrigatorio('#<%=txtRate.ClientID%>', '#Avisos') == false) {
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
