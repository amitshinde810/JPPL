using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Transactions_CoreInventoryTransaction : System.Web.UI.Page
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
                        LoadItems(); // Method call
                        txtMonth.Text = DateTime.Now.ToString("MMM yyyy");
                        txtMonth.Attributes.Add("readonly", "readonly");
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
                        CommonClasses.SendError("Core Inventory Transaction", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Core Inventory Transaction", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT DISTINCT CORE_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,I_NAME ,isnull(CORE_ACTUAL_QTY,0) as CORE_ACTUAL_QTY,isnull(CORE_MONTH,'1900-01-01') as CORE_MONTH FROM CORE_INVENTORY P INNER JOIN ITEM_MASTER I ON P.CORE_I_CODE=I.I_CODE where P.ES_DELETE=0 and I.ES_DELETE=0 and CORE_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CORE_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["CORE_MONTH"].ToString()).ToString("MMM yyyy");
                LoadItems();
                txtActualQty.Text = dt.Rows[0]["CORE_ACTUAL_QTY"].ToString();
                ddlPartName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();

                if (str == "VIEW")
                {
                    txtActualQty.Enabled = false; ddlPartName.Enabled = false; btnSubmit.Visible = false;
                }
                ddlPartName.Enabled = false;
                
                txtMonth.Enabled = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CORE_INVENTORY", "MODIFY", "CORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Core Inventory Transaction", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlPartName.SelectedIndex == -1 || ddlPartName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ddlPartName.Focus();
            return;
        }
        if (txtActualQty.Text == "" || Convert.ToInt32(txtActualQty.Text.Trim()) <= 0)
        {
            ShowMessage("#Avisos", "Please Enter Actual Qty(in Core Shop)", CommonClasses.MSG_Warning);
            txtActualQty.Focus();
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
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select CORE_CODE FROM CORE_INVENTORY WHERE CORE_I_CODE= '" + ddlPartName.SelectedValue + "'  AND CORE_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND CORE_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False' and CORE_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO CORE_INVENTORY(CORE_CM_CODE,CORE_COMP_ID,CORE_I_CODE,CORE_ACTUAL_QTY,CORE_MONTH)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlPartName.SelectedValue + "','" + txtActualQty.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CORE_CODE) from CORE_INVENTORY");
                        CommonClasses.WriteLog("Core Inventory Transaction", "Save", "Core Inventory Transaction", ddlPartName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCoreInventoryTransaction.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlPartName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Part Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM CORE_INVENTORY WHERE ES_DELETE=0 AND CORE_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "'  AND CORE_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND CORE_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CORE_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' AND CORE_I_CODE= '" + ddlPartName.SelectedValue + "' ");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE CORE_INVENTORY SET CORE_I_CODE='" + ddlPartName.SelectedValue + "', CORE_ACTUAL_QTY='" + txtActualQty.Text.Trim().Replace("'", "\''") + "',CORE_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' WHERE CORE_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("CORE_INVENTORY", "MODIFY", "CORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Core Inventory Transaction", "Update", "Core Inventory Transaction", ddlPartName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewCoreInventoryTransaction.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlPartName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Part Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPartName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Core Inventory Transaction", "SaveRec", ex.Message);
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
            CommonClasses.SendError("CORE_INVENTORY", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Core Inventory Transaction", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("CORE_INVENTORY", "MODIFY", "CORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewCoreInventoryTransaction.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Core Inventory Transaction", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlPartName.SelectedIndex == -1)
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
            CommonClasses.SendError("Core Inventory Transaction", "CheckValid", Ex.Message);
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

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlPartName_SelectedIndexChanged

    #region LoadItems
    protected void LoadItems()
    {
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        ddlPartName.DataSource = dtFinishItem;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItems

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
    }
    #endregion txtMonth_TextChanged

    #region txtActualQty_TextChanged
    protected void txtActualQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtActualQty.Text);
        txtActualQty.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 3));
    }
    #endregion txtActualQty_TextChanged

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result = "", result1 = "", totalStr = "", result2 = "", s = Text;
        result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15) { no = 15; }
            result1 = s.Substring((result.IndexOf(".") + 1), no);
            try { result2 = result1.Substring(0, result1.IndexOf(".")); }
            catch { }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".") { result1 = "00"; }
            if (result2 != "") { totalStr = result + result2; }
            else { totalStr = result + result1; }
        }
        else { result1 = "00"; totalStr = result + result1; }
        return totalStr;
    }
    #endregion
}
