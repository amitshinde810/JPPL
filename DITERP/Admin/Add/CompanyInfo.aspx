<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CompanyInfo.aspx.cs"
    Inherits="Admin_Add_CompanyInfo" Title="Company Information" %>

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
        function oknumber(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <!-- BEGIN CONTENT -->
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                        </div>
                        <div id="MSG" class="col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
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
                    <br>
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Company Information
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnClose_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <!-- Page Body -->
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Company Name</label>
                                                <div class="col-md-9">
                                                    <asp:TextBox placeholder="Company Name" CssClass="form-control" MsgObrigatorio="Please Enter Company Name"
                                                        ID="txtCompanyName" runat="server" TabIndex="1"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Corporate Address</label>
                                                <div class="col-md-9">
                                                    <asp:TextBox placeholder="Address line 1" CssClass="form-control" ID="txtAddressLine1"
                                                        MsgObrigatorio="Please Enter Corporate Address" runat="server" TabIndex="2" TextMode="MultiLine"
                                                        Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Plant Address</label>
                                                <div class="col-md-9">
                                                    <asp:TextBox placeholder="Address line 2" CssClass="form-control" ID="txtAddressLine2"
                                                        MsgObrigatorio="Please Enter Plant Address" runat="server" TabIndex="3" TextMode="MultiLine"
                                                        Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Country</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="4" ID="ddlCountry" AutoPostBack="True"
                                                                CssClass="select2_category form-control" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="Select Country" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="India"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="5" ID="ddlState" DataTextField="State_Name"
                                                                DataValueField="State_ID" AutoPostBack="True" CssClass="select2_category form-control"
                                                                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                <asp:ListItem Value="1" Text="Select State" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="6" ID="ddlCity" DataTextField="City_Name"
                                                                DataValueField="City_ID" AutoPostBack="True" CssClass="select2_category form-control">
                                                                <asp:ListItem Value="1" Text="Select City" Selected="True"></asp:ListItem>
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
                                                <label class="col-md-2 control-label">
                                                    Phone</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Phone 1" CssClass="form-control" ID="txtPhoneNo1" runat="server"
                                                        MsgObrigatorio="Please Enter Phone No." TabIndex="7"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtPhoneNo1"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Phone 2" CssClass="form-control" ID="txtPhoneNumber2" runat="server"
                                                        TabIndex="8"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtPhoneNumber2"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Phone 3" CssClass="form-control" ID="txtPhoneNumber3" runat="server"
                                                        TabIndex="9"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtPhoneNumber3"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Email</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Email" CssClass="form-control" ID="txtEmailId" runat="server"
                                                        MsgObrigatorio="Please Enter Email" TabIndex="10"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Website</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Webstite" CssClass="form-control" ID="txtWebsite" runat="server"
                                                        TabIndex="11"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Fax</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Fax No." CssClass="form-control" ID="txtFaxNo" runat="server"
                                                        TabIndex="12"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtFaxNo"
                                                        ValidChars="0123456789" runat="server" />
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Authorized Signatory</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder=" Auth.Sign." CssClass="form-control" ID="txtAuthSign" runat="server"
                                                        TabIndex="13"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <h4 class="form-section">
                                        Taxation Information</h4>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Regh. No.</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="Regh No." CssClass="form-control" ID="txtReghNo" runat="server"
                                                        MsgObrigatorio="Please Enter Regh No." TabIndex="14"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    VAT TIN No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="update121" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="VAT TIN No." CssClass="form-control" ID="txtVatTinNo" AutoPostBack="true"
                                                                runat="server" MsgObrigatorio="Please Enter VAT No." TabIndex="15" OnTextChanged="txtVatTinNo_TextChanged"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    CST No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="CST No." CssClass="form-control" ID="txtCstNo" runat="server"
                                                                MsgObrigatorio="Please Enter CST No." TabIndex="16"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtVatTinNo" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    ISO No.</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="ISO No." CssClass="form-control" ID="txtISONumber" runat="server"
                                                        MsgObrigatorio="Please Enter ISO Number" TabIndex="17"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    VAT W.E.F</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="dd/MMM/yyyy" CssClass="form-control" ID="txtVatWef" MsgObrigatorio="Please Enter VAT W.E.F Date"
                                                        TextMode="SingleLine" runat="server" TabIndex="18"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                                        TargetControlID="txtVatWef">
                                                    </cc1:CalendarExtender>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    CST W.E.F</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="dd/MMM/yyyy" CssClass="form-control" ID="txtCstWef" MsgObrigatorio="Please Enter CST W.E.F Date"
                                                        TextMode="SingleLine" runat="server" TabIndex="19"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                                        TargetControlID="txtCstWef">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    ECC No.</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="ECC No." CssClass="form-control" ID="txtEccNo" runat="server"
                                                        MsgObrigatorio="Please Enter ECC No." TabIndex="20"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    PAN No.</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="PAN No." CssClass="form-control" ID="txtPanNo" runat="server"
                                                        MsgObrigatorio="Please Enter PAN No." TabIndex="21"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Active</label>
                                                <div class="col-md-2">
                                                    <asp:CheckBox ID="cbActiveIndex" runat="server" CssClass="checker" Checked="true"
                                                        Text=" " TabIndex="22" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Service Tax No.</label>
                                                <div class="col-md-2">
                                                    <asp:TextBox placeholder="Service Tax No." CssClass="form-control" ID="txtServiceTax"
                                                        MsgObrigatorio="Please Enter Service Tax No." runat="server" TabIndex="23"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Opening Date</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="Opening Date" CssClass="form-control" ID="txtOpeningDate"
                                                                MsgObrigatorio="Please Enter Opening Date" TextMode="SingleLine" runat="server"
                                                                TabIndex="24" OnTextChanged="txtOpeningDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtOpeningDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd/MMM/yyyy" TargetControlID="txtOpeningDate">
                                                            </cc1:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span> Closing Date</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox placeholder="Closing Date" CssClass="form-control" ID="txtClosingDate"
                                                                MsgObrigatorio="Please Enter Closing Date" Enabled="false" TextMode="SingleLine"
                                                                runat="server" TabIndex="25"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtClosingDate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd/MMM/yyyy" TargetControlID="txtClosingDate">
                                                            </cc1:CalendarExtender>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtOpeningDate" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Export Licence No.</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Export Licence No" CssClass="form-control" ID="txtExpLicenNo"
                                                        runat="server" TabIndex="26"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Export Registration No.</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Export Registration No" CssClass="form-control" ID="txtExpPermisiionNo"
                                                        runat="server" MsgObrigatorio="Please Enter Export Registration No" TabIndex="27"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Newly Added -->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Excise Range</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Excise Range" CssClass="form-control" ID="txtExciseRange"
                                                        runat="server" TabIndex="28"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Excise Devision</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Excise Division" CssClass="form-control" ID="txtExciseDevision"
                                                        runat="server" TabIndex="29"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Commisionerate</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Commisionerate" CssClass="form-control" ID="txtCommisionerate"
                                                        runat="server" TabIndex="30"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    Exc. Supre. Details</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="Excise Superintendent Details" CssClass="form-control"
                                                        ID="txtExcSupreDetail" TextMode="MultiLine" Rows="3" runat="server" TabIndex="31"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    CIN No.</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="CIN Number" CssClass="form-control" ID="txtCinNo" runat="server"
                                                        TabIndex="32"></asp:TextBox>
                                                </div>
                                                <label class="col-md-3 control-label">
                                                    GST No.</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox placeholder="GST No." CssClass="form-control" ID="txtGSTNo" runat="server"
                                                        TabIndex="33"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Banker's Name</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Banker's Name" CssClass="form-control" ID="txtBankersName"
                                                        runat="server" TabIndex="33"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Branch Address</label>
                                                <div class="col-md-10">
                                                    <asp:TextBox placeholder="Branch Address" CssClass="form-control" ID="txtBranchAddress"
                                                        runat="server" TabIndex="34"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Account No.</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Account No" CssClass="form-control" ID="txtAccountNo" runat="server"
                                                        TabIndex="35"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Type of Account</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Type of Account" CssClass="form-control" ID="txtTypeofAccount"
                                                        runat="server" TabIndex="36"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Swift Code</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Swift Code" CssClass="form-control" ID="txtSwiftCode" runat="server"
                                                        TabIndex="37"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    IFSC Code</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="IFSC Code" CssClass="form-control" ID="txtIFSCCode" runat="server"
                                                        TabIndex="38"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Commissioner of Customs</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Commissioner of Customs" TextMode="MultiLine" Rows="2"
                                                        CssClass="form-control" ID="txtCommCustom" runat="server" TabIndex="39" MaxLength="200"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Authority Specimen Signature</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox placeholder="Authority Specimen Signature" CssClass="form-control" ID="txtSpecimenSign"
                                                        runat="server" TabIndex="40" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- end -->
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
                                            <div class="form-actions fluid">
                                                <div class="col-md-offset-4 col-md-9">
                                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="41" runat="server"
                                                        OnClientClick="return VerificaCamposObrigatorios();" OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="42" runat="server"
                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <!--/row-->
                                </div>
                            </div>
                        </div>
                        <!-- End  Page Body -->
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END CONTENT -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtCompanyName.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtAddressLine1.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtOpeningDate.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtClosingDate.ClientID%>', '#Avisos') == false) {
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

    <!-- END JAVASCRIPTS -->
</asp:Content>
