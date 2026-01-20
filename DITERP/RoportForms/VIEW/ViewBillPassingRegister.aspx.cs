using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_BillPassingRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadCombos();
            ddlCustomerName.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
        }
    }
    private void LoadCombos()
    {
        CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWD_IWM_CODE=IWM_CODE AND IWD_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0' AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME ", ddlCustomerName);
        ddlCustomerName.Items.Insert(0, new ListItem("--Select Supplier--", "0"));
    }
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

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
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
            CommonClasses.SendError("Supplier Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

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
                    lblmsg.Text = "Select Supplier ";
                    ddlCustomerName.Focus();
                    return;
                }
            }
            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str = "";
            #region ChkDateRegion
            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }
            #endregion
            #region WiseRegion
            if (rbtGroup.SelectedIndex == 0)
            {
                str = "Datewise";
                if (chkDateAll.Checked == true && chkAllCustomer.Checked == true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked != true && chkAllCustomer.Checked != true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked != true && chkAllCustomer.Checked == true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked == true && chkAllCustomer.Checked != true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
            }
            else if (rbtGroup.SelectedIndex == 1)
            {
                str = "CustWise";

                if (chkDateAll.Checked == true && chkAllCustomer.Checked == true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked != true && chkAllCustomer.Checked != true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked != true && chkAllCustomer.Checked == true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
                if (chkDateAll.Checked == true && chkAllCustomer.Checked != true)
                {
                    Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&datewise=" + str + "", false);
                }
            }
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}