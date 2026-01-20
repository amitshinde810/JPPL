using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewPrevmainPerformanceReport : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='171'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            ddlTool.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            chkDateAll.Checked = false;
            chkAllpart.Checked = true;
            ddlPartNo.Enabled = false;

            chkAllItem.Checked = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            //txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            //txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            LoadType();
            LoadPart();
        }
    }

    #region LoadType
    private void LoadType()
    {
        DataTable dt = new DataTable();
        string str = "";
        try
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str = "WPM_TOOL_RECEIVED_DATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
            }
            dt = CommonClasses.Execute("SELECT DISTINCT T_CODE,T_NAME FROM TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE " + str + " ( TOOL_MASTER.ES_DELETE = 0) AND T_CM_COMP_ID=" + Session["Companyid"] + " AND PM_TOOL_NO=WPM_T_CODE and WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND T_CODE=PM_TOOL_NO and MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 order by T_NAME");
            ddlTool.DataSource = dt;
            ddlTool.DataTextField = "T_NAME";
            ddlTool.DataValueField = "T_CODE";
            ddlTool.DataBind();
            ddlTool.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Tool No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Preventive Maintenance Performance Report", "LoadTools", Ex.Message);
        }
    }
    #endregion

    #region LoadPart
    private void LoadPart()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (ddlTool.SelectedIndex != 0)
        {
            str = str + "T_CODE=" + ddlTool.SelectedValue + " AND ";
        }
        try
        {
            dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+' - '+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER where " + str + " T_I_CODE=I_CODE and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and T_STATUS=1 order by I_NAME");
            ddlPartNo.DataSource = dt;
            ddlPartNo.DataTextField = "I_NAME";
            ddlPartNo.DataValueField = "I_CODE";
            ddlPartNo.DataBind();
            ddlPartNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Part No.", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Preventive Maintenance Performance Report", "LoadTools", Ex.Message);
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Improvement Register", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Improvement Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllpart_CheckedChanged
    protected void chkAllpart_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllpart.Checked == true)
        {
            ddlPartNo.SelectedIndex = 0;
            ddlPartNo.Enabled = false;
        }
        else
        {
            ddlPartNo.SelectedIndex = 0;
            ddlPartNo.Enabled = true;
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlTool.SelectedIndex = 0;
            ddlTool.Enabled = false;
        }
        else
        {
            ddlTool.SelectedIndex = 0;
            ddlTool.Enabled = true;
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            load();
            //Response.Redirect("../ADD/ImprovementRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "&InwdType=" + str2 + "&PTYPE=" + POType + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    string POType = "";
    string strCond = "";
    string From = "";
    string To = "";

    #region load
    private void load()
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

            if (chkAllItem.Checked == false)
            {
                if (ddlTool.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Tool No. ";
                    return;
                }
            }
            if (chkAllpart.Checked == false)
            {
                if (ddlPartNo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Part No. ";
                    return;
                }
            }

            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";

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
                From = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");
            }
            if (chkDateAll.Checked != true)
            {
                strCond = strCond + " PM_MONTH BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND ";
            }
            else
            {
                strCond = strCond + " PM_MONTH BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "' AND ";
            }
            if (chkAllItem.Checked == false)
            {
                strCond = strCond + " T_CODE='" + ddlTool.SelectedValue + "' AND ";
            }
            if (chkAllpart.Checked == false)
            {
                strCond = strCond + " T_I_CODE='" + ddlPartNo.SelectedValue + "' AND ";
            }
            Response.Redirect("../ADD/PrevmainPerformanceReport.aspx?Title=" + Title + "&FromDate=" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "&ToDate=" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "&Cond=" + strCond + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadType();
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtToDate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtFromDate.Focus();
            return;
        }
        txtToDate.Text = Convert.ToDateTime(dtDate1.AddMonths(1).AddDays(-1).ToString()).ToString("dd MMM yyyy");
        //txtToDate.Text = Convert.ToDateTime(dtDate1.AddMonths(1).ToString()).ToString("MMM yyyy");
        LoadType();
    }
    #endregion

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtToDate.Text);

        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtToDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtToDate.Focus();
            return;
        }
        if (dtDate1 < dtDate)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "To Date Should Be Less than From Date..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtToDate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
            return;
        }
        // txtFromDate.Text = Convert.ToDateTime(dtDate.AddMonths(-1).ToString()).ToString("MMM yyyy");
        LoadType();
    }
    #endregion txtToDate_TextChanged

    protected void ddlTool_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPart();
    }

    #region ddlPartNo_SelectedIndexChanged
    protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPartNo.SelectedValue != "0")
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + "  AND T_I_CODE='" + ddlPartNo.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN ( select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Year + "' AND ES_DELETE=0) order by T_NAME");

            if (dt.Rows.Count > 0)
            {
                ddlTool.DataSource = dt;
                ddlTool.DataTextField = "T_NAME";
                ddlTool.DataValueField = "T_CODE";
                ddlTool.DataBind();
                ddlTool.Items.Insert(0, new ListItem("Select Tool No.", "0"));
                ddlTool.SelectedIndex = 1;
            }
            else
            {
                ddlTool.DataSource = dt;
                ddlTool.DataBind();
                ddlTool.Items.Insert(0, new ListItem("Select Tool No.", "0"));
            }
        }
        else
        {
            LoadType();
        }
    }
    #endregion ddlPartNo_SelectedIndexChanged
}
