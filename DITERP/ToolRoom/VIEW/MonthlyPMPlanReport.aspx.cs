using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_VIEW_MonthlyPMPlanReport : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='200'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                chkToolNo.Checked = true;
                ddlToolNo.Enabled = false;
                LoadTool();
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadTool
    private void LoadTool()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (txtFromDate.Text != "")
        {
            str = str + "PM_MONTH='" +Convert.ToDateTime( txtFromDate.Text ).ToString("dd/MMM/yyyy")+ "' AND";
        }
        dt = CommonClasses.Execute("select distinct T_CODE,T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME as T_NAME from TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER where " + str + " T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 AND MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and TOOL_MASTER.ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " and T_CODE=PM_TOOL_NO and T_STATUS=1 order by T_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
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

            if (txtFromDate.Text != "")
            {
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            }

            if (txtFromDate.Text != "")
            {
                StrCond = StrCond + "PM_MONTH= '" + txtFromDate.Text + "' AND ";
            }

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {

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

                Response.Redirect("../../ToolRoom/ADD/MonthlyPMPlanReport.aspx?Title=" + Title + "&Cond=" + StrCond + "&FDATE=" + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "", false);
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

    #region chkToolNo_CheckedChanged
    protected void chkToolNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkToolNo.Checked == true)
        {
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = false;
        }
        else
        {
            LoadTool();
             
            ddlToolNo.Enabled = true;
            ddlToolNo.Focus();
        }
    }
    #endregion chkToolNo_CheckedChanged

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadTool();
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
        //}
        //else
        //{
        //    PanelMsg.Visible = false;
        //    lblmsg.Text = "";
        //    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
        //    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
        //    From = From1.ToShortDateString();
        //    To = To2.ToShortDateString();
        //}
    }
    #endregion
}
