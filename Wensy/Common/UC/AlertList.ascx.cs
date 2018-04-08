using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

namespace ServicePoint.Common.UC
{
    public partial class AlertList : System.Web.UI.UserControl
    {
        public int numMember;
        public int numCompany;
        public string strServerType;
        DataTable dtAlerts = new DataTable();
        public int Member
        {
            get
            {
                return numMember;
            }
            set
            {
                numMember = value;
            }
        }
        public int Company
        {
            get
            {
                return numCompany;
            }
            set
            {
                numCompany = value;
            }
        }
        public string ServerType
        {
            get
            {
                return strServerType;
            }
            set
            {
                strServerType = value;
            }
        }
        public DataTable dt_AlertList
        {
            get { return dtAlerts; }
            set { dtAlerts = value; }
        }
        bool bol_Critical;
        bool bol_Warning;
        bool bol_Information;
        protected void Page_Load(object sender, EventArgs e)
        {
            EnableViewState = true;

            if (!IsPostBack || ddl_Server.Items.Count == 0)
            {
                InitControl();
            }
            RequestForm();
            BindDate();

        }

        private void RequestForm()
        {
            string strServerNum = ddl_Server.SelectedValue;
            bol_Critical = chk_Critical.Checked;
            bol_Information = chk_Information.Checked;
            bol_Warning = chk_Warning.Checked;

        }
        public void InitControl()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("ALL", "ALL");
            list.Add("Windows", "Windows");
            list.Add("Web", "Web");
            list.Add("SQL", "SQL");
            string MyServerList = "";
            DB.Cloud cloud = new DB.Cloud();
            cloud.SetCmd("Cloud");
            cloud.get_ServerList(numMember);

            foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            {
                list.Add(dr["ServerNum"].ToString(), dr["DIsPlayName"].ToString());
            }

            ddl_Server.DataSource = list;
            ddl_Server.DataValueField = "Key";
            ddl_Server.DataTextField = "Value";
            ddl_Server.DataBind();
        }
        public void BindDate()
        {
            string strServerNum = ddl_Server.SelectedValue;
            IEnumerable<DataRow> queryResult;
            DataTable dt = new DataTable();
            if (strServerNum.ToUpper() == "ALL" || strServerNum.ToUpper() == "WINDOWS" || strServerNum.ToUpper() == "WEB" || strServerNum.ToUpper() == "SQL")
            {
                if (strServerNum.ToUpper() == "ALL")
                {
                    dt = dtAlerts;
                }
                else
                {
                    queryResult = from row in dtAlerts.AsEnumerable() where row.Field<string>("ServerType") == strServerNum select row;
                    if (queryResult.Count() == 0)
                    {
                        dt = dtAlerts.Clone();
                    }
                    else
                        dt = queryResult.CopyToDataTable<DataRow>();
                }
            }
            else
            {
                queryResult = from row in dtAlerts.AsEnumerable() where row.Field<string>("ServerNum") == strServerNum select row;
                if (queryResult.Count() == 0)
                {
                    dt = dtAlerts.Clone();
                }
                else
                    dt = queryResult.CopyToDataTable<DataRow>();

            }

            if (!(dt.Columns.Contains("AlertLevel")))
                dt.Columns.Add("AlertLevel");

            foreach (DataRow dr in dt.Rows)
            {
                if (String.IsNullOrEmpty(dr["AlertStatus"].ToString()))
                {
                    dr["AlertStatus"] = 1;
                    dr["AlertLevel"] = "Information";
                }
                if (Convert.ToInt32(dr["AlertStatus"]) == 1)
                {
                    dr["AlertLevel"] = "Information";
                }
                if (Convert.ToInt32(dr["AlertStatus"]) == 2)
                {
                    dr["AlertLevel"] = "Warning";
                }
                if (Convert.ToInt32(dr["AlertStatus"]) == 3)
                {
                    dr["AlertLevel"] = "Critical";
                }
            }
            // Information Warning Critical
            IEnumerable<DataRow> query_Critical = from row in dt.AsEnumerable() where row.Field<string>("AlertLevel") == "Critical" select row;
            DataTable dt_Critical = new DataTable();
            if (query_Critical.Count() != 0)
            {
                dt_Critical = query_Critical.CopyToDataTable<DataRow>();
            }

            IEnumerable<DataRow> query_Infomation = from row in dt.AsEnumerable() where row.Field<string>("AlertLevel") == "Information" select row;
            DataTable dt_Infomation = new DataTable();
            if (query_Infomation.Count() != 0)
            {
                dt_Infomation = query_Infomation.CopyToDataTable<DataRow>();
            }

            IEnumerable<DataRow> query_Warning = from row in dt.AsEnumerable() where row.Field<string>("AlertLevel") == "Warning" select row;
            DataTable dt_Warning = new DataTable();
            if (query_Warning.Count() != 0)
            {
                dt_Warning = query_Warning.CopyToDataTable<DataRow>();
            }

            DataTable dt_Bind = new DataTable();
            dt_Bind = dt.Clone();
            if (bol_Critical)
                dt_Bind.Merge(dt_Critical);
            if (bol_Warning)
                dt_Bind.Merge(dt_Warning);
            if (bol_Information)
                dt_Bind.Merge(dt_Infomation);

            dt_Bind.DefaultView.Sort = "TimeIn desc";
            dt_Bind = dt_Bind.DefaultView.ToTable();
            gv_List.DataSource = dt_Bind;
            gv_List.DataBind();
            dt.Clear();
            dt.Dispose();
        }


        protected void gv_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //[1] Level 컬럼 배경색 지정
                if (e.Row.Cells[4].Text == "Critical")
                {
                    e.Row.Cells[4].BackColor = ColorTranslator.FromHtml("#ff2d19");
                }
                else if (e.Row.Cells[4].Text == "Warning")
                {
                    e.Row.Cells[4].BackColor = ColorTranslator.FromHtml("#fa6800");
                }
                else if (e.Row.Cells[4].Text == "Information")
                {
                    e.Row.Cells[4].BackColor = ColorTranslator.FromHtml("#a4c400");
                }
                else
                {
                    e.Row.Cells[4].BackColor = ColorTranslator.FromHtml("#60a917");
                }

                //[2] Last Occured 컬럼 배경색 지정: 최근 1분내의 레코드면 LightSteelBlue, 나머진 White

                //TimeSpan ts = DateTime.Now - Convert.ToDateTime(DateTime.ParseExact(e.Row.Cells[4].Text, "yyyy-MM-dd HH:mm:ss", null));
                //if (ts.TotalMinutes <= 1)
                //{
                //    e.Row.Cells[4].BackColor = Color.Gainsboro;
                //}
                //else
                //{
                //    e.Row.Cells[4].BackColor = ColorTranslator.FromHtml("#eeeeee");
                //}

            }
        }
        public static string RedirectToAlertDetailPage(object name, object hostName, object reasonCode, object instanceName, object ServerNum, object dtTimeIn, object dtTimeIn_UTC)
        {
            // ALERT LINK URL

            string strUrl = "";
            string strName = name.ToString();
            string strHostName = hostName.ToString();
            string strReasonCode = reasonCode.ToString();
            string strInstanceName = instanceName.ToString();
            string strServerNumber = ServerNum.ToString();

            DateTime dtLastUpdate = new DateTime();
            dtLastUpdate = Convert.ToDateTime(dtTimeIn);
            string strTimeIn = dtLastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime dtLastUpdate_UTC = new DateTime();
            dtLastUpdate_UTC = Convert.ToDateTime(dtTimeIn_UTC);
            string strTimeIn_UTC = dtLastUpdate_UTC.ToString("yyyy-MM-dd HH:mm:ss");

            strUrl = "/Common/UC/AlertDetail.aspx";

            strUrl += String.Format("?ServerType={0}&HostName={1}&ReasonCode={2}&InstanceName={3}&SN={4}&TimeIn={5}&TimeIn_UTC={6}", strName, strHostName, strReasonCode, strInstanceName, strServerNumber, strTimeIn, strTimeIn_UTC);
            strUrl = strUrl.Replace("#", "URSR13");
            return strUrl;
        }

    }
}