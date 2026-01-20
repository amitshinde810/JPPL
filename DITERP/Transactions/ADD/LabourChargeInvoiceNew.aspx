<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="LabourChargeInvoiceNew.aspx.cs"
    Inherits="Transactions_ADD_LabourChargeInvoiceNew" Title="Labour Charge Invoice" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {

            jQuery("#<%=ddlFromInvNo.ClientID %>").select2();
            jQuery("#<%=ddlToInvNo.ClientID %>").select2();
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
        });
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
                <div id="MSG"  class="col-md-12">
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Labour Invoice
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" runat="server" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                <div id="pnl" runat="server">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                From Date
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="6" ValidationGroup="Save" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right ">
                                                To Date
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="7" ValidationGroup="Save" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                DC No. From
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlFromInvNo" CssClass="select2" runat="server" Width="100%"
                                                            MsgObrigatorio="Invoice No" AutoPostBack="true" OnSelectedIndexChanged="ddlInvNo_SelectedIndexChanged" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-1 control-label text-right ">
                                                To
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlToInvNo" CssClass="select2" runat="server" Width="100%"
                                                            MsgObrigatorio="Invoice No"  AutoPostBack="true" OnSelectedIndexChanged="ddlInvNo_SelectedIndexChanged"  TabIndex="5">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Customer Name
                                            </label>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel14">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" CssClass="select2"
                                                            Width="100%" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-6">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnLoad" CssClass="btn green" TabIndex="15" runat="server" OnClick="btnLoad_Click"><i class="fa fa-check-square"> </i>  Load </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="control-group">
                                                <%--Grid View--%>
                                                <div class="col-md-12" style="overflow: auto; width: 100%">
                                                    <asp:UpdatePanel ID="UpdatePanel7895" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgInvDetails" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                ForeColor="#333333" GridLines="None" DataKeyNames="INM_CODE" Font-Names="Verdana"
                                                                Font-Size="12px" ShowFooter="false" PageSize="6" CssClass="table table-striped table-bordered table-advance table-hover">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect"  runat="server" CssClass="checker" OnCheckedChanged="chkSelect_CheckedChanged" EnableViewState="true"
                                                                                AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%-- here we insert database field for bind and eval--%>
                                                                    <asp:TemplateField HeaderText="INM_CODE" SortExpression="INM_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_CODE" runat="server" Text='<%# Bind("INM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice No" SortExpression="INM_NO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_NO" runat="server" Text='<%# Eval("INM_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice Date" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_DATE" runat="server" Text='<%# Eval("INM_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customer" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="INM_P_CODE" Visible="false" SortExpression="INM_DATE"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_P_CODE" runat="server" Text='<%# Eval("INM_P_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="I_CODE" Visible="false" SortExpression="INM_DATE"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODE" runat="server" Text='<%# Eval("I_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Codeno" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Quantity" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_INQTY" runat="server" Text='<%# Eval("IND_INQTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_RATE" runat="server" Text='<%# Eval("IND_RATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_AMT" runat="server" Text='<%# Eval("IND_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Central Tax" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_EX_AMT" runat="server" Text='<%# Eval("IND_EX_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="State Tax" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_E_CESS_AMT" runat="server" Text='<%# Eval("IND_E_CESS_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Integrated Tax" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_SH_CESS_AMT" runat="server" Text='<%# Eval("IND_SH_CESS_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Grand Total" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC No" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblE_TARIFF_NO" runat="server" Text='<%# Eval("E_TARIFF_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="E_CODE" Visible="false" SortExpression="INM_DATE"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblE_CODE" runat="server" Text='<%# Eval("E_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <%-- End Grid View--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Basic Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" onkeypress="return validateFloatKeyPress(this,event);"
                                                                ID="txtBasicAmount" Text="0.00" runat="server" ReadOnly="true" TabIndex="7"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtBasicAmount"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    Taxable Amt
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" onkeypress="return validateFloatKeyPress(this,event);"
                                                                ID="txtTaxableAmt" Text="0.00" TabIndex="14" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtTaxableAmt"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    CGST Amt
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtExciseAmount" Text="0.00"
                                                                TabIndex="11" runat="server" onkeypress="return validateFloatKeyPress(this,event);"
                                                                ></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtExciseAmount"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    SGST Amt
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" onkeypress="return validateFloatKeyPress(this,event);"
                                                                ID="txtEduCessAmt" Text="0.00" TabIndex="12" runat="server" ></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtEduCessAmt"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    IGST Amt
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" onkeypress="return validateFloatKeyPress(this,event);"
                                                                ID="txtSHEduCessAmt" Text="0.00" TabIndex="13" runat="server" ReadOnly="true"
                                                                ></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtSHEduCessAmt"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    <span class="required"></span>Grand Total
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtGrandTotal" Text="0.00" TabIndex="24"
                                                                runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtGrandTotal"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnExport" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="btnSave_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT-->
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script>
<![endif]-->
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
