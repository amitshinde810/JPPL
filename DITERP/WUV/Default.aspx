<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<!--[if IE 8]> <html lang="en" class="ie8 no-js"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9 no-js"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en" class="no-js">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
    <meta charset="utf-8" />
    <title>DIT ERP</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <meta name="MobileOptimized" content="320">
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="assets/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet"
        type="text/css" />
    <link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <!-- END GLOBAL MANDATORY STYLES -->
    <!-- BEGIN PAGE LEVEL STYLES -->
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2_metro.css" />
    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- BEGIN THEME STYLES -->
    <link href="assets/css/style-metronic.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/style-responsive.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/themes/default.css" rel="stylesheet" type="text/css" id="style_color" />
    <link href="assets/css/pages/login.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/custom.css" rel="stylesheet" type="text/css" />
    <!-- END THEME STYLES -->
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body class="login">
    <!-- BEGIN LOGO -->
    <div class="logo">
        <%--<img src="assets/img/JKCC LOGO.JPG" height="60px" width="150px"  alt="" />--%>
      <%--  <img src="assets/img/logo_dccl_maroon_1.jpg" alt="" />--%>
          <%--<img src="assets/img/logoNew.png" alt="" />--%>
    </div>
    <!-- END LOGO -->
    <!-- BEGIN LOGIN -->
    <div class="content">
        <!-- BEGIN LOGIN FORM -->
        <form runat="server" defaultfocus="txtUserName" defaultbutton="btnSave">
           <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                        height: 50px; width: 100%; border: 1px solid #9f6000">
                        <div style="vertical-align: middle; margin-top: 10px;">
                            <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: small; font-weight: bold;
                                margin-top: 50px; margin-left: 10px;"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
            <asp:Panel ID="loginPanel" runat="server">
                <div class="login-form">
                <div><center>Version 2.1 G</center></div>
                
                    <h3 class="form-title">Login to your account</h3>
                    <div class="alert alert-danger display-hide">
                        <button class="close" data-close="alert">
                        </button>
                        <span>Enter any username and password. </span>
                    </div>
                    <div class="form-group">
                        <label class="control-label visible-ie8 visible-ie9">
                            Company Name</label>
                        <div class="input-icon">
                          <%--  <i class="fa fa-user"></i>--%>
                            <asp:DropDownList ID="ddlCompName" CssClass="select2_category form-control placeholder-no-fix"
                                runat="server" MsgObrigatorio="Company Name" data-required="1" TabIndex="1" OnSelectedIndexChanged="ddlCompName_SelectedIndexChanged" AutoPostBack="true">
                              
                            </asp:DropDownList>
                        </div>
                    </div>
                     <div class="form-group">
                        <label class="control-label visible-ie8 visible-ie9">
                            Finacial Year</label>       
                        <div class="input-icon">
                          <%--  <i class="fa fa-user"></i>--%>
                            <asp:DropDownList ID="ddlFinancialYear" CssClass="select2_category form-control placeholder-no-fix"
                                runat="server" MsgObrigatorio="Finacial Year" data-required="1" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                        <label class="control-label visible-ie8 visible-ie9">
                            Username</label>
                        <div class="input-icon">
                            <i class="fa fa-user"></i>
                            <%--<input class="form-control placeholder-no-fix" type="text" autocomplete="off" placeholder="Username"
                    name="username" />--%>
                            <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtUserName" placeholder="Username"
                                data-required="1" TabIndex="3" runat="server" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label visible-ie8 visible-ie9">
                            Password</label>
                        <div class="input-icon">
                            <i class="fa fa-lock"></i>
                            <%--<input class="form-control placeholder-no-fix" type="password" autocomplete="off"
                    placeholder="Password" name="password" />--%>
                            <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtPassword" placeholder="Password"
                                data-required="1" TabIndex="4" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>

                        </div>
                    </div>
                    <div class="form-actions">
                        <label class="checkbox">
                            <%--<input type="checkbox" name="remember" value="1" />--%>
                            <asp:CheckBox ID="chkRemeber" runat="server" />
                            Remember me
                        </label>
                        <asp:Button ID="btnSave" TabIndex="2" CssClass="btn blue pull-right" runat="server"
                            Text="Login" OnClick="btnLogin_Click" />
                    </div>
                    <asp:Label runat="server" CssClass="required" ID="lblmesg"></asp:Label>
                    <h4>Forgot your password ?</h4>
                    <p>
                        No worries, click
                    <asp:LinkButton ID="lnkClickHere" runat="server" OnClick="lnkClickHere_Click">here</asp:LinkButton>
                        to reset your password.
                    </p>

                </div>
            </asp:Panel>
            <!-- END LOGIN FORM -->
           <!-- BEGIN FORGOT PASSWORD FORM -->
        <asp:Panel ID="forgetpass" class="forget-password" runat="server" Visible="false">
            <h3>
                Forget Password ?</h3>
            <p>
                Get your password on below mentioned e-mail.
            </p>
            <div class="form-group">
                <div class="input-icon">
                    <i class="fa fa-envelope"></i>
                    <%--<input class="form-control placeholder-no-fix" type="text" autocomplete="off" placeholder="Email"
                            name="email" />--%>
                    <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtEmail" placeholder="Email" ReadOnly="true"
                        AutoPostBack="true" data-required="1" TabIndex="3" runat="server" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div class="form-actions">
                <asp:LinkButton ID="btnBack" CssClass="btn default" TabIndex="5" runat="server" OnClick="btnBack_Click"><i class="m-icon-swapleft"> </i> Back </asp:LinkButton>
                <asp:LinkButton ID="btnForgetSubmit" CssClass="btn green pull-right" TabIndex="5"
                    runat="server" OnClick="btnForgetSubmit_Click"><i class="m-icon-swapright m-icon-white"></i> Submit</asp:LinkButton>
            </div>
            <div class="form-actions">
                <asp:Label CssClass="" ID="Label1" AutoPostBack="true"
                    Visible="false" TabIndex="4" runat="server" Text="Your Password is send on your provided email. Please check the mail and re-login to system."></asp:Label>
            </div>
        </asp:Panel>
        <!-- END FORGOT PASSWORD FORM -->
        </form>
    </div>
    <!-- END LOGIN -->
    <!-- BEGIN COPYRIGHT -->
    <div class="copyright">
        2016 &copy; Dynamisch IT Pvt Ltd. ALL Rights Reserved.
    </div>
    <!-- END COPYRIGHT -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
	<script src="assets/plugins/respond.min.js"></script>
	<script src="assets/plugins/excanvas.min.js"></script> 
	<![endif]-->

    <script src="assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <script src="assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="assets/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>

    <script src="assets/plugins/backstretch/jquery.backstretch.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="assets/plugins/select2/select2.min.js"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="assets/scripts/app.js" type="text/javascript"></script>

    <script src="assets/scripts/login-soft.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script>
        jQuery(document).ready(function () {
            App.init();
            Login.init();
        });
    </script>

    <!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>
