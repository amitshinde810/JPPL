using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class IRN_VIEW_ViewProdEntryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            LoadItemCode();
            LoadItemName();
            ddlItemCode.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllItem.Checked = true;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }

    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
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
                From = Convert.ToDateTime(Session["OpeningDate"]).ToShortDateString();
                To = Convert.ToDateTime(Session["ClosingDate"]).ToShortDateString();
            }
            if (rbtType.SelectedIndex==0)
            {
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
                    str = "StoreWise";
                }   
            }
            if (rbtType.SelectedIndex==1)
            {
                  str1 = "Summary";
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
                      str = "StoreWise";
                  }   
            }
           
            string strCond = "";

            if (chkDateAll.Checked != true)
            {

                strCond = strCond + " IRN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            else
            {
                strCond = strCond + " IRN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            if (chkAllItem.Checked != true)
            {
                strCond = strCond + " IRND_I_CODE = '" + ddlItemCode.SelectedValue + "'  AND  ";
            }
            Response.Redirect("../../IRN/ADD/ProdEntryReport.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&strCond=" + strCond + " ", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production entry Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    public void LoadItemCode()
    {
        DataTable dtitemcode = new DataTable();
        dtitemcode = CommonClasses.Execute("select DISTINCT (I_CODE) as I_CODE,I_CODENO from ITEM_MASTER IM,IRN_DETAIL ID,IRN_ENTRY IE where IRND_I_CODE=I_CODE AND IE.IRN_CODE=ID.IRND_IRN_CODE AND IE.ES_DELETE=0");
        ddlItemCode.DataSource = dtitemcode;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        ddlItemCode.SelectedIndex = -1;

    }
    public void LoadItemName()
    {
        DataTable dtitemname = new DataTable();
        dtitemname = CommonClasses.Execute("select DISTINCT I_CODE,I_NAME from ITEM_MASTER IM,IRN_DETAIL ID,IRN_ENTRY IE where IRND_I_CODE=I_CODE AND IE.IRN_CODE=ID.IRND_IRN_CODE AND IE.ES_DELETE=0");
        ddlFinishedComponent.DataSource = dtitemname;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));
        ddlFinishedComponent.SelectedIndex = -1;

    }
}
