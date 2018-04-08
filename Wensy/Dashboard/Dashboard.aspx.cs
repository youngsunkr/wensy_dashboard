using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace ServicePoint.Dashboard
{
    public partial class Dashboard : Base
    {
        #region Member
        protected DataTable dt_Alert;
        public string strTest;
        public string serverType;
        protected int ServerNum;
        #endregion

        override protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
            base.Page_Load(sender, e);
            RequestQueryString();
            InitControl();
            //BindData();
            //AlertList.dt_AlertList = dt_Alert;
           
        }
        private void Page_PreRender(object sender, EventArgs e)
        {
        }
        private void RequestQueryString()
        {
            if (!Request.QueryString.AllKeys.Contains("ServerType"))
                serverType = "Windows";
            else
                serverType = Request.QueryString["ServerType"].ToString();

            if (Request.QueryString.AllKeys.Contains("ServerNum"))
                ServerNum = Convert.ToInt32(Request.QueryString["ServerNum"]);
            else
                ServerNum = 0;
        }

        private void InitControl()
        {
            phd.Controls.Clear();
            if (serverType.ToUpper() == "WINDOWS")
            {
                Common.UC.Dashboard.WindowDashboard phdControl = (Common.UC.Dashboard.WindowDashboard)Page.LoadControl("/Common/UC/Dashboard/WindowDashboard.ascx");
                phdControl.ServerNum = ServerNum;
                phd.Controls.Add(phdControl);
            }
            if (serverType.ToUpper() == "SQL")
            {
                Common.UC.Dashboard.SqlDashboard phdControl = (Common.UC.Dashboard.SqlDashboard)Page.LoadControl("/Common/UC/Dashboard/SQLDashboard.ascx");
                phdControl.ServerNum = ServerNum;

                phd.Controls.Add(phdControl);
            }
            if (serverType.ToUpper() == "WEB")
            {
                Common.UC.Dashboard.WebDashboard phdControl = (Common.UC.Dashboard.WebDashboard)Page.LoadControl("/Common/UC/Dashboard/WEBDashboard.ascx");
                phdControl.ServerNum = ServerNum;
                phd.Controls.Add(phdControl);
            }

            tmr.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PageRefreshTime"]);
        }
        private void BindData()
        {
            dt_Alert = new DataTable();
            DB.Cloud cloud = new DB.Cloud();
            //int nReturn = cloud.W_AlertCountList_Server(ServerNum, Convert.ToInt32(ConfigurationManager.AppSettings["ChartDataDuration"]));
            int nReturn = cloud.get_AlertMessage(ServerNum.ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["ChartDataDuration"]));

            //신규추가 2017 - 09 - 23
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
                DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
                DataTable dt_struct = new DataTable();
                dt_Alert = tester.Clone();
                dt_Alert.Columns.Add("RepeatCnt");
                dt_Alert.AcceptChanges();
                //이후 dt_struct 에 계속 merge(union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

                    //신규추가 2017 - 09 - 23 데이터 머지 = mssql union
                    //신규추가 2017 - 09 - 23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
                    dt_Alert.Merge(dt_tmp);
                }
            }

        }
    }
}