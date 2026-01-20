using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewAnalysisOfShortProductionReport : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='134'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                txtTodate.Text = DateTime.Now.ToString("dd MMM yyyy");//Convert.ToDateTime(txtFromDate.Text).AddDays(31).ToString("dd MMM yyyy");
                chkDateAll.Checked = true;
                chkDateAll.Visible = false;
                LoadCombos();
                LoadLine();
                ddlFinishedComponent.Enabled = false;
                ddlItemCode.Enabled = false;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkAllItem.Checked = true;
                ddlLineName.Enabled = false;
                chkLine.Checked = true;
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
            string str = "";
            if (ddlLineName.SelectedIndex != 0)
            {
                str = str + "LM_CODE='" + ddlLineName.SelectedValue + "' AND ";
            }
            DataTable dtItemDet = new DataTable();
            // dtItemDet = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE , I_NAME , I_CODENO  FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN ITEM_MASTER ON (IRN_DETAIL.IRND_I_CODE =  ITEM_MASTER.I_CODE) WHERE ITEM_MASTER.I_CM_COMP_ID = '" + (string)Session["CompanyId"] + "' AND ITEM_MASTER.ES_DELETE = 0 AND IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "' AND IRN_ENTRY.ES_DELETE = '0' ORDER BY I_NAME");
            dtItemDet = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_NAME, ITEM_MASTER.I_CODENO FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN ITEM_MASTER ON IRN_DETAIL.IRND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN LINE_CHANGE ON IRN_DETAIL.IRND_I_CODE = LINE_CHANGE.LC_I_CODE INNER JOIN LINE_MASTER ON LINE_CHANGE.LC_LM_CODE = LINE_MASTER.LM_CODE WHERE " + str + " LC_ACTIVE=1 AND (ITEM_MASTER.I_CM_COMP_ID = '1') AND (ITEM_MASTER.ES_DELETE = 0) AND (IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "') AND (IRN_ENTRY.ES_DELETE = '0') and LINE_MASTER.ES_DELETE=0 ORDER BY ITEM_MASTER.I_NAME");
            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
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
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtTodate.Text;

                // Server Side Validation check Item Name and Line is Seleected and then Proceed for add form
                #region Validation
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
                if (chkLine.Checked == false)
                {
                    if (ddlLineName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Line Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlLineName.Focus();
                        return;
                    }
                }

                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtTodate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }

                #endregion Validation

                From1 = Convert.ToDateTime(txtFromDate.Text);
                To2 = Convert.ToDateTime(txtTodate.Text);
                txtFromDate.Text = From1.ToString("dd MMM yyyy");
                txtTodate.Text = To2.ToString("dd MMM yyyy");
                string StrCond = "";

                if (chkDateAll.Checked != true)
                {
                    StrCond = StrCond + " IRN_DATE between '" + txtFromDate.Text + "' and '" + Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  AND ";
                }
                else
                {
                    StrCond = StrCond + " IRN_DATE between '" + txtFromDate.Text + "' and '" + Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  AND ";
                }
                if (chkLine.Checked != true)
                {
                    StrCond = StrCond + "LC_LM_CODE='" + ddlLineName.SelectedValue + "' AND ";
                }
                if (chkAllItem.Checked != true)
                {
                    StrCond = StrCond + " IRND_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
                }

                Response.Redirect("../../IRN/ADD/AnalysisOfShortProductionReport.aspx?Title=" + Title + "&FDATE=" + txtFromDate.Text + "&TDATE=" + Convert.ToDateTime(txtTodate.Text) + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&type=" + tbType.SelectedValue.ToString() + "&Cond=" + StrCond + "&line=" + ddlLineName.SelectedItem + "", false);
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
            Response.Redirect("~/Masters/ADD/COPPDefault.aspx", false);
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
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
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
            LoadCombos();
        }
        else
        {
            txtFromDate.Enabled = true;
            txtTodate.Enabled = true;

            txtFromDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            txtTodate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            LoadCombos();
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
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        // txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(31).ToString("dd/MMM/yyyy");
        LoadCombos();
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
            LoadLine();
        }
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        DateTime dttdate = Convert.ToDateTime(txtTodate.Text);
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        //if ((dttdate - dtFdate).TotalDays > 31)
        //{
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "You can not select more than 31 days.";
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //    txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(31).ToString("dd MMM yyyy");
        //    txtTodate.Focus();
        //    return;
        //}
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
                txtTodate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtTodate.Focus();
                return;
            }
            LoadLine();
        }
    }
    #endregion

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

    #region ddlLineName_SelectedIndexChanged
    protected void ddlLineName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion ddlLineName_SelectedIndexChanged

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