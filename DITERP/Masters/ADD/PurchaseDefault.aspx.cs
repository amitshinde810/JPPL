using System;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_PurchaseDefault : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");

        home.Attributes["class"] = "active";

        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");

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
                string potypemaster = "";
                DataTable dtpotypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 33 + "");
                potypemaster = dtpotypemaster.Rows.Count == 0 ? "0000000" : dtpotypemaster.Rows[0][0].ToString();
                if (potypemaster == "0000000" || potypemaster == "0111111")
                {
                    POTypeMaster.Visible = false;
                }
                string PO_MIS = "";
                DataTable dtPOMIS = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 118 + "");
                PO_MIS = dtPOMIS.Rows.Count == 0 ? "0000000" : dtPOMIS.Rows[0][0].ToString();
                if (PO_MIS == "0000000" || PO_MIS == "0111111")
                {
                    POMISREG.Visible = false;
                }
                string suptypemaster = "";
                DataTable dtsuptypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 17 + "");
                suptypemaster = dtsuptypemaster.Rows.Count == 0 ? "0000000" : dtsuptypemaster.Rows[0][0].ToString();
                if (suptypemaster == "0000000" || suptypemaster == "0111111")
                {
                    SuppTypeMaster.Visible = false;
                }
                string supplier = "";
                DataTable dtsupplier = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 26 + "");
                supplier = dtsupplier.Rows.Count == 0 ? "0000000" : dtsupplier.Rows[0][0].ToString();
                if (supplier == "0000000" || supplier == "0111111")
                {
                    SuppMaster.Visible = false;
                }
                string purord = "";
                DataTable dtpurord = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 42 + "");
                purord = dtpurord.Rows.Count == 0 ? "0000000" : dtpurord.Rows[0][0].ToString();
                if (purord == "0000000" || purord == "0111111")
                {
                    PurOrd.Visible = false;
                }
                string inwrd = "";
                DataTable dtinwrd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 39 + "");
                inwrd = dtinwrd.Rows.Count == 0 ? "0000000" : dtinwrd.Rows[0][0].ToString();
                if (inwrd == "0000000" || inwrd == "0111111")
                {
                    Inwrd.Visible = false;
                }
                string inspection = "";
                DataTable dtinspection = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 40 + "");
                inspection = dtinspection.Rows.Count == 0 ? "0000000" : dtinspection.Rows[0][0].ToString();
                if (inspection == "0000000" || inspection == "0111111")
                {
                    Inspection.Visible = false;
                }
                string billpass = "";
                DataTable dtbillpass = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 41 + "");
                billpass = dtbillpass.Rows.Count == 0 ? "0000000" : dtbillpass.Rows[0][0].ToString();
                if (billpass == "0000000" || billpass == "0111111")
                {
                    BillPass.Visible = false;
                }
                string purreq = "";
                DataTable dtpurreq = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 54 + "");
                purreq = dtpurreq.Rows.Count == 0 ? "0000000" : dtpurreq.Rows[0][0].ToString();
                if (purreq == "0000000" || purreq == "0111111")
                {
                    PurReq.Visible = false;
                }
                string purrej = "";
                DataTable dtpurrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 68 + "");
                purrej = dtpurrej.Rows.Count == 0 ? "0000000" : dtpurrej.Rows[0][0].ToString();
                if (purrej == "0000000" || purrej == "0111111")
                {
                    PurRej.Visible = false;
                }
                string amc = "";
                DataTable dtamc = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 69 + "");
                amc = dtamc.Rows.Count == 0 ? "0000000" : dtamc.Rows[0][0].ToString();
                if (amc == "0000000" || amc == "0111111")
                {
                    AMC.Visible = false;
                }
                string wo = "";
                DataTable dtwo = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 70 + "");
                wo = dtwo.Rows.Count == 0 ? "0000000" : dtwo.Rows[0][0].ToString();
                if (wo == "0000000" || wo == "0111111")
                {
                    WRK.Visible = false;
                }
                string PO_Transfer = "";
                DataTable dtPOTransfer = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 106 + "");
                wo = dtPOTransfer.Rows.Count == 0 ? "0000000" : dtPOTransfer.Rows[0][0].ToString();
                if (PO_Transfer == "0000000" || PO_Transfer == "0111111")
                {
                    POTransfer.Visible = false;
                }
                string purordreg = "";
                DataTable dtpurordreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 46 + "");
                purordreg = dtpurordreg.Rows.Count == 0 ? "0000000" : dtpurordreg.Rows[0][0].ToString();
                if (purordreg == "0000000" || purordreg == "0111111")
                {
                    POReg.Visible = false;
                }
                string matinwdreg = "";
                DataTable dtmatinwdreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 47 + "");
                matinwdreg = dtmatinwdreg.Rows.Count == 0 ? "0000000" : dtmatinwdreg.Rows[0][0].ToString();
                if (matinwdreg == "0000000" || matinwdreg == "0111111")
                {
                }
                string inspreg = "";
                DataTable dtinspreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 48 + "");
                inspreg = dtinspreg.Rows.Count == 0 ? "0000000" : dtinspreg.Rows[0][0].ToString();
                if (inspreg == "0000000" || inspreg == "0111111")
                {
                }
                string billpassreg = "";
                DataTable dtbillpassreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 49 + "");
                billpassreg = dtbillpassreg.Rows.Count == 0 ? "0000000" : dtbillpassreg.Rows[0][0].ToString();
                if (billpassreg == "0000000" || billpassreg == "0111111")
                {
                    BillPassReg.Visible = false;
                }
                string purreqreg = "";
                DataTable dtpurreqreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 58 + "");
                purreqreg = dtpurreqreg.Rows.Count == 0 ? "0000000" : dtpurreqreg.Rows[0][0].ToString();
                if (purreqreg == "0000000" || purreqreg == "0111111")
                {
                    PurReqReg.Visible = false;
                }
                string purrejreg = "";
                DataTable dtpurrejreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 79 + "");
                purrejreg = dtpurrejreg.Rows.Count == 0 ? "0000000" : dtpurrejreg.Rows[0][0].ToString();
                if (purrejreg == "0000000" || purrejreg == "0111111")
                {
                    PurRejReg.Visible = false;
                }
                string SubConPO = "";
                DataTable dtSubConPO = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 42 + "");
                SubConPO = dtSubConPO.Rows.Count == 0 ? "0000000" : dtSubConPO.Rows[0][0].ToString();
                if (SubConPO == "0000000" || SubConPO == "0111111")
                {
                    SubConPOT.Visible = false;
                }
                string Item = "";
                DataTable dtItem = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 11 + "");
                Item = dtItem.Rows.Count == 0 ? "0000000" : dtItem.Rows[0][0].ToString();
                if (Item == "0000000" || Item == "0111111")
                {
                    ItemM.Visible = false;
                }
                string Proces = "";
                DataTable dtProces = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 88 + "");
                Proces = dtProces.Rows.Count == 0 ? "0000000" : dtProces.Rows[0][0].ToString();
                if (Proces == "0000000" || Proces == "0111111")
                {
                    PrcessM.Visible = false;
                }
                string Proj = "";
                DataTable dtProj = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 112 + "");
                Proj = dtProj.Rows.Count == 0 ? "0000000" : dtProj.Rows[0][0].ToString();
                if (Proj == "0000000" || Proj == "0111111")
                {
                    ProjCodeM.Visible = false;
                }
                string PurAMD = "";
                DataTable dtPurAMD = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 89 + "");
                PurAMD = dtPurAMD.Rows.Count == 0 ? "0000000" : dtPurAMD.Rows[0][0].ToString();
                if (PurAMD == "0000000" || PurAMD == "0111111")
                {
                    PurAMDR.Visible = false;
                }

                string UnSettled = "";
                DataTable dtLiUnSettled = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 250 + "");
                UnSettled = dtLiUnSettled.Rows.Count == 0 ? "0000000" : dtLiUnSettled.Rows[0][0].ToString();
                if (UnSettled == "0000000" || UnSettled == "0111111")
                {
                    LiUnSettled.Visible = false;
                }

                string ASuppMastrs = "";
                DataTable dtASuppMastr = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 242 + "");
                ASuppMastrs = dtASuppMastr.Rows.Count == 0 ? "0000000" : dtASuppMastr.Rows[0][0].ToString();
                if (ASuppMastrs == "0000000" || ASuppMastrs == "0111111")
                {
                    ASuppMastr.Visible = false;
                }
                string DPerRpts = "";
                DataTable dtDPerRpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 243 + "");
                DPerRpts = dtDPerRpt.Rows.Count == 0 ? "0000000" : dtDPerRpt.Rows[0][0].ToString();
                if (DPerRpts == "0000000" || DPerRpts == "0111111")
                {
                    DPerRpt.Visible = false;
                }

                string SuppAudits = "";
                DataTable dtSuppAudits = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 244 + "");
                SuppAudits = dtSuppAudits.Rows.Count == 0 ? "0000000" : dtSuppAudits.Rows[0][0].ToString();
                if (SuppAudits == "0000000" || SuppAudits == "0111111")
                {
                    SuppAudit.Visible = false;
                }
                string SuppAuditsPlan = "";
                DataTable dtSuppAuditsPlan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 255 + "");
                SuppAuditsPlan = dtSuppAuditsPlan.Rows.Count == 0 ? "0000000" : dtSuppAuditsPlan.Rows[0][0].ToString();
                if (SuppAuditsPlan == "0000000" || SuppAuditsPlan == "0111111")
                {
                    liSuppAudiPlan.Visible = false;
                }

                string PurSchedule = "";
                DataTable dtSPurSchedule = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 240 + "");
                PurSchedule = dtSPurSchedule.Rows.Count == 0 ? "0000000" : dtSPurSchedule.Rows[0][0].ToString();
                if (PurSchedule == "0000000" || PurSchedule == "0111111")
                {
                    liPurSchedule.Visible = false;
                    liPurSchedulReg.Visible = false;
                }

                string PurScheduleApproval = "";
                DataTable dtPurScheduleApproval = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 258 + "");
                PurScheduleApproval = dtPurScheduleApproval.Rows.Count == 0 ? "0000000" : dtPurScheduleApproval.Rows[0][0].ToString();
                if (PurScheduleApproval == "0000000" || PurScheduleApproval == "0111111")
                {
                    liPurScheduleApprove.Visible = false;
                }

                string VendorScheduleApproval = "";
                DataTable dtVendorScheduleApproval = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 260 + "");
                VendorScheduleApproval = dtVendorScheduleApproval.Rows.Count == 0 ? "0000000" : dtVendorScheduleApproval.Rows[0][0].ToString();
                if (VendorScheduleApproval == "0000000" || VendorScheduleApproval == "0111111")
                {
                    liVendorScheduleApproval.Visible = false;
                }

                string SuppAuditplan = "";
                DataTable dtSuppAuditplan = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 261 + "");
                SuppAuditplan = dtSuppAuditplan.Rows.Count == 0 ? "0000000" : dtSuppAuditplan.Rows[0][0].ToString();
                if (SuppAuditplan == "0000000" || SuppAuditplan == "0111111")
                {
                    auditplan.Visible = false;
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

    #region Master
    protected void btnSupplierMaster_click(object sender, EventArgs e)
    {
        checkRights(26);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSupplierMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSupplierTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(17);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSupplierTypeMaster.aspx", false);
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
    protected void btnProcess_click(object sender, EventArgs e)
    {
        checkRights(88);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Admin/VIEW/ViewProcessMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPoTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(33);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewPoTypeMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnProCode_click(object sender, EventArgs e)
    {
        checkRights(112);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnSuppCatMaster_click
    protected void btnSuppCatMaster_click(object sender, EventArgs e)
    {
        checkRights(245);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewSupplierCategoryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppCatMaster_click

    #region btnSuppPerfRemark_click
    protected void btnSuppPerfRemark_click(object sender, EventArgs e)
    {
        checkRights(247);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewSupplierPerfRemark.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppPerfRemark_click


    protected void btnIndentType_click(object sender, EventArgs e)
    {
        checkRights(278);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/IndentTypeMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #endregion

    #region Transaction

    protected void IndentDetail_click(object sender, EventArgs e)
    {
        checkRights(278);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSupplierPO_click(object sender, EventArgs e)
    {
        checkRights(42);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnSubContractorPO_click(object sender, EventArgs e)
    {
        checkRights(42);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMaterialInspection_click(object sender, EventArgs e)
    {
        checkRights(40);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInspection.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMaterialInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnAMC_click(object sender, EventArgs e)
    {
        checkRights(69);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnWO_click(object sender, EventArgs e)
    {
        checkRights(70);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewAMCWorkOrder.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBillPassing_click(object sender, EventArgs e)
    {
        checkRights(41);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnPurReq_click(object sender, EventArgs e)
    {
        checkRights(54);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPurRej_click(object sender, EventArgs e)
    {
        checkRights(68);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void POTransfer_click(object sender, EventArgs e)
    {
        checkRights(106);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPOTransfer.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnliPurSchedule_click
    protected void btnliPurSchedule_click(object sender, EventArgs e)
    {
        checkRights(240);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseSchedule.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnliPurSchedule_click

    #region btnPurScheduleApprove_click
    protected void btnPurScheduleApprove_click(object sender, EventArgs e)
    {
        checkRights(258);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseScheduleApproval.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnliPurSchedule_click

    #region btnVendorScheduleApprove_click
    protected void btnVendorScheduleApprove_click(object sender, EventArgs e)
    {
        checkRights(258);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/PPC/VIEW/ViewVendorScheduleApproval.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVendorScheduleApprove_click



    #region btnSuppAudit_click
    protected void btnSuppAudit_click(object sender, EventArgs e)
    {
        checkRights(244);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSupplierAudit.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppAudit_click

    #region btnSuppAuditPlan_click
    protected void btnSuppAuditPlan_click(object sender, EventArgs e)
    {
        checkRights(255);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSupplierAuditPlan.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppAudit_click

    #endregion

    #region Reports



    protected void btnIndentReport_click(object sender, EventArgs e)
    {
        checkRights(278);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewIndent.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnSupplierPoRegister_click(object sender, EventArgs e)
    {
        checkRights(46);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    #region btnPurSchedulReg_click
    protected void btnPurSchedulReg_click(object sender, EventArgs e)
    {
        checkRights(240);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewPurchaseScheduleRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnliPurSchedule_click


    protected void btnPOMIS_click(object sender, EventArgs e)
    {
        checkRights(118);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/ADD/ProjcodeWiseMIS.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSupplierPoAmendRegister_click(object sender, EventArgs e)
    {
        checkRights(89);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoAmendRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBillPassingRegister_click(object sender, EventArgs e)
    {
        checkRights(49);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBillPassingRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPurchaseRequisitionRegister_Click(object sender, EventArgs e)
    {
        checkRights(58);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewPurchaseRequisitionRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnPurchaseRejectionRegister_Click(object sender, EventArgs e)
    {
        checkRights(79);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/PurchaseRejectionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSubConReg_click(object sender, EventArgs e)
    {
        checkRights(163);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubConReg.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnASuppMastr_click(object sender, EventArgs e)
    {
        checkRights(242);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnUnSettled_click(object sender, EventArgs e)
    {
        checkRights(250);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubContactorPORateReport1n2.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnQuality_click
    protected void btnQuality_click(object sender, EventArgs e)
    {
        checkRights(248);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/QualityPerfReportView.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnQuality_click

    #region btnOverallPer_click
    protected void btnOverallPer_click(object sender, EventArgs e)
    {
        checkRights(249);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/OverallPerfView.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnOverallPer_click

    protected void btnDPerRpt_click(object sender, EventArgs e)
    {
        checkRights(243);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DeliveryPerfView.aspx", false);  //ViewDeliveryPerformanceReport
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnSuppSchedule_click
    protected void btnSuppSchedule_click(object sender, EventArgs e)
    {
        checkRights(246);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierSchedule.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppSchedule_click

    #region btnSuppAuditPlanRepo_click
    protected void btnSuppAuditPlanRepo_click(object sender, EventArgs e)
    {
        checkRights(261);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierAuditRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSuppSchedule_click

    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Default", "btnOk_Click", Ex.Message);
        }
    }
}
