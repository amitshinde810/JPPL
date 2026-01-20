using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PPC_ADD_unplanSalesSchedule : System.Web.UI.Page
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
                        ViewState["InvQty"] = "0";
                        txtMonth.Attributes.Add("readonly", "readonly");
                        txtMonth.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        txtMonth.Enabled = false;
                        LoadItem(); //LoadCustomer(); // Call Method
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
                        CommonClasses.SendError("unplan Sales Schedule", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute(" SELECT convert(varchar,  US_DATE, 106) AS US_DATE, US_CODE,US_I_CODE,US_QTY FROM UNPLAN_SALE_SCHEDULE WHERE ES_DELETE = 0  AND US_CODE= '" + ViewState["mlCode"] + "'  ");
            if (dt.Rows.Count > 0)
            {
                txtMonth.Text = dt.Rows[0]["US_DATE"].ToString();
                LoadItem();
                ddlItemName.SelectedValue = dt.Rows[0]["US_I_CODE"].ToString();

                txtScheduleQty.Text = dt.Rows[0]["US_QTY"].ToString();

                if (str == "VIEW")
                {
                    ddlItemName.Enabled = false; txtScheduleQty.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    ViewState["InvQty"] = "0";
                    DataTable dtsales = CommonClasses.Execute("SELECT ISNULL(SUM(IND_INQTY),0)   AS IND_INQTY FROM INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_DATE='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' AND IND_I_CODE='" + ddlItemName.SelectedValue + "'");
                    if (dtsales.Rows.Count > 0)
                    {
                        ViewState["InvQty"] = dtsales.Rows[0]["IND_INQTY"].ToString();
                    }
                }
                ddlItemName.Enabled = false;

                txtMonth.Enabled = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("UNPLAN_SALE_SCHEDULE", "MODIFY", "US_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation



        if (ddlItemName.SelectedIndex == -1 || ddlItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Enter Part Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }


        //if ((txtAskingRate.Text.Trim() == "" || Convert.ToDouble(txtAskingRate.Text.Trim()) <= 0) || (txtWeek2.Text.Trim() == "" || Convert.ToDouble(txtWeek2.Text.Trim()) <= 0) || (txtWeek1.Text.Trim() == "" || Convert.ToDouble(txtWeek1.Text.Trim()) <= 0) || (txtWeek3.Text.Trim() == "" || Convert.ToDouble(txtWeek3.Text.Trim()) <= 0) || (txtWeek4.Text.Trim() == "" || Convert.ToDouble(txtWeek4.Text.Trim()) <= 0))
        //{
        //    ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
        //    txtAskingRate.Focus();
        //    return;
        //}
        if (Convert.ToDouble(ViewState["InvQty"]) > Convert.ToDouble(txtScheduleQty.Text))
        {
            ShowMessage("#Avisos", "Please Entry Quantity Grater than " + ViewState["InvQty"].ToString(), CommonClasses.MSG_Warning);
            txtScheduleQty.Focus();
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
                dt = CommonClasses.Execute("Select US_CODE FROM UNPLAN_SALE_SCHEDULE WHERE    US_I_CODE= '" + ddlItemName.SelectedValue + "' and US_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO UNPLAN_SALE_SCHEDULE (US_CM_CODE, US_DATE, US_I_CODE, US_QTY)values('" + Convert.ToInt32(Session["CompanyCode"]) + "', '" + dtMonth.ToString("dd/MMM/yyyy") + "' , '" + ddlItemName.SelectedValue + "','" + txtScheduleQty.Text + "' )"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(US_CODE) from UNPLAN_SALE_SCHEDULE");
                        CommonClasses.WriteLog("unplan Sales Schedule", "Save", "unplan Sales Schedule", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewunplanSalesSchedule.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
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
                dt = CommonClasses.Execute("SELECT * FROM UNPLAN_SALE_SCHEDULE WHERE ES_DELETE=0 AND US_CODE!= '" + ViewState["mlCode"] + "'   and US_I_CODE='" + ddlItemName.SelectedValue + "'   "); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update UNPLAN_SALE_SCHEDULE set   US_QTY='" + txtScheduleQty.Text + "'   WHERE US_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0"))
                    {
                        CommonClasses.RemoveModifyLock("UNPLAN_SALE_SCHEDULE", "MODIFY", "US_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("unplan Sales Schedule", "Update", "unplan Sales Schedule", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewunplanSalesSchedule.aspx", false);
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


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "SaveRec", ex.Message);
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
            CommonClasses.SendError("unplan Sales Schedule", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("unplan Sales Schedule", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("UNPLAN_SALE_SCHEDULE", "MODIFY", "US_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewunplanSalesSchedule.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("unplan Sales Schedule", "CheckValid", Ex.Message);
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
        //ddlCustomer.Items.Clear();
        //DataTable dtParty = new DataTable();
        //DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        ////dtParty = CommonClasses.Execute("select DISTINCT P_CODE,P_NAME from PARTY_MASTER INNER JOIN CUSTPO_MASTER ON P_CODE = CPOM_P_CODE inner join CUSTPO_detail on CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE PARTY_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND I_CODE='" + ddlItemName.SelectedValue + "' and datepart(MONTH, CPOM_DATE)='" + dtMonth.ToString("MM") + "' and datepart(YEAR, CPOM_DATE)='" + dtMonth.ToString("yyyy") + "'");
        //dtParty = CommonClasses.Execute(" SELECT DISTINCT P_CODE,P_NAME   FROM STANDARD_INVENTARY_MASTER INNER JOIN ITEM_MASTER ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER INNER JOIN CUSTPO_MASTER ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE ON STANDARD_INVENTARY_MASTER.SIM_I_CODE = CUSTPO_DETAIL.CPOD_I_CODE  WHERE     (STANDARD_INVENTARY_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.CPOM_CM_COMP_ID = '" + Session["CompanyId"] + "' ) AND I_CODE='" + ddlItemName.SelectedValue + "'  AND (CUSTPO_DETAIL.CPOD_STATUS = 0) ORDER BY P_NAME");

        //ddlCustomer.DataSource = dtParty;
        //ddlCustomer.DataTextField = "P_NAME";
        //ddlCustomer.DataValueField = "P_CODE";
        //ddlCustomer.DataBind();
        //ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
    }
    #endregion ddlItemName_SelectedIndexChanged



    //txtScheduleQty_TextChanged
    #region txtScheduleQty_TextChanged
    protected void txtScheduleQty_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion txtScheduleQty_TextChanged

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);

        dtFinishItem = CommonClasses.Execute("  SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO + ' - ' + ITEM_MASTER.I_NAME AS ICODE_INAME  from ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE AND CPOD_STATUS=0 AND CPOD_I_CODE=I_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND  I_CAT_CODE=-2147483648 AND I_CODE NOT IN (SELECT CS_I_CODE FROM CUSTOMER_SCHEDULE where ES_DELETE=0 AND CS_DATE='" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "') AND (CUSTPO_MASTER.CPOM_CM_COMP_ID = '" + Session["CompanyId"] + "' ) AND (CUSTPO_DETAIL.CPOD_STATUS = 0) ORDER BY ICODE_INAME");
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



    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        ;
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtMonth.Focus();

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



    #region Total
    public void Total()
    {
        double Tot = 0, Schedule_Qty = 0;
        #region Validate
        if (txtScheduleQty.Text.Trim() == "" || txtScheduleQty.Text.Trim() == ".")
            txtScheduleQty.Text = "0.00";
        else
            Schedule_Qty = Convert.ToDouble(txtScheduleQty.Text);

        #endregion Validate


        if (Tot >= Schedule_Qty)
        {
            txtScheduleQty.Text = Tot.ToString();
        }
        else
        {
            ShowMessage("#Avisos", "Total Qty should be match with Schedule Qty...", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
    }
    #endregion Total
}
