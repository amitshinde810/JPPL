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

public partial class RoportForms_VIEW_ViewStockLedgerTypewise : System.Web.UI.Page
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
            ddlitemtype.Enabled = false;
            Chkitem.Checked = true;
            LoadComponent();
            loadItemtype();

        }

    }

    private void loadItemtype()
    {
        DataTable dtitem = CommonClasses.Execute("select I_CAT_CODE,I_CAT_NAME from ITEM_CATEGORY_MASTER where I_CAT_CM_COMP_ID ='" + Session["CompanyId"] + "'  and ES_DELETE=0 order by I_CAT_NAME asc");
        if (dtitem.Rows.Count > 0)
        {
            ddlitemtype.DataSource = dtitem;
            ddlitemtype.DataTextField = "I_CAT_NAME";
            ddlitemtype.DataValueField = "I_CAT_CODE";
            ddlitemtype.DataBind();
            ddlitemtype.Items.Insert(0, new ListItem("select Type", "0"));

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


            Response.Redirect("~/RoportForms/ADD/StockLedgerTypeRegi.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&Comp_All=" + chkAllComp.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + ddlComponent.SelectedValue + "&detail=" + str1 + "&chkItem="+Chkitem.Checked.ToString() +"&Cat_Code="+ddlitemtype.SelectedValue +"", false);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlitemtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CAT_CODE="+ddlitemtype.SelectedValue +"  ORDER BY I_NAME");

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

    protected void Chkitem_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkitem.Checked == true)
        {
            ddlitemtype.SelectedIndex = 0;
            ddlitemtype.Enabled = false;
        }
        else
        {
            ddlitemtype.SelectedIndex = 0;
            ddlitemtype.Enabled = true;
            ddlitemtype.Focus();
        }
    }
}
