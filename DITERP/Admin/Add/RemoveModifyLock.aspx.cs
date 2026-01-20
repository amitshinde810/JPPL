using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Add_RemoveModifyLock : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    RemoveModifyLock_BL BL_RemoveModifyLock = new RemoveModifyLock_BL();
    static string TabName = "";
    static string TabCode = "";
    static string TabMod = "";
    static string TabDesc = "";
    static string TabDel = "";
    static string TabQuery = "";
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='4'");
                        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                        LoadForms();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Remove Modify Lock", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region LoadModule
    private void LoadModule()
    {
    }
    #endregion LoadModule

    #region LoadForm
    private void LoadForms()
    {
        DataTable dt = new DataTable();
        DataTable dtbind = new DataTable();
        try
        {
            dt = CommonClasses.Execute("SELECT SM_CODE,SM_NAME FROM SCREEN_MASTER,FORMS_MASTER where SM_CODE = FM_SM_CODE ");
            if (dt.Rows.Count > 0)
            {
                dtbind.Columns.Add("SM_CODE");
                dtbind.Columns.Add("SM_NAME");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dtForms = CommonClasses.Execute("select FM_SM_CODE, FM_TABLE, FM_TCODE, FM_DISP_FIELD, FM_TMODIFY, FM_TDEL, ISNULL(FM_QUERY,'') AS FM_QUERY from FORMS_MASTER where FM_SM_CODE=" + dt.Rows[i]["SM_CODE"].ToString() + "");
                    if (dtForms.Rows.Count > 0)
                    {
                        string TabName1 = dtForms.Rows[0]["FM_TABLE"].ToString();
                        string TabCode1 = dtForms.Rows[0]["FM_TCODE"].ToString();
                        string TabMod1 = dtForms.Rows[0]["FM_TMODIFY"].ToString();
                        string TabDesc1 = dtForms.Rows[0]["FM_DISP_FIELD"].ToString();
                        string TabDel1 = dtForms.Rows[0]["FM_TDEL"].ToString();
                        string TabQuery1 = dtForms.Rows[0]["FM_QUERY"].ToString();
                        DataTable dtcode = CommonClasses.Execute("Select " + TabCode1 + "," + TabDesc1 + " from " + TabName1 + " where " + TabMod1 + "=1 and " + TabDel1 + "=0    " + TabQuery1 + "");
                        if (dtcode.Rows.Count > 0)
                        {
                            dtbind.Rows.Add(dt.Rows[i]["SM_CODE"].ToString(), dt.Rows[i]["SM_NAME"].ToString());
                        }
                    }
                }
            }
            ddlFormName.DataSource = dtbind;
            ddlFormName.DataTextField = "SM_NAME";
            ddlFormName.DataValueField = "SM_CODE";
            ddlFormName.DataBind();
            ddlFormName.Items.Insert(0, new ListItem("Select Form Name", "0"));
            ddlFormName.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "LoadForms", ex.Message);
        }
    }
    #endregion Load State

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (dgRemoveModifyLock.Rows.Count == 0)
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
            CommonClasses.SendError("Currency Master", "CheckValid", Ex.Message);
        }
        return flag;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    private void CancelRecord()
    {
        try
        {
            dgRemoveModifyLock.DataSource = null;
            dgRemoveModifyLock.DataBind();
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region Imgclose_Click
    protected void Imgclose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "Imgclose_Click", ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtForms = CommonClasses.Execute("select * from FORMS_MASTER where FM_SM_CODE=" + ddlFormName.SelectedValue.ToString() + " ");
            if (dtForms.Rows.Count > 0)
            {
                TabName = dtForms.Rows[0]["FM_TABLE"].ToString();
                TabCode = dtForms.Rows[0]["FM_TCODE"].ToString();
                TabMod = dtForms.Rows[0]["FM_TMODIFY"].ToString();
                TabDesc = dtForms.Rows[0]["FM_DISP_FIELD"].ToString();
                TabDel = dtForms.Rows[0]["FM_TDEL"].ToString();
                TabQuery = dtForms.Rows[0]["FM_QUERY"].ToString();

                FillGrid();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "btnShow_Click", ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgRemoveModifyLock.Rows.Count != 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "btnSubmit_Click", ex.Message);
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
            CommonClasses.SendError("Remodify Lock Master ", "ShowMessage", Ex.Message);
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
            string[] values = TabName.Split(',');
            BL_RemoveModifyLock = new RemoveModifyLock_BL();
            BL_RemoveModifyLock.TableCode = TabCode;
            BL_RemoveModifyLock.TableName = values[0].ToString();
            BL_RemoveModifyLock.TableMod = TabMod;
            BL_RemoveModifyLock.UserID = Convert.ToInt32(Session["UserCode"]).ToString();
            BL_RemoveModifyLock.UserName = Session["Username"].ToString();
            BL_RemoveModifyLock.CompId = Session["CompanyId"].ToString();
            BL_RemoveModifyLock.CompCode = Session["CompanyCode"].ToString();
            if (BL_RemoveModifyLock.Save(dgRemoveModifyLock))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = BL_RemoveModifyLock.Msg;
                BL_RemoveModifyLock.Msg = "";
                result = true;
                dgRemoveModifyLock.DataSource = null;
                dgRemoveModifyLock.DataBind();
                LoadForms();
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region chkUpdate_CheckedChanged
    protected void chkUpdate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked == true)
        {
            for (int i = 0; i < dgRemoveModifyLock.Rows.Count; i++)
            {
                ((CheckBox)(dgRemoveModifyLock.Rows[i].FindControl("chkRemoveDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgRemoveModifyLock.Rows.Count; i++)
            {
                ((CheckBox)(dgRemoveModifyLock.Rows[i].FindControl("chkRemoveDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region dgUserRights_SelectedIndexChanged
    protected void dgUserRights_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region FillGrid
    private void FillGrid()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("Select DISTINCT " + TabCode + "," + TabDesc + " from " + TabName + " where " + TabMod + "=1 and " + TabDel + "=0   " + TabQuery + " ");

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("TabCode");
            dt1.Columns.Add("TabDesc");
            dt1.Columns.Add("Status");

            if (dt.Rows.Count != 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    DataRow dr = dt1.NewRow();
                    dr["TabCode"] = dt.Rows[k]["" + TabCode + ""].ToString();
                    dr["TabDesc"] = dt.Rows[k]["" + TabDesc + ""].ToString();
                    //comment by amit 20 09 2018
                    //if (dt.Columns.Count > 2)
                    //{
                    //    string aa = dt.Rows[k][1].ToString();
                    //    string bb = dt.Rows[k][2].ToString();
                    //    dr["TabDesc"] = aa + '/' + bb;
                    //}
                    //else
                    //{
                    //    //if (TabQuery == "")
                    //    //{
                    //    //    dr["TabDesc"] = dt.Rows[k]["" + TabDesc + ""].ToString();
                    //    //}
                    //    //else
                    //    //{
                    //    //    DataTable dtDisp = CommonClasses.Execute(TabQuery + " and " + TabDesc + "=" + dt.Rows[k]["" + TabDesc + ""].ToString());
                    //    //    dr["TabDesc"] = dtDisp.Rows[0][0].ToString();
                    //    //}
                    //}
                    dr["Status"] = false;
                    dt1.Rows.Add(dr);
                }
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Found", CommonClasses.MSG_Warning);
            }

            dgRemoveModifyLock.Visible = true;
            dgRemoveModifyLock.DataSource = dt1;
            dgRemoveModifyLock.DataBind();

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock", "FillGrid", ex.Message);
        }
    }
    #endregion
}
