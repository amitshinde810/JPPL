using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
public partial class RoportForms_ADD_MaterialRequisitionRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());

            bool ChkAllitem = Convert.ToBoolean(Request.QueryString[2].ToString());

            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();

            string i_name = Request.QueryString[5].ToString();
            string group = Request.QueryString[6].ToString();
            string way = Request.QueryString[7].ToString();

            i_name = i_name.Replace("'", "''");


            #region Detail
            if (way == "Direct")
            {
                if (group == "Datewise")
                {

                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }

                }
                if (group == "ItemWise")
                {

                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }

            }
            #endregion

            #region Summary
            if (way == "AsperReq")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }


            }
            #endregion

            #region AsperBatch
            if (way == "AsperBatch")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }


            }
            #endregion

            #region Detail
            if (way == "All")
            {
                if (group == "Datewise")
                {

                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }

                }
                if (group == "ItemWise")
                {

                    if (ChkAllDate == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group, way);
                    }
                    if (ChkAllDate != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", From, To, i_name, group, way);
                    }
                    if (ChkAllDate == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);
                    }
                }

            }
            #endregion

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string item, string From, string To, string i_name, string group, string way)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            if (way == "Direct")
            {
                Query = "select  I1.I_CODE,MR_CODE,MR_BATCH_NO,MR_DATE,MR_TYPE,MR_CPOM_CODE,MR_I_CODE,MRD_I_CODE,MRD_REQ_QTY  as BD_VQTY,MR_DEPT_NAME,MRD_ISSUE_QTY,MRD_PROD_QTY,MRD_PURC_REQ_QTY,I1.I_NAME as FINISH_NAME,I1.I_CODENO as FINISH_I_CODENO,I2.I_NAME as SUB_NAME,I2.I_CODENO as RAW_I_CODENO from  MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER as I1,ITEM_MASTER I2 where MR_CODE=MRD_MR_CODE and MR_I_CODE=I1.I_CODE and MRD_I_CODE=I2.I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='Direct'";
               // Query = "with t1 as ( select I_CODE,BM_CODE,BM_I_CODE,I_NAME,MR_TYPE,MR_DATE,MR_DEPT_NAME,MR_BATCH_NO,MATERIAL_REQUISITION_MASTER.ES_DELETE from BOM_DETAIL,BOM_MASTER,ITEM_MASTER, MATERIAL_REQUISITION_MASTER where BOM_MASTER.BM_I_CODE=I_CODE and MR_I_CODE=I_CODE),t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,MRD_REQ_QTY,I_CODENO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER,MATERIAL_REQUISITION_DETAIL where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0  )select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME  as SUB_NAME,t2.MRD_REQ_QTY as BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t2.I_CODENO from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE and t1.MR_TYPE='Direct' and t1.ES_DELETE=0  ";

            }
            if (way == "AsperReq")
            {
                Query = "select  I1.I_CODE,MR_CODE,MR_BATCH_NO,MR_DATE,MR_TYPE,MR_CPOM_CODE,MR_I_CODE,MRD_I_CODE,MRD_REQ_QTY  as BD_VQTY,MR_DEPT_NAME,MRD_ISSUE_QTY,MRD_PROD_QTY,MRD_PURC_REQ_QTY,I1.I_NAME as FINISH_NAME,I1.I_CODENO as FINISH_I_CODENO,I2.I_NAME as SUB_NAME,I2.I_CODENO as RAW_I_CODENO from  MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER as I1,ITEM_MASTER I2 where MR_CODE=MRD_MR_CODE and MR_I_CODE=I1.I_CODE and MRD_I_CODE=I2.I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='As Per Order'";
                //Query = "  with t1 as ( select I_CODE,BM_CODE,BM_I_CODE,I_NAME,MR_TYPE,MR_DATE,MR_DEPT_NAME,MR_BATCH_NO,MATERIAL_REQUISITION_MASTER.ES_DELETE,CPOM_PONO,CPOM_CODE,MR_CPOM_CODE from BOM_DETAIL,BOM_MASTER,ITEM_MASTER, MATERIAL_REQUISITION_MASTER,CUSTPO_MASTER where BOM_MASTER.BM_I_CODE=I_CODE and MR_I_CODE=I_CODE),t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,BD_VQTY,I_CODENO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0  )select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME  as SUB_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE and t1.MR_TYPE='As Per Order'and t1.ES_DELETE=0 and t1.CPOM_CODE=MR_CPOM_CODE   ";

            }

            if (way == "AsperBatch")
            {
                Query = "select  I1.I_CODE,MR_CODE,MR_BATCH_NO,MR_DATE,MR_TYPE,MR_CPOM_CODE,MR_I_CODE,MRD_I_CODE,MRD_REQ_QTY  as BD_VQTY,MR_DEPT_NAME,MRD_ISSUE_QTY,MRD_PROD_QTY,MRD_PURC_REQ_QTY,I1.I_NAME as FINISH_NAME,I1.I_CODENO as FINISH_I_CODENO,I2.I_NAME as SUB_NAME,I2.I_CODENO as RAW_I_CODENO from  MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER as I1,ITEM_MASTER I2 where MR_CODE=MRD_MR_CODE and MR_I_CODE=I1.I_CODE and MRD_I_CODE=I2.I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MR_TYPE='As Per Batch'";
                //Query = "  with t1 as ( select I_CODE,BM_CODE,BM_I_CODE,I_NAME,MR_TYPE,MR_DATE,MR_DEPT_NAME,MR_BATCH_NO,MATERIAL_REQUISITION_MASTER.ES_DELETE,CPOM_PONO,CPOM_CODE,MR_CPOM_CODE from BOM_DETAIL,BOM_MASTER,ITEM_MASTER, MATERIAL_REQUISITION_MASTER,CUSTPO_MASTER where BOM_MASTER.BM_I_CODE=I_CODE and MR_I_CODE=I_CODE),t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,BD_VQTY,I_CODENO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0  )select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME  as SUB_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE and t1.MR_TYPE='As Per Order'and t1.ES_DELETE=0 and t1.CPOM_CODE=MR_CPOM_CODE   ";

            }
            //if (way == "All")
            //{
            //    Query = "  with t1 as ( select distinct(I_CODE) as I_CODE,BM_CODE,BM_I_CODE,I_NAME,MR_TYPE,MR_DATE,MR_DEPT_NAME, MR_BATCH_NO,MATERIAL_REQUISITION_MASTER.ES_DELETE,CPOM_PONO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER, MATERIAL_REQUISITION_MASTER,CUSTPO_MASTER where BOM_MASTER.BM_I_CODE=I_CODE and  MR_I_CODE=I_CODE),t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,BD_VQTY from  BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0  )select t1.I_CODE,t1.I_NAME as FINISH_NAME,t2.I_NAME as SUB_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE  and t1.ES_DELETE=0  ";

            //}

            #region Detail
            if (way == "Direct")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";

                    }
                   // Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t2.I_CODENO";
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";
                    }
                   // Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t2.I_CODENO";
                }
                #endregion

            }
            #endregion

            #region AsPerReq
            if (way == "AsperReq")
            {
                #region Datewise
                if (group == "Datewise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";

                    }
                    //Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO";
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";

                    }
                   // Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO";
                }
                #endregion


            }
            #endregion

            #region AsPerBatch
            if (way == "AsperBatch")
            {
                #region Datewise
                if (group == "Datewise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";

                    }
                    //Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO";
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and MR_I_CODE='" + i_name + "'";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and MR_I_CODE='" + i_name + "'";

                    }
                    // Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.MRD_REQ_QTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO,t2.I_CODENO";
                }
                #endregion


            }
            #endregion

            //#region All
            //if (way == "All")
            //{
            //    #region Datewise
            //    if (group == "Datewise")
            //    {

            //        if (date1 == "All" && item == "All")
            //        {
            //            Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO ";


            //        }
            //        if (date1 != "All" && item != "All")
            //        {
            //            Query = Query + " and t1.MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.BM_I_CODE='" + i_name + "' group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO   ";
            //        }
            //        if (date1 != "All" && item == "All")
            //        {
            //            Query = Query + " and t1.MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO  ";

            //        }
            //        if (date1 == "All" && item != "All")
            //        {
            //            Query = Query + " and t1.BM_I_CODE='" + i_name + "'group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO   ";

            //        }

            //    }
            //    #endregion
            //    #region ItemWise
            //    if (group == "ItemWise")
            //    {
            //        if (date1 == "All" && item == "All")
            //        {
            //            Query = Query + " group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO ,t1.CPOM_PONO ";


            //        }
            //        if (date1 != "All" && item != "All")
            //        {
            //            Query = Query + " and t1.MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and t1.BM_I_CODE='" + i_name + "' group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO ,t1.CPOM_PONO   ";
            //        }
            //        if (date1 != "All" && item == "All")
            //        {
            //            Query = Query + " and t1.MR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO ,t1.CPOM_PONO ";

            //        }
            //        if (date1 == "All" && item != "All")
            //        {
            //            Query = Query + " and t1.BM_I_CODE='" + i_name + "' group by t1.I_CODE,t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t1.MR_TYPE,t1.MR_DATE,t1.MR_DEPT_NAME,t1.MR_BATCH_NO,t1.CPOM_PONO  ";

            //        }

            //    }
            //    #endregion

            //}
            //#endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/MaterialRequisitionDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/MaterialRequisitionDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    //if (way == "AsPerReg")
                    //{
                    //    rptname.SetParameterValue("txtType", "1");

                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtType", "0");

                    //}
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/MaterialRequisitionItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/MaterialRequisitionItemWise.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    //if (way == "Summary")
                    //{
                    //    rptname.SetParameterValue("txtType", "1");

                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtType", "0");

                    //}
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMaterialRequisitionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition  Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

