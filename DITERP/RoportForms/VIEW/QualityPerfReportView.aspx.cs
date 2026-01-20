using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class RoportForms_VIEW_QualityPerfReportView : System.Web.UI.Page
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
            ddlSupplier.Enabled = false;
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
                if (ddlSupplier.SelectedIndex == 0)
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
            string str = "QualityPerf";

            //DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime From1 = Convert.ToDateTime(txtFromDate.Text);
            From = From1.ToString("dd/MMM/yyyy");

            if (chkSupplier.Checked != true)
            {
                Cond = Cond + " P.P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
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
            ddlSupplier.SelectedIndex = 0;
            ddlSupplier.Enabled = false;
        }
        else
        {
            ddlSupplier.SelectedIndex = 0;
            ddlSupplier.Enabled = true;
            ddlSupplier.Focus();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER,PURCHASE_SCHEDULE_MASTER where PARTY_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_P_CODE=P_CODE AND P_INHOUSE_IND=1 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER,INWARD_MASTER IWM INNER JOIN INWARD_DETAIL IWD ON IWM.IWM_CODE = IWD.IWD_IWM_CODE where PARTY_MASTER.ES_DELETE=0 AND IWM.ES_DELETE=0 AND IWD.ES_DELETE=0 AND IWM.IWM_P_CODE=P_CODE AND P_INHOUSE_IND='1' and P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND='1' AND IWD.IWD_INSP_FLG='1' order by P_NAME");
        ddlSupplier.DataSource = dt;
        ddlSupplier.DataTextField = "P_NAME";
        ddlSupplier.DataValueField = "P_CODE";
        ddlSupplier.DataBind();
        ddlSupplier.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion
}
