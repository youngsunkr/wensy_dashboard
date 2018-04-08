using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;

namespace ServicePoint.Common.UC.Dashboard
{
    public partial class SqlDashboard : System.Web.UI.UserControl
    {
        #region Member
        private DB.Cloud cloud;
        protected DataTable dt_PerfmonValue, dt_Server, dt_Data, dt_FreeDisk, dt_MaxData, dt_MaxDisk, dt_DatabaseFileSize, dt_DBInfo, dt_SQLIndexFlag, dt_SQLSession, dt_Dashboard, dt_Processor;
        protected int numPQL, numProcess, numSQLService, numSQLAgent, numSQLLinked, numErrorLog, numSessionCNT, numActiveSession, numDatabaseCNT, numVLFCNT;
        protected double numRamSize, numCommited, numAvailable, numSQLTOtalServerMemoryGB, numSQLTargetServerMemoryGB, numDataFileSize_Use, numDataFileSize_Total, numLogFileSize_Use, numLogFileSize_Total, numIndexFlag;
        protected string stVLF_DB, numIndexFlagObject;
        public int MemberNum;
        public int CompanyNum;
        public int ServerNum;
        public int Servernum
        {
            get
            {
                return ServerNum;
            }
            set
            {
                ServerNum = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();
            InitControl();
            BindData();
            BindControl();
        }

        private void Page_PreRender(object sender, EventArgs e)
        {

        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {

        }
        #region Init

        private void InitControl()
        {
            MemberNum = Lib.Util.TConverter<int>(Lib.Util.GetCookieValue(Request, "MemberNum"));
            CompanyNum = Lib.Util.TConverter<int>(Lib.Util.GetCookieValue(Request, "CompanyNum"));
        }

        #endregion
        #region Bind
        private void BindData()
        {
            //호스트 정보 가져오기
            cloud.R_Adhoc("select ServerNum,HostName,DisplayName,DisplayGroup,RAMSIZE,IPAddress,ServerType,WinVer,Processors,CurrentStatus, TimeIn_UTC from tbHostStatus where ServerNum = " + ServerNum);
            dt_Server = cloud.dsReturn.Tables[0];
            dt_Server = Lib.ConvertingProc.SetDashboardDtServer(dt_Server);
            
            //각 차트에서 참조해서 사용할 공통 데이터 테이블 생성
            cloud.R_Adhoc("select a.TimeIn_UTC, Data_JSON from tbPerfmonValues_JSON as a with(nolock) inner join tbHostStatus as b on a.TimeIn_UTC = b.TimeIn_UTC and a.ServerNum = b.ServerNum where b.ServerNum = " + ServerNum);

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

            //tbDashboard 공통 데이터 생성
            cloud.R_Adhoc("select TimeIn_UTC, Data_JSON from tbDashboard_JSON with(nolock) where ServerNum = " + ServerNum + " and TimeIn_UTC >= dateadd(minute, -15, GETUTCDATE())");
            
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
                DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
                //DataTable dt_struct = new DataTable();
                dt_Dashboard = tester.Clone();
                dt_Dashboard.AcceptChanges();
                // 이후 dt_struct 에 계속 merge (union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

                    //신규추가 2017-09-23 데이터 머지 = mssql union 
                    //신규추가 2017-09-23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
                    dt_Dashboard.Merge(dt_tmp);
                }
            }

            //DB 정보 가져오기
            //int nReturn = cloud.w_Dashboard_SQL_Server(MemberNum, CompanyNum, ServerNum);
            cloud.R_Adhoc("select InstanceName,IsClustered,ComputerNamePhysicalNetBIOS,Edition,ProductLevel,ProductVersion,ProcessID,Collation,IsFullTextInstalled,IsIntegratedSecurityOnly,IsHadrEnabled,HadrManagerStatus,IsXTPSupported from tbSQLServerInfo where ServerNum = " + ServerNum);
            dt_DBInfo = cloud.dsReturn.Tables[0];
            


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

            //MaxDIsk 정보 가져오기
            dt_MaxDisk = Lib.ConvertingProc.SetDiskMaxProc(dt_FreeDisk);
            dt_MaxDisk = Lib.ConvertingProc.SetDashboardDtServer(dt_MaxDisk);

            //dt_FreeDisk = Lib.ConvertingProc.Pivot(dt_FreeDisk, "InstanceName", "TimeIn", "PValue");
            dt_FreeDisk = Lib.ConvertingProc.SetDiskProc(dt_FreeDisk);


            //dt_Processor 데이터 테이블 만들기
            var Processor = from r in dt_Dashboard.AsEnumerable()
                       where (r.Field<string>("PNum") == "P0")
                       select r;

            dt_Processor = dt_Dashboard.Clone();

            foreach (DataRow r in Processor)
            {
                var newRow = dt_Processor.NewRow();
                newRow.ItemArray = r.ItemArray;
                dt_Processor.Rows.Add(newRow);//I'm doubtful if you need to call this or not
            }












            //SQL Dashboard 성능 정보 가져오기
            int nReturn = cloud.get_Dashboard_Chart(ServerNum.ToString());
            dt_Data = Lib.ConvertingProc.ChangeDashboardColumnName(cloud.dsReturn.Tables[0]);
            dt_Data = Lib.ConvertingProc.SetDashboardDtServer(dt_Data);






            
            //성능 정보에서 최근값을 라벨에 뿌려주기 위해 MaxData 생성해서 마지막 값 읽기
            dt_MaxData = new DataTable();
            dt_MaxData = dt_Data.Clone();
            dt_MaxData.ImportRow(dt_Data.Rows[dt_Data.Rows.Count - 1]);
            //dt_MaxData = Lib.ConvertingProc.SetDashboardDtServer(dt_MaxData);



            //SQL Agent Fail
            //nReturn = cloud.get_SQLAgentFail(ServerNum);
            //numSQLAgent = cloud.dsReturn.Tables[0].Rows.Count;

            //SQL Lnked
            //nReturn = cloud.get_SQLLinked(ServerNum);
            //numSQLLinked = cloud.dsReturn.Tables[0].Rows.Count;

            //SQL Service
            //nReturn = cloud.get_SQLServiceStatus(ServerNum);
            //numSQLService = cloud.dsReturn.Tables[0].Rows.Count;

            //SQL Error
            //nReturn = cloud.get_SQLErrorlog(ServerNum);
            //numSQLLinked = cloud.dsReturn.Tables[0].Rows.Count;

            //SQL Session
            //nReturn = cloud.get_SQLSession(ServerNum);
            //if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            //{
            //    //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
            //    DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
            //    //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
            //    //DataTable dt_struct = new DataTable();
            //    dt_SQLSession = tester.Clone();
            //    dt_SQLSession.AcceptChanges();
            //    // 이후 dt_struct 에 계속 merge (union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
            //    foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            //    {
            //        DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

            //        //신규추가 2017-09-23 데이터 머지 = mssql union 
            //        //신규추가 2017-09-23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
            //        dt_SQLSession.Merge(dt_tmp);
            //    }
            //}

            //foreach (DataRow dr in dt_SQLSession.Rows)
            //{
            //    numSessionCNT += Convert.ToInt32(dr["TotalSession"].ToString());
            //    numActiveSession += Convert.ToInt32(dr["ActiveSession"].ToString());
            //}

            //SQL Database File Size
            //nReturn = cloud.get_SQLDatabaseFileSize(ServerNum);
            //if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            //{
            //    //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
            //    DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
            //    //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
            //    //DataTable dt_struct = new DataTable();
            //    dt_DatabaseFileSize = tester.Clone();
            //    dt_DatabaseFileSize.AcceptChanges();
            //    // 이후 dt_struct 에 계속 merge (union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
            //    foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            //    {
            //        DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

            //        //신규추가 2017-09-23 데이터 머지 = mssql union 
            //        //신규추가 2017-09-23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
            //        dt_DatabaseFileSize.Merge(dt_tmp);
            //    }
            //}

            //numDatabaseCNT = dt_DatabaseFileSize.Rows.Count;

            //numDataFileSize_Total = 0;
            //numDataFileSize_Use = 0;
            //numLogFileSize_Total = 0;
            //numLogFileSize_Use = 0;

            //if (dt_DatabaseFileSize.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt_DatabaseFileSize.Rows)
            //    {
            //        numDataFileSize_Total = numDataFileSize_Total + Convert.ToDouble(dr["Datafile_Size_MB"].ToString());
            //        numDataFileSize_Use = numDataFileSize_Use + Convert.ToDouble(dr["Data_MB"].ToString()) + Convert.ToDouble(dr["Index_MB"].ToString());
            //        numLogFileSize_Total = numLogFileSize_Total + Convert.ToDouble(dr["Transaction_Log_Size"].ToString());
            //        numLogFileSize_Use = numLogFileSize_Use + Convert.ToDouble(dr["Log_Used_Size_MB"].ToString());

            //        numVLFCNT = 0;
            //        stVLF_DB = "";


            //        if (Convert.ToInt32(dr["Total_vlf_cnt"].ToString()) > numVLFCNT)
            //        {
            //            numVLFCNT = Convert.ToInt32(dr["Total_vlf_cnt"].ToString());
            //            stVLF_DB = dr["DatabaseName"].ToString();
            //        }

            //    }
            //}


            //SQL Index Flag
            numIndexFlag = 0;
            numIndexFlagObject = "";

            //nReturn = cloud.get_SQLIndexflag(ServerNum);
            //if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            //{
            //    //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
            //    DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
            //    //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
            //    dt_SQLIndexFlag = tester.Clone();
            //    dt_SQLIndexFlag.AcceptChanges();
              
            //    foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            //    {
            //        DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

            //        dt_SQLIndexFlag.Merge(dt_tmp);
            //    }

            //    foreach (DataRow dr in dt_SQLIndexFlag.Rows)
            //    {
            //        if (Convert.ToInt32(dr["avg_frag_percent"].ToString()) > numIndexFlag)
            //        {
            //            numIndexFlag = Convert.ToInt32(dr["avg_frag_percent"].ToString());
            //            numIndexFlagObject = dr["db_name"].ToString() + ":" + dr["object_name"].ToString();
            //        }
            //    }
            //}
            
            
            //차트 데이터 던지기
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_Data, SetChartProperty());
            cplst = Lib.Flotr2.SetArrayString_Cols(dt_Data, cplst);
            cplst = Lib.Flotr2.SetArrayString_Bar(dt_FreeDisk, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_UC.Text += Lib.Util.BoxingScript(sb.ToString());
            
        }
        private void BindControl()
        {
            try
            {
                //public 변수 (구글프르세스바에 쓰이는것들)
                numProcess = Lib.Util.TConverter<int>(dt_Server.Rows[0]["Processors"]);
                numPQL = Lib.Util.TConverter<int>(dt_MaxData.Rows[0]["ProcessorQueueLength"]);
                numSQLTOtalServerMemoryGB = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["SQLTotalServerMemory"]);
                numSQLTargetServerMemoryGB = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["SQLTargetServerMemory"]);
                numDataFileSize_Total = Lib.Util.TConverter<double>(numDataFileSize_Use);
                numDataFileSize_Use = Lib.Util.TConverter<double>(numDataFileSize_Use);
                numLogFileSize_Total = Lib.Util.TConverter<double>(numLogFileSize_Total);
                numLogFileSize_Use = Lib.Util.TConverter<double>(numLogFileSize_Use);
                numIndexFlag = Lib.Util.TConverter<double>(numIndexFlag);
                numSQLService = Lib.Util.TConverter<int>(numSQLService);
                numSQLAgent = Lib.Util.TConverter<int>(numSQLAgent);
                numSQLLinked = Lib.Util.TConverter<int>(numSQLLinked);
                numErrorLog = Lib.Util.TConverter<int>(numErrorLog);
                numRamSize = Lib.Util.TConverter<double>(dt_Server.Rows[0]["RamSize"]);
                numCommited = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["CommittedMemory"]);
                numAvailable = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["AvailableMemory"]);
                numSessionCNT = Lib.Util.TConverter<int>(numSessionCNT);

                //System
                lbl_HostName.Text = dt_Server.Rows[0]["Hostname"].ToString();
                lbl_CPUCore.Text = dt_Server.Rows[0]["Processors"].ToString();
                lbl_RAMSize.Text = string.Format("{0:0.00}", Convert.ToDouble(dt_Server.Rows[0]["RamSize"])) + "GB"; ;
                lbl_OSVersion.Text = dt_Server.Rows[0]["WinVer"].ToString();

                //SQL Info
                lbl_SQLVersion.Text = dt_DBInfo.Rows[0]["ProductVersion"].ToString();
                lbl_SQLEdition.Text = dt_DBInfo.Rows[0]["Edition"].ToString();
                lbl_SQLProductLevel.Text = dt_DBInfo.Rows[0]["ProductLevel"].ToString();
                SQLCOllation.Text = dt_DBInfo.Rows[0]["Collation"].ToString();

                //Process
                lbl_TotalCPU.Text = dt_MaxData.Rows[0]["CPU-Total"].ToString() + "%";
                lbl_KenelCPU.Text = dt_MaxData.Rows[0]["CPU-Kernel"].ToString() + "%";
                lbl_UserCPU.Text = (Convert.ToDouble(dt_MaxData.Rows[0]["CPU-Total"]) - Convert.ToDouble(dt_MaxData.Rows[0]["CPU-Kernel"])).ToString() + "%";
                lbl_PQL.Text = dt_MaxData.Rows[0]["ProcessorQueueLength"].ToString();
                lbl_SQL.Text = dt_MaxData.Rows[0]["CPU-SQL"].ToString() + "%";

                //Memory
                lbl_BufferCacheSize.Text = dt_MaxData.Rows[0]["SQLBufferCacheSize"].ToString() + "MB";
                lbl_BufferCacheHit.Text = dt_MaxData.Rows[0]["SQLBufferCacheHit"].ToString() + "%";
                lbl_ProcedureCacheSize.Text = dt_MaxData.Rows[0]["SQLPlanCacheSize"].ToString() + "MB";
                lbl_ProcedureCacheHit.Text = dt_MaxData.Rows[0]["SQLPlanCacheHit"].ToString() + "%";
                lbl_PagelifeExpectancy.Text = dt_MaxData.Rows[0]["SQLPageLifeExpectancy"].ToString();
                lbl_CompilationSec.Text = dt_MaxData.Rows[0]["SQLCompilations"].ToString();
                lbl_ReCompilationSec.Text = dt_MaxData.Rows[0]["SQLReCompilations"].ToString();
                lbl_BatchRequest.Text = dt_MaxData.Rows[0]["SQLBatchRequests"].ToString();


                //Network
                lbl_BytesTotal.Text = dt_MaxData.Rows[0]["NetworkBytes-Total"].ToString() + "MB";
                lbl_BytesSent.Text = dt_MaxData.Rows[0]["NetworkBytes-Sent"].ToString() + "MB";
                lbl_BytesReceived.Text = dt_MaxData.Rows[0]["NetworkBytes-Received"].ToString() + "MB";
                lbl_OutputQueueLength.Text = dt_MaxData.Rows[0]["NetworkOutputQueueLength"].ToString();

                //Disk
                lbl_PhysicalRead.Text = dt_MaxData.Rows[0]["PhysicalDiskRead"].ToString();
                lbl_PhysicalWrite.Text = dt_MaxData.Rows[0]["PhysicalDiskWrite"].ToString();

                lbl_MaxDiskTime.Text = dt_MaxDisk.Rows[0]["MaxDiskTime"].ToString() + "%[" + dt_MaxDisk.Rows[0]["MaxDiskTimeInstance"].ToString() + "]";
                lbl_MaxDisk_Queuelength.Text = dt_MaxDisk.Rows[0]["MaxDiskQueueLength"].ToString() + "[" + dt_MaxDisk.Rows[0]["MaxDiskQueueLengthInstance"].ToString() + "]";
                lbl_MinDiskIdleTime.Text = dt_MaxDisk.Rows[0]["MinDiskIdleTime"].ToString() + "[" + dt_MaxDisk.Rows[0]["MinDiskIdleTimeInstance"].ToString() + "]";

                lbl_CheckPointPageSec.Text = dt_MaxData.Rows[0]["SQLCheckpointPage"].ToString();
                lbl_LogFlushSec.Text = dt_MaxData.Rows[0]["SQLLogFlushes"].ToString();
                lbl_LazyWriteSec.Text = dt_MaxData.Rows[0]["SQLLazyWrites"].ToString();
                lbl_ReadAheadPage.Text = dt_MaxData.Rows[0]["SQLReadahead"].ToString();
            

                //session
                lbl_ResponseTime.Text = dt_MaxData.Rows[0]["SQLReponstime"].ToString() + "ms";
                lbl_Sessioin.Text = numSessionCNT.ToString();
                lbl_ActiveSessions.Text = numActiveSession.ToString();
                lbl_UserConnections.Text = dt_MaxData.Rows[0]["SQLConnection"].ToString();

                //databases
                //lbl_DataFileSize.Text =  numDataFileSize_Total.ToString()+"GB";
                //lbl_LogFileSieze.Text =  numLogFileSize_Total.ToString()+"GB";
                //lbl_Databases.Text = numDatabaseCNT.ToString();
                //lbl_MaxVLF.Text = numVLFCNT.ToString() + "[" + stVLF_DB + "]";
                //lbl_MaxIndexFlagmentation.Text = numIndexFlagObject;
                //lbl_ActiveTransaction.Text = dt_MaxData.Rows[0]["SQLActiveTransactions"].ToString();
                //lbl_ActiveTempTable.Text = dt_MaxData.Rows[0]["SQLActiveTempTable"].ToString();
            }
            catch
            {

            }
            
        }
        #endregion
        #region Func
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU-Total", "", "TOTALCPU_COLS_CHART", "COLS", 100, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-Total", "", "BYTESTOTAL_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-Sent", "", "BYTESSENT_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-Received", "", "BYTESRECEIVED_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("FreePercent", "Drive", "FreeGB", "FREEDISK_BAR_CHART", "BAR", 100, 0, 100, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLBatchRequests", "", "BATCHREQUEST_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLBufferCacheHit", "", "BUFFERCACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLPlanCacheHit", "", "PROCEDURECACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLPageLifeExpectancy", "", "PAGELIFEEXPECTANCY_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLCompilations", "", "COMPILATIONSEC_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLReCompilations", "", "RECOMPILATIONSEC_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));

            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLCheckpointPage", "", "CHECKPOINTPAGESEC_LINE_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLLogFlushes", "", "LOGFLUSHSEC_LINE_CHART", "LINE", 100, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLLazyWrites", "", "LAZYWRITESEC_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLReadahead", "", "READAHEADPAGE_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLConnection", "", "USERCONNECTIONS_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));

            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLActiveTransactions", "", "ACTIVETRANSACTION_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "SQLActiveTempTable", "", "ACTIVETEMPTABLE_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            return cpList;
        }

        #endregion
    }
}