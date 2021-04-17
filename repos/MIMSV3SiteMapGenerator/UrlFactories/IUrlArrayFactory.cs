using MIMSV3SiteMapGenerator.Urls;
using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public interface IUrlArrayFactory
    {
        List<IUrl> GetNext();
    }
}
