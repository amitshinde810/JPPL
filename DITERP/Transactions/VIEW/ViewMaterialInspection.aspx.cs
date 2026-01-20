using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_VIEW_ViewMaterialInspection : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='40'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    if (string.IsNullOrEmpty((string)Session["InspType"]))
                    {
                        Session["InspType"] = "Pending";
                    }
                    if (Session["InspType"] == "Pending")
                    {
                        ddlType.SelectedIndex = 0;
                    }
                    else if (Session["InspType"] == "Hold")
                    {
                        ddlType.SelectedIndex = 2;
                    }
                    else
                    {
                        ddlType.SelectedIndex = 1;
                    }
                    dgInspection.Enabled = false;
                    txtFormDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                    txtFormDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                    if (dgInspection.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TYPE", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgInspection.DataSource = dtFilter;
                            dgInspection.DataBind();
                        }
                        else
                        {
                            dgInspection.Enabled = true;
                        }
                    }
                    LoadInspection();
                    FillCombo();
                    ddlSupplierName.Enabled = false;
                    ddlGinNo.Enabled = false;
                    txtFormDate.Enabled = false;
                    txtToDate.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlType.SelectedValue == "0")
            {
                Session["InspType"] = "Pending";
            }
            else if (ddlType.SelectedValue == "1")
            {
                Session["InspType"] = "Inspected";
            }
            else if (ddlType.SelectedValue == "2")
            {
                Session["InspType"] = "Hold";
            }
            LoadInspection();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "btnShow_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Event

    #region GridEvent
    #region dgInspection_RowCommand
    protected void dgInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        int index = Convert.ToInt32(e.CommandArgument.ToString());
                        string code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                        string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_NO"))).Text;
                        string P_Type = ((Label)(dgInspection.Rows[index].FindControl("lblP_TYPE1"))).Text;
                        string IWM_Type = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_TYPE"))).Text;
                        if ((ddlType.SelectedItem.ToString() == "Pending"))
                        {
                            type = "ADD";
                        }
                        Response.Redirect("~/Transactions/ADD/MaterialInspection.aspx?c_name=" + type + "&cpom_code=" + code + "&PType=" + P_Type + "&IwmType=" + IWM_Type + "", false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    return;
                }
            }
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    int index = Convert.ToInt32(e.CommandArgument.ToString());
                    string code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                    if (CommonClasses.CheckUsedInTran("BILL_PASSING_DETAIL", "BPD_IWM_CODE", "AND ES_DELETE=0", code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record Because it is used in Bill Passing";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (!ModifyLog(code))
                    {
                        string type = "MODIFY";

                        string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_NO"))).Text;
                        string P_Type = ((Label)(dgInspection.Rows[index].FindControl("lblP_TYPE1"))).Text;
                        string IWM_Type = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_TYPE"))).Text;
                        if ((ddlType.SelectedItem.ToString() == "Pending"))
                        {
                            type = "ADD";
                        }
                        if ((ddlType.SelectedItem.ToString() == "Hold"))
                        {
                            type = "ADDHOLD";
                        }
                        Response.Redirect("~/Transactions/ADD/MaterialInspection.aspx?c_name=" + type + "&cpom_code=" + code + "&PType=" + P_Type + "&IwmType=" + IWM_Type + "", false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Add/Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion Modify
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInspection_PageIndexChanging
    protected void dgInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInspection.PageIndex = e.NewPageIndex;
            LoadInspection();
        }
        catch (Exception)
        {
        }
    }
    #endregion
    #endregion

    #region chkSupplierAll_CheckedChanged
    protected void chkSupplierAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSupplierAll.Checked == true)
            {
                ddlSupplierName.Enabled = false;
            }
            else
            {
                ddlSupplierName.Enabled = true;
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "chkSupplierAll_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region chkGinNoAll_CheckedChanged
    protected void chkGinNoAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkGinNoAll.Checked == true)
            {
                ddlGinNo.Enabled = false;
            }
            else
            {
                ddlGinNo.Enabled = true;
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "chkGinNoAll_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region chkFormToAll_CheckedChanged
    protected void chkFormToAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkFormToAll.Checked == true)
            {
                txtFormDate.Enabled = false;
                txtToDate.Enabled = false;
            }
            else
            {
                txtFormDate.Enabled = true;
                txtToDate.Enabled = true;
                txtFormDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "chkFormToAll_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion
    #endregion

    #region UserDefiendMethod
    #region FillCombo
    private void FillCombo()
    {
        try
        {
            DataTable DtParty = new DataTable();
            //DtParty = CommonClasses.Execute("(SELECT DISTINCT P_CODE,P_NAME FROM  INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL,SUPPLIER_TYPE_MASTER where INWARD_MASTER.ES_DELETE=0   AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_P_CODE=P_CODE and P_ACTIVE_IND=1 and   P_CM_COMP_ID=" + (string)Session["CompanyId"] + "   UNION SELECT DISTINCT P_CODE,P_NAME FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0   AND CR_P_CODE=P_CODE and P_ACTIVE_IND=1 and   P_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ) ORDER BY P_NAME");
            DtParty = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME into #Temp FROM INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL,SUPPLIER_TYPE_MASTER where INWARD_MASTER.ES_DELETE=0 AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_P_CODE=P_CODE and P_ACTIVE_IND=1 and P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and IWM_CM_CODE='" + Session["CompanyCode"] + "' SELECT DISTINCT P_CODE,P_NAME into #Temp1 FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and P_ACTIVE_IND=1 and P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and CR_CM_CODE='" + Session["CompanyCode"] + "' select * from #Temp union select * from #Temp1 order by P_Name drop table #Temp,#Temp1");
            ddlSupplierName.DataSource = DtParty;
            ddlSupplierName.DataTextField = "P_NAME";
            ddlSupplierName.DataValueField = "P_CODE";
            ddlSupplierName.DataBind();
            ddlSupplierName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
            DataTable DtGIN = new DataTable();
            DtGIN = CommonClasses.Execute("  (SELECT DISTINCT IWM_CODE, CASE when IWM_TYPE='IWCP' THEN 'CASH Purchase' when IWM_TYPE='IWIM' THEN 'Raw Material Inward'when IWM_TYPE='OUTCUSTINV' THEN 'Sub Contractor Inward' when IWM_TYPE='IWIFP' THEN 'FOR Process Material Inward' END   +' - ' +CONVERT(VARCHAR,IWM_NO) AS IWM_NO FROM  INWARD_MASTER,INWARD_DETAIL where INWARD_MASTER.ES_DELETE=0   AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_CM_CODE='" + Session["CompanyCode"] + "' UNION SELECT DISTINCT CR_CODE AS IWM_CODE ,'Customer Rejection'+' - '+CONVERT(VARCHAR,CR_GIN_NO) AS  IWM_NO FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 and CR_CM_CODE='" + Session["CompanyCode"] + "'  )  ");
            ddlGinNo.DataSource = DtGIN;
            ddlGinNo.DataTextField = "IWM_NO";
            ddlGinNo.DataValueField = "IWM_CODE";
            ddlGinNo.DataBind();
            ddlGinNo.Items.Insert(0, new ListItem("Select Inward No ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-View", "FillCombo", Ex.Message);
        }
    }
    #endregion

    #region LoadInspection
    private void LoadInspection()
    {
        try
        {
            DataTable dt = new DataTable();
            string From = "";
            string To = "";
            From = txtFormDate.Text;
            To = txtToDate.Text;

            #region Datecondition
            if (chkFormToAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFormDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }
            #endregion

            #region Pending
            if (Session["InspType"] == "Pending")
            {
                string Query = "SELECT * INTO #temp FROM ( select distinct P_CODE, IWM_CODE,IWM_TYPE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME , CASE when IWM_TYPE='IWCP' THEN 'CASH Purchase' when IWM_TYPE='IWIM' THEN 'Raw Material Inward'when IWM_TYPE='OUTCUSTINV' THEN 'Sub Contractor Inward' when IWM_TYPE='IWIFP' THEN 'FOR Process Inward'  END  AS P_TYPE, 1 AS TYPE from INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL,SUPPLIER_TYPE_MASTER where INWARD_MASTER.ES_DELETE=0   AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_P_CODE=P_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and     IWD_INSP_FLG=0  AND ISNULL(IWD_HOLD,0)=0   UNION SELECT DISTINCT P_CODE, CR_CODE AS IWM_CODE ,'Customer Rejection'  AS  IWM_TYPE , CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CR_CHALLAN_NO AS IWM_CHALLAN_NO,convert(varchar,CR_CHALLAN_DATE,106) as IWM_CHAL_DATE, P_NAME,'Customer Rejection' AS P_TYPE,0 AS TYPE FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_INSP_FLG=0 AND P_CODE=CR_P_CODE  and  CR_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ) AS A  select * FROM  #temp  where TYPE IN (0,1) ";
                if (chkSupplierAll.Checked)
                {
                }
                else
                {
                    if (ddlSupplierName.SelectedIndex != 0)
                    {
                        Query = Query + "and P_CODE='" + ddlSupplierName.SelectedValue + "'";
                    }
                }
                if (chkFormToAll.Checked)
                {
                    Query = Query + " and CONVERT(date, IWM_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
                }
                else
                {
                    Query = Query + " and CONVERT(date, IWM_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
                }
                if (chkGinNoAll.Checked)
                {
                }
                else
                {
                    if (ddlGinNo.SelectedIndex != 0)
                    {
                        Query = Query + "and IWM_CODE='" + ddlGinNo.SelectedValue.ToString() + "'";
                    }
                }
                Query = Query + " order by IWM_NO DESC DROP TABLE #temp ";
                dt = CommonClasses.Execute(Query);
                dgInspection.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    dgInspection.Enabled = false;
                    dt.Rows.Add(dt.NewRow());
                }
                else
                {
                    dgInspection.Enabled = true;
                }
                dgInspection.DataBind();
                dgInspection.Columns[0].Visible = false;
                dgInspection.Columns[1].Visible = false;
                dgInspection.Columns[2].Visible = true;
            }
            #endregion

            #region hold
            else if (Session["InspType"] == "Hold")
            {
                string Query = "SELECT * INTO #temp FROM ( select distinct P_CODE, IWM_CODE,IWM_TYPE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME , CASE when IWM_TYPE='IWCP' THEN 'CASH Purchase' when IWM_TYPE='IWIM' THEN 'Raw Material Inward'when IWM_TYPE='OUTCUSTINV' THEN 'Sub Contractor Inward' when IWM_TYPE='IWIFP' THEN 'FOR Process Inward'  END  AS P_TYPE, 1 AS TYPE from INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL,SUPPLIER_TYPE_MASTER where INWARD_MASTER.ES_DELETE=0   AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_P_CODE=P_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and     IWD_INSP_FLG=0  AND ISNULL(IWD_HOLD,0)=1  ) AS A  select * FROM  #temp  where TYPE IN (0,1) ";
                if (chkSupplierAll.Checked)
                {
                }
                else
                {
                    if (ddlSupplierName.SelectedIndex != 0)
                    {
                        Query = Query + "and P_CODE='" + ddlSupplierName.SelectedValue + "'";
                    }
                }
                if (chkFormToAll.Checked)
                {
                    Query = Query + " and CONVERT(date, IWM_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
                }
                else
                {
                    Query = Query + " and CONVERT(date, IWM_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
                }
                if (chkGinNoAll.Checked)
                {
                }
                else
                {
                    if (ddlGinNo.SelectedIndex != 0)
                    {
                        Query = Query + "and IWM_CODE='" + ddlGinNo.SelectedValue.ToString() + "'";
                    }
                }
                Query = Query + " order by IWM_NO DESC DROP TABLE #temp ";
                dt = CommonClasses.Execute(Query);
                dgInspection.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    dgInspection.Enabled = false;
                    dt.Rows.Add(dt.NewRow());
                }
                else
                {
                    dgInspection.Enabled = true;
                }
                dgInspection.DataBind();
                dgInspection.Columns[0].Visible = false;
                dgInspection.Columns[1].Visible = false;
                dgInspection.Columns[2].Visible = true;
            }
            #endregion

            #region  Inscepted
            else
            {
                string Query = "SELECT * INTO #temp FROM ( select distinct P_CODE, IWM_CODE,IWM_TYPE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME , CASE when IWM_TYPE='IWCP' THEN 'CASH Purchase' when IWM_TYPE='IWIM' THEN 'Raw Material Inward'when IWM_TYPE='OUTCUSTINV' THEN 'Sub Contractor Inward' when IWM_TYPE='IWIFP' THEN 'FOR Process Inward'  END  AS P_TYPE, 1 AS TYPE from INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL,SUPPLIER_TYPE_MASTER where INWARD_MASTER.ES_DELETE=0   AND INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWM_P_CODE=P_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and     IWD_INSP_FLG=1  UNION SELECT DISTINCT P_CODE, CR_CODE AS IWM_CODE ,'Customer Rejection'  AS  IWM_TYPE , CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CR_CHALLAN_NO AS IWM_CHALLAN_NO,convert(varchar,CR_CHALLAN_DATE,106) as IWM_CHAL_DATE, P_NAME,'Customer Rejection'  AS  P_TYPE,0 AS TYPE FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0   AND CD_INSP_FLG=1 AND P_CODE=CR_P_CODE  and  CR_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ) AS A  select * FROM  #temp  where TYPE IN (0,1) ";
                if (chkSupplierAll.Checked)
                {
                }
                else
                {
                    if (ddlSupplierName.SelectedIndex != 0)
                    {
                        Query = Query + "and P_CODE='" + ddlSupplierName.SelectedValue + "'";
                    }
                }
                if (chkFormToAll.Checked)
                {
                    Query = Query + " and CAST(IWM_DATE  as datetime) between '" + Convert.ToDateTime(From).ToString("dd MMM yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd MMM yyyy") + "'";
                }
                else
                {
                    Query = Query + " and CAST(IWM_DATE  as datetime) between '" + Convert.ToDateTime(From).ToString("dd MMM yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd MMM yyyy") + "'";
                }
                if (chkGinNoAll.Checked)
                {
                }
                else
                {
                    if (ddlGinNo.SelectedIndex != 0)
                    {
                        Query = Query + "and IWM_CODE='" + ddlGinNo.SelectedValue.ToString() + "'";
                    }
                }
                Query = Query + " order by IWM_NO DESC DROP TABLE #temp ";
                dt = CommonClasses.Execute(Query);
                if (dt.Rows.Count == 0)
                {
                    dgInspection.Enabled = false;
                    dt.Rows.Add(dt.NewRow());
                }
                else
                {
                    dgInspection.Enabled = true;
                }
                dgInspection.Columns[2].Visible = false;
                dgInspection.Columns[0].Visible = true;
                dgInspection.Columns[1].Visible = true;
                dgInspection.DataSource = dt;
                dgInspection.DataBind();
            }
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-View", "LoadInspection", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from INWARD_MASTER where IWM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection -View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion
    #endregion
}
