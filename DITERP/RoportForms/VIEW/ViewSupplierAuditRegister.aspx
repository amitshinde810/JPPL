<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewSupplierAuditRegister.aspx.cs"
    Inherits="RoportForms_VIEW_ViewSupplierAuditRegister" Title="Supplier Audit Plan Register"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlsupplier.ClientID %>").select2();
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
        function onCalendarShown2() {
            var cal = $find("calendar2");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarHidden2() {
            var cal = $find("calendar2");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function call1(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar2");
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
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                                <i class="fa fa-reorder"></i>Supplier Audit Plan Register
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="4" runat="server" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div id="Div1" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtType" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Selected="True">&nbsp;Die&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                <%--<asp:ListItem Value="1">&nbsp;CoreBox</asp:ListItem>--%>
                                                                <%--<asp:ListItem Value="2" Text="Both">&nbsp; </asp:ListItem> --%>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" id="showgroup" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                </label>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker">
                                                                <asp:ListItem Value="0" Selected="True">&nbsp;Datewise&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="1">&nbsp;Itemwise&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="2">&nbsp;Partywise&nbsp;&nbsp;</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    From Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                    TabIndex="6" ValidationGroup="Save" AutoPostBack="true" MsgObrigatorio="From Date"
                                                                    OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                    Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="MMM yyyy"
                                                                    OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    To Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                    TabIndex="7" ValidationGroup="Save" AutoPostBack="true" MsgObrigatorio="To Date"
                                                                    OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                    Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="MMM yyyy"
                                                                    OnClientHidden="onCalendarHidden2" OnClientShown="onCalendarShown2">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkDateAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="8" OnCheckedChanged="chkDateAll_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Supplier Name
                                                </label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlsupplier" runat="server" CssClass="select2" Width="100%"
                                                                MsgObrigatorio="Item" TabIndex="9" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllItem" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="rbtType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllItem" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="10" OnCheckedChanged="chkAllItem_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-12" style="text-align: center;">
                                    <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="15" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                        OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                    <asp:LinkButton ID="btnExport" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnExport_Click" Visible="false" ><i class="fa fa-check-square"></i> Export to PDF</asp:LinkButton>
                                    <asp:LinkButton ID="btnReport" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnReport_Click"><i class="fa fa-check-square"></i> Report </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto;">
                                                <asp:UpdatePanel ID="UpdatePanel1s00" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgsupplierauditplan" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both"
                                                            DataKeyNames="SAP_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            AllowPaging="True" OnRowCommand="dgsupplierauditplan_RowCommand" OnPageIndexChanging="dgsupplierauditplan_PageIndexChanging"
                                                            PageSize="50">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <div class="clearfix">
                                                                            <div class="btn-group">
                                                                                <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                                    Select <i class="fa fa-angle-down"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu" role="menu">
                                                                                    <li>
                                                                                        <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                            Text="Modify" CommandArgument='<%# Bind("SAP_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                                    </li>
                                                                                    <li>
                                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                                            CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                                    </li>
                                                                                    <li>
                                                                                        <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" Visible="false"
                                                                                            CommandName="Print" Text="Print" CommandArgument='<%# Bind("SAP_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                                                </ul>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SAP_CODE" SortExpression="SAP_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSAP_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("SAP_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="Breakdown No" SortExpression="B_NO" Visible="false"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_NO" runat="server" Text='<%# Eval("B_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Doc No." SortExpression="SAP_DOC_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_SLIPNO" runat="server" Text='<%# Eval("SAP_DOC_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Supplier Name" SortExpression="SAP_AUDITOR_NAME" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIM_MATERIAL_REQ1" CssClass=" Control-label" runat="server" Text='<%# Eval("SAP_AUDITOR_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <%--  <asp:TemplateField HeaderText="Tool Type" Visible="true" SortExpression="T_TYPE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblT_TYPE" CssClass=" Control-label" runat="server" Text='<%# Eval("T_TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>--%>
                                                                <%-- <asp:TemplateField HeaderText="Part No. & Name" SortExpression="I_CODENO" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Doc Date" SortExpression="SAP_DOC_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_DATE" runat="server" Text='<%# Eval("SAP_DOC_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Audit Date" SortExpression="SAP_AUDIT_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSAP_AUDIT_DATE" runat="server" Text='<%# Eval("SAP_AUDIT_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="Reason" SortExpression="B_REASON" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_REASON" runat="server" Text='<%# Eval("B_REASON") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Action" SortExpression="SAP_ACTION" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_ACTION" runat="server" Text='<%# Eval("SAP_ACTION") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--   <asp:TemplateField HeaderText="Closure" SortExpression="B_CLOSURE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_CLOSURE" runat="server" Text='<%# Eval("B_CLOSURE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <%-- <asp:TemplateField HeaderText="Hours" SortExpression="B_HOURS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_HOURS" runat="server" Text='<%# Eval("B_HOURS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <%-- <asp:TemplateField HeaderText="Report" SortExpression="IM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <%--<asp:Label ID="lblB_FILE" runat="server" Text='<%# Eval("B_FILE") %>'></asp:Label>
                                                                        <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                            CommandName="ViewPDF" Text='<%# Eval("SAP_FILE") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="File" Visible="true" SortExpression="SAP_FILE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <%--<asp:Label ID="lblB_FILE" runat="server" Text='<%# Eval("B_FILE") %>'></asp:Label>--%>
                                                                        <asp:LinkButton ID="lnkView1" BorderStyle="None" runat="server" CausesValidation="False"
                                                                            CommandName="Download" Text='<%# Eval("SAP_FILE") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" SortExpression="SAP_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblB_STATUS" runat="server" Text='<%# Eval("SAP_STATUS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--  <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                                                <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server"> Ok</asp:LinkButton>
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this
    will reduce page load time) -->
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
    <!-- BEGIN PAGE LEVEL
    PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
