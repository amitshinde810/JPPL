using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewCompWiseReport : System.Web.UI.Page
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
                chkDateAll.Checked = false;
                chkDateAll_CheckedChanged(null, null);
                LoadCombos();
                ddlFinishedComponent.Enabled = false;
                ddlItemCode.Enabled = false;
                txtFromDate.Attributes.Add("readonly", "readonly");
                LoadStgae();
                LoadLine();
                chkAllItem.Checked = true;
                chkStgae.Checked = true;
                ddlStage.Enabled = false;
                chKLine.Checked = true;
                ddlLine.Enabled = false;
                rbSelType.Visible = false;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }

        PanelMsg.Visible = false;
        lblmsg.Text = "  ";
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            DateTime date = Convert.ToDateTime(txtFromDate.Text);
            DateTime fromdate = new DateTime();
            DateTime todate = new DateTime();
            if (rbType.SelectedValue == "0")
            {
                fromdate = new DateTime(date.Year, date.Month, 1);
                todate = fromdate.AddMonths(1).AddDays(-1);
                rbSelType.Visible = false;
            }
            else
            {
                fromdate = new DateTime(date.Year, date.Month, 1);
                todate = fromdate.AddMonths(12).AddDays(-1);
                rbSelType.Visible = true;
            }

            dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "' ORDER BY I_NAME");

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

    #region LoadCombos
    private void LoadCombos1()
    {
        try
        {
            DataTable dtItemDet = new DataTable();

            DateTime date = Convert.ToDateTime(txtFromDate.Text);
            DateTime fromdate = new DateTime();
            DateTime todate = new DateTime();
            if (rbType.SelectedValue == "0")
            {
                fromdate = new DateTime(date.Year, date.Month, 1);
                todate = fromdate.AddMonths(1).AddDays(-1);
                rbSelType.Visible = false;
            }
            else
            {
                fromdate = new DateTime(date.Year, date.Month, 1);
                todate = fromdate.AddMonths(12).AddDays(-1);
                rbSelType.Visible = true;
            }
            string stcond = "";
            if (chKLine.Checked != true)
            {
                if (chkStgae.Checked != true)
                {
                    dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "'  AND  I_CODE IN (SELECT SI_I_CODE  FROM STGAEWISE_ITEM where SI_ACTIVE=1 AND SI_STG_CODE='" + ddlStage.SelectedValue + "')  AND  I_CODE IN (SELECT LC_I_CODE  FROM LINE_CHANGE where LC_ACTIVE=1 AND LC_LM_CODE='" + ddlLine.SelectedValue + "') ORDER BY I_NAME");
                }
                else
                {
                    dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "'     AND  I_CODE IN (SELECT LC_I_CODE  FROM LINE_CHANGE where LC_ACTIVE=1 AND LC_LM_CODE='" + ddlLine.SelectedValue + "') ORDER BY I_NAME");
                }
            }
            else
            {
                if (chkStgae.Checked != true)
                {
                    dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "'  AND  I_CODE IN (SELECT SI_I_CODE  FROM STGAEWISE_ITEM where SI_ACTIVE=1 AND SI_STG_CODE='" + ddlStage.SelectedValue + "')  ORDER BY I_NAME");
                }
                else
                {
                    dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "' ORDER BY I_NAME");
                }
            }
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

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                DateTime date = Convert.ToDateTime(txtFromDate.Text);
                DateTime fromdate = new DateTime();
                DateTime todate = new DateTime();
                if (rbType.SelectedValue == "0")
                {
                    fromdate = new DateTime(date.Year, date.Month, 1);
                    todate = fromdate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    fromdate = new DateTime(date.Year, date.Month, 1);
                    todate = fromdate.AddMonths(12).AddDays(-1);
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
                if (chKLine.Checked == false)
                {
                    if (ddlLine.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Line";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkStgae.Checked == false)
                {
                    if (ddlStage.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Stage ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                string StrCond = "";
                string strTitle = "";
                string I_Code = "0";
                string L_Code = "0";
                string S_Code = "0";
                if (chkAllItem.Checked != true)
                {
                    I_Code = ddlFinishedComponent.SelectedValue.ToString();
                }
                if (chkStgae.Checked != true)
                {
                    S_Code = ddlStage.SelectedValue.ToString();
                    strTitle = strTitle + ddlStage.SelectedItem.ToString();
                }
                if (chKLine.Checked != true)
                {
                    L_Code = ddlLine.SelectedValue.ToString();
                    strTitle = strTitle + "  (" + ddlLine.SelectedItem.ToString() + ")";
                }
                Response.Redirect("../../IRN/ADD/CompWiseReport.aspx?Title=" + strTitle + "&DATE=" + Convert.ToDateTime(fromdate).ToString("dd/MMM/yyyy") + "&TDATE=" + Convert.ToDateTime(todate).ToString("dd/MMM/yyyy") + "&i_name=" + I_Code + "&L_name=" + L_Code + "&S_name=" + S_Code + "&type=" + rbType.SelectedValue + "&Rtype=" + rbSelType.SelectedValue + "", false);
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
            LoadCombos1();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            LoadCombos1();
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
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos1();
    }

    #region chkStgae_CheckedChanged
    protected void chkStgae_CheckedChanged(object sender, EventArgs e)
    {
        if (chkStgae.Checked == true)
        {
            ddlStage.SelectedIndex = 0;
            ddlStage.Enabled = false;
        }
        else
        {
            ddlStage.SelectedIndex = 0;
            ddlStage.Enabled = true;
            ddlStage.Focus();
        }
        LoadCombos1();
    }
    #endregion

    #region chKLine_CheckedChanged
    protected void chKLine_CheckedChanged(object sender, EventArgs e)
    {
        if (chKLine.Checked == true)
        {
            ddlLine.SelectedIndex = 0;
            ddlLine.Enabled = false;
        }
        else
        {
            ddlLine.SelectedIndex = 0;
            ddlLine.Enabled = true;
            ddlLine.Focus();
        }
        LoadCombos1();
    }
    #endregion

    #region LoadStgae
    private void LoadStgae()
    {
        try
        {
            DataTable dtStage = new DataTable();

            dtStage = CommonClasses.Execute(" SELECT DISTINCT STG_CODE ,STG_NAME  FROM STGAEWISE_ITEM,STAGE_MASTER where SI_STG_CODE=STG_CODE AND SI_ACTIVE=1  AND ES_DELETE=0 ORDER BY STG_NAME");

            ddlStage.DataSource = dtStage;
            ddlStage.DataTextField = "STG_NAME";
            ddlStage.DataValueField = "STG_CODE";
            ddlStage.DataBind();
            ddlStage.Items.Insert(0, new ListItem("Select Stage", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Register", "LoadStgae", Ex.Message);
        }
    }
    #endregion

    #region LoadLine
    private void LoadLine()
    {
        try
        {
            DataTable dtStage = new DataTable();

            dtStage = CommonClasses.Execute(" SELECT DISTINCT LM_CODE,LM_NAME FROM LINE_CHANGE,LINE_MASTER where LC_LM_CODE=LM_CODE AND LC_ACTIVE=1  AND   ES_DELETE=0 ORDER BY LM_NAME");

            ddlLine.DataSource = dtStage;
            ddlLine.DataTextField = "LM_NAME";
            ddlLine.DataValueField = "LM_CODE";
            ddlLine.DataBind();
            ddlLine.Items.Insert(0, new ListItem("Select Line", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Register", "LoadStgae", Ex.Message);
        }
    }
    #endregion

    #region LoadStageLine
    private void LoadStageLine(string ID)
    {
        try
        {
            DataTable dtLine = new DataTable();
            dtLine = CommonClasses.Execute(" SELECT DISTINCT LM_CODE,LM_NAME FROM LINE_CHANGE,LINE_MASTER where LC_LM_CODE=LM_CODE AND LC_ACTIVE=1  AND LC_I_CODE='" + ID + "' AND   ES_DELETE=0 ORDER BY LM_NAME");

            ddlLine.DataSource = dtLine;
            ddlLine.DataTextField = "LM_NAME";
            ddlLine.DataValueField = "LM_CODE";
            ddlLine.DataBind();
            ddlLine.Items.Insert(0, new ListItem("Select Line", "0"));

            DataTable dtStage = new DataTable();
            dtStage = CommonClasses.Execute(" SELECT DISTINCT STG_CODE ,STG_NAME  FROM STGAEWISE_ITEM,STAGE_MASTER where SI_STG_CODE=STG_CODE AND SI_ACTIVE=1 AND SI_I_CODE='" + ID + "' AND ES_DELETE=0 ORDER BY STG_NAME");

            ddlStage.DataSource = dtStage;
            ddlStage.DataTextField = "STG_NAME";
            ddlStage.DataValueField = "STG_CODE";
            ddlStage.DataBind();
            ddlStage.Items.Insert(0, new ListItem("Select Stage", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Register", "LoadStgae", Ex.Message);
        }
    }
    #endregion

    #region rbType_SelectedIndexChanged
    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbType.SelectedValue == "0")
        {
            chkAllItem.Checked = true;
            chkAllItem.Visible = true;
        }
        else
        {
            chkAllItem.Checked = false;
            chkAllItem.Visible = false;
        }
        chkAllItem_CheckedChanged(null, null);
    }
    #endregion

    protected void ddlLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos1();
    }

    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos1();
    }
}
