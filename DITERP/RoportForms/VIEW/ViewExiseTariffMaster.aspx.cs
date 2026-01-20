using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class RoportForms_VIEW_ViewExiseTariffMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadComponent();
            // ddlSubComponent.Enabled = false;
            // chkAll.Checked = true;
            ddlExiseTariffNo.Enabled = false;
            ChkExiseTariffNoAll.Checked = true;
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {

            if (ChkExiseTariffNoAll.Checked == true)
            {
                Response.Redirect("~/RoportForms/ADD/ExiseTariffMaster.aspx?Title=" + Title + "&ChkUserNameAll=" + ChkExiseTariffNoAll.Checked.ToString() + "&User_Name=" + ddlExiseTariffNo.SelectedValue.ToString() + "", false);
            }

            if (ChkExiseTariffNoAll.Checked == false)
            {
                if (ddlExiseTariffNo.SelectedIndex != 0)
                {

                    Response.Redirect("~/RoportForms/ADD/ExiseTariffMaster.aspx?Title=" + Title + "&ChkUserNameAll=" + ChkExiseTariffNoAll.Checked.ToString() + "&User_Name=" + ddlExiseTariffNo.SelectedValue.ToString() + "", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select Exise Tariff No ", CommonClasses.MSG_Warning);
                    return;
                }



            }

            else
            {
                ShowMessage("#Avisos", "Please Select Area Code", CommonClasses.MSG_Warning);
                return;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnShow_Click", Ex.Message);

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }
    }
    protected void ChkExiseTariffNoAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkExiseTariffNoAll.Checked == true)
        {
            ddlExiseTariffNo.SelectedIndex = 0;
            ddlExiseTariffNo.Enabled = false;
        }
        else
        {
            ddlExiseTariffNo.SelectedIndex = 0;
            ddlExiseTariffNo.Enabled = true;
            ddlExiseTariffNo.Focus();
        }
    }
   
    #region LoadComponent

    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO FROM EXCISE_TARIFF_MASTER WHERE E_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY E_TARIFF_NO");

            ddlExiseTariffNo.DataSource = dt;
            ddlExiseTariffNo.DataTextField = "E_TARIFF_NO";
            ddlExiseTariffNo.DataValueField = "E_CODE";
            ddlExiseTariffNo.DataBind();
            ddlExiseTariffNo.Items.Insert(0, new ListItem("Tariff No", "0"));




        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Area Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion



}
