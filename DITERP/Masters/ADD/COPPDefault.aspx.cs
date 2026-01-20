using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

public partial class Masters_ADD_COPPDefault : System.Web.UI.Page
{
    string right = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPP");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPPMV");
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
               
                string AnalysisProd = "";
                DataTable dtLiAnalysisProd = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE= '" + 156 + "'");
                AnalysisProd = dtLiAnalysisProd.Rows.Count == 0 ? "0000000" : dtLiAnalysisProd.Rows[0][0].ToString();
                if (AnalysisProd == "0000000" || AnalysisProd == "0111111")
                {
                    LiAnalysisProd.Visible = false;
                }

                string PerformanceSumReport = "";
                DataTable dtLiPerformanceSumReport = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE= '" + 157 + "'");
                PerformanceSumReport = dtLiPerformanceSumReport.Rows.Count == 0 ? "0000000" : dtLiPerformanceSumReport.Rows[0][0].ToString();
                if (PerformanceSumReport == "0000000" || PerformanceSumReport == "0111111")
                {
                    LiPerformanceSumReport.Visible = false;
                }

                string Reasonrpt = "";
                DataTable dtReasonrpt = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE= '" + 158 + "'");
                Reasonrpt = dtReasonrpt.Rows.Count == 0 ? "0000000" : dtReasonrpt.Rows[0][0].ToString();
                if (Reasonrpt == "0000000" || Reasonrpt == "0111111")
                {
                    ReasonReport.Visible = false;
                }

                string ItemStndWgtMs = "";
                DataTable dtItemStndWgtMs = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE= '" + 165 + "'");
                ItemStndWgtMs = dtItemStndWgtMs.Rows.Count == 0 ? "0000000" : dtItemStndWgtMs.Rows[0][0].ToString();
                if (ItemStndWgtMs == "0000000" || ItemStndWgtMs == "0111111")
                {
                    ItemStndWgtM.Visible = false;
                }
                #endregion
            }
        }
    }
    #endregion

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #region btnStandWeightmaster_click
    protected void btnStandWeightmaster_click(object sender, EventArgs e)
    {
        checkRights(165);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
           // Response.Redirect("~/Masters/VIEW/ItemStandWgtMaster.aspx", false);
            Response.Redirect("~/Masters/VIEW/ItemStandWgtMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnRejSummStage_click

    #region btnReasonReport_click
    protected void btnReasonReport_click(object sender, EventArgs e)
    {
        checkRights(158);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewReasonReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnRejSummStage_click

    #region btnAnalysisOfShortProdReport_click
    protected void btnAnalysisOfShortProdReport_click(object sender, EventArgs e)
    {
        checkRights(156);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewAnalysisOfShortProductionReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnAnalysisOfShortProdReport_click

    #region btnPerformanceSumReport_click
    protected void btnPerformanceSumReport_click(object sender, EventArgs e)
    {
        checkRights(157);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewPerformanceSummaryReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnPerformanceSumReport_click

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/COPPDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion
}
