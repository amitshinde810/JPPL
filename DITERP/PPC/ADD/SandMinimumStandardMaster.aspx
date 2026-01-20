<%@ Page Title="Raw Material and Sand Minimum Standard Master" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="SandMinimumStandardMaster.aspx.cs" Inherits="Masters_SandMinimumStandardMaster" %>

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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sand Minimum Standard Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> AC4B (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtAC4B" placeholder="AC4B" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="AC4B"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtAC4B"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                                <%--FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom"--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> LM25 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtLM25" placeholder="LM25" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="LM25"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtLM25"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> SAND (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtSAND" placeholder="SAND" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="SAND"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtSAND"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        
                                         <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> LM - 24 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtLM24" placeholder="LM - 24" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="LM - 24"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtLM24"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                                <%--FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom"--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> AC4CH (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtAC4CH" placeholder="AC4CH" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="AC4CH"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtAC4CH"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> ADC 12 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtADC12" placeholder="ADC 12" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="ADC 12"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtADC12"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        
                                         <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> KS 1275 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtKS1275" placeholder="KS 1275" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="KS 1275"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtKS1275"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                                <%--FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom"--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> LM - 26 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtLM26" placeholder="LM - 26" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="LM - 26"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtLM26"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> LM - 28 (Tons)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtLM28" placeholder="LM - 28" TabIndex="2"
                                                    runat="server" TextMode="SingleLine" MaxLength="100" MsgObrigatorio="LM - 28"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtLM28"
                                                    ValidChars=" " FilterType="Numbers" runat="server" />
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="6" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="7" OnClick="btnCancel_Click"
                                        runat="server"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!-- END PAGE CONTENT-->
        </div>
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
                if (VerificaObrigatorio('#<%=txtLM25.ClientID%>', '#Avisos') == false) {
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
        //-->
    </script>

</asp:Content>
