using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewTurningRegisterReport : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='114'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadCombos();
            ddlSubContracter.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllParty.Checked = true;
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtsubCont = new DataTable();
            dtsubCont = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1  AND P_STM_CODE IN (-2147483647,-2147483646)  order by P_NAME");

            ddlSubContracter.DataSource = dtsubCont;
            ddlSubContracter.DataTextField = "P_NAME";
            ddlSubContracter.DataValueField = "P_CODE";
            ddlSubContracter.DataBind();
            ddlSubContracter.Items.Insert(0, new ListItem("Select Sub Cont", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Turning Report", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
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
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
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
                        return;
                    }
                }

                if (chkAllParty.Checked == false)
                {
                    if (ddlSubContracter.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ddlSubContracter.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;


                if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
                {
                    From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
                }
                else
                {
                    From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                }

                string str1 = "";
                string str = "";
                string strType = "";

                str1 = "Detail";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "SubContWise";
                }
                string strCond = "";
                if (chkDateAll.Checked != true)
                {
                    strCond = strCond + " IWM_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
                }
                if (chkAllParty.Checked != true)
                {
                    strCond = strCond + "  P_CODE='" + ddlSubContracter.SelectedValue + "'  AND   ";
                }
                if (rbLstType.SelectedIndex == 0)
                {
                    strType = "Detail";
                }
                else
                {
                    strType = "Summary";
                }
                Response.Redirect("../../RoportForms/ADD/TurningRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&strCond=" + strCond + "&strType=" + strType + "", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register", "btnShow_Click", Ex.Message);
        }
    }
}
