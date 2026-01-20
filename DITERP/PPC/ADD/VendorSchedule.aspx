<%@ Page Title="Vendor Schedule" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="VendorSchedule.aspx.cs" Inherits="Transaction_VendorSchedule" %>

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
        function onCalendarShown() {
            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlVendor.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (charCode == 46 && el.value.indexOf(".") !== -1) {
                return false;
            }
            if (el.value.indexOf(".") !== -1) {
                var range = document.selection.createRange();
                if (range.text != "") {
                }
                else {
                    var number = el.value.split('.');
                    if (number.length == 2 && number[1].length > 1)
                        return false;
                }
            }
            return true;
        }
    </script>

    <%--<script language="JavaScript">        var message = 'Right Click is disabled';
        function clickIE() { if (event.button == 2) { alert(message); return false; } }
        function clickNS(e) {
            if (document.layers || (document.getElementById && !document.all)) {
                if (e.which == 2 || e.which == 3) { alert(message); return false; }
            }
        }
        if (document.layers) { document.captureEvents(Event.MOUSEDOWN); document.onmousedown = clickNS; }
        else if (document.all && !document.getElementById) { document.onmousedown = clickIE; }
        document.oncontextmenu = new Function('alert(message);return false') </script>--%>

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
                                <i class="fa fa-reorder"></i>Vendor Schedule
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
                                            <label class="col-md-4 control-label text-right ">
                                                <font color="red">*</font> Month
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        AutoPostBack="true" ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Entry Date"
                                                        ReadOnly="false" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="txtMonth_CalendarExtender" runat="server" Enabled="True"
                                                        Format="MMM yyyy" BehaviorID="calendar1" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"
                                                        TargetControlID="txtMonth" PopupButtonID="txtMonth">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Vendor
                                            </label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlVendor" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Owner"
                                                    TabIndex="1" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                <span class="required">*</span> Part Name & No.
                                            </label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" runat="server"
                                                    MsgObrigatorio="Owner" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Casting To be Offloaded
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtCasting" placeholder="Week 1" Enabled="false"
                                                            TabIndex="2" Text="0" ValidChars="0123456789." runat="server" MaxLength="14"
                                                            MsgObrigatorio="Week 1" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Tentative Week 1
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtWeek1" placeholder="Tentative Week 1"
                                                    Enabled="true" TabIndex="2" Text="0" ValidChars="0123456789." runat="server"
                                                    MaxLength="14" MsgObrigatorio="Tentative Week 1" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Tentative Week 2
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtWeek2" placeholder="Tentative Week 2"
                                                    Enabled="true" ValidChars="0123456789." TabIndex="2" Text="0" runat="server"
                                                    MaxLength="14" MsgObrigatorio="Tentative Week 2" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Tentative Week 3
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtWeek3" placeholder="Tentative Week 3"
                                                    Enabled="true" ValidChars="0123456789." TabIndex="2" Text="0" runat="server"
                                                    MaxLength="14" MsgObrigatorio="Tentative Week 3" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                                Tentative Week 4
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtWeek4" placeholder="Tentative Week 4"
                                                    Enabled="true" ValidChars="0123456789." TabIndex="2" Text="0" runat="server"
                                                    MaxLength="14" MsgObrigatorio="Tentative Week 4" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="6" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>Save </asp:LinkButton>
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

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
                if (VerificaObrigatorio('#<%=ddlItemName.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; }

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
