<%@ Page Title="Casting Conversion" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="RejStoreToFoundryCon.aspx.cs" Inherits="Transactions_RejStoreToFoundryCon" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlRejItem.ClientID %>").select2();
            jQuery("#<%=ddlConIntoItem.ClientID %>").select2();

        });
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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Casting Conversion
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <asp:Panel ID="MainPanel" runat="server">
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                         <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label label-sm">
                                                  Select Plant
                                                </label>
                                                <div class="col-md-3">
                                                        <asp:RadioButtonList ID="rbtWithAmt"   runat="server" AutoPostBack="True"
                                                                TabIndex="2" RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtWithAmt_SelectedIndexChanged">
                                                                <asp:ListItem Value="-2147483648" Selected="True">&nbsp; Plant 1&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="-2147483647" >&nbsp;Plant 2</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    
                                                </label>
                                                <div class="col-md-3">
                                                    
                                                </div>
                                            </div>
                                        </div>
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class=" col-md-2 control-label text-right ">
                                                        Doc No.</label>
                                                    <div class="col-md-3 ">
                                                        <asp:TextBox ID="txtdocNo" runat="server" placeholder="Doc No." CssClass="form-control"
                                                            ValidationGroup="Save" TabIndex="1" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red">*</font>Doc Date</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel runat="server" ID="uppnl">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtdocDate" runat="server" CssClass="form-control" placeholder="dd/MMM/yyyy"
                                                                        ValidationGroup="Save" TabIndex="2" AutoPostBack="true" OnTextChanged="txtGinDate_TextChanged"
                                                                        MsgObrigatorio="Please Select DOC Date"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="txtdocDate_CalendarExtender" runat="server" Enabled="True"
                                                                        Format="dd MMM yyyy" TargetControlID="txtdocDate" PopupButtonID="txtdocDate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
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
                                                    <label class="control-label col-md-2 label-sm" runat="server" visible="false">
                                                    </label>
                                                    <div class="col-md-2" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="dd/MMM/yyyy"
                                                                        ValidationGroup="Save" TabIndex="2" AutoPostBack="true" OnTextChanged="txtDate_TextChanged"
                                                                        MsgObrigatorio="Please Select Date"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                        TargetControlID="txtDate" PopupButtonID="txtDate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="control-label col-md-2 label-sm">
                                                        <font color="red">*</font>Item Code</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlConIntoItem" CssClass="select2" Width="100%" runat="server"
                                                                    MsgObrigatorio="Item Name" TabIndex="9" AutoPostBack="True" OnSelectedIndexChanged="ddlConIntoItem_SelectedIndexChanged">
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
                                                    <div class="col-md-1">
                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="horizontal-form">
                                        <div class="form-body">
                                            <!--/row-->
                                            <div class="row" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <div class="col-md-1">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label class="control-label label-sm">
                                                                <font color="red">*</font> Rejected Item</label>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlRejItem" CssClass="select2" Width="100%" runat="server"
                                                                        MsgObrigatorio="Rejected Item" TabIndex="8" AutoPostBack="True" OnSelectedIndexChanged="ddlRejItem_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label class="control-label label-sm">
                                                                Stock Qty</label>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox runat="server" ReadOnly="true" ID="txtstock" Enabled="false" CssClass="form-control"
                                                                        placeholder="Stock Qty"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <!--/span-->
                                                </div>
                                            </div>
                                            <div class="row" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <div class="col-md-1">
                                                    </div>
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Rej Qty</label>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox runat="server" ID="txtStockRejQty" CssClass="form-control" placeholder="Stock Rej Qty"
                                                                        AutoPostBack="true" MaxLength="15" OnTextChanged="txtStockRejQty_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtStockRejQty"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <!--/span-->
                                                    <label class="control-label label-sm">
                                                        <font color="red">*</font> Standard Weight</label>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox runat="server" ReadOnly="true" ID="txtStandWeight" Enabled="true" CssClass="form-control"
                                                                                placeholder="Standard Weight"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <label class="control-label label-sm">
                                                        <font color="red"></font>
                                                    </label>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" OnClick="btnInsert_Click"
                                                                                TabIndex="13" runat="server" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="col-md-1">
                                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" CssClass="checker"
                                                                        Text="&nbspAll" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="overflow: auto; width: 100%">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgvProductionStoreDetails" runat="server" Visible="false" TabIndex="14"
                                                                Style="width: 100%;" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                CellPadding="4" GridLines="Both" OnRowDeleting="dgvProductionStoreDetails_RowDeleting"
                                                                OnSelectedIndexChanged="dgvProductionStoreDetails_SelectedIndexChanged" OnRowCommand="dgvProductionStoreDetails_RowCommand">
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
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="RTFD_REJ_ITEMCODE" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_REJ_ITEMCODE" runat="server" Text='<%# Bind("RTFD_REJ_ITEMCODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" Visible="false" SortExpression="RTFD_CON_ITEMCODE">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_CON_ITEMCODE" runat="server" Text='<%# Bind("RTFD_CON_ITEMCODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rej Item Name" SortExpression="RTFD_REJ_ITEMNAME"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_REJ_ITEMNAME" runat="server" Text='<%# Eval("RTFD_REJ_ITEMNAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-left" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Converted Item Name" SortExpression="RTFD_CON_ITEMNAME"
                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_CON_ITEMNAME" runat="server" Text='<%# Eval("RTFD_CON_ITEMNAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rej. Qty" SortExpression="RTFD_STK_REJ_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_STK_REJ_QTY" runat="server" Text='<%# Eval("RTFD_STK_REJ_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Standerd weight" SortExpression="RTFD_STAND_WEIGHT">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_STAND_WEIGHT" runat="server" Text='<%# Bind("RTFD_STAND_WEIGHT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Con. Qty" SortExpression="RTFD_CON_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRTFD_CON_QTY" runat="server" Text='<%# Eval("RTFD_CON_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                            <asp:GridView ID="dgConvert" runat="server" TabIndex="14" Style="width: 100%;" AutoGenerateColumns="False"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" CellPadding="4"
                                                                GridLines="Both">
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
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODE" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Bind("I_CODENO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qty" SortExpression="STL_DOC_QTY" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSTL_DOC_QTY" runat="server" Text='<%# Eval("STL_DOC_QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Weight" SortExpression="I_UWEIGHT" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_UWEIGHT" runat="server" Text='<%# Eval("I_UWEIGHT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="tonnage" SortExpression="TOTALWEIGHT" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTOTALWEIGHT" runat="server" Text='<%# Eval("TOTALWEIGHT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtDate" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:Panel ID="Panel1" runat="server">
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                            <div class="row" style="overflow: auto; width: 100%">
                                                <div class="col-md-12">
                                                    <div class="col-md-5">
                                                    </div>
                                                    <label class="control-label col-md-1 label-sm">
                                                        <font color="red">*</font> Qty</label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" ID="txtConQty" CssClass="form-control" MaxLength="15"
                                                                    placeholder="Converted Qty" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtConQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="  col-md-2 label-sm text-right">
                                                        Total Weight</label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblWeight" class="  label-sm text-right" runat="server" Text=""></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                            </asp:Panel>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="15" runat="server" Text="Save"
                                        OnClick="btnSubmit_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

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

    <script type="text/javascript" src="assets/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
