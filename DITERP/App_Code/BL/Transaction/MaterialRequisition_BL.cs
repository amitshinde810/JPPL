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
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Net;

/// <summary>
/// Summary description for MaterialRequisition_BL
/// </summary>
public class MaterialRequisition_BL
{
	#region Counstructor
    public MaterialRequisition_BL()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region Parameterise Constructor
    public MaterialRequisition_BL(int Id)
    {
        MR_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    private int _MR_CODE;
    private int _MR_COMP_CM_CODE;
    private string _MR_DEPT_NAME;
    private string _MR_BATCH_NO;
    private DateTime _MR_DATE;
    private string _MR_FORMULA;
    private int _MR_FORMULA_CODE;
    private int _MR_BATCH_CODE;
    private string _MR_TYPE;
    private int _MR_CPOM_CODE;
    private int _MR_I_CODE;
    private int _MR_UM_CODE;
 
    public string message = "";
    public int PK_CODE;
    public string Msg = "";
    #endregion

    #region Public Properties
    public int MR_CODE
    {
        get { return _MR_CODE; }
        set { _MR_CODE = value; }
    }
    public int MR_COMP_CM_CODE
    {
        get { return _MR_COMP_CM_CODE; }
        set { _MR_COMP_CM_CODE = value; }
    }
    public string MR_BATCH_NO
    {
        get { return _MR_BATCH_NO; }
        set { _MR_BATCH_NO = value; }
    }
    public DateTime MR_DATE
    {
        get { return _MR_DATE; }
        set { _MR_DATE = value; }
    }
    public string MR_FORMULA
    {
        get { return _MR_FORMULA; }
        set { _MR_FORMULA = value; }
    }
    public int MR_FORMULA_CODE
    {
        get { return _MR_FORMULA_CODE; }
        set { _MR_FORMULA_CODE = value; }
    }

    public int MR_BATCH_CODE
    {
        get { return _MR_BATCH_CODE; }
        set { _MR_BATCH_CODE = value; }
    }

    public string MR_TYPE
    {
        get { return _MR_TYPE; }
        set { _MR_TYPE = value; }
    }

    public string MR_DEPT_NAME
    {
        get { return _MR_DEPT_NAME; }
        set { _MR_DEPT_NAME = value; }
    }
    public int MR_I_CODE
    {
        get { return _MR_I_CODE; }
        set { _MR_I_CODE = value; }
    }
    public int MR_CPOM_CODE
    {
        get { return _MR_CPOM_CODE; }
        set { _MR_CPOM_CODE = value; }
    }

    public int MR_UM_CODE
    {
        get { return _MR_UM_CODE; }
        set { _MR_UM_CODE = value; }
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
            //Inserting Materail Requsition
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
				new SqlParameter("@MR_CODE",MR_CODE),
				new SqlParameter("@MR_COMP_CM_CODE",MR_COMP_CM_CODE),
				new SqlParameter("@MR_DEPT_NAME",MR_DEPT_NAME),
				new SqlParameter("@MR_BATCH_NO",MR_BATCH_NO),
				new SqlParameter("@MR_DATE",MR_DATE),
				new SqlParameter("@MR_FORMULA",MR_FORMULA),
				new SqlParameter("@MR_TYPE",MR_TYPE),
				new SqlParameter("@MR_CPOM_CODE",MR_CPOM_CODE),
				new SqlParameter("@MR_I_CODE",MR_I_CODE) ,
                new SqlParameter("@MR_UM_CODE",MR_UM_CODE),
				new SqlParameter("@MR_BATCH_CODE",MR_BATCH_CODE) ,
                new SqlParameter("@MR_FORMULA_CODE",MR_FORMULA_CODE)
			};

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_MATERIAL_REQUISITION_MASTER", Params, out message, out PK_CODE);
            int MRD_MR_CODE = PK_CODE;
            
            if (result == true)
            {
                result = CommonClasses.Execute1("delete from MATERIAL_REQUISITION_DETAIL where MRD_MR_CODE='" + MRD_MR_CODE + "'");
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    string MRD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblBD_I_CODE")).Text;
                    double MRD_REQ_QTY = Math.Round((Convert.ToDouble(((Label)XGrid.Rows[i].FindControl("lblBD_VQTY")).Text)),3);
                    int MRD_PROCESS_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPROCESS_CODE")).Text);
                    int MRD_STEPS_NO = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblSTEP_NO")).Text);
                    double MRD_ADD_IN = 0;
                    if (Convert.ToString(((TextBox)XGrid.Rows[i].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)XGrid.Rows[i].FindControl("txtADD_IN_QTY")).Text) != "")
                    {
                        MRD_ADD_IN = Math.Round((Convert.ToDouble(((TextBox)XGrid.Rows[i].FindControl("txtADD_IN_QTY")).Text)), 3);
                    }
                    string MRD_BT_CODE = ((Label)XGrid.Rows[i].FindControl("lblBT_CODE")).Text;
                        //Inserting Detail Part
                        SqlParameter[] Params1 =               
                    	  { 
                               
				                new SqlParameter("@MRD_MR_CODE",MRD_MR_CODE),
				                new SqlParameter("@MRD_I_CODE",MRD_I_CODE),
				                new SqlParameter("@MRD_REQ_QTY",MRD_REQ_QTY),
				                new SqlParameter("@MRD_PROCESS_CODE",MRD_PROCESS_CODE),
                                new SqlParameter("@MRD_STEPS_NO",MRD_STEPS_NO),
                                new SqlParameter("@MRD_ADD_IN",MRD_ADD_IN),
                                new SqlParameter("@PK_CODE", DBNull.Value),
                                new SqlParameter("@MRD_BT_CODE",MRD_BT_CODE) 
			                };

                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_MATERIAL_REQUISITION_DETAIL", Params1, out message);
                        
                    }
                //result = CommonClasses.Execute1("update BATCH_MASTER set BT_IS_MAT_REQUSITION=1 where BT_CODE='" + MR_BATCH_CODE + "'");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition ", "Save/Update", Ex.Message);

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
            par[0] = new SqlParameter("@PK_CODE", MR_CODE);
            par[1] = new SqlParameter("@PK_Field", "MR_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "MATERIAL_REQUISITION_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);
            result = CommonClasses.Execute1("update BATCH_MASTER set BT_IS_MAT_REQUSITION=0 where BT_CODE=(select MR_BATCH_CODE from MATERIAL_REQUISITION_MASTER where MR_CODE='" + MR_CODE + "')");
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
