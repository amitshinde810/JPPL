using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_VIewExciseRegister : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='107'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                dateCheck();
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ExciseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan Register", "ShowMessage", Ex.Message);
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Menu"))
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
                if (ddlUser.SelectedValue == "0")
                {

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Type";
                    return;
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;

                if (ddlUser.SelectedItem.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Excise type.";
                    return;
                }
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
                    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                    From = From1.ToShortDateString();
                    To = To2.ToShortDateString();
                }
                string str1 = "";
                string str = "";
                string strcondition = "";
                if (chkDateAll.Checked != true)
                {
                    strcondition = strcondition + "  EX_DATE  BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND ";
                }
                else
                {
                    strcondition = strcondition + "  EX_DATE  BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "' AND ";
                }
                if (ddlUser.SelectedItem.Text != "")
                {
                    strcondition = strcondition + " EX_TYPE ='" + ddlUser.SelectedItem.Text + "' and ";
                }
                Response.Redirect("../../RoportForms/ADD/ExciseRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strcondition + "&Type=" + ddlUser.SelectedValue + "&RType=SHOW", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Print";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "btnShow_Click", Ex.Message);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Menu"))
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
                if (ddlUser.SelectedValue == "0")
                {

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Type";
                    return;
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                if (ddlUser.SelectedItem.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Excise type.";
                    return;
                }
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
                    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                    From = From1.ToShortDateString();
                    To = To2.ToShortDateString();
                }
                string str1 = "";
                string str = "";
                string strcondition = "";
                if (chkDateAll.Checked != true)
                {
                    strcondition = strcondition + "  EX_DATE  BETWEEN '" + Convert.ToDateTime(txtFromDate.Text) + "'  AND  '" + Convert.ToDateTime(txtToDate.Text) + "' AND ";
                }
                else
                {
                    strcondition = strcondition + "  EX_DATE  BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]) + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]) + "' AND ";
                }
                if (ddlUser.SelectedItem.Text != "")
                {
                    strcondition = strcondition + " EX_TYPE ='" + ddlUser.SelectedItem.Text + "' and ";
                }
                Response.Redirect("../../RoportForms/ADD/ExciseRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strcondition + "&Type=" + ddlUser.SelectedValue + "&RType=EXPORT", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Print";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "btnShow_Click", Ex.Message);
        }
    }
}
