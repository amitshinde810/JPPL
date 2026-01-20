using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewStockError : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='155'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            dateCheck();
            chkDateAll.Checked = true;
            chkAllItem.Checked = true;
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");
            LoadStore();
            chkAllStore.Checked = true;
            ddlstore.Enabled = false;
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {

            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("select distinct I_CODE,I_CODENO+' - '+I_NAME AS I_CODENO  from ITEM_MASTER,STOCK_ADJUSTMENT_DETAIL where I_CODE=SAD_I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_CODENO");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Code No", "0"));

            DataTable dtUserDet = new DataTable();
            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadStore
    private void LoadStore()
    {
        try
        {
            DataTable dtstore = new DataTable();
            dtstore = CommonClasses.Execute(" SELECT DISTINCT  STORE_CODE,STORE_NAME FROM STOCK_ADJUSTMENT_MASTER,STOCK_ADJUSTMENT_DETAIL,STORE_MASTER where SAM_CODE=SAD_SAM_CODE  AND SAM_TYPE=1 AND   STOCK_ADJUSTMENT_MASTER.ES_DELETE=0 AND SAD_TO_STORE=STORE_CODE  AND STOCK_ADJUSTMENT_MASTER.SAM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  ");

            ddlstore.DataSource = dtstore;
            ddlstore.DataTextField = "STORE_NAME";
            ddlstore.DataValueField = "STORE_CODE";
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, new ListItem("Select Store Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
        }
        dateCheck();
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;
        To = txtToDate.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtFromDate.Text = Date2.ToString("dd/MMM/yyyy");
                    return;
                }
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

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                string From = "";
                string To = "";
                From = txtFromDate.Text.Trim();
                To = txtToDate.Text.Trim();
                if (chkDateAll.Checked == false)
                {
                    if (From != "" && To != "")
                    {
                        DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                        DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                        if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else if (Date1 > Date2)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                else
                {
                    From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                }
                if (chkAllStore.Checked == false)
                {
                    if (ddlstore.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Store Name ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlstore.Focus();
                        return;
                    }
                }
                string str1 = "";
                string str = "";

                str1 = "Detail";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";
                }
                if (rbtGroup.SelectedIndex == 2)
                {
                    str = "CustWise";
                }
                string strCond = "";
                if (chkDateAll.Checked != true)
                {
                    strCond = strCond + " SAM_DATE between '" + Convert.ToDateTime(txtFromDate.Text) + "' AND  '" + Convert.ToDateTime(txtToDate.Text) + "'  AND  ";
                }
                if (chkAllItem.Checked != true)
                {
                    strCond = strCond + "  SAD_I_CODE='" + ddlFinishedComponent.SelectedValue + "'  AND   ";
                }
                if (chkAllStore.Checked != true)
                {
                    strCond = strCond + "  STORE_CODE='" + ddlstore.SelectedValue + "'  AND   ";
                }
                Response.Redirect("../../RoportForms/ADD/SrockError.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&strCond=" + strCond + "", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region chkAllStore_CheckedChanged
    protected void chkAllStore_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllStore.Checked == true)
        {
            ddlstore.SelectedIndex = 0;
            ddlstore.Enabled = false;
        }
        else
        {
            ddlstore.SelectedIndex = 0;
            ddlstore.Enabled = true;
            ddlstore.Focus();
        }
    }
    #endregion

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        dateCheck();
        LoadStore();
    }
    #endregion
}
