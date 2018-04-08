using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace ServicePoint.Common.UC
{
    public partial class AlertDetail : Base
    {
        private DataTable dt_SQLQuery, dt_SQLCurrentQuery;
        private string strHostName, strReasonCode, strInstanceName, strServerType;
        public int ServerNum;
        private DB.Cloud cloud;
        private DateTime dtmTimeIn;
        private DateTime dtmTimeIn_UTC;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            cloud = new DB.Cloud();
            RequestQueryString();

            if (strServerType.ToLower() == "sql")
                BindSql();
            else
                BindWindow();

        }
        private void RequestQueryString()
        {
            strServerType = "";
            if (Request.QueryString.AllKeys.Contains("ServerType"))
                strServerType = Request.QueryString["ServerType"].ToString();

            if (Request.QueryString.AllKeys.Contains("HostName"))
                strHostName = Request.QueryString["HostName"].ToString();

            if (Request.QueryString.AllKeys.Contains("ReasonCode"))
                strReasonCode = Request.QueryString["ReasonCode"].ToString();

            if (Request.QueryString.AllKeys.Contains("InstanceName"))
                strInstanceName = Request.QueryString["InstanceName"].ToString();


            if (Request.QueryString.AllKeys.Contains("TimeIn"))
                dtmTimeIn = Lib.Util.TConverter<DateTime>(Request.QueryString["TimeIn"].ToString());

            if (Request.QueryString.AllKeys.Contains("TimeIn_UTC"))
                dtmTimeIn_UTC = Lib.Util.TConverter<DateTime>(Request.QueryString["TimeIn_UTC"].ToString());

            if (Request.QueryString.AllKeys.Contains("SN"))
                ServerNum = Lib.Util.TConverter<int>(Request.QueryString["SN"]);
        }
        private void BindWindow()
        {
            pnl_Window.Visible = true;

            //gv2
            cloud.w_tbAppTrace(ServerNum, dtmTimeIn_UTC);
            DataTable dt_AppTrace = cloud.dsReturn.Tables[0];
            gv_Window_Class.DataSource = dt_AppTrace;
            gv_Window_Class.DataBind();

            //label 이름
            cloud.w_ChartSubject(CompanyNum, MemberNum, strReasonCode);
            DataTable dt_ChartSubject = cloud.dsReturn.Tables[0];
            if (dt_ChartSubject != null)
            {
                if (dt_ChartSubject.Rows.Count > 0)
                {
                    lbl_Window.Text = dt_ChartSubject.Rows[0][0].ToString();
                    lbl_Window_Sub.Text = dt_ChartSubject.Rows[0][0].ToString();
                }
            }
            //gv1
            cloud.w_AlertDetail_Table(CompanyNum, MemberNum, ServerNum, strInstanceName, strReasonCode);
            DataTable dt_Descript = cloud.dsReturn.Tables[0];
            gv_List_Window.DataSource = dt_Descript;
            gv_List_Window.DataBind();

            //chart
            cloud.w_AlertDetail_Chart(CompanyNum, MemberNum, ServerNum, strInstanceName, strReasonCode);
            DataTable dt_ChartValue = cloud.dsReturn.Tables[0];

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ChartValue, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());

        }
       
        private void BindSql()
        {
            pnl_SQL.Visible = true;

            //gv2
            //실행 쿼리 데이터 읽어오기
            cloud.R_Adhoc("select TimeIn_UTC, Data_JSON from tbSQLCurrentExecution_JSON where Timein_UTC = '" + dtmTimeIn_UTC.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum);

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
                
                foreach (DataRow r in test)
                {
                    var newRow = dt_SQLCurrentQuery.NewRow();
                    newRow.ItemArray = r.ItemArray;
                    dt_SQLCurrentQuery.Rows.Add(newRow);//I'm doubtful if you need to call this or not
                }

                rpt_Query.DataSource = dt_SQLCurrentQuery;
                rpt_Query.DataBind();
            }
            

            //label 이름
            cloud.w_ChartSubject(CompanyNum, MemberNum, strReasonCode);
            DataTable dt_ChartSubject = cloud.dsReturn.Tables[0];
            if (dt_ChartSubject != null)
            {
                if (dt_ChartSubject.Rows.Count > 0)
                {
                    lbl_Sql.Text = dt_ChartSubject.Rows[0][0].ToString();
                    lbl_Sql_Sub.Text = dt_ChartSubject.Rows[0][0].ToString();
                }
            }
            //gv1
            cloud.w_AlertDetail_Table(CompanyNum, MemberNum, ServerNum, strInstanceName, strReasonCode);
            DataTable dt_Descript = cloud.dsReturn.Tables[0];
            gv_List_Sql.DataSource = dt_Descript;
            gv_List_Sql.DataBind();

            //chart
            cloud.w_AlertDetail_Chart(CompanyNum, MemberNum, ServerNum, strInstanceName, strReasonCode);
            DataTable dt_ChartValue = cloud.dsReturn.Tables[0];

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty_SQL();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ChartValue, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PValue", "", "WINDOW_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
           
            return cpList;
        } 
        private List<Lib.chartProperty> SetChartProperty_SQL()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PValue", "", "SQL_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            return cpList;
        } 
    }
}