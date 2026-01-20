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
using System.Net.Mail;
using System.Text;
using System.Net;


public class EmployeeMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    public string Msg = "";
    SqlParameter[] para = null;


    #endregion

    #region "Variables"
    #region  Employee Master
    private int _EM_CODE, _EM_CM_COMP_ID, _EM_BM_CODE, _EM_COUNTRY, _EM_STATE, _EM_CITY, _EM_COUNTRY1, _EM_STATE1, _EM_CITY1, _EM_DM_CODE, 
        _EM_DPM_CODE, _EM_GM_CODE, _EM_PAY_MODE, _EM_PAYSLIP, _EM_CASUAL_LEAVE, _UserCode;
    
    private string _EM_TICKETNO, _EM_NAME, _EM_M_NAME, _EM_L_NAME, _EM_FATHER_NAME, _EM_MOTHER_NAME_BEFORE, _EM_MOTHER_NAME_AFTER, _EM_MARTIAL, 
        _EM_EMAIL_ID, _EM_CONTACTNO, _EM_LAND_LINE, _EM_BLOOD_GROUP, _EM_ADR1, _EM_PIN, _EM_ADR2, _EM_PIN1, _EM_NATIVE_PLACE, _EM_NATIONALITY, 
        _EM_RELIGION, _EM_MONTHER_TONGUE, _EM_WOFF, _EM_AADHAR, _EM_PAN, _EM_IFSC_CODE, _EM_BANK, _EM_BANK_AC_NO, _EM_PFNO, _EM_FNAME, _EM_RELATION, 
        _EM_ESINO, _EM_REMARK, _EM_TEMP_LEFT_DATE;
    DateTime _EM_APPLICATION_DATE, _EM_BIRTHDATE, _EM_JOINDATE, _EM_CONFIRM_DATE, _EM_PF_DATE, _EM_ESI_DATE;
    private bool _EM_GENDER, _EM_PAYIND, _EM_PF, _EM_ISEmpPFSal, _EM_ISEmprPFSal, _EM_ESI, _EM_PT, _EM_LWF, _EM_OTM, _EM_IS_MED, 
        _EM_IS_LTA, _EM_IS_CONFIRM, _EM_IS_ONROLE, _EM_IS_APROVED, _EM_POLICE_VERI, _EM_IS_SAL_INCREMENT, _EM_TEMP_LEFT, _EM_IsUser;
    private double _EM_CTC_TOTEAR, _EM_CTC_TOTDED, _EM_CTC_NETTOT, _EM_CTC_MEDICAL
        , _EM_CTC_LTA, _EM_CTC_PF, _EM_CTC_ESIC, _EM_CTC_GRADUITY, _EM_CTC_BONUS
        , _EM_CTC_EL, _EM_CTC_TOTAL, _EM_CTC_MONTHLYTOT, _EM_CTC_PA, _EM_CTC_EXTRA_HRA
        , _EM_CL, _EM_SL, _EM_EL, _EM_PFEMP, _EM_PFEMPLOYER, _EM_FPF, _EM_AGE, _EM_PAID_LEAVE,_EM_SICK_LEAVE, _EM_EARNED_LEAVE, _EM_OT_MULT;
    #endregion

    #region UserMaster

    int _UM_CODE;
    int _UM_CM_ID;
    int _UM_BM_CODE;
    int _UM_EM_CODE;
    string _UM_USERNAME;
    string _UM_PASSWORD;
    string _UM_LEVEL;
    DateTime _UM_LASTLOGIN_DATETIME;
    string _UM_IP_ADDRESS;
    bool _UM_ACTIVE_IND;
    bool _UM_IS_ADMIN;



    #endregion

    #endregion

    #region "Constructer"
    public EmployeeMaster_BL()
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region Parameterise Constructor
    public EmployeeMaster_BL(int Id, int bmCode)
    {
        _EM_CODE = Id;
        _EM_BM_CODE = bmCode;
    }
    #endregion

    #region "Propertey Employee master"


    #region UserCode
    public int UserCode
    {
        get { return _UserCode; }
        set { _UserCode = value; }
    }
    #endregion

    #region EM_IS_ONROLE
    public bool EM_IS_ONROLE
    {
        get { return _EM_IS_ONROLE; }
        set { _EM_IS_ONROLE = value; }
    }
    #endregion

    #region EM_CODE
    public int EM_CODE
    {
        get { return _EM_CODE; }
        set { _EM_CODE = value; }
    }
    #endregion

    #region EM_CM_COMP_ID
    public int EM_CM_COMP_ID
    {
        get { return _EM_CM_COMP_ID; }
        set { _EM_CM_COMP_ID = value; }
    }
    #endregion

    #region EM_BM_CODE
    public int EM_BM_CODE
    {
        get { return _EM_BM_CODE; }
        set { _EM_BM_CODE = value; }
    }
    #endregion

    #region EM_TICKETNO
    public string EM_TICKETNO
    {
        get { return _EM_TICKETNO; }
        set { _EM_TICKETNO = value; }
    }
    #endregion

    #region EM_APPLICATION_DATE
    public DateTime EM_APPLICATION_DATE
    {
        get { return _EM_APPLICATION_DATE; }
        set { _EM_APPLICATION_DATE = value; }
    }
    #endregion

    #region EM_NAME
    public string EM_NAME
    {
        get { return _EM_NAME; }
        set { _EM_NAME = value; }
    }
    #endregion

    #region EM_M_NAME
    public string EM_M_NAME
    {
        get { return _EM_M_NAME; }
        set { _EM_M_NAME = value; }
    }
    #endregion

    #region EM_L_NAME
    public string EM_L_NAME
    {
        get { return _EM_L_NAME; }
        set { _EM_L_NAME = value; }
    }
    #endregion

    #region EM_FATHER_NAME
    public string EM_FATHER_NAME
    {
        get { return _EM_FATHER_NAME; }
        set { _EM_FATHER_NAME = value; }
    }
    #endregion

    #region EM_MOTHER_NAME_BEFORE
    public string EM_MOTHER_NAME_BEFORE
    {
        get { return _EM_MOTHER_NAME_BEFORE; }
        set { _EM_MOTHER_NAME_BEFORE= value; }
    }
    #endregion

    #region EM_MOTHER_NAME_AFTER
    public string EM_MOTHER_NAME_AFTER
    {
        get { return _EM_MOTHER_NAME_AFTER; }
        set { _EM_MOTHER_NAME_AFTER = value; }
    }
    #endregion

    #region EM_GENDER
    public bool EM_GENDER
    {
        get { return _EM_GENDER; }
        set { _EM_GENDER = value; }
    }
    #endregion

    #region EM_MARTIAL
    public string EM_MARTIAL
    {
        get { return _EM_MARTIAL; }
        set { _EM_MARTIAL = value; }
    }
    #endregion

    #region EM_EMAIL_ID
    public string EM_EMAIL_ID
    {
        get { return _EM_EMAIL_ID; }
        set { _EM_EMAIL_ID = value; }
    }
    #endregion

    #region EM_CONTACTNO
    public string EM_CONTACTNO
    {
        get { return _EM_CONTACTNO; }
        set { _EM_CONTACTNO = value; }
    }
    #endregion

    #region EM_LAND_LINE
    public string EM_LAND_LINE
    {
        get { return _EM_LAND_LINE; }
        set { _EM_LAND_LINE = value; }
    }
    #endregion

    #region EM_BIRTHDATE
    public DateTime EM_BIRTHDATE
    {
        get { return _EM_BIRTHDATE; }
        set { _EM_BIRTHDATE = value; }
    }
    #endregion

    #region EM_BLOOD_GROUP
    public string EM_BLOOD_GROUP
    {
        get { return _EM_BLOOD_GROUP; }
        set { _EM_BLOOD_GROUP = value; }
    }
    #endregion

    #region EM_ADR1
    public string EM_ADR1
    {
        get { return _EM_ADR1; }
        set { _EM_ADR1 = value; }
    }
    #endregion

    #region EM_COUNTRY
    public int EM_COUNTRY
    {
        get { return _EM_COUNTRY; }
        set { _EM_COUNTRY = value; }
    }
    #endregion

    #region EM_STATE
    public int EM_STATE
    {
        get { return _EM_STATE; }
        set { _EM_STATE = value; }
    }
    #endregion

    #region EM_CITY
    public int EM_CITY
    {
        get { return _EM_CITY; }
        set { _EM_CITY = value; }
    }
    #endregion

    #region EM_PIN
    public string EM_PIN
    {
        get { return _EM_PIN; }
        set { _EM_PIN = value; }
    }
    #endregion

    #region EM_ADR2
    public string EM_ADR2
    {
        get { return _EM_ADR2; }
        set { _EM_ADR2 = value; }
    }
    #endregion

    #region EM_COUNTRY1
    public int EM_COUNTRY1
    {
        get { return _EM_COUNTRY1; }
        set { _EM_COUNTRY1 = value; }
    }
    #endregion

    #region EM_STATE1
    public int EM_STATE1
    {
        get { return _EM_STATE1; }
        set { _EM_STATE1 = value; }
    }
    #endregion

    #region EM_CITY1
    public int EM_CITY1
    {
        get { return _EM_CITY1; }
        set { _EM_CITY1 = value; }
    }
    #endregion

    #region EM_PIN1
    public string EM_PIN1
    {
        get { return _EM_PIN1; }
        set { _EM_PIN1 = value; }
    }
    #endregion

    #region EM_NATIVE_PLACE
    public string EM_NATIVE_PLACE
    {
        get { return _EM_NATIVE_PLACE; }
        set { _EM_NATIVE_PLACE = value; }
    }
    #endregion

    #region EM_NATIONALITY
    public string EM_NATIONALITY
    {
        get { return _EM_NATIONALITY; }
        set { _EM_NATIONALITY = value; }
    }
    #endregion

    #region EM_RELIGION
    public string EM_RELIGION
    {
        get { return _EM_RELIGION; }
        set { _EM_RELIGION = value; }
    }
    #endregion

    #region EM_MONTHER_TONGUE
    public string EM_MONTHER_TONGUE
    {
        get { return _EM_MONTHER_TONGUE; }
        set { _EM_MONTHER_TONGUE = value; }
    }
    #endregion

    #region EM_DM_CODE
    public int EM_DM_CODE
    {
        get { return _EM_DM_CODE; }
        set { _EM_DM_CODE = value; }
    }
    #endregion

    #region EM_DPM_CODE
    public int EM_DPM_CODE
    {
        get { return _EM_DPM_CODE; }
        set { _EM_DPM_CODE = value; }
    }
    #endregion

    #region EM_GM_CODE
    public int EM_GM_CODE
    {
        get { return _EM_GM_CODE; }
        set { _EM_GM_CODE = value; }
    }
    #endregion

    #region EM_JOINDATE
    public DateTime EM_JOINDATE
    {
        get { return _EM_JOINDATE; }
        set { _EM_JOINDATE = value; }
    }

    #endregion

    #region EM_CONFIRM_DATE
    public DateTime EM_CONFIRM_DATE
    {
        get { return _EM_CONFIRM_DATE; }
        set { _EM_CONFIRM_DATE = value; }
    }
    #endregion

    #region EM_WOFF
    public string EM_WOFF
    {
        get { return _EM_WOFF; }
        set { _EM_WOFF = value; }
    }
    #endregion

    #region EM_AADHAR
    public string EM_AADHAR
    {
        get { return _EM_AADHAR; }
        set { _EM_AADHAR = value; }
    }
    #endregion

    #region EM_PAN
    public string EM_PAN
    {
        get { return _EM_PAN; }
        set { _EM_PAN = value; }
    }
    #endregion

    #region EM_PF
    public bool EM_PF
    {
        get { return _EM_PF; }
        set { _EM_PF = value; }
    }
    #endregion

    #region EM_PF_DATE
    public DateTime EM_PF_DATE
    {
        get { return _EM_PF_DATE; }
        set { _EM_PF_DATE = value; }
    }
    #endregion

    #region EM_PFNO
    public string EM_PFNO
    {
        get { return _EM_PFNO; }
        set { _EM_PFNO = value; }
    }
    #endregion

    #region EM_RELATION
    public string EM_RELATION
    {
        get { return _EM_RELATION; }
        set { _EM_RELATION = value; }
    }
    #endregion

    #region EM_FNAME  // Nominee Name
    public string EM_FNAME
    {
        get { return _EM_FNAME; }
        set { _EM_FNAME = value; }
    }
    #endregion

    #region EM_ISEmpPFSal
    public bool EM_ISEmpPFSal
    {
        get { return _EM_ISEmpPFSal; }
        set { _EM_ISEmpPFSal = value; }
    }
    #endregion

    #region EM_PFEMP
    public double EM_PFEMP
    {
        get { return _EM_PFEMP; }
        set { _EM_PFEMP = value; }
    }
    #endregion

    #region EM_ISEmprPFSal
    public bool EM_ISEmprPFSal
    {
        get { return _EM_ISEmprPFSal; }
        set { _EM_ISEmprPFSal = value; }
    }
    #endregion

    #region EM_FPF
    public double EM_FPF
    {
        get { return _EM_FPF; }
        set { _EM_FPF = value; }
    }
    #endregion

    #region EM_PFEMPLOYER
    public double EM_PFEMPLOYER
    {
        get { return _EM_PFEMPLOYER; }
        set { _EM_PFEMPLOYER = value; }
    }
    #endregion

    #region EM_PT
    public bool EM_PT
    {
        get { return _EM_PT; }
        set { _EM_PT = value; }
    }
    #endregion

    #region EM_ESI
    public bool EM_ESI
    {
        get { return _EM_ESI; }
        set { _EM_ESI = value; }
    }
    #endregion

    #region EM_ESINO
    public string EM_ESINO
    {
        get { return _EM_ESINO; }
        set { _EM_ESINO = value; }
    }
    #endregion

    #region EM_ESI_DATE
    public DateTime EM_ESI_DATE
    {
        get { return _EM_ESI_DATE; }
        set { _EM_ESI_DATE = value; }
    }

    #endregion

    #region EM_LWF
    public bool EM_LWF
    {
        get { return _EM_LWF; }
        set { _EM_LWF = value; }
    }
    #endregion

    #region EM_OTM
    public bool EM_OTM
    {
        get { return _EM_OTM; }
        set { _EM_OTM = value; }
    }
    #endregion

    #region EM_IS_LTA
    public bool EM_IS_LTA
    {
        get { return _EM_IS_LTA; }
        set { _EM_IS_LTA = value; }
    }
    #endregion

    #region EM_IS_MED
    public bool EM_IS_MED
    {
        get { return _EM_IS_MED; }
        set { _EM_IS_MED = value; }
    }
    #endregion

    #region EM_IFSC_CODE
    public string EM_IFSC_CODE
    {
        get { return _EM_IFSC_CODE; }
        set { _EM_IFSC_CODE = value; }
    }
    #endregion

    #region EM_PAY_MODE
    public int EM_PAY_MODE
    {
        get { return _EM_PAY_MODE; }
        set { _EM_PAY_MODE = value; }
    }
    #endregion

    #region EM_PAYSLIP
    public int EM_PAYSLIP
    {
        get { return _EM_PAYSLIP; }
        set { _EM_PAYSLIP = value; }
    }
    #endregion

    #region EM_AGE
    public double EM_AGE
    {
        get { return _EM_AGE; }
        set { _EM_AGE= value; }
    }
    #endregion
    ////// Int

    ////// String

    #region EM_IS_APROVED
    public bool EM_IS_APROVED
    {
        get { return _EM_IS_APROVED; }
        set { _EM_IS_APROVED = value; }
    }
    #endregion

    #region EM_POLICE_VERI
    public bool EM_POLICE_VERI
    {
        get { return _EM_POLICE_VERI; }
        set { _EM_POLICE_VERI = value; }
    }
    #endregion

    #region EM_CASUAL_LEAVE
    public int EM_CASUAL_LEAVE
    {
        get { return _EM_CASUAL_LEAVE; }
        set { _EM_CASUAL_LEAVE = value; }
    }
    #endregion

    #region EM_PAID_LEAVE
    public double EM_PAID_LEAVE
    {
        get { return _EM_PAID_LEAVE; }
        set { _EM_PAID_LEAVE = value; }
    }
    #endregion

    #region EM_SICK_LEAVE
    public double EM_SICK_LEAVE
    {
        get { return _EM_SICK_LEAVE; }
        set { _EM_SICK_LEAVE = value; }
    }
    #endregion

    #region EM_EARNED_LEAVE
    public double EM_EARNED_LEAVE
    {
        get { return _EM_EARNED_LEAVE; }
        set { _EM_EARNED_LEAVE = value; }
    }
    #endregion

    #region EM_BANK
    public string EM_BANK
    {
        get { return _EM_BANK; }
        set { _EM_BANK = value; }
    }
    #endregion

    #region EM_BANK_AC_NO
    public string EM_BANK_AC_NO
    {
        get { return _EM_BANK_AC_NO; }
        set { _EM_BANK_AC_NO = value; }
    }
    #endregion

    #region EM_REMARK
    public string EM_REMARK
    {
        get { return _EM_REMARK; }
        set { _EM_REMARK = value; }
    }
    #endregion

    #region EM_PAYIND
    public bool EM_PAYIND
    {
        get { return _EM_PAYIND; }
        set { _EM_PAYIND = value; }
    }
    #endregion

    #region EM_IS_CONFIRM
    public bool EM_IS_CONFIRM
    {
        get { return _EM_IS_CONFIRM; }
        set { _EM_IS_CONFIRM = value; }
    }
    #endregion

    #region EM_CTC_TOTEAR
    public double EM_CTC_TOTEAR
    {
        get { return _EM_CTC_TOTEAR; }
        set { _EM_CTC_TOTEAR = value; }
    }
    #endregion

    #region EM_CTC_TOTDED
    public double EM_CTC_TOTDED
    {
        get { return _EM_CTC_TOTDED; }
        set { _EM_CTC_TOTDED = value; }
    }
    #endregion

    #region EM_CTC_NETTOT
    public double EM_CTC_NETTOT
    {
        get { return _EM_CTC_NETTOT; }
        set { _EM_CTC_NETTOT = value; }
    }
    #endregion

    #region EM_CTC_MEDICAL
    public double EM_CTC_MEDICAL
    {
        get { return _EM_CTC_MEDICAL; }
        set { _EM_CTC_MEDICAL = value; }
    }
    #endregion

    #region EM_CTC_LTA
    public double EM_CTC_LTA
    {
        get { return _EM_CTC_LTA; }
        set { _EM_CTC_LTA = value; }
    }
    #endregion

    #region EM_CTC_PF
    public double EM_CTC_PF
    {
        get { return _EM_CTC_PF; }
        set { _EM_CTC_PF = value; }
    }
    #endregion

    #region EM_CTC_ESIC
    public double EM_CTC_ESIC
    {
        get { return _EM_CTC_ESIC; }
        set { _EM_CTC_ESIC = value; }
    }
    #endregion

    #region EM_CTC_GRADUITY
    public double EM_CTC_GRADUITY
    {
        get { return _EM_CTC_GRADUITY; }
        set { _EM_CTC_GRADUITY = value; }
    }
    #endregion

    #region EM_CTC_BONUS
    public double EM_CTC_BONUS
    {
        get { return _EM_CTC_BONUS; }
        set { _EM_CTC_BONUS = value; }
    }
    #endregion

    #region EM_CTC_EL
    public double EM_CTC_EL
    {
        get { return _EM_CTC_EL; }
        set { _EM_CTC_EL = value; }
    }
    #endregion

    #region EM_CTC_TOTAL
    public double EM_CTC_TOTAL
    {
        get { return _EM_CTC_TOTAL; }
        set { _EM_CTC_TOTAL = value; }
    }
    #endregion

    #region EM_CTC_MONTHLYTOT
    public double EM_CTC_MONTHLYTOT
    {
        get { return _EM_CTC_MONTHLYTOT; }
        set { _EM_CTC_MONTHLYTOT = value; }
    }
    #endregion

    #region EM_CTC_PA
    public double EM_CTC_PA
    {
        get { return _EM_CTC_PA; }
        set { _EM_CTC_PA = value; }
    }
    #endregion

    #region EM_CTC_EXTRA_HRA
    public double EM_CTC_EXTRA_HRA
    {
        get { return _EM_CTC_EXTRA_HRA; }
        set { _EM_CTC_EXTRA_HRA = value; }
    }
    #endregion

    #region EM_IS_SAL_INCREMENT
    public bool EM_IS_SAL_INCREMENT
    {
        get { return _EM_IS_SAL_INCREMENT; }
        set { _EM_IS_SAL_INCREMENT = value; }
    }
    #endregion

    #region EM_CL
    public double EM_CL
    {
        get { return _EM_CL; }
        set { _EM_CL = value; }
    }
    #region EM_TEMP_LEFT
    public bool EM_TEMP_LEFT
    {
        get { return _EM_TEMP_LEFT; }
        set { _EM_TEMP_LEFT = value; }
    }
    #endregion

    #region EM_SL
    public double EM_SL
    {
        get { return _EM_SL; }
        set { _EM_SL = value; }
    }
    #endregion

    #region EM_EL
    public double EM_EL
    {
        get { return _EM_EL; }
        set { _EM_EL = value; }
    }
    #endregion

    #endregion

    #region EM_TEMP_LEFT_DATE
    public string EM_TEMP_LEFT_DATE
    {
        get { return _EM_TEMP_LEFT_DATE; }
        set { _EM_TEMP_LEFT_DATE = value; }
    }
    #endregion

    #region EM_IsUser
    public bool EM_IsUser
    {
        get { return _EM_IsUser; }
        set { _EM_IsUser = value; }
    }
    #endregion

    #region EM_OT_MULT
    public double EM_OT_MULT
    {
        get { return _EM_OT_MULT; }
        set { _EM_OT_MULT = value; }
    }
    #endregion

    #endregion

    #region "Properties For User Master"

    public int UM_CODE
    {
        get { return _UM_CODE; }
        set { _UM_CODE = value; }
    }

    public int UM_CM_ID
    {
        get { return _UM_CM_ID; }
        set { _UM_CM_ID = value; }
    }

    public int UM_BM_CODE
    {
        get { return _UM_BM_CODE; }
        set { _UM_BM_CODE = value; }
    }

    public int UM_EM_CODE
    {
        get { return _UM_EM_CODE; }
        set { _UM_EM_CODE = value; }
    }

    public string UM_USERNAME
    {
        get
        {
            return _UM_USERNAME;
        }
        set
        {
            _UM_USERNAME = value;
        }
    }

    public string UM_PASSWORD
    {
        get { return _UM_PASSWORD; }
        set { _UM_PASSWORD = value; }
    }

    public string UM_LEVEL
    {
        get { return _UM_LEVEL; }
        set { _UM_LEVEL = value; }
    }

    public DateTime UM_LASTLOGIN_DATETIME
    {
        get { return _UM_LASTLOGIN_DATETIME; }
        set { _UM_LASTLOGIN_DATETIME = value; }
    }

    public string UM_IP_ADDRESS
    {
        get { return _UM_IP_ADDRESS; }
        set { _UM_IP_ADDRESS = value; }
    }

    public bool UM_ACTIVE_IND
    {
        get { return _UM_ACTIVE_IND; }
        set { _UM_ACTIVE_IND = value; }
    }

    public bool UM_IS_ADMIN
    {
        get { return _UM_IS_ADMIN; }
        set { _UM_IS_ADMIN = value; }
    }




    #endregion


    #region Methods

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@EM_CODE", EM_CODE);
            par[1] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            dt = DL_DBAccess.SelectData("SP_HR_GetInfoEmp", par);
            if (dt.Rows.Count > 0)
            {
                DataTable dtEMPIncr = CommonClasses.Execute("SELECT EM_CODE,EMBM_BM_CODE as EM_BM_CODE,EMDM_DM_CODE as EM_DPM_CODE,EMDS_DS_CODE as EM_DM_CODE FROM HR_EMPLOYEE_MASTER ,HR_EMPLOYEE_BRANCH_DETAILS as [BM],HR_EMPLOYEE_DEPARTMENT_DETAILS as [DM],HR_EMPLOYEE_DESIGNATION_DETAILS as [DS] where EMBM_INCREMENT_DATE=(SELECT MAX(EMBM_INCREMENT_DATE) FROM HR_EMPLOYEE_BRANCH_DETAILS WHERE EMBM_EM_CODE = [BM].EMBM_EM_CODE) and EMDM_INCREMENT_DATE=(SELECT MAX(EMDM_INCREMENT_DATE) FROM HR_EMPLOYEE_DEPARTMENT_DETAILS WHERE EMDM_EM_CODE = [DM].EMDM_EM_CODE) and EMDS_INCREMENT_DATE=(SELECT MAX(EMDS_INCREMENT_DATE) FROM HR_EMPLOYEE_DESIGNATION_DETAILS WHERE EMDS_EM_CODE = [DS].EMDS_EM_CODE) and EM_DELETE_FLAG='False' and EMBM_EM_CODE=EM_CODE and EMDM_EM_CODE=EM_CODE and EMDS_EM_CODE=EM_CODE and EM_CODE=" + EM_CODE + "");
                string Dept = "0";
                string Desi = "0";
                string Branch = "0";
                if (dtEMPIncr.Rows.Count > 0)
                {
                    Branch = dtEMPIncr.Rows[0]["EM_BM_CODE"].ToString();
                    Dept = dtEMPIncr.Rows[0]["EM_DPM_CODE"].ToString();
                    Desi = dtEMPIncr.Rows[0]["EM_DM_CODE"].ToString();
                }
                EM_CODE = Convert.ToInt32(dt.Rows[0]["EM_CODE"]);
                
                EM_DPM_CODE = Convert.ToInt32(Dept);
                EM_DM_CODE = Convert.ToInt32(Desi);
                EM_BM_CODE = Convert.ToInt32(Branch);
                EM_CITY = Convert.ToInt32(dt.Rows[0]["EM_CITY"]);
                EM_STATE = Convert.ToInt32(dt.Rows[0]["EM_STATE"]);
                EM_COUNTRY = Convert.ToInt32(dt.Rows[0]["EM_COUNTRY"]);



                EM_TICKETNO = dt.Rows[0]["EM_TICKETNO"].ToString();
                EM_NAME = dt.Rows[0]["EM_NAME"].ToString();
                EM_ADR1 = dt.Rows[0]["EM_ADR1"].ToString();
                EM_ADR2 = dt.Rows[0]["EM_ADR2"].ToString();
                EM_WOFF = dt.Rows[0]["EM_WOFF"].ToString();
                EM_BANK = dt.Rows[0]["EM_BANK"].ToString();
                EM_BANK_AC_NO = dt.Rows[0]["EM_BANK_AC_NO"].ToString();
                EM_PFNO = dt.Rows[0]["EM_PFNO"].ToString();
                EM_FNAME = dt.Rows[0]["EM_FNAME"].ToString();
                EM_RELATION = dt.Rows[0]["EM_RELATION"].ToString().Trim();
                EM_ESINO = dt.Rows[0]["EM_ESINO"].ToString();
                EM_REMARK = dt.Rows[0]["EM_REMARK"].ToString();
                EM_CONTACTNO = dt.Rows[0]["EM_CONTACTNO"].ToString();
                EM_BIRTHDATE = Convert.ToDateTime(dt.Rows[0]["EM_BIRTHDATE"].ToString());
                EM_JOINDATE = Convert.ToDateTime(dt.Rows[0]["EM_JOINDATE"].ToString());
                EM_PF = Convert.ToBoolean(dt.Rows[0]["EM_PF"].ToString());
                if (Convert.ToBoolean(dt.Rows[0]["EM_PF"].ToString()))
                {
                    EM_PF_DATE = Convert.ToDateTime(dt.Rows[0]["EM_PF_DATE"].ToString());

                }

                EM_ISEmpPFSal = Convert.ToBoolean(dt.Rows[0]["EM_ISEmpPFSal"].ToString());
                EM_ISEmprPFSal = Convert.ToBoolean(dt.Rows[0]["EM_ISEmprPFSal"].ToString());


                EM_PFEMP = Convert.ToDouble(dt.Rows[0]["EM_PFEMP"]);
                EM_PFEMPLOYER = Convert.ToDouble(dt.Rows[0]["EM_PFEMPLOYER"]);
                EM_FPF = Convert.ToDouble(dt.Rows[0]["EM_FPF"]);
                EM_ESI = Convert.ToBoolean(dt.Rows[0]["EM_ESI"].ToString());
                if (Convert.ToBoolean(dt.Rows[0]["EM_ESI"].ToString()))
                {
                    EM_ESI_DATE = Convert.ToDateTime(dt.Rows[0]["EM_ESI_DATE"].ToString());
                }

                EM_GENDER = Convert.ToBoolean(dt.Rows[0]["EM_GENDER"].ToString());
                EM_PAYIND = Convert.ToBoolean(dt.Rows[0]["EM_PAYIND"].ToString());



                EM_LWF = Convert.ToBoolean(dt.Rows[0]["EM_LWF"].ToString());
                EM_IS_MED = Convert.ToBoolean(dt.Rows[0]["EM_IS_MED"].ToString());
                EM_IS_LTA = Convert.ToBoolean(dt.Rows[0]["EM_IS_LTA"].ToString());



                EM_MARTIAL = dt.Rows[0]["EM_MARTIAL"].ToString();
                EM_RELIGION = dt.Rows[0]["EM_RELIGION"].ToString();



                EM_APPLICATION_DATE = Convert.ToDateTime(dt.Rows[0]["EM_APPLICATION_DATE"].ToString());
                EM_M_NAME = dt.Rows[0]["EM_M_NAME"].ToString();
                EM_L_NAME = dt.Rows[0]["EM_L_NAME"].ToString();
                EM_FATHER_NAME = dt.Rows[0]["EM_FATHER_NAME"].ToString();
                EM_MOTHER_NAME_BEFORE = dt.Rows[0]["EM_MOTHER_NAME_BEFORE"].ToString();
                EM_MOTHER_NAME_AFTER = dt.Rows[0]["EM_MOTHER_NAME_AFTER"].ToString();
                EM_EMAIL_ID = dt.Rows[0]["EM_EMAIL_ID"].ToString();
                EM_LAND_LINE = dt.Rows[0]["EM_LAND_LINE"].ToString();
                EM_BLOOD_GROUP = dt.Rows[0]["EM_BLOOD_GROUP"].ToString();
                EM_PIN = dt.Rows[0]["EM_PIN"].ToString();
                EM_COUNTRY1 = int.Parse(dt.Rows[0]["EM_COUNTRY1"].ToString());
                EM_STATE1 = int.Parse(dt.Rows[0]["EM_STATE1"].ToString());
                EM_CITY1 = int.Parse(dt.Rows[0]["EM_CITY1"].ToString());
                EM_PIN1 = dt.Rows[0]["EM_PIN1"].ToString();
                EM_NATIVE_PLACE = dt.Rows[0]["EM_NATIVE_PLACE"].ToString();
                EM_MONTHER_TONGUE = dt.Rows[0]["EM_MONTHER_TONGUE"].ToString();
                EM_AADHAR = dt.Rows[0]["EM_AADHAR"].ToString();
                EM_PAN = dt.Rows[0]["EM_PAN"].ToString();
                EM_IFSC_CODE = dt.Rows[0]["EM_IFSC_CODE"].ToString();
                EM_PAY_MODE = int.Parse(dt.Rows[0]["EM_PAY_MODE"].ToString());
                EM_PAYSLIP = int.Parse(dt.Rows[0]["EM_PAYSLIP"].ToString());
                EM_NATIONALITY = dt.Rows[0]["EM_NATIONALITY"].ToString();
                EM_AGE = Convert.ToDouble(dt.Rows[0]["EM_AGE"].ToString());

                EM_CTC_TOTEAR = Convert.ToDouble(dt.Rows[0]["EM_CTC_TOTEAR"].ToString());
                EM_CTC_TOTDED = Convert.ToDouble(dt.Rows[0]["EM_CTC_TOTDED"].ToString());
                EM_CTC_NETTOT = Convert.ToDouble(dt.Rows[0]["EM_CTC_NETTOT"].ToString());
                EM_CTC_MEDICAL = Convert.ToDouble(dt.Rows[0]["EM_CTC_MEDICAL"].ToString());
                EM_CTC_LTA = Convert.ToDouble(dt.Rows[0]["EM_CTC_LTA"].ToString());
                if (dt.Rows[0]["EM_CTC_EXTRA_HRA"].ToString() != "")
                    EM_CTC_EXTRA_HRA = Convert.ToDouble(dt.Rows[0]["EM_CTC_EXTRA_HRA"].ToString());

                EM_CTC_PF = Convert.ToDouble(dt.Rows[0]["EM_CTC_PF"].ToString());
                EM_CTC_ESIC = Convert.ToDouble(dt.Rows[0]["EM_CTC_ESIC"].ToString());
                EM_CTC_GRADUITY = Convert.ToDouble(dt.Rows[0]["EM_CTC_GRADUITY"].ToString());
                EM_CTC_BONUS = Convert.ToDouble(dt.Rows[0]["EM_CTC_BONUS"].ToString());
                EM_CTC_EL = Convert.ToDouble(dt.Rows[0]["EM_CTC_EL"].ToString());
                EM_CTC_TOTAL = Convert.ToDouble(dt.Rows[0]["EM_CTC_TOTAL"].ToString());
                EM_CTC_MONTHLYTOT = Convert.ToDouble(dt.Rows[0]["EM_CTC_MONTHLYTOT"].ToString());
                EM_CTC_PA = Convert.ToDouble(dt.Rows[0]["EM_CTC_PA"].ToString());
                

                if (dt.Rows[0]["EM_CONFIRM_DATE"].ToString() != "" && Convert.ToDateTime(dt.Rows[0]["EM_CONFIRM_DATE"].ToString()).Year != 1900)
                {
                    EM_CONFIRM_DATE = Convert.ToDateTime(dt.Rows[0]["EM_CONFIRM_DATE"].ToString());
                }
                EM_IS_CONFIRM = Convert.ToBoolean(dt.Rows[0]["EM_IS_CONFIRM"].ToString());

                EM_PAID_LEAVE = Convert.ToDouble(dt.Rows[0]["EM_PAID_LEAVE"]);
                EM_CASUAL_LEAVE = Convert.ToInt32(dt.Rows[0]["EM_CASUAL_LEAVE"]);
                EM_SICK_LEAVE = Convert.ToDouble(dt.Rows[0]["EM_SICK_LEAVE"]);
                EM_EARNED_LEAVE = Convert.ToDouble(dt.Rows[0]["EM_EARNED_LEAVE"]);

                if (dt.Rows[0]["EM_POLICE_VERI"].ToString() != "")
                    EM_POLICE_VERI = Convert.ToBoolean(dt.Rows[0]["EM_POLICE_VERI"].ToString());

                if (dt.Rows[0]["EM_OTM"].ToString() != "")
                    EM_OTM = Convert.ToBoolean(dt.Rows[0]["EM_OTM"].ToString());

                EM_IS_ONROLE = Convert.ToBoolean(dt.Rows[0]["EM_IS_ONROLE"].ToString());
                if (dt.Rows[0]["EM_IS_APROVED"].ToString() != "")
                    EM_IS_APROVED = Convert.ToBoolean(dt.Rows[0]["EM_IS_APROVED"].ToString());
                if (dt.Rows[0]["EM_POLICE_VERI"].ToString() != "")
                    EM_POLICE_VERI = Convert.ToBoolean(dt.Rows[0]["EM_POLICE_VERI"].ToString());

                if (dt.Rows[0]["EM_IS_SAL_INCREMENT"].ToString() != "")
                    EM_IS_SAL_INCREMENT = Convert.ToBoolean(dt.Rows[0]["EM_IS_SAL_INCREMENT"].ToString());

                EM_PT = Convert.ToBoolean(dt.Rows[0]["EM_PT"].ToString());
                EM_IS_CONFIRM = Convert.ToBoolean(dt.Rows[0]["EM_IS_CONFIRM"].ToString());
                if (dt.Rows[0]["EM_CONFIRM_DATE"].ToString() != "")
                {
                    EM_CONFIRM_DATE = Convert.ToDateTime(dt.Rows[0]["EM_CONFIRM_DATE"].ToString());
                }



                EM_IsUser = Convert.ToBoolean(dt.Rows[0]["EM_IsUser"].ToString());

                if (dt.Rows[0]["EM_TEMP_LEFT"].ToString() != "")
                    EM_TEMP_LEFT = Convert.ToBoolean(dt.Rows[0]["EM_TEMP_LEFT"].ToString());
                if (dt.Rows[0]["EM_TEMP_LEFT_DATE"].ToString() != "")
                {
                    EM_TEMP_LEFT_DATE = Convert.ToDateTime(dt.Rows[0]["EM_TEMP_LEFT_DATE"].ToString()).ToString("dd/MMM/yyyy");
                }
                EM_OT_MULT = Convert.ToDouble(dt.Rows[0]["EM_OT_MULT"].ToString());
                if (Convert.ToBoolean(dt.Rows[0]["EM_IsUser"].ToString()))
                {
                    DataTable dtGetUser = CommonClasses.Execute("Select UM_CODE, UM_CM_ID, UM_BM_CODE, UM_DM_CODE, UM_EM_CODE, UM_USERNAME, UM_PASSWORD, UM_LEVEL, UM_LASTLOGIN_DATETIME, UM_IP_ADDRESS, UM_ACTIVE_IND, UM_EMAIL_SEND, UM_LOGIN_FLAG, UM_DELETE_FLAG, UM_MODIFY_FLAG, UM_IS_ADMIN from CM_USER_MASTER where UM_BM_CODE=" + dt.Rows[0]["EM_BM_CODE"] + " and UM_CM_ID=" + dt.Rows[0]["EM_CM_COMP_ID"] + " and UM_EM_CODE=" + dt.Rows[0]["EM_CODE"] + " and UM_DELETE_FLAG=0");
                    UM_CODE = Convert.ToInt32(dtGetUser.Rows[0]["UM_CODE"]);
                    UM_BM_CODE = Convert.ToInt32(dtGetUser.Rows[0]["UM_BM_CODE"]);
                    UM_EM_CODE = Convert.ToInt32(dtGetUser.Rows[0]["UM_EM_CODE"]);
                    UM_USERNAME = dtGetUser.Rows[0]["UM_USERNAME"].ToString();
                    UM_PASSWORD = dtGetUser.Rows[0]["UM_PASSWORD"].ToString();
                    UM_LEVEL = dtGetUser.Rows[0]["UM_LEVEL"].ToString();
                    UM_ACTIVE_IND = Convert.ToBoolean(dtGetUser.Rows[0]["UM_ACTIVE_IND"].ToString());
                    UM_IS_ADMIN = Convert.ToBoolean(dtGetUser.Rows[0]["UM_IS_ADMIN"].ToString());

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
            //SqlParameter[] par = new SqlParameter[3];
            //par[0] = new SqlParameter("@EM_NAME", EM_NAME);
            //par[1] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            //par[2] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
            //dt = DL_DBAccess.SelectData("SP_HR_CheckSaveEmp", par);
            dt = CommonClasses.Execute("SELECT * FROM HR_EMPLOYEE_MASTER WHERE  EM_CM_COMP_ID='" + EM_CM_COMP_ID + "' and EM_DELETE_FLAG = 0 AND EM_NAME = '" + EM_NAME + "' EM_M_NAME='" + EM_M_NAME + "' AND EM_L_NAME='" + EM_L_NAME + "' AND EM_JOINDATE = DateAdd(day, " + EM_JOINDATE.Day + " - 1,  DateAdd(month, " + EM_JOINDATE.Month + " - 1,  DateAdd(Year, " + EM_JOINDATE.Year + "-1900, 0))) AND EM_BIRTHDATE = DateAdd(day, " + EM_BIRTHDATE.Day + " - 1,  DateAdd(month, " + EM_BIRTHDATE.Month + " - 1,  DateAdd(Year, " + EM_BIRTHDATE.Year + "-1900, 0))) AND EM_IS_LEFT = 0");
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

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@DM_CODE", EM_CODE);
            par[1] = new SqlParameter("@DM_NAME", EM_NAME);
            par[2] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            par[3] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_CheckUpdateEmp", par);
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
    public bool Save(GridView XGridEarn, GridView XGridDeduct, GridView dgvworkExp,GridView dgvFamily, GridView dgvNominee, GridView dgEmpReference)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (CheckExistSaveName())
            {
                DataTable dtEmp = CommonClasses.Execute("select EM_TICKETNO from HR_EMPLOYEE_MASTER where EM_TICKETNO='" + EM_TICKETNO + "' and EM_DELETE_FLAG=0 and EM_CM_COMP_ID=" + EM_CM_COMP_ID + " ");
                if (dtEmp.Rows.Count == 0)
                {
                    SqlParameter[] par = new SqlParameter[82];

                    #region SqlParameter
                    par[0] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
                    par[1] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
                    par[2] = new SqlParameter("@EM_TICKETNO", EM_TICKETNO);
                    par[3] = new SqlParameter("@EM_APPLICATION_DATE", EM_APPLICATION_DATE);
                    par[4] = new SqlParameter("@EM_NAME", EM_NAME);
                    par[5] = new SqlParameter("@EM_M_NAME", EM_M_NAME);
                    par[6] = new SqlParameter("@EM_L_NAME", EM_L_NAME);
                    par[7] = new SqlParameter("@EM_FATHER_NAME", EM_FATHER_NAME);
                    par[8] = new SqlParameter("@EM_MOTHER_NAME_BEFORE", EM_MOTHER_NAME_BEFORE);
                    par[9] = new SqlParameter("@EM_MOTHER_NAME_AFTER", EM_MOTHER_NAME_AFTER);
                    par[10] = new SqlParameter("@EM_GENDER", EM_GENDER);
                    par[11] = new SqlParameter("@EM_MARTIAL", EM_MARTIAL);
                    par[12] = new SqlParameter("@EM_EMAIL_ID", EM_EMAIL_ID);
                    par[13] = new SqlParameter("@EM_CONTACTNO", EM_CONTACTNO);
                    par[14] = new SqlParameter("@EM_LAND_LINE", EM_LAND_LINE);
                    par[15] = new SqlParameter("@EM_BIRTHDATE", EM_BIRTHDATE);
                    par[16] = new SqlParameter("@EM_BLOOD_GROUP", EM_BLOOD_GROUP);
                    par[17] = new SqlParameter("@EM_ADR1", EM_ADR1);
                    par[18] = new SqlParameter("@EM_COUNTRY", EM_COUNTRY);
                    par[19] = new SqlParameter("@EM_STATE", EM_STATE);
                    par[20] = new SqlParameter("@EM_CITY", EM_CITY);
                    par[21] = new SqlParameter("@EM_PIN", EM_PIN);
                    par[22] = new SqlParameter("@EM_ADR2", EM_ADR2);
                    par[23] = new SqlParameter("@EM_COUNTRY1", EM_COUNTRY1);
                    par[24] = new SqlParameter("@EM_STATE1", EM_STATE1);
                    par[25] = new SqlParameter("@EM_CITY1", EM_CITY1);
                    par[26] = new SqlParameter("@EM_PIN1", EM_PIN1);
                    par[27] = new SqlParameter("@EM_NATIVE_PLACE", EM_NATIVE_PLACE);
                    par[28] = new SqlParameter("@EM_NATIONALITY", EM_NATIONALITY);
                    par[29] = new SqlParameter("@EM_RELIGION", EM_RELIGION);
                    par[30] = new SqlParameter("@EM_MONTHER_TONGUE", EM_MONTHER_TONGUE);
                    par[31] = new SqlParameter("@EM_DM_CODE", EM_DM_CODE);
                    par[32] = new SqlParameter("@EM_DPM_CODE", EM_DPM_CODE);
                    par[33] = new SqlParameter("@EM_GM_CODE", EM_GM_CODE);
                    par[34] = new SqlParameter("@EM_JOINDATE", EM_JOINDATE);
                    try
                    {
                        if (EM_IS_CONFIRM)
                        {
                            par[35] = new SqlParameter("@EM_CONFIRM_DATE", EM_CONFIRM_DATE);
                        }
                        else
                        {
                            par[35] = new SqlParameter("@EM_CONFIRM_DATE", DBNull.Value);
                        }
                    }
                    catch
                    {
                        par[35] = new SqlParameter("@EM_CONFIRM_DATE", DBNull.Value);
                    }
                    par[36] = new SqlParameter("@EM_WOFF", EM_WOFF);
                    par[37] = new SqlParameter("@EM_AADHAR", EM_AADHAR);
                    par[38] = new SqlParameter("@EM_PAN", EM_PAN);


                    if (EM_PF)
                    {
                        par[39] = new SqlParameter("@EM_PF", EM_PF);
                        par[40] = new SqlParameter("@EM_PF_DATE", EM_PF_DATE);
                        par[41] = new SqlParameter("@EM_PFNO", EM_PFNO);
                        par[42] = new SqlParameter("@EM_RELATION", EM_RELATION);
                        par[43] = new SqlParameter("@EM_FNAME", EM_FNAME);
                        par[44] = new SqlParameter("@EM_ISEmpPFSal", EM_ISEmpPFSal);
                        par[45] = new SqlParameter("@EM_PFEMP", EM_PFEMP);
                        par[46] = new SqlParameter("@EM_ISEmprPFSal", EM_ISEmprPFSal);
                        par[47] = new SqlParameter("@EM_PFEMPLOYER", EM_PFEMPLOYER);
                        par[48] = new SqlParameter("@EM_FPF", EM_FPF);
                    }
                    else
                    {
                        par[39] = new SqlParameter("@EM_PF", EM_PF);
                        par[40] = new SqlParameter("@EM_PF_DATE", DBNull.Value);
                        par[41] = new SqlParameter("@EM_PFNO", "0");
                        par[42] = new SqlParameter("@EM_RELATION", "");
                        par[43] = new SqlParameter("@EM_FNAME", "");
                        par[44] = new SqlParameter("@EM_ISEmpPFSal", "0");
                        par[45] = new SqlParameter("@EM_PFEMP", "0");
                        par[46] = new SqlParameter("@EM_ISEmprPFSal", "0");
                        par[47] = new SqlParameter("@EM_PFEMPLOYER", "0");
                        par[48] = new SqlParameter("@EM_FPF", "0");
                    }
                    par[49] = new SqlParameter("@EM_PT", EM_PT);
                    if (EM_ESI)
                    {
                        par[50] = new SqlParameter("@EM_ESI", EM_ESI);
                        par[51] = new SqlParameter("@EM_ESINO", EM_ESINO);
                        par[52] = new SqlParameter("@EM_ESI_DATE", EM_ESI_DATE);
                    }
                    else
                    {
                        par[50] = new SqlParameter("@EM_ESI", EM_ESI);
                        par[51] = new SqlParameter("@EM_ESINO", "0");
                        par[52] = new SqlParameter("@EM_ESI_DATE", DBNull.Value);
                    }
                    par[53] = new SqlParameter("@EM_LWF", EM_LWF);
                    par[54] = new SqlParameter("@EM_IS_LTA", EM_IS_LTA);
                    par[55] = new SqlParameter("@EM_IS_MED", EM_IS_MED);
                    par[56] = new SqlParameter("@EM_PAY_MODE", EM_PAY_MODE);
                    par[57] = new SqlParameter("@EM_BANK", EM_BANK);
                    par[58] = new SqlParameter("@EM_BANK_AC_NO", EM_BANK_AC_NO);
                    par[59] = new SqlParameter("@EM_IFSC_CODE", EM_IFSC_CODE);


                    par[60] = new SqlParameter("@EM_PAYIND", EM_PAYIND);
                    par[61] = new SqlParameter("@EM_PAYSLIP", EM_PAYSLIP);

                    par[62] = new SqlParameter("@EM_CTC_TOTEAR", EM_CTC_TOTEAR);
                    par[63] = new SqlParameter("@EM_CTC_TOTDED", EM_CTC_TOTDED);
                    par[64] = new SqlParameter("@EM_CTC_NETTOT", EM_CTC_NETTOT);
                    par[65] = new SqlParameter("@EM_CTC_MEDICAL", EM_CTC_MEDICAL);
                    par[66] = new SqlParameter("@EM_CTC_LTA", EM_CTC_LTA);
                    par[67] = new SqlParameter("@EM_CTC_EXTRA_HRA", EM_CTC_EXTRA_HRA);
                    par[68] = new SqlParameter("@EM_CTC_PF", EM_CTC_PF);
                    par[69] = new SqlParameter("@EM_CTC_ESIC", EM_CTC_ESIC);
                    par[70] = new SqlParameter("@EM_CTC_GRADUITY", EM_CTC_GRADUITY);
                    par[71] = new SqlParameter("@EM_CTC_BONUS", EM_CTC_BONUS);
                    par[72] = new SqlParameter("@EM_CTC_EL", EM_CTC_EL);
                    par[73] = new SqlParameter("@EM_CTC_TOTAL", EM_CTC_TOTAL);
                    par[74] = new SqlParameter("@EM_CTC_MONTHLYTOT", EM_CTC_MONTHLYTOT);
                    par[75] = new SqlParameter("@EM_CTC_PA", EM_CTC_PA);

                    par[76] = new SqlParameter("@EM_IS_CONFIRM", EM_IS_CONFIRM);
                    par[77] = new SqlParameter("@EM_OTM", EM_OTM);
                    par[78] = new SqlParameter("@EM_POLICE_VERI", EM_POLICE_VERI);
                    par[79] = new SqlParameter("@EM_IsUser", EM_IsUser);
                    par[80] = new SqlParameter("@EM_AGE", EM_AGE);
                    par[81] = new SqlParameter("@EM_OT_MULT", EM_OT_MULT);
                    #endregion


                    result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmp", par);
                    if (result)
                    {
                        string Code = "";
                        string query = "Select max(EM_CODE) as Code from HR_EMPLOYEE_MASTER where EM_CM_COMP_ID=" + EM_CM_COMP_ID + " and EM_DELETE_FLAG=0 AND EM_BM_CODE=" + EM_BM_CODE + "";
                        SqlParameter[] par1 = new SqlParameter[1];
                        par1[0] = new SqlParameter("@Query", query);
                        DataTable dtCode = DL_DBAccess.SelectData("SP_MaxID", par1);
                        if (dtCode.Rows.Count > 0)
                            Code = dtCode.Rows[0]["Code"].ToString();

                        #region Save All Grid
                        if (SaveEarnings(XGridDeduct, XGridEarn, Code))
                        {
                            result = true;
                        }
                        if (SaveWorkExperience(Code, dgvworkExp))
                        {
                            result = true;
                        }
                        //if (SaveEdcuation(Code, dgvEdcuation))
                        //{
                        //    result = true;
                        //}
                        if (SaveFamily(Code, dgvFamily))
                        {
                            result = true;
                        }
                        if (SaveNominees(Code, dgvNominee))
                        {
                            result = true;
                        }
                        if (SaveReference(Code, dgEmpReference))
                        {
                            result = true;
                        }
                        #endregion

                        //if (EM_IsUser)
                        //{
                        //    if (SaveUserMaster(chkLst, CheckBoxListNoti, Code))
                        //    {
                        //        sendmail();


                        //        result = true;
                        //    }
                        //}

                        //if (result == true)
                        //{

                        //    if (EM_IS_CONFIRM == true)
                        //    {
                        //        if (EM_CL > 0 || EM_SL > 0 || EM_EL > 0)
                        //        {
                        //            EM_CL = EM_SL = EM_EL = 0;
                        //            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + Code + "," + Code + "," + EM_CL + ",'CL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                        //            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + Code + "," + Code + "," + EM_SL + ",'SL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                        //            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + Code + "," + Code + "," + EM_EL + ",'EL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                        //        }
                        //    }
                        //}
                    }
                }
                else
                {
                    Msg = "Employee No Already Exist";
                    result = false;
                }
            }
            else
            {
                Msg = "Employee Already Exist";
                result = false;
            }
        }
        catch (Exception Ex)
        {
        }
        return result;
    }

    #endregion

    #region SaveConfirmationDates
    public bool SaveConfirmationDates(string Code, GridView dgConfirmDate)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgConfirmDate.Rows.Count > 0)
        {
            for (int i = 0; i < dgConfirmDate.Rows.Count; i++)
            {
                string Confirmdate = ((TextBox)(dgConfirmDate.Rows[i].FindControl("txtCOnfirm"))).Text;

                SqlParameter[] par = new SqlParameter[1];
                par[0] = new SqlParameter("@Query", "Insert into HR_EMPLOYEE_CONFIRMATION (EC_EM_CODE,EC_UM_CODE,EC_PROBATION_DATE) values(" + Code + "," + UserCode + ",'" + Confirmdate + "')");
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_Execute", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveEarnings
    public bool SaveEarnings(GridView XGridDeduct, GridView XGridEarn, string Code)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        if (EM_IS_SAL_INCREMENT == false)
        {
            if (XGridEarn.Rows.Count > 0)
            {
                for (int i = 0; i < XGridEarn.Rows.Count; i++)
                {
                    Label lbledm_code = (XGridEarn.Rows[i].FindControl("lblEDM_CODE") as Label);
                    string EDM_CODE = lbledm_code.Text;
                    TextBox txtedm_amt = (XGridEarn.Rows[i].FindControl("txtEarningAmmount") as TextBox);
                    string EMD_EDM_AMOUNT = txtedm_amt.Text;
                    Label lbledm_type = (XGridEarn.Rows[i].FindControl("lblEMD_EDM_TYPE") as Label);
                    string EMD_EDM_TYPE = lbledm_type.Text;

                    SqlParameter[] par = new SqlParameter[6];
                    par[0] = new SqlParameter("@EMD_EM_CODE", Code);
                    par[1] = new SqlParameter("@EMD_EDM_CODE", EDM_CODE);
                    par[2] = new SqlParameter("@EMD_EDM_AMOUNT", EMD_EDM_AMOUNT);
                    par[3] = new SqlParameter("@EMD_EDM_TYPE", EMD_EDM_TYPE);
                    par[4] = new SqlParameter("@EMD_WEF_DATE", EM_JOINDATE.ToString("dd/MMM/yyyy"));
                    par[5] = new SqlParameter("@EMD_INCREMENT_DATE", EM_JOINDATE.ToString("dd/MMM/yyyy"));

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpEarnDeduct", par);
                }
            }
        }
        if (XGridDeduct.Rows.Count > 0)
        {
            for (int i = 0; i < XGridDeduct.Rows.Count; i++)
            {
                Label lbledm_code1 = (XGridDeduct.Rows[i].FindControl("lblEDM_CODE1") as Label);
                string EDM_CODE1 = lbledm_code1.Text;
                TextBox txtedm_amt1 = (XGridDeduct.Rows[i].FindControl("txtDeductionAmount") as TextBox);
                string EMD_EDM_AMOUNT1 = txtedm_amt1.Text;
                Label lbledm_type1 = (XGridDeduct.Rows[i].FindControl("lblEMD_EDM_TYPE1") as Label);
                string EMD_EDM_TYPE1 = lbledm_type1.Text;

                SqlParameter[] par = new SqlParameter[6];
                par[0] = new SqlParameter("@EMD_EM_CODE", Code);
                par[1] = new SqlParameter("@EMD_EDM_CODE", EDM_CODE1);
                par[2] = new SqlParameter("@EMD_EDM_AMOUNT", EMD_EDM_AMOUNT1);
                par[3] = new SqlParameter("@EMD_EDM_TYPE", EMD_EDM_TYPE1);
                par[4] = new SqlParameter("@EMD_WEF_DATE", EM_JOINDATE.ToString("dd/MMM/yyyy"));
                par[5] = new SqlParameter("@EMD_INCREMENT_DATE", EM_JOINDATE.ToString("dd/MMM/yyyy"));
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpEarnDeduct", par);
            }
        }
        if (EM_IS_SAL_INCREMENT == false)
        {
            if (result)
            {
                string Query = "Insert into HR_EMPLOYEE_BRANCH_DETAILS(EMBM_EM_CODE,EMBM_BM_CODE,EMBM_INCREMENT_DATE,EMBM_WEF_DATE)values(" + Code + "," + EM_BM_CODE + ",'" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "','" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "')";
                SqlParameter[] par = new SqlParameter[1];
                par[0] = new SqlParameter("@Query", Query);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_Execute", par);
            }
            if (result)
            {
                string Query = "Insert into HR_EMPLOYEE_DEPARTMENT_DETAILS(EMDM_EM_CODE,EMDM_DM_CODE,EMDM_INCREMENT_DATE,EMDM_WEF_DATE)values(" + Code + "," + EM_DPM_CODE + ",'" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "','" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "')";
                SqlParameter[] par = new SqlParameter[1];
                par[0] = new SqlParameter("@Query", Query);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_Execute", par);
            }
            if (result)
            {
                string Query = "Insert into HR_EMPLOYEE_DESIGNATION_DETAILS(EMDS_EM_CODE,EMDS_DS_CODE,EMDS_INCREMENT_DATE,EMDS_WEF_DATE)values(" + Code + "," + EM_DM_CODE + ",'" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "','" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "')";
                SqlParameter[] par = new SqlParameter[1];
                par[0] = new SqlParameter("@Query", Query);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_Execute", par);
            }
        }

        return result;
    }
    #endregion

    #region SaveWorkExperience
    public bool SaveWorkExperience(string Code, GridView dgvworkExp)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgvworkExp.Rows.Count > 0)
        {
            for (int i = 0; i < dgvworkExp.Rows.Count; i++)
            {
                string EMWE_COMPANY_NAME = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtCompanyName"))).Text;
                string EMWE_POSITION = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtPosition"))).Text;
                string EMWE_FROM_DATE = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtFromDate"))).Text;
                string EMWE_TO_DATE = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtToDate"))).Text;

                string EMWE_LASTCTC = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtLastCTC"))).Text;
                string EMWE_REASON = ((TextBox)(dgvworkExp.Rows[i].FindControl("txtReasonForLeave"))).Text;

                SqlParameter[] par = new SqlParameter[7];
                par[0] = new SqlParameter("@EMWE_EM_CODE", Code);
                par[1] = new SqlParameter("@EMWE_COMPANY_NAME", EMWE_COMPANY_NAME);
                par[2] = new SqlParameter("@EMWE_POSITION", EMWE_POSITION);
                par[3] = new SqlParameter("@EMWE_FROM_DATE", EMWE_FROM_DATE);
                par[4] = new SqlParameter("@EMWE_TO_DATE", EMWE_TO_DATE);
                par[5] = new SqlParameter("@EMWE_LASTCTC", EMWE_LASTCTC);
                par[6] = new SqlParameter("@EMWE_REASON", EMWE_REASON);

                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpworkex", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveEdcuation
    public bool SaveEdcuation(string Code, GridView dgvEdcuation)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgvEdcuation.Rows.Count > 0)
        {
            for (int i = 0; i < dgvEdcuation.Rows.Count; i++)
            {
                string EME_EDUCATION_TYPE = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtEducationType"))).Text;
                string EME_BOARD = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtBoard"))).Text;
                string EME_COURSE = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtCourse"))).Text;
                string EME_PASSING = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtPassingYear"))).Text;

                string EME_MARKS = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtMarksObtain"))).Text;
                string EME_GRADE = ((TextBox)(dgvEdcuation.Rows[i].FindControl("txtGrade"))).Text;

                SqlParameter[] par = new SqlParameter[7];
                par[0] = new SqlParameter("@EME_EM_CODE", Code);
                par[1] = new SqlParameter("@EME_EDUCATION_TYPE", EME_EDUCATION_TYPE);
                par[2] = new SqlParameter("@EME_BOARD", EME_BOARD);
                par[3] = new SqlParameter("@EME_COURSE", EME_COURSE);
                par[4] = new SqlParameter("@EME_PASSING", EME_PASSING);
                par[5] = new SqlParameter("@EME_MARKS", EME_MARKS);
                par[6] = new SqlParameter("@EME_GRADE", EME_GRADE);

                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpEducation", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveFamily
    public bool SaveFamily(string Code, GridView dgvFamily)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgvFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dgvFamily.Rows.Count; i++)
            {
                string EMF_MEM_NAME = ((TextBox)(dgvFamily.Rows[i].FindControl("txtFamilyMemberName"))).Text;
                string EMF_RELATION = ((TextBox)(dgvFamily.Rows[i].FindControl("txtFamilyRelationship"))).Text;
                string EMF_DOB = ((TextBox)(dgvFamily.Rows[i].FindControl("txtfamilyDateofBirth"))).Text;


                SqlParameter[] par = new SqlParameter[4];
                par[0] = new SqlParameter("@EMF_EM_CODE", Code);
                par[1] = new SqlParameter("@EMF_MEM_NAME", EMF_MEM_NAME);
                par[2] = new SqlParameter("@EMF_RELATION", EMF_RELATION);
                par[3] = new SqlParameter("@EMF_DOB", EMF_DOB);

                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpFamily", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveNominees
    public bool SaveNominees(string Code, GridView dgEmpNominess)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgEmpNominess.Rows.Count > 0)
        {
            for (int i = 0; i < dgEmpNominess.Rows.Count; i++)
            {
                string EMN_NAME = ((TextBox)(dgEmpNominess.Rows[i].FindControl("txtNameofNominee"))).Text;
                string EMN_RELATION = ((TextBox)(dgEmpNominess.Rows[i].FindControl("txtRelationship"))).Text;
                string EMN_DOB = ((TextBox)(dgEmpNominess.Rows[i].FindControl("txtDateofBirth"))).Text;
                string EMN_AGE = ((TextBox)(dgEmpNominess.Rows[i].FindControl("txtAgeofNominee"))).Text;
                bool EMN_FOR_GRADUITY = ((CheckBox)(dgEmpNominess.Rows[i].FindControl("chkGraduity"))).Checked;
                bool EMN_FOR_PF = ((CheckBox)(dgEmpNominess.Rows[i].FindControl("chkPF"))).Checked;
                bool EMN_FOR_PF_TP = ((CheckBox)(dgEmpNominess.Rows[i].FindControl("chkPFThirdParty"))).Checked;

                SqlParameter[] par = new SqlParameter[8];
                par[0] = new SqlParameter("@EMN_EM_CODE", Code);
                par[1] = new SqlParameter("@EMN_NAME", EMN_NAME);
                par[2] = new SqlParameter("@EMN_RELATION", EMN_RELATION);
                par[3] = new SqlParameter("@EMN_DOB", EMN_DOB);
                par[4] = new SqlParameter("@EMN_AGE", EMN_AGE);
                par[5] = new SqlParameter("@EMN_FOR_GRADUITY", EMN_FOR_GRADUITY);
                par[6] = new SqlParameter("@EMN_FOR_PF", EMN_FOR_PF);
                par[7] = new SqlParameter("@EMN_FOR_PF_TP", EMN_FOR_PF_TP);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertEmpNominee", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveReference
    public bool SaveReference(string Code, GridView dgEmpReference)
    {
        bool result = false;
        DataTable dt1 = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        if (dgEmpReference.Rows.Count > 0)
        {
            for (int i = 0; i < dgEmpReference.Rows.Count; i++)
            {
                string EMR_NAME = ((TextBox)(dgEmpReference.Rows[i].FindControl("txtName"))).Text;
                string EMR_COMPANY_NAME = ((TextBox)(dgEmpReference.Rows[i].FindControl("txtCompanyName"))).Text;
                string EMR_RELATION = ((TextBox)(dgEmpReference.Rows[i].FindControl("txtRelation"))).Text;
                string EMR_CONTACT_NO = ((TextBox)(dgEmpReference.Rows[i].FindControl("txtContactNo"))).Text;


                SqlParameter[] par = new SqlParameter[5];
                par[0] = new SqlParameter("@EMR_EM_CODE", Code);
                par[1] = new SqlParameter("@EMR_NAME", EMR_NAME);
                par[2] = new SqlParameter("@EMR_COMPANY_NAME", EMR_COMPANY_NAME);
                par[3] = new SqlParameter("@EMR_RELATION", EMR_RELATION);
                par[4] = new SqlParameter("@EMR_CONTACT_NO", EMR_CONTACT_NO);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertReferance", par);
            }
        }
        return result;
    }
    #endregion

    #region SaveUserMaster
    public bool SaveUserMaster(CheckBoxList chkLst, CheckBoxList CheckBoxListNoti, string Code)
    {
        bool result = false;
        try
        {
            if (CheckExistUserName())
            {
                DL_DBAccess = new DatabaseAccessLayer();
                SqlParameter[] par = new SqlParameter[10];
                par[0] = new SqlParameter("@UM_CM_ID", UM_CM_ID);
                par[1] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
                par[2] = new SqlParameter("@UM_EM_CODE", Code);
                par[3] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
                par[4] = new SqlParameter("@UM_PASSWORD", UM_PASSWORD);
                par[5] = new SqlParameter("@UM_LEVEL", UM_LEVEL);
                par[6] = new SqlParameter("@UM_LASTLOGIN_DATETIME", UM_LASTLOGIN_DATETIME);
                par[7] = new SqlParameter("@UM_IP_ADDRESS", UM_IP_ADDRESS);
                par[8] = new SqlParameter("@UM_ACTIVE_IND", UM_ACTIVE_IND);
                par[9] = new SqlParameter("@UM_IS_ADMIN", UM_IS_ADMIN);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_InsertUser", par);
                if (result == true)
                {
                    DataTable dt = CommonClasses.Execute("SELECT Max(UM_CODE) FROM CM_USER_MASTER");
                    foreach (ListItem li in chkLst.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UMD_UM_CODE", dt.Rows[0][0]);
                            par1[1] = new SqlParameter("@UMD_BM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_UserDetail", par1);
                        }
                    }
                    foreach (ListItem li in CheckBoxListNoti.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UND_UM_CODE", dt.Rows[0][0]);
                            par1[1] = new SqlParameter("@UND_NM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_USERNOTIFICATION", par1);
                        }
                    }
                }
            }
            else
            {
                Msg = "User Name Already Exist";
            }
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    #endregion Save

    #region UpdateUserMaster
    public bool UpdateUserMaster(CheckBoxList chkLst, CheckBoxList CheckBoxListNoti, string Code)
    {
        bool result = false;
        try
        {
            DataTable dtGetUser = CommonClasses.Execute("Select UM_CODE from CM_USER_MASTER where UM_BM_CODE=" + EM_BM_CODE + " and UM_CM_ID=" + EM_CM_COMP_ID + " and UM_EM_CODE=" + Code + " and UM_DELETE_FLAG=0");
            if (dtGetUser.Rows.Count != 0)
            {
                UM_CODE = int.Parse(dtGetUser.Rows[0]["UM_CODE"].ToString());
            }
            //if (ChkExstUpdateUserName())
            //{
            //DL_DBAccess = new DatabaseAccessLayer();
            //SqlParameter[] par = new SqlParameter[10];
            //par[0] = new SqlParameter("@UM_CODE", UM_CODE);
            //par[1] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
            //par[2] = new SqlParameter("@UM_EM_CODE", Code);
            //par[3] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
            //par[4] = new SqlParameter("@UM_PASSWORD", UM_PASSWORD);
            //par[5] = new SqlParameter("@UM_LEVEL", UM_LEVEL);
            //par[6] = new SqlParameter("@UM_LASTLOGIN_DATETIME", UM_LASTLOGIN_DATETIME);
            //par[7] = new SqlParameter("@UM_IP_ADDRESS", UM_IP_ADDRESS);
            //par[8] = new SqlParameter("@UM_ACTIVE_IND", UM_ACTIVE_IND);
            //par[9] = new SqlParameter("@UM_IS_ADMIN", UM_IS_ADMIN);
            //result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_UpdateUser", par);
            //if (result == true)
            //{
            CommonClasses.Execute("DELETE FROM CM_USER_MASTER_DETAIL WHERE UMD_UM_CODE=" + UM_CODE + "");
            CommonClasses.Execute("DELETE FROM CM_USER_MASTER_NOTIFICATION WHERE UND_UM_CODE=" + UM_CODE + "");
            foreach (ListItem li in chkLst.Items)
            {
                if (li.Selected)
                {
                    DL_DBAccess = new DatabaseAccessLayer();
                    SqlParameter[] par1 = new SqlParameter[2];
                    par1[0] = new SqlParameter("@UMD_UM_CODE", UM_CODE);
                    par1[1] = new SqlParameter("@UMD_BM_CODE", li.Value.ToString());
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_UserDetail", par1);
                }
            }
            foreach (ListItem li in CheckBoxListNoti.Items)
            {
                if (li.Selected)
                {
                    DL_DBAccess = new DatabaseAccessLayer();
                    SqlParameter[] par1 = new SqlParameter[2];
                    par1[0] = new SqlParameter("@UND_UM_CODE", UM_CODE);
                    par1[1] = new SqlParameter("@UND_NM_CODE", li.Value.ToString());
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_USERNOTIFICATION", par1);
                }
            }
            // }
            //}
            //else
            //{
            //    Msg = "User Name Already Exist";
            //}

        }
        catch (Exception Ex)
        {
        }
        return result;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateUserName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {

            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@UM_CODE", UM_CODE);
            par[1] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
            dt = DL_DBAccess.SelectData("[SP_CM_CheckUpdateUser]", par);
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

    #region CheckExistSaveName
    public bool CheckExistUserName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
            dt = DL_DBAccess.SelectData("SP_CM_CheckSaveUser", par);
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


    #region Update
    public bool Update(GridView XGridEarn, GridView XGridDeduct, GridView dgvworkExp, GridView dgvFamily, GridView dgvNominee, GridView dgEmpReference)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[83];
            par[0] = new SqlParameter("@EM_CODE", EM_CODE);
            par[1] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            par[2] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
            par[3] = new SqlParameter("@EM_TICKETNO", EM_TICKETNO);
            par[4] = new SqlParameter("@EM_APPLICATION_DATE", EM_APPLICATION_DATE);
            par[5] = new SqlParameter("@EM_NAME", EM_NAME);
            par[6] = new SqlParameter("@EM_M_NAME", EM_M_NAME);
            par[7] = new SqlParameter("@EM_L_NAME", EM_L_NAME);
            par[8] = new SqlParameter("@EM_FATHER_NAME", EM_FATHER_NAME);
            par[9] = new SqlParameter("@EM_MOTHER_NAME_BEFORE", EM_MOTHER_NAME_BEFORE);
            par[10] = new SqlParameter("@EM_MOTHER_NAME_AFTER", EM_MOTHER_NAME_AFTER);
            par[11] = new SqlParameter("@EM_GENDER", EM_GENDER);
            par[12] = new SqlParameter("@EM_MARTIAL", EM_MARTIAL);
            par[13] = new SqlParameter("@EM_EMAIL_ID", EM_EMAIL_ID);
            par[14] = new SqlParameter("@EM_CONTACTNO", EM_CONTACTNO);
            par[15] = new SqlParameter("@EM_LAND_LINE", EM_LAND_LINE);
            par[16] = new SqlParameter("@EM_BIRTHDATE", EM_BIRTHDATE);
            par[17] = new SqlParameter("@EM_BLOOD_GROUP", EM_BLOOD_GROUP);
            par[18] = new SqlParameter("@EM_ADR1", EM_ADR1);
            par[19] = new SqlParameter("@EM_COUNTRY", EM_COUNTRY);
            par[20] = new SqlParameter("@EM_STATE", EM_STATE);
            par[21] = new SqlParameter("@EM_CITY", EM_CITY);
            par[22] = new SqlParameter("@EM_PIN", EM_PIN);
            par[23] = new SqlParameter("@EM_ADR2", EM_ADR2);
            par[24] = new SqlParameter("@EM_COUNTRY1", EM_COUNTRY1);
            par[25] = new SqlParameter("@EM_STATE1", EM_STATE1);
            par[26] = new SqlParameter("@EM_CITY1", EM_CITY1);
            par[27] = new SqlParameter("@EM_PIN1", EM_PIN1);
            par[28] = new SqlParameter("@EM_NATIVE_PLACE", EM_NATIVE_PLACE);
            par[29] = new SqlParameter("@EM_NATIONALITY", EM_NATIONALITY);
            par[30] = new SqlParameter("@EM_RELIGION", EM_RELIGION);
            par[31] = new SqlParameter("@EM_MONTHER_TONGUE", EM_MONTHER_TONGUE);
            par[32] = new SqlParameter("@EM_DM_CODE", EM_DM_CODE);
            par[33] = new SqlParameter("@EM_DPM_CODE", EM_DPM_CODE);
            par[34] = new SqlParameter("@EM_GM_CODE", EM_GM_CODE);
            par[35] = new SqlParameter("@EM_JOINDATE", EM_JOINDATE);

            try
            {
                if (EM_IS_CONFIRM)
                {
                    par[36] = new SqlParameter("@EM_CONFIRM_DATE", EM_CONFIRM_DATE);
                }
                else
                {
                    par[36] = new SqlParameter("@EM_CONFIRM_DATE", DBNull.Value);
                }
            }
            catch
            {
                par[36] = new SqlParameter("@EM_CONFIRM_DATE", DBNull.Value);
            }

            par[37] = new SqlParameter("@EM_WOFF", EM_WOFF);
            par[38] = new SqlParameter("@EM_AADHAR", EM_AADHAR);
            par[39] = new SqlParameter("@EM_PAN", EM_PAN);


            if (EM_PF)
            {
                par[40] = new SqlParameter("@EM_PF", EM_PF);
                par[41] = new SqlParameter("@EM_PF_DATE", EM_PF_DATE);
                par[42] = new SqlParameter("@EM_PFNO", EM_PFNO);
                par[43] = new SqlParameter("@EM_RELATION", EM_RELATION);
                par[44] = new SqlParameter("@EM_FNAME", EM_FNAME);
                par[45] = new SqlParameter("@EM_ISEmpPFSal", EM_ISEmpPFSal);
                par[46] = new SqlParameter("@EM_PFEMP", EM_PFEMP);
                par[47] = new SqlParameter("@EM_ISEmprPFSal", EM_ISEmprPFSal);
                par[48] = new SqlParameter("@EM_PFEMPLOYER", EM_PFEMPLOYER);
                par[49] = new SqlParameter("@EM_FPF", EM_FPF);
            }
            else
            {
                par[40] = new SqlParameter("@EM_PF", EM_PF);
                par[41] = new SqlParameter("@EM_PF_DATE", DBNull.Value);
                par[42] = new SqlParameter("@EM_PFNO", "0");
                par[43] = new SqlParameter("@EM_RELATION", "");
                par[44] = new SqlParameter("@EM_FNAME", "");
                par[45] = new SqlParameter("@EM_ISEmpPFSal", "0");
                par[46] = new SqlParameter("@EM_PFEMP", "0");
                par[47] = new SqlParameter("@EM_ISEmprPFSal", "0");
                par[48] = new SqlParameter("@EM_PFEMPLOYER", "0");
                par[49] = new SqlParameter("@EM_FPF", "0");
            }
            par[50] = new SqlParameter("@EM_PT", EM_PT);
            if (EM_ESI)
            {
                par[51] = new SqlParameter("@EM_ESI", EM_ESI);
                par[52] = new SqlParameter("@EM_ESINO", EM_ESINO);
                par[53] = new SqlParameter("@EM_ESI_DATE", EM_ESI_DATE);
            }
            else
            {
                par[51] = new SqlParameter("@EM_ESI", EM_ESI);
                par[53] = new SqlParameter("@EM_ESINO", "0");
                par[53] = new SqlParameter("@EM_ESI_DATE", DBNull.Value);
            }
            par[54] = new SqlParameter("@EM_LWF", EM_LWF);
            par[55] = new SqlParameter("@EM_IS_LTA", EM_IS_LTA);
            par[56] = new SqlParameter("@EM_IS_MED", EM_IS_MED);
            par[57] = new SqlParameter("@EM_PAY_MODE", EM_PAY_MODE);
            par[58] = new SqlParameter("@EM_BANK", EM_BANK);
            par[59] = new SqlParameter("@EM_BANK_AC_NO", EM_BANK_AC_NO);
            par[60] = new SqlParameter("@EM_IFSC_CODE", EM_IFSC_CODE);
            
            par[61] = new SqlParameter("@EM_PAYIND", EM_PAYIND);
            par[62] = new SqlParameter("@EM_PAYSLIP", EM_PAYSLIP);
            par[63] = new SqlParameter("@EM_CTC_TOTEAR", EM_CTC_TOTEAR);
            par[64] = new SqlParameter("@EM_CTC_TOTDED", EM_CTC_TOTDED);
            par[65] = new SqlParameter("@EM_CTC_NETTOT", EM_CTC_NETTOT);
            par[66] = new SqlParameter("@EM_CTC_MEDICAL", EM_CTC_MEDICAL);
            par[67] = new SqlParameter("@EM_CTC_LTA", EM_CTC_LTA);
            par[68] = new SqlParameter("@EM_CTC_EXTRA_HRA", EM_CTC_EXTRA_HRA);
            par[69] = new SqlParameter("@EM_CTC_PF", EM_CTC_PF);
            par[70] = new SqlParameter("@EM_CTC_ESIC", EM_CTC_ESIC);
            par[71] = new SqlParameter("@EM_CTC_GRADUITY", EM_CTC_GRADUITY);
            par[72] = new SqlParameter("@EM_CTC_BONUS", EM_CTC_BONUS);
            par[73] = new SqlParameter("@EM_CTC_EL", EM_CTC_EL);
            par[74] = new SqlParameter("@EM_CTC_TOTAL", EM_CTC_TOTAL);
            par[75] = new SqlParameter("@EM_CTC_MONTHLYTOT", EM_CTC_MONTHLYTOT);
            par[76] = new SqlParameter("@EM_CTC_PA", EM_CTC_PA);

            par[77] = new SqlParameter("@EM_IS_CONFIRM", EM_IS_CONFIRM);
            par[78] = new SqlParameter("@EM_OTM", EM_OTM);
            par[79] = new SqlParameter("@EM_POLICE_VERI", EM_POLICE_VERI);
            par[80] = new SqlParameter("EM_IsUser", EM_IsUser);
            par[81] = new SqlParameter("@EM_AGE", EM_AGE);
            par[82] = new SqlParameter("@EM_OT_MULT", EM_OT_MULT);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_UpdateEmp", par);
            if (result)
            {
                DataTable dtSalIncr = CommonClasses.Execute("select isnull(EM_IS_SAL_INCREMENT,0) as EM_IS_SAL_INCREMENT from HR_EMPLOYEE_MASTER where EM_CODE=" + EM_CODE + " and EM_DELETE_FLAG=0");
                if (dtSalIncr.Rows.Count > 0)
                {
                    if (dtSalIncr.Rows[0]["EM_IS_SAL_INCREMENT"].ToString() != "")
                        EM_IS_SAL_INCREMENT = Convert.ToBoolean(dtSalIncr.Rows[0]["EM_IS_SAL_INCREMENT"].ToString());
                }
                else
                {
                    EM_IS_SAL_INCREMENT = false;
                }
                if (EM_IS_SAL_INCREMENT == false)
                {
                    string query = "Delete from HR_EMPLOYEE_EARNING_DETAILS where EMD_EM_CODE=" + EM_CODE + " and EMD_WEF_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' and EMD_INCREMENT_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' ";
                    SqlParameter[] par1 = new SqlParameter[1];
                    par1[0] = new SqlParameter("@Query", query);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par1);

                    string query1 = "Delete from HR_EMPLOYEE_BRANCH_DETAILS where EMBM_EM_CODE=" + EM_CODE + " and EMBM_WEF_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' and EMBM_INCREMENT_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' ";
                    SqlParameter[] par2 = new SqlParameter[1];
                    par2[0] = new SqlParameter("@Query", query1);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par2);

                    string query2 = "Delete from HR_EMPLOYEE_DEPARTMENT_DETAILS where EMDM_EM_CODE=" + EM_CODE + " and EMDM_WEF_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' and EMDM_INCREMENT_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' ";
                    SqlParameter[] par3 = new SqlParameter[1];
                    par3[0] = new SqlParameter("@Query", query2);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par3);

                    string query3 = "Delete from HR_EMPLOYEE_DESIGNATION_DETAILS where EMDS_EM_CODE=" + EM_CODE + " and EMDS_WEF_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' and EMDS_INCREMENT_DATE = '" + EM_JOINDATE.ToString("dd/MMM/yyyy") + "' ";
                    SqlParameter[] par4 = new SqlParameter[1];
                    par4[0] = new SqlParameter("@Query", query3);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par4);
                    if (SaveEarnings(XGridDeduct, XGridEarn, EM_CODE.ToString()))
                    {
                        result = true;
                    }
                }
                if (result == true)
                {
                    if (EM_IS_CONFIRM == true)
                    {
                        if (EM_CL > 0 || EM_SL > 0 || EM_EL > 0)
                        {
                            EM_CL = EM_SL = EM_EL = 0;
                            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + EM_CODE + "," + EM_CODE + "," + EM_CL + ",'CL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + EM_CODE + "," + EM_CODE + "," + EM_SL + ",'SL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                            CommonClasses.Execute("INSERT INTO HR_LEAVE_TRANSACTION(LET_CM_CODE,LET_DOC_CODE,LET_EM_CODE,LET_LEAVE_DAY,LET_LEAVE_TYPE,LET_TYPE,LET_DATE)VALUES(" + EM_CM_COMP_ID + "," + EM_CODE + "," + EM_CODE + "," + EM_EL + ",'EL','Leave Increment','" + EM_CONFIRM_DATE + "')");
                        }
                    }
                }


                string query4 = "Delete from HR_EMPLOYEE_WORKEX where EMWE_EM_CODE=" + EM_CODE + "";
                SqlParameter[] par5 = new SqlParameter[1];
                par5[0] = new SqlParameter("@Query", query4);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par5);
                if (SaveWorkExperience(EM_CODE.ToString(), dgvworkExp))
                {
                    result = true;
                }


                string query5 = "Delete from HR_EMPLOYEE_EDUCATION where EME_EM_CODE=" + EM_CODE + "";
                SqlParameter[] par6 = new SqlParameter[1];
                par6[0] = new SqlParameter("@Query", query5);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par6);
                //if (SaveEdcuation(EM_CODE.ToString(), dgvEdcuation))
                //{
                //    result = true;
                //}



                string query6 = "Delete from HR_EMPLOYEE_FAMILY where EMF_EM_CODE=" + EM_CODE + "";
                SqlParameter[] par7 = new SqlParameter[1];
                par7[0] = new SqlParameter("@Query", query6);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par7);
                if (SaveFamily(EM_CODE.ToString(), dgvFamily))
                {
                    result = true;
                }



                string query7 = "Delete from HR_EMPLOYEE_NOMINEE_DETAILS where EMN_EM_CODE=" + EM_CODE + "";
                SqlParameter[] par8 = new SqlParameter[1];
                par8[0] = new SqlParameter("@Query", query7);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par8);
                if (SaveNominees(EM_CODE.ToString(), dgvNominee))
                {
                    result = true;
                }


                string query8 = "Delete from HR_EMPLOYEE_REFERANCE where EMR_EM_CODE=" + EM_CODE + "";
                SqlParameter[] par9 = new SqlParameter[1];
                par9[0] = new SqlParameter("@Query", query8);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par9);
                if (SaveReference(EM_CODE.ToString(), dgEmpReference))
                {
                    result = true;
                }

                if (EM_IsUser)
                {
                    DataTable dtGetUser = CommonClasses.Execute("Select UM_CODE from CM_USER_MASTER where UM_BM_CODE=" + EM_BM_CODE + " and UM_CM_ID=" + EM_CM_COMP_ID + " and UM_EM_CODE=" + EM_CODE + " and UM_DELETE_FLAG=0");
                    if (dtGetUser.Rows.Count != 0)
                    {
                        UM_CODE = int.Parse(dtGetUser.Rows[0]["UM_CODE"].ToString());
                        CommonClasses.Execute("Update CM_USER_MASTER Set UM_ACTIVE_IND=" + UM_ACTIVE_IND + " where UM_CODE=" + UM_CODE + " ");

                    }
                    //else if (SaveUserMaster(chkLst, CheckBoxListNoti, EM_CODE.ToString()))
                    //{
                    //    result = true;
                    //}
                }


                //string query9 = "Delete from HR_EMPLOYEE_CONFIRMATION where EC_EM_CODE=" + EM_CODE + "";
                //SqlParameter[] par10 = new SqlParameter[1];
                //par10[0] = new SqlParameter("@Query", query9);
                //result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par10);
                //if (SaveConfirmationDates(EM_CODE.ToString(), dgConfirmDate))
                //{
                //    result = true;
                //}





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
            DL_DBAccess = new DatabaseAccessLayer();

            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@EM_CODE", EM_CODE);
            par[1] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_DeleteEmp", par);
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
    public DataTable FillGrid(bool chkleft)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            par[1] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
            par[2] = new SqlParameter("@chkleft", chkleft);
            dt = DL_DBAccess.SelectData("SP_HR_FILLEMP", par);
        }
        catch (Exception ex)
        {
        }
        return dt;
    }
    #endregion

    //#region FillGridAllBranch
    //public DataTable FillGridAllBranch(bool chkleft)
    //{
    //    DL_DBAccess = new DatabaseAccessLayer();
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        SqlParameter[] par = new SqlParameter[3];
    //        par[0] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
    //        par[1] = new SqlParameter("@chkleft", chkleft);
    //        par[2] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
    //        dt = DL_DBAccess.SelectData("SP_HR_FILLEMPALLBRANCH", par);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return dt;
    //}
    //#endregion
    #region FillGridAllBranch
    public DataTable FillGridAllBranch()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@EM_CM_COMP_ID", EM_CM_COMP_ID);
            par[1] = new SqlParameter("@chkleft", false);
            par[2] = new SqlParameter("@EM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_FILLEMPALLBRANCH", par);
        }
        catch (Exception ex)
        {
        }
        return dt;
    }
    #endregion

    #region FillCombo
    public DataTable FillCombo(string Query)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", Query);

            dt = DL_DBAccess.SelectData("SP_CM_FillCombo", par);

        }
        catch (Exception ex)
        {
        }
        return dt;
    }
    #endregion

    #region LoadEarnings
    public DataTable LoadEarnings()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@EDM_CM_CODE", EM_CM_COMP_ID);
            par[1] = new SqlParameter("@EDM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_GetEarnings", par);
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

    #region LoadDeductions
    public DataTable LoadDeductions()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@EDM_CM_CODE", EM_CM_COMP_ID);
            par[1] = new SqlParameter("@EDM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_GetDeductions", par);
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

    #region LoadEarnings_Modify
    public DataTable LoadEarnings_Modify()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("EMD_EM_CODE", EM_CODE);
            //par[1] = new SqlParameter("EDM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_GetEarnings_Modify", par);
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

    #region LoadDeductions_Modify
    public DataTable LoadDeductions_Modify()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("EMD_EM_CODE", EM_CODE);
            //par[1] = new SqlParameter("EDM_BM_CODE", EM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_GetDeductions_Modify", par);
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

    #endregion

    public void sendmail()
    {
        DataTable Setting = CommonClasses.Execute("Select PORTNO, HOST, FROMUSERNAME, PASSWORD, CMP_CM_ID from dbo.EmailSetting where CMP_CM_ID=" + EM_CM_COMP_ID + "");

        string smsg = "Dear " + EM_NAME + " " + EM_M_NAME + " " + EM_L_NAME;
        smsg += "<br><b>User Name: </b>" + EM_EMAIL_ID;
        smsg += "<br><b> Password: </b>" + CommonClasses.DecriptText(UM_PASSWORD);
        smsg += "<br><br><br><br>";
        smsg += "<b>Administrator";

        MailMessage message = new MailMessage();
        try
        {
            message.To.Add(new MailAddress("dheerendra.joshi@simyainfo.com"));
            message.From = new MailAddress(Setting.Rows[0]["FROMUSERNAME"].ToString());

            message.Subject = "User created successfully ";
            message.Body = smsg;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Port = int.Parse(Setting.Rows[0]["PORTNO"].ToString()); // Gmail works on this port 587
            client.Host = Setting.Rows[0]["HOST"].ToString();
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(Setting.Rows[0]["FROMUSERNAME"].ToString(), Setting.Rows[0]["PASSWORD"].ToString());
            //client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = nc;
            client.Send(message);
        }
        catch
        {
            //catch block goes here
        }
    }


}
