using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewStockAdjustment : System.Web.UI.Page
{
    static string right = "";
    DataTable dtFilter = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='60'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadStockAdjustment();
            }
        }
    }

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("SELECT TOP 500 SAM_CODE,SAM_DOC_NO,convert(varchar,SAM_DATE,106)as SAM_DATE FROM STOCK_ADJUSTMENT_MASTER where ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " and (upper(SAM_DOC_NO) like upper('%" + str + "%') OR convert(varchar,SAM_DATE,106) like upper('%" + str + "%')) order by SAM_DOC_NO DESC");

            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 SAM_CODE,SAM_DOC_NO,convert(varchar,SAM_DATE,106)as SAM_DATE FROM STOCK_ADJUSTMENT_MASTER where ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " order by SAM_DOC_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgStockAdjustment.DataSource = dtfilter;
                dgStockAdjustment.DataBind();
                dgStockAdjustment.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAM_DOC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAM_DATE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgStockAdjustment.DataSource = dtFilter;
                    dgStockAdjustment.DataBind();
                    dgStockAdjustment.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadStatus", ex.Message);
        }
    }

    #endregion

    private void LoadStockAdjustment()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT TOP 500 SAM_CODE,SAM_DOC_NO,convert(varchar,SAM_DATE,106)as SAM_DATE FROM STOCK_ADJUSTMENT_MASTER where ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND SAM_TYPE=0 ORDER BY SAM_CODE DESC");
            if (dt.Rows.Count == 0)
            {
                if (dgStockAdjustment.Rows.Count == 0)
                {
                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SAM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SAM_DOC_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SAM_DATE", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgStockAdjustment.DataSource = dtFilter;
                        dgStockAdjustment.DataBind();
                        dgStockAdjustment.Enabled = false;
                    }
                }
            }
            else
            {
                dgStockAdjustment.Enabled = true;
                dgStockAdjustment.DataSource = dt;
                dgStockAdjustment.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadSupplierPO", Ex.Message);
        }
    }
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Productionn", "txtString_TextChanged", Ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Stock Adjustment", "btnCancel_Click", ex.Message.ToString());
        }
    }
    protected void dgStockAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        string sam_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/StockAdjustment.aspx?c_name=" + type + "&sam_code=" + sam_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
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
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string um_code = e.CommandArgument.ToString();
                        if (um_code == "-2147483648")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Modify  Stock Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        string type = "MODIFY";
                        Response.Redirect("~/Transactions/ADD/StockAdjustment.aspx?c_name=" + type + "&u_code=" + um_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "dgDetailSupplierPO_RowCommand", Ex.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from STOCK_ADJUSTMENT_MASTER where SAM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment- View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion

    protected void dgStockAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgStockAdjustment.Rows[e.RowIndex].FindControl("lblCode"))).Text))
                {
                    string um_code = ((Label)(dgStockAdjustment.Rows[e.RowIndex].FindControl("lblCode"))).Text;
                    string um_DOCNO = ((Label)(dgStockAdjustment.Rows[e.RowIndex].FindControl("lblDoc_No"))).Text;

                    DataTable DtOldDetails = CommonClasses.Execute(" select SAD_I_CODE,SAD_ADJUSTMENT_QTY,I_CODENO,I_CURRENT_BAL,SAM_FROM_STORE from STOCK_ADJUSTMENT_DETAIL,STOCK_ADJUSTMENT_MASTER, ITEM_MASTER WHERE SAD_I_CODE= I_CODE  AND    SAM_CODE=SAD_SAM_CODE AND  SAD_SAM_CODE='" + um_code + "'");

                    for (int i = 0; i < DtOldDetails.Rows.Count; i++)
                    {
                        if ((Convert.ToDouble(DtOldDetails.Rows[i]["I_CURRENT_BAL"].ToString()) - Convert.ToDouble(DtOldDetails.Rows[i]["SAD_ADJUSTMENT_QTY"].ToString())) < 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Delete " + DtOldDetails.Rows[i]["I_CODENO"].ToString() + "  Stock Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    bool flag = CommonClasses.Execute1("UPDATE STOCK_ADJUSTMENT_MASTER SET ES_DELETE = 1 WHERE SAM_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (um_code == "-2147483648")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete  Stock Used In Other Transaction ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (flag == true)
                    {
                        //---- Reseting Item Master Stock
                        for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                        {
                            if (DtOldDetails.Rows[n]["SAM_FROM_STORE"].ToString() == "-2147483642")
                            {
                                if (Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"]) > 0)
                                {
                                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=isnull(I_DISPATCH_BAL,0)-" + DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                                }
                                else
                                {
                                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=isnull(I_DISPATCH_BAL,0)+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"])) + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"]) > 0)
                                {
                                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                                }
                                else
                                {
                                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"])) + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                                }
                            }
                        }

                        flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + um_code + "' and STL_DOC_TYPE='STCADJ'");

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record delete successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        CommonClasses.WriteLog("STOCK Adjustment", "Delete", "STOCK Adjustment", um_DOCNO, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadStockAdjustment();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    protected void dgStockAdjustment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgStockAdjustment.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/StockAdjustment.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment", "btnAddNew_Click", Ex.Message);
        }
    }

    protected void dgStockAdjustment_RowEditing(object sender, EventArgs e)
    {

    }
}
