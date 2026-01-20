<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="TurningInward.aspx.cs"
    Inherits="Transactions_ADD_Turning_Inward" Title="Turning Inward" %>

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
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlSupplier.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();

        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Turning Inward
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="18" runat="server" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> Sub Contractor Name</label>
                                                <div class="col-md-8">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplier" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"
                                                                MsgObrigatorio="Sub Contracter Name">
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
                                                <label class="col-md-3 control-label text-right">
                                                    GRN No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGRNno" placeholder="0"
                                                                TabIndex="3" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtGRNno"
                                                                FilterType="Numbers" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> GRN Date.</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtGRNDate" runat="server" CssClass="form-control input-sm" placeholder=""
                                                            TabIndex="4" msgObrigatorio="Please Select GRN Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtGRNDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtGRNDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    Challan No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control text-right input-sm"
                                                                placeholder="0" TabIndex="5" AutoPostBack="true" MaxLength="100" MsgObrigatorio=" Enter Challan No"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Challan Date</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtChallanDate"
                                                            TabIndex="6" runat="server" MsgObrigatorio=" Select Challan Date" TextMode="SingleLine"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtChallanDate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font> Item Code
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="11" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font> Item Name
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-3">
                                                    <label class=" control-label label-sm">
                                                        Pending Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingQty" placeholder="0.000"
                                                                Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtPendingQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class=" control-label label-sm">
                                                        <font color="red">*</font>Recd Qty
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRecdQty" placeholder="0.000"
                                                                TabIndex="16" runat="server" AutoPostBack="true" MsgObrigatorio="Please Enter Recd Qty"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtRecdQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class=" control-label label-sm">
                                                        Unit
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUom" CssClass="select2_category form-control" runat="server"
                                                                Enabled="false" TabIndex="17" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-1">
                                                    <label class=" control-label label-sm">
                                                    </label>
                                                    <br />
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnInsert" CssClass="btn blue" TabIndex="16" runat="server" OnClick="BtnInsert_Click">Insert</asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12" style="overflow: auto;">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgInwardMaster" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                                PageSize="15" OnRowCommand="dgInwardMaster_RowCommand" OnRowDeleting="dgInwardMaster_Deleting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSelect" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                                CausesValidation="False" CommandName="Select" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                                CausesValidation="False" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                Text="Delete"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="SD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSD_I_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("SD_I_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_Name" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Recd. Qty" SortExpression="SD_REV_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSD_REV_QTY" CssClass="" runat="server" Text='<%# Eval("SD_REV_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit" SortExpression="I_UOM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_UOM_NAME" CssClass="" runat="server" Text='<%# Eval("I_UOM_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="UOMCODE" SortExpression="UOM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSD_UOM_CODE" CssClass="" runat="server" Text='<%# Eval("SD_UOM_CODE") %>'></asp:Label>
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
                                    <hr />
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
                                            <div class="form-actions fluid">
                                                <div class="col-md-offset-4 col-md-9">
                                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Save"
                                                        OnClick="btnSubmit_Click" />
                                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="18" runat="server"
                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                                </div>
                                            </div>
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

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlSupplier.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtGRNDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtChallanDate.ClientID%>', '#Avisos') == false) {
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
