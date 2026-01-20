using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PPC_ADD_UnplanVendorSchedule : System.Web.UI.Page
{


    # region Variables

    UnplanVendorScheduleBL BL_UnplanVendor = null;
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
                        txtMonth.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        LoadVendor();
                        LoadItem();


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


    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtMonth.Text = Convert.ToDateTime(BL_UnplanVendor.UVS_DATE.ToString()).ToString("dd/MMM/yyyy");
            LoadVendor();
            ddlVendor.SelectedValue = BL_UnplanVendor.UVS_P_CODE.ToString();
            LoadItem();
            ddlItemName.SelectedValue = BL_UnplanVendor.UVS_I_CODE.ToString();
            txtCasting.Text = BL_UnplanVendor.UVS_CASTING_OFFLOADED.ToString();
            txtWeek1.Text = BL_UnplanVendor.UVS_WEEK1.ToString();
            txtWeek2.Text = BL_UnplanVendor.UVS_WEEK2.ToString();
            txtWeek3.Text = BL_UnplanVendor.UVS_WEEK3.ToString();
            txtWeek4.Text = BL_UnplanVendor.UVS_WEEK4.ToString();

            if (str == "VIEW")
            {
                txtMonth.Enabled = false;
                ddlVendor.Enabled = false;
                ddlItemName.Enabled = false;
                txtCasting.Enabled = false;
                txtWeek1.Enabled = false;
                txtWeek2.Enabled = false;
                txtWeek3.Enabled = false;
                txtWeek4.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtMonth.Enabled = false;
                ddlVendor.Enabled = false;
                ddlItemName.Enabled = false;
                txtCasting.Enabled = false;
                txtWeek1.Enabled = true;
                txtWeek2.Enabled = true;
                txtWeek3.Enabled = true;
                txtWeek4.Enabled = true;
                btnSubmit.Visible = true;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            BL_UnplanVendor = new UnplanVendorScheduleBL(Convert.ToInt32(ViewState["mlCode"]));
            DataTable dt = new DataTable();
            BL_UnplanVendor.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("UNPLAN_VENDOR_SCHEDULE", "MODIFY", "UVS_CODE", Convert.ToInt32(ViewState["mlCode"]));
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


    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {

            BL_UnplanVendor.UVS_DATE = Convert.ToDateTime(txtMonth.Text);
            BL_UnplanVendor.UVS_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_UnplanVendor.UVS_P_CODE = Convert.ToInt32(ddlVendor.SelectedValue);
            BL_UnplanVendor.UVS_I_CODE = Convert.ToInt32(ddlItemName.SelectedValue);
            BL_UnplanVendor.UVS_CASTING_OFFLOADED = float.Parse(txtCasting.Text);
            BL_UnplanVendor.UVS_WEEK1 = float.Parse(txtWeek1.Text);
            BL_UnplanVendor.UVS_WEEK2 = float.Parse(txtWeek2.Text);
            BL_UnplanVendor.UVS_WEEK3 = float.Parse(txtWeek3.Text);
            BL_UnplanVendor.UVS_WEEK4 = float.Parse(txtWeek4.Text);
            BL_UnplanVendor.UVS_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            if (Request.QueryString[0].Equals("MODIFY"))
                BL_UnplanVendor.UVS_STATUS = true;
            BL_UnplanVendor.UVS_TYPE = false;
            BL_UnplanVendor.UVS_DESC = "NA";


            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "Setvalues", ex.Message);
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
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_UnplanVendor = new UnplanVendorScheduleBL();
                if (Setvalues())
                {
                    if (BL_UnplanVendor.Save("INSERT"))
                    {
                        CommonClasses.WriteLog("Unplan Vendor Schedule ", "Insert", "Unplan Vendor Schedule", BL_UnplanVendor.PK_CODE.ToString(), Convert.ToInt32(BL_UnplanVendor.PK_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewUnplanVendorSchedule.aspx", false);
                    }
                    else
                    {
                        if (BL_UnplanVendor.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_UnplanVendor.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_UnplanVendor.Msg = "";
                        }
                        ddlVendor.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_UnplanVendor = new UnplanVendorScheduleBL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_UnplanVendor.Save("MODIFY"))
                    {
                        CommonClasses.RemoveModifyLock("UNPLAN_VENDOR_SCHEDULE", "MODIFY", "UVS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Unplan Vendor Schedule ", "Update", "Unplan Vendor Schedule ", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewUnplanVendorSchedule.aspx", false);
                    }
                    else
                    {
                        if (BL_UnplanVendor.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_UnplanVendor.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_UnplanVendor.Msg = "";
                        }
                        ddlVendor.Focus();
                    }
                }
            }
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
                CommonClasses.RemoveModifyLock("UNPLAN_VENDOR_SCHEDULE", "MODIFY", "UVS_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewUnplanVendorSchedule.aspx", false);
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


    }
    #endregion ddlItemName_SelectedIndexChanged

    #region LoadVendor
    protected void LoadVendor()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute(" SELECT DISTINCT P_CODE,P_NAME FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PARTY_MASTER where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND  SPOM_P_CODE=P_CODE AND SPOM_POTYPE=1  ORDER BY P_NAME"); //and P_STM_CODE in(-2147483647,-2147483646) REMOVE 12072018
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
        //dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO +' - '+ I.I_NAME AS ICODE_INAME FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE PROD.PROD_MACHINE_LOC in ('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtDate.ToString("dd/MMM/yyyy") + "' ORDER BY ICODE_INAME");
        string strcond = "";
        string strcond1 = "";
        if (ddlVendor.SelectedValue != "0")
        {
            strcond = " AND SPOM_P_CODE='" + ddlVendor.SelectedValue + "' ";
            strcond1 = " AND VS_P_CODE='" + ddlVendor.SelectedValue + "' ";
        }

        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT  I_CODE, I_CODENO +' - '+  I_NAME AS ICODE_INAME  FROM ITEM_MASTER ,SUPP_PO_MASTER,SUPP_PO_DETAILS where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND  SPOD_I_CODE=I_CODE  AND I_CODE NOT IN (SELECT VS_I_CODE FROM VENDOR_SCHEDULE where VS_DATE='" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "' AND   ES_DELETE=0 " + strcond1 + ")  AND SPOM_POTYPE=1 AND I_CAT_CODE=-2147483648 " + strcond + " ");
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
        LoadItem();
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
    protected void txtWeek4_TextChanged(object sender, EventArgs e)
    {
        if (txtWeek1.Text.Trim() == "")
        {
            txtWeek1.Text = "0";
        }
        if (txtWeek2.Text.Trim() == "")
        {
            txtWeek2.Text = "0";
        }
        if (txtWeek3.Text.Trim() == "")
        {
            txtWeek3.Text = "0";
        }
        if (txtWeek4.Text.Trim() == "")
        {
            txtWeek4.Text = "0";
        }

        txtCasting.Text = (Convert.ToDouble(txtWeek1.Text) + Convert.ToDouble(txtWeek2.Text) + Convert.ToDouble(txtWeek3.Text) + Convert.ToDouble(txtWeek4.Text)).ToString();
    }
}
