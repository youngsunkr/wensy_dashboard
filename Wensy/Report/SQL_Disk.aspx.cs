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
    public partial class SQL_Disk : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_PerfmonValue, dt_FreeSpace_per, dt_FreeSpace_byte, dt_DiskTime, dt_DiskIdleTime, dt_AvgDiskRead, dt_AvgDiskWrite, dt_AvgDiskReadQueue, dt_AvgDiskWriteQueue, dt_AvgDiskQueue, dt_CurruntQueue;
        protected int ServerNum;
        private string strColumnName_FreeDiskPer, strColumnName_FreeDiskByte, strColumName_DiskTime, strColumName_DiskIdleTime, strColumName_AvDiskRead, strColumName_AvgDiskWrite, strColumName_AvgDiskReadQueueLength, strColumName_AvgDiskWriteQueueLength;
        private string strColumnName_AvgDiskQueueLength, strColumnName_CurrentDiskQueueLength;
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
                //dt_FreeSpace_per 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P164"
                           select r;

                dt_FreeSpace_per = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_FreeSpace_per.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_FreeSpace_per.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_FreeSpace_per = Lib.ConvertingProc.SetDiskProc(dt_FreeSpace_per);
                dt_FreeSpace_per = Lib.ConvertingProc.Pivot(dt_FreeSpace_per, "Instancename", "TimeIn", "Pvalue");
                strColumnName_FreeDiskPer = Lib.ConvertingProc.GetColumname(dt_FreeSpace_per.Columns);
            }

            {
                //dt_FreeSpace_byte 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P018"
                           select r;

                dt_FreeSpace_byte = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_FreeSpace_byte.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_FreeSpace_byte.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_FreeSpace_byte = Lib.ConvertingProc.SetDiskProc(dt_FreeSpace_byte);
                dt_FreeSpace_byte = Lib.ConvertingProc.Pivot(dt_FreeSpace_byte, "Instancename", "TimeIn", "Pvalue");
                strColumnName_FreeDiskByte = Lib.ConvertingProc.GetColumname(dt_FreeSpace_byte.Columns);
            }

            {
                //dt_DiskTime 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P015"
                           select r;

                dt_DiskTime = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_DiskTime.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_DiskTime.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_DiskTime = Lib.ConvertingProc.SetDiskProc(dt_DiskTime);
                dt_DiskTime = Lib.ConvertingProc.Pivot(dt_DiskTime, "Instancename", "TimeIn", "Pvalue");
                strColumName_DiskTime = Lib.ConvertingProc.GetColumname(dt_DiskTime.Columns);
            }

            {
                //dt_DiskIdleTime 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P190"
                           select r;

                dt_DiskIdleTime = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_DiskIdleTime.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_DiskIdleTime.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_DiskIdleTime = Lib.ConvertingProc.SetDiskProc(dt_DiskIdleTime);
                dt_DiskIdleTime = Lib.ConvertingProc.Pivot(dt_DiskIdleTime, "Instancename", "TimeIn", "Pvalue");
                strColumName_DiskIdleTime = Lib.ConvertingProc.GetColumname(dt_DiskIdleTime.Columns);
            }

            {
                //dt_AvgDiskRead 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P191"
                           select r;

                dt_AvgDiskRead = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_AvgDiskRead.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_AvgDiskRead.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_AvgDiskRead = Lib.ConvertingProc.SetDiskProc(dt_AvgDiskRead);
                dt_AvgDiskRead = Lib.ConvertingProc.Pivot(dt_AvgDiskRead, "Instancename", "TimeIn", "Pvalue");
                strColumName_AvDiskRead = Lib.ConvertingProc.GetColumname(dt_AvgDiskRead.Columns);
            }


            {
                //dt_AvgDiskWrite 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P193"
                           select r;

                dt_AvgDiskWrite = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_AvgDiskWrite.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_AvgDiskWrite.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_AvgDiskWrite = Lib.ConvertingProc.SetDiskProc(dt_AvgDiskWrite);
                dt_AvgDiskWrite = Lib.ConvertingProc.Pivot(dt_AvgDiskWrite, "Instancename", "TimeIn", "Pvalue");
                strColumName_AvgDiskWrite = Lib.ConvertingProc.GetColumname(dt_AvgDiskWrite.Columns);
            }

            {
                //dt_AvgDiskReadQueue 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P197"
                           select r;

                dt_AvgDiskReadQueue = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_AvgDiskReadQueue.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_AvgDiskReadQueue.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_AvgDiskReadQueue = Lib.ConvertingProc.SetDiskProc(dt_AvgDiskReadQueue);
                dt_AvgDiskReadQueue = Lib.ConvertingProc.Pivot(dt_AvgDiskReadQueue, "Instancename", "TimeIn", "Pvalue");
                strColumName_AvgDiskReadQueueLength = Lib.ConvertingProc.GetColumname(dt_AvgDiskReadQueue.Columns);
            }

            {
                //dt_AvgDiskWriteQueue 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P198"
                           select r;

                dt_AvgDiskWriteQueue = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_AvgDiskWriteQueue.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_AvgDiskWriteQueue.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_AvgDiskWriteQueue = Lib.ConvertingProc.SetDiskProc(dt_AvgDiskWriteQueue);
                dt_AvgDiskWriteQueue = Lib.ConvertingProc.Pivot(dt_AvgDiskWriteQueue, "Instancename", "TimeIn", "Pvalue");
                strColumName_AvgDiskWriteQueueLength = Lib.ConvertingProc.GetColumname(dt_AvgDiskWriteQueue.Columns);
            }


            {
                //dt_AvgDiskQueue 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P017"
                           select r;

                dt_AvgDiskQueue = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_AvgDiskQueue.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_AvgDiskQueue.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_AvgDiskQueue = Lib.ConvertingProc.SetDiskProc(dt_AvgDiskQueue);
                dt_AvgDiskQueue = Lib.ConvertingProc.Pivot(dt_AvgDiskQueue, "Instancename", "TimeIn", "Pvalue");
                strColumnName_AvgDiskQueueLength = Lib.ConvertingProc.GetColumname(dt_AvgDiskQueue.Columns);
            }

            {
                //dt_CurruntQueue 데이터 테이블 만들기
                var test = from r in dt_PerfmonValue.AsEnumerable()
                           where r.Field<string>("PCID") == "P194"
                           select r;

                dt_CurruntQueue = dt_PerfmonValue.Clone();

                foreach (DataRow r in test)
                {
                    var newRow = dt_CurruntQueue.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_CurruntQueue.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                //dt_CurruntQueue = Lib.ConvertingProc.SetDiskProc(dt_CurruntQueue);
                dt_CurruntQueue = Lib.ConvertingProc.Pivot(dt_CurruntQueue, "Instancename", "TimeIn", "Pvalue");
                strColumnName_CurrentDiskQueueLength = Lib.ConvertingProc.GetColumname(dt_CurruntQueue.Columns);
            }


            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_FreeSpace_per, cplst, "FREEDISKSPACEPER_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_FreeSpace_byte, cplst, "FREEDISKSPACEMB_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_DiskTime, cplst, "DISKTIMEMS_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_DiskIdleTime, cplst, "DISKIDLETIME_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AvgDiskRead, cplst, "AVGDISKREAD_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AvgDiskWrite, cplst, "AVGDISKWRITE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AvgDiskReadQueue, cplst, "AVGDISKREADQUEUE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AvgDiskWriteQueue, cplst, "AVGDISKWRITEQUEUE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AvgDiskQueue, cplst, "AVGDISKQUEUE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_CurruntQueue, cplst, "CURRENTDISKQUEUE_LINE_CHART");
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumnName_FreeDiskPer, "", "FREEDISKSPACEPER_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumnName_FreeDiskByte, "", "FREEDISKSPACEMB_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_DiskTime, "", "DISKTIMEMS_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_DiskIdleTime, "", "DISKIDLETIME_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Checkpoint pages/sec", "", "CHECKPOINTPAGES_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Lazy writes/sec", "", "LAZYWRITE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Page reads/sec", "", "PAGEREAD_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Page writes/sec", "", "PAGEWRITE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_AvDiskRead, "", "AVGDISKREAD_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_AvgDiskWrite, "", "AVGDISKWRITE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_AvgDiskReadQueueLength, "", "AVGDISKREADQUEUE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumName_AvgDiskWriteQueueLength, "", "AVGDISKWRITEQUEUE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumnName_AvgDiskQueueLength, "", "AVGDISKQUEUE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumnName_CurrentDiskQueueLength, "", "CURRENTDISKQUEUE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
        private DataTable Func_SetColumn(DataTable dt)
        {
            DataTable dt_tmp = new DataTable();
            SortedDictionary<string, string> dicCol = new SortedDictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                if (!(dicCol.ContainsKey(dr["PCID"].ToString())))
                    dicCol.Add(dr["PCID"].ToString(), dr["PCountername"].ToString());
            }
            dt_tmp = Lib.ConvertingProc.Pivot(dt, dicCol, "PCID", "TimeIn", "PValue");
            return dt_tmp;
        }
        private DataTable Func_Sorting(DataTable dt)
        {
            dt.DefaultView.Sort = "TimeIn asc";
            return dt.DefaultView.ToTable();
        }
    }
}