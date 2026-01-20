using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewTurningReg : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='114'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadVendors();
            LoadCombos();
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            dateCheck();
            chkDateAll.Checked = true;
            chkAllItem.Checked = true;
            chkAllVendor.Checked = true;
            ddlVendorName.Enabled = false;
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("  SELECT DISTINCT I_CODE,I_CODENO FROM SCRAP_MASTER,SCRAP_DETAIL,ITEM_MASTER   where SM_CODE=SD_SM_CODE AND SCRAP_MASTER.ES_DELETE=0   AND  SD_I_CODE=I_CODE   AND convert(date, SM_CH_DATE)    BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND SM_CM_CODE=" + Session["CompanyCode"].ToString() + "  ORDER BY I_CODENO ");


            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Code No", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
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
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                        else if (Date1 > Date2)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                }
                else
                {
                    From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ddlFinishedComponent.Focus();
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

                string strCond = "";
                //if (chkDateAll.Checked != true)
                //{
                strCond = strCond + " AND convert(date, SM_CH_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ";
                //}
                if (chkAllItem.Checked != true)
                {
                    strCond = strCond + " AND I_CODE='" + ddlFinishedComponent.SelectedValue + "'   ";
                }
                if (chkAllVendor.Checked != true)
                {
                    strCond = strCond + " AND SM_P_CODE= '" + ddlVendorName.SelectedValue + "'";
                    if (ddlVendorName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please select Sub Contractor";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlVendorName.Focus();
                        return;
                    }
                }
                Response.Redirect("../../RoportForms/ADD/TurningReg.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&strCond=" + strCond + "", false);
            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Turning Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion btnShow_Click

    #region chkAllVendor_CheckedChanged
    protected void chkAllVendor_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllVendor.Checked == true)
        {
            ddlVendorName.SelectedIndex = 0;
            ddlVendorName.Enabled = false;
            LoadCombos();
        }
        else
        {
            ddlVendorName.SelectedIndex = 0;
            ddlVendorName.Enabled = true;
            ddlVendorName.Focus();
        }
    }
    #endregion

    #region ddlVendorName_SelectedIndexChanged
    protected void ddlVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("  SELECT DISTINCT I_CODE,I_CODENO FROM SCRAP_MASTER,SCRAP_DETAIL,ITEM_MASTER   where SM_CODE=SD_SM_CODE AND SCRAP_MASTER.ES_DELETE=0   AND  SD_I_CODE=I_CODE   AND convert(date, SM_CH_DATE)    BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND SM_CM_CODE=" + Session["CompanyCode"].ToString() + "  AND SM_P_CODE='" + ddlVendorName.SelectedValue + "' ORDER BY I_CODENO ");
            ddlFinishedComponent.DataSource = custdet;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadItem", Ex.Message);
        }
    }
    #endregion

    #region LoadVendors
    private void LoadVendors()
    {
        try
        {
            DataTable dtVendors = new DataTable();
            dtVendors = CommonClasses.Execute("  SELECT DISTINCT P_CODE,P_NAME FROM SCRAP_MASTER,SCRAP_DETAIL,PARTY_MASTER   where SM_CODE=SD_SM_CODE AND SCRAP_MASTER.ES_DELETE=0   AND SM_P_CODE=P_CODE  AND convert(date, SM_CH_DATE)    BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " AND SM_CM_CODE=" + Session["CompanyCode"].ToString() + "  ORDER BY P_NAME ");
            ddlVendorName.DataSource = dtVendors;
            ddlVendorName.DataTextField = "P_NAME";
            ddlVendorName.DataValueField = "P_CODE";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new ListItem("Select Sub Contractor", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadCustomer", Ex.Message);
        }
    }
    #endregion LoadVendors

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadVendors();
    }
    #endregion txtFromDate_TextChanged

}
