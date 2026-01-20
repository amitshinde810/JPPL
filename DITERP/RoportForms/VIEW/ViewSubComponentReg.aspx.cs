using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewSubComponentReg : System.Web.UI.Page
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
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='28'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadComponent();
            ddlSubComponent.Enabled = false;
            chkAll.Checked = true;
            ddlSCCodeNo.Enabled = false;
            ChkCodeNoAll.Checked = true;
        }
    }

    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO +' - '+I_NAME AS I_CODENO FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND I_CAT_CODE=-2147483648 and ES_DELETE=0  ORDER BY I_CODENO");

            ddlSCCodeNo.DataSource = dt;
            ddlSCCodeNo.DataTextField = "I_CODENO";
            ddlSCCodeNo.DataValueField = "I_CODE";
            ddlSCCodeNo.DataBind();
            ddlSCCodeNo.Items.Insert(0, new ListItem("Product Name", "0"));

            //ddlSCCodeNo.DataSource = dt;
            //ddlSCCodeNo.DataTextField = "I_CODENO";
            //ddlSCCodeNo.DataValueField = "I_CODE";
            //ddlSCCodeNo.DataBind();
            //ddlSCCodeNo.Items.Insert(0, new ListItem("Product Code ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Component Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnCancel_Click
    private void CancelRecord()
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Raw Material Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            flag = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currency Master", "CheckValid", Ex.Message);
        }
        return flag;
    }

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
            CommonClasses.SendError("Sub Component Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAll_CheckedChanged
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked == true)
        {
            ddlSubComponent.SelectedIndex = 0;
            ddlSubComponent.Enabled = false;
        }
        else
        {
            ddlSubComponent.SelectedIndex = 0;
            ddlSubComponent.Enabled = true;
            ddlSubComponent.Focus();
        }
    }
    #endregion

    #region ChkCodeNoAll_CheckedChanged
    protected void ChkCodeNoAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkCodeNoAll.Checked == true)
        {
            ddlSCCodeNo.SelectedIndex = 0;
            ddlSCCodeNo.Enabled = false;
        }
        else
        {
            ddlSCCodeNo.SelectedIndex = 0;
            ddlSCCodeNo.Enabled = true;
            ddlSCCodeNo.Focus();
        }
    }
    #endregion

    #region ddlSCCodeNo_SelectedIndexChanged
    protected void ddlSCCodeNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkCodeNoAll.Checked != true && chkAll.Checked != true)
            {
                if (ddlSCCodeNo.SelectedIndex != 0)
                {
                    ddlSubComponent.SelectedValue = ddlSCCodeNo.SelectedValue;
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select Component Code", CommonClasses.MSG_Warning);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Component Register", "ddlSCCodeNo_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlSubComponent_SelectedIndexChanged
    protected void ddlSubComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkCodeNoAll.Checked != true && chkAll.Checked != true)
            {
                if (ddlSubComponent.SelectedIndex != 0)
                {
                    ddlSCCodeNo.SelectedValue = ddlSubComponent.SelectedValue;
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select Component Name", CommonClasses.MSG_Warning);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Component Register", "ddlSubComponent_SelectedIndexChanged", Ex.Message);
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
                if (ChkCodeNoAll.Checked == false)
                {
                    if (ddlSCCodeNo.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select'Product Code'";
                        return;
                    }
                }
                if (chkAll.Checked == false)
                {
                    if (ddlSubComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select'Product Name'";
                        return;
                    }
                }
                if (chkAll.Checked == true && ChkCodeNoAll.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/SubComponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&comp_code=" + ddlSubComponent.SelectedValue.ToString() + "&CodenoAll_code=" + ChkCodeNoAll.Checked.ToString() + "", false);
                }
                if (chkAll.Checked == false && ChkCodeNoAll.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/SubComponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&comp_code=" + ddlSubComponent.SelectedValue.ToString() + "&CodenoAll_code=" + ChkCodeNoAll.Checked.ToString() + "", false);
                }
                if (chkAll.Checked == true && ChkCodeNoAll.Checked == false)
                {
                    Response.Redirect("~/RoportForms/ADD/SubComponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&comp_code=" + ddlSCCodeNo.SelectedValue.ToString() + "&CodenoAll_code=" + ChkCodeNoAll.Checked.ToString() + "", false);
                }
                if (chkAll.Checked == false && ChkCodeNoAll.Checked == false)
                {
                    if (ddlSubComponent.SelectedIndex != 0 && ddlSCCodeNo.SelectedIndex != 0)
                    {
                        Response.Redirect("~/RoportForms/ADD/SubComponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&comp_code=" + ddlSCCodeNo.SelectedValue.ToString() + "&CodenoAll_code=" + ChkCodeNoAll.Checked.ToString() + "", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Please Select Component", CommonClasses.MSG_Warning);
                        return;
                    }
                }
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../VIEW/ViewMasterReport.aspx'; </script>");

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Finished Component Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
