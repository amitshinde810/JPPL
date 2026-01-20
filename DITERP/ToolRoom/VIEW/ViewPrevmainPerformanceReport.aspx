<%@ Page Title="Preventive Maintenance Performance Report" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewPrevmainPerformanceReport.aspx.cs" Inherits="RoportForms_VIEW_ViewPrevmainPerformanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
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
        jQuery("#<%=ddlTool.ClientID %>").select2();
        jQuery("#<%=ddlPartNo.ClientID %>").select2();  
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
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Preventive Maintenance Performance Report
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="4" runat="server" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                      <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Month
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="MMM yyyy" CssClass="form-control"
                                                                TabIndex="6" ValidationGroup="Save" AutoPostBack="true" MsgObrigatorio="From Date"
                                                                OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="MMM yyyy"
                                                                OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                <%--To Date--%>
                                            </label>
                                            <div id="Div1" class="col-md-3" runat="server" visible="false">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="7" ValidationGroup="Save" AutoPostBack="true" MsgObrigatorio="To Date"
                                                                OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="MMM yyyy"
                                                                OnClientHidden="onCalendarHidden2" OnClientShown="onCalendarShown2">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="Div2" class="col-md-2" runat="server" visible="false">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkDateAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                            TabIndex="8" OnCheckedChanged="chkDateAll_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Tool No. &  Part No. 
                                            </label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlTool" runat="server" OnSelectedIndexChanged="ddlTool_SelectedIndexChanged" CssClass="select2" Width="100%" MsgObrigatorio="Item"
                                                            TabIndex="9" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllItem" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                            TabIndex="10" OnCheckedChanged="chkAllItem_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Part No.
                                            </label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlPartNo" runat="server" CssClass="select2" Width="100%" MsgObrigatorio="Item"
                                                            TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="ddlPartNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllpart" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                            TabIndex="10" OnCheckedChanged="chkAllpart_CheckedChanged" />
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

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
