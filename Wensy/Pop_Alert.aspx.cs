using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace ServicePoint
{
    public partial class Pop_Alert : Base
    {
        #region Member
        private DB.Cloud cloud;
        public DataTable dt_Alert;
        public int numOverViewCnt;
        public string strGroupName;
        #endregion
        // 자바스크립트에 배열로 데이터를 만들어줘야하기때문에
        // 챠트의 데이터를 각각의 템플릿필드 헤더값으로 패핑해서 뿌려주게 만듬 dictionary<헤더값,챠트데이터배열>
        override protected void Page_Load(object sender, EventArgs e)
        {
            //strGroupName = HttpUtility.HtmlDecode(Request["grname"].ToString());
            cloud = new DB.Cloud();
            litScript.Text = "";
            base.Page_Load(sender, e);
            base.ReDirect();
            BindData();
            if (!IsPostBack)
            {
                AlertList.InitControl();
            }
            AlertList.Company = CompanyNum;
            AlertList.Member = MemberNum;
            AlertList.dt_AlertList = dt_Alert;

        }
        public void BindData()
        {
            dt_Alert = new DataTable();

            //서버리스트 읽어오기
            string MyServerList = "";
            cloud.SetCmd("Cloud");
            cloud.get_MyServerList(MemberNum);

            foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
            {
                MyServerList += dr["ServerNum"].ToString() + ",";
            }

            MyServerList = MyServerList.Substring(0, MyServerList.Length - 1);

            //DB.Cloud cloud = new DB.Cloud();
            int nReturn = cloud.get_AlertMessage(MyServerList, numAlertDataDuration);
            //if (cloud.dsReturn != null)
            //{
            //    DataTable dt_Alert_tmp = cloud.dsReturn.Tables[0];
            //    string strExpression = "DisPlayGroup = '" + strGroupName + "'";
            //    DataRow[] dr = dt_Alert_tmp.Select(strExpression);
            //    dt_Alert = dt_Alert_tmp.Clone();
            //    foreach (DataRow d in dr)
            //    {
            //        dt_Alert.ImportRow(d);
            //    }
            //}
            //cloud.dsReturn.Dispose();

            if (cloud.dsReturn.Tables[0].Rows.Count > 0)
            {
                //데이터 테이블 구조생성용으로 첫번째 json데이터를 불러와서 컬럼명을 자동셋팅하도록
                DataTable tester = (DataTable)JsonConvert.DeserializeObject(((string)cloud.dsReturn.Tables[0].Rows[0]["Data_Json"]), (typeof(DataTable)));
                //tester 에는 실제로 데이터가 들어가잇고 clone 을 이용해 dt_Struct 에 데이터테이블 구조만 복사
                DataTable dt_struct = new DataTable();
                dt_Alert = tester.Clone();
                dt_Alert.Columns.Add("RepeatCnt");
                dt_Alert.AcceptChanges();
                //이후 dt_struct 에 계속 merge(union) 하여 하나로 합체 테스트로 돌려보니 rowcount 8만 정도 나왓네요
                foreach (DataRow dr in cloud.dsReturn.Tables[0].Rows)
                {
                    DataTable dt_tmp = (DataTable)JsonConvert.DeserializeObject(((string)dr["Data_Json"]), (typeof(DataTable)));

                    //신규추가 2017 - 09 - 23 데이터 머지 = mssql union
                    //신규추가 2017 - 09 - 23 참조 https://msdn.microsoft.com/ko-kr/library/fk68ew7b(v=vs.110).aspx
                    dt_Alert.Merge(dt_tmp);
                }
            }
            cloud.dsReturn.Dispose();
        }
    }
}