using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;

namespace ServicePoint.Common.UC.Dashboard.Detail
{
    public partial class SQL_Agent : System.Web.UI.UserControl
    {
        private DB.Cloud cloud;
        public int ServerNum;
        protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();
            RequestQueryString();
            BindData();
        }
        private void RequestQueryString()
        {
            HiddenField hdnfield = (HiddenField)Parent.FindControl("hdn_ServerNum");
            ServerNum = Convert.ToInt32(hdnfield.Value);
        }
        private void BindData()
        {
            DataTable dt = new DataTable();

            cloud.w_SQLAgentFail(ServerNum);
            dt = cloud.dsReturn.Tables[0];
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }
    }
}