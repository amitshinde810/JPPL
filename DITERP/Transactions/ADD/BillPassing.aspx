<%@ Page Title="Bill Passing - ADD" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="BillPassing.aspx.cs" Inherits="Transactions_ADD_BillPassing" %>

<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
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
            jQuery("#<%=ddlSupplierName.ClientID %>").select2();
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

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content ">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
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
            <div class="row ">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Bill Passing
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="29" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <asp:Panel ID="MainPanel" runat="server">
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Bill No.
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtBillNo" placeholder="Bill No"
                                                            TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required"></span>Bill Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtBillDate" placeholder="dd MMM yyyy"
                                                                TabIndex="2" MsgObrigatorio="Please Select Bill Date" runat="server"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtBillDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtBillDate" PopupButtonID="txtBillDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Supplier Name
                                                    </label>
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSupplierName" CssClass="select2" Width="700px" runat="server"
                                                                    MsgObrigatorio="Select Supplier Name" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplierName_SelectedIndexChanged"
                                                                    TabIndex="3">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Invoice No.
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtInvoceNo" placeholder="Invoice No"
                                                            TabIndex="4" runat="server" ReadOnly="false" MsgObrigatorio="Invoice No"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required"></span>Invoice Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtInvoiceDate" placeholder="dd MMM yyyy"
                                                                TabIndex="5" MsgObrigatorio="Please Select Bill Date" runat="server"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtInvoiceDate_CalendarExtender1" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtInvoiceDate" PopupButtonID="txtInvoiceDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                            <ContentTemplate>
                                                                <asp:RadioButtonList OnSelectedIndexChanged="rbLstIsExise_SelectedIndexChanged" RepeatDirection="Horizontal"
                                                                    runat="server" CssClass="radio-list" AutoPostBack="true" ID="rbLstIsExise">
                                                                    <asp:ListItem Value="0">With GST</asp:ListItem>
                                                                    <asp:ListItem Value="1">Without GST</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="chkAllRec_CheckedChanged"
                                                                    ID="chkAllRec" Text="All" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row" style="overflow: auto;">
                                            <%--Grid View--%>
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel7895" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgBillPassing" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None" DataKeyNames="IWM_CODE" Font-Names="Verdana"
                                                            Font-Size="12px" ShowFooter="false" PageSize="6" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            OnPageIndexChanging="dgBillPassing_PageIndexChanging" OnRowCommand="dgBillPassing_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="checker" EnableViewState="true"
                                                                            AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%-- here we insert database field for bind and eval--%>
                                                                <asp:TemplateField HeaderText="IWM_CODE" SortExpression="IWM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CODE" runat="server" Text='<%# Bind("IWM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IWD_CPOM_CODE" SortExpression="IWD_CPOM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_CPOM_CODE" runat="server" Text='<%# Bind("IWD_CPOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false" HeaderText="IWM TYPE" SortExpression="IWM_TYPE"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_TYPE" runat="server" Text='<%# Eval("IWM_TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PoNo" SortExpression="SPOM_PO_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOM_PO_NO" runat="server" Text='<%# Eval("SPOM_PO_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GIN No" SortExpression="IWM_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_NO" runat="server" Text='<%# Eval("IWM_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GIN Date" SortExpression="IWM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_DATE" runat="server" Text='<%# Eval("IWM_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Challan No" SortExpression="IWM_CHALLAN_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CHALLAN_NO" runat="server" Text='<%# Eval("IWM_CHALLAN_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Challan Date" SortExpression="IWM_CHAL_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CHAL_DATE" runat="server" Text='<%# Eval("IWM_CHAL_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="IWD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_I_CODE" runat="server" Text='<%# Eval("IWD_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ch. Qty" SortExpression="IWD_CH_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_Ch_QTY" runat="server" Text='<%# Eval("IWD_CH_QTY") %>' CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rec. Qty" SortExpression="INSM_RECEIVED_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIIWD_REV_QTY" runat="server" Text='<%# Eval("IWD_REV_QTY") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ok. Qty" SortExpression="IWD_CON_OK_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_CON_OK_QTY" runat="server" Text='<%# Eval("IWD_CON_OK_QTY") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="SPOD_RATE" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_RATE" runat="server" Text='<%# Eval("SPOD_RATE") %>' CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="SPOD_TOTAL_AMT" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_TOTAL_AMT" runat="server" Text='<%# Eval("SPOD_TOTAL_AMT") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Disc.Amt" SortExpression="SPOD_DISC_AMT" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_DISC_AMT" runat="server" Text='<%# Eval("SPOD_DISC_AMT") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exc(%)" SortExpression="SPOD_EXC_PER" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_EXC_PER" runat="server" Text='<%# Eval("SPOD_EXC_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SPOD_EDU_CESS_PER(%)" SortExpression="SPOD_EDU_CESS_PER"
                                                                    HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_EDU_CESS_PER" runat="server" Text='<%# Eval("SPOD_EDU_CESS_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SPOD_H_EDU_CESS(%)" SortExpression="SPOD_H_EDU_CESS"
                                                                    HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_H_EDU_CESS" runat="server" Text='<%# Eval("SPOD_H_EDU_CESS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EX_EX_DUTY)" SortExpression="EX_EX_DUTY" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_DUTY" runat="server" Text='<%# Eval("EX_EX_DUTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EX_EX_CESS)" SortExpression="EX_EX_CESS" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_CESS" runat="server" Text='<%# Eval("EX_EX_CESS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EX_EX_HCESS)" SortExpression="EX_EX_DUTY" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_HCESS" runat="server" Text='<%# Eval("EX_EX_HCESS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Code" SortExpression="SPOD_T_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_T_CODE" runat="server" Text='<%# Eval("SPOD_T_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax" Visible="false" SortExpression="T_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblT_NAME" runat="server" Text='<%# Eval("ST_TAX_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax (%)" SortExpression="T_SALESTAX" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblT_SALESTAX" runat="server" Text='<%# Eval("ST_SALES_TAX") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SPOD_EXC_Y_N" SortExpression="SPOD_EXC_Y_N" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_EXC_Y_N" runat="server" Text='<%# Eval("SPOD_EXC_Y_N") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%-- End Grid View--%>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Basic Amount
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtBasicAmount" Text="0.00" runat="server"
                                                                    ReadOnly="true" TabIndex="7"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtBasicAmount"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required"></span>Vat/CST
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtTaxPer" Text="0.00" TabIndex="15"
                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtsalestaxamt" Text="0.00" TabIndex="16"
                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtsalestaxamt"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        Discount Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel1456" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtDiscountAmt" Text="0.00" TabIndex="8"
                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtDiscountAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Other Charges
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtOtherCharges" Text="0.00"
                                                                    TabIndex="17" runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtOtherCharges_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtOtherCharges"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        Packing & Forwd. Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtPackingAmt" Text="0.00" TabIndex="9"
                                                                    runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtPackingAmt_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtPackingAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required"></span>Additional Duty
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel895" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtloadingAmt" Text="0.00" TabIndex="18"
                                                                    runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtloadingAmt_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtloadingAmt"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        Accessable Value
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtAccesableValue" Text="0.00"
                                                                    TabIndex="10" runat="server" ReadOnly="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtAccesableValue"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Freight Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtFreightAmt1" Text="0.00" TabIndex="19"
                                                                    runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtFreightAmt1_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtFreightAmt1"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        Taxable Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtTaxableAmt" Text="0.00" TabIndex="14"
                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtTaxableAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Insurance Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtInsuranceAmt" Text="0.00"
                                                                    TabIndex="20" runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtInsuranceAmt_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txtInsuranceAmt"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        CGST Amt
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtexcper" Text="0.00" TabIndex="11"
                                                                    runat="server" AutoPostBack="true" OnTextChanged="txtexcper_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtexcper"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtExciseAmount" Text="0.00"
                                                                    TabIndex="11" runat="server" OnTextChanged="txtExciseAmount_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtExciseAmount"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Transport Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtTransportAmt" Text="0.00"
                                                                    TabIndex="21" runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtTransportAmt_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtTransportAmt"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        SGST Amt
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txteducessper" Text="0.00" TabIndex="12"
                                                                    runat="server" OnTextChanged="txteducessper_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" TargetControlID="txteducessper"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtEduCessAmt" Text="0.00" TabIndex="12"
                                                                    runat="server"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtEduCessAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Octroi Amt
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtOctroiAmt2" Text="0.00" TabIndex="22"
                                                                    runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtOctroiAmt2_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtOctroiAmt2"
                                                                    ValidChars="0123456789." runat="server" />
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
                                                        IGST Amt
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtsheper" Text="0.00" TabIndex="13"
                                                                    runat="server" OnTextChanged="txtsheper_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" TargetControlID="txtsheper"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtSHEduCessAmt" Text="0.00"
                                                                    TabIndex="13" runat="server"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtSHEduCessAmt"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Round Off
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtRoundOff" Text="0.00" TabIndex="23"
                                                                    runat="server" ReadOnly="false" AutoPostBack="true" OnTextChanged="txtRoundOff_TextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtRoundOff"
                                                                    ValidChars="0123456789.-" runat="server" />
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
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:LinkButton runat="server" ID="lnkPop" OnClick="lnkPop_Click" CssClass="">Detail</asp:LinkButton>
                                                    </div>
                                                    <label class="col-md-3 control-label label-sm">
                                                        <span class="required"></span>Grand Total
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtGrandTotal" Text="0.00" TabIndex="24"
                                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtGrandTotal"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="25" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="26" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel33">
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
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="29" runat="server" Visible="true"
                                                            OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                        <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="30" runat="server"
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
                    <asp:UpdatePanel runat="server" ID="UpdatePanel22">
                        <ContentTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupDetail" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                PopupControlID="PnlDetail" TargetControlID="LinkButton2">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="PnlDetail" runat="server" Style="display: none;">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="captionPopup">
                                            GST Details
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row" style="overflow: auto;">
                                                    <%--Grid View--%>
                                                    <div class="col-md-12">
                                                        <asp:GridView ID="dgDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None" DataKeyNames="" Font-Names="Verdana" Font-Size="12px"
                                                            ShowFooter="false" PageSize="6" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            OnPageIndexChanging="dgDetail_PageIndexChanging" OnRowCommand="dgDetail_RowCommand">
                                                            <Columns>
                                                                <%-- here we insert database field for bind and eval--%>
                                                                <asp:TemplateField HeaderText="Item Count" SortExpression="ICount" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblICount" runat="server" Text='<%# Eval("ICount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CGCT Per." SortExpression="SPOD_EXC_PER" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_EXC_PER" runat="server" Text='<%# Eval("EXD_EXC_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CGCT Amt" SortExpression="EX_EX_DUTY" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_DUTY" runat="server" Text='<%# Eval("EXD_EXC_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SGCT Per." SortExpression="SPOD_EDU_CESS_PER" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_EDU_CESS_PER" runat="server" Text='<%# Eval("EXD_EDU_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SGCT Amt" SortExpression="EX_EX_CESS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_CESS" runat="server" Text='<%# Eval("EXD_EDU_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IGST Per." SortExpression="SPOD_H_EDU_CESS" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOD_H_EDU_CESS" runat="server" Text='<%# Eval("EXD_HSEDU_PER") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IGST Amt" SortExpression="EX_EX_HCESS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEX_EX_HCESS" runat="server" Text='<%# Eval("EXD_HSEDU_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                        <%-- End Grid View--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-5 col-md-9">
                                                        <asp:LinkButton ID="lnkOkBtn" CssClass="btn blue" TabIndex="30" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
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
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtBillDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlSupplierName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtInvoceNo.ClientID%>', '#Avisos') == false) {
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
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
