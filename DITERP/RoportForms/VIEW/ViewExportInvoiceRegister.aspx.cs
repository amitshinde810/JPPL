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

public partial class RoportForms_VIEW_ViewExportInvoiceRegister : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='35'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlCustomerName.Enabled = false;
            //ddlUser.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            //chkAllUser.Checked = true;
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));


            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");

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
            Response.Redirect("~/Masters/Add/SalesDefault.aspx", false);
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
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";

                        return;
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
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;

                string str1 = "";
                string str = "";

                #region Detail
                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";

                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To +   "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";

                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To +  "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";

                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To +   "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";

                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To +   "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";

                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                        {
                            Response.Redirect("~/RoportForms/ADD/ExportInvoiceRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                        }
                        //if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                        //{
                        //    Response.Redirect("~/RoportForms/ADD/CustomerOrderRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To +   "&p_name=" + ddlCustomerName.SelectedItem.ToString() + "&i_name=" + ddlFinishedComponent.SelectedItem.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                        //}
                    }
                }
                #endregion






                //if (chkAll.Checked == true && chkDateAll.Checked == true)
                //{
                //}
                //if (chkAll.Checked == false && chkDateAll.Checked == true)
                //{
                //    if (ddlCustomerName.SelectedIndex == 0)
                //    {
                //        ShowMessage("#Avisos", "Select Customer Name", CommonClasses.MSG_Warning);
                //        return;
                //    }
                //    else
                //    {
                //        Response.Redirect("~/RoportForms/ADD/CustomerEnquiryRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&Date_All=" + chkDateAll.Checked.ToString() + "&From=" + From + "&To=" + To + "&ce_p_name=" + ddlCustomerName.SelectedItem.ToString() + "", false);
                //    }
                //}
                //if (chkAll.Checked == true && chkDateAll.Checked == false)
                //{
                //    if (From == "" && To == "")
                //    {
                //        ShowMessage("#Avisos", "Select From Date Or To Date", CommonClasses.MSG_Warning);
                //        return;
                //    }
                //    else
                //    {
                //        Response.Redirect("~/RoportForms/ADD/CustomerEnquiryRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&Date_All=" + chkDateAll.Checked.ToString() + "&From=" + From + "&To=" + To + "&ce_p_name=" + ddlCustomerName.SelectedItem.ToString() + "", false);
                //    }
                //}
                //if (chkAll.Checked == false && chkDateAll.Checked == false)
                //{
                //    if (From == "" && To == "" && ddlCustomerName.SelectedIndex == 0)
                //    {
                //        ShowMessage("#Avisos", "Select At Least on Criteria", CommonClasses.MSG_Warning);
                //        return;
                //    }
                //    else
                //    {
                //        Response.Redirect("~/RoportForms/ADD/CustomerEnquiryRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&Date_All=" + chkDateAll.Checked.ToString() + "&From=" + From + "&To=" + To + "&ce_p_name=" + ddlCustomerName.SelectedItem.ToString() + "", false);

            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
