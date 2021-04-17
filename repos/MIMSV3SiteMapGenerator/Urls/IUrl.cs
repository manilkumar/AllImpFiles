using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.Urls
{
    public interface IUrl
    {
        string ToUrl(string urlBase);
    }
}
