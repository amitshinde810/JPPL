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

public partial class PPC_VIEW_ViewPurchaseRequirement : System.Web.UI.Page
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
                LoadCombos(); LoadBoughout(); //Call Method

                chkAllItem.Checked = true;
                chkBoughtOut.Checked = true;
                ddlPartName.Enabled = false;
                ddlBoughtOut.Enabled = false;
                txtMonth.Attributes.Add("readonly", "readonly");
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
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME from ITEM_MASTER I inner join BOUGHTOUT_MASTER BM on I.I_CODE=BM.BOM_I_CODE where BM.ES_DELETE=0 AND I.ES_DELETE=0  and I.I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
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
                if (chkBoughtOut.Checked == false)
                {
                    if (ddlBoughtOut.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select BoghtOut Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                #endregion Validation

                #region StrCond
                string StrCond = "";
                if (chkBoughtOut.Checked != true && chkAllItem.Checked == true)
                    StrCond = StrCond + " BOD.BOD_I_BOUGHT_CODE='" + ddlBoughtOut.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkBoughtOut.Checked == true)
                    StrCond = StrCond + " BOM.BOM_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkBoughtOut.Checked != true)
                    StrCond = StrCond + " BOD.BOD_I_BOUGHT_CODE='" + ddlBoughtOut.SelectedValue + "' AND BOM.BOM_I_CODE = '" + ddlPartName.SelectedValue + "' AND ";
                #endregion StrCond

                Response.Redirect("~/PPC/ReportForms/ADD/PurchaseRequirement.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&GroupCode=" + ddlBoughtOut.SelectedValue + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Casting ToBe Inspected Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Casting ToBe Inspected Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Casting ToBe Inspected Report", "ShowMessage", Ex.Message);
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

    #region ddlBoughtOut_SelectedIndexChanged
    protected void ddlBoughtOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtFinishItem = new DataTable();
        if (ddlPartName.SelectedIndex == 0)
        {
            //Join with standard Inventory master,and Validate with customer schedule then bind Items
            dtFinishItem = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME INTO #Temp from GROUP_MASTER INNER JOIN STANDARD_INVENTARY_MASTER ON GP_CODE =SIM_GP_CODE INNER JOIN ITEM_MASTER ON SIM_I_CODE=I_CODE WHERE GROUP_MASTER.ES_DELETE=0 AND STANDARD_INVENTARY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Session["CompanyId"] + "' and SIM_GP_CODE='" + ddlBoughtOut.SelectedValue + "' SELECT I_CODE,ICODE_INAME FROM CUSTOMER_SCHEDULE inner join #Temp on CS_I_CODE=#Temp.I_CODE where ES_DELETE=0 order by ICODE_INAME drop table #Temp ");
            ddlPartName.DataSource = dtFinishItem;
            ddlPartName.DataTextField = "ICODE_INAME";
            ddlPartName.DataValueField = "I_CODE";
            ddlPartName.DataBind();
            ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
        }
    }
    #endregion ddlBoughtOut_SelectedIndexChanged

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtBoughtOut = new DataTable();
        dtBoughtOut = CommonClasses.Execute("SELECT DISTINCT BOD.BOD_I_BOUGHT_CODE,I.I_NAME AS BoughtOutName from ITEM_MASTER I inner join BOUGHTOUT_DETAIL BOD on I.I_CODE=BOD.BOD_I_BOUGHT_CODE inner join BOUGHTOUT_MASTER BM on BOD.BOD_BOM_CODE=BM.BOM_CODE where BM.ES_DELETE=0 AND I.ES_DELETE=0 and I.I_CAT_CODE='-2147483647' and I.I_CM_COMP_ID='" + Session["CompanyId"] + "' AND and BM.BOM_I_CODE='" + ddlPartName.SelectedValue + "' order by I_NAME");
        if (dtBoughtOut.Rows.Count > 0)
        {
            ddlBoughtOut.DataSource = dtBoughtOut;
            ddlBoughtOut.DataTextField = "BoughtOutName";
            ddlBoughtOut.DataValueField = "BOD_I_BOUGHT_CODE";
            ddlBoughtOut.DataBind();
            ddlBoughtOut.Items.Insert(0, new ListItem("Select Boughout Name", "0"));
        }
        else
        {
            ddlBoughtOut.Items.Clear();
            ddlBoughtOut.DataBind();
        }
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

    #region LoadBoughout
    protected void LoadBoughout()
    {
        // Check Hardcoded Boughtout Item -2147483647
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("SELECT DISTINCT BOD.BOD_I_BOUGHT_CODE,I.I_NAME AS BoughtOutName from ITEM_MASTER I inner join BOUGHTOUT_DETAIL BOD on I.I_CODE=BOD.BOD_I_BOUGHT_CODE inner join BOUGHTOUT_MASTER BM on BOD.BOD_BOM_CODE=BM.BOM_CODE where BM.ES_DELETE=0 AND I.ES_DELETE=0 and I.I_CAT_CODE='-2147483647' and I.I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_NAME");
        ddlBoughtOut.DataSource = dtProcess;
        ddlBoughtOut.DataTextField = "BoughtOutName";
        ddlBoughtOut.DataValueField = "BOD_I_BOUGHT_CODE";
        ddlBoughtOut.DataBind();
        ddlBoughtOut.Items.Insert(0, new ListItem("Select Boughout Name", "0"));
    }
    #endregion LoadBoughout

    #region chkBoughtOut_CheckedChanged
    protected void chkBoughtOut_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBoughtOut.Checked == true)
        {
            ddlBoughtOut.SelectedIndex = 0;
            ddlBoughtOut.Enabled = false;
        }
        else
        {
            ddlBoughtOut.SelectedIndex = 0;
            ddlBoughtOut.Enabled = true;
            ddlBoughtOut.Focus();
        }
    }
    #endregion chkBoughtOut_CheckedChanged
}
