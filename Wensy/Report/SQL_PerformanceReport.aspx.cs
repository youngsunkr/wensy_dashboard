﻿using System;
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
    public partial class SQL_PerformanceReport : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_FreeDisk, dt_SQL;
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
                //dt_SQLDATA 데이터 테이블 만들기
                var PCIDs = new [] { "P139", "P004", "P007", "P008", "P170"
                                   , "P171", "P100", "P101", "P081", "P138"
                                   , "P106", "P107", "P108", "P084", "P099"
                                   , "P098", "P168", "P178", "P180", "P006"
                                   , "P075", "P077", "P078", "P079", "P082"
                                   , "P083", "P085", "P086", "P087", "P088"
                                   , "P096", "P097", "P173", "P104", "P105"
                                   , "P172", "P175" };

                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where PCIDs.Contains(r.Field<string>("PCID"))
                           || (r.Field<string>("PCID") == "P001" && r.Field<string>("InstanceName") == "_Total")
                           || (r.Field<string>("PCID") == "P006" && r.Field<string>("InstanceName") == "sqlserver")
                         
                           select r;

                dt_SQL = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_SQL.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_SQL.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_SQL = Lib.ConvertingProc.Pivot(dt_SQL, "Instancename", "TimeIn", "PValue");
                dt_SQL = Func_SetColumn(dt_SQL);
            }

            //DataTable dt_SQL = new DataTable();
            //cloud.R_SQL_Performance(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            //dt_SQL = Func_SetColumn(cloud.dsReturn.Tables[0]);
            DataTable dt_sum = new DataTable();
            dt_sum = Func_SumDatatable(dt_SQL);

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Line_DualYAxis(dt_SQL, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_SQL, cplst);
            cplst = Lib.Flotr2.SetArrayString_Cols(dt_SQL, cplst);
            cplst = Lib.Flotr2.SetArrayString_Pie(dt_sum, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Byte, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_hit, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_App_hit, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_BytePerExtension, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_IpAddr, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Lines(dt, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU Total", "CPU SQL", "CPU_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PQL", "", "PROCESSQUEUELENGTH_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Batch Request", "", "BATCHREQUEST_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PageLifeExpectancy", "", "PAGELIFEEXPECTANCY_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Buffer Cache Hit#c#Plan Cache Hit", "", "BUFFERPLANCACHE_LINE_CHART", "LINE", 100, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Compilations#c#ReCompilations", "", "COMPILATIONPERSEC_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Sent", "Received", "NETWORK_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("", "Sent#c#Received", "", "NETWORK_PIE_CHART", "PIE", 0, 0, 130, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("", "Compilations#c#ReCompilations", "", "COMPILATION_PIE_CHART", "PIE", 0, 0, 130, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Total Server Memory", "", "SERVERMEMORY_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Committed Memory", "Target Server Memory", "MEMORY_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ActiveTempTable", "", "ACTIVETMPTABLE_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CreateTempTable", "", "CREATETMPTABLE_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "UserConnection", "", "USERCONNECTION_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ConnectionMemory", "", "CONNECTIONMEMORY_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "LockMemory", "", "LOCKMEMORY_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "OptimizeMemory", "", "OPTIMIZEMEMORY_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Page Splits/sec", "", "PAGESPLITS_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Checkpoint pages/sec", "", "CHECKPOINTPAGES_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Table Lock Escalations/sec", "", "TABLELOCK_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Lazy writes/sec", "", "LAZYWRITE_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Connection Reset/sec", "", "CONTTECTIONRESET_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Workfiles Created/sec", "", "WORKFILES_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Worktables Created/sec", "", "WORKTABLES_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Page reads/sec", "Page writes/sec", "PAGEREADWRITE_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Page lookups/sec", "", "PAGELOOKUP_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Readahead pages/sec", "", "READAHEADPAGE_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Logins/sec", "Logouts/sec", "LOGINLOGOUT_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Memory Grants Outstanding", "Memory Grants Pending", "MEMORYGRANTSOUTSTANDING_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "true", "true", "true"));


            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strFreeDiskColumName, "", "FREEDISK_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("", strFreeDiskColumName, "", "FREEDISK_PIE_CHART", "PIE", 0, 0, 130, "false", "false"));
            ////cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "BufferCacheHit", "", "BUFFERCACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            return cpList;
        }
        private DataTable Func_SumDatatable(DataTable dt)
        {
            double sumReceived = 0;
            double sumSent = 0;
            double sumCompilations = 0;
            double sumReCompilations = 0;
            foreach (DataRow dr in dt.Rows)
            {
                sumReceived += Lib.Util.TConverter<double>(dr["Received"]);
                sumSent += Lib.Util.TConverter<double>(dr["Sent"]);
                sumCompilations += Lib.Util.TConverter<double>(dr["Compilations"]);
                sumReCompilations += Lib.Util.TConverter<double>(dr["ReCompilations"]);
            }
            DataTable dt_sum = new DataTable();
            dt_sum.Columns.Add("Received");
            dt_sum.Columns.Add("Sent");
            dt_sum.Columns.Add("Compilations");
            dt_sum.Columns.Add("ReCompilations");
            DataRow dr_sum = dt_sum.NewRow();
            dr_sum["Received"] = sumReceived;
            dr_sum["Sent"] = sumSent;
            dr_sum["Compilations"] = sumCompilations;
            dr_sum["ReCompilations"] = sumReCompilations;
            dt_sum.Rows.Add(dr_sum);
            return dt_sum;
        }
        private DataTable Func_SetColumn(DataTable dt)
        {
            DataTable dt_tmp = new DataTable();
            SortedDictionary<string, string> colNm = new SortedDictionary<string, string>();

            colNm.Add("P001", "CPU Total");
            colNm.Add("P139", "CPU SQL");
            colNm.Add("P004", "PQL");
            colNm.Add("P007", "Available Memory");
            colNm.Add("P008", "Committed Memory");
            colNm.Add("P170", "Received");
            colNm.Add("P171", "Sent");
            colNm.Add("P100", "Target Server Memory");
            colNm.Add("P101", "Total Server Memory");
            colNm.Add("P081", "Buffer Cache Hit");
            colNm.Add("P138", "Plan Cache Hit");
            colNm.Add("P106", "Batch Request");
            colNm.Add("P107", "Compilations");
            colNm.Add("P108", "ReCompilations");
            colNm.Add("P084", "PageLifeExpectancy");
            colNm.Add("P099", "ActiveTempTable");
            colNm.Add("P098", "UserConnection");
            colNm.Add("P168", "ConnectionMemory");
            colNm.Add("P178", "LockMemory");
            colNm.Add("P180", "OptimizeMemory");
            colNm.Add("P075", "Page Splits/sec");
            colNm.Add("P082", "Checkpoint pages/sec");
            colNm.Add("P079", "Table Lock Escalations/sec");
            colNm.Add("P083", "Lazy writes/sec");
            colNm.Add("P172", "Processes blocked");
            colNm.Add("P175", "Connection Reset/sec");
            colNm.Add("P077", "Workfiles Created/sec");
            colNm.Add("P078", "Worktables Created/sec");
            colNm.Add("P085", "Page lookups/sec");
            colNm.Add("P088", "Readahead pages/sec");
            colNm.Add("P086", "Page reads/sec");
            colNm.Add("P087", "Page writes/sec");
            colNm.Add("P096", "Logins/sec");
            colNm.Add("P097", "Logouts/sec");
            colNm.Add("P173", "CreateTempTable");
            colNm.Add("P104", "Memory Grants Outstanding");
            colNm.Add("P105", "Memory Grants Pending");
            dt_tmp = ServicePoint.Lib.ConvertingProc.Pivot(dt, colNm, "PCID", "TimeIn", "Pvalue");

            foreach (DataRow dr in dt_tmp.Rows)
            {
                dr["Committed Memory"] = Convert.ToDouble(dr["Committed Memory"]) / (1024 * 1024);
                dr["Received"] = Convert.ToDouble(dr["Received"]) / (1024);
                dr["Sent"] = Convert.ToDouble(dr["Sent"]) / (1024);
                dr["Target Server Memory"] = Convert.ToDouble(dr["Target Server Memory"]) / (1024);
                dr["Total Server Memory"] = Convert.ToDouble(dr["Total Server Memory"]) / (1024);
            }
            return dt_tmp;
        }
    }
}