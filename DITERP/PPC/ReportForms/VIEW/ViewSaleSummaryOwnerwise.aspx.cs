using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewSaleSummaryOwnerwise : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='25'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            LoadOwner();
            txtFromDate.Enabled = true;
            chkDateAll.Checked = true;
            chkAllOwner.Checked = true;
            chkAllComp.Checked = true;
            ddlOwner.Enabled = false;
            ddlGroupName.Enabled = false;
        }
    }
    #endregion

    #region LoadOwner
    protected void LoadOwner()
    {
        DataTable dtOwner = new DataTable();
        dtOwner = CommonClasses.Execute("SELECT distinct UM_CODE,UM_NAME from GROUP_MASTER inner join USER_MASTER on GP_OWNER=UM_CODE where GROUP_MASTER.ES_DELETE=0 AND USER_MASTER.ES_DELETE=0 and UM_CM_ID='" + Session["CompanyId"] + "' ORDER BY UM_NAME");
        ddlOwner.DataSource = dtOwner;
        ddlOwner.DataTextField = "UM_NAME";
        ddlOwner.DataValueField = "UM_CODE";
        ddlOwner.DataBind();
        ddlOwner.Items.Insert(0, new ListItem("Select Owner Name", "0"));
    }
    #endregion LoadOwner

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception)
        {

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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void ddlGroupName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #region ddlOwnerName_SelectedIndexChanged
    protected void ddlOwnerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCond = "";
        if (chkAllOwner.Checked != true)
        {
            if (ddlOwner.SelectedValue != "-1" || ddlOwner.SelectedValue != "0")
            {
                strCond = strCond + "and GP_OWNER=" + ddlOwner.SelectedValue + " and";
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Owner Name...";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlGroupName.Focus();
                return;
            }
        }//DataTable dt = CommonClasses.Execute("SELECT GP_CODE,GP_NAME from USER_MASTER,GROUP_MASTER where GROUP_MASTER.GP_OWNER=UM_CODE and UM_CODE=" + ddlOwner.SelectedValue + " AND GROUP_MASTER.ES_DELETE=0 AND USER_MASTER.ES_DELETE=0 and UM_CM_ID='" + Session["CompanyId"].ToString() + "' ORDER BY GP_NAME");
        DataTable dt = CommonClasses.Execute("SELECT distinct G.GP_CODE,G.GP_NAME from GROUP_MASTER G inner join USER_MASTER U on GP_OWNER=UM_CODE inner join PRODUCT_MASTER P ON G.GP_CODE=P.PROD_GP_CODE  INNER JOIN CUSTOMER_SCHEDULE C ON P.PROD_I_CODE=C.CS_I_CODE WHERE G.ES_DELETE=0 AND C.ES_DELETE=0 AND U.ES_DELETE=0 AND P.ES_DELETE=0 AND CS_COMP_ID='" + Session["CompanyId"].ToString() + "' AND CONVERT(DATE,CS_DATE)='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'");
        ddlGroupName.DataSource = dt;
        ddlGroupName.DataTextField = "GP_NAME";
        ddlGroupName.DataValueField = "GP_CODE";
        ddlGroupName.DataBind();
        ddlGroupName.Items.Insert(0, new ListItem("Select Group Name", "0"));
    }
    #endregion ddlOwnerName_SelectedIndexChanged

    protected void chkAllOwner_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllOwner.Checked == true)
        {
            ddlOwner.Enabled = false;
        }
        else
        {
            ddlOwner.Enabled = true;
        }
    }

    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlGroupName.Enabled = false;
        }
        else
        {
            ddlGroupName.Enabled = true;
        }
    }

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkAllOwner.Checked == false)
                {
                    if (ddlOwner.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Owner";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlGroupName.Focus();
                        return;
                    }
                }
                if (chkAllComp.Checked == false)
                {
                    if (ddlGroupName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Group";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlGroupName.Focus();
                        return;
                    }
                }
                DateTime From = Convert.ToDateTime(txtFromDate.Text);
                string strCondition = "";
                strCondition = "CONVERT(date, CS_DATE) ='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' and ";

                if (chkAllOwner.Checked != true)
                {
                    strCondition = strCondition + "GP_OWNER= '" + ddlOwner.SelectedValue + "' AND ";
                }
                //

                if (chkAllComp.Checked != true)
                {
                    strCondition = strCondition + "GP_CODE= '" + ddlGroupName.SelectedValue + "' AND ";
                }
                string title = Title;
                title = Title + " " + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "";
                Response.Redirect("../../ReportForms/ADD/SaleSummaryCustomerwise.aspx?Title=" + title + "&Cond=" + strCondition + "&Type=Owner", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {

        if (txtFromDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
        }
    }


}
