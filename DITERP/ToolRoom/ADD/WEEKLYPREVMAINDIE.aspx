<%@ Page Title="Weekly Preventive maintenance" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="WEEKLYPREVMAINDIE.aspx.cs" Inherits="ToolRoom_ADD_WEEKLYPREVMAINDIE" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlToolNo.ClientID %>").select2();
            jQuery("#<%=ddlWeekly.ClientID %>").select2();
            jQuery("#<%=ddlReason.ClientID %>").select2();
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
        function Showalert1() {
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
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
                                <i class="fa fa-reorder"></i>Weekly Preventive Maintenance
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
                                                    <span class="required">* </span>Slip No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtslipNo" Enabled="false" MsgObrigatorio="Slip No."
                                                                placeholder="Slip No." TabIndex="3" AutoPostBack="true" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Month
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtMonth" MsgObrigatorio="Month"
                                                                    placeholder="Month" TabIndex="3" AutoPostBack="true" OnTextChanged="txtMonth_TextChanged"
                                                                    runat="server"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                    Enabled="True" TargetControlID="txtMonth" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"
                                                                    PopupButtonID="txtFromDate" Format="MMM yyyy">
                                                                </cc1:CalendarExtender>
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
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Week
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlWeekly" CssClass="select2" Width="100%" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlWeek1_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Tool No.</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UPToolNo">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlToolNo" AutoPostBack="true" CssClass="select2"
                                                                TabIndex="1" MsgObrigatorio="Tool No." Width="100%">
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
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Tooling Handover date</label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtReceivedDate" MsgObrigatorio="Tooling Handover date"
                                                            placeholder="Tooling Handover date" TabIndex="3" runat="server" OnTextChanged="txtReceivedDate_TextChanged"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtReceivedDate"
                                                            Format="dd MMM yyyy">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Expected Completion date</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" ID="txtECompDate" MsgObrigatorio="Expected  Completion  date"
                                                                    placeholder="Expected  Completion  date" AutoPostBack="true" TabIndex="3" runat="server"
                                                                    OnTextChanged="txtECompDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtECompDate"
                                                                    Format="dd MMM yyyy">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" id="CDate" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Completed date
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control input-sm" AutoPostBack="true" ID="txtcompetedDate"
                                                                    MsgObrigatorio="Completed date" placeholder="Completed date" TabIndex="3" OnTextChanged="txtcompetedDate_TextChanged"
                                                                    runat="server"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtcompetedDate"
                                                                    Format="dd MMM yyyy">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <label class="col-md-2 control-label" runat="server" visible="false" id="lblReason">
                                                            Reason
                                                        </label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" Visible="false" ID="ddlReason" CssClass="select2"
                                                                Width="100%">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="rep" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Upload REPORT</label>
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
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="34" runat="server"
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

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js
    before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
    -->

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
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <%--<script type="text/javascript"> function VerificaCamposObrigatorios()
    { try
     {  if (VerificaValorCombo('#<%=ddlToolNo.ClientID%>','#Avisos') == false)
    {  $("#Avisos").fadeOut(6000);  return false;  } 
     if (VerificaObrigatorio('#<%=txtMonth.ClientID%>','#Avisos') == false)
      {  $("#Avisos").fadeOut(6000);  return false; // }  else
    { return true; } } catch (err) { alert('Erro in Required Fields: ' + err.description);
    return false; } } </script>--%>
    <%--<!-- END JAVASCRIPTS -->--%>
    <!-- END PAGE LEVEL
    SCRIPTS -->
</asp:Content>
