using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewSalesOrderReport : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    string From = "";
    string To = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            ddlCustomerName.Enabled = false;
            chkAllComp.Checked = true;
            chkAllItem.Checked = true;
            ddlFinishedComponent.Enabled = false;
            ddlItemName.Enabled = false;
            chkAllPOType.Checked = true;
            chkAllProCode.Checked = true;
            ddlProCode.Enabled = false;
            ddlPOType.Enabled = false;
            LoadCombos();

            
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            string str = "";
            if (rbtAllPending.SelectedIndex == 1)
            {
                str = str + " ((CPOD_ORD_QTY -CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0) and ";
            }
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (chkAllItem.Checked != true)
            {
                if (ddlFinishedComponent.SelectedIndex > 0)
                {
                    str = str + "CPOD_I_CODE= " + ddlFinishedComponent.SelectedValue + " and ";
                }
            }
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL WHERE " + str + " CPOM_P_CODE=P_CODE and CUSTPO_DETAIL.CPOD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";

            if (rbtAllPending.SelectedIndex==1)
            {
                str = str + " ((CPOD_ORD_QTY -CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0) and ";   
            }
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }

            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("select distinct(I_CODE) as I_CODE,I_CAT_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE " + str + " I_CODE=CPOD_I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.I_CAT_CODE='-2147483648' ORDER BY I_CODENO");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemName.DataSource = dtItemDet;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadProJectCode
    private void LoadProJectCode()
    {
        try
        {
            string str = "";
            if (rbtAllPending.SelectedIndex == 1)
            {
                str = str + " ((CPOD_ORD_QTY -CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0) and ";
            }
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (chkAllItem.Checked != true)
            {
                if (ddlFinishedComponent.SelectedIndex > 0)
                {
                    str = str + "CPOD_I_CODE= " + ddlFinishedComponent.SelectedValue + " and ";
                }
            }
            if (ddlCustomerName.SelectedIndex > 0)
            {
                str = str + "CPOM_P_CODE= " + ddlCustomerName.SelectedValue + " and ";
            }

            DataTable dtItemDet = new DataTable();

            dtItemDet = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from ITEM_MASTER,dbo.PROJECT_CODE_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where  " + str + " I_CODE=CPOD_I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE and ITEM_MASTER.ES_DELETE=0 and PROJECT_CODE_MASTER.ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
            ddlProCode.DataSource = dtItemDet;
            ddlProCode.DataTextField = "PROCM_NAME";
            ddlProCode.DataValueField = "PROCM_CODE";
            ddlProCode.DataBind();
            ddlProCode.Items.Insert(0, new ListItem("Select Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Project Code", "LoadProJectCode", Ex.Message);
        }
    }
    #endregion

    #region LoadPONOType
    private void LoadPONOType()
    {
        try
        {
            string str = "";
            if (rbtAllPending.SelectedIndex == 1)
            {
                str = str + " ((CPOD_ORD_QTY -CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0) and ";
            }

            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (chkAllItem.Checked != true)
            {
                if (ddlFinishedComponent.SelectedIndex > 0)
                {
                    str = str + "CPOD_I_CODE= " + ddlFinishedComponent.SelectedValue + " and ";
                }
            }
            if (ddlCustomerName.SelectedIndex > 0)
            {
                str = str + "CPOM_P_CODE= " + ddlCustomerName.SelectedValue + " and ";
            }
            if (chkAllProCode.Checked != true)
            {
                if (ddlProCode.SelectedIndex > 0)
                {
                    str = str + "CPOM_PROJECT_CODE= " + ddlProCode.SelectedValue + " and ";
                }
            }
            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("select   distinct SO_T_SHORT_NAME AS PO_T_SHORT_NAME, SO_T_CODE AS PO_T_CODE from SO_TYPE_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL  where " + str + "  CUSTPO_MASTER.ES_DELETE=0 and CUSTPO_MASTER.CPOM_TYPE = SO_TYPE_MASTER.SO_T_CODE and CUSTPO_DETAIL.CPOD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and SO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by SO_T_SHORT_NAME");

            ddlPOType.DataSource = dtItemDet;
            ddlPOType.DataTextField = "PO_T_SHORT_NAME";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Select PO Type", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PO Type", "LoadPONOType", Ex.Message);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Sales Order Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadCustomer();
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
            LoadCombos();
            LoadCustomer();
        }
        else
        {
            LoadCombos();
            LoadCustomer();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadCustomer();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region chkAllProCode_CheckedChanged
    protected void chkAllProCode_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllProCode.Checked == true)
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = false;
        }
        else
        {
            LoadProJectCode();
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = true;
            ddlProCode.Focus();
        }
    }
    #endregion

    #region chkAllPOType_CheckedChanged
    protected void chkAllPOType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllPOType.Checked == true)
        {
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = false;
        }
        else
        {
            LoadPONOType();
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = true;
            ddlPOType.Focus();
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
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

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
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

    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlFinishedComponent.SelectedValue;
        LoadCustomer();
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemName.SelectedValue;
        LoadCustomer();
    }

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProJectCode();
    }

    protected void ddlProCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPONOType();
    }

    protected void rbtAllPending_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
        LoadCustomer();
        LoadProJectCode();
        LoadPONOType();
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime From;
            DateTime To;
            string Condition = "";
            From = Convert.ToDateTime(txtFromDate.Text);
            To = Convert.ToDateTime(txtToDate.Text);
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    From = Convert.ToDateTime(txtFromDate.Text);
                    To = Convert.ToDateTime(txtToDate.Text);
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]);
                To = Convert.ToDateTime(Session["ClosingDate"]);
            }


            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);               
                    return;
                }
                else
                {
                    Condition = Condition + "CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                }
            }
            else
            {
                Condition = Condition + "CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Outward Type ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);               
                    ddlCustomerName.Focus();
                    return;
                }
                else
                {
                    if (ddlFinishedComponent.SelectedValue != "0")
                    {
                        Condition = Condition + "ITEM_MASTER.I_CODE='" + ddlFinishedComponent.SelectedValue + "' and ";
                    }
                }
            }
            if (chkAllComp.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Customer ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);               
                    ddlCustomerName.Focus();
                    return;
                }
                else
                {
                    Condition = Condition + " PARTY_MASTER.P_CODE='" + ddlCustomerName.SelectedValue + "' and ";
                }
            }
            if (chkAllProCode.Checked == false)
            {
                if (ddlProCode.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Project Code ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);               
                    ddlProCode.Focus();
                    return;
                }
                else
                {
                    Condition = Condition + "PROJECT_CODE_MASTER.PROCM_CODE='" + ddlProCode.SelectedValue + "' and ";
                }
            }

            if (chkAllPOType.Checked == false)
            {
                if (ddlPOType.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO Type";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);               
                    ddlPOType.Focus();
                    return;
                }
                else
                {
                    Condition = Condition + "SO_TYPE_MASTER.SO_T_CODE='" + ddlPOType.SelectedValue + "' and ";
                }
            }

            string str1 = "";
            string str = "";     

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
            if (rbtAllPending.SelectedIndex == 0)
            {
                str1 = "All";
            }
            if (rbtAllPending.SelectedIndex == 1)
            {
                str1 = "Pending";
                Condition = Condition + "((CPOD_ORD_QTY -CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0) and ";
            }
            Response.Redirect("../../RoportForms/ADD/SalesOrderReport.aspx?Title=" + Title + "&FromDate=" + From.ToString("dd-MMM-yyyy") + "&ToDate=" + To.ToString("dd-MMM-yyyy") + "&datewise=" + str + "&detail=" + str1 + "&Condition=" + Condition + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
