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
    public partial class SQL_Database : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_ACTIVETRANSACTIONS, dt_LOGCACHEHITRATIO, dt_LOGBYTESFLUSHEDSEC, dt_LOGFLUSHWAITSSEC, dt_LOGFLUSHWAITTIME, dt_LOGFLUSHESSEC, dt_LOGGROWTHS, dt_LOGSHRINKS, dt_LOGTRUNCATIONS;
        private string strColumn_ACTIVETRANSACTIONS, strColumn_LOGCACHEHITRATIO, strColumn_LOGBYTESFLUSHEDSEC, strColumn_LOGFLUSHWAITTIME, strColumn_LOGFLUSHWAITSSEC, strColumn_LOGFLUSHESSEC, strColumn_LOGGROWTHS, strColumn_LOGSHRINKS, strColumn_LOGTRUNCATIONS;
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
            if (Request.QueryString.AllKeys.Contains("ServerNum"))
            {
                ServerNum = Lib.Util.TConverter<int>(Request.QueryString["ServerNum"]);
                ddl_Server.SelectedValue = ServerNum.ToString();
            }

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

                cloud.w_SQLDatabasesDetail(Lib.Util.TConverter<int>(ddl_Server.SelectedValue));
                gv_List.DataSource = cloud.dsReturn.Tables[0];
                gv_List.DataBind();
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
                //dt_ACTIVETRANSACTIONS 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P114"
                           select r;

                dt_ACTIVETRANSACTIONS = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_ACTIVETRANSACTIONS.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_ACTIVETRANSACTIONS.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_ACTIVETRANSACTIONS = Lib.ConvertingProc.Pivot(dt_ACTIVETRANSACTIONS, "Instancename", "TimeIn", "Pvalue");
                strColumn_ACTIVETRANSACTIONS = Lib.ConvertingProc.GetColumname(dt_ACTIVETRANSACTIONS.Columns);
            }

            {
                //dt_LOGCACHEHITRATIO 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P169"
                           select r;

                dt_LOGCACHEHITRATIO = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGCACHEHITRATIO.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGCACHEHITRATIO.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGCACHEHITRATIO = Lib.ConvertingProc.Pivot(dt_LOGCACHEHITRATIO, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGCACHEHITRATIO = Lib.ConvertingProc.GetColumname(dt_LOGCACHEHITRATIO.Columns);
            }

            {
                //dt_LOGCACHEHITRATIO 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P120"
                           select r;

                dt_LOGBYTESFLUSHEDSEC = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGBYTESFLUSHEDSEC.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGBYTESFLUSHEDSEC.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGBYTESFLUSHEDSEC = Lib.ConvertingProc.Pivot(dt_LOGBYTESFLUSHEDSEC, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGBYTESFLUSHEDSEC = Lib.ConvertingProc.GetColumname(dt_LOGBYTESFLUSHEDSEC.Columns);
            }

            {
                //dt_LOGFLUSHWAITTIME 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P121"
                           select r;

                dt_LOGFLUSHWAITTIME = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGFLUSHWAITTIME.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGFLUSHWAITTIME.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGFLUSHWAITTIME = Lib.ConvertingProc.Pivot(dt_LOGFLUSHWAITTIME, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGFLUSHWAITTIME = Lib.ConvertingProc.GetColumname(dt_LOGFLUSHWAITTIME.Columns);
            }


            {
                //dt_LOGFLUSHWAITTIME 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P118"
                           select r;

                dt_LOGFLUSHWAITSSEC = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGFLUSHWAITSSEC.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGFLUSHWAITSSEC.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGFLUSHWAITSSEC = Lib.ConvertingProc.Pivot(dt_LOGFLUSHWAITSSEC, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGFLUSHWAITSSEC = Lib.ConvertingProc.GetColumname(dt_LOGFLUSHWAITSSEC.Columns);
            }

            {
                //dt_LOGFLUSHESSEC 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P119"
                           select r;

                dt_LOGFLUSHESSEC = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGFLUSHESSEC.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGFLUSHESSEC.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGFLUSHESSEC = Lib.ConvertingProc.Pivot(dt_LOGFLUSHESSEC, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGFLUSHESSEC = Lib.ConvertingProc.GetColumname(dt_LOGFLUSHESSEC.Columns);
            }

            {
                //dt_LOGGROWTHS 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P122"
                           select r;

                dt_LOGGROWTHS = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGGROWTHS.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGGROWTHS.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGGROWTHS = Lib.ConvertingProc.Pivot(dt_LOGGROWTHS, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGGROWTHS = Lib.ConvertingProc.GetColumname(dt_LOGGROWTHS.Columns);
            }

            {
                //dt_LOGSHRINKS 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P123"
                           select r;

                dt_LOGSHRINKS = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGSHRINKS.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGSHRINKS.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGSHRINKS = Lib.ConvertingProc.Pivot(dt_LOGSHRINKS, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGSHRINKS = Lib.ConvertingProc.GetColumname(dt_LOGSHRINKS.Columns);
            }

            {
                //dt_LOGSHRINKS 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P124"
                           select r;

                dt_LOGTRUNCATIONS = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_LOGTRUNCATIONS.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_LOGTRUNCATIONS.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_LOGTRUNCATIONS = Lib.ConvertingProc.Pivot(dt_LOGTRUNCATIONS, "Instancename", "TimeIn", "Pvalue");
                strColumn_LOGTRUNCATIONS = Lib.ConvertingProc.GetColumname(dt_LOGTRUNCATIONS.Columns);
            }


            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ACTIVETRANSACTIONS, cplst, "ACTIVETRANSACTIONS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGCACHEHITRATIO, cplst, "LOGCACHEHITRATIO_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGBYTESFLUSHEDSEC, cplst, "LOGBYTESFLUSHEDSEC_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGFLUSHWAITTIME, cplst, "LOGFLUSHWAITTIME_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGFLUSHWAITSSEC, cplst, "LOGFLUSHWAITSSEC_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGFLUSHESSEC, cplst, "LOGFLUSHESSEC_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGGROWTHS, cplst, "LOGGROWTHS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGSHRINKS, cplst, "LOGSHRINKS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LOGTRUNCATIONS, cplst, "LOGTRUNCATIONS_LINE_CHART");
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_ACTIVETRANSACTIONS, "", "ACTIVETRANSACTIONS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGCACHEHITRATIO, "", "LOGCACHEHITRATIO_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGBYTESFLUSHEDSEC, "", "LOGBYTESFLUSHEDSEC_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGFLUSHWAITTIME, "", "LOGFLUSHWAITTIME_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGFLUSHWAITSSEC, "", "LOGFLUSHWAITSSEC_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGFLUSHESSEC, "", "LOGFLUSHESSEC_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGGROWTHS, "", "LOGGROWTHS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGSHRINKS, "", "LOGSHRINKS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LOGTRUNCATIONS, "", "LOGTRUNCATIONS_LINE_CHART", "LINE", 0, 0, 250, "false", "false", "true", "true", "true", "true"));

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