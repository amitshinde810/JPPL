using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Timers;

public partial class _Default : System.Web.UI.Page
{
    #region datamembers
    Login_BL BL_Login = new Login_BL();
    string uemail = null;
    string uname = null;
    bool responce;
    DataSet dscompany = new DataSet();
    string openingdate = string.Empty, closingdate = string.Empty;
    static DataTable DtUserDetails = new DataTable();
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        if (!IsPostBack)
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            lblmesg.Visible = false;
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select CM_ID,CM_NAME  from COMPANY_MASTER where CM_ACTIVE_IND=1");

            ddlCompName.DataSource = dt;
            ddlCompName.DataTextField = "CM_NAME";
            ddlCompName.DataValueField = "CM_ID";
            ddlCompName.DataBind();
         
            if (dt.Rows.Count > 0)
            {
                ddlCompName.SelectedIndex = 0;
                ddlCompName_SelectedIndexChanged(null, null);
                if (ddlFinancialYear.Items.Count != 0)
                {
                    ddlFinancialYear.SelectedIndex = 0;
                }
            }
         
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                txtUserName.Text = Request.Cookies["UserName"].Value;
                txtPassword.Attributes["value"] = Request.Cookies["Password"].Value;
                chkRemeber.Checked = true;
            }
        }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    #region ddlCompName_SelectedIndexChanged
    protected void ddlCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt1 = new DataTable();
            //dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + "");
            dt1 = CommonClasses.Execute("select  distinct CM_CODE, 'From  ' +convert(varchar(10),CM_OPENING_DATE,103)+' To '+ convert(varchar(10),CM_CLOSING_DATE,103)as FINANCIAL from COMPANY_MASTER where CM_ID=" + ddlCompName.SelectedValue + " order by CM_CODE desc");
            //dt1 = CommonClasses.Execute("select  distinct CM_CODE, CM_NAME from COMPANY_MASTER where CM_ID=" + ddlCompName.SelectedValue + " and CM_OPENING_DATE<'" + DateTime.Now.ToString("dd/MMM/yyyy") + "' order by CM_CODE desc");
            ddlFinancialYear.DataSource = dt1;
            ddlFinancialYear.DataTextField = "FINANCIAL";
            ddlFinancialYear.DataValueField = "CM_CODE";
            ddlFinancialYear.DataBind();
            //ddlFinancialYear.Items.Insert(0, new ListItem("Select Financial Year", "0"));
          
        }        
        catch (Exception Ex)
        {
            //CommonClasses.SendError("Login", "ddlCompName_SelectedIndexChanged", Ex.Message);
            throw Ex;
        }
    }
    #endregion

    #   region btnLogin_Click
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompName.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Company Name";
                return;
            }
            else if (ddlFinancialYear.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Financial Year";
                return;
            }
            DataTable dt = new DataTable();           
            dt = BL_Login.VerifyLogin(txtUserName.Text, CommonClasses.Encrypt(txtPassword.Text), ddlCompName.SelectedValue, ddlFinancialYear.SelectedValue);
           
            if (dt.Rows.Count > 0)
            {
                Session["Username"] = txtUserName.Text;
                Session["CompanyId"] = dt.Rows[0]["UM_CM_ID"].ToString();
                Session["CompanyCode"] = dt.Rows[0]["CM_CODE"].ToString();
                Session["CompanyName"] = dt.Rows[0]["CM_NAME"].ToString();
                Session["CompanyAdd"] = dt.Rows[0]["CM_ADDRESS1"].ToString();
                Session["CompanyAdd1"] = dt.Rows[0]["CM_ADDRESS2"].ToString();
                Session["CompanyPhone"] = dt.Rows[0]["CM_PHONENO1"].ToString();
                Session["CompanyFax"] = dt.Rows[0]["CM_FAXNO"].ToString();

                Session["CompanyVatTin"] = dt.Rows[0]["CM_VAT_TIN_NO"].ToString();
                Session["CompanyCstTin"] = dt.Rows[0]["CM_CST_NO"].ToString();
                Session["CompanyRegd"] = dt.Rows[0]["CM_REGD_NO"].ToString();
                Session["CompanyEccNo"] = dt.Rows[0]["CM_ECC_NO"].ToString();
                Session["CompanyVatWef"] = dt.Rows[0]["CM_VAT_WEF"].ToString();
                Session["CompanyCstWef"] = dt.Rows[0]["CM_CST_WEF"].ToString();
                Session["CompanyIso"] = dt.Rows[0]["CM_ISO_NUMBER"].ToString();
                Session["CompanyWebsite"] = dt.Rows[0]["CM_WEBSITE"].ToString();
                Session["CompanyEmail"] = dt.Rows[0]["CM_EMAILID"].ToString();
                Session["CompanyOpeningDate"] = Convert.ToDateTime(dt.Rows[0]["CM_OPENING_DATE"]).ToString();
                Session["CompanyClosingDate"] = Convert.ToDateTime(dt.Rows[0]["CM_CLOSING_DATE"]).ToString();
                Session["CompanyFinancialYr"] = Convert.ToDateTime(dt.Rows[0]["CM_OPENING_DATE"].ToString()).Year.ToString() + "-" + Convert.ToDateTime(dt.Rows[0]["CM_CLOSING_DATE"].ToString()).Year.ToString();


                Session["UserActivityCode"] = "133";
                Session["BranchCode"] = "-2147483648";
                Session["UserCode"] = dt.Rows[0]["UM_CODE"].ToString();
                //Session["UserCode"] = dt.Rows[0][0].ToString();
                Session["OpeningDate"] = dt.Rows[0]["CM_OPENING_DATE"].ToString();
                Session["ClosingDate"] = dt.Rows[0]["CM_CLOSING_DATE"].ToString();
                //Response.Redirect("E_Administration/Masters/ADD/BranchSelection.aspx", true);
                if (chkRemeber.Checked)
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

                }
                Response.Cookies["UserName"].Value = txtUserName.Text.Trim();
                Response.Cookies["Password"].Value = txtPassword.Text.Trim();
                Response.Redirect("~/Masters/ADD/Dashboard.aspx", false);

            }
            else
            {                
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter valid Username and Password";
            }
          
        }
        catch (Exception exc)
        {

        }
    }
    #endregion

    //#region MailSend
    //public void MailSend( string eid, string un)
    //{
    //  bool f = false;

    //    try
    //    {
    //        DataTable dt = BL_Login.DataEmail();
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendLine("Dear");
    //        sb.Append(un);
    //        sb.Append(",");
    //        sb.Append("<br/><br/>");
    //        sb.AppendLine("Given below is the current status of Simya Petro,");
    //        sb.AppendLine("<br/><br/>");
    //        sb.AppendLine("");
    //        sb.AppendLine("Sales");
    //        sb.AppendLine("<br/><br/>");
    //        sb.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
    //        sb.Append("style='border: solid 2px Green; font-size: small;'>");
    //        sb.Append("<tr align='left' valign='top'>");
    //        foreach (DataColumn myColumn in dt.Columns)
    //        {
    //            sb.Append("<td align='left' valign='top'>");
    //            sb.Append(myColumn.ColumnName);
    //            sb.Append("</td>");
    //        }
    //        sb.Append("</tr>");

    //        foreach (DataRow myRow in dt.Rows)
    //        {
    //            sb.Append("<tr align='left' valign='top'>");
    //            foreach (DataColumn myColumn in dt.Columns)
    //            {
    //                sb.Append("<td align='left' valign='top'>");
    //                sb.Append(myRow[myColumn.ColumnName].ToString());
    //                sb.Append("</td>");
    //            }
    //            sb.Append("</tr>");
    //        }
    //        sb.Append("</table>");
    //        sb.AppendLine("<br/><br/>");
    //        sb.AppendLine("");
    //        //sb.AppendLine(myBuilder.ToString());    //here I want the data to       display in table format
    //        sb.AppendLine("Thanks And Regards,");
    //        sb.AppendLine("<br/><br/>");
    //        sb.AppendLine("Simyainfo Team.");
    //        MailMessage mail = new MailMessage();
    //        mail.To.Add(eid);
    //        mail.From = new MailAddress("uday.prasad@simyainfo.com");
    //        mail.Subject = "Simya Petro Current Status";
    //        mail.Body = sb.ToString();
    //        mail.IsBodyHtml = true;
    //        StringWriter stw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(stw);
    //        SmtpClient smtp = new SmtpClient();
    //        smtp.Host = "mail.simyainfo.com"; //Or Your SMTP Server Address
    //        smtp.Credentials = new System.Net.NetworkCredential
    //             ("uday.prasad@simyainfo.com", "Uday#12545@");
    //        //Or your Smtp Email ID and Password
    //        //smtp.EnableSsl = true;
    //        smtp.Send(mail);
    //        f = true;
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //        f = false;
    //    }
    //    if (f == true)
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('Mail sent successfully');", true);
    //    else
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('Please try again');", true);

    //}
    //#endregion
    //System.Timers.Timer aTimer = new System.Timers.Timer(10000);
    //void a()
    //{

    //    // Hook up the Elapsed event for the timer.
    //    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
    //    // Set the Interval to 60 seconds (60000 milliseconds).
    //    aTimer.Interval = 5000;
    //    //for enabling for disabling the timer.
    //    aTimer.Enabled = true;
    //}

    //            private void OnTimedEvent(object source, ElapsedEventArgs e)
    //            {
    //                 //disable the timer
    //                aTimer.Enabled = false;
    //                MailSend("uday.prasad@simyainfo.com", "Uday Prasad");
    //             }

    protected void lnkClickHere_Click(object sender, EventArgs e)
    {
        if (ddlCompName.SelectedIndex == -1)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Company Name";
            return;
        }
        else if (ddlFinancialYear.SelectedIndex == -1)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Financial Year";
            return;
        }
        else if (txtUserName.Text == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Enter User Name";
            return;
        }

        forgetpass.Visible = true;
        loginPanel.Visible = false;

        //Getting User emial
        DtUserDetails = CommonClasses.Execute("SELECT UM_EMAIL,UM_USERNAME,UM_PASSWORD,UM_NAME FROM USER_MASTER WHERE UM_USERNAME=lower('" + txtUserName.Text + "')");
        if (DtUserDetails.Rows.Count > 0)
        {
            txtEmail.Text = DtUserDetails.Rows[0]["UM_EMAIL"].ToString();
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        forgetpass.Visible = false;
        loginPanel.Visible = true;
    }

    protected void btnForgetSubmit_Click(object sender, EventArgs e)
    {

        Page.Validate();
        if (Page.IsValid)
        {
            if (ddlCompName.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Company Name";
                return;
            }
            else if (ddlFinancialYear.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Financial Year";
                return;
            }
            else if (txtUserName.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter User Name";
                return;
            }

            // bl = new BusLay();
            // DataTable Setting = bl.Execute("SELECT SMP_EML_PORT_NO, SMP_EML_HOST, SMP_EML_USR, SMP_EML_PWDR FROM SMP_EML_ST");

            string smsg = "Dear " + DtUserDetails.Rows[0]["UM_NAME"].ToString() + ",";
            smsg += "<br>Your request for SiMYA-ERP Credentials";
            smsg += "<br>Your User Id is: " + DtUserDetails.Rows[0]["UM_USERNAME"].ToString();
            smsg += "<br>Password is: " + CommonClasses.DecriptText(DtUserDetails.Rows[0]["UM_PASSWORD"].ToString());

            MailMessage message = new MailMessage();
            try
            {
                message.To.Add(new MailAddress(txtEmail.Text));
                message.From = new MailAddress("info@simyainfo.com");

                message.Subject = "SiMYA-ERP  || Message From SiMYAERP";
                message.Body = smsg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Port = int.Parse("25");
                client.Host = "mail.simyainfo.com";
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential("info@simyainfo.com", "Simya@2013");
                client.UseDefaultCredentials = false;
                //client.Credentials = nc;
                client.Send(message);
                txtEmail.Text = "";
                lblmsg.Visible = true;
                //lblmsg.Text = "Your Password is send on your provided email. Please check the mail and relogin to system.";
                //forgetpass.Visible = false;
                //loginPanel.Visible = true;
            }



            catch (Exception ex)
            {
                //catch block goes here
            }
        }
        else
        {
            // error.Visible = true;
        }

    }
}




