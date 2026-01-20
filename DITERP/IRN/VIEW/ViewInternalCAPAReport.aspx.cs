using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewInternalCAPAReport : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='138'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");
                chkDateAll.Checked = true; 
                chkAllItem.Checked = true;
                chkDateAll_CheckedChanged(null, null);
                LoadCombos();
                ddlFinishedComponent.Enabled = false; 
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkAllItem.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItem = new DataTable();
            DateTime date = Convert.ToDateTime(txtFromDate.Text);
            DateTime dateFrom = Convert.ToDateTime(txtFromDate.Text);
            DateTime dateTo = Convert.ToDateTime(txtTodate.Text);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var LastDate = dateTo.AddMonths(1).AddDays(-1);
            dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME FROM LINE_MASTER INNER JOIN  LINE_CHANGE ON LINE_MASTER.LM_CODE = LINE_CHANGE.LC_LM_CODE INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE WHERE     (LINE_MASTER.ES_DELETE = 0)  AND LC_DATE BETWEEN '" + Convert.ToDateTime(dateFrom).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(LastDate).ToString("dd/MMM/yyyy") + "'  AND LM_CM_ID = '" + (string)Session["CompanyId"] + "' ORDER BY ICODE_INAME ");
            ddlFinishedComponent.DataSource = dtItem;
            ddlFinishedComponent.DataTextField = "ICODE_INAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
             
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                DateTime date = Convert.ToDateTime(txtFromDate.Text);
                DateTime dateFrom = Convert.ToDateTime(txtFromDate.Text);
                DateTime dateTo = Convert.ToDateTime(txtTodate.Text);
                var firstDayOfMonth = new DateTime(dateFrom.Year, date.Month, 1);
                var lastDayOfMonth = dateTo.AddMonths(1).AddDays(-1);
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Item Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                 
                string StrCond = "";
                if (chkDateAll.Checked != true)
                {
                    firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                    lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    StrCond = StrCond + " IRN_DATE between = '" + Convert.ToDateTime(firstDayOfMonth) + "' AND '" + Convert.ToDateTime(lastDayOfMonth) + "' AND ";
                }
                if (chkAllItem.Checked != true)
                {
                    StrCond = StrCond + "  IRND_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND ";
                }
                Response.Redirect("../../IRN/ADD/InternalCAPAReport.aspx?Title=" + Title + "&DATE=" + firstDayOfMonth + "&TDATE=" + lastDayOfMonth + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
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

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false; 
        }
        else
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("MMM yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
        }
        dateCheck();
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        DateTime dttdate = Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1);
        txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).AddMonths(-11).ToString("MMM yyyy");
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);

        LoadCombos();
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
    #endregion txtTodate_TextChanged

     
}
