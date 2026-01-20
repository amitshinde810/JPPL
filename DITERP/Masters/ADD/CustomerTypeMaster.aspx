<%@ Page Title="Customer Type Master" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="CustomerTypeMaster.aspx.cs" Inherits="Masters_ADD_CustomerTypeMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">

        function oknumber1(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <div id="Avisos">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-2">
                    </div>
                    <div id="MSG" class="col-md-8">
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
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Customer Type Master
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
                                                        <span class="required">*</span> Type Code
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox CssClass="form-control " ID="txtCustomerTypeCode" placeholder="Customer Type Code"
                                                            TabIndex="1" runat="server" MaxLength="30" MsgObrigatorio="Please Enter Customer Type Code"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label text-right">
                                                        <span class="required">*</span> Type Description
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control " ID="txtCustomerTypeDesc" placeholder="Customer Type Desc"
                                                                    TabIndex="1" runat="server" MaxLength="30" MsgObrigatorio="Please Enter Customer Type Desc"
                                                                    OnTextChanged="txtCustomerTypeDesc_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label text-right">
                                                        <span class="required">*</span> Starting Character
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel121" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control " ID="txtSuuplierNoFirstName" placeholder="Starting Character"
                                                                    TabIndex="1" runat="server" MaxLength="30" MsgObrigatorio="Please Enter Suuplier No First Name "></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtCustomerTypeDesc" EventName="TextChanged" />
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
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="3" runat="server" OnClick="btnSubmit_Click"
                                            OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="4" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel32">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                        <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                            OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
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
                                                                    Do you Want to Cancel Record?
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

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
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtCustomerTypeCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaObrigatorio('#<%=txtCustomerTypeDesc.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaObrigatorio('#<%=txtSuuplierNoFirstName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
