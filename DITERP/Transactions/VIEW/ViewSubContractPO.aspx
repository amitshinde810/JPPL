<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewSubContractPO.aspx.cs"
    Inherits="Transactions_VIEW_ViewSubContractPO" Title="Sub Contract PO " %>

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
    
      <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlFilter.ClientID %>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sub Contract Purchase Order
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnClose_Click"> </asp:LinkButton>
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
                                                <asp:TextBox ID="txtString" runat="server" CssClass="form-control input-medium" TabIndex="3"
                                                    OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlFilter" CssClass="select2" Width="100%" runat="server"
                                                    AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="ShortClose" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Cancel" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
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
                                    <asp:GridView ID="dgDetailSupplierPO" runat="server" AutoGenerateColumns="False"
                                        Width="100%" CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both"
                                        DataKeyNames="SPOM_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                        AllowPaging="True" OnRowEditing="dgDetailSupplierPO_RowEditing" OnRowCommand="dgDetailSupplierPO_RowCommand"
                                        OnRowDeleting="dgDetailSupplierPO_RowDeleting" OnPageIndexChanging="dgDetailSupplierPO_PageIndexChanging"
                                        PageSize="15">
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
                                                                        Text="View" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                        Text="Modify" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        Text="Delete" OnClientClick="return confirm('Are you sure,you want to Delete?');"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                        Text="Print" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkPost" runat="server" CausesValidation="False" CommandName="Post"
                                                                        Text="Post" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-envelope"></i> Post</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkAmend" runat="server" CausesValidation="False" CommandName="Amend"
                                                                        Visible="True" Text="Amend" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-link"></i> Amend</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkAuthorize" runat="server" CausesValidation="False" CommandName="Authorize"
                                                                        Text="Amend" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Authorize</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkShortClose" runat="server" CausesValidation="False" CommandName="ShortClose"
                                                                        Text="ShortClose" CommandArgument='<%# Bind("SPOM_CODE") %>'><i class="fa fa-ban"></i> Short Close</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkCancelPo" runat="server" CausesValidation="False" CommandName="CancelPO"
                                                                        Text="CancelPO" CommandArgument='<%# Bind("SPOM_CODE") %>' OnClientClick="return confirm('Are you sure,you want to Cancel PO?');"><i class="fa fa-ban"></i> Cancel</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CPOM_CODE" SortExpression="CPOM_CODE" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCPOM_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("SPOM_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO No." SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCPOM_PONO" runat="server" Text='<%# Eval("SPOM_PO_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier Name" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCM_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Material Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Date" SortExpression="CPOM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCPOM_DATE" CssClass=" Control-label" runat="server" Text='<%# Eval("SPOM_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Po. Qty" SortExpression="SPOD_ORDER_QTY" Visible="false"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPOD_ORDER_QTY" CssClass=" Control-label pull-right" runat="server"
                                                        Text='<%# Eval("SPOD_ORDER_QTY") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bal. Qty" SortExpression="SPOD_INW_QTY" Visible="false"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPOD_INW_QTY" CssClass=" Control-label pull-right" runat="server"
                                                        Text='<%# Eval("SPOD_INW_QTY") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Post" SortExpression="SPOM_POST" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPOM_POST" CssClass=" Control-label pull-left" runat="server" Text='<%# Eval("SPOM_POST") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Authorize" SortExpression="SPOM_AUTHR_FLAG" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAuthorize" CssClass=" Control-label pull-left" runat="server" Text='<%# Eval("SPOM_AUTHR_FLAG") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SPOM_CANCEL_PO" SortExpression="SPOM_CANCEL_PO" Visible="false"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPOM_CANCEL_PO" CssClass=" Control-label pull-left" runat="server"
                                                        Text='<%# Eval("SPOM_CANCEL_PO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SPOM_IS_SHORT_CLOSE" SortExpression="SPOM_IS_SHORT_CLOSE"
                                                Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPOM_IS_SHORT_CLOSE" CssClass=" Control-label pull-left" runat="server"
                                                        Text='<%# Eval("SPOM_IS_SHORT_CLOSE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-left" />
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
        </div>
    </div>
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
    
    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
