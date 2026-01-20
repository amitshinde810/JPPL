<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddAreaMaster.aspx.cs"
    Inherits="Masters_ADD_AddAreaMaster" Title="Area Master" %>

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

        function oknumber1(sender, e) {
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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div id="Avisos">
                    </div>
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
                                <i class="fa fa-reorder"></i>Area Master
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
                                        <div class="form-group">
                                            <div class="col-md-2">
                                            </div>
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span>Area Code
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtAreaCode" placeholder="Area Code" TabIndex="1"
                                                    runat="server" MaxLength="30" MsgObrigatorio="Please Enter Area Code"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                            </div>
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Area Description
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtAreaDescription" placeholder="Area Description"
                                                    TabIndex="1" runat="server" MaxLength="30" MsgObrigatorio="Please Enter Area Description"></asp:TextBox>
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

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtAreaCode.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaObrigatorio('#<%=txtAreaDescription.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
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
