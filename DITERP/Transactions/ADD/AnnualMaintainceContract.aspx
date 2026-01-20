<%@ Page Title="Annual Maintenance Contract" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="AnnualMaintainceContract.aspx.cs" Inherits="Transactions_ADD_AnnualMaintainceContract" %>

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
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
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
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Annual Maintenance Contract
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab_1_1" data-toggle="tab">AMC Details</a></li>
                                <li class=""><a href="#tab_2_2" data-toggle="tab">Term And Condition</a> </li>
                            </ul>
                            <div class="tab-content">
                                <!-- T1 Info Tab -->
                                <div class="tab-pane fade active in" id="tab_1_1">
                                    <!-- End Tabs Setting -->
                                    <!-- Start Tabs-->
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <!--/row-->
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label class="col-md-2 control-label text-right">
                                                                AMC No.
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" ID="txtAMCNo" TabIndex="1" runat="server" Enabled="false"
                                                                        TextMode="SingleLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <label class="col-md-2 control-label text-right">
                                                                <span class="required">*</span> PO Date
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtAMCDate" TabIndex="2"
                                                                        runat="server" MsgObrigatorio="Po Date " TextMode="SingleLine"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                                        Format="dd MMM yyyy" TargetControlID="txtAMCDate">
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
                                                                <span class="required">*</span> Supplier</label>
                                                            <div class="col-md-5">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="ddlSupplier" CssClass="select2_category form-control input-sm"
                                                                            runat="server" MsgObrigatorio="Please Select Supplier " TabIndex="3">
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <label class="col-md-2 control-label text-right">
                                                                <span class="required">*</span> Reference</label>
                                                            <div class="col-md-3">
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtReferance" placeholder="Referance"
                                                                            TabIndex="4" runat="server" MsgObrigatorio="Product Name" TextMode="SingleLine">
                                                                        </asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label class="col-md-2 control-label text-right">
                                                                <span class="required">*</span> From Date
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtfromDate" TabIndex="5"
                                                                        runat="server" MsgObrigatorio="From Date " TextMode="SingleLine" AutoPostBack="true"
                                                                        OnTextChanged="txtfromDate_TextChanged"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                        TargetControlID="txtfromDate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </div>
                                                            <label class="col-md-2 control-label text-right">
                                                                <span class="required">*</span> To Date
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtToDate" TabIndex="6"
                                                                        runat="server" MsgObrigatorio="To Date " TextMode="SingleLine" AutoPostBack="true"
                                                                        OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                        TargetControlID="txtToDate">
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
                                                                <span class="required">*</span> Contact Person</label>
                                                            <div class="col-md-3">
                                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtContactPerson" placeholder="Contact Person"
                                                                            TabIndex="7" runat="server" MsgObrigatorio="Contact Person" TextMode="SingleLine">
                                                                        </asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <label class="col-md-2 control-label text-right">
                                                                <span class="required">*</span> Phone Number</label>
                                                            <div class="col-md-3">
                                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtPhoneNo" placeholder="Phone No"
                                                                            TabIndex="8" runat="server" MsgObrigatorio="Phone No" TextMode="SingleLine">
                                                                        </asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <hr />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="horizontal-form">
                                        <div class="form-body">
                                            <!--Product Name-->
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> Product Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtProductName" placeholder="Product Name"
                                                                    TabIndex="9" runat="server" TextMode="MultiLine" Rows="2">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Product Desccription-->
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Product Description</label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtProducDesc" placeholder="Product Description"
                                                                    TabIndex="10" runat="server" TextMode="MultiLine" Rows="2">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Preventive Maintenance-->
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Preventive Maintenance</label>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtPreventiveDesc" placeholder="Preventive Maintenance"
                                                                    TabIndex="11" runat="server" TextMode="MultiLine" Rows="4">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Qty Rate Amount-->
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <!--Qty-->
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Qty</label>
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtQty" placeholder="0.000"
                                                                    TabIndex="12" runat="server" AutoPostBack="true" OnTextChanged="txtQty_TextChanged">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--Rate-->
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" placeholder="0.00"
                                                                    TabIndex="13" runat="server" AutoPostBack="true" OnTextChanged="txtRate_TextChanged">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="14" AutoPostBack="True" MsgObrigatorio="Unit" Visible="True">
                                                            </asp:DropDownList>
                                                            <asp:Label runat="server" ID="lblUnit" Visible="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--Amount-->
                                                <div class="col-md-3">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Amount</label>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAmount" placeholder="0.00"
                                                                TabIndex="15" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--Attachment-->
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                        </label>
                                                        <br />
                                                        <asp:LinkButton ID="btnAddDoc" CssClass="btn blue" TabIndex="16" runat="server" OnClick="btnAddDoc_Click"
                                                            Visible="True"><i class="fa fa-plus"></i> Add Document</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--Attachment Grid-->
                                            <div class="row">
                                                <div class="col-md-1"">
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:GridView ID="dgAttachment" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                        CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="Download"
                                                        AllowPaging="true" PageSize="15" OnRowUpdating="dgAttachment_RowUpdating" OnRowDeleting="dgAttachment_Deleting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Browse">
                                                                <ItemTemplate>
                                                                    <asp:FileUpload ID="imgUpload" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfilename" CssClass="" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnupload" runat="server" CommandName="Update" Text="Upload"
                                                                        CssClass="btn green btn-xs" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Download" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload" BorderStyle="None" runat="server" CssClass="btn blue btn-xs"
                                                                        CausesValidation="False" CommandName="Download" Text="Download" CommandArgument='<%# Bind("Download") %>'
                                                                        OnClick="lnkDownload_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                        CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                        CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                        Text="Delete" OnClick="lnkDelete_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <!--Insert-->
                                            <div class="row">
                                                <div class="col-md-9"">
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                        </label>
                                                        <br />
                                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnInsert" CssClass="btn green" runat="server" Text="Insert" OnClick="btnInsert_Click"
                                                                    TabIndex="17" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- Grid --%>
                                            <div class="row" style="overflow: auto; width: 100%">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgAMCDetails" runat="server" TabIndex="18" Style="width: 100%;"
                                                                AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                CellPadding="4" GridLines="Both" OnRowCommand="dgAMCDetails_RowCommand" OnRowDeleting="dgAMCDetails_RowDeleting">
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
                                                                                CausesValidation="False" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                Text="Delete"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="WOD_PROD_NAME">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_PROD_NAME" runat="server" Text='<%# Bind("WOD_PROD_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product Description" SortExpression="WOD_PROD_DESC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_PROD_DESC" runat="server" Text='<%# Bind("WOD_PROD_DESC") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Preventive Maintenance" SortExpression="WOD_PREV_MAINTAIN_DEC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_PREV_MAINTAIN_DEC" runat="server" Text='<%# Bind("WOD_PREV_MAINTAIN_DEC") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qty" SortExpression="WOD_QTY">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_QTY" runat="server" Text='<%# Bind("WOD_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate" SortExpression="WOD_RATE">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_RATE" runat="server" Text='<%# Bind("WOD_RATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit Code" SortExpression="WOD_UOM_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_UOM_CODE" runat="server" Text='<%# Bind("WOD_UOM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit Name" SortExpression="WOD_UOM_NAME">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_UOM_NAME" runat="server" Text='<%# Bind("WOD_UOM_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" SortExpression="WOD_TOT_AMT">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_TOT_AMT" runat="server" Text='<%# Bind("WOD_TOT_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id" SortExpression="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWOD_CODE" runat="server" Text='<%# Bind("WOD_CODE") %>'></asp:Label>
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
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:Panel ID="Panel1" runat="server">
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                            <%--End Comment--%>
                                            <%-- Start Comment --%>
                                            <div class="row">
                                                <div class="col-md-8">
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Total Amount
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtFinalTotalAmount"
                                                                    placeholder="" TabIndex="19" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgAMCDetails" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--End Comment--%>
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <!--/row-->
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label class="col-md-2 control-label text-right">
                                                                Service Tax %
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" ID="txtServiceTax" TabIndex="20" runat="server"
                                                                        TextMode="SingleLine" placeholder="0.00"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtServiceTax"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </div>
                                                            </div>
                                                            <label class="col-md-2 control-label text-right">
                                                                Tax Applicable
                                                            </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="Tax Applicable" ID="txtTaxApplicable"
                                                                        TabIndex="21" runat="server" TextMode="SingleLine" Text="As Applicable"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="tab-pane fade " id="tab_2_2">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <!--row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Delivery Schedule
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtDeliverySchedule" placeholder=" Delivery Schedule"
                                                                TabIndex="22" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Transpoter
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtTranspoter" placeholder=" Transpoter"
                                                                TabIndex="23" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Payment Term
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtPaymentTerm" placeholder="Payment Term"
                                                                TabIndex="24" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Delivery To
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtDeliveryTo" placeholder="Delivery To"
                                                                TabIndex="25" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Freight Terms
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:TextBox CssClass="form-control" ID="txtFreightTermsg" placeholder="Freight Terms"
                                                                TabIndex="26" runat="server" TextMode="SingleLine">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Gurantee/Waranty
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtGuranteeWaranty" placeholder="Gurantee/Waranty"
                                                                TabIndex="27" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Note
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNote" placeholder="Note" TabIndex="28"
                                                                runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="29" runat="server"
                                        OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="30" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
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
                                                        OnClick="btnCancel_Click"> No</asp:LinkButton>
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
    <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
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

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    type="text/javascript"></script>

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
                if (VerificaObrigatorio('#<%=txtAMCDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtfromDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtToDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtContactPerson.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtPhoneNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtReferance.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }

                else if (VerificaValorCombo('#<%=ddlSupplier.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlUnit.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Error in Required Fields: ' + err.description);
                return false;
            }
        }
        //-->
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
    </label> </label>
</asp:Content>
