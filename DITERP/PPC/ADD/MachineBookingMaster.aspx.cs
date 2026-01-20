using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_MachineBookingMaster : System.Web.UI.Page
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
                        LoadItem(); LoadProcess(); //LoadMachines(); // Method call
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
                        CommonClasses.SendError("Machine Booking Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Booking Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region BlankGrid
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgMachineBooking.Enabled = false;
            //dtFilter.Columns.Add(new System.Data.DataColumn("BOD_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("GP_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("GP_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PMN_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PMN_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("M_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("M_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("MBD_NO_OF_UNITS", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("MBD_SETUP", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgMachineBooking.DataSource = dtFilter;
            dgMachineBooking.DataBind();
        }
    }
    #endregion BlankGrid

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadItem(); LoadProcess();
            //dt = CommonClasses.Execute("select DISTINCT MBM_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,PMN_NAME,PMN_CODE,GP_CODE,GP_NAME from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE INNER JOIN PROCESS_MASTER_NEW P ON MBM.MBM_PMN_CODE=P.PMN_CODE INNER JOIN ITEM_MASTER I ON MBM.MBM_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON MBM.MBM_GP_CODE=G.GP_CODE WHERE MBM.ES_DELETE=0 and P.ES_DELETE=0 and I.ES_DELETE=0 and G.ES_DELETE=0 and MBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MBM_CODE='" + ViewState["mlCode"] + "'");
            dt = CommonClasses.Execute("SELECT DISTINCT MBM.MBM_CODE, I.I_CODE, I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, P.PMN_NAME, P.PMN_CODE FROM MACHINE_BOOKING_MASTER AS MBM INNER JOIN MACHINE_BOOKING_DETAIL AS MBD ON MBM.MBM_CODE = MBD.MBD_MBM_CODE INNER JOIN PROCESS_MASTER_NEW AS P ON MBM.MBM_PMN_CODE = P.PMN_CODE INNER JOIN ITEM_MASTER AS I ON MBM.MBM_I_CODE = I.I_CODE WHERE (MBM.ES_DELETE = 0) AND (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND (MBM.MBM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "') AND (MBM.MBM_CODE = '" + ViewState["mlCode"] + "')");
            if (dt.Rows.Count > 0)
            {
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                ddlProcess.SelectedValue = dt.Rows[0]["PMN_CODE"].ToString();
                //ddlMachine.SelectedValue = dt.Rows[0]["M_CODE"].ToString();
                //DataTable dtDetail = CommonClasses.Execute("select DISTINCT  MBM_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,PMN_NAME,PMN_CODE,MBD_NO_OF_UNITS,GP_CODE,GP_NAME,M_CODE,M_NAME from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE inner join MACHINE_MASTER ON MBD.MBD_M_CODE=M_CODE INNER JOIN PROCESS_MASTER_NEW P ON MBM.MBM_PMN_CODE=P.PMN_CODE INNER JOIN ITEM_MASTER I ON MBM.MBM_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON MBM.MBM_GP_CODE=G.GP_CODE WHERE MBM.ES_DELETE=0 and P.ES_DELETE=0 and I.ES_DELETE=0 and G.ES_DELETE=0 and MBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MBD.MBD_MBM_CODE ='" + ViewState["mlCode"] + "'");
                DataTable dtDetail = CommonClasses.Execute("SELECT DISTINCT MBM.MBM_CODE, I.I_CODE, I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, P.PMN_NAME, P.PMN_CODE, MBD.MBD_NO_OF_UNITS,ISNULL(MBD.MBD_SETUP,0) AS MBD_SETUP, MACHINE_MASTER.M_CODE, MACHINE_MASTER.M_NAME FROM MACHINE_BOOKING_MASTER AS MBM INNER JOIN MACHINE_BOOKING_DETAIL AS MBD ON MBM.MBM_CODE = MBD.MBD_MBM_CODE INNER JOIN MACHINE_MASTER ON MBD.MBD_M_CODE = MACHINE_MASTER.M_CODE INNER JOIN PROCESS_MASTER_NEW AS P ON MBM.MBM_PMN_CODE = P.PMN_CODE INNER JOIN ITEM_MASTER AS I ON MBM.MBM_I_CODE = I.I_CODE WHERE (MBM.ES_DELETE = 0) AND (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND (MBM.MBM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "') AND (MBD.MBD_MBM_CODE = '" + ViewState["mlCode"] + "')");
                if (dtDetail.Rows.Count > 0)
                {
                    dgMachineBooking.Enabled = true;
                    ViewState["dt2"] = dtDetail;
                    dgMachineBooking.DataSource = (DataTable)ViewState["dt2"];
                    dgMachineBooking.DataBind();
                }

                if (str == "VIEW")
                {
                    dgMachineBooking.Enabled = false; ddlItemName.Enabled = false; ddlProcess.Enabled = false;
                    ddlMachine.Enabled = false; txtNumbersOfUnits.Enabled = false; BtnInsert.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("MACHINE_BOOKING_MASTER", "MODIFY", "MBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Booking Master", "ViewRec", ex.Message);
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
                lblmsg.Text = "Select Part Name ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlItemName.Focus();
                return;
            }
            if (Convert.ToInt32(ddlProcess.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Process";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlProcess.Focus();
                return;
            }
            if (Convert.ToInt32(ddlMachine.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Machine";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlProcess.Focus();
                return;
            }
            if (txtNumbersOfUnits.Text.Trim() == "" || Convert.ToDouble(txtNumbersOfUnits.Text.Trim()) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Standard Production Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtNumbersOfUnits.Focus();
                return;
            }
            #endregion Validation

            #region Validate Data to insert Duplicate in Grid
            if (dgMachineBooking.Rows.Count > 0)
            {
                for (int i = 0; i < dgMachineBooking.Rows.Count; i++)
                {
                    string I_CODE = ((Label)(dgMachineBooking.Rows[i].FindControl("lblI_CODE"))).Text;
                    string PMN_CODE = ((Label)(dgMachineBooking.Rows[i].FindControl("lblPMN_CODE"))).Text;
                    string M_CODE = ((Label)(dgMachineBooking.Rows[i].FindControl("lblM_CODE"))).Text;
                    string MBD_SETUP = ((Label)(dgMachineBooking.Rows[i].FindControl("lblMBD_SETUP"))).Text;
                    if (MBD_SETUP.ToString() == "0")
                    {
                        if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                        {
                            if (I_CODE == ddlItemName.SelectedValue.ToString() && PMN_CODE == ddlProcess.SelectedValue.ToString() && M_CODE == ddlMachine.SelectedValue.ToString())
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Record Already Exist For This Item In Table";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                        }
                        else
                        {
                            if (I_CODE == ddlItemName.SelectedValue.ToString() && PMN_CODE == ddlProcess.SelectedValue.ToString() && M_CODE == ddlMachine.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Record Already Exist For This Item In Table";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                        }
                        #region One_Process_for_One_Transaction
                        if (ddlProcess.SelectedValue != "0")
                        {
                            if (PMN_CODE != ddlProcess.SelectedValue)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "User Can not change the process which is already used in following Grid...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                        }
                        #endregion One_Process_for_One_Transaction
                    }
                    else
                    {
                        if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                        {
                            if (I_CODE == ddlItemName.SelectedValue.ToString() && PMN_CODE == ddlProcess.SelectedValue.ToString() && M_CODE == ddlMachine.SelectedValue.ToString() && MBD_SETUP == ddlSetup.SelectedValue.ToString())
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Record Already Exist For This Item In Table";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                        }
                        else
                        {
                            if (I_CODE == ddlItemName.SelectedValue.ToString() && PMN_CODE == ddlProcess.SelectedValue.ToString() && M_CODE == ddlMachine.SelectedValue.ToString() && MBD_SETUP == ddlSetup.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Record Already Exist For This Item In Table";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                        }
                    }
                }
            }
            #endregion Validate Data to insert Duplicate in Grid

            #region Validation_In_db_For_Duplication
            //DataTable dtchkDuplicate = CommonClasses.Execute("Select MBM_CODE FROM MACHINE_BOOKING_MASTER inner join MACHINE_BOOKING_DETAIL on MBM_CODE=MBD_MBM_CODE WHERE MBM_I_CODE='" + ddlItemName.SelectedValue + "' and MBM_PMN_CODE= '" + ddlProcess.SelectedValue + "' AND MBD_M_CODE= '" + ddlMachine.SelectedValue + "' AND MBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MACHINE_BOOKING_MASTER.ES_DELETE='False' and MACHINE_BOOKING_DETAIL.ES_DELETE='False'");
            //if (dtchkDuplicate.Rows.Count == 0)
            //{ }
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Record Already Exist For This Item In Table";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //    return;
            //}
            #endregion

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("GP_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("GP_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("ICODE_INAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("PMN_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("PMN_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("M_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("M_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("MBD_NO_OF_UNITS");
                ((DataTable)ViewState["dt2"]).Columns.Add("MBD_SETUP");
                //MBD_SETUP
            }
            #endregion

            #region Add Value to dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["I_CODE"] = ddlItemName.SelectedValue;
            dr["ICODE_INAME"] = ddlItemName.SelectedItem.Text;
            dr["PMN_CODE"] = ddlProcess.SelectedValue;
            dr["PMN_NAME"] = ddlProcess.SelectedItem.Text;
            dr["M_CODE"] = ddlMachine.SelectedValue;
            dr["M_NAME"] = ddlMachine.SelectedItem.Text;
            dr["MBD_NO_OF_UNITS"] = string.Format("{0:0.00}", (Convert.ToDouble(txtNumbersOfUnits.Text)));
            dr["MBD_SETUP"] = ddlSetup.SelectedValue;
            #endregion

            #region Insert or Modify Data in Grid
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtNumbersOfUnits.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                //txtNumbersOfUnits.Text = "";
            }
            #endregion

            #region Binding dt to Grid
            dgMachineBooking.Enabled = true;
            dgMachineBooking.Visible = true;
            dgMachineBooking.DataSource = ((DataTable)ViewState["dt2"]);
            dgMachineBooking.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion

            dgMachineBooking.SelectedIndex = -1;
            ddlMachine.SelectedIndex = -1;
            //txtNumbersOfUnits.Text = "0"; // For Same Qty under same Process
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
            ddlProcess.Focus();
            return;
        }
        if (ddlProcess.SelectedIndex == -1 || ddlProcess.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Process Name", CommonClasses.MSG_Warning);
            ddlProcess.Focus();
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
                for (int i = 0; i < dgMachineBooking.Rows.Count; i++)
                {
                    dt = CommonClasses.Execute("Select MBM_CODE FROM MACHINE_BOOKING_MASTER inner join MACHINE_BOOKING_DETAIL on MBM_CODE=MBD_MBM_CODE WHERE MBM_I_CODE='" + ddlItemName.SelectedValue + "' and MBM_PMN_CODE= '" + ddlProcess.SelectedValue + "' AND MBD_M_CODE= '" + ((Label)dgMachineBooking.Rows[i].FindControl("lblM_CODE")).Text + "' AND MBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MACHINE_BOOKING_MASTER.ES_DELETE='False' and MACHINE_BOOKING_DETAIL.ES_DELETE='False'");
                    if (dt.Rows.Count == 0) { }
                    else
                    {
                        ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return result;
                    }
                }
                //if (dt.Rows.Count == 0)
                //{
                if (CommonClasses.Execute1("INSERT INTO MACHINE_BOOKING_MASTER(MBM_COMP_ID,MBM_I_CODE,MBM_PMN_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlItemName.SelectedValue + "','" + ddlProcess.SelectedValue + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(MBM_CODE) from MACHINE_BOOKING_MASTER");
                    for (int i = 0; i < dgMachineBooking.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO MACHINE_BOOKING_DETAIL(MBD_COMP_ID,MBD_MBM_CODE,MBD_M_CODE,MBD_NO_OF_UNITS,MBD_SETUP)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToInt32(Code) + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblM_CODE")).Text + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblMBD_NO_OF_UNITS")).Text + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblMBD_SETUP")).Text + "')");
                    }
                    CommonClasses.WriteLog("Machine Booking Master", "Save", "Machine Booking Master", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/PPC/VIEW/ViewMachineBookingMaster.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ddlProcess.Focus();
                }
                //}
                //else
                //{
                //    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //}
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select MBM_CODE FROM MACHINE_BOOKING_MASTER inner join MACHINE_BOOKING_DETAIL on MBM_CODE=MBD_MBM_CODE WHERE MBM_CODE !='" + Convert.ToInt32(ViewState["mlCode"]) + "' AND MBM_I_CODE='" + ddlItemName.SelectedValue + "' and MBM_PMN_CODE= '" + ddlProcess.SelectedValue + "' AND MBD_M_CODE= '" + ddlMachine.SelectedValue + "' AND MBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MACHINE_BOOKING_MASTER.ES_DELETE='False' and MACHINE_BOOKING_DETAIL.ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE MACHINE_BOOKING_DETAIL SET MBM_I_CODE='" + ddlItemName.SelectedValue + "' , MBM_PMN_CODE='" + ddlProcess.SelectedValue + "' WHERE MBM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.Execute1("delete from MACHINE_BOOKING_DETAIL where MBD_MBM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        for (int i = 0; i < dgMachineBooking.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO MACHINE_BOOKING_DETAIL(MBD_COMP_ID,MBD_MBM_CODE,MBD_M_CODE,MBD_NO_OF_UNITS,MBD_SETUP)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblM_CODE")).Text + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblMBD_NO_OF_UNITS")).Text + "','" + ((Label)dgMachineBooking.Rows[i].FindControl("lblMBD_SETUP")).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("MACHINE_BOOKING_MASTER", "MODIFY", "MBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Machine Booking Master", "Update", "Machine Booking Master", ddlProcess.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewMachineBookingMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlProcess.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlProcess.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Booking Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region dgMachineBooking_RowCommand
    protected void dgMachineBooking_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["Index"] = Index;
            GridViewRow row = dgMachineBooking.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
                {

                }
                dgMachineBooking.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgMachineBooking.DataSource = ((DataTable)ViewState["dt2"]);
                dgMachineBooking.DataBind();
                if (dgMachineBooking.Rows.Count == 0)
                {
                    BlankGrid();
                    dgMachineBooking.Enabled = false;
                }
                else
                    dgMachineBooking.Enabled = true;
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                LoadItem(); LoadProcess(); LoadMachines(); // Method Call
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblI_CODE"))).Text;
                ddlProcess.SelectedValue = ((Label)(row.FindControl("lblPMN_CODE"))).Text;
                ddlMachine.SelectedValue = ((Label)(row.FindControl("lblM_CODE"))).Text;
                txtNumbersOfUnits.Text = ((Label)(row.FindControl("lblMBD_NO_OF_UNITS"))).Text;
                ddlSetup.SelectedValue = ((Label)(row.FindControl("lblMBD_SETUP"))).Text;
                if (ddlSetup.SelectedIndex > 0)
                {
                    lblSetup.Visible = true; ddlSetup.Visible = true;
                }
                // All delete Enable False within Gridview
                foreach (GridViewRow gvr in dgMachineBooking.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "dgMachineBooking_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgMachineBooking_Deleting
    protected void dgMachineBooking_Deleting(object sender, GridViewDeleteEventArgs e)
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
            CommonClasses.SendError("MACHINE_BOOKING_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Machine Booking Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("MACHINE_BOOKING_MASTER", "MODIFY", "MBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewMachineBookingMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Booking Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlProcess.SelectedIndex == -1)
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
            CommonClasses.SendError("Machine Booking Master", "CheckValid", Ex.Message);
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
        lblSetup.Visible = false;
        ddlSetup.Visible = false;
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlProcess_SelectedIndexChanged
    protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSetup.Visible = false;
        ddlSetup.Visible = false;
        txtNumbersOfUnits.Text = "0"; //clear Control
        DataTable dtMachines = new DataTable();
        dtMachines = CommonClasses.Execute("SELECT DISTINCT M.M_CODE, M.M_NAME FROM PROCESS_MACHINE_MASTER PMM INNER JOIN MACHINE_MASTER M ON PMM.PMM_M_ID=M.M_CODE WHERE PMM.ES_DELETE=0 AND M.ES_DELETE=0 AND PMM.PMM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PMM.PMM_P_ID='" + ddlProcess.SelectedValue + "' ORDER by M_NAME");
        if (dtMachines.Rows.Count == 0)
        {
            ShowMessage("#Avisos", "Please Enter Machines in Process Machine Master under this process..", CommonClasses.MSG_Warning);
            ddlProcess.Focus();
            ddlMachine.Items.Clear(); // Clear the Machine form Dropdownlist
            return;
        }
        else
        {
            ddlMachine.DataSource = dtMachines;
            ddlMachine.DataTextField = "M_NAME";
            ddlMachine.DataValueField = "M_CODE";
            ddlMachine.DataBind();
            ddlMachine.Items.Insert(0, new ListItem("Select Machine Name", "0"));
        }
        BindQty();
    }
    #endregion ddlProcess_SelectedIndexChanged

    #region BindQty
    public void BindQty()
    {
        DataTable dt = new DataTable();
        if (ddlItemName.SelectedIndex == 0 || ddlItemName.SelectedIndex == -1)
        {
            ShowMessage("#Avisos", "Please select Part Name and then Select process..", CommonClasses.MSG_Warning);
            ddlProcess.Focus();
            ddlMachine.Items.Clear();
        }
        else
        {
            /* Check Hardcoded Process code and fetch qty from Productivity Master and Bind*/

            if (ddlProcess.SelectedValue == "-2147483648")  // Casting
            {
                dt = CommonClasses.Execute("SELECT isnull(PROM_CASTING,0) as PROM_CASTING FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dt.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dt.Rows[0]["PROM_CASTING"].ToString(); }
            }
            else if (ddlProcess.SelectedValue == "-2147483647")//CORE SHOP
            {
                dt = CommonClasses.Execute("SELECT isnull(PROM_CORE,0) as PROM_CORE FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dt.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dt.Rows[0]["PROM_CORE"].ToString(); }
            }
            else if (ddlProcess.SelectedValue == "-2147483646") //INSPECTION
            {
                dt = CommonClasses.Execute("SELECT isnull(PROM_IMP,0) as PROM_IMP FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dt.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dt.Rows[0]["PROM_IMP"].ToString(); }
            }
            else if (ddlProcess.SelectedValue == "-2147483645") //LEAKAGE TESTING
            {
                dt = CommonClasses.Execute("SELECT isnull(PROM_LEAKAGE_TESTING,0) as PROM_LEAKAGE_TESTING FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dt.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dt.Rows[0]["PROM_LEAKAGE_TESTING"].ToString(); }
            }
            else if (ddlProcess.SelectedValue == "-2147483644") //MACHIINING
            {
                //dt = CommonClasses.Execute("SELECT isnull(PROM_CASTING,0) as PROM_CASTING FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                //if (dt.Rows.Count == 0) { }
                //else { txtNumbersOfUnits.Text = dt.Rows[0]["PROM_CASTING"].ToString(); }
            }
        }
    }
    #endregion BindQty

    #region ddlMachine_SelectedIndexChanged
    protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtQty = new DataTable();
        if (ddlProcess.SelectedValue == "-2147483644") //MACHIINING from Process_Master_New
        {
            if (ddlMachine.SelectedValue == "-2147483640") //CNC Code from MACHINE_MASTER
            {
                lblSetup.Text = "Setup"; lblSetup.Visible = true; ddlSetup.Visible = true;
            }
            else
            {
                lblSetup.Text = ""; lblSetup.Visible = false; ddlSetup.Visible = false;
            }
            if (ddlMachine.SelectedValue == "-2147483639") //VMC Code from MACHINE_MASTER
            {
                dtQty = CommonClasses.Execute("SELECT isnull(PROM_VMC,0) as PROM_VMC FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtQty.Rows[0]["PROM_VMC"].ToString(); }
            }
            else if (ddlMachine.SelectedValue == "-2147483638") //4TH AXIS Code from MACHINE_MASTER
            {
                dtQty = CommonClasses.Execute("SELECT isnull(PROM_4THAXIS,0) as PROM_4THAXIS FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtQty.Rows[0]["PROM_4THAXIS"].ToString(); }
            }
            else if (ddlMachine.SelectedValue == "-2147483637") //5TH AXIS Code from MACHINE_MASTER
            {
                dtQty = CommonClasses.Execute("SELECT isnull(PROM_5THAXIS,0) as PROM_5THAXIS FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtQty.Rows[0]["PROM_5THAXIS"].ToString(); }
            }
            else if (ddlMachine.SelectedValue == "-2147483636") //HMC Code from MACHINE_MASTER
            {
                dtQty = CommonClasses.Execute("SELECT isnull(PROM_HMC,0) as PROM_HMC FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtQty.Rows[0]["PROM_HMC"].ToString(); }
            }
        }
    }
    #endregion ddlMachine_SelectedIndexChanged

    #region ddlSetup_SelectedIndexChanged
    protected void ddlSetup_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtSetupQty = new DataTable();
        if (ddlProcess.SelectedValue == "-2147483644") //MACHIINING from Process_Master_New
        {
            if (ddlSetup.SelectedValue == "1") //1st Setup Qty From Productivity_Master
            {
                dtSetupQty = CommonClasses.Execute("SELECT isnull(PROM_1STSETUP,0) as PROM_1STSETUP FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtSetupQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtSetupQty.Rows[0]["PROM_1STSETUP"].ToString(); }
            }
            else if (ddlSetup.SelectedValue == "2") //2nd Setup Qty From Productivity_Master
            {
                dtSetupQty = CommonClasses.Execute("SELECT isnull(PROM_2NDSETUP,0) as PROM_2NDSETUP FROM PRODUCTIVITY_MASTER where ES_DELETE=0 AND PROM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "'");
                if (dtSetupQty.Rows.Count == 0) { }
                else { txtNumbersOfUnits.Text = dtSetupQty.Rows[0]["PROM_2NDSETUP"].ToString(); }
            }
        }
    }
    #endregion ddlSetup_SelectedIndexChanged

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        //dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_COSTING_HEAD='FINISH GOOD' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO + ' - ' + ITEM_MASTER.I_NAME AS ICODE_INAME FROM ITEM_MASTER INNER JOIN PRODUCT_MASTER ON ITEM_MASTER.I_CODE  = PRODUCT_MASTER.PROD_I_CODE WHERE (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE='-2147483648') AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') and PRODUCT_MASTER.ES_DELETE=0 ORDER BY ICODE_INAME");
        ddlItemName.DataSource = dtFinishItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItem

    #region LoadProcess
    protected void LoadProcess()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("select PMN_CODE,PMN_NAME from PROCESS_MASTER_NEW where ES_DELETE=0 and PMN_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by PMN_NAME");
        ddlProcess.DataSource = dtProcess;
        ddlProcess.DataTextField = "PMN_NAME";
        ddlProcess.DataValueField = "PMN_CODE";
        ddlProcess.DataBind();
        ddlProcess.Items.Insert(0, new ListItem("Select Process Name", "0"));
    }
    #endregion LoadProcess

    #region LoadMachines
    protected void LoadMachines()
    {
        DataTable dtMachines = new DataTable();
        dtMachines = CommonClasses.Execute("SELECT M_CODE,M_NAME,M_DESCR FROM MACHINE_MASTER where M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 ORDER by M_NAME");
        ddlMachine.DataSource = dtMachines;
        ddlMachine.DataTextField = "M_NAME";
        ddlMachine.DataValueField = "M_CODE";
        ddlMachine.DataBind();
        ddlMachine.Items.Insert(0, new ListItem("Select Machine Name", "0"));
    }
    #endregion LoadMachines
}
