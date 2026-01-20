using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_ADD_ToolRRefurbish : System.Web.UI.Page
{
    #region Declartion
    DirectoryInfo ObjSearchDir;
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
                    else
                    {
                        LoadToolNo();
                    }
                    
                    // ddlType_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("tools For Refurbish", "PageLoad", ex.Message);
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
            if (ddlToolNo.SelectedIndex == -1)
            {
                ShowMessage("#Avisos", "Please Select Tool No.", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlToolNo.Focus();
                return;
            }
            if (ddlType.SelectedValue == "0")
            {
                if (txtStdProd.Text.Trim() == "")
                {
                    ShowMessage("#Avisos", "Enter Stad. Prod.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtStdProd.Focus();
                    return;
                }
                if (txtRefrNo.Text.Trim() == "")
                {
                    ShowMessage("#Avisos", "Enter Refurbish Rev. No.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtRefrNo.Focus();
                    return;
                }
            }
            else
            {

            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("tools For Refurbish", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("tools For Refurbish", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select TRR_CODE,TRR_T_CODE,case when TRR_TYPE=1 then 1 else 0 end as TRR_TYPE,TRR_STD_PROD,TRR_REF_REV_NO,TRR_CM_COMP_ID from TOOLROOM_REFURBISH_MASTER where TRR_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " and ES_DELETE=0 and TRR_CM_COMp_ID=" + (string)Session["CompanyId"] + "");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["TRR_CODE"]);
                LoadToolNo1();
                ddlToolNo.SelectedValue = dt.Rows[0]["TRR_T_CODE"].ToString();
                ddlType.SelectedValue = dt.Rows[0]["TRR_TYPE"].ToString();
                ddlType_SelectedIndexChanged(null, null);
                txtStdProd.Text = dt.Rows[0]["TRR_STD_PROD"].ToString();
                txtRefrNo.Text = dt.Rows[0]["TRR_REF_REV_NO"].ToString();

                if (str == "VIEW")
                {
                    ddlToolNo.Enabled = false;
                    btnSubmit.Visible = false;
                    ddlType.Enabled = false;
                    txtStdProd.Enabled = false;
                    txtRefrNo.Enabled = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("TOOLROOM_REFURBISH_MASTER", "ES_MODIFY", "TRR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("tools For Refurbish", "ViewRec", Ex.Message);
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
        string Code = "";
        //start of sql trandaction
        trans = connection.BeginTransaction();

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                SqlCommand command = new SqlCommand("INSERT INTO TOOLROOM_REFURBISH_MASTER (TRR_T_CODE,TRR_TYPE,TRR_STD_PROD,TRR_REF_REV_NO,TRR_CM_COMP_ID,TRR_DATE)VALUES ('" + ddlToolNo.SelectedValue.ToString() + "','" + ddlType.SelectedValue + "','" + txtStdProd.Text + "','" + txtRefrNo.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "',GETDATE())", connection, trans);
                command.ExecuteNonQuery();

                //string Code = CommonClasses.GetMaxId("Select Max(TRR_CODE) from TOOLROOM_REFURBISH_MASTER");

                SqlCommand cmd1 = new SqlCommand("Select Max(TRR_CODE) from TOOLROOM_REFURBISH_MASTER", connection, trans);
                cmd1.Transaction = trans;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    Code = (dr1[0].ToString().Trim());
                }
                cmd1.Dispose();
                dr1.Dispose();

                trans.Commit();
                CommonClasses.WriteLog("tools For Refurbish", "Save", "tools For Refurbish", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/ViewToolRRefurbish.aspx", false);
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("UPDATE TOOLROOM_REFURBISH_MASTER SET TRR_T_CODE='" + ddlToolNo.SelectedValue + "',TRR_TYPE='" + ddlType.SelectedValue + "',TRR_STD_PROD='" + txtStdProd.Text + "',TRR_REF_REV_NO='" + txtRefrNo.Text + "' WHERE TRR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                command.ExecuteNonQuery();

                trans.Commit();
                CommonClasses.RemoveModifyLock("TOOLROOM_REFURBISH_MASTER", "ES_MODIFY", "TRR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

                CommonClasses.WriteLog("tools For Refurbish", "Update", "tools For Refurbish", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/ViewToolRRefurbish.aspx", false);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("tools For Refurbish", "SaveRec", ex.Message);
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
            CommonClasses.SendError("tools For Refurbish", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("tools For Refurbish", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("TOOLROOM_REFURBISH_MASTER", "ES_MODIFY", "TRR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/ToolRoom/VIEW/ViewToolRRefurbish.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("tools For Refurbish", "CancelRecord", ex.Message.ToString());
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
            CommonClasses.SendError("tools For Refurbish", "CheckValid", Ex.Message);
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
        // Correct Query:- //
        //dt = CommonClasses.Execute("select distinct T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,IRN_DETAIL,ITEM_MASTER where IRND_T_CODE=T_CODE and TOOL_MASTER.ES_DELETE=0 and IRND_PROD_QTY>T_PENDTOOLLIFE AND (T_STATUS=1) AND ITEM_MASTER.ES_DELETE=0 and IRND_I_CODE=I_CODE");
        //dt = CommonClasses.Execute("SELECT T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME FROM TOOL_MASTER,ITEM_MASTER WHERE (T_CM_COMP_ID = 1) AND (TOOL_MASTER.ES_DELETE = 0) AND (T_STATUS=1) AND (T_I_CODE=I_CODE) AND (ITEM_MASTER.ES_DELETE=0) ORDER BY T_NAME");
       
        //dt = CommonClasses.Execute("select distinct T_CODE,I_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME , (select ISNULL(SUM(IRND_PROD_QTY),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) AS IRND_PROD_QTY, T_PENDTOOLLIFE,(ISNULL((SELECT ISNULL(SUM(TRR_STD_PROD),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),0)) AS TRR_STD_PROD into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1) SELECT T_CODE,I_CODE,T_NAME,IRND_PROD_QTY,T_PENDTOOLLIFE,TRR_STD_PROD FROM #temp where IRND_PROD_QTY>=(T_PENDTOOLLIFE+TRR_STD_PROD) drop table #temp");

        //Update as per logic change
        dt = CommonClasses.Execute(" select distinct T_CODE,I_CODE,T_PENDTOOLLIFE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME ,  ISNULL((SELECT CONVERT(varchar, MAX(TRR_DATE),106) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),'') AS ttt into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1)     SELECT T_CODE,I_CODE,T_NAME,ttt,T_PENDTOOLLIFE ,CASE when ttt='' then (select ISNULL(SUM(IRND_PROD_QTY),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) ELSE  (select ISNULL(SUM(IRND_PROD_QTY),0) from IRN_ENTRY inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE AND IRN_DATE>= ttt) END  AS IRND_PROD_QTY,  CASE when ttt='' then T_PENDTOOLLIFE else    (ISNULL((SELECT ISNULL(SUM(TRR_STD_PROD),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE AND  CONVERT(varchar, (TRR_DATE),106)=ttt),0)) END AS TRR_STD_PROD        into #temp1   FROM #temp  drop table #temp         SELECT * FROM  #temp1   where IRND_PROD_QTY>=(TRR_STD_PROD)     DROP TABLE #temp1  ");
        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
        }
        else
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataBind();
        }
    }
    #endregion LoadToolNo

    #region LoadToolNo1
    private void LoadToolNo1()
    {
        DataTable dt = new DataTable();
        // Correct Query:- //
        //dt = CommonClasses.Execute("select distinct T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,IRN_DETAIL,ITEM_MASTER where IRND_T_CODE=T_CODE and TOOL_MASTER.ES_DELETE=0 and IRND_PROD_QTY>T_PENDTOOLLIFE AND (T_STATUS=1) AND ITEM_MASTER.ES_DELETE=0 and IRND_I_CODE=I_CODE");
        //dt = CommonClasses.Execute("SELECT T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME FROM TOOL_MASTER,ITEM_MASTER WHERE (T_CM_COMP_ID = 1) AND (TOOL_MASTER.ES_DELETE = 0) AND (T_STATUS=1) AND (T_I_CODE=I_CODE) AND (ITEM_MASTER.ES_DELETE=0) ORDER BY T_NAME");
        dt = CommonClasses.Execute("select distinct T_CODE,I_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME , (select ISNULL(SUM(IRND_PROD_QTY),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) AS IRND_PROD_QTY, T_PENDTOOLLIFE, (ISNULL((SELECT ISNULL(SUM(TRR_STD_PROD),0)  FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),0)) AS TRR_STD_PROD into #temp from TOOL_MASTER ,ITEM_MASTER where TOOL_MASTER.ES_DELETE=0  AND ITEM_MASTER.ES_DELETE=0  AND T_I_CODE=I_CODE SELECT T_CODE,	I_CODE	,T_NAME	,IRND_PROD_QTY,	T_PENDTOOLLIFE,TRR_STD_PROD  FROM #temp   drop table #temp");
        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
        }
        else
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataBind();
        }
    }
    #endregion LoadToolNo

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "0")
        {
            divRefirbish.Visible = true;
            lblrevno.Visible = true;
            txtStdProd.Visible = true;
            txtRefrNo.Visible = true;
        }
        else
        {
            divRefirbish.Visible = false;
            lblrevno.Visible = false;
            txtRefrNo.Visible = false;
            txtStdProd.Visible = false;
            txtRefrNo.Text = "";
            txtStdProd.Text = "";
        }
    }
}

