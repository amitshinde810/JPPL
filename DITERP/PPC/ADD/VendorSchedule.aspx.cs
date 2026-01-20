using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transaction_VendorSchedule : System.Web.UI.Page
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
                        LoadItem();
                        LoadVendor();

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
                        CommonClasses.SendError("Vendor Schedule Transaction", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT DISTINCT I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME,P_NAME,right(convert(varchar,VS_DATE, 106), 8) as VS_DATE, I.I_CODE, ISNULL(V.VS_CASTING_OFFLOADED, 0) AS VS_CASTING_OFFLOADED, ISNULL(V.VS_WEEK1, 0) AS VS_WEEK1,  ISNULL(V.VS_WEEK2, 0) AS VS_WEEK2, ISNULL(V.VS_WEEK3, 0) AS VS_WEEK3, ISNULL(V.VS_WEEK4, 0) AS VS_WEEK4,P_CODE FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON V.VS_P_CODE=P.P_CODE WHERE (V.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "' AND V.VS_CODE= '" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtMonth.Text = dt.Rows[0]["VS_DATE"].ToString();
                LoadItem();
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                LoadVendor();
                ddlVendor.SelectedValue = dt.Rows[0]["P_CODE"].ToString();
                //13/02/2019 :- Calculate casting to be offloaded
                DataTable dtCasttobeOffloaded = new DataTable();
                DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
                double OffloadedQty = 0;
                dtCasttobeOffloaded = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) AS CS_SCHEDULE_QTY,ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)  ),0) AS Tot_Stnd_Stock , ISNULL((SELECT  SUM(STL_DOC_QTY) FROM STOCK_LEDGER where  STL_STORE_TYPE IN (-2147483642,-2147483643) AND STL_I_CODE=I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)  AS ActualStockQty,ISNULL((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)+ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)  ),0) - ISNULL((SELECT  SUM(STL_DOC_QTY) FROM STOCK_LEDGER where  STL_STORE_TYPE IN (-2147483642,-2147483643) AND STL_I_CODE=I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) )*ISNULL((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) AS RM_MACHINING_REJECTION FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0) as CastingToBeOffloaded FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE PROD_I_CODE='" + ddlItemName.SelectedValue + "' and PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ");
                if (dtCasttobeOffloaded.Rows.Count > 0)
                {
                    OffloadedQty = Convert.ToDouble(dtCasttobeOffloaded.Rows[0]["CastingToBeOffloaded"].ToString());
                }
                  
                DataTable dtMloca = new DataTable();
                if (OffloadedQty >= 0)
                    txtCasting.Text = Math.Round(OffloadedQty, 0).ToString();
                else
                    txtCasting.Text = dt.Rows[0]["VS_CASTING_OFFLOADED"].ToString();
                txtWeek1.Text = dt.Rows[0]["VS_WEEK1"].ToString();
                txtWeek2.Text = dt.Rows[0]["VS_WEEK2"].ToString();
                txtWeek3.Text = dt.Rows[0]["VS_WEEK3"].ToString();
                txtWeek4.Text = dt.Rows[0]["VS_WEEK4"].ToString();

                if (str == "VIEW")
                {
                    ddlVendor.Enabled = false; ddlItemName.Enabled = false; txtCasting.Enabled = false; txtWeek1.Enabled = false; txtWeek2.Enabled = false;
                    txtWeek3.Enabled = false; txtWeek4.Enabled = false;
                    btnSubmit.Visible = false;
                }
                ddlItemName.Enabled = false;
                ddlVendor.Enabled = false;
                txtMonth.Enabled = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("VENDOR_SCHEDULE", "MODIFY", "VS_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "ViewRec", ex.Message);
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
        if (ddlVendor.SelectedIndex == -1 || ddlVendor.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Enter Vendor Name", CommonClasses.MSG_Warning);
            ddlVendor.Focus();
            return;
        }
        if ((txtWeek1.Text.Trim() == "0" || txtWeek1.Text.Trim() == "") && (txtWeek2.Text.Trim() == "0" || txtWeek2.Text.Trim() == "") && (txtWeek3.Text.Trim() == "0" || txtWeek3.Text.Trim() == "") && (txtWeek4.Text.Trim() == "0" || txtWeek4.Text.Trim() == ""))
        {
            ShowMessage("#Avisos", "Please Enter Entry Field", CommonClasses.MSG_Warning);
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
                dt = CommonClasses.Execute("Select VS_CODE FROM VENDOR_SCHEDULE WHERE VS_P_CODE= '" + ddlVendor.SelectedValue + "' AND VS_CM_CODE='" + (string)Session["CompanyCode"] + "' and VS_I_CODE= '" + ddlItemName.SelectedValue + "' and ES_DELETE='False' and VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into VENDOR_SCHEDULE (VS_CM_CODE,VS_COMP_ID,	VS_P_CODE,	VS_I_CODE,	VS_DATE,	VS_CASTING_OFFLOADED,	VS_WEEK1,	VS_WEEK2,	VS_WEEK3,	VS_WEEK4)values('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlVendor.SelectedValue + "','" + ddlItemName.SelectedValue + "','" + dtMonth.ToString("dd/MMM/yyyy") + "','" + txtCasting.Text + "','" + txtWeek1.Text + "','" + txtWeek2.Text + "','" + txtWeek3.Text + "','" + txtWeek4.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(VS_CODE) from VENDOR_SCHEDULE");
                        CommonClasses.WriteLog("Vendor Schedule Transaction", "Save", "Vendor Schedule Transaction", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewVendorSchedule.aspx", false);
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
                dt = CommonClasses.Execute("SELECT * FROM VENDOR_SCHEDULE WHERE ES_DELETE=0 AND VS_CODE!= '" + ViewState["mlCode"] + "'  AND VS_CM_CODE='" + (string)Session["CompanyCode"] + "' and VS_P_CODE='" + ddlVendor.SelectedValue + "' and VS_I_CODE='" + ddlItemName.SelectedValue + "'  and VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'"); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update VENDOR_SCHEDULE set VS_P_CODE='" + ddlVendor.SelectedValue + "',VS_I_CODE='" + ddlItemName.SelectedValue + "',VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "',VS_CASTING_OFFLOADED='" + txtCasting.Text + "',VS_WEEK1='" + txtWeek1.Text + "',VS_WEEK2='" + txtWeek2.Text + "',VS_WEEK3='" + txtWeek3.Text + "',VS_WEEK4='" + txtWeek4.Text + "',VS_STATUS=1  WHERE VS_CODE='" + ViewState["mlCode"] + "' AND ES_DELETE=0"))
                    {
                        CommonClasses.RemoveModifyLock("VENDOR_SCHEDULE", "MODIFY", "VS_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Vendor Schedule Transaction", "Update", "Vendor Schedule Transaction", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewVendorSchedule.aspx", false);
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
            CommonClasses.SendError("Vendor Schedule Transaction", "SaveRec", ex.Message);
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
            CommonClasses.SendError("VENDOR_SCHEDULE", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Transaction", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("VENDOR_SCHEDULE", "MODIFY", "VS_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewVendorSchedule.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Transaction", "CheckValid", Ex.Message);
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
        DataTable dtCasttobeOffloaded = new DataTable();
        //dtQtyfromSubToDispatch = CommonClasses.Execute("Select P.P_CODE,S.STL_I_CODE,P.P_NAME,-SUM(S.STL_DOC_QTY) AS STL_DOC_QTY FROM INVOICE_MASTER INM INNER JOIN INVOICE_DETAIL IND ON INM.INM_CODE=IND.IND_INM_CODE INNER JOIN  PARTY_MASTER P ON INM.INM_P_CODE=P.P_CODE inner join STOCK_LEDGER S on INM.INM_CODE=S.STL_DOC_NO WHERE INM_P_CODE='" + ddlVendor.SelectedValue + "' and IND_I_CODE='" + ddlItemName.SelectedValue + "' AND INM.ES_DELETE=0 AND P.ES_DELETE=0 AND IND.ES_DELETE=0 and P.P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' GROUP BY P_CODE,STL_I_CODE,P_NAME");
        DateTime dtMonth = Convert.ToDateTime(txtMonth.Text);
        //dtCasttobeOffloaded = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) AS CS_SCHEDULE_QTY,ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0)),0) AS Tot_Stnd_Stock ,ISNULL((select ISNULL(SUM(ISNULL(CL_CQTY,0)),0) as STL_DOC_QTY from challan_stock_ledger where CL_I_CODE = I.I_CODE),0) AS ActualStockQty,ISNULL((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)+ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0)),0) -ISNULL((select ISNULL(SUM(ISNULL(CL_CQTY,0)),0) as STL_DOC_QTY from challan_stock_ledger where CL_I_CODE = I.I_CODE),0))*ISNULL((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) AS RM_MACHINING_REJECTION FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0) as CastingToBeOffloaded FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE PROD_I_CODE='" + ddlItemName.SelectedValue + "' and PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ");

        //as per NEw logic mail by DEEPAK SURVASE on 3rd JAN 2018
        dtCasttobeOffloaded = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) AS CS_SCHEDULE_QTY,ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)  ),0) AS Tot_Stnd_Stock , ISNULL((SELECT  SUM(STL_DOC_QTY) FROM STOCK_LEDGER where  STL_STORE_TYPE IN (-2147483642,-2147483643) AND STL_I_CODE=I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)  AS ActualStockQty,ISNULL((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)+ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)  ),0) - ISNULL((SELECT  SUM(STL_DOC_QTY) FROM STOCK_LEDGER where  STL_STORE_TYPE IN (-2147483642,-2147483643) AND STL_I_CODE=I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) )*ISNULL((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) AS RM_MACHINING_REJECTION FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0) as CastingToBeOffloaded FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE PROD_I_CODE='" + ddlItemName.SelectedValue + "' and PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ");
        double OffloadedQty = Convert.ToDouble(dtCasttobeOffloaded.Rows[0]["CastingToBeOffloaded"].ToString());
        DataTable dtMloca = new DataTable();
        string strMloca = "";
        //dtMloca = CommonClasses.Execute("SELECT ISNULL(PROD_MTYPE,0) AS PROD_MTYPE FROM PRODUCT_MASTER where PROD_MACHINE_LOC='BOTH' AND ES_DELETE=0  AND PROD_I_CODE='" + ddlItemName.SelectedValue + "'");
        //if (dtMloca.Rows.Count > 0)
        //{
        //    strMloca = dtMloca.Rows[0]["PROD_MTYPE"].ToString();
        //}
        if (dtCasttobeOffloaded.Rows.Count > 0)
        {
            if (OffloadedQty >= 0)
            {
                //if (strMloca == "1")
                //{
                //    txtCasting.Text = Math.Round(OffloadedQty / 2, 0).ToString();
                //}
                //else
                //{
                txtCasting.Text = Math.Round(OffloadedQty, 0).ToString();
                //}

            }
            else
                txtCasting.Text = "0";
        }
        else
        {
            ShowMessage("#Avisos", "Qty not found in ERP for this Item", CommonClasses.MSG_Warning);
            ddlVendor.Focus();
            return;
        }

    }
    #endregion ddlItemName_SelectedIndexChanged

    #region LoadVendor
    protected void LoadVendor()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("select P_CODE,P_NAME from PARTY_MASTER where P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and P_TYPE=2 and ES_DELETE=0 order by P_NAME"); //and P_STM_CODE in(-2147483647,-2147483646) REMOVE 12072018
        ddlVendor.DataSource = dtProcess;
        ddlVendor.DataTextField = "P_NAME";
        ddlVendor.DataValueField = "P_CODE";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, new ListItem("Select Vendor Name", "0"));
    }
    #endregion LoadVendor

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO +' - '+ I.I_NAME AS ICODE_INAME FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE PROD.PROD_MACHINE_LOC in ('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtDate.ToString("dd/MMM/yyyy") + "' ORDER BY ICODE_INAME");
        ddlItemName.DataSource = dtFinishItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItem

    #region ddlVendor_SelectedIndexChanged
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVendor.SelectedValue == "0")
        {
            LoadItem();
        }
        else
        {
            if (ddlItemName.SelectedValue == "0")
            {
                LoadItem();
            }

        }
        //DataTable dtFinishItem = new DataTable();
        //dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO +' - '+ I.I_NAME AS ICODE_INAME from ITEM_MASTER I inner join CUSTOMER_SCHEDULE C ON I.I_CODE=C.CS_I_CODE where I.ES_DELETE=0 and C.ES_DELETE=0 and I_CAT_CODE='-2147483648' and I.I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        //ddlItemName.DataSource = dtFinishItem;
        //ddlItemName.DataTextField = "ICODE_INAME";
        //ddlItemName.DataValueField = "I_CODE";
        //ddlItemName.DataBind();
        //ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion ddlVendor_SelectedIndexChanged

    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        LoadItem();
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtMonth.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtMonth.Focus();
            LoadItem();
            return;
        }
    }
    #endregion txtMonth_TextChanged
}
