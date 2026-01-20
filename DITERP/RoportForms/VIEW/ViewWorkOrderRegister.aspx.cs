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

public partial class RoportForms_VIEW_ViewWorkOrderRegister : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
        }

    }
    
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
            CommonClasses.SendError("Work Order Register", "ShowMessage", Ex.Message);
            return false;
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
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                if (chkDateAll.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/WorkOrderRegister.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "", false);
                }
                else
                {
                    Response.Redirect("~/RoportForms/ADD/WorkOrderRegister.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "", false);
               
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Work Order Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion


}
