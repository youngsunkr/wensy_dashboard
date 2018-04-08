using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace ServicePoint.ASHX
{
    /// <summary>
    /// ServerOverview의 요약 설명입니다.
    /// </summary>
    public class ServerOverview : IHttpHandler
    {

        public HttpRequest request;
        public HttpResponse response;
        private string callback
        {
            get
            {
                return request.QueryString["callback"].ToString();
            }
        }
        private int CompanyNum
        {
            get
            {
                return Lib.Util.TConverter<int>(request.QueryString["CompanyNum"].ToString());
            }
        }
        private int MemberNum
        {
            get
            {
                return Lib.Util.TConverter<int>(request.QueryString["MemberNum"].ToString());
            }
        }
        string strW_Dashboard_Cache, strW_Dashboard_Chart_Cache, strw_Dashboard_DiskFreeSpace_Company_Cache, strServerType;
        int numOverviewCnt;
        public void ProcessRequest(HttpContext context)
        {

            strServerType = "Windows";
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            request = context.Request;
            response = context.Response;
            //한그룹의 여러사용자가 사용할때 이미지저장시 꼬임방지위한 구분값을저장하는 키 
            string strCacheKey = CompanyNum.ToString() + "_overviewCnt";

            if (Lib.Util.GetCache(strCacheKey) == null)
            {
                Lib.Util.SetCache(strCacheKey, 0, 3600);
                numOverviewCnt = 0;
            }
            else
            {
                numOverviewCnt = Convert.ToInt32(Lib.Util.GetCache(strCacheKey));
                Lib.Util.SetCache(strCacheKey, numOverviewCnt + 1, 3600);
                numOverviewCnt = numOverviewCnt + 1;
            }
            strW_Dashboard_Cache = CompanyNum.ToString() + "_w_Dashboard_" + strServerType;
            strW_Dashboard_Chart_Cache = CompanyNum.ToString() + "_w_Dashboard_Chart_" + strServerType;
            strw_Dashboard_DiskFreeSpace_Company_Cache = CompanyNum.ToString() + "_w_Dashboard_DiskFreeSpace_Company_" + strServerType;
            if (Lib.Util.GetCache(strW_Dashboard_Cache) == null || Lib.Util.GetCache(strW_Dashboard_Chart_Cache) == null || Lib.Util.GetCache(strw_Dashboard_DiskFreeSpace_Company_Cache) == null)
                BindData();
            BindChart();
            context.Response.Write(callback + "({\"error\":0, \"desc\":\"success\"})");
        }
        private void BindChart()
        {
            DataTable dt_w_Dashboard = (DataTable)Lib.Util.GetCache(strW_Dashboard_Cache);
            DataTable dt_w_Dashboard_chart = (DataTable)Lib.Util.GetCache(strW_Dashboard_Chart_Cache);
            DataTable w_Dashboard_DiskFreeSpace_Company = (DataTable)Lib.Util.GetCache(strw_Dashboard_DiskFreeSpace_Company_Cache);
            foreach (DataRow dr in dt_w_Dashboard.Rows)
            {
                string strSuffix = dr["Servernum"].ToString() + "_" + (numOverviewCnt % 10).ToString();
                GridChart(dt_w_Dashboard, dt_w_Dashboard_chart, w_Dashboard_DiskFreeSpace_Company, strSuffix, Lib.Util.TConverter<int>(dr["Servernum"]));
            }
        }
        private void GridChart(DataTable dt_w_Dashboard, DataTable dt_w_Dashboard_chart, DataTable w_Dashboard_DiskFreeSpace_Company, string strSuffix, int ServerNum)
        {
            DataTable dt_w_Dashboard_tmp;
            var query = from data in dt_w_Dashboard_chart.AsEnumerable()
                        where (int)data["ServerNum"] == ServerNum
                        select data;

            if (query.Count() > 0)
            {
                dt_w_Dashboard_tmp = query.CopyToDataTable<DataRow>();
                dt_w_Dashboard_tmp.DefaultView.Sort = "TimeIn asc";
                dt_w_Dashboard_tmp = dt_w_Dashboard_tmp.DefaultView.ToTable();

            }
            else
            {
                dt_w_Dashboard_tmp = dt_w_Dashboard_chart.Clone();
            }

            DataTable dt_DiskFree = new DataTable();
            query = from data in w_Dashboard_DiskFreeSpace_Company.AsEnumerable()
                    where (int)data["ServerNum"] == ServerNum
                    orderby data["ServerNum"] ascending
                    select data;

            if (query.Count() > 0)
                dt_DiskFree = query.CopyToDataTable<DataRow>();
            else
            {
                dt_DiskFree = w_Dashboard_DiskFreeSpace_Company.Clone();
            }

            Chart chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.SetLineChart(chart_tmp, dt_w_Dashboard_tmp, "TimeIn", "TotalCPU", "KernelCPU", 101, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
            chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.SetColChart(chart_tmp, dt_w_Dashboard_tmp, "TimeIn", "PQL", 101, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
            chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.SetLineChart(chart_tmp, dt_w_Dashboard_tmp, "TimeIn", "FreeBytes", 101, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
            chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.SetLineChart(chart_tmp, dt_w_Dashboard_tmp, "TimeIn", "ReadTime", 101, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
            chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.SetLineChart(chart_tmp, dt_w_Dashboard_tmp, "TimeIn", "BytesTotal", 101, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
            chart_tmp = new Chart();
            chart_tmp = Lib.ChartClass.gridDiskBarChart(chart_tmp, dt_DiskFree, 0, 0);
            Lib.ChartClass.SaveChart(chart_tmp, strServerType, strSuffix);
        }
        private void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.W_dashboard(MemberNum, CompanyNum, "ALL");
            DataTable dt_w_Dashboard = Lib.ConvertingProc.w_Dashboard(cloud.dsReturn.Tables[0]);
            nReturn = cloud.w_Dashboard_Chart(MemberNum, CompanyNum, 15, "Windows");
            DataTable dt_w_Dashboard_chart = Lib.ConvertingProc.ChangeDashboardColumnName(cloud.dsReturn.Tables[0]);
            nReturn = cloud.w_Dashboard_DiskFreeSpace_Company(MemberNum, CompanyNum);
            DataTable dt_FreeDisk = Lib.ConvertingProc.SetDiskProc_All(cloud.dsReturn.Tables[0]);

            Lib.Util.SetCache(strW_Dashboard_Cache, dt_w_Dashboard, 15);
            Lib.Util.SetCache(strW_Dashboard_Chart_Cache, dt_w_Dashboard_chart, 15);
            Lib.Util.SetCache(strw_Dashboard_DiskFreeSpace_Company_Cache, dt_FreeDisk, 15);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}