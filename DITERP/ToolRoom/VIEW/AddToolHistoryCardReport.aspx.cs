using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_ADD_AddToolHistoryCardReport : System.Web.UI.Page
{
    static string right = "";
    DataTable dtSubComponenet1;
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='178'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                LoadTool();
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
        //Loadreport();
    }
    #endregion

    #region LoadTool
    private void LoadTool()
    {
        DataTable dt = new DataTable();
        string str = "";

        str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";

        //dt = CommonClasses.Execute("select distinct T_CODE,T_NAME from TOOL_MASTER where " + str + " TOOL_MASTER.ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " and T_STATUS=1 order by T_NAME");
        dt = CommonClasses.Execute("SELECT TOOL_MASTER.T_CODE, T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (BREAKDOWN_ENTRY.B_STATUS = 1) AND (ITEM_MASTER.ES_DELETE=0) UNION SELECT WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE AS T_CODE, T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER INNER JOIN TOOL_MASTER ON WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE = 0) AND (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_STATUS = 1) AND (ITEM_MASTER.ES_DELETE=0)");
        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
        else
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTool();
    }
    #endregion ddlType_SelectedIndexChanged

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool History card", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Loadreport
    private void Loadreport()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (ddlType.SelectedIndex != -1)
        {
            str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
        }
        if (ddlToolNo.SelectedIndex != 0)
        {
            str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
        }
        //dt = CommonClasses.Execute("SELECT BREAKDOWN_ENTRY.B_NO,BREAKDOWN_ENTRY.B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,BREAKDOWN_ENTRY.B_DATE,106) as B_DATE, BREAKDOWN_ENTRY.B_CLOSURE,BREAKDOWN_ENTRY.B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (BREAKDOWN_ENTRY.B_STATUS = 1) UNION SELECT 0 AS B_NO, WPM_T_CODE AS B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,WPM_TOOL_RECEIVED_DATE,106) AS B_DATE, WPM_TOOL_COMPLETED_DATE AS B_CLOSURE,I_CODE AS  B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER INNER JOIN TOOL_MASTER ON WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE  = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE = 0) AND (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_STATUS = 1) ");
        dt = CommonClasses.Execute("SELECT T_CODE AS B_T_CODE,T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,I_CODE,I_NAME,I_CODENO FROM TOOL_MASTER,ITEM_MASTER where " + str + " T_I_CODE=I_CODE AND TOOL_MASTER.ES_DELETE=0 AND (T_CODE IN (SELECT B_T_CODE FROM BREAKDOWN_ENTRY where BREAKDOWN_ENTRY.ES_DELETE=0) OR T_CODE IN (SELECT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0))");
        if (dt.Rows.Count > 0)
        {
            dgComponent.DataSource = dt;
            dgComponent.DataBind();
        }
        else
        {
            dgComponent.DataSource = dt;
            dgComponent.DataBind();
        }
        //dgComponent_RowDataBound(null, null); 
    }
    #endregion

    protected void dgComponent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dtSubComponenet = new DataTable();
        //DataTable dtSubComponenet1 = new DataTable();
        int code = 0, tot = 0;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grid = (GridView)sender;
                code = Convert.ToInt32(grid.DataKeys[e.Row.RowIndex].Values["B_T_CODE"].ToString());
                Session["B_T_CODE"] = code;

                string str = "";
                if (ddlType.SelectedIndex != -1)
                {
                    str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
                }
                if (ddlToolNo.SelectedIndex != 0)
                {
                    str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
                }
                dtSubComponenet1 = CommonClasses.Execute("SELECT CASE WHEN B_TYPE = 1 THEN 'IP' ELSE 'BD' END AS B_TYPE,BREAKDOWN_ENTRY.B_NO,BREAKDOWN_ENTRY.B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,BREAKDOWN_ENTRY.B_DATE,106) as B_DATE, BREAKDOWN_ENTRY.B_CLOSURE,BREAKDOWN_ENTRY.B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,BREAKDOWN_ENTRY.B_CODE, BREAKDOWN_ENTRY.B_FILE FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (BREAKDOWN_ENTRY.B_STATUS = 1) UNION SELECT 'PM' AS B_TYPE, 0 AS B_NO, WPM_T_CODE AS B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,WPM_TOOL_RECEIVED_DATE,106) AS B_DATE, WPM_TOOL_COMPLETED_DATE AS B_CLOSURE,I_CODE AS  B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,WPM_CODE  AS B_CODE, WPM_UPLOAD_FILE AS B_FILE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER INNER JOIN TOOL_MASTER ON WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE  = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE = 0) AND (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_STATUS = 1) ");

                GridView dgSubComponent = e.Row.FindControl("dgSubComponent") as GridView;

                if (dtSubComponenet1.Rows.Count > 0)
                {
                    dgSubComponent.DataSource = dtSubComponenet1;
                    dgSubComponent.DataBind();
                }
                else
                {
                    dgSubComponent.DataSource = null;
                    dgSubComponent.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void dgComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void dgSubComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            string code = "";
            string directory = "";
            GridView grid = (GridView)sender;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvRow = grid.Rows[index];
            if (e.CommandName == "ViewPDF")
            {
                string B_CODE = ((Label)(gvRow.FindControl("lblBCode"))).Text;
                string str = "";
                if (ddlType.SelectedIndex != -1)
                {
                    str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
                }
                if (ddlToolNo.SelectedIndex != 0)
                {
                    str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
                }
                DataTable dtBCode = CommonClasses.Execute("SELECT CASE WHEN B_TYPE = 1 THEN 'IP' ELSE 'BD' END AS B_TYPE,BREAKDOWN_ENTRY.B_NO,BREAKDOWN_ENTRY.B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,BREAKDOWN_ENTRY.B_DATE,106) as B_DATE, BREAKDOWN_ENTRY.B_CLOSURE,BREAKDOWN_ENTRY.B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,BREAKDOWN_ENTRY.B_CODE, BREAKDOWN_ENTRY.B_FILE FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (BREAKDOWN_ENTRY.B_STATUS = 1) AND (B_CODE= '" + B_CODE + "') UNION SELECT 'PM' AS B_TYPE, 0 AS B_NO, WPM_T_CODE AS B_T_CODE, TOOL_MASTER.T_NAME,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,convert(varchar,WPM_TOOL_RECEIVED_DATE,106) AS B_DATE, WPM_TOOL_COMPLETED_DATE AS B_CLOSURE,I_CODE AS  B_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,WPM_CODE  AS B_CODE, WPM_UPLOAD_FILE AS B_FILE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER INNER JOIN TOOL_MASTER ON WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE  = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE = 0) AND (WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_STATUS = 1) AND (WPM_CODE= '" + B_CODE + "')  ");

                filePath = ((LinkButton)(gvRow.FindControl("lnkView"))).Text;
                if (dtBCode.Rows[0]["B_TYPE"].ToString() == "IP")
                {
                    directory = "../../UpLoadPath/Improvement/" + dtBCode.Rows[0]["B_CODE"].ToString() + "/" + filePath;
                }
                else if (dtBCode.Rows[0]["B_TYPE"].ToString() == "PM")
                {
                    directory = "../../UpLoadPath/WEEKLYPMAIN/" + dtBCode.Rows[0]["B_CODE"].ToString() + "/" + filePath;
                }
                else
                {
                    directory = "../../UpLoadPath/BreakDown/" + dtBCode.Rows[0]["B_CODE"].ToString() + "/" + filePath;
                }
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }
        }
        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Improvement Entry", "dgBreakdown_RowCommand", Ex.Message);
        }
    }

    protected void dgSubComponent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlType.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (ddlToolNo.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Tool No.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            Loadreport();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tool History card", "btnShow_Click", ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex == -1)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Type";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        if (ddlToolNo.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Tool No.";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        string str = "";
        if (ddlType.SelectedIndex != -1)
        {
            str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
        }
        if (ddlToolNo.SelectedIndex != 0)
        {
            str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
        }
        Loadreport();
        // ExportGridToPDF();
        Response.Redirect("../ADD/ToolHistoryRpt.aspx?Title=" + Title + "&StrCond=" + str + "", false);
    }
    #endregion
}
