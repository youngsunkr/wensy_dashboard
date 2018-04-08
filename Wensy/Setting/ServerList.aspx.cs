using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicePoint.Lib;

namespace ServicePoint.Setting
{
    public partial class ServerList : Base
    {
        private int CompanyNum;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (IsPostBack)
                RequestForm();
            else
                RequestQueryString();
            BindData();
        }
        private void RequestQueryString()
        {
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
        }
        private void RequestForm()
        {
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
        }
        private void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.m_tbServers_List(CompanyNum);
            gv_List.DataSource = cloud.dsReturn.Tables[0];
            gv_List.DataBind();
        }
    }
}