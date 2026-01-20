using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Data;


public partial class RoportForms_VIEW_ViewSubConReg : System.Web.UI.Page
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
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='163'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlSubContracter.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllParty.Checked = true;
            chkAllPO.Checked = true;
            ddlPONo.Enabled = false;
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
            dtsubCont = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER where " + str + " ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 AND P_STM_CODE IN (-2147483647,-2147483646) order by P_NAME");

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

    #region LoadCPONo
    private void LoadCPONo()
    {
        try
        {
            DataTable dtsubCont = new DataTable();
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "SPOM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            if (chkAllParty.Checked != true)
            {
                if (ddlSubContracter.SelectedValue != "0")
                {
                    str = str + "SPOM_P_CODE=" + ddlSubContracter.SelectedValue + " AND ";
                }
            }

            dtsubCont = CommonClasses.Execute("select SPOM_CODE,SPOM_PONO from SUPP_PO_MASTER where " + str + " SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and SPOM_POTYPE=1 order by SPOM_PONO");

            ddlPONo.DataSource = dtsubCont;
            ddlPONo.DataTextField = "SPOM_PONO";
            ddlPONo.DataValueField = "SPOM_CODE";
            ddlPONo.DataBind();
            ddlPONo.Items.Insert(0, new ListItem("PO No.", "0"));
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
            ddlSubContracter.SelectedIndex = 0;
            ddlSubContracter.Enabled = true;
            ddlSubContracter.Focus();
        }
    }
    #endregion

    #region chkAllPO_CheckedChanged
    protected void chkAllPO_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllPO.Checked == true)
        {
            ddlPONo.SelectedIndex = 0;
            ddlPONo.Enabled = false;
        }
        else
        {
            LoadCPONo();
            ddlPONo.SelectedIndex = 0;
            ddlPONo.Enabled = true;
            ddlPONo.Focus();
        }
    }
    #endregion

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
                if (chkAllPO.Checked == false)
                {
                    if (ddlPONo.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select PO No.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlPONo.Focus();
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
                if (chkAllParty.Checked != true)
                {
                    strCond = strCond + " P_CODE='" + ddlSubContracter.SelectedValue + "' AND ";
                }
                if (chkAllPO.Checked != true)
                {
                    strCond = strCond + " SPOM_CODE='" + ddlPONo.SelectedValue + "' AND ";
                }

                Response.Redirect("../../RoportForms/ADD/AddSubConReg.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&strCond=" + strCond + "", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register", "btnShow_Click", Ex.Message);
        }
    }
    protected void ddlSubContracter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCPONo();
    }
}

