using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace ServicePoint.OverView
{
    public partial class WebOverView : Base
    {
        #region Member
        private DB.Cloud cloud;
        public DataTable dt_PerfmonValue, dt_w_Dashboard, dt_w_Dashboard_chart, dt_FreeDisk, dt_Alert;
        public int numOverViewCnt;
        #endregion
        // 자바스크립트에 배열로 데이터를 만들어줘야하기때문에
        // 챠트의 데이터를 각각의 템플릿필드 헤더값으로 패핑해서 뿌려주게 만듬 dictionary<헤더값,챠트데이터배열>
        override protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();
            litScript.Text = "";
            base.Page_Load(sender, e);
            InitControl();
            BindData();
            base.ReDirect();

            //AlertSlide.Company = CompanyNum;
            //AlertSlide.Member = MemberNum;
            //AlertSlide.dt_AlertList = dt_Alert;
            //AlertList.Company = CompanyNum;
            //AlertList.Member = MemberNum;
            //AlertList.dt_AlertList = dt_Alert;

        }
        private void Page_PreRender(object sender, EventArgs e)
        {
            dt_w_Dashboard.Dispose();
            dt_w_Dashboard_chart.Dispose();
            dt_FreeDisk.Dispose();
            dt_Alert.Dispose();
        }
        private void Page_LoadComplete(object sender, EventArgs e)
        {
        }
        #region Request
        public void RequestForm()
        { }
        public void RequestQueryString()
        { }
        #endregion
        #region Control
        private void InitControl()
        {
            tmr.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PageRefreshTime"]);
        }
        #endregion
        #region Bind
        public void BindData()
        {
            dt_Alert = new DataTable();

            //서버리스트 읽어오기
            string MyServerList = "";
            cloud.SetCmd("Cloud");
            cloud.get_MyServerList(MemberNum);

            foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            {
                MyServerList += dr["ServerNum"].ToString() + ",";
            }

            MyServerList = MyServerList.Substring(0, MyServerList.Length - 1);

            int nReturn = cloud.get_ServerStatus(MyServerList);
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                MyServerList = "";

                foreach (DataRow dr in cloud.dsReturn.Tables[0].Select("ServerType = 'Web'"))
                {
                    MyServerList += dr["ServerNum"].ToString() + ",";
                }

                MyServerList = MyServerList.Substring(0, MyServerList.Length - 1);
            }

            nReturn = cloud.get_Dashboard(MyServerList); ;
            dt_w_Dashboard = Lib.ConvertingProc.w_Dashboard(cloud.dsReturn.Tables[0]);
            gvList.DataSource = dt_w_Dashboard;
            
            nReturn = nReturn = cloud.get_Dashboard_Chart(MyServerList);
            dt_w_Dashboard_chart = Lib.ConvertingProc.ChangeDashboardColumnName(cloud.dsReturn.Tables[0]);

            //각 차트에서 참조해서 사용할 공통 데이터 테이블 생성
            nReturn = cloud.get_PerfvaluesData_Company(CompanyNum);

            //신규추가 2017-09-23 
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
                DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
                //DataTable dt_struct = new DataTable();
                dt_PerfmonValue = tester.Clone();
                dt_PerfmonValue.AcceptChanges();
                // 이후 dt_struct 에 계속 merge (union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

                    //신규추가 2017-09-23 데이터 머지 = mssql union 
                    //신규추가 2017-09-23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
                    dt_PerfmonValue.Merge(dt_tmp);
                }
            }

            //dt_FreeDisk 데이터 테이블 만들기
            var test = from r in dt_PerfmonValue.AsEnumerable()
                       where (r.Field<string>("PCID") == "P018" || r.Field<string>("PCID") == "P164" || r.Field<string>("PCID") == "P015" || r.Field<string>("PCID") == "P190" || r.Field<string>("PCID") == "P194") && !r.Field<string>("InstanceName").Contains("_Total") && !r.Field<string>("InstanceName").Contains("HarddiskVolume")
                       select r;

            dt_FreeDisk = dt_PerfmonValue.Clone();

            foreach (DataRow r in test)
            {
                var newRow = dt_FreeDisk.NewRow();
                newRow.ItemArray = r.ItemArray;
                dt_FreeDisk.Rows.Add(newRow);//I'm doubtful if you need to call this or not
            }

            dt_FreeDisk = Lib.ConvertingProc.SetDiskProc_All(dt_FreeDisk);
            gvList.DataBind();

            //Alert Message 로딩
            //nReturn = cloud.get_AlertMessage(MyServerList, numAlertDataDuration);

            //if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            //{
            //    //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
            //    DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
            //    //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
            //    DataTable dt_struct = new DataTable();
            //    dt_Alert = tester.Clone();
            //    dt_Alert.Columns.Add("RepeatCnt");
            //    dt_Alert.AcceptChanges();
            //    //이후 dt_struct 에 계속 merge(union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
            //    foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            //    {
            //        DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

            //        //신규추가 2017 - 09 - 23 데이터 머지 = mssql union
            //        //신규추가 2017 - 09 - 23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
            //        dt_Alert.Merge(dt_tmp);
            //    }
            //}
            //cloud.dsReturn.Dispose();
        }
        #endregion
        #region Func
        public List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU-Total#c#CPU-Kernel", "", "CPU_LINE_CHART", "LINE", 100, 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("", "ProcessorQueueLength", "", "CPU_QUEUE_COL_CHART", "COL", 32, 30, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "LogicalDiskAvgRead", "", "READTIME_LINE_CHART", "LINE", 100, 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-Total", "", "BYTESTOTALSEC_LINE_CHART", "LINE", ((1024 * 1024) * 100), 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("FreePercent", "Drive", "FreeGB", "FREEDISK_BAR_CHART", "BAR", 100, 150, 100, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IISCurrentConnection", "", "TOTALCONNECTION_LINE_CHART", "LINE", 1000, 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-IISTotal", "", "TOTALBYTESSEC_LINE_CHART", "LINE", ((1024 * 1024) * 100), 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU-W3WP", "", "CPUTIME_W3WP_LINE_CHART", "LINE", 100, 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestsExecuting", "", "REQUESTSEXECUTING_LINE_CHART", "LINE", 100, 100, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestsQueue", "", "REQUESTSQUEUED_COL_CHART", "COL", 100, 30, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestsPerSec", "", "REQUESTSSEC_COL_CHART", "COL", 100, 30, 100));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestExecutionTime", "", "REQUESTEXECUTIONTIME_LINE_CHART", "LINE", 10000, 100, 100));
            return cpList;
        }
        #endregion
        #region Event
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // e.Row.BackColor = Color.FromName("#333333");
                //e.Row.Style.Add("color", "white");
            }
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            int CurruntStatus = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "CurrentStatus"));
            string BgCss = "";
            if (CurruntStatus == 0)
                BgCss = "bg-status-good";
            if (CurruntStatus == 1)
                BgCss = "bg-status-info";
            if (CurruntStatus == 2)
                BgCss = "bg-status-warning";
            if (CurruntStatus == 3)
                BgCss = "bg-status-failure";
            if (DataBinder.Eval(e.Row.DataItem, "AgentStatus").ToString() == "0")
                BgCss = "bg-status-default";

            e.Row.Cells[0].CssClass = BgCss;

            int ServerNum = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ServerNum"));
            DataTable dt = new DataTable();
            DataTable dt_MaxTime = new DataTable();

            var query = from data in dt_w_Dashboard_chart.AsEnumerable()
                        where (int)data["ServerNum"] == ServerNum
                        orderby data["TimeIn"] descending
                        select data;


            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            if (query.Count() > 0)
            {
                dt = query.CopyToDataTable<DataRow>();
                dt.DefaultView.Sort = "TimeIn desc";
                dt = dt.DefaultView.ToTable();
                //라인차트 생성
                cplst = Lib.Flotr2.SetArrayString_Lines(dt, SetChartProperty());
                dt_MaxTime = dt.Clone();
                dt_MaxTime.ImportRow(dt.Rows[0]);
                //dt.DefaultView.Sort = "TimeIn asc";
                //dt = dt.DefaultView.ToTable();
                //Col챠트 생성
                cplst = Lib.Flotr2.SetArrayString_Col(dt_MaxTime, cplst);
            }

            DataTable dt_DiskFree = new DataTable();
            query = from data in dt_FreeDisk.AsEnumerable()
                    where Convert.ToInt32(data["ServerNum"]) == ServerNum
                    orderby data["ServerNum"] ascending
                    select data;

            if (query.Count() > 0)
                dt_DiskFree = query.CopyToDataTable<DataRow>();
            else
            {
                DataTable dtDiskFree_tmp = new DataTable();
                dtDiskFree_tmp.Columns.Clear();
                dtDiskFree_tmp.Columns.Add("Drive", typeof(string));
                dtDiskFree_tmp.Columns.Add("usage", typeof(float));
                dtDiskFree_tmp.Columns.Add("space", typeof(string));
                dtDiskFree_tmp.Columns.Add("remained", typeof(float));
                dt_DiskFree = dtDiskFree_tmp;
            }
            //디스크차트 생성
            cplst = Lib.Flotr2.SetArrayString_Bar(dt_DiskFree, cplst);

            StringBuilder sb = new StringBuilder();
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }

        protected void gvList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // 헤더만 설정
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // 현재 GridView 개체 가져오기ㅣ
                GridView gridView = sender as GridView;

                // 행 만들기
                GridViewRow gridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //gridRow.TableSection = TableRowSection.TableHeader;
                // 셀 만들기
                TableCell tableCell = null;

                //[2] HostName 컬럼 만들기
                tableCell = new TableCell();

                tableCell.Text = " ";
                tableCell.ColumnSpan = 1;
                tableCell.Font.Bold = true;
                //tableCell.RowSpan = 1;
                gridRow.Cells.Add(tableCell);
                //[5] Processor 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "Processor";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 2;
                tableCell.RowSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);
                //[6] Memory 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "Memory";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 2;
                tableCell.RowSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);
                //[7] Disk-System Drive 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "Disk - System Drive";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 2;
                tableCell.RowSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);
                //[8] 데이터 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "Network";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);
                //[3] WWW Service 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "WWW Service";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 4;
                tableCell.RowSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);

                //[4] ASP.NET 컬럼 만들기
                tableCell = new TableCell();
                tableCell.Text = "ASP/ASP.NET";
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                tableCell.ColumnSpan = 4;
                tableCell.RowSpan = 1;
                tableCell.Font.Bold = true;
                gridRow.Cells.Add(tableCell);

                gridRow.RowType = DataControlRowType.Header;
                gridView.Controls[0].Controls.AddAt(0, gridRow);

            }
        }
        #endregion
    }
}