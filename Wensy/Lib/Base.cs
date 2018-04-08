using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ServicePoint.Lib;


public abstract class Base : System.Web.UI.Page
{
    protected int MemberNum, CompanyNum, numChartDataDuration, numAlertDataDuration ;
    protected string strUserEmail;
    virtual protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ServicePoint.Lib.Util.GetCookieValue(Request, "MemberNum")))
        {
            string path = Request.ServerVariables["PATH_INFO"]
                + (String.IsNullOrEmpty(Request.ServerVariables["QUERY_STRING"]) ? String.Empty : "?" + Request.ServerVariables["QUERY_STRING"].ToString());

            Response.Redirect("/Login/Login.aspx?returnURL=" + Server.UrlPathEncode(path));
        }
        else
        {
            strUserEmail = ServicePoint.Lib.Util.GetCookieValue(Request, "Email").ToString();
            MemberNum = Convert.ToInt32(ServicePoint.Lib.Util.GetCookieValue(Request, "MemberNum"));
            CompanyNum = Convert.ToInt32(ServicePoint.Lib.Util.GetCookieValue(Request, "CompanyNum"));
            numChartDataDuration = Convert.ToInt32(ConfigurationManager.AppSettings["ChartDataDuration"].ToString());
            numAlertDataDuration = Convert.ToInt32(ConfigurationManager.AppSettings["AlertDataDuration"].ToString());
        }
        MaintainScrollPositionOnPostBack = true;
    }
    protected void ReDirect()
    {
        string cookievalue = Util.GetCookieValue(Request, "ReDirectExpire");
        if (string.IsNullOrEmpty(cookievalue))
        {
            Util.RemoveCookie(Response, "ReDirectExpire");
            Util.SetCookieValue(Response, "ReDirectExpire", "true", DateTime.Now.AddMinutes(60));

            string path = Request.ServerVariables["PATH_INFO"]
                   + (String.IsNullOrEmpty(Request.ServerVariables["QUERY_STRING"]) ? String.Empty : "?" + Request.ServerVariables["QUERY_STRING"].ToString());

            Response.Redirect(path);

        }
    }
    protected void SetGvHeader_Sort(GridView gv)
    {
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            if (String.IsNullOrEmpty(gv.Columns[i].SortExpression.ToString()))
                continue;

            if (gv.Columns[i].SortExpression == gv.SortExpression.ToString())
            {
                if (gv.SortDirection == SortDirection.Ascending)
                {
                    gv.Columns[i].HeaderText = gv.Columns[i].HeaderText.Replace(" ▲", String.Empty).Replace(" ▼", String.Empty) + " ▲";
                }
                else if (gv.SortDirection == SortDirection.Descending)
                {
                    gv.Columns[i].HeaderText = gv.Columns[i].HeaderText.Replace(" ▲", String.Empty).Replace(" ▼", String.Empty) + " ▼";
                }
            }
            else
            {
                gv.Columns[i].HeaderText = gv.Columns[i].HeaderText.Replace(" ▲", String.Empty).Replace(" ▼", String.Empty);
            }
        }
    }

}
