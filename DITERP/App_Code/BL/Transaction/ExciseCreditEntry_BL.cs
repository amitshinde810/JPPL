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
/// Summary description for ExciseCreditEntry_BL
/// </summary>
public class ExciseCreditEntry_BL
{
    public ExciseCreditEntry_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    #region Parameterise Constructor
    public ExciseCreditEntry_BL(int Id)
    {
        EX_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables

    private int _EX_CODE;
    private int _EX_CM_CODE;
    private string _EX_TYPE;
    private string _EX_IND;
    private string _EX_NO;
    private string _EX_DATE;
    private string _EX_DOC_TYPE;
    private int _EX_P_CODE;
    private string _EX_SUPP_TYPE;
    private string _EX_DOC_NO;
    private string _EX_DOC_DATE;
    private string _EX_BANK_CODE;
    private string _EX_BANK_AMT;
    private string _EX_EX_DUTY;
    private string _EX_EX_CESS;
    private string _EX_EX_HCESS;
    private string _EX_S_DUTY;
    private string _EX_S_CESS;
    private string _EX_S_HCESS;
    private string _EX_ADD_DUTY;
    private string _EX_P_DATE;
    private string _EX_PAY_DETAIL;
    private string _EX_I_CODE;
    private string _EX_TARRIF_NO;
    private string _EX_QTY;
    private string _EX_REMARK;
    private string _EX_INV_NO;
    private string _EX_INV_DATE;
    private string _EX_BASIC_AMT;
    private string _EX_TAX_AMT;
    private string _EX_DISCOUNT_AMT;
    private string _EX_NET_AMT;
    private string _EX_DISC_PER;
    private string _EX_DISC_AMT;
    private string _EX_FREIGHT;
    private string _EX_PACKING_PER;
    private string _EX_PACKING_AMT;
    private string _EX_OCTRO_PER;
    private string _EX_OCTRO_AMT;
    private string _EX_G_AMT;
    private string _EX_GATE_NO;
    private string _EX_GATE_DATE;
    private string _EX_EXCIES_AMT;
    private string _EX_ECESS_AMT;
    private string _EX_HECESS_AMT;
    private string _EX_D_FREIGHT;
    private string _E_EX_TYPE;
    private string _EX_IS_CUSTREJ;
    private string _EX_T_CODE;
    private string _EX_T_PER;
    private string _EX_ADDDUTY;


    private string _EX_INSURANCE_AMT;
    private string _EX_OTHER_AMT;
    private string _EX_TRANSPORT_AMT;



    public string message = "";

    public string Msg = "";
    #endregion



    #region Public Properties

    public int EX_CODE
    {
        get { return _EX_CODE; }
        set { _EX_CODE = value; }
    }

    public int EX_CM_CODE
    {
        get { return _EX_CM_CODE; }
        set { _EX_CM_CODE = value; }
    }

    public string EX_TYPE
    {
        get { return _EX_TYPE; }
        set { _EX_TYPE = value; }
    }

    public string EX_IND
    {
        get { return _EX_IND; }
        set { _EX_IND = value; }
    }

    public string EX_NO
    {
        get { return _EX_NO; }
        set { _EX_NO = value; }
    }

    public string EX_DATE
    {
        get { return _EX_DATE; }
        set { _EX_DATE = value; }
    }
    public string EX_DOC_TYPE
    {
        get { return _EX_DOC_TYPE; }
        set { _EX_DOC_TYPE = value; }
    }
    public int EX_P_CODE
    {
        get { return _EX_P_CODE; }
        set { _EX_P_CODE = value; }
    }
    public string EX_SUPP_TYPE
    {
        get { return _EX_SUPP_TYPE; }
        set { _EX_SUPP_TYPE = value; }
    }


    public string EX_DOC_NO
    {
        get { return _EX_DOC_NO; }
        set { _EX_DOC_NO = value; }
    }
    public string EX_DOC_DATE
    {
        get { return _EX_DOC_DATE; }
        set { _EX_DOC_DATE = value; }
    }
    public string EX_BANK_CODE
    {
        get { return _EX_BANK_CODE; }
        set { _EX_BANK_CODE = value; }
    }
    public string EX_BANK_AMT
    {
        get { return _EX_BANK_AMT; }
        set { _EX_BANK_AMT = value; }
    }
    public string EX_EX_DUTY
    {
        get { return _EX_EX_DUTY; }
        set { _EX_EX_DUTY = value; }
    }
    public string EX_EX_CESS
    {
        get { return _EX_EX_CESS; }
        set { _EX_EX_CESS = value; }
    }
    public string EX_EX_HCESS
    {
        get { return _EX_EX_HCESS; }
        set { _EX_EX_HCESS = value; }
    }
    public string EX_S_DUTY
    {
        get { return _EX_S_DUTY; }
        set { _EX_S_DUTY = value; }
    }
    public string EX_S_CESS
    {
        get { return _EX_S_CESS; }
        set { _EX_S_CESS = value; }
    }
    public string EX_S_HCESS
    {
        get { return _EX_S_HCESS; }
        set { _EX_S_HCESS = value; }
    }
    public string EX_ADD_DUTY
    {
        get { return _EX_ADD_DUTY; }
        set { _EX_ADD_DUTY = value; }
    }
    public string EX_P_DATE
    {
        get { return _EX_P_DATE; }
        set { _EX_P_DATE = value; }
    }
    public string EX_PAY_DETAIL
    {
        get { return _EX_PAY_DETAIL; }
        set { _EX_PAY_DETAIL = value; }
    }
    public string EX_I_CODE
    {
        get { return _EX_I_CODE; }
        set { _EX_I_CODE = value; }
    }
    public string EX_TARRIF_NO
    {
        get { return _EX_TARRIF_NO; }
        set { _EX_TARRIF_NO = value; }
    }
    public string EX_QTY
    {
        get { return _EX_QTY; }
        set { _EX_QTY = value; }
    }
    public string EX_REMARK
    {
        get { return _EX_REMARK; }
        set { _EX_REMARK = value; }
    }
    public string EX_INV_NO
    {
        get { return _EX_INV_NO; }
        set { _EX_INV_NO = value; }
    }

    public string EX_INV_DATE
    {
        get { return _EX_INV_DATE; }
        set { _EX_INV_DATE = value; }
    }

    public string EX_BASIC_AMT
    {
        get { return _EX_BASIC_AMT; }
        set { _EX_BASIC_AMT = value; }
    }
    public string EX_TAX_AMT
    {
        get { return _EX_TAX_AMT; }
        set { _EX_TAX_AMT = value; }
    }
    public string EX_DISCOUNT_AMT
    {
        get { return _EX_DISCOUNT_AMT; }
        set { _EX_DISCOUNT_AMT = value; }
    }
    public string EX_NET_AMT
    {
        get { return _EX_NET_AMT; }
        set { _EX_NET_AMT = value; }
    }
    public string EX_DISC_PER
    {
        get { return _EX_DISC_PER; }
        set { _EX_DISC_PER = value; }
    }
    public string EX_DISC_AMT
    {
        get { return _EX_DISC_AMT; }
        set { _EX_DISC_AMT = value; }
    }
    public string EX_FREIGHT
    {
        get { return _EX_FREIGHT; }
        set { _EX_FREIGHT = value; }
    }
    public string EX_PACKING_PER
    {
        get { return _EX_PACKING_PER; }
        set { _EX_PACKING_PER = value; }
    }
    public string EX_PACKING_AMT
    {
        get { return _EX_PACKING_AMT; }
        set { _EX_PACKING_AMT = value; }
    }
    public string EX_OCTRO_PER
    {
        get { return _EX_OCTRO_PER; }
        set { _EX_OCTRO_PER = value; }
    }
    public string EX_OCTRO_AMT
    {
        get { return _EX_OCTRO_AMT; }
        set { _EX_OCTRO_AMT = value; }
    }
    public string EX_G_AMT
    {
        get { return _EX_G_AMT; }
        set { _EX_G_AMT = value; }
    }
    public string EX_GATE_NO
    {
        get { return _EX_GATE_NO; }
        set { _EX_GATE_NO = value; }
    }
    public string EX_GATE_DATE
    {
        get { return _EX_GATE_DATE; }
        set { _EX_GATE_DATE = value; }
    }

    public string EX_EXCIES_AMT
    {
        get { return _EX_EXCIES_AMT; }
        set { _EX_EXCIES_AMT = value; }
    }

    public string EX_ECESS_AMT
    {
        get { return _EX_ECESS_AMT; }
        set { _EX_ECESS_AMT = value; }
    }
    public string EX_HECESS_AMT
    {
        get { return _EX_HECESS_AMT; }
        set { _EX_HECESS_AMT = value; }
    }
    public string EX_D_FREIGHT
    {
        get { return _EX_D_FREIGHT; }
        set { _EX_D_FREIGHT = value; }
    }
    public string E_EX_TYPE
    {
        get { return _E_EX_TYPE; }
        set { _E_EX_TYPE = value; }
    }
    public string EX_IS_CUSTREJ
    {
        get { return _EX_IS_CUSTREJ; }
        set { _EX_IS_CUSTREJ = value; }
    }
    public string EX_T_CODE
    {
        get { return _EX_T_CODE; }
        set { _EX_T_CODE = value; }
    }
    public string EX_T_PER
    {
        get { return _EX_T_PER; }
        set { _EX_T_PER = value; }
    }
    public string EX_ADDDUTY
    {
        get { return _EX_ADDDUTY; }
        set { _EX_ADDDUTY = value; }
    }



    public string EX_INSURANCE_AMT
    {
        get { return _EX_INSURANCE_AMT; }
        set { _EX_INSURANCE_AMT = value; }
    }


    public string EX_OTHER_AMT
    {
        get { return _EX_OTHER_AMT; }
        set { _EX_OTHER_AMT = value; }
    }

    public string EX_TRANSPORT_AMT
    {
        get { return _EX_TRANSPORT_AMT; }
        set { _EX_TRANSPORT_AMT = value; }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            //dt = CommonClasses.Execute("select distinct INSM_CODE,IWD_CPOM_CODE,SPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,UOM_NAME,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,SPOD_RATE,SPOD_TOTAL_AMT,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,T_NAME,T_SALESTAX,SPOM_FREIGHT_AMT,SPOM_OCTROI_PER,SPOM_LOADING_PER,(SPOD_TOTAL_AMT*SPOD_EXC_PER/100) as ExcAmt,((SPOD_TOTAL_AMT-SPOD_DISC_AMT+ (SPOD_TOTAL_AMT*SPOD_EXC_PER/100)) * T_SALESTAX/100) as TaxAmt from INWARD_DETAIL,INWARD_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAIL,INSPECTION_S_MASTER,ITEM_MASTER,UNIT_MASTER,TAX_MASTER WHERE IWD_I_CODE=I_CODE and IWM_CODE=IWD_IWM_CODE and IWD_CPOM_CODE=SPOM_CODE and INWARD_MASTER.ES_DELETE=0 and IWD_I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and I_UM_CODE=UOM_CODE and IWM_CODE=IWD_IWM_CODE  AND IWD_INSP_FLG=1 AND SPOD_T_CODE=T_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE=INSM_SPOM_CODE and INSM_CM_CODE='" + BPM_CM_CODE + "'");
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
                result = CommonClasses.Execute1("INSERT INTO EXICSE_ENTRY(EX_CM_CODE,EX_TYPE,EX_IND,EX_INV_NO,EX_INV_DATE,EX_NO,EX_DATE,EX_DOC_TYPE,EX_P_CODE,EX_SUPP_TYPE,EX_BASIC_AMT,EX_EXCIES_AMT,EX_ECESS_AMT,EX_HECESS_AMT,EX_TAX_AMT,EX_DISCOUNT_AMT,EX_NET_AMT,EX_FREIGHT,EX_PACKING_AMT,EX_OCTRO_AMT,EX_G_AMT,EX_DOC_NO,EX_DOC_DATE,EX_BANK_AMT,EX_EX_DUTY,EX_EX_CESS,EX_EX_HCESS,EX_ADDDUTY,EX_IS_CUSTREJ,EX_T_PER,EX_GATE_NO,EX_GATE_DATE,EX_S_DUTY,EX_S_CESS,EX_S_HCESS,EX_INSURANCE_AMT,EX_OTHER_AMT,EX_TRANSPORT_AMT) VALUES ('" + EX_CM_CODE + "','" + EX_TYPE + "','" + EX_IND + "','" + EX_INV_NO + "','" + EX_INV_DATE + "','" + EX_NO + "','" + EX_DATE + "','" + EX_DOC_TYPE + "','" + EX_P_CODE + "','" + EX_SUPP_TYPE + "','" + EX_BASIC_AMT + "','" + EX_EXCIES_AMT + "','" + EX_ECESS_AMT + "','" + EX_HECESS_AMT + "','" + EX_TAX_AMT + "','" + EX_DISCOUNT_AMT + "','" + EX_NET_AMT + "','" + EX_FREIGHT + "','" + EX_PACKING_AMT + "','" + EX_OCTRO_AMT + "','" + EX_G_AMT + "','" + EX_DOC_NO + "','" + EX_DOC_DATE + "','" + EX_BANK_AMT + "','" + EX_EX_DUTY + "','" + EX_EX_CESS + "','" + EX_EX_HCESS + "','" + EX_ADDDUTY + "','" + EX_IS_CUSTREJ + "','" + EX_T_PER + "','" + EX_GATE_NO + "','" + EX_GATE_DATE + "','" + EX_S_DUTY + "','" + EX_S_CESS + "','" + EX_S_HCESS + "','" + EX_INSURANCE_AMT + "','" + EX_OTHER_AMT + "','" + EX_TRANSPORT_AMT + "')");
                DataTable dtMax = CommonClasses.Execute("SELECT MAX(EX_CODE) AS EX_CODE FROM EXICSE_ENTRY");
                EX_CODE = Convert.ToInt32(dtMax.Rows[0][0].ToString());
            }
            //result = DL_DBAccess.Insertion_Updation_Delete("EXICSE_ENTRY", Params, out message, out PK_CODE);
            //int BPD_BPM_CODE = PK_CODE;
            if (Process == "UPDATE")
            {
                result = CommonClasses.Execute1("DELETE FROM EXCISE_DETAIL WHERE EXD_EX_CODE='" + EX_CODE + "'");
                if (result)
                {
                    result = CommonClasses.Execute1("UPDATE EXICSE_ENTRY SET EX_CM_CODE='" + EX_CM_CODE + "', EX_TYPE='" + EX_TYPE + "',EX_IND='" + EX_IND + "',EX_INV_NO='" + EX_INV_NO + "',EX_INV_DATE='" + EX_INV_DATE + "',EX_NO='" + EX_NO + "',EX_DATE='" + EX_DATE + "',EX_DOC_TYPE='" + EX_DOC_TYPE + "',EX_P_CODE='" + EX_P_CODE + "',EX_SUPP_TYPE='" + EX_SUPP_TYPE + "',EX_BASIC_AMT='" + EX_BASIC_AMT + "',EX_EXCIES_AMT='" + EX_EXCIES_AMT + "',EX_ECESS_AMT='" + EX_ECESS_AMT + "',EX_HECESS_AMT='" + EX_HECESS_AMT + "',EX_TAX_AMT='" + EX_TAX_AMT + "',EX_DISCOUNT_AMT='" + EX_DISCOUNT_AMT + "',EX_NET_AMT='" + EX_NET_AMT + "',EX_FREIGHT='" + EX_FREIGHT + "',EX_PACKING_AMT='" + EX_PACKING_AMT + "',EX_OCTRO_AMT='" + EX_OCTRO_AMT + "',EX_G_AMT='" + EX_G_AMT + "',EX_DOC_NO='" + EX_DOC_NO + "',EX_DOC_DATE='" + EX_DOC_DATE + "',EX_BANK_AMT='" + EX_BANK_AMT + "',EX_EX_DUTY='" + EX_EX_DUTY + "',EX_EX_CESS='" + EX_EX_CESS + "',EX_EX_HCESS='" + EX_EX_HCESS + "',EX_ADDDUTY='" + EX_ADDDUTY + "',EX_IS_CUSTREJ='" + EX_IS_CUSTREJ + "',EX_T_PER='" + EX_T_PER + "',EX_GATE_NO='" + EX_GATE_NO + "',EX_GATE_DATE='" + EX_GATE_DATE + "',EX_S_DUTY='" + EX_S_DUTY + "', EX_S_CESS='" + EX_S_CESS + "',EX_S_HCESS='" + EX_S_HCESS + "',EX_INSURANCE_AMT='" + EX_INSURANCE_AMT + "',EX_OTHER_AMT='" + EX_OTHER_AMT + "',EX_TRANSPORT_AMT='" + EX_TRANSPORT_AMT + "'  WHERE EX_CODE='" + EX_CODE + "'");

                    
                }
            }
            if (result == true)
            {

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    string EXD_I_CODE;
                    string EXD_IWM_CODE;

                    EXD_IWM_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWM_CODE")).Text;
                    EXD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text;

                    if (EX_IS_CUSTREJ == "1")
                    {
                        CommonClasses.Execute("Update INWARD_DETAIL set IWD_MODVAT_FLG=0 where IWD_I_CODE='" + EXD_I_CODE + "' and IWD_IWM_CODE='" + EXD_IWM_CODE + "'");
                    }
                    else
                    {
                        CommonClasses.Execute("UPDATE CUSTREJECTION_DETAIL set CD_MODVAT_FLG=0 WHERE CD_I_CODE='" + EXD_I_CODE + "' AND CD_CR_CODE='" + EXD_IWM_CODE + "'");
                    }
                    CheckBox chkRow = (((CheckBox)(XGrid.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        // BPD_IWM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWM_CODE")).Text);
                        //int BPD_SPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                        // BPD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                        //float BPD_RECD_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIIWD_REV_QTY")).Text);
                        //float BPD_OK_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CON_OK_QTY")).Text);
                        //float BPD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSPOD_RATE")).Text);
                        //float BPD_AMT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSPOD_TOTAL_AMT")).Text);                      
                        //Inserting Inward Detail Part


                        string EXD_EX_CODE = EX_CODE.ToString();
                        EXD_IWM_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWM_CODE")).Text;
                        string EXD_SPOM_CODE = ((Label)XGrid.Rows[i].FindControl("lblSPOM_CODE")).Text;
                        EXD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text;
                        string EXD_RECD_QTY = ((Label)XGrid.Rows[i].FindControl("lblIIWD_REV_QTY")).Text;
                        string EXD_OK_QTY = ((Label)XGrid.Rows[i].FindControl("lblIWD_CON_OK_QTY")).Text;
                        string EXD_RATE = ((Label)XGrid.Rows[i].FindControl("lblSPOD_RATE")).Text;

                        string EXD_AMT = ((Label)XGrid.Rows[i].FindControl("lblSPOD_TOTAL_AMT")).Text;
                        string EXD_DISC_AMT = ((Label)XGrid.Rows[i].FindControl("lblSPOD_DISC_AMT")).Text;
                        string EXD_EXC_PER = ((Label)XGrid.Rows[i].FindControl("lblSPOD_EXC_PER")).Text;
                        string EXD_EXC_AMT = ((Label)XGrid.Rows[i].FindControl("lblEXC_AMT")).Text;
                        string EXD_EDU_PER = ((Label)XGrid.Rows[i].FindControl("lblSPOD_EDU_CESS_PER")).Text;
                        string EXD_EDU_AMT = ((Label)XGrid.Rows[i].FindControl("lblEDU_AMT")).Text;
                        string EXD_HSEDU_PER = ((Label)XGrid.Rows[i].FindControl("lblSPOD_H_EDU_CESS")).Text;

                        string EXD_HSEDU_AMT = ((Label)XGrid.Rows[i].FindControl("lblHEDU_AMT")).Text;
                        string EXD_T_CODE = ((Label)XGrid.Rows[i].FindControl("lblSPOD_T_CODE")).Text;
                        string EXD_T_PER = ((Label)XGrid.Rows[i].FindControl("lblT_SALESTAX")).Text;
                        string EXD_T_AMT = ((Label)XGrid.Rows[i].FindControl("lblT_AMT")).Text;
                        string EXD_G_RATE = ((Label)XGrid.Rows[i].FindControl("lblGP_RATE")).Text;
                        string EXD_G_AMT = ((Label)XGrid.Rows[i].FindControl("lblGP_AMT")).Text;
                        string EXD_G_CESS = ((Label)XGrid.Rows[i].FindControl("lblECPG")).Text;
                        string EXD_G_SHCESS = ((Label)XGrid.Rows[i].FindControl("lblSECG")).Text;

                        result = CommonClasses.Execute1("INSERT INTO EXCISE_DETAIL(EXD_EX_CODE,EXD_IWM_CODE,EXD_SPOM_CODE,EXD_I_CODE,EXD_RECD_QTY,EXD_OK_QTY,EXD_RATE,EXD_AMT,EXD_DISC_AMT,EXD_EXC_PER,EXD_EXC_AMT,EXD_EDU_PER,EXD_EDU_AMT,EXD_HSEDU_PER,EXD_HSEDU_AMT,EXD_T_CODE,EXD_T_PER,EXD_T_AMT,EXD_G_RATE,EXD_G_AMT,EXD_G_CESS,EXD_G_SHCESS) VALUES ('" + EXD_EX_CODE + "','" + EXD_IWM_CODE + "','" + EXD_SPOM_CODE + "','" + EXD_I_CODE + "','" + EXD_RECD_QTY + "','" + EXD_OK_QTY + "','" + EXD_RATE + "','" + EXD_AMT + "','" + EXD_DISC_AMT + "','" + EXD_EXC_PER + "','" + EXD_EXC_AMT + "','" + EXD_EDU_PER + "','" + EXD_EDU_AMT + "','" + EXD_HSEDU_PER + "','" + EXD_HSEDU_AMT + "','" + EXD_T_CODE + "','" + EXD_T_PER + "','" + EXD_T_AMT + "','" + EXD_G_RATE + "','" + EXD_G_AMT + "','" + EXD_G_CESS + "','" + EXD_G_SHCESS + "')");
                        // Update Query To Update the flag in INward Master
                        if (result == true)
                        {
                            //Update Inward Detail Flag
                            if (EX_IS_CUSTREJ == "1")
                            {
                                CommonClasses.Execute("UPDATE CUSTREJECTION_DETAIL set CD_MODVAT_FLG=1 WHERE CD_I_CODE='" + EXD_I_CODE + "' AND CD_CR_CODE='" + EXD_IWM_CODE + "'");
                            }
                            else
                            {
                                CommonClasses.Execute("Update INWARD_DETAIL set IWD_MODVAT_FLG=1 WHERE IWD_I_CODE='" + EXD_I_CODE + "' AND IWD_IWM_CODE='" + EXD_IWM_CODE + "'");
                            }
                            //CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + IWD_REV_QTY + " where I_CODE='" + IWD_I_CODE + "'");
                        }
                    }
                    else
                    {

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
            //par[0] = new SqlParameter("@PK_CODE", BPM_CODE);
            par[1] = new SqlParameter("@PK_Field", "BPM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "BILL_PASSING_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //DataTable dt = CommonClasses.Execute("select BPD_IWM_CODE from BILL_PASSING_MASTER,BILL_PASSING_DETAIL where BPM_CODE=BPD_BPM_CODE and  BPM_CODE='" + BPM_CODE + "'");
                {
                    //if (dt.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        string Inward_Code = dt.Rows[i]["BPD_IWM_CODE"].ToString();
                    //        DataTable dtInwardDetail = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_RATE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE and IWD_IWM_CODE='" + Inward_Code + "' and INWARD_MASTER.ES_DELETE='0'");
                    //        for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
                    //        {
                    //            CommonClasses.Execute("Update INWARD_DETAIL set IWD_BILL_PASS_FLG=0 where IWD_I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "' and IWD_IWM_CODE='" + dtInwardDetail.Rows[k]["IWD_IWM_CODE"] + "'");

                    //        }
                    //    }

                    //}
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
