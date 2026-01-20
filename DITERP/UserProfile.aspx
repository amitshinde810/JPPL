<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile"
    Title="User Profile" %>

<!DOCTYPE html>
<html lang="en" class="no-js">
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
        <%--<img src="assets/img/logo-big.png" alt="" />--%>
        <img src="assets/img/logo_dccl_maroon_1.jpg" alt="" />
    </div>
    <!-- END LOGO -->
    <!-- BEGIN LOGIN -->
    <div class="content">
        <!-- BEGIN LOGIN FORM -->
        <form id="Form1" runat="server">
        <asp:Panel ID="loginPanel" runat="server">
            <div class="login-form">
                <h3 class="form-title">
                    User Profile</h3>
                <div class="form-group">
                    <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                    <label class="control-label">
                        User Name</label>
                    <div class="input-icon">
                        <i class="fa fa-user"></i>
                        <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtusername" placeholder="Username"
                            data-required="1" TabIndex="1" runat="server" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="UserName" runat="server" visible="false">
                    <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                    <label class="control-label">
                        User Type</label>
                    <div class="input-icon">
                        <asp:DropDownList ID="ddlUser" CssClass="select2_category form-control" runat="server"
                            TabIndex="3">
                            <asp:ListItem Value="0">Select Type</asp:ListItem>
                            <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                            <asp:ListItem Value="Manager">Manager</asp:ListItem>
                            <asp:ListItem Value="Accountant">Accountant</asp:ListItem>
                            <asp:ListItem Value="Operator">Operator</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                    <label class="control-label">
                        New Password</label>
                    <div class="input-icon">
                        <i class="fa fa-user"></i>
                        <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtNewPassward" placeholder="New Password"
                            data-required="1" TabIndex="1" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label">
                        Confirm Password</label>
                    <div class="input-icon">
                        <i class="fa fa-lock"></i>
                        <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtConfirmPass" placeholder="Confirm Password"
                            data-required="1" TabIndex="2" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="form-actions">
                    <asp:LinkButton ID="btnRegirct" CssClass="btn default" TabIndex="5" runat="server"
                        OnClick="btnRegirct_Click"><i class="m-icon-swapleft"> </i> Login </asp:LinkButton>
                    <asp:LinkButton ID="btnChangePass" CssClass="btn green pull-right" TabIndex="5" runat="server"
                        OnClick="btnChangePass_Click"><i class="m-icon-swapright m-icon-white"></i> Change Password</asp:LinkButton>
                </div>
                <asp:Label runat="server" CssClass="required" ID="lblmesg" ForeColor="Red"></asp:Label>
            </div>
        </asp:Panel>
        <!-- END LOGIN FORM -->
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
        jQuery(document).ready(function() {
            App.init();
            Login.init();
        });
    </script>

    <!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>
