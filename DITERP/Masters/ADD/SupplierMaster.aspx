<%@ Page Title="Supplier Master" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="SupplierMaster.aspx.cs" Inherits="Masters_ADD_SupplierMaster" %>

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script lang="javascript" type="text/javascript">
        function ValidateRegForm() {
            var email = document.getElementById("<%=txtEmailId.ClientID%>");
            var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(email.value)) {
                alert('Please provide a valid email address');
                email.focus;
                return false;
            }
            return true;
        }
    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlArea.ClientID %>").select2();
            jQuery("#<%=ddlSupplierType.ClientID %>").select2();
            jQuery("#<%=ddlState.ClientID %>").select2();
            jQuery("#<%=ddlCity.ClientID %>").select2();
            jQuery("#<%=ddlCountry.ClientID %>").select2();
            jQuery("#<%=ddlExiseType.ClientID %>").select2();
        });
   </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                margin-bottom: 10px; height: 50px; width: 100%; border: 1px solid #9f6000">
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
                                <i class="fa fa-reorder"></i>Supplier Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Party Name</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPartyName" TabIndex="1" MsgObrigatorio=" Party Name"
                                                        placeholder="Party Name" MaxLength="2000" AutoPostBack="true" runat="server"
                                                        OnTextChanged="txtPartyName_TextChanged"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Area
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlArea" CssClass="select2" Width="100%" runat="server" MsgObrigatorio=" Sector Area"
                                                                TabIndex="2">
                                                            </asp:DropDownList>
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
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Contact Person
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtContactPerson" placeholder="Contact Person"
                                                        TabIndex="3" runat="server" MaxLength="2000" MsgObrigatorio="Contact Person"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Supplier Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlSupplierType" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Supplier Type" TabIndex="4">
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
                                                <label class="col-md-2 control-label label-sm">
                                                    Party Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPartyCode" placeholder="Party Code"
                                                        TabIndex="5" runat="server" MaxLength="12" ReadOnly="true">  </asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Fax No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtFaxNo" placeholder="Fax No."
                                                        TabIndex="6" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtFaxNo"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Vendor Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtVendorCode" placeholder="Vendor Code"
                                                        TabIndex="7" MaxLength="12" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>Email Id
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEmailId" placeholder="Email Id"
                                                        TabIndex="8" runat="server" MsgObrigatorio="Email Id"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers,LowercaseLetters,Custom"
                                                        ValidChars=".@" TargetControlID="txtEmailId" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Address
                                                </label>
                                                <div class="col-md-9">
                                                    <asp:TextBox CssClass="form-control" ID="txtAddress" MsgObrigatorio="Address" placeholder="Address"
                                                        TabIndex="9" runat="server" MaxLength="2000" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Country
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" CssClass="select2"
                                                                Width="100%" TabIndex="10" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                                                MsgObrigatorio="Country">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    <span class="required">* </span>PAN NO.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPANNO" placeholder="PAN NO."
                                                        TabIndex="11" runat="server" MaxLength="10" MsgObrigatorio="PAN No."></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> State
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" MsgObrigatorio=" State"
                                                                CssClass="select2" Width="100%" TabIndex="12" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    VAT TIN No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="up11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtVATTINNo" AutoPostBack="true"
                                                                placeholder="VAT TIN No" MaxLength="15" TabIndex="13" runat="server" OnTextChanged="txtVatTinNo_TextChanged"></asp:TextBox>
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
                                                    <span class="required">*</span>City
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" MsgObrigatorio=" City"
                                                                CssClass="select2" Width="100%" TabIndex="14">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    CST TIN No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="up123" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCSTTINNo" placeholder="CST TIN No"
                                                                TabIndex="15" MaxLength="15" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtVATTINNo" EventName="TextChanged" />
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
                                                    <span class="required">* </span>Pin Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPinCode" placeholder="Pin Code"
                                                        TabIndex="16" runat="server" MaxLength="6" MsgObrigatorio=" Pin Code"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtPinCode"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Ser.Tax.Reg No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtSerTaxRegNo" placeholder="Ser.Tax.Reg No"
                                                        TabIndex="17" MaxLength="20" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Phone No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPhoneNo" placeholder="Phone No"
                                                        TabIndex="18" MaxLength="13" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtPhoneNo"
                                                        ValidChars="0123456789+,-" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Supplier Category
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlExiseType" runat="server" AutoPostBack="true" CssClass="select2"
                                                                Width="100%" TabIndex="19">
                                                                <%-- <asp:ListItem Value="0">--Select Excise Type--</asp:ListItem>
                                                                <asp:ListItem Value="1">First Stage Dealer</asp:ListItem>
                                                                <asp:ListItem Value="2">Second Stage Dealer</asp:ListItem>
                                                                <asp:ListItem Value="3">Trader</asp:ListItem>
                                                                <asp:ListItem Value="4">Manufacturer</asp:ListItem>--%>
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
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Mobile No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtMobileNo" placeholder="Mobile No"
                                                        TabIndex="20" runat="server" MaxLength="10" MsgObrigatorio=" Mobile No."></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtMobileNo"
                                                        ValidChars="0123456789," runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Excise Division
                                                </label>
                                                <div class="col-md-3">
                                                    <label class="checkbox">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtExiseDevision" placeholder="Excise Division"
                                                            TabIndex="21" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Category
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select2_category form-control"
                                                        TabIndex="22">
                                                        <asp:ListItem Value="0">A</asp:ListItem>
                                                        <asp:ListItem Value="1">B</asp:ListItem>
                                                        <asp:ListItem Value="2">C</asp:ListItem>
                                                        <asp:ListItem Value="3">D</asp:ListItem>
                                                        <asp:ListItem Value="4">E</asp:ListItem>
                                                        <asp:ListItem Value="5">G</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Excise Range
                                                </label>
                                                <div class="col-md-3">
                                                    <label class="checkbox">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtExiseRange" placeholder="Excise Range"
                                                            TabIndex="23" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Tally Name
                                                </label>
                                                <div class="col-md-3">
                                                    <label class="checkbox">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtTallyName" placeholder="Tally Name"
                                                            TabIndex="24" MaxLength="30000" runat="server" MsgObrigatorio="Tally Name"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Excise Collectorate
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtExiseCollectorate" placeholder="Excise Collectorate"
                                                        TabIndex="25" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 text-right label-sm">
                                                    GST Applicable
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="up34" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chlLBTApplicable" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                                TabIndex="26" OnCheckedChanged="chlLBTApplicable_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    TDS Tax %
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm text-right" ID="txtTDSTAX" placeholder=" TDS Tax %"
                                                                TabIndex="27" Text="0.00" MaxLength="5" runat="server" AutoPostBack="true" OnTextChanged="txtTDSTAX_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtTDSTAX"
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
                                                <label class="col-md-2 control-label label-sm">
                                                    GST No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtLBTNo" placeholder="GST No."
                                                                TabIndex="28" MaxLength="15" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chlLBTApplicable" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    ECC No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtECCNo" placeholder="ECC No."
                                                        TabIndex="29" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    User Name
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPUserName" placeholder="User Name"
                                                        TabIndex="28" MaxLength="15" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Password
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPPassword" TextMode="Password"
                                                        placeholder="Password" TabIndex="29" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Re-Enter Password
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtConPassword" TextMode="Password"
                                                        placeholder="Re-Enter Password" TabIndex="29" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Credit Days
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtCreditdays" placeholder="Credit Days"
                                                        TabIndex="30" MaxLength="50" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 text-right label-sm">
                                                    Active Indicator
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4DD721" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkActiveInd" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                                TabIndex="31" /></label>
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
                                                    Assessment Form
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePandel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:FileUpload ID="FileUpload2" TabIndex="5" ClientIDMode="Static" onchange="this.form.submit()"
                                                                runat="server" />
                                                            <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" Style="display: none" />
                                                            <asp:LinkButton ID="lnkupload" runat="server" Text="" OnClick="lnkupload_Click"> </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 text-right label-sm">
                                                    Approved Indicator
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkApproved" runat="server" Text="" CssClass="checker" TabIndex="31" /></label>
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
                                                    Supporting Document
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
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <%--GST Number--%>
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" Visible="false" ID="txtGSTNo" placeholder="GST Number"
                                                        TabIndex="34" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label label-sm">
                                                        IS All Type Supplier
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanelffff471" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkBoth" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                                    TabIndex="31" /></label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-5 col-md-9">
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="32" runat="server"
                                            OnClick="btnSubmit_Click" OnClientClick="return ValidateRegForm();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="33" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh" ></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel32">
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
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" runat="server" Visible="true" OnClick="btnOk_Click"> Yes </asp:LinkButton>
                                                        <asp:LinkButton ID="Button6" CssClass="btn default" runat="server" OnClick="btnCancel1_Click"> No</asp:LinkButton>
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
                                                        <iframe runat="server" id="myframe" class="reponsive-screen"></iframe>
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)
    -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js
    before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
    -->

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
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <%--<!-- END JAVASCRIPTS -->--%>
    <%-- <script type="text/javascript">
            function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtPartyName.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; } if (VerificaValorCombo('#<%=ddlArea.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } if (VerificaValorCombo('#<%=ddlSupplierType.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtAddress.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaValorCombo('#<%=ddlCountry.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaValorCombo('#<%=ddlState.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaValorCombo('#<%=ddlCity.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtTallyName.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtPinCode.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtPANNO.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtContactPerson.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtEmailId.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtMobileNo.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else {
                    return
                    true;
                }
            } catch (err) {
                alert('Erro in Required Fields: ' + err.description); return
                false;
            }
        } </script>--%>
</asp:Content>
