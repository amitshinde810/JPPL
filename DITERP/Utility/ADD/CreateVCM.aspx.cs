using System;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Utility_ADD_CreateVCM : System.Web.UI.Page
{
    DataTable dtFilter = new DataTable();
    static DataTable DtInvDet = new DataTable();
    DataRow dr;
    static int mlCode = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            LoadCombos();
            LoadInvoices();
            LoadFilter();
        }
    }

    private void LoadInvoices()
    {
        try
        {
            DataTable FromInv = new DataTable();
           // FromInv = CommonClasses.Execute("SELECT DISTINCT INM_CODE,INM_NO FROM INVOICE_MASTER WHERE   ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='TAXINV' AND ISNULL(INM_IS_TALLY_TRANS,0)=0 ORDER BY INM_NO DESC");

            FromInv = CommonClasses.Execute("SELECT DISTINCT top 1000 INM_CODE,INM_NO FROM INVOICE_MASTER WHERE   ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='TAXINV'  ORDER BY INM_NO DESC");
            ddlFromInvNo.DataSource = FromInv;
            ddlFromInvNo.DataTextField = "INM_NO";
            ddlFromInvNo.DataValueField = "INM_CODE";
            ddlFromInvNo.DataBind();
            ddlFromInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));

            ddlToInvNo.DataSource = FromInv;
            ddlToInvNo.DataTextField = "INM_NO";
            ddlToInvNo.DataValueField = "INM_CODE";
            ddlToInvNo.DataBind();
            ddlToInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Create VCM", "LoadInvoices", Ex.Message.ToString());
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomer.DataSource = custdet;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally transfer Sales", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnLoad_Click
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlInvType.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice Type";
                PanelMsg.Visible = true;
            }
            if (ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice From & To";
                PanelMsg.Visible = true;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Customer";
                PanelMsg.Visible = true;
            }
            string Query = "SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE FROM INVOICE_MASTER WHERE INM_NO BETWEEN '" + ddlFromInvNo.SelectedItem.ToString() + "' AND '" + ddlToInvNo.SelectedItem.ToString() + "' AND INM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "'";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count == 0)
            {
                LoadFilter();
            }
            else
            {
                dgInvDetails.DataSource = dt;
                dgInvDetails.DataBind();
                dgInvDetails.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    protected void btnExport_Click(object sender, EventArgs e)
    {

 


        DirectoryInfo DI = new DirectoryInfo(Server.MapPath(@"~/UpLoadPath/"));
        FileInfo[] Delfiles = DI.GetFiles("*.csv");
        int d = 0;
        foreach (FileInfo fi in Delfiles)
        {
            System.IO.File.Delete(DI + "/" + Delfiles[d]);
            d++;
        }
        string InvCodes = "";
        try
        {
            if (DtInvDet.Columns.Count == 0)
            {
                DtInvDet.Columns.Add("INM_CODE");
                DtInvDet.Columns.Add("INM_NO");
                DtInvDet.Columns.Add("INM_DATE");
            }
            for (int j = 0; j < dgInvDetails.Rows.Count; j++)
            {
                CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[j].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked)
                {
                    dr = DtInvDet.NewRow();
                    dr["INM_CODE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text;
                    dr["INM_NO"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_NO"))).Text;
                    dr["INM_DATE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_DATE"))).Text;
                    DtInvDet.Rows.Add(dr);
                }

                if (j == dgInvDetails.Rows.Count - 1)
                {
                    InvCodes = InvCodes + ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text;
                }
                else
                {
                    InvCodes = InvCodes + ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text + ",";
                }
            }
            if (ddlInvType.SelectedIndex == 1)
            {
               // DataTable dtExport = CommonClasses.Execute(" SELECT CPOM_PONO as PoNumber,'10' as [PO Item No],IND_INQTY as [Quantity],INM_TNO as [Vendor Challan No],INM_DATE as [Challan Date],CPOD_GROSS_RATE as [Gross Rate],IND_RATE AS [Net P O RATE],INM_NET_AMT AS [Basic value],  INM_ACCESSIBLE_AMT AS[Taxable Value], INM_BE_AMT as [SGST VALUE]  ,INM_EDUC_AMT as [CGST VALUE], INM_H_EDUC_AMT as [IGST VALUE],CASE WHEN INM_BE_AMT>0 then E_BASIC  ELSE '0' END  as [SGST RATE],CASE WHEN INM_EDUC_AMT>0 then E_EDU_CESS  ELSE '0' END  as [CGST RATE],CASE WHEN INM_H_EDUC_AMT>0 then E_H_EDU  ELSE '0' END  as [IGST RATE],INM_PACK_AMT AS [Packing Amount], INM_FREIGHT AS [Freight Amount],  INM_OTHER_AMT AS [Others Amount],INM_G_AMT as [INVOICE VALUE], 'INR' as [Currency],'' as [ROAD PERMIT NO],'' as [57F4 NUMBER],'' as [57F4 NO DATE],CM_GST_NO  as [GSTIN number],INM_VEH_NO as [Vehicle Number],'' as [DRG REV Level],'' as [COP Certificate] ,'' AS [Certificate Date],P_LBT_NO AS[TML GSTIN],'' AS[Field1],'' AS[Field2],'' AS[Field3],'' AS[Field4],'' AS[Field5] FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,ITEM_MASTER,COMPANY_MASTER,PARTY_MASTER,CUSTPO_DETAIL,EXCISE_TARIFF_MASTER  WHERE INM_CODE=IND_INM_CODE AND IND_CPOM_CODE=CPOM_CODE AND IND_I_CODE=I_CODE AND IND_CPOM_CODE=CPOM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=IND_I_CODE AND INM_CM_CODE=CM_CODE AND P_CODE=INM_P_CODE AND I_E_CODE=E_CODE AND INM_CODE in (" + InvCodes + ")   ORDER BY INM_TNO  ");

                DataTable dtExport = CommonClasses.Execute(" SELECT CPOM_PONO as PoNumber,'10' as [PO Item No],IND_INQTY as [Quantity],INM_TNO as [Vendor Challan No],INM_DATE as [Challan Date],CPOD_GROSS_RATE as [Gross Rate],IND_RATE AS [Net P O RATE],INM_NET_AMT AS [Basic value],  INM_ACCESSIBLE_AMT AS[Taxable Value],   CASE WHEN convert(varchar,INM_BE_AMT)='0' then ' '  ELSE convert(varchar,INM_BE_AMT)  END   as [SGST VALUE]  , CASE WHEN convert(varchar,INM_EDUC_AMT)='0' then ' '  ELSE convert(varchar,INM_EDUC_AMT)  END   as [CGST VALUE],   CASE WHEN   convert(varchar,INM_H_EDUC_AMT)='0' then ' '  ELSE convert(varchar,INM_H_EDUC_AMT)  END    as [IGST VALUE],   CASE WHEN convert(varchar,INM_BE_AMT)='0' then ' '  ELSE convert(varchar,E_BASIC)  END  as [SGST RATE], CASE  WHEN convert(varchar,INM_EDUC_AMT)='0' then ' '  ELSE convert(varchar,E_EDU_CESS)  END   as [CGST RATE],  CASE WHEN   convert(varchar,INM_H_EDUC_AMT)='0' then ' '  ELSE convert(varchar,E_H_EDU)  END   as [IGST RATE], INM_PACK_AMT AS [Packing Amount], INM_FREIGHT AS [Freight Amount],  INM_OTHER_AMT AS [Others Amount],INM_G_AMT as [INVOICE VALUE], 'INR' as [Currency],'' as [ROAD PERMIT NO],'' as [57F4 NUMBER],'' as [57F4 NO DATE],CM_GST_NO  as [GSTIN number],INM_VEH_NO as [Vehicle Number],'' as [DRG REV Level],'' as [COP Certificate] ,'' AS [Certificate Date],P_LBT_NO AS[TML GSTIN],'' AS[Field1],'' AS[Field2],'' AS[Field3],'' AS[Field4],'' AS[Field5] FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,ITEM_MASTER,COMPANY_MASTER,PARTY_MASTER,CUSTPO_DETAIL,EXCISE_TARIFF_MASTER  WHERE INM_CODE=IND_INM_CODE AND IND_CPOM_CODE=CPOM_CODE AND IND_I_CODE=I_CODE AND IND_CPOM_CODE=CPOM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=IND_I_CODE AND INM_CM_CODE=CM_CODE AND P_CODE=INM_P_CODE AND I_E_CODE=E_CODE AND INM_CODE in (" + InvCodes + ")   ORDER BY INM_TNO  ");
                
                if (dtExport.Rows.Count > 0)
                {
                    using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                    {
                        writer.WriteLine(dtExport.Columns[0].ColumnName.ToString()
                            + ',' + dtExport.Columns[1].ColumnName.ToString()
                            + ',' + dtExport.Columns[2].ColumnName.ToString()
                            + ',' + dtExport.Columns[3].ColumnName.ToString()
                            + ',' + dtExport.Columns[4].ColumnName.ToString()
                            + ',' + dtExport.Columns[5].ColumnName.ToString()
                            + ',' + dtExport.Columns[6].ColumnName.ToString()
                            + ',' + dtExport.Columns[7].ColumnName.ToString()
                            + ',' + dtExport.Columns[8].ColumnName.ToString()
                            + ',' + dtExport.Columns[9].ColumnName.ToString()
                            + ',' + dtExport.Columns[10].ColumnName.ToString()
                            + ',' + dtExport.Columns[11].ColumnName.ToString()
                            + ',' + dtExport.Columns[12].ColumnName.ToString()
                            + ',' + dtExport.Columns[13].ColumnName.ToString()
                            + ',' + dtExport.Columns[14].ColumnName.ToString()
                            + ',' + dtExport.Columns[15].ColumnName.ToString()
                            + ',' + dtExport.Columns[16].ColumnName.ToString()
                            + ',' + dtExport.Columns[17].ColumnName.ToString()
                            + ',' + dtExport.Columns[18].ColumnName.ToString()
                            + ',' + dtExport.Columns[19].ColumnName.ToString()
                            + ',' + dtExport.Columns[20].ColumnName.ToString()
                            + ',' + dtExport.Columns[21].ColumnName.ToString()
                            + ',' + dtExport.Columns[22].ColumnName.ToString()
                            + ',' + dtExport.Columns[23].ColumnName.ToString()
                            + ',' + dtExport.Columns[24].ColumnName.ToString()
                            + ',' + dtExport.Columns[25].ColumnName.ToString()
                            + ',' + dtExport.Columns[26].ColumnName.ToString()
                            + ',' + dtExport.Columns[27].ColumnName.ToString()
                            + ',' + dtExport.Columns[28].ColumnName.ToString()
                            + ',' + dtExport.Columns[29].ColumnName.ToString()
                            + ',' + dtExport.Columns[30].ColumnName.ToString()
                            + ',' + dtExport.Columns[31].ColumnName.ToString()
                            + ',' + dtExport.Columns[32].ColumnName.ToString()
                            + ',' + dtExport.Columns[33].ColumnName.ToString()
                            + ',');
                    }
                    for (int i = 0; i < dtExport.Rows.Count; i++)
                    {
                        using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                        {
                            writer.WriteLine(dtExport.Rows[i]["PoNumber"].ToString()
                                + ',' + dtExport.Rows[i]["PO Item No"].ToString()
                                + ',' + dtExport.Rows[i]["Quantity"].ToString()
                                + ',' + dtExport.Rows[i]["Vendor Challan No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["Challan Date"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Gross Rate"].ToString()
                                + ',' + dtExport.Rows[i]["Net P O RATE"].ToString()
                                + ',' + dtExport.Rows[i]["Basic value"].ToString()
                                + ',' + dtExport.Rows[i]["Taxable Value"].ToString()
                                + ',' + dtExport.Rows[i]["SGST VALUE"].ToString()
                                + ',' + dtExport.Rows[i]["CGST VALUE"].ToString()
                                + ',' + dtExport.Rows[i]["IGST VALUE"].ToString()
                                + ',' + dtExport.Rows[i]["SGST RATE"].ToString()
                                + ',' + dtExport.Rows[i]["CGST RATE"].ToString()
                                + ',' + dtExport.Rows[i]["IGST RATE"].ToString()
                                + ',' + dtExport.Rows[i]["Packing Amount"].ToString()
                                + ',' + dtExport.Rows[i]["Freight Amount"].ToString()
                                + ',' + dtExport.Rows[i]["Others Amount"].ToString()
                                + ',' + dtExport.Rows[i]["INVOICE VALUE"].ToString()
                                + ',' + dtExport.Rows[i]["Currency"].ToString()
                                + ',' + dtExport.Rows[i]["ROAD PERMIT NO"].ToString()
                                + ',' + dtExport.Rows[i]["57F4 NUMBER"].ToString()
                                + ',' + dtExport.Rows[i]["57F4 NO DATE"].ToString()
                                + ',' + dtExport.Rows[i]["GSTIN number"].ToString()
                                + ',' + dtExport.Rows[i]["Vehicle Number"].ToString()
                                + ',' + dtExport.Rows[i]["DRG REV Level"].ToString()
                                + ',' + dtExport.Rows[i]["COP Certificate"].ToString()
                                 + ',' + dtExport.Rows[i]["Certificate Date"].ToString()
                                  + ',' + dtExport.Rows[i]["TML GSTIN"].ToString()
                                   + ',' + dtExport.Rows[i]["Field1"].ToString()
                                    + ',' + dtExport.Rows[i]["Field2"].ToString()
                                     + ',' + dtExport.Rows[i]["Field3"].ToString()
                                      + ',' + dtExport.Rows[i]["Field4"].ToString()
                                       + ',' + dtExport.Rows[i]["Field5"].ToString()
                                + ',');
                        }
                    }
                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(Server.MapPath(@"~/UpLoadPath/ASN.csv"), System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "ASN.csv");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();
                }
            }
            else if (ddlInvType.SelectedIndex == 2)
            {
                DataTable dtExport = CommonClasses.Execute(" SELECT  CPOM_PONO as [PO/SA Number],IND_SR_NO as [Item Sr No],I_CODENO as [Part No],IND_INQTY as [ASN Quantity],INM_TNO as [Invoice No],INM_DATE as [Invoice Date (dd.mm.yyyy)],INM_G_AMT as [Invoice Amount],' ' as [Excise Amount],'NA' as [LR No],INM_LR_DATE as [LR Date (dd.mm.yyyy)],INM_VEH_NO as [Vehicle No], round( IND_RATE*IND_INQTY,2) as [Material Base Price]    ,'' AS Mat1,'' AS 	Qty1,'' AS 	Mat2,	'' AS Qty2,'' AS 	Mat3,'' AS 	Qty3 FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,ITEM_MASTER  WHERE INM_CODE=IND_INM_CODE AND IND_CPOM_CODE=CPOM_CODE AND IND_I_CODE=I_CODE  AND INM_CODE in (" + InvCodes + ")    ORDER BY INM_TNO");
                if (dtExport.Rows.Count > 0)
                {
                    using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                    {
                        writer.WriteLine(dtExport.Columns[0].ColumnName.ToString()
                            + ',' + dtExport.Columns[1].ColumnName.ToString()
                            + ',' + dtExport.Columns[2].ColumnName.ToString()
                            + ',' + dtExport.Columns[3].ColumnName.ToString()
                            + ',' + dtExport.Columns[4].ColumnName.ToString()
                            + ',' + dtExport.Columns[5].ColumnName.ToString()
                            + ',' + dtExport.Columns[6].ColumnName.ToString()
                            + ',' + dtExport.Columns[7].ColumnName.ToString()
                            + ',' + dtExport.Columns[8].ColumnName.ToString()
                            + ',' + dtExport.Columns[9].ColumnName.ToString()
                            + ',' + dtExport.Columns[10].ColumnName.ToString()
                            + ',' + dtExport.Columns[11].ColumnName.ToString()
                            + ',' + dtExport.Columns[12].ColumnName.ToString()
                            + ',' + dtExport.Columns[13].ColumnName.ToString()
                            + ',' + dtExport.Columns[14].ColumnName.ToString()
                            + ',' + dtExport.Columns[15].ColumnName.ToString()
                            + ',' + dtExport.Columns[16].ColumnName.ToString()
                            + ',' + dtExport.Columns[17].ColumnName.ToString()
                            + ',');
                    }
                    for (int i = 0; i < dtExport.Rows.Count; i++)
                    {
                        using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                        {
                            writer.WriteLine(dtExport.Rows[i]["PO/SA Number"].ToString()
                                + ',' + "10"
                                + ',' + dtExport.Rows[i]["Part No"].ToString()
                                + ',' + dtExport.Rows[i]["ASN Quantity"].ToString()
                                + ',' + dtExport.Rows[i]["Invoice No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["Invoice Date (dd.mm.yyyy)"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Invoice Amount"].ToString()
                                + ',' + dtExport.Rows[i]["Excise Amount"].ToString()
                                + ',' + dtExport.Rows[i]["LR No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["LR Date (dd.mm.yyyy)"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Vehicle No"].ToString()
                                + ',' + dtExport.Rows[i]["Material Base Price"].ToString()
                                + ',' + dtExport.Rows[i]["Mat1"].ToString()
                                + ',' + dtExport.Rows[i]["Qty1"].ToString()
                                + ',' + dtExport.Rows[i]["Mat2"].ToString()
                                + ',' + dtExport.Rows[i]["Qty2"].ToString()
                                + ',' + dtExport.Rows[i]["Mat3"].ToString()
                                + ',' + dtExport.Rows[i]["Qty3"].ToString()
                                + ',');
                        }
                    }
                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(Server.MapPath(@"~/UpLoadPath/ASN.csv"), System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "ASN.csv");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();
                }
            }
        }
        catch (Exception)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
    }

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvDetails.DataSource = dtFilter;
                dgInvDetails.DataBind();
                dgInvDetails.Enabled = false;
            }
        }
        else
        {
            dgInvDetails.Enabled = true;
        }
    }
    #endregion
}
