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
    public partial class SQL_DatabasesFile : System.Web.UI.UserControl
    {
        private DB.Cloud cloud;
        public int ServerNum;
        string strColumn_TotalDatabase, strColumn_DataFileSize, strColumn_LogSize, strColumn_TotalVlfCount, strColumn_ActiveVlfCount;

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
            int numDuration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ChartDataDuration"]);

            DataTable dt = new DataTable();
            cloud.w_SQLDatabasesFileSize(ServerNum, numDuration);
            dt = cloud.dsReturn.Tables[0];
            DataTable dt_Latest = cloud.dsReturn.Tables[0].Clone();

            var groupDatatable = from tbl in dt.AsEnumerable()
                                 group tbl by new { DatabaseName = tbl["DatabaseName"] } into groupby
                                 select new
                                 {
                                     value = groupby.Key
                                 ,
                                     maxDate = groupby.Max(e => e.Field<DateTime>("TimeIn"))
                                 ,
                                     columnvalue = groupby
                                 ,
                                 };

            foreach (var key in groupDatatable)
            {

                var lastTime = key.maxDate;

                foreach (var col in key.columnvalue)
                {
                    if (lastTime.ToString() == col["TimeIn"].ToString())
                    {
                        //                        DataRow dr = col;
                        dt_Latest.ImportRow(col);
                    }
                }
            }

            gv_List.DataSource = dt_Latest;
            gv_List.DataBind();
            int maxValue = Convert.ToInt32(dt.Compute("max(Total_Databases_Size_MB)", string.Empty));
            DataTable dt_TotalDatabase = Lib.ConvertingProc.Pivot(dt, "databaseName", "TimeIn", "Total_Databases_Size_MB");
            strColumn_TotalDatabase = Lib.ConvertingProc.GetColumname(dt_TotalDatabase.Columns);
            DataTable dt_DataFileSize = Lib.ConvertingProc.Pivot(dt, "databaseName", "TimeIn", "Datafile_Size_MB");
            strColumn_DataFileSize = Lib.ConvertingProc.GetColumname(dt_TotalDatabase.Columns);
            DataTable dt_LogSize = Lib.ConvertingProc.Pivot(dt, "databaseName", "TimeIn", "Log_Size_MB");
            strColumn_LogSize = Lib.ConvertingProc.GetColumname(dt_TotalDatabase.Columns);
            DataTable dt_TotalVlfCount = Lib.ConvertingProc.Pivot(dt, "databaseName", "TimeIn", "Total_vlf_Cnt");
            strColumn_TotalVlfCount = Lib.ConvertingProc.GetColumname(dt_TotalDatabase.Columns);
            DataTable dt_ActiveVlfCount = Lib.ConvertingProc.Pivot(dt, "databaseName", "TimeIn", "Active_vlf_Cnt");
            strColumn_ActiveVlfCount = Lib.ConvertingProc.GetColumname(dt_TotalDatabase.Columns);


            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_TotalDatabase, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_DataFileSize, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_LogSize, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_TotalVlfCount, cplst);
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_ActiveVlfCount, cplst);

            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_TotalDatabase, "", "TOTALDATABASESSIZEMB_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_DataFileSize, "", "DATAFILESIZEMB_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_LogSize, "", "LOGSIZEMB_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_TotalVlfCount, "", "TOTALVLFCOUNT_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_ActiveVlfCount, "", "ACTIVEVLFCOUNT_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
    }
}