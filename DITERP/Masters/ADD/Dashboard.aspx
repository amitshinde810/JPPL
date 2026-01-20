<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="Masters_ADD_Dashboard" Title="DIT ERP | Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
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
    <!--JS for nested griview-->

    <script src="../../assets/JS/nested-grid-1.8.3-jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $("[src*=plus]").live("click", function() {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../assets/img/minus.gif");
        });
        $("[src*=minus]").live("click", function() {
            $(this).attr("src", "../../assets/img/plus.gif");
            $(this).closest("tr").next().remove();
        });
    </script>

    <!--End of JS for nested griview-->

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div id="MSG" class="col-md-6">
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
            <div id="StockValuation" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Stock Valuation
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-left">
                                                    From Date:
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFromDate" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"
                                                                    runat="server" placeholder="dd MMM yyyy" CssClass="form-control" TabIndex="6"
                                                                    ValidationGroup="Save"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                    Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="dd MMM yyyy">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    To Date:
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"
                                                                    placeholder="dd MMM yyyy" CssClass="form-control" TabIndex="7" ValidationGroup="Save"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                    Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="dd MMM yyyy">
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
                                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                <ContentTemplate>
                                                    <div style="width: 100%; height: 500px; overflow: scroll">
                                                        <asp:GridView ID="dgDashboard" Width="100%" AutoGenerateColumns="false" CellPadding="0"
                                                            Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            PageSize="15" DataKeyNames="I_CAT_CODE" runat="server" AllowPaging="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Category" SortExpression="I_CAT_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CAT_CODE" runat="server" Text='<%# Bind("I_CAT_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Category" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CAT_NAME" runat="server" Text='<%# Bind("I_CAT_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Value (Lacs)" SortExpression="VALUE" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVALUE" CssClass="" runat="server" Text='<%# Eval("VALUE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Store Wise Stock
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div style="width: 100%; height: 570px; overflow: scroll">
                                                        <asp:GridView ID="dgStoreWiseStock" Width="100%" AutoGenerateColumns="false" CellPadding="0"
                                                            Font-Size="12px" ShowFooter="true" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            PageSize="15" runat="server" AllowPaging="false" OnRowDataBound="GridView1_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Store Name" SortExpression="STORE_NAME" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTORE_NAME" runat="server" Text='<%# Bind("STORE_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="abl" runat="server" Text="Total" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="total Qty" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQTY" runat="server" Text='<%# Bind("QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTOTQTY" runat="server" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tonnage (Tons)" SortExpression="INWARD_WT" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblINWARD_WT" CssClass="" runat="server" Text='<%# Eval("INWARD_WT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTOTWT" runat="server" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount (Lacs)" SortExpression="INWARD_AMT" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblINWARD_AMT" DataFormatString="{0:N2}" CssClass="" runat="server"
                                                                            Text='<%# Eval("INWARD_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTOTAMT" runat="server" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Total Sales
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="form-body">
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <label class="col-md-4 text-right" id="lblTillDate" runat="server">
                                                        Till Date</label>
                                                    <label class="col-md-4 text-right" id="lblCurrMonth" runat="server">
                                                        Current Month</label>
                                                    <label class="col-md-4 text-right" id="lblOnDate" runat="server">
                                                        On Date</label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblTillDateNetAmt" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblCurrMonthNetAmt" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblOnDateNetAmt" runat="server"></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Total Purchase
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <div style="width: 100%; height: 310px; overflow: scroll">
                                                        <asp:GridView ID="dgTotPurchase" Width="100%" AutoGenerateColumns="false" CellPadding="0"
                                                            Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            PageSize="15" DataKeyNames="I_CODE" OnRowDataBound="dgTotPurchase_RowDataBound"
                                                            runat="server" AllowPaging="false">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Image alt="" Style="cursor: pointer" src="../../assets/img/plus.gif" ID="ImgP"
                                                                            runat="server" />
                                                                        <asp:Panel ID="pnlCustomerDetails" runat="server" Style="display: none">
                                                                            <asp:GridView ID="dgMaterial" runat="server" CellPadding="4" Font-Size="12px" ShowFooter="false"
                                                                                Font-Names="Verdana" GridLines="None" DataKeyNames="I_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                AllowPaging="false" AutoGenerateColumns="false" OnRowDataBound="dgMaterial_RowDataBound"
                                                                                OnRowCommand="dgMaterial_RowCommand">
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblI_CODE" runat="server" Text='<%# Eval("I_CODE") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="I_CODE" HeaderText="CODE" Visible="false" />
                                                                                    <asp:BoundField DataField="I_CODENO" HeaderText="CODE No." />
                                                                                    <asp:BoundField DataField="I_NAME" HeaderText="Raw material" />
                                                                                    <asp:TemplateField HeaderText="ON Date Qty(Tons)">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDAILYQTY1" runat="server" Text='<%# Eval("DAILYQTY") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="On Date Amt(Lacs)">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDAILYAMT1" runat="server" Text='<%# Eval("DAILYAMT") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="IWD_REV_QTY_MONTH" HeaderText="Current Month Qty (Tons)" />
                                                                                    <asp:TemplateField HeaderText="Current Month Amt(Lacs)">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblMONTHLYAMT1" runat="server" Text='<%# Eval("MONTHLYAMT") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="IWD_REV_QTY" HeaderText="Till Date Qty(Tons)" />
                                                                                    <asp:BoundField DataField="AMT" HeaderText="Till Date Amt(Lacs)" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Category" SortExpression="I_CAT_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Raw material" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Raw material Name" SortExpression="VALUE" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="On Date Qty(Tons)" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDAILYQTY" runat="server" Text='<%# Bind("DAILYQTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="On Date Amt(Lacs)" SortExpression="VALUE" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDAILYAMT" CssClass="" runat="server" Text='<%# Eval("DAILYAMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Current Month Qty (Tons)" SortExpression="VALUE" HeaderStyle-HorizontalAlign="Right"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_REV_QTY_MONTH" CssClass="" runat="server" Text='<%# Eval("IWD_REV_QTY_MONTH") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Current Month Amt(Lacs)" SortExpression="I_CAT_CODE"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMONTHLYAMT" runat="server" Text='<%# Bind("MONTHLYAMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Till Date Qty(Tons)" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_REV_QTY" runat="server" Text='<%# Bind("IWD_REV_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Till Date Amt(Lacs)" SortExpression="I_CAT_CODE" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAMTE" runat="server" Text='<%# Bind("AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Total SubContractor Inward
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <label class="col-md-4 text-right" id="Label3" runat="server">
                                                        Till Date</label>
                                                    <label class="col-md-4 text-right" id="Label1" runat="server">
                                                        Current Month</label>
                                                    <label class="col-md-4 text-right" id="Label2" runat="server">
                                                        On Date</label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblTillDateNetAmtSubCon" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblCurrMonthNetAmtSubCon" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4 text-right">
                                                        <asp:Label ID="lblOnDateNetAmtSubCon" runat="server"></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Total Raw Material Stock
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a></a>
                                </div>
                            </div>
                           <div class="portlet-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <label class="col-md-8 text-right" id="Label4" runat="server">
                                                        Total Raw Material Stock</label>
                                                    <label class="col-md-3 text-right" id="Label6" runat="server">
                                                    </label>
                                                </ContentTemplate>
                                           </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <div class="col-md-8 text-right">
                                                        <asp:Label ID="lblrawMaterial" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-3 text-right">
                                                        <asp:Label ID="Label9" runat="server"></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>--%>
                    </div>
                    <!-- END PAGE CONTENT-->
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
                                <asp:Panel ID="popUpPanel5" runat="server" ScrollBars="None" Height="260px" CssClass="responsive-width">
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <center>
                                                <h4>
                                                    Acceptance Pending
                                                </h4>
                                            </center>
                                        </div>
                                        <div class="portlet-body form">
                                            <div class="form-horizontal">
                                                <div class="form-body">
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="PnlMsgDailyAct" runat="server" Visible="false" Style="background-color: #feefb3;
                                                                        height: 40px; width: 100%; border: 1px solid #9f6000">
                                                                        <div style="vertical-align: middle; margin-top: 10px;">
                                                                            <asp:Label ID="lblDailyActMsg" runat="server" Style="color: #9f6000; font-size: medium;
                                                                                font-weight: bold; margin-top: 5px; margin-left: 10px;"></asp:Label>
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
                                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgActivity_Task" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                        Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                        AllowPaging="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Store Name" Visible="true" SortExpression="STORE_NAME"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblACT_NO" CssClass="" runat="server" Text='<%# Eval("STORE_NAME") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Count" SortExpression="ACT_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblACT_DATE" CssClass="" runat="server" Text='<%# Eval("RECOUNT") %>'></asp:Label>
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
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                        </div>
                                                        <div class="col-md-8">
                                                            <div class="col-md-4">
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton ID="Button6" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                    OnClick="btnConfirm_Click">Continue</asp:LinkButton>
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
        <!-- END CONTENT -->
        <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will
    reduce page load time) -->
        <!-- BEGIN CORE PLUGINS -->

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

        <!-- END CORE PLUGINS -->
        <!-- BEGIN PAGE LEVEL PLUGINS -->

        <script src="../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

        <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

        <script src="../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
            type="text/javascript"></script>

        <!-- END PAGE LEVEL PLUGINS -->
        <!-- BEGIN PAGE LEVEL SCRIPTS -->

        <script src="../../assets/scripts/app.js" type="text/javascript"></script>

        <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
