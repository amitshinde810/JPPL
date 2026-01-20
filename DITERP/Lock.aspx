<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lock.aspx.cs" Inherits="Lock" %>

<!DOCTYPE html>
<!--[if IE 8]> <html lang="en" class="ie8 no-js"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9 no-js"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en" class="no-js">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
<meta charset="utf-8"/>
<title>
                 Lock Screen</title>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<meta content="" name="description"/>
<meta content="" name="author"/>
<meta name="MobileOptimized" content="320">
<!-- BEGIN GLOBAL MANDATORY STYLES -->
<link href="assets/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
<link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="assets/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css"/>
<!-- END GLOBAL MANDATORY STYLES -->
<!-- BEGIN THEME STYLES -->
<link href="assets/css/style-metronic.css" rel="stylesheet" type="text/css"/>
<link href="assets/css/style.css" rel="stylesheet" type="text/css"/>
<link href="assets/css/style-responsive.css" rel="stylesheet" type="text/css"/>
<link href="assets/css/plugins.css" rel="stylesheet" type="text/css"/>
<link href="assets/css/themes/default.css" rel="stylesheet" type="text/css" id="style_color"/>
<link href="assets/css/pages/lock.css" rel="stylesheet" type="text/css"/>
<link href="assets/css/custom.css" rel="stylesheet" type="text/css"/>
<!-- END THEME STYLES -->
<link rel="shortcut icon" href="favicon.ico"/>
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body>
<div class="page-lock">
	<div class="page-logo">
		<a class="brand" href="Default.aspx">
		<%--<img src="assets/img/logo-big.png" alt="logo"/>--%>
		</a>
	</div>
	<div class="page-body">
		<%--<img class="page-lock-img" src="assets/img/photo1.jpg" alt="">--%>
		<div class="page-lock-info">
			
			<h1>
                <asp:Label ID="lblusername" runat="server" Text="User Name"></asp:Label></h1>
			<span class="email">				
				 <asp:Label ID="lblmailId" runat="server" Text="Mail ID"></asp:Label>
			</span>
			<span class="locked">
				 Locked
			</span>
            <form class="form-inline" runat="server">
            <div class="form-group">
                <label class="control-label visible-ie8 visible-ie9">
                    Password</label>
                <div class="input-icon">
                    <i class="fa fa-lock"></i>
                    <%--<input class="form-control placeholder-no-fix" type="password" autocomplete="off"
                    placeholder="Password" name="password" />--%>
                    <asp:TextBox CssClass="form-control placeholder-no-fix" ID="txtPassword" placeholder="Password"
                        data-required="1" TabIndex="1" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                </div>
                 <asp:Button ID="btnSubmit" TabIndex="2" CssClass="btn blue pull-right" runat="server"
                    Text="Submit" OnClick="btnSubmit_Click" />
            </div>
           <%-- <div class="form-actions">--%>
               
           <%-- </div>--%>
            <div class="relogin">
                <a href="Default.aspx">Not
                    <asp:Label ID="lblreloginusername" runat="server" Text="Label" TabIndex="3"></asp:Label>
                    ?</a>
            </div>
            </form>
		</div>
	</div>
	<div class="page-footer">
		 2015 &copy; SiMYA Info. ALL Rights Reserved.
	</div>
</div>
<!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
<!-- BEGIN CORE PLUGINS -->
<!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script> 
<![endif]-->
<script src="assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
<script src="assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
<script src="assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js" type="text/javascript"></script>
<script src="assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>
<script src="assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>
<script src="assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>
<script src="assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>
<!-- END CORE PLUGINS -->
<!-- BEGIN PAGE LEVEL PLUGINS -->
<script src="assets/plugins/backstretch/jquery.backstretch.min.js" type="text/javascript"></script>
<!-- END PAGE LEVEL PLUGINS -->
<script src="assets/scripts/app.js"></script>
<script src="assets/scripts/lock.js"></script>
<script>
jQuery(document).ready(function() {    
   App.init();
   Lock.init();
});
</script>
<!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>