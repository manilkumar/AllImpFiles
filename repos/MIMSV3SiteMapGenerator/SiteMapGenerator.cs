using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MIMSV3SiteMapGenerator.UrlFactories;

namespace MIMSV3SiteMapGenerator
{
    public static class SiteMapGenerator    
    {
        [FunctionName("SiteMapGenerator")]
        public static void Run([TimerTrigger("0 */60 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                             .SetBasePath(context.FunctionAppDirectory)
                             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                             .AddEnvironmentVariables()
                             .Build();
            AppSettings.LoadAppSettings(config);

            string directory = AppSettings.OutputDirectory;
            if (Directory.Exists(directory))
            {
                while (Directory.Exists(directory))
                {
                    try
                    {
                        Directory.Delete(directory, true);
                    }
                    catch
                    {
                        if (Directory.Exists(directory))
                            Directory.Delete(directory, true);
                    }
                }
            }
            Directory.CreateDirectory(directory);

            SiteMapProtocol smpWriter = new SiteMapProtocol(directory, AppSettings.UrlBase);
            SiteMapWrapper siteMapWrapper = new SiteMapWrapper();
            siteMapWrapper.CreateSiteMapIndex(smpWriter, siteMapWrapper.CreateCountrySiteMaps(smpWriter, config, log), log);
            FTPService.MoveToWebSiteFolder(directory, log);
        }
    }
}
