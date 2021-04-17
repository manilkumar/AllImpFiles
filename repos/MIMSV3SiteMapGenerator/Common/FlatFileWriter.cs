using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MIMSV3SiteMapGenerator.Urls;
using System.Web;

namespace MIMSV3SiteMapGenerator
{
    public class FlatFileWriter
    {
        StreamWriter _writer;
        private string _directory;
        private string _domainUrl;
        private string _filename;

        public FlatFileWriter(string directory, string domainUrl)
        {
            _directory = directory.EndsWith("/") ? directory : directory + "/";
            _domainUrl = domainUrl;
        }

        public void WriteUrl(IUrl url)
        {
            _writer.WriteLine(HttpUtility.UrlPathEncode(url.ToUrl(_domainUrl)));
        }

        public void BeginWrite(string filename)
        {
            _filename = filename;
            _writer = new StreamWriter(_directory + _filename);
        }

        public string EndWrite()
        {
            _writer.Flush();
            _writer.Close();
            _writer.Dispose();

            return _directory + _filename;
        }
    }
}
