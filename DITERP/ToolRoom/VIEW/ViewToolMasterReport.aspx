<%@ Page Title="Tooling Master Report" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewToolMasterReport.aspx.cs" Inherits="ToolRoom_VIEW_ViewToolMasterReport" %>

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
               
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tooling Master Report
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
                                                            <asp:DropDownList ID="ddlToolNo" runat="server" placeholder="Select Tool No." OnSelectedIndexChanged="ddlToolNo_SelectedIndexChanged"
                                                                AutoPostBack="True" CssClass="select2" Width="100%" TabIndex="2">
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
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="15" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                            OnClick="btnShow1_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                        <asp:LinkButton ID="LinkExport" CssClass="btn green" TabIndex="15" runat="server"
                                            OnClientClick="return VerificaCamposObrigatorios();" OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Export </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto;">
                                                    <asp:UpdatePanel ID="UpdatePanel1s00" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgBreakdown" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="T_CODE"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                                                OnRowCommand="dgBreakdown_RowCommand" OnPageIndexChanging="dgBreakdown_PageIndexChanging"
                                                                PageSize="50">
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
                                                                                                Text="Modify" CommandArgument='<%# Bind("T_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                                        </li>
                                                                                        <li>
                                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                                                CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                                        </li>
                                                                                        <li>
                                                                                            <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" Visible="false"
                                                                                                CommandName="Print" Text="Print" CommandArgument='<%# Bind("T_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                                                    </ul>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="T_CODE" SortExpression="B_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("T_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tool No" SortExpression="T_NAME"  
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_NAME" runat="server" Text='<%# Eval("T_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Part No " SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Part Name" SortExpression="I_NAME" Visible="true" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_NAME"   runat="server"
                                                                                Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tool Type" Visible="true" SortExpression="T_TYPE"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_TYPE" CssClass=" Control-label" runat="server" Text='<%# Eval("T_TYPE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="STD Tool Life" SortExpression="T_STDLIFE" Visible="true"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_STDLIFE" runat="server" Text='<%# Eval("T_STDLIFE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pending tool Life(SHOTS)" SortExpression="T_PENDTOOLLIFE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_PENDTOOLLIFE" runat="server" Text='<%# Eval("T_PENDTOOLLIFE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pending tool Life(Months)" SortExpression="T_PENDTOOLLIFEMONTH" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_PENDTOOLLIFEMONTH" runat="server" Text='<%# Eval("T_PENDTOOLLIFEMONTH") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Owner" SortExpression="T_OWNER" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_OWNER" runat="server" Text='<%# Eval("T_OWNER") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Prev main Freq (SHOTS)" SortExpression="T_PMFREQ" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_PMFREQ" runat="server" Text='<%# Eval("T_PMFREQ") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Latest Rev. No. Of 3D" SortExpression="T_REVNO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_REVNO" runat="server" Text='<%# Eval("T_REVNO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tool Image" SortExpression="T_PHOTO_PATH" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <%--<asp:Label ID="lblB_FILE" runat="server" Text='<%# Eval("B_FILE") %>'></asp:Label>--%>
                                                                            <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                                CommandName="ViewPDF" Text='<%# Eval("T_PHOTO_PATH") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="3D Image" Visible="true" SortExpression="T_3D_PATH"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <%--<asp:Label ID="lblB_FILE" runat="server" Text='<%# Eval("B_FILE") %>'></asp:Label>--%>
                                                                            <asp:LinkButton ID="lnkView1" BorderStyle="None" runat="server" CausesValidation="False"
                                                                                CommandName="ViewPDF1" Text='<%# Eval("T_3D_PATH") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rev Date" SortExpression="T_REV_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblT_REV_DATE" runat="server" Text='<%# Eval("T_REV_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <%--  <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />--%>
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
