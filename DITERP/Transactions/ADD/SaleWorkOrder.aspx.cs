using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Transactions_ADD_SaleWorkOrder : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    BillPassing_BL billPassing_BL = null;
    static int mlCode = 0;
    static string right = "";
    public static string str = "";
    static DataTable dt2 = new DataTable();
    DataTable dtBillPassing = new DataTable();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
          
            if (!IsPostBack)
            {

                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='94'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadType();
                    LoadCustomer();
                    LoadSalesOrderNo();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                      
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());                      
                        ViewRec("VIEW");
                      
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                     
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());                     
                        ViewRec("MOD");
                      
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtOrderdate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                       
                        dt2.Rows.Clear();
                        str = "";                     
                        LoadFilter();
                    }
                    ddlPOType.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Sales-Work Order", "PageLoad", ex.Message);
                }
            }
        }

    }
    #endregion

    #region Event

    #region GirdEvent
    protected void dgBillPassing_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBillPassing.PageIndex = e.NewPageIndex;
            LoadOrderDetail();
        }
        catch (Exception)
        {
        }

    }

    protected void dgBillPassing_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    #region ButtonEvent

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                //CancelRecord();
                Response.Redirect("~/Transactions/VIEW/ViewSaleWorkOrder.aspx", false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
                int flag = 0;
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        flag++;
                    }
                }
                if (flag == 0)
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please, Select Item";
                    return;
                }
                int count = 0;
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    if (SaveRec())
                    {

                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Info);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
                    
               
                ddlCustomer.Focus();
            }
            else
            {
                ShowMessage("#Avisos", "You have no rights to Save", CommonClasses.MSG_Info);

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "You have no rights to Save";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "btnSubmit_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Cancel Record
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("WORK_ORDER_MASTER", "MODIFY", "WO_CODE", mlCode);
            }
            Response.Redirect("~/Transactions/VIEW/ViewSaleWorkOrder.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Work Order", "CancelRecord", ex.Message);
        }
    }
    #endregion

    #region btnOK_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec();
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Work Order", "btnOK_Click", ex.Message);
        }
    }

    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region Check Validation
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtOrderdate.Text == "")
            {
                flag = false;
            }
            else if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlPOType.Text == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Work Order", "CheckValid", ex.Message);
        }
        return flag;
    }
    #endregion

    #endregion

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;
          
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Work Order", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedIndex != 0)
            {
                LoadSalesOrderNo();
            }
            else if (ddlCustomer.SelectedIndex == 0)
            {
                dtFilter.Clear();
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_CPOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_ORD_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("WORK_ORD_QTY", typeof(String)));        

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
                ddlSaleOrderNo.SelectedIndex = 0;
                ddlSaleOrderNo.Enabled = false;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion


    #region ddlSaleOrderNo_SelectedIndexChanged
    protected void ddlSaleOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSaleOrderNo.SelectedIndex != 0)
            {
                LoadOrderDetail();
            }
            else if (ddlCustomer.SelectedIndex == 0)
            {
                dtFilter.Clear();
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_CPOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_ORD_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("WORK_ORD_QTY", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "ddlSaleOrderNo_SelectedIndexChanged", Ex.Message);
        }
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
            // int n = no - 1;
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

    #region User Defined Method
     
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

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            LoadType();
            LoadCustomer();
            //LoadSalesOrderNo();
            dt = CommonClasses.Execute("select WO_CODE,WO_NO,CONVERT(varchar,WO_DATE,106) as WO_DATE,WO_TYPE,WO_P_CODE,WO_CPOM_CODE  from WORK_ORDER_MASTER WHERE WO_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["WO_CODE"]);
                txtOrderNo.Text = Convert.ToInt32(dt.Rows[0]["WO_NO"]).ToString();
                txtOrderdate.Text = Convert.ToDateTime(dt.Rows[0]["WO_DATE"]).ToString("dd MMM yyyy");
                ddlPOType.Text = dt.Rows[0]["WO_TYPE"].ToString();
                ddlCustomer.SelectedValue = dt.Rows[0]["WO_P_CODE"].ToString();
                ddlSaleOrderNo.SelectedValue = dt.Rows[0]["WO_CPOM_CODE"].ToString();

                dtBillPassing = CommonClasses.Execute("select CPOD_CPOM_CODE,CPOD_I_CODE,I_CODENO,I_NAME,cast(CPOD_ORD_QTY as numeric(10,3)) as CPOD_ORD_QTY,cast(WOD_WORK_ORDER_QTY as numeric(10,3)) as WORK_ORD_QTY,cast(CPOD_ORD_QTY as numeric(10,3)) as WORK_BAL_QTY from CUSTPO_DETAIL,ITEM_MASTER,CUSTPO_MASTER,WORK_ORDER_MASTER,WORK_ORDER_DETAIL where CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE=WOD_I_CODE and WOD_I_CODE=I_CODE and WO_CODE=WOD_WO_CODE and WOD_CPOM_CODE=CPOM_CODE and WO_CODE='" + mlCode + "'");

                if (dtBillPassing.Rows.Count != 0)
                {
                    dgBillPassing.DataSource = dtBillPassing;
                    dgBillPassing.DataBind();
                    dt2 = dtBillPassing;
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        TextBox txtRow = (((TextBox)(dgBillPassing.Rows[i].FindControl("txtWORK_ORD_QTY"))) as TextBox);
                        chkRow.Checked = true;
                        
                        if (chkRow.Checked)
                        {
                            DataTable dt1 = CommonClasses.Execute("select distinct BT_CODE FROM BATCH_MASTER INNER JOIN BATCH_DETAIL ON BT_CODE=BTD_BT_CODE WHERE BT_WO_CODE='" + mlCode + "' AND BT_WOD_I_CODE='" + (((Label)(dgBillPassing.Rows[i].FindControl("lblICPOD_I_CODE"))) as Label).Text + "' and es_delete=0");
                            if (dt1.Rows.Count > 0)
                            {
                                chkRow.Enabled = false;
                                txtRow.Enabled = false;
                            }
                            //chkRow.Enabled = false;
                        }
                    }
                }


            }
            if (str == "MOD")
            {
                ddlPOType.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlSaleOrderNo.Enabled = false;
                CommonClasses.SetModifyLock("WORK_ORDER_MASTER", "MODIFY", "WO_CODE", mlCode);

            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "ViewRec", Ex.Message);
        }
    }
    #endregion


    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                // if (!CommonClasses.CheckExistSave("CUSTPO_MASTER", "CPOM_WORK_ODR_NO", txtOrderNo.Text))
                DataTable dtworkorder = CommonClasses.Execute("select * from WORK_ORDER_MASTER WHERE WO_NO='" + txtOrderNo.Text + "' and WO_CM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                if (dtworkorder.Rows.Count == 0)
                {
                    int Po_Doc_no = 0;
                    DataTable dt = new DataTable();
                    dt = CommonClasses.Execute("Select isnull(max(WO_NO),0) as WO_NO FROM WORK_ORDER_MASTER WHERE WO_CM_COMP_CODE = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                    if (dt.Rows.Count > 0)
                    {
                        Po_Doc_no = Convert.ToInt32(dt.Rows[0]["WO_NO"]);
                        Po_Doc_no = Po_Doc_no + 1;
                    }
                    if (CommonClasses.Execute1("INSERT INTO WORK_ORDER_MASTER (WO_CM_COMP_CODE,WO_NO,WO_DATE,WO_TYPE,WO_P_CODE,ES_DELETE,MODIFY,WO_CPOM_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Po_Doc_no + "','" + Convert.ToDateTime(txtOrderdate.Text).ToString("dd/MMM/yyyy") + "','" + ddlPOType.SelectedValue + "','" + ddlCustomer.SelectedValue + "','0','0','"+ddlSaleOrderNo.SelectedValue+"')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(WO_CODE) from WORK_ORDER_MASTER");
                        
                            for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                            {
                                CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                                if (chkRow.Checked)
                                {
                                    CommonClasses.Execute1("INSERT INTO WORK_ORDER_DETAIL (WOD_WO_CODE,WOD_I_CODE,WOD_CPOM_CODE,WOD_SALE_ORDER_QTY,WOD_WORK_ORDER_QTY) values ('" + Code + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblICPOD_I_CODE")).Text + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_CPOM_CODE")).Text + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_ORD_QTY")).Text + "','" + ((TextBox)dgBillPassing.Rows[i].FindControl("txtWORK_ORD_QTY")).Text + "')");                                    
                                   // CommonClasses.Execute1("update CUSTPO_DETAIL set CPOD_WO_QTY = CPOD_WO_QTY + " +Convert.ToDouble(((TextBox)dgBillPassing.Rows[i].FindControl("txtWORK_ORD_QTY")).Text) + " where CPOD_CPOM_CODE='" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_CPOM_CODE")).Text + "' and CPOD_I_CODE='" + ((Label)dgBillPassing.Rows[i].FindControl("lblICPOD_I_CODE")).Text + "'");
                                }
                            }                      
                        CommonClasses.WriteLog("Sales Work Order", "Save", "Sales Work Order Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        dt2.Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewSaleWorkOrder.aspx", false);

                    }
                    else
                    {

                        ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                        txtOrderdate.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Order No is already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Select Customer Name";
                    txtOrderNo.Focus();

                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE WORK_ORDER_MASTER SET WO_DATE='" + Convert.ToDateTime(txtOrderdate.Text).ToString("dd/MMM/yyyy") + "',WO_NO='" + txtOrderNo.Text + "',WO_TYPE='" + ddlPOType.SelectedValue + "',WO_P_CODE='" + ddlCustomer.SelectedValue + "',WO_CPOM_CODE='"+ddlSaleOrderNo.SelectedValue+"' where WO_CODE='" + mlCode + "'"))
                {
                    //DataTable dtOld = CommonClasses.Execute("select CPOD_CPOM_CODE,CPOD_I_CODE,I_CODENO,I_NAME,cast(CPOD_ORD_QTY as numeric(10,3)) as CPOD_ORD_QTY,cast(WOD_WORK_ORDER_QTY as numeric(10,3)) as WORK_ORD_QTY,cast(CPOD_WO_QTY as numeric(10,3)) as WORK_BAL_QTY from CUSTPO_DETAIL,ITEM_MASTER,CUSTPO_MASTER,WORK_ORDER_MASTER,WORK_ORDER_DETAIL where CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE=WOD_I_CODE and WOD_I_CODE=I_CODE and WO_CODE=WOD_WO_CODE and WOD_CPOM_CODE=CPOM_CODE and  WO_CODE='"+mlCode+"' ");

                    //for (int i = 0; i < dtOld.Rows.Count; i++)
                    //{
                    //    CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_WO_QTY = CPOD_WO_QTY - " + dtOld.Rows[i]["WORK_BAL_QTY"] + " where CPOD_CPOM_CODE='" + dtOld.Rows[i]["CPOD_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtOld.Rows[i]["CPOD_I_CODE"] + "'");
                    //}
                    result = CommonClasses.Execute1("DELETE FROM WORK_ORDER_DETAIL WHERE WOD_WO_CODE='" + mlCode + "'");
                    if (result)
                    {

                        for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                        {
                             CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                             if (chkRow.Checked)
                             {
                                 CommonClasses.Execute1("INSERT INTO WORK_ORDER_DETAIL (WOD_WO_CODE,WOD_I_CODE,WOD_CPOM_CODE,WOD_SALE_ORDER_QTY,WOD_WORK_ORDER_QTY) values ('" + mlCode + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblICPOD_I_CODE")).Text + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_CPOM_CODE")).Text + "','" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_ORD_QTY")).Text + "','" + ((TextBox)dgBillPassing.Rows[i].FindControl("txtWORK_ORD_QTY")).Text + "')");
                                 //CommonClasses.Execute1("update CUSTPO_DETAIL set CPOD_WO_QTY = CPOD_WO_QTY + " + Convert.ToDouble(((TextBox)dgBillPassing.Rows[i].FindControl("txtWORK_ORD_QTY")).Text) + " where CPOD_CPOM_CODE='" + ((Label)dgBillPassing.Rows[i].FindControl("lblCPOD_CPOM_CODE")).Text + "' and CPOD_I_CODE='" + ((Label)dgBillPassing.Rows[i].FindControl("lblICPOD_I_CODE")).Text + "'");
                             }                            
                         }
                        
                        CommonClasses.RemoveModifyLock("WORK_ORDER_MASTER", "MODIFY", "WO_CODE", mlCode);
                        CommonClasses.WriteLog("Sales Work Order", "Update", "Sales Work Order", txtOrderNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewSaleWorkOrder.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtOrderdate.Focus();
                }
            }

            //#region Amend
            //else if (Request.QueryString[0].Equals("AMEND"))
            //{
            //    int AMEND_COUNT = 0;
            //    DataTable dt = new DataTable();
            //    DataTable dt1 = new DataTable();
            //    dt = CommonClasses.Execute("select isnull(CPOM_AM_COUNT,0) as CPOM_AM_COUNT from CUSTPO_MASTER WHERE CPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and CPOM_CODE='" + mlCode + "'");
            //    if (dt.Rows.Count > 0)
            //    {
            //        AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["CPOM_AM_COUNT"]);
            //        AMEND_COUNT = AMEND_COUNT + 1;
            //    }
            //    if (AMEND_COUNT == 0)
            //    {
            //        AMEND_COUNT = AMEND_COUNT + 1;
            //    }
            //    CommonClasses.Execute1("update  CUSTPO_MASTER set CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',CPOM_AM_COUNT='" + AMEND_COUNT + "' WHERE CPOM_CODE='" + mlCode + "'");
            //    if (CommonClasses.Execute1("INSERT INTO CUSTPO_AM_MASTER select * from CUSTPO_MASTER where CPOM_CODE='" + mlCode + "' "))
            //    {
            //        string MatserCode = CommonClasses.GetMaxId("Select Max(CPOM_AM_CODE) from CPOM_AM_MASTER");
            //        DataTable dtDetail = CommonClasses.Execute("select * from CUSTPO_DETAIL where CPOD_CPOM_CODE='" + mlCode + "'");
            //        for (int j = 0; j < dtDetail.Rows.Count; j++)
            //        {
            //            CommonClasses.Execute1("INSERT INTO CUSTPO_AMD_DETAIL values('" + dtDetail.Rows[j]["CPOD_CPOM_CODE"] + "','" + dtDetail.Rows[j]["CPOD_I_CODE"] + "','" + dtDetail.Rows[j]["CPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["CPOD_ORD_QTY"] + "','" + dtDetail.Rows[j]["CPOD_RATE"] + "','" + dtDetail.Rows[j]["CPOD_AMT"] + "','" + dtDetail.Rows[j]["CPOD_STATUS"] + "','" + dtDetail.Rows[j]["CPOD_DISPACH"] + "','" + dtDetail.Rows[j]["CPOD_DESC"] + "','" + dtDetail.Rows[j]["CPOD_CUST_I_CODE"] + "','" + dtDetail.Rows[j]["CPOD_CUST_I_NAME"] + "','" + dtDetail.Rows[j]["CPOD_ST_CODE"] + "','" + dtDetail.Rows[j]["CPOD_CURR_CODE"] + "','" + MatserCode + "')");
            //        }

            //        #region ModifyOriginalPO
            //        if (CommonClasses.Execute1("UPDATE CUSTPO_MASTER SET CPOM_P_CODE='" + ddlCustomer.SelectedValue + "',CPOM_PONO='" + txtPONumber.Text + "',CPOM_DOC_NO='" + txtPODocNo.Text + "',CPOM_DATE='" + Convert.ToDateTime(txtPODate.Text).ToString("dd/MMM/yyyy") + "',CPOM_TYPE='" + ddlPOType.SelectedValue + "',CPOM_BASIC_AMT='" + txtBasicAmount.Text + "',CPOM_PAY_TERM='" + txtPayTerm.Text + "',CPOM_FINAL_DEST='" + txtFinalD.Text + "',CPOM_PRE_CARR_BY='" + txtPreCarr.Text + "',CPOM_PORT_LOAD='" + txtPortLoad.Text + "',CPOM_PORT_DIS='" + txtPortDis.Text + "',CPOM_PLACE_DEL='" + txtPlace.Text + "',CPOM_BUYER_NAME='" + txtBuyerName.Text + "',CPOM_BUYER_ADD='" + txtBuyerAdd.Text + "',CPOM_CURR_CODE='" + ddlCurrancy.SelectedValue + "',CPOM_WORK_ODR_NO='" + txtOrderNo.Text + "',CPOM_PO_DATE='" + Convert.ToDateTime(txtCustPoDate.Text).ToString("dd/MMM/yyyy") + "',CPOM_AM_COUNT='" + AMEND_COUNT + "',CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' where CPOM_CODE='" + mlCode + "'"))
            //        {
            //            result = CommonClasses.Execute1("update  CUSTPO_AM_MASTER set CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE CPOM_AM_CODE='" + mlCode + "' and CPOM_AM_COUNT='" + AMEND_COUNT + "'");
            //            result = CommonClasses.Execute1("DELETE FROM CUSTPO_DETAIL WHERE CPOD_CPOM_CODE='" + mlCode + "'");
            //            if (result)
            //            {
            //                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
            //                {
            //                    CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_DISPACH,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_UOM_CODE) values ('" + mlCode + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCPOD_DISPACH")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblI_UOM_CODE")).Text + "')");
            //                }

            //                //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");

            //                CommonClasses.RemoveModifyLock("CUSTPO_MASTER", "MODIFY", "CPOM_CODE", mlCode);
            //                CommonClasses.WriteLog("Customer Order", "Update", "Customer Order", txtPONumber.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
            //                dt2.Rows.Clear();
            //                result = true;
            //            }
            //            Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
            //        }

            //        #endregion
            //    }
            //    else
            //    {
            //        //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);



            //        ShowMessage("#Avisos", "PO Not Amend", CommonClasses.MSG_Warning);
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


            //        ddlPOType.Focus();
            //    }

            //}
            //#endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Oreder", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion
    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(WO_NO) as WO_NO from WORK_ORDER_MASTER ");
        if (dt.Rows[0]["WO_NO"] == null || dt.Rows[0]["WO_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["WO_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region ClearFunction
    public static void ClearTextBoxes(Control p1)
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
                        t.Text = String.Empty;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region LoadOrderDetail
    private void LoadOrderDetail()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                if (ddlCustomer.SelectedIndex != 0)
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        dt = CommonClasses.Execute("select distinct CPOD_CPOM_CODE,CPOD_I_CODE,I_CODENO,I_NAME,cast(CPOD_ORD_QTY as numeric(10,3)) AS CPOD_ORD_QTY, cast(CPOD_ORD_QTY-CPOD_WO_QTY as numeric(10,3)) as WORK_ORD_QTY,cast(CPOD_ORD_QTY-CPOD_WO_QTY as numeric(10,3)) as WORK_BAL_QTY FROM CUSTPO_MASTER ,CUSTPO_DETAIL,ITEM_MASTER where CPOM_CODE = CPOD_CPOM_CODE AND CPOD_I_CODE = I_CODE  AND CUSTPO_MASTER.ES_DELETE = 0 and CPOD_ORD_QTY > CPOD_WO_QTY and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and CPOM_CODE='" + ddlSaleOrderNo.SelectedValue + "' ORDER BY I_NAME ");
                        if (dt.Rows.Count > 0)
                        {
                            dgBillPassing.Enabled = true;
                            dgBillPassing.DataSource = dt;
                            dgBillPassing.DataBind();

                        }
                        else
                        {
                            dgBillPassing.DataSource = null;
                            dgBillPassing.DataBind();
                            LoadFilter();
                            PanelMsg.Visible = true;
                            lblmsg.Text = "This Customer Order Is Not Present";
                            LoadFilter();
                        }

                    }
                }


            }
            catch (Exception ex)
            {
 
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "LoadOrderDetail", Ex.Message);

        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                if (ddlPOType.SelectedIndex != 0 || ddlPOType.SelectedIndex != 0)
                {
                    if ((Request.QueryString[0].Equals("MODIFY")) || (Request.QueryString[0].Equals("VIEW")))
                    {
                        dt = CommonClasses.FillCombo("PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL", "P_NAME", "P_CODE", "CPOM_P_CODE=P_CODE AND PARTY_MASTER.ES_DELETE='0' and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE and  P_TYPE=1 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CPOM_TYPE='" + ddlPOType.SelectedValue + "' ", ddlCustomer);
                    }
                    else
                    {
                        dt = CommonClasses.FillCombo("PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL", "P_NAME", "P_CODE", "CPOM_P_CODE=P_CODE AND PARTY_MASTER.ES_DELETE='0' and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_ORD_QTY > CPOD_WO_QTY AND P_TYPE=1 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CPOM_TYPE='" + ddlPOType.SelectedValue + "' ", ddlCustomer);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ddlCustomer.Enabled = true;
                        ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
                    }
                }
                else
                {
                    if ((Request.QueryString[0].Equals("MODIFY")) || (Request.QueryString[0].Equals("VIEW")))
                    {
                        dt = CommonClasses.FillCombo("PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL", "P_NAME", "P_CODE", "CPOM_P_CODE=P_CODE AND PARTY_MASTER.ES_DELETE='0' and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE  AND P_TYPE=1 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlCustomer);
                    }
                    else
                    {
                        dt = CommonClasses.FillCombo("PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL", "P_NAME", "P_CODE", "CPOM_P_CODE=P_CODE AND PARTY_MASTER.ES_DELETE='0' and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_ORD_QTY > CPOD_WO_QTY AND P_TYPE=1 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlCustomer);
                    }
                    
                    if (dt.Rows.Count > 0)
                    {
                        ddlCustomer.Enabled = false;
                        ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
                    }
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "LoadCustomer", Ex.Message);

        }
    }
    #endregion

    #region LoadSalesOrderNo
    private void LoadSalesOrderNo()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                if (ddlPOType.SelectedIndex != 0 || ddlCustomer.SelectedIndex != 0)
                {                    
                    dt = CommonClasses.FillCombo("CUSTPO_MASTER,CUSTPO_DETAIL", "CPOM_PONO", "CPOM_CODE", "CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_ORD_QTY > CPOD_WO_QTY and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CPOM_TYPE='" + ddlPOType.SelectedValue + "' and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' order by  CPOM_CODE desc ", ddlSaleOrderNo);
                   
                    if (dt.Rows.Count > 0)
                    {
                        ddlSaleOrderNo.Enabled = true;
                        ddlSaleOrderNo.Items.Insert(0, new ListItem("Please Select Order No ", "0"));
                    }
                }
                else
                {
                    dt = CommonClasses.FillCombo("CUSTPO_MASTER,CUSTPO_DETAIL", "CPOM_PONO", "CPOM_CODE", "CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by  CPOM_CODE desc", ddlSaleOrderNo);
                    if (dt.Rows.Count > 0)
                    {
                        ddlSaleOrderNo.Enabled = false;
                        ddlSaleOrderNo.Items.Insert(0, new ListItem("Please Select Order No ", "0"));
                    }
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "LoadSalesOrderNo", Ex.Message);

        }
    }
    #endregion


    #region LoadType
    private void LoadType()
    {
        try
        {
           
                DataTable dt = CommonClasses.Execute("select distinct(SO_T_CODE) ,SO_T_DESC from SO_TYPE_MASTER where ES_DELETE=0 and SO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by SO_T_DESC");
                if (dt.Rows.Count > 0)
                {   
                ddlPOType.DataSource = dt;
                ddlPOType.DataTextField = "SO_T_DESC";
                ddlPOType.DataValueField = "SO_T_CODE";
                ddlPOType.DataBind();
                ddlPOType.Items.Insert(0, new ListItem("Select Work Order Type", "0"));
           
                }
                else
                {
                    ddlCustomer.SelectedIndex = 0;
                    ddlSaleOrderNo.SelectedIndex = 0;
                    ddlCustomer.Enabled = false;
                    ddlSaleOrderNo.Enabled = false;
                }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "LoadType", Ex.Message);
        }

    }
    #endregion


    #region ddlPOType_SelectedIndexChanged
    protected void ddlPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPOType.SelectedIndex !=0)
        {
            LoadCustomer();
        }
        else
        {           

        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgBillPassing.Rows.Count == 0)
        {

            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_CPOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_ORD_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("WORK_ORD_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("WORK_BAL_QTY", typeof(String)));             

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
            }
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from WORK_ORDER_MASTER where WO_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Record used by another user";
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Sales Work Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #endregion

    #region txtWORK_ORD_QTY_TextChanged
    protected void txtWORK_ORD_QTY_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox thisCheckBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;
            Double OrderQty = Convert.ToDouble(((TextBox)dgBillPassing.Rows[index].FindControl("txtWORK_ORD_QTY")).Text);
            Double BalQty = Convert.ToDouble(((Label)dgBillPassing.Rows[index].FindControl("lblIWORK_BAL_QTY")).Text);
            if (OrderQty <= BalQty)
            {
               
            }
            else
            {
                ShowMessage("#Avisos", "Order Qty Should Not Be Greater Than Balance Qty", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ((TextBox)dgBillPassing.Rows[index].FindControl("txtWORK_ORD_QTY")).Text = BalQty.ToString();
            }
          
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Work Order", "txtWORK_ORD_QTY_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion
  
}
