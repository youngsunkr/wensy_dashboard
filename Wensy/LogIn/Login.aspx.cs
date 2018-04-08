using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
namespace ServicePoint.LogIn
{
    public partial class Login : System.Web.UI.Page
    {
        #region Member
        private string strEmail, strPass;
        private string strIp, strReturnUrl;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (IsPostBack)
            {
                RequestForm();
                BindData();
            }
            else
            {
                RequestQueryString();
                if (ConfigurationManager.AppSettings["DeployMode"].ToString().ToLower() == "demo")
                    BindData();
            }
        }
        #region Request
        public void RequestForm()
        {
            strReturnUrl = "/Default.aspx";
            if (Request.QueryString.AllKeys.Contains("returnURL"))
                strReturnUrl = Request.QueryString["returnURL"].ToString();
            strEmail = txtEmail.Text;
            strPass = txtPassword.Text;
            if (string.IsNullOrEmpty(strEmail) || string.IsNullOrEmpty(strPass))
            {
                //오류처리
                lblError.Text = "이메일주소와 비밀번호를 확인해주세요!";
                return;
            }
        }
        public void RequestQueryString()
        {
            strReturnUrl = "/Default.aspx";
            if (Request.QueryString.AllKeys.Contains("returnURL"))
                strReturnUrl = Request.QueryString["returnURL"].ToString();
            strIp = Request.ServerVariables["REMOTE_HOST"].ToString();
            //if (ConfigurationManager.AppSettings["DeployMode"].ToString().ToLower() != "dev")
            //    return;
            if (ConfigurationManager.AppSettings["DeployMode"].ToString().ToLower() == "dev" && strIp == "::1")
            {
                txtEmail.Text = "t@t.com";
                txtPassword.Text = "1234";
            }

            if (ConfigurationManager.AppSettings["DeployMode"].ToString().ToLower() == "demo")// && strIp == "::1")
            {
                strEmail = "demo@nwiz.co.kr";
                strPass = "1234";
                txtEmail.Text = strEmail;
                txtPassword.Text = strPass;
            }
        }
        #endregion
        #region BindData
        public void BindData()
        {
            DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.ServicePointLogin(strEmail, Lib.Util.EncryptText(strPass));
            //if (nReturn == 1)
            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                DataRow dr_UserInfo = Lib.ConvertingProc.dataTableNull(cloud.dsReturn.Tables[0]).Rows[0];
                Lib.Util.RemoveCookie(Response, "Email"); 
                Lib.Util.RemoveCookie(Response, "MemberNum");
                Lib.Util.RemoveCookie(Response, "MemberName");
                Lib.Util.RemoveCookie(Response, "CompanyNum");
                Lib.Util.RemoveCookie(Response, "CompanyName");
                Lib.Util.RemoveCookie(Response, "Grade");
                Lib.Util.RemoveCookie(Response, "Authorize");
                Lib.Util.RemoveCookie(Response, "ReDirectExpire"); 
                Lib.Util.SetCookieValue(Response, "Email", strEmail);
                Lib.Util.SetCookieValue(Response, "MemberNum", dr_UserInfo["MemberNum"].ToString());
                Lib.Util.SetCookieValue(Response, "MemberName", dr_UserInfo["MemberName"].ToString());
                Lib.Util.SetCookieValue(Response, "CompanyNum", dr_UserInfo["CompanyNum"].ToString());
                Lib.Util.SetCookieValue(Response, "CompanyName", dr_UserInfo["CompanyName"].ToString());
                Lib.Util.SetCookieValue(Response, "Grade", dr_UserInfo["Grade"].ToString());
                Lib.Util.SetCookieValue(Response, "Authorize", dr_UserInfo["Grade"].ToString());
                Lib.Util.SetCookieValue(Response, "ReDirectExpire", "true",DateTime.Now.AddMinutes(1));

                Response.Redirect(Server.UrlDecode(strReturnUrl));
            }
            else
            {
                lblError.Text = "이메일과 비밀번호를 확인해주세요!";
            }
        }
        #endregion

        #region Event
        protected void btnOK_Click(object sender, EventArgs e)
        {

        }
        #endregion

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/join.aspx");
        }

     
        #region Func
        //private void userLogin()
        //{
        //    int oidUser;

        //    gms.Open();
        //    nReturn = gms.UserLogin(strUserId, strPassword, out oidUser);
        //    errorCode = gms.frk_n4ErrorCode;
        //    errorMessage = gms.frk_strErrorText;

        //    if (errorCode != 0)
        //    {
        //        Response.Write(callback + "({\"error\":" + errorCode.ToString() + ", \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailLogin") + " : " + errorMessage + "\"})");
        //        return;
        //    }
        //    else if (nReturn == 1)
        //    {
        //        Response.Write(callback + "({\"error\":1, \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailNotExistAccount") + "\"})");
        //        return;
        //    }
        //    else if (nReturn == 2)
        //    {
        //        Response.Write(callback + "({\"error\":1, \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailLogin") + " : " + strPassword + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailPassword") + "\"})");
        //        return;
        //    }

        //    nReturn = gms.UserInfo(oidUser);
        //    errorCode = gms.frk_n4ErrorCode;
        //    errorMessage = gms.frk_strErrorText;
        //    if (errorCode != 0)
        //    {
        //        Response.Write(callback + "({\"error\":" + errorCode.ToString() + ", \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailLogin") + " : " + errorMessage + "\"})");
        //        return;
        //    }
        //    DataSet ds = gms.dsReturn;
        //    int oidGroup;
        //    {
        //        DataRow dr = ds.Tables[0].Rows[0];
        //        oidGroup = dr.Field<int>("oidGroup");
        //        Util.RemoveCookie(Response, "strUserId");
        //        Util.RemoveCookie(Response, "bitSave");
        //        Util.SetCookieValue(Response, "oidUser", dr.Field<int>("oidUser").ToString());
        //        Util.SetCookieValue(Response, "strUserId", dr.Field<string>("strUserId"), bitSave);
        //        Util.SetCookieValue(Response, "strUserName", dr.Field<string>("strUserName"));
        //        Util.SetCookieValue(Response, "oidGroup", dr.Field<int>("oidGroup").ToString());
        //        Util.SetCookieValue(Response, "strIp", dr.Field<string>("strIp"));
        //        Util.SetCookieValue(Response, "bitSave", bitSave.ToString().ToLower(), bitSave);
        //    }

        //    nReturn = gms.AuthorizeList(oidGroup, oidUser);
        //    errorCode = gms.frk_n4ErrorCode;
        //    errorMessage = gms.frk_strErrorText;
        //    gms.Close();
        //    if (errorCode != 0)
        //    {
        //        Response.Write(callback + "({\"error\":" + errorCode.ToString() + ", \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_FailLogin") + " : " + errorMessage + "\"})");
        //        return;
        //    }
        //    ds = gms.dsReturn;
        //    DataTable dt1 = ds.Tables[0];
        //    DataTable dt2 = GMSInfo.Instance.MenuTable;

        //    var query = from a in dt1.AsEnumerable()
        //                join b in dt2.AsEnumerable()
        //                on a.Field<int>("oidMenu") equals Util.TConverter<int>(b.Field<string>("oidMenu"))
        //                select new
        //                {
        //                    oidMenu = a.Field<int>("oidMenu"),
        //                    strMenuCode = b.Field<string>("strMenuCode"),
        //                    numAuthorizeLevel = a.Field<byte>("numAuthorizeLevel")
        //                };

        //    HttpCookie cookie = new HttpCookie("Authorize");
        //    foreach (var row in query)
        //    {
        //        cookie.Values[row.strMenuCode] = row.numAuthorizeLevel.ToString();
        //    }
        //    Util.SetCookie(Response, cookie);

        //    Response.Write(callback + "({\"error\":0, \"desc\":\"" + Util.GetLocaleValue("gms.common.proc.member.userLogin.desc_Success") + "\"})");
        //    return;
        //}
        #endregion
    }
}