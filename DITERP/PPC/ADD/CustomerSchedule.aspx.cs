using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_CustomerSchedule : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    # endregion

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
                    try
                    {
                        ViewState["mlCode"] = "";
                        ViewState["mlCode"] = mlCode;
                        txtMonth.Attributes.Add("readonly", "readonly");
                        txtMonth.Text = System.DateTime.Now.ToString("MMM yyyy");
                        LoadItem(); LoadCustomer(); // Call Method
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("Amend"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("CUSTOMER SCHEDULE", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER SCHEDULE", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT DISTINCT CS_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS PROD_NAME, right(convert(varchar, C.CS_DATE, 106), 8) as CS_DATE,I.I_CODE, ISNULL(C.CS_SCHEDULE_QTY, 0) AS CS_SCHEDULE_QTY, ISNULL(C.CS_WEEK1, 0) AS CS_WEEK1, ISNULL(C.CS_WEEK2, 0) AS CS_WEEK2, ISNULL(C.CS_WEEK3, 0) AS CS_WEEK3, ISNULL(C.CS_WEEK4, 0) AS CS_WEEK4,isnull(CS_ASKING_RATE,0) as CS_ASKING_RATE,CS_SALES_RATE,CS_INTERNAL_FINISH_WEIGHT FROM CUSTOMER_SCHEDULE AS C INNER JOIN ITEM_MASTER AS I ON C.CS_I_CODE = I.I_CODE inner join PARTY_MASTER P ON C.CS_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.CS_CODE= '" + ViewState["mlCode"] + "'AND C.CS_COMP_ID='" + (string)Session["CompanyId"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtMonth.Text = dt.Rows[0]["CS_DATE"].ToString();
                LoadItem();
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                LoadCustomer();
                ddlCustomer.SelectedValue = dt.Rows[0]["P_CODE"].ToString();
                txtsalesRate.Text = dt.Rows[0]["CS_SALES_RATE"].ToString();
                txtIntFinishwgt.Text = dt.Rows[0]["CS_INTERNAL_FINISH_WEIGHT"].ToString();
                txtScheduleQty.Text = dt.Rows[0]["CS_SCHEDULE_QTY"].ToString();
                txtWeek1.Text = dt.Rows[0]["CS_WEEK1"].ToString();
                txtWeek2.Text = dt.Rows[0]["CS_WEEK2"].ToString();
                txtWeek3.Text = dt.Rows[0]["CS_WEEK3"].ToString();
                txtWeek4.Text = dt.Rows[0]["CS_WEEK4"].ToString();
                txtAskingRate.Text = dt.Rows[0]["CS_ASKING_RATE"].ToString();
                if (str == "VIEW")
                {
                    ddlItemName.Enabled = false; ddlCustomer.Enabled = false; txtScheduleQty.Enabled = false; txtWeek1.Enabled = false;
                    txtWeek2.Enabled = false; txtWeek3.Enabled = false; txtWeek4.Enabled = false;
                    btnSubmit.Visible = false;
                }
                ddlItemName.Enabled = false;
                ddlCustomer.Enabled = false;
                txtMonth.Enabled = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CUSTOMER_SCHEDULE", "MODIFY", "CS_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER SCHEDULE", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation

        if (ddlCustomer.SelectedValue == "0")
        {
            ShowMessage("#Avisos", "Please Select Customer Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlCustomer.Focus();
            return;
        }

        if (ddlItemName.SelectedIndex == -1 || ddlItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlItemName.Focus();
            return;
        }


        //if ((txtAskingRate.Text.Trim() == "" || Convert.ToDouble(txtAskingRate.Text.Trim()) <= 0) || (txtWeek2.Text.Trim() == "" || Convert.ToDouble(txtWeek2.Text.Trim()) <= 0) || (txtWeek1.Text.Trim() == "" || Convert.ToDouble(txtWeek1.Text.Trim()) <= 0) || (txtWeek3.Text.Trim() == "" || Convert.ToDouble(txtWeek3.Text.Trim()) <= 0) || (txtWeek4.Text.Trim() == "" || Convert.ToDouble(txtWeek4.Text.Trim()) <= 0))
        //{
        //    ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
        //    txtAskingRate.Focus();
        //    return;
        //}

        if ((txtAskingRate.Text.Trim() == "") || (txtWeek2.Text.Trim() == "") || (txtWeek1.Text.Trim() == "") || (txtWeek3.Text.Trim() == "") || (txtWeek4.Text.Trim() == ""))
        {
            ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtWeek1.Focus();
            return;
        }
        if ((Convert.ToDouble(txtWeek2.Text.Trim()) + Convert.ToDouble(txtWeek1.Text.Trim()) + Convert.ToDouble(txtWeek3.Text.Trim()) + Convert.ToDouble(txtWeek4.Text.Trim())) <= 0)
        {
            ShowMessage("#Avisos", "Please Insert Schedule Atleast for One Week...", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtWeek1.Focus();
            return;
        }
        #endregion

        SaveRec();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            DataTable dt = new DataTable();
            DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);

            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dt = CommonClasses.Execute("SELECT CS_CODE FROM CUSTOMER_SCHEDULE WHERE CS_P_CODE='" + ddlCustomer.SelectedValue + "' AND  CS_I_CODE= '" + ddlItemName.SelectedValue + "' AND CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO CUSTOMER_SCHEDULE (CS_CM_CODE,CS_COMP_ID,CS_P_CODE,CS_I_CODE,CS_SCHEDULE_QTY,CS_WEEK1,CS_WEEK2,CS_WEEK3,CS_WEEK4,CS_DATE,CS_ASKING_RATE,CS_SALES_RATE,CS_INTERNAL_FINISH_WEIGHT)values('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlCustomer.SelectedValue + "','" + ddlItemName.SelectedValue + "','" + txtScheduleQty.Text + "','" + txtWeek1.Text + "','" + txtWeek2.Text + "','" + txtWeek3.Text + "','" + txtWeek4.Text + "','" + dtMonth.ToString("dd/MMM/yyyy") + "'," + txtAskingRate.Text + "," + txtsalesRate.Text + "," + txtIntFinishwgt.Text + ")"))
                    {
                        string Code = CommonClasses.GetMaxId("SELECT MAX(CS_CODE) FROM CUSTOMER_SCHEDULE");
                        CommonClasses.WriteLog("CUSTOMER SCHEDULE", "Save", "CUSTOMER SCHEDULE", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCustomerSchedule.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                dt = CommonClasses.Execute("SELECT * FROM CUSTOMER_SCHEDULE WHERE ES_DELETE=0 AND CS_CODE!= '" + ViewState["mlCode"] + "' and CS_P_CODE='" + ddlCustomer.SelectedValue + "' and CS_I_CODE='" + ddlItemName.SelectedValue + "'  and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'"); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE CUSTOMER_SCHEDULE SET CS_P_CODE='" + ddlCustomer.SelectedValue + "',CS_I_CODE='" + ddlItemName.SelectedValue + "',CS_SCHEDULE_QTY='" + txtScheduleQty.Text + "',CS_WEEK1='" + txtWeek1.Text + "',CS_WEEK2='" + txtWeek2.Text + "',CS_WEEK3='" + txtWeek3.Text + "',CS_WEEK4='" + txtWeek4.Text + "',CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "',CS_ASKING_RATE=" + txtAskingRate.Text + ",CS_SALES_RATE=" + txtsalesRate.Text + ",CS_INTERNAL_FINISH_WEIGHT=" + txtIntFinishwgt.Text + " WHERE CS_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0"))
                    {
                        CommonClasses.RemoveModifyLock("CUSTOMER_SCHEDULE", "MODIFY", "CS_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("CUSTOMER SCHEDULE", "Update", "CUSTOMER SCHEDULE", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCustomerSchedule.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                }
            }
            #endregion MODIFY

            #region Amend
            else if (Request.QueryString[0].Equals("Amend"))
            {
                int count = 0;
                string Code = "";
                DataTable dtcount = new DataTable();
                dtcount = CommonClasses.Execute("select ISNULL(MAX(CS_AMEND_COUNT),0) AS CS_AMEND_COUNT from CUSTOMER_SCHEDULE WHERE ES_DELETE=0 AND CS_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_CODE='" + ViewState["mlCode"] + "'");
                if (dtcount.Rows.Count > 0)
                {
                    count = Convert.ToInt32(dtcount.Rows[0]["CS_AMEND_COUNT"].ToString());
                    count = count + 1;
                }
                else
                {
                    count = count + 1;
                }
                CommonClasses.Execute1("update CUSTOMER_SCHEDULE set CS_AMEND_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',CS_AMEND_COUNT='" + count + "' WHERE CS_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0");
                if (CommonClasses.Execute1("insert into CUSTOMER_SCHEDULE_AMENDMENT select CS_CODE,CS_COMP_ID,CS_DATE,CS_P_CODE,CS_I_CODE,CS_SCHEDULE_QTY,CS_WEEK1,CS_WEEK2,CS_WEEK3,CS_WEEK4,CS_AMEND_COUNT,CS_AMEND_DATE,ES_DELETE,MODIFY,CS_ASKING_RATE FROM CUSTOMER_SCHEDULE WHERE CS_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0"))
                {
                    CommonClasses.Execute1("update CUSTOMER_SCHEDULE set CS_P_CODE='" + ddlCustomer.SelectedValue + "',CS_I_CODE='" + ddlItemName.SelectedValue + "',CS_SCHEDULE_QTY='" + txtScheduleQty.Text + "',CS_WEEK1='" + txtWeek1.Text + "',CS_WEEK2='" + txtWeek2.Text + "',CS_WEEK3='" + txtWeek3.Text + "',CS_WEEK4='" + txtWeek4.Text + "',CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "',CS_ASKING_RATE=" + txtAskingRate.Text + " WHERE CS_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0");
                    //Code = CommonClasses.GetMaxId("select Max(AM_CS_CODE) from CUSTOMER_SCHEDULE_AMENDMENT ");
                    CommonClasses.RemoveModifyLock("CUSTOMER_SCHEDULE", "MODIFY", "CS_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("CUSTOMER SCHEDULE", "Amend", "CUSTOMER SCHEDULE", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/PPC/VIEW/ViewCustomerSchedule.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                }
            }
            #endregion Amend
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER SCHEDULE", "SaveRec", ex.Message);
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
            CommonClasses.SendError("CUSTOMER_SCHEDULE", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
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
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER SCHEDULE", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("CUSTOMER_SCHEDULE", "MODIFY", "CS_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewCustomerSchedule.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER SCHEDULE", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlItemName.SelectedIndex == -1)
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
            CommonClasses.SendError("CUSTOMER SCHEDULE", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtRatewgt = new DataTable();
        //Only Active sales Order records are taken here
        dtRatewgt = CommonClasses.Execute("SELECT CPOD_CPOM_CODE,CPOM_P_CODE,CPOD_I_CODE,CPOD_RATE,I_TARGET_WEIGHT FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER WHERE CPOM_CODE=CPOD_CPOM_CODE AND I_CODE=CPOD_I_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOD_I_CODE=" + ddlItemName.SelectedValue + " AND ITEM_MASTER.ES_DELETE=0 AND CPOD_STATUS=0");
        txtsalesRate.Text = Convert.ToDouble(dtRatewgt.Rows[0]["CPOD_RATE"]).ToString();
        txtIntFinishwgt.Text = Convert.ToDouble(dtRatewgt.Rows[0]["I_TARGET_WEIGHT"]).ToString();
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.Items.Clear();
        DataTable dtCustItem = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        //dtCustItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from CUSTPO_MASTER inner join CUSTPO_detail on CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and datepart(MONTH, CPOM_DATE)='" + dtMonth.ToString("MM") + "' and datepart(year, CPOM_DATE)='" + dtMonth.ToString("yyyy") + "'");
        dtCustItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME FROM CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' ");
        ddlItemName.DataSource = dtCustItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion ddlCustomer_SelectedIndexChanged

    //txtScheduleQty_TextChanged
    #region txtScheduleQty_TextChanged
    protected void txtScheduleQty_TextChanged(object sender, EventArgs e)
    {
        txtAskingRate.Text = Math.Round(Convert.ToDouble(GetDoubleValue(txtScheduleQty.Text) / 20), 0).ToString();
        Total();
    }
    #endregion txtScheduleQty_TextChanged

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        // dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_COSTING_HEAD='FINISH GOOD' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        //dtFinishItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from PARTY_MASTER INNER JOIN CUSTPO_MASTER ON P_CODE = CPOM_P_CODE inner join CUSTPO_detail on CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE PARTY_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' ");  //and datepart(MONTH, CPOM_DATE)='" + dtMonth.ToString("MM") + "' and datepart(YEAR, CPOM_DATE)='" + dtMonth.ToString("yyyy") + "'

        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO + ' - ' + ITEM_MASTER.I_NAME AS ICODE_INAME   FROM STANDARD_INVENTARY_MASTER INNER JOIN ITEM_MASTER ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER INNER JOIN CUSTPO_MASTER ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = CUSTPO_DETAIL.CPOD_I_CODE  WHERE     (STANDARD_INVENTARY_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.CPOM_CM_COMP_ID = '" + Session["CompanyId"] + "' ) AND (CUSTPO_DETAIL.CPOD_STATUS = 0) ORDER BY ICODE_INAME");
        ddlItemName.DataSource = dtFinishItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItem

    #region GetDoubleValue
    public double GetDoubleValue(string input)
    {
        double val;
        if (!double.TryParse(input, out val))
        {
            return val;
        }
        return double.Parse(input);
    }
    #endregion

    #region LoadCustomer
    protected void LoadCustomer()
    {
        DataTable dtCustomer = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        //dtCustomer = CommonClasses.Execute("select DISTINCT P_CODE,P_NAME from PARTY_MASTER INNER JOIN CUSTPO_MASTER ON P_CODE = CPOM_P_CODE inner join CUSTPO_detail on CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE PARTY_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and datepart(MONTH, CPOM_DATE)='" + dtMonth.ToString("MM") + "' and datepart(YEAR, CPOM_DATE)='" + dtMonth.ToString("yyyy") + "' order by P_NAME");
        dtCustomer = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM STANDARD_INVENTARY_MASTER INNER JOIN ITEM_MASTER ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER INNER JOIN CUSTPO_MASTER ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = CUSTPO_DETAIL.CPOD_I_CODE  WHERE     (STANDARD_INVENTARY_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.CPOM_CM_COMP_ID = '" + Session["CompanyId"] + "' ) AND (CUSTPO_DETAIL.CPOD_STATUS = 0) ORDER BY P_NAME");
        ddlCustomer.DataSource = dtCustomer;
        ddlCustomer.DataTextField = "P_NAME";
        ddlCustomer.DataValueField = "P_CODE";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
    }
    #endregion LoadCustomer

    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        LoadCustomer();
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtMonth.Focus();
            LoadCustomer();
            return;
        }
    }
    #endregion txtMonth_TextChanged

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

    #region txtWeek1_TextChanged
    protected void txtWeek1_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek1.Text);
        txtWeek1.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 3));
        Total();
    }
    #endregion txtWeek1_TextChanged

    #region txtWeek2_TextChanged
    protected void txtWeek2_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek2.Text);
        txtWeek2.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 3));
        Total();
    }
    #endregion txtWeek2_TextChanged

    #region txtWeek3_TextChanged
    protected void txtWeek3_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek3.Text);
        txtWeek3.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 3));
        Total();
    }
    #endregion txtWeek3_TextChanged

    #region txtWeek4_TextChanged
    protected void txtWeek4_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek4.Text);
        txtWeek4.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 3));
        Total();
    }
    #endregion txtWeek4_TextChanged

    #region Total
    public void Total()
    {
        double Tot = 0, Schedule_Qty = 0;
        #region Validate
        if (txtScheduleQty.Text.Trim() == "" || txtScheduleQty.Text.Trim() == ".")
            txtScheduleQty.Text = "0.00";
        else
            Schedule_Qty = Convert.ToDouble(txtScheduleQty.Text);
        if (txtWeek1.Text.Trim() == "" || txtWeek1.Text.Trim() == ".")
            txtWeek1.Text = "0";
        if (txtWeek2.Text == "" || txtWeek2.Text.Trim() == ".")
            txtWeek2.Text = "0";
        if (txtWeek3.Text == "" || txtWeek3.Text.Trim() == ".")
            txtWeek3.Text = "0";
        if (txtWeek4.Text == "" || txtWeek4.Text.Trim() == ".")
            txtWeek4.Text = "0";
        #endregion Validate

        if (txtWeek1.Text != "" && txtWeek2.Text != "" && txtWeek3.Text != "" && txtWeek4.Text != "")
        {
            Tot = Convert.ToDouble(txtWeek1.Text) + Convert.ToDouble(txtWeek2.Text) + Convert.ToDouble(txtWeek3.Text) + Convert.ToDouble(txtWeek4.Text);
        }

        if (Tot >= Schedule_Qty)
        {
            txtScheduleQty.Text = Tot.ToString();
            txtAskingRate.Text = Math.Round(Convert.ToDouble(GetDoubleValue(txtScheduleQty.Text) / 20), 0).ToString();
        }
        else
        {
            ShowMessage("#Avisos", "Total Qty should be match with Schedule Qty...", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlItemName.Focus();
            txtWeek1.Text = "0"; txtWeek2.Text = "0"; txtWeek3.Text = "0"; txtWeek4.Text = "0"; txtScheduleQty.Text = "0";
            return;
        }
    }
    #endregion Total
}
