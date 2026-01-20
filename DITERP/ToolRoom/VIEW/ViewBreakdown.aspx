<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewBreakdown.aspx.cs"
    Inherits="ToolRoom_VIEW_ViewBreakdown" Title="Breakdown Entry " %>

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
                                <i class="fa fa-reorder"></i>Breakdown Entry
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
                                            <label class="col-md-1 control-label">
                                                Search
                                            </label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtString" runat="server" CssClass="form-control" TabIndex="3" OnTextChanged="txtString_TextChanged"
                                                    onkeyup="RefreshUpdatePanel();"></asp:TextBox>
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
                            <div class="row">
                                <div class="col-md-12">
                                    <div style="overflow: auto;">
                                        <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgBreakdown" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="B_CODE"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                                    OnRowEditing="dgBreakdown_RowEditing" OnRowCommand="dgBreakdown_RowCommand" OnRowDeleting="dgBreakdown_RowDeleting"
                                                    OnPageIndexChanging="dgBreakdown_PageIndexChanging" PageSize="15">
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
                                                                                    Text="View" CommandArgument='<%# Bind("B_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                    Text="Modify" CommandArgument='<%# Bind("B_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                                    CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" Visible="true"
                                                                                    CommandName="Print" Text="Print" CommandArgument='<%# Bind("B_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="B_CODE" SortExpression="B_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblB_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("B_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Breakdown No." SortExpression="B_NO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblB_NO" runat="server" Text='<%# Eval("B_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Breakdown Slip No." Visible="false" SortExpression="B_SLIPNO"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblB_SLIPNO" runat="server" Text='<%# Eval("B_SLIPNO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Breakdown Date" SortExpression="B_DATE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblB_DATE" runat="server" Text='<%# Eval("B_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool Type" Visible="true" SortExpression="T_TYPE"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblT_TYPE" CssClass=" Control-label" runat="server" Text='<%# Eval("T_TYPE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool No." SortExpression="T_NAME" Visible="true" HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIM_MATERIAL_REQ1" CssClass=" Control-label pull-right" runat="server"
                                                                    Text='<%# Eval("T_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part Name & No." SortExpression="I_CODENO" Visible="true"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Report" SortExpression="IM_CODE" Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblB_FILE" runat="server" Text='<%# Eval("B_FILE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" SortExpression="B_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("B_STATUS"))=="1")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                    BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                    Text='<%#  Eval("B_STATUS").ToString()== "1" ? "Close" : "Open" %>' CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
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

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
