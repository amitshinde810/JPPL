using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewSalePlanningReport : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    string From = "";
    string To = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
            txtFromDate.Enabled = true;
            //txtToDate.Enabled = false;
            chkDateAll.Checked = false;
            //ddlGroupName.Enabled = false;
            //chkAllComp.Checked = true;
            chkAllItem.Checked = true;
            ddlFinishedComponent.Enabled = false;

            LoadGroup();
            LoadCombos();
           
            // txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        }
    }
    #endregion

    #region LoadCustomer_Comment
    //private void LoadCustomer()
    //{
    //    try
    //    {
    //        string str = "";

    //        if (chkDateAll.Checked != true)
    //        {
    //            if (txtFromDate.Text != "" && txtToDate.Text != "")
    //            {
    //                str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
    //            }
    //        }
    //        if (chkAllItem.Checked != true)
    //        {
    //            if (ddlFinishedComponent.SelectedIndex > 0)
    //            {
    //                str = str + "CPOD_I_CODE= " + ddlFinishedComponent.SelectedValue + " and ";
    //            }
    //        }
    //        DataTable custdet = new DataTable();
    //        custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL WHERE " + str + " CPOM_P_CODE=P_CODE and CUSTPO_DETAIL.CPOD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
    //        ddlGroupName.DataSource = custdet;
    //        ddlGroupName.DataTextField = "P_NAME";
    //        ddlGroupName.DataValueField = "P_CODE";
    //        ddlGroupName.DataBind();
    //        ddlGroupName.Items.Insert(0, new ListItem("Select Customer", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Sales Order Report", "LoadCustomer", Ex.Message);
    //    }
    //}
    #endregion

    #region LoadGroup
    protected void LoadGroup()
    {
        //DataTable dtGroup = new DataTable();
        //dtGroup = CommonClasses.Execute("select distinct GP_NAME,GP_CODE from GROUP_MASTER where ES_DELETE=0 and GP_COMP_ID='" + Session["CompanyId"] + "' ORDER BY GP_NAME");
        //ddlGroupName.DataSource = dtGroup;
        //ddlGroupName.DataTextField = "GP_NAME";
        //ddlGroupName.DataValueField = "GP_CODE";
        //ddlGroupName.DataBind();
        //ddlGroupName.Items.Insert(0, new ListItem("Select Group Name", "0"));
    }
    #endregion LoadGroup

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";

            str = "";
            //if (chkDateAll.Checked != true)
            //{
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
            //    }
            //}
            //if (chkAllComp.Checked != true)
            //{
            //    if (ddlGroupName.SelectedIndex > 0)
            //    {
            //        str = str + "GP_CODE= " + ddlGroupName.SelectedValue + " and ";
            //    }
            //}

            DataTable dtItemDet = new DataTable();
            //dtItemDet = CommonClasses.Execute("select distinct(I_CODE) as I_CODE,I_CAT_CODE,I_NAME+'-'+I_CODENO as I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,GROUP_MASTER,PRODUCT_MASTER WHERE " + str + " GP_CODE=PROD_GP_CODE and PROD_I_CODE=I_CODE and I_CODE=CPOD_I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.I_CAT_CODE='-2147483648' ORDER BY I_CAT_CODE,I_NAME+'-'+I_CODENO");
            dtItemDet = CommonClasses.Execute("SELECT distinct I_CODE,I_NAME+'-'+I_CODENO as I_CODENO ,I_CAT_CODE FROM ITEM_MASTER I INNER JOIN CUSTOMER_SCHEDULE C ON I.I_CODE=C.CS_I_CODE WHERE I.ES_DELETE=0 AND C.ES_DELETE=0 AND I.I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND CONVERT(DATE,CS_DATE)='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'");
            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Part No", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Planning Report", "LoadCombos", Ex.Message);
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
            CommonClasses.SendError("Sales Planning Report", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Sales Planning Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    //protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkAllComp.Checked == true)
    //    {
    //        LoadCombos();
    //        ddlGroupName.SelectedIndex = 0;
    //        ddlGroupName.Enabled = false;
    //    }
    //    else
    //    {
    //        LoadGroup();
    //        ddlGroupName.SelectedIndex = 0;
    //        ddlGroupName.Enabled = true;
    //        ddlGroupName.Focus();
    //    }
    //}
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            // txtToDate.Enabled = false;
            LoadGroup();
            LoadCombos();

        }
        else
        {
            LoadCombos();
            LoadGroup();
            txtFromDate.Enabled = true;
            //txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            // txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {

            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;

            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
        //if (txtFromDate.Text != "" || txtToDate.Text != "")
        //{
        //    DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
        //    DateTime todate = Convert.ToDateTime(txtToDate.Text);
        //    if (fdate > todate)
        //    {
        //        PanelMsg.Visible = true;
        //        lblmsg.Text = "From Date should be less than equal to To Date";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //        txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        //        txtFromDate.Focus();
        //        return;
        //    }
        //}
    }
    #endregion txtFromDate_TextChanged

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        //    LoadGroup();
        //    LoadCombos();
        //    if (txtFromDate.Text != "" || txtToDate.Text != "")
        //    {

        //        DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
        //        DateTime todate = Convert.ToDateTime(txtToDate.Text);
        //        if (todate < fdate)
        //        {
        //            PanelMsg.Visible = true;
        //            lblmsg.Text = "To Date should be greater than equal to From Date";
        //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        //            txtToDate.Focus();
        //            return;
        //        }
        //    }
    }
    #endregion txtToDate_TextChanged

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion ddlFinishedComponent_SelectedIndexChanged

    #region ddlGroupName_SelectedIndexChanged
    protected void ddlGroupName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion ddlGroupName_SelectedIndexChanged

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime From;
            DateTime To;
            string Condition = "";
            From = Convert.ToDateTime(txtFromDate.Text);
            To = Convert.ToDateTime(From).AddMonths(1);
            if (chkDateAll.Checked == false)
            {
                //if (txtFromDate.Text != "" && txtToDate.Text != "")
                if (txtFromDate.Text != "")
                {
                    From = Convert.ToDateTime(txtFromDate.Text);
                    //To = Convert.ToDateTime(txtToDate.Text);
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]);
                To = Convert.ToDateTime(Session["ClosingDate"]);
            }

            #region Comment
            //if (chkDateAll.Checked == false)
            //{
            //    //if (txtFromDate.Text == "" || txtToDate.Text == "")
            //    if (txtFromDate.Text == "")
            //    {
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "The Field 'Date' is required ";
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //        return;
            //    }
            //    else
            //    {
            //        Condition = Condition + "CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
            //    }
            //}
            //else
            //{
            //    Condition = Condition + "CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
            //}
            //if (chkAllItem.Checked == false)
            //{
            //    if (ddlFinishedComponent.SelectedIndex == 0)
            //    {
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "Select Part No";
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //        ddlFinishedComponent.Focus();
            //        return;
            //    }
            //    else
            //    {
            //        if (ddlFinishedComponent.SelectedValue != "0")
            //        {
            //            Condition = Condition + "ITEM_MASTER.I_CODE='" + ddlFinishedComponent.SelectedValue + "' and ";
            //        }

            //        // Condition = Condition + " INVOICE_MASTER.INM_TYPE='" + ddlOutwardType.SelectedValue + "' and ";
            //    }
            //}
            #endregion Comment

            //Condition = From.ToString("dd/MMM/yyyy");
            Condition = " CONVERT(date, CS_DATE)  ='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  ";
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Part No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlFinishedComponent.Focus();
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
            string str1 = "";
            string str = "";
            string str3 = "";
            // string str4 = "";           
            // Response.Redirect("../../ReportForms/ADD/SalePlanningReport.aspx?Title=" + Title + "&FromDate=" + From.ToString("dd-MM-yyyy") + "&ToDate=" + To.ToString("dd-MM-yyyy") + "&datewise=" + str + "&detail=" + str1 + "&Condition=" + Condition + "", false);

            string title = Title;
            title = Title + " " + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "";
            Response.Redirect("../../ReportForms/ADD/SalePlanningReport.aspx?Title=" + title + "&FromDate=" + From.ToString("dd/MM/yyyy") + "&ToDate=" + To.ToString("dd/MM/yyyy") + "&datewise=" + str + "&detail=" + str1 + "&Condition=" + Condition + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Planning Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
