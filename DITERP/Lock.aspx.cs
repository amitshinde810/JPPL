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

public partial class Lock : System.Web.UI.Page
{
    #region datamembers
    Login_BL BL_Login = new Login_BL();
   
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["Username"]) || string.IsNullOrEmpty((string)Session["UserCode"]))
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dt = CommonClasses.Execute("select UM_USERNAME,UM_NAME,UM_EMAIL from USER_MASTER where UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "'");
                if (dt.Rows.Count > 0)
                {
                    lblusername.Text = dt.Rows[0]["UM_USERNAME"].ToString();
                    lblreloginusername.Text = dt.Rows[0]["UM_NAME"].ToString();
                    lblmailId.Text = dt.Rows[0]["UM_EMAIL"].ToString();

                }
                //bool UAUpdate = CommonClasses.Execute1("update SMP_PETRO_USER_ACTIVITY  set SMP_UA_LO_DATE=getdate() where  SMP_UA_CODE='" + Session["UserActivityCode"].ToString() + "'");
                //if (UAUpdate)
                //{
                //    Session["UserActivityCode"] = null;
                //}
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
           // bool rslt = false;
            dt = BL_Login.VerifyLogin(Session["Username"].ToString(), CommonClasses.Encrypt(txtPassword.Text), Session["CompanyId"].ToString(), Session["CompanyCode"].ToString());
            if (dt.Rows.Count > 0)
            {              
               
                //rslt = CommonClasses.Execute1("insert into SMP_PETRO_USER_ACTIVITY (SMP_UA_U_CODE,SMP_UA_LI_DATE) values (" + Session["UserCode"].ToString() + ",getdate())");
                //if (rslt == true)
                //{
                //    DataTable dtUACode = CommonClasses.Execute("select max(SMP_UA_CODE) from SMP_PETRO_USER_ACTIVITY,SMP_PETRO_USER_MASTER where SMP_UA_U_CODE=SMP_U_CODE and SMP_UA_U_CODE='" + Session["UserCode"].ToString() + "'");
                //    if (dtUACode.Rows.Count > 0)
                //    {
                //        Session["UserActivityCode"] = dtUACode.Rows[0][0].ToString();
                //    }
                //}
                Response.Redirect("~/Masters/ADD/Dashboard.aspx");
            }
            else
            {
                txtPassword.Text = "";
                //lblmsg.Text = "Enter valid Username and Password";
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion
}
