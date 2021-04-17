using Microsoft.Azure;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MIMSV3SiteMapGenerator
{
    public class AzureTableOperations
    {
        private static CloudTable table;
        public AzureTableOperations()
        {
            try
            {

                var account = CloudStorageAccount.Parse(AppSettings.AzureStorageConnectionString);
                var client = account.CreateCloudTableClient();
                table = client.GetTableReference("HrefLangUrls");
                Task.Run(async () => await table.DeleteIfExistsAsync());
                Task.Run(async () => await table.CreateIfNotExistsAsync());
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace + ex.Message);
            }
        }

        public void DeleteTable()
        {

        }

        public void InsertHrefLangUrl(string countryCode, string hreflangurl, string requestURL, ILogger log)
        {
            try
            {
                log.LogInformation("Inserting into DB");
                table.CreateIfNotExistsAsync();
                var id = Guid.NewGuid().ToString();
                HrefLangUrlEntity entity = new HrefLangUrlEntity(EncodeUrlInKey(requestURL), id)
                {
                    Country = countryCode,
                    HrefLink = hreflangurl,
                    RequestURL = requestURL
                };

                TableOperation insertOperation = TableOperation.Insert(entity);
                Task.Run(async () => await table.ExecuteAsync(insertOperation));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while inserting into DB" + ex.StackTrace + ex.Message);
                log.LogError(ex.StackTrace + ex.Message);
            }

        }

        private static String EncodeUrlInKey(String url)
        {
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(url);
            var base64 = System.Convert.ToBase64String(keyBytes);
            return base64.Replace('/', '_');
        }

    }
}
