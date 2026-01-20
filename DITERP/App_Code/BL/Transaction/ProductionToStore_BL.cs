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
/// Summary description for ProductionToStore_BL
/// </summary>
public class ProductionToStore_BL
{
    #region Counstructor
    public ProductionToStore_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public ProductionToStore_BL(int Id)
    {
        PS_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    private int _PS_CODE;
    private int _PS_MR_CODE;
    private int _PS_CM_COMP_CODE;
    private int _PS_GIN_NO;
    private DateTime _PS_GIN_DATE;
    private string _PS_PERSON_NAME;
    private int _PS_TYPE;
    private int _PS_BATCH_NO;
    private int _PS_P_CODE;


    public string message = "";
    public int PK_CODE;
    public string Msg = "";
    #endregion

    #region Public Properties
    public int PS_CODE
    {
        get { return _PS_CODE; }
        set { _PS_CODE = value; }
    }
    public int PS_CM_COMP_CODE
    {
        get { return _PS_CM_COMP_CODE; }
        set { _PS_CM_COMP_CODE = value; }
    }
    public int PS_GIN_NO
    {
        get { return _PS_GIN_NO; }
        set { _PS_GIN_NO = value; }
    }
    public DateTime PS_GIN_DATE
    {
        get { return _PS_GIN_DATE; }
        set { _PS_GIN_DATE = value; }
    }
    public string PS_PERSON_NAME
    {
        get { return _PS_PERSON_NAME; }
        set { _PS_PERSON_NAME = value; }
    }
    public int PS_TYPE
    {
        get { return _PS_TYPE; }
        set { _PS_TYPE = value; }
    }
    public int PS_MR_CODE
    {
        get { return _PS_MR_CODE; }
        set { _PS_MR_CODE = value; }
    }
    public int PS_BATCH_NO
    {
        get { return _PS_BATCH_NO; }
        set { _PS_BATCH_NO = value; }
    }
    public int PS_P_CODE
    {
        get { return _PS_P_CODE; }
        set { _PS_P_CODE = value; }
    }
    #endregion

    #region Save
    public bool Save(GridView XGrid, string Process)
    {
        string Proc;
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
            if (Process == "INSERT")
            {
                Proc = "Insert";
            }
            else
            {
                Proc = "Update";
            }

            SqlParameter[] Params = 
			{ 
                
                new SqlParameter("@PROCESS", Proc),
				new SqlParameter("@PS_CODE",PS_CODE),
                new SqlParameter("@PS_MR_CODE",PS_MR_CODE),
				new SqlParameter("@PS_CM_COMP_CODE",PS_CM_COMP_CODE),
				new SqlParameter("@PS_GIN_NO",PS_GIN_NO),
				new SqlParameter("@PS_GIN_DATE",PS_GIN_DATE),
				new SqlParameter("@PS_PERSON_NAME",PS_PERSON_NAME),
				new SqlParameter("@PS_TYPE",PS_TYPE),
			    new SqlParameter("@PS_BATCH_NO",PS_BATCH_NO),
				new SqlParameter("@PS_P_CODE",PS_P_CODE)
				
			};

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PRODUCTION_TO_STORE_MASTER", Params, out message, out PK_CODE);
            int PSD_PS_CODE = PK_CODE;
            if (Process == "INSERT")
            {

            }
            else
            {
                DataTable dtProductionDetail = CommonClasses.Execute("select PSD_PS_CODE,PSD_I_CODE,PSD_QTY from PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER where PSD_PS_CODE=PS_CODE and PSD_PS_CODE='" + PK_CODE + "'");

                float Quantity = 0;
                float QuantityTot = 0;
                for (int k = 0; k < dtProductionDetail.Rows.Count; k++)
                {
                    //CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtProductionDetail.Rows[k]["IWD_REV_QTY"] + " where  SPOD_I_CODE='" + dtProductionDetail.Rows[k]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtProductionDetail.Rows[k]["IWD_CPOM_CODE"] + "'");
                    Quantity = float.Parse(dtProductionDetail.Rows[k]["PSD_QTY"].ToString());
                  //  QuantityTot = QuantityTot + Quantity;
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + Quantity + " where  I_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");
                }
                result = CommonClasses.Execute1("delete from PRODUCTION_TO_STORE_DETAIL where PSD_PS_CODE='" + PK_CODE + "'");
                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='PRODTOSTORE'");

            }
            if (result == true)
            {
                 for (int i = 0; i < XGrid.Rows.Count; i++)
                {
                    string PSD_I_CODE = "";
                    string PSD_UOM_CODE = "";
                    string PSD_REMARK = "";
                    string BT_CODE = "";
                    float PSD_QTY = 0;
                    float PSD_QTY_TOTAL = 0;

                    PSD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblPSD_I_CODE")).Text;
                    PSD_UOM_CODE = ((Label)XGrid.Rows[i].FindControl("lblUOMCODE")).Text;
                    PSD_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPSD_QTY")).Text);
                    PSD_QTY_TOTAL = PSD_QTY_TOTAL + PSD_QTY;
                    PSD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblPSD_REMARK")).Text;
                    BT_CODE = ((Label)XGrid.Rows[i].FindControl("lblBT_CODE")).Text;
                    //Inserting Production Detail Part
                    SqlParameter[] Params1 =               
                    	  { 
                               
				                new SqlParameter("@PSD_PS_CODE",PSD_PS_CODE),
				                new SqlParameter("@PSD_I_CODE",PSD_I_CODE),
                                 //new SqlParameter("@PSD_UOM_CODE",PSD_UOM_CODE),
				                new SqlParameter("@PSD_QTY",PSD_QTY),
				                new SqlParameter("@PSD_REMARK",PSD_REMARK),
                               // new SqlParameter("@BT_CODE",BT_CODE ),
                                new SqlParameter("@PK_CODE", DBNull.Value)
			                };

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PRODUCTION_TO_STORE_DETAIL", Params1, out message);
                    result = CommonClasses.Execute1("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + PSD_QTY_TOTAL + ",I_RECEIPT_DATE='" + PS_GIN_DATE.ToString("dd/MMM/yyyy") + "' where I_CODE='" + PSD_I_CODE + "'");
                    #region Insert Into Stock Ledger
                    if (result == true)
                    {
                        SqlParameter[] par2 = new SqlParameter[7];
                        par2[0] = new SqlParameter("@STL_I_CODE", PSD_I_CODE);
                        par2[1] = new SqlParameter("@STL_DOC_NO", PSD_PS_CODE);
                        par2[2] = new SqlParameter("@STL_DOC_NUMBER", PS_GIN_NO);
                        par2[3] = new SqlParameter("@STL_DOC_TYPE", "PRODTOSTORE");
                        par2[4] = new SqlParameter("@STL_DOC_DATE", PS_GIN_DATE);
                        par2[5] = new SqlParameter("@STL_DOC_QTY", PSD_QTY_TOTAL);
                        par2[6] = new SqlParameter("@STL_STORE_TYPE", PS_P_CODE);
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    }
                    #endregion


                }




            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Prod TO Store ", "Save/Update", Ex.Message);

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
            par[0] = new SqlParameter("@PK_CODE", PS_CODE);
            par[1] = new SqlParameter("@PK_Field", "PS_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "PRODUCTION_TO_STORE_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);
            if (result)
            {
                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PS_CODE + "' and  STL_DOC_TYPE='PRODTOSTORE'");
                DataTable dtProductionDetail = CommonClasses.Execute("select PSD_PS_CODE,PSD_I_CODE,PSD_QTY from PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER where PSD_PS_CODE=PS_CODE and PSD_PS_CODE='" + PS_CODE + "'");

                float Quantity = 0;
                float QtyTotal = 0;
                for (int k = 0; k < dtProductionDetail.Rows.Count; k++)
                {
                    Quantity = float.Parse(dtProductionDetail.Rows[k]["PSD_QTY"].ToString());
                    QtyTotal = QtyTotal + Quantity;

                }
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + QtyTotal + " where  I_CODE='" + dtProductionDetail.Rows[0]["PSD_I_CODE"] + "'");
            }
            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion
}
