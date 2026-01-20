<%@ Page Title="Bill OF Coimponent" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="VeiwBOComponent.aspx.cs" Inherits="Masters_VIEW_VeiwBOComponent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Bill of Components
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a><a href="javascript:;" class="remove">
                                </a>
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
                                    <div class="btn-group">
                                        <button class="btn dropdown-toggle" data-toggle="dropdown">
                                            Tools <i class="fa fa-angle-down"></i>
                                        </button>
                                        <ul class="dropdown-menu pull-right">
                                            <li><a href="#">Print</a> </li>
                                            <li><a href="#">Save as PDF</a> </li>
                                            <li><a href="#">Export to Excel</a> </li>
                                        </ul>
                                    </div>
                                    <div class="btn-group pull-right">
                                        <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                    </div>
                                    <div class="pull-right">
                                        &nbsp
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="dgBillofComponent" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                        CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="BM_CODE"
                                        OnRowDeleting="dgBillofComponent_RowDeleting" OnRowEditing="dgBillofComponent_RowEditing"
                                        OnPageIndexChanging="dgBillofComponent_PageIndexChanging" OnRowCommand="dgBillofComponent_RowCommand"
                                        OnRowUpdating="dgBillofComponent_RowUpdating" AllowPaging="true" PageSize="15">
                                        <Columns>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                        CausesValidation="False" CommandName="View" Text="" CommandArgument='<%# Bind("BM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                        CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# Bind("BM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" CssClass="btn red btn-xs" BorderStyle="None" runat="server"
                                                        CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                        CommandName="Delete" Text=""><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BM_CODE" SortExpression="BM_CODE" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBM_CODE" runat="server" Text='<%# Bind("BM_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Finish Component Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Finish Component Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
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
