<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProductionToStore.aspx.cs"
    Inherits="Transactions_ADD_ProductionToStore" Title="Production to Store" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlSubComponentName.ClientID %>").select2();
            jQuery("#<%=ddlSubComponentCode.ClientID %>").select2();
            jQuery("#<%=txtPersonName.ClientID %>").select2();
            jQuery("#<%=ddlCategory.ClientID %>").select2();


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
                                <i class="fa fa-reorder"></i>Production to Store
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
                                                    <label class="col-md-4 control-label label-sm">
                                                        Select Plant
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbtWithAmt" runat="server" AutoPostBack="True" TabIndex="2"
                                                                    RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtWithAmt_SelectedIndexChanged">
                                                                    <asp:ListItem Value="-2147483648" Selected="True">&nbsp; Plant 1&nbsp;&nbsp;</asp:ListItem>
                                                                    <asp:ListItem Value="-2147483634">&nbsp;Plant 2</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label">
                                                    </label>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class=" col-md-2 control-label text-right ">
                                                        Production No.</label>
                                                    <div class="col-md-3 ">
                                                        <asp:TextBox ID="txtGinNo" runat="server" placeholder="Production Number" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="1" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Date</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtGinDate" runat="server" CssClass="form-control" placeholder="dd/MMM/yyyy"
                                                                ValidationGroup="Save" TabIndex="2" AutoPostBack="true" OnTextChanged="txtGinDate_TextChanged"
                                                                MsgObrigatorio="Please Select Gin Date"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtGinDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtGinDate" PopupButtonID="txtGinDate">
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
                                                        <font color="red">*</font> Type</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="up1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlType" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="3" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" Visible="True"
                                                                    MsgObrigatorio="Please Select Type" AutoPostBack="true">
                                                                    <asp:ListItem Selected="True" Value="1">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2">Assembly</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class=" col-md-2 control-label text-right ">
                                                    </label>
                                                    <div class="col-md-3 ">
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlBatchNo" CssClass="select2_category form-control" runat="server"
                                                                    Visible="false" TabIndex="4" MsgObrigatorio="Batch Number" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlBatchNo_SelectedIndexChanged">
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
                                                        Fromula Code
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlMatReqNo" CssClass="select2_category form-control" runat="server"
                                                                    MsgObrigatorio="Order No" OnSelectedIndexChanged="ddlMatReqNo_SelectedIndexChanged"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtFormula" runat="server" CssClass="form-control" ValidationGroup="Save"
                                                                    TabIndex="5" ReadOnly="true"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        Customer Name</label>
                                                    <div class="col-md-5">
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="6" MsgObrigatorio="Customer Name" Visible="True">
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
                                                        Produced By</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="txtPersonName" CssClass="select2" Width="450px" runat="server"
                                                                    TabIndex="3" AutoPostBack="true">
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
                                            <div class="col-md-12">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> Item Category</label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCategory" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="6" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" MsgObrigatorio="Item Code"
                                                                    placeholder="Select Item Code" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgvProductionStoreDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Code</label>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSubComponentCode" CssClass="select2" Width="100%" runat="server"
                                                                    MsgObrigatorio="Item Code" TabIndex="8" AutoPostBack="True" OnSelectedIndexChanged="ddlSubComponentCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <font color="red">*</font> Item Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSubComponentName" CssClass="select2" Width="100%" runat="server"
                                                                    MsgObrigatorio="Item Name" TabIndex="9" AutoPostBack="True" OnSelectedIndexChanged="ddlSubComponentName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentCode" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Unit
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtunit" runat="server" CssClass="form-control" ValidationGroup="Save"
                                                                    TabIndex="12" MsgObrigatorio=" " Enabled="false" AutoPostBack="True"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlItembatchno" CssClass="select2_category form-control input-sm"
                                                                    Visible="false" runat="server" MsgObrigatorio="Item Name" TabIndex="9" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlSubComponentName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            </Triggers>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlBatchNo" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span -->
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="10" MsgObrigatorio="Unit" Visible="false" Enabled="true">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlSubComponentName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm ">
                                                            <font color="red">*</font> Quantity</label>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtQty" CssClass="form-control text-right input-sm" MsgObrigatorio="Please Insert Quantity"
                                                                    AutoPostBack="true" placeholder="0.000" MaxLength="20" TabIndex="11" runat="server"
                                                                    OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Remark</label>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" ValidationGroup="Save"
                                                                    TabIndex="12" MsgObrigatorio="Please Enter Name Of Person" AutoPostBack="True"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            &nbsp</label>
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" OnClick="btnInsert_Click"
                                                                    TabIndex="13" runat="server" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="overflow: auto; width: 100%">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgvProductionStoreDetails" runat="server" TabIndex="14" Style="width: 100%;"
                                                            AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            CellPadding="4" GridLines="Both" OnRowDeleting="dgvProductionStoreDetails_RowDeleting"
                                                            OnSelectedIndexChanged="dgvProductionStoreDetails_SelectedIndexChanged" OnRowCommand="dgvProductionStoreDetails_RowCommand">
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
                                                                            CommandName="Delete" Text="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ItemCode" SortExpression="SubComponentCode" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPSD_I_CODE" runat="server" Text='<%# Bind("PSD_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="Sub Component Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="Sub Component Name" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="UOM" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("UOM") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit Code" SortExpression="UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUOMCODE" runat="server" Text='<%# Eval("UOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch Code" SortExpression="Batch Code" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBT_CODE" runat="server" Text='<%# Bind("BT_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch No" SortExpression="Batch No" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBT_NO" runat="server" Text='<%# Eval("BT_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPSD_QTY" runat="server" Text='<%# Eval("PSD_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remark" SortExpression="Remark" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPSD_REMARK" runat="server" Text='<%# Eval("PSD_REMARK") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rbtWithAmt" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:Panel ID="Panel1" runat="server">
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="15" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
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
                if (VerificaObrigatorio('#<%=txtGinDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlSubComponentCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlSubComponentName.ClientID%>', '#Avisos') == false) {
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
