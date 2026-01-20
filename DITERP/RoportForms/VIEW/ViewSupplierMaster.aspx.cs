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

public partial class RoportForms_VIEW_ViewSupplierMaster : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='242'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                chkAll.Checked = true;
                ddlSupplier.Enabled = false;
                chkCategory.Checked = true;
                ddlCategory.Enabled = false;

            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region Loadreport
    private void Loadreport(string str)
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT P_CODE,P_NAME,P_CONTACT,P_PARTY_CODE,P_VEND_CODE,P_ADD1	,P_CITY,P_PIN_CODE ,P_PHONE,P_MOB, P_EMAIL,P_PAN	,P_CST,P_LBT_NO ,P_CITY,P_COORDINATOR,	P_COORDINATOR_EMAIL FROM PARTY_MASTER  where " + str + "  ES_DELETE=0 AND P_ACTIVE_IND=1 AND P_INHOUSE_IND=1  ");

        dgBreakdown.DataSource = dt;
        dgBreakdown.DataBind();
    }
    #endregion

    #region LoadToolNo
    private void LoadToolNo()
    {
        DataTable dt = new DataTable();
        string str = "";

        dt = CommonClasses.Execute("select  DISTINCT P_CODE,P_NAME from PARTY_MASTER where ES_DELETE=0 AND P_INHOUSE_IND=1  order by P_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
        }
    }
    #endregion LoadToolNo

    #region LoadCategory
    private void LoadCategory()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select DISTINCT SCM_CODE,SCM_NAME from SUPPLIER_CATEGORY_MASTER where ES_DELETE=0 AND SCM_CM_COMP_ID='" + Session["CompanyId"].ToString() + "'  order by SCM_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "SCM_NAME";
            ddlCategory.DataValueField = "SCM_CODE";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Select Supplier Category", "0"));
        }
    }
    #endregion LoadCategory

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string StrCond = "";
            string StrParty = "ALL";
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                if (chkAll.Checked == false)
                {
                    if (ddlSupplier.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Supplier";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
                    }
                }
                if (chkAll.Checked == false && chkCategory.Checked == true)
                {
                    StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
                }
                else if (chkAll.Checked == false && chkCategory.Checked == false)
                {
                    StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND P_E_CODE='" + ddlCategory.SelectedValue + "' AND ";
                }
                else if (chkAll.Checked == true && chkCategory.Checked == false)
                {
                    StrCond = StrCond + " P_E_CODE='" + ddlCategory.SelectedValue + "' AND ";
                }
                Loadreport(StrCond);
                // Response.Redirect("../../ToolRoom/ADD/WeeklyPM.aspx?Title=" + Title + "&Cond=" + StrCond + " ", false);
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
            string StrCond = "";
            string StrParty = "ALL";
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {

                if (chkAll.Checked == false)
                {
                    if (ddlSupplier.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Supplier";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
                    }
                }
                if (chkAll.Checked == false && chkCategory.Checked == true)
                {
                    StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
                }
                else if (chkAll.Checked == false && chkCategory.Checked == false)
                {
                    StrCond = StrCond + "P_CODE='" + ddlSupplier.SelectedValue + "' AND P_E_CODE='" + ddlCategory.SelectedValue + "' AND ";
                }
                else if (chkAll.Checked == true && chkCategory.Checked == false)
                {
                    StrCond = StrCond + " P_E_CODE='" + ddlCategory.SelectedValue + "' AND ";
                }
                // Loadreport(StrCond);
                Response.Redirect("../../RoportForms/ADD/SupplierMaster.aspx?Title=" + Title + "&Cond=" + StrCond + " ", false);
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
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
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
        if (chkAll.Checked == true)
        {
            ddlSupplier.SelectedIndex = 0;
            ddlSupplier.Enabled = false;
        }
        else
        {
            LoadToolNo();
            ddlSupplier.SelectedIndex = 0;
            ddlSupplier.Enabled = true;
            ddlSupplier.Focus();
        }
    }
    #endregion chkToolNo_CheckedChanged

    #region chkCategory_CheckedChanged
    protected void chkCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCategory.Checked == true)
        {
            ddlCategory.SelectedIndex = 0;
            ddlCategory.Enabled = false;
        }
        else
        {
            LoadCategory();
            ddlCategory.SelectedIndex = 0;
            ddlCategory.Enabled = true;
            ddlCategory.Focus();
        }
    }
    #endregion chkCategory_CheckedChanged

    #region dgBreakdown_PageIndexChanging
    protected void dgBreakdown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBreakdown.PageIndex = e.NewPageIndex;
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
            GridViewRow gvRow = dgBreakdown.Rows[index];
            if (e.CommandName == "ViewPDF")
            {
                code = ((Label)(gvRow.FindControl("lblP_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView"))).Text;
                directory = "../../UpLoadPath/SUPPILER/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;

                //filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                //directory = "../../UpLoadPath/QuotationEntry/" + code + "/" + filePath;
                //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
                //Response.TransmitFile(Server.MapPath(directory));
                //Response.End();
            }
            if (e.CommandName == "ViewPDF1")
            {
                code = ((Label)(gvRow.FindControl("lblP_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView1"))).Text;
                directory = "../../UpLoadPath/SUPPILER/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;

                //filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                //directory = "../../UpLoadPath/QuotationEntry/" + code + "/" + filePath;
                //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
                //Response.TransmitFile(Server.MapPath(directory));
                //Response.End();
            }
        }
        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Improvement Entry", "dgBreakdown_RowCommand", Ex.Message);
        }
    }
    #endregion dgBreakdown_RowCommand


}
