<%@ WebService Language="C#" Class="test" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Collections.Generic;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class test  : System.Web.Services.WebService {

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]
    public List<string> CheckUser(string useroremail)
    {
        string retval = "";
        DataTable dtUserCheck = new DataTable();
        dtUserCheck = CommonClasses.Execute("select I_CODE, I_CODENO,I_NAME from ITEM_MASTER where I_CODENO like '%" + useroremail + "%'");
        List<String> Item = new List<string>();
        if (dtUserCheck.Rows.Count > 0)
        {
            foreach (DataRow dritem in dtUserCheck.Rows)
            {
                Item.Add(string.Format("{0}-{1}", dritem["I_CODENO"], dritem["I_NAME"]));
            }
        }
        else
        {

        }

        return Item;
    }
}

