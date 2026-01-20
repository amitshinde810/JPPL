using System;
using System.Data;
using System.Web.UI;
using System.Net.Mail;

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
                dt = CommonClasses.Execute("select DISTINCT CM_ID,CM_NAME  from COMPANY_MASTER where CM_ACTIVE_IND=1");

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
                        DataTable dtSelect = new DataTable();
                        dtSelect = CommonClasses.Execute("SELECT DISTINCT CM_CODE, 'From  ' +convert(varchar(10),CM_OPENING_DATE,103)+' To '+ convert(varchar(10),CM_CLOSING_DATE,103)as FINANCIAL from COMPANY_MASTER where CM_ID=" + ddlCompName.SelectedValue + " and convert(date,GETDATE()) between convert(date,CM_OPENING_DATE) and convert(date,CM_CLOSING_DATE) ORDER BY CM_CODE DESC");
                        if (dtSelect.Rows.Count > 0)
                        {
                            ddlFinancialYear.SelectedValue = dtSelect.Rows[0][0].ToString();
                        }
                        else
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
            dt1 = CommonClasses.Execute("select  distinct CM_CODE, 'From  ' +convert(varchar(10),CM_OPENING_DATE,103)+' To '+ convert(varchar(10),CM_CLOSING_DATE,103)as FINANCIAL from COMPANY_MASTER where CM_ID=" + ddlCompName.SelectedValue + " order by CM_CODE DESC ");
            ddlFinancialYear.DataSource = dt1;
            ddlFinancialYear.DataTextField = "FINANCIAL";
            ddlFinancialYear.DataValueField = "CM_CODE";
            ddlFinancialYear.DataBind();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    #endregion

    #region btnLogin_Click
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
                Session["Username"] = txtUserName.Text.Trim();
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
                Session["DASHBOARD"] = "1";
                Session["UserActivityCode"] = "133";
                Session["BranchCode"] = "-2147483648";
                Session["UserCode"] = dt.Rows[0]["UM_CODE"].ToString();
                Session["OpeningDate"] = dt.Rows[0]["CM_OPENING_DATE"].ToString();
                Session["ClosingDate"] = dt.Rows[0]["CM_CLOSING_DATE"].ToString();
                Session["CompanyGST"] = dt.Rows[0]["CM_GST_NO"].ToString();
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
                Session["PartyCode"] = "";
                if (dt.Rows[0]["P_CODE"].ToString().Trim() == "" || dt.Rows[0]["P_CODE"].ToString().Trim() == "0")
                {
                    Response.Redirect("~/Masters/ADD/Dashboard.aspx", false);
                }
                else
                {
                    Session["PartyCode"] = dt.Rows[0]["P_CODE"].ToString();

                    Response.Redirect("~/Masters/ADD/VendorDefault.aspx", false);
                }
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
            string smsg = "Dear  " + DtUserDetails.Rows[0]["UM_NAME"].ToString() + ",";
            smsg += "<br>Your request for DIT-ERP Credentials";
            smsg += "<br>Your User Id is: " + DtUserDetails.Rows[0]["UM_USERNAME"].ToString();
            smsg += "<br>Password is: " + CommonClasses.DecriptText(DtUserDetails.Rows[0]["UM_PASSWORD"].ToString());

            MailMessage message = new MailMessage();
            try
            {
                message.To.Add(new MailAddress(txtEmail.Text.Trim()));
                message.From = new MailAddress("dynamischiterp@gmail.com");
                message.Subject = "DIT-ERP  || Message From DITERP";
                message.Body = smsg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Port = int.Parse("587");
                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential("dynamischiterp@gmail.com", "ABCD123$$");
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.EnableSsl = true;
                client.Send(message);
                txtEmail.Text = "";
                lblmsg.Visible = true;
            }
            catch (Exception ex)
            {
                //catch block goes here
            }
        }
    }
}