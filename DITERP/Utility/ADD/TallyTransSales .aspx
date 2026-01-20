<%@ Page Title="Tally Transfer Sales" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="TallyTransSales .aspx.cs" Inherits="Utility_TallyTransSales_" %>

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
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
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
                                <i class="fa fa-reorder"></i>Tally Transfer Sales
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Doc no.
                                            </label>
                                            <div class="col-md-1">
                                                <div class="input-group">
                                                    <asp:TextBox CssClass="form-control" ID="txtDocNo" TabIndex="1" runat="server" Enabled="false"
                                                        TextMode="SingleLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-1 control-label text-right ">
                                                Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel17">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtDocDate" TabIndex="2"
                                                                runat="server" TextMode="SingleLine"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Invoice Type
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel19">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlInvType" CssClass="select2_category form-control input-sm"
                                                            runat="server" AutoPostBack="true" MsgObrigatorio="Invoice Type " TabIndex="3"
                                                            OnSelectedIndexChanged="ddlInvType_OnSelectedIndexChanged">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                            <asp:ListItem Value="1" Selected="True">Tax Invoice</asp:ListItem>
                                                            <asp:ListItem Value="2">Labour Charge</asp:ListItem>
                                                            <asp:ListItem Value="3">Labour Charge PCPL</asp:ListItem>
                                                            <asp:ListItem Value="4">Suppliementory Invoice  </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Invoice No. From
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlFromInvNo" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Invoice No" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkAllInv" EventName="CheckedChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlInvType" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-1 control-label text-right ">
                                                To
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlToInvNo" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Invoice No" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkAllInv" EventName="CheckedChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlInvType" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel9">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllInv" runat="server" CssClass="checker" Text="&nbsp;All" AutoPostBack="True"
                                                            TabIndex="6" OnCheckedChanged="chkAllInv_CheckedChanged" />
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
                                                Inv. From Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtInvFromDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="7" ValidationGroup="Save"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtInvFromDate" PopupButtonID="txtInvFromDate"
                                                                Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-1 control-label text-right ">
                                                To Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtInvToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="7" ValidationGroup="Save"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                Enabled="True" TargetControlID="txtInvToDate" PopupButtonID="txtInvToDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkDateAll" runat="server" CssClass="checker" Text="&nbsp;All"
                                                            AutoPostBack="True" OnCheckedChanged="chkDateAll_CheckedChanged" TabIndex="8" />
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
                                                Customer Name
                                            </label>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel14">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="True" CssClass="select2_category form-control"
                                                            TabIndex="10">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkAllCustomer" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel15">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllCustomer" runat="server" CssClass="checker" Text="&nbsp;All"
                                                            AutoPostBack="True" TabIndex="11" OnCheckedChanged="chkAllCustomer_CheckedChanged" />
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
                                                Status
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel10">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlStatus" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Invoice Type " TabIndex="3">
                                                            <asp:ListItem Value="0" Selected="True">Pending</asp:ListItem>
                                                            <asp:ListItem Value="1">Transfered</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkAllStatus" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllStatus" runat="server" CssClass="checker" Text="&nbsp;All"
                                                            AutoPostBack="True" TabIndex="13" OnCheckedChanged="chkAllStatus_CheckedChanged" />
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
                                                Operation
                                            </label>
                                            <div class="col-md-6">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel16">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtOperation" runat="server" AutoPostBack="True" TabIndex="12"
                                                            RepeatDirection="Horizontal" CssClass="checker" CellPadding="10">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp;New Entry</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp;Alter Entry</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnLoad" CssClass="btn green" TabIndex="15" runat="server" OnClick="btnLoad_Click"><i class="fa fa-check-square"> </i>  Load </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="horizontal-form">
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
                                                                            <asp:CheckBox ID="chkSelect" Checked="true" runat="server" CssClass="checker" EnableViewState="true"
                                                                                AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%-- here we insert database field for bind and eval--%>
                                                                    <asp:TemplateField HeaderText="IWM_CODE" SortExpression="INM_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWM_CODE" runat="server" Text='<%# Bind("INM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice No" SortExpression="INM_NO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWM_NO" runat="server" Text='<%# Eval("INM_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice Date" SortExpression="INM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIWM_DATE" runat="server" Text='<%# Eval("INM_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="CustomerCode" Visible="false" SortExpression="Customer Name"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblP_CODE" runat="server" Text='<%# Eval("P_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customer Name" SortExpression="Customer Name" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Net Amount" SortExpression="INM_NET_AMT" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_NET_AMT" runat="server" Text='<%# Eval("INM_NET_AMT") %>' CssClass="Control-label pull-right"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" SortExpression="INM_IS_TALLY_TRANS" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINM_IS_TALLY_TRANS" runat="server" Text='<%# Eval("INM_IS_TALLY_TRANS") %>'
                                                                                CssClass="Control-label pull-right"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
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
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnExport" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="btnExport_Click"><i class="fa fa-check-square"> </i>  Generate </asp:LinkButton>
                                    <asp:LinkButton ID="btnDownload" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="lb_DownloadXML_Click"><i class="fa fa-check-square"> </i>  Download </asp:LinkButton>
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
