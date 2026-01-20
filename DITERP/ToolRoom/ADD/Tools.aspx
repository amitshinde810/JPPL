<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Tools.aspx.cs"
    Inherits="ToolRoom_ADD_Tools" Title="Tooling Master" %>

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
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <link href="../../assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link href="../../assets/plugins/bootstrap-datetimepicker/css/datetimepicker.css"
        rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
            jQuery("#<%=ddlpart.ClientID %>").select2();
            jQuery("#<%=ddlOwner.ClientID %>").select2();
        });
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
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
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
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tooling Master
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
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Tool No.</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txttoolno" TabIndex="1" MsgObrigatorio="Tool No."
                                                        placeholder="Tool No." runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Tool type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Tool type"
                                                                TabIndex="2">
                                                                <asp:ListItem Text="Die" Value="0"></asp:ListItem>
                                                                <%--<asp:ListItem Text="Corebox" Value="1"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Customer
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomer" CssClass="select2" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                Width="100%" runat="server" MsgObrigatorio="select Customer" TabIndex="3">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Part No. & Name
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlpart" CssClass="select2" Width="100%" OnSelectedIndexChanged="ddlpart_SelectedIndexChanged"
                                                                AutoPostBack="true" runat="server" MsgObrigatorio="Part No. & Name" TabIndex="4">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Tooling Photo
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:FileUpload ID="FileUpload2" TabIndex="5" ClientIDMode="Static" onchange="this.form.submit()"
                                                        runat="server" />
                                                    <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" Style="display: none" />
                                                    <asp:LinkButton ID="lnkupload" runat="server" Text="" OnClick="lnkupload_Click"> </asp:LinkButton>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Standard Tools Life
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtStdToolLife" placeholder=" Standard Tools Life"
                                                        TabIndex="6" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtStdToolLife"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Pending Tools Life(Shots)
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPendingShots" placeholder=" Pending Tools Life(Shots)"
                                                        TabIndex="7" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtPendingShots"
                                                        ValidChars="0123456789.-" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Pending Tools Life(month)
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPendingMonth" placeholder=" Pending Tools Life(month)"
                                                        TabIndex="8" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtPendingMonth"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Tool owner
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlOwner" runat="server" AutoPostBack="true" CssClass="select2"
                                                                Width="100%" TabIndex="9" MsgObrigatorio="select Tool Owner">
                                                                <asp:ListItem Text="PCPL" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Preventive Maint Frequance (SHOTS)
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPMfreq" placeholder="Preventive Maint Frequance"
                                                        TabIndex="10" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtPMfreq"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Drawing Revision No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtRevNo" placeholder="Revision No."
                                                        TabIndex="11" runat="server" MsgObrigatorio="Revision No."></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtRevNo"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    3D Model
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:FileUpload ID="FileUpload1" TabIndex="12" ClientIDMode="Static" onchange="this.form.submit()"
                                                        runat="server" />
                                                    <asp:Button ID="Button2" Text="Upload" runat="server" OnClick="Upload2" Style="display: none" />
                                                    <asp:LinkButton ID="lnkTModel" runat="server" Text="" OnClick="lnkuploadTModel_Click"> </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 text-right label-sm">
                                                    <span class="required"></span>Status
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkStatus" runat="server" Checked="true" Text="&nbspActive" CssClass="checker"
                                                        TabIndex="13" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    3D model Rev. date
                                                </label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtRevDate" placeholder="Rev. Date"
                                                            TabIndex="14" runat="server"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtRevDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Cavity
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtcavity" placeholder="Cavity"
                                                        TabIndex="15" runat="server" MsgObrigatorio="Cavity"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtcavity"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                                <label class="col-md-4 control-label label-sm">
                                                    Ref. No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" TextMode="MultiLine" ID="txtRefNo"
                                                        placeholder="Ref. No." TabIndex="16" runat="server" Rows="2" Columns="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="33" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="18" runat="server"
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
                                                            OnClick="btnOk_Click"> Yes </asp:LinkButton>
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
                    <asp:UpdatePanel runat="server" ID="UpdatePanel39">
                        <ContentTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupExtenderDovView" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                PopupControlID="PanelDoc" TargetControlID="LinkButton1">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="PanelDoc" runat="server" Style="display: none;">
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
                                                        <iframe runat="server" id="myframe" width="900px" height="600px"></iframe>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-5 col-md-9">
                                                        <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)
    -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix
    bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL
    PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
