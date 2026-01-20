using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class RoportForms_VIEW_ViewBillOfComponentRegister : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='18'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
        
            LoadComponent();
            ddlFinishedComponent.Enabled = false;
            chkAll.Checked = true;
        }

    }
    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE='-2147483647' ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dt;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Finished Component", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component Register", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

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
            CommonClasses.SendError("Bill Of Component Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAll_CheckedChanged
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
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
                //DateTime dt = new DateTime();
                //dt = Convert.ToDateTime(txtMonth.Text);

                //Session["Date"] = dt;
                if (chkAll.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/BillOfcomponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "", false);
                }
                else
                {
                    Response.Redirect("~/RoportForms/ADD/BillOfcomponentRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&comp_code=" + ddlFinishedComponent.SelectedValue.ToString() + "", false);

                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
