using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ServicePoint.Lib;
using System.Configuration;

namespace ServicePoint.Setting
{
    public partial class User : Base
    {
        DataTable dt = new DataTable();
        private DB.Cloud cloud;
        private int nReturn;
        
        override protected void Page_Load(object sender, EventArgs e)
        {
            cloud = new DB.Cloud();

            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                CheckDemo();
                BindData();
                BindControl();
            }
        }
        private void CheckDemo()
        {
            if (ConfigurationManager.AppSettings["DeployMode"].ToLower() == "demo")
            {
                litScript.Text = Util.AlertScript("데모 버전에서는 수정 불가합니다","/");
                
                return;
            }
        }
        private void BindData()
        {
            nReturn = cloud.m_Profile_Info(MemberNum);
            dt = cloud.dsReturn.Tables[0];

        }
        private void BindControl()
        {
            txt_Email.Text = dt.Rows[0]["Email"].ToString();
            txt_Name.Text = dt.Rows[0]["Name"].ToString();
            txt_Company.Text = dt.Rows[0]["CompanyName"].ToString();
            txt_Tel.Text = dt.Rows[0]["hp"].ToString();
            txt_dtmReg.Text = dt.Rows[0]["regdate"].ToString();
        }


        private bool Check()
        {
            if (string.IsNullOrEmpty(txt_Tel.Text))
            {
                litScript.Text = Util.AlertScript("연락처를 입력해주세요");
                return false;
            }
            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                litScript.Text = Util.AlertScript("이름을 입력해주세요");
                return false;
            }
            if (string.IsNullOrEmpty(txt_Pass.Text))
            {
                litScript.Text = Util.AlertScript("비밀번호를 입력해주세요");
                return false;
            }
            return true;
        }
        private bool CheckPass()
        {
            if (string.IsNullOrEmpty(txt_Pass_new.Text))
            {
                litScript.Text = Util.AlertScript("변경비밀번호를 입력해주세요");
                return false;
            }
            if (string.IsNullOrEmpty(txt_Pass_new_confirm.Text))
            {
                litScript.Text = Util.AlertScript("변경비밀번호확인을 입력해주세요");
                return false;
            }
            if (txt_Pass_new.Text != txt_Pass_new_confirm.Text)
            {
                litScript.Text = Util.AlertScript("변경비밀번호가 일치하지 않습니다!");
                return false;
            }
            return true;
        }

        protected void btn_PassUpdate_Click(object sender, EventArgs e)
        {
            if (!Check())
                return;
            if (!CheckPass())
                return;
            nReturn = cloud.m_tbMember_Update(MemberNum, txt_Name.Text, Util.EncryptText(txt_Pass.Text), Util.EncryptText(txt_Pass_new.Text), txt_Tel.Text, txt_Company.Text);
            if (nReturn == -2)
            {
                litScript.Text = Util.AlertScript("비밀번호가 일치하지않습니다!");
                return;
            }
            if (nReturn == -1)
            {
                litScript.Text = Util.AlertScript("정보수정중 오류발생");
                return;
            }
           
        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            if (!Check())
                return;
            nReturn = cloud.m_tbMember_Update(MemberNum, txt_Name.Text, Util.EncryptText(txt_Pass.Text), Util.EncryptText(txt_Pass.Text), txt_Tel.Text, txt_Company.Text);
            if (nReturn == -2)
            {
                litScript.Text = Util.AlertScript("비밀번호가 일치하지않습니다!");
                return;
            }
            if (nReturn == -1)
            {
                litScript.Text = Util.AlertScript("정보수정중 오류발생");
                return;
            }
           

        }
    }
}