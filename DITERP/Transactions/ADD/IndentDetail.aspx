<%@ Page Title="Indent Detail" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="IndentDetail.aspx.cs" Inherits="Transactions_ADD_IndentDetail" %>

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
            jQuery("#<%=ddlIndentType.ClientID %>").select2();
            jQuery("#<%=ddlProjectCode.ClientID %>").select2();
            jQuery("#<%=ddlDepartment.ClientID %>").select2();
            jQuery("#<%=ddlSuppType.ClientID %>").select2();
            jQuery("#<%=ddlSupplierName.ClientID %>").select2();
            jQuery("#<%=ddlProject.ClientID %>").select2();
            jQuery("#<%=ddlItem.ClientID %>").select2();
        });
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
                                <i class="fa fa-reorder"></i>Indent Detail
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="18" runat="server"></asp:LinkButton>
                                <%-- OnClick="btnCancel_Click"--%>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font>Indent Type</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlIndentType" runat="server" TabIndex="1" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" MsgObrigatorio=" Supplier Name" OnSelectedIndexChanged="ddlIndentType_SelectedIndexChanged">
                                                                <%--OnSelectedIndexChanged="ddlIndentType_SelectedIndexChanged"--%>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Indent No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtIndentNo" placeholder="0" TabIndex="1"
                                                                runat="server" ReadOnly="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Indent Date</label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtIndentDate" runat="server" CssClass="form-control input-sm" TabIndex="2"
                                                            msgObrigatorio="Please Select GRN Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="txtIndentDate_CalendarExtender" runat="server" Enabled="True"
                                                            Format="dd MMM yyyy" TargetControlID="txtIndentDate">
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
                                                    <font color="red">*</font>Project Funding</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlProjectCode" runat="server" TabIndex="3" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" MsgObrigatorio=" Supplier Name" OnSelectedIndexChanged="ddlProjectCode_SelectedIndexChanged">
                                                                <%--OnSelectedIndexChanged="ddlIndentType_SelectedIndexChanged"--%>
                                                                <asp:ListItem Value="1">Internal Funding</asp:ListItem>
                                                                <asp:ListItem Value="2">External Funding</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Budget No.</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBudgetNo" runat="server" CssClass="form-control input-sm" placeholder="0"
                                                                TabIndex="4" AutoPostBack="true" MaxLength="100" MsgObrigatorio=" Enter Budget No."></asp:TextBox>
                                                            <%--OnTextChanged="txtChallanNo_TextChanged" --%>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font>Department</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlDepartment" runat="server" TabIndex="7" CssClass="select2"
                                                                Width="100%" AutoPostBack="True" MsgObrigatorio="Department" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                                                                <%--OnSelectedIndexChanged="ddlIndentType_SelectedIndexChanged"--%>
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
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Project Code</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlProject" MsgObrigatorio="Project Code" AutoPostBack="true"
                                                                CssClass="select2" Width="100%" runat="server" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Reason for purchase
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control input-sm" placeholder="Reason for purchase"
                                                                TabIndex="4" AutoPostBack="true" MaxLength="100" MsgObrigatorio=" Reason for purchase"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    <font color="red">*</font> Indent Validity</label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtValidity" runat="server" CssClass="form-control input-sm" TabIndex="2"
                                                            msgObrigatorio="Please Select GRN Date"></asp:TextBox>
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                            TargetControlID="txtValidity">
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
                                                    <font color="red">*</font> Budget Amount</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBudgetAmt" runat="server" CssClass="form-control input-sm right"
                                                                placeholder="Project Amount" TabIndex="4" AutoPostBack="true" ReadOnly="true"
                                                                MaxLength="100" MsgObrigatorio=" Project Amount"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2 control-label text-right">
                                                    <asp:Label ID="lbl" runat="server" Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lnkupload" runat="server" Text="Indent Raised Amount" OnClick="lnkPop_Click"> </asp:LinkButton>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtIndRaisedAmt" runat="server" CssClass="form-control input-sm right"
                                                                placeholder=" Indent Raised Amount" TabIndex="4" AutoPostBack="true" ReadOnly="true"
                                                                MaxLength="100" MsgObrigatorio="  Indent Raised Amount"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label text-right">
                                                    Bal. Budget</label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtBalamt" runat="server" CssClass="form-control input-sm right"
                                                                placeholder=" Bal. Amount For Indent" TabIndex="4" AutoPostBack="true" ReadOnly="true"
                                                                MaxLength="100" MsgObrigatorio=" Bal. Amount For Indent"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label label-sm col-md-3">
                                                Supplier Type
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlSuppType" CssClass="select2" Width="100%" runat="server"
                                                            TabIndex="8" AutoPostBack="True" OnSelectedIndexChanged="ddlSuppType_SelectedIndexChanged">
                                                            <%-- <asp:ListItem Value="0">Select Supplier Type</asp:ListItem>--%>
                                                            <asp:ListItem Value="1">New</asp:ListItem>
                                                            <asp:ListItem Value="2">Present</asp:ListItem>
                                                            <%--OnSelectedIndexChanged="ddlPoNumber_SelectedIndexChanged"--%>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%-- <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel234" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <label class="control-label label-sm col-md-2" id="lblsuppname" runat="server">
1`                                                          Supplier Name
                                                    </label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />  
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlSupplierName" CssClass="select2" runat="server" TabIndex="9"
                                                            AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlSupplierName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--  <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class=" control-label label-sm col-md-3">
                                                Supplier Name
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtSuppName" runat="server" AutoPostBack="true"
                                                            TabIndex="10"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                Contact Name
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtContactNo" runat="server" AutoPostBack="true"
                                                            TabIndex="11"></asp:TextBox>
                                                        <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtContactNo"
                                                                runat="server" />--%>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPoNumber" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class=" control-label label-sm col-md-3">
                                                Address
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-left input-sm" ID="txtAddress" placeholder="Address"
                                                            TabIndex="12" MaxLength="50" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                Email ID
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtEmailid" placeholder="Email ID" TabIndex="13"
                                                            runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class=" control-label label-sm col-md-3">
                                                Mobile No.
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-left input-sm" ID="txtMobileNo" placeholder="Mobile No."
                                                            TabIndex="14" MaxLength="10" runat="server" AutoPostBack="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtMobileNo"
                                                            ValidChars="0123456789" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                GST No.
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtGSTNo" placeholder="GST No." TabIndex="15"
                                                            runat="server" MaxLength="20" AutoPostBack="true"></asp:TextBox>
                                                        <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtGSTNo"
                                                                ValidChars="0123456789" runat="server" />--%>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class=" control-label label-sm col-md-3">
                                                Pin Code
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-left input-sm" ID="txtPinCode" placeholder="Pin Code"
                                                            TabIndex="16" MaxLength="10" runat="server" AutoPostBack="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtPinCode"
                                                            ValidChars="0123456789" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                PAN No.
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtPanNo" placeholder=" PAN No." TabIndex="17"
                                                            runat="server" MaxLength="20" AutoPostBack="true"></asp:TextBox>
                                                        <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtPanNo"
                                                                ValidChars="0123456789" runat="server" />--%>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSupplierName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSuppType" EventName="SelectedIndexChanged" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class=" control-label label-sm col-md-2">
                                                Cheque
                                            </label>
                                            <div class="col-md-2" style="margin-top: 7px;">
                                                <asp:UpdatePanel ID="UpdatePanel45" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="fuCheque" ClientIDMode="Static" onchange="this.form.submit()"
                                                            runat="server" TabIndex="18" />
                                                        <asp:Button ID="btnCheque" Text="Upload" runat="server" Style="display: none" OnClick="UploadCheque" />
                                                        <%--OnClick="UploadDrawing"--%>
                                                        <asp:LinkButton ID="lnkcheque" runat="server" Text="" OnClick="lnkcheque_Click"> </asp:LinkButton>
                                                        <%--OnClick="lnkDrawing_Click"--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                GST File
                                            </label>
                                            <div class="col-md-2" style="margin-top: 7px;">
                                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="fugst" ClientIDMode="Static" onchange="this.form.submit()" runat="server"
                                                            TabIndex="19" />
                                                        <asp:Button ID="btnGST" Text="Upload" runat="server" Style="display: none" OnClick="UploadGST" />
                                                        <%--OnClick="UploadDrawing"--%>
                                                        <asp:LinkButton ID="lnkGST" runat="server" Text="" OnClick="lnkGST_Click"> </asp:LinkButton>
                                                        <%--OnClick="lnkDrawing_Click"--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                Certificate
                                            </label>
                                            <div class="col-md-2" style="margin-top: 7px;">
                                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="fucertificate" ClientIDMode="Static" onchange="this.form.submit()"
                                                            runat="server" TabIndex="20" />
                                                        <asp:Button ID="btnCertificate" Text="Upload" runat="server" Style="display: none"
                                                            OnClick="UploadCertificate" />
                                                        <%--OnClick="UploadDrawing"--%>
                                                        <asp:LinkButton ID="lnkCertificate" runat="server" Text="" OnClick="lnkCertificate_Click"> </asp:LinkButton>
                                                        <%--OnClick="lnkDrawing_Click"--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <label style="color: lightGray; margin-bottom: auto;">
                                    Please don't upload same file</label>
                                <hr />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <label class=" control-label label-sm ">
                                                    Description
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlItem" CssClass="select2" runat="server" TabIndex="9" AutoPostBack="True"
                                                            Width="100%" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox CssClass="form-control text-left input-sm" ID="txtDesc" ReadOnly="true"
                                                            placeholder="Description" TabIndex="21" MaxLength="50" runat="server" AutoPostBack="false"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                                <label class=" control-label label-sm">
                                                    Qty
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtQty" placeholder="Qty" TabIndex="22"
                                                            runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtQty"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-1">
                                                <label class=" control-label label-sm">
                                                    Rate
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtRate" placeholder="Rate" TabIndex="23"
                                                            runat="server" MaxLength="25" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"
                                                            Enabled="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtRate"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <label class=" control-label label-sm">
                                                    Amount
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtAmount" placeholder="Amount" TabIndex="24"
                                                            runat="server" MaxLength="20" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtAmount"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <label class=" control-label label-sm">
                                                </label>
                                                <br />
                                                <asp:UpdatePanel ID="UpdatePanel112" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="BtnInsert" CssClass="btn blue" TabIndex="25" runat="server" OnClick="BtnInsert_Click">Insert</asp:LinkButton>
                                                        <%--OnClick="BtnInsert_Click"--%>
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
                                                            CssClass="table table-striped table-bordered table-advance table-hover" OnRowCommand="dgInwardMaster_RowCommand"
                                                            OnRowDeleting="dgInwardMaster_Deleting" TabIndex="26">
                                                            <%--OnRowCommand="dgInwardMaster_RowCommand"  OnRowDeleting="dgInwardMaster_Deleting"--%>
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
                                                                <asp:TemplateField HeaderText="IND_CODE" SortExpression="IND_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IND_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IND_SPECIFICATION" SortExpression="IND_SPECIFICATION"
                                                                    HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_SPECIFICATION" CssClass="" runat="server" Visible="True" Text='<%# Eval("IND_SPECIFICATION") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description" SortExpression="IND_DESC" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_DESC" CssClass="" runat="server" Visible="True" Text='<%# Eval("IND_DESC") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty" SortExpression="IND_QTY" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_QTY" CssClass="" runat="server" Text='<%# Eval("IND_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="IND_RATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_RATE" CssClass="" runat="server" Text='<%# Eval("IND_RATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="IND_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_AMT" CssClass="" runat="server" Visible="true" Text='<%# Eval("IND_AMT") %>'></asp:Label>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2 ">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Amount
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblamt" runat="server"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-3" style="margin-top: 7px;">
                                                <label class=" control-label label-sm">
                                                    Quotation Copy
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="fuFile" ClientIDMode="Static" onchange="this.form.submit()" runat="server"
                                                            TabIndex="27" />
                                                        <asp:Button ID="btnFile" Text="Upload" runat="server" Style="display: none" OnClick="UploadFile" />
                                                        <%--OnClick="UploadFile"--%>
                                                        <asp:LinkButton ID="lnkFile" runat="server" Text="" OnClick="lnkFile_Click"> </asp:LinkButton>
                                                        <%--OnClick="lnkFile_Click"--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-3">
                                                <label class=" control-label label-sm ">
                                                    File Description
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-left input-sm" ID="txtFileDesc" placeholder="File Description"
                                                            TabIndex="28" MaxLength="50" runat="server" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnUpload" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgfileupload" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-5">
                                                <label class=" control-label label-sm">
                                                </label>
                                                <br />
                                                <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnUpload" CssClass="btn blue" TabIndex="29" runat="server" OnClick="btnUpload_Click">Upload</asp:LinkButton>
                                                        <%--OnClick="lnkUpload_Click"--%>
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
                                                <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgfileupload" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                            CssClass="table table-striped table-bordered table-advance table-hover" OnRowCommand="dgfileupload_RowCommand"
                                                            OnRowDeleting="dgfileupload_Deleting" OnRowDataBound="dgfileupload_RowDataBound"
                                                            TabIndex="30">
                                                            <%--OnRowCommand="dgfileupload_RowCommand"  OnRowDeleting="dgfileupload_Deleting"--%>
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
                                                                <asp:TemplateField HeaderText="IFU_CODE" SortExpression="IFU_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIFU_CODE" CssClass="" runat="server" Visible="True" Text='<%# Eval("IFU_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="File Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkFiledesc" BorderStyle="None" runat="server" CausesValidation="False"
                                                                            CommandName="ViewFiledesc" Text='<%# Eval("IFU_FILE") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="File Description" SortExpression="IFU_DESC" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIFU_DESC" CssClass="" runat="server" Text='<%# Eval("IFU_DESC") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" SortExpression="IFU_APPROVE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <%--                                                                            <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("IFU_APPROVE"))=="0")? "btn default btn-xs":"btn blue btn-xs" %>'
                                                                                BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                                Text='<%#Eval("IFU_APPROVE").ToString()== "True" ? "Accept" : "" %>'
                                                                               Enabled='<%#Eval("IFU_APPROVE").ToString()== "True" ? true : false %>'CommandArgument='<%#Container.DataItemIndex%>'><i class="fa fa-lock"></i></asp:LinkButton>--%>
                                                                        <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("IFU_APPROVE"))=="True")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                            BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                            Enabled='<%#Eval("IFU_APPROVE").ToString()== "True" ? false  : true%>' Text='<%#Eval("IFU_APPROVE").ToString()== "True" ? "Approve" : "Unapprove" %>'
                                                                            CommandArgument='<%#((GridViewRow)Container).RowIndex%>' OnClick="lnkPost_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-3">
                                            </div>
                                            <label class=" control-label label-sm col-md-2">
                                                <font color="red">*</font> Payment Term
                                            </label>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control " ID="txtPaymentTerm" placeholder="Payment Term"
                                                            TabIndex="31" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:AsyncPostBackTrigger ControlID="dgInwardMaster" EventName="RowCommand" />--%>
                                                        <%-- <asp:AsyncPostBackTrigger ControlID="BtnInsert" EventName="Click" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-actions fluid">
                            <div class="col-md-offset-4 col-md-9">
                                <asp:LinkButton ID="btnApprove" CssClass="btn green" TabIndex="32" runat="server"
                                    OnClick="btnApprove_Click" Visible="false">Approve</asp:LinkButton>
                                <asp:LinkButton ID="btnAuthorize" CssClass="btn green" TabIndex="32" runat="server"
                                    OnClick="btnAuthorize_Click" Visible="false">Authorize</asp:LinkButton>
                                <asp:LinkButton ID="btnReject" CssClass="btn green" TabIndex="33" runat="server"
                                    OnClick="btnReject_Click" Visible="false">Reject</asp:LinkButton>
                                <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                    UseSubmitBehavior="false" CssClass="btn green" TabIndex="34" runat="server" Text="Save"
                                    OnClick="btnSubmit_Click" />
                                <%--OnClick="btnSubmit_Click"--%>
                                <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="35" runat="server"
                                    OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                <%--OnClick="btnCancel_Click"--%>
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
                                                                OnClick="btnOk_Click">  Yes </asp:LinkButton><%-- OnClick="btnOk_Click"--%>
                                                            <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                OnClick="btnCancel1_Click"> No</asp:LinkButton>
                                                            <%--OnClick="btnCancel1_Click"--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel25">
                            <ContentTemplate>
                                <asp:LinkButton ID="LnkDoc" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                <cc1:ModalPopupExtender runat="server" ID="ModalPopDocument" BackgroundCssClass="modalBackground"
                                    OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                    PopupControlID="PopDocument" TargetControlID="LnkDoc">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="PopDocument" runat="server" Style="display: none; width: 90%; height: 100%;">
                                    <div class="portlet box blue" style="width: 90%; height: 100%;">
                                        <div class="portlet-title">
                                            <div class="captionPopup">
                                                Document View
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <div class="form-horizontal">
                                                <div class="form-body">
                                                    <div class="row">
                                                        <div class="col-md-offset-5 col-md-9">
                                                            <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="row" height="100%">
                                                        <div class="col-md-12">
                                                            <iframe runat="server" id="IframeViewPDF" width="100%" style="height: 90vh"></iframe>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel34">
                            <ContentTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                <cc1:ModalPopupExtender runat="server" ID="ModalPopupDetail" BackgroundCssClass="modalBackground"
                                    OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                    PopupControlID="PnlDetail" TargetControlID="LinkButton2">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="PnlDetail" runat="server" Style="display: none; width: 80%; height: 90%;">
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="captionPopup">
                                                Indent Details
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <div class="form-horizontal">
                                                <div class="form-body">
                                                    <div class="row">
                                                        <div class="col-md-offset-5 col-md-9">
                                                            <asp:LinkButton ID="lnkOkBtn" CssClass="btn blue" TabIndex="30" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="overflow: auto;">
                                                        <%--Grid View--%>
                                                        <div class="col-md-12">
                                                            <asp:GridView ID="dgDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                ForeColor="#333333" GridLines="None" DataKeyNames="" Font-Names="Verdana" Font-Size="12px"
                                                                ShowFooter="false" PageSize="15" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                OnPageIndexChanging="dgDetail_PageIndexChanging" AllowPaging="True">
                                                                <Columns>
                                                                    <%-- here we insert database field for bind and eval--%>
                                                                    <asp:TemplateField HeaderText="Party Name" SortExpression="IN_SUPP_NAME" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIN_SUPP_NAME" runat="server" Text='<%# Eval("IN_SUPP_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Indent No." SortExpression="IN_TNO" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIN_TNO" runat="server" Text='<%# Eval("IN_TNO") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Indent Date" SortExpression="IN_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIN_DATE" runat="server" Text='<%# Eval("IN_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Used In" SortExpression="IN_USEDIN" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIN_USEDIN" runat="server" Text='<%# Eval("IN_USEDIN") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" SortExpression="IND_AMT" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIND_AMT" runat="server" Text='<%# Eval("IND_AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                            <%-- End Grid View--%>
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
    <!-- BEGIN PAGE LEVEL
    PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE
    LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtIndentDate.ClientID%>', '#Avisos') == false)
                { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=txtBudgetNo.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else if (VerificaObrigatorio('#<%=ddlProject.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); return false; } else {
                    return
                    true;
                }
            } catch (err) {
                alert('Erro in Required Fields: ' + err.description); return
                false;
            }
        } </script>

</asp:Content>
