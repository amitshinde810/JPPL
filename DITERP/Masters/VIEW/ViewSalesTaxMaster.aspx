<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewSalesTaxMaster.aspx.cs"
    Inherits="Masters_VIEW_ViewSectorMaster" Title="Sales Tax Master" %>

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
            <div id="Avisos" runat="server">
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sales Tax Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="btn-group">
                                        <label class="control-label">
                                            Search
                                        </label>
                                    </div>
                                    <div class="btn-group">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtString" runat="server" CssClass="form-control input-medium" TabIndex="3"
                                                OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="btn-group pull-right">
                                        <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                    </div>
                                    <div class="pull-right">
                                        &nbsp
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel100" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgSalesTaxMaster" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                        CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="ST_CODE"
                                        OnRowDeleting="dgSalesTaxMaster_RowDeleting" OnRowEditing="dgSalesTaxMaster_RowEditing"
                                        OnPageIndexChanging="dgSalesTaxMaster_PageIndexChanging" OnRowCommand="dgSalesTaxMaster_RowCommand "
                                        OnRowUpdating="dgSalesTaxMaster_RowUpdating" AllowPaging="true" PageSize="15"
                                        on>
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
                                                                        Text="View" CommandArgument='<%# Bind("ST_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                        Text="Modify" CommandArgument='<%# Bind("ST_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        Text="Delete" OnClientClick="return confirm('Are you sure, you want to Delete?');"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ST_CODE" SortExpression="ST_CODE" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_CODE" runat="server" Text='<%# Bind("ST_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Name" SortExpression="ST_TAX_NAME" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_TAX_NAME" CssClass="" runat="server" Text='<%# Bind("ST_TAX_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Alias" SortExpression="ST_ALIAS" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_ALIAS" CssClass="" runat="server" Text='<%# Bind("ST_ALIAS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sales Tax" SortExpression="ST_SALES_TAX" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_SALES_TAX" CssClass="" runat="server" Text='<%# Bind("ST_SALES_TAX") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TCS Tax" SortExpression="ST_TCS_TAX" HeaderStyle-HorizontalAlign="Right"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_TCS_TAX" CssClass="" runat="server" Text='<%# Bind("ST_TCS_TAX") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Form No" SortExpression="ST_FORM_NO" HeaderStyle-HorizontalAlign="Right"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_FORM_NO" CssClass="" runat="server" Text='<%# Bind("ST_FORM_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Acc. Head(S)" SortExpression="ST_SALES_ACC_HEAD"
                                                HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_SALES_ACC_HEAD" CssClass="" runat="server" Text='<%# Bind("ST_SALES_ACC_HEAD") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Acc. Head(P)" SortExpression="ST_TAX_ACC_HEAD"
                                                HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblST_TAX_ACC_HEAD" CssClass="" runat="server" Text='<%# Bind("ST_TAX_ACC_HEAD") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
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
            <div class="col-md-offset-10">
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

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
