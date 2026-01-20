<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Test.aspx.cs"
    Inherits="Admin_Add_Test" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <!-- END SIDEBAR -->
    <!-- BEGIN CONTENT -->

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
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <%-- <div class="row">
                <div class="col-md-12">
                    <div class="col-md-12 col-sm-8">
                        <ul class="pull-right breadcrumb">
                            <li><a id="A7" href="../Default.aspx" runat="server">Admin</a></li>
                            <li class="active"><a id="A8" href="UserMaster.aspx" runat="server"></a>User Master</li>
                        </ul>
                    </div>
                </div>
            </div>--%>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
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
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>User Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <!-- Page Body -->
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> User Name</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="User Name" MsgObrigatorio="User name" CssClass="form-control"
                                                                ID="txtusername" MaxLength="50" runat="server" TabIndex="1"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> User ID</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="User ID" MsgObrigatorio="User ID" CssClass="form-control"
                                                        ID="txtUserID" MaxLength="50" runat="server" TabIndex="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> User Type</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="3" ID="ddlUserlevel" MsgObrigatorio="User Type"
                                                                CssClass="select2_category form-control">
                                                                <asp:ListItem Value="0">Select Type</asp:ListItem>
                                                                <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                                                                <asp:ListItem Value="Manager">Manager</asp:ListItem>
                                                                <asp:ListItem Value="Accountant">Accountant</asp:ListItem>
                                                                <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    E-Mail ID</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Email ID" EnableViewState="false" CssClass="form-control"
                                                        ID="txtUserEmail" runat="server" TabIndex="4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> Password</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Password" MsgObrigatorio="Password" CssClass="form-control"
                                                        ID="txtpass" MaxLength="50" runat="server" TabIndex="5" TextMode="Password" EnableViewState="false"
                                                        AutoCompleteType="Disabled"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> Re-Enter Password</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Re-Password" MsgObrigatorio="Re-Enter Password" EnableViewState="false"
                                                        CssClass="form-control" MaxLength="50" ID="txtRepass" runat="server" TabIndex="6"
                                                        TextMode="Password"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkActive" runat="server" CssClass="checker" Text="Active" Checked="true"
                                                                TabIndex="7" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkIsAdmin" runat="server" CssClass="checker" Text="IsAdmin" TabIndex="8" />
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
                                                    Store Name
                                                </label>
                                                <div class="col-md-6">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBoxList ID="chkItems" CssClass="checker" runat="server" RepeatDirection="Vertical">
                                                            </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                        <Triggers>
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
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="9" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="10" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                </div>
                            </div>
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
                                                    <div class="col-md-12">
                                                        <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                                        <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbtType" runat="server" TabIndex="1" RepeatDirection="Vertical"
                                                                    CssClass="checker" CellPadding="15">
                                                                    <asp:ListItem Value="0" Selected="True">Export Invoice</asp:ListItem>
                                                                    <asp:ListItem Value="1">Export Invoice - Domestic Print</asp:ListItem>
                                                                    <asp:ListItem Value="2">ARE 1 Invoice</asp:ListItem>
                                                                    <asp:ListItem Value="3">Packaging List</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                   </ContentTemplate>
                                                                    </asp:UpdatePanel>--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <script src="../../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

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

    <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->
    </div>
</asp:Content>
