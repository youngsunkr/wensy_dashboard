using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace ServicePoint.Report
{
    public partial class SQL_Lock : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_FreeDisk, dt_Avgwaintime, dt_LockRequest, dt_LockTimeoutTimeout, dt_LockTimeout, dt_LockWaitTimeMs, dt_LockWaitTimeSec, dt_NumberofDeadlock;
        private string strColumn_AVGWAITTIME, strColumn_LockTimeOut, strColumn_LockTimeOutTimeOut, strColumn_LockWaitTimeMs, strColumn_LockWaitSec, strColumn_NumberofDeadlock, strColumn_LockRequest;
        protected int ServerNum;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            cloud = new DB.Cloud();
            litScript.Text = "";
            if (!IsPostBack)
            {
                InitControl();
                BindControl();
                RequestQueryString();
            }
            else
            {
                RequestForm();
                BindData();
            }
        }
        private void RequestQueryString()
        {
            int numHour = DateTime.Now.Hour;
            int numMin = DateTime.Now.Minute;
            txt_dtmStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txt_dtmEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ddl_Min_Start.SelectedValue = numMin.ToString();
            ddl_Min_End.SelectedValue = numMin.ToString();
            ddl_Hour_Start.SelectedValue = (numHour - 1).ToString();
            ddl_Hour_End.SelectedValue = numHour.ToString();

        }
        private void RequestForm()
        {
            ServerNum = Lib.Util.TConverter<int>(ddl_Server.SelectedValue);
            dtmStart = Convert.ToDateTime(txt_dtmStart.Text).AddHours(Lib.Util.TConverter<int>(ddl_Hour_Start.SelectedValue)).AddMinutes(Lib.Util.TConverter<int>(ddl_Min_Start.SelectedValue));
            dtmEnd = Convert.ToDateTime(txt_dtmEnd.Text).AddHours(Lib.Util.TConverter<int>(ddl_Hour_End.SelectedValue)).AddMinutes(Lib.Util.TConverter<int>(ddl_Min_End.SelectedValue));

            //UTC 변환
            DateTime dtmStart_UTC = dtmStart.ToUniversalTime();
            DateTime dtmEnd_UTC = dtmEnd.ToUniversalTime();
            dtmStart = dtmStart_UTC;
            dtmEnd = dtmEnd_UTC;
        }
        private void InitControl()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            for (int i = 1; i <= 24; i++)
            {
                list.Add(i.ToString(), i.ToString() + "시간");
            }
            ddl_Time.DataSource = list;
            ddl_Time.DataValueField = "Key";
            ddl_Time.DataTextField = "Value";
            ddl_Time.DataBind();
            list = new Dictionary<string, string>();
            for (int i = 0; i <= 24; i++)
            {
                list.Add(i.ToString(), i.ToString() + "시");
            }
            ddl_Hour_Start.DataSource = list;
            ddl_Hour_Start.DataValueField = "Key";
            ddl_Hour_Start.DataTextField = "Value";
            ddl_Hour_Start.DataBind();
            ddl_Hour_End.DataSource = list;
            ddl_Hour_End.DataValueField = "Key";
            ddl_Hour_End.DataTextField = "Value";
            ddl_Hour_End.DataBind();

            list = new Dictionary<string, string>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(i.ToString(), i.ToString() + "분");
            }
            ddl_Min_Start.DataSource = list;
            ddl_Min_Start.DataValueField = "Key";
            ddl_Min_Start.DataTextField = "Value";
            ddl_Min_Start.DataBind();
            ddl_Min_End.DataSource = list;
            ddl_Min_End.DataValueField = "Key";
            ddl_Min_End.DataTextField = "Value";
            ddl_Min_End.DataBind();
        }
        private void BindControl()
        {
            int nReturn = cloud.R_ServerList(MemberNum, "SQL");
            DataTable dt = cloud.dsReturn.Tables[0];
            ddl_Server.DataSource = dt;
            ddl_Server.DataTextField = "DisplayName";
            ddl_Server.DataValueField = "ServerNum";
            ddl_Server.DataBind();
        }
        private void BindData()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ddl_Server.SelectedValue))
            {
                cloud.R_HostInfo(Lib.Util.TConverter<int>(ddl_Server.SelectedValue));
                dt = cloud.dsReturn.Tables[0];
                gv_Info.DataSource = dt;
                gv_Info.DataBind();
            }
            int numDuration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ChartDataDuration"]);

            //각 차트에서 참조해서 사용할 공통 데이터 테이블 생성
            cloud.R_Adhoc("select TimeIn_UTC, Data_JSON from tbPerfmonValues_JSON where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum);

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


            {
                //dt_Avgwaintime 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P129" 
                           select r;

                dt_Avgwaintime = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_Avgwaintime.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_Avgwaintime.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_Avgwaintime = Lib.ConvertingProc.Pivot(dt_Avgwaintime, "Instancename", "TimeIn", "Pvalue");
                strColumn_AVGWAITTIME = Lib.ConvertingProc.GetColumname(dt_Avgwaintime.Columns);
            }

           
            {
                //dt_LockRequest 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P130"
                           select r;

                dt_LockRequest = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LockRequest.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LockRequest.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LockRequest = Lib.ConvertingProc.Pivot(dt_LockRequest, "Instancename", "TimeIn", "Pvalue");
                strColumn_LockRequest = Lib.ConvertingProc.GetColumname(dt_LockRequest.Columns);
            }

           
            {
                //dt_LockTimeoutTimeout 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P199"
                           select r;

                dt_LockTimeoutTimeout = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LockTimeoutTimeout.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LockTimeoutTimeout.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LockTimeoutTimeout = Lib.ConvertingProc.Pivot(dt_LockTimeoutTimeout, "Instancename", "TimeIn", "Pvalue");
                strColumn_LockTimeOutTimeOut = Lib.ConvertingProc.GetColumname(dt_LockTimeoutTimeout.Columns);
            }

            {
                //dt_LockTimeout 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P131"
                           select r;

                dt_LockTimeout = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LockTimeout.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LockTimeout.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LockTimeout = Lib.ConvertingProc.Pivot(dt_LockTimeout, "Instancename", "TimeIn", "Pvalue");
                strColumn_LockTimeOut = Lib.ConvertingProc.GetColumname(dt_LockTimeout.Columns);
            }

            {
                //dt_LockWaitTimeMs 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P132"
                           select r;

                dt_LockWaitTimeMs = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LockWaitTimeMs.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LockWaitTimeMs.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LockWaitTimeMs = Lib.ConvertingProc.Pivot(dt_LockWaitTimeMs, "Instancename", "TimeIn", "Pvalue");
                strColumn_LockWaitTimeMs = Lib.ConvertingProc.GetColumname(dt_LockWaitTimeMs.Columns);
            }

            {
                //dt_LockWaitTimeSec 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P133"
                           select r;

                dt_LockWaitTimeSec = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LockWaitTimeSec.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LockWaitTimeSec.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LockWaitTimeSec = Lib.ConvertingProc.Pivot(dt_LockWaitTimeSec, "Instancename", "TimeIn", "Pvalue");
                strColumn_LockWaitSec = Lib.ConvertingProc.GetColumname(dt_LockWaitTimeSec.Columns);
            }


            {
                //dt_NumberofDeadlock 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P134"
                           select r;

                dt_NumberofDeadlock = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_NumberofDeadlock.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_NumberofDeadlock.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_NumberofDeadlock = Lib.ConvertingProc.Pivot(dt_NumberofDeadlock, "Instancename", "TimeIn", "Pvalue");
                strColumn_NumberofDeadlock = Lib.ConvertingProc.GetColumname(dt_NumberofDeadlock.Columns);
            }

            
            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_Avgwaintime, cplst, "AVGWAITTIME_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LockRequest, cplst, "LOCKREQUESTS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LockTimeoutTimeout, cplst, "LOCKTIMEOUTSTIMEOUT_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LockTimeout, cplst, "LOCKTIMEOUTS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LockWaitTimeMs, cplst, "LOCKWAITTIMEMS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LockWaitTimeSec, cplst, "LOCKWAITSEC_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_NumberofDeadlock, cplst, "NUMBEROFDEADLOCKS_LINE_CHART");
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_AVGWAITTIME, "", "AVGWAITTIME_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LockRequest, "", "LOCKREQUESTS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LockTimeOutTimeOut, "", "LOCKTIMEOUTSTIMEOUT_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LockTimeOut, "", "LOCKTIMEOUTS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LockWaitTimeMs, "", "LOCKWAITTIMEMS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LockWaitSec, "", "LOCKWAITSEC_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_NumberofDeadlock, "", "NUMBEROFDEADLOCKS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
        private DataTable Func_SetColumn(DataTable dt)
        {
            DataTable dt_tmp = new DataTable();
            SortedDictionary<string, string> dicColumn = new SortedDictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!(dicColumn.ContainsKey(dr["PCID"].ToString())))
                    dicColumn.Add(dr["PCID"].ToString(), dr["PCountername"].ToString());
            }
            dt_tmp = ServicePoint.Lib.ConvertingProc.Pivot(dt, dicColumn, "PCID", "TimeIn", "Pvalue");
            return dt_tmp;
        }
    }
}