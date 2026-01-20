using System;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_IRNDefault : System.Web.UI.Page
{
    string right = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
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

                string Stage = "";
                DataTable dtStage = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 121 + "");
                Stage = dtStage.Rows.Count == 0 ? "0000000" : dtStage.Rows[0][0].ToString();
                if (Stage == "0000000" || Stage == "0111111")
                {
                    StageMaster.Visible = false;
                }
                string ReasonM = "";
                DataTable dtReasonM = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 122 + "");
                ReasonM = dtReasonM.Rows.Count == 0 ? "0000000" : dtReasonM.Rows[0][0].ToString();
                if (ReasonM == "0000000" || ReasonM == "0111111")
                {
                    Reason.Visible = false;
                }
                string FoundryY = "";
                DataTable dtFoundryT = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 123 + "");
                FoundryY = dtFoundryT.Rows.Count == 0 ? "0000000" : dtFoundryT.Rows[0][0].ToString();
                if (FoundryY == "0000000" || FoundryY == "0111111")
                {
                    Foundry.Visible = false;
                }
                string OtherT = "";
                DataTable dtOtherT = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 124 + "");
                OtherT = dtOtherT.Rows.Count == 0 ? "0000000" : dtOtherT.Rows[0][0].ToString();
                if (OtherT == "0000000" || OtherT == "0111111")
                {
                    Other.Visible = false;
                }
                string ProdT = "";
                DataTable dtProdT = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 126 + "");
                ProdT = dtProdT.Rows.Count == 0 ? "0000000" : dtProdT.Rows[0][0].ToString();
                if (ProdT == "0000000" || ProdT == "0111111")
                {
                    Prod.Visible = false;
                    liProEntryReg.Visible = false;
                }

                string DailyIRNR = "";
                DataTable dtDailyIRNR = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 127 + "");
                DailyIRNR = dtDailyIRNR.Rows.Count == 0 ? "0000000" : dtDailyIRNR.Rows[0][0].ToString();
                if (DailyIRNR == "0000000" || DailyIRNR == "0111111")
                {
                    DailyIRN.Visible = false;
                }

                string RejSummary = "";
                DataTable dtRejSummary = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 130 + "");
                RejSummary = dtRejSummary.Rows.Count == 0 ? "0000000" : dtRejSummary.Rows[0][0].ToString();
                if (RejSummary == "0000000" || RejSummary == "0111111")
                {
                    RejSummaryR.Visible = false;
                }

                string RejDetail = "";
                DataTable dtRejDetail = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 131 + "");
                RejDetail = dtRejDetail.Rows.Count == 0 ? "0000000" : dtRejDetail.Rows[0][0].ToString();
                if (RejDetail == "0000000" || RejDetail == "0111111")
                {
                    RejDetailR.Visible = false;
                }

                string DefRejDetils = "";
                DataTable dtDefRejDetils = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 132 + "");
                RejDetail = dtDefRejDetils.Rows.Count == 0 ? "0000000" : dtDefRejDetils.Rows[0][0].ToString();
                if (DefRejDetils == "0000000" || DefRejDetils == "0111111")
                {
                    DefRejDetilsR.Visible = false;
                }


                string FdryRejYearly = "";
                DataTable dtFdryRejYearly = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 133 + "");
                FdryRejYearly = dtFdryRejYearly.Rows.Count == 0 ? "0000000" : dtFdryRejYearly.Rows[0][0].ToString();
                if (FdryRejYearly == "0000000" || FdryRejYearly == "0111111")
                {
                    FdryRejYearlyR.Visible = false;
                }

                string MechRejYearly = "";
                DataTable dtMechRejYearly = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 134 + "");
                MechRejYearly = dtMechRejYearly.Rows.Count == 0 ? "0000000" : dtMechRejYearly.Rows[0][0].ToString();
                if (MechRejYearly == "0000000" || MechRejYearly == "0111111")
                {
                    MechRejYearlyR.Visible = false;
                }

                string DefFodryRejYearly = "";
                DataTable dtDefFodryRejYearly = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 135 + "");
                DefFodryRejYearly = dtDefFodryRejYearly.Rows.Count == 0 ? "0000000" : dtDefFodryRejYearly.Rows[0][0].ToString();
                if (DefFodryRejYearly == "0000000" || DefFodryRejYearly == "0111111")
                {
                    DefFodryRejYearlyR.Visible = false;
                }

                string DefMechRejYearly = "";
                DataTable dtDefMechRejYearly = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 136 + "");
                DefMechRejYearly = dtDefMechRejYearly.Rows.Count == 0 ? "0000000" : dtDefMechRejYearly.Rows[0][0].ToString();
                if (DefMechRejYearly == "0000000" || DefMechRejYearly == "0111111")
                {
                    DefMechRejYearlyR.Visible = false;
                }

                string DeptWise = "";
                DataTable dtDeptWise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 137 + "");
                DeptWise = dtDeptWise.Rows.Count == 0 ? "0000000" : dtDeptWise.Rows[0][0].ToString();
                if (DeptWise == "0000000" || DeptWise == "0111111")
                {
                    DeptWiseR.Visible = false;
                }


                string RejPerf = "";
                DataTable dtRejPerf = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 138 + "");
                RejPerf = dtRejPerf.Rows.Count == 0 ? "0000000" : dtRejPerf.Rows[0][0].ToString();
                if (RejPerf == "0000000" || RejPerf == "0111111")
                {
                    RejPerfR.Visible = false;
                }

                string Vendor = "";
                DataTable dtVendor = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 140 + "");
                Vendor = dtVendor.Rows.Count == 0 ? "0000000" : dtVendor.Rows[0][0].ToString();
                if (Vendor == "0000000" || Vendor == "0111111")
                {
                    VendorR.Visible = false;
                }

                string StageC = "";
                DataTable dtStageC = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 141 + "");
                StageC = dtStageC.Rows.Count == 0 ? "0000000" : dtStageC.Rows[0][0].ToString();
                if (StageC == "0000000" || StageC == "0111111")
                {
                    StageChange.Visible = false;
                }

                string Line = "";
                DataTable dtLine = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 142 + "");
                Line = dtLine.Rows.Count == 0 ? "0000000" : dtLine.Rows[0][0].ToString();
                if (Line == "0000000" || Line == "0111111")
                {
                    LineChange.Visible = false;
                }

                string IntrRej = "";
                DataTable dtIntrRej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 257 + "");
                IntrRej = dtIntrRej.Rows.Count == 0 ? "0000000" : dtIntrRej.Rows[0][0].ToString();
                if (IntrRej == "0000000" || IntrRej == "0111111")
                {
                    liIntrRejCAPA.Visible = false;
                    InternalCAPA.Visible = false;
                }

                string CustRejIRN = "";
                DataTable dtCustRejIRN = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 256 + "");
                CustRejIRN = dtCustRejIRN.Rows.Count == 0 ? "0000000" : dtCustRejIRN.Rows[0][0].ToString();
                if (CustRejIRN == "0000000" || CustRejIRN == "0111111")
                {
                    liCustRej.Visible = false;

                }
                string CustRejIwn = "";
                DataTable dtCustRejIwn = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 21 + "");
                CustRejIwn = dtCustRejIwn.Rows.Count == 0 ? "0000000" : dtCustRejIwn.Rows[0][0].ToString();
                if (CustRejIwn == "0000000" || CustRejIwn == "0111111")
                {
                    CustRejInward.Visible = false;
                }

                string StageHistory = "";
                DataTable dtStageHistory = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 143 + "");
                StageHistory = dtStageHistory.Rows.Count == 0 ? "0000000" : dtStageHistory.Rows[0][0].ToString();
                if (StageC == "0000000" || StageC == "0111111")
                {
                    StageHistoryR.Visible = false;
                }

                string LineHistory = "";
                DataTable dtLineHistory = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 144 + "");
                LineHistory = dtLineHistory.Rows.Count == 0 ? "0000000" : dtLineHistory.Rows[0][0].ToString();
                if (LineHistory == "0000000" || LineHistory == "0111111")
                {
                    LineHistoryR.Visible = false;
                }
                string CompStage = "";
                DataTable dtCompStage = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 145 + "");
                CompStage = dtCompStage.Rows.Count == 0 ? "0000000" : dtCompStage.Rows[0][0].ToString();
                if (CompStage == "0000000" || CompStage == "0111111")
                {
                    CompR.Visible = false;
                }


                string RejSummStage = "";
                DataTable dtRejSummStage = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 146 + "");
                RejSummStage = dtRejSummStage.Rows.Count == 0 ? "0000000" : dtRejSummStage.Rows[0][0].ToString();
                if (RejSummStage == "0000000" || RejSummStage == "0111111")
                {
                    RejSummStageR.Visible = false;
                }
                string liEffectiveCAPAs = "";
                DataTable dtliEffectiveCAPA = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 259 + "");
                liEffectiveCAPAs = dtliEffectiveCAPA.Rows.Count == 0 ? "0000000" : dtliEffectiveCAPA.Rows[0][0].ToString();
                if (liEffectiveCAPAs == "0000000" || liEffectiveCAPAs == "0111111")
                {
                    liEffectiveCAPA.Visible = false;
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

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region MASTER
    #region btnRejection_click
    protected void btnRejection_click(object sender, EventArgs e)
    {
        checkRights(121);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewRejectionStageMaster.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion
    #region btnReason_click
    protected void btnReason_click(object sender, EventArgs e)
    {
        checkRights(122);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewReasonMaster.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion
    #endregion

    #region TRANSACTION
    #region btnFoundry_click
    protected void btnFoundry_click(object sender, EventArgs e)
    {
        checkRights(123);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewFoundryIRNEntry.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnOther_click
    protected void btnOther_click(object sender, EventArgs e)
    {
        checkRights(124);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewOtherIRNEntry.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnOther_click
    protected void btnProd_click(object sender, EventArgs e)
    {
        checkRights(126);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewProdEntry.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnStageChange_Click
    protected void btnStageChange_Click(object sender, EventArgs e)
    {
        checkRights(141);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/ADD/StageChange.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnLineChange_Click
    protected void btnLineChange_Click(object sender, EventArgs e)
    {
        checkRights(142);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/ADD/LineChange.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnCustRejInward_Click
    protected void btnCustRejInward_Click(object sender, EventArgs e)
    {
        checkRights(21);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewCustRej.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnCustRej_Click
    protected void btnCustRej_Click(object sender, EventArgs e)
    {
        checkRights(256);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnIntrRejCAPA_Click
    protected void btnIntrRejCAPA_Click(object sender, EventArgs e)
    {
        checkRights(257);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewInternalRejCAPA.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnInternalCAPA_click
    protected void btnInternalCAPA_click(object sender, EventArgs e)
    {
        checkRights(257);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewInternalCAPAReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #endregion

    #region REPORTS
    #region btnDailyIRN_click
    protected void btnDailyIRN_click(object sender, EventArgs e)
    {
        checkRights(127);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewDailyIRNReport.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnRejSummary_click
    protected void btnRejSummary_click(object sender, EventArgs e)
    {
        checkRights(130);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewRejectionSummary.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnRejDetails_click
    protected void btnRejDetails_click(object sender, EventArgs e)
    {
        checkRights(131);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewRejectiondetails.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnDefectWiseRej_click
    protected void btnDefectWiseRej_click(object sender, EventArgs e)
    {
        checkRights(132);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewDefectWiseRej.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnfoundryRejper_click
    protected void btnfoundryRejper_click(object sender, EventArgs e)
    {
        checkRights(133);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewfoundaryRejYearly.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnMecRejPerf
    protected void btnMecRejPerf(object sender, EventArgs e)
    {
        checkRights(134);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewMechiningRejYearly.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnDefectWiseFoundRej_click
    protected void btnDefectWiseFoundRej_click(object sender, EventArgs e)
    {
        checkRights(135);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewDefectWiseFoundryRejYearly.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnDefectWiseMechRej_click
    protected void btnDefectWiseMechRej_click(object sender, EventArgs e)
    {
        checkRights(136);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewDefectWiseMechiningRejYearly.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnDeptWise_click
    protected void btnDeptWise_click(object sender, EventArgs e)
    {
        checkRights(137);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewDeptWiseRej.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnRejPerf_click
    protected void btnRejPerf_click(object sender, EventArgs e)
    {
        checkRights(138);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/View8020.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
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

    #region btnStageWiseHistory_click
    protected void btnStageWiseHistory_click(object sender, EventArgs e)
    {
        checkRights(143);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewStageWiseHistory.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnStageWiseHistory_click

    #region btnLineWiseHistory_click
    protected void btnLineWiseHistory_click(object sender, EventArgs e)
    {
        checkRights(144);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewLineWiseHistory.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnLineWiseHistory_click

    #region btnComp_click
    protected void btnComp_click(object sender, EventArgs e)
    {
        checkRights(145);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewCompWiseReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnLineWiseHistory_click

    #region btnRejSummStage_click
    protected void btnRejSummStage_click(object sender, EventArgs e)
    {
        checkRights(146);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewRejSummaryStageWise.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnRejSummStage_click

    #region btnliEffectiveCAPA_click
    protected void btnliEffectiveCAPA_click(object sender, EventArgs e)
    {
        checkRights(259);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewIRNCustRejectionReport8D.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion
    #region btnProEntryReg_click
    protected void btnProEntryReg_click(object sender, EventArgs e)
    {
        checkRights(126);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/IRN/VIEW/ViewProdEntryReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #endregion
}
