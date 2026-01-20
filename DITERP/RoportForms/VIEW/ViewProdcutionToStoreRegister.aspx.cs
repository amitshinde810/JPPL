using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewProdcutionToStoreRegister : System.Web.UI.Page
{
    #region Variables
    static string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='52'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");

            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            ChkCodeNoAll.Checked = true;
            ddlSCCodeNo.Enabled = false;
            ddlProductionNo.Enabled = false;
            ddlItemCode.Enabled = false;
            ChkProductionNoAll.Checked = true;
            LoadComponent();
            LoadDept();
            ddlDepartment.Enabled = false;
            chkallDept.Checked = true;
            LoadProductionNo();
        }
    }
    #endregion

    #region LoadProductionNo
    private void LoadProductionNo()
    {
        try
        {
            string str = "";
            if (rbtType.SelectedIndex == 0)
            {
                str = "select PS_CODE,PS_GIN_NO FROM PRODUCTION_TO_STORE_MASTER WHERE  PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and PS_TYPE=2 and ES_DELETE=0 ";
            }
            else if (rbtType.SelectedIndex == 1)
            {
                str = "select PS_CODE,PS_GIN_NO FROM PRODUCTION_TO_STORE_MASTER WHERE  PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and PS_TYPE=1 and ES_DELETE=0 ";
            }
            else
            {
                str = "select PS_CODE,PS_GIN_NO FROM PRODUCTION_TO_STORE_MASTER WHERE  PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 ";
            }
            DataTable dt = CommonClasses.Execute(str);

            ddlProductionNo.DataSource = dt;
            ddlProductionNo.DataTextField = "PS_GIN_NO";
            ddlProductionNo.DataValueField = "PS_CODE";
            ddlProductionNo.DataBind();
            ddlProductionNo.Items.Insert(0, new ListItem("Production Number", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadProductionNo1
    private void LoadProductionNo1()
    {
        try
        {
            string str = "";

            str = "select PS_CODE,PS_GIN_NO FROM PRODUCTION_TO_STORE_MASTER WHERE  PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and PS_GIN_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ";

            DataTable dt = CommonClasses.Execute(str);

            ddlProductionNo.DataSource = dt;
            ddlProductionNo.DataTextField = "PS_GIN_NO";
            ddlProductionNo.DataValueField = "PS_CODE";
            ddlProductionNo.DataBind();
            ddlProductionNo.Items.Insert(0, new ListItem("Production Number", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region ChkProductionNoAll_CheckedChanged
    protected void ChkProductionNoAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkProductionNoAll.Checked == true)
        {
            ddlProductionNo.Enabled = false;
            ddlProductionNo.Enabled = false;
            ddlProductionNo.SelectedValue = "0";
            LoadComponent();
        }
        else
        {
            ddlProductionNo.Enabled = true;
        }
    }
    #endregion

    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            string str = "";
            if (rbtType.SelectedIndex == 0)
            {
                str = "select distinct(I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER,PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER WHERE PS_CODE=PSD_PS_CODE and PSD_I_CODE=I_CODE and PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and PS_TYPE=2 and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0  ORDER BY I_NAME";
            }
            else if (rbtType.SelectedIndex == 1)
            {
                str = "select distinct(I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER,PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER WHERE PS_CODE=PSD_PS_CODE and PSD_I_CODE=I_CODE and PS_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + " and PS_TYPE=1 and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0  ORDER BY I_NAME";
            }
            else
            {
                str = "select distinct(I_CODE),I_CODENO,I_NAME   FROM ITEM_MASTER,PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL WHERE PSD_I_CODE=I_CODE AND PSD_PS_CODE=PS_CODE  and ITEM_MASTER.ES_DELETE=0 AND PRODUCTION_TO_STORE_MASTER.ES_DELETE=0    ORDER BY I_NAME";
            }
            DataTable dt = CommonClasses.Execute(str);

            ddlSCCodeNo.DataSource = dt;
            ddlSCCodeNo.DataTextField = "I_NAME";
            ddlSCCodeNo.DataValueField = "I_CODE";
            ddlSCCodeNo.DataBind();
            ddlSCCodeNo.Items.Insert(0, new ListItem("Product Name", "0"));

            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Product Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Production To Store Register", "ShowMessage", Ex.Message);
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
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
            LoadProductionNo1();
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            LoadProductionNo1();
        }
    }
    #endregion

    #region ChkCodeNoAll_CheckedChanged
    protected void ChkCodeNoAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkCodeNoAll.Checked == true)
        {
            ddlSCCodeNo.Enabled = false;
            ddlItemCode.Enabled = false;
        }
        else
        {
            ddlSCCodeNo.Enabled = true;
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

            if (ChkProductionNoAll.Checked == false)
            {
                if (ddlProductionNo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Production Number ";
                    ddlProductionNo.Focus();
                    return;
                }
            }
            if (ChkCodeNoAll.Checked == false)
            {
                if (ddlSCCodeNo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Product code Or Name";
                    ddlSCCodeNo.Focus();
                    return;
                }
            }
            if (chkallDept.Checked == false)
            {
                if (ddlDepartment.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Department ";
                    ddlDepartment.Focus();
                    return;
                }
            }
            string From = "";
            string To = "";
            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }
            string str = "";
            string str1 = rbtType.SelectedIndex.ToString();
            if (rbtGroup.SelectedIndex == 1)
            {
                str = "Datewise";
            }
            if (rbtGroup.SelectedIndex == 0)
            {
                str = "Itemwise";
            }
            string strCondition = "";
            if (chkallDept.Checked != true)
            {
                strCondition = strCondition + "  PS_PERSON_NAME='" + ddlDepartment.SelectedValue + "' AND ";
            }
            if (ChkCodeNoAll.Checked != true)
            {
                strCondition = strCondition + "  PSD_I_CODE='" + ddlItemCode.SelectedValue + "' AND ";
            }
            if (chkDateAll.Checked != true)
            {
                strCondition = strCondition + " PS_GIN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND ";
            }
            if (ChkProductionNoAll.Checked != true)
            {
                strCondition = strCondition + "  PS_CODE='" + ddlProductionNo.SelectedValue + "'  AND ";
            }
            Response.Redirect("../../RoportForms/ADD/ProductionToStoreRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&type=" + str1 + "&Cond=" + strCondition + "&rType=" + rbType.SelectedValue + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductionNo();
        LoadComponent();
    }
    #endregion

    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSCCodeNo.SelectedValue = ddlItemCode.SelectedValue;
    }
    protected void ddlSCCodeNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlSCCodeNo.SelectedValue;
    }

    #region chkallDept_CheckedChanged
    protected void chkallDept_CheckedChanged(object sender, EventArgs e)
    {
        if (chkallDept.Checked == true)
        {
            ddlDepartment.Enabled = false;
            ddlDepartment.SelectedIndex = 0;
        }
        else
        {
            ddlDepartment.Enabled = true;
            ddlDepartment.SelectedIndex = 0;
        }
    }
    #endregion

    #region LoadDept
    private void LoadDept()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" SELECT * FROM DEPARTMENT ");
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataTextField = "Dept_Name";
        ddlDepartment.DataValueField = "Dept_Name";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("Select Department Name", "0"));
    }
    #endregion

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadProductionNo1();
    }
    protected void ddlProductionNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProductionNo.SelectedValue != "0")
        {
            DataTable dt = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME,PS_PERSON_NAME  FROM ITEM_MASTER,PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL WHERE PSD_I_CODE=I_CODE AND PSD_PS_CODE=PS_CODE  and ITEM_MASTER.ES_DELETE=0 AND PRODUCTION_TO_STORE_MASTER.ES_DELETE=0  AND PS_CODE='" + ddlProductionNo.SelectedValue + "'   ORDER BY I_NAME");

            ddlSCCodeNo.DataSource = dt;
            ddlSCCodeNo.DataTextField = "I_NAME";
            ddlSCCodeNo.DataValueField = "I_CODE";
            ddlSCCodeNo.DataBind();
            ddlSCCodeNo.Items.Insert(0, new ListItem("Product Name", "0"));

            dt.DefaultView.Sort = "I_CODENO";
            dt.DefaultView.ToTable();
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Product Code", "0"));
            ddlDepartment.SelectedValue = dt.Rows[0]["PS_PERSON_NAME"].ToString();
        }
    }
}
