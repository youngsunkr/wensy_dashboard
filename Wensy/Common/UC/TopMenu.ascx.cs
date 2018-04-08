using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicePoint.Common.UC
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
        public string strUserEmail;
        protected void Page_Load(object sender, EventArgs e)
        {
            strUserEmail =HttpUtility.UrlDecode(ServicePoint.Lib.Util.GetCookieValue(Request, "Email")).ToString();
        }
    }
}