using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Net;

public class CommonClasses
{
    #region DataMembers
    DataSet ds = null;
    SqlParameter[] para = null;


    public const string MSG_Info = "AvisoInfo";
    public const string MSG_Ok = "AvisoOk";
    public const string MSG_Warning = "AvisoAviso";
    public const string MSG_Erro = "AvisoErro";

    public static string strRegInsSucesso = "Record Inserted Successfully.";
    public static string strRegAltSucesso = "Record Update Successfully.";
    public static string strRegDelSucesso = "Record Sucessfully Deleted.";
    public static string strRegModFlgSucesso = "Record Open by another person.";

    public static string machinename = Environment.MachineName;
    #endregion

    #region Constructor
    public CommonClasses()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region SetModifyLock
    public static bool SetModifyLock(string TableName, string ModField, string CodeField, int codeVal)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@TableName", TableName);
            par[1] = new SqlParameter("@ModField", ModField);
            par[2] = new SqlParameter("@CodeField", CodeField);
            par[3] = new SqlParameter("@codeVal", codeVal);
            res = DL_DBAccess.Insertion_Updation_Delete("SP_CM_SetModify", par);
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    #region CheckExistSave
    public static bool CheckExistSave(string TableName, string Field, string FieldVal)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@TableName", TableName);
            par[1] = new SqlParameter("@Field", Field);
            par[2] = new SqlParameter("@FieldVal", FieldVal);
            res = DL_DBAccess.Insertion_Updation_Delete("SP_CHECK_EXISTS_SAVE", par);
        }
        catch (Exception Ex)
        {
        }
        return res;

    }
    #endregion

    #region CheckExistUpdate
    public static bool CheckExistUpdate(string TableName, string Field, string FieldVal, string CodeField, int CodeVal)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@TableName", TableName);
            par[1] = new SqlParameter("@Field", Field);
            par[2] = new SqlParameter("@FieldVal", FieldVal);
            par[3] = new SqlParameter("@CodeField", CodeField);
            par[4] = new SqlParameter("@CodeVal", CodeVal);

            res = DL_DBAccess.Insertion_Updation_Delete("SP_CHECK_EXISTS_UPDATE", par);
        }
        catch (Exception Ex)
        {
        }
        return res;

    }
    #endregion

    #region RemoveModifyLock
    public static bool RemoveModifyLock(string TableName, string ModField, string CodeField, int codeVal)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@TableName", TableName);
            par[1] = new SqlParameter("@ModField", ModField);
            par[2] = new SqlParameter("@CodeField", CodeField);
            par[3] = new SqlParameter("@codeVal", codeVal);
            res = DL_DBAccess.Insertion_Updation_Delete("SP_CM_RemoveModify", par);
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    #region SaveAccountLedger
    public static bool SaveAccountLedger(string ACCL_CM_CODE, string ACCL_BM_CODE, string ACCL_DBM_CODE, string ACCL_LM_CODE, string ACCL_DOC_NO, string ACCL_DOC_NUMBER, string ACCL_DOC_TYPE, string ACCL_DOC_DATE, string ACCL_AMT, string ACCL_IS_ADJ, string ACCL_ADJ_AMT)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[11];
            par[0] = new SqlParameter("@ACCL_CM_CODE", ACCL_CM_CODE);
            par[1] = new SqlParameter("@ACCL_BM_CODE", ACCL_BM_CODE);
            par[2] = new SqlParameter("@ACCL_DBM_CODE", ACCL_DBM_CODE);
            par[3] = new SqlParameter("@ACCL_LM_CODE", ACCL_LM_CODE);
            par[4] = new SqlParameter("@ACCL_DOC_NO", ACCL_DOC_NO);
            par[5] = new SqlParameter("@ACCL_DOC_NUMBER", ACCL_DOC_NUMBER);
            par[6] = new SqlParameter("@ACCL_DOC_TYPE", ACCL_DOC_TYPE);
            par[7] = new SqlParameter("@ACCL_DOC_DATE", ACCL_DOC_DATE);
            par[8] = new SqlParameter("@ACCL_AMT", ACCL_AMT);
            par[9] = new SqlParameter("@ACCL_IS_ADJ", ACCL_IS_ADJ);
            par[10] = new SqlParameter("@ACCL_ADJ_AMT", ACCL_ADJ_AMT);

            res = DL_DBAccess.Insertion_Updation_Delete("SP_Insert_AccountLedger", par);
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    #region SaveAccountVoucherLedger
    public static bool SaveAccountVoucherLedger(string ACC_VCL_NO, string ACC_CM_CODE, string ACC_UM_CODE, string ACC_VCL_DATE, string ACC_VCL_EX_NO, string ACC_VCL_LM_CODE, string ACC_VCL_P_TYPE, string ACC_VCL_DBM_CODE, string ACC_VCL_AMT, string ACC_VCL_ADJ_AMT, string ACC_VCL_AMT_TEMP, string ACC_VCL_DOC_ID, string ACC_VCL_DOC_NO, string ACC_VCL_DOC_DATE, string ACC_VCL_DOC_TYPE, string ACC_VCL_CON_FLAG)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[16];
            par[0] = new SqlParameter("@ACC_VCL_NO", ACC_VCL_NO);
            par[1] = new SqlParameter("@ACC_CM_CODE", ACC_CM_CODE);
            par[2] = new SqlParameter("@ACC_UM_CODE", ACC_UM_CODE);
            par[3] = new SqlParameter("@ACC_VCL_DATE", ACC_VCL_DATE);
            par[4] = new SqlParameter("@ACC_VCL_EX_NO", ACC_VCL_EX_NO);
            par[5] = new SqlParameter("@ACC_VCL_LM_CODE", ACC_VCL_LM_CODE);
            par[6] = new SqlParameter("@ACC_VCL_P_TYPE", ACC_VCL_P_TYPE);
            par[7] = new SqlParameter("@ACC_VCL_DBM_CODE", ACC_VCL_DBM_CODE);
            par[8] = new SqlParameter("@ACC_VCL_AMT", ACC_VCL_AMT);
            par[9] = new SqlParameter("@ACC_VCL_ADJ_AMT", ACC_VCL_ADJ_AMT);
            par[10] = new SqlParameter("@ACC_VCL_AMT_TEMP", ACC_VCL_AMT_TEMP);
            par[11] = new SqlParameter("@ACC_VCL_DOC_ID", ACC_VCL_DOC_ID);
            par[12] = new SqlParameter("@ACC_VCL_DOC_NO", ACC_VCL_DOC_NO);
            par[13] = new SqlParameter("@ACC_VCL_DOC_DATE", ACC_VCL_DOC_DATE);
            par[14] = new SqlParameter("@ACC_VCL_DOC_TYPE", ACC_VCL_DOC_TYPE);
            par[15] = new SqlParameter("@ACC_VCL_CON_FLAG", ACC_VCL_CON_FLAG);

            res = DL_DBAccess.Insertion_Updation_Delete("SP_Insert_AccountVoucherLedger", par);
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    public static string ToLiteral(string input)
    {
        return input.Replace("'", "\''");
    }

    #region Execute
    public static DataTable Execute(string strQuery)
    {       
      DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", strQuery);
            dt = DL_DBAccess.SelectData("SP_CM_Execute", par);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
            return dt;
        }
        finally
        {
            dt.Dispose();
        }
    }
    #endregion

    #region Execute1
    public static bool Execute1(string strQuery)
    {

        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", strQuery);
            dt = DL_DBAccess.SelectData("SP_CM_Execute", par);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dt.Dispose();
        }
    }
    #endregion

    #region Encrypt
    public static string Encrypt(string Pwd)
    {
        int I = 0;
        int Len, Pos;
        Char ChrSt;
        //string EncryptText;
        string STR = "";
        Len = Pwd.Length;
        Pos = 0;
        for (I = 0; I < Len; I++)
        {
            if (I == Len)
            {
                break;
            }
            ChrSt = Pwd[I];
            int encript = Strings.Asc(ChrSt);
            encript = encript * 20;
            encript = encript / 2;
            encript = encript - 100;
            if (Pos == 0)
            {
                STR = STR + encript.ToString();
                Pos = Pos + 1;
            }
            else
            {
                STR = STR + "-" + encript.ToString();
            }
        }
        return STR;
    }
    //------------------------------------------Ddescript Text---------------------------------
    #endregion

    #region DecriptText
    public static string DecriptText(string password)
    {
        string finalvalue = "", DecryptText;
        string[] codeword = Regex.Split(password.Trim(), "-");

        for (int u = 0; u < codeword.Length; u++)
        {
            int ff = 0;
            ff = Convert.ToInt32(codeword[u]) + 100;
            ff = ff * 2;
            ff = ff / 20;
            DecryptText = Convert.ToString(Strings.ChrW(ff));
            finalvalue = finalvalue + DecryptText;
        }
        return finalvalue;

    }
    #endregion

    #region FillCombo
    public static DataTable FillCombo(string Tablename, string DispMember, string ValueMember, string Condition)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@TbName", Tablename);
            par[1] = new SqlParameter("@fname", DispMember);
            par[2] = new SqlParameter("@code", ValueMember);
            par[3] = new SqlParameter("@cond", Condition);
            dt = DL_DBAccess.SelectData("SP_CM_FillCombo", par);
        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion

    #region FillCombo
    public static DataTable FillCombo(string Tablename, string DispMember, string ValueMember, string Condition, object ddList)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@TbName", Tablename);
            par[1] = new SqlParameter("@fname", DispMember);
            par[2] = new SqlParameter("@code", ValueMember);
            par[3] = new SqlParameter("@cond", Condition);
            dt = DL_DBAccess.SelectData("SP_CM_FillCombo", par);
            ListControl ddl = null;
            ddl = ddList as DropDownList;
            ddl.DataSource = dt;
            ddl.DataTextField = DispMember;
            ddl.DataValueField = ValueMember;
            ddl.DataBind();
        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion

    #region ValidRights
    public static bool ValidRights(int N, Page page, string message)
    {
        if (N == 0)
        {
            //String Output;
            //Output = String.Format("alert('{0}');","You Have No Rights " + message);
            //page.ClientScript.RegisterStartupScript(page.GetType(), "Key", Output, true);
            //return false;
            page.ClientScript.RegisterStartupScript(page.GetType(), "regMostrarMensagem", "MostrarMensagem('#Avisos', 'You Have No Rights " + message + "', '" + CommonClasses.MSG_Warning + "');", true);
            return false;
        }
        return true;
    }
    #endregion

    #region WriteLog
    public static void WriteLog(string FormName, string Event, string DocName, string DocNo, int DocCode, int CompId, string UserName, int UserCode)
    {
        Execute("INSERT INTO LOG_MASTER(LG_CM_COMP_ID,LG_DATE,LG_SOURCE,LG_EVENT,LG_COMP_NAME,LG_DOC_NO,LG_DOC_NAME,LG_DOC_CODE,LG_U_NAME,LG_U_CODE,LG_IP_ADDRESS)VALUES(" + CompId + ",convert(varchar(23), getdate(), 121),'" + FormName + "','" + Event + "','" + machinename + "','" + DocNo + "','" + DocName + "','" + DocCode + "','" + UserName + "','" + UserCode + "','" + GetIP4Address() + "')");
    }
    public static void WriteLog(string FormName, string Event, string DocName, string DocNo, int DocCode, int CompId, int CompCode, string UserName, int UserCode)
    {
        Execute("INSERT INTO LOG_MASTER(LG_CM_CODE,LG_CM_COMP_ID,LG_DATE,LG_SOURCE,LG_EVENT,LG_COMP_NAME,LG_DOC_NO,LG_DOC_NAME,LG_DOC_CODE,LG_U_NAME,LG_U_CODE,LG_IP_ADDRESS)VALUES('" + CompCode + "'," + CompId + ",convert(varchar(23), getdate(), 121),'" + FormName + "','" + Event + "','" + machinename + "','" + DocNo + "','" + DocName + "','" + DocCode + "','" + UserName + "','" + UserCode + "','" + GetIP4Address() + "')");
    }
    #endregion

    #region GetMaxId
    public static string GetMaxId(string Qry)
    {
        string Maxid;
        DataTable dtMax = Execute(Qry);
        Maxid = dtMax.Rows.Count == 0 ? "" : dtMax.Rows[0][0].ToString();

        if (Maxid == "")
        {
            Maxid = "-2147483648";
        }
        else
        {
            Maxid = Convert.ToString(Convert.ToInt32(Maxid));
        }
        return Maxid;
    }
    #endregion

    #region BulkInsertDataTable
    public static bool BulkInsertDataTable(string tableName, DataTable dataTable)
    {
        bool isSuccuss;
        try
        {
            DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
            isSuccuss = DL_DBAccess.BulkInsertDataTable(tableName, dataTable);
        }
        catch (Exception ex)
        {
            isSuccuss = false;
        }
        return isSuccuss;
    }
    #endregion

    #region GetMaxId
    public static string GetMaxNO(string Qry)
    {
        string Maxid;
        DataTable dtMax = Execute(Qry);
        Maxid = dtMax.Rows.Count == 0 ? "" : dtMax.Rows[0][0].ToString();

        if (Maxid == "")
        {
            Maxid = "1";
        }
        else
        {
            Maxid = Convert.ToString(Convert.ToInt32(Maxid));
        }
        return Maxid;
    }
    #endregion

    #region GetIP4Address
    public static string GetIP4Address()
    {
        string IP4Address = String.Empty;
        try
        {
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
             
        }
        catch (Exception exc)
        {
            return IP4Address;
        }
    }
    #endregion

    #region SendError
    public static void SendError(string Module, string Function, string ErroeMsg)
    {
        HttpContext.Current.Response.Redirect("~/Masters/ADD/SendErrorReportPage.aspx?Module=" + Module + " &Function=" + Function + " &Message=" + ErroeMsg + "");
    }
    #endregion

    #region GenBillNo
    public static string GenBillNo(int BillNo)
    {
        string GenBillNo = "";
        if (BillNo.ToString().Length == 1)
        {
            GenBillNo = "0000" + BillNo.ToString();
        }
        else if (BillNo.ToString().Length == 2)
        {
            GenBillNo = "000" + BillNo.ToString();
        }
        else if (BillNo.ToString().Length == 3)
        {
            GenBillNo = "00" + BillNo.ToString();
        }
        else if (BillNo.ToString().Length == 4)
        {
            GenBillNo = "0" + BillNo.ToString();
        }
        else
        {
            GenBillNo = BillNo.ToString();
        }
        return GenBillNo;
    }
    #endregion

    #region CheckMainBranch
    public static bool CheckMainBranch(string BranchCode)
    {
        bool res = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            DataTable dtCheckMain = Execute("SELECT BM_IS_MAIN FROM CM_BRANCH_MASTER WHERE BM_CODE=" + BranchCode + "");
            if (dtCheckMain.Rows[0]["BM_IS_MAIN"].ToString().ToLower() == "true" || dtCheckMain.Rows[0]["BM_IS_MAIN"].ToString().ToLower() == "1")
            {
                res = true;
            }
            else
            {
                res = false;
            }
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    #region CheckUsedInTran
    public static bool CheckUsedInTran(string TableName, string FieldName, string FlgField, string Code)
    {
        bool res = false;
        try
        {
            DataTable dtCheckInTran = CommonClasses.Execute("SELECT * FROM " + TableName + " WHERE " + FieldName + "='" + Code + "' " + FlgField + "");
            if (dtCheckInTran.Rows.Count > 0)
            {
                res = true;
            }
            else
            {
                res = false;
            }
        }
        catch (Exception ex)
        {
        }
        return res;
    }
    #endregion

    public static void WriteLogActivity(string p, string p_2, string p_3, int p_4, string p_5, int p_6)
    {
        throw new NotImplementedException();
    }

    #region GetDoubleValue
    public static double GetDoubleValue(string input)
    {
        double val;
        if (!double.TryParse(input, out val))
        {
            return val;
        }
        return double.Parse(input);
    }
    #endregion
}
