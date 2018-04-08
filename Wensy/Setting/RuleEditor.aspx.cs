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
    public partial class RuleEditor : Base
    {
        protected int CompanyNum, numServer;
        private DataTable dt_ServerList, dt_AlertOption;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RequestForm();
            InitControl();
            BindData();
            if (!IsPostBack)
                BindControl();

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (gv_List.SortExpression == "" && gv_List.DataSourceID != "")
                gv_List.Sort("Threshold", SortDirection.Descending);
            SetGvHeader_Sort(gv_List);
        }
        #region Request
        private void RequestForm()
        {
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
            numServer = Util.TConverter<int>(ddlList.SelectedValue);
        }
        #endregion
        #region Control
        private void InitControl()
        {
            SortedDictionary<int, int> list = new SortedDictionary<int, int>();
            for (int i = 1; i <= 60; i++)
            {
                list.Add(i, i);
            }
            ddl_Delay.DataSource = list;
            ddl_Delay.DataValueField = "Key";
            ddl_Delay.DataTextField = "Value";
            ddl_Delay.DataBind();

            list.Clear();
            for (int i = 1; i <= 5; i++)
            {
                list.Add(i, i);
            }
            ddl_Cnt.DataSource = list;
            ddl_Cnt.DataValueField = "Key";
            ddl_Cnt.DataTextField = "Value";
            ddl_Cnt.DataBind();

            list.Clear();
            for (int i = 1; i <= 24; i++)
            {
                list.Add(i, i);
            }
            ddl_Reset.DataSource = list;
            ddl_Reset.DataValueField = "Key";
            ddl_Reset.DataTextField = "Value";
            ddl_Reset.DataBind();

        }
        private void BindControl()
        {
            SortedDictionary<int, string> list = new SortedDictionary<int, string>();
            foreach (DataRow dr in dt_ServerList.Rows)
            {
                list.Add(Util.TConverter<int>(dr["ServerNum"]), Util.TConverter<string>(dr["Displayname"]));
            }
            ddlList.DataSource = list;
            ddlList.DataValueField = "Key";
            ddlList.DataTextField = "Value";
            ddlList.DataBind();

            if (dt_AlertOption != null)
            {
                //chk_Use.Checked = Util.TConverter<bool>(dt_AlertOption.Rows[0]["UsePushAlert"]);
                //ddl_Delay.SelectedValue = Util.TConverter<string>(dt_AlertOption.Rows[0]["PushInterval"]);
                //ddl_Cnt.SelectedValue = Util.TConverter<string>(dt_AlertOption.Rows[0]["PushMaxOccurs"]);
                //ddl_Reset.SelectedValue = Util.TConverter<string>(dt_AlertOption.Rows[0]["PushResetInterval"]);
            }
        }
        #endregion



        #region Bind
        private void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.m_tbServers_List(CompanyNum);
            dt_ServerList = cloud.dsReturn.Tables[0];

            nReturn = cloud.m_tbAlertOptions_List(CompanyNum);
            dt_AlertOption = cloud.dsReturn.Tables[0];

            ods_List.TypeName = cloud.GetType().AssemblyQualifiedName;
            ods_List.SelectMethod = "m_tbAlertRules_Server_List_AlertLevel_ods";
            ods_List.SelectParameters.Clear();
            ods_List.SelectParameters.Add("numServer", numServer.ToString());
            gv_List.DataSourceID = "ods_List";

            //nReturn = cloud.m_tbAlertRules_Server_List_AlertLevel(numServer);
            //gv_List.DataSource = cloud.dsReturn.Tables[0];
            //gv_List.DataBind();
        }

        #endregion

    }
}