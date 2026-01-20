<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="VendorScheduleApproval.aspx.cs"
    Inherits="PPC_VIEW_VendorScheduleApproval" Title="Vendor Schedule Approval" %>

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
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
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
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Vendor Schedule Approval
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <asp:Panel ID="panelInspection" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <asp:Label runat="server" ID="lblType"></asp:Label></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red">*</font> Schedule Month</label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtInspDate" runat="server" CssClass="form-control" placeholder=" MMM yyyy"
                                                                    AutoPostBack="true" ValidationGroup="Save" TabIndex="2" MsgObrigatorio="Inspection Date"
                                                                    OnTextChanged="txtInspDate_TextChanged"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="txtInspDate_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="MMM yyyy" TargetControlID="txtInspDate" PopupButtonID="txtInspDate">
                                                                </cc1:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button ID="Button1" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                                    UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Accept All"
                                                                    OnClick="Button1_Click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-1">
                                            </div>
                                            <div class="col-md-10">
                                                <div class="form-group">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgMaterialAcceptance" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="I_CODE"
                                                                OnPageIndexChanging="dgMaterialAcceptance_PageIndexChanging" OnRowCommand="dgMaterialAcceptance_RowCommand"
                                                                OnRowDataBound="dgMaterialAcceptance_RowDataBound1">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="View" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkView" CssClass="btn blue btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="View" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Modify" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Add" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkAdd" Visible="false" CssClass="btn green btn-xs" BorderStyle="None"
                                                                                runat="server" CausesValidation="False" CommandName="Modify" Text="Add" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" CssClass="btn red btn-xs" BorderStyle="None" runat="server"
                                                                                CausesValidation="False" CommandName="Delete" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="Party Code" SortExpression="P_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblP_CODE" CssClass="" runat="server" Text='<%# Eval("P_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Party Name" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblP_NAME" CssClass="" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_CODE" CssClass="" runat="server" Text='<%# Eval("I_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
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
                                                                    <asp:TemplateField HeaderText="Required QTY" SortExpression="QTY" ItemStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQTY" CssClass="" runat="server" Text='<%# Eval("QTY") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Off Load QTY" SortExpression="VSA_QTY_PPC" ItemStyle-HorizontalAlign="Right"
                                                                        HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtVSA_QTY_PPC" CssClass="text-right" OnTextChanged="txtVSA_QTY_PPC_TextChanged"
                                                                                onkeypress="return validateFloatKeyPress(this,event);" runat="server" AutoPostBack="true"
                                                                                Text='<%# Eval("VSA_QTY_PPC") %>'></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtVSA_QTY_PPC"
                                                                                ValidChars="0123456789." runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate" SortExpression="I_INV_RATE" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblI_INV_RATE" CssClass="" runat="server" Text='<%# Eval("I_INV_RATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AMT" SortExpression="IM_DATE" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAMT" CssClass="text-right" runat="server" Text='<%# Eval("AMT") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="VS_STATUS" SortExpression="VS_STATUS" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVS_STATUS" CssClass="text-right" runat="server" Text='<%# Eval("VS_STATUS") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="VS_TYPE" SortExpression="VS_TYPE" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVS_TYPE" CssClass="text-right" runat="server" Text='<%# Eval("VS_TYPE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="VSA_TYPE" SortExpression="VSA_TYPE" HeaderStyle-HorizontalAlign="Right"
                                                                        Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVSA_TYPE" CssClass="text-right" runat="server" Text='<%# Eval("VSA_TYPE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" SortExpression="VS_TYPE" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkPost" CssClass='<%# (Convert.ToString(Eval("VS_STATUS"))=="True")? "btn green btn-xs":"btn blue btn-xs" %>'
                                                                                BorderStyle="None" runat="server" CausesValidation="False" CommandName="Status"
                                                                                Enabled='<%#Eval("VS_STATUS").ToString()== "True" ? true : false %>' Text='<%#Eval("VS_STATUS").ToString()== "True" ? "Accept" : "" %>'
                                                                                CommandArgument='<%#((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
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
                                                    <div class="col-md-7">
                                                    </div>
                                                    <label class="col-md-2 control-label text-right ">
                                                        <font color="red"></font>Total Amount
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-10">
                                    <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                        UseSubmitBehavior="false" CssClass="btn green" TabIndex="17" runat="server" Text="Save"
                                        OnClick="btnSave_Click" />
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="13" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                            OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                        <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- END JAVASCRIPTS -->
    <!-- END PAGE LEVEL SCRIPTS -->
    </div>
</asp:Content>
