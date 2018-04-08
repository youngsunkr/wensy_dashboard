using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicePoint.Common.UC
{
    public partial class AdminLeft : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string menu = Request.ServerVariables["PATH_INFO"].Split('/')[1].ToLower();

            Panel pan = (Panel)FindControl("pnl_" + menu);
            if (pan != null)
            {
                pan.Visible = true;
            }
        }
    }
}