using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ShortSaleEntry : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    static int mlCode = 0;
    DatabaseAccessLayer DL_DBAccess = null;
    public static string str = "";
    int PK_CODE;
    public string message = "";
    static DataTable dt2 = new DataTable();
    DataTable dtRequsitionDetail = new DataTable();
    static string ItemUpdateIndex = "-1";
    DataRow dr;
    public static int Index = 0;

    #endregion

    #region Page_Load
    protected void Page_Load(object sender,EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx",false);
            }
            else
            {
                if (!IsPostBack)
                {
                    HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
                    home.Attributes["class"] = "active";
                    HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
                    home1.Attributes["class"] = "active";

                    txtMonth.Attributes.Add("readonly","readonly");

                    ViewState["dt2"] = null;
                    ViewState["mlCode"] = null;
                    ViewState["mlCode"] = mlCode;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = mlCode;
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        EnabaleTextBoxes(MainPanel);
                        ViewState["mlCode"] = mlCode;
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("AMEND"))
                    {
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        EnabaleTextBoxes(MainPanel);
                        ViewRec("AMEND");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtMonth.Text = System.DateTime.Now.AddDays(-1).ToString("dd MMM yyyy");
                        txtMonth.Attributes.Add("readonly","readonly");
                        dt2.Rows.Clear();
                        LoadFilter();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        dgShortSaleEntry.Enabled = false;
                        LoadItem();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","Page_Load",Ex.Message);
        }
    }
    #endregion

    #region User Defined Method

    #region LoadItem
    public void LoadItem()
    {
        DateTime dtday = Convert.ToDateTime(Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy")); /*Selected Date*/
        /*Join With Customer Schedule,Daily Sale Entry and Load Items which was short qty as Compare with yesterdays And Todays qty*/
        //DataTable dtLoadGrid = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,0 as SSED_SHORT_QTY,0 as SSED_FDY,0 as SSED_MC_SHOP,0 as SSED_QA,0 as SSED_MAINT,0 as SSED_TOOL_ROOM,0 as SSED_PPC,0 as SSED_PURCHASE,0 as SSED_OUTSIDE_VENDOR,0 as SSED_CSN from ITEM_MASTER INNER JOIN CUSTOMER_SCHEDULE CS ON I_CODE=CS_I_CODE where CS.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND datepart(MM,CS.CS_DATE)='" + Convert.ToDateTime(txtMonth.Text).ToString("MM") + "' AND datepart(yyyy,CS.CS_DATE)='" + Convert.ToDateTime(txtMonth.Text).ToString("yyyy") + "' AND I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        DataTable dtLoadGrid = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,0 as SSED_SHORT_QTY,0 as SSED_FDY,0 as SSED_MC_SHOP,0 as SSED_QA,0 as SSED_MAINT,0 as SSED_TOOL_ROOM,0 as SSED_PPC,0 as SSED_PURCHASE,0 as SSED_OUTSIDE_VENDOR,0 as SSED_CSN,isnull((select sum(isnull(DSED_ACTUAL_QTY,0)) from DAILY_SALE_ENTRY_DETAIL inner join DAILY_SALE_ENTRY_MASTER on DSED_DSEM_CODE=DSEM_CODE where DSEM_CODE=DSEM.DSEM_CODE and DSED_I_CODE=DSED.DSED_I_CODE and DSEM_MONTH='" + dtday.ToString("dd/MMM/yyyy") + "'),0) as DailyQty,ISNULL((SELECT SUM(ISNULL(IND_INQTY,0)) FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INM_CODE=IND_INM_CODE where IND_I_CODE=DSED.DSED_I_CODE AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE=0 AND INM_DATE='" + dtday.ToString("dd/MMM/yyyy") + "'),0) AS InvoiceQty,ISNULL((isnull((select sum(isnull(DSED_ACTUAL_QTY,0)) from DAILY_SALE_ENTRY_DETAIL inner join DAILY_SALE_ENTRY_MASTER on DSED_DSEM_CODE=DSEM_CODE where DSEM_CODE=DSEM.DSEM_CODE and DSED_I_CODE=DSED.DSED_I_CODE and DSEM_MONTH='" + dtday.ToString("dd/MMM/yyyy") + "'),0) -ISNULL((SELECT SUM(ISNULL(IND_INQTY,0)) FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INM_CODE=IND_INM_CODE where IND_I_CODE=DSED.DSED_I_CODE AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE=0 AND INM_DATE='" + dtday.ToString("dd/MMM/yyyy") + "'),0)),0) as DiffQty into #Temp from ITEM_MASTER INNER JOIN CUSTOMER_SCHEDULE CS ON I_CODE=CS_I_CODE inner join DAILY_SALE_ENTRY_DETAIL DSED on DSED.DSED_I_CODE=CS.CS_I_CODE inner join DAILY_SALE_ENTRY_MASTER DSEM on DSED.DSED_DSEM_CODE=DSEM.DSEM_CODE where CS.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND datepart(MM,CS.CS_DATE)='" + dtday.ToString("MM") + "' AND datepart(yyyy,CS.CS_DATE)='" + dtday.ToString("yyyy") + "' AND I_CAT_CODE='-2147483648' and I_CM_COMP_ID='1' order by I_CODENO +' - '+ I_NAME select I_CODE,I_CODENO,I_NAME,ICODE_INAME,DiffQty as SSED_SHORT_QTY,SSED_FDY,SSED_MC_SHOP,SSED_QA,SSED_MAINT,SSED_TOOL_ROOM,SSED_PPC,SSED_PURCHASE,SSED_OUTSIDE_VENDOR,SSED_CSN,DailyQty,InvoiceQty from #Temp where DiffQty>0 drop table #Temp");
        if (dtLoadGrid.Rows.Count > 0)
        {
            dgShortSaleEntry.DataSource = dtLoadGrid;
            dgShortSaleEntry.DataBind();
            dgShortSaleEntry.Enabled = true;
            dt2 = dtLoadGrid;
            ViewState["dt2"] = dt2;
        }
    }
    #endregion LoadItem

    #region Enabale
    public static void EnabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = true;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = true;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = true;

                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = true;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        EnabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region Diabale
    public static void DiabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = false;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = false;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = false;

                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = false;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        DiabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgShortSaleEntry.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_SHORT_QTY",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_FDY",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_MC_SHOP",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_QA",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_MAINT",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_TOOL_ROOM",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_PPC",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_PURCHASE",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_OUTSIDE_VENDOR",typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SSED_CSN",typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgShortSaleEntry.DataSource = dtFilter;
                dgShortSaleEntry.DataBind();
            }
        }
    }
    #endregion

    #region txtShortQty_TextChanged
    protected void txtShortQty_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtShortQty = (TextBox)currentRow.FindControl("txtShortQty");
        txtShortQty.Text = CommonClasses.GetDoubleValue(txtShortQty.Text).ToString();

        //((DataTable)ViewState["dt2"]).Select("NEW_I_CODE=" + lblNEW_I_CODE.Text). = "";
        ((DataTable)ViewState["dt2"]).AcceptChanges();
        DataRow[] rows = ((DataTable)ViewState["dt2"]).Select("I_CODE='" + lblI_CODE.Text + "'");
        //If you don't want to select a specific id ignore the parameter for select.
        string ShortQty = DecimalMasking(txtShortQty.Text);
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i]["ShortQty"] = ShortQty;
        }
    }
    #endregion txtShortQty_TextChanged

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtMonth.Attributes.Add("readonly","readonly");
            dt = CommonClasses.Execute("SELECT SSEM_CODE,CONVERT(varchar,SSEM_MONTH,106) as SSEM_MONTH FROM SHORT_SALE_ENTRY_MASTER where SSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' and SSEM_CODE='" + ViewState["mlCode"].ToString() + "' and ES_DELETE=0 ");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["SSEM_CODE"]);

                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["SSEM_MONTH"]).ToString("dd MMM yyyy");
                dtRequsitionDetail = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO,I_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,SSED_SHORT_QTY,SSED_FDY,SSED_MC_SHOP,SSED_QA,SSED_MAINT,SSED_TOOL_ROOM,SSED_PPC,SSED_PURCHASE,SSED_OUTSIDE_VENDOR,SSED_CSN  from SHORT_SALE_ENTRY_MASTER SSEM inner join SHORT_SALE_ENTRY_DETAIL on SSEM.SSEM_CODE=SSED_SSEM_CODE INNER JOIN ITEM_MASTER ON SSED_I_CODE=I_CODE WHERE SSEM.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND  SSEM.SSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND SSEM.SSEM_CODE=" + ViewState["mlCode"].ToString() + " ");
                if (dtRequsitionDetail.Rows.Count != 0)
                {
                    dgShortSaleEntry.DataSource = dtRequsitionDetail;
                    dgShortSaleEntry.DataBind();
                    dt2 = dtRequsitionDetail;
                    ViewState["dt2"] = dt2;
                }
            }
            if (str == "MOD" || str == "AMEND")
            {
                CommonClasses.SetModifyLock("SHORT_SALE_ENTRY_MASTER","MODIFY","SSEM_CODE",Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","ViewRec",Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dtChkExist = CommonClasses.Execute("Select SSEM_CODE From SHORT_SALE_ENTRY_MASTER where SSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND SSEM_CM_CODE='" + Session["CompanyCode"].ToString() + "'  AND ES_DELETE=0 and SSEM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "'");
                if (dtChkExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO [dbo].[SHORT_SALE_ENTRY_MASTER] (SSEM_CM_CODE,[SSEM_COMP_ID],[SSEM_MONTH]) VALUES ('" + Session["CompanyCode"].ToString() + "','" + Session["CompanyId"].ToString() + "','" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SSEM_CODE) from SHORT_SALE_ENTRY_MASTER");
                        for (int i = 0; i < dgShortSaleEntry.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO [dbo].[SHORT_SALE_ENTRY_DETAIL]([SSED_SSEM_CODE],[SSED_I_CODE],[SSED_SHORT_QTY],[SSED_FDY],[SSED_MC_SHOP],[SSED_QA],[SSED_MAINT],[SSED_TOOL_ROOM],[SSED_PPC],[SSED_PURCHASE],[SSED_OUTSIDE_VENDOR],[SSED_CSN])VALUES('" + Convert.ToInt32(Code) + "','" + ((Label)dgShortSaleEntry.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_SHORT_QTY")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_FDY")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_MC_SHOP")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_QA")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_MAINT")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_TOOL_ROOM")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_PPC")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_PURCHASE")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_OUTSIDE_VENDOR")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_CSN")).Text + "')");
                        }
                        CommonClasses.WriteLog("Machine Booking Master","Save","Machine Booking Master",Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy"),Convert.ToInt32(Code),Convert.ToInt32(Session["CompanyId"]),Convert.ToInt32(Session["CompanyCode"]),(Session["Username"].ToString()),Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ShortSaleEntryView.aspx",false);
                    }
                }
                else
                {
                    ShowMessage("#Avisos","Record Already Exists",CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert();",true);
                    return result;
                }
            }
            #endregion Insert

            else
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select SSEM_CODE From SHORT_SALE_ENTRY_MASTER where SSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND SSEM_CM_CODE='" + Session["CompanyCode"].ToString() + "' and SSEM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' and SHORT_SALE_ENTRY_MASTER.ES_DELETE='False' and SSEM_CODE!='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE SHORT_SALE_ENTRY_MASTER SET [SSEM_COMP_ID]='" + Session["CompanyId"].ToString() + "' WHERE SSEM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.Execute1("Delete From [dbo].[SHORT_SALE_ENTRY_DETAIL] where SSED_SSEM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        for (int i = 0; i < dgShortSaleEntry.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO [dbo].[SHORT_SALE_ENTRY_DETAIL]([SSED_SSEM_CODE],[SSED_I_CODE],[SSED_SHORT_QTY],[SSED_FDY],[SSED_MC_SHOP],[SSED_QA],[SSED_MAINT],[SSED_TOOL_ROOM],[SSED_PPC],[SSED_PURCHASE],[SSED_OUTSIDE_VENDOR],[SSED_CSN])VALUES('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgShortSaleEntry.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_SHORT_QTY")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_FDY")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_MC_SHOP")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_QA")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_MAINT")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_TOOL_ROOM")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_PPC")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_PURCHASE")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_OUTSIDE_VENDOR")).Text + "','" + ((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtSSED_CSN")).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("SHORT_SALE_ENTRY_MASTER","MODIFY","SSEM_CODE",Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Machine Booking Master","Update","Machine Booking Master",Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy"),Convert.ToInt32(ViewState["mlCode"]),Convert.ToInt32(Session["CompanyId"]),Convert.ToInt32(Session["CompanyCode"]),(Session["Username"].ToString()),Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ShortSaleEntryView.aspx",false);
                    }
                    else
                    {
                        ShowMessage("#Avisos","Invalid Update",CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert();",true);
                        txtMonth.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos","Record Already Exists",CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert();",true);
                    txtMonth.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Short Sale Entry","SaveRec",ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(MR_BATCH_NO,0)) as numeric(10,0))) as MR_BATCH_NO from SHORT_SALE_ENTRY_MASTER where MR_COMP_CM_CODE='" + Session["CompanyCode"] + "' and ES_DELETE=0 ");
        if (dt.Rows[0]["MR_BATCH_NO"] == null || dt.Rows[0]["MR_BATCH_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["MR_BATCH_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from SHORT_SALE_ENTRY_MASTER where SSEM_CODE=" + PrimaryKey + " ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos","Record used by another user",CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert1();",true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry -ADD","ModifyLog",Ex.Message);
        }
        return false;
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName,string Message,string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),"regMostrarMensagem","MostrarMensagem('" + DiveName + "','" + Message + "','" + MessageType + "');",true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry - ADD","ShowMessage",Ex.Message);
            return false;
        }
    }
    #endregion

    private void InserRecord()
    {
        #region Validations


        #endregion

        #region CheckExist
        if (dgShortSaleEntry.Rows.Count > 0)
        {
            for (int i = 0; i < dgShortSaleEntry.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgShortSaleEntry.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                string PROCESSS_CODE = ((Label)(dgShortSaleEntry.Rows[i].FindControl("lblPROCESS_CODE"))).Text;
                string BT_CODE = ((Label)(dgShortSaleEntry.Rows[i].FindControl("lblBT_CODE"))).Text;
            }
        }
        #endregion

        #region Datatable Initialization

        if (dt2.Columns.Count == 0)
        {
            dt2.Columns.Add(new System.Data.DataColumn("I_CODE",typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CODENO",typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_NAME",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_SHORT_QTY",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_FDY",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_MC_SHOP",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_QA",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_MAINT",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_TOOL_ROOM",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_PPC",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_PURCHASE",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_OUTSIDE_VENDOR",typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SSED_CSN",typeof(String)));
        }
        #endregion

        if (dt2.Rows.Count > 0)
        {
            for (int i = 0; i < dgShortSaleEntry.Rows.Count; i++)
            {
                double PTS_QTY = 0;
                if (Convert.ToString(((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text) != "")
                {
                    PTS_QTY = Math.Round(Convert.ToDouble(((TextBox)dgShortSaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text),3);
                }
                dt2.Rows[i]["ADD_IN_QTY"] = PTS_QTY.ToString();
                dt2.AcceptChanges();
            }
        }


        #region InsertData
        if (str == "Modify")
        {
            if (dt2.Rows.Count > 0)
            {
                dt2.Rows.RemoveAt(Index);
                dt2.Rows.InsertAt(dr,Index);
            }
        }
        else
        {
            dt2.Rows.Add(dr);
        }
        #endregion


        dt2.DefaultView.Sort = "PROCESS_CODE ASC";
        dt2.AcceptChanges();

        DataView dv = dt2.DefaultView;
        dv.Sort = "PROCESS_CODE ASC";
        dt2 = dv.ToTable();

        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            dt2.Rows[i]["STEP_NO"] = ((i + 1) * 10).ToString();
        }

        dgShortSaleEntry.DataSource = dt2;
        dgShortSaleEntry.DataBind();
        dgShortSaleEntry.Enabled = true;

        clearDetail();
    }

    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","clearDetail",Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender,EventArgs e)
    {
        try
        {
            /*Clear Dt For Bind GridView Data*/
           
            if (Convert.ToDateTime(txtMonth.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]) || Convert.ToDateTime(txtMonth.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Date in current Financial Year";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert();",true);
                return;
            }
            #region Validate For insert Short Sale For Last Days Only
            DateTime Today1 = System.DateTime.Now;  /*Today date*/
            DateTime Today = Convert.ToDateTime(txtMonth.Text); /*Inserted Date*/
            DateTime day1 = Today1.AddDays(-1); /*Yesterday*/
            DateTime day2 = Today1.AddDays(-2); /*day before yesterday*/
            /*Check Inserted date Should be in between day before yesterday and today*/
            if (Today.ToString("dd/MMM/yyyy") != Today1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day2.ToString("dd/MMM/yyyy"))
            {
                PanelMsg.Visible = false; lblmsg.Text = "";
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You can't insert data for selected date,Please select correct date between day before yesterday and yesterday....";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                /*Clear GridView*/
                dgShortSaleEntry.DataSource = null;
                dgShortSaleEntry.DataBind();
                return;
            }
            #endregion Validate For insert Short Sale For Last Days Only
            if (dgShortSaleEntry.Enabled)
            {
                SaveRec();
                ((DataTable)ViewState["dt2"]).Clear();
            }
            else
            {
                ShowMessage("#Avisos","Data Not Present.",CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert1();",true);
                return;
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Short Sale Entry","btnSubmit_Click",Ex.Message);
        }
    }
    #endregion
    protected void txtMonth_TextChanged(object sender,EventArgs e)
    {
        if (Convert.ToDateTime(txtMonth.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]) || Convert.ToDateTime(txtMonth.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Date in current Financial Year";
            txtMonth.Focus();
            ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert();",true);
            return;
        }
        else
        {
            #region Validate For insert Short Sale For Last Days Only
            DateTime Today1 = System.DateTime.Now;  /*Today date*/
            DateTime Today = Convert.ToDateTime(txtMonth.Text); /*Inserted Date*/
            DateTime day1 = Today1.AddDays(-1); /*Yesterday*/
            DateTime day2 = Today1.AddDays(-2); /*day before yesterday*/
            /*Check Inserted date Should be in between day before yesterday and today*/
            if (Today.ToString("dd/MMM/yyyy") != Today1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day2.ToString("dd/MMM/yyyy"))
            {
                PanelMsg.Visible = false; lblmsg.Text = "";
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You can't insert data for selected date,Please select correct date between day before yesterday and yesterday....";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert1();",true);
                /*Clear GridView*/
                dgShortSaleEntry.DataSource = null;
                dgShortSaleEntry.DataBind();
                return;
            }
            #endregion Validate For insert Short Sale For Last Days Only
        }
        /*Load Items On Date Change Event*/
        LoadItem();
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender,EventArgs e)
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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Short Sale Entry","btnCancel_Click",ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            //if (ddlType.SelectedIndex == 0)
            {
                flag = false;
            }

            //else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","CheckValid",Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender,EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (ViewState["mlCode"].ToString() != null && Convert.ToInt32(ViewState["mlCode"].ToString()) != 0)
            {
                CommonClasses.RemoveModifyLock("SHORT_SALE_ENTRY_MASTER","MODIFY","SSEM_CODE",Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/PPC/VIEW/ShortSaleEntryView.aspx",false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","btnCancel_Click",Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender,EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Short Sale Entry","btnOk_Click",Ex.Message);
        }
    }
    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0,(s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 5)
            {
                no = 5;
            }
            result1 = s.Substring((result.IndexOf(".") + 1),no);
            try
            {
                result2 = result1.Substring(0,result1.IndexOf("."));
            }
            catch
            {
            }
            string result3 = s.Substring((s.IndexOf(".") + 1),1);
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

    #region GridView_Events

    #region dgShortSaleEntry_RowDeleting
    protected void dgShortSaleEntry_RowDeleting(object sender,GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgShortSaleEntry_SelectedIndexChanged
    protected void dgShortSaleEntry_SelectedIndexChanged(object sender,EventArgs e)
    {

    }
    #endregion

    #region dgShortSaleEntry_RowCommand
    protected void dgShortSaleEntry_RowCommand(object sender,GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgShortSaleEntry.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgShortSaleEntry.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgShortSaleEntry.DataSource = dt2;
                dgShortSaleEntry.DataBind();

                for (int i = 0; i < dgShortSaleEntry.Rows.Count; i++)
                {
                    string Code = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                    if (Code == "")
                    {
                        dgShortSaleEntry.Enabled = false;
                    }
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Short Sale Entry","dgShortSaleEntry_RowCommand",Ex.Message);
        }
    }
    #endregion dgShortSaleEntry_RowCommand

    #endregion GridView_Events

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender,EventArgs e)
    {
    }
    #endregion ddlPartName_SelectedIndexChanged

    #region txtActualQty_TextChanged
    protected void txtActualQty_TextChanged(object sender,EventArgs e)
    {
    }
    #endregion txtActualQty_TextChanged

    #region txtSSED_SHORT_QTY_TextChanged
    protected void txtSSED_SHORT_QTY_TextChanged(object sender,EventArgs e)
    {
    }
    #endregion txtSSED_SHORT_QTY_TextChanged

    #region txtSSED_FDY_TextChanged
    protected void txtSSED_FDY_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_FDY");
        TextBox txtShortQty = (TextBox)currentRow.FindControl("txtSSED_SHORT_QTY");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            ScriptManager.RegisterStartupScript(this,GetType(),"displayalertmessage","Showalert1();",true);
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_FDY_TextChanged

    #region txtSSED_MC_SHOP_TextChanged
    protected void txtSSED_MC_SHOP_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_MC_SHOP");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_MC_SHOP_TextChanged

    #region txtSSED_QA_TextChanged
    protected void txtSSED_QA_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_QA");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_QA_TextChanged

    #region txtSSED_MAINT_TextChanged
    protected void txtSSED_MAINT_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_MAINT");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_MAINT_TextChanged

    #region txtSSED_TOOL_ROOM_TextChanged
    protected void txtSSED_TOOL_ROOM_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_TOOL_ROOM_TextChanged

    #region txtSSED_PPC_TextChanged
    protected void txtSSED_PPC_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_PPC");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_PPC_TextChanged

    #region txtSSED_PURCHASE_TextChanged
    protected void txtSSED_PURCHASE_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_PURCHASE");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_PURCHASE_TextChanged

    #region txtSSED_OUTSIDE_VENDOR_TextChanged
    protected void txtSSED_OUTSIDE_VENDOR_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0"; return;
        }
    }
    #endregion txtSSED_OUTSIDE_VENDOR_TextChanged

    #region txtSSED_CSN_TextChanged
    protected void txtSSED_CSN_TextChanged(object sender,EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtQty = (TextBox)currentRow.FindControl("txtSSED_CSN");
        txtQty.Text = CommonClasses.GetDoubleValue(txtQty.Text).ToString();
        txtQty.Focus();
        double Cnt = 0;
        Cnt = Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_SHORT_QTY")).Text.ToString())) - (Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_FDY")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MC_SHOP")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_QA")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_MAINT")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_TOOL_ROOM")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PPC")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_PURCHASE")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_OUTSIDE_VENDOR")).Text.ToString())) + Convert.ToDouble(CommonClasses.GetDoubleValue(((TextBox)currentRow.FindControl("txtSSED_CSN")).Text.ToString())));
        if (Cnt >= 0)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Short Qty Should be Match with Reason Qty...";
            txtQty.Text = "0";
            return;
        }
    }
    #endregion txtSSED_CSN_TextChanged
}