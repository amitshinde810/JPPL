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
/// Summary description for PurchaseRequisition_BL
/// </summary>
public class PurchaseRequisition_BL
{
    #region Counstructor
    public PurchaseRequisition_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public PurchaseRequisition_BL(int Id)
    {
        PRM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    private int _PRM_CODE;
    private int _PRM_CM_COMP_CODE;
    private int _PRM_TYPE;
    private string _PRM_NO;
    private DateTime _PRM_DATE;
    private int _PRM_MR_CODE;
    private int _PRM_I_CODE;
    private string _PRM_DEPARTMENT;
    private int _PRM_UM_CODE;
    public string message = "";
    public int PK_CODE;
    public string Msg = "";
    #endregion

    #region Public Properties
    public int PRM_CODE
    {
        get { return _PRM_CODE; }
        set { _PRM_CODE = value; }
    }
    public int PRM_CM_COMP_CODE
    {
        get { return _PRM_CM_COMP_CODE; }
        set { _PRM_CM_COMP_CODE = value; }
    }
    public int PRM_TYPE
    {
        get { return _PRM_TYPE; }
        set { _PRM_TYPE = value; }
    }
    public string PRM_NO
    {
        get { return _PRM_NO; }
        set { _PRM_NO = value; }
    }
    public DateTime PRM_DATE
    {
        get { return _PRM_DATE; }
        set { _PRM_DATE = value; }
    }
    public int PRM_MR_CODE
    {
        get { return _PRM_MR_CODE; }
        set { _PRM_MR_CODE = value; }
    }
    public int PRM_I_CODE
    {
        get { return _PRM_I_CODE; }
        set { _PRM_I_CODE = value; }
    }
    public string PRM_DEPARTMENT
    {
        get { return _PRM_DEPARTMENT; }
        set { _PRM_DEPARTMENT = value; }
    }
    public int PRM_UM_CODE
    {
        get { return _PRM_UM_CODE; }
        set { _PRM_UM_CODE = value; }
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
            //Inserting Master Part
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
				new SqlParameter("@PRM_CODE",PRM_CODE),
				new SqlParameter("@PRM_CM_COMP_CODE",PRM_CM_COMP_CODE),
				new SqlParameter("@PRM_TYPE",PRM_TYPE),
                new SqlParameter("@PRM_NO",PRM_NO),
                new SqlParameter("@PRM_DATE",PRM_DATE),
                new SqlParameter("@PRM_MR_CODE",PRM_MR_CODE),
				new SqlParameter("@PRM_I_CODE",PRM_I_CODE),
                new SqlParameter("@PRM_DEPARTMENT",PRM_DEPARTMENT),
				new SqlParameter("@PRM_UM_CODE",PRM_UM_CODE)
				
			};
            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PURCHASE_REQUISITION_MASTER", Params, out message, out PK_CODE);
            int PRD_PRM_CODE = PK_CODE;
            if (result == true)
                {
                    result = CommonClasses.Execute1("DELETE FROM PURCHASE_REQUISION_DETAIL where PRD_PRM_CODE='" + PRD_PRM_CODE + "'");
                    //Inserting Detail Part
                    for (int i = 0; i < XGrid.Rows.Count; i++)
                    {
                        double PRD_ORD_QTY = Convert.ToDouble(((TextBox)XGrid.Rows[i].FindControl("txtPRD_ORD_QTY")).Text);
                        if (PRD_ORD_QTY > 0)
                        {
                            string PRD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblPRD_I_CODE")).Text;
                            double PRD_REQ_QTY =Math.Round(Convert.ToDouble(((Label)XGrid.Rows[i].FindControl("lblPRD_REQ_QTY")).Text),3);
                            double PRD_OLD_QTY = Math.Round(Convert.ToDouble(((Label)XGrid.Rows[i].FindControl("lblPRD_OLD_QTY")).Text),3);
                           
                            string PRD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblPRD_REMARK")).Text;

                            SqlParameter[] Params1 =               
                    	  {                                
				                new SqlParameter("@PRD_PRM_CODE",PRD_PRM_CODE),
				                new SqlParameter("@PRD_I_CODE",PRD_I_CODE),
				                new SqlParameter("@PRD_REQ_QTY",PRD_REQ_QTY),
				                new SqlParameter("@PRD_OLD_QTY",PRD_OLD_QTY),
				                new SqlParameter("@PRD_ORD_QTY",PRD_ORD_QTY),
                                new SqlParameter("@PRD_REMARK",PRD_REMARK),
				                new SqlParameter("@PK_CODE", DBNull.Value)
			                };

                            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PURCHASE_REQUISITION_DETAIL", Params1, out message);
                            // Update Query To Update the flag in INward Master
                            if (result == true && PRM_MR_CODE != 0)
                            {
                                result = CommonClasses.Execute1("UPDATE MATERIAL_REQUISITION_DETAIL set MRD_PURC_REQ_QTY=MRD_PURC_REQ_QTY+" + PRD_ORD_QTY + " where MRD_MR_CODE='" + PRM_MR_CODE + "' and MRD_I_CODE='" + PRD_I_CODE + "'");
                            }
                        }
                    }

                }            
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "Save/Update", Ex.Message);

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
            DataTable dtReqDet = CommonClasses.Execute("SELECT PRD_PRM_CODE,PRD_ORD_QTY,PRD_I_CODE,PRM_MR_CODE FROM PURCHASE_REQUISION_DETAIL,PRUCHASE_REQUISITION_MASTER WHERE PRD_PRM_CODE=PRM_CODE and PRD_PRM_CODE='" + PRM_CODE + "'");
            //Update Master Table Flag
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", PRM_CODE);
            par[1] = new SqlParameter("@PK_Field", "PRM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "PRUCHASE_REQUISITION_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);
            if (result == true)
            {
                for(int i=0;i<dtReqDet.Rows.Count;i++)
                {
                    result = CommonClasses.Execute1("UPDATE MATERIAL_REQUISITION_DETAIL set MRD_PURC_REQ_QTY=MRD_PURC_REQ_QTY-" + dtReqDet.Rows[i]["PRD_ORD_QTY"] + " where MRD_MR_CODE='" + dtReqDet.Rows[i]["PRM_MR_CODE"] + "' and MRD_I_CODE='" + dtReqDet.Rows[i]["PRD_I_CODE"] + "'");
                }
            }
            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion

}
