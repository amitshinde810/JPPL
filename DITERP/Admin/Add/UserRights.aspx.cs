using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Add_UserRights : System.Web.UI.Page
{
    # region Variables
    private char[] passCharactors = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!@#$%^&*".ToCharArray();
    static int mlCode = 0;
    static string right = "";
    UserRights_BL BL_UserRights = new UserRights_BL();
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
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='3'");
                        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                        {
                            LoadForms();
                            LoadUser();
                            LoadUserCopyFrom();
                            ddlUserName.SelectedIndex = 0;

                            ChkCustCopy_CheckedChanged(null, null);
                        }
                        else
                        {
                            Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("User Rights", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Rights", "Page_Load", Ex.Message);

        }
    }

    private void LoadBranchName()
    {
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_UserRights = new UserRights_BL(mlCode);
            DataTable dt = new DataTable();
            if (Request.QueryString[0].Equals("VIEW"))
            {
                BL_UserRights.GetInfo(dgUserRights);
            }
            GetValues(str);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region LoadUser
    private void LoadUser()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                BL_UserRights = new UserRights_BL();
                dt = CommonClasses.Execute("SELECT UM_CODE,UM_NAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID=" + Session["CompanyId"] + "  ORDER BY UM_NAME");
                ddlUserName.DataSource = dt;
                ddlUserName.DataTextField = "UM_NAME";
                ddlUserName.DataValueField = "UM_CODE";
                ddlUserName.DataBind();
                ddlUserName.Items.Insert(0, new ListItem("Select User Name", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("User Rights", "LoadUser", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "LoadUser", ex.Message);
        }
    }
    #endregion LoadUser

    #region LoadUserCopyFrom
    private void LoadUserCopyFrom()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                BL_UserRights = new UserRights_BL();
                dt = CommonClasses.Execute("select UM_CODE,UM_NAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID=" + Session["CompanyId"] + " ");
                ddlanUser.DataSource = dt;
                ddlanUser.DataTextField = "UM_NAME";
                ddlanUser.DataValueField = "UM_CODE";
                ddlanUser.DataBind();
                ddlanUser.Items.Insert(0, new ListItem("Select User Name", "0"));
                ddlanUser.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("User Rights", "LoadUserCopyFrom", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "LoadUserCopyFrom", ex.Message);
        }
    }
    #endregion LoadUserlblm

    #region LoadModule
    private void LoadModule()
    {
    }
    #endregion LoadModule

    #region LoadForm
    private void LoadForms()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                BL_UserRights = new UserRights_BL();
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from SCREEN_MASTER order by SM_MOD_CODE,SM_NAME");
                ddlFormName.DataSource = dt;
                ddlFormName.DataTextField = "SM_NAME";
                ddlFormName.DataValueField = "SM_CODE";
                ddlFormName.DataBind();
                ddlFormName.Items.Insert(0, new ListItem("Select FormName", "0"));
                ddlFormName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("User Rights", "LoadForms", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "LoadForms", ex.Message);
        }
    }
    #endregion Load State

    #region FillGrid
    private void FillGrid(string UName, string FormName)
    {
        DataTable dt = new DataTable();
        try
        {
            BL_UserRights = new UserRights_BL();
            if (ChkCustCopy.Checked == false)
            {
                if (ChkAll.Checked == true)
                {
                    dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER order by SM_MOD_CODE,SM_NAME");
                }
                else
                {
                    dt = BL_UserRights.GetDepartment("select UR_UM_CODE as USR_CODE,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE  and UR_IS_DELETE='0' and UR_UM_CODE='" + UName + "'  and SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("USR_CODE");
                dt1.Columns.Add("SM_CODE");
                dt1.Columns.Add("Name");
                dt1.Columns.Add("Save");
                dt1.Columns.Add("View");
                dt1.Columns.Add("Update");
                dt1.Columns.Add("Add");
                dt1.Columns.Add("Delete");
                dt1.Columns.Add("Print");
                dt1.Columns.Add("Back Date");
                if (dt.Rows.Count != 0)
                {
                    string Rights = "";
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        Rights = "0000000";

                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = UName.ToString();
                        dr["SM_CODE"] = dt.Rows[k]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[k]["Name"].ToString();

                        DataTable dtRight = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where  UR_IS_DELETE='0' and UR_UM_CODE='" + UName + "' and UR_SM_CODE=" + dt.Rows[k]["SM_CODE"] + "");
                        if (dtRight.Rows.Count == 0)
                        {
                            Rights = "0000000";
                        }
                        else
                        {
                            Rights = dtRight.Rows[0][0].ToString();
                        }

                        char[] URight = (Rights).ToCharArray();
                        DataTable dtExist = BL_UserRights.GetDepartment("select UR_UM_CODE ,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE and UR_IS_DELETE='0' and  UR_UM_CODE='" + UName + "' and SM_CODE='" + dt.Rows[k]["SM_CODE"].ToString() + "'  order by SM_MOD_CODE,SM_NAME");
                        if (dtExist.Rows.Count != 0)
                        {
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {

                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        else
                        {
                            URight = ("0000000").ToCharArray();
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string val = ((Label)(dgUserRights.Rows[i].FindControl("USR_CODE"))).Text;
                        if (val == dt1.Rows[i]["USR_CODE"].ToString())
                        {
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Save"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["View"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Update"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Add"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Delete"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Print"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Back Date"]);
                        }
                    }
                }
                else
                {
                    dt.Dispose();
                    if (ChkAll.Checked == true)
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER order by SM_MOD_CODE,SM_NAME");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER where SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                    }

                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = ddlUserName.SelectedValue.ToString();
                        dr["SM_CODE"] = dt.Rows[i]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[i]["Name"].ToString();
                        for (int n = 3; n < dt1.Columns.Count; n++)
                        {
                            dr[n] = false;
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                }
            }
            else
            {
                if (ChkAll.Checked == true)
                {
                    dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER order by SM_MOD_CODE,SM_NAME ");
                }
                else
                {
                    dt = BL_UserRights.GetDepartment("select UR_UM_CODE as USR_CODE,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE  and UR_IS_DELETE='0' and UR_UM_CODE='" + ddlanUser.SelectedValue.ToString() + "'  and SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("USR_CODE");
                dt1.Columns.Add("SM_CODE");
                dt1.Columns.Add("Name");
                dt1.Columns.Add("Save");
                dt1.Columns.Add("View");
                dt1.Columns.Add("Update");
                dt1.Columns.Add("Add");
                dt1.Columns.Add("Delete");
                dt1.Columns.Add("Print");
                dt1.Columns.Add("Back Date");
                if (dt.Rows.Count != 0)
                {
                    string Rights = "";
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        Rights = "0000000";

                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = UName.ToString();
                        dr["SM_CODE"] = dt.Rows[k]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[k]["Name"].ToString();

                        DataTable dtRight = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where  UR_IS_DELETE='0' and UR_UM_CODE='" + ddlanUser.SelectedValue.ToString() + "' and UR_SM_CODE=" + dt.Rows[k]["SM_CODE"] + "");
                        if (dtRight.Rows.Count == 0)
                        {
                            Rights = "0000000";
                        }
                        else
                        {
                            Rights = dtRight.Rows[0][0].ToString();
                        }

                        char[] URight = (Rights).ToCharArray();
                        DataTable dtExist = BL_UserRights.GetDepartment("select UR_UM_CODE ,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER,CM_MODULE where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE and UR_IS_DELETE='0' and  UR_UM_CODE='" + UName + "' and SM_CODE='" + dt.Rows[k]["SM_CODE"].ToString() + "' ");
                        if (dtExist.Rows.Count != 0)
                        {
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        else
                        {
                            //URight = ("0000000").ToCharArray();
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string val = ((Label)(dgUserRights.Rows[i].FindControl("USR_CODE"))).Text;
                        if (val == dt1.Rows[i]["USR_CODE"].ToString())
                        {
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Save"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["View"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Update"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Add"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Delete"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Print"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Back Date"]);
                        }
                    }
                }
                else
                {
                    dt.Dispose();
                    if (ChkAll.Checked == true)
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER order by SM_MOD_CODE,SM_NAME ");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER where And SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                    }
                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = ddlUserName.SelectedValue.ToString();
                        dr["SM_CODE"] = dt.Rows[i]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[i]["Name"].ToString();
                        for (int n = 3; n < dt1.Columns.Count; n++)
                        {
                            dr[n] = false;
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "FillGrid", ex.Message);
        }
    }
    #endregion

    #region FillGrid
    private void FillGrid(string UName, string FormName, string Module)
    {
        DataTable dt = new DataTable();
        try
        {
            BL_UserRights = new UserRights_BL();
            if (ChkCustCopy.Checked == false)
            {
                if (ChkAll.Checked == true)
                {
                    if (Module == "0")
                    {
                        dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER    order by SM_MOD_CODE,SM_NAME");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER  where SM_MOD_CODE='" + Module + "' order by SM_MOD_CODE,SM_NAME");
                    }
                }
                else
                {
                    if (Module == "0")
                    {
                        dt = BL_UserRights.GetDepartment("select UR_UM_CODE as USR_CODE,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where   SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE  and UR_IS_DELETE='0' and UR_UM_CODE='" + UName + "'  and SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("select UR_UM_CODE as USR_CODE,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where  SM_MOD_CODE='" + Module + "' AND  SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE  and UR_IS_DELETE='0' and UR_UM_CODE='" + UName + "'  and SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");

                    }
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("USR_CODE");
                dt1.Columns.Add("SM_CODE");
                dt1.Columns.Add("Name");
                dt1.Columns.Add("Save");
                dt1.Columns.Add("View");
                dt1.Columns.Add("Update");
                dt1.Columns.Add("Add");
                dt1.Columns.Add("Delete");
                dt1.Columns.Add("Print");
                dt1.Columns.Add("Back Date");
                if (dt.Rows.Count != 0)
                {
                    string Rights = "";
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        Rights = "0000000";

                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = UName.ToString();
                        dr["SM_CODE"] = dt.Rows[k]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[k]["Name"].ToString();
                        DataTable dtRight = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where  UR_IS_DELETE='0' and UR_UM_CODE='" + UName + "' and UR_SM_CODE=" + dt.Rows[k]["SM_CODE"] + "");
                        if (dtRight.Rows.Count == 0)
                        {
                            Rights = "0000000";
                        }
                        else
                        {
                            Rights = dtRight.Rows[0][0].ToString();
                        }
                        char[] URight = (Rights).ToCharArray();
                        DataTable dtExist = BL_UserRights.GetDepartment("select UR_UM_CODE ,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE and UR_IS_DELETE='0' and  UR_UM_CODE='" + UName + "' and SM_CODE='" + dt.Rows[k]["SM_CODE"].ToString() + "'  order by SM_MOD_CODE,SM_NAME");
                        if (dtExist.Rows.Count != 0)
                        {
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        else
                        {
                            URight = ("0000000").ToCharArray();
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();

                    try
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string val = ((Label)(dgUserRights.Rows[i].FindControl("USR_CODE"))).Text;
                            if (val == dt1.Rows[i]["USR_CODE"].ToString())
                            {
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Save"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["View"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Update"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Add"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Delete"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Print"]);
                                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Back Date"]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                   
                }
                else
                {
                    dt.Dispose();
                    if (ChkAll.Checked == true)
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER order by SM_MOD_CODE,SM_NAME");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER where SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                    }

                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = ddlUserName.SelectedValue.ToString();
                        dr["SM_CODE"] = dt.Rows[i]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[i]["Name"].ToString();
                        for (int n = 3; n < dt1.Columns.Count; n++)
                        {
                            dr[n] = false;
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                }
            }
            else
            {
                if (ChkAll.Checked == true)
                {
                    if (Module == "0")
                    {
                        dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER    order by SM_MOD_CODE,SM_NAME");

                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("SELECT  SM_CODE, SM_NAME as Name FROM SCREEN_MASTER  where SM_MOD_CODE='" + Module + "' order by SM_MOD_CODE,SM_NAME");
                    }
                }
                else
                {
                    dt = BL_UserRights.GetDepartment("select UR_UM_CODE as USR_CODE,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER where SM_MOD_CODE='" + Module + "' AND  SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE  and UR_IS_DELETE='0' and UR_UM_CODE='" + ddlanUser.SelectedValue.ToString() + "'  and SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("USR_CODE");
                dt1.Columns.Add("SM_CODE");
                dt1.Columns.Add("Name");
                dt1.Columns.Add("Save");
                dt1.Columns.Add("View");
                dt1.Columns.Add("Update");
                dt1.Columns.Add("Add");
                dt1.Columns.Add("Delete");
                dt1.Columns.Add("Print");
                dt1.Columns.Add("Back Date");
                if (dt.Rows.Count != 0)
                {
                    string Rights = "";
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        Rights = "0000000";

                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = UName.ToString();
                        dr["SM_CODE"] = dt.Rows[k]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[k]["Name"].ToString();

                        DataTable dtRight = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where  UR_IS_DELETE='0' and UR_UM_CODE='" + ddlanUser.SelectedValue.ToString() + "' and UR_SM_CODE=" + dt.Rows[k]["SM_CODE"] + "");
                        if (dtRight.Rows.Count == 0)
                        {
                            Rights = "0000000";
                        }
                        else
                        {
                            Rights = dtRight.Rows[0][0].ToString();
                        }
                        char[] URight = (Rights).ToCharArray();
                        DataTable dtExist = BL_UserRights.GetDepartment("select UR_UM_CODE ,SM_CODE,SM_NAME as Name,UR_RIGHTS from USER_RIGHT,SCREEN_MASTER,USER_MASTER,CM_MODULE where SM_CODE=UR_SM_CODE and UM_CODE=UR_UM_CODE and UR_IS_DELETE='0' and  UR_UM_CODE='" + UName + "' and SM_CODE='" + dt.Rows[k]["SM_CODE"].ToString() + "' ");
                        if (dtExist.Rows.Count != 0)
                        {
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        else
                        {
                            for (int m = 0, n = 3; m < URight.Length; m++, n++)
                            {
                                if (URight[m] == '1')
                                {
                                    dr[n] = true;
                                }
                                else
                                {
                                    dr[n] = false;
                                }
                            }
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string val = ((Label)(dgUserRights.Rows[i].FindControl("USR_CODE"))).Text;
                        if (val == dt1.Rows[i]["USR_CODE"].ToString())
                        {
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Save"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["View"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Update"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Add"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Delete"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Print"]);
                            ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Back Date"]);
                        }
                    }
                }
                else
                {
                    dt.Dispose();
                    if (ChkAll.Checked == true)
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER order by SM_MOD_CODE,SM_NAME ");
                    }
                    else
                    {
                        dt = BL_UserRights.GetDepartment("select SM_CODE,SM_NAME as Name from SCREEN_MASTER where And SM_CODE='" + FormName + "' order by SM_MOD_CODE,SM_NAME");
                    }
                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["USR_CODE"] = ddlUserName.SelectedValue.ToString();
                        dr["SM_CODE"] = dt.Rows[i]["SM_CODE"].ToString();
                        dr["Name"] = dt.Rows[i]["Name"].ToString();
                        for (int n = 3; n < dt1.Columns.Count; n++)
                        {
                            dr[n] = false;
                        }
                        dt1.Rows.Add(dr);
                    }
                    dgUserRights.Visible = true;
                    dgUserRights.DataSource = dt1;
                    dgUserRights.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "FillGrid", ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Request.QueryString[0].Equals("VIEW"))
            //{
            //    CancelRecord();
            //}
            //else
            //{
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
            //}

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
            if (dgUserRights.Rows.Count == 0)
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
            CommonClasses.SendError("User Right", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    private void CancelRecord()
    {
        try
        {
            dgUserRights.DataSource = null;
            dgUserRights.DataBind();
            ddlUserName.SelectedIndex = 0;
            ChkAll.Checked = false;
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "btnCancel_Click", ex.Message);
        }
    }

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
            CommonClasses.SendError("User Right", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgUserRights.Rows.Count != 0)
        {
            SaveRec();
        }
        else
        {
            lblmsg.Text = "Record Not Found In Table";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        DataTable CheckDt = new DataTable();
        try
        {
            if (ChkAll.Checked != true)
            {
                CheckDt = CommonClasses.Execute("select UR_UM_CODE,UR_SM_CODE,UR_RIGHTS,UR_IS_DELETE,SM_MOD_CODE from USER_RIGHT,SCREEN_MASTER where UR_IS_DELETE=0 and UR_SM_CODE=SM_CODE AND UR_UM_CODE='" + ddlUserName.SelectedValue.ToString() + "'and UR_SM_CODE='" + ddlFormName.SelectedValue.ToString() + "'");
            }
            else
            {
                CheckDt = CommonClasses.Execute("select UR_UM_CODE,UR_SM_CODE,UR_RIGHTS,UR_IS_DELETE,SM_MOD_CODE from USER_RIGHT,SCREEN_MASTER where UR_IS_DELETE=0 and UR_SM_CODE=SM_CODE AND UR_UM_CODE='" + ddlUserName.SelectedValue.ToString() + "' ");
            }
            if (CheckDt.Rows.Count == 0)
            {
                BL_UserRights = new UserRights_BL();
                if (BL_UserRights.Save(dgUserRights))
                {
                    CommonClasses.WriteLog("User Rights", "Save", "User Rights", ddlUserName.SelectedItem.Text.ToString(), Convert.ToInt32(ddlUserName.SelectedValue), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    lblmsg.Visible = true;
                    PanelMsg.Visible = true;
                    lblmsg.Text = BL_UserRights.Msg;
                    lblmsg.ForeColor = System.Drawing.Color.Green;
                    BL_UserRights.Msg = "";
                     ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    result = true;
                    dgUserRights.DataSource = null;
                    dgUserRights.DataBind();
                    ddlFormName.SelectedIndex = 0;
                    ddlanUser.SelectedIndex = 0;
                    ddlUserName.SelectedIndex = 0;
                    chkMenu.Checked = false;
                    chkDelete.Checked = false;
                    chkBackDate.Checked = false;
                    chkAdd.Checked = false;
                    ChkCustCopy.Checked = false;
                    ChkAll.Checked = false;
                    chkPrint.Checked = false;
                    chkUpdate.Checked = false;
                    chkView.Checked = false;
                }
                else
                {
                    if (BL_UserRights.Msg != "")
                    {
                        lblmsg.Text = BL_UserRights.Msg;
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        BL_UserRights.Msg = "";
                    }
                }
            }
            else
            {
                BL_UserRights = new UserRights_BL(mlCode);

                if (BL_UserRights.Update(dgUserRights))
                {
                    CommonClasses.WriteLog("Admin User Rights", "Update", "Admin User Rights", ddlUserName.SelectedItem.Text.ToString(), Convert.ToInt32(ddlUserName.SelectedValue), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    lblmsg.Text = BL_UserRights.Msg;
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    BL_UserRights.Msg = "";
                    result = true;
                    dgUserRights.DataSource = null;
                    dgUserRights.DataBind();
                    ddlFormName.SelectedIndex = 0;
                }
                else
                {
                    if (BL_UserRights.Msg != "")
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Record Not Found In Table";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        BL_UserRights.Msg = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            if (ddlUserName.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select User Name";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (ChkCustCopy.Checked == true)
            {
                if (ddlanUser.SelectedIndex == 0)
                {
                    lblmsg.Text = "Please Select Form User Name";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (ChkAll.Checked == false)
            {
                if (ddlFormName.SelectedIndex == 0)
                {
                    lblmsg.Text = "Please Select Form Name Or All Forms";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            FillGrid(ddlUserName.SelectedValue.ToString(), ddlFormName.SelectedValue.ToString(), ddlModuleName.SelectedValue.ToString());
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "btnShow_Click", ex.Message);
        }
    }
    #endregion

    #region ddlDepartment_SelectedIndexChanged
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region ChkAll_CheckedChanged
    protected void ChkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkAll.Checked == true)
        {
            ddlFormName.Enabled = false;
            ddlFormName.SelectedIndex = 0;
        }
        else
        {
            ddlFormName.Enabled = true;
        }
    }
    #endregion

    #region chkAdd_CheckedChanged
    protected void chkAdd_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAdd.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkMenu_CheckedChanged
    protected void chkMenu_CheckedChanged(object sender, EventArgs e)
    {

        if (chkMenu.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = true;
            }
        }
        else
        {
            chkView.Checked = false;
            chkUpdate.Checked = false;
            chkAdd.Checked = false;
            chkDelete.Checked = false;
            chkPrint.Checked = false;
            ChkAll.Checked = false;
            chkBackDate.Checked = false;

            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkDelete_CheckedChanged
    protected void chkDelete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDelete.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = true;
            }

        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkView_CheckedChanged
    protected void chkView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkView.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = false;

            }
        }
    }
    #endregion

    #region chkPrint_CheckedChanged
    protected void chkPrint_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPrint.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkUpdate_CheckedChanged
    protected void chkUpdate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkUpdate.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkBackDate_CheckedChanged
    protected void chkBackDate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBackDate.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region chkSelectAll_CheckedChanged
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked == true)
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = true;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < dgUserRights.Rows.Count; i++)
            {
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkMenuDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkViewDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkUpdateDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkAddDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkDeleteDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkPrintDg"))).Checked = false;
                ((CheckBox)(dgUserRights.Rows[i].FindControl("chkBackDateDg"))).Checked = false;
            }
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("select UR_UM_CODE,UM_USERNAME from USER_MASTER,USER_RIGHT,SCREEN_MASTER where UM_CODE=UR_UM_CODE and UR_SM_CODE=SM_CODE and UR_IS_DELETE='0' and UR_UM_CODE='" + mlCode + "' group by UR_UM_CODE,UM_USERNAME");
            if (dt.Rows.Count > 0)
            {
                ddlUserName.Enabled = false;
                int a = Convert.ToInt32(dt.Rows[0]["UR_UM_CODE"].ToString());
                LoadUser();
                LoadUserCopyFrom();
                ddlUserName.SelectedValue = a.ToString();
            }
            if (str == "VIEW")
            {
                chkAdd.Enabled = false;
                ChkAll.Enabled = false;
                chkBackDate.Enabled = false;
                chkDelete.Enabled = false;
                chkPrint.Enabled = false;
                chkMenu.Enabled = false;
                chkSelectAll.Enabled = false;
                chkUpdate.Enabled = false;
                chkView.Enabled = false;
                ddlUserName.Enabled = false;
                ddlFormName.Enabled = false;
                btnShow.Enabled = false;
                btnSubmit.Visible = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion GetValues

    #region ddlModuleName_SelectedIndexChanged
    protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    protected void dgUserRights_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dgUserRights.DataSource = null;
            dgUserRights.DataBind();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "ddlUserName_SelectedIndexChanged", ex.Message);
        }
    }

    protected void ChkCustCopy_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkCustCopy.Checked == true)
            {
                ddlanUser.Enabled = true;
                ddlanUser.SelectedIndex = -1;
            }
            else
            {
                ddlanUser.Enabled = false;
                ddlanUser.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights", "ChkCustCopy_CheckedChanged", ex.Message);
        }
    }

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox thisCheckBox = (CheckBox)sender;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    protected void ddlModuleName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select SM_CODE,SM_NAME from SCREEN_MASTER WHERE SM_MOD_CODE='" + ddlModuleName.SelectedValue + "' order by SM_MOD_CODE,SM_NAME");
        ddlFormName.DataSource = dt;
        ddlFormName.DataTextField = "SM_NAME";
        ddlFormName.DataValueField = "SM_CODE";
        ddlFormName.DataBind();
        ddlFormName.Items.Insert(0, new ListItem("Select FormName", "0"));
        ddlFormName.SelectedIndex = 0;
    }
}