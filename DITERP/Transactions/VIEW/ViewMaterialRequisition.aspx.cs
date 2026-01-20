using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Transactions_VIEW_ViewMaterialRequisition : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    MaterialRequisition_BL materialReqisition_BL  = null;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='50'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadMaterial();
                    if (dgMaterialRequsition.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("MR_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("MR_BATCH_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("MR_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("MR_I_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));                            

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgMaterialRequsition.DataSource = dtFilter;
                            dgMaterialRequsition.DataBind();
                        }
                    }


                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Reqisition -View", "Page_Load", Ex.Message);
        }


    }
    #endregion

    #region dgMaterialRequsition_PageIndexChanging
    protected void dgMaterialRequsition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgMaterialRequsition.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Reqisition- View", "dgMaterialRequsition_PageIndexChanging", Ex.Message);
        }

    }
    #endregion

    #region LoadBill
    private void LoadMaterial()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO,BT_NO,MR_I_CODE,convert(varchar,MR_DATE,106) as MR_DATE,I_NAME from MATERIAL_REQUISITION_MASTER,ITEM_MASTER,BATCH_MASTER where MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_BATCH_CODE=BT_CODE and I_CODE=MR_I_CODE  and  MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by MR_BATCH_NO DESC");
            dgMaterialRequsition.DataSource = dt;
            dgMaterialRequsition.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition", "LoadMaterial", Ex.Message);
        }
    }
    #endregion

    #region dgMaterialRequsition_RowCommand
    protected void dgMaterialRequsition_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/MaterialRequisition.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    return;
                }

            }
            #endregion 

            #region Modify
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    if (CommonClasses.CheckUsedInTran("ISSUE_MASTER", "IM_MATERIAL_REQ", "and ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Can't Modify this record because it is used in Issue To Production";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "You can't delete this record, it is used in Work Order", CommonClasses.MSG_Warning);
                        return;
                    }
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {  
                        Response.Redirect("~/Transactions/ADD/MaterialRequisition.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Modify";

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
 
                    return;
                }
            }
            #endregion

            #region Amend
            else if (e.CommandName.Equals("Amend"))
            {                
                string cpom_code = e.CommandArgument.ToString();
                if (!ModifyLog(e.CommandArgument.ToString()))
                {                    
                    string type1 = "AMEND";
                    Session["AMEND_MAT_REQ"] = "AMEND";
                    Response.Redirect("~/Transactions/ADD/MaterialRequisition.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code, false);
                    return;  
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion

            #region Print
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "REQUSITION";
                        string MatReq_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionPrint.aspx?MatReq_Code=" + MatReq_Code + "&print_type=" + type, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            #endregion 

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Reqisition-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgMaterialRequsition_RowDeleting
    protected void dgMaterialRequsition_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (CommonClasses.CheckUsedInTran("ISSUE_MASTER", "IM_MATERIAL_REQ", "and ES_DELETE=0", ((Label)(dgMaterialRequsition.Rows[e.RowIndex].FindControl("lblMR_CODE"))).Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Deleted, It is used in Issue To Production";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You can't delete this record, it is used in Work Order", CommonClasses.MSG_Warning);
                    return;
                }
                else
                {
                    if (!ModifyLog(((Label)(dgMaterialRequsition.Rows[e.RowIndex].FindControl("lblMR_CODE"))).Text))
                    {
                        string cpom_code = ((Label)(dgMaterialRequsition.Rows[e.RowIndex].FindControl("lblMR_CODE"))).Text;
                        materialReqisition_BL = new MaterialRequisition_BL();
                        materialReqisition_BL.MR_CODE = Convert.ToInt32(cpom_code);
                        if (materialReqisition_BL.Delete())
                        {
                            CommonClasses.WriteLog("Material Requisition", "Delete", "Material Requisition", "", Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            LoadMaterial();
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Deleted..";
                        }
                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to delete";

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
 
                //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition-View", "dgMaterialRequsition_RowDeleting", Ex.Message);
        }


    }
    #endregion
    

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from MATERIAL_REQUISITION_MASTER where MR_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
 
                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition-View", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO,BT_NO,MR_I_CODE,convert(varchar,MR_DATE,106) as MR_DATE,I_NAME from MATERIAL_REQUISITION_MASTER,ITEM_MASTER,BATCH_MASTER where MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_BATCH_CODE=BT_CODE and I_CODE=MR_I_CODE  and  MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and (upper(MR_BATCH_NO) like upper('%" + str + "%') or upper(BT_NO) like upper('%" + str + "%') OR convert(varchar,MR_DATE,106) like upper('%" + str + "%') OR upper(I_NAME) like upper('%" + str + "%')) order by MR_BATCH_NO DESC");
            else
                dtfilter = CommonClasses.Execute("select MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO,BT_NO,MR_I_CODE,convert(varchar,MR_DATE,106) as MR_DATE,I_NAME from MATERIAL_REQUISITION_MASTER,ITEM_MASTER,BATCH_MASTER where MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and I_CODE=MR_I_CODE and MR_BATCH_CODE=BT_CODE  and  MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by MR_BATCH_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgMaterialRequsition.DataSource = dtfilter;
                dgMaterialRequsition.DataBind();
            }
            else
            {

                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_BATCH_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));    
                        
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMaterialRequsition.DataSource = dtFilter;
                    dgMaterialRequsition.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material  Requisition", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/MaterialRequisition.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
 
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
}
