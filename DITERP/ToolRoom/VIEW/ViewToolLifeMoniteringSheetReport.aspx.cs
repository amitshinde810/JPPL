using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class ToolRoom_VIEW_ViewToolLifeMoniteringSheetReport : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='174'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                ddlCustomerName.Enabled = false;
                chkAllCust.Checked = true;

                chkAllType.Checked = true;
                ddlType.Enabled = false;

                chkToolNo.Checked = true;
                ddlToolNo.Enabled = false;

                chkRType.Checked = true;
                ddlRefType.Enabled = false;

                LoadCust();
                LoadTool();
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtTodate.Attributes.Add("readonly", "readonly");
                chkDateAll.Checked = false;
                txtFromDate.Enabled = true;
                txtTodate.Enabled = true;
                //txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("MMM yyyy");
                //txtTodate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("MMM yyyy");
                txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
                txtTodate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    private void LoadCust()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (chkAllType.Checked != true)
        {
            if (ddlType.SelectedIndex != 0)
            {
                str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
            }
        }
        if (chkToolNo.Checked != true)
        {
            if (ddlToolNo.SelectedIndex != 0)
            {
                str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
            }
        }
        dt = CommonClasses.Execute("select distinct P_CODE,P_NAME from PARTY_MASTER,CUSTPO_MASTER,TOOL_MASTER where " + str + " PARTY_MASTER.ES_DELETE=0 and P_CODE=TOOL_MASTER.T_P_CODE and TOOL_MASTER.ES_DELETE=0 and P_TYPE=1 and P_ACTIVE_IND=1 and CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyID"] + " AND T_STATUS=1 order by P_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlCustomerName.DataSource = dt;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));

            if (ddlToolNo.SelectedIndex != -1)
            {
                ddlCustomerName.SelectedIndex = 1;
            }
        }
    }

    #region LoadTool
    private void LoadTool()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (chkAllType.Checked != true)
        {
            str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
        }
        if (chkAllCust.Checked != true)
        {
            if (ddlCustomerName.SelectedIndex != 0)
            {
                str = str + "T_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
            }
        }
        dt = CommonClasses.Execute("SELECT DISTINCT T_CODE,T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME FROM TOOL_MASTER,ITEM_MASTER WHERE " + str + " TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CM_COMP_ID=" + Session["Companyid"] + " AND T_STATUS=1 ORDER BY T_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));

            if (ddlCustomerName.SelectedIndex != 0)
            {
                ddlToolNo.SelectedIndex = 1;
            }
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string StrCond = "";
            string StrParty = "ALL";

            if (chkDateAll.Checked == false)
            {
                if (txtTodate.Text != "" && txtFromDate.Text != "")
                {
                }
                else
                {
                    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                }
            }

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                if (chkAllType.Checked == false)
                {
                    if (ddlType.SelectedIndex == -1)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Type";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
                    }
                }
                if (chkAllCust.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Customer Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "T_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
                    }
                }
                if (chkToolNo.Checked == false)
                {
                    if (ddlToolNo.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Tool No.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
                    }
                }
                if (chkAllCust.Checked != true)
                {
                    StrParty = ddlCustomerName.SelectedValue.ToString();
                }
                if (Session["PartyCode"].ToString() != "")
                {
                }
                Response.Redirect("../../ToolRoom/ADD/ToolLifeMoniteringSheetReport.aspx?Title=" + Title + "&Cond=" + StrCond + "&FDATE=" + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "&TDATE=" + Convert.ToDateTime(txtTodate.Text).ToString("MMM yyyy") + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Tooling Master Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllType_CheckedChanged
    protected void chkAllType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllType.Checked == true)
        {
            LoadCust(); LoadTool();
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = false;
        }
        else
        {
            LoadCust(); LoadTool();
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = true;
            ddlType.Focus();
        }
    }
    #endregion

    #region chkRType_CheckedChanged
    protected void chkRType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRType.Checked == true)
        {
            ddlRefType.SelectedIndex = 0;
            ddlRefType.Enabled = false;
        }
        else
        {
            ddlRefType.SelectedIndex = 0;
            ddlRefType.Enabled = true;
            ddlRefType.Focus();
        }
    }
    #endregion

    #region chkAllCust_CheckedChanged
    protected void chkAllCust_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCust.Checked == true)
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
    #endregion chkAllCust_CheckedChanged

    #region chkToolNo_CheckedChanged
    protected void chkToolNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkToolNo.Checked == true)
        {
            LoadTool();
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = false;
        }
        else
        {
            LoadTool();
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = true;
            ddlToolNo.Focus();
        }
    }
    #endregion chkToolNo_CheckedChanged

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCust();
        LoadTool();
    }
    #endregion ddlType_SelectedIndexChanged

    #region ddlRefType_SelectedIndexChanged
    protected void ddlRefType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCust();
    }
    #endregion ddlRefType_SelectedIndexChanged

    protected void ddlToolNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCust();
    }

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTool();
    }
    #endregion ddlType_SelectedIndexChanged

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        // txtTodate.Text = Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy");
        DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtTodate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            //ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtFromDate.Focus();
            return;
        }
       // txtTodate.Text = Convert.ToDateTime(dtDate1.AddMonths(1).ToString()).ToString("MMM yyyy");
        if (txtFromDate.Text != "" || txtTodate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtTodate.Text);

            if (txtFromDate.Text != "" || txtTodate.Text != "")

                if (fdate > todate)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date should be less than equal to To Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
                    txtFromDate.Focus();
                    return;
                }
        }
    }
    #endregion

    #region txtTodate_TextChanged
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {

        // DateTime dttdate = Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1);

        //txtFromDate.Text = Convert.ToDateTime(txtTodate.Text).ToString("MMM yyyy");
        // DateTime dtFdate = Convert.ToDateTime(txtFromDate.Text);

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

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtTodate.Enabled = false;

            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("MMM yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtTodate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtTodate.Text = System.DateTime.Now.ToString("MMM yyyy");
        }
        dateCheck();
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

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
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
        }
    }

    #endregion
}
