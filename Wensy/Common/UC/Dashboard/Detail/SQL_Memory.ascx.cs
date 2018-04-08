using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;

namespace ServicePoint.Common.UC.Dashboard.Detail
{
    public partial class SQL_Memory : System.Web.UI.UserControl
    {
        private DB.Cloud cloud;
        public int ServerNum;
        protected string strColumn_CacheHit, strColumn_CacheSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();
            RequestQueryString();
            BindData();
        }
        private void RequestQueryString()
        {
            HiddenField hdnfield = (HiddenField)Parent.FindControl("hdn_ServerNum");
            ServerNum = Convert.ToInt32(hdnfield.Value);
        }
        private void BindData()
        {
            DataTable dt_Memory = new DataTable();
            DataTable dt_ProcHit = new DataTable();
            DataTable dt_ProcSize = new DataTable();
            cloud.w_SQLMemory(ServerNum, Convert.ToInt32(ConfigurationManager.AppSettings["ChartDataDuration"]));
            //dt_Memory = Func_SetColum_SqlMemory(cloud.dsReturn.Tables[0]);
            dt_Memory = cloud.dsReturn.Tables[0];
            int numDuration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ChartDataDuration"]);
            
            cloud.w_PCID_Instance(ServerNum, numDuration, "P091");
            dt_ProcSize =Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0],"Instancename", "TimeIn", "PValue");
            strColumn_CacheSize = Lib.ConvertingProc.GetColumname(dt_ProcSize.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P138");
            dt_ProcHit = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_CacheHit = Lib.ConvertingProc.GetColumname(dt_ProcHit.Columns);

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_Memory, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ProcSize, cplst, "PROCCACHESIZE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ProcHit, cplst, "PROCCACHEHIT_LINE_CHART");
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Target Server Memory(MB)#c#Total Server Memory(MB)", "", "SERVERMEMORY_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Optimizer Memory(MB)#c#Connection Memory(MB)#c#Lock Memory(MB)#c#SQL Cache Memory(MB)", "", "MEMORYAREAS_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Buffer Cache Hit Ratio#c#Plan Cache Hit Ratio", "", "CACHERATES_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Plan Cache Pages(MB)#c#Stolen Server Memory(MB)#c#Free Memory(MB)#c#Buffer Cache Pages(MB)", "", "CACHESIZE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_CacheSize, "", "PROCCACHESIZE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_CacheHit, "", "PROCCACHEHIT_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
        private DataTable Func_SetColum_SqlMemory(DataTable dt)
        {
            SortedDictionary<string, string> colNm = new SortedDictionary<string, string>();
            colNm.Add("P100", "Target Server Memory(MB)");
            colNm.Add("P101", "Total Server Memory(MB)");
            colNm.Add("P138", "Plan Cache Hit Ratio");
            colNm.Add("P081", "Buffer Cache Hit Ratio");
            colNm.Add("P091", "Plan Cache Pages");
            colNm.Add("P090", "buffer Cache Pages");
            colNm.Add("P182", "SQL Cache Memory(MB)");
            colNm.Add("P183", "Stolen Server Memory(MB)");
            colNm.Add("P180", "Optimizer Memory(MB)");
            colNm.Add("P178", "Lock Memory(MB)");
            colNm.Add("P168", "Connection Memory(MB)");
            colNm.Add("P177", "Free Memory(MB)");
            dt = Lib.ConvertingProc.Pivot(dt, colNm, "PCID", "TimeIn", "PValue");
            foreach (DataRow dr in dt.Rows)
            {
                dr["Target Server Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Target Server Memory(MB)"])) / 1024), 2);
                dr["Total Server Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Total Server Memory(MB)"])) / 1024), 2);
                dr["Plan Cache Hit Ratio"] = Math.Round(((Convert.ToDouble(dr["Plan Cache Hit Ratio"]))), 2);
                dr["Buffer Cache Hit Ratio"] = Math.Round(((Convert.ToDouble(dr["Buffer Cache Hit Ratio"]))), 2);
                dr["Plan Cache Pages"] = Math.Round(((Convert.ToDouble(dr["Plan Cache Pages"])) * 8 / 1024), 2);
                dr["buffer Cache Pages"] = Math.Round(((Convert.ToDouble(dr["buffer Cache Pages"])) * 8 / 1024), 2);
                dr["SQL Cache Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["SQL Cache Memory(MB)"])) / 1024), 2);
                dr["Stolen Server Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Stolen Server Memory(MB)"])) / 1024), 2);
                dr["Optimizer Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Optimizer Memory(MB)"])) / 1024), 2);
                dr["Lock Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Lock Memory(MB)"])) / 1024), 2);
                dr["Connection Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Connection Memory(MB)"])) / 1024), 2);
                dr["Free Memory(MB)"] = Math.Round(((Convert.ToDouble(dr["Free Memory(MB)"]))), 2);
            }
            return dt;
        }

    }
}