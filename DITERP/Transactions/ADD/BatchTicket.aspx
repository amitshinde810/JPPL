<%@ Page Title="Batch Ticket" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="BatchTicket.aspx.cs" Inherits="Transactions_ADD_BatchTicket" %>

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
        function oncancel1(sender, e) {
            $find('ModalPopupPrintSelection1').hide();
            __doPostBack('btnCamcelBatch', e);
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
        function Showalert1() {
            $('#Msg').fadeIn(6000)
            $('#Msg').fadeOut(6000)
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
            <div class="row" id="Msg">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
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
                                <i class="fa fa-reorder"></i>Batch Ticket
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Batch Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlBatchType_SelectedIndexChanged" ID="ddlBatchType"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="1" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Batch Type">
                                                                <asp:ListItem Selected="True" Value="0">Select Batch Type</asp:ListItem>
                                                                <asp:ListItem Value="1">As Per Work Order</asp:ListItem>
                                                                <asp:ListItem Value="2">Direct</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Batch No</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBatchNo" runat="server" placeholder="Batch No" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Batch No" MaxLength="20"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Batch Date</label>
                                                <div class="col-md-3 ">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtBatchDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Batch Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtBatchDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtBatchDate" PopupButtonID="txtBatchDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Work Order No</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlWorkOrderNo_SelectedIndexChanged" ID="ddlWorkOrderNo"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="4" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Work Order">
                                                                <asp:ListItem Selected="True" Value="0">Select Work Order</asp:ListItem>
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
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Item Code</label>
                                                <div class="col-md-3 ">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="5" Visible="True" MsgObrigatorio="Item Code" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Value="0">Select Item Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Formula Type</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlFormulaType_SelectedIndexChanged" ID="ddlFormulaType"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="6" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Batch Type">
                                                                <asp:ListItem Selected="True" Value="0">Select Formula Type</asp:ListItem>
                                                                <asp:ListItem Value="1">With Bases</asp:ListItem>
                                                                <asp:ListItem Value="2">Without Bases</asp:ListItem>
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
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Formula Code</label>
                                                <div class="col-md-3 ">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlFormulaCode_SelectedIndexChanged" ID="ddlFormulaCode"
                                                                CssClass="select2_category form-control" runat="server" TabIndex="7" AutoPostBack="True"
                                                                Visible="True" MsgObrigatorio="Formula Code">
                                                                <asp:ListItem Selected="True" Value="0">Select Formula Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Work Order Qty
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtWorkOrdQty" placeholder="0.00"
                                                                TabIndex="19" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtWorkOrdQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <div class="row" style="overflow-x: auto;">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel26">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgMainShade" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        AutoGenerateColumns="False" CellPadding="4" TabIndex="8" ShowHeaderWhenEmpty="true"
                                                        OnRowDataBound="OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Process Code" SortExpression="PROCESS_CODE" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROCESS_CODE" runat="server" Text='<%# Bind("PROCESS_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Step No" SortExpression="STEP_NO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSTEP_NO" runat="server" Text='<%# Bind("STEP_NO") %>' Width="60px"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Process Name" SortExpression="PROCESS_NAME" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROCESS_NAME" runat="server" Text='<%# Bind("PROCESS_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="I_CURRENT_BAL" HeaderText="Stock Qty" ItemStyle-Width="100px"
                                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" SortExpression="I_CURRENT_BAL" />
                                                            <asp:TemplateField HeaderText="Liters" SortExpression="QTY_IN_LTR" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQTY_IN_LTR" runat="server" Width="100px" Text='<%# Eval("QTY_IN_LTR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" Width="100px" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Raw Material" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO" runat="server" Width="100px" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Raw Material Code" SortExpression="I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODE" runat="server" Width="100px" Text='<%# Eval("I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="WEIGHT_IN_KG" HeaderText="Kilogram" ItemStyle-Width="100px"
                                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" SortExpression="WEIGHT_IN_KG" />
                                                            <asp:TemplateField HeaderText="Qty(In KG)" SortExpression="QtyinKG" HeaderStyle-HorizontalAlign="Right"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQtyInKg" CssClass=" Control-label" runat="server" Width="100px"
                                                                        Text='<%# Eval("QtyinKG")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Density" SortExpression="Density" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDensity" CssClass=" Control-label" runat="server" Width="60px"
                                                                        Text='<%#Eval("Density") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" Width="60px" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <%-- </div>--%>
                                </div>
                            </div>
                            <br />
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Total (In Kg).
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right" ID="txtTotalInKg" placeholder="0.00"
                                                            TabIndex="19" runat="server" MsgObrigatorio="Total (In Kg)" OnTextChanged="txtTotalInKG_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtTotalInKg"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Total (In Ltr).
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right" ID="txtTotalInLtr" placeholder="0.00"
                                                            TabIndex="19" runat="server" MsgObrigatorio="Total (In Ltr)" Enabled="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtTotalInLtr"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-1 control-label text-right">
                                                Density
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right" ID="txtAvgDensity" placeholder="0.00"
                                                            TabIndex="20" runat="server" MaxLength="50" MsgObrigatorio="Density" Enabled="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtAvgDensity"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="9" runat="server" OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="10" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                    <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                        <ContentTemplate>
                            <asp:LinkButton ID="CheckCondition1" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection1" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel1()" DynamicServicePath="" Enabled="True"
                                PopupControlID="Panel1" TargetControlID="CheckCondition1">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="Panel1" runat="server" Style="display: none;">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="captionPopup">
                                            Batch Number
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right" ID="txtAutoBatchNo" TabIndex="20"
                                                                    runat="server" MaxLength="50" MsgObrigatorio="Batch No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtAutoBatchNo"
                                                                    ValidChars="0123456789" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-3 col-md-9">
                                                        <asp:LinkButton ID="btnOkBatchNo" CssClass="btn blue" TabIndex="26" runat="server"
                                                            Visible="true" OnClick="btnOkBatchNo_Click">  OK </asp:LinkButton>
                                                        <asp:LinkButton ID="btnCamcelBatch" CssClass="btn default" TabIndex="28" runat="server"
                                                            OnClick="btnCamcelBatch_Click"> Cancel</asp:LinkButton>
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts
    at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if
    lt IE 9]> <script src="assets/plugins/respond.min.js"></script> <script src="assets/plugins/excanvas.min.js"></script>
    <![endif]-->
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

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlBatchType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtBatchDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlItemCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlFormulaCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    $("#Avisos").fadeOut(6000);
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
