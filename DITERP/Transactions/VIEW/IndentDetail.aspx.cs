using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Transactions_VIEW_IndentDetail : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string right2 = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='278'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();


                    CommonClasses.Execute1(" UPDATE INDENT_MASTER set IN_STATUS=2 where INM_CODE IN ( select INM_CODE from INDENT_MASTER where IN_VALIDITY<GETDATE() AND IN_AUTHORIZE!=1 AND IN_AUTHORIZE1!=1 AND IN_STATUS!=3)");
                    LoadInward();
                    DataTable dtRights1 = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='70'");
                    string right1 = dtRights1.Rows.Count == 0 ? "0000000" : dtRights1.Rows[0][0].ToString();
                    if (CommonClasses.ValidRights(int.Parse(right1.Substring(2, 1)), this, "For Authorize"))
                    {
                        //  right2 = "Authorize";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "Page_Load", Ex.Message);
        }
    }

    #region LoadInward
    private void LoadInward()
    {
        try
        {
            DataTable dt = new DataTable();

            //dt = CommonClasses.Execute("select TOP 500 IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,P_NAME from INWARD_MASTER,PARTY_MASTER where INWARD_MASTER.ES_DELETE=0 and P_CODE=IWM_P_CODE  and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  and P_TYPE='2'  AND IWM_TYPE='IWIM'    order by IWM_CODE desc ");
            string condition = "";
            if (right2 != "")
            {
                condition = " AND IN_APPROVE=1 ";
            }
            //dt = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME ,CASE when (IN_STATUS=2) then  'Cancel' when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'   WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN, ISNULL(IN_REASON,'') AS IN_REASON  FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE  " + condition + " AND  INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  order by CASE  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  '4'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=0) then '3'   WHEN IN_APPROVE=1 then '2'    WHEN IN_APPROVE=2 then '5' ELSE '1' END  ");

            //dt = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME ,CASE when (IN_STATUS=2) then  'Cancel' when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'   WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN, ISNULL(IN_REASON,'') AS IN_REASON  FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE  " + condition + " AND  INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  order by CASE  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  '4'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=0) then '3'   WHEN IN_APPROVE=1 then '2'    WHEN IN_APPROVE=2 then '5' ELSE '1' END  ");
            dt = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME ,CASE when (IN_STATUS=2) then  'Cancel' when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize' when (IN_AUTHORIZE=0 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'   WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN, ISNULL(IN_REASON,'') AS IN_REASON  FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE  " + condition + " AND  INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'   order by CASE  when (IN_STATUS=2) then  '6' when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  '4'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=0) then '3'   WHEN IN_APPROVE=1 then '2'    WHEN IN_APPROVE=2 then '5' ELSE '1' END  ");

            if (dt.Rows.Count == 0)
            {
                if (dgDetailPO.Rows.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_TNO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_TYPE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_PRO_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("DM_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_APPROVE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_SUPP_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("UM_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IND_AMT", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_USEDIN", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IN_REASON", typeof(string)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailPO.DataSource = dtFilter;
                        dgDetailPO.DataBind();
                        dgDetailPO.Columns[0].Visible = false;
                    }
                }
            }
            else
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dt;
                dgDetailPO.DataBind();
                dgDetailPO.Columns[0].Visible = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "LoadInward", Ex.Message);
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

        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();


            string condition = "";
            if (right2 != "")
            {
                condition = " AND IN_APPROVE=1 ";
            }

            if (txtString.Text != "")
            {
                //dtfilter = CommonClasses.Execute("SELECT TOP 500 IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME FROM INWARD_MASTER,PARTY_MASTER WHERE INWARD_MASTER.ES_DELETE=0 and P_CODE=IWM_P_CODE and INWARD_MASTER.IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INWARD_MASTER.ES_DELETE='0' and (IWM_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, IWM_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR IWM_CHALLAN_NO like upper('%" + str + "%') OR IWM_CHAL_DATE like upper('%" + str + "%') OR IWM_P_CODE like upper('%" + str + "%'))   and P_TYPE='2' AND IWM_TYPE='IWIM'   order by IWM_CODE desc");
                dtfilter = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME,CASE when (IN_STATUS=2) then  'Cancel'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize'   when (IN_AUTHORIZE=0 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'  WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE  ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE  , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN , ISNULL(IN_REASON,'') AS IN_REASON FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE AND INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE " + condition + " and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and (IN_TYPE like upper('%" + str + "%')  or convert(varchar,IN_DATE,106) like upper('%" + str + "%') or IN_TNO like  upper('%" + str + "%') or IN_PRO_CODE like upper('%" + str + "%')  or IN_REASON like upper('%" + str + "%')  or  DM_NAME like upper('%" + str + "%')  or  IN_SUPP_NAME like upper('%" + str + "%')  or  UM_NAME like upper('%" + str + "%') OR CASE WHEN IN_APPROVE=1 then 'APPROVE'    WHEN IN_APPROVE=2 then 'NOT APPROVE' ELSE 'PENDING' END  like upper('%" + str + "%') )    order by CASE  when (IN_STATUS=2) then  '6'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  '4'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=0) then '3'   WHEN IN_APPROVE=1 then '2'    WHEN IN_APPROVE=2 then '5' ELSE '1' END  ");
            }
            else
            {
                dtfilter = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME,CASE when (IN_STATUS=2) then  'Cancel'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize'   when (IN_AUTHORIZE=0 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'  WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE  ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE  , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN , ISNULL(IN_REASON,'') AS IN_REASON FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE AND INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE " + condition + " and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'   order by CASE   when (IN_STATUS=2) then  '6'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  '4'  when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=0) then '3'   WHEN IN_APPROVE=1 then '2'    WHEN IN_APPROVE=2 then '5' ELSE '1' END  ");
            }
            if (dtfilter.Rows.Count > 0)
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dtfilter;
                dgDetailPO.DataBind();
                dgDetailPO.Columns[0].Visible = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_TNO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_TYPE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_PRO_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("DM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_APPROVE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_SUPP_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IND_AMT", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_USEDIN", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IN_REASON", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                    dgDetailPO.Columns[0].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/Indentdetail.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "you have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text;
                    string cpom_no = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblIN_TNO"))).Text;



                    bool flag = CommonClasses.Execute1("UPDATE INDENT_MASTER SET ES_DELETE = 1 WHERE INM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                    //if (flag == true)
                    //{
                    //    DataTable dtq = CommonClasses.Execute("SELECT IWD_REV_QTY,IWD_I_CODE,IWD_CPOM_CODE FROM INWARD_DETAIL where IWD_IWM_CODE=" + cpom_code + " ");

                    //    for (int i = 0; i < dtq.Rows.Count; i++)
                    //    {
                    //        CommonClasses.Execute("update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtq.Rows[i]["IWD_REV_QTY"] + " where SPOD_I_CODE='" + dtq.Rows[i]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtq.Rows[i]["IWD_CPOM_CODE"] + "'");
                    //        CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtq.Rows[i]["IWD_REV_QTY"] + " where I_CODE='" + dtq.Rows[i]["IWD_I_CODE"] + "'");
                    //    }

                    //    flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + cpom_code + "' and STL_DOC_TYPE='RAWINWARD'");
                    CommonClasses.WriteLog("Indent Detail", "Delete", "Indent Detail", cpom_no, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record deleted successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //}

                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Material Inward", "dgDetailPO_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadInward();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You have no rights to delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgDetailPO_RowEditing
    protected void dgDetailPO_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string cpom_code = ((Label)(dgDetailPO.Rows[e.NewEditIndex].FindControl("lblIWM_CODE"))).Text;
                string type = "MODIFY";

                if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_IWM_CODE", "AND ES_DELETE=0", cpom_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Material Inspection ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                    Response.Redirect("~/Transactions/ADD/MaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgPODetail_RowEditing", Ex.Message);
        }
    }
    #endregion
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    #region dgDetailPO_RowCommand
    protected void dgDetailPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);


            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string cpom_code = e.CommandArgument.ToString();

                        DataTable dtapprove = new DataTable();
                        dtapprove = CommonClasses.Execute("SELECT * FROM INDENT_MASTER WHERE IN_APPROVE=1 AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "'");
                        if (dtapprove.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Approved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                        DataTable dtrej = new DataTable();
                        dtrej = CommonClasses.Execute("SELECT * FROM INDENT_MASTER WHERE IN_APPROVE=2 AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "'");
                        if (dtrej.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Rejected";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
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
            else if (e.CommandName.Equals("Delete"))
            {

            }
            else if (e.CommandName.Equals("Print"))
            {

                string PrintType = "Single";
                string cpom_code = e.CommandArgument.ToString();
                Response.Redirect("~/RoportForms/ADD/IndentDetailPrint.aspx?Title=" + Title + "&cpom_code=" + cpom_code + "&PType=" + PrintType, false);
            }
            else if (e.CommandName.Equals("Approve"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "Approve";
                        string cpom_code = e.CommandArgument.ToString();

                        DataTable dtapprove = new DataTable();
                        dtapprove = CommonClasses.Execute("SELECT * FROM INDENT_MASTER WHERE IN_APPROVE=1 AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "'");
                        if (dtapprove.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Approved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                        DataTable dtrej = new DataTable();
                        dtrej = CommonClasses.Execute("SELECT * FROM INDENT_MASTER WHERE IN_APPROVE=2 AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "'");
                        if (dtrej.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Rejected";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {

                            Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                        }

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "Approve";
                        string cpom_code = e.CommandArgument.ToString();

                        DataTable dtcat = CommonClasses.Execute("select  ISNULL(IM_APPROVAL1,0) AS IM_APPROVAL1,ISNULL(IM_APPROVAL2,0) AS IM_APPROVAL2,ISNULL(IM_APPROVAL3,0) AS IM_APPROVAL3, ISNULL(IM_APPROVDBY1,0) AS IM_APPROVDBY1, ISNULL(IM_APPROVDBY2,0) AS IM_APPROVDBY2, ISNULL(IM_APPROVDBY3,0) AS IM_APPROVDBY3  FROM INDENT_TYPE_MASTER ,INDENT_MASTER where INM_CODE='" + cpom_code + "' AND IN_TYPE=IM_CODE AND (IM_APPROVDBY1='" + Session["UserCode"] + "' OR IM_APPROVDBY2='" + Session["UserCode"] + "' OR IM_APPROVDBY3='" + Session["UserCode"] + "')");

                        if (dtcat.Rows.Count > 0)
                        {
                            if (dtcat.Rows[0]["IM_APPROVAL3"].ToString().ToUpper() == "FALSE" || dtcat.Rows[0]["IM_APPROVAL3"].ToString().ToUpper() == "0")
                            {
                                if (dtcat.Rows[0]["IM_APPROVDBY2"].ToString().ToUpper() == Session["UserCode"].ToString())
                                {
                                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_STATUS=2 WHERE INM_CODE='" + cpom_code + "'");
                                }
                            }
                            else
                            {
                                if (dtcat.Rows[0]["IM_APPROVDBY2"].ToString().ToUpper() == Session["UserCode"].ToString())
                                {

                                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_STATUS=2 WHERE INM_CODE='" + cpom_code + "'");
                                }
                                else if (dtcat.Rows[0]["IM_APPROVDBY3"].ToString().ToUpper() == Session["UserCode"].ToString())
                                {

                                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_STATUS=2 WHERE INM_CODE='" + cpom_code + "'");
                                }
                            }
                        }

                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Authorize this record, it's Not Approved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            else if (e.CommandName.Equals("Authorize"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "Authorize";
                        string cpom_code = e.CommandArgument.ToString();

                        DataTable dtapprove = new DataTable();
                        dtapprove = CommonClasses.Execute("SELECT * FROM INDENT_MASTER WHERE IN_APPROVE=1 AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "'");
                        if (dtapprove.Rows.Count > 0)
                        {
                            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='70'");
                            //string right1 = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                            //if (CommonClasses.ValidRights(int.Parse(right1.Substring(2, 1)), this, "For Authorize"))
                            //{
                            Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                            //}
                            //else
                            //{
                            //    PanelMsg.Visible = true;
                            //    lblmsg.Text = "You have no rights to Authorize";
                            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            //    return;
                            //}
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Authorize this record, it's Not Approved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else if (e.CommandName.Equals("Export"))
            {

                string type = "Export";
                string cpom_code = e.CommandArgument.ToString();

                Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

            }

            else if (e.CommandName.Equals("PExport"))
            {

                string type = "PExport";
                string cpom_code = e.CommandArgument.ToString();

                Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

            }
            else if (e.CommandName.Equals("ViewPDF"))
            {

                string code = "";
                string type = "ViewPDF";
                string cpom_code = e.CommandArgument.ToString();

                //string  textss = ((LinkButton)(dgDetailPO.FindControl("lblIN_USEDIN"))).Text;
                int page = dgDetailPO.PageIndex;
                int Size = dgDetailPO.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgDetailPO.Rows[index];

                //GridView gvRow = (GridView)sender;
                //int selectedRowIndex = ((gvRow.PageIndex) * gvRow.PageSize) + gvRow.SelectedIndex;
                //gvRow = dgBreakdown.Rows[selectedRowIndex];

                code = ((LinkButton)(gvRow.FindControl("lblIN_USEDIN"))).Text;
                DataTable dtpo = new DataTable();
                if (code.Length == 9)
                {
                    dtpo = CommonClasses.Execute("SELECT * FROM SUPP_PO_MASTER where SPOM_PONO='" + code + "' and ES_DELETE=0 and SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  ");
                    string type1 = "VIEW";
                    string PoType = "Domestic PO";
                    string cpom_code1 = dtpo.Rows[0]["SPOM_CODE"].ToString();
                    //Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code1, false);
                    //DataTable dtAutoFlag = CommonClasses.Execute("select SPOM_AUTHR_FLAG from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code1 + "'");
                    if (dtpo.Rows[0]["SPOM_AUTHR_FLAG"].ToString().ToUpper() == "TRUE")
                    {
                        Response.Redirect("~/RoportForms/ADD/SupplierOrderPrint.aspx?cpom_code=" + cpom_code1 + "&AuthoType=&PoType=" + PoType, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Firstly Authorize PO";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else if (code.Length == 12)
                {
                    dtpo = CommonClasses.Execute("SELECT * FROM SUPP_PO_MASTER where SPOM_PONO='" + code + "' and ES_DELETE=0and SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
                    string type1 = "VIEW";
                    string cpom_code1 = dtpo.Rows[0]["SPOM_CODE"].ToString();
                    string PoType = "Domestic PO";
                   // DataTable dtAutoFlag = CommonClasses.Execute("select SPOM_AUTHR_FLAG from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code1 + "'");
                    if (dtpo.Rows[0]["SPOM_AUTHR_FLAG"].ToString().ToUpper() == "TRUE")
                    {
                        Response.Redirect("~/RoportForms/ADD/SubContractorPOPrint.aspx?cpom_code=" + cpom_code1 + "&AuthoType=&PoType=" + PoType, false);

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Firstly Authorize PO";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    //Response.Redirect("~/Transactions/ADD/SubContractPO.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code1, false);

                }
                else if (code.Length == 10)
                {
                    dtpo = CommonClasses.Execute("SELECT * FROM SERVICE_PO_MASTER where SRPOM_PONO='" + code + "' and ES_DELETE=0 and SRPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
                    string type1 = "VIEW";
                    string PoType = "Domestic PO";
                    string cpom_code1 = dtpo.Rows[0]["SRPOM_CODE"].ToString();
                    //  Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code1, false);

                   // DataTable dtAutoFlag = CommonClasses.Execute("select SPOM_AUTHR_FLAG from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code1 + "'");
                    if (dtpo.Rows[0]["SPOM_AUTHR_FLAG"].ToString().ToUpper() == "TRUE")
                    {
                        Response.Redirect("~/RoportForms/ADD/ServiceOrderPrint.aspx?cpom_code=" + cpom_code1 + "&AuthoType=&PoType=" + PoType, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Firstly Authorize PO";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                }
                //Response.Redirect("~/Transactions/ADD/IndentDetail.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

            }
        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError("Indent Detail-Export", "dgDetailPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from INDENT_MASTER where INM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Master-View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward -View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgDetailPO_PageIndexChanging
    protected void dgDetailPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailPO.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
    }



}
