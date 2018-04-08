using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicePoint.Lib;
using System.Data;

namespace ServicePoint.Setting
{
    public partial class ProcessInstance : Base
    {
        private int CompanyNum, numServer;
        private string strDisplayName;
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
            strDisplayName = Lib.Util.TConverter<string>(hdn_DisplayName.Value);
            txt_Server.Text = strDisplayName;
            txt_Server.ReadOnly = true;
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

            if (!string.IsNullOrEmpty(txt_InstanceName.Text))
            {
                nReturn = cloud.m_tbPInstance_Server_Add(Util.TConverter<int>(hdn_ServerNum.Value), "P006", txt_InstanceName.Text, true);
                nReturn = cloud.m_tbPInstance_Server_Add(Util.TConverter<int>(hdn_ServerNum.Value), "P014", txt_InstanceName.Text, true);
                nReturn = cloud.m_tbPInstance_Server_Add(Util.TConverter<int>(hdn_ServerNum.Value), "P013", txt_InstanceName.Text, true);
                txt_InstanceName.Text = "";
            }
            if (numServer != 0)
            {
                pnl_Proess.Visible = true;
                pnl_Instance.Visible = true;
                nReturn = cloud.m_tbPCID_Server_PCounterName_List(numServer, "process");
                gv_List_Process.DataSource = cloud.dsReturn.Tables[0];
                gv_List_Process.DataBind();
                DataTable dt = new DataTable();
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    nReturn = cloud.m_tbPInstance_Server_PInstance_List(numServer, dr["PCID"].ToString());
                    DataTable dt_tmp = cloud.dsReturn.Tables[0];
                    dt.Merge(dt_tmp);
                }
                gv_List_Instance.DataSource = dt;
                gv_List_Instance.DataBind();
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