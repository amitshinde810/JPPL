using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewDefectWiseFoundryRejYearly : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='136'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");

                chkDateAll.Checked = true;
                chkDateAll.Visible = false;
                LoadCombos();
                ddlFinishedComponent.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlDefect.Enabled = false;
                chkAllDefect.Checked = true;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkAllItem.Checked = true;
                LoadDefect();
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
            DataTable dtItemDet = new DataTable();

            dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE AND  IRN_TRANS_TYPE=0 AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and    I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "' ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "IRND_I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "IRND_I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadDefect
    private void LoadDefect()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            if (chkAllItem.Checked == true)
            {
                dtItemDet = CommonClasses.Execute(" SELECT DISTINCT  RM_CODE,RSM_NAME+' '+ RM_DEFECT  AS RM_DEFECT FROM REASON_MASTER,IRN_DETAIL,IRN_ENTRY,REJECTIONSTAGE_MASTER where REASON_MASTER.ES_DELETE=0  AND IRND_RM_CODE=RM_CODE AND IRN_CODE=IRND_IRN_CODE  AND  RSM_CODE=RM_RSM_CODE AND RM_CM_ID= '" + (string)Session["CompanyId"] + "'  AND     IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "'   ORDER BY RM_DEFECT");
            }
            else
            {
                dtItemDet = CommonClasses.Execute(" SELECT DISTINCT  RM_CODE,RSM_NAME+' '+ RM_DEFECT  AS RM_DEFECT FROM REASON_MASTER,IRN_DETAIL,IRN_ENTRY,REJECTIONSTAGE_MASTER where REASON_MASTER.ES_DELETE=0  AND IRND_RM_CODE=RM_CODE AND IRN_CODE=IRND_IRN_CODE  AND  RSM_CODE=RM_RSM_CODE  AND RM_CM_ID= '" + (string)Session["CompanyId"] + "'  AND     IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "'  AND IRND_I_CODE='" + ddlFinishedComponent.SelectedValue + "'  ORDER BY RM_DEFECT");
            }

            ddlDefect.DataSource = dtItemDet;
            ddlDefect.DataTextField = "RM_DEFECT";
            ddlDefect.DataValueField = "RM_CODE";
            ddlDefect.DataBind();
            ddlDefect.Items.Insert(0, new ListItem("Select Defect", "0"));
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
                    StrCond = StrCond + "  IRN_DATE between '" + txtFromDate.Text + "' and '" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  AND ";
                }
                else
                {
                    StrCond = StrCond + " IRN_DATE between '" + txtFromDate.Text + "' and '" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  AND ";
                }
                if (chkAllItem.Checked != true)
                {
                    StrCond = StrCond + " IRND_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
                }
                Response.Redirect("../../IRN/ADD/DefectWiseFoundryRejYearly.aspx?Title=" + Title + "&FDATE=" + txtFromDate.Text + "&TDATE=" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&type=" + tbType.SelectedValue.ToString() + "", false);
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

    #region chkAllDefect_CheckedChanged
    protected void chkAllDefect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllDefect.Checked == true)
        {
            ddlDefect.SelectedIndex = 0;
            ddlDefect.Enabled = false;
        }
        else
        {
            ddlDefect.SelectedIndex = 0;
            ddlDefect.Enabled = true;
            ddlDefect.Focus();
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
        LoadDefect();
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(12).AddDays(-1).ToString("MMM yyyy");
        LoadCombos();
        LoadDefect();
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);
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
        DateTime dttdate = Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1);

        txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).AddMonths(-11).ToString("MMM yyyy");
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        if ((dttdate - dtFdate).TotalDays > 365)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You can not select more than year";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(12).AddDays(-1).ToString("MMM yyyy");
            txtTodate.Focus();
            return;
        }
        LoadCombos();
        LoadDefect();
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
    #endregion
}
