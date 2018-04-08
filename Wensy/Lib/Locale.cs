using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections;
using System.Reflection;

namespace ServicePoint.Lib
{
    public sealed class Locale
    {
        static readonly Locale instance = new Locale();
        bool inited;

        static Locale()
        {
        }
        private Locale()
        {

            Init();
        }
        public static Locale Instance
        {
            get
            {
                return instance;
            }
        }

        #region Member

        public SortedDictionary<string, SortedDictionary<string, string>> dicLocalelist = new SortedDictionary<string, SortedDictionary<string, string>>();
        public SortedDictionary<string, SortedDictionary<string, string>> DicLocalelist
        {
            get
            {
                if (!inited)
                {
                    Init();
                }
                return dicLocalelist;
            }
        }
        #endregion
        private void Init()
        {
            initLocale();
            inited = true;
        }
        private void initLocale()
        {
            SortedDictionary<string, SortedDictionary<string, string>> list = new SortedDictionary<string, SortedDictionary<string, string>>();


            //이부분에 반복문으로 로케일 리소스에서 국가별로 일어서 데이터를 입력해줍니다~
            //엑셀로 가져오든 텍스트를 읽던 잘가져와서 키값을 항상 소문자로 입력되게합니다 .ToLower() 함수로...
            // 호출시에 항상 소문자로 ㄱㄱ

            var resxUrl = HttpContext.Current.Request.MapPath(@"/App_GlobalResource/resSettings.resx");
            var resxUrlko = HttpContext.Current.Request.MapPath(@"/App_GlobalResource/resSettings.ko.resx");
            var resxUrlja = HttpContext.Current.Request.MapPath(@"/App_GlobalResource/resSettings.ja.resx");


            ResXResourceReader resxReader = new ResXResourceReader(resxUrl);
            ResXResourceReader resxReaderko = new ResXResourceReader(resxUrlko);
            ResXResourceReader resxReaderja = new ResXResourceReader(resxUrlja);

            list.Add("ko", new SortedDictionary<string, string>());
            foreach (DictionaryEntry a in resxReaderko)
            {
                list["ko"].Add(a.Key.ToString().ToLower(), a.Value.ToString());
            }

            list.Add("en", new SortedDictionary<string, string>());
            foreach (DictionaryEntry a in resxReader)
            {
                list["en"].Add(a.Key.ToString().ToLower(), a.Value.ToString());
            }

            list.Add("ja", new SortedDictionary<string, string>());
            foreach (DictionaryEntry a in resxReaderja)
            {
                list["ja"].Add(a.Key.ToString().ToLower(), a.Value.ToString());
            }

            dicLocalelist = list;
        }


        #region function
        public SortedDictionary<string, string> GetLocaleList(string localeCode)
        {

            if (dicLocalelist.ContainsKey(localeCode))
                return dicLocalelist[localeCode];
            else
                return new SortedDictionary<string, string>();
        }
        public string GetLocaleValue(string localeCode, string localeKey)
        {
            if (localeKey == null)
                localeKey = string.Empty;
            else
                localeKey = localeKey.ToLower();

            string[] strLocaleCode = localeCode.Split('-');

            if (!dicLocalelist.ContainsKey(strLocaleCode[0]))
                strLocaleCode[0] = "en";


            SortedDictionary<string, string> currentLocaleList = GetLocaleList(strLocaleCode[0]);

            if (currentLocaleList.ContainsKey(localeKey))
                return currentLocaleList[localeKey];
            else
                return localeKey;
        }
        public string GetLocaleValue(string localeKey)
        {

            string localeCode = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2); ;//쿠키나 세션에 현재 로케일값을 적어준다
            if (string.IsNullOrEmpty(localeCode))
                localeCode = "en";

            return GetLocaleValue(localeCode, localeKey); // 여기에 localeCode 는 함수를 호출할때 넘겨받는게아니라 세션이나 쿠키에서 가져옵니다
        }
        #endregion
    }
}
