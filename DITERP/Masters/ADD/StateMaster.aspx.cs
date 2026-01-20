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

public partial class Masters_ADD_StateMaster : System.Web.UI.Page
{
    #region Var
    static int mlCode = 0;
   
    static string right = "";
    #endregion
    //
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='127'");
                // right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                LoadCountry();

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
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("State Master", "Page_Load", Ex.Message);
            }
        }
    }
    #endregion


    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
        //{
            SaveRec();
        //}
    }
    #endregion

   
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtStateName.Text = "";
        ddlCountry.SelectedIndex = 0;
        if ( mlCode != 0 &&  mlCode != null)
        {
            CommonClasses.RemoveModifyLock("STATE_MASTER", "SM_MODIFY_FLAG", "SM_CODE",  mlCode);
        }
        Response.Redirect("~/Masters/VIEW/ViewStateMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtStateName.Text = "";
            ddlCountry.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "ClearFied", Ex.Message);
        }
    }
    #endregion


    //newly Added 


    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID='1'");
                ddlCountry.DataSource = dt;
                ddlCountry.DataTextField = "COUNTRY_NAME";
                ddlCountry.DataValueField = "COUNTRY_CODE";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("------Country-------", "0"));

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "LoadCountry", Ex.Message);
       
        }
    }
    #endregion


    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_CODE FROM STATE_MASTER WHERE  SM_CODE=" + mlCode + " AND SM_CM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["SM_CODE"]); ;
                txtStateName.Text=dt.Rows[0]["SM_NAME"].ToString();               
                ddlCountry.SelectedValue=dt.Rows[0]["COUNTRY_CODE"].ToString();
                
                if (str == "VIEW")
                {
                    txtStateName.Enabled=false;
                    ddlCountry.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {


            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select SM_CODE,SM_NAME FROM STATE_MASTER WHERE lower(SM_NAME)= lower('" + txtStateName.Text.Trim() + "') and ES_DELETE='False'");
                //result = CommonClasses.CheckExistSave("PARTY_MASTER", "P_NAME",txtCustomerName.Text.Trim());
                if (dt.Rows.Count == 0)
                {

                    if (CommonClasses.Execute1("INSERT INTO STATE_MASTER (SM_CM_COMP_ID,SM_NAME,COUNTRY_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtStateName.Text + "','" + ddlCountry.SelectedValue  + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SM_CODE) from STATE_MASTER");
                        CommonClasses.WriteLog("State Master", "Save", "State Master", txtStateName.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewStateMaster.aspx", false);
                    }
                    else
                    {

                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);

                        txtStateName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM STATE_MASTER WHERE ES_DELETE='FALSE' AND SM_CODE != '" + mlCode + "' AND lower(SM_NAME) = lower('" + txtStateName.Text + "')");
                //result = CommonClasses.CheckExistUpdate("PARTY_MASTER", "P_NAME", txtCustomerName.Text, "P_CODE", mlCode);
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE STATE_MASTER SET SM_NAME='" + txtStateName.Text + "',COUNTRY_CODE='" + ddlCountry.SelectedValue + "' WHERE SM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
                        CommonClasses.WriteLog("Party Master", "Update", "Party Master", txtStateName.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewStateMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);

                        txtStateName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("State Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    

    

   
}
