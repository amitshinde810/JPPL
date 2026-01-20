using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class ToolRoom_VIEW_ViewToolMasterReport : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='199'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                ddlCustomerName.Enabled = false;
                chkAllCust.Checked = true;

                chkAllType.Checked = true;
                ddlType.Enabled = false;

                chkToolNo.Checked = true;
                ddlToolNo.Enabled = false;

                LoadCust();
                LoadTool();
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
            if (ddlType.SelectedIndex != 0)
            {
                str = str + "T_TYPE='" + ddlType.SelectedValue + "' AND ";
            }
        }
        if (chkAllCust.Checked != true)
        {
            if (ddlCustomerName.SelectedIndex != 0)
            {
                str = str + "T_P_CODE='" + ddlCustomerName.SelectedValue + "' AND ";
            }
        }
        dt = CommonClasses.Execute("select distinct T_CODE,T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME as T_NAME from TOOL_MASTER,ITEM_MASTER where " + str + " TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CM_COMP_ID=" + Session["CompanyID"] + " and T_STATUS=1 order by T_NAME");

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
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            string StrCond = "";
            string StrParty = "ALL";

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
                Response.Redirect("../../ToolRoom/ADD/ToolMasterReport.aspx?Title=" + Title + "&Cond=" + StrCond + "", false);
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

    #region btnShow1_Click
    protected void btnShow1_Click(object sender, EventArgs e)
    {
        try
        {
            load();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion


    public void load()
    {
        DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
        DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
        string StrCond = "";
        string StrParty = "ALL";


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
        DataTable dt2 = new DataTable();

        string Query = "select T_CODE,T_NAME,T_P_CODE,T_I_CODE,T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,T_PENDTOOLLIFEMONTH,case when T_OWNER=0 then 'PCPL' else 'Customer' end as T_OWNER,T_PMFREQ,T_3D,	T_3D_PATH,T_REVNO,convert(varchar,T_REV_DATE,106) as T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,case when T_TYPE=0 then 'DIE' else 'CORE BOX' end as T_TYPE from TOOL_MASTER,ITEM_MASTER,PARTY_MASTER where " + StrCond + " I_CODE=TOOL_MASTER.T_I_CODE and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and P_CODE=TOOL_MASTER.T_P_CODE and TOOL_MASTER.T_CM_COMP_ID=1 and TOOL_MASTER.T_STATUS=1 and T_CM_COMP_ID='" + Session["CompanyID"] + "'";

        dt2 = CommonClasses.Execute(Query);
        dgBreakdown.DataSource = dt2;
        dgBreakdown.DataBind();

    }
    #region dgBreakdown_PageIndexChanging
    protected void dgBreakdown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBreakdown.PageIndex = e.NewPageIndex;
            load();


        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgBreakdown_RowCommand
    protected void dgBreakdown_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            string code = "";
            string directory = "";

            int index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ViewPDF")
            {
                int page = dgBreakdown.PageIndex;
                int Size = dgBreakdown.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgBreakdown.Rows[index]; 

                code = ((Label)(gvRow.FindControl("lblT_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView"))).Text;
                directory = "../../UpLoadPath/ToolRoom/ToolPhoto/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }
            if (e.CommandName == "ViewPDF1")
            {
                int page = dgBreakdown.PageIndex;
                int Size = dgBreakdown.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgBreakdown.Rows[index];
                 

                code = ((Label)(gvRow.FindControl("lblT_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView1"))).Text;
                directory = "../../UpLoadPath/ToolRoom/3DModel/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }



            if (e.CommandName == "Download")
            {

                int page = dgBreakdown.PageIndex;
                int Size = dgBreakdown.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgBreakdown.Rows[index];

                //filePath = ((Label)(gvRow.FindControl("lblfilename"))).Text;
                code = ((Label)(gvRow.FindControl("lblB_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView1"))).Text;
                directory = "../../UpLoadPath/BreakDown/" + code + "/" + filePath;
                string absolute = Path.GetFullPath(directory);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
                Response.TransmitFile(Server.MapPath(directory));
                //Response.Flush();
                FileInfo fileToDownload = new FileInfo(directory);

                Response.Flush();

                Response.WriteFile(fileToDownload.FullName);
                Response.End();

            }
        }
        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Improvement Entry", "dgBreakdown_RowCommand", Ex.Message);
        }
    }
    #endregion dgBreakdown_RowCommand

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
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = false;
        }
        else
        {
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
    }
    #endregion ddlType_SelectedIndexChanged

    protected void ddlToolNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCust();
    }

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTool();
    }
    #endregion ddlCustomerName_SelectedIndexChanged




}