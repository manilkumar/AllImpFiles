using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MIMSV3SiteMapGenerator.UrlFactories;
using MIMSV3SiteMapGenerator.Urls;
using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator
{
    public class SiteMapWrapper
    {
        AzureTableOperations azureTableOperations = new AzureTableOperations();
        public void CreateSiteMapIndex(SiteMapProtocol smpWriter, string[] filenames, ILogger log)
        {
            try
            {
                log.LogInformation("Start creating sitemap index... ");
                smpWriter.StartIndexFile();
                foreach (string filename in filenames)
                {
                    smpWriter.AddIndexNode(filename, DateTime.Now);
                }
                smpWriter.EndFile();
                log.LogInformation("done!");
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace + ex.Message);
            }
        }

        public string[] CreateCountrySiteMaps(SiteMapProtocol smpWriter, IConfiguration config, ILogger log)
        {
            List<string> files = new List<string>();
            log.LogInformation("\nCreating sitemaps for all countries");
            try
            {
                foreach (string countryCode in ConnectionStringManager.GetCountryCodes())
                {
                    int fileIndex = 0;

                    log.LogInformation("\nCreating " + countryCode);
                    log.LogInformation(string.Format("\nCreating Monograph Url(s) : {0}. \n", countryCode));
                    fileIndex = AddNodes(smpWriter, countryCode, new MonographUrlFactory(countryCode, config), files, fileIndex, log);
                    log.LogInformation(string.Format("\nCreating DrugImage Url(s) : {0}. \n", countryCode));
                    fileIndex = AddNodes(smpWriter, countryCode, new DrugImageUrlFactory(countryCode, config), files, fileIndex, log);

                    // ---------------------------------------------------------------
                    // Add in Company Url generation
                    // Only generate the company url if enabled in the setting.
                    // ---------------------------------------------------------------
                    if (AppSettings.GenerateCompany)
                    {
                        log.LogInformation(string.Format("\nCreating Company Url(s) : {0}. \n", countryCode));

                        fileIndex = AddNodes(smpWriter, countryCode, new CompanyUrlFactory(countryCode, config), files, fileIndex, log);
                    }
                    log.LogInformation(string.Format("\nCompleted {0} with {1} file(s) written", countryCode, fileIndex));


                    //if (SiteMapGenerator.Default.GenerateDiseasePortal
                    //    && EnabledDiseasePortal(countryCode))
                    //{
                    //    log.LogInformation(string.Format("\nCreating Disease Portal Url(s) : {0}. \n", countryCode));
                    //    //fileIndex = AddNodes(smpWriter, countryCode, new DiseasePortalUrlFactory(countryCode), files, fileIndex);
                    //    log.LogInformation(string.Format("\nCompleted {0} with {1} file(s) written", countryCode, fileIndex));
                    //}


                    if (AppSettings.PatientMedInfoCountries.IndexOf(
                        countryCode, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        log.LogInformation(string.Format("\nCreating Patient MedInfo Url(s) : {0}. \n", countryCode));
                        fileIndex = AddNodes(smpWriter, countryCode, new PatientMedInfoUrlFactory(countryCode, config), files, fileIndex, log);
                        log.LogInformation(string.Format("\nCompleted {0} with {1} file(s) written", countryCode, fileIndex));
                    }

                    if (AppSettings.FullMonographSectionCountries.IndexOf(
                        countryCode, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        log.LogInformation(string.Format("\nCreating Full Monograph Section Url(s) : {0}. \n", countryCode));
                        fileIndex = AddNodes(smpWriter, countryCode, new FullMonographSectionFactory(countryCode, config), files, fileIndex, log);
                        log.LogInformation(string.Format("\nCompleted {0} with {1} file(s) written", countryCode, fileIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace + ex.Message);
            }

            return files.ToArray();
        }

        private static bool EnabledDiseasePortal(string countryCode, IConfiguration config)
        {
            string countries = AppSettings.COUNTRY_DB_AVAILABILITY.ToUpper();
            bool enabled = countries.Contains(countryCode.ToUpper());
            return enabled;
        }

        private int AddNodes(SiteMapProtocol smpWriter, string countryCode, IUrlArrayFactory factory, List<string> files, int fileIndex, ILogger log)
        {

            int nodecount = 1;
            string urlTypeName = "";
            try
            {
                var urllist = factory.GetNext();
                if (urllist == null || urllist.Count == 0)
                {
                    log.LogInformation(string.Format("\n File not created because no nodes exists for the country : {0}.", countryCode));
                    return fileIndex;
                }
                fileIndex++;
                log.LogInformation(string.Format("\n {0} : New file created", countryCode));
                smpWriter.StartSitemapFile(countryCode, fileIndex);
                foreach (var item in urllist)
                {
                    log.LogInformation("Looping urls");
                    log.LogInformation("node count" + nodecount);

                    if (nodecount > AppSettings.SitemapFileNodeLimit)
                    {
                        log.LogInformation("node count > limit.Start new index file");
                        files.Add(smpWriter.EndFile());

                        smpWriter.StartSitemapFile(countryCode, ++fileIndex);
                        nodecount = 1;
                    }
                    var hrefUrl = smpWriter.AddUrlNode(item, DateTime.Now);

                    urlTypeName = string.IsNullOrEmpty(urlTypeName) ? urllist.GetType().Name : urlTypeName;
                    nodecount += 1;
                    log.LogInformation("Build hreflang url");

                    var hrefLangUrl = BuildHrefLangURL(countryCode.ToLower(), hrefUrl);

                    azureTableOperations.InsertHrefLangUrl(countryCode, hrefLangUrl, hrefUrl, log);
                }
                files.Add(smpWriter.EndFile());

                log.LogInformation(string.Format("\n {0} : File count: {1} ", countryCode, fileIndex));
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace + ex.Message);
            }

            return fileIndex;
        }

        private static string BuildHrefLangURL(string countryCode, string href)
        {
            StringBuilder url = new StringBuilder();

            url.Append("<link rel=\"attribute\" href=\"" + href + "\" hreflang=\"en-" + countryCode + "\" />");

            return url.ToString();
        }

    }
}
