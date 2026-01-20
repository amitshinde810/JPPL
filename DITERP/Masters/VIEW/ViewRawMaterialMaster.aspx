<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewRawMaterialMaster.aspx.cs"
    Inherits="Masters_VIEW_ViewRawMaterialMaster" Title="Item Master" %>

<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
    </script>

    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />
    <style>
        li.active
        {
            color: red;
        }
    </style>
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
                                <i class="fa fa-reorder"></i>Item Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnCancel" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
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
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div style="overflow: auto;">
                                        <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgRawMaterial" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="I_CODE"
                                                    OnRowDeleting="dgRawMaterial_RowDeleting" OnPageIndexChanging="dgRawMaterial_PageIndexChanging"
                                                    OnRowCommand="dgRawMaterial_RowCommand" AllowPaging="true" PageSize="25">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div class="clearfix">
                                                                    <div class="btn-group">
                                                                        <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                            Select <i class="fa fa-angle-down"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu" role="menu" c  >
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                                    Text="View" CommandArgument='<%# Bind("I_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                    Text="Modify" CommandArgument='<%# Bind("I_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure,you want to Delete?');"
                                                                                    CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="I_CODE" SortExpression="I_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Name" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Unit" SortExpression="I_UOM_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_UOM_NAME" CssClass="" runat="server" Text='<%# Eval("I_UOM_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Category" SortExpression="I_CAT_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CAT_NAME" CssClass="" runat="server" Text='<%# Eval("I_CAT_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Inventory Category" SortExpression="I_INV_CAT" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_INV_CAT" CssClass="" runat="server" Text='<%# Eval("I_INV_CAT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Current Balance" SortExpression="I_CURRENT_BAL" HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_CURRENT_BAL" CssClass="Control-label pull-right" runat="server"
                                                                    Text='<%# Eval("I_CURRENT_BAL") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dispatch Balance" SortExpression="I_DISPATCH_BAL"
                                                            HeaderStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblI_DISPATCH_BAL" CssClass="Control-label pull-right" runat="server"
                                                                    Text='<%# Eval("I_DISPATCH_BAL") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Drawing Image" Visible="true" SortExpression="I_DRAWING_PATH"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView1" BorderStyle="None" runat="server" CausesValidation="False"
                                                                    CommandName="ViewPDF1" Text='<%# Eval("I_DRAWING_PATH") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part Photo" SortExpression="I_PHOTO_PATH" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView2" BorderStyle="None" runat="server" CausesValidation="False"
                                                                    CommandName="ViewPDF" Text='<%# Eval("I_PHOTO_PATH") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
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
                            
                              <asp:UpdatePanel runat="server" ID="UpdatePanel39">
                                <ContentTemplate>
                                    <asp:LinkButton ID="LnkDoc" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:modalpopupextender runat="server" id="ModalPopDocument" backgroundcssclass="modalBackground"
                                        onokscript="oknumber()" oncancelscript="oncancel()" dynamicservicepath="" enabled="True"
                                        popupcontrolid="PopDocument" targetcontrolid="LnkDoc">
                                </cc1:modalpopupextender>
                                    <asp:Panel ID="PopDocument" runat="server" Style="display: none;">
                                        <div class="portlet box blue">
                                            <div class="portlet-title">
                                                <div class="captionPopup">
                                                    Document View
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="form-horizontal">
                                                    <div class="form-body">
                                                        <div class="row">
                                                            <label class="col-md-12 control-label">
                                                            </label>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <iframe runat="server" id="IframeViewPDF" width="600px" height="400px"></iframe>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-offset-5 col-md-9">
                                                                <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server"> Ok</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--1 New file added by suja-->

    <script src="../../assets/JS/jquery-1.11.3.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

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

    <script type="text/javascript"> // var jQuery = jQuery.noConflict(); $(document).ready(function()
    { function Showalert() { $('#MSG').fadeIn(6000) $('#MSG').fadeOut(6000) $("#up").click();
    } var selector = '.nav li'; $(selector).on('click', function() { $(selector).removeClass('active');
    $(this).addClass('active'); }); }) </script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <!--
    END JAVASCRIPTS -->
</asp:Content>
