using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public class FullMonographSectionURL : IUrl
    {
        public string CountryName { get; set; }
        public string BrandName { get; set; }
        public string MonographName { get; set; }
        public string SectionName { get; set; }

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            // If brand name and mononame are the same, remove the mononame.
            // This is to avoid duplication in drug info URL
            string url = string.Format("{0}{1}/drug/info/{2}/{3}",
                    urlBase, CountryName, BrandName, SectionName);

            return Utility.fixURL(url.ToLower());
        }
    }
}
