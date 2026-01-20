<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="StockCompare.aspx.cs"
    Inherits="Transactions_ADD_StockCompare" Title="Stock Error Correction Utility - Upload Physical Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function() {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function() {
            ShowProgress();
        });
   </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlToStore.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .test tr input
        {
            border: 1px solid red;
            margin-right: 10px;
            padding-right: 10px;
        }
    </style>
    <style type="text/css">
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        .loading2
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 1;
            filter: alpha(opacity=1);
            -moz-opacity: 1;
            min-height: 100%;
            width: 100%;
            padding-top: 14%;
        }
        .loading
        {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            display: none;
            position: absolute;
            background-color: White;
            z-index: 999;
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
                                <i class="fa fa-reorder"></i>Stock Error Correction - Upload Physical Stock
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnCancel" CssClass="remove" TabIndex="29" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    To store :
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlToStore" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlToStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="3" CssClass="st" Height="100%"
                                                                Width="195px" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="btnImport" runat="server" CssClass="btn dark" ValidationGroup="Load"
                                                        TabIndex="4" OnClick="btnImport_Click"><i class="fa fa-download">&nbsp;Import Excel</i></asp:LinkButton>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn dark" ValidationGroup="Load"
                                                                TabIndex="4" OnClick="btnExport_Click"><i class="fa fa-download">&nbsp;Export Excel</i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgMaterialAcceptance" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                            CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="I_CODE">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" Visible="false" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" Visible="false" CommandName="View" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" Visible="false" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" Visible="false" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" Visible="false" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAdd" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Add</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Accept" SortExpression="IM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkCheck" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                            CausesValidation="False" CommandName="Check" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Accept</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IM_CODE" SortExpression="IM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIM_CODE" runat="server" Text='<%# Bind("I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="IM_NO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" CssClass="" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item name" SortExpression="IM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" CssClass="" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From Store Code" SortExpression="FROM_STORE_CODE"
                                                                    HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_UOM_CODE" CssClass="" runat="server" Text='<%# Eval("I_UOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item unit" SortExpression="STORE_NAME" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_UOM_NAME" CssClass="" runat="server" Text='<%# Eval("I_UOM_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TO Store" SortExpression="TO_STORE_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTL_STORE_TYPE" CssClass="" runat="server" Text='<%# Eval("STL_STORE_TYPE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Store Name" SortExpression="TO_STORE_NAME" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTORE_NAME" CssClass="" runat="server" Text='<%# Eval("STORE_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Store Stock Qty" SortExpression="STORE_NAME" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTL_DOC_QTY" CssClass="" runat="server" Text='<%# Eval("STL_DOC_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Upload Qty" SortExpression="TO_STORE_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUPLOAD_QTY" CssClass="" runat="server" Text='<%# Eval("UPLOAD_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Diff Qty" SortExpression="TO_STORE_NAME" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDIFF_QTY" CssClass="" runat="server" Text='<%# Eval("DIFF_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Weight" SortExpression="I_UWEIGHT" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_UWEIGHT" CssClass="" runat="server" Text='<%# Eval("I_UWEIGHT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tonnage" SortExpression="TONUGE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTONUGE" CssClass="" runat="server" Text='<%# Eval("TONUGE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="I_INV_RATE" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_INV_RATE" CssClass="" runat="server" Text='<%# Eval("I_INV_RATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="AMT" HeaderStyle-HorizontalAlign="Left"
                                                                    Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAMT" CssClass="" runat="server" Text='<%# Eval("AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Total Qty :
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblNOS" runat="server" Text=""></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Total Amount :
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblAMT" runat="server" Text=""></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Total Tonnuge :
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lbltonnuge" runat="server" Text=""></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <div class="col-md-1">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                    UseSubmitBehavior="false" CssClass="btn green" TabIndex="15" runat="server" Text="Save"
                                                    OnClick="btnSubmit_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="LinkButton1" CssClass="btn default" TabIndex="16" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="loading" align="center">
            <div class="loading2">
                <img src="../../assets/img/giphy.gif" />
            </div>
        </div>
    </div>
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

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
    <!-- END JAVASCRIPTS -->
</asp:Content>
