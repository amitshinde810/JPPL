<%@ Page Title="Sales - Work Order" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="SaleWorkOrder.aspx.cs" Inherits="Transactions_ADD_SaleWorkOrder" %>

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

        function oknumber1(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
        }
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content ">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
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
            <div class="row ">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sales Work Order
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="29" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <asp:Panel ID="MainPanel" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Order No.
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:TextBox CssClass="form-control input-sm" ID="txtOrderNo" placeholder="Order No"
                                                            TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required"></span>Order Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtOrderdate" placeholder="dd MMM yyyy"
                                                                TabIndex="2" MsgObrigatorio="Please Select Bill Date" runat="server"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txttxtOrderdate_CalendarExtender" runat="server" Enabled="True"
                                                                Format="dd MMM yyyy" TargetControlID="txtOrderdate" PopupButtonID="txtOrderdate">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <font color="red">*</font> Sales Order Type</label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList OnSelectedIndexChanged="ddlPOType_SelectedIndexChanged" ID="ddlPOType"
                                                                    CssClass="select2_category form-control" runat="server" TabIndex="3" AutoPostBack="True"
                                                                    Visible="True" MsgObrigatorio="Sales Order Type">
                                                                    <asp:ListItem Selected="True" Value="0">Select Sales Order Type</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Customer Name</label>
                                                    <div class="col-md-5">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="4" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Sales Order No
                                                    </label>
                                                    <div class="col-md-5">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSaleOrderNo" CssClass="select2_category form-control" runat="server"
                                                                    TabIndex="5" MsgObrigatorio="Sales Order Number" OnSelectedIndexChanged="ddlSaleOrderNo_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--/row-->
                                        <div class="row" style="overflow: auto;">
                                            <%--Grid View--%>
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel7895" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgBillPassing" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None" DataKeyNames="CPOD_CPOM_CODE" Font-Names="Verdana"
                                                            Font-Size="12px" TabIndex="6" ShowFooter="false" PageSize="6" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            OnPageIndexChanging="dgBillPassing_PageIndexChanging" OnRowCommand="dgBillPassing_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="checker" EnableViewState="true"
                                                                            AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%-- here we insert database field for bind and eval--%>
                                                                <asp:TemplateField HeaderText="CPOD_CPOM_CODE" SortExpression="CPOD_CPOM_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_CPOM_CODE" runat="server" Text='<%# Bind("CPOD_CPOM_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CPOD_I_CODE" SortExpression="CPOD_I_CODE" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblICPOD_I_CODE" runat="server" Text='<%# Bind("CPOD_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODENO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_CODENO" runat="server" Text='<%# Eval("I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name No" SortExpression="I_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblI_NAME" runat="server" Text='<%# Eval("I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sales Order Qty" SortExpression="CPOD_ORD_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCPOD_ORD_QTY" runat="server" Text='<%# Eval("CPOD_ORD_QTY") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Work Order Qty" SortExpression="WORK_ORD_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtWORK_ORD_QTY" runat="server" CssClass="form-control text-sm text-right"
                                                                            placeholder="0.00" MsgObrigatorio="Qty" Text='<%# Eval("WORK_ORD_QTY") %>' AutoPostBack="true"
                                                                            OnTextChanged="txtWORK_ORD_QTY_TextChanged"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtWORK_ORD_QTY"
                                                                            ValidChars="0123456789." runat="server" />
                                                                        <%-- <asp:Label ID="lblIWORK_ORD_QTY" runat="server" Text='<%# Eval("WORK_ORD_QTY") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Balance Qty" SortExpression="WORK_BAL_QTY" HeaderStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIWORK_BAL_QTY" runat="server" Text='<%# Eval("WORK_BAL_QTY") %>'
                                                                            CssClass="Control-label pull-right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass="Control-text text-right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%-- End Grid View--%>
                                            </div>
                                        </div>
                                        <hr />
                                        <div class="form-actions fluid">
                                            <div class="col-md-offset-4 col-md-9">
                                                <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                    UseSubmitBehavior="false" CssClass="btn green" TabIndex="7" runat="server" Text="Save"
                                                    OnClick="btnSubmit_Click" />
                                                <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="8" runat="server"
                                                    OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                            </div>
                                        </div>
                                        <!--/row-->
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel33">
                        <ContentTemplate>
                            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="captionPopup">
                                            Alert
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <label class="col-md-12 control-label">
                                                        Do you Want to Cancel Record?
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-3 col-md-9">
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="29" runat="server" Visible="true"
                                                            OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                        <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="30" runat="server"
                                                            OnClick="btnCancel1_Click"> No</asp:LinkButton>
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
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
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

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {

                if (VerificaObrigatorio('#<%=txtOrderdate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlPOType.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlCustomer.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlSaleOrderNo.ClientID%>', '#Avisos') == false) {

                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
