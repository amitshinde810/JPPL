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

public partial class RoportForms_VIEW_ViewCustomerOrderRegister : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='23'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {

                LoadCombos();
                ddlCustomerName.Enabled = false;
                //ddlUser.Enabled = false;
                ddlFinishedComponent.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                chkAllCustomer.Checked = true;
                chkAllItem.Checked = true;
                chkAllOWType.Checked = true;
                ddlOutwardType.Enabled = false;
                dateCheck();

                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                //chkAllUser.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");

            }
        }

    }

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    private void LoadCust()
    {
        string str = "";
        if (chkDateAll.Checked != true)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
            }
        }
        DataTable custdet = new DataTable();
        custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE " + str + " CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
        ddlCustomerName.DataSource = custdet;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));
    }
    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CPOM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (ddlCustomerName.SelectedIndex > 0)
            {
                str = str + "CPOM_P_CODE= " + ddlCustomerName.SelectedValue + " and ";
            }
            DataTable dtItemDet = new DataTable();
            // dtItemDet = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE='-2147483647' ORDER BY I_NAME");
            dtItemDet = CommonClasses.Execute("select distinct(I_CODE) as I_CODE,I_CAT_CODE,I_NAME FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE " + str + " I_CODE=CPOD_I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.I_CAT_CODE='-2147483648' ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");

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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadCust();
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
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
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
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region chkAllOWType_CheckedChanged
    protected void chkAllOWType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllOWType.Checked == true)
        {
            ddlOutwardType.SelectedIndex = 0;
            ddlOutwardType.Enabled = false;
        }
        else
        {
            ddlOutwardType.SelectedIndex = 0;
            ddlOutwardType.Enabled = true;
            ddlOutwardType.Focus();
        }
    }
    #endregion

    protected void dateCheck()
    {
        //if(Convert.ToDateTime(txtFromDate.Text))
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

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCust();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCust();
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime From;
            DateTime To;
            string Condition = "";
            From = Convert.ToDateTime(txtFromDate.Text);
            To = Convert.ToDateTime(txtToDate.Text);
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    From = Convert.ToDateTime(txtFromDate.Text);
                    To = Convert.ToDateTime(txtToDate.Text);
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]);
                To = Convert.ToDateTime(Session["ClosingDate"]);
            }

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        return;
                    }
                }
                else
                {
                    Condition = Condition + "CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ";
                }
                if (chkAllOWType.Checked == false)
                {
                    if (ddlOutwardType.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Outward Type ";
                        ddlCustomerName.Focus();
                        return;
                    }
                    else
                    {
                        if (ddlOutwardType.SelectedValue == "1")
                        {
                            Condition = Condition + "INVOICE_MASTER.INM_TYPE='" + "outcustinm" + "' and ";
                        }
                        if (ddlOutwardType.SelectedValue == "2")
                        {
                            Condition = Condition + "";
                        }
                        if (ddlOutwardType.SelectedValue == "3")
                        {
                            Condition = Condition + "";
                        }
                        // Condition = Condition + " INVOICE_MASTER.INM_TYPE='" + ddlOutwardType.SelectedValue + "' and ";
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ddlCustomerName.Focus();
                        return;
                    }
                    else
                    {
                        Condition = Condition + " PARTY_MASTER.P_NAME='" + ddlCustomerName.SelectedItem.Text + "' and ";
                    }
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ddlFinishedComponent.Focus();
                        return;
                    }
                    else
                    {
                        Condition = Condition + "ITEM_MASTER.I_CODE='" + ddlFinishedComponent.SelectedValue + "' and ";
                    }
                }

                string str1 = "";
                string str = "";
                string str3 = "";
                string str4 = "";


                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                }
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";
                }

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

                if (rbtlstWithvalue.SelectedIndex == 0)
                {
                    str3 = "WithValue";
                }
                if (rbtlstWithvalue.SelectedIndex == 1)
                {
                    str3 = "WithoutValue";
                }
                Response.Redirect("../ADD/CustomerOrderRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Condition=" + Condition + "&WithValue=" + str3 + "", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Print";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

}
