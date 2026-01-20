using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for MonthlyTransaction_BL
/// </summary>
/// 

public class MonthlyAttendance_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Variables"
    private int _MT_CODE;
    private int _MT_MONTH;
    private int _MT_YEAR;
    private int _MT_CM_ID;
    private int _MT_BM_CODE;

    private string _EM_TEMP_LEFT_DATE;
    #endregion

    #region  "Propertey"

    #region MT_CODE
    public int MT_CODE
    {
        get { return _MT_CODE; }
        set { _MT_CODE = value; }
    }
    #endregion

    #region MT_MONTH
    public int MT_MONTH
    {
        get { return _MT_MONTH; }
        set { _MT_MONTH = value; }
    }
    #endregion

    #region MT_YEAR
    public int MT_YEAR
    {
        get { return _MT_YEAR; }
        set { _MT_YEAR = value; }
    }
    #endregion

    #region MT_CM_ID
    public int MT_CM_ID
    {
        get { return _MT_CM_ID; }
        set { _MT_CM_ID = value; }
    }
    #endregion

    #region MT_BM_CODE
    public int MT_BM_CODE
    {
        get { return _MT_BM_CODE; }
        set { _MT_BM_CODE = value; }
    }
    #endregion

    #region EM_TEMP_LEFT_DATE
    public string EM_TEMP_LEFT_DATE
    {
        get { return _EM_TEMP_LEFT_DATE; }
        set { _EM_TEMP_LEFT_DATE = value; }
    }
    #endregion

    #endregion

    #region Costructor
    public MonthlyAttendance_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Prameterise Costructor
    public MonthlyAttendance_BL(int Id)
    {
        _MT_CODE = Id;
    }
    #endregion

    #region GetInfo
    public void GetInfo(GridView XGrid, DataTable dtMainLeaves, DataTable dtMainCOff)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@MT_MONTH", MT_MONTH);
            par[1] = new SqlParameter("@MT_YEAR", MT_YEAR);
            par[2] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
            dt = DL_DBAccess.SelectData("SP_HR_GetInfoMonthAttend", par);
            if (dt.Rows.Count > 0)
            {
                XGrid.DataSource = dt;
                XGrid.DataBind();
            }
            dtMainLeaves = CommonClasses.Execute("SELECT TMLD_EM_CODE,TMLD_DATE,TMLD_LEAVE_NAME,TMLD_LEAVE_DAY,TMLD_MT_CODE FROM HR_TEMP_MONTHLY_LEAVEDETAIL,HR_MONTHLY_TRANSACTION WHERE MT_CODE=TMLD_MT_CODE AND MT_MONTH=" + MT_MONTH + " and MT_YEAR=" + MT_YEAR + " and MT_CM_ID=" + MT_CM_ID + "");
            dt1 = CommonClasses.Execute("select MTC_EM_CODE as EM_CODE,REPLACE(CONVERT(VARCHAR(11), MTC_COFF_DATE, 106), ' ', '/') as COffDate,REPLACE(CONVERT(VARCHAR(11), MTC_COFF_TAKEN_DATE, 106), ' ', '/') as COffTakeDate,MTC_COM_CODE as COM_CODE,MTC_COFF_DAY,MTC_COFF_TAKEN_DAY ,MTC_MT_CODE from HR_MONTHLY_TRANSACTION_COFF_DETAIL,HR_MONTHLY_TRANSACTION where MT_CODE=MTC_MT_CODE and MT_MONTH=" + MT_MONTH + " and MT_YEAR=" + MT_YEAR + " and MT_CM_ID=" + MT_CM_ID + "");
            if (dtMainCOff.Columns.Count == 0)
            {
                dtMainCOff.Columns.Add(new System.Data.DataColumn("EM_CODE", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("AllFlag", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("COffDate", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("COffTakeDate", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("COM_CODE", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("COffDay", typeof(String)));
                dtMainCOff.Columns.Add(new System.Data.DataColumn("COffTakenDay", typeof(String)));
            }
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    dtMainCOff.Rows.Add(dt1.Rows[i]["EM_CODE"].ToString(), "True", dt1.Rows[i]["COffDate"].ToString(), dt1.Rows[i]["COffTakeDate"].ToString(), dt1.Rows[i]["COM_CODE"].ToString(), dt1.Rows[i]["MTC_COFF_DAY"].ToString(), dt1.Rows[i]["MTC_COFF_TAKEN_DAY"].ToString());
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@MT_MONTH", MT_MONTH);
            par[1] = new SqlParameter("@MT_YEAR", MT_YEAR);
            par[2] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
            dt = DL_DBAccess.SelectData("SP_HR_CheckSaveMonth", par);
            if (dt.Rows.Count > 0)
                res = false;
            else
                res = true;
        }
        catch (Exception Ex)
        {
        }
        return res;
    }
    #endregion

    #region Save
    public bool Save(GridView XGrid, DataTable dtMainLeaves, DataTable dtMainCOff)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (CheckExistSaveName())
            {
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {
                    string Pdays = ((TextBox)(XGrid.Rows[i].FindControl("txtPreDays"))).Text;
                    string EM_Code = ((Label)(XGrid.Rows[i].FindControl("lblEMPCODE"))).Text;
                    string PH = ((TextBox)(XGrid.Rows[i].FindControl("txtPH"))).Text;
                    string woff = ((TextBox)(XGrid.Rows[i].FindControl("txtWO"))).Text;
                    string CL = ((TextBox)(XGrid.Rows[i].FindControl("txtCL"))).Text;
                    string SL = ((TextBox)(XGrid.Rows[i].FindControl("txtSL"))).Text;
                    string EL = ((TextBox)(XGrid.Rows[i].FindControl("txtEarningLeave"))).Text;
                    string UL = ((TextBox)(XGrid.Rows[i].FindControl("lblUL"))).Text;
                    string Days = ((Label)(XGrid.Rows[i].FindControl("lblDays"))).Text;
                    string PayDays = ((Label)(XGrid.Rows[i].FindControl("lblPayDays"))).Text;
                    string OT = ((TextBox)(XGrid.Rows[i].FindControl("txtOT"))).Text;
                    bool Bonus = ((CheckBox)(XGrid.Rows[i].FindControl("ChkBonus"))).Checked;
                    bool LTA = ((CheckBox)(XGrid.Rows[i].FindControl("ChkLTA"))).Checked;
                    bool isHold = ((CheckBox)(XGrid.Rows[i].FindControl("ChkSalaryHold"))).Checked;
                    string COff = ((TextBox)(XGrid.Rows[i].FindControl("txtCOff"))).Text;
                   
                    SqlParameter[] par = new SqlParameter[18];
                    par[0] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
                    //par[1] = new SqlParameter("@MT_BM_CODE", MT_BM_CODE);
                    par[1] = new SqlParameter("@MT_EM_CODE", EM_Code);
                    par[2] = new SqlParameter("@MT_MONTH", MT_MONTH);
                    par[3] = new SqlParameter("@MT_YEAR", MT_YEAR);
                    par[4] = new SqlParameter("@MT_PAID_HOLIDAYS", PH);
                    par[5] = new SqlParameter("@MT_PDAYS", Pdays);
                    par[6] = new SqlParameter("@MT_WOFF", woff);
                    par[7] = new SqlParameter("@MT_CL", CL);
                    par[8] = new SqlParameter("@MT_SL", SL);
                    par[9] = new SqlParameter("@MT_EL", EL);
                    par[10] = new SqlParameter("@MT_UL", UL);
                    par[11] = new SqlParameter("@MT_DAYS", Days);
                    par[12] = new SqlParameter("@MT_PAYBLEDAYS", PayDays);
                    par[13] = new SqlParameter("@MT_OT_HRS", OT);
                    par[14] = new SqlParameter("@MT_BONUS_FLAG", Bonus);
                    par[15] = new SqlParameter("@MT_MED_LTA_FLAG", LTA);
                    par[16] = new SqlParameter("@MT_IS_HOLD", isHold);
                    par[17] = new SqlParameter("@MT_COFF", COff);
                   
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthAttend", par);
                    if (result)
                    {
                        string query = "Select max(MT_CODE) as Code from HR_MONTHLY_TRANSACTION where MT_CM_ID=" + MT_CM_ID + " and MT_DELETE_FLAG=0";
                        SqlParameter[] par1 = new SqlParameter[1];
                        par1[0] = new SqlParameter("@Query", query);
                        DataTable dtCode = DL_DBAccess.SelectData("SP_MaxID", par1);
                        if (dtCode.Rows.Count > 0)
                            MT_CODE = Convert.ToInt32(dtCode.Rows[0]["Code"].ToString());
                        if (SaveDetail(dtMainLeaves, EM_Code))
                        {
                            if (SaveCOffDetail(dtMainCOff, EM_Code))
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                Msg = "Month Year Already Exist";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Monthly Attendance BL", "Save", ex.Message.ToString());
        }
        return result;
    }
    #endregion

    #region SaveDetail
    bool SaveDetail(DataTable dtMainLeaves, string EM_Code)
    {
        bool result = false;
        DataTable Dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (dtMainLeaves.Rows.Count > 0)
            {
                for (int i = 0; i < dtMainLeaves.Rows.Count; i++)
                {
                    if (dtMainLeaves.Rows[i]["EM_CODE"].ToString() == EM_Code)
                    {
                        SqlParameter[] par = new SqlParameter[5];
                        par[0] = new SqlParameter("@TMLD_EM_CODE", dtMainLeaves.Rows[i]["EM_CODE"].ToString());
                        par[1] = new SqlParameter("@TMLD_DATE", dtMainLeaves.Rows[i]["Date"].ToString());
                        par[2] = new SqlParameter("@TMLD_LEAVE_NAME", dtMainLeaves.Rows[i]["Leave_Name"].ToString());
                        par[3] = new SqlParameter("@TMLD_MT_CODE", MT_CODE);
                        par[4] = new SqlParameter("@TMLD_LEAVE_DAY", dtMainLeaves.Rows[i]["LeaveDay"].ToString());
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertLeaveDetail", par);
                        if (result)
                        {
                            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE) values(" + MT_CM_ID + "," + MT_CODE + "," + dtMainLeaves.Rows[i]["EM_CODE"].ToString() + ",-(" + dtMainLeaves.Rows[i]["LeaveDay"].ToString() + "),'" + dtMainLeaves.Rows[i]["Leave_Name"].ToString() + "','Leave Taken','" + dtMainLeaves.Rows[i]["Date"].ToString() + "')");
                        }
                    }
                }
            }
            else
            {
                result = true;
            }
        }
        catch { }
        return result;
    }
    #endregion

    #region SaveCOffDetail
    bool SaveCOffDetail(DataTable dtMainCOff, string EM_Code)
    {
        bool result = false;
        DataTable Dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (dtMainCOff.Rows.Count > 0)
            {
                for (int i = 0; i < dtMainCOff.Rows.Count; i++)
                {
                    if (dtMainCOff.Rows[i]["EM_CODE"].ToString() == EM_Code)
                    {
                        SqlParameter[] par = new SqlParameter[7];
                        par[0] = new SqlParameter("@MTC_MT_CODE", MT_CODE);
                        par[1] = new SqlParameter("@MTC_EM_CODE", dtMainCOff.Rows[i]["EM_CODE"].ToString());
                        par[2] = new SqlParameter("@MTC_COM_CODE", dtMainCOff.Rows[i]["COM_CODE"].ToString());
                        par[3] = new SqlParameter("@MTC_COFF_DATE", dtMainCOff.Rows[i]["COffDate"].ToString());
                        par[4] = new SqlParameter("@MTC_COFF_TAKEN_DATE", dtMainCOff.Rows[i]["COffTakeDate"].ToString());
                        par[5] = new SqlParameter("@MTC_COFF_DAY", dtMainCOff.Rows[i]["COffDay"].ToString());
                        par[6] = new SqlParameter("@MTC_COFF_TAKEN_DAY", dtMainCOff.Rows[i]["COffTakenDay"].ToString());
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthlyCOffDetail", par);
                    }
                }
            }
            else
            {
                result = true;
            }
        }
        catch { }
        return result;
    }
    #endregion

    #region Update
    public bool Update(GridView XGrid, DataTable dtMainLeaves, DataTable dtMainCOff)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select MT_CODE FROM HR_MONTHLY_TRANSACTION WHERE MT_CM_ID=" + MT_CM_ID + " AND MT_MONTH=" + MT_MONTH + " AND MT_YEAR=" + MT_YEAR + "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CommonClasses.Execute("DELETE FROM HR_TEMP_MONTHLY_LEAVEDETAIL WHERE TMLD_MT_CODE=" + dt.Rows[i]["MT_CODE"].ToString() + "");
                    CommonClasses.Execute("DELETE FROM HR_MONTHLY_TRANSACTION_COFF_DETAIL WHERE MTC_MT_CODE=" + dt.Rows[i]["MT_CODE"].ToString() + "");
                }
            }
            CommonClasses.Execute("DELETE FROM HR_MONTHLY_TRANSACTION WHERE MT_CM_ID=" + MT_CM_ID + " AND MT_MONTH=" + MT_MONTH + " AND MT_YEAR=" + MT_YEAR + " --and MT_BM_CODE=" + MT_BM_CODE + "");
            for (int i = 0; i < XGrid.Rows.Count; i++)
            {
                string EM_Code = ((Label)(XGrid.Rows[i].FindControl("lblEMPCODE"))).Text;
                string PH = ((TextBox)(XGrid.Rows[i].FindControl("txtPH"))).Text;
                string Pdays = ((TextBox)(XGrid.Rows[i].FindControl("txtPreDays"))).Text;
                string woff = ((TextBox)(XGrid.Rows[i].FindControl("txtWO"))).Text;
                string CL = ((TextBox)(XGrid.Rows[i].FindControl("txtCL"))).Text;
                string SL = ((TextBox)(XGrid.Rows[i].FindControl("txtSL"))).Text;
                string EL = ((TextBox)(XGrid.Rows[i].FindControl("txtEarningLeave"))).Text;
                string UL = ((TextBox)(XGrid.Rows[i].FindControl("lblUL"))).Text;
                string Days = ((Label)(XGrid.Rows[i].FindControl("lblDays"))).Text;
                string PayDays = ((Label)(XGrid.Rows[i].FindControl("lblPayDays"))).Text;
                string OT = ((TextBox)(XGrid.Rows[i].FindControl("txtOT"))).Text;
                bool Bonus = ((CheckBox)(XGrid.Rows[i].FindControl("ChkBonus"))).Checked;
                bool LTA = ((CheckBox)(XGrid.Rows[i].FindControl("ChkLTA"))).Checked;
                bool isHold = ((CheckBox)(XGrid.Rows[i].FindControl("ChkSalaryHold"))).Checked;
                string COff = ((TextBox)(XGrid.Rows[i].FindControl("txtCOff"))).Text;
              
                SqlParameter[] par = new SqlParameter[18];
                par[0] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
                //par[1] = new SqlParameter("@MT_BM_CODE", MT_BM_CODE);
                par[1] = new SqlParameter("@MT_EM_CODE", EM_Code);
                par[2] = new SqlParameter("@MT_MONTH", MT_MONTH);
                par[3] = new SqlParameter("@MT_YEAR", MT_YEAR);
                par[4] = new SqlParameter("@MT_PAID_HOLIDAYS", PH);
                par[5] = new SqlParameter("@MT_PDAYS", Pdays);
                par[6] = new SqlParameter("@MT_WOFF", woff);
                par[7] = new SqlParameter("@MT_CL", CL);
                par[8] = new SqlParameter("@MT_SL", SL);
                par[9] = new SqlParameter("@MT_EL", EL);
                par[10] = new SqlParameter("@MT_UL", UL);
                par[11] = new SqlParameter("@MT_DAYS", Days);
                par[12] = new SqlParameter("@MT_PAYBLEDAYS", PayDays);
                par[13] = new SqlParameter("@MT_OT_HRS", OT);
                par[14] = new SqlParameter("@MT_BONUS_FLAG", Bonus);
                par[15] = new SqlParameter("@MT_MED_LTA_FLAG", LTA);
                par[16] = new SqlParameter("@MT_IS_HOLD", isHold);
                par[17] = new SqlParameter("@MT_COFF", COff);
               
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthAttend", par);
                if (result)
                {
                    string query = "Select max(MT_CODE) as Code from HR_MONTHLY_TRANSACTION where MT_CM_ID=" + MT_CM_ID + " and MT_DELETE_FLAG=0";
                    SqlParameter[] par1 = new SqlParameter[1];
                    par1[0] = new SqlParameter("@Query", query);
                    DataTable dtCode = DL_DBAccess.SelectData("SP_MaxID", par1);
                    if (dtCode.Rows.Count > 0)
                        MT_CODE = Convert.ToInt32(dtCode.Rows[0]["Code"].ToString());
                    if (SaveDetail(dtMainLeaves, EM_Code))
                    {
                        if (SaveCOffDetail(dtMainCOff, EM_Code))
                        {
                            result = true;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return result;
    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select MT_CODE FROM HR_MONTHLY_TRANSACTION WHERE MT_CM_ID=" + MT_CM_ID + " AND MT_MONTH=" + MT_MONTH + " AND MT_YEAR=" + MT_YEAR + " and MT_DELETE_FLAG=0");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CommonClasses.Execute("DELETE FROM HR_TEMP_MONTHLY_LEAVEDETAIL WHERE TMLD_MT_CODE=" + dt.Rows[i]["MT_CODE"].ToString() + "");
                    CommonClasses.Execute("DELETE FROM HR_LEAVE_TRANSACTION WHERE LET_DOC_CODE=" + dt.Rows[i]["MT_CODE"].ToString() + "");
                    CommonClasses.Execute("DELETE FROM HR_MONTHLY_TRANSACTION_COFF_DETAIL WHERE MTC_MT_CODE=" + dt.Rows[i]["MT_CODE"].ToString() + "");
                }
            }
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@MT_MONTH", MT_MONTH);
            par[1] = new SqlParameter("@MT_YEAR", MT_YEAR);
            par[2] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
            //par[3] = new SqlParameter("@MT_BM_CODE", MT_BM_CODE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_DeleteMonthAttend", par);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        { }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@MT_CM_ID", MT_CM_ID);
            //par[1] = new SqlParameter("@MT_BM_CODE", MT_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_FILLMonthAttend", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region LoadEmp
    public DataTable LoadEmp()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@EM_CM_COMP_ID", MT_CM_ID);
            par[1] = new SqlParameter("@EM_TEMP_LEFT_DATE", EM_TEMP_LEFT_DATE);
            par[2] = new SqlParameter("EM_BM_CODE", MT_BM_CODE);

            dt = DL_DBAccess.SelectData("SP_HR_LoadEmp", par);
            return dt;

        }
        catch (Exception ex)
        {
            return dt;
        }
        finally
        {
            dt.Dispose();
        }
    }
    #endregion
}
