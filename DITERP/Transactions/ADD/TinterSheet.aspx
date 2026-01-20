<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="TinterSheet.aspx.cs"
    Inherits="Transactions_ADD_TinterSheet" Title="Tinter Sheet" %>

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
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tinter Sheet
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
                                                    <font color="red">*</font> Batch No</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlBatchNo" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="1" AutoPostBack="True" Visible="True" MsgObrigatorio="Batch No" OnSelectedIndexChanged="ddlBatchNo_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Value="0">Batch No</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Formula</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFormula" runat="server" placeholder="Formula" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Formula" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class=" col-md-1 control-label text-right ">
                                                    <font color="red">*</font> Date</label>
                                                <div class="col-md-3 ">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="3" MsgObrigatorio="Batch Date" ReadOnly="false"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtDate" PopupButtonID="txtDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label text-right ">
                                                    <font color="red">*</font> Customer</label>
                                                <div class="col-md-5 ">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                                TabIndex="6" Visible="True" MsgObrigatorio="Select Customer" Enabled="false">
                                                                <asp:ListItem Selected="True" Value="0">Select Customer</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Liters</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtLiters" runat="server" placeholder="0.000" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="4" MsgObrigatorio="Liters" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtLiters"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    Tinter Name</label>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtTinerName" runat="server" placeholder="Tinter Name" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="7" MsgObrigatorio="Tinter Name" MaxLength="200"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    KG</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtKg" runat="server" placeholder="0.000" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="5" MsgObrigatorio="Kg" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtKg"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--Initial Color--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Initial Color
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    DL
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDL" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="8" MsgObrigatorio="DL" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtDL"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Da
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDa" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="9" MsgObrigatorio="Da" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtDa"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Db
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDb" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="10" MsgObrigatorio="Db" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtDb"
                                                                ValidChars="0123456789." runat="server" />
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
                                                    Initial Gloss
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInitialGloss" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="11" MsgObrigatorio="Initial gloss" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtInitialGloss"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--Initial Viscosity--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Initial Viscosity
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInitialViscosity" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="12" MsgObrigatorio="Viscosity" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtInitialViscosity"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Cup
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtCup" runat="server" placeholder="Cup" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="13" MsgObrigatorio="Cup" MaxLength="50"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Initial S.G.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInitialSG" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="14" MsgObrigatorio="0.00" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtInitialSG"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row" style="overflow-x: auto;">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel26">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgMainShade" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        AutoGenerateColumns="False" CellPadding="4" TabIndex="16" ShowHeaderWhenEmpty="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Process Code" SortExpression="TSD_PROCESS_CODE" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSD_PROCESS_CODE" runat="server" Text='<%# Bind("TSD_PROCESS_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Process Name" SortExpression="PROCESS_NAME" Visible="true"
                                                                ControlStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROCESS_NAME" runat="server" Text='<%# Bind("PROCESS_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Iterm Code" SortExpression="I_CODE" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_Code" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" Visible="true"
                                                                ControlStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lot" SortExpression="LOT" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLot" runat="server" CssClass="form-control text-right" placeholder=""
                                                                        MsgObrigatorio="LOT" Text='<%# Eval("TSD_LOT") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty (Kg/Lt/Gm)" SortExpression="QTY" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQTY" runat="server" CssClass="form-control text-right input-sm"
                                                                        placeholder="0.00" MsgObrigatorio="Qty" Text='<%# Eval("TSD_QTY") %>' MaxLength="50"
                                                                        TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtQTY"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Add By" SortExpression="ADD_BY" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAddBy" runat="server" CssClass="form-control text-right" placeholder="Name"
                                                                        MsgObrigatorio="Add By" Text='<%# Eval("TSD_ADD_BY") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DL" SortExpression="DL" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtaddDL" runat="server" CssClass="form-control text-right" placeholder="0.00"
                                                                        MsgObrigatorio="DL" Text='<%# Eval("TSD_DL") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtaddDL"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DA" SortExpression="DA" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtaddDA" runat="server" CssClass="form-control text-right" placeholder="0.00"
                                                                        MsgObrigatorio="DA" Text='<%# Eval("TSD_DA") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtaddDA"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DB" SortExpression="DB" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtaddDB" runat="server" CssClass="form-control text-right" placeholder="0.00"
                                                                        MsgObrigatorio="DB" Text='<%# Eval("TSD_DB") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtaddDB"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GLOSS" SortExpression="GLOSS" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtaddGLOSS" runat="server" CssClass="form-control text-right" placeholder="0.00"
                                                                        MsgObrigatorio="GLOSS" Text='<%# Eval("TSD_GLOSS") %>' MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txtaddGLOSS"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VISCOSITY" SortExpression="DL" HeaderStyle-HorizontalAlign="Right"
                                                                ControlStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtaddVISCOSITY" runat="server" CssClass="form-control text-right"
                                                                        placeholder="0.00" MsgObrigatorio="VISCOSITY" Text='<%# Eval("TSD_VISCOSITY") %>'
                                                                        MaxLength="50" TabIndex="17"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtaddVISCOSITY"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="control-label text-right" />
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
                                    <br />
                                    <%--Final Color--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Final Color
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    DL
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFDL" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="18" MsgObrigatorio="DL" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtFDL"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Da
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFDa" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="19" MsgObrigatorio="Da" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtFDa"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    Db
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFDB" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="20" MsgObrigatorio="Db" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtFDB"
                                                                ValidChars="0123456789." runat="server" />
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
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    DE
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFDE" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="21" MsgObrigatorio="DE" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtFDE"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--Final Viscosity--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Final Gloss
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFinalGloss" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="22" MsgObrigatorio="Final gloss" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtFinalGloss"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Final Viscosity
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFinalVisxosity" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="23" MsgObrigatorio="Viscosity" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtFinalVisxosity"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Final S.G
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtFinalSG" runat="server" placeholder="0.00" CssClass="form-control text-right"
                                                                ValidationGroup="Save" TabIndex="24" MsgObrigatorio="Final S.G" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtFinalSG"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--CheckEd by--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Checked By
                                                </label>
                                                <div class="col-md-6">
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtCheckedBy" runat="server" placeholder="Checked By" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="25" MsgObrigatorio="Checked By" MaxLength="50"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--Approved By--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    Apporved By
                                                </label>
                                                <div class="col-md-6">
                                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtApporvedBy" runat="server" placeholder="Apporved By" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="26" MsgObrigatorio="Apporved By" MaxLength="50"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="27" runat="server"
                                        OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="28" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js
    before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip-->

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

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
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlBatchNo.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; }
                else if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; }
                else if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; }
                else {
                    $("#Avisos").fadeOut(6000);
                    return true;
                }
            } catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        } </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
