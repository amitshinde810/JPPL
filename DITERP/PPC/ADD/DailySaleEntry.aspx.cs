using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_DailySaleEntry : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    txtMonth.Attributes.Add("readonly", "readonly");
                    LoadGroup();
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
                        txtMonth.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtMonth.Attributes.Add("readonly", "readonly");
                        dt2.Rows.Clear();
                        LoadFilter();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        dgDailySaleEntry.Enabled = false;
                        txtMonth_TextChanged(null, null);
                        LoadItem();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Daily Sale Entry", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region User Defined Method

    #region LoadItem
    public void LoadItem()
    {
        /*Join With Customer Schedule and Load Items For Selected Month and Year*/
        dtRequsitionDetail = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,0 as SaleQty ,PROD_GP_CODE from ITEM_MASTER INNER JOIN CUSTOMER_SCHEDULE CS ON I_CODE=CS_I_CODE  INNER JOIN PRODUCT_MASTER ON I_CODE=PRODUCT_MASTER.PROD_I_CODE where CS.ES_DELETE=0   AND PRODUCT_MASTER.ES_DELETE=0   and  ITEM_MASTER.ES_DELETE=0 AND datepart(MM, CS.CS_DATE)='" + Convert.ToDateTime(txtMonth.Text).ToString("MM") + "' AND datepart(yyyy, CS.CS_DATE)='" + Convert.ToDateTime(txtMonth.Text).ToString("yyyy") + "' AND I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        if (dtRequsitionDetail.Rows.Count > 0)
        {
            dgDailySaleEntry.DataSource = dtRequsitionDetail;
            dgDailySaleEntry.DataBind();
            dgDailySaleEntry.Enabled = true;
            dt2 = dtRequsitionDetail;
            ViewState["dt2"] = dt2;
        }
    }
    #endregion LoadItem
    //protected void ddlServiceReqNo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //DataTable dtLoadGrid = CommonClasses.Execute("SELECT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, COST_PREPARATION_DETAIL.CPD_QTY as SaleQty FROM ITEM_MASTER INNER JOIN COST_PREPARATION_DETAIL ON ITEM_MASTER.I_CODE = COST_PREPARATION_DETAIL.CPD_NEW_I_CODE INNER JOIN COST_PREPARATION_MASTER ON COST_PREPARATION_DETAIL.CPD_CP_CODE = COST_PREPARATION_MASTER.CP_CODE WHERE (COST_PREPARATION_MASTER.CP_SERVICE_REQ_NO = " + ddlServiceReq.SelectedValue + ") AND COST_PREPARATION_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 and COST_PREPARATION_DETAIL.CPD_DAMAGE=1");
    //    if (dtLoadGrid.Rows.Count > 0)
    //    {
    //        dgDailySaleEntry.DataSource = dtLoadGrid;
    //        dgDailySaleEntry.DataBind();
    //        dgDailySaleEntry.Enabled = true;
    //        dt2 = dtLoadGrid;
    //        ViewState["dt2"] = dt2;
    //        GetTotal();
    //    }
    //}

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
        if (dgDailySaleEntry.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SaleQty", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PROD_GP_CODE", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgDailySaleEntry.DataSource = dtFilter;
                dgDailySaleEntry.DataBind();
            }
        }
    }
    #endregion

    #region txtSaleQty_TextChanged
    protected void txtSaleQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblI_CODE = (Label)currentRow.FindControl("lblI_CODE");
        TextBox txtSaleQty = (TextBox)currentRow.FindControl("txtSaleQty");
        txtSaleQty.Text = CommonClasses.GetDoubleValue(txtSaleQty.Text).ToString();
        txtSaleQty.Focus();
        //((DataTable)ViewState["dt2"]).Select("NEW_I_CODE=" + lblNEW_I_CODE.Text). = "";
        ((DataTable)ViewState["dt2"]).AcceptChanges();
        DataRow[] rows = ((DataTable)ViewState["dt2"]).Select("I_CODE='" + lblI_CODE.Text + "'");
        //If you don't want to select a specific id ignore the parameter for select.
        string SaleQty = DecimalMasking(txtSaleQty.Text);
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i]["SaleQty"] = SaleQty;
        }
    }
    #endregion txtSaleQty_TextChanged

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtMonth.Attributes.Add("readonly", "readonly");
            dt = CommonClasses.Execute("SELECT DSEM_CODE,CONVERT(varchar, DSEM_MONTH,106) as DSEM_MONTH FROM DAILY_SALE_ENTRY_MASTER where DSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' and DSEM_CODE='" + ViewState["mlCode"].ToString() + "' and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["DSEM_CODE"]);

                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["DSEM_MONTH"]).ToString("dd MMM yyyy");
                dtRequsitionDetail = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,DSED_ACTUAL_QTY as SaleQty,PROD_GP_CODE FROM DAILY_SALE_ENTRY_MASTER DSEM INNER JOIN DAILY_SALE_ENTRY_DETAIL ON DSEM_CODE=DSED_DSEM_CODE INNER JOIN ITEM_MASTER ON DSED_I_CODE=I_CODE  INNER JOIN PRODUCT_MASTER ON I_CODE=PRODUCT_MASTER.PROD_I_CODE WHERE DSEM.ES_DELETE=0 AND PRODUCT_MASTER.ES_DELETE=0   AND ITEM_MASTER.ES_DELETE=0 AND DSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND DSEM_CODE=" + ViewState["mlCode"].ToString() + " ORDER BY I_CODENO +' - '+ I_NAME");
                if (dtRequsitionDetail.Rows.Count != 0)
                {
                    dgDailySaleEntry.DataSource = dtRequsitionDetail;
                    dgDailySaleEntry.DataBind();
                    dt2 = dtRequsitionDetail;
                    ViewState["dt2"] = dt2;
                }
            }
            if (str == "MOD" || str == "AMEND")
            {
                CommonClasses.SetModifyLock("DAILY_SALE_ENTRY_MASTER", "MODIFY", "DSEM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition", "ViewRec", Ex.Message);
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
                DataTable dtChkExist = CommonClasses.Execute("Select DSEM_CODE From DAILY_SALE_ENTRY_MASTER where DSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND DSEM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND ES_DELETE=0 and DSEM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "'");
                if (dtChkExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO [dbo].[DAILY_SALE_ENTRY_MASTER] (DSEM_CM_CODE,[DSEM_COMP_ID],[DSEM_MONTH]) VALUES ('" + Session["CompanyCode"].ToString() + "','" + Session["CompanyId"].ToString() + "','" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(DSEM_CODE) from DAILY_SALE_ENTRY_MASTER");
                        for (int i = 0; i < dgDailySaleEntry.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO [dbo].[DAILY_SALE_ENTRY_DETAIL]([DSED_DSEM_CODE],[DSED_I_CODE],[DSED_ACTUAL_QTY])VALUES('" + Convert.ToInt32(Code) + "','" + ((Label)dgDailySaleEntry.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgDailySaleEntry.Rows[i].FindControl("txtSaleQty")).Text + "')");
                        }
                        //for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                        //{
                        //    CommonClasses.Execute1("INSERT INTO [dbo].[DAILY_SALE_ENTRY_DETAIL]([DSED_DSEM_CODE],[DSED_I_CODE],[DSED_ACTUAL_QTY])VALUES('" + Convert.ToInt32(Code) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["I_CODE"].ToString() + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SaleQty"].ToString() + "')");
                        //}
                        CommonClasses.WriteLog("Daily sales plan", "Save", "Daily sales plan", Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/DailySaleEntryView.aspx", false);
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return result;
                }
                #region Comment_Bulk
                //result = CommonClasses.BulkInsertDataTable("DAILY_SALE_ENTRY_MASTER", ((DataTable)ViewState["dt2"]));
                //if (result == true)
                //{
                //    string Code = CommonClasses.GetMaxId("Select Max(DSEM_CODE) from DAILY_SALE_ENTRY_MASTER");
                //    CommonClasses.WriteLog("Core Inventory Transaction", "Save", "Core Inventory Transaction", ddlPartName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                //    result = true;
                //    Response.Redirect("~/PPC/VIEW/ViewCoreInventoryTransaction.aspx", false);
                //}
                //else
                //{
                //    ShowMessage("#Avisos", "Record not save...", CommonClasses.MSG_Warning);
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //}
                #endregion Comment_Bulk
            }
            #endregion Insert

            else
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select DSEM_CODE From DAILY_SALE_ENTRY_MASTER where DSEM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND DSEM_CM_CODE='" + Session["CompanyCode"].ToString() + "' and DSEM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' and DAILY_SALE_ENTRY_MASTER.ES_DELETE=0 and DSEM_CODE!='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE DAILY_SALE_ENTRY_MASTER SET [DSEM_COMP_ID]='" + Session["CompanyId"].ToString() + "' WHERE DSEM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.Execute1("delete from [dbo].[DAILY_SALE_ENTRY_DETAIL] where DSED_DSEM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        for (int i = 0; i < dgDailySaleEntry.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO [dbo].[DAILY_SALE_ENTRY_DETAIL]([DSED_DSEM_CODE],[DSED_I_CODE],[DSED_ACTUAL_QTY])VALUES('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgDailySaleEntry.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgDailySaleEntry.Rows[i].FindControl("txtSaleQty")).Text + "')");
                        }
                        //for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                        //{
                        //    CommonClasses.Execute1("INSERT INTO [dbo].[DAILY_SALE_ENTRY_DETAIL]([DSED_DSEM_CODE],[DSED_I_CODE],[DSED_ACTUAL_QTY])VALUES('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["I_CODE"].ToString() + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SaleQty"].ToString() + "')");
                        //}
                        CommonClasses.RemoveModifyLock("DAILY_SALE_ENTRY_MASTER", "MODIFY", "DSEM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Daily sales plan", "Update", "Daily sales plan", Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy"), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/DailySaleEntryView.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtMonth.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtMonth.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Daily Sale Entry", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(MR_BATCH_NO,0)) as numeric(10,0))) as MR_BATCH_NO from DAILY_SALE_ENTRY_MASTER where MR_COMP_CM_CODE='" + Session["CompanyCode"] + "' and ES_DELETE=0 ");
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
            DataTable dt = CommonClasses.Execute("select MODIFY from DAILY_SALE_ENTRY_MASTER where DSEM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition -ADD", "ModifyLog", Ex.Message);
        }
        return false;
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
            CommonClasses.SendError("Material Requsition - ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    private void InserRecord()
    {
        #region Validations


        #endregion

        #region CheckExist
        if (dgDailySaleEntry.Rows.Count > 0)
        {
            for (int i = 0; i < dgDailySaleEntry.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgDailySaleEntry.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                string PROCESSS_CODE = ((Label)(dgDailySaleEntry.Rows[i].FindControl("lblPROCESS_CODE"))).Text;
                string BT_CODE = ((Label)(dgDailySaleEntry.Rows[i].FindControl("lblBT_CODE"))).Text;
            }
        }
        #endregion

        #region Datatable Initialization

        if (dt2.Columns.Count == 0)
        {
            dt2.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("SaleQty", typeof(String)));

        }
        #endregion

        if (dt2.Rows.Count > 0)
        {
            for (int i = 0; i < dgDailySaleEntry.Rows.Count; i++)
            {
                double PTS_QTY = 0;
                if (Convert.ToString(((TextBox)dgDailySaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)dgDailySaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text) != "")
                {
                    PTS_QTY = Math.Round(Convert.ToDouble(((TextBox)dgDailySaleEntry.Rows[i].FindControl("txtADD_IN_QTY")).Text), 3);
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
                dt2.Rows.InsertAt(dr, Index);
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

        dgDailySaleEntry.DataSource = dt2;
        dgDailySaleEntry.DataBind();
        dgDailySaleEntry.Enabled = true;

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
            CommonClasses.SendError("Daily Sale Entry", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
         
           
            if (Convert.ToDateTime(txtMonth.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]) || Convert.ToDateTime(txtMonth.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Date in current Financial Year";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            DateTime Today1 = System.DateTime.Now;  /*Today date*/
            DateTime Today = Convert.ToDateTime(txtMonth.Text); /*Inserted Date*/
            DateTime day1 = Today1.AddDays(1); /*Yesterday*/
            //DateTime day2 = Today1.AddDays(-2); /*day before yesterday*/
            /*Check Inserted date Should be in between day before yesterday and today*/
            if (Today.ToString("dd/MMM/yyyy") == Today1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day1.ToString("dd/MMM/yyyy"))
            {
                PanelMsg.Visible = false; lblmsg.Text = "";
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You can't insert data for selected date, Please select correct date between today and Tomorrow....";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                /*Clear GridView*/
                dgDailySaleEntry.DataSource = null;
                dgDailySaleEntry.DataBind();
                return;
            }

            if (dgDailySaleEntry.Enabled)
            {
                #region Comment
                /*Copy Grid Data to Array*/
                //GridViewRow[] Chartarr = new GridViewRow[dgDailySaleEntry.Rows.Count];
                //dgDailySaleEntry.Rows.CopyTo(Chartarr, 0);
                //foreach (GridViewRow row in Chartarr)
                //{
                //    DataRow datarw;
                //    datarw = ((DataTable)ViewState["dt2"]).NewRow();
                //    /*Add data in Datarow*/
                //    for (int i = 0; i < row.Cells.Count; i++)
                //    {
                //        datarw[i] = row.Cells[i].Text;
                //    }
                //    ((DataTable)ViewState["dt2"]).Rows.Add(datarw);
                //}
                //DataView dv = new DataView(((DataTable)ViewState["dt2"]));
                //DataTable dtView = dv.ToTable(true, "I_CODE", "Sale Qty");
                #endregion Comment
                ddlGroup.SelectedValue = "0";
                ddlGroup_SelectedIndexChanged(null, null);
                SaveRec();
                /*Clear Dt For Bind GridView Data*/
                ((DataTable)ViewState["dt2"]).Clear();
            }
            else
            {
                ShowMessage("#Avisos", "Material Not Present.", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Daily Sale Entry", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadGroup
    protected void LoadGroup()
    {
        DataTable dtGroup = new DataTable();
        dtGroup = CommonClasses.Execute("select distinct GP_NAME,GP_CODE from GROUP_MASTER where ES_DELETE=0 and GP_COMP_ID='" + Session["CompanyId"] + "' ORDER BY GP_NAME");
        ddlGroup.DataSource = dtGroup;
        ddlGroup.DataTextField = "GP_NAME";
        ddlGroup.DataValueField = "GP_CODE";
        ddlGroup.DataBind();
         ddlGroup.Items.Insert(0, new ListItem("Select Group Name", "0"));
    }
    #endregion LoadGroup
    #region ddlGroup_SelectedIndexChanged
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

        //((DataTable)ViewState["dt2"]).DefaultView.RowFilter = string.Format("PROD_GP_CODE =  ", ddlGroup.SelectedValue);


        if (ddlGroup.SelectedValue != "0")
        {


            //results = ((DataTable)ViewState["dt2"]).Select("PROD_GP_CODE ='" + ddlGroup.SelectedValue + "'").CopyToDataTable();


            DataView dv;
            dv = new DataView(((DataTable)ViewState["dt2"]), "PROD_GP_CODE = '" + ddlGroup.SelectedValue + "' ", "PROD_GP_CODE", DataViewRowState.CurrentRows);

            dgDailySaleEntry.DataSource = dv;
            dgDailySaleEntry.DataBind();
            //dgDailySaleEntry.Enabled = true;
            // dt2 = results;


        }
        else
        {
            dgDailySaleEntry.DataSource = ((DataTable)ViewState["dt2"]);
            dgDailySaleEntry.DataBind();
        }
    }
    #endregion ddlGroup_SelectedIndexChanged

    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {

        if (Convert.ToDateTime(txtMonth.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]) || Convert.ToDateTime(txtMonth.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Date in current Financial Year";
            txtMonth.Focus();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        else
        {
            DateTime Today1 = System.DateTime.Now;  /*Today date*/
            DateTime Today = Convert.ToDateTime(txtMonth.Text); /*Inserted Date*/
            DateTime day1 = Today1.AddDays(1); /*Yesterday*/
            //DateTime day2 = Today1.AddDays(-2); /*day before yesterday*/
            /*Check Inserted date Should be in between day before yesterday and today*/
            if (Today.ToString("dd/MMM/yyyy") == Today1.ToString("dd/MMM/yyyy") || Today.ToString("dd/MMM/yyyy") == day1.ToString("dd/MMM/yyyy"))
            {
                PanelMsg.Visible = false; lblmsg.Text = "";
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You can't insert data for selected date, Please select correct date between today and Tomorrow....";
                txtMonth.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                /*Clear GridView*/
                dgDailySaleEntry.DataSource = null;
                dgDailySaleEntry.DataBind();
                return;
            }
        }
        /*Load Items On Date Change Event*/
        LoadItem();
    }

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
            CommonClasses.SendError("Daily Sale Entry", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Daily Sale Entry", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
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
                CommonClasses.RemoveModifyLock("DAILY_SALE_ENTRY_MASTER", "MODIFY", "DSEM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/PPC/VIEW/DailySaleEntryView.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Reqisition", "btnCancel_Click", Ex.Message);
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

            CommonClasses.SendError("Daily Sale Entry", "btnOk_Click", Ex.Message);
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
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 5)
            {
                no = 5;
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

    #region GridView_Events

    #region dgDailySaleEntry_RowDeleting
    protected void dgDailySaleEntry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgDailySaleEntry_SelectedIndexChanged
    protected void dgDailySaleEntry_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region dgDailySaleEntry_RowCommand
    protected void dgDailySaleEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgDailySaleEntry.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgDailySaleEntry.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgDailySaleEntry.DataSource = dt2;
                dgDailySaleEntry.DataBind();

                for (int i = 0; i < dgDailySaleEntry.Rows.Count; i++)
                {
                    string Code = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                    if (Code == "")
                    {
                        dgDailySaleEntry.Enabled = false;
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
            CommonClasses.SendError("Daily Sale Entry", "dgDailySaleEntry_RowCommand", Ex.Message);
        }
    }
    #endregion dgDailySaleEntry_RowCommand

    #endregion GridView_Events

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlPartName_SelectedIndexChanged

    #region txtActualQty_TextChanged
    protected void txtActualQty_TextChanged(object sender, EventArgs e)
    {
    }
    #endregion txtActualQty_TextChanged
}