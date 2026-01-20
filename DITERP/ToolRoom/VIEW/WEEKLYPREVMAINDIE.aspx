<%@ Page Title="Weekly Preventive maintenance" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="WEEKLYPREVMAINDIE.aspx.cs" Inherits="ToolRoom_VIEW_WEEKLYPREVMAINDIE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
    </script>

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
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Weekly Preventive maintenance
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
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
                                                <asp:GridView ID="dgWPM" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="WPM_CODE"
                                                    OnRowDeleting="dgWPM_RowDeleting" OnRowEditing="dgWPM_RowEditing" OnPageIndexChanging="dgWPM_PageIndexChanging"
                                                    OnRowCommand="dgWPM_RowCommand" OnRowUpdating="dgWPM_RowUpdating" AllowPaging="true"
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
                                                                                    Text="View" CommandArgument='<%# Bind("WPM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                    Text="Modify" CommandArgument='<%# Bind("WPM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                                    Text="Print" CommandArgument='<%# Bind("WPM_CODE") %>'><i class="fa fa-edit"></i> Print</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                    Text="Delete" OnClientClick="return confirm('Are you sure,you want to Delete?');"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WPM_CODE" SortExpression="WPM_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_CODE" runat="server" Text='<%# Bind("WPM_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Slip No." SortExpression="WPM_SLIP_NO" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_SLIP_NO" CssClass="" runat="server" Text='<%# Eval("WPM_SLIP_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool No." SortExpression="T_NAME" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblT_NAME" CssClass="" runat="server" Text='<%# Eval("T_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month" SortExpression="WPM_MONTH" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_MONTH" CssClass="" runat="server" Text='<%# Eval("WPM_MONTH") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Week" SortExpression="WPM_WEEK" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_WEEK" CssClass="" runat="server" Text='<%# Eval("WPM_WEEK") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part No." SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tool handOver Date" SortExpression="WPM_TOOL_RECEIVED_DATE"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_TOOL_RECEIVED_DATE" CssClass="" runat="server" Text='<%# Eval("WPM_TOOL_RECEIVED_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exp. Comp Date" SortExpression="WPM_TOOL_EXPCOMP_DATE"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPM_TOOL_EXPCOMP_DATE" CssClass="" runat="server" Text='<%# Eval("WPM_TOOL_EXPCOMP_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" SortExpression="B_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("WPM_STATUS"))=="1")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                    BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                    Text='<%#  Eval("WPM_STATUS").ToString()== "1" ? "Close" : "Open" %>' CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
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
