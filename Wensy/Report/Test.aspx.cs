using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
namespace ServicePoint.Report
{
    public partial class Test : Base
    {
        private DB.Cloud cloud;
        private DateTime dtmStart, dtmEnd;
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
            int nReturn = cloud.R_ServerList(MemberNum, "ALL");
            DataTable dt = cloud.dsReturn.Tables[0];
            ddl_Server.DataSource = dt;
            ddl_Server.DataTextField = "DisplayName";
            ddl_Server.DataValueField = "ServerNum";
            ddl_Server.DataBind();
        }
        private void BindData()
        {
            DataTable dt_rptData = new DataTable();
            // 챠트그릴 리스트를 가져온다
            cloud.R_Adhoc("SELECT TOP 1 Data_JSON FROM tbPerfmonValues_JSON where ServerNum = " + ServerNum + " and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' order by TimeIn_UTC desc");
            //챠트그릴 리스트가 잇다면
            if (cloud.dsReturn.Tables.Count > 0 && cloud.dsReturn.Tables[0].Rows.Count > 0)
            {

                //1단계 리피트에 챠트디자인

                dt_rptData = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                dt_rptData.DefaultView.Sort = "PCounterName asc";

            }
            rpt_Chart.DataSource = dt_rptData.DefaultView.ToTable();
            rpt_Chart.DataBind();

            //2단계 디자인된챠트의 헤더값을 바인딩하여  챠트그리기
            if (dt_rptData.Rows.Count > 0)
            {
                DataTable dt_PerfmonValue = new DataTable();
                List<DataTable> dt_list = new List<DataTable>();
                cloud.R_Adhoc("SELECT servernum, Data_JSON FROM tbPerfmonValues_JSON where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum+" order by TimeIn_UTC desc");

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

                List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
                StringBuilder sb = new StringBuilder();
                cplst = Lib.Flotr2.SetArrayString_Lines_Report(dt_PerfmonValue, SetChartProperty(dt_rptData));
                
                sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
                litScript.Text += Lib.Util.BoxingScript(sb.ToString());
            }
        }
        private List<Lib.chartProperty> SetChartProperty(DataTable dt_rptTable)
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            //dt_rptData 를 반복문으로 x값 y 값 챠트명을 바인딩시켜준다~
            
            foreach (DataRow dr in dt_rptTable.Rows)
            {
                string strChartNm = IncodingChartName(dr["PObjectName"].ToString(), dr["PCounterName"].ToString(), dr["InstanceName"].ToString())+"_LINE_CHART" ;
                cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PValue", "", strChartNm, "LINE", 0, 0, 130, "false", "false", "false", "true", "true", "true",dr["PObjectName"].ToString(), dr["PCounterName"].ToString(), dr["InstanceName"].ToString()));
            }
            //cpList.Add(Lib.Flotr2.chartProperty("", strFreeDiskColumName, "", "_FREEDISK_BAR_CHART", "PIE", 0, 0, 130, "false", "false"));
            //cpList.Add(Lib.Flotr2.chartProperty("FreePercent", "Drive", "FreeGB", "FREEDISK_BAR_CHART", "BAR", 100, 0, 130, "false", "false"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "BufferCacheHit", "", "BUFFERCACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            return cpList;
        }
        protected void rpt_Chart_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (rpt_Chart.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlGenericControl noRecordsDiv = (e.Item.FindControl("NoRecords") as HtmlGenericControl);
                    if (noRecordsDiv != null)
                    {
                        noRecordsDiv.Visible = true;
                    }
                }
            }
        }

        public string IncodingChartName(string PObejctName, string PCounter, string InstanceName)
        {
          
            string strReturn = Regex.Replace(PObejctName+PCounter+InstanceName, @"[^a-zA-Z0-9가-힣_]", "", RegexOptions.Singleline);

            return strReturn;
        }
        public string IncodingTitleName(string PObejctName, string PCounter, string InstanceName)
        {

            string strReturn =PObejctName+"_"+ PCounter;
            if (InstanceName != "")
            {
                strReturn = strReturn+" [" + InstanceName+"]";
            }

            return strReturn;
        }

    }
}