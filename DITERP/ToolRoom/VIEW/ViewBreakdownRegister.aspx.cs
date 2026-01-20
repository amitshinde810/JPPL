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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


public partial class ToolRoom_VIEW_ViewBreakdownRegister : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='171'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            ddlTool.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            chkDateAll.Checked = false;

            chkAllItem.Checked = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            //txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            //txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtToDate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
            LoadType();
        }
    }

    #region LoadType
    private void LoadType()
    {
        DataTable dt = new DataTable();

        try
        {
            string str = "";
            if (rbtType.SelectedValue != "2")
            {
                str = " B_T_TYPE=" + rbtType.SelectedValue + " AND ";
            }

            dt = CommonClasses.Execute("SELECT DISTINCT T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME,T_CODE FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON  B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON  B_I_CODE = ITEM_MASTER.I_CODE WHERE   B_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND  " + str + "  ( B_TYPE = 0) AND B_STATUS=1 AND ( BREAKDOWN_ENTRY.ES_DELETE = 0)  AND B_CM_CODE=" + Session["CompanyCode"].ToString() + " and B_CM_ID=" + (string)Session["CompanyId"] + " order by T_NAME");
            ddlTool.DataSource = dt;
            ddlTool.DataTextField = "T_NAME";
            ddlTool.DataValueField = "T_CODE";
            ddlTool.DataBind();
            ddlTool.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Tool No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Improvement Register", "LoadTools", Ex.Message);
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Improvement Register", "btnCancel_Click", ex.Message);
        }
    }

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
            CommonClasses.SendError("Improvement Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlTool.SelectedIndex = 0;
            ddlTool.Enabled = false;
        }
        else
        {
            ddlTool.SelectedIndex = 0;
            ddlTool.Enabled = true;

        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            load();
            //Response.Redirect("../ADD/ImprovementRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "&InwdType=" + str2 + "&PTYPE=" + POType + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    string POType = "";
    string strCond = "";
    string From = "";
    string To = "";

    #region load
    private void load()
    {
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            if (chkAllItem.Checked == false)
            {
                if (ddlTool.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Tool No. ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            PanelMsg.Visible = false;

            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]).ToShortDateString();
                To = Convert.ToDateTime(Session["ClosingDate"]).ToShortDateString();
            }

            if (chkDateAll.Checked != true)
            {

                strCond = strCond + " convert(date,B_DATE) BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            else
            {
                strCond = strCond + " convert(date,B_DATE) BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'  AND  ";
            }

            if (chkAllItem.Checked == false)
            {
                strCond = strCond + " B_T_CODE='" + ddlTool.SelectedValue + "' AND  ";
            }
            if (rbtType.SelectedValue == "0")
            {
                strCond = strCond + " T_TYPE=0 AND ";
            }
            if (rbtType.SelectedValue == "1")
            {
                strCond = strCond + " T_TYPE=1 AND ";
            }
            Loadreport(strCond);

            //Response.Redirect("../ADD/ImprovementRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "&InwdType=" + str2 + "&PTYPE=" + POType + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        load();
        // ExportGridToPDF();
        Response.Redirect("../ADD/BreakdownRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&strCond=" + strCond + "", false);


    }
    #endregion

    #region VerifyRenderingInServerForm
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    #endregion

    #region Loadreport
    private void Loadreport(string str)
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT B_CODE,case when B_STATUS=0 then 'Open' else 'Close' end as B_STATUS,B_T_CODE,B_NO as B_SLIPNO, B_NO,  B_DATE , B_REASON, B_ACTION, B_CLOSURE, isnull(B_HOURS ,0) as B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO +' - '+ I_NAME  AS  I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " B_CM_CODE=" + Session["CompanyCode"].ToString() + " AND (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=0 ORDER BY B_NO,T_NAME");
        dgBreakdown.DataSource = dt;
        dgBreakdown.DataBind();
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadType();
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtToDate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtFromDate.Focus();
            return;
        }
        //txtToDate.Text = Convert.ToDateTime(dtDate1.AddMonths(1).ToString()).ToString("MMM yyyy");
        LoadType();
    }
    #endregion

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        /*Validate Date SHould be within Financial year*/
        DateTime dtDate1 = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtToDate.Text);

        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtToDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //ShowMessage("#Avisos", "Date Must Be In Between Financial Year..", CommonClasses.MSG_Warning);
            txtToDate.Focus();
            return;
        }
        if (dtDate1 < dtDate)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "To Date Should Be Less than From Date..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtToDate.Text = System.DateTime.Now.AddMonths(1).ToString("MMM yyyy");
            return;
        }
        // txtFromDate.Text = Convert.ToDateTime(dtDate.AddMonths(-1).ToString()).ToString("MMM yyyy");
        LoadType();

    }
    #endregion txtToDate_TextChanged

    #region dgBreakdown_PageIndexChanging
    protected void dgBreakdown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBreakdown.PageIndex = e.NewPageIndex;
            load();


        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgBreakdown_RowCommand
    protected void dgBreakdown_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            string code = "";
            string directory = "";

            int index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ViewPDF")
            {
                int page = dgBreakdown.PageIndex;
                int Size = dgBreakdown.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgBreakdown.Rows[index];

                //GridView gvRow = (GridView)sender;
                //int selectedRowIndex = ((gvRow.PageIndex) * gvRow.PageSize) + gvRow.SelectedIndex;
                //gvRow = dgBreakdown.Rows[selectedRowIndex];

                code = ((Label)(gvRow.FindControl("lblB_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView"))).Text;
                directory = "../../UpLoadPath/BreakDown/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }



            if (e.CommandName == "Download")
            {

                int page = dgBreakdown.PageIndex;
                int Size = dgBreakdown.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgBreakdown.Rows[index];

                //filePath = ((Label)(gvRow.FindControl("lblfilename"))).Text;
                code = ((Label)(gvRow.FindControl("lblB_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkView1"))).Text;
                directory = "../../UpLoadPath/BreakDown/" + code + "/" + filePath;
                string absolute = Path.GetFullPath(directory);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
                Response.TransmitFile(Server.MapPath(directory));
                //Response.Flush();
                FileInfo fileToDownload = new FileInfo(directory);

                Response.Flush();

                Response.WriteFile(fileToDownload.FullName);
                Response.End();

            }
        }
        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Improvement Entry", "dgBreakdown_RowCommand", Ex.Message);
        }
    }
    #endregion dgBreakdown_RowCommand

    #region ExportGridToPDF
    private void ExportGridToPDF()
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Amit.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            dgBreakdown.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            pdfDoc.AddTitle("Improvement Register");
            pdfDoc.AddHeader("Header", "PO Type Master");
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.Flush();
            Response.End();
            dgBreakdown.AllowPaging = true;
            dgBreakdown.DataBind();
        }
        catch (Exception)
        {

        }
        finally
        {
        }

    }
    #endregion
}
