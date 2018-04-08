using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ServicePoint
{
    public partial class PopChart : System.Web.UI.Page
    {
        string strCtName;

        protected void Page_Load(object sender, EventArgs e)
        {
            litScript.Text = "";
            requestQueryString();
            SetJscript();

        }
        private void requestQueryString()
        {
            strCtName = Request.QueryString["ct_name"].ToString();
            ct_name.Value = Request.QueryString["ct_name"].ToString();

        }
        private void SetJscript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var datavalue= window.opener." + strCtName + ";");
           // sb.Append("alert('헤헤헤');");
            sb.Append("func_Line_Report(document.getElementById('grp_LINE_CHART'),datavalue,0,0,400,true,true,true,true,true,true);");
            litScript.Text = Lib.Util.BoxingScript(sb.ToString());

        }
    }
}