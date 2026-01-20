using System;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ToolRoom_ADD_MonthlyPlannedPreventiveMaintenance : System.Web.UI.Page
{
    #region Declartion
    static int mlCode = 0;
    static string right = "";
    int CValue;
    string fileName = "";
    string fileNameD = "";
    int SValue;
    #endregion

    #region Events

    #region Page_Load
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
                try
                {
                    ViewState["mlCode"] = mlCode;
                    txtMonth.Text = System.DateTime.Now.ToString("MMM/yyyy");
                    LoadToolNo();
                    LoadPartNo();
                    txtMonth.Attributes.Add("readonly", "readonly");

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }

                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Monthly Planned Preventive maintenance", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select PM_CODE,PM_TOOL_NO,PM_MONTH,PM_WEEK,PM_TYPE,PM_I_CODE FROM MP_PREVENTIVE_MAINTENANCE_MASTER where PM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND PM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["PM_CODE"]);
                LoadToolNo();
                ddlToolNo.SelectedValue = dt.Rows[0]["PM_TOOL_NO"].ToString();
                ddlType.SelectedValue = dt.Rows[0]["PM_TYPE"].ToString();
                ddlpart.SelectedValue = dt.Rows[0]["PM_I_CODE"].ToString();
                ddlWeek.SelectedValue = dt.Rows[0]["PM_WEEK"].ToString();
                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["PM_MONTH"]).ToString("MMM yyyy");

                if (str == "VIEW")
                {
                    ddlType.Enabled = false;
                    ddlpart.Enabled = false;
                    ddlToolNo.Enabled = false;
                    ddlWeek.Enabled = false;
                    txtMonth.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("MP_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "PM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        //initilize sql connection
        string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
        //object of sql transaction
        SqlTransaction trans;
        //initilize connection
        SqlConnection connection = new SqlConnection(strConnString);
        //open connection
        connection.Open();
        //start of sql trandaction
        trans = connection.BeginTransaction();

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                SqlCommand command = new SqlCommand("INSERT INTO MP_PREVENTIVE_MAINTENANCE_MASTER (PM_TOOL_NO,PM_MONTH,PM_WEEK,PM_TYPE,PM_I_CODE,PM_CM_COMP_ID)VALUES ('" + ddlToolNo.SelectedValue + "','" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "','" + ddlWeek.SelectedValue.ToString() + "','" + ddlType.SelectedValue.ToString() + "','" + ddlpart.SelectedValue.ToString() + "','" + Convert.ToInt32(Session["CompanyId"]) + "')", connection, trans);
                command.ExecuteNonQuery();

                string Code = "";
                SqlCommand cmd1 = new SqlCommand("Select Max(PM_CODE) from MP_PREVENTIVE_MAINTENANCE_MASTER", connection, trans);
                cmd1.Transaction = trans;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    Code = (dr1[0].ToString().Trim());
                }
                cmd1.Dispose();
                dr1.Dispose();

                trans.Commit();
                CommonClasses.WriteLog("Monthly Planned Preventive maintenance", "Save", "Monthly Planned Preventive maintenance", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/MonthlyPlannedPreventiveMaintenanceDIE.aspx", false);
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("UPDATE MP_PREVENTIVE_MAINTENANCE_MASTER SET PM_TOOL_NO='" + ddlToolNo.SelectedValue + "', PM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "', PM_WEEK='" + ddlWeek.SelectedValue + "', PM_TYPE='" + ddlType.SelectedValue + "', PM_I_CODE='" + ddlpart.SelectedValue + "' WHERE PM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                command.ExecuteNonQuery();
                trans.Commit();
                CommonClasses.RemoveModifyLock("MP_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "PM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                CommonClasses.WriteLog("Monthly Planned Preventive maintenance", "Update", "Monthly Planned Preventive maintenance", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/MonthlyPlannedPreventiveMaintenanceDIE.aspx", false);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region ddlToolNo_SelectedIndexChanged
    protected void ddlToolNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPartNo();
    }

    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("MP_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "PM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/ToolRoom/VIEW/MonthlyPlannedPreventiveMaintenanceDIE.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlToolNo.Text.Trim() == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion
    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {
            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion

    #region LoadToolNo
    private void LoadToolNo()
    {
        DataTable dt = new DataTable();
        if (Request.QueryString[0].Equals("INSERT"))
        {
            //dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0) order by T_NAME");
             dt = CommonClasses.Execute("select distinct T_CODE,I_CODE,T_NAME , (select ISNULL(SUM(isnull(IRND_PROD_QTY,0)),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) AS IRND_PROD_QTY, T_PENDTOOLLIFE,(ISNULL((SELECT ISNULL(SUM(isnull(TRR_STD_PROD,0)),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),0)) AS TRR_STD_PROD into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1)  SELECT T_CODE,T_NAME FROM #temp where IRND_PROD_QTY<(T_PENDTOOLLIFE+TRR_STD_PROD) union select T_CODE,T_NAME from TOOL_MASTER where  ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN (select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0)  order by T_NAME drop table #temp");

            //new per New logic of pending life calcualtion 6-12-18

            //dt = CommonClasses.Execute(" select distinct T_CODE,I_CODE,T_NAME ,T_PENDTOOLLIFE,  ISNULL((SELECT CONVERT(VARCHAR,MAX(TRR_DATE),106)  FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_T_CODE=T_CODE) ,'') AS T_REF_DATE  into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0   AND T_STATUS=1 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1)    SELECT  T_CODE,I_CODE,T_NAME,T_REF_DATE ,T_PENDTOOLLIFE, CASE WHEN T_REF_DATE='' then ISNULL((select ISNULL(SUM(isnull(IRND_PROD_QTY,0)),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE),0) ELSE ISNULL((select ISNULL(SUM(isnull(IRND_PROD_QTY,0)),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE AND IRN_DATE>=CONVERT(datetime,T_REF_DATE )),0) END  AS IRND_PROD_QTY, CASE WHEN T_REF_DATE='' then T_PENDTOOLLIFE ELSE  (ISNULL((SELECT ISNULL(SUM(isnull(TRR_STD_PROD,0)),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE AND TRR_DATE<=CONVERT(datetime,T_REF_DATE ) ),0)) END   AS TRR_STD_PROD   into #temp1   FROM #temp     SELECT T_CODE,T_NAME   FROM #temp1 where IRND_PROD_QTY<TRR_STD_PROD union select T_CODE,T_NAME from TOOL_MASTER where  ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN (select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0)  order by T_NAME drop table #temp drop table #temp1");
        }
        else
        {
            dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_STATUS=1 order by T_NAME");
        }
        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
        else
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
    }
    #endregion

    #region LoadPartNo
    private void LoadPartNo()
    {
        DataTable dt = new DataTable();
        string str = "";

        if (ddlToolNo.SelectedIndex != 0)
        {
            str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER where " + str + " I_CM_COMP_ID=" + Session["CompanyID"] + " and T_I_CODE=I_CODE and I_ACTIVE_IND=1 and I_CAT_CODE=-2147483648 AND T_STATUS=1 AND TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND  T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0)  order by I_NAME");
        }
        else
        {
            dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER where " + str + " I_CM_COMP_ID=" + Session["CompanyID"] + " and T_I_CODE=I_CODE and I_ACTIVE_IND=1 and I_CAT_CODE=-2147483648 AND T_STATUS=1 AND TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0  order by I_NAME");
        }
        if (dt.Rows.Count > 0)
        {
            ddlpart.DataSource = dt;
            ddlpart.DataTextField = "I_NAME";
            ddlpart.DataValueField = "I_CODE";
            ddlpart.DataBind();
            ddlpart.Items.Insert(0, new ListItem("Select Part No.", "0"));
            if (ddlToolNo.SelectedIndex != 0)
            {
                ddlpart.SelectedIndex = 1;
            }
        }
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToolNo();
    }
    #endregion

    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        LoadToolNo();
        LoadPartNo();

        /*Change By Mahesh :- Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("MMM/yyyy");// Set Bydefault current date

            PanelMsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtMonth.Focus();
            return;
        }
    }
    #endregion

    #region ddlpart_SelectedIndexChanged
    protected void ddlpart_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToolNo1();
    }
    #endregion

    #region LoadToolNo1
    private void LoadToolNo1()
    {
        if (ddlpart.SelectedValue != "0")
        {
            DataTable dt = new DataTable();
            if (Request.QueryString[0].Equals("INSERT"))
            {
                //dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_I_CODE='" + ddlpart.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0) order by T_NAME");
                //dt = CommonClasses.Execute("select distinct T_CODE,I_CODE,T_NAME , (select ISNULL(SUM(isnull(IRND_PROD_QTY,0)),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) AS IRND_PROD_QTY, T_PENDTOOLLIFE,(ISNULL((SELECT ISNULL(SUM(isnull(TRR_STD_PROD,0)),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),0)) AS TRR_STD_PROD into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1)   SELECT T_CODE,T_NAME FROM #temp inner join TOOLROOM_REFURBISH_MASTER TRM on T_CODE=TRM.TRR_T_CODE  where IRND_PROD_QTY<(T_PENDTOOLLIFE+TRM.TRR_STD_PROD) union select T_CODE,T_NAME  from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0)  order by T_NAME drop table #temp");
                dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_I_CODE='" + ddlpart.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0) order by T_NAME");
            }
            else
            {
                dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_I_CODE='" + ddlpart.SelectedValue + "' AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_STATUS=1 order by T_NAME");
            }
            if (dt.Rows.Count > 0)
            {
                ddlToolNo.DataSource = dt;
                ddlToolNo.DataTextField = "T_NAME";
                ddlToolNo.DataValueField = "T_CODE";
                ddlToolNo.DataBind();
                ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
                ddlToolNo.SelectedIndex = 1;
            }
            else
            {
                ddlToolNo.DataSource = dt;

                ddlToolNo.DataBind();
                ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
            }
        }
        else
        {
            LoadToolNo();
        }
    }
    #endregion
}
