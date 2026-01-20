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

public partial class RoportForms_VIEW_ViewStockLedgerRegister : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            ddlComponent.Enabled = false;
            chkAllComp.Checked = true;
            LoadComponent();

        }

    }
    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY I_NAME");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_NAME";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Component", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "LoadComponent", Ex.Message);
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = false;
        }
        else
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = true;
            ddlComponent.Focus();
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

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str1 = "";
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Detail";
              
            }


            if (rbtType.SelectedIndex == 1)
            {
                str1 = "Summary";
              
            }


            Response.Redirect("~/RoportForms/ADD/StockLedgerRegister.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&Comp_All=" + chkAllComp.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + ddlComponent.SelectedValue.ToString() + "&detail=" + str1 + "", false);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

}
