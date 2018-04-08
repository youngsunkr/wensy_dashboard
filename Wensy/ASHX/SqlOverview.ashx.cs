using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoint.ASHX
{
    /// <summary>
    /// SqlOverview의 요약 설명입니다.
    /// </summary>
    public class SqlOverview : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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