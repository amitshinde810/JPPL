using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewInspectionRegisterReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadCombos(); LoadSupplier();
            ddlItemName.Enabled = false;
            ddlItemCode.Enabled = false;
            ddlSupplierName.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllItemName.Checked = true;
            chkAllSupplierName.Checked = true;
            rbtGroup.SelectedIndex = 0;
            rbtType.SelectedIndex = 0;
            rbtReject.SelectedIndex = 0;
            // txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            // txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (chkDateAll.Checked == false)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + " and convert(date,INSM_DATE) between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' ";
            }
        }
        CommonClasses.FillCombo("ITEM_MASTER,INSPECTION_S_MASTER", "I_NAME", "I_CODE", "ITEM_MASTER.I_CODE = INSPECTION_S_MASTER.INSM_I_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 " + str + " ORDER BY I_NAME", ddlItemName);
        ddlItemName.Items.Insert(0, new ListItem("--Select Item--", "0"));

        CommonClasses.FillCombo("ITEM_MASTER,INSPECTION_S_MASTER", "I_CODENO", "I_CODE", "ITEM_MASTER.I_CODE = INSPECTION_S_MASTER.INSM_I_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 " + str + " ORDER BY I_CODENO", ddlItemCode);
        ddlItemCode.Items.Insert(0, new ListItem("--Select Item Code--", "0"));
    }
    #endregion

    private void LoadSupplier()
    {
        string str = "";
        if (chkDateAll.Checked == false)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + " and CONVERT(DATE,INSM_DATE) between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' ";
            }
        }
        if (chkAllItemName.Checked == false)
        {
            if (ddlItemName.SelectedValue != "0")
            {
                str = str + " and I_CODE=" + ddlItemName.SelectedValue + " ";
            }
        }
        CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,ITEM_MASTER,INSPECTION_S_MASTER", "P_NAME", "P_CODE", "INSPECTION_S_MASTER.INSM_IWM_CODE = INWARD_MASTER.IWM_CODE and ITEM_MASTER.I_CODE = INSM_I_CODE and INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and P_TYPE=2 " + str + " ORDER BY P_NAME", ddlSupplierName);
        ddlSupplierName.Items.Insert(0, new ListItem("--Select Supplier--", "0"));
    }

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

            if (chkAllItemName.Checked == false)
            {
                if (ddlItemName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Item ";
                    ddlItemName.Focus();
                    return;
                }
            }
            if (chkAllSupplierName.Checked == false)
            {
                if (ddlSupplierName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Supplier ";
                    ddlSupplierName.Focus();
                    return;
                }
            }
            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";
            string str3 = "";
            string Condition = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date Must Be Equal or Smaller Than To Date";
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
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    Condition = Condition + "convert(date, INSM_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Date";
                    txtFromDate.Focus();
                    return;
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
                Condition = Condition + "INSM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ";
            }
            if (chkAllItemName.Checked == false)
            {
                if (ddlItemName.SelectedIndex != 0)
                {
                    Condition = Condition + "ITEM_MASTER.I_CODE='" + ddlItemName.SelectedValue + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Item Name";
                    ddlItemName.Focus();
                    return;
                }
            }
            if (chkAllSupplierName.Checked == false)
            {
                if (ddlSupplierName.SelectedIndex != 0)
                {
                    Condition = Condition + "PARTY_MASTER.P_CODE='" + ddlSupplierName.SelectedValue + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Supplier Name";
                    ddlSupplierName.Focus();
                    return;
                }
            }
            if (ddlPDINo.SelectedValue == "0")
            {
                Condition = Condition + "INSM_PDR_CHECK=0 and ";
            }
            if (ddlPDINo.SelectedValue == "1")
            {
                Condition = Condition + "INSM_PDR_CHECK=1 and ";
            }
            if (ddlPDINo.SelectedValue == "2")
            {
            }

            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Inspected";
                Condition = Condition + "INWARD_DETAIL.IWD_INSP_FLG=1 and ";
            }
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "pending";
                Condition = Condition + "INWARD_DETAIL.IWD_INSP_FLG=0 and ";
            }

            if (rbtReject.SelectedIndex == 0)
            {
                str2 = "all";
            }
            if (rbtReject.SelectedIndex == 1)
            {
                str2 = "rejected";
                Condition = Condition + " INSPECTION_S_MASTER.INSM_REJ_QTY>0 and ";
            }

            if (rbtGroup.SelectedIndex == 0)
            {
                str = "Datewise";
            }
            if (rbtGroup.SelectedIndex == 1)
            {
                str = "ItemWise";
            }
            if (rbtGroup.SelectedIndex == 2)
            {
                str = "CustWise";
            }
            Response.Redirect("~/RoportForms/ADD/InspectionRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&inspected=" + str1 + "&showStatus=" + str2 + "&Cond=" + Condition + "", false);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Inspection Register Report", "btnShow_Click", Ex.Message);
        }
    }

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
            CommonClasses.SendError("Inspection Register Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

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
            LoadCombos();
        }
    }

    protected void chkAllItemName_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItemName.Checked == true)
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
            ddlItemCode.Enabled = false;
            LoadSupplier();
        }
        else
        {
            LoadCombos();
            ddlItemCode.SelectedIndex = 0;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
        }
    }

    protected void chkAllSupplierName_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllSupplierName.Checked == true)
        {
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = false;
        }
        else
        {
            LoadSupplier();
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = true;
            ddlSupplierName.Focus();
        }
    }

    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtType.SelectedIndex == 1)
        {
            rbtReject.Items[1].Enabled = false;
        }
        else
        {
            rbtReject.Items[1].Enabled = true;
        }
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (chkAllSupplierName.Checked == false)
        {
            LoadSupplier();
        }
    }

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
        if (chkAllSupplierName.Checked == false)
        {
            LoadSupplier();
        }
    }
    #endregion ddlItemCode_SelectedIndexChanged

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos(); LoadSupplier();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
        }
    }
    #endregion txtFromDate_TextChanged

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos(); LoadSupplier();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Focus();
                return;
            }
        }
    }
    #endregion txtToDate_TextChanged
}
