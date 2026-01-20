using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Masters_ADD_ToolsDefault : System.Web.UI.Page
{
    #region Variable
    string right = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
        home1.Attributes["class"] = "active";
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                string ToolingRoom = "";
                DataTable dtToolingRoom = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 167 + "");
                ToolingRoom = dtToolingRoom.Rows.Count == 0 ? "0000000" : dtToolingRoom.Rows[0][0].ToString();
                if (ToolingRoom == "0000000" || ToolingRoom == "0111111")
                {
                    Toolmstr.Visible = false;
                }

                string MonthlyPlanPM = "";
                DataTable dtMonthlyPlanPM = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 169 + "");
                MonthlyPlanPM = dtMonthlyPlanPM.Rows.Count == 0 ? "0000000" : dtMonthlyPlanPM.Rows[0][0].ToString();
                if (MonthlyPlanPM == "0000000" || MonthlyPlanPM == "0111111")
                {
                    MonthlyPlan.Visible = false;
                }

                string WeeklyPlanPM = "";
                DataTable dtWeeklyPlanPM = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 170 + "");
                WeeklyPlanPM = dtWeeklyPlanPM.Rows.Count == 0 ? "0000000" : dtWeeklyPlanPM.Rows[0][0].ToString();
                if (WeeklyPlanPM == "0000000" || WeeklyPlanPM == "0111111")
                {
                    WeeklyPlan.Visible = false;
                }

                string BreakDownR = "";
                DataTable dtBreakDownR = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 196 + "");
                BreakDownR = dtBreakDownR.Rows.Count == 0 ? "0000000" : dtBreakDownR.Rows[0][0].ToString();
                if (BreakDownR == "0000000" || BreakDownR == "0111111")
                {
                    BreakDownReg.Visible = false;
                }

                string ImpromentR = "";
                DataTable dtImpromentR = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 197 + "");
                ImpromentR = dtImpromentR.Rows.Count == 0 ? "0000000" : dtImpromentR.Rows[0][0].ToString();
                if (ImpromentR == "0000000" || ImpromentR == "0111111")
                {
                    ImpromentReg.Visible = false;
                }

                string ThreeDModel = "";
                DataTable dtThreeDModel = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 198 + "");
                ThreeDModel = dtThreeDModel.Rows.Count == 0 ? "0000000" : dtThreeDModel.Rows[0][0].ToString();
                if (ThreeDModel == "0000000" || ThreeDModel == "0111111")
                {
                    TDModel.Visible = false;
                }

                string ToolingLife = "";
                DataTable dtToolingLife = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 174 + "");
                ToolingLife = dtToolingLife.Rows.Count == 0 ? "0000000" : dtToolingLife.Rows[0][0].ToString();
                if (ToolingLife == "0000000" || ToolingLife == "0111111")
                {
                    ToolLife.Visible = false;
                }
                string PreMTrigger = "";
                DataTable dtPreMTrigger = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 200 + "");
                PreMTrigger = dtPreMTrigger.Rows.Count == 0 ? "0000000" : dtPreMTrigger.Rows[0][0].ToString();
                if (PreMTrigger == "0000000" || PreMTrigger == "0111111")
                {
                    PMTrigger.Visible = false;
                }

                string PeMPerformance = "";
                DataTable dtPeMPerformance = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 201 + "");
                PeMPerformance = dtPeMPerformance.Rows.Count == 0 ? "0000000" : dtPeMPerformance.Rows[0][0].ToString();
                if (PeMPerformance == "0000000" || PeMPerformance == "0111111")
                {
                    PMPerformance.Visible = false;
                }

                string BrekDwnChart = "";
                DataTable dtBrekDwnChart = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 171 + "");
                BrekDwnChart = dtBrekDwnChart.Rows.Count == 0 ? "0000000" : dtBrekDwnChart.Rows[0][0].ToString();
                if (BrekDwnChart == "0000000" || BrekDwnChart == "0111111")
                {
                    BrkDwnChart.Visible = false;
                }

                string ToolHistoryCards = "";
                DataTable dtToolHistoryCards = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 178 + "");
                ToolHistoryCards = dtToolHistoryCards.Rows.Count == 0 ? "0000000" : dtToolHistoryCards.Rows[0][0].ToString();
                if (ToolHistoryCards == "0000000" || ToolHistoryCards == "0111111")
                {
                    ToolHistoryCard.Visible = false;
                }

                string toolMstrrpt = "";
                DataTable dttoolMstrrpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 199 + "");
                toolMstrrpt = dttoolMstrrpt.Rows.Count == 0 ? "0000000" : dttoolMstrrpt.Rows[0][0].ToString();
                if (toolMstrrpt == "0000000" || toolMstrrpt == "0111111")
                {
                    toolingMstrrpt.Visible = false;
                }

                string ImpRegs = "";
                DataTable dtImpRegs = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 172 + "");
                ImpRegs = dtImpRegs.Rows.Count == 0 ? "0000000" : dtImpRegs.Rows[0][0].ToString();
                if (ImpRegs == "0000000" || ImpRegs == "0111111")
                {
                    ImpReg.Visible = false;
                }

                string PMPendings = "";
                DataTable dtPMPendings = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 175 + "");
                PMPendings = dtPMPendings.Rows.Count == 0 ? "0000000" : dtPMPendings.Rows[0][0].ToString();
                if (PMPendings == "0000000" || PMPendings == "0111111")
                {
                    PMPending.Visible = false;
                }

                string TRRefurbishs = "";
                DataTable dtTRRefurbishs = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 176 + "");
                TRRefurbishs = dtTRRefurbishs.Rows.Count == 0 ? "0000000" : dtTRRefurbishs.Rows[0][0].ToString();
                if (TRRefurbishs == "0000000" || TRRefurbishs == "0111111")
                {
                    TRRefurbish.Visible = false;
                }

                string PMPerforms = "";
                DataTable dtPMPerforms = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 177 + "");
                PMPerforms = dtPMPerforms.Rows.Count == 0 ? "0000000" : dtPMPerforms.Rows[0][0].ToString();
                if (PMPerforms == "0000000" || PMPerforms == "0111111")
                {
                    PMPerform.Visible = false;
                }

                string BreakdownCharts = "";
                DataTable dtBreakdownCharts = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 177 + "");
                BreakdownCharts = dtBreakdownCharts.Rows.Count == 0 ? "0000000" : dtBreakdownCharts.Rows[0][0].ToString();
                if (BreakdownCharts == "0000000" || BreakdownCharts == "0111111")
                {
                    BreakdownChart.Visible = false;
                }
            }
        }
    }

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region Master
    protected void btnToolingMstr_click(object sender, EventArgs e)
    {
        checkRights(167);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewTools.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    protected void btntoolMasterrpt_click(object sender, EventArgs e)
    {
        checkRights(199);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewToolMasterReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMonthlyPlan_click(object sender, EventArgs e)
    {
        checkRights(169);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/MonthlyPlannedPreventiveMaintenanceDIE.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnWeeklyPlan_click(object sender, EventArgs e)
    {
        checkRights(170);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/WEEKLYPREVMAINDIE.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBrkEntry_click(object sender, EventArgs e)
    {
        checkRights(196);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewBreakdown.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnInproentry_click(object sender, EventArgs e)
    {
        checkRights(197);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewImprovement.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btn3DModelnRevision_click(object sender, EventArgs e)
    {
        checkRights(198);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/TDModelStorageNReqForRevision.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btntoolLifeMonitering_click(object sender, EventArgs e)
    {
        checkRights(174);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewToolLifeMoniteringSheetReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPreMaintenanceTrigger_click(object sender, EventArgs e)
    {
        checkRights(200);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/MonthlyPMPlanReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnWeeklyPremenPerformance_click(object sender, EventArgs e)
    {
        checkRights(201);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewWeeklyPM.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBreakOccChart_click(object sender, EventArgs e)
    {
        checkRights(171);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/viewBreakdownRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btntoolHistoryCard_click(object sender, EventArgs e)
    {
        checkRights(178);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/AddToolHistoryCardReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnImproReg_click(object sender, EventArgs e)
    {
        checkRights(172);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewImprovementRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btntoolForPM_click(object sender, EventArgs e)
    {
        //checkRights(172);
        //  if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        // {
        Response.Redirect("~/ToolRoom/ADD/ToolsForMaintaince.aspx", false);
        // }
        //  else
        //{
        ModalPopupMsg.Show();
        return;
        //}
    }

    protected void btnPMPending_click(object sender, EventArgs e)
    {
        checkRights(175);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewPMFailuerReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTRRefurbish_click(object sender, EventArgs e)
    {
        checkRights(176);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewToolRRefurbish.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPMPerform_click(object sender, EventArgs e)
    {
        checkRights(177);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewPrevmainPerformanceReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBreakdownChart_click(object sender, EventArgs e)
    {
        checkRights(177);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewBreakdownOccuChartReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion
}
