using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public class PatientMedInfoUrl : IUrl
    {
        public string CountryName { get; set; }
        public string Description { get; set; }

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            //string safeDescription = Uri.EscapeDataString(PatientMedInfoDescription);

            string url = string.Format("{0}{1}/patientmedicine/generic/{2}",
                urlBase, CountryName, Description);

            return Utility.fixURL(url.ToLower());
        }
    }
}
