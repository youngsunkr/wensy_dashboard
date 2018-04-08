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
    public partial class Win_CounterReport : Base
    {
        private DB.Cloud cloud;
        private DateTime dtmStart, dtmEnd;
        private DataTable dt_PerfmonValue, dt_PerfmonValue_select;
        protected int ServerNum;
        protected string strFreeDiskColumName, strParameter, strChartname, strColumnNames;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            cloud = new DB.Cloud();
            litScript.Text = "";
            if (!IsPostBack)
            {
                InitControl();
                RequestQueryString();
                BindControl();
            }
            else
            {
                RequestForm();
                BindControl();
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

            strParameter = ddl_Instance.SelectedValue;
            strChartname = ddl_Instance.SelectedItem.ToString();
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

            int nReturn = cloud.R_ServerList(MemberNum, "ALL");
            DataTable dt = cloud.dsReturn.Tables[0];
            ddl_Server.DataSource = dt;
            ddl_Server.DataTextField = "DisplayName";
            ddl_Server.DataValueField = "ServerNum";
            ddl_Server.DataBind();
        }
        private void BindControl()
        {
            ServerNum = Lib.Util.TConverter<int>(ddl_Server.SelectedValue);
            cloud.m_tbPCID_Server_Counter_List(ServerNum);
            DataTable dt = cloud.dsReturn.Tables[0];
            Func_Setddl_Instance(dt);
        }
        private void BindData()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ddl_Server.SelectedValue))
            {
                pnl_chart.Visible = true;
                cloud.get_ServerStatus(ServerNum.ToString());
                dt = cloud.dsReturn.Tables[0];
                gv_Info.DataSource = dt;
                gv_Info.DataBind();

                string strPCID = System.Text.RegularExpressions.Regex.Split(strParameter, "#c#")[0];
                string strInstanceName = System.Text.RegularExpressions.Regex.Split(strParameter, "#c#")[1];
                //if (strInstanceName.ToLower() == "allinstance")
                //    strInstanceName = "";

                //각 차트에서 참조해서 사용할 공통 데이터 테이블 생성
                cloud.R_Adhoc("select TimeIn_UTC, Data_JSON from tbPerfmonValues_JSON where TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum);

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

                //특정 카운터 데이터 테이블 만들기
                if (strInstanceName.ToLower() == "allinstance")
                {
                    var test = from r in dt_PerfmonValue.AsEnumerable()
                               where r.Field<string>("PCID") == strPCID.ToString()
                           select r;

                    dt_PerfmonValue_select = dt_PerfmonValue.Clone();

                    foreach (DataRow r in test)
                    {
                        var newRow = dt_PerfmonValue_select.NewRow();
                        newRow.ItemArray = r.ItemArray;
                        dt_PerfmonValue_select.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                    }
                }
                else
                {
                    var test = from r in dt_PerfmonValue.AsEnumerable()
                               where r.Field<string>("PCID") == strPCID && r.Field<string>("InstanceName") == strInstanceName
                           select r;

                    dt_PerfmonValue_select = dt_PerfmonValue.Clone();

                    foreach (DataRow r in test)
                    {
                        var newRow = dt_PerfmonValue_select.NewRow();
                        newRow.ItemArray = r.ItemArray;
                        dt_PerfmonValue_select.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                    }
                }

                ////cloud.R_Windows_Perfmon(ServerNum, dtmStart, dtmEnd, strPCID, strInstanceName);
                //if (strInstanceName.ToLower() == "allinstance")
                //    cloud.R_Adhoc("select TImeIn,InstanceName,PValue from tbPerfmonValues where TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = '" + strPCID + "'");
                //else
                //    cloud.R_Adhoc("select TImeIn,InstanceName,PValue from tbPerfmonValues where TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = '" + strPCID + "' and InstanceName = '" + strInstanceName + "'");
 

                dt = Func_SetColumn(dt_PerfmonValue_select);
                strColumnNames = Lib.ConvertingProc.GetColumname(dt.Columns);
                gv_List.DataSource = dt;
                gv_List.DataBind();

                //챠트바인드
                List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
                StringBuilder sb = new StringBuilder();
                cplst = SetChartProperty();
                cplst = Lib.Flotr2.SetArrayString_Lines(dt, cplst);
                sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
                litScript.Text += Lib.Util.BoxingScript(sb.ToString());

            }
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumnNames, "", "LINE_CHART", "LINE", 0, 0, 500, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
        private void Func_Setddl_Instance(DataTable dt)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                string InstanceName = "";
                if (string.IsNullOrEmpty(dr["InstanceName"].ToString()))
                    InstanceName = "AllInstance";
                else
                    InstanceName = dr["InstanceName"].ToString();
                list.Add(dr["PCID"].ToString() + "#c#" + InstanceName, dr["PObjectname"].ToString() + " " + dr["PcounterName"].ToString() + "  [" + InstanceName + "]");
            }
            ddl_Instance.DataSource = list;
            ddl_Instance.DataValueField = "Key";
            ddl_Instance.DataTextField = "Value";
            ddl_Instance.DataBind();

        }
        private DataTable Func_SetColumn(DataTable dt)
        {
            DataTable dt_tmp = new DataTable();
            dt_tmp = ServicePoint.Lib.ConvertingProc.Pivot(dt, "Instancename", "TimeIn", "Pvalue");
            return dt_tmp;
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}