<%@ Page Title="Customer Master" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="CustomerMaster.aspx.cs" Inherits="Masters_ADD_CustomerMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <label class="col-md-2 control-label label-sm">
        Abbreviation</label>
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlArea.ClientID %>").select2();
            jQuery("#<%=ddlCustomerType.ClientID %>").select2();
            jQuery("#<%=ddlState.ClientID %>").select2();
            jQuery("#<%=ddlCity.ClientID %>").select2();
            jQuery("#<%=ddlCountry.ClientID %>").select2();
            jQuery("#<%=ddlExiseType.ClientID %>").select2();
            jQuery("#<%=ddlCategory.ClientID %>").select2();
        });
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

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Customer Master
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
                                                    <span class="required">* </span>Customer Name</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPartyName" TabIndex="1" MsgObrigatorio="Customer Name"
                                                        placeholder="Party Name" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Area
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlArea" CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Sector Area"
                                                                TabIndex="17">
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
                                                    Contact Person
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtContactPerson" placeholder="Contact Person"
                                                        TabIndex="2" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">* </span>Customer Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomerType" CssClass="select2" Width="100%" runat="server"
                                                                MsgObrigatorio="Customer Type" TabIndex="18">
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
                                                    Customer Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPartyCode" placeholder="Party Code"
                                                        TabIndex="3" runat="server" ReadOnly="true">  </asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Fax No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtFaxNo" placeholder="Fax No."
                                                        TabIndex="19" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtFaxNo"
                                                        ValidChars="0123456789.-" runat="server" />
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
                                                        TabIndex="4" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Email Id
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEmailId" placeholder="Email Id"
                                                        TabIndex="20" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Address
                                                </label>
                                                <div class="col-md-9">
                                                    <asp:TextBox CssClass="form-control" ID="txtAddress" MsgObrigatorio="Adress" placeholder="Address"
                                                        TabIndex="5" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
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
                                                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" MsgObrigatorio=" Country"
                                                                CssClass="select2" Width="100%" TabIndex="6" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Credit Days
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtCreditdays" placeholder="Credit Days"
                                                        TabIndex="21" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtCreditdays"
                                                        ValidChars="0123456789." runat="server" />
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
                                                                CssClass="select2" Width="100%" TabIndex="7" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    PAN NO.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPANNO" placeholder="PAN NO."
                                                        TabIndex="22" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> City
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" MsgObrigatorio=" City"
                                                                CssClass="select2" Width="100%" TabIndex="8">
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
                                                                placeholder="VAT TIN No." TabIndex="23" runat="server" OnTextChanged="txtVatTinNo_TextChanged"></asp:TextBox>
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
                                                    Pin Code
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPinCode" placeholder="Pin Code"
                                                        TabIndex="9" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtPinCode"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    CST TIN No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="up123" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtCSTTINNo" placeholder="CST TIN No."
                                                                TabIndex="24" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtVATTINNo" EventName="TextChanged" />
                                                        </Triggers>
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
                                                    Phone No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtPhoneNo" placeholder="Phone No."
                                                        TabIndex="10" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtPhoneNo"
                                                        ValidChars="0123456789+,-" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Ser.Tax.Reg No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtSerTaxRegNo" placeholder="Ser.Tax.Reg No."
                                                        TabIndex="25" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtPinCode"
                                                        ValidChars="0123456789." runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Mobile No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtMobileNo" placeholder="Mobile No."
                                                        TabIndex="11" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtMobileNo"
                                                        ValidChars="0123456789," runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Exise Type
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlExiseType" runat="server" AutoPostBack="true" CssClass="select2"
                                                                Width="100%" TabIndex="26">
                                                                <asp:ListItem Value="0">--Select Excise Type--</asp:ListItem>
                                                                <asp:ListItem Value="1">First Stage Dealer</asp:ListItem>
                                                                <asp:ListItem Value="2">Second Stage Dealer</asp:ListItem>
                                                                <asp:ListItem Value="3">Trader</asp:ListItem>
                                                                <asp:ListItem Value="4">Manufacturer</asp:ListItem>
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
                                                    Category
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select2" Width="100%"
                                                        TabIndex="12">
                                                        <asp:ListItem Value="0">A</asp:ListItem>
                                                        <asp:ListItem Value="1">B</asp:ListItem>
                                                        <asp:ListItem Value="2">C</asp:ListItem>
                                                        <asp:ListItem Value="3">D</asp:ListItem>
                                                        <asp:ListItem Value="4">E</asp:ListItem>
                                                        <asp:ListItem Value="5">G</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Exise Division
                                                </label>
                                                <div class="col-md-3">
                                                    <label class="checkbox">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtExiseDevision" placeholder="Exise Division"
                                                            TabIndex="27" runat="server"></asp:TextBox>
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
                                                            TabIndex="13" runat="server" MsgObrigatorio="Tally Name"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    Exise Range
                                                </label>
                                                <div class="col-md-3">
                                                    <label class="checkbox">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtExiseRange" placeholder="Exise Range"
                                                            TabIndex="28" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    GST Applicable
                                                </label>
                                                <div class="col-md-4">
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel ID="up34" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chlLBTApplicable" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                                    TabIndex="14" OnCheckedChanged="chlLBTApplicable_CheckedChanged" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control input-sm" Width="90%" ID="txtLBTNo" placeholder="GST No."
                                                                    TabIndex="15" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="chlLBTApplicable" EventName="CheckedChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <%-- <div class="col-md-3">
                                                </div>--%>
                                                <label class="col-md-2 control-label label-sm">
                                                    Exise Collectorate
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtExiseCollectorate" placeholder="Exise Collectorate"
                                                        TabIndex="29" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Abbreviation</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtAbbrevation" MaxLength="20"
                                                        TabIndex="32" MsgObrigatorio="Abbreviation Name" placeholder="Abbreviation" runat="server"></asp:TextBox>
                                                </div>
                                                <label runat="server" visible="false" class="col-md-2 control-label label-sm">
                                                    GST No.
                                                </label>
                                                <label class="col-md-3 control-label label-sm">
                                                    ECC No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtECCNo" placeholder="ECC No."
                                                        TabIndex="30" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Active Indicator
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ChkActiveInd" runat="server" Text="" Checked="true" CssClass="checker"
                                                                AutoPostBack="True" TabIndex="16" /></label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    TDS %
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel88" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm text-right" ID="txtTDSTAX" placeholder=" TDS Tax %"
                                                                TabIndex="31" MaxLength="5" AutoPostBack="true" Text="0.00" runat="server" OnTextChanged="txtTDSTAX_TextChanged"></asp:TextBox>
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
                                                    Reverse charge
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtReversecharge" placeholder="Reverse charge"
                                                        TabIndex="30" runat="server"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 text-right label-sm">
                                                    QR Code On Invoice
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkQRCode" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                        TabIndex="14" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="33" runat="server"
                                        OnClick="btnSubmit_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="34" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                            OnClick="btnOk_Click"> Yes </asp:LinkButton>
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
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time)
    -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix
    bootstrap tooltip conflict with jquery ui tooltip -->

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
            });
        });
    </script>

    <%--<!-- END JAVASCRIPTS -->--%>

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtPartyName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaValorCombo('#<%=ddlArea.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                if (VerificaValorCombo('#<%=ddlCustomerType.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlCountry.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlState.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlCity.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtTallyName.ClientID%>', '#Avisos') == false) {
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
