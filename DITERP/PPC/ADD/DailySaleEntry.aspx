<%@ Page Title="Daily Sales Plan Entry" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="DailySaleEntry.aspx.cs" Inherits="Transactions_DailySaleEntry"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .test tr input
        {
            border: 1px solid red;
            margin-right: 10px;
            padding-right: 10px;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {

            jQuery("#<%=ddlGroup.ClientID %>").select2();

        });
    </script>

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
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
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
            jQuery("#<%=ddlPartName.ClientID %>").select2();
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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
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
                                <i class="fa fa-reorder"></i>Daily Sales Plan Entry
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <asp:Panel ID="MainPanel" runat="server">
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label text-right">
                                                    <span class="required">*</span>Month
                                                </label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtMonth" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                            TabIndex="6" AutoPostBack="True" ValidationGroup="Save" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtMonth_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                            Enabled="True" TargetControlID="txtMonth" PopupButtonID="txtMonth" Format="dd MMM yyyy">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlddltxtFalse" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label text-right">
                                                        <span class="required">*</span> Part Name and Number
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlPartName" CssClass="select2" Width="100%" runat="server"
                                                            Visible="false" MsgObrigatorio="Owner" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlPartName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label text-right">
                                                        <span class="required">*</span>Actual Qty(in Core Shop)
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox CssClass="form-control " ID="txtActualQty" placeholder="Actual Qty"
                                                            Visible="false" TabIndex="2" runat="server" onkeypress="return validateFloatKeyPress(this,event);"
                                                            Text="0" TextMode="SingleLine" MaxLength="10" MsgObrigatorio="Box Qty" OnTextChanged="txtActualQty_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="row"  >
                                            <div class="form-group" >
                                                <label class="col-md-4 control-label text-right">
                                                    <span class="required">*</span> Group Name
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlGroup" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Owner"
                                                                TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="horizontal-form">
                                    <div class="form-body">
                                        <!--/row-->
                                        <div class="row" style="overflow: auto; width: 100%">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgDailySaleEntry" runat="server" TabIndex="15" Style="width: 100%;"
                                                            AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            CellPadding="4" GridLines="Both" OnRowDeleting="dgDailySaleEntry_RowDeleting"
                                                            OnSelectedIndexChanged="dgDailySaleEntry_SelectedIndexChanged" OnRowCommand="dgDailySaleEntry_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="I_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PROD_GP_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPROD_GP_CODE" runat="server" Text='<%# Bind("PROD_GP_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Part Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Part Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="12px"
                                                                    HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Qty" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSaleQty" AutoPostBack="true" runat="server" Text='<%# Eval("SaleQty")%>'
                                                                            OnTextChanged="txtSaleQty_TextChanged" MaxLength="10"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtSaleQty"
                                                                            ValidChars="0123456789" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
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
                //                if (VerificaObrigatorio('#<%=ddlPartName.ClientID%>', '#Avisos') == false) {
                //                    $("#Avisos").fadeOut(6000);
                //                    return false;
                //                }
                //                if (VerificaObrigatorio('#<%=txtActualQty.ClientID%>', '#Avisos') == false) {
                //                    $("#Avisos").fadeOut(6000);
                //                    return false;
                //                }
                //                else {
                return true;
                //                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
        //-->
    </script>

</asp:Content>
