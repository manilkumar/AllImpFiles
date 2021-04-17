using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public class DiseasePortalUrl : IUrl
    {
        public string CountryName { get; set; }
        public string Specialty { get; set; }
        public string TreatmentGuideline { get; set; }

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            string url = string.Format("{0}{1}/Disease/Info/{2}/{3}", urlBase, CountryName,
                Specialty, TreatmentGuideline);

            return Utility.fixURL(url.ToLower());
        }
    }
}
