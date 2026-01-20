using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_StoreToStoreRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool ChkAllitem = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            string i_name = Request.QueryString[5].ToString();
            string group = Request.QueryString[6].ToString();
            string way = Request.QueryString[7].ToString();
            string StrCond = Request.QueryString[8].ToString();
            string PTYPE = Request.QueryString[9].ToString();
            i_name = i_name.Replace("'", "''");

            #region Detail
            if (way == "Direct")
            {
                if (group == "Datewise")
                {
                }
                if (group == "ItemWise")
                {
                }
            }
            #endregion

            #region Summary
            if (way == "AsperReq")
            {
                if (group == "Datewise")
                {
                }
                if (group == "ItemWise")
                {
                }
            }
            #endregion

            GenerateReport(Title, From, To, group, way, StrCond, PTYPE);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string strcond, string PTYPE)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (way == "Direct")
            {
                Query = "SELECT ISSUE_MASTER.IM_CODE ,IM_NO ,CONVERT(VARCHAR(17),IM_DATE,113) AS IM_DATE ,IM_TYPE , IMD_I_CODE, IMD_REC_QTY, IMD_SHORT_QTY , CONVERT(VARCHAR(17),IMD_REC_DATE,113) AS IMD_REC_DATE , IMD_REQ_QTY, IMD_ISSUE_QTY ,I2.I_NAME ,I2.I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME ,STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME], STORE_MASTER1.STORE_NAME AS [TO_STORE_NAME]  FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER , STORE_MASTER , STORE_MASTER AS STORE_MASTER1  WHERE " + strcond + " ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE   AND IM_UM_CODE = UM_CODE AND ISSUE_MASTER.IM_FROM_STORE = STORE_MASTER.STORE_CODE AND STORE_MASTER1.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE AND STORE_MASTER.ES_DELETE = '0' AND STORE_MASTER1.ES_DELETE = '0'  ORDER BY I_CODENO";
            }
            else
            {
                Query = "  SELECT distinct(I_CODENO) as I_CODENO,ITEM_MASTER.I_CODE,ISSUE_MASTER.IM_CODE,IM_MATERIAL_REQ,IM_NO,CONVERT(VARCHAR(17),IM_DATE,113) AS   IM_DATE,(case when IM_TYPE=1 then 'As Per Material Req' else 'Direct' end) as IM_TYPE  ,IM_ISSUEBY,IM_REQBY,isnull(MR_BATCH_NO,'') as MR_BATCH_NO ,I_NAME,I_UOM_NAME,isnull(I_CURRENT_BAL,0.00)as I_CURRENT_BAL,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK FROM  MATERIAL_REQUISITION_MASTER , ISSUE_MASTER INNER JOIN ISSUE_MASTER_DETAIL INNER JOIN ITEM_MASTER ON ISSUE_MASTER_DETAIL.IMD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ISSUE_MASTER_DETAIL.IMD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE ON  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE    where " + strcond + " ISSUE_MASTER.ES_DELETE=0 and IM_COMP_ID=" + Session["CompanyCode"] + " and I_CODE=IMD_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and ISSUE_MASTER_DETAIL.ES_DELETE=0 and MATERIAL_REQUISITION_MASTER.MR_CODE = ISSUE_MASTER.IM_MATERIAL_REQ ";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/StoreToStoreRegisterDatewise.rpt"));  //StoreToStoreRegisterDatewise
                    rptname.FileName = Server.MapPath("~/Reports/StoreToStoreRegisterDatewise.rpt"); //rptIssueToProductionDateWise
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtType", PTYPE);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                else if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/StoreToStoreRegisterItemwise.rpt"));  //rptIssueToProductionItemWise
                    rptname.FileName = Server.MapPath("~/Reports/StoreToStoreRegisterItemwise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtType", PTYPE);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                else if (group == "Storewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/StoreToStoreRegisterStorewise.rpt"));  //rptIssueToProductionItemWise
                    rptname.FileName = Server.MapPath("~/Reports/StoreToStoreRegisterStorewise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtType", PTYPE);
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStoreToStoreRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Store To Store Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

}
