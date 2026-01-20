using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Add_RunMe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            Response.Redirect("~/E_Administration/Masters/ADD/Dashboard.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bank Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtQuery.Text == "")
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
            CommonClasses.SendError("Paid Holidays", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        gd.Visible = false;
        gdDiv.Visible = false;
        if (txtQuery.Text.Length > 0 && txtQuery.Text.Trim().Length > 0)
        {
            if (CommonClasses.Execute1(txtQuery.Text.Trim()))
            {
                if (txtQuery.Text.ToUpper().Trim().StartsWith("SELECT"))
                {
                    DataTable dt = new DataTable();
                    dt = CommonClasses.Execute(txtQuery.Text.Trim());
                    if (dt.Rows.Count > 0)
                    {
                        gd.DataSource = dt;
                        gd.DataBind();
                        gd.Visible = true;
                        gdDiv.Visible = true;
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        PanelMsg.Visible = true;
                        lblmsg.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "displayralert", "Showalert1();", true);
                    }
                }
                else
                {
                    lblmsg.Visible = true;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Executed Successfully";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "displayralert", "Showalert1();", true);
                }
            }
            else
            {
                lblmsg.Visible = true;
                PanelMsg.Visible = true;
                lblmsg.Text = "UnSuccessfully";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "displayralert", "Showalert1();", true);
            }
        }
        else
        {

            lblmsg.Visible = true;
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Query";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "displayralert", "Showalert1();", true);
        }
        txtQuery.Focus();

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PT Setting", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
