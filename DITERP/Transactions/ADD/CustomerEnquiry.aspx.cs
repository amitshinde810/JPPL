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
using System.Data.SqlClient;

public partial class Transactions_ADD_CustomerEnquiry : System.Web.UI.Page
{
    # region Variables  
    static int mlCode = 0;
    static string right = "";
    # endregion

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
                    try
                    {
                        LoadCustomer();
                        txtRequestDate.Attributes.Add("readonly", "readonly");
                        txtDesiredDate.Attributes.Add("readonly", "readonly");
                        txtCompletionDate.Attributes.Add("readonly", "readonly");
                        txtRequestDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtDesiredDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtCompletionDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Enquiry", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Enquiry", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1 order by P_NAME");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM ENQUIRY_MASTER WHERE INQ_CODE=" + mlCode + " AND INQ_CM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtProjectType.Text = dt.Rows[0]["INQ_TYPE"].ToString();
                txtProjectNo.Text = dt.Rows[0]["INQ_NO"].ToString();
                txtGloss.Text = dt.Rows[0]["INQ_GLOSS"].ToString();
                txtShadeName.Text = dt.Rows[0]["INQ_SHADE_NAME"].ToString();
                ddlCustomer.SelectedValue = dt.Rows[0]["INQ_CUST_NAME"].ToString();
                txtShadeCode.Text = dt.Rows[0]["INQ_SHADE_CODE"].ToString();
                txtShadeNo.Text = dt.Rows[0]["INQ_SHADE_NO"].ToString();
                txtRequestDate.Text = Convert.ToDateTime(dt.Rows[0]["INQ_REQ_DATE"]).ToString("dd MMM yyyy");
                txtRequestedBy.Text = dt.Rows[0]["INQ_REQ_BY"].ToString();
                txtDesiredDate.Text = Convert.ToDateTime(dt.Rows[0]["INQ_DES_DUE_DATE"]).ToString("dd MMM yyyy");
                txtEnduse.Text = dt.Rows[0]["INQ_END_USE"].ToString();
                txtSubstrate.Text = dt.Rows[0]["INQ_SUBRTATE"].ToString();
                txtPrimerDFT.Text = dt.Rows[0]["INQ_PRIMER_DFT"].ToString();
                txtVolume.Text = dt.Rows[0]["INQ_VOL_SOLID"].ToString();
                txtDFT.Text = dt.Rows[0]["INQ_DFT"].ToString();
                txtPMT.Text = dt.Rows[0]["INQ_PMT"].ToString();
                txtProperties.Text = dt.Rows[0]["INQ_CUST_SPEC"].ToString();
                txtNote.Text = dt.Rows[0]["INQ_NOTE"].ToString();
                txtCompletionDate.Text = Convert.ToDateTime(dt.Rows[0]["INQ_COMP_DATE"]).ToString("dd MMM yyyy");

                //Newly Added
                txtBendtest.Text = dt.Rows[0]["INQ_BENDTEST"].ToString();

                if (str == "VIEW")
                {
                    txtProjectType.Enabled = false;
                    txtProjectNo.Enabled = false;
                    txtGloss.Enabled = false;
                    txtShadeName.Enabled = false;
                    ddlCustomer.Enabled = false;
                    txtShadeCode.Enabled = false;
                    txtShadeNo.Enabled = false;
                    txtRequestDate.Enabled = false;
                    txtRequestedBy.Enabled = false;
                    txtDesiredDate.Enabled = false;
                    txtEnduse.Enabled = false;
                    txtSubstrate.Enabled = false;
                    txtPrimerDFT.Enabled = false;
                    txtVolume.Enabled = false;
                    txtDFT.Enabled = false;
                    txtPMT.Enabled = false;
                    txtProperties.Enabled = false;
                    txtNote.Enabled = false;
                    txtCompletionDate.Enabled = false;
                    btnSubmit.Visible = false;

                    txtBendtest.Enabled = false;
                }
            }

            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ENQUIRY_MASTER", "MODIFY", "INQ_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Enquiry", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(txtCompletionDate.Text) < Convert.ToDateTime(txtRequestDate.Text))
        {
            ShowMessage("#Avisos", "Completion Date should not less than Request Date", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            txtCompletionDate.Focus();
            return;
        }
        SaveRec();
    }
    #endregion 

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {

            if (Request.QueryString[0].Equals("INSERT"))
            {

                DataTable dt = new DataTable();

                int Doc_no = GenProjectNo();
                //DataTable dtNo = new DataTable();
                //dtNo = CommonClasses.Execute("select isnull(max(INQ_NO),0) as INQ_NO from ENQUIRY_MASTER where  INQ_CUST_NAME='" + ddlCustomer.SelectedValue + "' and INQ_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ENQUIRY_MASTER.ES_DELETE=0 ");
                ////dtNo = CommonClasses.Execute("Select isnull(max(INQ_NO),0) as INQ_NO FROM ENQUIRY_MASTER WHERE INQ_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                //if (dtNo.Rows.Count > 0)
                //{
                //    Doc_no = Convert.ToInt32(dtNo.Rows[0]["INQ_NO"]);
                //    Doc_no = Doc_no + 1;
                //}
                dt = CommonClasses.Execute("Select INQ_NO FROM ENQUIRY_MASTER WHERE lower(INQ_NO)= lower('" + Doc_no + "') and INQ_CUST_NAME='" + ddlCustomer.SelectedValue + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {

                    if (CommonClasses.Execute1("INSERT INTO ENQUIRY_MASTER (INQ_CM_COMP_ID,INQ_NO,INQ_TYPE,INQ_GLOSS,INQ_SHADE_NAME,INQ_CUST_NAME,INQ_SHADE_CODE,INQ_SHADE_NO,INQ_REQ_DATE,INQ_REQ_BY,INQ_DES_DUE_DATE,INQ_END_USE,INQ_SUBRTATE,INQ_PRIMER_DFT,INQ_VOL_SOLID,INQ_DFT,INQ_PMT,INQ_CUST_SPEC,INQ_NOTE,INQ_COMP_DATE,MODIFY,ES_DELETE,INQ_QT_FLAG,INQ_BENDTEST) VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Doc_no + "','" + txtProjectType.Text + "','" + txtGloss.Text + "','" + txtShadeName.Text + "','" + ddlCustomer.SelectedValue + "','" + txtShadeCode.Text + "','" + txtShadeNo.Text + "','" + Convert.ToDateTime(txtRequestDate.Text).ToString("dd/MMM/yyyy") + "','" + txtRequestedBy.Text + "','" + Convert.ToDateTime(txtDesiredDate.Text).ToString("dd/MMM/yyyy") + "','" + txtEnduse.Text + "','" + txtSubstrate.Text + "','" + txtPrimerDFT.Text + "','" + txtVolume.Text + "','" + txtDFT.Text + "','" + txtPMT.Text + "','" + txtProperties.Text + "','" + txtNote.Text + "','" + Convert.ToDateTime(txtCompletionDate.Text).ToString("dd/MMM/yyyy") + "',0,0,0,'"+txtBendtest.Text +"')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(INQ_CODE) from ENQUIRY_MASTER");
                        CommonClasses.WriteLog("Customer Enquiry", "Save", "Customer Enquiry", Convert.ToString(txtProjectNo.Text), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                      
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewCustomerEnquiry.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);

                        txtProjectType.Focus();
                    }
                }
                else
                {


                    ShowMessage("#Avisos", "Enquiry No Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {


                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM ENQUIRY_MASTER WHERE ES_DELETE=0 AND INQ_CUST_NAME= '" + ddlCustomer.SelectedValue + "' AND lower(INQ_NO) = lower('" + txtProjectNo.Text + "')");
                if (dt.Rows.Count == 1)
                {
                    if (CommonClasses.Execute1("UPDATE ENQUIRY_MASTER SET INQ_NO='" + txtProjectNo.Text.Trim() + "',INQ_TYPE='" + txtProjectType.Text + "',INQ_GLOSS='" + txtGloss.Text + "',INQ_SHADE_NAME='" + txtShadeName.Text + "',INQ_CUST_NAME='" + ddlCustomer.SelectedValue + "',INQ_SHADE_CODE='" + txtShadeCode.Text + "',INQ_SHADE_NO='" + txtShadeNo.Text + "',INQ_REQ_DATE='" + Convert.ToDateTime(txtRequestDate.Text).ToString("dd/MMM/yyyy") + "',INQ_REQ_BY='" + txtRequestedBy.Text + "',INQ_DES_DUE_DATE='" + Convert.ToDateTime(txtDesiredDate.Text).ToString("dd/MMM/yyyy") + "',INQ_END_USE='" + txtEnduse.Text + "',INQ_SUBRTATE='" + txtSubstrate.Text + "',INQ_PRIMER_DFT='" + txtPrimerDFT.Text + "',INQ_VOL_SOLID='" + txtVolume.Text + "',INQ_DFT='" + txtDFT.Text + "',INQ_PMT='" + txtPMT.Text + "',INQ_CUST_SPEC='" + txtProperties.Text + "',INQ_NOTE='" + txtNote.Text + "',INQ_COMP_DATE='" + Convert.ToDateTime(txtCompletionDate.Text).ToString("dd/MMM/yyyy") + "',INQ_BENDTEST='"+txtBendtest .Text +"' WHERE INQ_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("ENQUIRY_MASTER", "MODIFY", "INQ_CODE", mlCode);
                        CommonClasses.WriteLog("Customer Enquiry", "Update", "Customer Enquiry", txtProjectNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewCustomerEnquiry.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        txtProjectType.Focus();
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "Enquiry No Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtProjectType.Focus();
                }


            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Enquiry", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Customer Enquiry", "ShowMessage", Ex.Message);
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
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Customer Enquiry", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("ENQUIRY_MASTER", "MODIFY", "INQ_CODE", mlCode);
            }

            Response.Redirect("~/Transactions/VIEW/ViewCustomerEnquiry.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Enquiry", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtProjectType.Text.Trim() == "")
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
            CommonClasses.SendError("Customer Enquiry", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region GenProjectNo
    protected int GenProjectNo()
    {
        int P_NO = 0;
        try
        {
           
            if (ddlCustomer.SelectedIndex != 0)
            {
                DataTable dtAdd = CommonClasses.Execute("select isnull(max(INQ_NO),0)+1 as INQ_NO from ENQUIRY_MASTER where  INQ_CUST_NAME='" + ddlCustomer.SelectedValue + "' and INQ_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ENQUIRY_MASTER.ES_DELETE=0 ");
                DataTable dtAbb = CommonClasses.Execute("select P_ABBREVATION from PARTY_MASTER where  P_CODE='" + ddlCustomer.SelectedValue + "'");
                string no = "";
                if (dtAbb.Rows.Count > 0)
                {
                    no = dtAbb.Rows[0]["P_ABBREVATION"].ToString();
                }
                if (dtAdd.Rows.Count > 0)
                {
                    no = no + dtAdd.Rows[0]["INQ_NO"].ToString();
                }
                P_NO = Convert.ToInt32(dtAdd.Rows[0]["INQ_NO"].ToString());
                txtProjectNo.Text = no.ToString();
              
            }
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry", "GenProjectNo", Ex.Message);
        }
        return P_NO;
       
    }
     #endregion
}
