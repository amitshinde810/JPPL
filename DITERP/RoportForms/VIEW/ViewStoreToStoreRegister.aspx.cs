using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewStoreToStoreRegister : System.Web.UI.Page
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
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadCombos();
            LoadStore();
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllItem.Checked = true;
            dateCheck();
            chkAllStore.Checked = true;
            ddlStore.Enabled = false;
        }
    }
    #endregion

    #region LoadStore
    private void LoadStore()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text == "" && txtToDate.Text == "")
                {
                    str = str + " IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text) + "' AND '" + Convert.ToDateTime(txtToDate.Text) + "' AND ";
                }
            }
            DataTable dtStore = CommonClasses.Execute("select distinct STORE_CODE,STORE_NAME from ISSUE_MASTER,STORE_MASTER where " + str + " IM_FROM_STORE=STORE_CODE and STORE_MASTER.ES_DELETE=0 AND ISSUE_MASTER.ES_DELETE=0 ");
            if (dtStore.Rows.Count > 0)
            {
                ddlStore.DataSource = dtStore;
                ddlStore.DataTextField = "STORE_NAME";
                ddlStore.DataValueField = "STORE_CODE";
                ddlStore.DataBind();
                ddlStore.Items.Insert(0, new ListItem("Select Store Name", "0"));
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion LoadStore

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dtItemDet = new DataTable();

            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + " IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text) + "'  AND '" + Convert.ToDateTime(txtToDate.Text) + "'  AND ";
                }
            }
            if (chkAllStore.Checked != true)
            {
                if (ddlStore.SelectedValue != "0")
                {
                    str = str + " IM_FROM_STORE = '" + ddlStore.SelectedValue + "'  AND  ";
                }
            }
            if (chkAllItem.Checked != true)
            {
                if (ddlItemCode.SelectedValue != "0")
                {
                    str = str + " I_CODE = '" + ddlItemCode.SelectedValue + "'  AND  ";
                }
            }
            if (rbtType.SelectedIndex == 0)
            {
                dtItemDet = CommonClasses.Execute("Select DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where " + str + " IM_TYPE=2 and IM_TRANS_TYPE=1 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");
            }
            if (rbtType.SelectedIndex == 1)
            {
                dtItemDet = CommonClasses.Execute("Select DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where " + str + " IM_TYPE=1 and IM_TRANS_TYPE=1 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");
            }

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    return;
                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
                    return;
                }
            }
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
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        return;
                    }
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }

            string str1 = "";
            string str = "";

            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Direct";
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
                    str = "Storewise";
                }
            }
            #endregion

            #region AsperReq
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "AsperReq";
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
                    str = "Storewise";
                }
            }
            #endregion

            string StrCond = "";
            //if (chkDateAll.Checked != true)
            //{
            //StrCond = StrCond + "  Convert(Date,IM_DATE) BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND ";
            StrCond = StrCond + "  Convert(Date,IM_DATE) BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND ";
            //}
            if (chkAllItem.Checked != true)
            {
                StrCond = StrCond + "  IMD_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
            }
            if (chkAllStore.Checked != true)
            {
                StrCond = StrCond + "  IM_FROM_STORE = '" + ddlStore.SelectedValue + "'  AND  ";
            }
            Response.Redirect("../../RoportForms/ADD/StoreToStoreRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&Cond=" + StrCond + "&RTYPE=" + rbPrintType.SelectedValue + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
            return false;
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
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
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

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadStore();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadStore();
    }

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
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
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
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

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion

    #region chkAlldept_CheckedChanged
    protected void chkAlldept_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllStore.Checked == true)
        {
            LoadCombos();
            LoadStore();
            ddlStore.SelectedIndex = 0;
            ddlStore.Enabled = false;
        }
        else
        {
            LoadStore();
            LoadCombos();
            ddlStore.SelectedIndex = 0;
            ddlStore.Enabled = true;
        }
    }
    #endregion
}
