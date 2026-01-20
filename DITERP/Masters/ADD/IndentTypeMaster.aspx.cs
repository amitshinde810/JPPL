using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;
public partial class Masters_ADD_IndentTypeMaster : System.Web.UI.Page
{
    #region variable
    DatabaseAccessLayer DB_Access;
    DataTable dt;
    static DataTable dt1 = new DataTable();
    public static string str = "";
    public static int index = 0;
    public static int rowind = 0;
    static string right = "";
    static int mlCode = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='1410'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    ViewState["index"] = index;
                    ViewState["dt1"] = dt1;
                    ViewState["IM_CODE"] = mlCode;

                    LoadUser();

                    if (Request.QueryString[0].Equals("modify"))
                    {
                        ViewState["IM_CODE"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        vs("MOD");
                    }
                    else if (Request.QueryString[0].Equals("View"))
                    {
                        btnSubmit.Enabled = false;
                        ViewState["IM_CODE"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        vs("VIEW");
                    }
                }
            }

        }
        catch (Exception)
        {

            throw;
        }

    }
    //Disable the control when view the data
    #region viewdata
    public void viewdata()
    {
        txtIndesc.Enabled = false;
        txtInShName.Enabled = false;

    }
    #endregion

    #region LoadUser
    private void LoadUser()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("SELECT UM_CODE,UM_NAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID=" + Session["CompanyId"] + "  ORDER BY UM_NAME");
                ddlApproval1.DataSource = dt;
                ddlApproval1.DataTextField = "UM_NAME";
                ddlApproval1.DataValueField = "UM_CODE";
                ddlApproval1.DataBind();
                ddlApproval1.Items.Insert(0, new ListItem("Select User Name", "0"));

                ddlApproval2.DataSource = dt;
                ddlApproval2.DataTextField = "UM_NAME";
                ddlApproval2.DataValueField = "UM_CODE";
                ddlApproval2.DataBind();
                ddlApproval2.Items.Insert(0, new ListItem("Select User Name", "0"));

                ddlApproval3.DataSource = dt;
                ddlApproval3.DataTextField = "UM_NAME";
                ddlApproval3.DataValueField = "UM_CODE";
                ddlApproval3.DataBind();
                ddlApproval3.Items.Insert(0, new ListItem("Select User Name", "0"));
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

    //Bind the dropdown list of  Machine Type from MACHINE_TYPE table

    //View and modify the record
    #region vs
    public void vs(string Type)
    {
        dt = new DataTable();
        dt = CommonClasses.Execute("select IM_SHORT,IM_DESC,ISNULL(IM_APPROVAL1,0) AS IM_APPROVAL1,ISNULL(IM_APPROVAL2,0) AS IM_APPROVAL2,ISNULL(IM_APPROVAL3,0) AS IM_APPROVAL3 , ISNULL(IM_APPROVDBY1,0) AS IM_APPROVDBY1, ISNULL(IM_APPROVDBY2,0) AS IM_APPROVDBY2, ISNULL(IM_APPROVDBY3,0) AS IM_APPROVDBY3 from INDENT_TYPE_MASTER where IM_CODE='" + ViewState["IM_CODE"] + "'");
        if (dt.Rows.Count > 0)
        {

            txtInShName.Text = dt.Rows[0]["IM_SHORT"].ToString();
            txtIndesc.Text = dt.Rows[0]["IM_DESC"].ToString();
            chkindent1.Checked = Convert.ToBoolean(dt.Rows[0]["IM_APPROVAL1"].ToString());
            chkindent2.Checked = Convert.ToBoolean(dt.Rows[0]["IM_APPROVAL2"].ToString());
            chkindent3.Checked = Convert.ToBoolean(dt.Rows[0]["IM_APPROVAL3"].ToString());

            ddlApproval1.Text = dt.Rows[0]["IM_APPROVDBY1"].ToString();
            ddlApproval2.Text = dt.Rows[0]["IM_APPROVDBY2"].ToString();
            ddlApproval3.Text = dt.Rows[0]["IM_APPROVDBY3"].ToString();
        }
        if (Type == "VIEW")
        {
            viewdata();
        }
        if (str == "modify")
        {
            CommonClasses.SetModifyLock("INDENT_TYPE_MASTER", "MODIFY", "IM_CODE", (Int32)ViewState["IM_CODE"]);
        }

    }
    #endregion

    //Save click to Save the record in database
    #region btnSave_Click
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtIndesc.Text.Trim() == "" || txtIndesc.Text == null || txtIndesc.Text == String.Empty)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter indent type description";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }

        if (txtInShName.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter indent short name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        else
        {
            SaveRec();
        }


    }
    #endregion


    public void SaveRec()
    {
        try
        {
            if (Request.QueryString[0].Equals("insert"))
            {
                if (selectdata())
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Indent name already exist";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
                else
                {
                    dt = new DataTable();
                    string Code = CommonClasses.GetMaxId("Select Max(IM_CODE) from INDENT_TYPE_MASTER");
                    dt = CommonClasses.Execute("insert into INDENT_TYPE_MASTER(IM_SHORT,IM_DESC,IM_CM_ID,IM_APPROVAL1,IM_APPROVAL2,IM_APPROVAL3,IM_APPROVDBY1,IM_APPROVDBY2,IM_APPROVDBY3) values('" + txtInShName.Text.Trim().Replace("'", "''") + "','" + txtIndesc.Text.Trim() + "','" + Session["CompanyId"].ToString() + "','" + chkindent1.Checked + "','" + chkindent2.Checked + "','" + chkindent2.Checked + "','" + ddlApproval1.SelectedValue + "','" + ddlApproval2.SelectedValue + "','" + ddlApproval3.SelectedValue + "') ");
                    string shname = txtInShName.Text;
                    CommonClasses.WriteLog("Indent Type Master", "Save", "Indent Type Master", shname, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    Response.Redirect("~/Masters/VIEW/IndentTypeMaster.aspx", false);
                }
            }
            if (Request.QueryString[0].Equals("modify"))
            {
                if (selectdata())
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Indent name already exist";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
                else
                {
                    selectdata();
                    dt = new DataTable();
                    dt = CommonClasses.Execute("update INDENT_TYPE_MASTER set IM_SHORT='" + txtInShName.Text.Trim().Replace("'", "''") + "', IM_DESC='" + txtIndesc.Text.Trim().Replace("'", "''") + "',IM_APPROVAL1='" + chkindent1.Checked + "',IM_APPROVAL2='" + chkindent2.Checked + "',IM_APPROVAL3='" + chkindent3.Checked + "'  ,IM_APPROVDBY1='" + ddlApproval1.SelectedValue + "' ,IM_APPROVDBY2='" + ddlApproval2.SelectedValue + "' ,IM_APPROVDBY3='" + ddlApproval3.SelectedValue + "'    where IM_CODE='" + ViewState["IM_CODE"] + "'");
                    CommonClasses.RemoveModifyLock("INDENT_TYPE_MASTER", "MODIFY", "IM_CODE", (Int32)ViewState["IM_CODE"]);
                    CommonClasses.WriteLog("Indent Type Master", "Update", "Indent Type Master", ViewState["IM_CODE"].ToString(), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Updated Successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    Response.Redirect("~/Masters/VIEW/IndentTypeMaster.aspx");
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    //This Method is use for to check the record Already exist or not 
    #region selectdata
    public bool selectdata()
    {
        dt = new DataTable();
        if (Request.QueryString[0].Equals("modify"))
            dt = CommonClasses.Execute("select IM_SHORT from INDENT_TYPE_MASTER where IM_SHORT='" + txtInShName.Text.Trim() + "' AND  IM_CODE!='" + ViewState["IM_CODE"].ToString() + "' and ES_DELETE=0");
        else
            dt = CommonClasses.Execute("select IM_SHORT from INDENT_TYPE_MASTER where IM_SHORT='" + txtInShName.Text.Trim() + "' and ES_DELETE=0");

        if (dt.Rows.Count > 0)
        {
            if (txtInShName.Text.Trim() != "")
            {

                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }
    #endregion

    //When View the record and then click on cancel then jump on the viewpage of the Machine master page
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("View"))
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
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Type Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    //This method is use for cancel the record to save or modify
    #region CancelRecord
    private void CancelRecord()
    {
        int im_code = Convert.ToInt32(ViewState["IM_CODE"]);
        try
        {
            if (im_code != 0 && im_code != null)
            {
                CommonClasses.RemoveModifyLock("INDENT_TYPE_MASTER", "MODIFY", "IM_CODE", im_code);
            }
            Response.Redirect("~/Masters/VIEW/IndenttypeMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Type Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    //This button click is popup window for asking cancel the record yes 
    #region btnYes_Click
    protected void btnYes_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnNo_Click
    protected void btnNo_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    //When click on cross sign of the machine master page then jump onthe View page of the machine master
    #region btnclose_Click
    protected void btnclose_Click(object sender, EventArgs e)
    {
        PanelMsg.Visible = false;
        CancelRecord();
    }
    #endregion

    //This method is use for modify record cancel or not
    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtInShName.Text.Trim() == String.Empty)
            {
                flag = false;
            }
            else if (txtInShName.Text.Trim() == String.Empty)
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
            CommonClasses.SendError("Indent Type Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion
}
