using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.IO;
using System.Web.Caching;

namespace ServicePoint.Lib
{
    public class Util
    {
        #region Function
        public static double GetRate(double x, double y)
        {
            double rate = 0f;
            if (y > 0)
                rate = (Convert.ToDouble(x) / Convert.ToDouble(y)) * 100;

            return Math.Round(rate, 2);
        }

        public static double FormatBytesToDouble(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return TConverter<double>((String.Format("{0:0.##}", dblSByte)));
        }

        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public static void SetCache(string strCacheKey, DataTable dtCacheValue, int numInterval)
        {
            HttpContext.Current.Cache.Insert(strCacheKey, dtCacheValue, null, DateTime.Now.AddSeconds(numInterval), Cache.NoSlidingExpiration);
        }

        public static object GetCache(string strCacheKey)
        {
            return HttpContext.Current.Cache[strCacheKey];
        }

        public static void SetCache(string strCacheKey, string strValue, int numInterval)
        {
            HttpContext.Current.Cache.Insert(strCacheKey, strValue, null, DateTime.Now.AddSeconds(numInterval), Cache.NoSlidingExpiration);
        }

        public static void SetCache(string strCacheKey, int numValue, int numInterval)
        {
            HttpContext.Current.Cache.Insert(strCacheKey, numValue, null, DateTime.Now.AddSeconds(numInterval), Cache.NoSlidingExpiration);
        }

        public static string FormatNumber(float num, int point)
        {
            if (point > 0)
            {
                num = (float)(num * Math.Pow(10, point));
                num = (float)(num < 0 ? Math.Floor(num) + 1 : Math.Floor(num));
                num = (float)(num / Math.Pow(10, point));

                string formatString = String.Empty;
                for (int i = 0; i < point; i++)
                {
                    formatString += "#";
                }

                return String.Format("{0:#,0." + formatString + "}", num);
            }
            else
            {
                num = (float)(num < 0 ? Math.Floor(num) + 1 : Math.Floor(num));
                return String.Format("{0:#,0}", num);
            }
        }

        public static string FormatNumber(float num)
        {
            return FormatNumber(num, 0);
        }

        public static string FormatNumber(int num)
        {
            return String.Format("{0:#,0}", num);
        }

        public static string FormatNumber(Int64 num)
        {
            return String.Format("{0:#,0}", num);
        }

        public static string FormatRange(float min, float max, float lowerBound, float upperBound, int point)
        {
            string strGreaterEqual = "{x} 이상";
            string strLessEqual = "{y} 이하";
            string strGELE = "{x} 이상 {y} 이하";

            if (min <= lowerBound && max >= upperBound) return strGreaterEqual.Replace("{x}", FormatNumber(min, point));
            else if (min <= lowerBound && max < upperBound) return strLessEqual.Replace("{y}", FormatNumber(max, point));
            else if (min > lowerBound && max >= upperBound) return strGreaterEqual.Replace("{x}", FormatNumber(min, point));
            else return strGELE.Replace("{x}", FormatNumber(min, point)).Replace("{y}", FormatNumber(max, point));
        }

        public static string FormatRange(int min, int max, int lowerBound, int upperBound)
        {
            string strGreaterEqual = "{x} 이상";
            string strLessEqual = "{y} 이하";
            string strGELE = "{x} 이상 {y} 이하";

            if (min <= lowerBound && max >= upperBound) return strGreaterEqual.Replace("{x}", FormatNumber(min));
            else if (min <= lowerBound && max < upperBound) return strLessEqual.Replace("{y}", FormatNumber(max));
            else if (min > lowerBound && max >= upperBound) return strGreaterEqual.Replace("{x}", FormatNumber(min));
            else return strGELE.Replace("{x}", FormatNumber(min)).Replace("{y}", FormatNumber(max));
        }

        public static string FomatTimeSpan(long sec)
        {
            string str = "";
            TimeSpan ts = TimeSpan.FromSeconds(sec);
            str = String.Format("{0:00}:{1:00}:{2:00}", ts.Days * 24 + ts.Hours, ts.Minutes, ts.Seconds);

            return str;
        }

        public static string ConvertEmpty2Null(string input)
        {
            if (input == String.Empty)
                input = null;

            return input;
        }
        public static string ConvertNull2Empty(string input)
        {
            if (input == null)
                input = String.Empty;
            else if (input == "1400-01-01 00:00:00")
                input = String.Empty;

            return input;
        }

        public static T TConverter<T>(object value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static string GetMd5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                sb.Append(result[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string GetStringSeperate(string str, string seperator)
        {
            return (!String.IsNullOrEmpty(str) ? seperator : String.Empty) + str;
        }
        
        public static string GetPublisher()
        {
            #if NHN
			            return "nhn";
            #elif GAMEON
			            return "gameon";
            #elif GAMEFLIER
			            return "gameflier";
            #elif TENCENT
			            return "tencent";
            #else
                        return "allm";
            #endif
        }
        #endregion

        #region HTTP Function
        public static HttpCookie GetCookie(System.Web.HttpRequest Request, string key)
        {
            return Request.Cookies.Get(key);
        }
        public static string GetCookieValue(System.Web.HttpRequest Request, string key)
        {
            string value = String.Empty;
            HttpCookie cookie = Request.Cookies.Get(key);
            try
            {
                value = cookie.Value;
            }
            catch { }
            return value;
        }
        public static string GetCookieValue(System.Web.HttpRequest Request, string key1, string key2)
        {
            string value = String.Empty;
            HttpCookie cookie = Request.Cookies.Get(key1);
            try
            {
                value = cookie[key2];
            }
            catch { }
            return value;
        }

        public static void SetCookieValue(HttpResponse Response, string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = HttpUtility.UrlEncodeUnicode(value); // IE 에서는 UTF-9 stirng 의 cookie 를 지원하지 않는 문제 해결을 위해 Unicode encoding 사용
            Response.Cookies.Add(cookie);
        }
        public static void SetCookieValue(HttpResponse Response, string key, string value, DateTime expire)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(key);
                cookie.Value = value;
                cookie.Expires = expire;
                Response.Cookies.Add(cookie);
            }
            catch { }
        }
        public static void SetCookieValue(HttpResponse Response, string key, string value, bool keep)
        {
            try
            {
                if (keep)
                {
                    SetCookieValue(Response, key, value, DateTime.Now.AddYears(1));
                }
                else
                {
                    SetCookieValue(Response, key, value);
                }
            }
            catch { }
        }
        public static void SetCookie(HttpResponse Response, HttpCookie cookie)
        {
            try
            {
                Response.Cookies.Add(cookie);
            }
            catch { }
        }
        public static void RemoveCookie(HttpResponse Response, string key)
        {
            SetCookieValue(Response, key, String.Empty, DateTime.Now.AddYears(-1));
        }
        #endregion

        #region HTML Function
        public static void Alert(HttpResponse Response, string message)
        {
            Response.Write(AlertScript(message));
        }
        public static void Alert(HttpResponse Response, string message, string url)
        {
            Response.Write(AlertScript(message, url));
        }
        public static string AlertScript(string message)
        {
            string script = getAlertScript(message);
            return BoxingScript(script);
        }
        public static string AlertScript(string message, string url)
        {
            string script = "location.href='" + url + "';" + getAlertScript(message);
            return BoxingScript(script);
        }
        public static string BoxingScript(string script)
        {
            return "<script type='text/javascript'>" + script + "</script>";
        }
        private static string getAlertScript(string message)
        {
            if (message == null || message == String.Empty)
            {
                return String.Empty;
            }
            else
            {
                return "alert('" + message + "');";
            }
        }

        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
        #endregion
        #region ByteConvert Function
        public static string FuncMemoryValue(object objValue, string strColumn)
        {
            double numMemory = TConverter<double>(objValue);
            double numMemoryValue = 0;
            string strByte = "";
            if (strColumn.ToUpper() == "AVAILABLEMEMORY")
            {
                
                    numMemoryValue = (numMemory / (1024));
                    strByte = "GB";
               
            }
            else
            {
                    numMemoryValue = (numMemory / (1024 * 1024 * 1024));
                    strByte = "GB";
            }
            return Math.Round(numMemoryValue, 2).ToString() + strByte;
        }
        public static string ConvertToFileSizeWitFormatWithNullCheck(object intByte)
        {
            double intFileSize = Convert.ToDouble((intByte == DBNull.Value) ? 0 : intByte);
            intFileSize = intFileSize / 1024;
            string strResult = "";
            strResult = string.Format("{0:#,##0}", intFileSize);
            return strResult;
        }
        public static string ConvertToFileSizeToKilloBytesWithNullCheck(object intByte)
        {
            double intFileSize = Convert.ToDouble((intByte == DBNull.Value) ? 0 : intByte);
            string strResult = "";
            strResult = string.Format("{0:#,##0}", Math.Round((intFileSize / 1000), 0));
            return strResult;
        }
        public static string ConvertToFileSizeToMegaBytesWithNullCheck(object intByte)
        {
            double intFileSize = Convert.ToDouble((intByte == DBNull.Value) ? 0 : intByte);
            string strResult = "";
            strResult = string.Format("{0:#,##0.0}", Math.Round((intFileSize / 1000000), 1));
            return strResult;
        }
        public static string ConvertToFileSizeToMegaBytesWithNullCheckInt(object intByte)
        {
            double intFileSize = Convert.ToDouble((intByte == DBNull.Value) ? 0 : intByte);
            string strResult = "";
            strResult = string.Format("{0:#,##0}", Math.Round((intFileSize / 1000000), 0));
            return strResult;
        }
        public static double FuncCommittedByAvailable(object committed, object ramSize)
        {
           
                double c = TConverter<double>(committed) / (1024 * 1024); // SQL 쿼리에서 Committed는 100만으로 나누지 않았음, 원칙은 1048576으로 나누어야 함
                double a = TConverter<double>(ramSize) / (1024 * 1024);

                double percent = (c / a) * 100;

                return Math.Round(percent, 1);
           
        }
        public static string ConvertSize(object value, Int64 mod, int numRoundPosition)
        {
            double dblValue = TConverter<double>(value);
            return Math.Round((dblValue / (mod)), numRoundPosition).ToString();
        }
        public static double ConvertRound(object value)
        {
            double dbValue = TConverter<double>(value);

            return dbValue;
        }
        public static double ConvertRound(object value, int numRoundPosition)
        {
            double dbValue = ConvertRound(value);
            return Math.Round(dbValue, numRoundPosition);
        }
        public static double ByteExchange(object value, int numMod, int numRoundPosition)
        {
            double dbValue = TConverter<double>(value);
            return Math.Round((dbValue / numMod), numRoundPosition);
        }
        #endregion
        #region Game Function
        public static string GetAdminUrl(int numWorld)
        {
            string url = String.Empty;
            string key = "worldServerUrl_" + numWorld.ToString();
            try
            {
                url = ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
            }

            return url;
        }
        public static string ReplaceHangameId(System.Web.HttpRequest Request, string strAccountId)
        {
            HttpCookie cookie = GetCookie(Request, "Authorize");
            if (TConverter<int>(cookie.Values["game_hangameid"]) == 0)
            {
                return "******";
            }
            else
            {
                return strAccountId;
            }
        }
        public static string CreateLicense(int numWindows, int numWeb, int numSql, int numSharePoint, int numBiztalk)
        {
            License Lic = new License();
            Lic.CREATE_LICENSE_OPTIONS.iWindows = numWindows;
            Lic.CREATE_LICENSE_OPTIONS.iWeb = numWeb;
            Lic.CREATE_LICENSE_OPTIONS.iSQL = numSql;
            Lic.CREATE_LICENSE_OPTIONS.iSharePoint = numSharePoint;
            Lic.CREATE_LICENSE_OPTIONS.iBizTalk = numBiztalk;

            return Lic.CreateEEKey();         // 제품키 생성됨

        }
        public static bool CheckLicense(string strKey, out int numWindow, out int numWeb, out  int numSql, out  int numSharePoint, out  int numBiztalk)
        {
            bool bitCheckKey = false;
            License Lic = new License();
            if (Lic.CheckEEKey(strKey))
                bitCheckKey = true;

            //서버 총사용 가능대수
            numWindow = Lic.CHECK_LICENSE_VALUES.iWindows;
            numWeb = Lic.CHECK_LICENSE_VALUES.iWeb;
            numSql = Lic.CHECK_LICENSE_VALUES.iSQL;
            numSharePoint = Lic.CHECK_LICENSE_VALUES.iSharePoint;
            numBiztalk = Lic.CHECK_LICENSE_VALUES.iBizTalk;

            return bitCheckKey;

        }
        /// <summary>
        /// Gets a Inverted DataTable
        /// </summary>
        /// <param name="table">Provided DataTable</param>
        /// <param name="columnX">X Axis Column</param>
        /// <param name="columnY">Y Axis Column</param>
        /// <param name="columnZ">Z Axis Column (values)</param>
        /// <param name="columnsToIgnore">Whether to ignore some column, it must be 
        /// provided here</param>
        /// <param name="nullValue">null Values to be filled</param> 
        /// <returns>C# Pivot Table Method  - Felipe Sabino</returns>
        public static DataTable GetInversedDataTable(DataTable table, string columnX,
             string columnY, string columnZ, string nullValue, bool sumValues)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnY);


            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            foreach (DataRow dr in table.Rows)
            {

                string columnXTemp = dr[columnX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    //Read each row value, if it's different from others provided, add to 
                    //the list of values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //Verify if Y and Z Axis columns re provided
            if (columnY != "" && columnZ != "")
            {
                //Read DISTINCT Values for Y Axis Column
                List<string> columnYValues = new List<string>();

                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnY].ToString()))
                        columnYValues.Add(dr[columnY].ToString());
                }

                //Loop all Column Y Distinct Value
                foreach (string columnYValue in columnYValues)
                {
                    //Creates a new Row
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;
                    //foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select(columnY + "='" + columnYValue + "'");

                    //Read each row to fill the DataTable
                    foreach (DataRow dr in rows)
                    {
                        string rowColumnTitle = dr[columnX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //If Sum of Values is True it try to perform a Sum
                                //If sum is not possible due to value types, the value 
                                // displayed is the last one read
                                if (sumValues)
                                {
                                    try
                                    {
                                        drReturn[rowColumnTitle] =
                                             Convert.ToDecimal(drReturn[rowColumnTitle]) +
                                             Convert.ToDecimal(dr[columnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[columnZ];
                                    }
                                }
                                else
                                {
                                    drReturn[rowColumnTitle] = dr[columnZ];
                                }
                            }
                        }
                    }
                    returnTable.Rows.Add(drReturn);
                }
            }
            else
            {
                throw new Exception("The columns to perform inversion are not provided");
            }

            //if a nullValue is provided, fill the datable with it
            if (nullValue != "")
            {
                foreach (DataRow dr in returnTable.Rows)
                {
                    foreach (DataColumn dc in returnTable.Columns)
                    {
                        if (dr[dc.ColumnName].ToString() == "")
                            dr[dc.ColumnName] = nullValue;
                    }
                }
            }

            return returnTable;
        }
        /// <summary>
        /// 암호화
        /// 기본 암호화 해시 함수 : SHA512
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <returns></returns>
        public static string EncryptText(string plainText)
        {
            string strCryptoCheck = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoCheck"].ToString();     // 해시함수
            string strCryptoType = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoType"].ToString();       // 암호화 알고리즘
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return EncryptText(plainText, strCryptoCheck, strCryptoType, strCryptoKey);
        }

        /// <summary>
        /// 암호화
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <param name="EncryptType">암호화 알고리즘</param>
        /// <returns></returns>
        public static string EncryptText(string plainText, string EncryptType)
        {
            string strCryptoCheck = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoCheck"].ToString();     // 해시함수
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return EncryptText(plainText, strCryptoCheck, EncryptType, strCryptoKey);
        }

        /// <summary>
        /// 암호화
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <param name="EncryptType">암호화 알고리즘</param>
        /// <returns></returns>
        public static string EncryptText(string plainText, string EncryptCheck, string EncryptType)
        {
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return EncryptText(plainText, EncryptCheck, EncryptType, strCryptoKey);
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="plainText">평문</param>
        /// <param name="EncryptCheck">암호화 해시 함수</param>
        /// <param name="EncryptType">암호화 알고리즘</param>
        /// <returns></returns>
        public static string EncryptText(string plainText, string EncryptCheck, string EncryptType, string CryptoKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            string key = CryptoKey;

            switch (EncryptCheck.Trim().ToUpper().ToString())
            {
                case "MD5":
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                    break;
                case "SHA1":
                    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                    keyArray = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha1.Clear();
                    break;
                case "SHA256":
                    SHA256Managed sha256 = new SHA256Managed();
                    keyArray = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha256.Clear();
                    break;
                case "SHA384":
                    SHA384Managed sha384 = new SHA384Managed();
                    keyArray = sha384.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha384.Clear();
                    break;
                case "SHA512":
                    SHA512Managed sha512 = new SHA512Managed();
                    keyArray = sha512.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha512.Clear();
                    break;
                default:
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    break;
            }

            byte[] resultArray = null;

            if (keyArray.Length > 16)
            {
                byte[] temp = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = keyArray[i];
                }

                keyArray = temp;
            }

            if (keyArray.Length < 16)
            {
                byte[] temp = new byte[16];
                for (int j = keyArray.Length; j < 16; j++)
                {
                    temp[j] = keyArray[0];
                }

                keyArray = temp;
            }

            switch (EncryptType.Trim().ToUpper().ToString())
            {

                case "RC2":
                    RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                    rc2.Key = keyArray;
                    rc2.Mode = CipherMode.ECB;
                    rc2.Padding = PaddingMode.PKCS7;

                    ICryptoTransform rc2Transform = rc2.CreateEncryptor();
                    resultArray = rc2Transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    rc2.Clear();
                    break;
                case "TRIPLE":
                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform tdesTransform = tdes.CreateEncryptor();
                    resultArray = tdesTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    tdes.Clear();
                    break;
                case "DES":
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    des.Key = UTF8Encoding.UTF8.GetBytes(key.Substring(0, 8).ToString().Trim());
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;

                    ICryptoTransform desTransform = des.CreateEncryptor();
                    resultArray = desTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    des.Clear();
                    break;
                case "AES":
                    RijndaelManaged Rijndae = new RijndaelManaged();
                    Rijndae.Key = keyArray;
                    Rijndae.Mode = CipherMode.ECB;
                    Rijndae.Padding = PaddingMode.PKCS7;

                    ICryptoTransform RijndaeTransform = Rijndae.CreateEncryptor();
                    resultArray = RijndaeTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    Rijndae.Clear();
                    break;
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        /// <summary>
        /// 복호화
        /// 기본 암호화 해시 함수 : SHA512
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <returns></returns>
        public static string DecryptText(string cipherText)
        {
            string strCryptoCheck = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoCheck"].ToString();     // 해시함수
            string strCryptoType = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoType"].ToString();       // 암호화 알고리즘
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return DecryptText(cipherText, strCryptoCheck, strCryptoType, strCryptoKey);
        }

        /// <summary>
        /// 복호화
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <param name="DecryptType">암호화 알고리즘</param>
        /// <returns></returns>
        public static string DecryptText(string cipherText, string DecryptType)
        {
            string strCryptoCheck = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoCheck"].ToString();     // 해시함수
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return DecryptText(cipherText, strCryptoCheck, DecryptType, strCryptoKey);
        }

        /// <summary>
        /// 복호화
        /// 기본 암호화 알고리즘 : AES
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <param name="DecryptType">암호화 알고리즘</param>
        /// <returns></returns>
        public static string DecryptText(string cipherText, string DecryptCheck, string DecryptType)
        {
            string strCryptoKey = System.Configuration.ConfigurationManager.AppSettings["DefaultCryptoKey"].ToString();         // 키

            return DecryptText(cipherText, DecryptCheck, DecryptType, strCryptoKey);
        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="cipherText">암호문</param>
        /// <param name="DecryptCheck">암호화 해시 함수</param>
        /// <param name="DecryptType">암호화 알고리즘</param>
        public static string DecryptText(string cipherText, string DecryptCheck, string DecryptType, string CryptoKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherText);

            string key = CryptoKey;

            switch (DecryptCheck.Trim().ToUpper().ToString())
            {
                case "MD5":
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                    break;
                case "SHA1":
                    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                    keyArray = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha1.Clear();
                    break;
                case "SHA256":
                    SHA256Managed sha256 = new SHA256Managed();
                    keyArray = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha256.Clear();
                    break;
                case "SHA384":
                    SHA384Managed sha384 = new SHA384Managed();
                    keyArray = sha384.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha384.Clear();
                    break;
                case "SHA512":
                    SHA512Managed sha512 = new SHA512Managed();
                    keyArray = sha512.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    sha512.Clear();
                    break;
                default:
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    break;
            }

            byte[] resultArray = null;

            if (keyArray.Length > 16)
            {
                byte[] temp = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    temp[i] = keyArray[i];
                }

                keyArray = temp;
            }

            if (keyArray.Length < 16)
            {
                byte[] temp = new byte[16];
                for (int j = keyArray.Length; j < 16; j++)
                {
                    temp[j] = keyArray[0];
                }

                keyArray = temp;
            }

            switch (DecryptType.Trim().ToUpper().ToString())
            {
                case "RC2":
                    RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                    rc2.Key = keyArray;
                    rc2.Mode = CipherMode.ECB;
                    rc2.Padding = PaddingMode.PKCS7;

                    ICryptoTransform rc2Transform = rc2.CreateDecryptor();
                    resultArray = rc2Transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    rc2.Clear();
                    break;
                case "TRIPLE":
                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform tdesTransform = tdes.CreateDecryptor();
                    resultArray = tdesTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    tdes.Clear();
                    break;
                case "DES":
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    des.Key = UTF8Encoding.UTF8.GetBytes(key.Substring(0, 8).ToString().Trim());
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;

                    ICryptoTransform desTransform = des.CreateDecryptor();
                    resultArray = desTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    des.Clear();
                    break;
                case "AES":
                    RijndaelManaged Rijndae = new RijndaelManaged();
                    Rijndae.Key = keyArray;
                    Rijndae.Mode = CipherMode.ECB;
                    Rijndae.Padding = PaddingMode.PKCS7;

                    ICryptoTransform RijndaeTransform = Rijndae.CreateDecryptor();
                    resultArray = RijndaeTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    Rijndae.Clear();
                    break;
            }

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static byte[] func_DataTableToByte(DataTable dt)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, dt);
                return stream.GetBuffer();
            }
            catch (Exception e)
            {
                return new byte[0];
            }
        }
        public static DataTable func_ByteToDataTable(byte[] bytearray)
        {
            try
            {
                System.IO.MemoryStream stream = new MemoryStream(bytearray);
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                DataTable dt = (DataTable)formatter.Deserialize(stream);

                //ds.ReadXml(sr);
                if (dt == null)
                {
                    return new DataTable();
                }
                return dt;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }

        }
        #endregion

        #region Temp
        public static void logging1()
        {
            StreamWriter sw = new StreamWriter("E:\\03.Test\\01.Project\\Sdb\\Sdb\\Log\\log.txt", true);
            sw.WriteLine(System.DateTime.Now.ToString());
            sw.Close();
        }
        public static void logging2()
        {
            StreamWriter sw = new StreamWriter("E:\\03.Test\\01.Project\\Sdb\\Sdb\\Log\\log2.txt", true);
            sw.WriteLine(System.DateTime.Now.ToString());
            sw.Close();
        }
        public static double getDateTime(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = TheDate;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return ts.TotalMilliseconds;
        }
        #endregion
    }
}
