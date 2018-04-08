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
    public partial class WebDashboard : System.Web.UI.UserControl
    {
        #region Member
        private DB.Cloud cloud;
        protected DataTable dt_PerfmonValue,dt_Server, dt_Data, dt_FreeDisk, dt_MaxData, dt_MaxDisk;
        protected int numPQL, numProcess;
        protected double numRamSize, numCommited, numAvailable,numTotalWorketProcessmemory,numMaxWorkerProcessmemory;
        public int MemberNum, numRequestsQueued,CompanyNum,ServerNum;
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
            try
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

                //웹 성능 정보 가져오기
                int nReturn = cloud.get_Dashboard_Chart(ServerNum.ToString());
                dt_Data = Lib.ConvertingProc.ChangeDashboardColumnName(cloud.dsReturn.Tables[0]);
                dt_Data = Lib.ConvertingProc.SetDashboardDtServer(dt_Data);
                //성능 정보에서 최근값을 라벨에 뿌려주기 위해 MaxData 생성해서 마지막 값 읽기
                dt_MaxData = new DataTable();
                dt_MaxData = dt_Data.Clone();
                dt_MaxData.ImportRow(dt_Data.Rows[dt_Data.Rows.Count - 1]);
                //dt_MaxData = Lib.ConvertingProc.SetDashboardDtServer(dt_MaxData);
            
                //차트 데이터 던지기
                List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
                StringBuilder sb = new StringBuilder();
                cplst = Lib.Flotr2.SetArrayString_Lines(dt_Data, SetChartProperty());
                cplst = Lib.Flotr2.SetArrayString_Cols(dt_Data, cplst);
                cplst = Lib.Flotr2.SetArrayString_Bar(dt_FreeDisk, cplst);
                sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
                litScript_UC.Text += Lib.Util.BoxingScript(sb.ToString());
            }
            catch
            {

            }

           
        }
        private void BindControl()
        {
            try
            { 
                DataRow dr = dt_MaxData.Rows[0];

                numRamSize = Lib.Util.TConverter<double>(dt_Server.Rows[0]["RamSize"]);
                numCommited = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["CommittedMemory"]);
                numAvailable = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["AvailableMemory"]);
                numProcess = Lib.Util.TConverter<int>(dt_Server.Rows[0]["Processors"]);
                numPQL = Lib.Util.TConverter<int>(dt_MaxData.Rows[0]["ProcessorQueueLength"]);
                numTotalWorketProcessmemory = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["w3wpTotalWorkProcessMemory"]);
                numMaxWorkerProcessmemory = Lib.Util.TConverter<double>(dt_MaxData.Rows[0]["w3wpMaxWorkProcessMemory"]);
                numRequestsQueued = Lib.Util.TConverter<int>(dt_MaxData.Rows[0]["ASPRequestsQueue"]);

                //System
                lbl_HostName.Text = dt_Server.Rows[0]["Hostname"].ToString();
                lbl_CPUCore.Text = dt_Server.Rows[0]["Processors"].ToString();
                lbl_RAMSize.Text = string.Format("{0:0.00}", Convert.ToDouble(dt_Server.Rows[0]["RamSize"])) + "GB"; ;
                lbl_OSVersion.Text = dt_Server.Rows[0]["WinVer"].ToString();

                //Process
                lbl_TotalCPU.Text = dt_MaxData.Rows[0]["CPU-Total"].ToString() + "%";
                lbl_KenelCPU.Text = dt_MaxData.Rows[0]["CPU-Kernel"].ToString() + "%";
                lbl_UserCPU.Text = (Convert.ToDouble(dt_MaxData.Rows[0]["CPU-Total"]) - Convert.ToDouble(dt_MaxData.Rows[0]["CPU-Kernel"])).ToString() + "%";
                lbl_PQL.Text = dt_MaxData.Rows[0]["ProcessorQueueLength"].ToString();
                lbl_WorkerProcesses.Text = dt_MaxData.Rows[0]["CPU-W3WP"].ToString() + "%";
            

                //Memory
            

                //Network
                lbl_BytesTotal.Text = dr["NetworkBytes-Total"].ToString() + "MB";
                lbl_BytesSent.Text = dr["NetworkBytes-Sent"].ToString() + "MB";
                lbl_BytesReceived.Text = dr["NetworkBytes-Received"].ToString() + "MB";
                lbl_BytesTotalSecWeb.Text = dr["NetworkBytes-IISTotal"].ToString() + "MB";
                lbl_OutputQueueLength.Text = dr["NetworkOutputQueueLength"].ToString();

                //Disk
                lbl_PhysicalRead.Text = dt_MaxData.Rows[0]["PhysicalDiskRead"].ToString();
                lbl_PhysicalWrite.Text = dt_MaxData.Rows[0]["PhysicalDiskWrite"].ToString();

                lbl_MaxDiskTime.Text = dt_MaxDisk.Rows[0]["MaxDiskTime"].ToString() + "%[" + dt_MaxDisk.Rows[0]["MaxDiskTimeInstance"].ToString() + "]";
                lbl_MaxDisk_Queuelength.Text = dt_MaxDisk.Rows[0]["MaxDiskQueueLength"].ToString() + "[" + dt_MaxDisk.Rows[0]["MaxDiskQueueLengthInstance"].ToString() + "]";
                lbl_MinDiskIdleTime.Text = dt_MaxDisk.Rows[0]["MinDiskIdleTime"].ToString() + "[" + dt_MaxDisk.Rows[0]["MinDiskIdleTimeInstance"].ToString() + "]";

                //WebService
                lbl_RequestSec.Text = dt_MaxData.Rows[0]["IISRequests"].ToString();
                lbl_ActiveRequest.Text = dt_MaxData.Rows[0]["IISActiveRequests"].ToString();
                lbl_CurrentConnection.Text = dt_MaxData.Rows[0]["IISCurrentConnection"].ToString();
                lbl_CurrentUsers.Text = dt_MaxData.Rows[0]["IISCurrentUser"].ToString();
                lbl_WebToDbConnections.Text = dt_MaxData.Rows[0]["IIStoDBConnection"].ToString();

                //ASP.NET
                lbl_RequestExecutionTime.Text = dt_MaxData.Rows[0]["ASPRequestExecutionTime"].ToString() + "ms";
                lbl_RequestsExecutiong.Text = dt_MaxData.Rows[0]["ASPRequestsExecuting"].ToString();
                lbl_RequestsSec.Text = dt_MaxData.Rows[0]["ASPRequestsPerSec"].ToString();
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
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU-W3WP", "", "WORKERPROCESSES_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "NetworkBytes-IISTotal", "", "BYTESTOTALSECWEB_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IISRequests", "", "REQUESTSEC_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IISActiveRequests", "", "ACTIVEREQUEST_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IISCurrentConnection", "", "CURRENTCONNECTION_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IISCurrentUser", "", "CURRENTUSERS_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "IIStoDBConnection", "", "WEBTODBCONNECTIONS_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestExecutionTime", "", "REQUESTEXECUTIONTIME_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestsExecuting", "", "REQUESTSEXECUTIONG_LINE_CHART", "LINE", 0, 0, 40, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "ASPRequestsPerSec", "", "REQUESTSSEC_COLS_CHART", "COLS", 0, 0, 40, "false", "false"));
            
            return cpList;
        }

        #endregion
    }
}