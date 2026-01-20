using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_ADD_Turning_Inward : System.Web.UI.Page
{
    #region General Declaration
    double PedQty = 0;
    static int mlCode = 0;
    static string right = "";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    public string Msg = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                ViewState["mlCode"] = mlCode;
                ViewState["Index"] = Index;
                ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                ViewState["dt2"] = dt2;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='10'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtGRNDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        BlankGrid();
                        LoadCombos();
                        dtFilter.Rows.Clear();
                        str = "";
                        txtGRNDate.Attributes.Add("readonly", "readonly");
                        txtChallanDate.Attributes.Add("readonly", "readonly");
                    }
                    ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Material Inward", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region CtlDisable
    public void CtlDisable()
    {
        ddlSupplier.Enabled = false;
        txtGRNno.Enabled = false;
        txtChallanNo.Enabled = false;
        txtChallanDate.Enabled = false;
        txtGRNDate.Enabled = false;
        BtnInsert.Visible = false;
        btnSubmit.Visible = false;
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGRNDate.Attributes.Add("readonly", "readonly");
            LoadCombos();
            dt = CommonClasses.Execute("SELECT * FROM SCRAP_MASTER where ES_DELETE=0 and SM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SM_CODE"]);
                txtGRNno.Text = Convert.ToInt32(dt.Rows[0]["SM_IWM_NO"]).ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["SM_IWM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = Convert.ToInt32(dt.Rows[0]["SM_P_CODE"]).ToString();
                txtChallanNo.Text = (dt.Rows[0]["SM_CH_NO"]).ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["SM_CH_DATE"]).ToString("dd MMM yyyy");
                int id = Convert.ToInt32(ddlSupplier.SelectedValue);

                dtInwardDetail = CommonClasses.Execute("select SD_I_CODE,I_CODENO,I_NAME,SD_REV_QTY,I_UOM_NAME,SD_UOM_CODE from SCRAP_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE I_CODE=SD_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=SD_UOM_CODE AND SD_SM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }
            PendQty();

            if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;
                ddlItemName.Enabled = false;
                ddlItemCode.Enabled = false;
                txtRecdQty.Enabled = false;
                dgInwardMaster.Enabled = false;
            }

            if (str == "MOD")
            {
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("SCRAP_MASTER", "MODIFY", "SM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SCRAP Inward", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {
                if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtGRNDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtGRNDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtChallanDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtChallanDate.Text))))
                {
                    if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Sub Contractor";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemCode.Focus();
                        return;
                    }
                    if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                    {
                        SaveRec();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Material Inward", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT max(SM_IWM_NO) as SM_IWM_NO from SCRAP_MASTER where SM_CM_CODE='" + Session["CompanyCode"] + "' and ES_DELETE=0");
        if (dt.Rows[0]["SM_IWM_NO"] == null || dt.Rows[0]["SM_IWM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["SM_IWM_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region Save
    bool Save()
    {
        bool res = false;
        try
        {
            res = CommonClasses.Execute1("INSERT INTO SCRAP_MASTER(SM_CM_CODE,SM_P_CODE,SM_CH_NO,SM_CH_DATE,SM_IWM_NO,SM_IWM_DATE)VALUES('" + Session["CompanyCode"] + "','" + ddlSupplier.SelectedValue + "','" + txtChallanNo.Text + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + txtGRNno.Text + "','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "')");
            if (res)
            {
                string SD_SM_CODE = CommonClasses.GetMaxId("SELECT MAX(SM_CODE) FROM SCRAP_MASTER");
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string SD_I_CODE = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_I_CODE")).Text;
                    string SD_REV_QTY = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_REV_QTY")).Text;
                    string SD_UOM_CODE = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_UOM_CODE")).Text;
                    res = CommonClasses.Execute1("INSERT INTO SCRAP_DETAIL (SD_SM_CODE,SD_I_CODE,SD_REV_QTY,SD_UOM_CODE)VALUES('" + SD_SM_CODE + "','" + SD_I_CODE + "','" + SD_REV_QTY + "','" + SD_UOM_CODE + "')");
                    if (res)
                    {
                        double temQty = Convert.ToDouble(SD_REV_QTY);
                        DataTable dtBallance = CommonClasses.Execute("SELECT ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL,IWD_TUR_QTY,IWD_REC_TUR_QTY,IWD_CODE,IWM_CODE,IWD_IWM_CODE,IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CODE='" + ddlSupplier.SelectedValue + "' and ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) >0 ORDER BY  IWM_CODE");
                        double TurBal = 0;
                        for (int j = 0; j < dtBallance.Rows.Count; j++)
                        {
                            TurBal = Convert.ToDouble(dtBallance.Rows[j]["TUR_BAL"].ToString());
                            if (TurBal >= temQty)
                            {
                                CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY+ " + temQty + " WHERE IWD_CODE='" + dtBallance.Rows[j]["IWD_CODE"].ToString() + "'");
                                temQty = temQty - temQty;
                            }
                            else
                            {
                                CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY+ " + TurBal + " WHERE IWD_CODE='" + dtBallance.Rows[j]["IWD_CODE"].ToString() + "'");
                                temQty = temQty - TurBal;
                            }
                            if (temQty <= 0)
                            {
                                res = true;
                                break;
                            }
                        }
                    }
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER(STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY , STL_STORE_TYPE)VALUES('" + SD_I_CODE + "','" + SD_SM_CODE + "','" + txtGRNno.Text + "','TURIWD','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + SD_REV_QTY + "', -2147483647)");
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + SD_REV_QTY + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "' where  I_CODE='" + SD_I_CODE + "'");
                }
            }
        }
        catch (Exception)
        {

        }
        return res;
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
            CommonClasses.SendError("Turning Inward", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                txtGRNno.Text = Numbering();

                if (Save())
                {
                    string Code = CommonClasses.GetMaxId("Select ISNULL(Max(SM_IWM_NO),0) SM_IWM_NO FROM SCRAP_MASTER");
                    CommonClasses.WriteLog("Turning Inward ", "Insert", "Turning Inward", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
                }
                else
                {
                    ddlSupplier.Focus();
                }
            }
            #endregion

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dtinwDetail = CommonClasses.Execute("select SD_I_CODE,I_CODENO,I_NAME,SD_REV_QTY,I_UOM_NAME,SD_UOM_CODE from SCRAP_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE I_CODE=SD_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=SD_UOM_CODE AND SD_SM_CODE='" + ViewState["mlCode"].ToString() + "'");
                for (int i = 0; i < dtinwDetail.Rows.Count; i++)
                {
                    DataTable dtRevQty = CommonClasses.Execute("SELECT ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL,IWD_TUR_QTY,IWD_REC_TUR_QTY,IWD_CODE,IWM_CODE,IWD_IWM_CODE,IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CODE='" + ddlSupplier.SelectedValue + "' and ROUND(IWD_REC_TUR_QTY,3) >0 ORDER BY  IWM_CODE desc");
                    double TurQty = 0;
                    double TEmpQty = 0;
                    double inwUpdtedQty = 0;
                    TurQty = Convert.ToDouble(dtinwDetail.Rows[i]["SD_REV_QTY"].ToString());
                    CommonClasses.Execute("UPDATE ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + TurQty + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "' where  I_CODE='" + dtinwDetail.Rows[i]["SD_I_CODE"].ToString() + "'");
                    TEmpQty = TurQty;
                    for (int j = 0; j < dtRevQty.Rows.Count; j++)
                    {
                        inwUpdtedQty = Convert.ToDouble(dtRevQty.Rows[j]["IWD_REC_TUR_QTY"].ToString());
                        if (TEmpQty > inwUpdtedQty)
                        {
                            CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY- " + inwUpdtedQty + " WHERE IWD_CODE='" + dtRevQty.Rows[j]["IWD_CODE"].ToString() + "'");
                            TEmpQty = TEmpQty - inwUpdtedQty;
                        }
                        else
                        {
                            CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY- " + TEmpQty + " WHERE IWD_CODE='" + dtRevQty.Rows[j]["IWD_CODE"].ToString() + "'");
                            break;
                        }
                    }
                }
                bool res = false;
                res = CommonClasses.Execute1("UPDATE SCRAP_MASTER SET SM_CM_CODE='" + Session["CompanyCode"] + "',SM_P_CODE='" + ddlSupplier.SelectedValue + "',SM_CH_NO='" + txtChallanNo.Text + "',SM_CH_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "',SM_IWM_NO='" + txtGRNno.Text + "',SM_IWM_DATE='" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "' where SM_CODE='" + ViewState["mlCode"].ToString() + "'");
                if (res)
                {
                    CommonClasses.Execute("DELETE FROM SCRAP_DETAIL WHERE SD_SM_CODE='" + ViewState["mlCode"].ToString() + "'");
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + ViewState["mlCode"].ToString() + "' and  STL_DOC_TYPE='TURIWD'");
                    for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                    {
                        string SD_I_CODE = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_I_CODE")).Text;
                        string SD_REV_QTY = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_REV_QTY")).Text;
                        string SD_UOM_CODE = ((Label)dgInwardMaster.Rows[i].FindControl("lblSD_UOM_CODE")).Text;
                        res = CommonClasses.Execute1("INSERT INTO SCRAP_DETAIL (SD_SM_CODE,SD_I_CODE,SD_REV_QTY,SD_UOM_CODE)VALUES('" + ViewState["mlCode"].ToString() + "','" + SD_I_CODE + "','" + SD_REV_QTY + "','" + SD_UOM_CODE + "')");
                        if (res)
                        {
                            double temQty = Convert.ToDouble(SD_REV_QTY);
                            DataTable dtBallance = CommonClasses.Execute("SELECT ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL,IWD_TUR_QTY,IWD_REC_TUR_QTY,IWD_CODE,IWM_CODE,IWD_IWM_CODE,IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CODE='" + ddlSupplier.SelectedValue + "' and ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) >0 ORDER BY  IWM_CODE");
                            double TurBal = 0;
                            for (int j = 0; j < dtBallance.Rows.Count; j++)
                            {
                                TurBal = Convert.ToDouble(dtBallance.Rows[j]["TUR_BAL"].ToString());
                                if (TurBal >= temQty)
                                {
                                    CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY+ " + temQty + " WHERE IWD_CODE='" + dtBallance.Rows[j]["IWD_CODE"].ToString() + "'");
                                    temQty = temQty - temQty;
                                }
                                else
                                {
                                    CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY+ " + TurBal + " WHERE IWD_CODE='" + dtBallance.Rows[j]["IWD_CODE"].ToString() + "'");
                                    temQty = temQty - TurBal;
                                }
                                if (temQty <= 0)
                                {
                                    res = true;
                                    break;
                                }
                            }
                        }
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER(STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY , STL_STORE_TYPE)VALUES('" + SD_I_CODE + "','" + ViewState["mlCode"].ToString() + "','" + txtGRNno.Text + "','TURIWD','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + SD_REV_QTY + "', -2147483647)");
                        CommonClasses.Execute("UPDATE ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + SD_REV_QTY + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "' where  I_CODE='" + SD_I_CODE + "'");
                    }
                }
                CommonClasses.RemoveModifyLock("SCRAP_MASTER", "MODIFY", "SM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                CommonClasses.WriteLog("Turning Inward ", "Update", "Material Inward", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillSupplier
        try
        {
            DataTable dtParty = new DataTable();
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME");
            }
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
            dt = CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME", ddlItemName);
            ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
            DataTable dt1 = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO", ddlItemCode);
            ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillUOM
        try
        {
            dt = CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_UOM_NAME", ddlUom);
            ddlUom.Items.Insert(0, new ListItem(" ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Turning Inward", "LoadCombos", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSupplier.Text == "")
            {
                flag = false;
            }
            else if (txtChallanNo.Text == "")
            {
                flag = false;
            }
            else if (txtChallanDate.Text == "")
            {
                flag = false;
            }
            else if (txtGRNDate.Text == "")
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
            CommonClasses.SendError("Material Inward", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && ViewState["mlCode"].ToString() != null)
            {
                CommonClasses.RemoveModifyLock("SCRAP_MASTER", "MODIFY", "SM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Turning Inward", "CancelRecord", ex.Message.ToString());
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
                Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        PendQty();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.SelectedIndex != 0)
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            }
            else
            {
                ddlUom.Text = "";
            }
        }
        txtRecdQty.Text = "";
        pendingQty();
    }
    #endregion

    private void PendQty()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT (SUM(ISNULL(IWD_TUR_QTY,0))-SUM(ISNULL(IWD_REC_TUR_QTY,0))) AS PENDQTY FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception)
        {
        }
    }

    #region pendingQty
    private void pendingQty()
    {
        #region PendingQty
        try
        {
            if (ddlSupplier.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Sub Contractor";
                return;
            }
            dt = CommonClasses.Execute("SELECT (SUM(ISNULL(IWD_TUR_QTY,0))-SUM(ISNULL(IWD_REC_TUR_QTY,0))) AS PENDQTY FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                DataTable dtUOM = CommonClasses.Execute("SELECT I_UOM_CODE FROM ITEM_MASTER WHERE I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtUOM.Rows.Count > 0)
                {
                    ddlUom.SelectedValue = dtUOM.Rows[0]["I_UOM_CODE"].ToString();
                }
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "PendingQty", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (ddlItemName.SelectedIndex != 0)
        {
            ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            }
            else
            {
                ddlUom.Text = "";
            }
            txtRecdQty.Text = "";
            pendingQty();
        }
    }
    #endregion

    #region BtnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;

            if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Sub Contractor";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (Convert.ToInt32(ddlItemName.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (txtRecdQty.Text == "" || Convert.ToDecimal(txtRecdQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recd. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecdQty.Focus();
                return;
            }
            double chq = Convert.ToDouble(txtRecdQty.Text);
            double pdq = Convert.ToDouble(txtPendingQty.Text);
            if (pdq < chq)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Recieved quantity should always be less or equal to pending quantity";
                txtRecdQty.Text = "";
                txtRecdQty.Focus();
                return;
            }
            PanelMsg.Visible = false;

            #region Check Duplicate Inserted_Grid
            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblSD_I_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion Check Duplicate Inserted_Grid

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("SD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("SD_REV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("SD_UOM_CODE");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["SD_I_CODE"] = ddlItemName.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["I_NAME"] = ddlItemName.SelectedItem;
            dr["SD_REV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtRecdQty.Text)));
            dr["I_UOM_NAME"] = ddlUom.SelectedItem;
            dr["SD_UOM_CODE"] = ddlUom.SelectedValue;
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    ddlItemName.SelectedIndex = 0;
                    ddlItemCode.SelectedIndex = 0;
                    double pendQty = Convert.ToDouble(txtPendingQty.Text);
                    double recQty = Convert.ToDouble(txtRecdQty.Text);
                    txtPendingQty.Text = Math.Round((pendQty - recQty), 3).ToString();
                    txtRecdQty.Text = "";
                    str = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                ddlItemName.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                double pendQty = Convert.ToDouble(txtPendingQty.Text);
                double recQty = Convert.ToDouble(txtRecdQty.Text);
                txtPendingQty.Text = Math.Round((pendQty - recQty), 3).ToString();
                txtRecdQty.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            #endregion
            ViewState["ItemUpdateIndex"] = "-1";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region BlankGrid
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("SD_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SD_REV_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SD_UOM_CODE", typeof(string)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Clear();
        }
    }
    #endregion

    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgInwardMaster.DataBind();
                if (dgInwardMaster.Rows.Count == 0)
                    BlankGrid();
            }
            if (e.CommandName == "Select")
            {
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                string s = ((Label)(row.FindControl("lblSD_I_CODE"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblSD_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblSD_I_CODE"))).Text;

                txtRecdQty.Text = ((Label)(row.FindControl("lblSD_REV_QTY"))).Text;
                if (txtRecdQty.Text == "")
                {
                    txtRecdQty.Text = "0";
                } dt = CommonClasses.Execute("SELECT (SUM(ISNULL(IWD_TUR_QTY,0))-SUM(ISNULL(IWD_REC_TUR_QTY,0))) AS PENDQTY FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0");

                double gridRev = 0;

                foreach (GridViewRow row1 in dgInwardMaster.Rows)
                {
                    var numberLabel = row1.FindControl("lblSD_REV_QTY") as Label;
                    gridRev = gridRev + Convert.ToDouble(numberLabel.Text);
                }

                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    PedQty = Convert.ToDouble(txtPendingQty.Text) + Convert.ToDouble(txtRecdQty.Text);
                }
                else
                {
                    PedQty = Convert.ToDouble(txtPendingQty.Text) + Convert.ToDouble(txtRecdQty.Text);
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                ddlUom.SelectedValue = ((Label)(row.FindControl("lblSD_UOM_CODE"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInwardMaster_Deleting
    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
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
            CommonClasses.SendError("Turming Inward", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion
}
