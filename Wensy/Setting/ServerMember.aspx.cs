using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicePoint.Lib;

namespace ServicePoint.Setting
{
    public partial class ServerMember : Base
    {
        private int CompanyNum, numServer;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (IsPostBack)
            {
                RequestForm();
            }
            else
            {
                RequestQueryString();
            }
            BindData();
        }
        private void RequestForm()
        {
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
            numServer = Lib.Util.TConverter<int>(hdn_ServerNum.Value);
        }
        private void RequestQueryString()
        {
            numServer = 0;
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
            if (Request.QueryString.AllKeys.Contains("ServerNum"))
                numServer = Util.TConverter<int>(Request.QueryString["ServerNum"]);
            hdn_ServerNum.Value = numServer.ToString();

        }
        private void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.m_tbServers_List(CompanyNum);
            gv_List.DataSource = cloud.dsReturn.Tables[0];
            gv_List.DataBind();

            if (numServer != 0)
            {
                pnl_View.Visible = true;
                nReturn = cloud.m_tbServers_Member_List(CompanyNum, numServer);
                gv_List_Selected.DataSource = cloud.dsReturn.Tables[0];
                gv_List_Selected.DataBind();
                nReturn = cloud.m_tbServers_Member_List_NotExist(CompanyNum, numServer);
                gv_List_UnSelected.DataSource = cloud.dsReturn.Tables[0];
                gv_List_UnSelected.DataBind();
            }
        }

        protected void gv_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            int tmp_ServerNum = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ServerNum"));
            if (tmp_ServerNum == numServer)
                e.Row.CssClass = "alert-info";
        }
    }
}