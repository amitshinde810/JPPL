using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewPerformanceSummaryReport : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                //txtFromDate.Text = Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy");
                //txtTodate.Text = Convert.ToDateTime(System.DateTime.Now).AddMonths(1).ToString("dd/MMM/yyyy");
                DateTime dtcurrentdate = System.DateTime.Now;
                txtFromDate.Text = dtcurrentdate.ToString("01 MMM yyyy");
                txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                chkDateAll.Checked = false;
                chkDateAll.Visible = false;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                tbType.SelectedValue = "0";
                ddlLineName.Enabled = false;
                chkLine.Checked = true;
                LoadLine();
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {

                if (chkLine.Checked == false)
                {
                    if (ddlLineName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Line Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                From1 = Convert.ToDateTime(txtFromDate.Text);
                To2 = Convert.ToDateTime(txtTodate.Text);
                txtFromDate.Text = From1.ToString("dd MMM yyyy");
                if (tbType.SelectedValue=="0")
                {
                    txtTodate.Text = To2.ToString("dd MMM yyyy"); 
                }
                else
                {
                    txtTodate.Text = To2.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
                }

                Response.Redirect("../../IRN/ADD/PerformanceSummaryReport1.aspx?Title=" + Title + "&FDATE=" + txtFromDate.Text + "&TDATE=" + txtTodate.Text + "&L_Name=" + ddlLineName.SelectedValue.ToString() + "&type=" + tbType.SelectedValue.ToString() + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Performance Summary Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Performance Summary Report", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Performance Summary Report", "ShowMessage", Ex.Message);
            return false;
        }
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

    #region tbType_SelectedIndexChanged
    protected void tbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime From1 = new DateTime();
        DateTime To2 = new DateTime();
        if (tbType.SelectedValue == "0") //.Equals(0)
        {
            From1 = DateTime.Now;
            To2 = DateTime.Now.AddMonths(1);
            txtFromDate_CalenderExtender.BehaviorID = "";
            txtFromDate_CalenderExtender.TargetControlID = "txtFromDate";
            DateTime dtcurrentdate = System.DateTime.Now;
            txtFromDate.Text = dtcurrentdate.ToString("01 MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(30).ToString("dd MMM yyyy");
            txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
            txtFromDate_CalenderExtender.Enabled = true;
            CalendarExtender1.BehaviorID = "";
            CalendarExtender1.PopupButtonID = "txtTodate";
            CalendarExtender1.TargetControlID = "txtTodate";
            CalendarExtender1.Enabled = true;
            //txtFromDate.Text = From1.ToString("dd MMM yyyy");
            //txtTodate.Text = To2.ToString("dd MMM yyyy");
        }
        else
        {
            From1 = Convert.ToDateTime(Session["OpeningDate"]);
            To2 = Convert.ToDateTime(Session["ClosingDate"]);
            txtFromDate_CalenderExtender.BehaviorID = "calendar1";
            txtFromDate_CalenderExtender.TargetControlID = "txtFromDate";
            txtFromDate_CalenderExtender.OnClientHidden = "onCalendarHidden";
            txtFromDate_CalenderExtender.OnClientShown = "onCalendarShown";
            txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
            txtFromDate_CalenderExtender.Enabled = true;
            CalendarExtender1.BehaviorID = "calendar2";
            CalendarExtender1.OnClientHidden = "onCalendarHidden2";
            CalendarExtender1.OnClientShown = "onCalendarShown2";
            CalendarExtender1.PopupButtonID = "txtTodate";
            CalendarExtender1.TargetControlID = "txtTodate";
            CalendarExtender1.Enabled = true;
            txtFromDate.Text = From1.ToString("MMM yyyy");
            txtTodate.Text = To2.ToString("MMM yyyy");
        }
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        // Check flag for Monthly/Yearly.
        if (tbType.SelectedValue == "0")  //.Equals(0)
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
        else
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(12).AddDays(-1).ToString("MMM/yyyy");

        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);
            if (tbType.SelectedValue == "1") //.Equals(1)
                txtFromDate.Text = fdate.ToString("MMM yyyy");
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
                txtFromDate.Focus();
                return;
            }
        }
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        DateTime dttdate = Convert.ToDateTime(txtTodate.Text);
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        if ((dttdate - dtFdate).TotalDays > 31)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You can not select more than 31 days.";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(31).ToString("dd MMM yyyy");
            txtTodate.Focus();
            return;
        }
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtTodate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtTodate.Focus();
                return;
            }
        }
    }
    #endregion

    #region ddlLineName_SelectedIndexChanged
    protected void ddlLineName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlLineName_SelectedIndexChanged

    // Load Line Name from LINE_MASTER join with LINE_CHANGE and show only active lines from LINE_CHANGE.
    #region LoadLine
    public void LoadLine()
    {
        try
        {
            DataTable dtLine = new DataTable();
            dtLine = CommonClasses.Execute("SELECT DISTINCT LM_NAME,LM_CODE FROM LINE_MASTER INNER JOIN LINE_CHANGE ON LM_CODE = LC_LM_CODE AND ES_DELETE = 0 AND LM_CM_ID = '" + (string)Session["CompanyId"] + "' AND LC_ACTIVE = '1' ");
            ddlLineName.DataSource = dtLine;
            ddlLineName.DataTextField = "LM_NAME";
            ddlLineName.DataValueField = "LM_CODE";
            ddlLineName.DataBind();
            ddlLineName.Items.Insert(0, new ListItem("Select Line Name", "0"));
        }
        catch
        {
        }
    }
    #endregion LoadLine

    // Check All Lines and enable the Dropdown.
    #region chkLine_CheckedChanged
    protected void chkLine_CheckedChanged(object sender, EventArgs e)
    {
        if (chkLine.Checked == true)
        {
            ddlLineName.Enabled = false;
        }
        else
        {
            ddlLineName.Enabled = true;
            ddlLineName.SelectedIndex = 0;
        }
    } 
    #endregion chkLine_CheckedChanged 
}
