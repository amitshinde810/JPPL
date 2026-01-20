using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_VIEW_ViewBreakdownOccuChartReport : System.Web.UI.Page
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
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='177'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            ddlTool.Enabled = false;
            txtFromDate.Enabled = true;
            txtTodate.Enabled = true;
            chkDateAll.Checked = false;
            chkAllpart.Checked = true;
            ddlPartNo.Enabled = false;
            tbType.SelectedValue = "0";
            chkAllType.Checked = true;
            ddlType.Enabled = false;

            chkAlltool.Checked = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtTodate.Attributes.Add("readonly", "readonly");
            //txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            //txtTodate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtTodate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
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
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text != "" && txtTodate.Text != "")
                {
                    str = str + "convert(date, B_DATE) BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "' AND ";
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");

                str = str = "convert(date, B_DATE) BETWEEN '" + From + "' AND '" + To + "' AND ";
            }
            if (chkAllType.Checked == false)
            {
                if (ddlType.SelectedIndex != -1)
                {
                    str = str + "B_T_TYPE='" + ddlType.SelectedValue + "' AND ";
                }
            }
            dt = CommonClasses.Execute("select distinct T_CODE,T_NAME from BREAKDOWN_ENTRY,TOOL_MASTER where " + str + " T_CODE=B_T_CODE and TOOL_MASTER.ES_DELETE=0 AND BREAKDOWN_ENTRY.ES_DELETE=0 AND T_CM_COMP_ID='" + Session["CompanyId"] + "' AND B_TYPE=0 order by T_NAME");
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
        if (chkDateAll.Checked == false)
        {
            if (txtFromDate.Text != "" && txtTodate.Text != "")
            {
                str = str = "convert(date, B_DATE) BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "' AND ";
            }
        }
        else
        {
            From = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
            To = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");

            str = str = "convert(date, B_DATE) BETWEEN '" + From + "' AND '" + To + "' AND ";
        }

        if (chkAllType.Checked == false)
        {
            if (ddlType.SelectedIndex != -1)
            {
                str = str + "B_T_TYPE=" + ddlType.SelectedValue + " AND ";
            }
        }
        if (chkAlltool.Checked == false)
        {
            if (ddlTool.SelectedIndex != 0)
            {
                str = str + "B_T_CODE=" + ddlTool.SelectedValue + " AND ";
            }
        }
        try
        {
            dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+' - '+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER,BREAKDOWN_ENTRY where " + str + " T_I_CODE=I_CODE and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and T_STATUS=1 AND BREAKDOWN_ENTRY.ES_DELETE=0 AND BREAKDOWN_ENTRY.B_T_CODE=T_CODE and B_I_CODE=T_I_CODE AND B_TYPE=0 order by I_NAME");
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
            txtTodate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtTodate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtTodate.Attributes.Add("readonly", "readonly");
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

    #region chkAlltool_CheckedChanged
    protected void chkAlltool_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAlltool.Checked == true)
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

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadType();
        //LoadPart();
    }
    #endregion ddlType_SelectedIndexChanged

    #region chkAllType_CheckedChanged
    protected void chkAllType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllType.Checked == true)
        {
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = false;
        }
        else
        {
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = true;
            ddlType.Focus();
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            load();
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
                if (txtFromDate.Text == "" || txtTodate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    return;
                }
            }

            if (chkAllType.Checked == false)
            {
                if (ddlType.SelectedIndex == -1)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Tool Type ";
                    return;
                }
            }

            if (chkAlltool.Checked == false)
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
            To = txtTodate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtTodate.Text);
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
                From = Convert.ToDateTime(Session["OpeningDate"]).ToShortDateString();
                To = Convert.ToDateTime(Session["ClosingDate"]).ToShortDateString();
            }
            if (chkDateAll.Checked != true)
            {
                strCond = strCond + " B_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "' AND ";
            }
            else
            {
                strCond = strCond + " B_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "' AND ";
            }
            if (chkAlltool.Checked == false)
            {
                strCond = strCond + " B_T_CODE='" + ddlTool.SelectedValue + "' AND ";
            }
            if (chkAllpart.Checked == false)
            {
                strCond = strCond + " T_I_CODE='" + ddlPartNo.SelectedValue + "' AND ";
            }
            if (chkAllType.Checked == false)
            {
                strCond = strCond + " B_T_TYPE='" + ddlType.SelectedValue + "' AND ";
            }
            Response.Redirect("../ADD/BreakdownOccuChartReport.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&Cond=" + strCond + "&Type=" + tbType.SelectedValue + "", false);
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
        DateTime dtDate = Convert.ToDateTime(txtTodate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtFromDate.Focus();
            return;
        }
        txtTodate.Text = Convert.ToDateTime(dtDate1.AddMonths(1).ToString()).ToString("MMM yyyy");
        LoadType();
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtTodate.Text);

        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtTodate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtTodate.Focus();
            return;
        }
        if (dtDate1 < dtDate)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "To Date Should Be Less than From Date..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtTodate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
            return;
        }
         txtFromDate.Text = Convert.ToDateTime(dtDate.AddMonths(-1).ToString()).ToString("MMM yyyy");
        LoadType();
    }
    #endregion

    protected void ddlTool_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPart();
    }

    #region tbType_SelectedIndexChanged
    protected void tbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DateTime From1 = new DateTime();
        //DateTime To2 = new DateTime();
        //if (tbType.SelectedValue == "0") //.Equals(0)
        //{
        //    From1 = DateTime.Now;
        //    To2 = DateTime.Now.AddMonths(1);
        //    txtFromDate_CalenderExtender.BehaviorID = "";
        //    txtFromDate_CalenderExtender.TargetControlID = "txtFromDate";
        //    DateTime dtcurrentdate = System.DateTime.Now;
        //    txtFromDate.Text = dtcurrentdate.ToString("01 MMM yyyy");
        //    txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(30).ToString("dd MMM yyyy");
        //    txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
        //    txtFromDate_CalenderExtender.Enabled = true;
        //    CalendarExtender1.BehaviorID = "";
        //    CalendarExtender1.PopupButtonID = "txtTodate";
        //    CalendarExtender1.TargetControlID = "txtTodate";
        //    CalendarExtender1.Enabled = true;
        //    //txtFromDate.Text = From1.ToString("dd MMM yyyy");
        //    //txtTodate.Text = To2.ToString("dd MMM yyyy");
        //}
        //else
        //{
        //    From1 = Convert.ToDateTime(Session["OpeningDate"]);
        //    To2 = Convert.ToDateTime(Session["ClosingDate"]);
        //    txtFromDate_CalenderExtender.BehaviorID = "calendar1";
        //    txtFromDate_CalenderExtender.TargetControlID = "txtFromDate";
        //    txtFromDate_CalenderExtender.OnClientHidden = "onCalendarHidden";
        //    txtFromDate_CalenderExtender.OnClientShown = "onCalendarShown";
        //    txtFromDate_CalenderExtender.PopupButtonID = "txtFromDate";
        //    txtFromDate_CalenderExtender.Enabled = true;
        //    CalendarExtender1.BehaviorID = "calendar2";
        //    CalendarExtender1.OnClientHidden = "onCalendarHidden2";
        //    CalendarExtender1.OnClientShown = "onCalendarShown2";
        //    CalendarExtender1.PopupButtonID = "txtTodate";
        //    CalendarExtender1.TargetControlID = "txtTodate";
        //    CalendarExtender1.Enabled = true;
        //    txtFromDate.Text = From1.ToString("MMM yyyy");
        //    txtTodate.Text = To2.ToString("MMM yyyy");
        //}
    }
    #endregion

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

