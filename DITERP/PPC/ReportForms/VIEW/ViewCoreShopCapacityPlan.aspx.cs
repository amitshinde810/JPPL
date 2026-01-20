using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class PPC_VIEW_ViewCoreShopCapacityPlan : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='206'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtMonth.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");
                LoadCombos(); //LoadGroup(); //Call Method
                ddlPartName.Enabled = false;
                txtMonth.Attributes.Add("readonly", "readonly");

                chkAllItem.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        //Join with Process BOM Master With Checking for PBM_MACHINING
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute("select DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME from PROCESS_BOM_MASTER PBM INNER JOIN ITEM_MASTER AS I ON PBM.PBM_I_CODE = I.I_CODE WHERE PBM.PBM_MACHINING='1' AND PBM.ES_DELETE = 0 AND I.ES_DELETE = 0  AND PBM.PBM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
        ddlPartName.DataSource = dtPartName;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                #region Validation
                //if (ddlGroup.SelectedIndex == 0)
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "Please Select Group Name";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    return;
                //}
                if (chkAllItem.Checked == false)
                {
                    if (ddlPartName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Part Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                #endregion Validation

                string StrCond = "";

                if (chkAllItem.Checked != true)
                    StrCond = StrCond + " SIM_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";

                //Response.Redirect("~/PPC/ReportForms/ADD/RMnSandRequire.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&GroupCode=" + ddlGroup.SelectedValue + "&StrCond=" + StrCond + "", false);
                Response.Redirect("~/PPC/ReportForms/ADD/CoreShopCapacityPlan.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Casting ToBe Machined Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Casting ToBe Machined Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Casting ToBe Machined Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlPartName.SelectedIndex = 0;
            ddlPartName.Enabled = false;
        }
        else
        {
            ddlPartName.SelectedIndex = 0;
            ddlPartName.Enabled = true;
            ddlPartName.Focus();
        }
    }
    #endregion

    #region ddlGroup_SelectedIndexChanged_Comment
    //protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable dtFinishItem = new DataTable();
    //    if (ddlPartName.SelectedIndex == 0)
    //    {
    //        //Join with Process BOM master for Machining,and Validate with customer schedule then bind Items
    //        //dtFinishItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME INTO #Temp from GROUP_MASTER INNER JOIN STANDARD_INVENTARY_MASTER ON GP_CODE =SIM_GP_CODE INNER JOIN ITEM_MASTER ON SIM_I_CODE=I_CODE WHERE GROUP_MASTER.ES_DELETE=0 AND STANDARD_INVENTARY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Session["CompanyId"] + "' and SIM_GP_CODE='" + ddlGroup.SelectedValue + "' SELECT I_CODE,ICODE_INAME FROM CUSTOMER_SCHEDULE inner join #Temp on CS_I_CODE=#Temp.I_CODE where ES_DELETE=0 order by ICODE_INAME drop table #Temp ");
    //        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM PRODUCT_MASTER PROD inner join PROCESS_BOM_MASTER PBM on PROD.PROD_I_CODE =PBM.PBM_I_CODE  INNER JOIN ITEM_MASTER AS I ON PROD.PROD_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON PBM.PBM_I_CODE=C.CS_I_CODE WHERE PBM.PBM_MACHINING='1' AND PBM.ES_DELETE = 0 AND I.ES_DELETE = 0 and PROD.ES_DELETE = 0 AND C.ES_DELETE = 0 AND PBM.PBM_COMP_ID='" + Session["CompanyId"] + "' and PROD_GP_CODE='" + ddlGroup.SelectedValue + "' ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
    //        if (dtFinishItem.Rows.Count > 0)
    //        {
    //            ddlPartName.DataSource = dtFinishItem;
    //            ddlPartName.DataTextField = "ICODE_INAME";
    //            ddlPartName.DataValueField = "I_CODE";
    //            ddlPartName.DataBind();
    //            ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    //        }
    //        else
    //        {
    //            ddlPartName.Items.Clear();
    //            ddlPartName.DataBind();
    //            ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    //            PanelMsg.Visible = true;
    //            lblmsg.Text = "There is no any Part Name for this Group...";
    //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
    //            return;
    //        }
    //    }
    //}
    #endregion ddlGroup_SelectedIndexChanged

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        if (txtMonth.Text != "")
        {
            LoadCombos();
        }
    }
    #endregion

    #region LoadGroup_Comment
    //protected void LoadGroup()
    //{
    //    DataTable dtGroup = new DataTable();
    //    dtGroup = CommonClasses.Execute("select distinct GP_NAME,GP_CODE from GROUP_MASTER where ES_DELETE=0 and GP_COMP_ID='" + Session["CompanyId"] + "' ORDER BY GP_NAME");
    //    ddlGroup.DataSource = dtGroup;
    //    ddlGroup.DataTextField = "GP_NAME";
    //    ddlGroup.DataValueField = "GP_CODE";
    //    ddlGroup.DataBind();
    //    ddlGroup.Items.Insert(0, new ListItem("Select Group Name", "0"));
    //}
    #endregion LoadGroup
}
