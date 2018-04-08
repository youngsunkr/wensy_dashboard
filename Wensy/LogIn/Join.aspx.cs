using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicePoint.Lib;
using System.Data;

namespace ServicePoint.LogIn
{
    public partial class Join : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litScript.Text = "";
        }

        protected void btn_Check_Click(object sender, EventArgs e)
        {
            //string strEmail = txt_Email.Text;

            //CheckValue();
            //    hdn_Email.Value = strEmail;
            //    txt_Email.ReadOnly = true;
            //    SendMail();
           
        }
        private bool CheckValue()
        {
            //if (string.IsNullOrEmpty(hdn_Email.Value))
            //{
            //    litScript.Text = Lib.Util.AlertScript("이메일 중복확인을 해주세요!");
            //    return false;
            //}

            //if (string.IsNullOrEmpty(Lib.Util.GetCookieValue(Request, "EmailAUth")))
            //{
            //    litScript.Text = Util.AlertScript("이메일 인증확인을 해주세요!");
            //    return false;
            //}
            //if (Lib.Util.GetCookieValue(Request, "EmailAUth") != txt_Email_Auth.Text)
            //{
            //    litScript.Text = Lib.Util.AlertScript("이메일 인증번호가 일치하지않습니다!");
            //    return false;
            //}

            if (string.IsNullOrEmpty(txt_Email.Text))
            {
                litScript.Text = Lib.Util.AlertScript("Enter your username.");
                return false;
            }
            
            if (string.IsNullOrEmpty(txt_Pass.Text))
            {
                litScript.Text = Lib.Util.AlertScript("Enter your password.");
                return false;
            }
            if (txt_Pass.Text != txt_Pass_Confirm.Text)
            {
                litScript.Text = Lib.Util.AlertScript("Enter your password confirm.");
                return false;
            }

            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                litScript.Text = Lib.Util.AlertScript("Enter your name.");
                return false;
            }
            return true;
        }
        protected void btn_Join_Click(object sender, EventArgs e)
        {

            DB.Cloud cloud = new DB.Cloud();
            DataTable dt = new DataTable();
            int nReturn = cloud.m_tbMember_Add(txt_Email.Text, Util.EncryptText(txt_Pass.Text), txt_Name.Text, txt_Company.Text, 9);
              
            Response.Redirect("/Login/login.aspx");
            
        }

        protected void btn_Main_Click(object sender, EventArgs e)
        {
            Response.Redirect("/default.aspx");
        }
        private void SendMail()
        {

            Lib.EmailControls sendMail = new EmailControls();
            int iAuthNumber = GenerateAuthNumber();
            //MsgBox(context.Session["RANDOM_NUMBER"].ToString(), this.Page, this);
            //메일 발송이 안되서 팝업 메시지로 대체
            litScript.Text = Util.AlertScript("인증번호 : " + iAuthNumber.ToString());

            //string sSender = "kangstar05@hotmail.com"; //발송메일 계정 입력
            //string sPassword = "1234"; //위 계정 비밀번호 입력
            //string sTo = txt_Email.Text;
            //string sHeader = "서비스포인트 [인증번호] 발송 안내 메일입니다";
            ////string sMassage = "서비스포인트 인증번호 [" + iAuthNumber.ToString() + "] 모바일앱 다운로드: <a href='http://goo.gl/jAlmdq'>http://goo.gl/jAlmdq</a>";
            //string sMassage = "서비스포인트 인증번호 [" + iAuthNumber.ToString() + "]";

            //sendMail.g_EMailSettings.strSender = sSender;
            //sendMail.g_EMailSettings.strPassword = sPassword;
            //sendMail.g_EMailSettings.strTo = sTo;
            //sendMail.g_EMailSettings.strHeader = sHeader;
            //sendMail.g_EMailSettings.strMessage = sMassage;
            //if (sendMail.SendGmail() == "success")
            //{
            //    litScript.Text = Util.AlertScript("인증 메일이 발송되었습니다.");
            //}
            //else
            //{
            //    litScript.Text = Util.AlertScript("인증메일 발송 실패");
            //}
        }
        private int GenerateAuthNumber()
        {
            Random r = new Random();
            var rnumber = r.Next(100000, 999999);
            Util.SetCookieValue(Response, "EmailAUth", rnumber.ToString());
            return Convert.ToInt32(rnumber);
        }
    }
}