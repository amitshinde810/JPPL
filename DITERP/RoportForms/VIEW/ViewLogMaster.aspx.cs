using System;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewLogMaster : System.Web.UI.Page
{
    static string right = "";
    DataTable dt = new DataTable();

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");


            LoadCombos();
            ddlModule.Enabled = false;
            ddlUser.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllModule.Checked = true;
            chkAllUser.Checked = true;
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
                    lblmsg.Text = "The Field Date is required";
                    return;
                }
            }
            if (chkAllUser.Checked == false)
            {
                if (ddlUser.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select User Name";
                    return;
                }
            }
            if (chkAllModule.Checked == false)
            {
                if (ddlModule.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Module";
                    return;
                }
            }
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
                    DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                    if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }

            if (rbtType.SelectedIndex == 0)
            {
                str1 = "DateWise";

                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }

                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
            }
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "ModuleWise";

                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }

                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
            }
            if (rbtType.SelectedIndex == 2)
            {
                str1 = "UserWise";

                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
                if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                {
                    Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);
                }
            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        dt = CommonClasses.Execute("select distinct(UPPER(ltrim(rtrim(LG_SOURCE)))) as LG_SOURCE  from LOG_MASTER where LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY LG_SOURCE");
        ddlModule.DataSource = dt;
        ddlModule.DataTextField = "LG_SOURCE";
        ddlModule.DataBind();
        ddlModule.Items.Insert(0, new ListItem("Select Screen ", "0"));

        dt = CommonClasses.Execute("select UM_CODE,UM_NAME  from USER_MASTER where ES_DELETE=0 and UM_CM_ID=" + (string)Session["CompanyId"] + "");
        ddlUser.DataSource = dt;
        ddlUser.DataTextField = "UM_NAME";
        ddlUser.DataValueField = "UM_CODE";
        ddlUser.DataBind();
        ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
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
    }
    #endregion

    #region chkAllModule_CheckedChanged
    protected void chkAllModule_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllModule.Checked == true)
        {
            ddlModule.SelectedIndex = 0;
            ddlModule.Enabled = false;
        }
        else
        {
            ddlModule.SelectedIndex = 0;
            ddlModule.Enabled = true;
            ddlModule.Focus();
        }
    }
    #endregion

    #region chkAllUser_CheckedChanged
    protected void chkAllUser_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllUser.Checked == true)
        {
            ddlUser.SelectedIndex = 0;
            ddlUser.Enabled = false;
        }
        else
        {
            ddlUser.SelectedIndex = 0;
            ddlUser.Enabled = true;
            ddlUser.Focus();
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Log Master", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            flag = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion
}
