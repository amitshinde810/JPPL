<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Masters_ADD_Default" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function pageLoad() {
            var modalPopup = $find('mpe');
            modalPopup.add_shown(function() {
                modalPopup._backgroundElement.addEventListener("click", function() {
                    modalPopup.hide();
                });
            });
        };
    </script>

    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function() {
            SearchText();
        });
        function SearchText() {
            $("#txtItemCode").autocomplete({
                source: function(request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "test.asmx/CheckUser",
                        data: "{'I_CODENO':'" + document.getElementById('txtItemCode').value + "'}",
                        dataType: "json",
                        success: function(data) {
                            response(data.d);
                        },
                        error: function(result) {
                            alert("No Match");
                        }
                    });
                }
            });
        }  
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemCategory.ClientID %>").select2();
            jQuery("#<%=ddlTariffHeading.ClientID %>").select2();
            jQuery("#<%=ddlSAC.ClientID %>").select2();
            jQuery("#<%=ddltallyAccP.ClientID %>").select2();
            jQuery("#<%=ddlTallyAccS.ClientID %>").select2();
            jQuery("#<%=ddlWeightUOM.ClientID %>").select2();
            jQuery("#<%=ddlStockUOM.ClientID %>").select2();
            jQuery("#<%=ddlInventoryCategory.ClientID %>").select2();
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
                <div class="col-md-12 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Item Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Item Category</label>
                                                <div class="col-md-5">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCategory" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Item Category" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Maximum Level
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel55" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtMaximumLevel" placeholder="0.00"
                                                                TabIndex="2" AutoPostBack="true" MaxLength="20" runat="server" OnTextChanged="txtMaximumLevel_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtMaximumLevel"
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
                                                <label class="col-md-2 control-label">
                                                </label>
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddlSubCategory" CssClass="select2_category form-control" runat="server"
                                                        MsgObrigatorio="Sub Category" TabIndex="1" Visible="false">
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-2   control-label">
                                                    Minimum Level
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtMinimumLevel" placeholder="0.00"
                                                                TabIndex="3" AutoPostBack="true" MaxLength="20" runat="server" OnTextChanged="txtMinimumLevel_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtMinimumLevel"
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
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Item Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtItemCode" placeholder="Item Code" TabIndex="4"
                                                                MaxLength="3000" runat="server" MsgObrigatorio="Item Code" AutoPostBack="true"
                                                                OnTextChanged="txtItemCode_TextChanged"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-4 control-label">
                                                    Re-Order Level
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtReOrderLevel" placeholder="0.00"
                                                                TabIndex="5" AutoPostBack="true" MaxLength="20" runat="server" OnTextChanged="txtReOrderLevel_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtReOrderLevel"
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
                                                <label class="col-md-2 control-label">
                                                    Drawing Number
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" ID="txtDrawingNumnber" placeholder="Drawing Number"
                                                                TabIndex="6" MaxLength="3000" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-4 control-label" visible="false">
                                                    Op Balance
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control text-right" ID="txtOpeningBalance" placeholder="0.00"
                                                        TabIndex="7" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtOpeningBalance"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Item Name
                                                </label>
                                                <div class="col-md-5">
                                                    <asp:TextBox CssClass="form-control" ID="txtItemName" placeholder="Item Name" TabIndex="8"
                                                        MaxLength="3000" runat="server" MsgObrigatorio="Item Name"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label" visible="false">
                                                    Op. Bal. Rate
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control text-right" ID="txtOpeningBalanceRate" placeholder="0.00"
                                                        TabIndex="9" MaxLength="10" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtOpeningBalanceRate"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Specifications
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control" ID="txtSpecifications" placeholder="Specifications"
                                                        TabIndex="10" MaxLength="2000" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Store Loc.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control" MaxLength="200" ID="txtStoreLocation" placeholder="Store Location"
                                                        TabIndex="11" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span>HSN No.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlTariffHeading" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select HSN No" TabIndex="12">
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Rate
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtInventoryRate" placeholder="0.00"
                                                                TabIndex="13" AutoPostBack="true" MaxLength="10" runat="server" OnTextChanged="txtInventoryRate_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtInventoryRate"
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
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span>SAC No.
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlSAC" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Select SAC NO"
                                                        TabIndex="14">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Costing Head
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control" ID="txtCoastingHead" placeholder="Costing Head"
                                                        TabIndex="15" MaxLength="1000" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Current Bal.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control text-right" ReadOnly="true" ID="txtCurrentBal"
                                                        placeholder="0.00" TabIndex="16" MaxLength="10" Text="0.00" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtCurrentBal"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Tally Acct(P)
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddltallyAccP" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select Tally Acct (P)" TabIndex="17">
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Last Recd. Date
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtLastRecdDate"
                                                        TabIndex="18" runat="server" TextMode="SingleLine" Enabled="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLastRecdDate_CalendarExtender1" runat="server" Enabled="True"
                                                        Format="dd MMM yyyy" TargetControlID="txtLastRecdDate" PopupButtonID="txtLastRecdDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Tally Acct(S)
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlTallyAccS" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select Tally Acct (S)" TabIndex="19">
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Last Issue. Date
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtLastIssueDate"
                                                        TabIndex="20" runat="server" TextMode="SingleLine" Enabled="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtLastIssueDate_CalendarExtender" runat="server" Enabled="True"
                                                        Format="dd MMM yyyy" TargetControlID="txtLastIssueDate" PopupButtonID="txtLastIssueDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Stock Unit
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlStockUOM" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select Stock Unit" TabIndex="21">
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Internal cast weight
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtUniWeight" placeholder="0.00"
                                                                TabIndex="22" AutoPostBack="true" MaxLength="10" runat="server" OnTextChanged="txtUniWeight_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtUniWeight"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                    Weight Unit
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlWeightUOM" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select Weight Unit" TabIndex="23">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Inventory Category
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlInventoryCategory" CssClass="select2" Width="100%" runat="server"
                                                        MsgObrigatorio="Select Inventory Category" TabIndex="24">
                                                        <asp:ListItem Selected="True" Value="0">Select </asp:ListItem>
                                                        <asp:ListItem Value="1">A</asp:ListItem>
                                                        <asp:ListItem Value="2">B</asp:ListItem>
                                                        <asp:ListItem Value="3">C</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Active
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="ChkActiveInd" runat="server" CssClass="checker" AutoPostBack="True"
                                                                    TabIndex="25" Checked="true" />
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Under Development
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkDevelopment" runat="server" CssClass="checker" AutoPostBack="True"
                                                                    TabIndex="26" />
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div id="Div1" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Item Solids
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="TextBox1" placeholder="0.00"
                                                                TabIndex="24" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtSolids"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-5 control-label">
                                                    Volume Solids
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="TextBox2" placeholder="0.00"
                                                                TabIndex="24" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtVolatile"
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
                                                <label class="col-md-2 control-label">
                                                    Internal finish weight
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtTargetWeight"
                                                                placeholder="0.00" TabIndex="27" AutoPostBack="true" runat="server" OnTextChanged="txtTargetWeight_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtTargetWeight"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    BD finish Weight
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtDensity" placeholder="0.00"
                                                                TabIndex="28" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtDensity"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1 control-label">
                                                    <%--Item Pigment--%>
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtPigment" placeholder="0.00"
                                                                TabIndex="24" runat="server" Visible="false">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtPigment"
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
                                                <label class="col-md-2 control-label">
                                                    Dispatch Balance
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtDispatchBal"
                                                                placeholder="0.00" TabIndex="27" AutoPostBack="true" runat="server" ReadOnly="true"
                                                                OnTextChanged="txtTargetWeight_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtDispatchBal"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    STD weight
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtStdWeight"
                                                                placeholder="0.00" TabIndex="28" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txtStdWeight"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div51" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Item Solids
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtSolids" placeholder="0.00"
                                                                TabIndex="24" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtSolids"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-5 control-label">
                                                    Volume Solids
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" ID="txtVolatile" placeholder="0.00"
                                                                TabIndex="24" runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtVolatile"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div2" class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Standard Production
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtStandardProduction"
                                                                placeholder="0.00" TabIndex="27" onkeypress="return validateFloatKeyPress(this,event);"
                                                                AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtStandardProduction"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    As Cast Weight
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right" MaxLength="10" ID="txtAs_Cast_Weight"
                                                                placeholder="0.00" TabIndex="28" onkeypress="return validateFloatKeyPress(this,event);"
                                                                runat="server">0</asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" TargetControlID="txtAs_Cast_Weight"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="29" runat="server"
                                        OnClick="btnSubmit_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="30" runat="server"
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
                    <asp:UpdatePanel runat="server" ID="UpdatePanel22">
                        <ContentTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" PopupControlID="textPanel1"
                                TargetControlID="LinkButton2" BehaviorID="mpe">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="textPanel1" runat="server" Width="50%" Height="10%">
                                <div class="portlet box blue" style="position: fixed!important; width: 50%!important;
                                    top: 5%!important">
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="dgRawMaterial" runat="server" Width="100%" Height="50px" AutoGenerateColumns="False"
                                                                    ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                    DataKeyNames="I_CODE" AllowPaging="false">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="I_CODE" SortExpression="I_CODE" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblI_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                            </ItemTemplate>
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
                                                                    </Columns>
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <PagerStyle CssClass="pgr" />
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtItemCode" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
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
    
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)-->
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlItemCategory.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                if (VerificaObrigatorio('#<%=txtItemCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaObrigatorio('#<%=txtItemName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaValorCombo('#<%=ddlTariffHeading.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaValorCombo('#<%=ddltallyAccP.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyAccS.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaValorCombo('#<%=ddlInventoryCategory.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else if (VerificaValorCombo('#<%=ddlStockUOM.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000); return false;
                }
                else
                { return true; }
            } catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
