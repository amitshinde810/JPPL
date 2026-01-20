using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class RoportForms_VIEW_OverallPerfView : System.Web.UI.Page
{
    static string right = "";
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(System.DateTime.Now).ToString("MMM yyyy");

            LoadCombos();
            ddlCategory.Enabled = false;
            txtFromDate.Enabled = true;

            chkSupplier.Checked = true;
            txtFromDate.Attributes.Add("readonly", "readonly");

        }
    }
    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkSupplier.Checked == false)
            {
                if (ddlCategory.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Supplier ";
                    return;
                }
            }
            string Cond = "";
            string From = "";
            string To = "";
            From = txtFromDate.Text;

            string str1 = "";
            string str = "Overall";

            //DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime From1 = Convert.ToDateTime(txtFromDate.Text);
            From = From1.ToString("dd/MMM/yyyy");

            if (chkSupplier.Checked != true)
            {
                Cond = Cond + " P.P_E_CODE='" + ddlCategory.SelectedValue + "' AND ";
            }

            Response.Redirect("~/RoportForms/ADD/QualityPerfReport.aspx?Title=" + Title + "&FromDate=" + From + "&str=" + str + "&Cond=" + Cond + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Quality Performance Monitoring Sheet", "btnShow_Click", Ex.Message);
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
        catch (Exception)
        {

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
            CommonClasses.SendError("Quality Performance Monitoring Sheet", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkSupplier_CheckedChanged
    protected void chkSupplier_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSupplier.Checked == true)
        {
            ddlCategory.SelectedIndex = 0;
            ddlCategory.Enabled = false;
        }
        else
        {
            ddlCategory.SelectedIndex = 0;
            ddlCategory.Enabled = true;
            ddlCategory.Focus();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
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
    #endregion
}
