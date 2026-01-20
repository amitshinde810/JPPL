using System;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_UtilityDefault : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                #region Hiding Menus As Per Rights
                string tallysales = "";
                DataTable dttallysales = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 66 + "");
                tallysales = dttallysales.Rows.Count == 0 ? "0000000" : dttallysales.Rows[0][0].ToString();
                if (tallysales == "0000000" || tallysales == "0111111")
                {
                    TallySales.Visible = false;
                }

                string tallypurchase = "";
                DataTable dttallypurchase = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 67 + "");
                tallypurchase = dttallypurchase.Rows.Count == 0 ? "0000000" : dttallypurchase.Rows[0][0].ToString();
                if (tallypurchase == "0000000" || tallypurchase == "0111111")
                {
                    TallyPurchase.Visible = false;
                }

                string itemMaster = "";
                DataTable dtItemMaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 113 + "");
                itemMaster = dtItemMaster.Rows.Count == 0 ? "0000000" : dtItemMaster.Rows[0][0].ToString();
                if (itemMaster == "0000000" || itemMaster == "0111111")
                {
                    ItemMaster.Visible = false;
                }

                string supp = "";
                DataTable dtsupp = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 159 + "");
                supp = dtsupp.Rows.Count == 0 ? "0000000" : dtsupp.Rows[0][0].ToString();
                if (supp == "0000000" || supp == "0111111")
                {
                    suppUpload.Visible = false;
                }

                string vcn = "";
                DataTable dtvcn = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 19 + "");
                vcn = dtvcn.Rows.Count == 0 ? "0000000" : dtvcn.Rows[0][0].ToString();
                if (vcn == "0000000" || vcn == "0111111")
                {
                    vcnUtility.Visible = false;
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




    #region EInvoice
    protected void EInvoice_click(object sender, EventArgs e)
    {
        checkRights(19);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            // Hiten 
            // Direct Shows Tally Transfer Screen
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/EInvoice.aspx?c_name=" + type, false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnTallySales_click
    protected void btnTallySales_click(object sender, EventArgs e)
    {
        checkRights(66);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            // Hiten 
            // Direct Shows Tally Transfer Screen
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/TallyTransSales .aspx?c_name=" + type, false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnTallyPurchase_click
    protected void btnTallyPurchase_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Utility/ADD/TallyTransferPurchase.aspx?c_name=INSERT", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnExciseReg_click
    protected void btnExciseReg_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../RoportForms/VIEW/VIewExciseRegister.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnStockAccRpt_click
    protected void btnStockAccRpt_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../RoportForms/VIEW/ViewDailyStockAccDetail.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnVCM_click
    protected void btnVCM_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Utility/ADD/CreateVCM.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnECE_click
    protected void btnECE_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnsuppUpload_click
    protected void btnsuppUpload_click(object sender, EventArgs e)
    {
        checkRights(159);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Utility/ADD/SuppliementoryInvoice.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Utility Default", "btnOk_Click", Ex.Message);
        }
    }

    protected void btnItemAdd_click(object sender, EventArgs e)
    {
        try
        {
            checkRights(113);
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                Response.Redirect("~/Utility/ADD/ItemAddtion.aspx", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');</script>");
                ModalPopupMsg.Show();
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Utility Default", "btnItemAdd_click", Ex.Message);
        }
    }
}
