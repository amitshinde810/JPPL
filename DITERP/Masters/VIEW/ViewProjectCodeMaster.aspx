<%@ Page Title="Project Code Master" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewProjectCodeMaster.aspx.cs" Inherits="Masters_VIEW_ViewProjectCodeMaster" %>

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
                <div class="col-md-2">
                </div>
                <div id="MSG" class="col-md-8">
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
            <br>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Project Code Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnClose_Click"> </asp:LinkButton>
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
                                        <asp:LinkButton ID="lnkInsert" CssClass="btn green" runat="server" OnClick="btnInsert_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                    </div>
                                    <div class="pull-right">
                                        &nbsp</div>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="upnlGridView">
                                <ContentTemplate>
                                    <asp:GridView ID="dgProCode" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        GridLines="Both" DataKeyNames="PROCM_CODE" CssClass="table table-striped table-bordered table-advance table-hover"
                                        ShowFooter="false" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing"
                                        OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand"
                                        AllowPaging="true" PageSize="15">
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
                                                                    <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                        CommandName="View" Text="" CommandArgument='<%# Bind("PROCM_CODE") %>'>
                                                                       <i class="fa fa-search"></i>View</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkModify" BorderStyle="None" runat="server" CausesValidation="False"
                                                                        CommandName="Modify" Text="" CommandArgument='<%# Bind("PROCM_CODE") %>'>
                                                                          <i class="fa fa-edit"></i>Modify</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CausesValidation="False"
                                                                        OnClientClick="return confirm('Are you sure,you want to Delete?');" CommandName="Delete"
                                                                        Text=""><i class="fa fa-trash-o"></i>Delete</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkAmend" runat="server" CausesValidation="False" CommandName="Amend"
                                                                        Visible="True" Text="Amend" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-link"></i> Amend</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkAuthorize" runat="server" CausesValidation="False" CommandName="Authorize"
                                                                        Text="Authorize" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Authorize</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project Code" SortExpression="PROCM_CODE" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROCM_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("PROCM_CODE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project Code" SortExpression="PROCM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROCM_NAME" CssClass="formlabelUpper" runat="server" Text='<%# Eval("PROCM_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Project Budget" SortExpression="PROCM_AMT" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROCM_AMT" CssClass="formlabelUpper" runat="server" Text='<%# Eval("PROCM_AMT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="Project Code Status" SortExpression="PROCM_APPROVE" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROCM_APPROVE" CssClass="formlabelUpper" runat="server" Text='<%#  Eval("PROCM_APPROVE").ToString()== "True" ? "Approve" : "Open" %>' ></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerStyle CssClass="pgr" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtString" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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
