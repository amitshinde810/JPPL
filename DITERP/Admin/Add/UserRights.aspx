<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UserRights.aspx.cs"
    Inherits="Admin_Add_UserRights" Title="User Rights" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlUserName.ClientID %>").select2();
            jQuery("#<%=ddlanUser.ClientID %>").select2();
            jQuery("#<%=ddlFormName.ClientID %>").select2();
            jQuery("#<%=ddlModuleName.ClientID %>").select2();
        });
    </script>

    <!-- BEGIN CONTENT -->
    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>User Rights
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnClose_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <!-- Page Body -->
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> User Name</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="2" MsgObrigatorio="User Name" ID="ddlUserName"
                                                                AutoPostBack="True" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged">
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
                                                <label class="col-md-4 control-label">
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkCustCopy" runat="server" Text="&nbsp;Copy Rights from Another User"
                                                                CssClass="checker" AutoPostBack="True" Font-Bold="True" TabIndex="3" OnCheckedChanged="ChkCustCopy_CheckedChanged"
                                                                Checked="False" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> From User Name</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="4" ID="ddlanUser" AutoPostBack="True"
                                                                CssClass="select2" Width="100%">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ChkCustCopy" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Module Name</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="5" MsgObrigatorio="Form Name" ID="ddlModuleName"
                                                                AutoPostBack="True" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged1">
                                                                <asp:ListItem Value="0"> Select</asp:ListItem>
                                                                <asp:ListItem Value="1"> Administrator</asp:ListItem>
                                                                <asp:ListItem Value="2">Master</asp:ListItem>
                                                                <asp:ListItem Value="3">Purchase</asp:ListItem>
                                                                <asp:ListItem Value="4">Store</asp:ListItem>
                                                                <asp:ListItem Value="5">Sales</asp:ListItem>
                                                                <asp:ListItem Value="6">Utility</asp:ListItem>
                                                                <asp:ListItem Value="7">Excise</asp:ListItem>
                                                                <asp:ListItem Value="8">IRN</asp:ListItem>
                                                                <asp:ListItem Value="9">Dashboard</asp:ListItem>
                                                                <asp:ListItem Value="10">COPP</asp:ListItem>
                                                                <asp:ListItem Value="11">Tools</asp:ListItem>
                                                                <asp:ListItem Value="12">PPC</asp:ListItem>
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
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Form Name</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="5" MsgObrigatorio="Form Name" ID="ddlFormName"
                                                                AutoPostBack="True" CssClass="select2" Width="100%">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="6" OnCheckedChanged="ChkAll_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:LinkButton ID="btnShow" CssClass="btn blue" TabIndex="15" runat="server" OnClick="btnShow_Click"
                                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-up"> </i> Show</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkMenu" runat="server" CssClass="checker" Text="&nbsp;Menu" TabIndex="7"
                                                                AutoPostBack="True" OnCheckedChanged="chkMenu_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAdd" runat="server" CssClass="checker" Text="&nbsp;Add" TabIndex="8"
                                                                AutoPostBack="True" OnCheckedChanged="chkAdd_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkView" runat="server" CssClass="checker" Text="&nbsp;View" TabIndex="9"
                                                                AutoPostBack="True" OnCheckedChanged="chkView_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkUpdate" runat="server" CssClass="checker" Text="&nbsp;Update"
                                                                TabIndex="10" AutoPostBack="True" OnCheckedChanged="chkUpdate_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkDelete" runat="server" CssClass="checker" Text="&nbsp;Delete"
                                                                TabIndex="11" AutoPostBack="True" OnCheckedChanged="chkDelete_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkPrint" runat="server" CssClass="checker" Text="&nbsp;Print"
                                                                TabIndex="12" AutoPostBack="True" OnCheckedChanged="chkPrint_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkBackDate" runat="server" CssClass="checker" Text="&nbsp;BackDate"
                                                                TabIndex="13" AutoPostBack="True" OnCheckedChanged="chkBackDate_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="checker" Text="&nbsp;All"
                                                                AutoPostBack="True" TabIndex="14" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgUserRights" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                GridLines="Both" DataKeyNames="USR_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                                ShowFooter="false" OnSelectedIndexChanged="dgUserRights_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="USR_CODE" SortExpression="USR_CODE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="USR_CODE" runat="server" Text='<%# Bind("USR_CODE") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SM_CODE" SortExpression="USR_CODE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="SM_CODE" runat="server" Text='<%# Bind("SM_CODE") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" SortExpression="Form Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Name" runat="server" Text='<%# Eval("Name") %>' Width="200px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Menu" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkMenuDg" runat="server" CssClass="checker" EnableViewState="true"
                                                                        AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkMenu" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkViewDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkView" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkUpdateDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkUpdate" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkAddDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkAdd" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkDeleteDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkDelete" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Print" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkPrintDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkPrint" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Back Date" SortExpression="">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkBackDateDg" runat="server" CssClass="checker" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkBackDate" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="alt" />
                                                <PagerStyle CssClass="pgr" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                                <ContentTemplate>
                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                        PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                                        <div class="portlet box blue">
                                            <div class="portlet-title">
                                                <div class="captionPopup">
                                                    Alert
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="form-horizontal">
                                                    <div class="form-body">
                                                        <div class="row">
                                                            <label class="col-md-12 control-label">
                                                                Do you want to cancel record ?
                                                            </label>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-offset-3 col-md-9">
                                                                <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                    OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                                <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                    OnClick="btnCancel1_Click"> No</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="form-actions fluid">
                                        <div class="col-md-offset-4 col-md-9">
                                            <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="16" runat="server"
                                                OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="17" runat="server"
                                                OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--/row-->
                </div>
            </div>
        </div>
        <!-- End Page Body -->
    </div>
    <!-- END PAGE CONTENT-->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

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

    <script src="../../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
