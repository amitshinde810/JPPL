using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_ADD_SectorMaster : System.Web.UI.Page
{
    #region General Declaration
    SalesTaxMaster_BL BL_SalesTaxMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Events
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='16'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadTaxHeadSales();
                    LoadTaxHeadPurchase();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_SalesTaxMaster = new SalesTaxMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_SalesTaxMaster = new SalesTaxMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtTaxName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Sales Tax Master", "PageLoad", ex.Message);
                }
            }
        }
    }

    private void LoadTaxHeadSales()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0");

            ddlTaxAccHeadSales.DataSource = dt;
            ddlTaxAccHeadSales.DataTextField = "TALLY_NAME";
            ddlTaxAccHeadSales.DataValueField = "TALLY_CODE";
            ddlTaxAccHeadSales.DataBind();
            ddlTaxAccHeadSales.Items.Insert(0, new ListItem("Select Tax Acc Head for Sales", "0"));

            ddlTaxSales.DataSource = dt;
            ddlTaxSales.DataTextField = "TALLY_NAME";
            ddlTaxSales.DataValueField = "TALLY_CODE";
            ddlTaxSales.DataBind();
            ddlTaxSales.Items.Insert(0, new ListItem("Select Tax Acc For Sales", "0"));

            ddlTaxPurchase.DataSource = dt;
            ddlTaxPurchase.DataTextField = "TALLY_NAME";
            ddlTaxPurchase.DataValueField = "TALLY_CODE";
            ddlTaxPurchase.DataBind();
            ddlTaxPurchase.Items.Insert(0, new ListItem("Select Tax Acc For Purchase", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Tax Master", "Load Tax Head", ex.Message);
        }
        finally
        {
        }
    }
    private void LoadTaxHeadPurchase()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0");
            ddlTaxAccHeadPurchase.DataSource = dt;
            ddlTaxAccHeadPurchase.DataTextField = "TALLY_NAME";
            ddlTaxAccHeadPurchase.DataValueField = "TALLY_CODE";
            ddlTaxAccHeadPurchase.DataBind();
            ddlTaxAccHeadPurchase.Items.Insert(0, new ListItem("Select Tax Acc Head for Purchase", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Tax Master", "Load Tax Head", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTaxName.Text.Trim() == "")
            {
                lblmsg.Text = "Please Enter Tax Name";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else if (txtSalesTax.Text.Trim() == "")
            {
                lblmsg.Text = "Please Enter Sales Tax";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else
            {
                SaveRec();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region txtSalesTax_TextChanged
    protected void txtSalesTax_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSalesTax.Text);
        txtSalesTax.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtSalesTax.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSalesTax.Text = "";
        }
    }
    #endregion

    #region txtTCSTax_TextChanged
    protected void txtTCSTax_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtTCSTax.Text);
        txtTCSTax.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtTCSTax.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtTCSTax.Text = "";
        }
    }
    #endregion

    #endregion

    #region Methods

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_SalesTaxMaster = new SalesTaxMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_SalesTaxMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SALES_TAX_MASTER", "MODIFY", "ST_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtTaxName.Text = BL_SalesTaxMaster.ST_TAX_NAME;
            txtAlias.Text = BL_SalesTaxMaster.ST_ALIAS;
            txtSalesTax.Text = BL_SalesTaxMaster.ST_SALES_TAX.ToString();
            txtSalesTax.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSalesTax.Text));
            txtTCSTax.Text = BL_SalesTaxMaster.ST_TCS_TAX.ToString();
            txtTCSTax.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTCSTax.Text));
            txtFormNo.Text = BL_SalesTaxMaster.ST_FORM_NO.ToString();
            ddlTaxAccHeadSales.SelectedValue = BL_SalesTaxMaster.ST_SALES_ACC_HEAD.ToString();
            ddlTaxAccHeadPurchase.SelectedValue = BL_SalesTaxMaster.ST_TAX_ACC_HEAD.ToString();
            ddlTaxSales.SelectedValue = BL_SalesTaxMaster.ST_TAX_SALE_ACC.ToString();
            ddlTaxPurchase.SelectedValue = BL_SalesTaxMaster.ST_TAX_PUR_ACC.ToString();
            if (str == "VIEW")
            {
                txtTaxName.Enabled = false;
                txtAlias.Enabled = false;
                txtSalesTax.Enabled = false;
                txtTCSTax.Enabled = false;
                txtFormNo.Enabled = false;
                ddlTaxAccHeadSales.Enabled = false;
                ddlTaxAccHeadPurchase.Enabled = false;
                ddlTaxSales.Enabled = false;
                ddlTaxPurchase.Enabled = false;

                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Tax Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            if (txtTaxName.Text.ToUpper().Trim() != "")
            {
                BL_SalesTaxMaster.ST_TAX_NAME = txtTaxName.Text;
                BL_SalesTaxMaster.ST_ALIAS = txtAlias.Text;
                BL_SalesTaxMaster.ST_SALES_TAX = Convert.ToDouble(txtSalesTax.Text == "" ? "0.00" : txtSalesTax.Text);
                BL_SalesTaxMaster.ST_TCS_TAX = Convert.ToDouble(txtTCSTax.Text == "" ? "0.00" : txtTCSTax.Text);
                BL_SalesTaxMaster.ST_FORM_NO = int.Parse(txtFormNo.Text == "" ? "0" : txtFormNo.Text);
                BL_SalesTaxMaster.ST_SALES_ACC_HEAD = ddlTaxAccHeadSales.SelectedValue;
                BL_SalesTaxMaster.ST_TAX_ACC_HEAD = ddlTaxAccHeadPurchase.SelectedValue;
                BL_SalesTaxMaster.ST_TAX_SALE_ACC = ddlTaxSales.SelectedValue;
                BL_SalesTaxMaster.ST_TAX_PUR_ACC = ddlTaxPurchase.SelectedValue;

                BL_SalesTaxMaster.ST_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
                res = true;
            }
            else
            {
                txtTaxName.Text = txtTaxName.Text.ToUpper().Trim();
                ShowMessage("#Avisos", "Please Enter TaxName", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtTaxName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Tax Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceTaxName = txtTaxName.Text;
            StrReplaceTaxName = StrReplaceTaxName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_SalesTaxMaster = new SalesTaxMaster_BL();
                if (Setvalues())
                {
                    if (BL_SalesTaxMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(ST_CODE) from  SALES_TAX_MASTER");
                        CommonClasses.WriteLog("Sales Tax Master", "Save", "Sales Tax Master", BL_SalesTaxMaster.ST_TAX_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_SalesTaxMaster.Msg != "")
                        {
                            lblmsg.Text = BL_SalesTaxMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            BL_SalesTaxMaster.Msg = "";
                        }
                        txtTaxName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_SalesTaxMaster = new SalesTaxMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_SalesTaxMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock(" SALES_TAX_MASTER", "MODIFY", "ST_CODE", mlCode);
                        CommonClasses.WriteLog("Sales Tax Master", "Update", "Sales Tax Master", BL_SalesTaxMaster.ST_TAX_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_SalesTaxMaster.Msg != "")
                        {
                            lblmsg.Text = BL_SalesTaxMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            BL_SalesTaxMaster.Msg = "";
                        }
                        txtTaxName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Tax Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "btnOk_Click", Ex.Message);
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("SALES_TAX_MASTER", "MODIFY", "ST_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Tax Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtTaxName.Text == "")
            {
                flag = false;
            }
            else if (txtSalesTax.Text == "")
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
            CommonClasses.SendError("Sales Tax Master", "CheckValid", Ex.Message);
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
}
