using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public class DrugImageUrl : IUrl
    {
        public string CountryName { get; set; }
        public string ProductName { get; set; }
        public string ProductStrength { get; set; }
        public string DrugImageId { get; set; }

        public string ToUrl(string urlBase)
        {
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            return Utility.fixURL(string.Format("{0}{1}/image/info/{2}/{3}",
                                urlBase, CountryName, ProductName,
                                ProductStrength).ToLower());
        }
    }
}
