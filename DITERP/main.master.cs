using System;
using System.Data;

public partial class main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["Username"]) || string.IsNullOrEmpty((string)Session["UserCode"]) || string.IsNullOrEmpty((string)Session["UserActivityCode"]))
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                lblUserName.Text = Session["Username"].ToString();
                DataTable dt = CommonClasses.Execute("select UM_NAME from USER_MASTER where UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "'");
                if (dt.Rows.Count > 0)
                {
                    lblUserName.Text = dt.Rows[0]["UM_NAME"].ToString();
                }
                #region Hiding Module Menus

                string masters = "";
                DataTable dtmaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 72 + "");
                masters = dtmaster.Rows.Count == 0 ? "0000000" : dtmaster.Rows[0][0].ToString();
                if (masters == "0000000" || masters == "0111111")
                {
                    Masters.Visible = false;
                    Masters1MV.Visible = false;
                }

                string Purchase1 = "";
                DataTable dtPurchase = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 73 + "");
                Purchase1 = dtPurchase.Rows.Count == 0 ? "0000000" : dtPurchase.Rows[0][0].ToString();
                if (Purchase1 == "0000000" || Purchase1 == "0111111")
                {
                    Purchase.Visible = false;
                    Purchase1MV.Visible = false;
                }

                string Production1 = "";
                DataTable dtProduction = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 74 + "");
                Production1 = dtProduction.Rows.Count == 0 ? "0000000" : dtProduction.Rows[0][0].ToString();
                if (Production1 == "0000000" || Production1 == "0111111")
                {
                    Production.Visible = false;
                    Production1MV.Visible = false;
                }

                string Sales = "";
                DataTable dtSalses = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 75 + "");
                Sales = dtSalses.Rows.Count == 0 ? "0000000" : dtSalses.Rows[0][0].ToString();
                if (Sales == "0000000" || Sales == "0111111")
                {
                    Sale.Visible = false;
                    Sale1MV.Visible = false;
                }

                string RND = "";
                DataTable dtRnd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 99 + "");
                RND = dtRnd.Rows.Count == 0 ? "0000000" : dtRnd.Rows[0][0].ToString();
                if (RND == "0000000" || RND == "0111111")
                {
                    RNDQC.Visible = false;
                }

                string ExciseString = "";
                DataTable dtExcise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 106 + "");
                ExciseString = dtExcise.Rows.Count == 0 ? "0000000" : dtExcise.Rows[0][0].ToString();
                if (ExciseString == "0000000" || ExciseString == "0111111")
                {
                    Excise.Visible = false;
                    Excise1MV.Visible = false;
                }
                else
                {
                    Excise.Visible = true;
                    Excise1MV.Visible = true;
                }

                string Utility1 = "";
                DataTable dtUtility = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 76 + "");
                Utility1 = dtUtility.Rows.Count == 0 ? "0000000" : dtUtility.Rows[0][0].ToString();
                if (Utility1 == "0000000" || Utility1 == "0111111")
                {
                    Utility.Visible = false;
                    Utility1MV.Visible = false;
                }

                string Admin = "";
                DataTable dtAdmin = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 77 + "");
                Admin = dtAdmin.Rows.Count == 0 ? "0000000" : dtAdmin.Rows[0][0].ToString();
                if (Admin == "0000000" || Admin == "0111111")
                {
                    Adminstrator.Visible = false;
                }

                string IRNM = "";
                DataTable dtIRNM = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 125 + "");
                IRNM = dtIRNM.Rows.Count == 0 ? "0000000" : dtIRNM.Rows[0][0].ToString();
                if (IRNM == "0000000" || IRNM == "0111111")
                {
                    IRN.Visible = false;
                    IRN1MV.Visible = false;
                }

                string COPPs = "";
                DataTable dtCOPP = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 164 + "");
                COPPs = dtCOPP.Rows.Count == 0 ? "0000000" : dtCOPP.Rows[0][0].ToString();
                if (COPPs == "0000000" || COPPs == "0111111")
                {
                    COPP.Visible = false;
                    COPPMV.Visible = false;//Tools
                }

                string tool = "";
                DataTable dttool = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 166 + "");
                tool = dttool.Rows.Count == 0 ? "0000000" : dttool.Rows[0][0].ToString();
                if (tool == "0000000" || tool == "0111111")
                {
                    Tools.Visible = false;
                    ToolsMV.Visible = false;//Tools
                }
                string ppcm = "";
                DataTable dtppcm = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 179 + "");
                ppcm = dtppcm.Rows.Count == 0 ? "0000000" : dtppcm.Rows[0][0].ToString();
                if (ppcm == "0000000" || ppcm == "0111111")
                {
                    PPC.Visible = false;
                    PPCMV.Visible = false;//Tools
                }
                #endregion
            }
        }
    }
    protected void lnk_logout(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/Default.aspx", true);
    }
}
