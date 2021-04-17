using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using MIMSV3SiteMapGenerator.Urls;
using System.IO;

namespace MIMSV3SiteMapGenerator
{
    public class SiteMapProtocol
    {
        private XmlTextWriter _xmlWriter;
        private string _directory;
        private string _domainUrl;
        private string _filename;

        public SiteMapProtocol(string directory, string domainUrl)
        {
            if (!directory.EndsWith("/"))
            {
                directory += "/";
            }

            _directory = directory;
            _domainUrl = domainUrl;
        }

        public void StartIndexFile()
        {
            _filename = "SiteMapIndex.xml";
            _xmlWriter = new XmlTextWriter(_directory + _filename, Encoding.UTF8);
            _xmlWriter.Formatting = Formatting.Indented;
            _xmlWriter.Indentation = 1;
            _xmlWriter.IndentChar = '\t';
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
        }

        public void AddIndexNode(string filename, DateTime lastModifiedDate)
        {
            _xmlWriter.WriteStartElement("sitemap");

            _xmlWriter.WriteStartElement("loc");
            _xmlWriter.WriteValue(string.Format("{0}/{1}", _domainUrl, filename));
            _xmlWriter.WriteEndElement();

            _xmlWriter.WriteStartElement("lastmod");
            _xmlWriter.WriteValue(lastModifiedDate.ToString("yyyy-MM-dd"));
            _xmlWriter.WriteEndElement();

            _xmlWriter.WriteEndElement();
        }

        public void StartSitemapFile(string countryCode, int fileIndex)
        {

            _filename = string.Format("SiteMap-{0}-{1}.xml", countryCode, fileIndex);
            string path = string.Format("{0}{1}", _directory, _filename);

            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            _xmlWriter = new XmlTextWriter(path, Encoding.UTF8);
            _xmlWriter.Formatting = Formatting.Indented;
            _xmlWriter.Indentation = 1;
            _xmlWriter.IndentChar = '\t';
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        }

        public string AddUrlNode(IUrl url, DateTime lastModifiedDate)
        {
            var hrefUrl = HttpUtility.UrlPathEncode(url.ToUrl(_domainUrl));
            _xmlWriter.WriteStartElement("url");

            _xmlWriter.WriteStartElement("loc");
            _xmlWriter.WriteValue(HttpUtility.UrlPathEncode(url.ToUrl(_domainUrl)));
            _xmlWriter.WriteEndElement();

            _xmlWriter.WriteEndElement();
            _xmlWriter.Flush();
            return hrefUrl;
        }

        public string EndFile()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.Flush();
            return _filename;
        }

        public void Dispose()
        {
            _xmlWriter.Flush();
            _xmlWriter.Dispose();
        }

    }
}
