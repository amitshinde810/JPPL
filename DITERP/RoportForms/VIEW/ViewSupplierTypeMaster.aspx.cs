using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class RoportForms_VIEW_ViewSupplierTypeMaster : System.Web.UI.Page
{
    string SupplierName1="";
    string SupplierCode1="" ;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // LoadCombos();
            SupplierName();
            SupplierCode();
            ddlSupplierName.Enabled = false;
            chkAll.Checked = true;
            ddlSupplierCode.Enabled = false;
            chkAllSupplier.Checked = true;
        }


    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
       


        try
        {
            //DateTime dt = new DateTime();
            //dt = Convert.ToDateTime(txtMonth.Text);

            //Session["Date"] = dt;
            if (chkAll.Checked == true && chkAllSupplier.Checked == true)
            {
                Response.Redirect("~/RoportForms/ADD/SupplierTypeMaster.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlSupplierName.SelectedValue.ToString() + "&SupplierAll_code=" + chkAllSupplier.Checked.ToString() + "", false);
            }
            if (chkAll.Checked == false && chkAllSupplier.Checked == true)
            {
                Response.Redirect("~/RoportForms/ADD/SupplierTypeMaster.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlSupplierName.SelectedValue.ToString() + "&SupplierAll_code=" + chkAllSupplier.Checked.ToString() + "", false);

            }
            if (chkAll.Checked == true && chkAllSupplier.Checked == false)
            {
                Response.Redirect("~/RoportForms/ADD/SupplierTypeMaster.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlSupplierCode.SelectedValue.ToString() + "&SupplierAll_code=" + chkAllSupplier.Checked.ToString() + "", false);

            }
            if (chkAll.Checked == false && chkAllSupplier.Checked == false)
            {
                if (ddlSupplierName.SelectedIndex != 0 && ddlSupplierCode.SelectedIndex != 0)
                {

                    Response.Redirect("~/RoportForms/ADD/SupplierTypeMaster.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlSupplierName.SelectedValue.ToString() + "&SupplierAll_code=" + chkAllSupplier.Checked.ToString() + "", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select Sector  ", CommonClasses.MSG_Warning);
                    return;
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master Register", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Supplier Master", "btnCancel_Click", Ex.Message);
        }
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
            CommonClasses.SendError("User Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region SupplierName
    private void SupplierName()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 and P_TYPE=2 and P_SCT_CODE="+SupplierCode1+" ORDER BY P_NAME");

            ddlSupplierName.DataSource = dt;
            ddlSupplierName.DataTextField = "P_NAME";
            ddlSupplierName.DataValueField = "P_CODE";
            ddlSupplierName.DataBind();
            ddlSupplierName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));

          
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "LoadCombos", Ex.Message);
        }

    }
    #endregion



    private void SupplierCode()
    {
        try
        {

            DataTable dt1 = CommonClasses.Execute("select SCT_CODE,SCT_DESC FROM SECTOR_MASTER WHERE SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ");

            ddlSupplierCode.DataSource = dt1;
            ddlSupplierCode.DataTextField = "SCT_DESC";
            ddlSupplierCode.DataValueField = "SCT_CODE";
            ddlSupplierCode.DataBind();
            ddlSupplierCode.Items.Insert(0, new ListItem("Sector", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "LoadCombos", Ex.Message);
        }

    }



    protected void chkAllSupplier_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllSupplier.Checked == true)
        {
            ddlSupplierCode.SelectedIndex = 0;
            ddlSupplierCode.Enabled = false;
        }
        else
        {
            ddlSupplierCode.SelectedIndex = 0;
            ddlSupplierCode.Enabled = true;
            ddlSupplierCode.Focus();
        }

    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked == true)
        {
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = false;
        }
        else
        {
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = true;
            ddlSupplierName.Focus();
        }
    }
    protected void ddlSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (chkAllSupplier.Checked != true && chkAll.Checked != true)
            //{
            //    if (ddlSupplierName.SelectedIndex != 0)
            //    {
            //        DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME,P_STM_CODE FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 and P_CODE='" + ddlSupplierName.SelectedValue.ToString() + "'  and P_TYPE=2 ORDER BY P_NAME");

            //        ddlSupplierName.SelectedValue = dt.Rows[0]["P_STM_CODE"].ToString();
            //    }
            //    else
            //    {
            //        ShowMessage("#Avisos", "Please Select Customer Name", CommonClasses.MSG_Warning);
            //        return;
            //    }
            //}
            SupplierName1 = "";
             SupplierName1 = ddlSupplierName.SelectedValue.ToString();
             //SupplierCode();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
        }
    }
    protected void ddlSupplierCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (chkAllSupplier.Checked != true && chkAll.Checked != true)
            //{
            //    DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 and P_SCT_CODE='" + ddlSupplierCode.SelectedValue.ToString() + "' and P_TYPE=2 ORDER BY P_NAME");

            //    ddlSupplierName.DataSource = dt;
            //    ddlSupplierName.DataTextField = "P_NAME";
            //    ddlSupplierName.DataValueField = "P_CODE";
            //    ddlSupplierName.DataBind();
            //    ddlSupplierName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));

            //}
            SupplierCode1 = "";
            SupplierCode1 = ddlSupplierCode.SelectedValue.ToString();
            SupplierName();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "ddlSector_SelectedIndexChanged", Ex.Message);

        }
    }
}
