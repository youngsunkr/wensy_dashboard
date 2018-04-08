using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicePoint.LogIn
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Lib.Util.RemoveCookie(Response, "MemberNum");
            Lib.Util.RemoveCookie(Response, "MemberName");
            Lib.Util.RemoveCookie(Response, "CompanyName");
            Lib.Util.RemoveCookie(Response, "Grade");
            Response.Redirect("/Login/Login.aspx");
        }
    }
}