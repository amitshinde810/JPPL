using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewSaleSummaryCustomerwise : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='25'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Text = DateTime.Today.ToString("MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            LoadCombos();
            ddlCustomerName.Enabled = false;
            txtFromDate.Enabled = true;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "")
                {
                    str = str + "INM_DATE between '" + txtFromDate.Text + "'";
                }
            }


            DataTable dtCustBind = new DataTable();
            //custdet = CommonClasses.Execute("SELECT distinct PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM ITEM_MASTER INNER JOIN INVOICE_DETAIL ON ITEM_MASTER.I_CODE = INVOICE_DETAIL.IND_I_CODE INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE where " + str + " ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " order by P_NAME");
            dtCustBind = CommonClasses.Execute("SELECT distinct P.P_CODE,P.P_NAME FROM PARTY_MASTER P INNER JOIN CUSTOMER_SCHEDULE C ON P.P_CODE=C.CS_P_CODE  WHERE P.ES_DELETE=0 AND C.ES_DELETE=0 AND CS_COMP_ID='" + Session["CompanyId"].ToString() + "' AND CONVERT(DATE,CS_DATE)='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'");
            ddlCustomerName.DataSource = dtCustBind;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Summary Customerwise Report", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
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
            CommonClasses.SendError("Sales Summary Customerwise Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadCombos();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;

            LoadCombos();
        }
        else
        {

            LoadCombos();
            txtFromDate.Enabled = true;

            txtFromDate.Attributes.Add("readonly", "readonly");

        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {

                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlCustomerName.Focus();
                        return;
                    }
                }
                DateTime From = Convert.ToDateTime(txtFromDate.Text);
                string To = "";
                string str1 = "";
                string str = "";

                string strCondition = "";
                strCondition = " CONVERT(date, CS_DATE)  ='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  ";

                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + " P_CODE= '" + ddlCustomerName.SelectedValue + "' AND ";
                }
                string title = Title;
                title = Title + " " + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "";
                Response.Redirect("../../ReportForms/ADD/SaleSummaryCustomerwise.aspx?Title=" + title + "&Cond=" + strCondition + "&Type=Cust", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            LoadCombos();
        }
    }
    #endregion txtFromDate_TextChanged
}
