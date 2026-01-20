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

public partial class Masters_ADD_SendErrorReportPage : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string ModName = Request.QueryString[0].ToString();
            string Function = Request.QueryString[1].ToString();
            string Message = Request.QueryString[2].ToString();
            lblModuleName.Text = ModName;
            lblFunctionName.Text = Function;
            lblError.Text = Message;
            lblError.ForeColor = System.Drawing.Color.Red;
            lblDateNtime.Text = DateTime.Now.ToString();
            lblIP.Text = CommonClasses.GetIP4Address();
        }
    }
    #endregion 

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/Dashboard.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Send Error Page", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
}
