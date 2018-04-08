using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Text;
using ServicePoint.Lib;

namespace ServicePoint.Common.Proc
{
    /// <summary>
    /// Admin의 요약 설명입니다.
    /// </summary>
    public class Admin : IHttpHandler
    {
        #region Member
       
        private HttpRequest Request;
        private HttpResponse Response;
        private int numError;
        private string strDescript;
        #endregion
        #region property
        private string callback
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["callback"]);
            }
        }
        private string command
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["command"]);
            }
        }
        private string strEmail
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["hdn_Email"]);
            }
        }
        private string strMemberName
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["hdn_MemberName"]);
            }
        }
        private string strTel
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["hdn_Tel"]);
            }
        }
        private string strPass
        {
            get
            {
                return Lib.Util.TConverter<string>(Request.QueryString["hdn_Pass"]);
            }
        }
        private int numGrade
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_Grade"]);
            }
        }
        private string strCompanyName
        {
            get
            {
                return ServicePoint.Lib.Util.GetCookieValue(Request, "CompanyName").ToString();
            }
        }
        private int CompanyNum
        {
            get
            {
                return Lib.Util.TConverter<int>(ServicePoint.Lib.Util.GetCookieValue(Request, "CompanyNum"));
            }
        }
        private int MemberNum
        {
            get
            {
                return Lib.Util.TConverter<int>(ServicePoint.Lib.Util.GetCookieValue(Request, "MemberNum"));
            }
        }
        private int MemberNum_Target
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_MemberNum_Target"]);
            }
        }
        private int ServerNum
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_numServer"]);
            }
        }
        private string strDisplayName
        {
            get
            {
                return Request.QueryString["hdn_strDisplayName"].ToString();
            }
        }
        private string strDisplayGroup
        {
            get
            {
                return Request.QueryString["hdn_strDisplayGroup"].ToString();
            }
        }
        private string strServerType
        {
            get
            {
                return Request.QueryString["hdn_strServerType"].ToString();
            }
        }
        private string strLanguage
        {
            get
            {
                return Request.QueryString["hdn_strLanguage"].ToString();
            }
        }
        private int numPushInterval
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_PushInterval"]);
            }
        }
        private int numPushMaxOccurs
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_PushMaxOccurs"]);
            }
        }
        private int numPushResetInterval
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_PushResetInterval"]);
            }
        }
        private bool bolUsePushAlert
        {
            get
            {
                return Lib.Util.TConverter<bool>(Request.QueryString["hdn_UsePushAlert"]);
            }
        }
        private string strPerformanceObject
        {
            get
            {
                return Request.QueryString["hdn_strPerformanceObject"].ToString();
            }
        }
        private string strReasonCode
        {
            get
            {
                return Request.QueryString["hdn_strReasonCode"].ToString();
            }
        }
        private string strCounter
        {
            get
            {
                return Request.QueryString["hdn_strCounter"].ToString();
            }
        }
        private string strLevel
        {
            get
            {
                return Request.QueryString["hdn_strLevel"].ToString();
            }
        }
        private string strDescription
        {
            get
            {
                return Request.QueryString["hdn_strDescription"].ToString();
            }
        }
        private string strInstance
        {
            get
            {
                return Request.QueryString["hdn_strInstance"].ToString();
            }
        }
        private int numThreshold
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_numThreshold"]);
            }
        }
        private int numDuration
        {
            get
            {
                return Lib.Util.TConverter<int>(Request.QueryString["hdn_numDuration"]);
            }
        }
        private string strMobileAlert
        {
            get
            {
                return Request.QueryString["hdn_strMobileAlert"].ToString();
            }
        }
        private bool bolEnabled
        {
            get
            {
                return Util.TConverter<bool>(Request.QueryString["hdn_bolEnabled"]);
            }
        }
        private bool bolRecordApps
        {
            get
            {
                return Util.TConverter<bool>(Request.QueryString["hdn_bolRecordApps"]);
            }
        }
        private string strPCID
        {
            get
            {
                return Request.QueryString["hdn_strPCID"].ToString();
            }
        }
        private string strInstanceName
        {
            get
            {
                return Request.QueryString["hdn_strInstanceName"].ToString();
            }
        }
        private bool bolIfContains
        {
            get
            {
                return Lib.Util.TConverter<bool>(Request.QueryString["hdn_bolIfContains"]);
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            Request = context.Request;
            Response = context.Response;

            Response.ContentType = "application/json";
            switch (command)
            {
                case "addmember":
                    AddMember();
                    break;
                case "updatemember":
                    UpdateMember();
                    break;
                case "delmember":
                    DelMember();
                    break;
                case "emailcheck":
                    CheckEmail();
                    break;
                case "addserver":
                    AddServer();
                    break;
                case "updateserver":
                    UpdateServer();
                    break;
                case "delserver":
                    DelServer();
                    break;
                case "addservermember":
                    AddServerMember();
                    break;
                case "delservermember":
                    DelServerMember();
                    break;
                case "updatealertoptions":
                    UpdateAlertoptions();
                    break;
                case "updatealertrules":
                    UpdateAlertRules();
                    break;
                case "delserverinstance":
                    DelServerInstance();
                    break;
                case "addserverinstance":
                    AddServerInstance();
                    break;

            }
        }
        #region Function
        private void AddServerInstance()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbPInstance_Server_Add(ServerNum, strPCID, strInstanceName, bolIfContains);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"추가 완료\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"추가 실패\"})");
        }
        private void DelServerInstance()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbPInstance_Server_Del(ServerNum, strPCID, strInstanceName);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"삭제 완료\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"삭제 실패\"})");
        }
        private void UpdateAlertRules()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbAlertRules_Server_Update(ServerNum, strReasonCode, numThreshold, strInstance, numDuration, bolEnabled, strLevel, bolRecordApps, strDescription, strMobileAlert);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"수정 완료\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"수정 실패\"})");
        }
        private void UpdateAlertoptions()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbAlertOptions_Update(CompanyNum, numPushInterval, numPushMaxOccurs, numPushResetInterval, bolUsePushAlert);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"수정 완료\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"수정 실패\"})");
        }
        private void AddServerMember()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbServers_Member_Add(CompanyNum, ServerNum, MemberNum_Target);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"사용자 추가 성공\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"사용자 추가 실패\"})");
        }
        private void DelServerMember()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbServers_Member_Del(CompanyNum, ServerNum, MemberNum_Target);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"서버 사용자 삭제 성공\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"서버 사용자 삭제 실패\"})");
        }
        private void AddServer()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbServers_Add(MemberNum, CompanyNum, strDisplayName, strDisplayGroup, strServerType, strLanguage);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"서버 등록 성공\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"서버 등록 실패\"})");
        }
        private void UpdateServer()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbServers_Update(MemberNum, CompanyNum, strDisplayName, strDisplayGroup, ServerNum, strLanguage);
            if (nResult == 1)
                Response.Write(callback + "({\"error\":1, \"desc\":\"서버 정보 수정 성공\"})");
            else
                Response.Write(callback + "({\"error\":2, \"desc\":\"서버 정보 수정 성공\"})");
        }
        private void DelServer()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nResult = cloud.m_tbServers_Del(MemberNum, CompanyNum, ServerNum);
            Response.Write(callback + "({\"error\":1, \"desc\":\"삭제 성공\"})");
        }
        private void AddMember()
        {
           
            DataTable dt = new DataTable();
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = 1;
            //웹서비스에 멤버 insert 요청 true 시 cloud에 insert 
            nReturn = cloud.m_tbCompany_Member_Add(CompanyNum, strEmail, Util.EncryptText(strPass), strMemberName, strCompanyName, numGrade);

            //bytearray를 datatable로 변환
            //cloud 디비 insert 실패 Master db도 삭제 
            if (nReturn == 1)
            {
                strDescript = "Complete";
            }
            else
            {
                numError = 2;
                strDescript = "Fail";
            }


            Response.Write(callback + "({\"error\":" + numError.ToString() + ", \"desc\":\"" + strDescript + "\"})");

        }
        private void CheckEmail()
        {
            DB.Cloud cloud = new DB.Cloud();
           
            int MailCnt = cloud.m_CheckUserEmail(strEmail);
           
                if (cloud.dsReturn.Tables[0].Rows.Count == 0)
                    Response.Write(callback + "({\"error\":1, \"desc\":\"사용가능한 Email 입니다\"})");
                else
                    Response.Write(callback + "({\"error\":2, \"desc\":\"cloud 이미 사용중인 Email입니다\"})");
            
        }
        private void DelMember()
        {
            DB.Cloud cloud = new DB.Cloud();
            
            
            int nReturn = cloud.m_tbCompany_Member_Del(MemberNum, CompanyNum, MemberNum_Target);


            if (nReturn == 1)
                Response.Write(callback + "({\"error\":" + nReturn.ToString() + ", \"desc\":\"삭제하였습니다.\"})");
            else
                Response.Write(callback + "({\"error\":" + nReturn.ToString() + ", \"desc\":\"삭제 실패!!\"})");
          
        }
        private void UpdateMember()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.m_tbCompany_Member_Update(MemberNum, CompanyNum, MemberNum_Target, numGrade);
            if (nReturn == 1)
                Response.Write(callback + "({\"error\":" + nReturn.ToString() + ", \"desc\":\"수정하였습니다.\"})");
            else
                Response.Write(callback + "({\"error\":" + nReturn.ToString() + ", \"desc\":\"수정 실패!!\"})");
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}