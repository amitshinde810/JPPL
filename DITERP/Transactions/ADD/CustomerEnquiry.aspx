<%@ Page Title="Shade Development Requisition" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="CustomerEnquiry.aspx.cs" Inherits="Transactions_ADD_CustomerEnquiry" %>

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

    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
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
                                <i class="fa fa-reorder"></i>Shade Development Requisition
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
                                            <label class="col-md-2 control-label text-right">
                                                <span class="required">*</span> Project Type
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtProjectType" placeholder="Project Type"
                                                    TabIndex="1" runat="server" MaxLength="50" MsgObrigatorio="Project Type"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Project No
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtProjectNo" placeholder="Project No"
                                                    TabIndex="2" runat="server" MaxLength="30" ReadOnly="true" MsgObrigatorio="Project No"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Gloss
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtGloss" placeholder="Gloss" TabIndex="3"
                                                    runat="server" MaxLength="30" MsgObrigatorio="Gloss"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Shade Name
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtShadeName" placeholder="Shade Name"
                                                    TabIndex="4" runat="server" MaxLength="100" MsgObrigatorio="Shade Name"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                <span class="required">*</span> Customer Name
                                            </label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                    TabIndex="6" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                    Visible="True">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Shade Code
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtShadeCode" placeholder="Shade Code"
                                                    TabIndex="6" runat="server" MaxLength="100" MsgObrigatorio="Shade Code"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Shade No
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtShadeNo" placeholder="Shade No" TabIndex="7"
                                                    runat="server" MaxLength="100" MsgObrigatorio="Shade No"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                <span class="required">*</span> Request Date
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        ValidationGroup="Save" TabIndex="8" MsgObrigatorio="Request Date" ReadOnly="false"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="txtRequestDate_CalendarExtender" runat="server" Enabled="True"
                                                        Format="dd MMM yyyy" TargetControlID="txtRequestDate" PopupButtonID="txtRequestDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Requested By
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtRequestedBy" placeholder="Requested By"
                                                    TabIndex="9" runat="server" MaxLength="100" MsgObrigatorio="Requested By"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Desired Due Date
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDesiredDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        ValidationGroup="Save" TabIndex="10" MsgObrigatorio="Desired Date" ReadOnly="false"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                        TargetControlID="txtDesiredDate" PopupButtonID="txtDesiredDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                End Use
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtEnduse" placeholder="End Use" TabIndex="11"
                                                    runat="server" MaxLength="100" MsgObrigatorio="End Use"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Substrate
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtSubstrate" placeholder="Substrate" TabIndex="12"
                                                    runat="server" MaxLength="100" MsgObrigatorio="Substrate"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Primer & DFT
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtPrimerDFT" placeholder="Primer & DFT"
                                                    TabIndex="13" runat="server" MaxLength="100" MsgObrigatorio="Primer & DFT"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Volume Solids
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtVolume" placeholder="Volume Solids"
                                                    TabIndex="14" runat="server" MaxLength="100" MsgObrigatorio="Volume Solids"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                DFT
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtDFT" placeholder="DFT" TabIndex="15"
                                                    runat="server" MaxLength="100" MsgObrigatorio="DFT"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                PMT
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtPMT" placeholder="PMT" TabIndex="16"
                                                    runat="server" MaxLength="100" MsgObrigatorio="PMT"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Properties
                                            </label>
                                            <div class="col-md-8">
                                                <asp:TextBox CssClass="form-control " ID="txtProperties" placeholder="Properties as per Customer Sepecification"
                                                    TabIndex="17" runat="server" MaxLength="100" MsgObrigatorio="Properties"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Notes
                                            </label>
                                            <div class="col-md-8">
                                                <asp:TextBox CssClass="form-control " ID="txtNote" placeholder="Note" TabIndex="18"
                                                    runat="server" MaxLength="100" MsgObrigatorio="Note" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Completion Date
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCompletionDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        ValidationGroup="Save" TabIndex="19" MsgObrigatorio="Completion Date" ReadOnly="false"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                        TargetControlID="txtCompletionDate" PopupButtonID="txtCompletionDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Bendtest
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox CssClass="form-control " ID="txtBendtest" placeholder="Bendtest" TabIndex="20"
                                                    runat="server" MsgObrigatorio="Bendtest"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="21" runat="server"
                                        OnClick="btnSubmit_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="22" OnClick="btnCancel_Click"
                                        runat="server"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
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

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {

                if (VerificaObrigatorio('#<%=txtProjectType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;

                }

                if (VerificaObrigatorio('#<%=txtRequestDate.ClientID%>', '#Avisos') == false) {
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
