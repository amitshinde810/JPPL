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

public partial class RoportForms_VIEW_ViewMaterialRequisition_MIS : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        // LoadCombos();
        if (!IsPostBack)
        {         

            LoadCombos();
            LoadMatReqNo();
            ddlFinishedComponent.Enabled = false;
            ddlRequisitionNo.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;

            chkAllItem.Checked = true;
            dateCheck();

         
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {

            DataTable dtItemDet = new DataTable();

            if (rbtType.SelectedIndex == 0)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='Direct' ORDER BY I_NAME");

            }
            if (rbtType.SelectedIndex == 1)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='As Per Order' ORDER BY I_NAME");

            }
            if (rbtType.SelectedIndex == 2)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  ORDER BY I_NAME");

            }

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition MIS Report", "LoadCombos", Ex.Message);
        }

    }
    #endregion

    #region LoadMatReqNo
    private void LoadMatReqNo()
    {
        try
        {
            string str = "";
            if (rbtType.SelectedIndex == 0)
            {
                str = "select MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and MR_TYPE='Direct' and ES_DELETE=0 order by MR_BATCH_NO desc";
            }
            else if (rbtType.SelectedIndex == 1)
            {
                str = "select MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and MR_TYPE='As Per Order' and ES_DELETE=0 order by MR_BATCH_NO desc";
            }
            else
            {
                str = "select MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 order by MR_BATCH_NO desc";
            }
            DataTable dt = CommonClasses.Execute(str);

            ddlRequisitionNo.DataSource = dt;
            ddlRequisitionNo.DataTextField = "MR_BATCH_NO";
            ddlRequisitionNo.DataValueField = "MR_CODE";
            ddlRequisitionNo.DataBind();
            ddlRequisitionNo.Items.Insert(0, new ListItem("Mat.Req.No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition MIS-Report", "LoadMatReqNo", Ex.Message);
        }
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
            CommonClasses.SendError("Material Requisition MIS Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        //if(Convert.ToDateTime(txtFromDate.Text))
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;
        To = txtToDate.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    return;

                }
            }

        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {         

            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    return;

                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
                    return;

                }
            }
            if (ChkProductionNoAll.Checked == false)
            {
                if (ddlRequisitionNo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Mat.Req.No";
                    return;

                }
            }

            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";

            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Direct";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO="+ChkProductionNoAll.Checked.ToString()+"&REQ_NO="+ddlRequisitionNo.SelectedValue.ToString()+"", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }

                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }

                }

            }
            #endregion

            #region AsperReq
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "AsperReq";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                }

            }
            #endregion

            #region All
            if (rbtType.SelectedIndex == 2)
            {
                str1 = "All";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }

                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisition_MIS.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&ALL_NO=" + ChkProductionNoAll.Checked.ToString() + "&REQ_NO=" + ddlRequisitionNo.SelectedValue.ToString() + "", false);

                    }

                }

            }
            #endregion





            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "You have no rights to Print";
            //}
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition -MIS Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition -MIS Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
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
        dateCheck();
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
        LoadMatReqNo();
    }
    #endregion

    #region ChkProductionNoAll_CheckedChanged
    protected void ChkProductionNoAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkProductionNoAll.Checked == true)
        {
            ddlRequisitionNo.Enabled = false;
            ddlRequisitionNo.Enabled = false;
        }
        else
        {
            ddlRequisitionNo.Enabled = true;
            ddlRequisitionNo.Enabled = true;
        }
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFinishedComponent.SelectedIndex != -1 && ddlFinishedComponent.SelectedIndex !=0)
            {
                string str = "";
                if (rbtType.SelectedIndex == 0)
                {
                    str = "select MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and MR_TYPE='Direct' and MR_I_CODE='"+ddlFinishedComponent.SelectedValue+"' and ES_DELETE=0 ";
                }
                else if (rbtType.SelectedIndex == 1)
                {
                    str = "MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and MR_TYPE='As Per Order' and MR_I_CODE='" + ddlFinishedComponent.SelectedValue + "' and ES_DELETE=0 ";
                }
                else
                {
                    str = "MR_CODE,MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER WHERE  MR_COMP_CM_CODE=" + Session["CompanyCode"].ToString() + " and MR_I_CODE='" + ddlFinishedComponent.SelectedValue + "' and  ES_DELETE=0 ";
                }
                DataTable dt = CommonClasses.Execute(str);

                ddlRequisitionNo.DataSource = dt;
                ddlRequisitionNo.DataTextField = "MR_BATCH_NO";
                ddlRequisitionNo.DataValueField = "MR_CODE";
                ddlRequisitionNo.DataBind();
                ddlRequisitionNo.Items.Insert(0, new ListItem("Mat.Req.No", "0"));
                ddlFinishedComponent.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition MIS-Report", "ddlFinishedComponent_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion
}
