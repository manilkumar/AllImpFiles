using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public class CompanyUrl : IUrl
    {
        public string CountryName { get; set; }
        public string CompanyName { get; set; }

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            string url = string.Format("{0}{1}/Company/Info/{2}", urlBase, CountryName, CompanyName);
            return Utility.fixURL(url.ToLower());
        }
    }
}
