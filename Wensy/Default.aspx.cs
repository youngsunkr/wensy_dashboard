using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;

namespace ServicePoint
{
    public partial class Default : Base
    {
        public int MemberNum, CompanyNum;
        public DataTable dt_ServerList;
        public string fileName;
        public int numCellCnt;
        public SortedDictionary<string, SortedDictionary<int, DataTable>> dicAlert;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            dicAlert = new SortedDictionary<string, SortedDictionary<int, DataTable>>();
            RequestQueryString();
            InitControl();
            if (IsPostBack)
                RequestForm();
            BindData();
            base.ReDirect();

        }
        private void Page_PreRender(object sender, EventArgs e)
        {
            dt_ServerList.Clear();
            dt_ServerList.Dispose();
            dicAlert.Clear();
        }
        public void RequestQueryString()
        {
            MemberNum = Lib.Util.TConverter<int>(Lib.Util.GetCookie(Request, "MemberNum").Value);
            CompanyNum = Lib.Util.TConverter<int>(Lib.Util.GetCookie(Request, "CompanyNum").Value);
            if (string.IsNullOrEmpty(Request.QueryString["Type"]) || Request.QueryString["Type"].ToString() == "Default")
            {
                //fileName = "Type1";
                //numCellCnt = 3;
                fileName = "Type2";
                numCellCnt = 6;
            }
            else
            {
                fileName = "Type2";
                numCellCnt = 6;
            }
        }
        public void RequestForm()
        {
            MemberNum = Lib.Util.TConverter<int>(Lib.Util.GetCookie(Request, "MemberNum").Value);
            CompanyNum = Lib.Util.TConverter<int>(Lib.Util.GetCookie(Request, "CompanyNum").Value);
        }
        private void InitControl()
        {
            tmr.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PageRefreshTime"]);
        }
        public void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();

            //DataTable dt_ServerAlertList = new DataTable();
            
            //서버리스트 읽어오기
            string MyServerList = "";
            cloud.SetCmd("Cloud");
            cloud.get_MyServerList(MemberNum);

            foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            {
                MyServerList += dr["ServerNum"].ToString() + ",";
            }
                                     
            MyServerList = MyServerList.Substring(0, MyServerList.Length - 1);
            
            cloud.SetCmd("Cloud");
            int nReturn = cloud.get_ServerStatus(MyServerList);
            DataSet ds = cloud.dsReturn;
            List<string> dt_Group = new List<string>();
            if (ds != null)
            {
                dt_ServerList = Lib.ConvertingProc.w_Dashboard(ds.Tables[0]);
                var tmp_Group = from tbl in ds.Tables[0].AsEnumerable()
                                orderby tbl["DisplayGroup"] ascending
                                group tbl by tbl["DisplayGroup"] into tmp
                                select tmp;
                foreach (var i in tmp_Group)
                {
                    dt_Group.Add(i.Key.ToString());
                }
                tmp_Group = null;
            }
            else
                return;

            ds.Clear();
            ds.Dispose();

            rpt_ServerStatus.DataSource = dt_Group;
            rpt_ServerStatus.DataBind();

            dt_Group.Clear();
            //nReturn = cloud.W_AlertCountList(MemberNum, CompanyNum, numAlertDataDuration);
            //if (cloud.dsReturn != null)
            //{
            //    dt_ServerAlertList = cloud.dsReturn.Tables[0];
            //    var tmp_Group = from tbl in dt_ServerAlertList.AsEnumerable()
            //                    orderby tbl["DisplayGroup"] ascending
            //                    group tbl by tbl["DisplayGroup"] into tmp
            //                    select tmp;
            //    foreach (var i in tmp_Group)
            //    {

            //        SetAlertDic(dt_ServerAlertList, i.Key.ToString());
            //        dt_Group.Add(i.Key.ToString());
            //    }
            //    cloud.dsReturn.Clear();
            //    cloud.dsReturn.Dispose();
            //    tmp_Group = null;
            //}
            //else
            //{
            //    return;
            //}
            ////rpt_Alert.DataSource = dt_Group;
            ////rpt_Alert.DataBind();
            dt_Group.Clear();
            cloud.CloseCon();
          
        }
        #region Func
        public string GetUrlEncodingValue(string strGroupName)
        {
            var ParamValue = HttpUtility.UrlEncode(strGroupName);
            return ParamValue;
        }
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
        /*
        public string GetAlertList(string strGroupname, int numDicIdx)
        {

            string strHtmltag = "";
            string strHtmltag_tmp = "";
            string BGCOLOR = "";
            string IMG = "";
            int idx = 1;
            string text = Lib.ChartClass.strdefaultalert;
            if (dicAlert[strGroupname].ContainsKey(numDicIdx) == false)
            {
                strHtmltag = text;
                strHtmltag = strHtmltag.Replace("#BGCOLOR#", BGCOLOR);
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
                strHtmltag_tmp = strHtmltag_tmp + strHtmltag;
            }
            else
            {
                DataTable dt = (DataTable)dicAlert[strGroupname][numDicIdx];
                if (dt.Rows.Count == 0)
                {
                    strHtmltag = text;
                    strHtmltag = strHtmltag.Replace("#BGCOLOR#", BGCOLOR);
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

                    strHtmltag_tmp = strHtmltag_tmp + strHtmltag;
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
                        strHtmltag = text;
                        strHtmltag = strHtmltag.Replace("#BGCOLOR#", BGCOLOR);
                        strHtmltag = strHtmltag.Replace("#SERVERNUM#", dr["ServerNum"].ToString());
                        strHtmltag = strHtmltag.Replace("#AlertDescription#", dr["AlertDescription"].ToString());
                        strHtmltag = strHtmltag.Replace("#InstanceName#", dr["InstanceName"].ToString());
                        strHtmltag = strHtmltag.Replace("#RepeatCnt#", dr["RepeatCnt"].ToString());
                        strHtmltag = strHtmltag.Replace("#SERVERTYPE#", dr["SERVERTYPE"].ToString());
                        strHtmltag = strHtmltag.Replace("#DisplayName#", dr["DisplayName"].ToString());
                        strHtmltag = strHtmltag.Replace("#AlertStatus#", dr["AlertStatus"].ToString());
                        strHtmltag = strHtmltag.Replace("#dtLastUpdate#", dr["TimeIn"].ToString());
                        strHtmltag = strHtmltag.Replace("#IMG#", IMG);
                        strHtmltag = strHtmltag.Replace("#STRURLLINK#", strLink);

                        if (idx == 1)
                            strHtmltag = strHtmltag.Replace("#FIRST#", "active");
                        else
                            strHtmltag = strHtmltag.Replace("#FIRST#", "");
                        idx++;
                        strHtmltag_tmp = strHtmltag_tmp + strHtmltag;
                        strLink = "";
                    }
                }
                dt.Clear();
                dt.Dispose();
                dicAlert[strGroupname][numDicIdx].Clear();
            }
            strHtmltag = "";
            return strHtmltag_tmp;
        }
        */
        public string GetServerList(string strGroupName)
        {
            string BGCOLOR = "";
            string returnString = "";
            string strHtmltag = "";
            string text = "";
            if (fileName.ToLower() == "type1")
                text = Lib.ChartClass.strdefaultpage1;
            else
                text = Lib.ChartClass.strdefaultpage2;
            int idx = 1;
            int numCellWidth = 12 / numCellCnt;

            var tmp_Serverlist = from tbl in dt_ServerList.AsEnumerable()
                                 where tbl["DisplayGroup"].ToString() == strGroupName
                                 select tbl;
            foreach (DataRow i in tmp_Serverlist)
            {
                string divid = "carousel" + strGroupName + idx.ToString();

                DataRow dr = i;

                if (dr["CurrentStatus"] == DBNull.Value)
                {
                    BGCOLOR = "bg-status-default";
                }
                else
                {
                    if (Convert.ToInt32(dr["CurrentStatus"]) == 0)
                        BGCOLOR = "bg-status-good";
                    if (Convert.ToInt32(dr["CurrentStatus"]) == 1)
                        BGCOLOR = "bg-status-info";
                    if (Convert.ToInt32(dr["CurrentStatus"]) == 2)
                        BGCOLOR = "bg-status-warning";
                    if (Convert.ToInt32(dr["CurrentStatus"]) == 3)
                        BGCOLOR = "bg-status-failure";
                    if (dr["AgentStatus"].ToString() == "0")
                        BGCOLOR = "bg-status-default";
                }
                if (idx % (numCellCnt + 1) == 0 || idx == 1)
                    strHtmltag = "<Div class=\"row\">" + text;
                else
                    strHtmltag = text;


                strHtmltag = strHtmltag.Replace("#ID#", divid);
                strHtmltag = strHtmltag.Replace("#BGCOLOR#", BGCOLOR);
                strHtmltag = strHtmltag.Replace("#SERVERNUM#", dr["ServerNum"].ToString());
                strHtmltag = strHtmltag.Replace("#SERVERNAME#", dr["DISPLAYNAME"].ToString());
                strHtmltag = strHtmltag.Replace("#STATUS#", dr["CurrentStatus"].ToString());
                strHtmltag = strHtmltag.Replace("#IP#", dr["IPAddress"].ToString());
                strHtmltag = strHtmltag.Replace("#IMG#", dr["SERVERTYPE"].ToString());
                strHtmltag = strHtmltag.Replace("#SERVERTYPE#", dr["SERVERTYPE"].ToString());
                //strHtmltag = strHtmltag.Replace("#TOTAL#", string.Format("{0:#,##0}", Lib.Util.ConvertRound(dr["CPU-Total"], 2)));
                //strHtmltag = strHtmltag.Replace("#KENEL#", string.Format("{0:#,##0}", Lib.Util.ConvertRound(dr["CPU-Kernel"], 2)));
                //strHtmltag = strHtmltag.Replace("#PQL#", dr["ProcessorQueueLength"].ToString());
                //strHtmltag = strHtmltag.Replace("#RAM#", string.Format("{0:#,##0}", Lib.Util.ConvertSize(dr["RAMSIZE"], (1024 * 1024 * 1024), 2)));
                //strHtmltag = strHtmltag.Replace("#AVAILABLE#", string.Format("{0:#,##0}", Lib.Util.FuncMemoryValue(dr["AvailableMemory"], "AvailableMemory")));
                //strHtmltag = strHtmltag.Replace("#COMMITED#", string.Format("{0:#,##0}", Lib.Util.FuncMemoryValue(dr["CommittedMemory"], "CommittedMemory")));
                ////strHtmltag = strHtmltag.Replace("#DISKREADSPEED#", Math.Round(Convert.ToDouble(dr["LogicalDiskAvgRead"]), 2).ToString());
                //strHtmltag = strHtmltag.Replace("#DISKREADSPEED#", string.Format("{0:#,##0}", Lib.Util.ConvertRound(dr["LogicalDiskAvgRead"]), 2));
                //strHtmltag = strHtmltag.Replace("#DISKTIME#", string.Format("{0:#,##0}", Lib.Util.ConvertRound(dr["LogicalDiskTIme"], 2)).ToString());
                //strHtmltag = strHtmltag.Replace("#FREESPACE#", Lib.Util.ConvertSize(dr["LogicalDiskFreeMByte-C"], (1024), 2).ToString());
                //strHtmltag = strHtmltag.Replace("#BYTESTOTAL#", string.Format("{0:#,##0}", Lib.Util.ConvertSize(dr["NetworkBytes-Total"], (1024 * 1024), 2)).ToString());
                //strHtmltag = strHtmltag.Replace("#BYTESRECY#", string.Format("{0:#,##0}", Lib.Util.ConvertSize(dr["NetworkBytes-Received"], (1024 * 1024), 2)).ToString());
                //strHtmltag = strHtmltag.Replace("#BYTESSEND#", string.Format("{0:#,##0}", Lib.Util.ConvertSize(dr["NetworkBytes-Sent"], (1024 * 1024), 2)).ToString());
                ////strHtmltag = strHtmltag.Replace("#LASTDATE#", dr["TimeIn"].ToString());
                DateTime TimeIn = new DateTime();
                TimeIn = Convert.ToDateTime(dr["TimeIn"].ToString());
                string strTimeIn = TimeIn.ToString("yyyy-MM-dd HH:mm:ss");
                strHtmltag = strHtmltag.Replace("#LASTDATE#", strTimeIn);

                returnString = returnString + strHtmltag;
                if (idx % numCellCnt == 0)
                    returnString = returnString + "</div>";
                // div 짝맞쳐주기
                if ((tmp_Serverlist.Count() == (idx)))
                {
                    if (idx % numCellCnt != 0)
                    {
                        // 한 row에 col4짜리 3개가 들어갈때 등록된서버가 1이면 2개를 공백처리해준다 
                        // 등록된서버가 2개면 1개를 처리
                        // 반응형으로 만들기대문에~!
                        int modValue = numCellCnt - (idx % numCellCnt);
                        for (int index = 0; index < modValue; index++)
                            returnString = returnString + "<div class=\"col-sm-" + numCellWidth.ToString() + " col-md-" + numCellWidth + "\"></div>";
                        returnString = returnString + "</div>";
                    }
                }
                idx++;

            }
            tmp_Serverlist = null;
            text = "";
            strHtmltag = "";
            return returnString;
        }

       
        //public static string RedirectToAlertDetailPage(object name, object hostName, object reasonCode, object instanceName, object ServerNum, object TimeIn, object TimeIn_UTC)
        //{
        //    // ALERT LINK URL

        //    string strUrl = "";
        //    string strName = name.ToString();
        //    string strHostName = hostName.ToString();
        //    string strReasonCode = reasonCode.ToString();
        //    string strInstanceName = instanceName.ToString();
        //    string strServerNumber = ServerNum.ToString();

        //    DateTime dtLastUpdate = new DateTime();
        //    dtLastUpdate = Convert.ToDateTime(TimeIn);
        //    string strTimeIn = dtLastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

        //    DateTime dtLastUpdate_UTC = new DateTime();
        //    dtLastUpdate_UTC = Convert.ToDateTime(TimeIn_UTC);
        //    string strTimeIn_UTC = dtLastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

        //    strUrl = "/Common/UC/AlertDetail.aspx";

        //    strUrl += String.Format("?ServerType={0}&HostName={1}&ReasonCode={2}&InstanceName={3}&SN={4}&TimeIn={5}&TimeIn_UTC={6}", strName, strHostName, strReasonCode, strInstanceName, strServerNumber, strTimeIn, strTimeIn_UTC);
        //    strUrl = strUrl.Replace("#", "URSR13");
        //    return strUrl;
        //}
        #endregion


    }
}