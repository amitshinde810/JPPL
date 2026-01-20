<%@ Page Title="Customer Rejection" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewIRNCustomerRejection.aspx.cs" Inherits="Transactions_VIEW_ViewIRNCustomerRejection" %>

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
            <br>
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Customer Rejection
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
                                            <div class="col-md-6">
                                                <asp:LinkButton ID="btnAddNew" Visible="false" CssClass="btn green" runat="server"
                                                    OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                                <asp:RadioButtonList ID="rbType" CssClass="checker" runat="server" AutoPostBack="true" 
                                                    RepeatDirection="Horizontal" 
                                                    onselectedindexchanged="rbType_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">Pending&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="1">Analysed</asp:ListItem>
                                                </asp:RadioButtonList>
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
                                                <asp:GridView ID="dgCustRej" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="CRM_CODE"
                                                    OnRowDeleting="dgCustRej_RowDeleting" OnRowEditing="dgCustRej_RowEditing" OnPageIndexChanging="dgCustRej_PageIndexChanging"
                                                    OnRowCommand="dgCustRej_RowCommand" OnRowDataBound="dgCustRej_RowDataBound" OnRowUpdating="dgCustRej_RowUpdating"
                                                    AllowPaging="true" PageSize="15">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px"
                                                            Visible="true">
                                                            <ItemTemplate>
                                                                <div class="clearfix">
                                                                    <div class="btn-group">
                                                                        <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                            Select <i class="fa fa-angle-down"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu" role="menu">
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                                    Text="View" CommandArgument='<%# Bind("CRM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" Visible="false" runat="server" CausesValidation="False"
                                                                                    CommandName="Modify" Text="Modify" CommandArgument='<%# Bind("CRM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" Visible="false" runat="server" CausesValidation="False"
                                                                                    CommandName="Delete" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                                    Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" Visible="false"
                                                                                    CommandName="Print" Text="Print" Width="50px" CommandArgument='<%# Bind("CRM_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkAdd" BorderStyle="None" runat="server" CausesValidation="False"
                                                                                    CommandName="Add" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Add</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Rej. No." SortExpression="CRM_NOCHAR" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRM_NOCHAR" runat="server" Text='<%# Bind("CRM_NOCHAR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CRM_CODE" SortExpression="CRM_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRM_CODE" runat="server" Text='<%# Bind("CR_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date" SortExpression="CRM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRM_DOC_DATE" CssClass="" runat="server" Text='<%# Eval("CRM_DOC_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GIN No." SortExpression="CRM_NO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCR_GIN_NO" CssClass="" runat="server" Text='<%# Eval("CR_GIN_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GIN Date" SortExpression="CRM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCR_GIN_DATE" CssClass="" runat="server" Text='<%# Eval("CR_GIN_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" SortExpression="CD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCD_I_CODE" CssClass="" runat="server" Text='<%# Eval("CD_I_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty" SortExpression="CD_CHALLAN_QTY" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCD_CHALLAN_QTY" CssClass="" runat="server" Text='<%# Eval("CD_CHALLAN_QTY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Code" SortExpression="P_CODE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblP_CODE" CssClass="" runat="server" Text='<%# Eval("P_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblP_NAME" CssClass="" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" SortExpression="CD_CHK_REJ_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("CD_CHK_REJ_STATUS"))=="1")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                    BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                    Text='<%#Eval("CRM_STATUS").ToString()== "1" ? "Close" : "Open" %>' CommandArgument='<%#((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Iwd Cust. D. Status" SortExpression="CD_CHK_REJ_STATUS"
                                                            Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCD_CHK_REJ_STATUS" CssClass="" runat="server" Text='<%# Eval("CD_CHK_REJ_STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Master Status" SortExpression="CRM_STATUS" Visible="false"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCRM_STATUS" CssClass="" runat="server" Text='<%# Eval("CRM_STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
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
                <!-- END JAVASCRIPTS -->
            </div>
        </div>
    </div>
</asp:Content>
