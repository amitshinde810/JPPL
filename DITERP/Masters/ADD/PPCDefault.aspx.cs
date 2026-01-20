using System.Web.UI.HtmlControls;
using System;
using System.Data;

public partial class Masters_ADD_PPCDefault : System.Web.UI.Page
{
    #region Variable
    string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            if (!IsPostBack)
            {
                //LiMonth.Visible = true; LiCapcity.Visible = true; LiWeekly.Visible = true;
                #region Hiding Menus As Per Rights

                #region Group
                string Group = "";
                DataTable dtGroup = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 180 + "");
                Group = dtGroup.Rows.Count == 0 ? "0000000" : dtGroup.Rows[0][0].ToString();
                if (Group == "0000000" || Group == "0111111")
                {
                    LiGroup.Visible = false;
                }
                #endregion

                #region Prod
                string Prod = "";
                DataTable dtProd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 181 + "");
                Prod = dtProd.Rows.Count == 0 ? "0000000" : dtProd.Rows[0][0].ToString();
                if (Prod == "0000000" || Prod == "0111111")
                {
                    LiProd.Visible = false;
                }
                #endregion

                #region StandInv
                string StandInv = "";
                DataTable dtStandInv = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 182 + "");
                StandInv = dtStandInv.Rows.Count == 0 ? "0000000" : dtStandInv.Rows[0][0].ToString();
                if (StandInv == "0000000" || StandInv == "0111111")
                {
                    LiStandInv.Visible = false;
                }
                #endregion StandInv

                #region Reason
                string Reason = "";
                DataTable dtReason = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 183 + "");
                Reason = dtStandInv.Rows.Count == 0 ? "0000000" : dtReason.Rows[0][0].ToString();
                if (Reason == "0000000" || Reason == "0111111")
                {
                    LiReason.Visible = false;
                }
                #endregion Reason

                #region Productivity
                string Productivity = "";
                DataTable dtProductivity = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 184 + "");
                Productivity = dtStandInv.Rows.Count == 0 ? "0000000" : dtProductivity.Rows[0][0].ToString();
                if (Productivity == "0000000" || Productivity == "0111111")
                {
                    LiProductivity.Visible = false;
                }
                #endregion Productivity

                #region Machine
                string Machine = "";
                DataTable dtMachine = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 185 + "");
                Machine = dtMachine.Rows.Count == 0 ? "0000000" : dtMachine.Rows[0][0].ToString();
                if (Machine == "0000000" || Machine == "0111111")
                {
                    LiMachine.Visible = false;
                }
                #endregion Machine

                #region ProcessMachine
                string ProcessMachine = "";
                DataTable dtProcessMachine = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 186 + "");
                ProcessMachine = dtProcessMachine.Rows.Count == 0 ? "0000000" : dtProcessMachine.Rows[0][0].ToString();
                if (ProcessMachine == "0000000" || ProcessMachine == "0111111")
                {
                    LiProcessMachine.Visible = false;
                }
                #endregion ProcessMachine

                #region VendorMachine
                string VendorMachine = "";
                DataTable dtVendorMachine = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 187 + "");
                VendorMachine = dtStandInv.Rows.Count == 0 ? "0000000" : dtStandInv.Rows[0][0].ToString();
                if (VendorMachine == "0000000" || VendorMachine == "0111111")
                {
                    LiVendorMachine.Visible = false;
                }
                #endregion VendorMachine

                #region Boughtout
                string Boughtout = "";
                DataTable dtBoughtout = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 188 + "");
                Boughtout = dtBoughtout.Rows.Count == 0 ? "0000000" : dtBoughtout.Rows[0][0].ToString();
                if (Boughtout == "0000000" || Boughtout == "0111111")
                {
                    LiBoughtout.Visible = false;
                }
                #endregion Boughtout

                #region ProcessBom
                string ProcessBom = "";
                DataTable dtProcessBom = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 189 + "");
                ProcessBom = dtProcessBom.Rows.Count == 0 ? "0000000" : dtProcessBom.Rows[0][0].ToString();
                if (ProcessBom == "0000000" || ProcessBom == "0111111")
                {
                    LiProcessBom.Visible = false;
                }
                #endregion ProcessBom

                #region Pallet
                string Pallet = "";
                DataTable dtPallet = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 190 + "");
                Pallet = dtPallet.Rows.Count == 0 ? "0000000" : dtPallet.Rows[0][0].ToString();
                if (Pallet == "0000000" || Pallet == "0111111")
                {
                    LiPallet.Visible = false;
                }
                #endregion Pallet

                #region Box
                string Box = "";
                DataTable dtBox = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 191 + "");
                Box = dtBox.Rows.Count == 0 ? "0000000" : dtBox.Rows[0][0].ToString();
                if (Box == "0000000" || Box == "0111111")
                {
                    LiBox.Visible = false;
                }
                #endregion Box

                #region Bom
                string Bom = "";
                DataTable dtBom = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE=" + 266 + "");
                Bom = dtBom.Rows.Count == 0 ? "0000000" : dtBom.Rows[0][0].ToString();
                if (Bom == "0000000" || Bom == "0111111")
                {
                    liBOM.Visible = false;
                }
                #endregion Box

                #region MachiningToolCost
                string MachiningToolCost = "";
                DataTable dtMachiningToolCost = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 192 + "");
                MachiningToolCost = dtMachiningToolCost.Rows.Count == 0 ? "0000000" : dtMachiningToolCost.Rows[0][0].ToString();
                if (MachiningToolCost == "0000000" || MachiningToolCost == "0111111")
                {
                    LiMachiningToolCost.Visible = false;
                }
                #endregion MachiningToolCost

                #region MachineBooking
                string MachineBooking = "";
                DataTable dtMachineBooking = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 193 + "");
                MachineBooking = dtMachineBooking.Rows.Count == 0 ? "0000000" : dtMachineBooking.Rows[0][0].ToString();
                if (MachineBooking == "0000000" || MachineBooking == "0111111")
                {
                    LiMachineBooking.Visible = false;
                }
                #endregion MachineBooking

                #region CoreInv
                string CoreInv = "";
                DataTable dtCoreInv = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 204 + "");
                CoreInv = dtCoreInv.Rows.Count == 0 ? "0000000" : dtCoreInv.Rows[0][0].ToString();
                if (CoreInv == "0000000" || CoreInv == "0111111")
                {
                    LiCoreInv.Visible = false;
                }
                #endregion CoreInv

                #region CustomerSchedule
                string CustomerSchedule = "";
                DataTable dtCustomerSchedule = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 203 + "");
                CustomerSchedule = dtCustomerSchedule.Rows.Count == 0 ? "0000000" : dtCustomerSchedule.Rows[0][0].ToString();
                if (CustomerSchedule == "0000000" || CustomerSchedule == "0111111")
                {
                    LiCustomerSchedule.Visible = false;
                }
                #endregion CustomerSchedule

                #region VendorSchedule
                string VendorSchedule = "";
                DataTable dtVendorSchedule = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 205 + "");
                VendorSchedule = dtVendorSchedule.Rows.Count == 0 ? "0000000" : dtVendorSchedule.Rows[0][0].ToString();
                if (VendorSchedule == "0000000" || VendorSchedule == "0111111")
                {
                    LiVendorSchedule.Visible = false;
                    Li2.Visible = false;
                }
                #endregion VendorSchedule

                #region CustomerWeeklyPlan
                string CustomerWeeklyPlan = "";
                DataTable dtCustomerWeeklyPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 238 + "");
                CustomerWeeklyPlan = dtCustomerWeeklyPlan.Rows.Count == 0 ? "0000000" : dtCustomerWeeklyPlan.Rows[0][0].ToString();
                if (CustomerWeeklyPlan == "0000000" || CustomerWeeklyPlan == "0111111")
                {
                    LiCustomerWeeklyPlan.Visible = false;
                }
                #endregion CustomerWeeklyPlan

                #region VendorWeeklyPlan
                string VendorWeeklyPlan = "";
                DataTable dtVendorWeeklyPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 239 + "");
                VendorWeeklyPlan = dtVendorWeeklyPlan.Rows.Count == 0 ? "0000000" : dtVendorWeeklyPlan.Rows[0][0].ToString();
                if (VendorWeeklyPlan == "0000000" || VendorWeeklyPlan == "0111111")
                {
                    LiVendorWeeklyPlan.Visible = false;
                }
                #endregion VendorWeeklyPlan

                #region DailySaleEntry
                string DailySaleEntry = "";
                DataTable dtDailySaleEntry = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 251 + "");
                DailySaleEntry = dtDailySaleEntry.Rows.Count == 0 ? "0000000" : dtDailySaleEntry.Rows[0][0].ToString();
                if (DailySaleEntry == "0000000" || DailySaleEntry == "0111111")
                {
                    LiDailySaleEntry.Visible = false;
                }
                #endregion DailySaleEntry

                #region unplanSaleEntry
                string Unplan = "";
                DataTable dtUnplan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 251 + "");
                Unplan = dtUnplan.Rows.Count == 0 ? "0000000" : dtUnplan.Rows[0][0].ToString();
                if (Unplan == "0000000" || Unplan == "0111111")
                {
                    LiUnplan.Visible = false;
                }
                #endregion DailySaleEntry

                #region ShortSaleEntry
                string ShortSaleEntry = "";
                DataTable dtShortSaleEntry = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 252 + "");
                ShortSaleEntry = dtShortSaleEntry.Rows.Count == 0 ? "0000000" : dtShortSaleEntry.Rows[0][0].ToString();
                if (ShortSaleEntry == "0000000" || ShortSaleEntry == "0111111")
                {
                    LiShortSaleEntry.Visible = false;
                }
                #endregion ShortSaleEntry

                #region VendorScheduleReport
                string VendorScheduleReport = "";
                DataTable dtVendorScheduleReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 206 + "");
                VendorScheduleReport = dtVendorScheduleReport.Rows.Count == 0 ? "0000000" : dtVendorScheduleReport.Rows[0][0].ToString();
                if (VendorScheduleReport == "0000000" || VendorScheduleReport == "0111111")
                {
                    LiVendorScheduleReport.Visible = false;
                }
                #endregion VendorScheduleReport

                #region CustomerScheduleReport
                string CustomerScheduleReport = "";
                DataTable dtCustomerScheduleReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 213 + "");
                CustomerScheduleReport = dtCustomerScheduleReport.Rows.Count == 0 ? "0000000" : dtCustomerScheduleReport.Rows[0][0].ToString();
                if (CustomerScheduleReport == "0000000" || CustomerScheduleReport == "0111111")
                {
                    LiCustomerScheduleReport.Visible = false;
                }
                #endregion CustomerScheduleReport

                #region CastingtobeInspectReport
                string CastingtobeInspectReport = "";
                DataTable dtCastingtobeInspectReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 207 + "");
                CastingtobeInspectReport = dtCastingtobeInspectReport.Rows.Count == 0 ? "0000000" : dtCastingtobeInspectReport.Rows[0][0].ToString();
                if (CastingtobeInspectReport == "0000000" || CastingtobeInspectReport == "0111111")
                {
                    LiCastingtobeInspectReport.Visible = false;
                }
                #endregion CastingtobeInspectReport

                #region CastingtobeOffloadedReport
                string CastingtobeOffloadedReport = "";
                DataTable dtCastingtobeOffloadedReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 208 + "");
                CastingtobeOffloadedReport = dtCastingtobeOffloadedReport.Rows.Count == 0 ? "0000000" : dtCastingtobeOffloadedReport.Rows[0][0].ToString();
                if (CastingtobeOffloadedReport == "0000000" || CastingtobeOffloadedReport == "0111111")
                {
                    LiCastingtobeOffloadedReport.Visible = false;
                }
                #endregion CastingtobeOffloadedReport

                #region CastingtobeCast
                string CastingtobeCast = "";
                DataTable dtCastingtobeCast = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 209 + "");
                CastingtobeCast = dtCastingtobeCast.Rows.Count == 0 ? "0000000" : dtCastingtobeCast.Rows[0][0].ToString();
                if (CastingtobeCast == "0000000" || CastingtobeCast == "0111111")
                {
                    LiCastingtobeCast.Visible = false;
                }
                #endregion CastingtobeCast

                #region CorestobeMade
                string CorestobeMade = "";
                DataTable dtCorestobeMade = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 210 + "");
                CorestobeMade = dtCorestobeMade.Rows.Count == 0 ? "0000000" : dtCorestobeMade.Rows[0][0].ToString();
                if (CorestobeMade == "0000000" || CorestobeMade == "0111111")
                {
                    LiCorestobeMade.Visible = false;
                }
                #endregion CorestobeMade

                #region PurchaseRequirement
                string PurchaseRequirement = "";
                DataTable dtPurchaseRequirement = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 214 + "");
                PurchaseRequirement = dtPurchaseRequirement.Rows.Count == 0 ? "0000000" : dtPurchaseRequirement.Rows[0][0].ToString();
                if (PurchaseRequirement == "0000000" || PurchaseRequirement == "0111111")
                {
                    LiPurchaseRequirement.Visible = false;
                }
                #endregion PurchaseRequirement

                #region CastingtobeMachined
                string CastingtobeMachined = "";
                DataTable dtCastingtobeMachined = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 211 + "");
                CastingtobeMachined = dtCastingtobeMachined.Rows.Count == 0 ? "0000000" : dtCastingtobeMachined.Rows[0][0].ToString();
                if (CastingtobeMachined == "0000000" || CastingtobeMachined == "0111111")
                {
                    LiCastingtobeMachined.Visible = false;
                }
                #endregion CastingtobeMachined

                #region CastingInventoryReport
                string CastingInventoryReport = "";
                DataTable dtCastingInventoryReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 212 + "");
                CastingInventoryReport = dtCastingInventoryReport.Rows.Count == 0 ? "0000000" : dtCastingInventoryReport.Rows[0][0].ToString();
                if (CastingInventoryReport == "0000000" || CastingInventoryReport == "0111111")
                {
                    LiCastingInventoryReport.Visible = false;
                }
                #endregion CastingInventoryReport

                #region SalePlanningReport
                string SalePlanningReport = "";
                DataTable dtSalePlanningReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 215 + "");
                SalePlanningReport = dtSalePlanningReport.Rows.Count == 0 ? "0000000" : dtSalePlanningReport.Rows[0][0].ToString();
                if (SalePlanningReport == "0000000" || SalePlanningReport == "0111111")
                {
                    LiSalePlanningReport.Visible = false;
                }
                #endregion SalePlanningReport

                #region SaleSummaryCustomerwise
                string SaleSummaryCustomerwise = "";
                DataTable dtSaleSummaryCustomerwise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 216 + "");
                SaleSummaryCustomerwise = dtSaleSummaryCustomerwise.Rows.Count == 0 ? "0000000" : dtSaleSummaryCustomerwise.Rows[0][0].ToString();
                if (SaleSummaryCustomerwise == "0000000" || SaleSummaryCustomerwise == "0111111")
                {
                    LiSaleSummaryCustomerwise.Visible = false;
                }
                #endregion SaleSummaryCustomerwise

                #region SaleSummaryOwnerwise
                string SaleSummaryOwnerwise = "";
                DataTable dtSaleSummaryOwnerwise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 217 + "");
                SaleSummaryOwnerwise = dtSaleSummaryOwnerwise.Rows.Count == 0 ? "0000000" : dtSaleSummaryOwnerwise.Rows[0][0].ToString();
                if (SaleSummaryOwnerwise == "0000000" || SaleSummaryOwnerwise == "0111111")
                {
                    LiSaleSummaryOwnerwise.Visible = false;
                }
                #endregion SaleSummaryOwnerwise

                #region RMnSandRequire
                string RMnSandRequire = "";
                DataTable dtRMnSandRequire = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 218 + "");
                RMnSandRequire = dtRMnSandRequire.Rows.Count == 0 ? "0000000" : dtRMnSandRequire.Rows[0][0].ToString();
                if (RMnSandRequire == "0000000" || RMnSandRequire == "0111111")
                {
                    LiRMnSandRequire.Visible = false;
                }
                #endregion RMnSandRequire

                #region RMnSandPurchase
                string RMnSandPurchase = "";
                DataTable dtRMnSandPurchase = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 219 + "");
                RMnSandPurchase = dtRMnSandPurchase.Rows.Count == 0 ? "0000000" : dtRMnSandPurchase.Rows[0][0].ToString();
                if (RMnSandPurchase == "0000000" || RMnSandPurchase == "0111111")
                {
                    LiRMnSandPurchase.Visible = false;
                }
                #endregion RMnSandPurchase

                #region SandMinimumStandardMaster
                string SandMinimumStandardMaster = "";
                DataTable dtSandMinimumStandardMaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 220 + "");
                SandMinimumStandardMaster = dtSandMinimumStandardMaster.Rows.Count == 0 ? "0000000" : dtSandMinimumStandardMaster.Rows[0][0].ToString();
                if (SandMinimumStandardMaster == "0000000" || SandMinimumStandardMaster == "0111111")
                {
                    LiSandMinimumStandardMaster.Visible = false;
                }
                #endregion SandMinimumStandardMaster

                #region CoreShopCapacityPlan
                string CoreShopCapacityPlan = "";
                DataTable dtCoreShopCapacityPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 222 + "");
                CoreShopCapacityPlan = dtCoreShopCapacityPlan.Rows.Count == 0 ? "0000000" : dtCoreShopCapacityPlan.Rows[0][0].ToString();
                if (CoreShopCapacityPlan == "0000000" || CoreShopCapacityPlan == "0111111")
                {
                    LiCoreShopCapacityPlan.Visible = false;
                }
                #endregion CoreShopCapacityPlan

                #region CastingCapacityPlan
                string CastingCapacityPlan = "";
                DataTable dtCastingCapacityPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 223 + "");
                CastingCapacityPlan = dtCastingCapacityPlan.Rows.Count == 0 ? "0000000" : dtCastingCapacityPlan.Rows[0][0].ToString();
                if (CastingCapacityPlan == "0000000" || CastingCapacityPlan == "0111111")
                {
                    LiCastingCapacityPlan.Visible = false;
                }
                #endregion CastingCapacityPlan

                #region InhouseMachineShopCapacityPlan
                string InhouseMachineShopCapacityPlan = "";
                DataTable dtInhouseMachineShopCapacityPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 224 + "");
                InhouseMachineShopCapacityPlan = dtInhouseMachineShopCapacityPlan.Rows.Count == 0 ? "0000000" : dtInhouseMachineShopCapacityPlan.Rows[0][0].ToString();
                if (InhouseMachineShopCapacityPlan == "0000000" || InhouseMachineShopCapacityPlan == "0111111")
                {
                    LiInhouseMachineShopCapacityPlan.Visible = false;
                }
                #endregion InhouseMachineShopCapacityPlan

                #region VendorCapacityPlan
                string VendorCapacityPlan = "";
                DataTable dtVendorCapacityPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 225 + "");
                VendorCapacityPlan = dtVendorCapacityPlan.Rows.Count == 0 ? "0000000" : dtVendorCapacityPlan.Rows[0][0].ToString();
                if (VendorCapacityPlan == "0000000" || VendorCapacityPlan == "0111111")
                {
                    LiVendorCapacityPlan.Visible = false;
                }
                #endregion VendorCapacityPlan

                #region LeakageTestCapacityPlan
                string LeakageTestCapacityPlan = "";
                DataTable dtLeakageTestCapacityPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 226 + "");
                LeakageTestCapacityPlan = dtLeakageTestCapacityPlan.Rows.Count == 0 ? "0000000" : dtLeakageTestCapacityPlan.Rows[0][0].ToString();
                if (LeakageTestCapacityPlan == "0000000" || LeakageTestCapacityPlan == "0111111")
                {
                    LiLeakageTestCapacityPlan.Visible = false;
                }
                #endregion LeakageTestCapacityPlan

                #region FoundryCapBookSummary
                string FoundryCapBookSummary = "";
                DataTable dtFoundryCapBookSummary = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 227 + "");
                FoundryCapBookSummary = dtFoundryCapBookSummary.Rows.Count == 0 ? "0000000" : dtFoundryCapBookSummary.Rows[0][0].ToString();
                if (FoundryCapBookSummary == "0000000" || FoundryCapBookSummary == "0111111")
                {
                    LiFoundryCapBookSummary.Visible = false;
                }
                #endregion FoundryCapBookSummary

                #region TotMachineCapBookSummary
                string TotMachineCapBookSummary = "";
                DataTable dtTotMachineCapBookSummary = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 228 + "");
                TotMachineCapBookSummary = dtTotMachineCapBookSummary.Rows.Count == 0 ? "0000000" : dtTotMachineCapBookSummary.Rows[0][0].ToString();
                if (TotMachineCapBookSummary == "0000000" || TotMachineCapBookSummary == "0111111")
                {
                    LiTotMachineCapBookSummary.Visible = false;
                }
                #endregion TotMachineCapBookSummary

                #region LeakageBookCapSummary
                string LeakageBookCapSummary = "";
                DataTable dtLeakageBookCapSummary = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 229 + "");
                LeakageBookCapSummary = dtLeakageBookCapSummary.Rows.Count == 0 ? "0000000" : dtLeakageBookCapSummary.Rows[0][0].ToString();
                if (LeakageBookCapSummary == "0000000" || LeakageBookCapSummary == "0111111")
                {
                    LiLeakageBookCapSummary.Visible = false;
                }
                #endregion LeakageBookCapSummary

                #region LiWeeklysalePlan
                string LiWeeklysalePlans = "";
                DataTable dtLiWeeklysalePlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 230 + "");
                LiWeeklysalePlans = dtLiWeeklysalePlan.Rows.Count == 0 ? "0000000" : dtLiWeeklysalePlan.Rows[0][0].ToString();
                if (LiWeeklysalePlans == "0000000" || LiWeeklysalePlans == "0111111")
                {
                    LiWeeklysalePlan.Visible = false;
                }
                #endregion LiWeeklysalePlan

                #region LiCastReqForInsps
                string LiCastReqForInspss = "";
                DataTable dtLiCastReqForInsps = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 231 + "");
                LiCastReqForInspss = dtLiCastReqForInsps.Rows.Count == 0 ? "0000000" : dtLiCastReqForInsps.Rows[0][0].ToString();
                if (LiCastReqForInspss == "0000000" || LiCastReqForInspss == "0111111")
                {
                    LiCastReqForInsps.Visible = false;
                }
                #endregion LiCastReqForInsps

                #region LiCastReqForMachShop
                string LiCastReqForMachShops = "";
                DataTable dtLiCastReqForMachShop = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 232 + "");
                LiCastReqForMachShops = dtLiCastReqForMachShop.Rows.Count == 0 ? "0000000" : dtLiCastReqForMachShop.Rows[0][0].ToString();
                if (LiCastReqForMachShops == "0000000" || LiCastReqForMachShops == "0111111")
                {
                    LiCastReqForMachShop.Visible = false;
                }
                #endregion LiCastReqForMachShop

                #region LiCastReqForRFMToMS
                string LiCastReqForRFMToMSs = "";
                DataTable dtLiCastReqForRFMToMSs = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 233 + "");
                LiCastReqForRFMToMSs = dtLiCastReqForRFMToMSs.Rows.Count == 0 ? "0000000" : dtLiCastReqForRFMToMSs.Rows[0][0].ToString();
                if (LiCastReqForRFMToMSs == "0000000" || LiCastReqForRFMToMSs == "0111111")
                {
                    LiCastReqForRFMToMS.Visible = false;
                }
                #endregion LiCastReqForRFMToMS

                #region LiCastReqForRFMToVndor
                string LiCastReqForRFMToVndors = "";
                DataTable dtLiWeeklysalePlans = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 234 + "");
                LiCastReqForRFMToVndors = dtLiWeeklysalePlans.Rows.Count == 0 ? "0000000" : dtLiWeeklysalePlans.Rows[0][0].ToString();
                if (LiCastReqForRFMToVndors == "0000000" || LiCastReqForRFMToVndors == "0111111")
                {
                    LiCastReqForRFMToVndor.Visible = false;
                }
                #endregion LiCastReqForRFMToVndor

                #region LiWeeklysalePlan
                string LiCastToBeDisFromFoundrys = "";
                DataTable dtLiCastToBeDisFromFoundrys = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 235 + "");
                LiCastToBeDisFromFoundrys = dtLiCastToBeDisFromFoundrys.Rows.Count == 0 ? "0000000" : dtLiCastToBeDisFromFoundrys.Rows[0][0].ToString();
                if (LiCastToBeDisFromFoundrys == "0000000" || LiCastToBeDisFromFoundrys == "0111111")
                {
                    LiCastToBeDisFromFoundry.Visible = false;
                }
                #endregion LeakageBookCapSummary

                #region LiCastToBeDisFromCoreShop
                string LiCastToBeDisFromCoreShops = "";
                DataTable dtLiCastToBeDisFromCoreShop = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 236 + "");
                LiCastToBeDisFromCoreShops = dtLiCastToBeDisFromCoreShop.Rows.Count == 0 ? "0000000" : dtLiCastToBeDisFromCoreShop.Rows[0][0].ToString();
                if (LiCastToBeDisFromCoreShops == "0000000" || LiCastToBeDisFromCoreShops == "0111111")
                {
                    LiCastToBeDisFromCoreShop.Visible = false;
                }
                #endregion LiCastToBeDisFromCoreShop

                #region LiDailySalePerfrpt
                string LiDailySalePerfrpts = "";
                DataTable dtLiDailySalePerfrpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 253 + "");
                LiDailySalePerfrpts = dtLiDailySalePerfrpt.Rows.Count == 0 ? "0000000" : dtLiDailySalePerfrpt.Rows[0][0].ToString();
                if (LiDailySalePerfrpts == "0000000" || LiDailySalePerfrpts == "0111111")
                {
                    LiDailySalePerfrpt.Visible = false;
                }
                #endregion LiDailySalePerfrpt

                #region LiCastToBeDisFromCoreShop
                string LiCastReqForVendors = "";
                DataTable dtLiCastReqForVendors = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 237 + "");
                LiCastReqForVendors = dtLiCastReqForVendors.Rows.Count == 0 ? "0000000" : dtLiCastReqForVendors.Rows[0][0].ToString();
                if (LiCastReqForVendors == "0000000" || LiCastReqForVendors == "0111111")
                {
                    LiCastReqForVendor.Visible = false;
                }
                #endregion LiCastToBeDisFromCoreShop

                #endregion
            }
        }
    }
    #endregion

    #region Masters

    #region btnGroup_click
    protected void btnGroup_click(object sender, EventArgs e)
    {
        checkRights(180); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewGroupMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnGroup_click

    #region btnProduct_click
    protected void btnProduct_click(object sender, EventArgs e)
    {
        checkRights(181); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewProductMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnProduct_click

    #region btnStandard_click
    protected void btnStandard_click(object sender, EventArgs e)
    {
        checkRights(182); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewStandardInventoryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnStandard_click

    #region btnReason_click
    protected void btnReason_click(object sender, EventArgs e)
    {
        checkRights(183); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewReasonMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnReason_click

    #region btnProductivity_click
    protected void btnProductivity_click(object sender, EventArgs e)
    {
        checkRights(184); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewProductivityMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnProductivity_click

    #region btnMachine_click
    protected void btnMachine_click(object sender, EventArgs e)
    {
        checkRights(185); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewMachineMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnMachine_click

    #region btnProcessMachine_click
    protected void btnProcessMachine_click(object sender, EventArgs e)
    {
        checkRights(186); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewProcessMachineMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnProcessMachine_click

    #region btnVendorMachine_click
    protected void btnVendorMachine_click(object sender, EventArgs e)
    {
        checkRights(187); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewVendorMachineMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVendorMachine_click

    #region btnBoughtout_click
    protected void btnBoughtout_click(object sender, EventArgs e)
    {
        checkRights(188); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewBoughtoutMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnBoughtout_click

    #region btnProcessBom_click
    protected void btnProcessBom_click(object sender, EventArgs e)
    {
        checkRights(189); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewProcessBomMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnProcessBom_click

    #region btnPallet_click
    protected void btnPallet_click(object sender, EventArgs e)
    {
        checkRights(190); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewPalletMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnPallet_click

    #region btnBox_click
    protected void btnBox_click(object sender, EventArgs e)
    {
        checkRights(191); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewBoxMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnBox_click

    #region btnMachiningToolCost_click
    protected void btnMachiningToolCost_click(object sender, EventArgs e)
    {
        checkRights(192); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewMachiningToolCostMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnMachiningToolCost_click

    #region btnMachineBooking_click
    protected void btnMachineBooking_click(object sender, EventArgs e)
    {
        checkRights(193); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewMachineBookingMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnMachineBooking_click

    #region btnBOMMaster_click
    protected void btnBOMMaster_click(object sender, EventArgs e)
    {
        checkRights(266); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/CPBOM_Master.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnBOMMaster_click

    #endregion Masters

    #region Transactions

    #region btnCoreInventory_click
    protected void btnCoreInventory_click(object sender, EventArgs e)
    {
        checkRights(204); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewCoreInventoryTransaction.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnCoreInventory_click

    #region btnCustomerSchedule_click
    protected void btnCustomerSchedule_click(object sender, EventArgs e)
    {
        checkRights(203); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewCustomerSchedule.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnCustomerSchedule_click

    #region btnVendorSchedule_click
    protected void btnVendorSchedule_click(object sender, EventArgs e)
    {
        checkRights(205); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewVendorSchedule.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVendorSchedule_click


    #region btnUnplanVendorSchedule_click
    protected void btnUnplanVendorSchedule_click(object sender, EventArgs e)
    {
        checkRights(205); // Check Rights
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewUnplanVendorSchedule.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVendorSchedule_click

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

    #region btnMonth_click
    protected void btnMonth_click(object sender, EventArgs e)
    {
        pnlMonth.Visible = true;
        pnlCapacity.Visible = false;
        pnlWeekly.Visible = false;
        pnlDaily.Visible = false;
    }
    #endregion btnMonth_click

    #region btnCapacity_click
    protected void btnCapacity_click(object sender, EventArgs e)
    {
        pnlMonth.Visible = false;
        pnlCapacity.Visible = true;
        pnlWeekly.Visible = false;
        pnlDaily.Visible = false;
    }
    #endregion btnCapacity_click

    #region btnWeekly_click
    protected void btnWeekly_click(object sender, EventArgs e)
    {
        pnlMonth.Visible = false;
        pnlCapacity.Visible = false;
        pnlWeekly.Visible = true;
        pnlDaily.Visible = false;
    }
    #endregion btnWeekly_click

    #region btnDaily_click
    protected void btnDaily_click(object sender, EventArgs e)
    {
        pnlMonth.Visible = false;
        pnlCapacity.Visible = false;
        pnlWeekly.Visible = false;
        pnlDaily.Visible = true;
    }
    #endregion btnDaily_click

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