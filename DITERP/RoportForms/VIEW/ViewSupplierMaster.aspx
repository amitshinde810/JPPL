<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewSupplierMaster.aspx.cs"
    Inherits="RoportForms_VIEW_ViewSupplierMaster" Title="Approved Supplier Master" %>

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
            jQuery("#<%=ddlSupplier.ClientID %>").select2();
            jQuery("#<%=ddlCategory.ClientID %>").select2();
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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-2">
                </div>
                <div id="MSG" class="col-md-8">
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
                                <i class="fa fa-reorder"></i>Approved Supplier Master
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
                                                <label class="col-md-4 control-label text-right">
                                                    Supplier Name
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplier" runat="server" placeholder="Select Supplier" AutoPostBack="True"
                                                                CssClass="select2" Width="100%" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="3" OnCheckedChanged="chkToolNo_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    Supplier Category
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCategory" runat="server" placeholder="Select Category" AutoPostBack="True"
                                                                CssClass="select2" Width="100%" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkCategory" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkCategory" runat="server" CssClass="checker" Text="&nbspAll"
                                                                AutoPostBack="True" TabIndex="3" OnCheckedChanged="chkCategory_CheckedChanged" />
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
                                    <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="15" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                        OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    <asp:LinkButton ID="btnExport" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClientClick="return VerificaCamposObrigatorios();" OnClick="btnShow1_Click"><i class="fa fa-check-square"> </i>  Export </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1s00" runat="server">
                        <ContentTemplate>
                            <div style="width: 100%; overflow: auto">
                                <asp:GridView ID="dgBreakdown" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="P_CODE"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                    OnRowCommand="dgBreakdown_RowCommand" OnPageIndexChanging="dgBreakdown_PageIndexChanging"
                                    PageSize="15">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <div class="clearfix">
                                                    <div class="btn-group">
                                                        <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                            Select <i class="fa fa-angle-down"></i>
                                                        </button>
                                                        <ul class="dropdown-menu" role="menu">
                                                            <li>
                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                    Text="Modify" CommandArgument='<%# Bind("P_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                    CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" Visible="false"
                                                                    CommandName="Print" Text="Print" CommandArgument='<%# Bind("P_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P_CODE" SortExpression="P_CODE" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("P_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Code" SortExpression="P_VEND_CODE" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_VEND_CODE" runat="server" Text='<%# Eval("P_VEND_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Party Name" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contact Person" SortExpression="P_CONTACT" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_CONTACT" runat="server" Text='<%# Eval("P_CONTACT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No" SortExpression="P_MOB" Visible="true" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_MOB" CssClass=" Control-label pull-right" runat="server" Text='<%# Eval("P_MOB") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GST No" Visible="true" SortExpression="P_LBT_NO" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_LBT_NO" CssClass=" Control-label" runat="server" Text='<%# Eval("P_LBT_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address" SortExpression="P_ADD1" Visible="true" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_ADD1" runat="server" Text='<%# Eval("P_ADD1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PAN NO" SortExpression="P_PAN" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_PAN" runat="server" Text='<%# Eval("P_PAN") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City" SortExpression="P_CITY" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_CITY" runat="server" Text='<%# Eval("P_CITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email" SortExpression="P_EMAIL" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblP_EMAIL" runat="server" Text='<%# Eval("P_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assement From" SortExpression="P_COORDINATOR" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                    CommandName="ViewPDF" Text='<%# Eval("P_COORDINATOR") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supporting Document" SortExpression="P_COORDINATOR"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView1" BorderStyle="None" runat="server" CausesValidation="False"
                                                    CommandName="ViewPDF1" Text='<%# Eval("P_COORDINATOR_EMAIL") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="alt" />
                                    <PagerStyle CssClass="pgr" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <%--  <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                    <%-- FOR VIEW --%>
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
                                                    k
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
    <!-- END PAGE CONTENT-->
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
