using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_VendorDefault : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Control c = this.Master.FindControl("Dashboard");
        c.Visible = false;

        Control c1 = this.Master.FindControl("Masters");
        c1.Visible = false;

        Control c2 = this.Master.FindControl("Purchase");
        c2.Visible = false;

        Control c3 = this.Master.FindControl("Production");
        c3.Visible = false;

        Control c4 = this.Master.FindControl("Sale");
        c4.Visible = false;

        Control c5 = this.Master.FindControl("Excise");
        c5.Visible = false;

        Control c6 = this.Master.FindControl("Utility");
        c6.Visible = false;

        Control c7 = this.Master.FindControl("IRN");
        c7.Visible = false;

        //Mobile View Menu Hide
        Control c8 = this.Master.FindControl("Dashboard1MV");
        c8.Visible = false;

        Control c9 = this.Master.FindControl("Masters1MV");
        c9.Visible = false;

        Control c10 = this.Master.FindControl("Purchase1MV");
        c10.Visible = false;

        Control c11 = this.Master.FindControl("Production1MV");
        c11.Visible = false;

        Control c12 = this.Master.FindControl("Sale1MV");
        c12.Visible = false;

        Control c13 = this.Master.FindControl("Excise1MV");
        c13.Visible = false;

        Control c14 = this.Master.FindControl("Utility1MV");
        c14.Visible = false;

        Control c15 = this.Master.FindControl("IRN1MV");
        c15.Visible = false;

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                #region Hiding Menus As Per Rights
                string Vendor = "";
                DataTable dtVendor = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 140 + "");
                Vendor = dtVendor.Rows.Count == 0 ? "0000000" : dtVendor.Rows[0][0].ToString();
                if (Vendor == "0000000" || Vendor == "0111111")
                {
                    VendorR.Visible = false;
                }
                #endregion
            }
        }
    }

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #region btnVendorRejYrly_click
    protected void btnVendorRejYrly_click(object sender, EventArgs e)
    {
        checkRights(140);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewVendorRej.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVendorRejYrly_click

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/VendorDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion
}
