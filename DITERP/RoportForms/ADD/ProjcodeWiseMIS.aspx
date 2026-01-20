<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProjcodeWiseMIS.aspx.cs"
    Inherits="RoportForms_ADD_ProjcodeWiseMIS" Title="Project Code wise Supplier Purchase Order - MIS Valuation Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
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

            jQuery("#<%=ddlProCode.ClientID%>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Project Code wise Supplier Purchase Order
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                From Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                AutoPostBack="true" TabIndex="6" ValidationGroup="Save" MsgObrigatorio="From Date"
                                                                OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                To Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanelff4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                AutoPostBack="true" TabIndex="7" ValidationGroup="Save" MsgObrigatorio="To Date"
                                                                OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkDateAll" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkDateAll" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                            TabIndex="8" OnCheckedChanged="chkDateAll_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label">
                                                <span class="required"></span>Project Code</label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlProCode" CssClass="select2" Width="100%" runat="server"
                                                            MsgObrigatorio="Please Select PO Type " TabIndex="1" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkProCodeAll" runat="server" CssClass="checker" Text="&nbspAll"
                                                            AutoPostBack="True" TabIndex="8" OnCheckedChanged="chkProCodeAll_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-actions fluid">
                                        <div class="col-md-offset-4 col-md-9">
                                            <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="15" runat="server" OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                                OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto;">
                                                <div class="table-responsive">
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgProjectCode" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                DataKeyNames="PROCM_CODE" OnPageIndexChanging="dgProjectCode_PageIndexChanging"
                                                                OnRowCommand="dgProjectCode_RowCommand" OnRowDataBound="dgProjectCode_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="PROCM_CODE" SortExpression="ACT_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPROCM_CODE" runat="server" Text='<%# Bind("PROCM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="true" HeaderText="Project Code">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                Text='<%# Bind("PROCM_NAME") %>' CommandArgument='<%# Bind("PROCM_CODE") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Proj Name" Visible="false" SortExpression="PROCM_NAME"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPROCM_NAME" CssClass="" runat="server" Text='<%# Eval("PROCM_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO COUNT" SortExpression="PO_COUNT" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPO_COUNT" CssClass="" runat="server" Text='<%# Eval("PO_COUNT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO AMOUNT" SortExpression="PO_AMOUNT" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPO_AMOUNT" CssClass="" runat="server" Text='<%# Eval("PO_AMOUNT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="INWARD QTY" SortExpression="INWARD_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINWARD_QTY" CssClass="" runat="server" Text='<%# Eval("INWARD_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="INWARD AMT" SortExpression="INWARD_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblINWARD_AMT" CssClass="" runat="server" Text='<%# Eval("INWARD_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="BAL AMT" SortExpression="BAL_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBAL_AMT" CssClass="" runat="server" Text='<%# Eval("BAL_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel33">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                                    <cc1:ModalPopupExtender runat="server" ID="ModalCancleConfirmation" BackgroundCssClass="modalBackground"
                                                        OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                                        PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:Panel ID="popUpPanel5" runat="server" CssClass="table-responsive" Height="600px"
                                                        Width="1200px" ScrollBars="Vertical" BackColor="White">
                                                        <div class="portlet box blue">
                                                            <div class="portlet-title">
                                                                <div>
                                                                    Project Code wise
                                                                </div>
                                                            </div>
                                                            <div class="portlet-body form">
                                                                <div class="form-horizontal">
                                                                    <div class="form-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <div style="overflow: auto;">
                                                                                    <div class="table-responsive">
                                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:GridView ID="dgProjCodeWisePO" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                                    Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                    DataKeyNames="PROCM_CODE" OnPageIndexChanging="dgProjCodeWisePO_PageIndexChanging"
                                                                                                    OnRowCommand="dgProjCodeWisePO_RowCommand">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="PROCM_CODE" SortExpression="ACT_CODE" Visible="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPROCM_CODE" runat="server" Text='<%# Bind("PROCM_CODE") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField Visible="true" HeaderText="PO No">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                                                    Text='<%# Bind("SPOM_PONO") %>' CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Proj Name" Visible="false" SortExpression="PROCM_NAME"
                                                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPROCM_NAME" CssClass="" runat="server" Text='<%# Eval("PROCM_NAME") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Item COUNT" SortExpression="ITEM_COUNT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblITEM_COUNT" CssClass="" runat="server" Text='<%# Eval("ITEM_COUNT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="PO AMOUNT" SortExpression="PO_AMOUNT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPO_AMOUNT" CssClass="" runat="server" Text='<%# Eval("PO_AMOUNT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="ORDER QTY" SortExpression="ORDER_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblORDER_QTY" CssClass="" runat="server" Text='<%# Eval("ORDER_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="INWARD QTY" SortExpression="INWARD_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblINWARD_QTY" CssClass="" runat="server" Text='<%# Eval("INWARD_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="INWARD AMT" SortExpression="INWARD_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblINWARD_AMT" CssClass="" runat="server" Text='<%# Eval("INWARD_AMT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="BAL QTY" SortExpression="BAL_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBAL_QTY" CssClass="" runat="server" Text='<%# Eval("BAL_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="BAL AMT" SortExpression="BAL_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBAL_AMT" CssClass="" runat="server" Text='<%# Eval("BAL_AMT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                                    <PagerStyle CssClass="pgr" />
                                                                                                </asp:GridView>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <br />
                                                                            <div class="col-md-12">
                                                                                <div class="col-md-5">
                                                                                </div>
                                                                                <div class="col-md-1">
                                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:Button ID="btnBack" CssClass="btn green" runat="server" Text="Back" OnClick="btnBack_Click" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                                <div class="col-md-1">
                                                                                    <asp:LinkButton ID="LinkButton2" CssClass="btn default" TabIndex="16" runat="server"
                                                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                                                                </div>
                                                                            </div>
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
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="UpdayiitePaneld3">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                                    <cc2:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BackgroundCssClass="modalBackground"
                                                        OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                                        PopupControlID="Panel1" TargetControlID="LinkButton1">
                                                    </cc2:ModalPopupExtender>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" Height="600px"
                                                        Width="1200px" ScrollBars="Vertical" BackColor="White">
                                                        <div class="portlet box blue">
                                                            <div class="portlet-title">
                                                                <div>
                                                                    Purchase Order
                                                                </div>
                                                            </div>
                                                            <div class="portlet-body form">
                                                                <div class="form-horizontal">
                                                                    <div class="form-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <div style="overflow: auto;">
                                                                                    <div class="table-responsive">
                                                                                        <asp:UpdatePanel ID="UpdatePaneluy4" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:GridView ID="dgPO" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                                    Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                    DataKeyNames="PROCM_CODE" OnPageIndexChanging="dgPO_PageIndexChanging">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="PROCM_CODE" SortExpression="ACT_CODE" Visible="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPROCM_CODE" runat="server" Text='<%# Bind("PROCM_CODE") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField Visible="false" HeaderText="PO No">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                                                    Text='<%# Bind("SPOM_PONO") %>' CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="PO NO" Visible="true" SortExpression="PROCM_NAME"
                                                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPROCM_NAME" CssClass="" runat="server" Text='<%# Eval("SPOM_PONO") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="PO AMOUNT" SortExpression="PO_AMOUNT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPO_AMOUNT" CssClass="" runat="server" Text='<%# Eval("PO_AMOUNT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Item name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="ORDER QTY" SortExpression="ORDER_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblORDER_QTY" CssClass="" runat="server" Text='<%# Eval("ORDER_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="INWARD QTY" SortExpression="INWARD_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblINWARD_QTY" CssClass="" runat="server" Text='<%# Eval("INWARD_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="INWARD AMT" SortExpression="INWARD_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblINWARD_AMT" CssClass="" runat="server" Text='<%# Eval("INWARD_AMT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="BAL QTY" SortExpression="BAL_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBAL_QTY" CssClass="" runat="server" Text='<%# Eval("BAL_QTY") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="BAL AMT" SortExpression="BAL_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBAL_AMT" CssClass="" runat="server" Text='<%# Eval("BAL_AMT") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                                    <PagerStyle CssClass="pgr" />
                                                                                                </asp:GridView>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <br />
                                                                            <div class="col-md-12">
                                                                                <div class="col-md-5">
                                                                                </div>
                                                                                <div class="col-md-1">
                                                                                    <asp:UpdatePanel ID="UpdatePaneddftyil5" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:Button ID="Button1dd" CssClass="btn green" runat="server" Text="Back" OnClick="Button1dd_Click" />
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                                <div class="col-md-1">
                                                                                    <asp:LinkButton ID="LinkButton3" CssClass="btn default" TabIndex="16" runat="server"
                                                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                                                                </div>
                                                                            </div>
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
</asp:Content>
