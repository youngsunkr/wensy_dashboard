using System;
using System.Data;
using System.Configuration;
using ServicePoint.Lib;
using System.Data.SqlClient;

namespace DB
{
    /// <summary>
    /// DB_Base의 요약 설명입니다.
    /// </summary>

    public class Admin : DB_Base_NoTran
    {
        public Admin()
        {
        }
        
        public new void Close()
        {
            base.Close();
        }
        public void SetCmd(string dbName)
        {
            base.SetCmdCon(dbName);
        }
        public int Create_Key(string strKey, int numWindows, int numWeb, int numSql, int numSharePoint, int numBiztalk)
        {
            InitFrameParameter();
            AddParameter("@strKey", SqlDbType.VarChar, 16, strKey);
            AddParameter("@numWindows", SqlDbType.TinyInt, 1, numWindows);
            AddParameter("@numWeb", SqlDbType.TinyInt, 1, numWeb);
            AddParameter("@numSql", SqlDbType.TinyInt, 1, numSql);
            AddParameter("@numSharePoint", SqlDbType.TinyInt, 1, numSharePoint);
            AddParameter("@numBiztalk", SqlDbType.TinyInt, 1, numBiztalk);
            SetProcedure("p_Create_Key");

            dsReturn = ExecuteDataSet();

            return nReturn;
        }
        //public DataSet Create_Key_ds(string strKey, int numWindows, int numWeb, int numSql, int numSharePoint, int numBiztalk)
        //{
        //    nReturn = Create_Key(strKey, numWindows, numWeb, numSql, numSharePoint, numBiztalk);

        //    if (nReturn == 1)
        //        return dsReturn;
        //    else
        //        return new DataSet();
        //}
        public int Key_List()
        {
            InitFrameParameter();
            SetProcedure("p_Key_List");

            dsReturn = ExecuteDataSet();

            return nReturn;
        }
        public DataSet Key_List_ds()
        {
            nReturn = Key_List();

            if (nReturn == 0)
                return dsReturn;
            else
                return new DataSet();
        }
    }
}