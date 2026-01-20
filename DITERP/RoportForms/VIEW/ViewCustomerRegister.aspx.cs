using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewCustomerRegister : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='29'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlCustomerName.Enabled = false;
            chkAll.Checked = true;
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY P_NAME");

            ddlCustomerName.DataSource = dt;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAll_CheckedChanged
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
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
                if (chkAll.Checked == true && ddlCustomerName.SelectedIndex == 0)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "", false);
                }
                if (chkAll.Checked == false && ddlCustomerName.SelectedIndex > 0)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "", false);
                }
                if (chkAll.Checked == false && ddlCustomerName.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Customer ", CommonClasses.MSG_Warning);
                    return;
                }
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../VIEW/ViewMasterReport.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
