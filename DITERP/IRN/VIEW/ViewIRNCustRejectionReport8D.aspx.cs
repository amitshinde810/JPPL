using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewIRNCustRejectionReport8D : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            if (Session["PartyCode"].ToString() != "")
            {
                Control c = this.Master.FindControl("Dashboard");
                c.Visible = false;

                Control c1 = this.Master.FindControl("Masters");
                c1.Visible = false;

                Control c2 = this.Master.FindControl("Purchase");
                c2.Visible = false;

                Control c3 = this.Master.FindControl("Production");
                c3.Visible = false;

                Control c4 = this.Master.FindControl("Sale");
                c4.Visible = false;

                Control c5 = this.Master.FindControl("Excise");
                c5.Visible = false;

                Control c6 = this.Master.FindControl("Utility");
                c6.Visible = false;

                Control c7 = this.Master.FindControl("IRN");
                c7.Visible = false;

                //Mobile View Menu Hide
                Control c8 = this.Master.FindControl("Dashboard1MV");
                c8.Visible = false;

                Control c9 = this.Master.FindControl("Masters1MV");
                c9.Visible = false;

                Control c10 = this.Master.FindControl("Purchase1MV");
                c10.Visible = false;

                Control c11 = this.Master.FindControl("Production1MV");
                c11.Visible = false;

                Control c12 = this.Master.FindControl("Sale1MV");
                c12.Visible = false;

                Control c13 = this.Master.FindControl("Excise1MV");
                c13.Visible = false;

                Control c14 = this.Master.FindControl("Utility1MV");
                c14.Visible = false;
            }
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='140'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");
                rbType_SelectedIndexChanged(null, null);
                //chkDateAll.Checked = true;
                // chkDateAll.Visible = false;
                ddlCustomerName.Enabled = false;
                chkAllComp.Checked = true;
                LoadCustomer();
                LoadCombos();
                ddlFinishedComponent.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlDefObserv.Enabled = false;
                chkAllDefObserv.Checked = true;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkAllItem.Checked = true;
                LoadDefObservation();
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable dtVendor = new DataTable();

            if (Session["PartyCode"].ToString() != "")
            {
                dtVendor = CommonClasses.Execute("SELECT DISTINCT P_CODE, PARTY_MASTER.P_NAME FROM PARTY_MASTER,IRN_ENTRY,IRN_DETAIL WHERE IRND_IRN_CODE = IRN_CODE AND IRN_ENTRY.ES_DELETE = 0 AND IRND_P_CODE = P_CODE AND IRN_CM_ID = '" + Session["CompanyId"].ToString() + "' and P_CODE='" + Session["PartyCode"] + "' ORDER BY P_NAME ");
                if (dtVendor.Rows.Count > 0)
                {
                    ddlCustomerName.Enabled = true;
                    chkAllComp.Checked = false;
                }
            }
            else
            {
                dtVendor = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM IRN_CUSTOMER_REJECTION_MASTER,PARTY_MASTER WHERE IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND CRM_P_CODE=P_CODE AND PARTY_MASTER.ES_DELETE=0 AND CRM_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' ORDER BY P_NAME");
            }
            ddlCustomerName.DataSource = dtVendor;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));
            if (Session["PartyCode"].ToString() != "")
            {
                ddlCustomerName.SelectedIndex = 1;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Effectiveness Of The CAPA", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadDefObservation
    private void LoadDefObservation()
    {
        try
        {
            DataTable dtVendor = new DataTable();
            string str = "";
            if (chkAllComp.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex > 0)
                {
                    str = str + "CRM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlItemCode.SelectedIndex > 0)
                {
                    str = str + "CRD_I_CODE='" + ddlItemCode.SelectedValue + "' AND ";
                }
            }
            ddlDefObserv.Items.Clear();
            dtVendor = CommonClasses.Execute("SELECT DISTINCT CRD_REASON AS CRD_DEF_OBSERVED FROM IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER WHERE " + str + " CRM_CODE=CRD_CRM_CODE AND    ES_DELETE=0 AND CRD_REASON IS NOT NULL AND CRD_REASON<>'' ORDER BY CRD_REASON");
            if (dtVendor.Rows.Count > 0)
            {
                ddlDefObserv.DataSource = dtVendor;
                ddlDefObserv.DataTextField = "CRD_DEF_OBSERVED";
                ddlDefObserv.DataValueField = "CRD_DEF_OBSERVED";
                ddlDefObserv.DataBind();
            }
            else
            {
                ddlDefObserv.DataSource = null; 
                ddlDefObserv.DataBind();
               // chkAllDefObserv.Checked = true;
                if (chkAllDefObserv.Checked == true)
                {
                    ddlDefObserv.Enabled = false;
                }
                //chkAllDefObserv_CheckedChanged(null,null);
            }
            
            ddlDefObserv.Items.Insert(0, new ListItem("Select Defect Observation", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Effectiveness Of The CAPA", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            string str = "";
            if (chkAllComp.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex > 0)
                {
                    str = str + "CRM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
                }
            }
            // dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE AND  IRN_TRANS_TYPE=0 AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and    I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "' ORDER BY I_NAME");
            dtItemDet = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL WHERE " + str + " ITEM_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND I_ACTIVE_IND=1 AND CRM_CODE=CRD_CRM_CODE AND CRD_I_CODE=I_CODE AND I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' ORDER BY I_CODENO");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Effectiveness Of The CAPA", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Item";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkAllComp.Checked == false)
                {
                    if (ddlCustomerName.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Customer";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
               
                string StrCond = "";
                if (chkAllItem.Checked != true)
                {
                    StrCond = StrCond + "  I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
                }
                if (chkAllComp.Checked != true)
                {
                    StrCond = StrCond + "P_CODE='" + ddlCustomerName.SelectedValue.ToString() + "' AND ";
                }
                if (chkAllDefObserv.Checked != true)
                {
                    StrCond = StrCond + "CRD_REASON='" + ddlDefObserv.SelectedValue.ToString() + "' AND ";
                }
                if (Session["PartyCode"].ToString() != "")
                {
                }
                Response.Redirect("../../IRN/ADD/EffectivenessOfCAPA.aspx?Title=" + Title + "&FDATE=" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "&TDATE=" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "&Cond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Effectiveness Of The CAPA", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Effectiveness Of The CAPA", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Effectiveness Of The CAPA", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;

        string str1 = "";
        string str = "";

        if (From != "" && To != "")
        {
            DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
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
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
        }
    }
    #endregion

    #region chkAllDefObserv_CheckedChanged
    protected void chkAllDefObserv_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllDefObserv.Checked == true)
        {
            ddlDefObserv.SelectedIndex = 0;
            ddlDefObserv.Enabled = false;
        }
        else
        {
            LoadDefObservation();
            ddlDefObserv.SelectedIndex = 0;
            ddlDefObserv.Enabled = true;
            ddlDefObserv.Focus();
        }
    }
    #endregion

    //#region chkDateAll_CheckedChanged
    //protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkDateAll.Checked == true)
    //    {
    //        txtFromDate.Enabled = false;
    //        txtTodate.Enabled = false;

    //        txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
    //        txtTodate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
    //        LoadCombos();
    //    }
    //    else
    //    {
    //        txtFromDate.Enabled = true;
    //        txtTodate.Enabled = true;

    //        txtFromDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
    //        txtTodate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
    //        LoadCombos();
    //    }
    //    dateCheck();
    //}
    //#endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
        LoadDefObservation();
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
        LoadDefObservation();
    }
    #endregion

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        //if (rbType.SelectedValue == "0")
        //{
        //    txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("MMM yyyy");
        //}
        //else
        //{
        //    txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(12).AddDays(-1).ToString("MMM yyyy");
        //}
        LoadCombos();
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtFromDate.Text).AddMonths(11); ;
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
                
                txtFromDate.Focus();
                return;
            }
            txtTodate.Text = todate.ToString("MMM yyyy");
        }
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        DateTime dttdate = Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1);
        txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).AddMonths(-11).ToString("MMM yyyy");
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        LoadCombos();

        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtTodate.Text = DateTime.Today.ToString("MMM yyyy");
                txtTodate.Focus();
                return;
            }
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
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion  chkAllComp_CheckedChanged

    #region rbType_SelectedIndexChanged
    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rbType.SelectedValue == "0")
        //{
        //   // txtTodate.Enabled = false;
        //    txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
        //    txtTodate.Text = System.DateTime.Now.ToString("MMM yyyy");
        //}
        //else
        //{
        //    txtTodate.Enabled = true;
        //    txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
        //    txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");
        //}
    }
    #endregion
}

