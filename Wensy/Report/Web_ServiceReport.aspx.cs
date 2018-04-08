using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace ServicePoint.Report
{
    public partial class Web_ServiceReport : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_FreeDisk;
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

            //cloud.R_WEB_TimeTaken(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            cloud.R_Adhoc("SELECT TOP(20) URI,AVG(AvgTimeTaken) AS [Average Time Taken], MAX(MaxTimeTaken) AS [Max Time Taken] FROM tbIISLog WHERE TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum + " GROUP BY URI ORDER BY [Average Time Taken] DESC");
            DataTable dt_Web_TimeTaken = cloud.dsReturn.Tables[0];
            gv_List_AvgTimeTaken.DataSource = dt_Web_TimeTaken;
            gv_List_MaxTimeTaken.DataSource = dt_Web_TimeTaken;
            gv_List_AvgTimeTaken.DataBind();
            gv_List_MaxTimeTaken.DataBind();

            //cloud.R_WEB_Byte(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            cloud.R_Adhoc("SELECT TOP(20) URI,SUM(CAST(SCBytes AS float)) AS [Total Bytes from Server], SUM(Hits) AS [Total Hits] FROM tbIISLog WHERE TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum + " GROUP BY URI ORDER BY [Total Bytes from Server] DESC");
            DataTable dt_Web_Byte = cloud.dsReturn.Tables[0];
            gv_List_TotalBytes.DataSource = dt_Web_Byte;
            gv_List_TotalHit.DataSource = dt_Web_Byte;
            gv_List_TotalBytes.DataBind();
            gv_List_TotalHit.DataBind();
            
             
            //cloud.R_WEB_RequestStatus(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            cloud.R_Adhoc("SELECT ValueDescription,LogValue,SUM(TotalNumber) AS [Total] FROM tbIISRequestStatus WHERE TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum + " GROUP BY ValueDescription, LogValue ORDER BY ValueDescription, Total DESC");
            DataTable dt_Web_Request = cloud.dsReturn.Tables[0];

            DataTable dt_Web_Request_hit = new DataTable();
            var query = from data in dt_Web_Request.AsEnumerable()
                        where data["ValueDescription"].ToString() == "TOP 20 Hits"
                        orderby data["Total"] descending
                        select data;
            if (query.Count() > 0)
            {
                dt_Web_Request_hit = query.CopyToDataTable<DataRow>();
                dt_Web_Request_hit.Columns["LogValue"].ColumnName = "hit";
            }
            gv_List_Hit.DataSource = dt_Web_Request_hit;
            gv_List_Hit.DataBind();

            DataTable dt_Web_Request_App_hit = new DataTable();
            query = from data in dt_Web_Request.AsEnumerable()
                    where data["ValueDescription"].ToString() == "TOP 20 Application Hits"
                    orderby data["Total"] descending
                    select data;
            if (query.Count() > 0)
            {
                dt_Web_Request_App_hit = query.CopyToDataTable<DataRow>();
                dt_Web_Request_App_hit.Columns["LogValue"].ColumnName = "App_hit";
            }
            gv_List_App_Hit.DataSource = dt_Web_Request_App_hit;
            gv_List_App_Hit.DataBind();

            DataTable dt_Web_Request_IpAddr = new DataTable();
            query = from data in dt_Web_Request.AsEnumerable()
                    where data["ValueDescription"].ToString() == "TOP 20 Clients IP"
                    orderby data["Total"] descending
                    select data;
            if (query.Count() > 0)
            {
                dt_Web_Request_IpAddr = query.CopyToDataTable<DataRow>();
                dt_Web_Request_IpAddr.Columns["LogValue"].ColumnName = "IP";
            }
            gv_List_IP.DataSource = dt_Web_Request_IpAddr;
            gv_List_IP.DataBind();

            DataTable dt_Web_Request_BytePerExtension = new DataTable();
            query = from data in dt_Web_Request.AsEnumerable()
                    where data["ValueDescription"].ToString() == "TOP 20 Bytes per Extension"
                    orderby data["Total"] descending
                    select data;
            if (query.Count() > 0)
            {
                dt_Web_Request_BytePerExtension = query.CopyToDataTable<DataRow>();
                dt_Web_Request_BytePerExtension.Columns["LogValue"].ColumnName = "BytePerExtension";
            }
            gv_List_BytePerExtenstion.DataSource = dt_Web_Request_BytePerExtension;
            gv_List_BytePerExtenstion.DataBind();

            //cloud.R_WEB_Errors(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            cloud.R_Adhoc("SELECT TOP(20) URI, SUM(Hits) AS [Total Hits], StatusCode as [Status Code], Win32StatusCode as [Win32 Status Code] FROM tbIISLog WHERE TimeIn >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum + " and StatusCode >= 400 GROUP BY URI, StatusCode, Win32StatusCode ORDER BY [Total Hits] DESC");
            DataTable dt_Error = cloud.dsReturn.Tables[0];
            gv_List_Err.DataSource = dt_Error;
            gv_List_Err.DataBind();

            //cloud.R_WEB_ServiceStatus(Lib.Util.TConverter<int>(ddl_Server.SelectedValue), dtmStart, dtmEnd);
            cloud.R_Adhoc("SELECT SUM(TotalHits) AS [Total Hits], SUM(TotalSCBytes) AS [Total Bytes from Server], SUM(TotalCSBytes) AS [Total Bytes from Clients], SUM(TotalCIP) AS [Total Client IP], SUM(TotalErrors) AS [Total Errors] FROM tbIISServiceStatus WHERE TimeIn >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + " ' and ServerNum = " + ServerNum);
            DataTable dt_ServiceStatus = cloud.dsReturn.Tables[0];
            gv_List_ServiceStatus.DataSource = dt_ServiceStatus;
            gv_List_ServiceStatus.DataBind();

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_TimeTaken, SetChartProperty());
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Byte, cplst);
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_hit, cplst);
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_App_hit, cplst);
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_BytePerExtension, cplst);
            cplst = Lib.Flotr2.SetArrayString_Cols_Horizon(dt_Web_Request_IpAddr, cplst);
            //cplst = Lib.Flotr2.SetArrayString_Lines(dt, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("URI", "Average Time Taken", "", "AVGTIMETAKEN_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("URI", "Max Time Taken", "", "MAXTIMETAKEN_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("URI", "Total Bytes from Server", "", "TOTALBYTES_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("URI", "Total Hits", "", "TOTALHIT_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("hit", "Total", "", "HIT_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("App_hit", "Total", "", "APPHIT_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("IP", "Total", "", "IP_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            cpList.Add(Lib.Flotr2.chartProperty("BytePerExtension", "Total", "", "BYTEPEREXTENSION_COLSHORIZON_CHART", "COLSHORIZON", 0, 0, 400, "false", "true", "true", "true", "false", "false"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Processor Time#c#Privileged Time", "", "CPU_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Processor Time#c#PQL", "", "PROCESSORQUEUELENGTH_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Committed Bytes", "Available Mbytes", "COMMITTEDBYTES_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Disk Time", "Avg. Disk sec/Read", "DISKTIME_LINE_CHART", "DUALLINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "Bytes Received#c#Bytes Sent#c#Bytes Total", "", "NETWORKINTERFACE_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", strFreeDiskColumName, "", "FREEDISK_LINE_CHART", "LINE", 0, 0, 130, "false", "false", "true", "false", "true", "true"));
            //cpList.Add(Lib.Flotr2.chartProperty("", strFreeDiskColumName, "", "FREEDISK_PIE_CHART", "PIE", 0, 0, 130, "false", "false"));
            //cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "BufferCacheHit", "", "BUFFERCACHEHIT_LINE_CHART", "LINE", 102, 0, 40, "false", "false"));
            return cpList;
        }
    }
}