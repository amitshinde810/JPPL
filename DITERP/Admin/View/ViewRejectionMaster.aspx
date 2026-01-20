<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewRejectionMaster.aspx.cs"
    Inherits="Admin_View_ViewRejectionMaster" Title="Rejection Master" %>

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
                <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div id="MSG" class="col-md-7">
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
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-7">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Rejection Master
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
                                            <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                        </div>
                                        <div class="pull-right">
                                            &nbsp
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel100" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgRejectionMaster" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                            CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="RM_CODE"
                                            OnRowDeleting="dgRejectionMaster_RowDeleting" OnRowEditing="dgRejectionMaster_RowEditing"
                                            OnPageIndexChanging="dgRejectionMaster_PageIndexChanging" OnRowCommand="dgRejectionMaster_RowCommand "
                                            OnRowUpdating="dgRejectionMaster_RowUpdating" AllowPaging="true" PageSize="15"
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
                                                                            Text="" CommandArgument='<%# Bind("RM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                            Text="" CommandArgument='<%# Bind("RM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                            CommandName="Delete" Text=""><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SCT_CODE" SortExpression="SCT_CODE" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_CODE" runat="server" Text='<%# Bind("RM_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MELTING LOSS" SortExpression="RM_MELTING_LOSS" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_MELTING_LOSS" CssClass="" runat="server" Text='<%# Eval("RM_MELTING_LOSS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CASTING REJECTION" SortExpression="RM_MELTING_LOSS"
                                                    HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_CASTING_REJECTION" CssClass="" runat="server" Text='<%# Eval("RM_CASTING_REJECTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MACHINING REJECTION" SortExpression="RM_MELTING_LOSS"
                                                    HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_MACHINING_REJECTION" CssClass="" runat="server" Text='<%# Eval("RM_MACHINING_REJECTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CORE BREAKAGE" SortExpression="RM_MELTING_LOSS" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_CORE_BREAKAGE" CssClass="" runat="server" Text='<%# Eval("RM_CORE_BREAKAGE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SAND WASTAGE" SortExpression="RM_MELTING_LOSS" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRM_SAND_WASTAGE" CssClass="" runat="server" Text='<%# Eval("RM_SAND_WASTAGE") %>'></asp:Label>
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
</asp:Content>
