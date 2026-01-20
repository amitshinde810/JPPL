using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewReasonReport : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPP");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPPMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='158'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                //txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy");
                //txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy");
                DateTime dtcurrentdate = System.DateTime.Now;
                txtFromDate.Text = dtcurrentdate.ToString("01 MMM yyyy");
                txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                chkDateAll.Checked = false;
                chkDateAll.Visible = false;
                ddlReason.Enabled = false;
                chkAllComp.Checked = true;
                LoadReason();
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkAllComp.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/COPPDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Wise Rejection Yearly", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        #region Validate_Date_with_Current_Date
        DateTime Fromdate = Convert.ToDateTime(txtFromDate.Text);
        if (Convert.ToDateTime(Fromdate.ToString("dd/MMM/yyyy")) > Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy")))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "From Date should not be greater than todays date...";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            if (rbType.SelectedValue == "0")
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            else
                txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
            txtFromDate.Focus();
            return;
        }
        #endregion Validate_Date_with_Current_Date

        if (rbType.SelectedValue == "0")
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).ToString("dd MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
        }
        else if (rbType.SelectedValue == "1")
        {
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(+11).ToString("MMM yyyy");
            DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        }
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);

            if (txtFromDate.Text != "" || txtTodate.Text != "")

                if (fdate > todate)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date should be less than equal to To Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                    txtFromDate.Focus();
                    return;
                }
        }
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        if (rbType.SelectedValue == "0")
        {
            txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).AddDays(-31).ToString("dd MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(txtTodate.Text).ToString("dd MMM yyyy");
        }
        else if (rbType.SelectedValue == "1")
        {
            DateTime dttdate = Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1);

            txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).AddMonths(-11).ToString("MMM yyyy");
            DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);

            if (txtFromDate.Text != "" || txtTodate.Text != "")
            {
                DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtTodate.Text);
                if (todate < fdate)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "To Date should be greater than equal to From Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtTodate.Text = DateTime.Today.ToString("MMM yyyy");
                    txtTodate.Focus();
                    return;
                }
            }
        }
    }
    #endregion

    #region LoadReason
    private void LoadReason()
    {
        try
        {
            DataTable dtVendor = new DataTable();
            dtVendor = CommonClasses.Execute("SELECT DISTINCT SPR_CODE,SPR_DESC FROM SHORTPROD_REASON WHERE SHORTPROD_REASON.ES_DELETE = 0 AND SPR_COMPID = '" + Session["CompanyId"].ToString() + "'  ORDER BY SPR_DESC ");
            ddlReason.DataSource = dtVendor;
            ddlReason.DataTextField = "SPR_DESC";
            ddlReason.DataValueField = "SPR_CODE";
            ddlReason.DataBind();
            ddlReason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Wise Rejection Yearly", "LoadReason", Ex.Message);
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtTodate.Enabled = false;

            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtTodate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtTodate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            txtTodate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
        }
        dateCheck();
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
            }
        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
        }
    }

    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlReason.SelectedIndex = 0;
            ddlReason.Enabled = false;
        }
        else
        {
            ddlReason.SelectedIndex = 0;
            ddlReason.Enabled = true;
            ddlReason.Focus();
        }
    }
    #endregion  chkAllComp_CheckedChanged

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string StrCond = "";
            if (chkDateAll.Checked == false)
            {
                if (txtTodate.Text != "" && txtFromDate.Text != "")
                {
                }
                else
                {
                    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                }
            }
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtTodate.Text != "" && txtFromDate.Text != "")
                    {
                        StrCond = StrCond + "IRN_ENTRY.IRN_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtTodate.Text + "' AND ";
                    }
                }
                if (chkAllComp.Checked == false)
                {
                    if (ddlReason.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Vendor Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "SHORTPROD_REASON.SPR_CODE=" + ddlReason.SelectedValue + " AND ";
                    }
                }
                string StrParty = "ALL";
                if (chkAllComp.Checked != true)
                {
                    StrParty = ddlReason.SelectedValue.ToString();
                }
                Response.Redirect("../../IRN/ADD/AddReasonReport.aspx?Title=" + Title + "&FDATE=" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "&TDATE=" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "&type=" + rbType.SelectedValue.ToString() + "&Cond=" + StrCond + "&StrParty=" + StrParty + "", false);//.AddMonths(1).AddDays(-1)
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Wise Rejection Yearly", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region rbType_SelectedIndexChanged
    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbType.SelectedValue == "0")
        {
            txtTodate.Enabled = false;
            DateTime dtcurrentdate = System.DateTime.Now;
            txtFromDate.Text = dtcurrentdate.ToString("01 MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(30).ToString("dd MMM yyyy");
            //txtFromDate.Text = System.DateTime.Now.AddDays(-31).ToString("dd MMM yyyy");
            //txtTodate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            txtTodate.Enabled = true;
            txtFromDate_CalenderExtender.BehaviorID = "";
            txtFromDate_CalenderExtender.OnClientHidden = "";
            txtFromDate_CalenderExtender.OnClientShown = "";
            txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
            txtFromDate_CalenderExtender.Format = "dd MMM yyyy";
            txtTodate.Enabled = true;
            txtTodate_CalendarExtender.BehaviorID = "";
            txtTodate_CalendarExtender.OnClientHidden = "";
            txtTodate_CalendarExtender.OnClientShown = "";
            txtTodate_CalendarExtender.PopupButtonID = "txtTodate";
            txtTodate_CalendarExtender.Format = "dd MMM yyyy";
        }
        else
        {
            txtTodate.Enabled = true;
            txtFromDate_CalenderExtender.BehaviorID = "calendar1";
            txtFromDate_CalenderExtender.OnClientHidden = "onCalendarHidden";
            txtFromDate_CalenderExtender.OnClientShown = "onCalendarShown";
            txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
            txtFromDate_CalenderExtender.Format = "MMM yyyy";
            txtTodate.Enabled = true;
            txtTodate_CalendarExtender.BehaviorID = "calendar2";
            txtTodate_CalendarExtender.OnClientHidden = "onCalendarHidden2";
            txtTodate_CalendarExtender.OnClientShown = "onCalendarShown2";
            txtTodate_CalendarExtender.PopupButtonID = "txtTodate";
            txtTodate_CalendarExtender.Format = "MMM yyyy";
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");
        }
    }
    #endregion
}
