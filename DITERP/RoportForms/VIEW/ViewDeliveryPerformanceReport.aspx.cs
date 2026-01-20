using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class RoportForms_VIEW_ViewDeliveryPerformanceReport : System.Web.UI.Page
{
    static string right = "";
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");
            LoadCombos();
            ddlCustomerName.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    return;
                }
            }
            if (chkAllCustomer.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Customer ";
                    return;
                }
            }
            string Cond = "";
            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str1 = "";
            string str = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                    if (Convert.ToDateTime(From) < Convert.ToDateTime(Date1) || Convert.ToDateTime(From) > Convert.ToDateTime(Date2) || Convert.ToDateTime(To) < Convert.ToDateTime(Date1) || Convert.ToDateTime(To) > Convert.ToDateTime(Date2))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToString("dd/MMM/yyyy");
                To = To2.ToString("dd/MMM/yyyy");
            }
            
            if (chkAllCustomer.Checked != true)
            {
                Cond = Cond + " PSM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
            }
            if (chkDateAll.Checked == true)
            {
                Cond = Cond + " PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "' AND ";
            }
            else
            {
                Cond = Cond + " PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND ";
            }
            Response.Redirect("~/RoportForms/ADD/DeliveryPerformanceReport.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&Cond=" + Cond + "", false);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Performance Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
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
            CommonClasses.SendError("Delivery Performance Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER,PURCHASE_SCHEDULE_MASTER where PARTY_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_P_CODE=P_CODE AND P_INHOUSE_IND=1 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion
}
