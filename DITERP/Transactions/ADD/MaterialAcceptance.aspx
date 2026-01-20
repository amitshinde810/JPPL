<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="MaterialAcceptance.aspx.cs"
    Inherits="Transactions_ADD_MaterialAcceptance" Title="Material Acceptance " %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                <i class="fa fa-reorder"></i>Material Acceptance
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <asp:Panel ID="panelInspection" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <asp:Label runat="server" ID="lblType"></asp:Label></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-1 control-label">
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkMaterialAcceptance" runat="server" TabIndex="2" CssClass="checker"
                                                                    AutoPostBack="True" Text="" Checked="false" Visible="false" OnCheckedChanged="chkMaterialAcceptance_CheckedChanged" />
                                                                <label class="control-label label-sm">
                                                                </label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgMaterialAcceptance" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="IM_CODE"
                                                                OnPageIndexChanging="dgMaterialAcceptance_PageIndexChanging" OnRowDeleting="dgMaterialAcceptance_RowDeleting"
                                                                OnRowCommand="dgMaterialAcceptance_RowCommand"    >
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="View" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="View" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Modify" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Add" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAdd" Visible="false" CssClass="btn green btn-xs" BorderStyle="None"
                                                                                runat="server" CausesValidation="False" CommandName="Modify" Text="Add" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" CssClass="btn red btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="Delete" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IM_CODE" SortExpression="IM_CODE" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIM_CODE" runat="server" Text='<%# Bind("IM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODE" CssClass="" runat="server" Text='<%# Eval("I_CODE") %>'></asp:Label>
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
                                                                            <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document No." SortExpression="IM_NO" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIM_NO" CssClass="" runat="server" Text='<%# Eval("IM_NO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document Date" SortExpression="IM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIM_DATE" CssClass="" runat="server" Text='<%# Eval("IM_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="From Store Code" SortExpression="FROM_STORE_CODE"
                                                                        HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFROM_STORE_CODE" CssClass="" runat="server" Text='<%# Eval("FROM_STORE_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="From Store" SortExpression="FROM_STORE_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFROM_STORE_NAME" CssClass="" runat="server" Text='<%# Eval("FROM_STORE_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Store Code" SortExpression="TO_STORE_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTO_STORE_CODE" CssClass="" runat="server" Text='<%# Eval("TO_STORE_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Store" SortExpression="TO_STORE_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTO_STORE_NAME" CssClass="" runat="server" Text='<%# Eval("TO_STORE_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Transferd qty." SortExpression="IMD_ISSUE_QTY" Visible="true"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIMD_ISSUE_QTY" CssClass="" runat="server" Text='<%# Eval("IMD_ISSUE_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Skip" SortExpression="IMD_ISSUE_QTY" Visible="true"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox   ID="lblchk" CssClass="checker" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Received qty." SortExpression="OK_Qty" Visible="true"
                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblOK_Qty" CssClass="text-right" runat="server" Text='<%# Eval("OK_Qty") %>'
                                                                                OnTextChanged="lblOK_Qty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" TargetControlID="lblOK_Qty"
                                                                                ValidChars="0123456789." runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass=" Control-text text-right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Short qty." SortExpression="Rej_Qty" Visible="true"
                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblRej_Qty" CssClass="text-right" runat="server" Text='<%# Eval("Rej_Qty") %>'
                                                                                OnTextChanged="lblRej_Qty_TextChanged" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="lblRej_Qty"
                                                                                ValidChars="0123456789." runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" CssClass=" Control-text text-right" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelDetail" runat="server" Visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        Insp. No.
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtInspNo" runat="server" CssClass="form-control" ValidationGroup="Save"
                                                            TabIndex="1" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Inspection Date</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtInspDate" runat="server" CssClass="form-control" placeholder="dd MMM yyyy"
                                                                ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Inspection Date"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtInspDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtInspDate" PopupButtonID="txtInspDate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Label ID="lblpartycode" runat="server" Visible="false"></asp:Label></div>
                                                    <asp:Label ID="lblGRNCODE" runat="server"></asp:Label></div>
                                            </div>
                                        </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-1 control-label text-right">
                                                PO Number</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control" Text="0" ValidationGroup="Save"
                                                    TabIndex="3" MsgObrigatorio="PO Number" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <label class="col-md-1 control-label text-right">
                                                Item Code</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control" Text="" ValidationGroup="Save"
                                                    TabIndex="4" MsgObrigatorio="Item Code" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <label class="col-md-1 control-label text-right">
                                                Item Name</label>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" Text="" ValidationGroup="Save"
                                                    TabIndex="4" MsgObrigatorio="Item Name" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                <label class="control-label label-sm">
                                                    Rate</label>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="form-control text-right" ValidationGroup="Save"
                                                    TabIndex="5" ReadOnly="true"></asp:TextBox>
                                            </label>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Recd. Qty</label>
                                                <asp:TextBox ID="txtrecQty" runat="server" CssClass="form-control text-right" ValidationGroup="Save"
                                                    TabIndex="5" MsgObrigatorio="Please Enter Received Qty" ReadOnly="true"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtrecQty"
                                                    ValidChars="0123456789." runat="server" />
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Unit</label>
                                                <asp:TextBox ID="txtUnitName" runat="server" CssClass="form-control" Text="" ValidationGroup="Save"
                                                    TabIndex="6" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    OK Qty.</label>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtOkQty" runat="server" CssClass="form-control text-right" Text=""
                                                            ValidationGroup="Save" TabIndex="7" MsgObrigatorio="Please Enter Ok Quantity"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtOkQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtRejQty" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtScrapQty" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Rej. Qty.</label>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtRejQty" runat="server" CssClass="form-control text-right" Text=""
                                                            ValidationGroup="Save" TabIndex="8" AutoPostBack="True" OnTextChanged="txtRejQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtRejQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Scrap Qty.</label>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtScrapQty" runat="server" CssClass="form-control text-right" Text=""
                                                            ValidationGroup="Save" TabIndex="9" MsgObrigatorio="Please Enter Scrap Quantity"
                                                            AutoPostBack="True" OnTextChanged="txtScrapQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtScrapQty"
                                                            ValidChars="0123456789." runat="server" />
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
                                            <label class="col-md-2 control-label label-sm">
                                                <font color="red">*</font> Reason</label>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Text="" ValidationGroup="Save"
                                                    TabIndex="10" MsgObrigatorio="Resaon"></asp:TextBox>
                                            </div>
                                            <label class=" col-md-2 control-label label-sm">
                                                PDR Check</label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkPDR" runat="server" AutoPostBack="true" OnCheckedChanged="chkPDR_CheckedChanged" />
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
                                                Remark/TC No.
                                            </label>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Text="" ValidationGroup="Save"
                                                    TabIndex="11"></asp:TextBox>
                                            </div>
                                            <label class=" col-md-2 control-label label-sm">
                                                PDR No.</label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtPDRNo" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkPDR" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="form-actions fluid">
                            <div class="col-md-offset-4 col-md-9">
                                   <asp:Button ID="btnSubmit"  OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';" UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Save"  OnClick="btnSave_Click"/>                                   
                                <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="13" runat="server"
                                    OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                var value = document.getElementById("<%= txtRejQty.ClientID %>").text;
                if (parseInt(value) > 0) {
                    if (VerificaObrigatorio('#<%=txtReason.ClientID%>', '#Avisos') == false) {
                        $("#Avisos").fadeOut(6000);
                        return false;
                    }
                }
                if (VerificaObrigatorio('#<%=txtInspDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtPONumber.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtItemName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtrecQty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtOkQty.ClientID%>', '#Avisos') == false) {
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
