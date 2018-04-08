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
    public partial class SQL_CPU : System.Web.UI.UserControl
    {
        
        private DB.Cloud cloud;
        public int ServerNum;
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
            DataTable dt = new DataTable();

            cloud.w_CPUUsage_Detail(ServerNum);
            dt = cloud.dsReturn.Tables[0];
            gv_List.DataSource = Func_dtValueSet(dt);
            gv_List.DataBind();

            cloud.w_CPUUsage(ServerNum, Convert.ToInt32(ConfigurationManager.AppSettings["ChartDataDuration"]));
            dt = cloud.dsReturn.Tables[0];

            //챠트바인드
            List<Lib.chartProperty> cplst = new List<Lib.chartProperty>();
            StringBuilder sb = new StringBuilder();
            cplst = SetChartProperty();
            cplst = Lib.Flotr2.SetArrayString_Lines(dt, cplst);
            sb = Lib.Flotr2.SetStringValue(cplst, sb, ServerNum.ToString());
            litScript_Pop.Text += Lib.Util.BoxingScript(sb.ToString());
        }
        private List<Lib.chartProperty> SetChartProperty()
        {
            List<Lib.chartProperty> cpList = new List<Lib.chartProperty>();
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "TotalCPU#c#KernelCPU#c#UserCPU", "", "CPU_LINE_CHART", "LINE", 0, 0, 300, "false", "false", "true", "true", "true", "true"));
            cpList.Add(Lib.Flotr2.chartProperty("TimeIn", "PQL", "", "QUEUE_LINE_CHART", "LINE", 0, 0, 300, "false", "false", "true", "true", "true", "true"));

            return cpList;
        }
        private DataTable Func_dtValueSet(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                dr["PValue"] = Math.Round(Convert.ToDouble(dr["PValue"]), 2);
            }
            return dt;
        }
    }
}