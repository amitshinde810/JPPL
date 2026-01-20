using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Transactions_ADD_ExportPo1 : System.Web.UI.Page
{
    #region Variable
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    DataTable dtInw = new DataTable();

    static int mlCode = 0;
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    static string right = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadCustomer();
                    LoadICode();
                    LoadIName();
                    LoadCurr();
                    LoadTax();
                    try
                    {
                        //if (Request.QueryString[0].Equals("VIEW"))
                        //{
                        //    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        //    //ViewRec("VIEW");
                        //}
                        //else if (Request.QueryString[0].Equals("MODIFY"))
                        //{
                        //    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        //    //ViewRec("MOD");
                        //}
                        //else if (Request.QueryString[0].Equals("INSERT"))
                        //{
                            
                        //    //LoadICode();
                        //    //LoadIName();
                        //    //LoadTax();
                        //    dt2.Rows.Clear();
                        //}
                        //txtPONumber.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
        }
    }

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadICode", Ex.Message);
        }

    }
    #endregion


    #region LoadTax
    private void LoadTax()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlTaxCategory.DataSource = dt;
            ddlTaxCategory.DataTextField = "ST_TAX_NAME";
            ddlTaxCategory.DataValueField = "ST_CODE";
            ddlTaxCategory.DataBind();
            ddlTaxCategory.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            ddlTaxCategory.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadTax", Ex.Message);
        }

    }
    #endregion

    private void LoadCurr()
    {
        try
        {
            dt = CommonClasses.Execute("select CURR_CODE,CURR_NAME from CURRANCY_MASTER where ES_DELETE=0 and CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrancy.DataSource = dt;
            ddlCurrancy.DataTextField = "CURR_NAME";
            ddlCurrancy.DataValueField = "CURR_CODE";
            ddlCurrancy.DataBind();
            ddlCurrancy.Items.Insert(0, new ListItem("Select currency", "0"));
            ddlCurrancy.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
        }
    }

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
        }

    }
    #endregion

    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.Items.Count > 0 && ddlItemName.Items.Count > 0)
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
        }
        else
        {
            ddlItemName.SelectedIndex = 0;
        }
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

        if (dt1.Rows.Count > 0)
        {
            txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
            lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
            //txtCustWeight.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
        }
        else
        {
            txtUOM.Text = "";
        }
        if (ddlItemCode.SelectedIndex > 0)
        {
            txtCustomerItemCode.Text = ddlItemCode.SelectedItem.ToString();
            txtCustomerItemName.Text = ddlItemName.SelectedItem.ToString();
        }
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.Items.Count > 0 && ddlItemName.Items.Count > 0)
        {
            ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        }
        else
        {
            ddlItemCode.SelectedIndex = 0;
        }
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

        if (dt1.Rows.Count > 0)
        {
            txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
            lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
        }
        else
        {
            txtUOM.Text = "";
        }
        if (ddlItemName.SelectedIndex > 0)
        {
            txtCustomerItemCode.Text = ddlItemCode.SelectedItem.ToString();
            txtCustomerItemName.Text = ddlItemName.SelectedItem.ToString();
        }
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void dgMainPO_RowCommand(object sender, EventArgs e)
    {
 
    }

    protected void dgMainPO_Deleting(object sender, EventArgs e)
    {

    }
}
