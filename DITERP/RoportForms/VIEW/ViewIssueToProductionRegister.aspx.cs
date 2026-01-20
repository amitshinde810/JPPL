using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewIssueToProductionRegister : System.Web.UI.Page
{
    static string right = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate1.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate1.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadCombos();
            LoadDept();
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            txtFromDate1.Enabled = false;
            txtToDate1.Enabled = false;
            chkDateAll1.Checked = true;
            chkAllItem.Checked = true;
          
            chkAlldept.Checked = true;
            ddlDepartment.Enabled = false;
        }
    }
    #endregion

    #region LoadDept
    private void LoadDept()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" SELECT * FROM DEPARTMENT ");
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataTextField = "Dept_Name";
        ddlDepartment.DataValueField = "Dept_Name";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("Select Department Name", "0"));
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            if (rbtType.SelectedIndex == 0)
            {
                dtItemDet = CommonClasses.Execute("Select  DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where IM_TYPE=2 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE AND IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate1.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate1.Text).ToString("dd/MMM/yyyy") + "' and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");
            }
            if (rbtType.SelectedIndex == 1)
            {
                dtItemDet = CommonClasses.Execute("Select DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where IM_TYPE=1 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE AND IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate1.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate1.Text).ToString("dd/MMM/yyyy") + "' and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");
            }
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
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll1.Checked == false)
            {
                if (txtFromDate1.Text == "" || txtToDate1.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            if (chkAlldept.Checked == false)
            {
                if (ddlDepartment.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select department Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            string From = "";
            string To = "";
            if (txtFromDate1.Text.Trim() != "" && txtToDate1.Text.Trim() != "")
            {
                From = Convert.ToDateTime(txtFromDate1.Text).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(txtToDate1.Text).ToString("dd/MMM/yyyy");
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }
            string str1 = "";
            string str = "";

            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Direct";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";
                }
            }
            #endregion

            #region AsperReq
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "AsperReq";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";
                }
            }
            #endregion

            string StrCond = "";
            if (chkDateAll1.Checked != true)
            {
                StrCond = StrCond + "  IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate1.Text) + "'  AND '" + Convert.ToDateTime(txtToDate1.Text) + "'  AND ";
            }
            if (chkAllItem.Checked != true)
            {
                StrCond = StrCond + "  IMD_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
            }
            if (chkAlldept.Checked != true)
            {
                StrCond = StrCond + "  IM_REQBY = '" + ddlDepartment.SelectedValue + "'  AND  ";
            }
            Response.Redirect("../../RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll1.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&Cond=" + StrCond + "&RTYPE=" + rbPrintType.SelectedValue + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
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
        From = txtFromDate1.Text;
        To = txtToDate1.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll1.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate1.Text);
                DateTime Date2 = Convert.ToDateTime(txtToDate1.Text);
                if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    return;
                }
            }
        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
            txtFromDate1.Enabled = false;
            txtToDate1.Enabled = false;
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
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll1.Checked == true)
        {
            txtFromDate1.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate1.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
            txtFromDate1.Enabled = false;
            txtToDate1.Enabled = false;

        }
        else
        {
            txtFromDate1.Enabled = true;
            txtToDate1.Enabled = true;
            txtFromDate1.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate1.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate1.Attributes.Add("readonly", "readonly");
            txtToDate1.Attributes.Add("readonly", "readonly");
           
        }
        //dateCheck();
    }
    #endregion

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
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion

    #region chkAlldept_CheckedChanged
    protected void chkAlldept_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAlldept.Checked == true)
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDepartment.Enabled = false;
        }
        else
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDepartment.Enabled = true;
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
    //    dateCheck();
    }
}
