<%@ Page Title="Tools Pending for Preventive Maintenance" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewPMFailuerReport.aspx.cs" Inherits="ToolRoom_VIEW_ViewPMFailuerReport" %>

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
                                <i class="fa fa-reorder"></i>Tools Pending for Preventive Maintenance
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div id="Div1" class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    Month
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
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnShow" Visible="false" CssClass="btn green" TabIndex="16" runat="server"
                                                                OnClick="btnShow_Click"><i class="fa fa-check-square"> </i> Show</asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive">
                                                <div style="overflow: auto;">
                                                    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgPMPending" runat="server" CellPadding="4" Font-Size="12px" ShowFooter="false"
                                                                Font-Names="Verdana" GridLines="None" DataKeyNames="PM_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="dgPMPending_RowDataBound"
                                                                OnRowCommand="dgPMPending_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="PM_CODE" SortExpression="PM_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("PM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="TOOL NO." SortExpression="T_NAME" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_NAME" runat="server" Text='<%# Eval("T_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PART NO." SortExpression="I_CODENO" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PART NAME" SortExpression="I_NAME" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="MONTH" SortExpression="PM_MONTH" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPM_MONTH" runat="server" Text='<%# Eval("PM_MONTH") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="YEAR" SortExpression="PM_YEAR" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPM_YEAR" runat="server" Text='<%# Eval("PM_YEAR") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="REASON" SortExpression="PM_REASON_CODE" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="ddlReason" CssClass="form-control select2" AppendDataBoundItems="true"
                                                                                        Width="150px" runat="server" AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="REASON" Visible="false" SortExpression="PM_REASON_CODE">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPM_REASON_CODE" runat="server" Text='<%# Eval("PM_REASON_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="STATUS" SortExpression="PM_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton Enabled="true" ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("PM_STATUS"))=="1")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                                BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                                Text='<%#  Eval("PM_STATUS").ToString()== "1" ? "Update" : "Update" %>' CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel39">
                                <ContentTemplate>
                                    <asp:LinkButton ID="LnkDoc" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopDocument" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                        PopupControlID="PopDocument" TargetControlID="LnkDoc">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="PopDocument" runat="server" Style="display: none;">
                                        <div class="portlet box blue">
                                            <div class="portlet-title">
                                                <div class="captionPopup">
                                                    Document View
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="form-horizontal">
                                                    <div class="form-body">
                                                        <div class="row">
                                                            <label class="col-md-12 control-label">
                                                            </label>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <iframe runat="server" id="IframeViewPDF" width="900px" height="600px"></iframe>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-offset-5 col-md-9">
                                                                <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server"> Ok</asp:LinkButton>
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
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page
load time) -->
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

    <script type="text/javascript">
        $("[src*=plus]").live("click", function() {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../assets/img/minus.gif");
        });
        $("[src*=minus]").live("click", function() {
            $(this).attr("src", "../../assets/img/plus.gif");
            $(this).closest("tr").next().remove();
        });
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
