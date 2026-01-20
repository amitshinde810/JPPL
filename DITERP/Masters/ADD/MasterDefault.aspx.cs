using System;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_MasterDefault : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
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
 
                string item = "";
                DataTable dtitem = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 11 + "");
                item = dtitem.Rows.Count == 0 ? "0000000" : dtitem.Rows[0][0].ToString();
                if (item == "0000000" || item == "0111111")
                {
                    Item.Visible = false;
                }
                string itemcat = "";
                DataTable dtitemcat = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 9 + "");
                itemcat = dtitemcat.Rows.Count == 0 ? "0000000" : dtitemcat.Rows[0][0].ToString();
                if (itemcat == "0000000" || itemcat == "0111111")
                {
                    ItemCategory.Visible = false;
                }
                string tally = "";
                DataTable dttally = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 32 + "");
                tally = dttally.Rows.Count == 0 ? "0000000" : dttally.Rows[0][0].ToString();
                if (tally == "0000000" || tally == "0111111")
                {
                    Tally.Visible = false;
                }
                string excise = "";
                DataTable dtexcise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 15 + "");
                excise = dtexcise.Rows.Count == 0 ? "0000000" : dtexcise.Rows[0][0].ToString();
                if (excise == "0000000" || excise == "0111111")
                {
                    Excise.Visible = false;
                    HSNSAC.Visible = false;
                }
                string saletax = "";
                DataTable dtsaletax = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 16 + "");
                saletax = dtsaletax.Rows.Count == 0 ? "0000000" : dtsaletax.Rows[0][0].ToString();
                if (saletax == "0000000" || saletax == "0111111")
                {
                    SalesTax.Visible = false;
                }
                string BOM = "";
                DataTable dtBOM = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 44 + "");
                BOM = dtBOM.Rows.Count == 0 ? "0000000" : dtBOM.Rows[0][0].ToString();
                if (BOM == "0000000" || BOM == "0111111")
                {
                    BOMMASTER.Visible = false;
                }
                string BOMRej = "";
                DataTable dtBOMrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 45 + "");
                BOMRej = dtBOMrej.Rows.Count == 0 ? "0000000" : dtBOMrej.Rows[0][0].ToString();
                if (BOMRej == "0000000" || BOMRej == "0111111")
                {
                    BOMReg.Visible = false;
                }
                string MASTER = "";
                DataTable dtMASTER = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 117 + "");
                MASTER = dtMASTER.Rows.Count == 0 ? "0000000" : dtMASTER.Rows[0][0].ToString();
                if (MASTER == "0000000" || MASTER == "0111111")
                {
                    MasterRep.Visible = false;
                }
                string TMASTER = "";
                DataTable dtTMASTER = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 160 + "");
                TMASTER = dtTMASTER.Rows.Count == 0 ? "0000000" : dtTMASTER.Rows[0][0].ToString();
                if (TMASTER == "0000000" || TMASTER == "0111111")
                {
                    ItemTranMaster.Visible = false;
                }
                #endregion
            }
        }
    }

    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }

    protected void btnTallyMaster_click(object sender, EventArgs e)
    {
        checkRights(32);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewTallyMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCountryMaster_click(object sender, EventArgs e)
    {
        checkRights(5);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCountryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStateMaster_click(object sender, EventArgs e)
    {
        checkRights(6);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCityMaster_click(object sender, EventArgs e)
    {
        checkRights(7);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCityMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCurrencyMaster_click(object sender, EventArgs e)
    {
        checkRights(8);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCurrancyMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnItemCategory_click(object sender, EventArgs e)
    {
        checkRights(9);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnUnitMaster_click(object sender, EventArgs e)
    {
        checkRights(10);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnItemSubCategoryMaster_click(object sender, EventArgs e)
    {
        checkRights(34);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSubCategoryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnAreaMaster_click(object sender, EventArgs e)
    {
        checkRights(13);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewAreaMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnExciseTariffDetails_click(object sender, EventArgs e)
    {
        checkRights(15);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewExciseTariffDetails.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnGST_click(object sender, EventArgs e)
    {
        checkRights(15);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewGST_HSNSAC_Master.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBOMMaster_click(object sender, EventArgs e)
    {
        checkRights(44);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBOMRegister_click(object sender, EventArgs e)
    {
        checkRights(45);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/RoportForms/VIEW/ViewBillOfMaterialRegister.aspx?c_name=" + type + "", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMasterReport_click(object sender, EventArgs e)
    {
        checkRights(117);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/RoportForms/ADD/AllMasterReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnItemMaster_click(object sender, EventArgs e)
    {
        checkRights(11);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSalesTaxMaster_click(object sender, EventArgs e)
    {
        checkRights(16);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void StoreMaster_click(object sender, EventArgs e)
    {
        checkRights(147);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/StoreMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void ItemTranMaster_click(object sender, EventArgs e)
    {
        checkRights(160);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewItemTransferMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Default", "btnOk_Click", Ex.Message);
        }
    }
}
