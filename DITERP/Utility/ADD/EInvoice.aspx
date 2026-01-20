<%@ Page Title=" E Invoice Export and Import" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="EInvoice.aspx.cs" Inherits="Utility_ADD_EInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 

 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
 

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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlFromInvNo.ClientID %>").select2();
            jQuery("#<%=ddlToInvNo.ClientID %>").select2();

        });
    </script>

        <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; margin-bottom: 10px; border: 1px solid #9f6000">
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
                                <i class="fa fa-reorder"></i>Create E Invoice Export Import
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                           
                                            <label class="col-md-4 control-label text-right">
                                                Invoice Type
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel19">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" TabIndex="1"
                                                            RepeatDirection="Horizontal" CssClass="checker" CellPadding="10" OnSelectedIndexChanged="rbtGroup_SelectedIndexChanged">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp;Tax Invoice</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp;Labour Invoice</asp:ListItem> 
                                                              <asp:ListItem Value="2">&nbsp;Labour Challan</asp:ListItem>
                                                               <asp:ListItem Value="3">&nbsp;Supplementary </asp:ListItem> 
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Invoice No. From
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlFromInvNo" CssClass="select2" runat="server" Width="100%"
                                                            MsgObrigatorio="Invoice No" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rbtGroup" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-1 control-label text-right ">
                                                To
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlToInvNo" CssClass="select2" runat="server" Width="100%"
                                                            MsgObrigatorio="Invoice No" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rbtGroup" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </div>
                                   
                                    
                                </div>
                            </div>
                        
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnExport" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="btnExport_Click"><i class="fa fa-check-square"> </i>  Export Type A </asp:LinkButton>
                                        
                                        
                                        <asp:LinkButton ID="btnExportB" CssClass="btn green" TabIndex="15" runat="server"  
                                        OnClick="btnExportB_Click"><i class="fa fa-check-square"> </i>  Export Type B Master </asp:LinkButton>
                                        
                                         <asp:LinkButton ID="btnExportBDetails" CssClass="btn green" TabIndex="15" runat="server"  
                                        OnClick="btnExportBDetails_Click"><i class="fa fa-check-square"> </i>  Export Type B Items </asp:LinkButton>
                                        
                                        
                                   <%-- <asp:LinkButton ID="btnDownload" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="lb_DownloadXML_Click"><i class="fa fa-check-square"> </i>  Download </asp:LinkButton>--%>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                            
                             <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-4 control-label text-right">
                                            Select File For E Invoice Upload
                                            </label>
                                            <label class="col-md-2 control-label text-right">
                                                 <asp:FileUpload ID="FileUpload2" runat="server" TabIndex="3" CssClass="st"  />
                                            </label>
                                            <div class="col-md-2">
                                                  <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn dark" ValidationGroup="Load"
                                                    TabIndex="4" OnClick="btnImport1_Click"><i class="fa fa-download">&nbsp;Import Excel</i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                  
                                   
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT-->
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script>
<![endif]-->
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

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
