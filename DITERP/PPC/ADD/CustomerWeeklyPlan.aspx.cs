using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_CustomerWeeklyPlan : System.Web.UI.Page
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
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT DISTINCT CWP_CODE, P_CODE,P_NAME,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME, right(convert(varchar, C.CWP_DATE, 106), 8) as CWP_DATE ,isnull(CWP_CUSTOMER_STOCK,0) as CWP_CUSTOMER_STOCK,isnull(CWP_SALE_QTY,0) as CWP_SALE_QTY,isnull(CWP_WEEK,0) as CWP_WEEK FROM CUSTOMER_WEEKLY_PLAN AS C INNER JOIN ITEM_MASTER AS I ON C.CWP_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.CWP_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.CWP_COMP_ID='" + (string)Session["CompanyId"] + "' AND CWP_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtMonth.Text = dt.Rows[0]["CWP_DATE"].ToString();
                LoadItem(); LoadCustomer();
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                ddlCustomer.SelectedValue = dt.Rows[0]["P_CODE"].ToString();
                ddlWeek.SelectedValue = dt.Rows[0]["CWP_WEEK"].ToString();
                txtCustomer_Stock.Text = dt.Rows[0]["CWP_CUSTOMER_STOCK"].ToString();
                txtWeekly_Sale_Qty.Text = dt.Rows[0]["CWP_SALE_QTY"].ToString();

                if (str == "VIEW")
                {
                    ddlWeek.Enabled = false; ddlItemName.Enabled = false; ddlCustomer.Enabled = false; txtCustomer_Stock.Enabled = false;
                    txtWeekly_Sale_Qty.Enabled = false;
                    btnSubmit.Visible = false;
                }
                ddlItemName.Enabled = false;
                ddlWeek.Enabled = false;
                ddlCustomer.Enabled = false;
                txtMonth.Enabled = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CUSTOMER_WEEKLY_PLAN", "MODIFY", "CWP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlWeek.SelectedIndex == -1 || ddlWeek.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Week", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        if (ddlCustomer.SelectedIndex == -1 || ddlCustomer.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Customer Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        if (ddlItemName.SelectedIndex == -1 || ddlItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        /* Zero validation remove :- Deshpande sir 12072018*/
        //if ((txtCustomer_Stock.Text == "" || Convert.ToInt32(txtCustomer_Stock.Text.Trim()) <= 0) || (txtWeekly_Sale_Qty.Text == "" || Convert.ToInt32(txtWeekly_Sale_Qty.Text.Trim()) <= 0))
        //{
        //    ShowMessage("#Avisos", "Please Enter Number", CommonClasses.MSG_Warning);
        //    txtCustomer_Stock.Focus();
        //    return;
        //}
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
                dt = CommonClasses.Execute("Select CWP_CODE FROM CUSTOMER_WEEKLY_PLAN WHERE CWP_P_CODE='" + ddlCustomer.SelectedValue + "' and  CWP_I_CODE= '" + ddlItemName.SelectedValue + "' and ES_DELETE='False' and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND CWP_WEEK='" + ddlWeek.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into CUSTOMER_WEEKLY_PLAN ([CWP_COMP_CODE],[CWP_COMP_ID],[CWP_DATE],[CWP_WEEK],[CWP_P_CODE],[CWP_I_CODE],[CWP_CUSTOMER_STOCK],[CWP_SALE_QTY])values('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + dtMonth.ToString("dd/MMM/yyyy") + "','" + ddlWeek.SelectedValue + "','" + ddlCustomer.SelectedValue + "','" + ddlItemName.SelectedValue + "','" + txtCustomer_Stock.Text + "','" + txtWeekly_Sale_Qty.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CWP_CODE) from CUSTOMER_WEEKLY_PLAN");
                        CommonClasses.WriteLog("CUSTOMER_WEEKLY_PLAN", "Save", "CUSTOMER_WEEKLY_PLAN", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCustomerWeeklyPlan.aspx", false);
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
                dt = CommonClasses.Execute("SELECT * FROM CUSTOMER_WEEKLY_PLAN WHERE ES_DELETE=0 AND CWP_CODE!= '" + ViewState["mlCode"] + "' and CS_P_CODE='" + ddlCustomer.SelectedValue + "' and CS_I_CODE='" + ddlItemName.SelectedValue + "' and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND CWP_WEEK='" + ddlWeek.SelectedValue + "'"); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update CUSTOMER_WEEKLY_PLAN set [CWP_COMP_ID]='" + Convert.ToInt32(Session["CompanyId"]) + "',[CWP_DATE]='" + dtMonth.ToString("dd/MMM/yyyy") + "',[CWP_WEEK]='" + ddlWeek.SelectedValue + "',[CWP_P_CODE]='" + ddlCustomer.SelectedValue + "',[CWP_I_CODE]='" + ddlItemName.SelectedValue + "',[CWP_CUSTOMER_STOCK]='" + txtCustomer_Stock.Text + "',[CWP_SALE_QTY]='" + txtWeekly_Sale_Qty.Text + "' WHERE CWP_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0"))
                    {
                        CommonClasses.RemoveModifyLock("CUSTOMER_WEEKLY_PLAN", "MODIFY", "CWP_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("CUSTOMER_WEEKLY_PLAN", "Update", "CUSTOMER_WEEKLY_PLAN", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCustomerWeeklyPlan.aspx", false);
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
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "SaveRec", ex.Message);
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
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("CUSTOMER_WEEKLY_PLAN", "MODIFY", "CWP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewCustomerWeeklyPlan.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("CUSTOMER_WEEKLY_PLAN", "CheckValid", Ex.Message);
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
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.Items.Clear();
        DataTable dtCustItem = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        //dtCustItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from CUSTPO_MASTER inner join CUSTPO_detail on CPOM_CODE=CPOD_CPOM_CODE INNER JOIN ITEM_MASTER ON CPOD_I_CODE=I_CODE WHERE CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and datepart(MONTH, CPOM_DATE)='" + dtMonth.ToString("MM") + "' and datepart(year, CPOM_DATE)='" + dtMonth.ToString("yyyy") + "'");
        //dtCustItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from PARTY_MASTER inner join CUSTOMER_SCHEDULE on P_CODE=CS_P_CODE inner join ITEM_MASTER on CS_I_CODE=I_CODE where P_CM_COMP_ID='" + Session["CompanyId"] + "' and CUSTOMER_SCHEDULE.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 AND PARTY_MASTER.P_CODE='" + ddlCustomer.SelectedValue + "' AND CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'  order by I_CODENO +' - '+ I_NAME");
        dtCustItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from PARTY_MASTER inner join CUSTOMER_SCHEDULE on P_CODE=CS_P_CODE inner join ITEM_MASTER on CS_I_CODE=I_CODE inner join PRODUCT_MASTER on CS_I_CODE=PROD_I_CODE where P_CM_COMP_ID='" + Session["CompanyId"] + "' and CUSTOMER_SCHEDULE.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 AND PARTY_MASTER.P_CODE='" + ddlCustomer.SelectedValue + "'  and PRODUCT_MASTER.ES_DELETE=0 AND CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' order by I_CODENO +' - '+ I_NAME "); //and PROD_MACHINE_LOC IN('INHOUSE','BOTH') 
        ddlItemName.DataSource = dtCustItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion ddlCustomer_SelectedIndexChanged

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER INNER JOIN CUSTOMER_SCHEDULE CS ON I_CODE=CS_I_CODE where CS.ES_DELETE=0 and  ITEM_MASTER.ES_DELETE=0 AND CS.CS_DATE='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' AND I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
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
        dtCustomer = CommonClasses.Execute("select DISTINCT P_CODE,P_NAME from PARTY_MASTER P INNER JOIN CUSTOMER_SCHEDULE CS on P_CODE=CS.CS_P_CODE where CS.ES_DELETE=0 AND P.ES_DELETE=0 AND P.P_CM_COMP_ID='" + Session["CompanyId"] + "' AND CS.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' order by P.P_NAME");
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
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtMonth.Focus();
            return;
        }
        LoadCustomer(); LoadItem();
    }
    #endregion txtMonth_TextChanged
}
