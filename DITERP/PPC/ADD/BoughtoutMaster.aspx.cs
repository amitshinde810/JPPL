using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_BoughtoutMaster : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    DataRow dr;
    public static string str = "";
    public static int Index = 0;
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        ViewState["mlCode"] = mlCode;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        ViewState["str"] = str;
                        BlankGrid();
                        LoadItem(); LoadBoughout();  // Method call
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Boughtout Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Boughtout Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region BlankGrid
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgBoughtout.Enabled = false;
            //dtFilter.Columns.Add(new System.Data.DataColumn("BOD_CODE", typeof(string)));
            //dtFilter.Columns.Add(new System.Data.DataColumn("GP_CODE", typeof(string)));
            //dtFilter.Columns.Add(new System.Data.DataColumn("GP_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE1", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE_BOUGHT_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("BOD_QTY", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgBoughtout.DataSource = dtFilter;
            dgBoughtout.DataBind();
        }
    }
    #endregion BlankGrid

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadItem(); LoadBoughout();
            dt = CommonClasses.Execute(" SELECT BOM.BOM_CODE,G.GP_CODE,G.GP_NAME,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME FROM BOUGHTOUT_MASTER BOM inner join GROUP_MASTER G ON BOM.BOM_GP_CODE=G.GP_CODE INNER JOIN ITEM_MASTER I ON BOM.BOM_I_CODE=I.I_CODE WHERE BOM.ES_DELETE=0 AND G.ES_DELETE=0 AND I.ES_DELETE=0 AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and BOM.BOM_CODE='" + ViewState["mlCode"] + "'");
            dt = CommonClasses.Execute("SELECT BOM.BOM_CODE, I.I_CODE, I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM BOUGHTOUT_MASTER AS BOM INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE WHERE (BOM.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND (BOM.BOM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "') AND (BOM.BOM_CODE = '" + ViewState["mlCode"] + "')");
            if (dt.Rows.Count > 0)
            {
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                //DataTable dtDetail = CommonClasses.Execute("SELECT BOM.BOM_CODE,BOD.BOD_CODE,BOD.BOD_I_BOUGHT_CODE,ISNULL(BOD.BOD_QTY,0) AS BOD_QTY,G.GP_CODE,G.GP_NAME,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME ,(SELECT I_CODE FROM BOUGHTOUT_DETAIL BOD1 INNER JOIN ITEM_MASTER I1 ON BOD1.BOD_I_BOUGHT_CODE=I1.I_CODE WHERE BOD1.BOD_CODE=BOD.BOD_CODE AND BOD.BOD_I_BOUGHT_CODE=BOD1.BOD_I_BOUGHT_CODE AND BOD1.ES_DELETE=0 AND I1.ES_DELETE=0 and I1.I_CM_COMP_ID=1) AS I_CODE1,(SELECT I_NAME FROM BOUGHTOUT_DETAIL BOD1 INNER JOIN ITEM_MASTER I1 ON BOD1.BOD_I_BOUGHT_CODE=I1.I_CODE WHERE BOD1.BOD_CODE=BOD.BOD_CODE AND BOD.BOD_I_BOUGHT_CODE=BOD1.BOD_I_BOUGHT_CODE AND BOD1.ES_DELETE=0 AND I1.ES_DELETE=0 and I1.I_CM_COMP_ID=1) AS I_CODE_BOUGHT_NAME FROM BOUGHTOUT_MASTER BOM inner join BOUGHTOUT_DETAIL BOD on BOM.BOM_CODE=BOD.BOD_BOM_CODE INNER JOIN GROUP_MASTER G ON BOM.BOM_GP_CODE=G.GP_CODE INNER JOIN ITEM_MASTER I ON BOM.BOM_I_CODE=I.I_CODE WHERE BOM.ES_DELETE=0 AND BOD.ES_DELETE=0 AND G.ES_DELETE=0 AND I.ES_DELETE=0 AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and BOM.BOM_CODE='" + ViewState["mlCode"] + "'");
                DataTable dtDetail = CommonClasses.Execute("SELECT BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, I.I_CODE, I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, (SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE WHERE (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and BOM.BOM_CODE='" + ViewState["mlCode"] + "'");
                if (dtDetail.Rows.Count > 0)
                {
                    dgBoughtout.Enabled = true;
                    ViewState["dt2"] = dtDetail;
                    dgBoughtout.DataSource = (DataTable)ViewState["dt2"];
                    dgBoughtout.DataBind();
                }
                if (str == "VIEW")
                {
                    ddlItemName.Enabled = false; txtQty.Enabled = false; ddlBoughtOut.Enabled = false;
                    btnSubmit.Visible = false; BtnInsert.Enabled = false; dgBoughtout.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("BOUGHTOUT_MASTER", "MODIFY", "BOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Boughtout Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region BtnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        PanelMsg.Visible = false;
        try
        {
            #region Validation

            if (Convert.ToInt32(ddlItemName.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            if (Convert.ToInt32(ddlBoughtOut.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select BoughtOut";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlBoughtOut.Focus();
                return;
            }
            if (txtQty.Text.Trim() == "" || Convert.ToDouble(txtQty.Text.Trim()) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtQty.Focus();
                return;
            }
            #endregion Validation

            #region Validate Data to insert Duplicate in Grid
            if (dgBoughtout.Rows.Count > 0)
            {
                for (int i = 0; i < dgBoughtout.Rows.Count; i++)
                {
                    string I_CODE = ((Label)(dgBoughtout.Rows[i].FindControl("lblI_CODE"))).Text;
                    //string GP_CODE = ((Label)(dgBoughtout.Rows[i].FindControl("lblGP_CODE"))).Text;
                    string I_CODE1 = ((Label)(dgBoughtout.Rows[i].FindControl("lblI_CODE1"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (I_CODE == ddlItemName.SelectedValue.ToString() && I_CODE1 == ddlBoughtOut.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (I_CODE == ddlItemName.SelectedValue.ToString() && I_CODE1 == ddlBoughtOut.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion Validate Data to insert Duplicate in Grid

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                //((DataTable)ViewState["dt2"]).Columns.Add("BOD_CODE");
                //((DataTable)ViewState["dt2"]).Columns.Add("GP_CODE");
                //((DataTable)ViewState["dt2"]).Columns.Add("GP_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("ICODE_INAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODE1");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODE_BOUGHT_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("BOD_QTY");
            }
            #endregion

            #region Add Value to dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["I_CODE"] = ddlItemName.SelectedValue;
            dr["ICODE_INAME"] = ddlItemName.SelectedItem.Text;
            dr["I_CODE1"] = ddlBoughtOut.SelectedValue;
            dr["I_CODE_BOUGHT_NAME"] = ddlBoughtOut.SelectedItem.Text;
            dr["BOD_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtQty.Text)));
            #endregion

            #region Insert or Modify Data in Grid
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtQty.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtQty.Text = "";
            }
            #endregion

            #region Binding dt to Grid
            dgBoughtout.Enabled = true;
            dgBoughtout.Visible = true;
            dgBoughtout.DataSource = ((DataTable)ViewState["dt2"]);
            dgBoughtout.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion

            dgBoughtout.SelectedIndex = -1;
            txtQty.Text = "0";
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion BtnInsert_Click

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation

        if (ddlItemName.SelectedIndex == -1 || ddlItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        if (ddlBoughtOut.SelectedIndex == -1 || ddlBoughtOut.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select BoughtOut Name", CommonClasses.MSG_Warning);
            ddlBoughtOut.Focus();
            return;
        }
        #endregion

        SaveRec();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT BOM.BOM_CODE FROM BOUGHTOUT_MASTER BOM inner join BOUGHTOUT_DETAIL BOD on BOM.BOM_CODE=BOD.BOD_BOM_CODE WHERE BOM.ES_DELETE=0 AND BOD.ES_DELETE=0 AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND BOM.BOM_I_CODE='" + ddlItemName.SelectedValue + "' AND BOD.BOD_I_BOUGHT_CODE='" + ddlBoughtOut.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO BOUGHTOUT_MASTER(BOM_COMP_ID,BOM_I_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlItemName.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(BOM_CODE) from BOUGHTOUT_MASTER");
                        for (int i = 0; i < dgBoughtout.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO BOUGHTOUT_DETAIL(BOD_COMP_ID,BOD_BOM_CODE,BOD_I_BOUGHT_CODE,BOD_QTY)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToInt32(Code) + "','" + ((Label)dgBoughtout.Rows[i].FindControl("lblI_CODE1")).Text + "','" + ((Label)dgBoughtout.Rows[i].FindControl("lblBOD_QTY")).Text + "')");
                        }
                        CommonClasses.WriteLog("Boughtout Master", "Save", "Boughtout Master", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewBoughtoutMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlBoughtOut.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT BOM.BOM_CODE FROM BOUGHTOUT_MASTER BOM inner join BOUGHTOUT_DETAIL BOD on BOM.BOM_CODE=BOD.BOD_BOM_CODE WHERE BOM.ES_DELETE=0 AND BOD.ES_DELETE=0 AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND BOM.BOM_I_CODE='" + ddlItemName.SelectedValue + "' AND BOD.BOD_I_BOUGHT_CODE='" + ddlBoughtOut.SelectedValue + "' and BOM.BOM_CODE!='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE BOUGHTOUT_MASTER SET BOM_I_CODE='" + ddlItemName.SelectedValue + "'  WHERE BOM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.Execute1("delete from BOUGHTOUT_DETAIL where BOD_BOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        for (int i = 0; i < dgBoughtout.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO BOUGHTOUT_DETAIL(BOD_COMP_ID,BOD_BOM_CODE,BOD_I_BOUGHT_CODE,BOD_QTY)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgBoughtout.Rows[i].FindControl("lblI_CODE1")).Text + "','" + ((Label)dgBoughtout.Rows[i].FindControl("lblBOD_QTY")).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("BOUGHTOUT_MASTER", "MODIFY", "BOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Boughtout Master", "Update", "Boughtout Master", ddlBoughtOut.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewBoughtoutMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlBoughtOut.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlBoughtOut.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Boughtout Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region dgBoughtout_RowCommand
    protected void dgBoughtout_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["Index"] = Index;
            GridViewRow row = dgBoughtout.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
                {

                }
                dgBoughtout.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgBoughtout.DataSource = ((DataTable)ViewState["dt2"]);
                dgBoughtout.DataBind();
                if (dgBoughtout.Rows.Count == 0)
                {
                    BlankGrid();
                    dgBoughtout.Enabled = false;
                }
                else
                    dgBoughtout.Enabled = true;
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                LoadItem(); LoadBoughout(); // Method Call

                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblI_CODE"))).Text;
                ddlBoughtOut.SelectedValue = ((Label)(row.FindControl("lblI_CODE1"))).Text;
                txtQty.Text = ((Label)(row.FindControl("lblBOD_QTY"))).Text;
                // All delete Enable False within Gridview
                foreach (GridViewRow gvr in dgBoughtout.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "dgBoughtout_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgBoughtout_Deleting
    protected void dgBoughtout_Deleting(object sender, GridViewDeleteEventArgs e)
    {
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
            CommonClasses.SendError("BOUGHTOUT_MASTER", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Boughtout Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("BOUGHTOUT_MASTER", "MODIFY", "BOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewBoughtoutMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Boughtout Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlBoughtOut.SelectedIndex == -1)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Boughtout Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlBoughtOut_SelectedIndexChanged
    protected void ddlBoughtOut_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlBoughtOut_SelectedIndexChanged

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO + ' - ' + ITEM_MASTER.I_NAME AS ICODE_INAME FROM ITEM_MASTER INNER JOIN PRODUCT_MASTER ON ITEM_MASTER.I_CODE = PRODUCT_MASTER.PROD_I_CODE WHERE (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE='-2147483648') AND I_CM_COMP_ID='" + Session["CompanyId"] + "' and PRODUCT_MASTER.ES_DELETE=0 order by I_CODENO +' - '+ I_NAME");
        ddlItemName.DataSource = dtFinishItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItem

    #region LoadBoughout
    protected void LoadBoughout()
    {
        // Check Hardcoded Boughtout Item -2147483647
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE in('-2147483647') AND I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_NAME");
        ddlBoughtOut.DataSource = dtProcess;
        ddlBoughtOut.DataTextField = "I_NAME";
        ddlBoughtOut.DataValueField = "I_CODE";
        ddlBoughtOut.DataBind();
        ddlBoughtOut.Items.Insert(0, new ListItem("Select Boughout Name", "0"));
    }
    #endregion LoadBoughout
}
