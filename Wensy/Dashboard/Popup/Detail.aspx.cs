using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ServicePoint.Dashboard.Popup
{
    public partial class Detail : Base
    {
        protected int ServerNum;
        private string strType;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            RequestQueryString();
            InitControl();
            BindControl();
        }
        private void RequestQueryString()
        {
            strType = "SQL_CPU";
            if (Request.QueryString.AllKeys.Contains("ServerNum"))
            {
                ServerNum = Lib.Util.TConverter<int>(Request.QueryString["ServerNum"]);

            }
            if (Request.QueryString.AllKeys.Contains("TypeName"))
            {
                strType = Lib.Util.TConverter<string>(Request.QueryString["TypeName"]);
            }
        }
        private void RequestForm()
        {
        }
        private void InitControl()
        {
            tmr.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["PageRefreshTime"]);
        }
        private void BindControl()
        {
            hdn_ServerNum.Value = ServerNum.ToString();
            string strPath = "/Common/UC/Dashboard/Detail/" + strType + ".ascx";
            phd.Controls.Clear();
            phd.Controls.Add(LoadControl(strPath));
        }
    }
}