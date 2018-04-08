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
    public partial class SQL_Cpu : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_CPU, dt_SQLQuery, dt_SQLCurrentQuery;
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

            ddl_Min_Start.SelectedValue = (numMin - 5).ToString();
            ddl_Min_End.SelectedValue = numMin.ToString();

            ddl_Hour_Start.SelectedValue = numHour.ToString();
            ddl_Hour_End.SelectedValue = numHour.ToString();

        }
        private void RequestForm()
        {
            ServerNum = Lib.Util.TConverter<int>(ddl_Server.SelectedValue);
            dtmStart = Convert.ToDateTime(txt_dtmStart.Text).AddHours(Lib.Util.TConverter<int>(ddl_Hour_Start.SelectedValue)).AddMinutes(Lib.Util.TConverter<int>(ddl_Min_Start.SelectedValue)); ;
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

            list.Add("5", "5min");
            list.Add("10", "10min");
            list.Add("20", "20min");
            list.Add("30", "30min");
            list.Add("60", "60min");

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
                //dt_CPU 데이터 테이블 만들기
                var PCIDs = new[] { "P139", "P004", "P107", "P108", "P106", "P184" };

                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where PCIDs.Contains(r.Field<string>("PCID"))
                           || (r.Field<string>("PCID") == "P001" && r.Field<string>("InstanceName") == "_Total")

                           select r;

                dt_CPU = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_CPU.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_CPU.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_SQL = Lib.ConvertingProc.Pivot(dt_SQL, "Instancename", "TimeIn", "PValue");
                dt_CPU = Func_SetColumn(dt_CPU);
            }


            //실행 쿼리 데이터 읽어오기
            cloud.R_Adhoc("select TimeIn_UTC, Data_JSON from tbSQLCurrentExecution_JSON where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum);

            //신규추가 2017-09-23 
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
                DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
                //DataTable dt_struct = new DataTable();
                dt_SQLQuery = tester.Clone();
                dt_SQLQuery.AcceptChanges();
                // 이후 dt_struct 에 계속 merge (union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

                    //신규추가 2017-09-23 데이터 머지 = mssql union 
                    //신규추가 2017-09-23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
                    dt_SQLQuery.Merge(dt_tmp);
                }
            }

            {
                //dt_SQLCurrentQuery 데이터 테이블 만들기
                var test = (from r in dt_SQLQuery.AsEnumerable()
                           orderby r.Field<Int64>("cpu_time") descending
                           
                           select r).Take(20);

                dt_SQLCurrentQuery = dt_SQLQuery.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_SQLCurrentQuery.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_SQLCurrentQuery.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                gv_List_Query.DataSource = dt_SQLCurrentQuery;
                gv_List_Query.DataBind();
            }


          
            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Line_DualYAxis(dt_CPU, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_CPU, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU Total", "CPU SQL", "CPU_LINE_CHART", "DUALLINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PQL", "", "PROCESSQUEUELENGTH_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU Total", "Privileged Time", "PROCESSTIME_LINE_CHART", "DUALLINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Batch Request", "", "BATCH_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Compilations", "ReCompilations", "COMPILATIONS_LINE_CHART", "DUALLINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Transaction", "", "TRANSACTION_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));

            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strFreeDiskColumName, "", "FREEDISK_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("", strFreeDiskColumName, "", "FREEDISK_PIE_CHART", "PIE", 0, 0, 130, "false", "false"));
            ////cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "BufferCacheHit", "", "BUFFERCACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            return cpList;
        }
        private DataTable Func_SetColumn(DataTable dt)
        {
            DataTable dt_tmp = new DataTable();
            SortedDictionary<string, string> dicColumn = new SortedDictionary<string, string>();

            dicColumn.Add("P001", "CPU Total");
            dicColumn.Add("P006", "CPU SQL");
            dicColumn.Add("P002", "Privileged Time");
            dicColumn.Add("P004", "PQL");
            dicColumn.Add("P107", "Compilations");
            dicColumn.Add("P108", "ReCompilations");
            dicColumn.Add("P106", "Batch Request");
            dicColumn.Add("P184", "Transaction");
            dt_tmp = ServicePoint.Lib.ConvertingProc.Pivot(dt, dicColumn, "PCID", "TimeIn", "Pvalue");
            return dt_tmp;
        }
        protected string GetQuery(string str)
        {
            string strTag = "<textarea  rows='5' class='col-lg-12 input-sm'>" + str + "</textarea>";
            return strTag;
        }
    }
}