using System;
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
/// Summary description for MaterialInspection_BL
/// </summary>
public class MaterialInspection_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables

    private int _INSM_CODE;
    private int _INSM_CM_CODE;
    private DateTime _INSM_DATE;
    private int _INSM_NO;
    private int _INSM_IWM_CODE;
    private int _INSM_SPOM_CODE;
    private int _INSM_I_CODE;
    private int _INSM_UOM_CODE;
    private double _INSM_RECEIVED_QTY;
    private double _INSM_OK_QTY;
    private double _INSM_REJ_QTY;
    private double _INSM_SCRAP_QTY;
    private string _INSM_REMARK;
    private string _INSM_PTYPE;

    private Boolean _INSM_PDR_CHECK;
    private string _INSM_PDR_NO;
    private string _INSM_TYPE;
    private double _INSM_RATE;
    private int _INSM_STORE_TYPE;

    private int _INSD_CODE;
    private int _INSD_INSM_CODE;
    private int _INSD_I_CODE;
    private string _INSD_RM_CODE;
    private double _INSD_REJ_QTY;
    private string _INSM_FILE;



    string message;
    int PK_CODE;
    #endregion

    #region "Constructor"
    public MaterialInspection_BL()
    {

    }
    #endregion

    #region Parameterise Constructor
    public MaterialInspection_BL(int Id)
    {
        _INSM_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int INSM_CODE
    {
        get { return _INSM_CODE; }
        set { _INSM_CODE = value; }
    }
    public int INSM_CM_CODE
    {
        get { return _INSM_CM_CODE; }
        set { _INSM_CM_CODE = value; }
    }
    public DateTime INSM_DATE
    {
        get { return _INSM_DATE; }
        set { _INSM_DATE = value; }
    }

    public string INSM_PTYPE
    {
        get { return _INSM_PTYPE; }
        set { _INSM_PTYPE = value; }
    }
    public int INSM_NO
    {
        get { return _INSM_NO; }
        set { _INSM_NO = value; }
    }

    public int INSM_IWM_CODE
    {
        get { return _INSM_IWM_CODE; }
        set { _INSM_IWM_CODE = value; }
    }
    public int INSM_SPOM_CODE
    {
        get { return _INSM_SPOM_CODE; }
        set { _INSM_SPOM_CODE = value; }
    }
    public int INSM_I_CODE
    {
        get { return _INSM_I_CODE; }
        set { _INSM_I_CODE = value; }
    }
    public int INSM_UOM_CODE
    {
        get { return _INSM_UOM_CODE; }
        set { _INSM_UOM_CODE = value; }
    }
    public double INSM_RECEIVED_QTY
    {
        get { return _INSM_RECEIVED_QTY; }
        set { _INSM_RECEIVED_QTY = value; }
    }
    public double INSM_OK_QTY
    {
        get { return _INSM_OK_QTY; }
        set { _INSM_OK_QTY = value; }
    }
    public double INSM_REJ_QTY
    {
        get { return _INSM_REJ_QTY; }
        set { _INSM_REJ_QTY = value; }
    }
    public double INSM_SCRAP_QTY
    {
        get { return _INSM_SCRAP_QTY; }
        set { _INSM_SCRAP_QTY = value; }
    }
    public string INSM_REMARK
    {
        get { return _INSM_REMARK; }
        set { _INSM_REMARK = value; }
    }
    public Boolean INSM_PDR_CHECK
    {
        get { return _INSM_PDR_CHECK; }
        set { _INSM_PDR_CHECK = value; }
    }

    public string INSM_PDR_NO
    {
        get { return _INSM_PDR_NO; }
        set { _INSM_PDR_NO = value; }
    }
    public string INSM_TYPE
    {
        get { return _INSM_TYPE; }
        set { _INSM_TYPE = value; }
    }
    public double INSM_RATE
    {
        get { return _INSM_RATE; }
        set { _INSM_RATE = value; }
    }


    public int INSD_CODE
    {
        get { return _INSD_CODE; }
        set { _INSD_CODE = value; }
    }

    public int INSD_INSM_CODE
    {
        get { return _INSD_INSM_CODE; }
        set { _INSD_INSM_CODE = value; }
    }
    public int INSD_I_CODE
    {
        get { return _INSD_I_CODE; }
        set { _INSD_I_CODE = value; }
    }

    public string INSD_RM_CODE
    {
        get { return _INSD_RM_CODE; }
        set { _INSD_RM_CODE = value; }
    }
    public double INSD_REJ_QTY
    {
        get { return _INSD_REJ_QTY; }
        set { _INSD_REJ_QTY = value; }
    }

    public int INSM_STORE_TYPE
    {
        get { return _INSM_STORE_TYPE; }
        set { _INSM_STORE_TYPE = value; }
    }


    public string INSM_FILE
    {
        get { return _INSM_FILE; }
        set { _INSM_FILE = value; }
    }
    #endregion

    #region Public Methods

    #region Save
    public bool Save(out int MAX_CODE)
    {
        bool result = false;
        MAX_CODE = 0;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[17];

            par[0] = new SqlParameter("@INSM_CM_CODE", INSM_CM_CODE);
            par[1] = new SqlParameter("@INSM_DATE", INSM_DATE);
            par[2] = new SqlParameter("@INSM_NO", INSM_NO);
            par[3] = new SqlParameter("@INSM_IWM_CODE", INSM_IWM_CODE);
            par[4] = new SqlParameter("@INSM_SPOM_CODE", INSM_SPOM_CODE);
            par[5] = new SqlParameter("@INSM_I_CODE", INSM_I_CODE);
            par[6] = new SqlParameter("@INSM_UOM_CODE", INSM_UOM_CODE);
            par[7] = new SqlParameter("@INSM_RECEIVED_QTY", INSM_RECEIVED_QTY);
            par[8] = new SqlParameter("@INSM_OK_QTY", INSM_OK_QTY);
            par[9] = new SqlParameter("@INSM_REJ_QTY", INSM_REJ_QTY);
            par[10] = new SqlParameter("@INSM_SCRAP_QTY", INSM_SCRAP_QTY);
            par[11] = new SqlParameter("@INSM_REMARK", INSM_REMARK);
            par[12] = new SqlParameter("@INSM_PDR_CHECK", INSM_PDR_CHECK);
            par[13] = new SqlParameter("@INSM_PDR_NO", INSM_PDR_NO);
            par[14] = new SqlParameter("@INSM_STORE_TYPE", INSM_STORE_TYPE);
            par[15] = new SqlParameter("@INSM_TYPE", INSM_PTYPE);
            par[16] = new SqlParameter("@INSM_FILE", INSM_FILE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_MaterialInspection", par);
            INSD_INSM_CODE = Convert.ToInt32(CommonClasses.GetMaxId("Select MAX(INSM_CODE) from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + INSM_CM_CODE + "'"));
            MAX_CODE = INSD_INSM_CODE;
            if (INSM_REJ_QTY > 0)
            {
                if (result == true)
                {
                    INSD_I_CODE = INSM_I_CODE;
                    INSD_REJ_QTY = INSM_REJ_QTY;
                    SqlParameter[] par1 = new SqlParameter[4];
                    par1[0] = new SqlParameter("@INSD_INSM_CODE", INSD_INSM_CODE);
                    par1[1] = new SqlParameter("@INSD_I_CODE", INSD_I_CODE);
                    par1[2] = new SqlParameter("@INSD_RM_CODE", INSD_RM_CODE);
                    par1[3] = new SqlParameter("@INSD_REJ_QTY", INSD_REJ_QTY);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionDetail", par1);
                }
            }
            if (result)
            {
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + INSM_RECEIVED_QTY + "   where I_CODE='" + INSM_I_CODE + "'");

                DataTable dtInward = new DataTable();
                if (INSM_PTYPE == "0")
                {
                    dtInward = CommonClasses.Execute("SELECT CR_CODE AS IWM_CODE, CR_GIN_NO AS IWM_NO, CR_GIN_DATE AS IWM_DATE  FROM CUSTREJECTION_MASTER WHERE CR_CODE='" + INSM_IWM_CODE + "' ");
                     CommonClasses.Execute("update CUSTREJECTION_DETAIL set  CD_INSP_NO='" + INSM_NO + "',CD_INSP_FLG=1 where CD_CR_CODE='" + INSM_IWM_CODE + "' and CD_I_CODE='" + INSM_I_CODE + "'");
                }
                else
                {
                    dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");
                    CommonClasses.Execute("update INWARD_MASTER set IWM_INSP_NO=ISNULL(IWM_INSP_NO,'')+'," + INSM_NO + "' where IWM_CODE='" + INSM_IWM_CODE + "' ");
                    CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY='" + INSM_OK_QTY + "',IWD_CON_REJ_QTY='" + INSM_REJ_QTY + "',IWD_CON_SCRAP_QTY='" + INSM_SCRAP_QTY + "',IWD_INSP_NO='" + INSM_NO + "',IWD_INSP_FLG=1 where IWD_IWM_CODE='" + INSM_IWM_CODE + "' and IWD_I_CODE='" + INSM_I_CODE + "'");
                }
                //for Stock add
                if (INSM_TYPE == "IWIAP")
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER where STL_I_CODE='" + INSM_I_CODE + "' AND STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' AND STL_DOC_NUMBER='" + dtInward.Rows[0]["IWM_NO"].ToString() + "'   AND STL_STORE_TYPE IN (-2147483648,-2147483635)");

                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY + INSM_REJ_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'   where I_CODE='" + INSM_I_CODE + "'");
                    //for StockLedger Effiect
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "','" + INSM_STORE_TYPE + "')");
                    if (INSM_REJ_QTY > 0)
                    {
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'SubContractorRejection' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_REJ_QTY) + "','" + INSM_STORE_TYPE + "')");
                    }

                 }
                else if (INSM_TYPE == "CustRej")
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER where STL_I_CODE='" + INSM_I_CODE + "' AND STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' AND STL_DOC_NUMBER='" + dtInward.Rows[0]["IWM_NO"].ToString() + "'   AND STL_STORE_TYPE=-2147483648");

                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'  where I_CODE='" + INSM_I_CODE + "'");
                    //for StockLedger Effiect
                    if (INSM_OK_QTY > 0)
                    {
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY + INSM_REJ_QTY) + "','" + INSM_STORE_TYPE + "')");
                    }

                   
                }
                else
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER where STL_I_CODE='" + INSM_I_CODE + "' AND STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' AND STL_DOC_NUMBER='" + dtInward.Rows[0]["IWM_NO"].ToString() + "'   AND STL_STORE_TYPE IN (-2147483648,-2147483635)");
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "',I_INV_RATE='" + INSM_RATE + "'   where I_CODE='" + INSM_I_CODE + "'");
                    //for StockLedger Effiect
                    if (INSM_OK_QTY > 0)
                    {
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY ) + "','" + INSM_STORE_TYPE + "')");
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

    #region Update
    public bool Update(out int MAX_CODE)
    {
        bool result = false;
        MAX_CODE = 0;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            //DataTable dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY FROM INSPECTION_S_MASTER where  INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            DataTable dtinsecption = new DataTable();
            if (INSM_TYPE == "IWIAP")
            {
                dtinsecption = CommonClasses.Execute("SELECT (INSM_OK_QTY+INSM_REJ_QTY) AS INSM_OK_QTY FROM INSPECTION_S_MASTER where  INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            }
            else
            {
                dtinsecption = CommonClasses.Execute("SELECT (INSM_OK_QTY+INSM_REJ_QTY) AS AS INSM_OK_QTY FROM INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            }
            for (int k = 0; k < dtinsecption.Rows.Count; k++)
            {
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtinsecption.Rows[k]["INSM_OK_QTY"] + " where  I_CODE='" + INSM_I_CODE + "'");
            }
            /*10082018 :- Change Parameter Count*/
            SqlParameter[] par = new SqlParameter[17];

            par[0] = new SqlParameter("@INSM_CODE", INSM_CODE);
            par[1] = new SqlParameter("@INSM_CM_CODE", INSM_CM_CODE);
            par[2] = new SqlParameter("@INSM_DATE", INSM_DATE);
            par[3] = new SqlParameter("@INSM_NO", INSM_NO);
            par[4] = new SqlParameter("@INSM_IWM_CODE", INSM_IWM_CODE);
            par[5] = new SqlParameter("@INSM_SPOM_CODE", INSM_SPOM_CODE);
            par[6] = new SqlParameter("@INSM_I_CODE", INSM_I_CODE);
            par[7] = new SqlParameter("@INSM_UOM_CODE", INSM_UOM_CODE);
            par[8] = new SqlParameter("@INSM_RECEIVED_QTY", INSM_RECEIVED_QTY);
            par[9] = new SqlParameter("@INSM_OK_QTY", INSM_OK_QTY);
            par[10] = new SqlParameter("@INSM_REJ_QTY", INSM_REJ_QTY);
            par[11] = new SqlParameter("@INSM_SCRAP_QTY", INSM_SCRAP_QTY);
            par[12] = new SqlParameter("@INSM_REMARK", INSM_REMARK);
            par[13] = new SqlParameter("@INSM_PDR_CHECK", INSM_PDR_CHECK);
            par[14] = new SqlParameter("@INSM_PDR_NO", INSM_PDR_NO);
            par[15] = new SqlParameter("@INSM_STORE_TYPE", INSM_STORE_TYPE);
            par[16] = new SqlParameter("@INSM_FILE", INSM_FILE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_Update_MaterialInspection", par);
            MAX_CODE = INSM_CODE;
            //for Stock less
            //  result = false;
            DataTable dtInward = new DataTable();
            if (INSM_PTYPE == "0")
            {
                dtInward = CommonClasses.Execute("SELECT CR_CODE AS IWM_CODE, CR_GIN_NO AS IWM_NO, CR_GIN_DATE AS IWM_DATE  FROM CUSTREJECTION_MASTER WHERE CR_CODE='" + INSM_IWM_CODE + "' ");
            }
            else
            {
                dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");
            }
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM INSPECTION_S_DETAIL WHERE INSD_INSM_CODE='" + INSM_CODE + "'");
                CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' AND STL_I_CODE='" + INSM_I_CODE + "'   ");
                if (INSM_TYPE == "IWIAP")
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "'  AND STL_I_CODE='" + INSM_I_CODE + "'  AND STL_DOC_TYPE='SubContractorRejection'");
                }
            }
            if (INSM_REJ_QTY > 0)
            {
                if (result == true)
                {
                    INSD_I_CODE = INSM_I_CODE;
                    INSD_REJ_QTY = INSM_REJ_QTY;
                    INSD_INSM_CODE = INSM_CODE;
                    SqlParameter[] par1 = new SqlParameter[4];
                    par1[0] = new SqlParameter("@INSD_INSM_CODE", INSD_INSM_CODE);
                    par1[1] = new SqlParameter("@INSD_I_CODE", INSD_I_CODE);
                    par1[2] = new SqlParameter("@INSD_RM_CODE", INSD_RM_CODE);
                    par1[3] = new SqlParameter("@INSD_REJ_QTY", INSD_REJ_QTY);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionDetail", par1);
                }
            }
            if (result)
            {
                CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY='" + INSM_OK_QTY + "',IWD_CON_REJ_QTY='" + INSM_REJ_QTY + "',IWD_CON_SCRAP_QTY='" + INSM_SCRAP_QTY + "',IWD_INSP_NO='" + INSM_NO + "',IWD_INSP_FLG=1 where IWD_IWM_CODE='" + INSM_IWM_CODE + "' and IWD_I_CODE='" + INSM_I_CODE + "'");
                //for Stock add
                if (INSM_TYPE == "IWIAP")
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY + INSM_REJ_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'     where I_CODE='" + INSM_I_CODE + "'");
                    //for StockLedger Effiect
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "','" + INSM_STORE_TYPE + "')");

                    if (INSM_REJ_QTY > 0)
                    {
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'SubContractorRejection' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_REJ_QTY) + "','" + INSM_STORE_TYPE + "')");
                    }
                }
                else if (INSM_TYPE == "CustRej")
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'   where I_CODE='" + INSM_I_CODE + "'");
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY + INSM_REJ_QTY) + "','" + INSM_STORE_TYPE + "')");
                }
                else
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "',I_INV_RATE='" + INSM_RATE + "'     where I_CODE='" + INSM_I_CODE + "'");
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "','" + INSM_STORE_TYPE + "')");
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
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", INSM_CODE);
            par[1] = new SqlParameter("@PK_Field", "INSM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "INSPECTION_S_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result)
            {

                DataTable dtInward = new DataTable();
                dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("select IWD_Code,IWD_CON_OK_QTY+IWD_CON_REJ_QTY AS QTY from INWARD_DETAIL,INSPECTION_S_MASTER where INSM_I_CODE=IWD_I_CODE and INSM_NO=IWD_INSP_NO and INWARD_DETAIL.ES_DELETE=0 and INSM_IWM_CODE='" + INSM_CODE + "' ");
                if (dt.Rows.Count > 0)
                {
                    CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY=0,IWD_CON_REJ_QTY='0',IWD_CON_SCRAP_QTY='0',IWD_INSP_NO='',IWD_INSP_FLG=0 where IWD_Code='" + Convert.ToInt32(dt.Rows[0]["IWD_Code"].ToString()) + "' ");
                    result = true;
                }
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + Convert.ToDouble(dt.Rows[0]["QTY"].ToString()) + "  where I_CODE='" + INSM_I_CODE + "'");
                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM INSPECTION_S_DETAIL WHERE INSD_INSM_CODE='" + INSM_CODE + "'");
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + INSM_CODE + "'   AND STL_I_CODE='" + INSM_I_CODE + "' AND STL_DOC_TYPE='" + INSM_TYPE + "'");
                }
            }
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

    #endregion
}
