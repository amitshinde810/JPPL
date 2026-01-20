using System;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_View_ViewScreenMaster : System.Web.UI.Page
{
    #region " Var "

    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DataTable dt = new DataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadStatus(txtString);
    }

    #region dgCurrancyMaster_RowEditing
    protected void dgCurrancyMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    #endregion

    #region dgItemCategory_RowCommand
    protected void dgCurrancyMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    #region dgCurrancyMaster_PageIndexChanging
    protected void dgCurrancyMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCurrancyMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region dgCurrancyMaster_RowDeleting
    protected void dgCurrancyMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMonthTranInsert_RowDataBound
    protected void dgMonthTranInsert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string a = "";
            try
            {
                a = (string)rowView["SM_ISO_NO"];
            }
            catch
            {
                a = ((string)rowView["SM_ISO_NO"]).ToString();
            }
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME, SM_ISO_NO FROM SCREEN_MASTER where (COUNTRY_NAME like upper('%" + str + "%') or  SM_NAME like upper('%" + str + "%')or  SM_ISO_NO like upper('%" + str + "%') )");
            else
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME, SM_ISO_NO FROM SCREEN_MASTER ");

            if (dtfilter.Rows.Count > 0)
            {
                dgCurrancyMaster.Enabled = true;
                dgCurrancyMaster.DataSource = dtfilter;
                dgCurrancyMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_ISO_NO", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCurrancyMaster.DataSource = dtFilter;
                    dgCurrancyMaster.DataBind();
                    dgCurrancyMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master-View", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master-View", "btnSearch_Click", ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
