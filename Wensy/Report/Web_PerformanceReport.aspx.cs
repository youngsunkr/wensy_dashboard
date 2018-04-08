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
    public partial class Web_PerformanceReport : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_Data, dt_FreeDisk, dt_FreeDiskPercent;
        protected int ServerNum;
        private string strFreeDiskColumName;
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
            var i = ddl_Server.SelectedItem;
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
            int nReturn = cloud.R_ServerList(MemberNum, "Web");
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
                cloud.get_ServerStatus(ServerNum.ToString());
                dt = cloud.dsReturn.Tables[0];
                gv_Info.DataSource = dt;
                gv_Info.DataBind();
            }

            //cloud.R_WEB_Report(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            //Adhoc으로 던져봄 (tbDashboard 데이터 가져오기)
            cloud.R_Adhoc("select TimeIn,P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19 from tbDashboard where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum);
            //dt = new DataTable();
            dt_Data = Lib.ConvertingProc.ChangeDashboardColumnName(cloud.dsReturn.Tables[0]);
            dt_Data = Lib.ConvertingProc.SetDashboardDtServer(dt_Data);
            dt_Data = func_MatchColumName(dt_Data);


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
                //dt_FreeDisk 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P018" && !r.Field<string>("InstanceName").Contains("_Total") && !r.Field<string>("InstanceName").Contains("HarddiskVolume")
                           select r;

                dt_FreeDisk = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_FreeDisk.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_FreeDisk.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_FreeDisk = Lib.ConvertingProc.SetDiskProc(dt_FreeDisk);
                //dt_FreeDisk = Lib.ConvertingProc.Pivot(dt_FreeDisk, "Instancename", "TimeIn", "PValue");
                //strFreeDiskColumName = Lib.ConvertingProc.GetColumname(dt_FreeDisk.Columns);
            }

            {
                //dt_FreeDiskPercent 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P018" && !r.Field<string>("InstanceName").Contains("_Total") && !r.Field<string>("InstanceName").Contains("HarddiskVolume")
                           select r;

                dt_FreeDiskPercent = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_FreeDiskPercent.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_FreeDiskPercent.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                dt_FreeDiskPercent = Lib.ConvertingProc.SetDiskProc(dt_FreeDiskPercent);
            }
  
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Line_DualYAxis(dt_Data, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Pie(dt_FreeDisk_Max, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_FreeDisk, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_Data, cplst);
            cplst = Lib.Flotr2.SetArrayString_Bar(dt_FreeDiskPercent, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "CPU-Total#c#CPU-Kernel#c#Time#c#CPU-IIS", "", "CPU_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PQL", "", "PROCESSORQUEUELENGTH_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Requests Executing#c#Requests/Sec#c#Requests Queued", "", "ASPREQUEST_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Current Connections", "Request Execution Time", "CURRENTCONNECTIONREQUEST_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "true", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Committed#c#Available", "", "COMMITEDBYTE_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "W3WP(Total)#c#W3WP(MAX)", "", "W3WP_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Total#c#Sent#c#Received", "", "NETWORKMAX_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Total(Web)", "", "NETWORKWEB_LINE_CHART", "LINE", 0, 0, 130, "false", "true", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Disk Time", "Avg. Disk sec/Read", "DISKTIME_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strFreeDiskColumName, "", "FREEDISK_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("", strFreeDiskColumName, "", "FREEDISK_PIE_CHART", "PIE", 0, 0, 300, "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("FreePercent", "Drive", "FreeGB", "FREEDISK_BAR_CHART", "BAR", 100, 0, 130, "false", "false"));
            return cpList;
        }
        private DataTable func_MatchColumName(DataTable dt_tmp)
        {
            dt_tmp.Columns["CPU-Total"].ColumnName = "CPU-Total";
            dt_tmp.Columns["CPU-Kernel"].ColumnName = "CPU-Kernel";
            dt_tmp.Columns["ProcessorQueueLength"].ColumnName = "PQL";
            dt_tmp.Columns["AvailableMemory"].ColumnName = "Available";
            dt_tmp.Columns["CommittedMemory"].ColumnName = "Committed";
            dt_tmp.Columns["LogicalDiskTIme"].ColumnName = "Disk Time";
            dt_tmp.Columns["LogicalDiskAvgRead"].ColumnName = "Avg. Disk sec/Read";
            dt_tmp.Columns["LogicalDiskFreeMByte-C"].ColumnName = "Free Megabytes";
            dt_tmp.Columns["NetworkBytes-Total"].ColumnName = "Total";
            dt_tmp.Columns["NetworkBytes-Received"].ColumnName = "Received";
            dt_tmp.Columns["NetworkBytes-Sent"].ColumnName = "Sent";
            dt_tmp.Columns["NetworkBytes-IISTotal"].ColumnName = "Total(Web)";
            dt_tmp.Columns["IISCurrentConnection"].ColumnName = "Current connections";
            dt_tmp.Columns["CPU-W3WP"].ColumnName = "CPU-IIS";
            dt_tmp.Columns["w3wpMaxWorkProcessMemory"].ColumnName = "W3WP(Max)";
            dt_tmp.Columns["w3wpTotalWorkProcessMemory"].ColumnName = "W3WP(Total)";
            dt_tmp.Columns["ASPRequestExecutionTime"].ColumnName = "Request Execution Time";
            dt_tmp.Columns["ASPRequestsExecuting"].ColumnName = "Requests Executing";
            dt_tmp.Columns["ASPRequestsQueue"].ColumnName = "Requests Queued";
            dt_tmp.Columns["ASPRequestsPerSec"].ColumnName = "Requests/Sec";




            //foreach (DataRow dr in dt_tmp.Rows)
            //{
            //    dr["W3WP (Max)"] = Convert.ToDouble(dr["W3WP (Max)"]) / (1024 * 1024);
            //    dr["W3WP (Total)"] = Convert.ToDouble(dr["W3WP (Total)"]) / (1024 * 1024);
            //    dr["Committed Bytes"] = Convert.ToDouble(dr["Committed Bytes"]) / (1024 * 1024);
            //    dr["Bytes Total"] = Convert.ToDouble(dr["Bytes Total"]) / (1024);
            //    dr["Bytes Total(Web)"] = Convert.ToDouble(dr["Bytes Total(Web)"]) / (1024);
            //    dr["Bytes Received"] = Convert.ToDouble(dr["Bytes Received"]) / (1024);
            //    dr["Bytes Sent"] = Convert.ToDouble(dr["Bytes Sent"]) / (1024);
            //}
            //dt_tmp.DefaultView.Sort = "TimeIn desc";
            return dt_tmp.DefaultView.ToTable();

        }
    }
}