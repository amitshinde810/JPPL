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


public partial class RoportForms_VIEW_ViewRNDMasterReport : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='22'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

        }

    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        //}
        //catch (Exception)
        //{

        //}
        try
        {
            if (CheckValid())
            {
                popUpPanel5.Visible = true;
                //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                ModalPopupPrintSelection.Show();

            }
            else
            {
                CancelRecord();

            }


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("R & D Master Report", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Masters/ADD/RNDDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("R & D Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ShowRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlMasterForms.SelectedIndex == 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("R & D Master Report", "CheckValid", Ex.Message);
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
            CommonClasses.SendError("R & D Master Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }


    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
            {
                ShowRecord();               
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../VIEW/ViewMasterReport.aspx'; </script>");

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("R & D Master Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion


    private void ShowRecord()
    {
        string master_index = "";

        if (ddlMasterForms.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Master Form", CommonClasses.MSG_Warning);

        }
        else
        {

            if (ddlMasterForms.SelectedIndex == 1)
            {
                checkRights(102);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {
                    master_index = "5";
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            }
            if (ddlMasterForms.SelectedIndex == 2)
            {
                checkRights(103);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {
                    master_index = "6";
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            } 
        }
        if (master_index != "")
        {
            Response.Redirect("~/RoportForms/ADD/MasterRegister.aspx?Title=" + Title + "&master_index=" + master_index + "", false);
        }
    }

}
