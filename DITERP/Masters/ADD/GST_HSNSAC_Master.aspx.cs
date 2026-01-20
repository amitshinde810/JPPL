using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_ADD_GST_HSNSAC_Master : System.Web.UI.Page
{
    #region General Declaration
    ExciseTariffDetails_BL BL_ExciseTariffDetails = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='15'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadTallyAcc();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_ExciseTariffDetails = new ExciseTariffDetails_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_ExciseTariffDetails = new ExciseTariffDetails_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtTariffNo.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Excise Tariff Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = " ";
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region txtBasicExciseDuty_TextChangesd
    protected void txtBasicExciseDuty_TextChangesd(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExciseDuty.Text);
        txtBasicExciseDuty.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtBasicExciseDuty.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtBasicExciseDuty.Text = "";
        }
    }
    #endregion

    #region txtEducationalCess_TextChangesd
    protected void txtEducationalCess_TextChangesd(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtEducationalCess.Text);
        txtEducationalCess.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtEducationalCess.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtEducationalCess.Text = "";
        }
    }
    #endregion
    protected void ddlTallyBasic_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlTallyBasic.SelectedIndex > 0)
        //{
        //    if (ddlTallyBasic.SelectedValue == ddlTallyEdu.SelectedValue || ddlTallyBasic.SelectedValue == ddlTallySHEdu.SelectedValue || ddlTallyBasic.SelectedValue == ddlTallySpecial.SelectedValue)
        //    {
        //        PanelMsg.Visible = true;
        //        lblmsg.Text = "Tally Name should not be same";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //        ddlTallyBasic.SelectedIndex = 0;
        //        ddlTallyBasic.Focus();
        //    }
        //}
    }

    protected void ddlTallySpecial_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlTallySpecial.SelectedIndex > 0)
        //{
        //    if (ddlTallySpecial.SelectedValue == ddlTallyEdu.SelectedValue || ddlTallySpecial.SelectedValue == ddlTallySHEdu.SelectedValue || ddlTallyBasic.SelectedValue == ddlTallySpecial.SelectedValue)
        //    {
        //        PanelMsg.Visible = true;
        //        lblmsg.Text = "Tally Name should not be same";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //        ddlTallySpecial.SelectedIndex = 0;
        //        ddlTallySpecial.Focus();
        //    }
        //}
    }

    protected void ddlTallyEdu_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlTallyEdu.SelectedIndex > 0)
        //{
        //    if (ddlTallyBasic.SelectedValue == ddlTallyEdu.SelectedValue || ddlTallyEdu.SelectedValue == ddlTallySHEdu.SelectedValue || ddlTallyEdu.SelectedValue == ddlTallySpecial.SelectedValue)
        //    {
        //        PanelMsg.Visible = true;
        //        lblmsg.Text = "Tally Name should not be same";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //        ddlTallyEdu.SelectedIndex = 0;
        //        ddlTallyEdu.Focus();
        //    }
        //}
    }

    protected void ddlTallySHEdu_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlTallySHEdu.SelectedIndex > 0)
        //{
        //    if (ddlTallySHEdu.SelectedValue == ddlTallyEdu.SelectedValue || ddlTallyBasic.SelectedValue == ddlTallySHEdu.SelectedValue || ddlTallySHEdu.SelectedValue == ddlTallySpecial.SelectedValue)
        //    {
        //        PanelMsg.Visible = true;
        //        lblmsg.Text = "Tally Name should not be same";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //        ddlTallySHEdu.SelectedIndex = 0;
        //        ddlTallySHEdu.Focus();
        //    }
        //}
    }

    #region txtSHEdu_TextChangesd
    protected void txtSHEdu_TextChangesd(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSHEdu.Text);
        txtSHEdu.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtSHEdu.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSHEdu.Text = "";
        }
    }
    #endregion

    #region txtSpecialExciseDuty_TextChangesd
    protected void txtSpecialExciseDuty_TextChangesd(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSpecialExciseDuty.Text);
        txtSpecialExciseDuty.Text = string.Format("{0:0.00}", Convert.ToDouble(totalStr));
        double a = Convert.ToDouble(txtSpecialExciseDuty.Text);
        if (a > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Percentage Can Not Be more than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSpecialExciseDuty.Text = "";
        }
    }
    #endregion

    #endregion

    #region Methods
    #region LoadTallyAcc
    private void LoadTallyAcc()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where ES_DELETE=0 and TALLY_COMP_ID=" + Session["CompanyId"] + " order by TALLY_NAME");

                ddlTallyBasic.DataSource = dt;
                ddlTallyBasic.DataTextField = "TALLY_NAME";
                ddlTallyBasic.DataValueField = "TALLY_CODE";
                ddlTallyBasic.DataBind();
                ddlTallyBasic.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallySpecial.DataSource = dt;
                ddlTallySpecial.DataTextField = "TALLY_NAME";
                ddlTallySpecial.DataValueField = "TALLY_CODE";
                ddlTallySpecial.DataBind();
                ddlTallySpecial.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyEdu.DataSource = dt;
                ddlTallyEdu.DataTextField = "TALLY_NAME";
                ddlTallyEdu.DataValueField = "TALLY_CODE";
                ddlTallyEdu.DataBind();
                ddlTallyEdu.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallySHEdu.DataSource = dt;
                ddlTallySHEdu.DataTextField = "TALLY_NAME";
                ddlTallySHEdu.DataValueField = "TALLY_CODE";
                ddlTallySHEdu.DataBind();
                ddlTallySHEdu.Items.Insert(0, new ListItem("Select Tally Name", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Excise Tariff Master", "LoadTallyAcc", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "LoadCountry", ex.Message);
        }
    }
    #endregion Load Country

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_ExciseTariffDetails = new ExciseTariffDetails_BL(mlCode);
            DataTable dt = new DataTable();
            BL_ExciseTariffDetails.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("EXCISE_TARIFF_MASTER", "MODIFY", "E_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXCISE_TARIFF_MASTER", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute(" SELECT * FROM EXCISE_TARIFF_MASTER where E_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                txtTariffNo.Text = dt.Rows[0]["E_TARIFF_NO"].ToString();
                txtCommodity.Text = dt.Rows[0]["E_COMMODITY"].ToString();

                txtBasicExciseDuty.Text = dt.Rows[0]["E_BASIC"].ToString();
                txtBasicExciseDuty.Text = string.Format("{0:0.00}", Convert.ToDouble(txtBasicExciseDuty.Text));
                ddlTallyBasic.SelectedValue = dt.Rows[0]["E_TALLY_BASIC"].ToString();

                txtSpecialExciseDuty.Text = dt.Rows[0]["E_SPECIAL"].ToString();
                txtSpecialExciseDuty.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSpecialExciseDuty.Text));
                ddlTallySpecial.SelectedValue = dt.Rows[0]["E_TALLY_SPECIAL"].ToString();

                txtEducationalCess.Text = dt.Rows[0]["E_EDU_CESS"].ToString();
                txtEducationalCess.Text = string.Format("{0:0.00}", Convert.ToDouble(txtEducationalCess.Text));
                ddlTallyEdu.SelectedValue = dt.Rows[0]["E_TALLY_EDU"].ToString();

                txtSHEdu.Text = dt.Rows[0]["E_H_EDU"].ToString();
                txtSHEdu.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSHEdu.Text));
                ddlTallySHEdu.SelectedValue = dt.Rows[0]["E_TALLY_H_EDU"].ToString();
                rbtType.SelectedValue = dt.Rows[0]["E_EX_TYPE"].ToString();
            }
            if (str == "VIEW")
            {
                txtTariffNo.Enabled = false;
                txtCommodity.Enabled = false;
                txtBasicExciseDuty.Enabled = false;
                txtSpecialExciseDuty.Enabled = false;
                txtEducationalCess.Enabled = false;
                txtSHEdu.Enabled = false;
                ddlTallyBasic.Enabled = false;
                ddlTallyEdu.Enabled = false;
                ddlTallySHEdu.Enabled = false;
                ddlTallySpecial.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            if (txtTariffNo.Text.ToUpper().Trim() != "")
            {
                if (txtCommodity.Text.ToUpper().Trim() != "")
                {
                    txtTariffNo.Text = txtTariffNo.Text.Trim();
                    txtCommodity.Text = txtCommodity.Text.Trim();
                    BL_ExciseTariffDetails.E_TARIFF_NO = txtTariffNo.Text.Trim();
                    BL_ExciseTariffDetails.E_COMMODITY = txtCommodity.Text.Trim();
                    BL_ExciseTariffDetails.E_BASIC = Convert.ToDouble(txtBasicExciseDuty.Text == "" ? "0.00" : txtBasicExciseDuty.Text);
                    BL_ExciseTariffDetails.E_SPECIAL = Convert.ToDouble(txtSpecialExciseDuty.Text == "" ? "0.00" : txtSpecialExciseDuty.Text);
                    BL_ExciseTariffDetails.E_EDU_CESS = Convert.ToDouble(txtEducationalCess.Text == "" ? "0.00" : txtEducationalCess.Text);
                    BL_ExciseTariffDetails.E_H_EDU = Convert.ToDouble(txtSHEdu.Text == "" ? "0.00" : txtSHEdu.Text);
                    BL_ExciseTariffDetails.E_TALLY_BASIC = Convert.ToInt32(ddlTallyBasic.SelectedValue);
                    BL_ExciseTariffDetails.E_TALLY_SPECIAL = Convert.ToInt32(ddlTallySpecial.SelectedValue);
                    BL_ExciseTariffDetails.E_TALLY_EDU = Convert.ToInt32(ddlTallyEdu.SelectedValue);
                    BL_ExciseTariffDetails.E_TALLY_H_EDU = Convert.ToInt32(ddlTallySHEdu.SelectedValue);
                    BL_ExciseTariffDetails.E_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
                    BL_ExciseTariffDetails.E_TALLY_GST_EXCISE = true;
                    res = true;
                }
                else
                {
                    txtCommodity.Text = txtCommodity.Text.ToUpper().Trim();
                    ShowMessage("#Avisos", "Please Enter Commodity", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtCommodity.Focus();
                }
            }
            else
            {
                txtTariffNo.Text = txtTariffNo.Text.ToUpper().Trim();
                ShowMessage("#Avisos", "Please Enter TariffNo", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtTariffNo.Focus();
            }
            BL_ExciseTariffDetails.E_TALLY_GST_EXCISE = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceCommodity = txtCommodity.Text;
            StrReplaceCommodity = StrReplaceCommodity.Replace("'", "''");
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_ExciseTariffDetails = new ExciseTariffDetails_BL();
                if (Setvalues())
                {
                    DataTable dtExist = CommonClasses.Execute(" SELECT * FROM EXCISE_TARIFF_MASTER where( E_TARIFF_NO='" + txtTariffNo.Text.Trim() + "' OR E_COMMODITY='" + txtCommodity.Text.Trim() + "') AND ES_DELETE=0 AND E_TALLY_GST_EXCISE=1 AND  E_EX_TYPE='" + rbtType.SelectedValue + "'"); ;
                    if (dtExist.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtTariffNo.Focus();
                        return true;
                    }
                    if (CommonClasses.Execute1("INSERT INTO EXCISE_TARIFF_MASTER ( E_CM_COMP_ID, E_TARIFF_NO, E_COMMODITY, E_BASIC, E_SPECIAL, E_EDU_CESS, E_H_EDU,  E_TALLY_BASIC, E_TALLY_SPECIAL, E_TALLY_EDU, E_TALLY_H_EDU, E_TALLY_GST_EXCISE,E_EX_TYPE)VALUES ('" + BL_ExciseTariffDetails.E_CM_COMP_ID + "', '" + BL_ExciseTariffDetails.E_TARIFF_NO + "','" + BL_ExciseTariffDetails.E_COMMODITY + "','" + BL_ExciseTariffDetails.E_BASIC + "','" + BL_ExciseTariffDetails.E_SPECIAL + "' ,'" + BL_ExciseTariffDetails.E_EDU_CESS + "','" + BL_ExciseTariffDetails.E_H_EDU + "' ,'" + BL_ExciseTariffDetails.E_TALLY_BASIC + "','" + BL_ExciseTariffDetails.E_TALLY_SPECIAL + "','" + BL_ExciseTariffDetails.E_TALLY_EDU + "','" + BL_ExciseTariffDetails.E_TALLY_H_EDU + "','" + BL_ExciseTariffDetails.E_TALLY_GST_EXCISE + "','" + rbtType.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(E_CODE) from  EXCISE_TARIFF_MASTER");
                        CommonClasses.WriteLog("Excise Tariff master", "Save", "Excise Tariff Master", BL_ExciseTariffDetails.E_COMMODITY, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewGST_HSNSAC_Master.aspx?Title=true", false);
                    }
                    else
                    {
                        if (BL_ExciseTariffDetails.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ExciseTariffDetails.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_ExciseTariffDetails.Msg = "";
                        }
                        txtTariffNo.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_ExciseTariffDetails = new ExciseTariffDetails_BL(mlCode);
                if (Setvalues())
                {
                    DataTable dtExist = CommonClasses.Execute(" SELECT * FROM EXCISE_TARIFF_MASTER where( E_TARIFF_NO='" + txtTariffNo.Text.Trim() + "' OR E_COMMODITY='" + txtCommodity.Text.Trim() + "') AND ES_DELETE=0 AND E_TALLY_GST_EXCISE=1 AND E_CODE!='" + mlCode + "' AND  E_EX_TYPE='" + rbtType.SelectedValue + "'"); ;
                    if (dtExist.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtTariffNo.Focus();
                        return true;
                    }
                    if (CommonClasses.Execute1("UPDATE    EXCISE_TARIFF_MASTER SET E_TARIFF_NO ='" + BL_ExciseTariffDetails.E_TARIFF_NO + "', E_COMMODITY ='" + BL_ExciseTariffDetails.E_COMMODITY + "', E_BASIC ='" + BL_ExciseTariffDetails.E_BASIC + "', E_SPECIAL ='" + BL_ExciseTariffDetails.E_SPECIAL + "', E_EDU_CESS ='" + BL_ExciseTariffDetails.E_EDU_CESS + "', E_H_EDU ='" + BL_ExciseTariffDetails.E_H_EDU + "', E_TALLY_BASIC ='" + BL_ExciseTariffDetails.E_TALLY_BASIC + "', E_TALLY_SPECIAL ='" + BL_ExciseTariffDetails.E_TALLY_SPECIAL + "', E_TALLY_EDU ='" + BL_ExciseTariffDetails.E_TALLY_EDU + "', E_TALLY_H_EDU = '" + BL_ExciseTariffDetails.E_TALLY_H_EDU + "',E_EX_TYPE='" + rbtType.SelectedValue + "' WHERE E_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock(" EXCISE_TARIFF_MASTER", "MODIFY", "E_CODE", mlCode);
                        CommonClasses.WriteLog("Excise Tariff Master", "Update", "Excise Tariff Master", BL_ExciseTariffDetails.E_COMMODITY, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewGST_HSNSAC_Master.aspx?Title=true", false);
                    }
                    else
                    {
                        if (BL_ExciseTariffDetails.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ExciseTariffDetails.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_ExciseTariffDetails.Msg = "";
                        }
                        txtTariffNo.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Excise Tariff Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("EXCISE_TARIFF_MASTER", "MODIFY", "E_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewGST_HSNSAC_Master.aspx?Title=true", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtTariffNo.Text == "")
            {
                flag = false;
            }
            else if (txtCommodity.Text == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);
            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {
            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion
}