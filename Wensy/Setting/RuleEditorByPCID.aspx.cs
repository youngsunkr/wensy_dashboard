using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ServicePoint.Lib;
namespace ServicePoint.Setting
{
    public partial class RuleEditorByPCID : Base
    {
        private int CompanyNum, numServer, nReturn;
        private string strPObjectName, strPCID;
        private DataTable dt;
        private DB.Cloud cloud;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            cloud = new DB.Cloud();
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
            numServer = Lib.Util.TConverter<int>(ddl_Server.SelectedValue);
            strPObjectName = Lib.Util.TConverter<string>(ddl_Object.SelectedValue);
            strPCID = Lib.Util.TConverter<string>(ddl_Counter.SelectedValue);
        }
        private void RequestQueryString()
        {
            numServer = 0;
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
            if (Request.QueryString.AllKeys.Contains("ServerNum"))
                numServer = Util.TConverter<int>(Request.QueryString["ServerNum"]);

        }
        private void BindData()
        {
            if (numServer == 0)
            {
                nReturn = cloud.m_tbServers_List(CompanyNum);
                dt = cloud.dsReturn.Tables[0];
                SortedList<int, string> list = new SortedList<int, string>();
                list.Add(0, "=Server=");
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(Convert.ToInt32(dr["ServerNum"]), dr["DisplayName"].ToString());
                }
                ddl_Server.DataSource = list;
                ddl_Server.DataValueField = "Key";
                ddl_Server.DataTextField = "Value";
                ddl_Server.DataBind();
            }
        }

        protected void ddl_Server_SelectedIndexChanged(object sender, EventArgs e)
        {
            nReturn = cloud.m_tbPCID_Server_PObject_List(numServer);
            dt = cloud.dsReturn.Tables[0];
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add(".", "=Object=");
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(Convert.ToString(dr["PObjectName"]), dr["PObjectName"].ToString());
            }
            ddl_Object.DataSource = list;
            ddl_Object.DataValueField = "Key";
            ddl_Object.DataTextField = "Value";
            ddl_Object.DataBind();
        }

        protected void ddl_Object_SelectedIndexChanged(object sender, EventArgs e)
        {
            nReturn = cloud.m_tbPCID_Server_PCounterName_List(numServer, strPObjectName);

            dt = cloud.dsReturn.Tables[0];
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add("%", "=Counter=");
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(Convert.ToString(dr["PCID"]), dr["PCounterName"].ToString());
            }
            ddl_Counter.DataSource = list;
            ddl_Counter.DataValueField = "Key";
            ddl_Counter.DataTextField = "Value";
            ddl_Counter.DataBind();
        }

        protected void ddl_Counter_SelectedIndexChanged(object sender, EventArgs e)
        {
            nReturn = cloud.m_tbPInstance_Server_PInstance_List(numServer, strPCID);

            dt = cloud.dsReturn.Tables[0];

            gv_List.DataSource = dt;
            gv_List.DataBind();

            nReturn = cloud.m_tbAlertRules_Server_List(numServer, strPCID);
            dt = cloud.dsReturn.Tables[0];

            gv_List_Rule.DataSource = dt;
            gv_List_Rule.DataBind();


        }
    }
}