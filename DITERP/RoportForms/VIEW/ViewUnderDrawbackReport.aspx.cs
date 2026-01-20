using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


public partial class RoportForms_VIEW_ViewUnderDrawbackReport : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            ddlFromInvoiceNo.Enabled = false;
            ddlToInvoiceNo.Enabled = false;
            LoadCustomer();
            txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
            txtDate.Attributes.Add("readonly", "readonly");
            //LoadInvoice();

        }

    }
    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadInvoice
    private void LoadInvoice()
    {
        try
        {
            DataTable dtInvoice = new DataTable();
            dtInvoice = CommonClasses.Execute("select  INM_CODE,INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE='" + ddlCustomerName.SelectedValue + "' and INM_INVOICE_TYPE<>1  ORDER BY INM_NO DESC");
            ddlFromInvoiceNo.DataSource = dtInvoice;
            ddlFromInvoiceNo.DataTextField = "INM_NO";
            ddlFromInvoiceNo.DataValueField = "INM_CODE";
            ddlFromInvoiceNo.DataBind();
            ddlFromInvoiceNo.Items.Insert(0, new ListItem("Select Invocie", "0"));

            ddlFromInvoiceNo.Enabled = true;

            ddlToInvoiceNo.DataSource = dtInvoice;
            ddlToInvoiceNo.DataTextField = "INM_NO";
            ddlToInvoiceNo.DataValueField = "INM_CODE";
            ddlToInvoiceNo.DataBind();
            ddlToInvoiceNo.Items.Insert(0, new ListItem("Select Invocie", "0"));

            ddlToInvoiceNo.Enabled = true;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "LoadInvoice", Ex.Message);
        }

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception)
        {

        }
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedIndex != 0)
            {
                LoadInvoice();
            }
            else
            {
                ddlFromInvoiceNo.Enabled = false;
                ddlToInvoiceNo.Enabled = false;
            }
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlToInvoiceNo_SelectedIndexChanged
    protected void ddlToInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlToInvoiceNo.SelectedIndex != 0 && ddlFromInvoiceNo.SelectedIndex!=0)
            {
                if (Convert.ToInt32(ddlFromInvoiceNo.SelectedItem.Text) > Convert.ToInt32(ddlToInvoiceNo.SelectedItem.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Invoice No is not greater than To Invoice No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ddlToInvoiceNo.Focus();
                    ddlToInvoiceNo.SelectedIndex = 0;
                }
                else
                {
 
                }
            }
           

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlFromInvoiceNo_SelectedIndexChanged
    protected void ddlFromInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlToInvoiceNo.SelectedIndex != 0 && ddlFromInvoiceNo.SelectedIndex != 0)
            {
                if (Convert.ToInt32(ddlFromInvoiceNo.SelectedItem.Text) > Convert.ToInt32(ddlToInvoiceNo.SelectedItem.Text))
                {
                    //ShowMessage("#Avisos", "From Invoice No is not greater than To Invoice No", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Invoice No is not greater than To Invoice No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ddlFromInvoiceNo.Focus();
                    ddlFromInvoiceNo.SelectedIndex = 0;
                }
                else
                {

                }
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion
   
    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            From = txtDate.Text;
            Response.Redirect("~/RoportForms/ADD/UnderDrawbackReport.aspx?Cust=" + ddlCustomerName.SelectedValue + "&FromNo=" + ddlFromInvoiceNo.SelectedItem.Text + "&ToNo=" + ddlToInvoiceNo.SelectedItem.Text + "&FromDate=" + From + "", false);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
