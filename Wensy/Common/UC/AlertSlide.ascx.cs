using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ServicePoint.Lib;

namespace ServicePoint.Common.UC
{
    public partial class AlertSlide : System.Web.UI.UserControl
    {
        DataTable dtAlerts = new DataTable();
        DataTable dtGroups = new DataTable();
        public SortedDictionary<string, SortedDictionary<int, DataTable>> dicAlert;
        public int numMember;
        public int numCompany;
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
        public DataTable dt_AlertList
        {
            get { return dtAlerts; }
            set { dtAlerts = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dicAlert = new SortedDictionary<string, SortedDictionary<int, DataTable>>();
            Bind();
        }
        private void Page_PreRender(object sender, EventArgs e)
        {
            dt_AlertList.Clear();
            dt_AlertList.Dispose();
            dicAlert.Clear();
        }
        private void Bind()
        {
            DataTable dt_ServerAlertList = new DataTable();
            List<string> dt_Group = new List<string>();

            if (dtAlerts != null)
            {
                dt_ServerAlertList = dtAlerts;

                var tmp_Group = from tbl in dt_ServerAlertList.AsEnumerable()
                                orderby tbl["DisplayGroup"] ascending
                                group tbl by tbl["DisplayGroup"] into tmp
                                select tmp;
                foreach (var i in tmp_Group)
                {
                    SetAlertDic(dt_ServerAlertList, i.Key.ToString());
                    dt_Group.Add(i.Key.ToString());
                }
                tmp_Group = null;
            }
            else
                return;

            rpt_Alert.DataSource = dt_Group;
            rpt_Alert.DataBind();
            dt_Group.Clear();
        }
        #region Func
        public void SetAlertDic(DataTable dt_ServerAlertList, string strGroupName)
        {
            SortedDictionary<int, DataTable> dic = new SortedDictionary<int, DataTable>();

            DataSet ds = new DataSet();
            for (int i = 0; i < 6; i++)
            {
                DataTable dt = new DataTable();
                dt = dt_ServerAlertList.Clone();
                dt.TableName = i.ToString();
                ds.Tables.Add(dt);
            }
            var tmp_Serverlist = from tbl in dt_ServerAlertList.AsEnumerable()
                                 where tbl["DisplayGroup"].ToString() == strGroupName
                                 orderby tbl["TimeIn"] descending
                                 select tbl;
            int idx = 0;
            foreach (DataRow tmp in tmp_Serverlist)
            {
                if (idx == 6)
                    idx = 0;
                ds.Tables[idx].ImportRow(tmp);
                idx++;
            }
            tmp_Serverlist = null;
            idx = 0;
            foreach (DataTable dt_alert in ds.Tables)
            {
                dic.Add(idx, dt_alert);
                idx++;
            }
            dicAlert.Add(strGroupName, dic);
        }
        public string GetAlertList(string strGroupname, int numDicIdx)
        {
            var path = HttpContext.Current.Request.MapPath("/COMMON/UC/");
            string strHtmltag = "";
            string strHtmltag_tmp = "";

            string IMG = "";
            int idx = 1;
            string text = ServicePoint.Lib.ChartClass.strdefaultalert;
            if (dicAlert[strGroupname].ContainsKey(numDicIdx) == false)
            {
                strHtmltag = text;
                strHtmltag = strHtmltag.Replace("#AlertDescription#", "No Alerts Found");
                strHtmltag = strHtmltag.Replace("#InstanceName#", "");
                strHtmltag = strHtmltag.Replace("#RepeatCnt#", "");
                strHtmltag = strHtmltag.Replace("#SERVERTYPE#", "");
                strHtmltag = strHtmltag.Replace("#DisplayName#", "");
                strHtmltag = strHtmltag.Replace("#AlertStatus#", "");
                strHtmltag = strHtmltag.Replace("#dtLastUpdate#", "");
                strHtmltag = strHtmltag.Replace("#IMG#", "NoAlerts");
                strHtmltag = strHtmltag.Replace("#FIRST#", "active");
                strHtmltag = strHtmltag.Replace("#STRURLLINK#", "");
            }
            else
            {
                DataTable dt = (DataTable)dicAlert[strGroupname][numDicIdx];
                if (dt.Rows.Count == 0)
                {
                    string strLink = "";

                    strHtmltag = text;
                    strHtmltag = strHtmltag.Replace("#AlertDescription#", "No Alerts Found");
                    strHtmltag = strHtmltag.Replace("#InstanceName#", "");
                    strHtmltag = strHtmltag.Replace("#RepeatCnt#", "");
                    strHtmltag = strHtmltag.Replace("#SERVERTYPE#", "");
                    strHtmltag = strHtmltag.Replace("#DisplayName#", "");
                    strHtmltag = strHtmltag.Replace("#AlertStatus#", "");
                    strHtmltag = strHtmltag.Replace("#dtLastUpdate#", "");
                    strHtmltag = strHtmltag.Replace("#IMG#", "NoAlerts");
                    strHtmltag = strHtmltag.Replace("#FIRST#", "active");
                    strHtmltag = strHtmltag.Replace("#STRURLLINK#", strLink);

                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["AlertStatus"] == DBNull.Value)
                            continue;
                        if (Convert.ToInt32(dr["AlertStatus"]) == 1)
                        { IMG = "Information"; }
                        if (Convert.ToInt32(dr["AlertStatus"]) == 2)
                        { IMG = "Warning"; }
                        if (Convert.ToInt32(dr["AlertStatus"]) == 3)
                        { IMG = "Critical"; }
                        string strLink = RedirectToAlertDetailPage(dr["ServerType"], dr["DisplayName"], dr["ReasonCode"], dr["InstanceName"], dr["ServerNum"], dr["TimeIn"], dr["TimeIn_UTC"]);
                        strHtmltag_tmp = text;
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#AlertDescription#", dr["AlertDescription"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#InstanceName#", dr["InstanceName"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#RepeatCnt#", dr["RepeatCnt"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#SERVERTYPE#", dr["SERVERTYPE"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#DisplayName#", dr["DisplayName"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#AlertStatus#", dr["AlertStatus"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#dtLastUpdate#", dr["TimeIn"].ToString());
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#IMG#", IMG);
                        strHtmltag_tmp = strHtmltag_tmp.Replace("#STRURLLINK#", strLink);

                        if (idx == 1)
                            strHtmltag_tmp = strHtmltag_tmp.Replace("#FIRST#", "active");
                        else
                            strHtmltag_tmp = strHtmltag_tmp.Replace("#FIRST#", "");
                        idx++;
                        strHtmltag = strHtmltag + strHtmltag_tmp;
                    }
                }
                dt.Clear();
                dt.Dispose();
                dicAlert[strGroupname][numDicIdx].Clear();
            }
            strHtmltag_tmp = "";
            return strHtmltag;
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
            string strTimeIn_UTC = dtLastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

            strUrl = "/Common/UC/AlertDetail.aspx";

            strUrl += String.Format("?ServerType={0}&HostName={1}&ReasonCode={2}&InstanceName={3}&SN={4}&TimeIn={5}&TimeIn_UTC={6}", strName, strHostName, strReasonCode, strInstanceName, strServerNumber, strTimeIn, strTimeIn_UTC);
            strUrl = strUrl.Replace("#", "URSR13");
            return strUrl;
        }
        #endregion
    }
}