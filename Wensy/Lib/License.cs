using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoint.Lib
{
    public class License
    {
        public struct GetLicense
        {
            public int iWindows;
            public int iSQL;
            public int iWeb;
            public int iBizTalk;
            public int iSharePoint;
        }

        public struct CheckLicense
        {
            public int iWindows;
            public int iSQL;
            public int iWeb;
            public int iBizTalk;
            public int iSharePoint;
        }


        public GetLicense CREATE_LICENSE_OPTIONS = new GetLicense();
        public CheckLicense CHECK_LICENSE_VALUES = new CheckLicense();

        //public string CreateEEKey()
        //{

        //    CheckLicense ChkLic = new CheckLicense();

        //    try
        //    {
        //        var r = new Random();

        //        ChkLic.iRandom = Convert.ToInt16(r.Next(1000, 9999));
        //        ChkLic.iWindows = CREATE_LICENSE_OPTIONS.iWindows;
        //        ChkLic.iWeb = CREATE_LICENSE_OPTIONS.iWeb;
        //        ChkLic.iSQL = CREATE_LICENSE_OPTIONS.iSQL;
        //        ChkLic.iSharePoint = CREATE_LICENSE_OPTIONS.iSharePoint;
        //        ChkLic.iBizTalk = CREATE_LICENSE_OPTIONS.iBizTalk;
        //        ChkLic.iCheckSum = 1000001;
        //        ChkLic.dtmCreateDate = DateTime.Now;
        //        ChkLic.bIsValid = true;

        //        string strEEKey = StructToString(ChkLic);

        //        return strEEKey;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Enterprise Edition 키를 생성하려면, CREATE_LICENSE_OPTIONS 구조체에서 
        //각 서비스별 수(예: SQL 2대, Web : 1대)를 설정하고 CreateEEKey()를 호출하여 제품키를 리턴 받는다.
        //
        // 제품키는 16자리이며, 임의수 + 각서비스별 서버수 + 임의수 + 체크섬(4091으로 나눈 나머지) 값으로 구성한다.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string CreateEEKey()
        {
            CheckLicense ChkLic = new CheckLicense();
            try
            {
                var r = new Random();

                int iRandom1 = Convert.ToInt16(r.Next(10, 15));
                ChkLic.iWindows = CREATE_LICENSE_OPTIONS.iWindows;
                ChkLic.iWeb = CREATE_LICENSE_OPTIONS.iWeb;
                ChkLic.iSQL = CREATE_LICENSE_OPTIONS.iSQL;
                ChkLic.iSharePoint = CREATE_LICENSE_OPTIONS.iSharePoint;
                ChkLic.iBizTalk = CREATE_LICENSE_OPTIONS.iBizTalk;

                // 각 서비스 별로 255 대 이하로 자리수(16진수 2자리)를 맞춘다. FF = 255
                if (ChkLic.iWindows > (255))
                    ChkLic.iWindows = 255;

                if (ChkLic.iWeb > (255))
                    ChkLic.iWeb = 255;

                if (ChkLic.iSQL > (255))
                    ChkLic.iSQL = 255;

                if (ChkLic.iSharePoint > (255))
                    ChkLic.iSharePoint = 255;

                if (ChkLic.iBizTalk > (255))
                    ChkLic.iBizTalk = 255;

                // 첫 자리는 임의수
                string s1 = string.Format("{0:X}", iRandom1);

                // 둘째 자리부터는 서비스별 서버수 + 16 (16진수 두자리)
                string s2 = string.Format("{0:X2}", ChkLic.iWindows);
                string s3 = string.Format("{0:X2}", ChkLic.iWeb);
                string s4 = string.Format("{0:X2}", ChkLic.iSQL);
                string s5 = string.Format("{0:X2}", ChkLic.iSharePoint);
                string s6 = string.Format("{0:X2}", ChkLic.iBizTalk);

                int iRandom2 = Convert.ToInt16(r.Next(16, 255));
                string s7 = string.Format("{0:X}", iRandom2);

                string strKey = s1 + s2 + s3 + s4 + s5 + s6;

                long iKey1 = Convert.ToInt64(strKey, 16);
                iKey1 = iKey1 + (iRandom2 * 12345678912);           // 난수를 더해 앞의 수를 변형함.

                strKey = string.Format("{0:X}", iKey1) + s7;        // 변형한 난수 2자리 추가

                long iKey2 = Convert.ToInt64(strKey, 16);           // 난수 포함한 키로 16진수키 생성.

                long iMod = iKey2 % 4091;         // 4091 로 나눈 나머지를 마지막 세자리에 더한다.
                string s8 = string.Format("{0:X3}", iMod);

                strKey = strKey + s8;

                return strKey;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool CheckEEKey(string strUserEEKey)
        {
            try
            {
                if (strUserEEKey.Length != 16)
                    return false;

                string strKey = strUserEEKey.Substring(0, 13);      // 뒤에 나머지(4091로 나눈) 자리 세자리 뺀다.

                long iKey = Convert.ToInt64(strKey, 16);
                string strMod = strUserEEKey.Substring(13, 3);
                int iMod = Convert.ToInt32(strMod, 16);             // 나머지로 더해진 세자리 구함.

                if ((iKey % 4091) != iMod)                          // 나머지값이 틀리면 잘못된 제품키임.
                    return false;

                string strRandom = strKey.Substring(11, 2);         // 뒤에 더해진 난수 구함
                int iRandom = Convert.ToInt32(strRandom, 16);

                strKey = strKey.Substring(0, 11);

                iKey = Convert.ToInt64(strKey, 16) - iRandom * 12345678912;        // 더해진 난수만큼 뺀다.

                strKey = string.Format("{0:X}", iKey);

                CHECK_LICENSE_VALUES.iWindows = Convert.ToInt32(strKey.Substring(1, 2), 16);
                CHECK_LICENSE_VALUES.iWeb = Convert.ToInt32(strKey.Substring(3, 2), 16);
                CHECK_LICENSE_VALUES.iSQL = Convert.ToInt32(strKey.Substring(5, 2), 16);
                CHECK_LICENSE_VALUES.iSharePoint = Convert.ToInt32(strKey.Substring(7, 2), 16);
                CHECK_LICENSE_VALUES.iBizTalk = Convert.ToInt32(strKey.Substring(9, 2), 16);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}