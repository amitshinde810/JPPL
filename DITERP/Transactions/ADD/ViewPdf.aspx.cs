using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_ViewPdf : System.Web.UI.Page
{
    string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (true)
            {
                
            }
            string str=Request.QueryString[0];
            path = ("" + str + ""); 
            IframeViewPDF.Attributes["src"] = path;
        }
    }
}
