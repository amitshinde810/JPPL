using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


public class CompanyMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"

    private int _CM_CODE;
    private int _CM_ID;
    private string _CM_NAME;
    private string _CM_ADDRESS1;
    private string _CM_ADDRESS2;
    private string _CM_ADDRESS3;
    private int _CM_CITY;
    private int _CM_STATE;
    private int _CM_COUNTRY;
    private string _CM_OWNER;
    private string _CM_PHONENO1;
    private string _CM_PHONENO2;
    private string _CM_PHONENO3;
    private string _CM_FAXNO;
    private string _CM_EMAILID;
    private string _CM_WEBSITE;
    private string _CM_SURCHARGE_NO;
    private string _CM_VAT_TIN_NO;
    private string _CM_CST_NO;
    private string _CM_SERVICE_TAX_NO;
    private string _CM_PAN_NO;
    private string _CM_COMMODITY_NO;
    private bool _CM_ACTIVE_IND;
    private DateTime _CM_OPENING_DATE;
    private DateTime _CM_CLOSING_DATE;
    private string _CM_REGD_NO;
    private string _CM_ECC_NO;
    private DateTime _CM_VAT_WEF;
    private DateTime _CM_CST_WEF;
    private string _CM_ISO_NUMBER;
    private string _CM_EXP_LICEN_NO;
    private string _CM_EXP_PERMISSOIN_NO;
    private string _CM_EXCISE_RANGE;
    private string _CM_EXCISE_DIVISION;
    private string _CM_COMMISONERATE;
    private string _CM_EXC_SUPRE_DETAILS;
    private string _CM_BANK_NAME;
    private string _CM_BANK_ADDRESS;
    private string _CM_BANK_ACC_NO;
    private string _CM_ACC_TYPE;
    private string _CM_B_SWIFT_CODE;
    private string _CM_IFSC_CODE;
    private string _CM_GST_NO;

    private string _CM_CIN_NO;
    private string _CM_COMM_CUSTOM;
    private string _CM_AUT_SPEC_SIGN;

    public string Msg = "";

    #endregion

    #region Properties
    public int CM_CODE
    {
        get { return _CM_CODE; }
        set { _CM_CODE = value; }
    }
    public int CM_ID
    {
        get { return _CM_ID; }
        set { _CM_ID = value; }
    }
    public string CM_NAME
    {
        get { return _CM_NAME; }
        set { _CM_NAME = value; }
    }
    public string CM_ADDRESS1
    {
        get { return _CM_ADDRESS1; }
        set { _CM_ADDRESS1 = value; }
    }
    public string CM_ADDRESS2
    {
        get { return _CM_ADDRESS2; }
        set { _CM_ADDRESS2 = value; }
    }
    public string CM_ADDRESS3
    {
        get { return _CM_ADDRESS3; }
        set { _CM_ADDRESS3 = value; }
    }
    public int CM_CITY
    {
        get { return _CM_CITY; }
        set { _CM_CITY = value; }
    }
    public int CM_STATE
    {
        get { return _CM_STATE; }
        set { _CM_STATE = value; }
    }
    public int CM_COUNTRY
    {
        get { return _CM_COUNTRY; }
        set { _CM_COUNTRY = value; }
    }
    public string CM_OWNER
    {
        get { return _CM_OWNER; }
        set { _CM_OWNER = value; }
    }
    public string CM_PHONENO1
    {
        get { return _CM_PHONENO1; }
        set { _CM_PHONENO1 = value; }
    }
    public string CM_PHONENO2
    {
        get { return _CM_PHONENO2; }
        set { _CM_PHONENO2 = value; }
    }
    public string CM_PHONENO3
    {
        get { return _CM_PHONENO3; }
        set { _CM_PHONENO3 = value; }
    }
    public string CM_FAXNO
    {
        get { return _CM_FAXNO; }
        set { _CM_FAXNO = value; }
    }
    public string CM_EMAILID
    {
        get { return _CM_EMAILID; }
        set { _CM_EMAILID = value; }
    }
    public string CM_WEBSITE
    {
        get { return _CM_WEBSITE; }
        set { _CM_WEBSITE = value; }
    }
    public string CM_SURCHARGE_NO
    {
        get { return _CM_SURCHARGE_NO; }
        set { _CM_SURCHARGE_NO = value; }
    }
    public string CM_VAT_TIN_NO
    {
        get { return _CM_VAT_TIN_NO; }
        set { _CM_VAT_TIN_NO = value; }
    }
    public string CM_CST_NO
    {
        get { return _CM_CST_NO; }
        set { _CM_CST_NO = value; }
    }
    public string CM_SERVICE_TAX_NO
    {
        get { return _CM_SERVICE_TAX_NO; }
        set { _CM_SERVICE_TAX_NO = value; }
    }
    public string CM_PAN_NO
    {
        get { return _CM_PAN_NO; }
        set { _CM_PAN_NO = value; }
    }
    public string CM_COMMODITY_NO
    {
        get { return _CM_COMMODITY_NO; }
        set { _CM_COMMODITY_NO = value; }
    }
    public bool CM_ACTIVE_IND
    {
        get { return _CM_ACTIVE_IND; }
        set { _CM_ACTIVE_IND = value; }
    }
    public DateTime CM_OPENING_DATE
    {
        get { return _CM_OPENING_DATE; }
        set { _CM_OPENING_DATE = value; }
    }
    public DateTime CM_CLOSING_DATE
    {
        get { return _CM_CLOSING_DATE; }
        set { _CM_CLOSING_DATE = value; }
    }
    public string CM_REGD_NO
    {
        get { return _CM_REGD_NO; }
        set { _CM_REGD_NO = value; }
    }
    public string CM_ECC_NO
    {
        get { return _CM_ECC_NO; }
        set { _CM_ECC_NO = value; }
    }
    public DateTime CM_VAT_WEF
    {
        get { return _CM_VAT_WEF; }
        set { _CM_VAT_WEF = value; }
    }
    public DateTime CM_CST_WEF
    {
        get { return _CM_CST_WEF; }
        set { _CM_CST_WEF = value; }
    }
    public string CM_ISO_NUMBER
    {
        get { return _CM_ISO_NUMBER; }
        set { _CM_ISO_NUMBER = value; }
    }
    public string CM_EXP_LICEN_NO
    {
        get { return _CM_EXP_LICEN_NO; }
        set { _CM_EXP_LICEN_NO = value; }
    }
    public string CM_EXP_PERMISSOIN_NO
    {
        get { return _CM_EXP_PERMISSOIN_NO; }
        set { _CM_EXP_PERMISSOIN_NO = value; }
    }
    public string CM_GST_NO
    {
        get { return _CM_GST_NO; }
        set { _CM_GST_NO = value; }
    }

    public string CM_EXCISE_RANGE
    {
        get { return _CM_EXCISE_RANGE; }
        set { _CM_EXCISE_RANGE = value; }
    }
    public string CM_EXCISE_DIVISION
    {
        get { return _CM_EXCISE_DIVISION; }
        set { _CM_EXCISE_DIVISION = value; }
    }
    public string CM_COMMISONERATE
    {
        get { return _CM_COMMISONERATE; }
        set { _CM_COMMISONERATE = value; }
    }
    public string CM_EXC_SUPRE_DETAILS
    {
        get { return _CM_EXC_SUPRE_DETAILS; }
        set { _CM_EXC_SUPRE_DETAILS = value; }
    }

    public string CM_BANK_NAME
    {
        get { return _CM_BANK_NAME; }
        set { _CM_BANK_NAME = value; }
    }
    public string CM_BANK_ADDRESS
    {
        get { return _CM_BANK_ADDRESS; }
        set { _CM_BANK_ADDRESS = value; }
    }
    public string CM_BANK_ACC_NO
    {
        get { return _CM_BANK_ACC_NO; }
        set { _CM_BANK_ACC_NO = value; }
    }
    public string CM_ACC_TYPE
    {
        get { return _CM_ACC_TYPE; }
        set { _CM_ACC_TYPE = value; }
    }
    public string CM_B_SWIFT_CODE
    {
        get { return _CM_B_SWIFT_CODE; }
        set { _CM_B_SWIFT_CODE = value; }
    }
    public string CM_IFSC_CODE
    {
        get { return _CM_IFSC_CODE; }
        set { _CM_IFSC_CODE = value; }
    }

    public string CM_CIN_NO
    {
        get { return _CM_CIN_NO; }
        set { _CM_CIN_NO = value; }
    }

    public string CM_COMM_CUSTOM
    {
        get { return _CM_COMM_CUSTOM; }
        set { _CM_COMM_CUSTOM = value; }
    }

    public string CM_AUT_SPEC_SIGN
    {
        get { return _CM_AUT_SPEC_SIGN; }
        set { _CM_AUT_SPEC_SIGN = value; }
    }

    #endregion

    #region "Constructor"
    public CompanyMaster_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public CompanyMaster_BL(int Id)
    {
        _CM_CODE = Id;
    }
    #endregion

    #region CompanyID
    public int CompanyId()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        int companyId = 0;
        try
        {
            dt = DL_DBAccess.SelectData("SP_GetCompanyId", null);

            if (dt.Rows.Count > 0)
            {
                companyId = Convert.ToInt32(dt.Rows[0][0].ToString());
                companyId = companyId + 1;
                return companyId;
            }

            else
                return companyId;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master Class", "CompanyId", ex.Message);
            return companyId;
        }
        finally
        { }
    }
    #endregion CompanyID


    #region Update
    public bool Update()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[46];
            par[0] = new SqlParameter("@CM_ID", CM_ID);
            par[1] = new SqlParameter("@CM_NAME", CM_NAME);
            par[2] = new SqlParameter("@CM_ADDRESS1", CM_ADDRESS1);
            par[3] = new SqlParameter("@CM_ADDRESS2", CM_ADDRESS2);
            par[4] = new SqlParameter("@CM_ADDRESS3", CM_ADDRESS3);
            par[5] = new SqlParameter("@CM_CITY ", CM_CITY);
            par[6] = new SqlParameter("@CM_STATE ", CM_STATE);
            par[7] = new SqlParameter("@CM_COUNTRY ", CM_COUNTRY);
            par[8] = new SqlParameter("@CM_OWNER ", CM_OWNER);
            par[9] = new SqlParameter("@CM_PHONENO1 ", CM_PHONENO1);
            par[10] = new SqlParameter("@CM_PHONENO2 ", CM_PHONENO2);
            par[11] = new SqlParameter("@CM_PHONENO3 ", CM_PHONENO3);
            par[12] = new SqlParameter("@CM_FAXNO ", CM_FAXNO);
            par[13] = new SqlParameter("@CM_EMAILID ", CM_EMAILID);
            par[14] = new SqlParameter("@CM_WEBSITE ", CM_WEBSITE);
            par[15] = new SqlParameter("@CM_SURCHARGE_NO ", CM_SURCHARGE_NO);
            par[16] = new SqlParameter("@CM_VAT_TIN_NO ", CM_VAT_TIN_NO);
            par[17] = new SqlParameter("@CM_CST_NO ", CM_CST_NO);
            par[18] = new SqlParameter("@CM_SERVICE_TAX_NO ", CM_SERVICE_TAX_NO);
            par[19] = new SqlParameter("@CM_PAN_NO", CM_PAN_NO);
            par[20] = new SqlParameter("@CM_COMMODITY_NO", CM_COMMODITY_NO);
            par[21] = new SqlParameter("@CM_ACTIVE_IND", CM_ACTIVE_IND);
            par[22] = new SqlParameter("@CM_OPENING_DATE", CM_OPENING_DATE);
            par[23] = new SqlParameter("@CM_CLOSING_DATE", CM_CLOSING_DATE);
            par[24] = new SqlParameter("@CM_REGD_NO", CM_REGD_NO);
            par[25] = new SqlParameter("@CM_ECC_NO", CM_ECC_NO);
            par[26] = new SqlParameter("@CM_VAT_WEF", CM_VAT_WEF);
            par[27] = new SqlParameter("@CM_CST_WEF", CM_CST_WEF);
            par[28] = new SqlParameter("@CM_ISO_NUMBER", CM_ISO_NUMBER);
            par[29] = new SqlParameter("@CM_EXP_LICEN_NO", CM_EXP_LICEN_NO);
            par[30] = new SqlParameter("@CM_EXP_PERMISSOIN_NO", CM_EXP_PERMISSOIN_NO);

            par[31] = new SqlParameter("@CM_EXCISE_RANGE", CM_EXCISE_RANGE);
            par[32] = new SqlParameter("@CM_EXCISE_DIVISION", CM_EXCISE_DIVISION);
            par[33] = new SqlParameter("@CM_COMMISONERATE", CM_COMMISONERATE);
            par[34] = new SqlParameter("@CM_EXC_SUPRE_DETAILS", CM_EXC_SUPRE_DETAILS);


            par[35] = new SqlParameter("@CM_CODE", CM_CODE);

            par[36] = new SqlParameter("@CM_BANK_NAME", CM_BANK_NAME);
            par[37] = new SqlParameter("@CM_BANK_ADDRESS", CM_BANK_ADDRESS);
            par[38] = new SqlParameter("@CM_BANK_ACC_NO", CM_BANK_ACC_NO);
            par[39] = new SqlParameter("@CM_ACC_TYPE", CM_ACC_TYPE);
            par[40] = new SqlParameter("@CM_B_SWIFT_CODE", CM_B_SWIFT_CODE);
            par[41] = new SqlParameter("@CM_IFSC_CODE", CM_IFSC_CODE);
            par[42] = new SqlParameter("@CM_CIN_NO", CM_CIN_NO);
            par[43] = new SqlParameter("@CM_COMM_CUSTOM", CM_COMM_CUSTOM);
            par[44] = new SqlParameter("@CM_AUT_SPEC_SIGN", CM_AUT_SPEC_SIGN);
            par[45] = new SqlParameter("@CM_GST_NO", CM_GST_NO);

            Boolean responce = DL_DBAccess.Insertion_Updation_Delete("SP_COMPANY_MASTER_UPDATE", par);
            if (responce)
                Msg = "Record Update Succssfully!!";
            return responce;






        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Company Master", "Update", Ex.Message);
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
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@CM_CODE", CM_CODE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_COMPANY_MASTER_DELETE", par);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        try
        {
            DataTable dt = new DataTable();
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@CM_ID", CM_ID);
            dt = DL_DBAccess.SelectData("SP_COMPANY_MASTER_SELECT", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "FillGrid", ex.Message);
        }
    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@CM_CODE", CM_CODE);
            //par[1] = new SqlParameter("@CM_CODE", CM_CODE);
            dt = DL_DBAccess.SelectData("SP_COMPANY_MASTER_SELECT", par);
            if (dt.Rows.Count > 0)
            {

                CM_CODE = Convert.ToInt32(dt.Rows[0]["CM_CODE"]);
                CM_ID = Convert.ToInt32(dt.Rows[0]["CM_ID"]);
                CM_NAME = Regex.Replace(dt.Rows[0]["CM_NAME"].ToString(), "`", "'");
                CM_ADDRESS1 = Regex.Replace(dt.Rows[0]["CM_ADDRESS1"].ToString(), "`", "'");
                CM_ADDRESS2 = Regex.Replace(dt.Rows[0]["CM_ADDRESS2"].ToString(), "`", "'");
                CM_ADDRESS3 = Regex.Replace(dt.Rows[0]["CM_ADDRESS3"].ToString(), "`", "'");
                CM_CITY = Convert.ToInt32(dt.Rows[0]["CM_CITY"].ToString());
                CM_STATE = Convert.ToInt32(dt.Rows[0]["CM_STATE"].ToString());
                CM_COUNTRY = Convert.ToInt32(dt.Rows[0]["CM_COUNTRY"].ToString());
                CM_OWNER = Regex.Replace(dt.Rows[0]["CM_OWNER"].ToString(), "`", "'");
                CM_PHONENO1 = Regex.Replace(dt.Rows[0]["CM_PHONENO1"].ToString(), "`", "'");
                CM_PHONENO2 = Regex.Replace(dt.Rows[0]["CM_PHONENO2"].ToString(), "`", "'");
                CM_PHONENO3 = Regex.Replace(dt.Rows[0]["CM_PHONENO3"].ToString(), "`", "'");
                CM_FAXNO = Regex.Replace(dt.Rows[0]["CM_FAXNO"].ToString(), "`", "'");
                CM_EMAILID = Regex.Replace(dt.Rows[0]["CM_EMAILID"].ToString(), "`", "'");
                CM_WEBSITE = Regex.Replace(dt.Rows[0]["CM_WEBSITE"].ToString(), "`", "'");
                CM_SURCHARGE_NO = Regex.Replace(dt.Rows[0]["CM_SURCHARGE_NO"].ToString(), "`", "'");
                CM_VAT_TIN_NO = Regex.Replace(dt.Rows[0]["CM_VAT_TIN_NO"].ToString(), "`", "'");
                CM_CST_NO = Regex.Replace(dt.Rows[0]["CM_CST_NO"].ToString(), "`", "'");
                CM_SERVICE_TAX_NO = Regex.Replace(dt.Rows[0]["CM_SERVICE_TAX_NO"].ToString(), "`", "'");
                CM_PAN_NO = Regex.Replace(dt.Rows[0]["CM_PAN_NO"].ToString(), "`", "'");
                CM_COMMODITY_NO = Regex.Replace(dt.Rows[0]["CM_COMMODITY_NO"].ToString(), "`", "'");
                CM_ACTIVE_IND = Convert.ToBoolean(dt.Rows[0]["CM_ACTIVE_IND"]);
                CM_OPENING_DATE = Convert.ToDateTime(dt.Rows[0]["CM_OPENING_DATE"]);
                CM_CLOSING_DATE = Convert.ToDateTime(dt.Rows[0]["CM_CLOSING_DATE"]);

                CM_REGD_NO = dt.Rows[0]["CM_REGD_NO"].ToString();
                CM_ECC_NO = dt.Rows[0]["CM_ECC_NO"].ToString();
                CM_VAT_WEF = Convert.ToDateTime(dt.Rows[0]["CM_VAT_WEF"]);
                CM_CST_WEF = Convert.ToDateTime(dt.Rows[0]["CM_CST_WEF"]);
                CM_ISO_NUMBER = dt.Rows[0]["CM_ISO_NUMBER"].ToString();

                CM_EXP_LICEN_NO = dt.Rows[0]["CM_EXP_LICEN_NO"].ToString();
                CM_EXP_PERMISSOIN_NO = dt.Rows[0]["CM_EXP_PERMISSOIN_NO"].ToString();

                CM_EXCISE_RANGE = dt.Rows[0]["CM_EXCISE_RANGE"].ToString();
                CM_EXCISE_DIVISION = dt.Rows[0]["CM_EXCISE_DIVISION"].ToString();
                CM_COMMISONERATE = dt.Rows[0]["CM_COMMISONERATE"].ToString();
                CM_EXC_SUPRE_DETAILS = dt.Rows[0]["CM_EXC_SUPRE_DETAILS"].ToString();

                CM_BANK_NAME = dt.Rows[0]["CM_BANK_NAME"].ToString();
                CM_BANK_ADDRESS = dt.Rows[0]["CM_BANK_ADDRESS"].ToString();
                CM_BANK_ACC_NO = dt.Rows[0]["CM_BANK_ACC_NO"].ToString();
                CM_ACC_TYPE = dt.Rows[0]["CM_ACC_TYPE"].ToString();
                CM_B_SWIFT_CODE = dt.Rows[0]["CM_B_SWIFT_CODE"].ToString();
                CM_IFSC_CODE = dt.Rows[0]["CM_IFSC_CODE"].ToString();

                CM_CIN_NO = dt.Rows[0]["CM_CIN_NO"].ToString();
                CM_COMM_CUSTOM = dt.Rows[0]["CM_COMM_CUSTOM"].ToString();
                CM_AUT_SPEC_SIGN = dt.Rows[0]["CM_AUT_SPEC_SIGN"].ToString();
                CM_GST_NO = dt.Rows[0]["CM_GST_NO"].ToString();



            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "GetInfo", ex.Message);
        }

    }
    #endregion

    #region FillCombo
    public DataTable GetCountry(string Query)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", Query);

            dt = DL_DBAccess.SelectData("SP_HR_EmpCountryCombo", par);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "GetCountry", ex.Message);
        }
        return dt;
    }
    #endregion FillCombo


}
