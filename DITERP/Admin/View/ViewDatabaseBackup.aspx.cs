using System;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_View_ViewDatabaseBackup : System.Web.UI.Page
{
    # region Variables   
    static int mlCode = 0;
    static string right = "";
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
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Database Backup", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region BackupDatabase
    public bool BackupDatabase()
    {
        bool flag = false;
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ConnectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            string database = builder.InitialCatalog;
            string dbPass = builder.Password;
            string dbUser = builder.UserID;

            flag = CommonClasses.Execute1("Backup database " + database + " to disk = 'D:\\DIT\\BACKUP\\" + System.DateTime.Now.ToString("dd-MMM-yyyy HHmm") + ".bak' ");
        

            //var dbPath = Server.MapPath("~/App_Data/Utility/SimyaBackup.bat");
            //var dir = Server.MapPath("~/App_Data/Utility/");
            //var dirForBak = Server.MapPath("~/App_Data/Utility/7z.exe");
            //var BackupPath = Server.MapPath("~/App_Data/Utility/Backup/");
            //var BackupPathForsql = Server.MapPath("~/App_Data/Utility/Backup/");
            //string name = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            //FileInfo file = new FileInfo(dir + "SimyaBackup.bat");
            //FileInfo file1 = new FileInfo(dir + "SimyaBackup.sql");
            //if (file.Exists)
            //{
            //    file.Delete();
            //}
            //string connectionString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ConnectionString;

            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            //string database = builder.InitialCatalog;
            //string dbPass = builder.Password;
            //string dbUser=builder.UserID;           
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "BackupDatabase", ex.Message.ToString());
        }
        return flag;
    }

    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (BackupDatabase())
        {
            ShowMessage("#Avisos", "Backup Processed", CommonClasses.MSG_Warning);
            return;
        }
        else
        {
            ShowMessage("#Avisos", "Backup Not Processed", CommonClasses.MSG_Warning);
            return;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Database Backup", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
