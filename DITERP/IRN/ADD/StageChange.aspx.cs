using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_ADD_StageChange : System.Web.UI.Page
{
  #region General Declaration
  UnitMaster_BL BL_UnitMaster = null;
  static int mlCode = 0;
  static string right = "";
  DataRow dr;
  public static string str = "";
  public static int Index = 0;
  static string ItemUpdateIndex = "-1";
  static DataTable dt2 = new DataTable();
  #endregion

  #region Events

  #region Page_Load
  protected void Page_Load(object sender, EventArgs e)
  {

    HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
    home.Attributes["class"] = "active";
    HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
    home1.Attributes["class"] = "active";

    if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
    {
      Response.Redirect("~/Default.aspx", false);
    }
    else
    {
      if (!IsPostBack)
      {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='141'");
        right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
        try
        {
          ViewState["mlCode"] = mlCode;
          ViewState["Index"] = Index;
          ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
          ViewState["dt2"] = dt2;
          ((DataTable)ViewState["dt2"]).Rows.Clear();
          str = "";
          ViewState["str"] = str;

          txtGRNDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
          txtGRNDate.Attributes.Add("readonly", "readonly");


          loadStage();
          loadItem();



        }
        catch (Exception ex)
        {
          CommonClasses.SendError("FOUNDRY IRN ENTRY", "PageLoad", ex.Message);
        }
      }

    }
  }
  #endregion

  #region btnSubmit_Click
  protected void btnSubmit_Click(object sender, EventArgs e)
  {
    try
    {
      if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
      {
        if (ddlItemCode.SelectedValue=="0")
        {
          PanelMsg.Visible = true;
          lblmsg.Text = "Please Select From Stage";
          ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
          ddlItemCode.Focus();
          return;
        }
        if (ddlItemName.SelectedValue == "0")
        {
          PanelMsg.Visible = true;
          lblmsg.Text = "Please Select To Stage";
          ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
          ddlItemName.Focus();
          return;
        }
        Boolean res = false;
        for (int i = 0; i < chkItems.Items.Count; i++)
        {
          if (chkItems.Items[i].Selected)
          {
            res = true;
            break;
          }
        }
        if (!res)
        {
          PanelMsg.Visible = true;
          lblmsg.Text = "Please Select Item";
          ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
          chkItems.Focus();
          return;
        }
        SaveRec();
      }
      else
      {
        PanelMsg.Visible = true;
        lblmsg.Text = "You have No Rigts To Save";
        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

      }

    }
    catch (Exception Ex)
    {

      CommonClasses.SendError("FOUNDRY IRN ENTRY", "btnSubmit_Click", Ex.Message);
    }
  }
  #endregion

  #region btnCancel_Click
  protected void btnCancel_Click(object sender, EventArgs e)
  {
    try
    {
      CancelRecord();
    }
    catch (Exception ex)
    {
      CommonClasses.SendError("Component Stages Change", "btnCancel_Click", ex.Message.ToString());
    }

  }

  private bool CheckValid()
  {
    bool flag = false;
    try
    {
      //if (txtName.Text.Trim() == "")
      //{
      //  flag = false;
      //}
      //else if (txtStageNo.Text.Trim() == "")
      //{
      //  flag = false;
      //}
      //else
      //{
      //  flag = true;
      //}

    }
    catch (Exception Ex)
    {
      CommonClasses.SendError("Component Stages Change", "CheckValid", Ex.Message);
    }

    return flag;
  }
  protected void btnOk_Click(object sender, EventArgs e)
  {
    //SaveRec();
    CancelRecord();
  }
  protected void btnCancel1_Click(object sender, EventArgs e)
  {
    //CancelRecord();
  }
  private void CancelRecord()
  {
    try
    {

      Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
    }
    catch (Exception Ex)
    {
      CommonClasses.SendError("Component Stages Change", "btnCancel_Click", Ex.Message);
    }
  }
  #endregion
  #endregion

  #region ViewRec
  private void ViewRec(string str)
  {
    try
    {
      DataTable dt = new DataTable();
      dt = CommonClasses.Execute("SELECT  IRN_CODE, IRN_NO, IRN_CM_ID, IRN_DATE FROM IRN_ENTRY WHERE   (ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
      if (dt.Rows.Count > 0)
      {
        txtGRNno.Text = dt.Rows[0]["IRN_NO"].ToString();
        txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IRN_DATE"].ToString()).ToString("dd/MMM/yyyy");
      }
      DataTable dtdetails = new DataTable();
      dtdetails = CommonClasses.Execute("SELECT IRN_DETAIL.IRND_RSM_CODE, CASE WHEN IRND_TYPE=1 then 'CASTING' ELSE 'MECHINING' END AS IRND_TYPE, IRN_DETAIL.IRND_I_CODE, IRN_DETAIL.IRND_UOM, IRN_DETAIL.IRND_RM_CODE,  IRN_DETAIL.IRND_PROD_QTY, IRN_DETAIL.IRND_REJ_QTY, IRN_DETAIL.IRND_RATE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,  ITEM_UNIT_MASTER.I_UOM_NAME, '' AS RSM_NO,'' AS RM_DEFECT, '' AS RSM_NAME FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON IRN_DETAIL.IRND_I_CODE = ITEM_MASTER.I_CODE WHERE   (IRN_ENTRY.ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
      if (dtdetails.Rows.Count > 0)
      {
        ViewState["dt2"] = dtdetails;
      }
      if (str == "MOD")
      {
        CommonClasses.SetModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
      }
      else
      {
        txtGRNno.Enabled = false;
        txtGRNDate.Enabled = false;
        ddlItemCode.Enabled = false;
        ddlItemName.Enabled = false;
        btnSubmit.Visible = false;
      }
    }
    catch (Exception Ex)
    {
      CommonClasses.SendError("FOUNDRY IRN ENTRY", "ViewRec", Ex.Message);
    }
  }
  #endregion

  #region GetValues
  public bool GetValues(string str)
  {
    bool res = false;
    try
    {
      if (str == "VIEW")
      {
        btnSubmit.Visible = false;
      }
      else if (str == "MOD")
      {
      }
      res = true;
    }
    catch (Exception ex)
    {
      CommonClasses.SendError("FOUNDRY IRN ENTRY", "GetValues", ex.Message);
    }
    return res;
  }
  #endregion

  #region Setvalues
  public bool Setvalues()
  {
    bool res = false;
    try
    {
      res = true;
    }
    catch (Exception ex)
    {
      CommonClasses.SendError("FOUNDRY IRN ENTRY", "Setvalues", ex.Message);
    }
    return res;
  }
  #endregion

  #region SaveRec
  bool SaveRec()
  {
    bool result = false;
    try
    {
      string strType = "";
      txtGRNno.Text = CommonClasses.GetMaxNO("SELECT MAX(SI_NO)+1 FROM STGAEWISE_ITEM  ");
      if (chkItems.Items.Count > 0)
      {
        for (int i = 0; i < chkItems.Items.Count; i++)
        {
          if (chkItems.Items[i].Selected == true)
          {
            CommonClasses.Execute1(" UPDATE STGAEWISE_ITEM SET SI_ACTIVE=0 where SI_I_CODE= '" + chkItems.Items[i].Value.ToString() + "'");
            CommonClasses.Execute1("INSERT INTO STGAEWISE_ITEM (SI_I_CODE, SI_STG_CODE, SI_DATE, SI_NO,SI_ACTIVE) VALUES ('" + chkItems.Items[i].Value.ToString() + "','" + ddlItemName.SelectedValue + "','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + txtGRNno.Text.Trim() + "',1) ");
            string Code = CommonClasses.GetMaxId("Select Max(SI_CODE) from STGAEWISE_ITEM");
            CommonClasses.WriteLog("Stage Change", "Save", "Stage Change", txtGRNno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
          }
        }
        result = true;
        chkItems.Items.Clear();
        loadItem();
      }
      else
      {
        ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
      }
    }
    catch (Exception ex)
    {
      CommonClasses.SendError("FOUNDRY IRN ENTRY", "SaveRec", ex.Message);
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
      CommonClasses.SendError("FOUNDRY IRN ENTRY", "ShowMessage", Ex.Message);
      return false;
    }
  }
  #endregion



  public void loadStage()
  {
  }

  public void loadItem()
  {
    DataTable dtStage = new DataTable();
    dtStage = CommonClasses.Execute(" SELECT DISTINCT STG_CODE,STG_NAME FROM STAGE_MASTER where ES_DELETE=0 AND STG_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  ORDER BY STG_NAME");
    ddlItemCode.DataSource = dtStage;
    ddlItemCode.DataTextField = "STG_NAME";
    ddlItemCode.DataValueField = "STG_CODE";
    ddlItemCode.DataBind();
    ddlItemCode.Items.Insert(0, new ListItem("----Select Stage----", "0"));

    ddlItemName.DataSource = dtStage;
    ddlItemName.DataTextField = "STG_NAME";
    ddlItemName.DataValueField = "STG_CODE";
    ddlItemName.DataBind();
    ddlItemName.Items.Insert(0, new ListItem("----Select Stage----", "0"));
  }

  protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
  {
    PanelMsg.Visible = false;
  }
  protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
  {
    PanelMsg.Visible = false;
    chkItems.Items.Clear();
    DataTable dtStage = new DataTable();
    if (ddlItemCode.SelectedValue=="-2147483648")
    {
        dtStage = CommonClasses.Execute(" SELECT I_CODE, I_CAT_CODE, I_CODENO +'-'+ I_NAME as I_CODENO,I_NAME FROM ITEM_MASTER where I_CAT_CODE=-2147483648 AND ES_DELETE=0 AND I_CODE NOT IN (SELECT DISTINCT SI_I_CODE FROM STGAEWISE_ITEM )");
    }
    else
    {
      dtStage = CommonClasses.Execute("SELECT MAX(SI_DATE),I_CODE, I_CAT_CODE, I_CODENO +'-'+ I_NAME as I_CODENO,I_NAME  FROM STGAEWISE_ITEM,STAGE_MASTER,ITEM_MASTER where  STAGE_MASTER.ES_DELETE=0 AND SI_STG_CODE= STG_CODE AND I_CODE=SI_I_CODE AND STG_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND STG_CODE ='" + ddlItemCode.SelectedValue + "' AND SI_ACTIVE=1 GROUP BY I_CODE,I_CAT_CODE,I_CODENO,I_NAME ORDER BY I_CODENO");
    }
    for (int i = 0; i < dtStage.Rows.Count; i++)
    {
      ListItem item = new ListItem();
      item.Text = dtStage.Rows[i]["I_CODENO"].ToString();
      item.Value = dtStage.Rows[i]["I_CODE"].ToString();
      item.Selected = false;
      chkItems.Items.Add(item);
    }

    dtStage = CommonClasses.Execute(" SELECT DISTINCT STG_CODE,STG_NAME FROM STAGE_MASTER where ES_DELETE=0 AND  STG_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND STG_CODE <>'" + ddlItemCode.SelectedValue + "' ORDER BY STG_NAME");

    ddlItemName.DataSource = dtStage;
    ddlItemName.DataTextField = "STG_NAME";
    ddlItemName.DataValueField = "STG_CODE";
    ddlItemName.DataBind();
    ddlItemName.Items.Insert(0, new ListItem("----Select Stage----", "0"));
    chkAll.Visible = true;
  }
  protected void chkAll_CheckedChanged(object sender, EventArgs e)
  {
    if (chkAll.Checked == true)
    {
      for (int i = 0; i < chkItems.Items.Count; i++)
      {
        chkItems.Items[i].Selected = true;
      }
    }
    else
    {
      for (int i = 0; i < chkItems.Items.Count; i++)
      {
        chkItems.Items[i].Selected = false;
      }
    }
  }

  #region txtGRNDate_TextChanged
  protected void txtGRNDate_TextChanged(object sender, EventArgs e)
  {
    if (CommonClasses.ValidRights(int.Parse(right.Substring(6, 1)), this, "For BackDate Entry"))
    {
      PanelMsg.Visible = false;
      if (Convert.ToDateTime(txtGRNDate.Text) > System.DateTime.Now)
      {
        PanelMsg.Visible = true;
        lblmsg.Text = "Please Select Date Less Than Current date.";
        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        txtGRNDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
        txtGRNDate.Attributes.Add("readonly", "readonly");
      }
    }
    else
    {
      txtGRNDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
      txtGRNDate.Attributes.Add("readonly", "readonly");
    }
  }
  #endregion
}
