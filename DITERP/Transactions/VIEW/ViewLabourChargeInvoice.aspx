<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewLabourChargeInvoice.aspx.cs"
    Inherits="Transactions_VIEW_ViewLabourChargeInvoice" Title="Labour Charge Invoice/Delivery Challan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
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
                                <i class="fa fa-reorder"></i>Labour Charge Invoice/Delivery Challan
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label class="control-label col-md-1">
                                                Search
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtString" runat="server" CssClass="form-control" TabIndex="3"
                                                    OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel65">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" TabIndex="1"
                                                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtGroup_SelectedIndexChanged"
                                                            CssClass="checker" CellPadding="10">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp;Invoice</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp;Challan</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="btn-group pull-right">
                                                <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                            </div>
                                            <div class="pull-right">
                                                &nbsp
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="dgInvoiceDettail" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="INM_CODE"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                        OnRowCommand="dgInvoiceDettail_RowCommand" OnRowDeleting="dgInvoiceDettail_RowDeleting"
                                        OnPageIndexChanging="dgInvoiceDettail_PageIndexChanging" PageSize="15">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <div class="clearfix">
                                                        <div class="btn-group">
                                                            <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                Select <i class="fa fa-angle-down"></i>
                                                            </button>
                                                            <ul class="dropdown-menu" role="menu">
                                                                <li>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                        Text="View" CommandArgument='<%# Bind("INM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                        Text="Modify" CommandArgument='<%# Bind("INM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        Text="Delete" OnClientClick="return confirm('Are you sure,you want to Delete?');"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                        Text="Print" CommandArgument='<%# Bind("INM_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkMultiple" runat="server" Visible="false" CausesValidation="False" CommandName="PrintMult"
                                                                        Text="Print Multiple" CommandArgument='<%# Bind("INM_CODE") %>'><i class="fa fa-print"></i> Print Multiple</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="INM_CODE" SortExpression="INM_CODE" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblINM_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("INM_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice No." SortExpression="INM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblINM_NO" runat="server" Text='<%# Eval("INM_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name" SortExpression="INM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Date" SortExpression="INM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblINM_DATE" CssClass=" Control-label" runat="server" Text='<%# Eval("INM_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerStyle CssClass="pgr" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                    <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                        OnOkScript="oknumber()" CancelControlID="Button7" DynamicServicePath="" Enabled="True"
                        PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="popUpPanel5" runat="server" Width="500px">
                        <div class="portlet box blue">
                            <div class="portlet-title">
                                <div class="captionPopup">
                                    Invoice Print Selection
                                </div>
                            </div>
                            <div class="portlet-body form">
                                <div class="form-horizontal">
                                    <div class="form-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-6 text-right" id="Label1" runat="server" visible="false">
                                                        Invoice For:
                                                    </label>
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelgf7">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" ID="chlSupp" Text="&nbspSuppliementory Invoice" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <label class="col-md-6 control-label">
                                                                Invoice Print Option:
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="col-md-6">
                                                        <asp:UpdatePanel runat="server" ID="Updpanel1">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlPrintOpt" CssClass="form-control" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlPrintOpt_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Text="Select Print Option"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Selected="True" Text="Printed Material"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Plain Print"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="Excise Print"></asp:ListItem>
                                                                     <asp:ListItem Value="4" Text="E Invocie Print"></asp:ListItem>
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
                                                    <label class="col-md-6 text-right" id="lblPrintCopy" runat="server">
                                                        Invoice Print Copies:
                                                    </label>
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePuiu">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" ID="chkPrintCopy1" OnCheckedChanged="chk1_CheckedChanged"
                                                                    AutoPostBack="true" Text="1" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" ID="chkPrintCopy2" Text="2" OnCheckedChanged="chk1_CheckedChanged"
                                                                    AutoPostBack="true" Checked="true" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" ID="chkPrintCopy3" Text="3" OnCheckedChanged="chk1_CheckedChanged"
                                                                    AutoPostBack="true" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                            <ContentTemplate>
                                                                <asp:CheckBox runat="server" ID="chkPrintCopy4" Text="4" OnCheckedChanged="chk1_CheckedChanged"
                                                                    AutoPostBack="true" CssClass="checker" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-offset-4 col-md-9">
                                                <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                    OnClick="btnOk_Click">  OK </asp:LinkButton>
                                                <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="col-md-offset-10">
            </div>
        </div>
    </div>
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />
    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script> <script
    src="../../assets/JS/Util.js" type="text/javascript"></script> <script src="../../assets/scripts/jquery-1.7.1.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-1.10.2.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js"
    type="text/javascript"></script> <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js
    before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
    --> <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery.blockui.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery.cokie.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/uniform/jquery.uniform.min.js"
    type="text/javascript"></script> <!-- END CORE PLUGINS --> <!-- BEGIN PAGE LEVEL
    PLUGINS --> <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js"
    type="text/javascript"></script> <script src="../../assets/plugins/gritter/js/jquery.gritter.js"
    type="text/javascript"></script> <!-- END PAGE LEVEL PLUGINS --> <!-- BEGIN PAGE
    LEVEL SCRIPTS --> <script src="../../assets/scripts/app.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
