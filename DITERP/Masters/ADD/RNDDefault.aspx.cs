using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Masters_ADD_RNDDefault : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            if (!IsPostBack)
            {
                #region Hiding Menus As Per Rights
                
                string process = "";
                DataTable dtProcess = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 88 + "");
                process = dtProcess.Rows.Count == 0 ? "0000000" : dtProcess.Rows[0][0].ToString();
                if (process == "0000000" || process == "0111111")
                {
                    Process.Visible = false;
                }

                string shadecreation = "";
                DataTable dtShade = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 96 + "");
                shadecreation = dtShade.Rows.Count == 0 ? "0000000" : dtShade.Rows[0][0].ToString();
                if (shadecreation == "0000000" || shadecreation == "0111111")
                {
                    ShadeCreation.Visible = false;
                }

                string batch = "";
                DataTable dtBatch = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 98 + "");
                batch = dtBatch.Rows.Count == 0 ? "0000000" : dtBatch.Rows[0][0].ToString();
                if (batch == "0000000" || batch == "0111111")
                {
                    BatchTicket.Visible = false;
                }

                string tintersheet = "";
                DataTable dtTinter = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 100 + "");
                tintersheet = dtTinter.Rows.Count == 0 ? "0000000" : dtTinter.Rows[0][0].ToString();
                if (tintersheet == "0000000" || tintersheet == "0111111")
                {
                    TinterSheet.Visible = false;
                }

                //string fillofsheet = "";
                //DataTable dtfill = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 101 + "");
                //fillofsheet = dtfill.Rows.Count == 0 ? "0000000" : dtfill.Rows[0][0].ToString();
                //if (fillofsheet == "0000000" || fillofsheet == "0111111")
                //{
                //    FillOffSheet.Visible = false;
                //}

                string masterreg = "";
                DataTable dtmasterreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 22 + "");
                masterreg = dtmasterreg.Rows.Count == 0 ? "0000000" : dtmasterreg.Rows[0][0].ToString();
                if (masterreg == "0000000" || masterreg == "0111111")
                {
                    MasterRpt.Visible = false;
                }

                string batchreg = "";
                DataTable dtbatchreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 104 + "");
                batchreg = dtbatchreg.Rows.Count == 0 ? "0000000" : dtbatchreg.Rows[0][0].ToString();
                if (batchreg == "0000000" || batchreg == "0111111")
                {
                    BatchReg.Visible = false;
                }
               
                #endregion
            }

        }
    }

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #region Masters
    protected void btnProcessMaster_click(object sender, EventArgs e)
    {
        checkRights(88);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewProcessMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }
       
    #endregion

    #region Transaction
       

    protected void btnShadeCreation_click(object sender, EventArgs e)
    {
        checkRights(96);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewShadeCreation.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBatchTicket_click(object sender, EventArgs e)
    {
        checkRights(98);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewBatchTicket.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //btnTinterSheet_click

    protected void btnTinterSheet_click(object sender, EventArgs e)
    {
        checkRights(100);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTinterSheet.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    //protected void btnFillOffSheet_click(object sender, EventArgs e)
    //{
    //    checkRights(101);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/Transactions/VIEW/ViewFillOffSheet.aspx", false);
    //    }
    //    else
    //    {
    //        //Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}

    #endregion

    #region Reports

    protected void btnMasterReport_click(object sender, EventArgs e)
    {
        checkRights(22);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewRNDMasterReport.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBatchRegister_click(object sender, EventArgs e)
    {
        checkRights(104);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBatchRegister.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/RNDDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("RND & QC Default", "btnOk_Click", Ex.Message);
        }
    }
}
