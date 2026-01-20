using System;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_ProductionDefault : System.Web.UI.Page
{
    #region Variable
    string right = "";
    #endregion

    #region Event
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
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
                    string bom = "";
                    DataTable dtbom = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 44 + "");
                    bom = dtbom.Rows.Count == 0 ? "0000000" : dtbom.Rows[0][0].ToString();
                    if (bom == "0000000" || bom == "0111111")
                    {
                        //  BOM.Visible = false;
                    }
                    string matreq = "";
                    DataTable dtmatreq = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 50 + "");
                    matreq = dtmatreq.Rows.Count == 0 ? "0000000" : dtmatreq.Rows[0][0].ToString();
                    if (matreq == "0000000" || matreq == "0111111")
                    {
                        MatReq.Visible = false;
                    }

                    string fillofsheet = "";
                    DataTable dtfill = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 101 + "");
                    fillofsheet = dtfill.Rows.Count == 0 ? "0000000" : dtfill.Rows[0][0].ToString();
                    if (fillofsheet == "0000000" || fillofsheet == "0111111")
                    {
                        FillOffSheet.Visible = false;
                    }

                    string issueprod = "";
                    DataTable dtissueprod = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 51 + "");
                    issueprod = dtissueprod.Rows.Count == 0 ? "0000000" : dtissueprod.Rows[0][0].ToString();
                    if (issueprod == "0000000" || issueprod == "0111111")
                    {
                        IssueProd.Visible = false;
                    }

                    string issueprodA = "";
                    DataTable dtissueprodA = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 149 + "");
                    issueprodA = dtissueprodA.Rows.Count == 0 ? "0000000" : dtissueprodA.Rows[0][0].ToString();
                    if (issueprodA == "0000000" || issueprodA == "0111111")
                    {
                        IssueProdA.Visible = false;
                        main2main.Visible = false;
                    }
                    string issuefillofsheet = "";
                    DataTable dtissuefill = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 105 + "");
                    issuefillofsheet = dtissuefill.Rows.Count == 0 ? "0000000" : dtissuefill.Rows[0][0].ToString();
                    if (issuefillofsheet == "0000000" || issuefillofsheet == "0111111")
                    {
                        IssueFillOffSheet.Visible = false;
                    }

                    string prodstore = "";
                    DataTable dtprodstore = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 52 + "");
                    prodstore = dtprodstore.Rows.Count == 0 ? "0000000" : dtprodstore.Rows[0][0].ToString();
                    if (prodstore == "0000000" || prodstore == "0111111")
                    {
                        ProdStore.Visible = false;
                    }

                    string stockadj = "";
                    DataTable dtstockadj = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 60 + "");
                    stockadj = dtstockadj.Rows.Count == 0 ? "0000000" : dtstockadj.Rows[0][0].ToString();
                    if (stockadj == "0000000" || stockadj == "0111111")
                    {
                        StockAdj.Visible = false;
                    }

                    string LiMaterialAcpt = "";
                    DataTable dtLiMaterialAcpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 148 + "");
                    LiMaterialAcpt = dtLiMaterialAcpt.Rows.Count == 0 ? "0000000" : dtLiMaterialAcpt.Rows[0][0].ToString();
                    if (LiMaterialAcpt == "0000000" || LiMaterialAcpt == "0111111")
                    {
                        LiMaterialAcceptance.Visible = false;
                    }

                    string bomreg = "";
                    DataTable dtbomreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 45 + "");
                    bomreg = dtbomreg.Rows.Count == 0 ? "0000000" : dtbomreg.Rows[0][0].ToString();
                    if (bomreg == "0000000" || bomreg == "0111111")
                    {
                        BOMReg.Visible = false;
                    }

                    string matreqreg = "";
                    DataTable dtmatreqreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 55 + "");
                    matreqreg = dtmatreqreg.Rows.Count == 0 ? "0000000" : dtmatreqreg.Rows[0][0].ToString();
                    if (matreqreg == "0000000" || matreqreg == "0111111")
                    {
                        MatReqReg.Visible = false;
                    }

                    string issueprodreg = "";
                    DataTable dtissueprodreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 56 + "");
                    issueprodreg = dtissueprodreg.Rows.Count == 0 ? "0000000" : dtissueprodreg.Rows[0][0].ToString();
                    if (issueprodreg == "0000000" || issueprodreg == "0111111")
                    {
                        IssueProdReg.Visible = false;
                    }

                    string prodstorereg = "";
                    DataTable dtprodstorereg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 57 + "");
                    prodstorereg = dtprodstorereg.Rows.Count == 0 ? "0000000" : dtprodstorereg.Rows[0][0].ToString();
                    if (prodstorereg == "0000000" || prodstorereg == "0111111")
                    {
                        ProdReg.Visible = false;
                    }

                    string matreqmis = "";
                    DataTable dtmatreqmis = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 62 + "");
                    matreqmis = dtmatreqmis.Rows.Count == 0 ? "0000000" : dtmatreqmis.Rows[0][0].ToString();
                    if (matreqmis == "0000000" || matreqmis == "0111111")
                    {
                        MatreqMisReg.Visible = false;
                    }

                    string stockadjreg = "";
                    DataTable dtstockadjreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 61 + "");
                    stockadjreg = dtstockadjreg.Rows.Count == 0 ? "0000000" : dtstockadjreg.Rows[0][0].ToString();
                    if (stockadjreg == "0000000" || stockadjreg == "0111111")
                    {
                        StockAsjreg.Visible = false;
                    }

                    string custrej = "";
                    DataTable dtcustrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 21 + "");
                    custrej = dtcustrej.Rows.Count == 0 ? "0000000" : dtcustrej.Rows[0][0].ToString();
                    if (custrej == "0000000" || custrej == "0111111")
                    {
                        CustRej.Visible = false;
                    }

                    string custrejreg = "";
                    DataTable dtcustrejreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 53 + "");
                    custrejreg = dtcustrejreg.Rows.Count == 0 ? "0000000" : dtcustrejreg.Rows[0][0].ToString();
                    if (custrejreg == "0000000" || custrejreg == "0111111")
                    {
                        CustRejReg.Visible = false;
                    }

                    string matinwdreg = "";
                    DataTable dtmatinwdreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 47 + "");
                    matinwdreg = dtmatinwdreg.Rows.Count == 0 ? "0000000" : dtmatinwdreg.Rows[0][0].ToString();
                    if (matinwdreg == "0000000" || matinwdreg == "0111111")
                    {
                        InwdReg.Visible = false;
                    }

                    string inspreg = "";
                    DataTable dtinspreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 48 + "");
                    inspreg = dtinspreg.Rows.Count == 0 ? "0000000" : dtinspreg.Rows[0][0].ToString();
                    if (inspreg == "0000000" || inspreg == "0111111")
                    {
                        Inspreg.Visible = false;
                    }


                    string turningRegister = "";
                    DataTable dtturningRegister = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 114 + "");
                    turningRegister = dtturningRegister.Rows.Count == 0 ? "0000000" : dtturningRegister.Rows[0][0].ToString();
                    if (turningRegister == "0000000" || turningRegister == "0111111")
                    {
                        TurningRegister.Visible = false;
                    }
                    string StockReport = "";
                    DataTable dtStockReport = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 152 + "");
                    StockReport = dtStockReport.Rows.Count == 0 ? "0000000" : dtStockReport.Rows[0][0].ToString();
                    if (StockReport == "0000000" || StockReport == "0111111")
                    {
                        LiStockReport.Visible = false;
                    }
                    string StoreRegister = "";
                    DataTable dtStoreRegister = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 154 + "");
                    StoreRegister = dtStoreRegister.Rows.Count == 0 ? "0000000" : dtStoreRegister.Rows[0][0].ToString();
                    if (StoreRegister == "0000000" || StoreRegister == "0111111")
                    {
                        LiStoreRegister.Visible = false;
                    }
                    string turIWD = "";
                    DataTable dtturIWD = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 115 + "");
                    turIWD = dtturIWD.Rows.Count == 0 ? "0000000" : dtturIWD.Rows[0][0].ToString();
                    if (turIWD == "0000000" || turIWD == "0111111")
                    {
                        TurIWD.Visible = false;
                    }
                    string StockCompate = "";
                    DataTable dtStockCompate = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 151 + "");
                    StockCompate = dtStockCompate.Rows.Count == 0 ? "0000000" : dtStockCompate.Rows[0][0].ToString();
                    if (StockCompate == "0000000" || StockCompate == "0111111")
                    {
                        StockCompateT.Visible = false;
                    }
                    string RejConv = "";
                    DataTable dtRejConv = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 150 + "");
                    RejConv = dtRejConv.Rows.Count == 0 ? "0000000" : dtRejConv.Rows[0][0].ToString();
                    if (RejConv == "0000000" || RejConv == "0111111")
                    {
                        RejConvT.Visible = false;
                    }
                    string StockError = "";
                    DataTable dtStockError = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 155 + "");
                    StockError = dtStockError.Rows.Count == 0 ? "0000000" : dtStockError.Rows[0][0].ToString();
                    if (StockError == "0000000" || StockError == "0111111")
                    {
                        StockErrorT.Visible = false;
                    }
                    string StockTrans = "";
                    DataTable dtStockTran = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 161 + "");
                    StockTrans = dtStockTran.Rows.Count == 0 ? "0000000" : dtStockTran.Rows[0][0].ToString();
                    if (StockTrans == "0000000" || StockTrans == "0111111")
                    {
                        StockTran.Visible = false;
                    }

                    string StockTR = "";
                    DataTable dtStockTR = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 162 + "");
                    StockTR = dtStockTR.Rows.Count == 0 ? "0000000" : dtStockTR.Rows[0][0].ToString();
                    if (StockTR == "0000000" || StockTR == "0111111")
                    {
                        StockTranReg.Visible = false;
                    }
                    string stklvlRpts = "";
                    DataTable dtstklvlRpts = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 91 + "");
                    stklvlRpts = dtstklvlRpts.Rows.Count == 0 ? "0000000" : dtstklvlRpts.Rows[0][0].ToString();
                    if (stklvlRpts == "0000000" || stklvlRpts == "0111111")
                    {
                        StlLvlRpt.Visible = false;
                    }
                    string trunReg = "";
                    DataTable dttrunReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 114 + "");
                    trunReg = dttrunReg.Rows.Count == 0 ? "0000000" : dttrunReg.Rows[0][0].ToString();
                    if (trunReg == "0000000" || trunReg == "0111111")
                    {
                        trunreg.Visible = false;
                    }
                    string MaterialInsp = "";
                    DataTable dtMaterialInp = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 40 + "");
                    MaterialInsp = dtMaterialInp.Rows.Count == 0 ? "0000000" : dtMaterialInp.Rows[0][0].ToString();
                    if (MaterialInsp == "0000000" || MaterialInsp == "0111111")
                    {
                        Inspection.Visible = false;
                    }

                    string MatInwrd = "";
                    DataTable dtmatInrd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 39 + "");
                    MatInwrd = dtmatInrd.Rows.Count == 0 ? "0000000" : dtmatInrd.Rows[0][0].ToString();
                    if (MatInwrd == "0000000" || MatInwrd == "0111111")
                    {
                        Inwrd.Visible = false;
                        liSubconInwrd.Visible = false;
                        LiCashiwrd.Visible = false;
                        LiForProInwrd.Visible = false;
                    }

                    string DistToSubcon = "";
                    DataTable dtDistToSubCon = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 128 + "");
                    DistToSubcon = dtDistToSubCon.Rows.Count == 0 ? "0000000" : dtDistToSubCon.Rows[0][0].ToString();
                    if (DistToSubcon == "0000000" || DistToSubcon == "0111111")
                    {
                        liDistToSubCon.Visible = false;
                    }

                    string DistToSubconrpt = "";
                    DataTable dtDistToSubConrpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 129 + "");
                    DistToSubconrpt = dtDistToSubConrpt.Rows.Count == 0 ? "0000000" : dtDistToSubConrpt.Rows[0][0].ToString();
                    if (DistToSubconrpt == "0000000" || DistToSubconrpt == "0111111")
                    {
                        liDistToSubReg.Visible = false;
                    }

                    string StockReg = "";
                    DataTable dtStockReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 91 + "");
                    StockReg = dtStockReg.Rows.Count == 0 ? "0000000" : dtStockReg.Rows[0][0].ToString();
                    if (StockReg == "0000000" || StockReg == "0111111")
                    {
                        LiStockRegReport.Visible = false;
                    }

                    string DcRReg = "";
                    DataTable dtDcRReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 264 + "");
                    DcRReg = dtDcRReg.Rows.Count == 0 ? "0000000" : dtDcRReg.Rows[0][0].ToString();
                    if (DcRReg == "0000000" || DcRReg == "0111111")
                    {
                        liDcRReg.Visible = false;
                    }
                    string TDcRReg = "";
                    DataTable dtTDcRReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 265 + "");
                    TDcRReg = dtTDcRReg.Rows.Count == 0 ? "0000000" : dtTDcRReg.Rows[0][0].ToString();
                    if (TDcRReg == "0000000" || TDcRReg == "0111111")
                    {
                        LiTDCRReg.Visible = false;
                    }
                    string DCReturn = "";
                    DataTable dtDCReturn = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 262 + "");
                    DCReturn = dtDCReturn.Rows.Count == 0 ? "0000000" : dtDCReturn.Rows[0][0].ToString();
                    if (DCReturn == "0000000" || DCReturn == "0111111")
                    {
                        LiDCReturn.Visible = false;
                    }

                    string TDCReturn = "";
                    DataTable dtTDCReturn = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 263 + "");
                    TDCReturn = dtTDCReturn.Rows.Count == 0 ? "0000000" : dtTDCReturn.Rows[0][0].ToString();
                    if (TDCReturn == "0000000" || TDCReturn == "0111111")
                    {
                        LiTDCReturn.Visible = false;
                    }
                    string StcoRtpReg = "";
                    DataTable dtStcoRtpReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 91 + "");
                    StcoRtpReg = dtStcoRtpReg.Rows.Count == 0 ? "0000000" : dtStcoRtpReg.Rows[0][0].ToString();
                    if (StcoRtpReg == "0000000" || StcoRtpReg == "0111111")
                    {
                        LiTSReg.Visible = false;
                        LiTSMISRpt.Visible = false;
                        liVenSReg.Visible = false;
                        LiCustStockReg.Visible = false;
                    }

                    string SCastConReg = "";
                    DataTable dtSCastConReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 153 + "");
                    SCastConReg = dtSCastConReg.Rows.Count == 0 ? "0000000" : dtSCastConReg.Rows[0][0].ToString();
                    if (SCastConReg == "0000000" || SCastConReg == "0111111")
                    {
                        LiCastConReg.Visible = false;
                    }

                    string SIwrdMisRpt = "";
                    DataTable dtIwrdMisRpt = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 241 + "");
                    SIwrdMisRpt = dtIwrdMisRpt.Rows.Count == 0 ? "0000000" : dtIwrdMisRpt.Rows[0][0].ToString();
                    if (SIwrdMisRpt == "0000000" || SIwrdMisRpt == "0111111")
                    {
                        LiCastConReg.Visible = false;
                        InwMISRpt.Visible = false;
                    }
                    #endregion
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region Master
    protected void btnBOMMaster_click(object sender, EventArgs e)
    {
        checkRights(44);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region Transaction
    protected void btnMaterialRequisition_click(object sender, EventArgs e)
    {
        checkRights(50);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnFillOffSheet_click(object sender, EventArgs e)
    {
        checkRights(101);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewFillOffSheet.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnSubContractorInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSubContractorIssue.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnCashInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnForProcessInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnIssueFillOffSheet_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssueFillOffSheet.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnProductionToStore_click(object sender, EventArgs e)
    {
        checkRights(52);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnIssueToProduction_click(object sender, EventArgs e)
    {
        checkRights(51);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnmain2main_click(object sender, EventArgs e)
    {
        checkRights(149);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/mainStoreStockTransfer.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnIssueToProductionA_click(object sender, EventArgs e)
    {
        checkRights(149);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssue.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnStockAdjustment_click(object sender, EventArgs e)
    {
        checkRights(60);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustomerRejection_click(object sender, EventArgs e)
    {
        checkRights(21);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDCRetrun_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewDCReturn.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnTrayDCRetrun_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTrayDCReturn.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }



    #region btnStockAcceptance_click
    protected void btnMaterialAcceptance_click(object sender, EventArgs e)
    {
        checkRights(148);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialAcceptance.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnStockAcceptance_click

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

    protected void btnTurningIWD_click(object sender, EventArgs e)
    {
        checkRights(115);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnInwardSuppWise_click(object sender, EventArgs e)
    {
        checkRights(47);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewInwardSuppWise.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnInspectionRegisterReport_click(object sender, EventArgs e)
    {
        checkRights(48);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewInspectionRegisterReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDispatchToSub_click(object sender, EventArgs e)
    {
        checkRights(128);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnRejToFondryCov_click(object sender, EventArgs e)
    {
        checkRights(150);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/RejStoreToFoundryCon.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnUploadStock_click(object sender, EventArgs e)
    {
        checkRights(151);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/ADD/StockCompare.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnStockTran_click(object sender, EventArgs e)
    {
        try
        {
            checkRights(161);
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                Server.Transfer("~/Transactions/VIEW/ViewStockTransfer.aspx", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');</script>");
                ModalPopupMsg.Show();
                return;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Reports
    protected void btnBillOfMaterialRegister_Click(object sender, EventArgs e)
    {
        checkRights(45);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBillOfMaterialRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDispatchToSubReport_click(object sender, EventArgs e)
    {
        checkRights(129);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewDispatchtoSubcontractor.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMaterialRequisitionRegister_Click(object sender, EventArgs e)
    {
        checkRights(55);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMaterialRequisitionRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnIssueToProductionRegister_Click(object sender, EventArgs e)
    {
        checkRights(56);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewIssueToProductionRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnProdcutionToStoreRegister_Click(object sender, EventArgs e)
    {
        checkRights(57);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewProdcutionToStoreRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnMaterialRequisitionMISReport_Click(object sender, EventArgs e)
    {
        checkRights(62);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMaterialRequisition_MIS.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnStockAdjustmentRegister_Click(object sender, EventArgs e)
    {
        checkRights(61);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockAdjustmentRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnCustomerRejectionRegister_click(object sender, EventArgs e)
    {
        checkRights(53);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRejectionRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSubContStockRegister_click(object sender, EventArgs e)
    {
        checkRights(53);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubContStockRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedger.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnVendorStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubContStockRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTrayStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewtrayDCRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTrayStockMIS_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/TrayStockReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDCRetrunRegister_Click(object sender, EventArgs e)
    {
        checkRights(78);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DCReturnRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDCTRetrunRegister_Click(object sender, EventArgs e)
    {
        checkRights(78);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DCTReturnRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTurning_click(object sender, EventArgs e)
    {
        checkRights(114);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTurningRegisterReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnTurningReg_click(object sender, EventArgs e)
    {
        checkRights(114);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTurningReg.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStockError_click(object sender, EventArgs e)
    {
        checkRights(155);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockError.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnStockReport_click
    protected void btnStockReport_click(object sender, EventArgs e)
    {
        checkRights(152);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/StockReport.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnStockReport_click

    #region btnStoreRegister_click
    protected void btnStoreRegister_click(object sender, EventArgs e)
    {
        checkRights(154);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStoreToStoreRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnStoreRegister_click

    protected void btnStockTranReg_click(object sender, EventArgs e)
    {
        checkRights(162);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockTransferRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnInwMISRpt_click
    protected void btnInwMISRpt_click(object sender, EventArgs e)
    {
        checkRights(241);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewInwardMIS.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnInwMISRpt_click

    protected void btnStockLevel_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLevel.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/CustomerStockRegister.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnRejToFound_click
    protected void btnRejToFound_click(object sender, EventArgs e)
    {
        checkRights(153);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/RejectionToFoundryConReg.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnRejToFound_click
    #endregion

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
}
