<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewInspectionRegisterReport.aspx.cs"
    Inherits="RoportForms_VIEW_ViewInspectionRegisterReport" Title="Inspection Register" %>

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
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlSupplierName.ClientID %>").select2();
        });
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
                <div class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Inspection Register
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="CncLnk" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-3 control-label text-right">
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtType" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">&nbsp;Inspected&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="1">&nbsp;Pending</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                </label>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker">
                                                                <asp:ListItem Value="0" Selected="True">&nbsp;Date wise&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                                <asp:ListItem Value="1">&nbsp;Item wise&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                                <asp:ListItem Value="2">&nbsp;Supplier wise</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-3 control-label text-right">
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtReject" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker">
                                                                <asp:ListItem Value="0">&nbsp;All&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="1">&nbsp;Rejected</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    From Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFromDate" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true"
                                                                    runat="server" placeholder="dd MMM yyyy" CssClass="form-control" MsgObrigatorio="Please Select From Date"
                                                                    TabIndex="6" ValidationGroup="Save"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                    Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="dd MMM yyyy">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    To Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                    MsgObrigatorio="Please Select To Date" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"
                                                                    TabIndex="7" ValidationGroup="Save"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                    Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="dd MMM yyyy">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkDateAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="8" OnCheckedChanged="chkDateAll_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Item Code
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" runat="server" Width="100%" AutoPostBack="True"
                                                                CssClass="select2" MsgObrigatorio="Please Select Code" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged"
                                                                TabIndex="11">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllItemName" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllItemName" runat="server" CssClass="checker" Text="&nbspAll"
                                                                AutoPostBack="True" TabIndex="12" OnCheckedChanged="chkAllItemName_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Item Name
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" runat="server" Width="100%" AutoPostBack="True"
                                                                CssClass="select2" MsgObrigatorio="Please Select Item" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged"
                                                                TabIndex="11">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllItemName" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Supplier Name
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel10">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplierName" runat="server" Width="100%" AutoPostBack="True"
                                                                CssClass="select2" MsgObrigatorio="Please Select Supplier" TabIndex="11">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllSupplierName" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllSupplierName" runat="server" CssClass="checker" Text="&nbspAll"
                                                                AutoPostBack="True" TabIndex="12" OnCheckedChanged="chkAllSupplierName_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    PDI No.
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlPDINo" runat="server" AutoPostBack="True" CssClass="select2_category form-control"
                                                                MsgObrigatorio="Please Select PDI No." TabIndex="11">
                                                                <asp:ListItem Value="0" Text="PDI No Pending"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="PDI No Complete"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="All"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="15" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                            OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
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
    </div>
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this
    will reduce page load time) -->
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
    <!-- BEGIN PAGE LEVEL  PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
