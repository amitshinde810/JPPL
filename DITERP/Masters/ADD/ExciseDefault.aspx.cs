using System.Web.UI.HtmlControls;
using System;
using System.Data;

public partial class Masters_ADD_ExciseDefault : System.Web.UI.Page
{
    #region Variable
    string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
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
                string Excise = "";
                DataTable dtExcise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 105 + "");
                Excise = dtExcise.Rows.Count == 0 ? "0000000" : dtExcise.Rows[0][0].ToString();
                if (Excise == "0000000" || Excise == "0111111")
                {
                    excise.Visible = false;
                }

                string ER1 = "";
                DataTable dtER1 = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 108 + "");
                ER1 = dtER1.Rows.Count == 0 ? "0000000" : dtER1.Rows[0][0].ToString();
                if (ER1 == "0000000" || ER1 == "0111111")
                {
                    ER1Report.Visible = false;
                }

                string Annexure4 = "";
                DataTable dtAnnexure4 = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 110 + "");
                Annexure4 = dtAnnexure4.Rows.Count == 0 ? "0000000" : dtAnnexure4.Rows[0][0].ToString();
                if (Annexure4 == "0000000" || Annexure4 == "0111111")
                {
                    Annexure4Report.Visible = false;
                }
                string Annexure5 = "";
                DataTable dtAnnexure5 = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 111 + "");
                Annexure5 = dtAnnexure5.Rows.Count == 0 ? "0000000" : dtExcise.Rows[0][0].ToString();
                if (Annexure5 == "0000000" || Annexure5 == "0111111")
                {
                    Annexure5Report.Visible = false;
                }

                string ExciseReg = "";
                DataTable dtExciseReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 107 + "");
                ExciseReg = dtExciseReg.Rows.Count == 0 ? "0000000" : dtExciseReg.Rows[0][0].ToString();
                if (ExciseReg == "0000000" || ExciseReg == "0111111")
                {
                    ExciseRegister.Visible = false;
                }
                string Daily = "";
                DataTable dtDaily = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 109 + "");
                Daily = dtDaily.Rows.Count == 0 ? "0000000" : dtDaily.Rows[0][0].ToString();
                if (Daily == "0000000" || Daily == "0111111")
                {
                    DailyRegister.Visible = false;
                }


                string ITC = "";
                DataTable dtITC = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 168 + "");
                ITC = dtITC.Rows.Count == 0 ? "0000000" : dtITC.Rows[0][0].ToString();
                if (ITC == "0000000" || ITC == "0111111")
                {
                    Li4.Visible = false;
                }
                #endregion
            }
        }
    }
    #endregion

    #region Transaction
    protected void btnMaterialRequisition_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region Report
    protected void btnExciseRegister_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/VIewExciseRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDailyRegister_click(object sender, EventArgs e)
    {
        checkRights(109);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/RCMInwardReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnRCMInwardReport_click(object sender, EventArgs e)
    {
        checkRights(109);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewDailyStockAccDetail.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnAnnexureIV_click(object sender, EventArgs e)
    {
        checkRights(109);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewAnnexureIVGST.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnITC_click(object sender, EventArgs e)
    {
        checkRights(168);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewGST_ITC_04.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnAnnexureV_click(object sender, EventArgs e)
    {
        checkRights(109);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewAnnexureVGST.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region Methods

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ExciseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion
}