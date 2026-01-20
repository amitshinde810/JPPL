<%@ Page Title="Fill Off Sheet" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="FillOffSheet.aspx.cs" Inherits="Transactions_ADD_FillOffSheet" %>

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
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
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Fill Off Sheet
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
                                            <label class="col-md-2 control-label label-sm">
                                                <font color="red">*</font> Batch No.
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlbatchNo" CssClass="select2_category form-control" runat="server"
                                                            MsgObrigatorio="Batch No" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlBatchNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right ">
                                                <font color="red">*</font> Date
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Date" ReadOnly="false"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                        Format="dd MMM yyyy" TargetControlID="txtDate" PopupButtonID="txtDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label label-sm">
                                                Formula Code
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtFormula" runat="server" CssClass="form-control" ValidationGroup="Save"
                                                            TabIndex="3" ReadOnly="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right ">
                                                Customer Name</label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                            TabIndex="4" MsgObrigatorio="Customer Name" Visible="True">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Approved By
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtApprovedBy" placeholder="Approved By"
                                                    TabIndex="5" runat="server" MaxLength="100" MsgObrigatorio="Approved By"></asp:TextBox>
                                            </div>
                                            <label class=" col-md-1 control-label text-right ">
                                                Doc No.</label>
                                            <div class="col-md-2 ">
                                                <asp:TextBox ID="txtDocNo" runat="server" placeholder="Doc No" CssClass="form-control"
                                                    ValidationGroup="Save" ReadOnly="true" TabIndex="6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Special Instructions
                                            </label>
                                            <div class="col-md-9">
                                                <asp:TextBox CssClass="form-control " ID="txtSpecInstruction" placeholder="Special Instruction"
                                                    TabIndex="7" runat="server" TextMode="SingleLine" Rows="1" MsgObrigatorio="Special Instruction"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Type</label>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlType" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Type" TabIndex="8">
                                                            <asp:ListItem Selected="True" Value="0">Select Type</asp:ListItem>
                                                            <asp:ListItem Value="1">Package</asp:ListItem>
                                                            <asp:ListItem Value="2">Extra</asp:ListItem>
                                                            <asp:ListItem Value="3">Retain Sample</asp:ListItem>
                                                            <asp:ListItem Value="4">FILTRATION</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm ">
                                                    <font color="red">*</font> Barrels</label>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtBarrels" placeholder="0"
                                                            Text="0" TabIndex="9" runat="server" MsgObrigatorio="Barrels"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtBarrels"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm ">
                                                    <font color="red">*</font> Quantity</label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtQty" placeholder="0.000"
                                                            TabIndex="10" runat="server" MsgObrigatorio="Quantity"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    <font color="red">*</font> Unit</label>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlUnit" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Type" TabIndex="11">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <!--/span-->
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <label class="control-label label-sm">
                                                    &nbsp</label>
                                                <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" OnClick="btnInsert_Click"
                                                    TabIndex="12" runat="server" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="overflow: auto; width: 100%">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgvFillDetail" runat="server" TabIndex="13" Style="width: 100%;"
                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        CellPadding="4" GridLines="Both" OnRowDeleting="dgvFillDetail_Deleting" OnRowCommand="dgvFillDetail_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkModify" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                        CausesValidation="False" CommandName="Modify" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                        CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                        CommandName="Delete" Text="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" SortExpression="FOSD_TYPE" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFOSD_TYPE" runat="server" Text='<%# Bind("FOSD_TYPE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType_Name" runat="server" Text='<%# Bind("Type_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Barrels" SortExpression="FOSD_QTY" HeaderStyle-HorizontalAlign="Right"
                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFOSD_QTY" runat="server" Text='<%# Eval("FOSD_QTY") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty" SortExpression="FOSD_WGT" HeaderStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFOSD_WGT" runat="server" Text='<%# Eval("FOSD_WGT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit Code" SortExpression="UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUOM_CODE" runat="server" Text='<%# Eval("UOM_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit" SortExpression="UOM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUOM_NAME" runat="server" Text='<%# Eval("UOM_NAME") %>'></asp:Label>
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
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Filled By
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtFillby" placeholder="Filled By" TabIndex="14"
                                                    runat="server" MaxLength="100" MsgObrigatorio="Filled By"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label text-right ">
                                                <font color="red">*</font> Fill Date
                                            </label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFillDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                        ValidationGroup="Save" TabIndex="15" MsgObrigatorio="Fill Date" ReadOnly="false"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                        TargetControlID="txtFillDate" PopupButtonID="txtFillDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Final Yield
                                            </label>
                                            <div class="col-md-9">
                                                <asp:TextBox CssClass="form-control text-right" ID="txtFinalyield" placeholder="0.00"
                                                    TabIndex="16" runat="server" MsgObrigatorio="Final Yield"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtFinalyield"
                                                    ValidChars="0123456789." runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Notes
                                            </label>
                                            <div class="col-md-9">
                                                <asp:TextBox CssClass="form-control " ID="txtNotes" placeholder="Notes" TabIndex="17"
                                                    runat="server" Rows="1" MsgObrigatorio="Notes"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Filter Used
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtFilterUsed" placeholder="Filter Used"
                                                    TabIndex="18" runat="server" MaxLength="50" MsgObrigatorio="Filter Used"></asp:TextBox>
                                            </div>
                                            <label class="col-md-2 control-label label-sm">
                                                Filter Unit
                                            </label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlFilterUnit" CssClass="select2_category form-control" runat="server"
                                                    MsgObrigatorio="Filter Unit" TabIndex="19">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="20" runat="server"
                                        OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="21" OnClick="btnCancel_Click"
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
                            <%-- </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {

                if (VerificaValorCombo('#<%=ddlbatchNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtFillDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaValorCombo('#<%=ddlType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtBarrels.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtQty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaValorCombo('#<%=ddlUnit.ClientID%>', '#Avisos') == false) {
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

</asp:Content>
