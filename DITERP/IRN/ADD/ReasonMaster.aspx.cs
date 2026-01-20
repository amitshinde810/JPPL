using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_ADD_ReasonMaster : System.Web.UI.Page
{
    #region General Declaration
    UnitMaster_BL BL_UnitMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                //right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    loadStage();
                    ddlStage.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError(" Reason Master", "PageLoad", ex.Message);
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
            if (ddlStage.SelectedValue == "0")
            {
                ShowMessage("#Avisos", "Please Select Stage", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStage.Focus();
                return;
            }
            if (txtDefect.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Please Enter Defect", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStage.Focus();
                return;
            }
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            //{
            SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError(" Reason Master", "btnSubmit_Click", Ex.Message);
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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();

                }
                else
                {
                    CancelRecord();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError(" Reason Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            //if (txtName.Text.Trim() == "")
            //{
            //    flag = false;
            //}
            //else if (txtStageNo.Text.Trim() == "")
            //{
            //    flag = false;
            //}
            //else
            //{
            //    flag = true;
            //}

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Reason Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("REJECTIONSTAGE_MASTER", "MODIFY", "RSM_CODE", mlCode);
            }
            Response.Redirect("~/IRN/VIEW/ViewReasonMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Reason Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(" SELECT RM_CODE,RM_RSM_CODE,RM_CM_ID,RM_TYPE,RM_DEFECT,RM_ACTIVE  FROM REASON_MASTER,REJECTIONSTAGE_MASTER  where RM_RSM_CODE=RSM_CODE AND REASON_MASTER.ES_DELETE=0 AND RM_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                ddlStage.SelectedValue = dt.Rows[0]["RM_RSM_CODE"].ToString();
                txtDefect.Text = dt.Rows[0]["RM_DEFECT"].ToString();
                chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["RM_ACTIVE"].ToString());
                if (Convert.ToBoolean(dt.Rows[0]["RM_TYPE"].ToString()) == true)
                {
                    rbType.Items[0].Selected = true;
                }
                else
                {
                    rbType.Items[1].Selected = true;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REASON_MASTER", "MODIFY", "RM_CODE", mlCode);
            }
            else
            {
                ddlStage.Enabled = false;
                txtDefect.Enabled = false;
                btnSubmit.Visible = false;
                rbType.Enabled = false;
                chkActive.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Reason Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError(" Reason Master", "GetValues", ex.Message);
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
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError(" Reason Master", "Setvalues", ex.Message);
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
            string strType = "";
            bool type = true;
            if (rbType.SelectedValue == "1")
            {
                type = false;
            }
            DataTable dtExist = new DataTable();

            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM REASON_MASTER where RM_RSM_CODE='" + ddlStage.SelectedValue + "' AND RM_DEFECT='" + txtDefect.Text.Trim().Replace("'", "\''") + "' AND RM_TYPE='" + type + "' AND ES_DELETE=0");
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "This Defect already Exist for this Stage.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStage.Focus();
                }
                else
                {
                    if (CommonClasses.Execute1("INSERT INTO REASON_MASTER(RM_RSM_CODE, RM_TYPE, RM_DEFECT, RM_ACTIVE,RM_CM_ID)VALUES('" + ddlStage.SelectedValue + "','" + type + "','" + txtDefect.Text.Trim().Replace("'", "\''") + "','" + chkActive.Checked + "','" + Convert.ToInt32(Session["CompanyId"]) + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(RM_CODE) from REASON_MASTER");
                        CommonClasses.WriteLog("Reason Master", "Save", "Reason Master", ddlStage.SelectedItem.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/IRN/VIEW/ViewReasonMaster.aspx", false);
                    }
                    else
                    {

                        ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlStage.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM REASON_MASTER where RM_RSM_CODE='" + ddlStage.SelectedValue + "' AND RM_DEFECT='" + txtDefect.Text.Trim().Replace("'", "\''") + "' AND RM_TYPE='" + type + "' AND ES_DELETE=0 AND RM_CODE!='" + mlCode + "'");
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "This Defect already Exist for this Stage.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStage.Focus();
                }
                else
                {
                    if (CommonClasses.Execute1(" UPDATE    REASON_MASTER SET RM_RSM_CODE ='" + ddlStage.SelectedValue + "', RM_TYPE = '" + type + "', RM_DEFECT ='" + txtDefect.Text.Trim().Replace("'", "\''") + "', RM_ACTIVE = '" + chkActive.Checked + "'  where RM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("REASON_MASTER", "MODIFY", "RM_CODE", mlCode);
                        CommonClasses.WriteLog("Reason Master", "Update", "Reason Master", ddlStage.SelectedItem.ToString(), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/IRN/VIEW/ViewReasonMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlStage.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError(" Reason Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError(" Reason Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    public void loadStage()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute("SELECT RSM_CODE, RSM_NO+' - '+RSM_NAME  AS RSM_NO  FROM  REJECTIONSTAGE_MASTER where    RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0");
        ddlStage.DataSource = dtStage;
        ddlStage.DataTextField = "RSM_NO";
        ddlStage.DataValueField = "RSM_CODE";
        ddlStage.DataBind();
        ddlStage.Items.Insert(0, new ListItem("----Select Stage----", "0"));
    }

    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute("SELECT ISNULL(RSM_CASTING,0) AS RSM_CASTING,INSULL(RSM_MECHINING,0) AS RSM_MECHINING FROM  REJECTIONSTAGE_MASTER where    RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND RSM_CODE='" + ddlStage.SelectedValue + "'");
        if (dtStage.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtStage.Rows[0]["RSM_CASTING"].ToString()) == true)
            {
                rbType.Items[0].Selected = true;
                rbType.Items[0].Enabled = true;
            }
            else
            {
                rbType.Items[1].Selected = true;
                rbType.Items[0].Selected = false;
                rbType.Items[0].Enabled = false;
            }
            if (Convert.ToBoolean(dtStage.Rows[0]["RSM_MECHINING"].ToString()) == true)
            {
                rbType.Items[1].Selected = true;
                rbType.Items[1].Enabled = true;
            }
            else
            {
                rbType.Items[0].Selected = true;
                rbType.Items[1].Selected = false;
                rbType.Items[1].Enabled = false;
            }
        }
    }
}
