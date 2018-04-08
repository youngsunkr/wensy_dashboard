using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ServicePoint.Lib;

namespace ServicePoint.LogIn
{
    public partial class Search : System.Web.UI.Page
    {
        private DB.Cloud cloud;
        private string strTel, strPass,strEmail;
        protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();
            strTel = "";
            strPass = "";
            lbl_Text.Text = "";
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            cloud.m_FindPWD(txt_Email.Text);
            DataTable dt = cloud.dsReturn.Tables[0];
            if (dt.Rows.Count > 0)
            {
                strTel = dt.Rows[0]["HP"].ToString();
                strPass = dt.Rows[0]["UserPwd"].ToString();
                strEmail = dt.Rows[0]["Email"].ToString();
                if (strTel == txt_strTel_Confirm.Text && strEmail == txt_Email.Text)
                {
                    SendMail();
                }
                else
                {
                    lbl_Text.Text = "이메일과 연락처가 일치하지 않습니다!";
                }
            }
            else
            { 
                lbl_Text.Text="이메일과 연락처가 일치하는 정보가 없습니다!";
            }
        }
        private void SendMail()
        {

            Lib.EmailControls sendMail = new EmailControls();
            //MsgBox(context.Session["RANDOM_NUMBER"].ToString(), this.Page, this);

            string sSender = "codeclassic.help@gmail.com";
            string sPassword = "C0declass1c";
            string sTo = txt_Email.Text;
            string sHeader = "코드클래식 [비밀번호] 발송 안내 메일입니다";
            string sMassage = "코드클래식 비밀번호는 [" + Util.DecryptText(strPass).ToString() + "] 입니다!";

            sendMail.g_EMailSettings.strSender = sSender;
            sendMail.g_EMailSettings.strPassword = sPassword;
            sendMail.g_EMailSettings.strTo = sTo;
            sendMail.g_EMailSettings.strHeader = sHeader;
            sendMail.g_EMailSettings.strMessage = sMassage;
            if (sendMail.SendGmail() == "success")
            {
                litScript.Text = Util.AlertScript("인증 메일이 발송되었습니다.");
                lbl_Text.Text = "인증 메일이 발송되었습니다.";
            }
            else
            {
                litScript.Text = Util.AlertScript("인증메일 발송 실패");
                lbl_Text.Text = "인증메일 발송 실패";
            }
        }
    }
}