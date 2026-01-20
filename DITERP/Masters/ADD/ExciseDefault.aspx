<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ExciseDefault.aspx.cs"
    Inherits="Masters_ADD_ExciseDefault" Title="DITERP | Excise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupMsg').hide();
            __doPostBack('Button5', e);
        }
        
    </script>

    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
        <ContentTemplate>
            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
            <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                OnOkScript="oknumber()" CancelControlID="Button7" DynamicServicePath="" Enabled="True"
                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div style="font-size: medium;" class="captionPopup">
                            Warning
                        </div>
                    </div>
                    <div class="portlet-body form">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="row">
                                    <label style="font-size: medium;" class="col-md-12 control-label">
                                        You Have No Right To View
                                    </label>
                                </div>
                                <div class="row">
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Masters</a>
                                    <span class="after"></span></li>
                                <li id="BOM" runat="server"><a id="A1" runat="server"><i class="fa fa-tasks" visible="false">
                                </i></a></li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="excise" runat="server"><a href="../../Transactions/VIEW/ViewExciseCreditEntry.aspx"
                                    onserverclick="btnExciseCreditEntry_click"><i class="fa fa-tasks"></i>Excise Credit
                                    Entry</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="ER1Report" runat="server" visible="false"><a href="../../RoportForms/VIEW/ViewWorkOrderRegister.aspx">
                                    <i class="fa fa-tasks"></i>Excise Register</a> </li>
                                <li id="Annexure4Report" runat="server" visible="false"><a href="../../RoportForms/VIEW/ViewWorkOrderRegister.aspx">
                                    <i class="fa fa-tasks"></i>Work Order Register</a> </li>
                                <li id="Annexure5Report" runat="server" visible="false"><a href="../../RoportForms/VIEW/ViewWorkOrderRegister.aspx">
                                    <i class="fa fa-tasks"></i>Work Order Register</a> </li>
                                <li id="ExciseRegister" runat="server"><a href="../../RoportForms/VIEW/VIewExciseRegister.aspx"
                                    onserverclick="btnExciseRegister_click"><i class="fa fa-tasks"></i>Excise Register</a>
                                </li>
                                <li id="DailyRegister" runat="server"><a href="../../RoportForms/VIEW/ViewDailyStockAccDetail.aspx"
                                    onserverclick="btnDailyRegister_click"><i class="fa fa-tasks"></i>Daily Stock Account</a>
                                </li>
                                <li id="Li1" runat="server"><a href="../../RoportForms/VIEW/RCMInwardReport.aspx"
                                    onserverclick="btnRCMInwardReport_click"><i class="fa fa-tasks"></i>RCM Inward Register</a>
                                </li>
                                <li id="Li2" runat="server"><a href="../../RoportForms/VIEW/ViewAnnexureIVGST.aspx"
                                    onserverclick="btnAnnexureIV_click"><i class="fa fa-tasks"></i>Annexure IV</a>
                                </li>
                                <li id="Li3" runat="server"><a href="../../RoportForms/VIEW/ViewAnnexureVGST.aspx"
                                    onserverclick="btnAnnexureV_click"><i class="fa fa-tasks"></i>Annexure V</a>
                                
                                </li>
                                
                                <li id="Li4" runat="server"><a href="../../RoportForms/VIEW/ViewGST_ITC_04.aspx"
                                    onserverclick="btnITC_click"><i class="fa fa-tasks"></i>GST ITC 04</a>
                                
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
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

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
</asp:Content>
