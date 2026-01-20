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
/// Summary description for Inward_BL
/// </summary>
public class Inward_BL
{
    #region Constructor
    public Inward_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public Inward_BL(int Id)
    {
        IWM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Variables

    private int _IWM_CODE;
    private int _IWM_CM_CODE;
    private int _IWM_INWARD_TYPE;
    private int _IWM_NO;
    private string _IWM_TYPE;
    private DateTime _IWM_DATE;
    private int _IWM_P_CODE;
    private string _IWM_CHALLAN_NO;
    private DateTime _IWM_CHAL_DATE;
    private string _IWM_EGP_NO;
    private string _IWM_LR_NO;
    private string _IWM_OCT_NO;
    private string _IWM_VEH_NO;
    private int _IWM_SITE_CODE;
    private int _IWM_UOM_CODE;

    private string _IWM_INV_NO;
    private DateTime _IWM_INV_DATE;
    private string _IWM_TRANSPORATOR_NAME;
    private string _IWD_BATCH_NO;



    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int IWM_CODE
    {
        get { return _IWM_CODE; }
        set { _IWM_CODE = value; }
    }
    public int IWM_CM_CODE
    {
        get { return _IWM_CM_CODE; }
        set { _IWM_CM_CODE = value; }
    }
    public int IWM_INWARD_TYPE
    {
        get { return _IWM_INWARD_TYPE; }
        set { _IWM_INWARD_TYPE = value; }
    }

    public int IWM_NO
    {
        get { return _IWM_NO; }
        set { _IWM_NO = value; }
    }

    public string IWM_TYPE
    {
        get { return _IWM_TYPE; }
        set { _IWM_TYPE = value; }
    }
    public DateTime IWM_DATE
    {
        get { return _IWM_DATE; }
        set { _IWM_DATE = value; }
    }
    public int IWM_P_CODE
    {
        get { return _IWM_P_CODE; }
        set { _IWM_P_CODE = value; }
    }
    public string IWM_CHALLAN_NO
    {
        get { return _IWM_CHALLAN_NO; }
        set { _IWM_CHALLAN_NO = value; }
    }
    public DateTime IWM_CHAL_DATE
    {
        get { return _IWM_CHAL_DATE; }
        set { _IWM_CHAL_DATE = value; }
    }
    public string IWM_EGP_NO
    {
        get { return _IWM_EGP_NO; }
        set { _IWM_EGP_NO = value; }
    }
    public string IWM_LR_NO
    {
        get { return _IWM_LR_NO; }
        set { _IWM_LR_NO = value; }
    }
    public string IWM_OCT_NO
    {
        get { return _IWM_OCT_NO; }
        set { _IWM_OCT_NO = value; }
    }
    public string IWM_VEH_NO
    {
        get { return _IWM_VEH_NO; }
        set { _IWM_VEH_NO = value; }
    }
    public int IWM_SITE_CODE
    {
        get { return _IWM_SITE_CODE; }
        set { _IWM_SITE_CODE = value; }
    }

    public int IWM_UOM_CODE
    {
        get { return _IWM_UOM_CODE; }
        set { _IWM_UOM_CODE = value; }
    }

    public string IWM_INV_NO
    {
        get { return _IWM_INV_NO; }
        set { _IWM_INV_NO = value; }
    }

    public DateTime IWM_INV_DATE
    {
        get { return _IWM_INV_DATE; }
        set { _IWM_INV_DATE = value; }
    }
    public string IWM_TRANSPORATOR_NAME
    {
        get { return _IWM_TRANSPORATOR_NAME; }
        set { _IWM_TRANSPORATOR_NAME = value; }
    }
    public string IWD_BATCH_NO
    {
        get { return _IWD_BATCH_NO; }
        set { _IWD_BATCH_NO = value; }
    }




    int PK_CODE;

    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {

            dt = CommonClasses.Execute("select IWM_CODE,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE ,P_NAME from INWARD_MASTER,PARTY_MASTER where INWARD_MASTER.IWM_P_CODE=PARTY_MASTER.P_CODE AND INWARD_MASTER.ES_DELETE=0 and IWM_CM_CODE=" + IWM_CM_CODE + "");
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Save
    public bool Save(GridView XGrid)
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
            //Inserting Inward Master Part
            SqlParameter[] par = new SqlParameter[18];
            par[0] = new SqlParameter("@PROCESS", "Insert");
            par[1] = new SqlParameter("@IWM_CODE", DBNull.Value);
            par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
            par[3] = new SqlParameter("@IWM_INWARD_TYPE", IWM_INWARD_TYPE);
            par[4] = new SqlParameter("@IWM_NO", IWM_NO);
            par[5] = new SqlParameter("@IWM_TYPE", IWM_TYPE);
            par[6] = new SqlParameter("@IWM_DATE", IWM_DATE);
            par[7] = new SqlParameter("@IWM_P_CODE", IWM_P_CODE);
            par[8] = new SqlParameter("@IWM_CHALLAN_NO", IWM_CHALLAN_NO);
            par[9] = new SqlParameter("@IWM_CHAL_DATE", IWM_CHAL_DATE);
            par[10] = new SqlParameter("@IWM_EGP_NO", IWM_EGP_NO);
            par[11] = new SqlParameter("@IWM_LR_NO", IWM_LR_NO);
            par[12] = new SqlParameter("@IWM_OCT_NO", IWM_OCT_NO);
            par[13] = new SqlParameter("@IWM_VEH_NO", IWM_VEH_NO);
            //par[14] = new SqlParameter("@IWM_SITE_CODE", IWM_SITE_CODE);
            par[14] = new SqlParameter("@IWM_INV_NO", IWM_INV_NO);
            par[15] = new SqlParameter("@IWM_INV_DATE", IWM_INV_DATE);
            par[16] = new SqlParameter("@IWM_TRANSPORATOR_NAME", IWM_TRANSPORATOR_NAME);
            par[17] = new SqlParameter("@IWM_SITE_CODE", IWM_SITE_CODE);

            int plant_code = IWM_SITE_CODE;
            int MainstoreCode = -2147483647;
            int InwardstoreCode = -2147483648;
            if (plant_code == -2147483647)
            {
                MainstoreCode = -2147483634;
                InwardstoreCode = -2147483635;
            }

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_MASTER", par, out message, out PK_CODE);
            if (result == true)
            {
                bool shortclose = false;
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int IWD_IWM_CODE = PK_CODE;
                    int IWD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                    float IWD_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CH_QTY")).Text);
                    double IWD_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    float IWD_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    int IWD_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                    float IWD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_RATE")).Text);
                    string IWD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblIWD_REMARK")).Text;
                    int IWD_UOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);
                    string IWD_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblIWD_BATCH_NO")).Text;
                    string IWD_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_PROCESS_CODE")).Text;

                    int ASS_I_CODE = IWD_I_CODE;
                    string IWD_TUR_QTY = "0";
                    string IWD_TUR_WEIGHT = "0";

                    if (IWM_TYPE == "OUTCUSTINV")
                    {
                        IWD_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_QTY")).Text;
                        IWD_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_WEIGHT")).Text;
                    }

                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[15];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@IWD_IWM_CODE", IWD_IWM_CODE);
                    par1[2] = new SqlParameter("@IWD_I_CODE", IWD_I_CODE);
                    par1[3] = new SqlParameter("@IWD_CH_QTY", IWD_CH_QTY);
                    par1[4] = new SqlParameter("@IWD_REV_QTY", IWD_REV_QTY);
                    par1[5] = new SqlParameter("@IWD_SQTY", IWD_SQTY);
                    par1[6] = new SqlParameter("@IWD_CPOM_CODE", IWD_CPOM_CODE);
                    par1[7] = new SqlParameter("@IWD_RATE", Math.Round(IWD_RATE, 3));
                    par1[8] = new SqlParameter("@IWD_REMARK", IWD_REMARK);
                    par1[9] = new SqlParameter("@IWD_UOM_CODE", IWD_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@IWD_BATCH_NO", IWD_BATCH_NO);
                    par1[12] = new SqlParameter("@IWD_PROCESS_CODE", IWD_PROCESS_CODE);

                    par1[13] = new SqlParameter("@IWD_TUR_QTY", IWD_TUR_QTY);
                    par1[14] = new SqlParameter("@IWD_TUR_WEIGHT", IWD_TUR_WEIGHT);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_DETAIL", par1, out message);
                    double orgQty = IWD_REV_QTY;
                    double IWD_REV_QTYy = IWD_REV_QTY;
                    int Item_code = IWD_I_CODE;

                    #region for cash purchase inspection
                    if (IWM_P_CODE.ToString() == "-2147483578")//'CASH PARTY' code from cash inward without PO
                    {
                        CommonClasses.Execute("Update INWARD_DETAIL set IWD_INSP_FLG=1 where IWD_IWM_CODE='" + IWD_IWM_CODE + "'");
                    }
                    #endregion
                    #region challan stock ledger for sub contractor
                    if (IWM_TYPE != "IWIM" && IWM_TYPE != "IWCP")
                    {
                        DataTable rowmateril = new DataTable();
                        DataTable dtchallan = new DataTable();
                        if (IWM_INWARD_TYPE == 0)
                        {
                            dtchallan = CommonClasses.Execute("SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where SPOD_SPOM_CODE='" + IWD_CPOM_CODE + "' AND BM_I_CODE='" + IWD_I_CODE + "' AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + IWM_P_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0  ORDER BY  CL_DATE,Convert (int,CL_CH_NO)");
                            rowmateril = CommonClasses.Execute(" SELECT BD_SCRAPQTY+BD_VQTY AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + IWD_I_CODE + "' AND BOM_MASTER.ES_DELETE=0");

                            //IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[0]["BD_QTY"].ToString()));
                            //IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[0]["CL_I_CODE"].ToString());
                        }
                        //else if (IWM_INWARD_TYPE == 2)
                        //{
                        //    dtchallan = CommonClasses.Execute(" SELECT  SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,INM_INVOICE_TYPE  FROM CHALLAN_STOCK_LEDGER,INVOICE_MASTER,INVOICE_DETAIL  where CL_P_CODE='" + IWM_P_CODE + "' AND CL_DOC_TYPE='OutSUBINM'   AND INM_CODE=IND_INM_CODE  AND CL_DOC_ID=INM_CODE AND IND_CPOM_CODE ='" + IWD_CPOM_CODE + "'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE,INM_INVOICE_TYPE HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ORDER BY  Convert (int,CL_CH_NO)");

                        //}
                        else
                        {
                            dtchallan = CommonClasses.Execute("SELECT SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ORDER BY  CL_DATE, Convert (int,CL_CH_NO)");
                        }
                        if (dtchallan.Rows.Count > 0)
                        {
                            if (IWM_INWARD_TYPE == 0)
                            {
                                double qty = IWD_REV_QTY;
                                for (int s = 0; s < rowmateril.Rows.Count; s++)
                                {
                                    IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                    IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                    double Bal_Qty = IWD_REV_QTY;
                                    for (int z = 0; z < dtchallan.Rows.Count; z++)
                                    {
                                        if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == IWD_I_CODE.ToString())
                                        {
                                            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                            {
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute("UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                Bal_Qty = 0;

                                                break;
                                            }
                                            else
                                            {
                                                Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute("UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            }
                                        }

                                    }
                                    IWD_REV_QTY = qty;
                                }
                            }
                            #region hide
                            //if (IWM_INWARD_TYPE == 2)
                            //{
                            //    double Bal_Qty = IWD_REV_QTY;
                            //    for (int z = 0; z < dtchallan.Rows.Count; z++)
                            //    {
                            //        if (dtchallan.Rows[z]["INM_INVOICE_TYPE"].ToString()=="0")
                            //        {

                            //        }
                            //        if (Bal_Qty > 0)
                            //        {
                            //            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                            //            {
                            //                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                            //                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                            //                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                            //                Bal_Qty = 0;
                            //                break;
                            //            }
                            //            else
                            //            {
                            //                Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                            //                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                            //                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                            //                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                            //                IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                            //            }
                            //        }
                            //    }
                            //} 
                            #endregion
                            else
                            {

                                double Bal_Qty = IWD_REV_QTY;
                                for (int z = 0; z < dtchallan.Rows.Count; z++)
                                {
                                    if (Bal_Qty > 0)
                                    {
                                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                        {
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            Bal_Qty = 0;
                                            break;
                                        }
                                        else
                                        {
                                            Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    // CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + IWD_I_CODE + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','RAWINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + IWD_SQTY + "','','',-2147483648)");
                    #region Stock_Ledger
                    if (IWM_TYPE == "OUTCUSTINV")
                    {
                        // CommonClasses.Execute("DELETE from STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='RAWINWARD'");
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + Item_code + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','SUBINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + orgQty + "','','','" + InwardstoreCode + "')");

                    }
                    else
                    {
                        if (IWM_P_CODE.ToString() == "-2147483578")
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + Item_code + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','RAWINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + orgQty + "','','','" + MainstoreCode + "')");
                        }
                        else
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + Item_code + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','RAWINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + orgQty + "','','','" + InwardstoreCode + "')");
                        }
                    }
                    #endregion


                    #region item master curren bal update
                    //if (IWM_TYPE == "OUTCUSTINV")
                    //{
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + orgQty + ",I_RECEIPT_DATE='" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "' where I_CODE='" + Item_code + "'");
                    //}
                    #endregion

                    #region ShortClose
                    DataTable dtShortclose = new DataTable();
                    if (result == true)
                    {
                        //Update Supplier PO Qty
                        CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY+" + IWD_REV_QTYy + " where SPOD_I_CODE='" + Item_code + "' and SPOD_SPOM_CODE='" + IWD_CPOM_CODE + "'");
                        //CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + IWD_REV_QTY + ",I_RECEIPT_DATE='" + IWM_DATE + "' where I_CODE='" + IWD_I_CODE + "'");
                        // bool Res = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_IS_SHORT_CLOSE=1 WHERE SPOM_CODE='" + cpom_code + "'");
                        dtShortclose = CommonClasses.Execute("SELECT * FROM ITEM_MASTER,ITEM_CATEGORY_MASTER WHERE ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE and ITEM_CATEGORY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND ISNULL(ITEM_CATEGORY_MASTER.I_CAT_SHORTCLOSE,0)=1 AND I_CODE='" + Item_code + "' ");
                        if (dtShortclose.Rows.Count > 0)
                        {
                            bool Res = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_IS_SHORT_CLOSE=1 WHERE SPOM_CODE='" + IWD_CPOM_CODE + "'");
                        }
                    }
                    #endregion

                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Save", Ex.Message);

        }
        return result;
    }
    #endregion

    #region Update
    public bool Update(GridView XGrid)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            #region for delete old entires
            DataTable dtInwardDetail = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_RATE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE and IWD_IWM_CODE='" + IWM_CODE + "' and INWARD_MASTER.ES_DELETE='0'");
            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
            {
                CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtInwardDetail.Rows[k]["IWD_REV_QTY"] + " where  SPOD_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtInwardDetail.Rows[k]["IWD_CPOM_CODE"] + "'");
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[k]["IWD_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "'");

                PK_CODE = IWM_CODE;

                if (IWM_TYPE != "IWIM" && IWM_TYPE != "IWCP")
                {

                    DataTable stockLedger = new DataTable();
                    if (IWM_INWARD_TYPE == 0)
                    {
                        stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE IN (SELECT BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BOM_MASTER.ES_DELETE=0 AND BM_CODE=BD_BM_CODE AND BM_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "') ");
                        for (int z = 0; z < stockLedger.Rows.Count; z++)
                        {
                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                            CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "'  ");
                        }
                    }
                    else
                    {
                        stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' ");
                        for (int z = 0; z < stockLedger.Rows.Count; z++)
                        {
                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                        }
                        CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "'  ");
                    }
                }
            }

            #endregion
            //updating Master Record
            SqlParameter[] par = new SqlParameter[17];
            par[0] = new SqlParameter("@PROCESS", "Update");
            par[1] = new SqlParameter("@IWM_CODE", IWM_CODE);
            par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
            par[3] = new SqlParameter("@IWM_INWARD_TYPE", IWM_INWARD_TYPE);
            par[4] = new SqlParameter("@IWM_NO", IWM_NO);
            par[5] = new SqlParameter("@IWM_TYPE", IWM_TYPE);
            par[6] = new SqlParameter("@IWM_DATE", IWM_DATE);
            par[7] = new SqlParameter("@IWM_P_CODE", IWM_P_CODE);
            par[8] = new SqlParameter("@IWM_CHALLAN_NO", IWM_CHALLAN_NO);
            par[9] = new SqlParameter("@IWM_CHAL_DATE", IWM_CHAL_DATE);
            par[10] = new SqlParameter("@IWM_EGP_NO", IWM_EGP_NO);
            par[11] = new SqlParameter("@IWM_LR_NO", IWM_LR_NO);
            par[12] = new SqlParameter("@IWM_OCT_NO", IWM_OCT_NO);
            par[13] = new SqlParameter("@IWM_VEH_NO", IWM_VEH_NO);
            //par[14] = new SqlParameter("@IWM_SITE_CODE", IWM_SITE_CODE);
            par[14] = new SqlParameter("@IWM_INV_NO", IWM_INV_NO);
            par[15] = new SqlParameter("@IWM_INV_DATE", IWM_INV_DATE);
            par[16] = new SqlParameter("@IWM_TRANSPORATOR_NAME", IWM_TRANSPORATOR_NAME);

            int plant_code = IWM_SITE_CODE;
            int MainstoreCode = -2147483647;
            int InwardstoreCode = -2147483648;
            if (plant_code == -2147483647)
            {
                MainstoreCode = -2147483634;
                InwardstoreCode = -2147483635;
            }
            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_MASTER", par, out message, out PK_CODE);
            //result = DL_DBAccess.Insertion_Updation_Delete_Modify("SP_UPDATE_INWARD_MASTER", par, out PK_CODE);


            //Deleteing Inward Detail part
            #region deletefrom inward details
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM INWARD_DETAIL WHERE IWD_IWM_CODE='" + PK_CODE + "'");
                //CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='" + IWM_TYPE + "'");
                //CommonClasses.Execute("DELETE FROM GIN_STOCK_LEDGER where GL_DOC_ID='" + PK_CODE + "' and GL_DOC_TYPE='" + IWM_TYPE + "'");

            }
            #endregion

            #region delete from stock ledger
            if (IWM_TYPE == "OUTCUSTINV")
            {
                CommonClasses.Execute("DELETE from STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='SUBINWARD'");
            }
            else
            {
                CommonClasses.Execute("DELETE from STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='RAWINWARD'");

            }
            #endregion

            if (result == true)
            {

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int IWD_IWM_CODE = PK_CODE;
                    int IWD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                    float IWD_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CH_QTY")).Text);
                    double IWD_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    float IWD_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    int IWD_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                    float IWD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_RATE")).Text);
                    string IWD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblIWD_REMARK")).Text;
                    int IWD_UOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);

                    string IWD_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblIWD_BATCH_NO")).Text;

                    string IWD_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_PROCESS_CODE")).Text;

                    string IWD_TUR_QTY = "0";
                    string IWD_TUR_WEIGHT = "0";
                    int ASS_I_CODE = IWD_I_CODE;

                    if (IWM_TYPE == "OUTCUSTINV")
                    {
                        IWD_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_QTY")).Text;
                        IWD_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_WEIGHT")).Text;
                    }
                    //DataTable dtitemMaster = CommonClasses.Execute("select I_CURRENT_BAL,I_CODE from ITEM_MASTER where I_CODE='" + IWD_I_CODE + "' and ITEM_MASTER.ES_DELETE='0'");
                    //for (int p = 0; p < dtInwardDetail.Rows.Count; p++)
                    //{
                    //    if (dtInwardDetail.Rows[i]["IWD_I_CODE"].ToString() == IWD_I_CODE.ToString())
                    //    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[p]["IWD_REV_QTY"] + " where  I_CODE='" + IWD_I_CODE + "'");
                    //}

                    //Inserting new Inward Detail Part

                    SqlParameter[] par1 = new SqlParameter[15];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@IWD_IWM_CODE", IWD_IWM_CODE);
                    par1[2] = new SqlParameter("@IWD_I_CODE", IWD_I_CODE);
                    par1[3] = new SqlParameter("@IWD_CH_QTY", IWD_CH_QTY);
                    par1[4] = new SqlParameter("@IWD_REV_QTY", IWD_REV_QTY);
                    par1[5] = new SqlParameter("@IWD_SQTY", IWD_SQTY);
                    par1[6] = new SqlParameter("@IWD_CPOM_CODE", IWD_CPOM_CODE);
                    par1[7] = new SqlParameter("@IWD_RATE", IWD_RATE);
                    par1[8] = new SqlParameter("@IWD_REMARK", IWD_REMARK);
                    par1[9] = new SqlParameter("@IWD_UOM_CODE", IWD_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@IWD_BATCH_NO", IWD_BATCH_NO);
                    par1[12] = new SqlParameter("@IWD_PROCESS_CODE", IWD_PROCESS_CODE);

                    par1[13] = new SqlParameter("@IWD_TUR_QTY", IWD_TUR_QTY);
                    par1[14] = new SqlParameter("@IWD_TUR_WEIGHT", IWD_TUR_WEIGHT);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_DETAIL", par1, out message);

                    #region inseert into challan stock ledger
                    double IWD_REV_QTYy = IWD_REV_QTY;
                    if (IWM_TYPE != "IWIM" && IWM_TYPE != "IWCP")
                    {

                        DataTable rowmateril = new DataTable();
                        DataTable dtchallan = new DataTable();
                        if (IWM_INWARD_TYPE == 0)
                        {
                            dtchallan = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where SPOD_SPOM_CODE='" + IWD_CPOM_CODE + "' AND BM_I_CODE='" + IWD_I_CODE + "' AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + IWM_P_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ORDER BY  Convert (int,CL_CH_NO)");
                            rowmateril = CommonClasses.Execute(" SELECT BD_SCRAPQTY+BD_VQTY AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + IWD_I_CODE + "' AND BOM_MASTER.ES_DELETE=0");
                        }
                        else
                        {
                            dtchallan = CommonClasses.Execute("SELECT SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "'   AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0  ORDER BY  Convert (int,CL_CH_NO)");
                        }

                        #region MyRegion
                        /*
                        if (dtchallan.Rows.Count > 0)                       
                        {
                            double qty = IWD_REV_QTY;
                            if (IWM_INWARD_TYPE == 0)
                            {

                                for (int s = 0; s < rowmateril.Rows.Count; s++)
                                {
                                    IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                    IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                    double Bal_Qty = IWD_REV_QTY;
                                    for (int z = 0; z < dtchallan.Rows.Count; z++)
                                    {
                                        if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == IWD_I_CODE.ToString())
                                        {
                                            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                            {
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                Bal_Qty = 0;
                                                break;
                                            }
                                            else
                                            {
                                                Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            }
                                        }
                                    }
                                    IWD_REV_QTY = qty;
                                }
                            }
                            else
                            {
                                double Bal_Qty = IWD_REV_QTY;
                                for (int z = 0; z < dtchallan.Rows.Count; z++)
                                {
                                    if (Bal_Qty > 0)
                                    {
                                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                        {
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            Bal_Qty = 0;
                                            break;
                                        }
                                        else
                                        {
                                            Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                        }
                                    }
                                    IWD_REV_QTY = qty;
                                }
                            }
                        } 
                       
                        */
                        #endregion
                        if (dtchallan.Rows.Count > 0)
                        {
                            if (IWM_INWARD_TYPE == 0)
                            {
                                double qty = IWD_REV_QTY;
                                for (int s = 0; s < rowmateril.Rows.Count; s++)
                                {
                                    IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                    IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                    double Bal_Qty = IWD_REV_QTY;
                                    for (int z = 0; z < dtchallan.Rows.Count; z++)
                                    {
                                        if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == IWD_I_CODE.ToString())
                                        {
                                            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                            {
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                Bal_Qty = 0;
                                                break;
                                            }
                                            else
                                            {
                                                Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            }
                                        }
                                    }
                                    IWD_REV_QTY = qty;
                                }
                            }
                            else
                            {


                                for (int z = 0; z < dtchallan.Rows.Count; z++)
                                {
                                    double Bal_Qty = IWD_REV_QTY;
                                    if (Bal_Qty > 0)
                                    {
                                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                        {
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            Bal_Qty = 0;
                                            break;
                                        }
                                        else
                                        {
                                            Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + IWM_P_CODE + "','" + ASS_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + IWD_IWM_CODE + "','" + IWM_NO + "','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region insert into stock ledger
                    if (IWM_TYPE == "OUTCUSTINV")
                    {
                        // CommonClasses.Execute("DELETE from STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='RAWINWARD'");
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + IWD_I_CODE + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','SUBINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + IWD_SQTY + "','','','"+InwardstoreCode+"')");
                    }
                    else
                    {
                        if (IWM_P_CODE.ToString() == "-2147483578")
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + IWD_I_CODE + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','RAWINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + IWD_SQTY + "','','','"+MainstoreCode+"')");

                        }
                        else
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_INSP_FLAG,STL_SIT_CODE,STL_STORE_TYPE) values('" + IWD_I_CODE + "','" + IWD_IWM_CODE + "','" + IWM_NO + "','RAWINWARD','" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "','" + IWD_SQTY + "','','','"+InwardstoreCode+"')");

                        }

                    }
                    #endregion

                    #region item master current bal update
                    //if (IWM_TYPE == "OUTCUSTINV")
                    //{
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + IWD_REV_QTY + ",I_RECEIPT_DATE='" + Convert.ToDateTime(IWM_DATE).ToString("dd/MMM/yyyy") + "' where I_CODE='" + IWD_I_CODE + "'");
                    //}
                    #endregion

                    if (result == true)
                    {
                        DataTable dtSpodUpdate = CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY+" + IWD_REV_QTYy + " where SPOD_I_CODE='" + IWD_I_CODE + "' and SPOD_SPOM_CODE='" + IWD_CPOM_CODE + "'");
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Update", Ex.Message);

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
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@PK_CODE", IWM_CODE);
            par[1] = new SqlParameter("@PK_Field", "IWM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "INWARD_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //Delete from stock table


                DataTable dtInwardDetail = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_RATE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE and IWD_IWM_CODE='" + IWM_CODE + "' and INWARD_MASTER.ES_DELETE='0'");
                for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtInwardDetail.Rows[k]["IWD_REV_QTY"] + " where  SPOD_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtInwardDetail.Rows[k]["IWD_CPOM_CODE"] + "'");
                    // CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[k]["IWD_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "'");
                    PK_CODE = IWM_CODE;

                    if (IWM_TYPE != "IWIM" && IWM_TYPE != "IWCP")
                    {
                        DataTable stockLedger = new DataTable();
                        if (IWM_INWARD_TYPE == 0)
                        {
                            stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE IN (SELECT BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BOM_MASTER.ES_DELETE=0 AND BM_CODE=BD_BM_CODE AND BM_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "') ");
                            for (int z = 0; z < stockLedger.Rows.Count; z++)
                            {
                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                                CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "'  ");
                            }
                        }
                        else
                        {
                            stockLedger = CommonClasses.Execute("SELECT * FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' ");
                            for (int z = 0; z < stockLedger.Rows.Count; z++)
                            {
                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                            }
                            CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "'  ");
                        }
                    }
                    if (IWM_TYPE != "IWIM")
                    {
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER where STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='SUBINWARD'");
                    }
                    else
                    {
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER where STL_DOC_NO='" + PK_CODE + "' and STL_DOC_TYPE='RAWINWARD'");
                    }
                }

                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + IWM_CODE + "' and STL_DOC_TYPE='IWIM' ");
                }
            }

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion

    #region CheckExistSaveNo
    public bool CheckExistSaveNo(string genpono, string IN_MIWM_CM_CODE)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("Select IWM_NO from INWARD_MASTER where ES_DELETE<> '1' and IWM_NO='" + genpono + "' and IWM_CM_CODE=" + IN_MIWM_CM_CODE + "");
            if (dt.Rows.Count > 0)
            {
                res = true;
                //MessageBox.Show("GIN  Number Already Exists");
                // throw new Exception("Inward Number Already Exists");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "CheckExistSaveNo", Ex.Message);

        }
        return res;
    }
    #endregion

}
