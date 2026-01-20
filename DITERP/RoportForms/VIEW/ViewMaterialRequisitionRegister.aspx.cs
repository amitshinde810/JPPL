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

public partial class RoportForms_VIEW_ViewMaterialRequisitionRegister : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       // LoadCombos();
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='23'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            //{


            LoadCombos();

            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;

            chkAllItem.Checked = true;
            dateCheck();

            //}
            //else
            //{
            //    Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            //}
        }
    }
    #region LoadCombos
    private void LoadCombos()
    {
        try
        {


            DataTable dtItemDet = new DataTable();

            if (rbtType.SelectedIndex == 0)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='"+(string)Session["CompanyId"]+"' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='Direct' ORDER BY I_NAME");

            }
            if (rbtType.SelectedIndex == 1)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='As Per Order' ORDER BY I_NAME");

            }
            if (rbtType.SelectedIndex == 2)
            {
                dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='As Per Batch' ORDER BY I_NAME");

            }
            //if (rbtType.SelectedIndex == 2)
            //{
            //    dtItemDet = CommonClasses.Execute("Select I_CODE,I_CODENO,I_NAME,MR_BATCH_NO,MR_TYPE from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MATERIAL_REQUISITION_MASTER.MR_I_CODE=I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  ORDER BY I_NAME");

            //}



            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            //{

            if(chkDateAll.Checked==false )
            {
                if(txtFromDate.Text=="" || txtToDate.Text=="")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    return;

                }
            }
            if(chkAllItem.Checked==false)
            {
                if(ddlFinishedComponent.SelectedIndex==0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
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
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }

                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

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
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                }

            }
            #endregion

            #region AsperBatch
            if (rbtType.SelectedIndex == 2)
            {
                str1 = "AsperBatch";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    }
                }

            }
            #endregion

            //#region All
            //if (rbtType.SelectedIndex == 2)
            //{
            //    str1 = "All";
            //    if (rbtGroup.SelectedIndex == 0)
            //    {
            //        str = "Datewise";
            //        if (chkDateAll.Checked == true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked == true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }

            //    }
            //    if (rbtGroup.SelectedIndex == 1)
            //    {
            //        str = "ItemWise";

            //        if (chkDateAll.Checked == true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked == true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/MaterialRequisitionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }

            //    }

            //}
            //#endregion





            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "You have no rights to Print";
            //}
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Register", "LoadCustomer", Ex.Message);
        }
    }
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
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
}
