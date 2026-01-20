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

/// <summary>
/// Summary description for RawMaterial_BL
/// </summary>
public class RawMaterial_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _I_CODE;
    private int _I_CM_COMP_ID;
    private int _I_CAT_CODE;
    private int _I_SCAT_CODE;
    private string _I_CODENO;
    private string _I_DRAW_NO;
    private string _I_NAME;
    private string _I_MATERIAL;
    private string _I_SPECIFICATION;
    private int _I_E_CODE;
    private int _I_ACCOUNT_SALES;
    private int _I_ACCOUNT_PURCHASE;
    private int _I_UOM_CODE;
    private string _I_INV_CAT;
    private double _I_MAX_LEVEL;
    private double _I_MIN_LEVEL;
    private double _I_REORDER_LEVEL;
    private double _I_OP_BAL;
    private double _I_OP_BAL_RATE;
    private string _I_STORE_LOC;
    private double _I_INV_RATE;
    private System.DateTime _I_RECEIPT_DATE;
    private System.DateTime _I_ISSUE_DATE;
    private double _I_CURRENT_BAL;
    private bool _I_ACTIVE_IND;
    private bool _ES_DELETE;
    private bool _MODIFY;
    private double _I_UWEIGHT;
    private int _I_UW_UOM_CODE;
    private string _I_COSTING_HEAD;
    private string _I_SIZE;
    private double _I_DENSITY;
    private double _I_PIGMENT;
    private double _I_SOLIDS;
    private double _I_VOLATILE;
    private int _I_WEIGHT_UOM;
    private bool _I_DEVELOMENT;
    private double _I_TARGET_WEIGHT;
    private double _I_STD_WEIGHT;
    private double _I_DISPATCH_BAL;
    private double _I_STANDARD_PRODUCTION;
    private double _I_AS_CAST_WEIGHT;
    private string _I_DRAWING_PATH;
    private string _I_PHOTO_PATH;
    #endregion

    #region Public Properties
    public double I_VOLATILE
    {
        get { return _I_VOLATILE; }
        set { _I_VOLATILE = value; }
    }
    public double I_SOLIDS
    {
        get { return _I_SOLIDS; }
        set { _I_SOLIDS = value; }
    }

    public double I_PIGMENT
    {
        get { return _I_PIGMENT; }
        set { _I_PIGMENT = value; }
    }

    public double I_DENSITY
    {
        get { return _I_DENSITY; }
        set { _I_DENSITY = value; }
    }

    public int I_CODE
    {
        get { return _I_CODE; }
        set { _I_CODE = value; }
    }
    public int I_CM_COMP_ID
    {
        get { return _I_CM_COMP_ID; }
        set { _I_CM_COMP_ID = value; }
    }
    public int I_CAT_CODE
    {
        get { return _I_CAT_CODE; }
        set { _I_CAT_CODE = value; }
    }
    public int I_SCAT_CODE
    {
        get { return _I_SCAT_CODE; }
        set { _I_SCAT_CODE = value; }
    }
    public string I_CODENO
    {
        get { return _I_CODENO; }
        set { _I_CODENO = value; }
    }
    public string I_DRAW_NO
    {
        get { return _I_DRAW_NO; }
        set { _I_DRAW_NO = value; }
    }
    public string I_NAME
    {
        get { return _I_NAME; }
        set { _I_NAME = value; }
    }
    public string I_MATERIAL
    {
        get { return _I_MATERIAL; }
        set { _I_MATERIAL = value; }
    }
    public string I_SPECIFICATION
    {
        get { return _I_SPECIFICATION; }
        set { _I_SPECIFICATION = value; }
    }
    public int I_E_CODE
    {
        get { return _I_E_CODE; }
        set { _I_E_CODE = value; }
    }
    public int I_ACCOUNT_SALES
    {
        get { return _I_ACCOUNT_SALES; }
        set { _I_ACCOUNT_SALES = value; }
    }
    public int I_ACCOUNT_PURCHASE
    {
        get { return _I_ACCOUNT_PURCHASE; }
        set { _I_ACCOUNT_PURCHASE = value; }
    }
    public int I_UOM_CODE
    {
        get { return _I_UOM_CODE; }
        set { _I_UOM_CODE = value; }
    }
    public string I_INV_CAT
    {
        get { return _I_INV_CAT; }
        set { _I_INV_CAT = value; }
    }
    public double I_MAX_LEVEL
    {
        get { return _I_MAX_LEVEL; }
        set { _I_MAX_LEVEL = value; }
    }
    public double I_MIN_LEVEL
    {
        get { return _I_MIN_LEVEL; }
        set { _I_MIN_LEVEL = value; }
    }
    public double I_REORDER_LEVEL
    {
        get { return _I_REORDER_LEVEL; }
        set { _I_REORDER_LEVEL = value; }
    }
    public double I_OP_BAL
    {
        get { return _I_OP_BAL; }
        set { _I_OP_BAL = value; }
    }
    public double I_OP_BAL_RATE
    {
        get { return _I_OP_BAL_RATE; }
        set { _I_OP_BAL_RATE = value; }
    }
    public string I_STORE_LOC
    {
        get { return _I_STORE_LOC; }
        set { _I_STORE_LOC = value; }
    }
    public double I_INV_RATE
    {
        get { return _I_INV_RATE; }
        set { _I_INV_RATE = value; }
    }
    public System.DateTime I_RECEIPT_DATE
    {
        get { return _I_RECEIPT_DATE; }
        set { _I_RECEIPT_DATE = value; }
    }
    public System.DateTime I_ISSUE_DATE
    {
        get { return _I_ISSUE_DATE; }
        set { _I_ISSUE_DATE = value; }
    }
    public double I_CURRENT_BAL
    {
        get { return _I_CURRENT_BAL; }
        set { _I_CURRENT_BAL = value; }
    }
    public bool I_ACTIVE_IND
    {
        get { return _I_ACTIVE_IND; }
        set { _I_ACTIVE_IND = value; }
    }
    public bool ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }
    public bool MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }
    public double I_UWEIGHT
    {
        get { return _I_UWEIGHT; }
        set { _I_UWEIGHT = value; }
    }
    public int I_UW_UOM_CODE
    {
        get { return _I_UW_UOM_CODE; }
        set { _I_UW_UOM_CODE = value; }
    }
    public string I_COSTING_HEAD
    {
        get { return _I_COSTING_HEAD; }
        set { _I_COSTING_HEAD = value; }
    }
    public string I_SIZE
    {
        get { return _I_SIZE; }
        set { _I_SIZE = value; }
    }
    public int I_WEIGHT_UOM
    {
        get { return _I_WEIGHT_UOM; }
        set { _I_WEIGHT_UOM = value; }
    }

    public bool I_DEVELOMENT
    {
        get { return _I_DEVELOMENT; }
        set { _I_DEVELOMENT = value; }
    }
    public double I_TARGET_WEIGHT
    {
        get { return _I_TARGET_WEIGHT; }
        set { _I_TARGET_WEIGHT = value; }
    }
    public double I_TARGET_WEIGHT1
    {
        get { return _I_TARGET_WEIGHT; }
        set { _I_TARGET_WEIGHT = value; }
    }
    public double I_STD_WEIGHT
    {
        get { return _I_STD_WEIGHT; }
        set { _I_STD_WEIGHT = value; }
    }
    public double I_DISPATCH_BAL
    {
        get { return _I_DISPATCH_BAL; }
        set { _I_DISPATCH_BAL = value; }
    }

    public string I_DRAWING_PATH
    {
        get { return _I_DRAWING_PATH; }
        set { _I_DRAWING_PATH = value; }
    }
    public string I_PHOTO_PATH
    {
        get { return _I_PHOTO_PATH; }
        set { _I_PHOTO_PATH = value; }
    }
    #region I_STANDARD_PRODUCTION
    public double I_STANDARD_PRODUCTION
    {
        get { return _I_STANDARD_PRODUCTION; }
        set { _I_STANDARD_PRODUCTION = value; }
    }
    #endregion I_STANDARD_PRODUCTION

    #region I_AS_CAST_WEIGHT
    public double I_AS_CAST_WEIGHT
    {
        get { return _I_AS_CAST_WEIGHT; }
        set { _I_AS_CAST_WEIGHT = value; }
    }
    #endregion I_AS_CAST_WEIGHT

    #endregion

    #region "Constructor"
    public RawMaterial_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public RawMaterial_BL(int Id)
    {
        _I_CODE = Id;
    }
    #endregion

    #region Public Methods

    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT I_CODE,I_CM_COMP_ID,I_CAT_CODE,ISNULL(I_STD_WEIGHT,0) as I_STD_WEIGHT,I_PHOTO_PATH,I_DRAWING_PATH,isnull(I_DISPATCH_BAL,0) as I_DISPATCH_BAL,ISNULL(I_SCAT_CODE,0) AS I_SCAT_CODE,I_CODENO,I_DRAW_NO,I_NAME,I_MATERIAL,I_SPECIFICATION,I_E_CODE,I_ACCOUNT_SALES,I_ACCOUNT_PURCHASE,I_UOM_CODE,I_INV_CAT,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL,I_OP_BAL,I_OP_BAL_RATE,I_STORE_LOC,I_INV_RATE,I_RECEIPT_DATE,I_ISSUE_DATE,I_CURRENT_BAL,I_ACTIVE_IND,ES_DELETE,MODIFY,I_UWEIGHT,I_UW_UOM_CODE,I_COSTING_HEAD,I_SIZE,I_DOC_NAME,I_DOC_PATH,I_WC_CODE,I_INV_CAT,I_WEIGHT,cast(isnull(I_OPEN_RATE,0) as numeric(20,2)) as I_OPEN_RATE,cast(isnull(I_DENSITY,0)  as numeric(20,2)) as I_DENSITY,cast(isnull(I_PIGMENT,0)  as numeric(20,2)) as I_PIGMENT,cast(isnull(I_SOLIDS,0)  as numeric(20,2)) as I_SOLIDS,cast(isnull(I_VOLATILE,0)  as numeric(20,2)) as I_VOLATILE  ,ISNULL(I_WEIGHT_UOM,0) AS I_WEIGHT_UOM,ISNULL(I_DEVELOMENT,0) AS I_DEVELOMENT,ISNULL(I_TARGET_WEIGHT,0) AS I_TARGET_WEIGHT ,ISNULL(I_STANDARD_PRODUCTION , 0) AS I_STANDARD_PRODUCTION ,ISNULL(I_AS_CAST_WEIGHT,0) AS I_AS_CAST_WEIGHT  FROM ITEM_MASTER WHERE I_CODE = '" + I_CODE + "' AND I_CM_COMP_ID = '" + I_CM_COMP_ID + "' AND ES_DELETE=0");

        if (dt.Rows.Count > 0)
        {
            I_CODE = Convert.ToInt32(dt.Rows[0]["I_CODE"]);
            I_CM_COMP_ID = Convert.ToInt32(dt.Rows[0]["I_CM_COMP_ID"]);
            I_CAT_CODE = Convert.ToInt32(dt.Rows[0]["I_CAT_CODE"]);
            I_SCAT_CODE = Convert.ToInt32(dt.Rows[0]["I_SCAT_CODE"]);
            I_CODENO = dt.Rows[0]["I_CODENO"].ToString();
            I_DRAW_NO = dt.Rows[0]["I_DRAW_NO"].ToString();
            I_NAME = dt.Rows[0]["I_NAME"].ToString();
            I_MATERIAL = dt.Rows[0]["I_MATERIAL"].ToString();
            I_SPECIFICATION = dt.Rows[0]["I_SPECIFICATION"].ToString();
            I_E_CODE = Convert.ToInt32(dt.Rows[0]["I_E_CODE"]);
            I_ACCOUNT_SALES = Convert.ToInt32(dt.Rows[0]["I_ACCOUNT_SALES"].ToString());
            I_ACCOUNT_PURCHASE = Convert.ToInt32(dt.Rows[0]["I_ACCOUNT_PURCHASE"].ToString());
            I_UOM_CODE = Convert.ToInt32(dt.Rows[0]["I_UOM_CODE"]);
            I_INV_CAT = dt.Rows[0]["I_INV_CAT"].ToString();
            I_MAX_LEVEL = Convert.ToDouble(dt.Rows[0]["I_MAX_LEVEL"].ToString());
            I_MIN_LEVEL = Convert.ToDouble(dt.Rows[0]["I_MIN_LEVEL"].ToString());
            I_REORDER_LEVEL = Convert.ToDouble(dt.Rows[0]["I_REORDER_LEVEL"].ToString());
            I_OP_BAL = Convert.ToDouble(dt.Rows[0]["I_OP_BAL"].ToString());
            I_OP_BAL_RATE = Convert.ToDouble(dt.Rows[0]["I_OP_BAL_RATE"].ToString());
            I_STORE_LOC = dt.Rows[0]["I_STORE_LOC"].ToString();
            I_INV_RATE = Convert.ToDouble(dt.Rows[0]["I_INV_RATE"].ToString());
            I_DRAWING_PATH = dt.Rows[0]["I_DRAWING_PATH"].ToString();
            I_PHOTO_PATH = dt.Rows[0]["I_PHOTO_PATH"].ToString();
            if (dt.Rows[0]["I_RECEIPT_DATE"].ToString() != "")
            {
                I_RECEIPT_DATE = Convert.ToDateTime(dt.Rows[0]["I_RECEIPT_DATE"].ToString());
            }
            else
            {
                I_RECEIPT_DATE = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy"));
            }
            if (dt.Rows[0]["I_ISSUE_DATE"].ToString() != "")
            {
                I_ISSUE_DATE = Convert.ToDateTime(dt.Rows[0]["I_ISSUE_DATE"].ToString());
            }
            else
            {
                I_ISSUE_DATE = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy"));
            }

            I_CURRENT_BAL = Convert.ToDouble(dt.Rows[0]["I_CURRENT_BAL"].ToString());
            I_ACTIVE_IND = Convert.ToBoolean(dt.Rows[0]["I_ACTIVE_IND"].ToString());
            ES_DELETE = Convert.ToBoolean(dt.Rows[0]["ES_DELETE"].ToString());
            MODIFY = Convert.ToBoolean(dt.Rows[0]["MODIFY"].ToString());
            I_UWEIGHT = Convert.ToDouble(dt.Rows[0]["I_UWEIGHT"].ToString());
            I_COSTING_HEAD = dt.Rows[0]["I_COSTING_HEAD"].ToString();
            I_SIZE = dt.Rows[0]["I_SIZE"].ToString();
            I_OP_BAL_RATE = Convert.ToDouble(dt.Rows[0]["I_OP_BAL_RATE"].ToString());
            I_DENSITY = Convert.ToDouble(dt.Rows[0]["I_DENSITY"].ToString());
            I_PIGMENT = Convert.ToDouble(dt.Rows[0]["I_PIGMENT"].ToString());
            I_SOLIDS = Convert.ToDouble(dt.Rows[0]["I_SOLIDS"].ToString());
            I_VOLATILE = Convert.ToDouble(dt.Rows[0]["I_VOLATILE"].ToString());
            I_WEIGHT_UOM = Convert.ToInt32(dt.Rows[0]["I_WEIGHT_UOM"]);
            I_DEVELOMENT = Convert.ToBoolean(dt.Rows[0]["I_DEVELOMENT"].ToString());
            I_TARGET_WEIGHT = Convert.ToDouble(dt.Rows[0]["I_TARGET_WEIGHT"].ToString());
            I_STD_WEIGHT = Convert.ToDouble(dt.Rows[0]["I_STD_WEIGHT"].ToString());
            I_DISPATCH_BAL = Convert.ToDouble(dt.Rows[0]["I_DISPATCH_BAL"].ToString());
            I_STANDARD_PRODUCTION = Convert.ToDouble(dt.Rows[0]["I_STANDARD_PRODUCTION"].ToString());
            I_AS_CAST_WEIGHT = Convert.ToDouble(dt.Rows[0]["I_AS_CAST_WEIGHT"].ToString());
        }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" SELECT I_CODE,I_CODENO,I_NAME,I_SIZE,cast(I_CURRENT_BAL as numeric(20,2)) as I_CURRENT_BAL,I_PHOTO_PATH,I_DRAWING_PATH, Cast(I_DISPATCH_BAL as numeric(20,2)) as   I_DISPATCH_BAL ,I_CAT_NAME,I_UOM_NAME,  CASE WHEN I_INV_CAT=1 then'A' WHEN I_INV_CAT=2 then'B' WHEN I_INV_CAT=3 then'C' ELSE '' END AS I_INV_CAT   FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER  WHERE ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE and I_CM_COMP_ID = '1' and ITEM_MASTER.ES_DELETE='0'  AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND I_CM_COMP_ID = '" + I_CM_COMP_ID + "' and  ITEM_MASTER.ES_DELETE=0 ORDER BY I_NAME");
        XGrid.DataSource = dt;
        XGrid.DataBind();
    }
    #endregion

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {
        bool res = false;
        DataTable dt = new DataTable();
        //dt = GetRecords();
        dt = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE (I_CODENO=lower('" + I_CODENO + "')) and ES_DELETE=0");

        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DataTable dt = new DataTable();
        //dt = GetRecords();
        dt = CommonClasses.Execute("SELECT * FROM ITEM_MASTER WHERE ES_DELETE='0' AND I_CODE != '" + I_CODE + "' AND lower(I_CODENO) = lower('" + I_CODENO + "')");


        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion

    #region Save
    public bool Save()
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            #region old code
            //SqlParameter[] Params = 
            //{ 
            //new SqlParameter("@I_CODE",I_CODE),
            //new SqlParameter("@I_CM_COMP_ID",I_CM_COMP_ID),
            //new SqlParameter("@I_CAT_CODE",I_CAT_CODE),
            //new SqlParameter("@I_CODENO",I_CODENO),
            //new SqlParameter("@I_DRAW_NO",I_DRAW_NO),
            //new SqlParameter("@I_NAME",I_NAME),
            //new SqlParameter("@I_MATERIAL",I_MATERIAL),
            //new SqlParameter("@I_SPECIFICATION",I_SPECIFICATION),
            //new SqlParameter("@I_ET_CODE",I_ET_CODE),
            //new SqlParameter("@I_T_ACCT_S",I_T_ACCT_S),
            //new SqlParameter("@I_T_ACCT_P",I_T_ACCT_P),
            //new SqlParameter("@I_UOM_CODE",I_UOM_CODE),
            //new SqlParameter("@I_INV_CAT",I_INV_CAT),
            //new SqlParameter("@I_MAX_LEVEL",I_MAX_LEVEL),
            //new SqlParameter("@I_MIN_LEVEL",I_MIN_LEVEL),
            //new SqlParameter("@I_REORDER_LEVEL",I_REORDER_LEVEL),
            //new SqlParameter("@I_OP_BAL",I_OP_BAL),
            //new SqlParameter("@I_OP_BAL_RATE",I_OP_BAL_RATE),
            //new SqlParameter("@I_STORE_LOC",I_STORE_LOC),
            //new SqlParameter("@I_INV_RATE",I_INV_RATE),
            //new SqlParameter("@I_RECEIPT_DATE",I_RECEIPT_DATE),
            //new SqlParameter("@I_ISSUE_DATE",I_ISSUE_DATE),
            //new SqlParameter("@I_CURRENT_BAL",I_CURRENT_BAL),
            //new SqlParameter("@I_ACTIVE_IND",I_ACTIVE_IND),
            //new SqlParameter("@ES_DELETE",ES_DELETE),
            //new SqlParameter("@MODIFY",MODIFY),
            //new SqlParameter("@I_UWEIGHT",I_UWEIGHT),
            //new SqlParameter("@I_UW_UOM_CODE",I_UW_UOM_CODE),
            //new SqlParameter("@I_COST_HEAD",I_COST_HEAD) 
            //};
            //result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_MASTER_Insert", Params);

            #endregion    bool result = false;

            if (CheckExistSaveName())
            {
                string Code = CommonClasses.GetMaxId("Select Max(I_CODE) from ITEM_MASTER");
                result = CommonClasses.Execute1("INSERT INTO ITEM_MASTER (I_CM_COMP_ID,I_CAT_CODE,I_SCAT_CODE,I_CODENO,I_DRAW_NO,I_NAME,I_MATERIAL,I_SPECIFICATION,I_SIZE,I_E_CODE,I_ACCOUNT_SALES,I_ACCOUNT_PURCHASE,I_COSTING_HEAD,I_UOM_CODE,I_INV_CAT,I_ACTIVE_IND,I_UWEIGHT,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL,I_OP_BAL,I_OPEN_RATE,I_INV_RATE,I_STORE_LOC,I_RECEIPT_DATE,I_ISSUE_DATE,I_DENSITY,I_PIGMENT,I_SOLIDS,I_VOLATILE,I_WEIGHT_UOM,I_DEVELOMENT,I_TARGET_WEIGHT,I_STD_WEIGHT,I_DISPATCH_BAL ,I_STANDARD_PRODUCTION , I_AS_CAST_WEIGHT,I_DRAWING_PATH,I_PHOTO_PATH ) VALUES('" + I_CM_COMP_ID + "','" + I_CAT_CODE + "','" + I_SCAT_CODE + "','" + I_CODENO + "','" + I_DRAW_NO + "','" + I_NAME + "','" + I_MATERIAL + "','" + I_SPECIFICATION + "','" + I_SIZE + "','" + I_E_CODE + "','" + I_ACCOUNT_SALES + "','" + I_ACCOUNT_PURCHASE + "','" + I_COSTING_HEAD + "','" + I_UOM_CODE + "','" + I_INV_CAT + "','" + I_ACTIVE_IND + "','" + I_UWEIGHT + "','" + I_MAX_LEVEL + "','" + I_MIN_LEVEL + "','" + I_REORDER_LEVEL + "','" + I_OP_BAL + "','" + I_OP_BAL_RATE + "','" + I_INV_RATE + "','" + I_STORE_LOC + "','" + I_RECEIPT_DATE.ToString("dd/MMM/yyyy") + "','" + I_ISSUE_DATE.ToString("dd/MMM/yyyy") + "','" + I_DENSITY + "','" + I_PIGMENT + "','" + I_SOLIDS + "','" + I_VOLATILE + "','" + I_WEIGHT_UOM + "','" + I_DEVELOMENT + "','" + I_TARGET_WEIGHT + "','" + I_STD_WEIGHT + "','" + I_DISPATCH_BAL + "' ,'" + I_STANDARD_PRODUCTION + "' , '" + I_AS_CAST_WEIGHT + "','" + I_DRAWING_PATH + "','" + I_PHOTO_PATH + "')");
            }
            else
            {
                Msg = "Item Code Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Finished Product Class", "Save", Ex.Message);
        }
        return result;
    }
    #endregion

    #region Update
    public bool Update()
    {
        bool result = false;
        try
        {
            if (ChkExstUpdateName())
            {
                #region old code
                //SqlParameter[] Params = 
                //{ 
                //    new SqlParameter("@I_CODE",I_CODE),
                //    new SqlParameter("@I_CM_COMP_ID",I_CM_COMP_ID),
                //    new SqlParameter("@I_CAT_CODE",I_CAT_CODE),
                //    new SqlParameter("@I_CODENO",I_CODENO),
                //    new SqlParameter("@I_DRAW_NO",I_DRAW_NO),
                //    new SqlParameter("@I_NAME",I_NAME),
                //    new SqlParameter("@I_MATERIAL",I_MATERIAL),
                //    new SqlParameter("@I_SPECIFICATION",I_SPECIFICATION),
                //    new SqlParameter("@I_ET_CODE",I_ET_CODE),
                //    new SqlParameter("@I_T_ACCT_S",I_T_ACCT_S),
                //    new SqlParameter("@I_T_ACCT_P",I_T_ACCT_P),
                //    new SqlParameter("@I_UOM_CODE",I_UOM_CODE),
                //    new SqlParameter("@I_INV_CAT",I_INV_CAT),
                //    new SqlParameter("@I_MAX_LEVEL",I_MAX_LEVEL),
                //    new SqlParameter("@I_MIN_LEVEL",I_MIN_LEVEL),
                //    new SqlParameter("@I_REORDER_LEVEL",I_REORDER_LEVEL),
                //    new SqlParameter("@I_OP_BAL",I_OP_BAL),
                //    new SqlParameter("@I_OP_BAL_RATE",I_OP_BAL_RATE),
                //    new SqlParameter("@I_STORE_LOC",I_STORE_LOC),
                //    new SqlParameter("@I_INV_RATE",I_INV_RATE),
                //    new SqlParameter("@I_RECEIPT_DATE",I_RECEIPT_DATE),
                //    new SqlParameter("@I_ISSUE_DATE",I_ISSUE_DATE),
                //    new SqlParameter("@I_CURRENT_BAL",I_CURRENT_BAL),
                //    new SqlParameter("@I_ACTIVE_IND",I_ACTIVE_IND),
                //    new SqlParameter("@ES_DELETE",ES_DELETE),
                //    new SqlParameter("@MODIFY",MODIFY),
                //    new SqlParameter("@I_UWEIGHT",I_UWEIGHT),
                //    new SqlParameter("@I_UW_UOM_CODE",I_UW_UOM_CODE),
                //    new SqlParameter("@I_COST_HEAD",I_COST_HEAD) 
                //};
                //result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_MASTER_Update", Params); 
                #endregion
                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CAT_CODE='" + I_CAT_CODE + "',I_SCAT_CODE='" + I_SCAT_CODE + "',I_CODENO='" + I_CODENO + "',I_DRAW_NO='" + I_DRAW_NO + "',I_NAME='" + I_NAME + "',I_MATERIAL='" + I_MATERIAL + "',I_SPECIFICATION='" + I_SPECIFICATION + "',I_SIZE='" + I_SIZE + "',I_E_CODE='" + I_E_CODE + "',I_ACCOUNT_SALES='" + I_ACCOUNT_SALES + "',I_ACCOUNT_PURCHASE='" + I_ACCOUNT_PURCHASE + "',I_COSTING_HEAD='" + I_COSTING_HEAD + "',I_UOM_CODE='" + I_UOM_CODE + "',I_INV_CAT='" + I_INV_CAT + "',I_ACTIVE_IND='" + I_ACTIVE_IND + "',I_UWEIGHT='" + I_UWEIGHT + "',I_MAX_LEVEL='" + I_MAX_LEVEL + "',I_MIN_LEVEL='" + I_MIN_LEVEL + "', I_REORDER_LEVEL='" + I_REORDER_LEVEL + "',I_OP_BAL='" + I_OP_BAL + "',I_OPEN_RATE='" + I_OP_BAL_RATE + "',I_INV_RATE='" + I_INV_RATE + "',I_STORE_LOC='" + I_STORE_LOC + "',I_RECEIPT_DATE='" + I_RECEIPT_DATE.ToString("dd/MMM/yyyy") + "',I_ISSUE_DATE='" + I_ISSUE_DATE.ToString("dd/MMM/yyyy") + "',I_DENSITY='" + I_DENSITY + "',I_PIGMENT='" + I_PIGMENT + "',I_SOLIDS='" + I_SOLIDS + "',I_VOLATILE='" + I_VOLATILE + "',I_WEIGHT_UOM='" + I_WEIGHT_UOM + "' ,I_DEVELOMENT='" + I_DEVELOMENT + "' ,I_TARGET_WEIGHT='" + I_TARGET_WEIGHT + "',I_STD_WEIGHT='" + I_STD_WEIGHT + "',I_DISPATCH_BAL='" + I_DISPATCH_BAL + "' ,I_STANDARD_PRODUCTION = '" + I_STANDARD_PRODUCTION + "' , I_AS_CAST_WEIGHT = '" + I_AS_CAST_WEIGHT + "',I_DRAWING_PATH='" + I_DRAWING_PATH + "',I_PHOTO_PATH='" + I_PHOTO_PATH + "' WHERE I_CODE= '" + I_CODE + "'");

                if (result)
                {
                     result = CommonClasses.Execute1("UPDATE PRODUCTIVITY_MASTER SET  PROM_CASTING= '" + I_STANDARD_PRODUCTION*2 + "'   WHERE PROM_I_CODE= '" + I_CODE + "'");
                }
            }
            else
            {
                Msg = "Item Code Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("inished Product Class", "Update", Ex.Message);
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
            //SqlParameter[] Params = { new SqlParameter("@I_CODE", I_CODE) };
            result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET ES_DELETE = 1 WHERE I_CODE='" + I_CODE + "'");
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Finished Product Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion

    #endregion

}