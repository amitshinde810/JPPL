<%@ Page Title="Customer Rejection" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="IRNCustomerRejection.aspx.cs" Inherits="IRN_ADD_IRNCustomerRejection" %>

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

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlUOM.ClientID %>").select2();
            jQuery("#<%=ddlParty.ClientID %>").select2();
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlReason.ClientID %>").select2();
        });
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
                                <i class="fa fa-reorder"></i>Customer Rejection
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
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    GIN No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGINO" placeholder="0"
                                                                TabIndex="3" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtGINO"
                                                                FilterType="Numbers" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> GIN Date</label>
                                                <div class="col-md-2">
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
                                                <label class="col-md-2 control-label text-right">
                                                    Rej. No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGRNno" placeholder="0"
                                                                TabIndex="3" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtGRNno"
                                                                FilterType="Numbers" runat="server" />--%>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label text-right">
                                                    <font color="red">*</font> Rej. Date</label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDocdate" runat="server" CssClass="form-control input-sm" placeholder=""
                                                            TabIndex="4" msgObrigatorio="Please Select Doc. Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtDocdate">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    <span class="required">*</span> Customer Name
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlParty" runat="server" OnSelectedIndexChanged="ddlParty_SelectedIndexChanged"
                                                                TabIndex="1" CssClass="select2" Width="100%" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <span class="required">*</span> Complaint No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtComplaintNo" runat="server" placeholder="Complaint No." TabIndex="1"
                                                                CssClass="form-control" AutoPostBack="True">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span>Item Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Item Name
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> UOM
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanelf5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlUOM" runat="server" TabIndex="1" CssClass="select2" Width="100%"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Type
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" runat="server" Columns="1" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                                                Width="100%" Rows="1" TabIndex="6" CssClass="select2" AutoPostBack="True">
                                                                <asp:ListItem Value="0" Text="New"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Repetitive"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Qty
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" Enabled="false" ID="txtQTy"
                                                                placeholder="0.000" runat="server" MaxLength="15" onkeypress="return validateFloatKeyPress(this,event);"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtQTy"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <%--<asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />--%>
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Rej. Qty
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRejQty" placeholder="0.000"
                                                                runat="server" MaxLength="15" onkeypress="return validateFloatKeyPress(this,event);"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtRejQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkIsRequired" Text="&nbsp8D Required" runat="server" CssClass="checker"
                                                                AutoPostBack="True" TabIndex="25" Checked="true" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkDocument" Text="&nbspDocument" runat="server" CssClass="checker"
                                                                AutoPostBack="True" TabIndex="25" Checked="true" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkEvidence" Text="&nbspEvidence" runat="server" CssClass="checker"
                                                                AutoPostBack="True" TabIndex="25" Checked="true" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label class="col-md-2 control-label text-right">
                                                    <span class="required"></span>Defect Observed
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtReason" runat="server" placeholder="Defect Observed" TextMode="MultiLine"
                                                                Columns="1" Rows="3" TabIndex="1" CssClass="form-control" AutoPostBack="True">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <label class="col-md-2 control-label text-right" runat="server" visible="false" id="lblreason">
                                                            <span class="required"></span>Defect Observed
                                                        </label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlReason" Visible="false" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <%--<asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                                    <asp:LinkButton ID="BtnInsert" CssClass="btn blue" TabIndex="16" runat="server" OnClick="BtnInsert_Click">Insert</asp:LinkButton>
                                                    <%-- </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto;">
                                                <%--<asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                    <ContentTemplate>--%>
                                                <asp:GridView ID="dgIRN" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CellPadding="4" Font-Size="12px" ShowFooter="true" Font-Names="Verdana" GridLines="Both"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" OnRowCommand="dgIRN_RowCommand"
                                                    OnRowDeleting="dgIRN_Deleting" OnRowUpdating="dgIRN_RowUpdating">
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
                                                        <asp:TemplateField HeaderText="CRD_CRM_CODE" SortExpression="CRD_CRM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_CRM_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_CRM_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CRD_I_CODE" SortExpression="CRD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_I_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_I_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="True">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Visible="True" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="True">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_NAME" CssClass="" runat="server" Visible="True" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Unit" SortExpression="CRD_UOM" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_UOM" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_UOM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Unit" SortExpression="I_UOM_NAME" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="True">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_UOM_NAME" CssClass="" runat="server" Visible="True" Text='<%# Eval("I_UOM_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty" SortExpression="CRD_QTY" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_QTY" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_QTY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblCRD_QTYGrandTotal" runat="server" CssClass="text-center bold"></asp:Label>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rej. Qty" SortExpression="CRD_REJ_QTY" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="True">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_REJ_QTY" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_REJ_QTY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblCRD_REJ_QTYGrandTotal" runat="server" CssClass="text-center bold"></asp:Label>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" SortExpression="CRD_TYPE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_TYPE" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_TYPE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" SortExpression="CRD_TYPE1" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_TYPE1" CssClass="" runat="server" Visible="false" Text='<%# Eval("CRD_TYPE1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Defect Observed" SortExpression="CRD_REASON" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="True">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_REASON" CssClass="" runat="server" Visible="True" Text='<%# Eval("CRD_REASON") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Defect Observed" SortExpression="CRD_DEF_OBSERVED"
                                                            HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCRD_DEF_OBSERVED" Width="300px" runat="server" CssClass="form-control"
                                                                    placeholder="" Text='<%# Eval("CRD_DEF_OBSERVED") %>' TextMode="MultiLine" TabIndex="16"
                                                                    AutoPostBack="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Route Cause" SortExpression="CRD_ROUTE_CAUSE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCRD_ROUTE_CAUSE" Width="300px" TextMode="MultiLine" runat="server"
                                                                    CssClass="form-control" placeholder="" Text='<%# Eval("CRD_ROUTE_CAUSE") %>'
                                                                    TabIndex="16" AutoPostBack="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" SortExpression="CRD_ACTION_TAKEN" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCRD_ACTION_TAKEN" TextMode="MultiLine" runat="server" Width="300px"
                                                                    CssClass="form-control" placeholder="" Text='<%# Eval("CRD_ACTION_TAKEN") %>'
                                                                    TabIndex="16" AutoPostBack="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Browse" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:FileUpload ID="imgUpload" runat="server" />
                                                                <%-- <asp:Button ID="btnAsyncUpload" runat="server" Text="Async_Upload" OnClick="Async_Upload_File" />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Upload" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnupload" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                                    CommandName="Update" Text="Upload" CssClass="btn green btn-xs" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="8D View" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CssClass="btn green btn-xs"   Enabled='<%#Eval("CRD_IS_REQUIRED").ToString()== "YES" ? true : false %>' 
                                                                    CausesValidation="False" CommandName="View" Text="View" CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfilename" CssClass="" runat="server" Text='<%# Bind("CRD_FILE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="8D Required" SortExpression="CRD_IS_REQUIRED" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_IS_REQUIRED" CssClass="label-sm" runat="server" Text='<%# Bind("CRD_IS_REQUIRED") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      
                                                        <asp:TemplateField HeaderText="Browse" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:FileUpload ID="DocumentUpload" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Upload" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnDocumentupload" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                                    CommandName="Update" Text="Upload" CssClass="btn green btn-xs" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document View" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDocumentView" BorderStyle="None" runat="server" CssClass="btn green btn-xs"  Enabled='<%#Eval("CRD_DOCUMENT_REQUIRED").ToString()== "YES" ? true : false %>' 
                                                                    CausesValidation="False" CommandName="ViewDocument" Text="View" CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_DOCUMENT" CssClass="" runat="server" Text='<%# Bind("CRD_DOCUMENT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document" SortExpression="CRD_DOCUMENT" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_DOCUMENT_REQUIRED" CssClass="label-sm" runat="server" Text='<%# Bind("CRD_DOCUMENT_REQUIRED") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        
                                                          <asp:TemplateField HeaderText="Browse" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:FileUpload ID="EvidenceUpload" runat="server" />
                                                                <%-- <asp:Button ID="btnAsyncUpload" runat="server" Text="Async_Upload" OnClick="Async_Upload_File" />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Upload" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnEvidenceupload" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                                    CommandName="Update" Text="Upload" CssClass="btn green btn-xs" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Evidence View" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEvidenceView" BorderStyle="None" runat="server" CssClass="btn green btn-xs"  Enabled='<%#Eval("CRD_EVIDENCE_REQUIRED").ToString()== "YES" ? true : false %>' 
                                                                    CausesValidation="False" CommandName="ViewEvidence" Text="View" CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_EVIDENCE" CssClass="" runat="server" Text='<%# Bind("CRD_EVIDENCE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Evidence" SortExpression="CRD_EVIDENCE" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRD_EVIDENCE_REQUIRED" CssClass="label-sm" runat="server" Text='<%# Bind("CRD_EVIDENCE_REQUIRED") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <PagerStyle CssClass="pgr" />
                                                </asp:GridView>
                                                <%--</ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="3" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="4" runat="server"
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
                                                        Do you Want to Cancel record?
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

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
