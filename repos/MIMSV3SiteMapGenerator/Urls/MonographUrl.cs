using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public enum MonoGraphUrlType
    {
        BRIEF,
        BASIC,
        FULL,
        VIDAL
    }

    public class MonographUrl : IUrl
    {
        public string CountryName { get; set; }
        public string BrandName { get; set; }
        public string MonographName { get; set; }
        public MonoGraphUrlType UrlType { get; set; }
        public bool IsGeneric { get; set; }
        private IDictionary<string, string> _monographAlias;

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            //string brandName = Utility.CleanUpNames(BrandName);
            //string monographName = Utility.CleanUpNames(MonographName);

            // If brand name and mononame are the same, remove the mononame.
            // This is to avoid duplication in drug info URL
            string url = string.Format("{0}{1}/drug/info/{2}",
                    urlBase, CountryName, BrandName);

            if (string.Compare(BrandName, MonographName, true) != 0)
            {
                url = string.Format("{0}{1}/drug/info/{2}/{3}",
                    urlBase, CountryName, BrandName, MonographName);
            }

            if (UrlType == MonoGraphUrlType.FULL)
            {
                url += "?type=full";
            }
            else if (UrlType == MonoGraphUrlType.VIDAL)
            {
                url += "?type=vidal";
            }

            if (IsGeneric)
            {
                url += url.Contains("?type") ? "&mtype=generic" : "?mtype=generic";
            }

            return Utility.fixURL(url.ToLower());
        }
    }
}
