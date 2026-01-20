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
using System.Net;
/// <summary>
/// Summary description for UserMaster_BL
/// </summary>
public class UserRights_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"
    int _UR_UM_CODE;
    int _UR_SM_CODE;
    string _UR_RIGHTS;
    #endregion

    #region "Properties"

    public int UR_UM_CODE
    {
        get { return _UR_UM_CODE; }
        set { _UR_UM_CODE = value; }
    }

    public int UR_SM_CODE
    {
        get { return _UR_SM_CODE; }
        set { _UR_SM_CODE = value; }
    }

    public string UR_RIGHTS
    {
        get { return _UR_RIGHTS; }
        set { _UR_RIGHTS = value; }
    }
    public string Msg = "";
    #endregion

    #region "Constructor"
    public UserRights_BL()
    {
    }
    #endregion "Constructor"

    #region Parameterise Constructor
    public UserRights_BL(int Id)
    {
        _UR_UM_CODE = Id;
    }
    #endregion

    #region UserDefineMethods

    #region FillCombo
    public DataTable GetDepartment(string Query)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", Query);

            dt = DL_DBAccess.SelectData("SP_EmpCountryCombo", par);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "GetDepartment", ex.Message);
        }
        return dt;
    }
    #endregion FillCombo

    #region Save
    public bool Save(GridView xgrid)
    {
        bool result = false;

        try
        {
            //if (CheckExistSaveName())
            //{
            DL_DBAccess = new DatabaseAccessLayer();
            for (int i = 0; i < xgrid.Rows.Count; i++)
            {
                string str = "";
                //if (((CheckBox)(xgrid.Rows[i].FindControl("chkMenuDg"))).Checked == true)
                //{
                //    str = str + "1";
                //}
                //else
                //{
                //    str = str + "0";
                //}
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkMenuDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkViewDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkUpdateDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkAddDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkDeleteDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkPrintDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkBackDateDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                UR_RIGHTS = str;
                UR_SM_CODE = Convert.ToInt32(((Label)(xgrid.Rows[i].FindControl("SM_CODE"))).Text);
                UR_UM_CODE = Convert.ToInt32(((Label)(xgrid.Rows[i].FindControl("USR_CODE"))).Text);
                SqlParameter[] par = new SqlParameter[3];
                par[0] = new SqlParameter("@UR_UM_CODE", UR_UM_CODE);
                par[1] = new SqlParameter("@UR_SM_CODE", UR_SM_CODE);
                par[2] = new SqlParameter("@UR_RIGHTS", UR_RIGHTS);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_InsertUserRights", par);
                if (result == true)
                {
                    Msg = "Data Saved Successfully";
                }
            }



            //}
            //else
            //{
            //    Msg = "User Name Already Exist";
            //}
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "Save", ex.Message);
        }
        return result;
    }
    #endregion Save

    #region Update
    public bool Update(GridView xgrid)
    {
        bool result = false;
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            for (int i = 0; i < xgrid.Rows.Count; i++)
            {
                string str = "";
                //if (((CheckBox)(xgrid.Rows[i].FindControl("chkMenuDg"))).Checked == true)
                //{
                //    str = str + "1";
                //}
                //else
                //{
                //    str = str + "0";
                //}
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkMenuDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkViewDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkUpdateDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkAddDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkDeleteDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkPrintDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkBackDateDg"))).Checked == true)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                UR_RIGHTS = str;
                UR_SM_CODE = Convert.ToInt32(((Label)(xgrid.Rows[i].FindControl("SM_CODE"))).Text);
                UR_UM_CODE = Convert.ToInt32(((Label)(xgrid.Rows[i].FindControl("USR_CODE"))).Text);
                SqlParameter[] par = new SqlParameter[3];
                par[0] = new SqlParameter("@UR_UM_CODE", UR_UM_CODE);
                par[1] = new SqlParameter("@UR_SM_CODE", UR_SM_CODE);
                par[2] = new SqlParameter("@UR_RIGHTS", UR_RIGHTS);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_UpdateUserRights", par);
                if (result == true)
                {
                    Msg = "Data Updated Successfully";
                }
            }


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "Update", ex.Message);
        }
        return result;
    }
    #endregion

    #region GetUserRightsDetails
    public DataTable GetUserRightsDetails()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {

            dt = DL_DBAccess.SelectData("SP_GetUserRightsDetails", null);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "GetUserRightsDetails", ex.Message);
        }

        if (dt != null && dt.Rows.Count > 0)
            return dt;
        else
            return null;

    }
    #endregion

    #region GetInfo
    public void GetInfo(GridView XGrid)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@UR_UM_CODE", UR_UM_CODE);
            dt = DL_DBAccess.SelectData("SP_GetUserRightInfo", par);

            UR_UM_CODE = Convert.ToInt32(dt.Rows[0]["UR_UM_CODE"]);
            UR_SM_CODE = Convert.ToInt32(dt.Rows[0]["UR_SM_CODE"]);
            UR_RIGHTS = dt.Rows[0]["UR_RIGHTS"].ToString();
            Griddata(XGrid, dt);

        }


        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "GetInfo", ex.Message);
        }

    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@UR_UM_CODE", UR_UM_CODE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_DeleteUserRights", par);
            
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "Delete", ex.Message);
        }
        
        return result;
    }
    #endregion

    #endregion Methods

    #region Griddata
    public void Griddata(GridView XGrid, DataTable dt)
    {
        try
        {

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("USR_CODE");
            dt1.Columns.Add("SM_CODE");
            dt1.Columns.Add("Name");
            //dt1.Columns.Add("Menu");
            dt1.Columns.Add("Menu");
            dt1.Columns.Add("View");
            dt1.Columns.Add("Update");
            dt1.Columns.Add("Add");
            dt1.Columns.Add("Delete");
            dt1.Columns.Add("Print");
            dt1.Columns.Add("Back Date");


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                char[] URight = (dt.Rows[i]["UR_RIGHTS"].ToString()).ToCharArray();
                DataRow dr = dt1.NewRow();
                dr["USR_CODE"] = dt.Rows[i]["UR_UM_CODE"].ToString();
                dr["SM_CODE"] = dt.Rows[i]["UR_SM_CODE"].ToString();
                dr["Name"] = dt.Rows[i]["SM_NAME"].ToString();

                for (int m = 0, n = 3; m < URight.Length; m++, n++)
                {
                    if (URight[m] == '1')
                    {
                        dr[n] = true;
                    }
                    else
                    {
                        dr[n] = false;
                    }
                }
                dt1.Rows.Add(dr);
            }
            XGrid.Visible = true;
            XGrid.DataSource = dt1;
            XGrid.DataBind();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string val = ((Label)(XGrid.Rows[i].FindControl("USR_CODE"))).Text;
                if (val == dt1.Rows[i]["USR_CODE"].ToString())
                {
                   // ((CheckBox)(XGrid.Rows[i].FindControl("chkMenuDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Menu"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkMenu"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Menu"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkViewDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["View"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkUpdateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Update"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkAddDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Add"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkDeleteDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Delete"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkPrintDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Print"]);
                    ((CheckBox)(XGrid.Rows[i].FindControl("chkBackDateDg"))).Checked = Convert.ToBoolean(dt1.Rows[i]["Back Date"]);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Rights class", "Griddata", ex.Message);
        }
    #endregion

    }
}






