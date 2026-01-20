<%@ Page Title="Customer Rejection Inward" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewCustRej.aspx.cs" Inherits="IRN_VIEW_ViewCustRej" %>

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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Customer Rejection Inward
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" runat="server" CssClass="remove" OnClick="btnCancel_Click"></asp:LinkButton>
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
                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="dgCustomerRejection" runat="server" AutoGenerateColumns="False"
                                        Width="100%" CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both"
                                        DataKeyNames="CR_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                        AllowPaging="True" OnRowEditing="dgCustomerRejection_RowEditing" OnRowCommand="dgCustomerRejection_RowCommand"
                                        OnRowDeleting="dgCustomerRejection_RowDeleting" OnPageIndexChanging="dgCustomerRejection_PageIndexChanging"
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
                                                                        Text="View" CommandArgument='<%# Bind("CR_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                        Text="Modify" CommandArgument='<%# Bind("CR_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                        CausesValidation="False" CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                        Text="Print" CommandArgument='<%# Bind("CR_CODE") %>'><i class="fa fa-print"></i> Print Cuctomer Rejection</asp:LinkButton></li>
                                                                <li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CR CODE" SortExpression="CR_CODE" Visible="false"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("CR_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gin No." SortExpression="CR_GIN_NO" Visible="true"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_GIN_NO" CssClass="formlabel" runat="server" Text='<%# Bind("CR_GIN_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gin Date" SortExpression="CR_GIN_DATE" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_GIN_DATE" runat="server" Text='<%# Eval("CR_GIN_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" SortExpression="CR_TYPE" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_TYPE" runat="server" Text='<%# Eval("CR_TYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No." SortExpression="CR_CHALLAN_NO" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_CHALLAN_NO" CssClass="Control-label" runat="server" Text='<%# Eval("CR_CHALLAN_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan Date" SortExpression="CR_CHALLAN_DATE" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_CHALLAN_DATE" CssClass="Control-label" runat="server" Text='<%# Eval("CR_CHALLAN_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P CODE" SortExpression="CR_P_CODE" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCR_P_CODE" CssClass=" Control-label" runat="server" Text='<%# Eval("CR_P_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblP_NAME" CssClass=" Control-label" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
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

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

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