using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class RoportForms_VIEW_ViewSubContactorPORateReport1n2 : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='250'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlSubContracter.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllParty.Checked = true;
            ChkAllRate.Checked = true;
            ddlRate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtsubCont = new DataTable();
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str = "SPOM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");

                str = str = "SPOM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
            }
            if (ChkAllRate.Checked != true)
            {
                if (ddlRate.SelectedIndex != 0)
                {
                    str = str + "SPOD_RATE='" + ddlRate.SelectedValue + "' AND ";
                }
            }
            dtsubCont = CommonClasses.Execute("SELECT DISTINCT PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM PARTY_MASTER INNER JOIN SUPP_PO_MASTER ON PARTY_MASTER.P_CODE = SUPP_PO_MASTER.SPOM_P_CODE INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE WHERE " + str + " (PARTY_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.P_CM_COMP_ID = " + (string)Session["CompanyId"] + ") AND (PARTY_MASTER.P_TYPE = '2') AND (PARTY_MASTER.P_ACTIVE_IND = 1) AND SPOD_RATE IN(1,2) ORDER BY PARTY_MASTER.P_NAME");
            ddlSubContracter.DataSource = dtsubCont;
            ddlSubContracter.DataTextField = "P_NAME";
            ddlSubContracter.DataValueField = "P_CODE";
            ddlSubContracter.DataBind();
            ddlSubContracter.Items.Insert(0, new ListItem("Sub Contractor", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Con. Report", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception)
        {
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
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllParty_CheckedChanged
    protected void chkAllParty_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllParty.Checked == true)
        {
            ddlSubContracter.SelectedIndex = 0;
            ddlSubContracter.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlSubContracter.SelectedIndex = 0;
            ddlSubContracter.Enabled = true;
            ddlSubContracter.Focus();
        }
    }
    #endregion

    #region ChkAllRate_CheckedChanged
    protected void ChkAllRate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkAllRate.Checked == true)
        {
            ddlRate.SelectedIndex = 0;
            ddlRate.Enabled = false;
        }
        else
        {
            ddlRate.SelectedIndex = 0;
            ddlRate.Enabled = true;
            ddlRate.Focus();
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (ChkAllRate.Checked == false)
                {
                    if (ddlRate.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Rate";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlRate.Focus();
                        return;
                    }
                }
                if (chkAllParty.Checked == false)
                {
                    if (ddlSubContracter.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Sub Contractor";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlSubContracter.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                string strCond = "";

                From = txtFromDate.Text;
                To = txtToDate.Text;

                if (chkDateAll.Checked != true)
                {
                    strCond = strCond + " SPOM_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND ";
                }
                if (ChkAllRate.Checked != true)
                {
                    strCond = strCond + " SPOD_RATE='" + ddlRate.SelectedValue + "' AND ";
                }

                if (chkAllParty.Checked != true)
                {
                    strCond = strCond + " P_CODE='" + ddlSubContracter.SelectedValue + "' AND ";
                }

                Response.Redirect("../../RoportForms/ADD/AddSubContactorPORateReport1n2.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&strCond=" + strCond + "", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Po Rate Report", "btnShow_Click", Ex.Message);
        }
    }

    protected void ddlRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
}


