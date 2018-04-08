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
    public partial class SQL_Configuration : Base
    {
        private DateTime dtmStart, dtmEnd;
        private DB.Cloud cloud;
        private DataTable dt_FreeDisk;
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
               
            }
            BindData();
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
            if (Request.QueryString.AllKeys.Contains("ServerNum"))
            {
                ServerNum = Lib.Util.TConverter<int>(Request.QueryString["ServerNum"]);
                ddl_Server.SelectedValue = ServerNum.ToString();
            }

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

                cloud.W_SQLConfiguration(Lib.Util.TConverter<int>(ddl_Server.SelectedValue));
                gv_Config.DataSource = cloud.dsReturn.Tables[0];
                gv_Config.DataBind();
            }
        }
    }
}