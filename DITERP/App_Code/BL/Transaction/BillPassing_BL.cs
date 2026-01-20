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
/// Summary description for BillPassing_BL
/// </summary>
public class BillPassing_BL
{
    #region Counstructor
    public BillPassing_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public BillPassing_BL(int Id)
    {
        BPM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    private int _BPM_CODE;
    private int _BPM_CM_CODE;
    private decimal _BPM_NO;
    private DateTime _BPM_DATE;
    private int _BPM_P_CODE;
    private int _BPM_IWM_CODE;
    private string _BPM_INV_NO;
    private DateTime _BPM_INV_DATE;
    private string _BPM_BILL_PASS_BY;
    private double _BPM_BASIC_AMT;
    private double _BPM_EXCIES_AMT;
    private double _BPM_ECESS_AMT;
    private double _BPM_HECESS_AMT;
    private double _BPM_TAX_AMT;
    private double _BPM_DISCOUNT_AMT;
    private double _BPM_NET_AMT;
    private double _BPM_DISC_PER;
    private double _BPM_DISC_AMT;
    private double _BPM_FREIGHT;
    private double _BPM_PACKING_PER;
    private double _BPM_PACKING_AMT;
    private double _BPM_OCTRO_PER;
    private double _BPM_OCTRO_AMT;
    private double _BPM_G_AMT;
    private double _BPM_LOADING_AMT;
    private double _BPM_AS_PER_BILL_AMT;
    private double _BPM_DIFFRENCE;
    private bool _BPM_EX_YN;
    private string _BPM_GATE_NO;
    private DateTime _BPM_GATE_DATE;
    private double _BPM_GEXCISE;
    private double _BPM_GCESS;
    private double _BPM_GSHCESS;
    private int _BPM_EX_CODE;
    private bool _ES_DELETE;
    private bool _MODIFY;
    private double _BPM_D_FREIGHT;
    private byte _BPM_TALLYTNF;
    private byte _BPM_TEMP_TALLYTNF;
    private decimal _BPM_TALLY_VNO;
    private string _BPM_TALLY_VMONTH;
    private string _BPM_TALLY_VFNO;
    private DateTime _BPM_TALLY_DATE;
    private string _BPM_TYPE;
    private string _BPM_EX_TYPE;
    private double _BPM_ACCESS_AMT;
    private double _BPM_TAXABLE_AMT;
    private double _BPM_OTHER_AMT;
    private double _BPM_ADD_DUTY;
    private double _BPM_INSURRANCE;
    private double _BPM_TRANSPORT;
    private double _BPM_EXCPER;
    private double _BPM_EXCEDCESS_PER;
    private double _BPM_EXCHIEDU_PER;
    private double _BPM_TAX_PER;
    private int _BPM_TAX_CODE;
    private double _BPM_ROUND_OFF;
    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int BPM_CODE
    {
        get { return _BPM_CODE; }
        set { _BPM_CODE = value; }
    }
    public int BPM_CM_CODE
    {
        get { return _BPM_CM_CODE; }
        set { _BPM_CM_CODE = value; }
    }
    public decimal BPM_NO
    {
        get { return _BPM_NO; }
        set { _BPM_NO = value; }
    }
    public System.DateTime BPM_DATE
    {
        get { return _BPM_DATE; }
        set { _BPM_DATE = value; }
    }
    public int BPM_P_CODE
    {
        get { return _BPM_P_CODE; }
        set { _BPM_P_CODE = value; }
    }
    public int BPM_IWM_CODE
    {
        get { return _BPM_IWM_CODE; }
        set { _BPM_IWM_CODE = value; }
    }
    public string BPM_INV_NO
    {
        get { return _BPM_INV_NO; }
        set { _BPM_INV_NO = value; }
    }
    public System.DateTime BPM_INV_DATE
    {
        get { return _BPM_INV_DATE; }
        set { _BPM_INV_DATE = value; }
    }
    public string BPM_BILL_PASS_BY
    {
        get { return _BPM_BILL_PASS_BY; }
        set { _BPM_BILL_PASS_BY = value; }
    }
    public double BPM_BASIC_AMT
    {
        get { return _BPM_BASIC_AMT; }
        set { _BPM_BASIC_AMT = value; }
    }
    public double BPM_EXCIES_AMT
    {
        get { return _BPM_EXCIES_AMT; }
        set { _BPM_EXCIES_AMT = value; }
    }
    public double BPM_ECESS_AMT
    {
        get { return _BPM_ECESS_AMT; }
        set { _BPM_ECESS_AMT = value; }
    }
    public double BPM_HECESS_AMT
    {
        get { return _BPM_HECESS_AMT; }
        set { _BPM_HECESS_AMT = value; }
    }
    public double BPM_TAX_AMT
    {
        get { return _BPM_TAX_AMT; }
        set { _BPM_TAX_AMT = value; }
    }
    public double BPM_DISCOUNT_AMT
    {
        get { return _BPM_DISCOUNT_AMT; }
        set { _BPM_DISCOUNT_AMT = value; }
    }
    public double BPM_NET_AMT
    {
        get { return _BPM_NET_AMT; }
        set { _BPM_NET_AMT = value; }
    }
    public double BPM_DISC_PER
    {
        get { return _BPM_DISC_PER; }
        set { _BPM_DISC_PER = value; }
    }
    public double BPM_DISC_AMT
    {
        get { return _BPM_DISC_AMT; }
        set { _BPM_DISC_AMT = value; }
    }
    public double BPM_FREIGHT
    {
        get { return _BPM_FREIGHT; }
        set { _BPM_FREIGHT = value; }
    }
    public double BPM_PACKING_PER
    {
        get { return _BPM_PACKING_PER; }
        set { _BPM_PACKING_PER = value; }
    }
    public double BPM_PACKING_AMT
    {
        get { return _BPM_PACKING_AMT; }
        set { _BPM_PACKING_AMT = value; }
    }
    public double BPM_OCTRO_PER
    {
        get { return _BPM_OCTRO_PER; }
        set { _BPM_OCTRO_PER = value; }
    }
    public double BPM_OCTRO_AMT
    {
        get { return _BPM_OCTRO_AMT; }
        set { _BPM_OCTRO_AMT = value; }
    }
    public double BPM_G_AMT
    {
        get { return _BPM_G_AMT; }
        set { _BPM_G_AMT = value; }
    }
    public double BPM_AS_PER_BILL_AMT
    {
        get { return _BPM_AS_PER_BILL_AMT; }
        set { _BPM_AS_PER_BILL_AMT = value; }
    }
    public double BPM_DIFFRENCE
    {
        get { return _BPM_DIFFRENCE; }
        set { _BPM_DIFFRENCE = value; }
    }
    public bool BPM_EX_YN
    {
        get { return _BPM_EX_YN; }
        set { _BPM_EX_YN = value; }
    }
    public string BPM_GATE_NO
    {
        get { return _BPM_GATE_NO; }
        set { _BPM_GATE_NO = value; }
    }
    public System.DateTime BPM_GATE_DATE
    {
        get { return _BPM_GATE_DATE; }
        set { _BPM_GATE_DATE = value; }
    }
    public double BPM_GEXCISE
    {
        get { return _BPM_GEXCISE; }
        set { _BPM_GEXCISE = value; }
    }
    public double BPM_GCESS
    {
        get { return _BPM_GCESS; }
        set { _BPM_GCESS = value; }
    }
    public double BPM_GSHCESS
    {
        get { return _BPM_GSHCESS; }
        set { _BPM_GSHCESS = value; }
    }
    public int BPM_EX_CODE
    {
        get { return _BPM_EX_CODE; }
        set { _BPM_EX_CODE = value; }
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
    public double BPM_D_FREIGHT
    {
        get { return _BPM_D_FREIGHT; }
        set { _BPM_D_FREIGHT = value; }
    }
    public byte BPM_TALLYTNF
    {
        get { return _BPM_TALLYTNF; }
        set { _BPM_TALLYTNF = value; }
    }
    public byte BPM_TEMP_TALLYTNF
    {
        get { return _BPM_TEMP_TALLYTNF; }
        set { _BPM_TEMP_TALLYTNF = value; }
    }
    public decimal BPM_TALLY_VNO
    {
        get { return _BPM_TALLY_VNO; }
        set { _BPM_TALLY_VNO = value; }
    }
    public string BPM_TALLY_VMONTH
    {
        get { return _BPM_TALLY_VMONTH; }
        set { _BPM_TALLY_VMONTH = value; }
    }
    public string BPM_TALLY_VFNO
    {
        get { return _BPM_TALLY_VFNO; }
        set { _BPM_TALLY_VFNO = value; }
    }
    public System.DateTime BPM_TALLY_DATE
    {
        get { return _BPM_TALLY_DATE; }
        set { _BPM_TALLY_DATE = value; }
    }
    public string BPM_TYPE
    {
        get { return _BPM_TYPE; }
        set { _BPM_TYPE = value; }
    }

    public double BPM_LOADING_AMT
    {
        get { return _BPM_LOADING_AMT; }
        set { _BPM_LOADING_AMT = value; }
    }

    public double BPM_ACCESS_AMT
    {
        get { return _BPM_ACCESS_AMT; }
        set { _BPM_ACCESS_AMT = value; }
    }
    public double BPM_TAXABLE_AMT
    {
        get { return _BPM_TAXABLE_AMT; }
        set { _BPM_TAXABLE_AMT = value; }
    }
    public double BPM_OTHER_AMT
    {
        get { return _BPM_OTHER_AMT; }
        set { _BPM_OTHER_AMT = value; }
    }
    public double BPM_ADD_DUTY
    {
        get { return _BPM_ADD_DUTY; }
        set { _BPM_ADD_DUTY = value; }
    }
    public double BPM_INSURRANCE
    {
        get { return _BPM_INSURRANCE; }
        set { _BPM_INSURRANCE = value; }
    }
    public double BPM_TRANSPORT
    {
        get { return _BPM_TRANSPORT; }
        set { _BPM_TRANSPORT = value; }
    }
    public double BPM_EXCPER
    {
        get { return _BPM_EXCPER; }
        set { _BPM_EXCPER = value; }
    }
    public double BPM_EXCEDCESS_PER
    {
        get { return _BPM_EXCEDCESS_PER; }
        set { _BPM_EXCEDCESS_PER = value; }
    }
    public double BPM_EXCHIEDU_PER
    {
        get { return _BPM_EXCHIEDU_PER; }
        set { _BPM_EXCHIEDU_PER = value; }
    }
    public double BPM_TAX_PER
    {
        get { return _BPM_TAX_PER; }
        set { _BPM_TAX_PER = value; }
    }
    public int BPM_TAX_CODE
    {
        get { return _BPM_TAX_CODE; }
        set { _BPM_TAX_CODE = value; }
    }
    public double BPM_ROUND_OFF
    {
        get { return _BPM_ROUND_OFF; }
        set { _BPM_ROUND_OFF = value; }
    }
    public string BPM_EX_TYPE
    {
        get { return _BPM_EX_TYPE; }
        set { _BPM_EX_TYPE = value; }
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
            dt = CommonClasses.Execute("select distinct INSM_CODE,IWD_CPOM_CODE,SPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,UOM_NAME,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,SPOD_RATE,SPOD_TOTAL_AMT,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,T_NAME,T_SALESTAX,SPOM_FREIGHT_AMT,SPOM_OCTROI_PER,SPOM_LOADING_PER,(SPOD_TOTAL_AMT*SPOD_EXC_PER/100) as ExcAmt,((SPOD_TOTAL_AMT-SPOD_DISC_AMT+ (SPOD_TOTAL_AMT*SPOD_EXC_PER/100)) * T_SALESTAX/100) as TaxAmt from INWARD_DETAIL,INWARD_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAIL,INSPECTION_S_MASTER,ITEM_MASTER,UNIT_MASTER,TAX_MASTER WHERE IWD_I_CODE=I_CODE and IWM_CODE=IWD_IWM_CODE and IWD_CPOM_CODE=SPOM_CODE and INWARD_MASTER.ES_DELETE=0 and IWD_I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and I_UM_CODE=UOM_CODE and IWM_CODE=IWD_IWM_CODE  AND IWD_INSP_FLG=1 AND SPOD_T_CODE=T_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE=INSM_SPOM_CODE and INSM_CM_CODE='" + BPM_CM_CODE + "'");
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
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
				new SqlParameter("@BPM_CODE",BPM_CODE),
				new SqlParameter("@BPM_CM_CODE",BPM_CM_CODE),
				new SqlParameter("@BPM_NO",BPM_NO),
				new SqlParameter("@BPM_DATE",BPM_DATE),
				new SqlParameter("@BPM_P_CODE",BPM_P_CODE),
				new SqlParameter("@BPM_INV_NO",BPM_INV_NO),
				new SqlParameter("@BPM_INV_DATE",BPM_INV_DATE),
				new SqlParameter("@BPM_BILL_PASS_BY",BPM_BILL_PASS_BY),
				new SqlParameter("@BPM_BASIC_AMT",BPM_BASIC_AMT),
                new SqlParameter("@BPM_DISCOUNT_AMT",BPM_DISCOUNT_AMT),
                new SqlParameter("@BPM_PACKING_AMT",BPM_PACKING_AMT),   
                new SqlParameter("@BPM_ACCESS_AMT",BPM_ACCESS_AMT),    
				new SqlParameter("@BPM_EXCIES_AMT",BPM_EXCIES_AMT),
				new SqlParameter("@BPM_ECESS_AMT",BPM_ECESS_AMT),
				new SqlParameter("@BPM_HECESS_AMT",BPM_HECESS_AMT),
                new SqlParameter("@BPM_EXCPER",BPM_EXCPER),
                new SqlParameter("@BPM_EXCEDCESS_PER",BPM_EXCEDCESS_PER),
                new SqlParameter("@BPM_EXCHIEDU_PER",BPM_EXCHIEDU_PER),
                new SqlParameter("@BPM_TAXABLE_AMT",BPM_TAXABLE_AMT),
				new SqlParameter("@BPM_TAX_AMT",BPM_TAX_AMT),
                new SqlParameter("@BPM_TAX_PER",BPM_TAX_PER),
                new SqlParameter("@BPM_TAX_CODE",BPM_TAX_CODE),
				new SqlParameter("@BPM_OTHER_AMT",BPM_OTHER_AMT),
				new SqlParameter("@BPM_ADD_DUTY",BPM_ADD_DUTY),
				new SqlParameter("@BPM_FREIGHT",BPM_FREIGHT),
				new SqlParameter("@BPM_INSURRANCE",BPM_INSURRANCE),
				new SqlParameter("@BPM_TRANSPORT",BPM_TRANSPORT),
				new SqlParameter("@BPM_ROUND_OFF",BPM_ROUND_OFF),
                new SqlParameter("@BPM_OCTRO_AMT",BPM_OCTRO_AMT),			
				new SqlParameter("@BPM_G_AMT",BPM_G_AMT) ,
                new SqlParameter("@BPM_EX_TYPE",BPM_EX_TYPE)  ,
                new SqlParameter("@BPM_TYPE",BPM_TYPE)  
				
			};

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_BILLPASSING_MASTER", Params, out message, out PK_CODE);
            int BPD_BPM_CODE = PK_CODE;
            if (Proc == "Update")
            {
                result = CommonClasses.Execute1("delete from BILL_PASSING_DETAIL where BPD_BPM_CODE='" + BPD_BPM_CODE + "'");

            }
            if (result == true)
            {

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {
                    int BPD_I_CODE;
                    int BPD_IWM_CODE;
                    CheckBox chkRow = (((CheckBox)(XGrid.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        BPD_IWM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWM_CODE")).Text);
                        int BPD_SPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                        BPD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                        float BPD_RECD_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIIWD_REV_QTY")).Text);
                        float BPD_OK_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CON_OK_QTY")).Text);
                        float BPD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSPOD_RATE")).Text);
                        float BPD_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSPOD_TOTAL_AMT")).Text);
                        float BPD_EXC_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblEX_EX_DUTY")).Text);
                        float BPD_EDU_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblEX_EX_CESS")).Text);
                        float BPD_HSEDU_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblEX_EX_HCESS")).Text);
                        float BPD_DISC_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSPOD_DISC_AMT")).Text);
                        //Inserting Inward Detail Part
                        SqlParameter[] Params1 =               
                    	  {                               
				                new SqlParameter("@BPD_BPM_CODE",BPD_BPM_CODE),
				                new SqlParameter("@BPD_IWM_CODE",BPD_IWM_CODE),
				                new SqlParameter("@BPD_SPOM_CODE",BPD_SPOM_CODE),
				                new SqlParameter("@BPD_I_CODE",BPD_I_CODE),
				                new SqlParameter("@BPD_RECD_QTY",Math.Round(BPD_RECD_QTY,2)),
				                new SqlParameter("@BPD_OK_QTY",Math.Round(BPD_OK_QTY,2)),
				                new SqlParameter("@BPD_RATE",Math.Round(BPD_RATE,2)),
				                new SqlParameter("@BPD_AMT",Math.Round(BPD_AMT,2)),  
                                new SqlParameter("@BPD_EXC_AMT",Math.Round(BPD_EXC_AMT,2)),  
                                   new SqlParameter("@BPD_EDU_AMT",Math.Round(BPD_EDU_AMT,2)),
                                      new SqlParameter("@BPD_HSEDU_AMT",Math.Round(BPD_HSEDU_AMT,2)),
				                new SqlParameter("@BPD_DISC_AMT",Math.Round(BPD_DISC_AMT,2)),  
                                new SqlParameter("@PK_CODE", DBNull.Value)
			                };

                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_BILLPASSING_DETAIL", Params1, out message);
                        // Update Query To Update the flag in INward Master
                        if (result == true)
                        {
                            //Update Inward Detail Flag
                            CommonClasses.Execute("Update INWARD_DETAIL set IWD_BILL_PASS_FLG=1 where IWD_I_CODE='" + BPD_I_CODE + "' and IWD_IWM_CODE='" + BPD_IWM_CODE + "'");
                            //CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + IWD_REV_QTY + " where I_CODE='" + IWD_I_CODE + "'");
                        }
                    }
                    else
                    {
                        BPD_IWM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWM_CODE")).Text);
                        BPD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                        CommonClasses.Execute("Update INWARD_DETAIL set IWD_BILL_PASS_FLG=0 where IWD_I_CODE='" + BPD_I_CODE + "' and IWD_IWM_CODE='" + BPD_IWM_CODE + "'");
                    }

                }

            }
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
            par[0] = new SqlParameter("@PK_CODE", BPM_CODE);
            par[1] = new SqlParameter("@PK_Field", "BPM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "BILL_PASSING_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                DataTable dt = CommonClasses.Execute("select BPD_IWM_CODE from BILL_PASSING_MASTER,BILL_PASSING_DETAIL where BPM_CODE=BPD_BPM_CODE and  BPM_CODE='" + BPM_CODE + "'");
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string Inward_Code = dt.Rows[i]["BPD_IWM_CODE"].ToString();
                            DataTable dtInwardDetail = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_RATE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE and IWD_IWM_CODE='" + Inward_Code + "' and INWARD_MASTER.ES_DELETE='0'");
                            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
                            {
                                CommonClasses.Execute("Update INWARD_DETAIL set IWD_BILL_PASS_FLG=0 where IWD_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' and IWD_IWM_CODE='" + dtInwardDetail.Rows[k]["IWD_IWM_CODE"] + "'");

                            }
                        }

                    }
                }

            }

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
