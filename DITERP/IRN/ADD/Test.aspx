<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Test.aspx.cs"
    Inherits="IRN_ADD_Test" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
     

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
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlType.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlUOM.ClientID %>").select2();
            jQuery("#<%=ddlDefect.ClientID %>").select2();

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
                                <i class="fa fa-reorder"></i>Production Entry
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
                                                <label class="col-md-3 control-label text-right">
                                                    Prod No.</label>
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
                                                    <font color="red">*</font> Prod. Date.</label>
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
                                    <br />
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div id="Div1" class="col-md-1" runat="server" visible="false">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlStage" Visible="false" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Type
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel1d1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlType" runat="server" TabIndex="1" CssClass="select2" Width="100%"
                                                                AutoPostBack="True">
                                                                <asp:ListItem Value="1">CASTING</asp:ListItem>
                                                                <asp:ListItem Value="0">MECHINING</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Item Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
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
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> UOM
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanelf5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlUOM" runat="server" TabIndex="1" CssClass="select2" Width="100%"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanedl5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlDefect" Visible="false" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlStage" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Prod Qty
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtProdQty" placeholder="0.000"
                                                                runat="server" MaxLength="15" onkeypress="return validateFloatKeyPress(this,event);"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtProdQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRejQTy" placeholder="0.000"
                                                                runat="server" MaxLength="15" Visible="false" onkeypress="return validateFloatKeyPress(this,event);"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtRejQTy"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label text-right">
                                                    <span class="required">*</span> Rate
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" placeholder="0.000"
                                                                runat="server" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtRate"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgIRN" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="BtnInsert" CssClass="btn blue" TabIndex="16" runat="server" OnClick="BtnInsert_Click">Insert</asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" style="overflow: auto;">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgIRN" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" OnRowCommand="dgIRN_RowCommand"
                                                                OnRowDeleting="dgIRN_Deleting">
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
                                                                    <asp:TemplateField HeaderText="IRND_RSM_CODE" SortExpression="IRND_RSM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_RSM_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_RSM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Stage" SortExpression="RSM_NO" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRSM_NO" CssClass="" runat="server" Visible="True" Text='<%# Eval("RSM_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Type" SortExpression="IRND_TYPE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_TYPE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_TYPE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IRND_I_CODE" SortExpression="IRND_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_I_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_I_CODE") %>'></asp:Label>
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
                                                                    <asp:TemplateField HeaderText="Item Unit" SortExpression="IRND_UOM" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_UOM" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_UOM") %>'></asp:Label>
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
                                                                    <asp:TemplateField HeaderText="Defect" SortExpression="IRND_RM_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_RM_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_RM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Defect" SortExpression="RM_DEFECT" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRM_DEFECT" CssClass="" runat="server" Visible="True" Text='<%# Eval("RM_DEFECT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Prod. Qty" SortExpression="IRND_PROD_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="True">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_PROD_QTY" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_PROD_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rejection Qty" SortExpression="IRND_REJ_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_REJ_QTY" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_REJ_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Rate" SortExpression="IRND_RATE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIRND_RATE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IRND_RATE") %>'></asp:Label>
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
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <%--<asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="3" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return confirm('Are you sure to Save..!!');"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>--%>
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
