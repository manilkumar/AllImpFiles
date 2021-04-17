using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMSV3SiteMapGenerator
{
    public static class Utility
    {
        public static IList<string> getQueryAliasMappingCondition()
        {
            IList<string> list = new List<string>();
            list.Add("'%/%'");
            list.Add("'%\\%'");
            list.Add("'%<%'");
            list.Add("'%>%'");
            list.Add("'%.'");
            list.Add("'.%'");
            list.Add("'%:%'");
            list.Add("'%*%'");
            list.Add("'%?%'");
            list.Add("'%[%]%'");
            list.Add("'%&%'");
            list.Add("'%\"%'");
            list.Add("'%' + char(13) + char(10) + '%'");
            list.Add("'%''%'");
            list.Add("'%#%'");
            return list;
        }

        private static IDictionary<string, string> URLSafeMappingDict = new Dictionary<string, string>
        {
            {"w/","with"},
            {"\\", "-"},
            {"/", "-"},
            {"<","-"},
            {">", "-"},
            {".", "-"},
            {":", "-"},
            {"*", "-"},
            {"?", "-"},
            {"%", "percent"},
            {"&", "and"},
            {"\r\n", "-cr-"},
            {"\"", "-"},
            {"'", "-"},
            {"#","-"}
        };

        public static string MakeItURLSafe(string unsafeString)
        {
            if (string.IsNullOrEmpty(unsafeString)) return string.Empty;
            string normalCleanString = unsafeString.ToLower();

            //remove all HTML tags
            normalCleanString = Regex.Replace(normalCleanString, @"</?(?:[^>]|\n)*?>", string.Empty);

            //remove non-URL safe characters
            foreach (string key in URLSafeMappingDict.Keys)
            {
                normalCleanString = normalCleanString.Replace(key, URLSafeMappingDict[key]);
            }

            return normalCleanString;
        }

        public static string fixURL(string input)
        {
            string returl = input != null ? input : string.Empty;

            returl = returl != null && (returl.Contains("<") || returl.Contains(">")) ? Regex.Replace(returl, @"<(.|\n)*?>", " ") : returl;
            returl = returl != null && (returl.Contains("%3C") || returl.Contains("%3E")) ? Regex.Replace(returl, @"%3C(.|\n)*?%3E", " ") : returl;

            return returl.ToLower();
        }
    }
}
