<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="MaterialInspection.aspx.cs"
    Inherits="Transactions_ADD_MaterialInspection" Title="Material Inspection " %>

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
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Material Inspection
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
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <asp:GridView ID="dgInspection" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                            CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="IWM_CODE"
                                                            OnPageIndexChanging="dgInspection_PageIndexChanging" OnRowDeleting="dgInspection_RowDeleting"
                                                            OnRowDataBound="dgPMPending_RowDataBound" OnRowCommand="dgInspection_RowCommand"
                                                            AllowPaging="true" PageSize="15">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="View" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAdd" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Add</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" CssClass="btn red btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Delete" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Hold" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkHold" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="HOLD" Text="Hold" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="REASON" SortExpression="PM_REASON_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="ddlReason" CssClass="form-control select2" AppendDataBoundItems="true"
                                                                                    Width="150px" runat="server" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IWM_CODE" SortExpression="IWM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_CODE" runat="server" Text='<%# Bind("IWM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Inward NO." SortExpression="IWM_NO" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_NO" CssClass="" runat="server" Text='<%# Eval("IWM_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Inward Date" SortExpression="IWM_DATE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_DATE" CssClass="" runat="server" Text='<%# Eval("IWM_DATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Inspection No" SortExpression="IWD_INSP_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_INSP_NO" CssClass="" runat="server" Text='<%# Eval("IWD_INSP_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PO Number" SortExpression="SPOM_PO_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSPOM_PO_NO" CssClass="" runat="server" Text='<%# Eval("SPOM_PO_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="IWD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_I_CODE" CssClass="" runat="server" Text='<%# Eval("IWD_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
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
                                                                <asp:TemplateField HeaderText="Item Rate" SortExpression="IWD_RATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWD_RATE" CssClass="" runat="server" Text='<%# Eval("IWD_RATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IWM_P_CODE" SortExpression="IWM_P_CODE" Visible="false"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWM_P_CODE" CssClass="" runat="server" Text='<%# Eval("IWM_P_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
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
                                                                <%--<cc1:CalendarExtender ID="txtInspDate_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtInspDate" PopupButtonID="txtInspDate">
                                                                </cc1:CalendarExtender>--%>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:Label ID="lblpartycode" runat="server" Visible="false"></asp:Label></div>
                                                        <asp:Label ID="lblGRNCODE" Visible="false" runat="server"></asp:Label></div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 text-right">
                                                            Inward No.
                                                        </label>
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lblIwdNo" runat="server" Visible="true"></asp:Label>
                                                        </div>
                                                        <label class="col-md-2 text-right ">
                                                            <font color="red">*</font> Inward Date</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:Label ID="lblIwddate" Visible="true" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
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
                                                        <div class="col-md-2">
                                                            <label class="control-label label-sm">
                                                                Rate</label>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control text-right" ValidationGroup="Save"
                                                                TabIndex="5" ReadOnly="true"></asp:TextBox>
                                                        </div>
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
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label text-right">
                                                            <font color="red"></font>Plant</label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:RadioButtonList ID="rbtWithAmt" runat="server" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="rbtWithAmt_SelectedIndexChanged" Enabled="false"
                                                                        RepeatDirection="Horizontal" CssClass="checker">
                                                                        <asp:ListItem Value="-2147483648" Selected="True">&nbsp; Plant 1&nbsp;&nbsp;</asp:ListItem>
                                                                        <asp:ListItem Value="-2147483647">&nbsp;Plant 2</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label text-right">
                                                            <font color="red"></font>Store Name</label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlStoreType" runat="server" TabIndex="1" CssClass="form-control"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlStoreType_SelectedIndexChanged"
                                                                        MsgObrigatorio=" Store Name">
                                                                    </asp:DropDownList>
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
                                                        <label class=" col-md-2 text-right label-sm">
                                                            PDR Check</label>
                                                        <div class="col-md-2">
                                                            <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkPDR" runat="server" CssClass="checker" AutoPostBack="true" OnCheckedChanged="chkPDR_CheckedChanged" />
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
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label label-sm">
                                                            PDIR
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:FileUpload ID="FileUpload1" TabIndex="12" ClientIDMode="Static" onchange="this.form.submit()"
                                                                runat="server" />
                                                            <asp:Button ID="Button2" Text="Upload" runat="server" OnClick="Upload2" Style="display: none" />
                                                            <asp:LinkButton ID="lnkTModel" runat="server" Text="" OnClick="lnkuploadTModel_Click"> </asp:LinkButton>
                                                        </div>
                                                        <label class="col-md-3 text-right label-sm">
                                                            <div class="col-md-3">
                                                            </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                            UseSubmitBehavior="false" CssClass="btn green" TabIndex="12" runat="server" Text="Save"
                                            OnClick="btnSave_Click" />
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
                            <asp:UpdatePanel runat="server" ID="UpdatePanel39">
                                <ContentTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopupExtenderDovView" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                        PopupControlID="PanelDoc" TargetControlID="LinkButton1">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="PanelDoc" runat="server" Style="display: none;">
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
                                                                <iframe runat="server" id="myframe" width="900px" height="600px"></iframe>
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
