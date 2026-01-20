<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Breakdown.aspx.cs"
    Inherits="ToolRoom_ADD_Breakdown" Title="BreakDown Entry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <!-- END SIDEBAR -->
    <!-- BEGIN CONTENT -->
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

    <script type="text/javascript">
        function Showalert1() {
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlTool.ClientID %>").select2();
            jQuery("#<%=ddlPart.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function dateselect(ev) {
            var calendarBehavior1 = $find("Calendar1");
            var d = calendarBehavior1._selectedDate;
            var now = new Date();
            calendarBehavior1.get_element().value = d.format("dd/MMM/yyyy") + " " + now.format("HH:mm")
        }
    </script>

    <script type="text/javascript">
        function dateselect1(ev) {
            var calendarBehavior1 = $find("Calendar2");
            var d = calendarBehavior1._selectedDate;
            var now = new Date();
            calendarBehavior1.get_element().value = d.format("dd/MMM/yyyy") + " " + now.format("HH:mm")
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
            <div class="row">
                <div class="col-md-2">
                </div>
                <div id="MSG" class="col-md-8">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Breakdown Entry
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
                                                    <span class="required">*</span> BREAKDOWN NO.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="BREAKDOWN NO. " MsgObrigatorio="BREAKDOWN NO. " CssClass="form-control"
                                                                ID="txtBNo" MaxLength="50" Enabled="false" runat="server" TabIndex="1"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div1" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> BREAKDOWN SLIP NO.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="BREAKDOWN SLIP NO. " MsgObrigatorio="BREAKDOWN SLIP NO. "
                                                                CssClass="form-control" ID="txtSlipNo" MaxLength="50" runat="server" TabIndex="1"></asp:TextBox>
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
                                                    <span class="required">*</span> TYPE OF TOOL
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" runat="server" Width="100%" AutoPostBack="true" CssClass="select2"
                                                                OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">DIE</asp:ListItem>
                                                                <%--<asp:ListItem Value="1">CORE BOX</asp:ListItem>--%>
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
                                                    <span class="required">*</span> TOOL NO.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlTool" runat="server" Width="100%" AutoPostBack="true" CssClass="select2"
                                                                OnSelectedIndexChanged="ddlTool_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
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
                                                    <span class="required">*</span> PART NAME AND NO.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlPart" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPart_SelectedIndexChanged"
                                                                Width="100%" CssClass="select2">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlTool" EventName="SelectedIndexChanged" />
                                                        </Triggers>
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
                                                    <span class="required">*</span> BREAKDOWN DATE TIME
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group" style="width: 100%">
                                                                <%-- <div id="dtopening" class="controls input-append date form_datetime1" data-date-format="dd M yyyy  HH:ii p"
                                                                    data-link-field="dtp_input1">--%>
                                                                <asp:TextBox ReadOnly="false" TabIndex="38" ID="txtBreakDownDate" AutoPostBack="true"
                                                                    OnTextChanged="txtBreakDownDate_TextChanged" Style="width: 80%; float: left"
                                                                    type="text" CssClass="form-control" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup" BehaviorID="Calendar1"
                                                                    runat="server" TargetControlID="txtBreakDownDate" OnClientDateSelectionChanged="dateselect">
                                                                </cc1:CalendarExtender>
                                                                <span class="input-group-addon add-on" style="width: 8%; float: left; padding-top: 8px;
                                                                    padding-bottom: 9px;"><i class="fa fa-calendar"></i></span>
                                                            </div>
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
                                                    <span class="required">*</span> REASON</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="REASON" CssClass="form-control" ID="txtReason" runat="server"
                                                        TextMode="MultiLine" TabIndex="4" Height="50px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="Div2" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    Upload drawing/image for Tool room</label>
                                                <div class="col-md-4">
                                                    <asp:FileUpload ID="FUDrawingTR" ClientIDMode="Static" onchange="this.form.submit()"
                                                        runat="server" />
                                                    <asp:Button ID="btnUpload1" Text="Upload" runat="server" OnClick="Upload1" Style="display: none" />
                                                    <asp:LinkButton ID="lnkFUDrawingTR" runat="server" Text="" OnClick="lnkFUDrawingTR_Click"> </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="act" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> ACTION TAKEN
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="ACTION TAKEN " CssClass="form-control" ID="txtAction" runat="server"
                                                        TextMode="MultiLine" TabIndex="4" Height="50px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="close" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> CLOSURE TIME
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <%--<div id="dtclosure" class="controls input-append date form_datetime1" data-date-format="dd M yyyy  HH:ii p"
                                                                data-link-field="dtp_input1">--%>
                                                            <%--<asp:TextBox placeholder="DD MMM YYYY" TabIndex="38" ID="txtClosureDate" Style="width: 80%;
                                                                float: left" type="text" CssClass="form-control" AutoPostBack="true" runat="server"
                                                                OnTextChanged="txtClosureDate_TextChanged"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" BehaviorID="Calendar1"
                                                                runat="server" TargetControlID="txtClosureDate" OnClientDateSelectionChanged="dateselect">
                                                            </cc1:CalendarExtender>
                                                            <span class="input-group-addon add-on" style="width: 8%; float: left; padding-top: 8px;
                                                                padding-bottom: 9px;"><i class="fa fa-calendar"></i></span>--%>
                                                            <asp:TextBox ReadOnly="false" TabIndex="38" ID="txtClosureDate" AutoPostBack="true"
                                                                OnTextChanged="txtClosureDate_TextChanged" Style="width: 80%; float: left" type="text"
                                                                CssClass="form-control" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup" BehaviorID="Calendar2"
                                                                runat="server" TargetControlID="txtClosureDate" Format="dd/MMM/yyyy" OnClientDateSelectionChanged="dateselect1">
                                                            </cc1:CalendarExtender>
                                                            <span class="input-group-addon add-on" style="width: 8%; float: left; padding-top: 8px;
                                                                padding-bottom: 9px;"><i class="fa fa-calendar"></i></span>
                                                            <%-- </div>--%>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="hrs" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    BREAKDOWN HOURS
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-4">
                                                            <asp:TextBox placeholder="BREAKDOWN HOURS " CssClass="form-control" ID="txtHours"
                                                                runat="server" TabIndex="5" AutoPostBack="true" AutoCompleteType="Disabled"></asp:TextBox>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="rep" runat="server">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">
                                                </label>
                                                <label class="col-md-3 control-label">
                                                    <span class="required">*</span> BREAKDOWN REPORT</label>
                                                <div class="col-md-4">
                                                    <asp:FileUpload ID="FileUpload1" ClientIDMode="Static" onchange="this.form.submit()"
                                                        runat="server" />
                                                    <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" Style="display: none" />
                                                    <asp:LinkButton ID="lnkupload" runat="server" Text="" OnClick="lnkupload_Click"> </asp:LinkButton>
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

    <script src="../../assets/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker1.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <!-- Used For dateand time picker -->
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        $('.form_datetime1').datetimepicker({
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            forceParse: 0,
            showMeridian: 1
        });	
    </script>

</asp:Content>
