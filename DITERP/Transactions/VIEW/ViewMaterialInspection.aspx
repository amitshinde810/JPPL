<%@ Page Title="Material Inspection" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewMaterialInspection.aspx.cs" Inherits="Transactions_VIEW_ViewMaterialInspection" %>

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
            jQuery("#<%=ddlSupplierName.ClientID %>").select2();
            jQuery("#<%=ddlGinNo.ClientID %>").select2();
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
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Material Inspection
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnCancel" CssClass="remove" TabIndex="29" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Party Name
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplierName" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="1">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkSupplierAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkSupplierAll" runat="server" TabIndex="2" CssClass="checker"
                                                                OnCheckedChanged="chkSupplierAll_CheckedChanged" AutoPostBack="True" Text=""
                                                                Checked="true" />
                                                            <label class="control-label label-sm">
                                                                ALL
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    GIN No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlGinNo" CssClass="select2" Width="100%" runat="server" TabIndex="1">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkGinNoAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkGinNoAll" runat="server" TabIndex="2" CssClass="checker" Text=""
                                                                Checked="true" OnCheckedChanged="chkGinNoAll_CheckedChanged" AutoPostBack="True" />
                                                            <label class="control-label label-sm">
                                                                ALL
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Form Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFormDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    TabIndex="3" MsgObrigatorio="Please Form Date"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFormDate_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtFormDate" PopupButtonID="txtFormDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkFormToAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    To Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                    TabIndex="3" MsgObrigatorio="Please To Date"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtToDate" PopupButtonID="txtToDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkFormToAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkFormToAll" runat="server" TabIndex="2" CssClass="checker" Text=""
                                                                OnCheckedChanged="chkFormToAll_CheckedChanged" AutoPostBack="True" Checked="true" />
                                                            <label class="control-label label-sm">
                                                                ALL
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlType" CssClass="select2_category form-control" runat="server"
                                                        TabIndex="1">
                                                        <asp:ListItem Value="0" Text="Pending">Pending</asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Inspected">Inspected</asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Inspected">Hold</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnShow" CssClass="btn green" runat="server" OnClick="btnShow_Click"><i class="fa fa-refresh"></i> Show </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgInspection" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                            CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="IWM_CODE"
                                                            OnPageIndexChanging="dgInspection_PageIndexChanging" OnRowCommand="dgInspection_RowCommand"
                                                            AllowPaging="true" PageSize="15">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="View" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAdd" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Add</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IWM_CODE" SortExpression="IWM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CODE" runat="server" Text='<%# Bind("IWM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Inward NO" SortExpression="IWM_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_NO" CssClass="" runat="server" Text='<%# Eval("IWM_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Date" SortExpression="IWM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_DATE" CssClass="" runat="server" Text='<%# Eval("IWM_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Type" SortExpression="IWM_TYPE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_TYPE" CssClass="" runat="server" Text='<%# Eval("IWM_TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Challan No." SortExpression="IWM_CHALLAN_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CHALLAN_NO" CssClass="" runat="server" Text='<%# Eval("IWM_CHALLAN_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Challan Date" SortExpression="IWM_CHAL_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CHAL_DATE" CssClass="" runat="server" Text='<%# Eval("IWM_CHAL_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Supplier" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblP_NAME" CssClass="" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Type" SortExpression="P_TYPE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblP_TYPE" CssClass="" runat="server" Text='<%# Eval("P_TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Type" SortExpression="P_TYPE" Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblP_TYPE1" CssClass="" runat="server" Text='<%# Eval("TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
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
