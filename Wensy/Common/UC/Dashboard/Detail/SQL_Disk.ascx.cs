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
    public partial class SQL_Disk : System.Web.UI.UserControl
    {
        private DB.Cloud cloud;
        public int ServerNum;
        string strColumn_DISKTIME, strColumn_DISKIDLETIME, strColumn_CURRENTDISKQUEUELENGTH, strColumn_AVGDISKQUEUE, strColumn_AVGDISKBYTESREAD, strColumn_AVGDISKBYTESWRITE, strColumn_FREEDISKSPACEMB, strColumn_FREEDISKSPACEPER;

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
            DataTable dt_DISKTIME = new DataTable();
            DataTable dt_DISKIDLETIME = new DataTable();
            DataTable dt_CURRENTDISKQUEUELENGTH = new DataTable();
            DataTable dt_AVGDISKQUEUE = new DataTable();
            DataTable dt_AVGDISKBYTESREAD = new DataTable();
            DataTable dt_AVGDISKBYTESWRITE = new DataTable();
            DataTable dt_FREEDISKSPACEMB = new DataTable();
            DataTable dt_FREEDISKSPACEPER = new DataTable();

            int numDuration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ChartDataDuration"]);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P015");
            dt_DISKTIME = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_DISKTIME = Lib.ConvertingProc.GetColumname(dt_DISKTIME.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P190");
            dt_DISKIDLETIME = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_DISKIDLETIME = Lib.ConvertingProc.GetColumname(dt_DISKIDLETIME.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P194");
            dt_CURRENTDISKQUEUELENGTH = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_CURRENTDISKQUEUELENGTH = Lib.ConvertingProc.GetColumname(dt_CURRENTDISKQUEUELENGTH.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P017");
            dt_AVGDISKQUEUE = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_AVGDISKQUEUE = Lib.ConvertingProc.GetColumname(dt_AVGDISKQUEUE.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P191");
            dt_AVGDISKBYTESREAD = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_AVGDISKBYTESREAD = Lib.ConvertingProc.GetColumname(dt_AVGDISKBYTESREAD.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P193");
            dt_AVGDISKBYTESWRITE = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_AVGDISKBYTESWRITE = Lib.ConvertingProc.GetColumname(dt_AVGDISKBYTESWRITE.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P018");
            dt_FREEDISKSPACEMB = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_FREEDISKSPACEMB = Lib.ConvertingProc.GetColumname(dt_FREEDISKSPACEMB.Columns);

            cloud.w_PCID_Instance(ServerNum, numDuration, "P164");
            dt_FREEDISKSPACEPER = Lib.ConvertingProc.Pivot(cloud.dsReturn.Tables[0], "Instancename", "TimeIn", "PValue");
            strColumn_FREEDISKSPACEPER = Lib.ConvertingProc.GetColumname(dt_FREEDISKSPACEPER.Columns);


            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_DISKTIME, cplst, "DISKTIME_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_DISKIDLETIME, cplst, "DISKIDLETIME_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_CURRENTDISKQUEUELENGTH, cplst, "CURRENTDISKQUEUELENGTH_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AVGDISKQUEUE, cplst, "AVGDISKQUEUE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AVGDISKBYTESREAD, cplst, "AVGDISKBYTESREAD_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_AVGDISKBYTESWRITE, cplst, "AVGDISKBYTESWRITE_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_FREEDISKSPACEMB, cplst, "FREEDISKSPACEMB_LINE_CHART");
            cplst = Lib.Flotr2.SetArrayString_Lines(dt_FREEDISKSPACEPER, cplst, "FREEDISKSPACEPER_LINE_CHART");
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_DISKTIME, "", "DISKTIME_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_DISKIDLETIME, "", "DISKIDLETIME_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_CURRENTDISKQUEUELENGTH, "", "CURRENTDISKQUEUELENGTH_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_AVGDISKQUEUE, "", "AVGDISKQUEUE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_AVGDISKBYTESREAD, "", "AVGDISKBYTESREAD_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_AVGDISKBYTESWRITE, "", "AVGDISKBYTESWRITE_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_FREEDISKSPACEMB, "", "FREEDISKSPACEMB_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strColumn_FREEDISKSPACEPER, "", "FREEDISKSPACEPER_LINE_CHART", "LINE", 0, 0, 200, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }

    }
}