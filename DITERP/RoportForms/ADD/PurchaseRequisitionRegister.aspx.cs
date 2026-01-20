using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_PurchaseRequisitionRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
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

            i_name = i_name.Replace("'", "''");

            #region Detail
            if (way == "Direct")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
            }
            #endregion

            #region Summary
            if (way == "AsperReq")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
            }
            #endregion

            #region Detail
            if (way == "All")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
            }
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string item, string From, string To, string i_name, string group, string way)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            if (way == "Direct")
            {
                Query = "with t1 as ( select I_CODE,PRM_CODE,I_NAME, PRM_TYPE,PRM_DATE,PRM_DEPARTMENT, PRM_NO,PRUCHASE_REQUISITION_MASTER.ES_DELETE  from PURCHASE_REQUISION_DETAIL,PRUCHASE_REQUISITION_MASTER,ITEM_MASTER where PRD_PRM_CODE=PRM_CODE and PRM_I_CODE=I_CODE), t2 as(select PRD_PRM_CODE,PRD_I_CODE,I_NAME,PRD_REQ_QTY,I_CODENO from PURCHASE_REQUISION_DETAIL,PRUCHASE_REQUISITION_MASTER,ITEM_MASTER  where PRD_PRM_CODE=PRM_CODE and PRD_I_CODE=I_CODE  ) select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME  as SUB_NAME,t2.PRD_REQ_QTY as BD_VQTY,(case t1.PRM_TYPE when 2 then 'Direct' else 'As Per Material Req.' end) as MR_TYPE,t1.PRM_DATE as MR_DATE, t1.PRM_DEPARTMENT as MR_DEPT_NAME, t1.PRM_NO as MR_BATCH_NO,t2.I_CODENO from t1,t2 where t2.PRD_PRM_CODE=t1.PRM_CODE and t1.PRM_TYPE=2 and t1.ES_DELETE=0   ";
            }
            if (way == "AsperReq")
            {
                Query = "with t1 as ( select I_CODE,PRM_CODE,I_NAME, PRM_TYPE,PRM_DATE,PRM_DEPARTMENT, PRM_NO,PRUCHASE_REQUISITION_MASTER.ES_DELETE  from PURCHASE_REQUISION_DETAIL,PRUCHASE_REQUISITION_MASTER,ITEM_MASTER where PRD_PRM_CODE=PRM_CODE and PRM_I_CODE=I_CODE), t2 as(select PRD_PRM_CODE,PRD_I_CODE,I_NAME,PRD_REQ_QTY,I_CODENO from PURCHASE_REQUISION_DETAIL,PRUCHASE_REQUISITION_MASTER,ITEM_MASTER  where PRD_PRM_CODE=PRM_CODE and PRD_I_CODE=I_CODE  ) select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME  as SUB_NAME,t2.PRD_REQ_QTY as BD_VQTY,(case t1.PRM_TYPE when 2 then 'Direct' else 'As Per Material Req.' end) as MR_TYPE,t1.PRM_DATE as MR_DATE, t1.PRM_DEPARTMENT as MR_DEPT_NAME, t1.PRM_NO as MR_BATCH_NO,t2.I_CODENO from t1,t2 where t2.PRD_PRM_CODE=t1.PRM_CODE and t1.PRM_TYPE=1 and t1.ES_DELETE=0   ";
            }

            #region Detail
            if (way == "Direct")
            {
                #region Datewise
                if (group == "Datewise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and t1.I_CODE='" + i_name + "'";
                    }
                    Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.PRD_REQ_QTY,t1.PRM_TYPE,t1.PRM_DATE, t1.PRM_DEPARTMENT,t1.PRM_NO,t2.I_CODENO";
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and t1.I_CODE='" + i_name + "'";
                    }
                    Query = Query + "   group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.PRD_REQ_QTY,t1.PRM_TYPE,t1.PRM_DATE, t1.PRM_DEPARTMENT,t1.PRM_NO,t2.I_CODENO";
                }
                #endregion
            }
            #endregion

            #region AsPerReq
            if (way == "AsperReq")
            {
                #region Datewise
                if (group == "Datewise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and t1.I_CODE='" + i_name + "'";
                    }
                    Query = Query + "group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.PRD_REQ_QTY,t1.PRM_TYPE,t1.PRM_DATE, t1.PRM_DEPARTMENT,t1.PRM_NO,t2.I_CODENO";
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and t1.PRM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and t1.I_CODE='" + i_name + "'";
                    }
                    Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.PRD_REQ_QTY,t1.PRM_TYPE,t1.PRM_DATE, t1.PRM_DEPARTMENT,t1.PRM_NO,t2.I_CODENO";
                }
                #endregion
            }
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/purchaseRequisitionRegisterDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/purchaseRequisitionRegisterDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/PurchaseRequisitionRegisterItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/PurchaseRequisitionRegisterItemWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
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
            CommonClasses.SendError("Purchase Requisition  Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewPurchaseRequisitionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition  Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
