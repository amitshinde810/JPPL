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


public partial class Masters_ADD_CityMaster : System.Web.UI.Page
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
                //LoadState();

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
                CommonClasses.SendError("City Master", "Page_Load", Ex.Message);
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
        txtCityName.Text = "";
        ddlCountry.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
        }
        Response.Redirect("~/Masters/VIEW/ViewCityMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtCityName.Text = "";
            ddlCountry.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "ClearFied", Ex.Message);
        }
    }
    #endregion

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
            CommonClasses.SendError("City Master", "LoadCountry", Ex.Message);

        }
    }
    #endregion

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadState();
    }

    #region Load State
    private void LoadState()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + " and STATE_MASTER.COUNTRY_CODE=" + ddlCountry.SelectedValue + "");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("------State-------", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("City Master", "LoadCity", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "LoadCity", ex.Message);
        }
    }
    #endregion Load State

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT CITY_CODE,CITY_NAME,CITY_SM_CODE,CITY_COUNTRY_CODE FROM CITY_MASTER WHERE  CITY_CODE=" + mlCode + " AND CITY_CM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["CITY_CODE"]); ;
                txtCityName.Text = dt.Rows[0]["CITY_NAME"].ToString();
                ddlCountry.SelectedValue = dt.Rows[0]["CITY_COUNTRY_CODE"].ToString();
                ddlCountry_SelectedIndexChanged(null, null);
                ddlState.SelectedValue = dt.Rows[0]["CITY_SM_CODE"].ToString();
                if (str == "VIEW")
                {
                    txtCityName.Enabled = false;
                    ddlCountry.Enabled = false;
                    ddlState.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "ViewRec", Ex.Message);
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
                dt = CommonClasses.Execute("Select CITY_CODE,CITY_NAME FROM CITY_MASTER WHERE lower(CITY_NAME)= lower('" + txtCityName.Text.Trim() + "') and ES_DELETE='False'");
                //result = CommonClasses.CheckExistSave("PARTY_MASTER", "P_NAME",txtCustomerName.Text.Trim());
                if (dt.Rows.Count == 0)
                {

                    if (CommonClasses.Execute1("INSERT INTO CITY_MASTER (CITY_CM_COMP_ID,CITY_NAME,CITY_SM_CODE,CITY_COUNTRY_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtCityName.Text + "','" + ddlState.SelectedValue + "','" + ddlCountry.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CITY_CODE) from CITY_MASTER");
                        CommonClasses.WriteLog("City Master", "Save", "City Master", txtCityName.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewCityMaster.aspx", false);
                    }
                    else
                    {

                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);

                        txtCityName.Focus();
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
                dt = CommonClasses.Execute("SELECT * FROM CITY_MASTER WHERE ES_DELETE='FALSE' AND CITY_CODE != '" + mlCode + "' AND lower(CITY_NAME) = lower('" + txtCityName.Text + "')");
                //result = CommonClasses.CheckExistUpdate("PARTY_MASTER", "P_NAME", txtCustomerName.Text, "P_CODE", mlCode);
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE CITY_MASTER SET CITY_NAME='" + txtCityName.Text + "',CITY_SM_CODE='" + ddlState.SelectedValue + "',CITY_COUNTRY_CODE='" + ddlCountry.SelectedValue + "' WHERE CITY_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
                        CommonClasses.WriteLog("City Master", "Update", "City Master", txtCityName.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewCityMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);

                        txtCityName.Focus();
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
            CommonClasses.SendError("City Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("City Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    

    

}
