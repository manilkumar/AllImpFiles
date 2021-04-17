using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator
{
    public class HrefLangUrlEntity : TableEntity
    {
        public HrefLangUrlEntity(string requestURL, string id)
    : base(requestURL, id) { }

        public HrefLangUrlEntity() { }
        public string Id { get; set; }

        public string Country { get; set; }

        public string RequestURL { get; set; }

        public string HrefLink { get; set; }
    }

}
