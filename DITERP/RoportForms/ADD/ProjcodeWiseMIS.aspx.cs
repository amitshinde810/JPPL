using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_ADD_ProjcodeWiseMIS : System.Web.UI.Page
{
    public static string Cond = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            ViewState["Cond"] = Cond;

            ddlProCode.Enabled = false;
            chkProCodeAll.Checked = true;
            loadpo();
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            LoadProCode();
            if (chkDateAll.Checked == true)
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "'  ";
            }
            else
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "'  ";
            }
            if (chkProCodeAll.Checked != true)
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_PROJECT = '" + ddlProCode.SelectedValue + "'  ";
            }
        }
    }
    #endregion

    #region Popup

    #region btnConfirm_Click
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            CommonClasses.WriteLogActivity("Dashboard", "Confirm", "Dashboard", Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Dashboard", "btnConfirm_Click", Ex.Message);
        }
    }
    #endregion

    #region dgActivity_Task_RowCommand
    protected void dgActivity_Task_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion
    #endregion

    #region loadpo
    public void loadpo()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" SELECT DISTINCT PROCM_CODE,PROCM_NAME,(SELECT COUNT(SPOM_CODE) FROM SUPP_PO_MASTER where SPOM_CODE=SPOM_CODE AND SPOM_PROJECT=PROCM_CODE AND   SPOM_POTYPE=0 AND SUPP_PO_MASTER.ES_DELETE=0  " + ViewState["Cond"].ToString() + "   ) AS PO_COUNT ,ROUND(SUM(SPOD_ORDER_QTY*(SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY))),2 ) AS PO_AMOUNT,ROUND(SUM(SPOD_INW_QTY),2) INWARD_QTY ,ROUND(SUM(SPOD_INW_QTY*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2) AS INWARD_AMT,ROUND(SUM((SPOD_ORDER_QTY-SPOD_INW_QTY)*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2)  AS BAL_AMT  FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,PARTY_MASTER,ITEM_MASTER  where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0   AND SPOM_PROJECT=PROCM_CODE AND P_CODE=SPOM_P_CODE AND SPOD_I_CODE=I_CODE AND   SPOM_POTYPE=0  " + ViewState["Cond"].ToString() + "  GROUP BY PROCM_CODE,PROCM_NAME");
        dgProjectCode.DataSource = dt;
        dgProjectCode.DataBind();
    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("SELECT DISTINCT PROCM_CODE,PROCM_NAME FROM PROJECT_CODE_MASTER,SUPP_PO_MASTER WHERE PROJECT_CODE_MASTER.ES_DELETE=0  AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_PROJECT=PROCM_CODE AND SPOM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_POTYPE=0   order by PROCM_NAME");
            ddlProCode.DataSource = dt;
            ddlProCode.DataTextField = "PROCM_NAME";
            ddlProCode.DataValueField = "PROCM_CODE";
            ddlProCode.DataBind();
            ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region dgProjectCode_PageIndexChanging
    protected void dgProjectCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgProjectCode.PageIndex = e.NewPageIndex;
    }
    #endregion

    #region dgProjCodeWisePO_PageIndexChanging
    protected void dgProjCodeWisePO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgProjCodeWisePO.PageIndex = e.NewPageIndex;
    }
    #endregion

    #region dgPO_PageIndexChanging
    protected void dgPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgPO.PageIndex = e.NewPageIndex;
    }
    #endregion

    #region dgProjectCode_RowDataBound
    protected void dgProjectCode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    #endregion

    #region dgProjectCode_RowCommand
    protected void dgProjectCode_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string type = "VIEW";
        string inv_code = e.CommandArgument.ToString();

        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT DISTINCT PROCM_CODE,PROCM_NAME,SPOM_CODE,SPOM_PONO,SPOM_PO_NO ,COUNT(SPOD_CODE) AS ITEM_COUNT,ROUND(SUM(SPOD_ORDER_QTY*(SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY))),2 ) AS PO_AMOUNT,ROUND(SUM(SPOD_INW_QTY),2) INWARD_QTY ,ROUND(SUM(SPOD_INW_QTY*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2) AS INWARD_AMT,ROUND(SUM((SPOD_ORDER_QTY-SPOD_INW_QTY)*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2)  AS BAL_AMT, SUM(SPOD_ORDER_QTY) AS ORDER_QTY,SUM(SPOD_ORDER_QTY-SPOD_INW_QTY) AS BAL_QTY FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,PARTY_MASTER,ITEM_MASTER  where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0   AND PROCM_CODE='" + inv_code + "' AND SPOM_POTYPE=0 AND SPOM_PROJECT=PROCM_CODE AND P_CODE=SPOM_P_CODE AND SPOD_I_CODE=I_CODE  " + ViewState["Cond"].ToString() + " GROUP BY PROCM_CODE,PROCM_NAME,SPOM_PONO,SPOM_PO_NO,SPOM_CODE");
        dgProjCodeWisePO.DataSource = dt;
        dgProjCodeWisePO.DataBind();
        popUpPanel5.Visible = true;
        ModalCancleConfirmation.Show();
    }
    #endregion

    #region dgProjCodeWisePO_RowCommand
    protected void dgProjCodeWisePO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string type = "VIEW";
        string inv_code = e.CommandArgument.ToString();
        Panel1.Visible = true;
        ModalPopupExtender1.Show();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("  SELECT DISTINCT PROCM_CODE,PROCM_NAME,SPOM_CODE,SPOM_PONO,SPOM_PO_NO, I_CODE,I_CODENO,I_NAME,ROUND(SUM(SPOD_ORDER_QTY*(SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY))),2 ) AS PO_AMOUNT,ROUND(SUM(SPOD_INW_QTY),2) INWARD_QTY ,ROUND(SUM(SPOD_INW_QTY*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2) AS INWARD_AMT,ROUND(SUM((SPOD_ORDER_QTY-SPOD_INW_QTY)*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2)  AS BAL_AMT, SUM(SPOD_ORDER_QTY) AS ORDER_QTY,SUM(SPOD_ORDER_QTY-SPOD_INW_QTY) AS BAL_QTY,SUM((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY))) AS RATE FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,PARTY_MASTER,ITEM_MASTER where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0  AND SPOM_POTYPE=0 AND SPOM_CODE='" + inv_code + "' AND SPOM_PROJECT=PROCM_CODE AND P_CODE=SPOM_P_CODE AND SPOD_I_CODE=I_CODE    GROUP BY PROCM_CODE,PROCM_NAME,SPOM_PONO,SPOM_PO_NO,SPOM_CODE, I_CODE,I_CODENO,I_NAME");
        dgPO.DataSource = dt;
        dgPO.DataBind();
    }
    #endregion

    #region btnBack_Click
    protected void btnBack_Click(object sender, EventArgs e)
    {
        loadpo();
    }
    #endregion

    #region Button1dd_Click
    protected void Button1dd_Click(object sender, EventArgs e)
    {
        popUpPanel5.Visible = true;
        ModalCancleConfirmation.Show();
    }
    #endregion

    #region chkProCodeAll_CheckedChanged
    protected void chkProCodeAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkProCodeAll.Checked == true)
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = false;
        }
        else
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = true;
            ddlProCode.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Cond"] = "";
            if (chkDateAll.Checked == true)
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "'  ";
            }
            else
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "'  ";
            }
            if (chkProCodeAll.Checked != true)
            {
                ViewState["Cond"] = ViewState["Cond"].ToString() + "  AND SPOM_PROJECT = '" + ddlProCode.SelectedValue + "'  ";
            }
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(" SELECT DISTINCT PROCM_CODE,PROCM_NAME,  (SELECT COUNT(SPOM_CODE)  FROM SUPP_PO_MASTER where  SPOM_CODE=SPOM_CODE AND SPOM_PROJECT=PROCM_CODE AND   SPOM_POTYPE=0 AND SUPP_PO_MASTER.ES_DELETE=0 " + ViewState["Cond"].ToString() + " ) AS PO_COUNT ,ROUND(SUM(SPOD_ORDER_QTY*(SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY))),2 ) AS PO_AMOUNT,ROUND(SUM(SPOD_INW_QTY),2) INWARD_QTY ,ROUND(SUM(SPOD_INW_QTY*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2) AS INWARD_AMT,ROUND(SUM((SPOD_ORDER_QTY-SPOD_INW_QTY)*((SPOD_RATE-(SPOD_DISC_AMT/SPOD_ORDER_QTY)))),2)  AS BAL_AMT  FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,PARTY_MASTER,ITEM_MASTER  where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0   AND SPOM_PROJECT=PROCM_CODE AND P_CODE=SPOM_P_CODE AND SPOD_I_CODE=I_CODE AND   SPOM_POTYPE=0  " + ViewState["Cond"].ToString() + " GROUP BY PROCM_CODE,PROCM_NAME");
            dgProjectCode.DataSource = dt;
            dgProjectCode.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/ADD/ProjcodeWiseMIS.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Inward Supplierwise", "btnCancel_Click", ex.Message);
        }
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadProCode();
    }
}
