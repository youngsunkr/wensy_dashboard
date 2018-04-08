using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicePoint.Lib;
using System.Data;

namespace ServicePoint.Setting
{
    public partial class GroupMember : Base
    {
        protected int CompanyNum;
        override protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            RequestQueryString();
            BindData();
            GetMemberGrade(1);
        }
        private void RequestQueryString()
        {
            CompanyNum = Util.TConverter<int>(Util.GetCookieValue(Request, "CompanyNum"));
        }

        private void BindData()
        {
            int nReturn;
            DB.Cloud cloud = new DB.Cloud();
            nReturn = cloud.m_tbCompany_Member_List(CompanyNum);
            gv_List.DataSource = cloud.dsReturn.Tables[0];
            gv_List.DataBind();
        }

        protected void gv_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected string GetMemberGrade(object numGrade)
        {
            string strGradeName = Enum.Parse(typeof(Lib.MemberGrade.Type), numGrade.ToString()).ToString();
            return strGradeName;
        }

    }
}