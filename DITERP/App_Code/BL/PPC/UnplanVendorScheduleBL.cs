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
/// Summary description for UnplanVendorScheduleBL
/// </summary>
public class UnplanVendorScheduleBL
{

    #region Counstructor
    public UnplanVendorScheduleBL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public UnplanVendorScheduleBL(int Id)
    {
        UVS_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    private int _UVS_CODE;
    private int? _UVS_COMP_ID;
    private int? _UVS_P_CODE;
    private int? _UVS_I_CODE;
    private DateTime? _UVS_DATE;
    private float? _UVS_CASTING_OFFLOADED;
    private float? _UVS_WEEK1;
    private float? _UVS_WEEK2;
    private float? _UVS_WEEK3;
    private float? _UVS_WEEK4;
    private bool? _ES_DELETE;
    private bool? _MODIFY;
    private int? _UVS_CM_CODE;
    private bool? _UVS_STATUS;
    private bool? _UVS_TYPE;
    private string _UVS_DESC;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables


    public int UVS_CODE
    {
        get { return _UVS_CODE; }
        set { _UVS_CODE = value; }
    }

    public int? UVS_COMP_ID
    {
        get { return _UVS_COMP_ID; }
        set { _UVS_COMP_ID = value; }
    }

    public int? UVS_P_CODE
    {
        get { return _UVS_P_CODE; }
        set { _UVS_P_CODE = value; }
    }

    public int? UVS_I_CODE
    {
        get { return _UVS_I_CODE; }
        set { _UVS_I_CODE = value; }
    }

    public DateTime? UVS_DATE
    {
        get { return _UVS_DATE; }
        set { _UVS_DATE = value; }
    }

    public float? UVS_CASTING_OFFLOADED
    {
        get { return _UVS_CASTING_OFFLOADED; }
        set { _UVS_CASTING_OFFLOADED = value; }
    }

    public float? UVS_WEEK1
    {
        get { return _UVS_WEEK1; }
        set { _UVS_WEEK1 = value; }
    }

    public float? UVS_WEEK2
    {
        get { return _UVS_WEEK2; }
        set { _UVS_WEEK2 = value; }
    }

    public float? UVS_WEEK3
    {
        get { return _UVS_WEEK3; }
        set { _UVS_WEEK3 = value; }
    }

    public float? UVS_WEEK4
    {
        get { return _UVS_WEEK4; }
        set { _UVS_WEEK4 = value; }
    }

    public bool? ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }

    public bool? MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }

    public int? UVS_CM_CODE
    {
        get { return _UVS_CM_CODE; }
        set { _UVS_CM_CODE = value; }
    }

    public bool? UVS_STATUS
    {
        get { return _UVS_STATUS; }
        set { _UVS_STATUS = value; }
    }

    public bool? UVS_TYPE
    {
        get { return _UVS_TYPE; }
        set { _UVS_TYPE = value; }
    }

    public string UVS_DESC
    {
        get { return _UVS_DESC; }
        set { _UVS_DESC = value; }
    }

    public string message = "";

    public string Msg = "";


    public int PK_CODE;
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = {
            new SqlParameter("@PROCESS", "GetInfo"), 
            new SqlParameter("@UVS_CODE", UVS_CODE), 
        };
            dt = DL_DBAccess.SelectData("UNPLAN_VENDOR_SCHEDULE_SP", par, out Msg, out PK_CODE);
            if (dt.Rows.Count > 0)
            {
                UVS_CODE = Convert.ToInt32(dt.Rows[0]["UVS_CODE"]);
                UVS_P_CODE = Convert.ToInt32(dt.Rows[0]["UVS_P_CODE"].ToString());
                UVS_I_CODE = Convert.ToInt32(dt.Rows[0]["UVS_I_CODE"].ToString());
                UVS_DATE = Convert.ToDateTime(dt.Rows[0]["UVS_DATE"].ToString());
                UVS_WEEK1 = float.Parse(dt.Rows[0]["UVS_WEEK1"].ToString());
                UVS_WEEK2 = float.Parse(dt.Rows[0]["UVS_WEEK2"].ToString());
                UVS_WEEK3 = float.Parse(dt.Rows[0]["UVS_WEEK3"].ToString());
                UVS_WEEK4 = float.Parse(dt.Rows[0]["UVS_WEEK4"].ToString());
                UVS_CASTING_OFFLOADED = float.Parse(dt.Rows[0]["UVS_CASTING_OFFLOADED"].ToString());
            }
        }
        catch (Exception ex)
        {
             
        }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            dt = CommonClasses.Execute("select distinct INSM_CODE,IWD_CPOM_CODE,SPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,UOM_NAME,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,SPOD_RATE,SPOD_TOTAL_AMT,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,T_NAME,T_SALESTAX,SPOM_FREIGHT_AMT,SPOM_OCTROI_PER,SPOM_LOADING_PER,(SPOD_TOTAL_AMT*SPOD_EXC_PER/100) as ExcAmt,((SPOD_TOTAL_AMT-SPOD_DISC_AMT+ (SPOD_TOTAL_AMT*SPOD_EXC_PER/100)) * T_SALESTAX/100) as TaxAmt from INWARD_DETAIL,INWARD_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAIL,INSPECTION_S_MASTER,ITEM_MASTER,UNIT_MASTER,TAX_MASTER WHERE IWD_I_CODE=I_CODE and IWM_CODE=IWD_IWM_CODE and IWD_CPOM_CODE=SPOM_CODE and INWARD_MASTER.ES_DELETE=0 and IWD_I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and I_UM_CODE=UOM_CODE and IWM_CODE=IWD_IWM_CODE  AND IWD_INSP_FLG=1 AND SPOD_T_CODE=T_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE=INSM_SPOM_CODE  ");
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region FillGrid
    public DataTable FillGrid(string str)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = {
            new SqlParameter("@PROCESS", "FillGrid"), 
            new SqlParameter("@UVS_CM_CODE", UVS_CM_CODE),
            new SqlParameter("@str", str)
        };
            dt = DL_DBAccess.SelectData("UNPLAN_VENDOR_SCHEDULE_SP", par, out Msg, out PK_CODE);
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    #endregion

    #region Save
    public bool Save(string Process)
    {

        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
        }
        catch (Exception ex)
        { }
        try
        {
            SqlParameter[] Params = 
			{ 
                
                new SqlParameter("@PROCESS", Process),
				new SqlParameter("@UVS_CODE", UVS_CODE),
				new SqlParameter("@UVS_COMP_ID",UVS_COMP_ID),
				new SqlParameter("@UVS_P_CODE",UVS_P_CODE),
				new SqlParameter("@UVS_I_CODE",UVS_I_CODE),
				new SqlParameter("@UVS_DATE",UVS_DATE),
				new SqlParameter("@UVS_CASTING_OFFLOADED",UVS_CASTING_OFFLOADED),
				new SqlParameter("@UVS_WEEK1",UVS_WEEK1),
				new SqlParameter("@UVS_WEEK2",UVS_WEEK2),
				new SqlParameter("@UVS_WEEK3",UVS_WEEK3),
                new SqlParameter("@UVS_WEEK4",UVS_WEEK4),
                new SqlParameter("@UVS_CM_CODE",UVS_CM_CODE),   
                new SqlParameter("@UVS_STATUS",UVS_STATUS) 
				
			};

            result = DL_DBAccess.Insertion_Updation_Delete("UNPLAN_VENDOR_SCHEDULE_SP", Params, out message, out PK_CODE);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "Save/Update", Ex.Message);

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
            //Update Master Table Flag
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", UVS_CODE);
            par[1] = new SqlParameter("@PK_Field", "UVS_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "UNPLAN_VENDOR_SCHEDULE");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion
}
