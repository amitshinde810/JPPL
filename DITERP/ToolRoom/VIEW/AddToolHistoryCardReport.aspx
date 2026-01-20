<%@ Page Title="Tool History Card Report" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="AddToolHistoryCardReport.aspx.cs" Inherits="ToolRoom_ADD_AddToolHistoryCardReport" %>

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
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlToolNo.ClientID %>").select2();

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
                                <i class="fa fa-reorder"></i>Tool History Card
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
                                                <label class="col-md-2 control-label text-right">
                                                    Type
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
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlToolNo" runat="server" placeholder="Select Tool No." AutoPostBack="True"
                                                                CssClass="select2" Width="100%" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnShow" Visible="true" CssClass="btn green" TabIndex="16" runat="server"
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
                                                            <asp:GridView ID="dgComponent" runat="server" CellPadding="4" Font-Size="12px" ShowFooter="false"
                                                                Font-Names="Verdana" GridLines="None" DataKeyNames="B_T_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="dgComponent_RowDataBound"
                                                                OnRowCommand="dgComponent_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <img alt="" style="cursor: pointer" src="../../assets/img/plus.gif" />
                                                                            <asp:Panel ID="pnlCustomerDetails" runat="server" Style="display: none">
                                                                                <asp:GridView ID="dgSubComponent" runat="server" CellPadding="4" Font-Size="12px"
                                                                                    ShowFooter="false" Font-Names="Verdana" GridLines="None" DataKeyNames="B_T_CODE"
                                                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false"
                                                                                    AutoGenerateColumns="false" OnRowCommand="dgSubComponent_RowCommand" OnRowDataBound="dgSubComponent_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="B_T_CODE" SortExpression="B_T_CODE" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblB_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("B_T_CODE") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DATE" SortExpression="B_DATE" Visible="true">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblB_DATE" runat="server" Text='<%# Eval("B_DATE") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="TYPE OF MAINTENANCE (PM/BD/IP)" SortExpression="B_TYPE"
                                                                                            Visible="true">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblB_TYPE" runat="server" Text='<%# Eval("B_TYPE") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="RECORD OF MAINTENANCE" SortExpression="B_FILE" Visible="true">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                                                    CommandName="ViewPDF" Text='<%# Eval("B_FILE") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="TYPE OF MAINTENANCE" Visible="false" SortExpression="B_CODE">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblBCode" runat="server" Text='<%# Eval("B_CODE") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="B_T_CODE" SortExpression="B_T_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblB_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("B_T_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="TOOL HISTORY CARD" HeaderStyle-Font-Size="Larger"
                                                                        HeaderStyle-Font-Bold="true">
                                                                        <ItemTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="form-group">
                                                                                        <label class="col-md-2 bold label-sm">
                                                                                            TOOL NO.</label>
                                                                                        <div class="col-md-3 text-left">
                                                                                            <asp:Label ID="lblT_NAME" runat="server" Text='<%# Eval("T_NAME") %>'></asp:Label>
                                                                                        </div>
                                                                                        <label class="col-md-3 bold label-sm">
                                                                                            TYPE OF Tool</label>
                                                                                        <div class="col-md-3 text-left">
                                                                                            <asp:Label ID="lblT_TYPE" runat="server" Text='<%# Eval("T_TYPE") %>'></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="form-group">
                                                                                        <label class="col-md-2 bold label-sm">
                                                                                            PART NAME</label>
                                                                                        <div class="col-md-6 text-left">
                                                                                            <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="form-group">
                                                                                        <label class="col-md-2 bold label-sm">
                                                                                            PART NO.</label>
                                                                                        <div class="col-md-6 text-left">
                                                                                            <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
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
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnExport" CssClass="btn green" TabIndex="16" runat="server"
                                        OnClick="btnExport_Click"><i class="fa fa-refresh"></i> Export</asp:LinkButton>
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
