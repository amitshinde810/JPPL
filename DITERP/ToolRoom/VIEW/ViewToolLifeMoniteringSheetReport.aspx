<%@ Page Title="Tool Life Monitoring Sheet" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewToolLifeMoniteringSheetReport.aspx.cs" Inherits="ToolRoom_VIEW_ViewToolLifeMoniteringSheetReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlCustomerName.ClientID %>").select2();
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlToolNo.ClientID %>").select2();
            jQuery("#<%=ddlRefType.ClientID %>").select2();

        });
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function onCalendarShown() {
            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarShown2() {
            var cal = $find("calendar2");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarHidden2() {
            var cal = $find("calendar2");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function call1(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar2");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
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
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tool Life Monitoring Sheet
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
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    From :
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFromDate" runat="server" placeholder="MMM yyyy" CssClass="form-control"
                                                                    TabIndex="6" ValidationGroup="Save" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtFromDate" Format="MMM yyyy" BehaviorID="calendar1" PopupButtonID="txtFromDate"
                                                                    OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label runat="server" id="lblTo" class="col-md-2 control-label text-right">
                                                    To :
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtTodate" runat="server" placeholder="MMM yyyy" CssClass="form-control"
                                                                    TabIndex="6" ValidationGroup="Save" AutoPostBack="true" OnTextChanged="txtTodate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtTodate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtToDate" Format="MMM yyyy" BehaviorID="calendar2" PopupButtonID="txtTodate"
                                                                    OnClientHidden="onCalendarHidden2" OnClientShown="onCalendarShown2">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkDateAll" Visible="true" runat="server" CssClass="checker" Text="&nbspAll"
                                                                AutoPostBack="True" TabIndex="8" OnCheckedChanged="chkDateAll_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div1" class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Tool Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" CssClass="select2" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                                AutoPostBack="true" Width="100%" runat="server" MsgObrigatorio="Tool type" TabIndex="17">
                                                                <asp:ListItem Text="Die" Value="0"></asp:ListItem>
                                                                <%--<asp:ListItem Text="Corebox" Value="1"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllType" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllType" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="10" OnCheckedChanged="chkAllType_CheckedChanged" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomerName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlRefType" CssClass="select2" Width="100%" runat="server"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlRefType_SelectedIndexChanged"
                                                                TabIndex="2">
                                                                <asp:ListItem Text="Refurbish" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Scrap" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkRType" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkRType" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="10" OnCheckedChanged="chkRType_CheckedChanged" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRefType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Customer
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel9">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomerName" OnSelectedIndexChanged="ddlCustomerName_SelectedIndexChanged"
                                                                runat="server" placeholder="Select Customer" AutoPostBack="True" CssClass="select2"
                                                                Width="100%" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllCust" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllCust" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="3" OnCheckedChanged="chkAllCust_CheckedChanged" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomerName" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Tool No.
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlToolNo" runat="server" placeholder="Select Tool No." OnSelectedIndexChanged="ddlToolNo_SelectedIndexChanged" AutoPostBack="True"
                                                                CssClass="select2" Width="100%" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkToolNo" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkToolNo" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="3" OnCheckedChanged="chkToolNo_CheckedChanged" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlToolNo" EventName="" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
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
        <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)
    -->
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
    <!-- BEGIN PAGE LEVEL
    PLUGINS -->

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
