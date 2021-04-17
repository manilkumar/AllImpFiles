using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MIMSV3SiteMapGenerator
{
    public class AppSettings
    {

        public static int SitemapFileNodeLimit = 0;
        public static string COUNTRY_DB_AVAILABILITY = "";
        public static string DISEASE_PORTAL_COUNTRIES = "";
        public static string OutputDirectory = "";
        public static string UrlBase = "";
        public static bool GenerateDiseasePortal = false;
        public static bool ForCacheGeneration = false;
        public static bool GenerateCompany = false;
        public static string FlatFileName = "";
        public static string PatientMedInfoCountries = "";
        public static string FullMonographSectionCountries = "";
        public static string AzureStorageConnectionString = "";
        public static string AzureFTPUserName = "";
        public static string AzureFTPPassword = ""; 
        public static string AzureFTPBaseURL = ""; 

        public static void LoadAppSettings(IConfiguration _config)
        {
            SitemapFileNodeLimit = Convert.ToInt32(_config["SitemapFileNodeLimit"]);
            COUNTRY_DB_AVAILABILITY = Convert.ToString(_config["COUNTRY_DB_AVAILABILITY"]);
            DISEASE_PORTAL_COUNTRIES = Convert.ToString(_config["DISEASE_PORTAL_COUNTRIES"]);
            OutputDirectory = Convert.ToString(_config["OutputDirectory"]);
            UrlBase = Convert.ToString(_config["UrlBase"]);
            GenerateDiseasePortal = Convert.ToBoolean(_config["GenerateDiseasePortal"]);
            ForCacheGeneration = Convert.ToBoolean(_config["ForCacheGeneration"]);
            GenerateCompany = Convert.ToBoolean(_config["GenerateCompany"]);
            FlatFileName = Convert.ToString(_config["FlatFileName"]);
            PatientMedInfoCountries = Convert.ToString(_config["PatientMedInfoCountries"]);
            FullMonographSectionCountries = Convert.ToString(_config["FullMonographSectionCountries"]);
            AzureStorageConnectionString = Convert.ToString(_config["AzureStorageConnectionString"]);
            AzureFTPUserName = Convert.ToString(_config["AzureFTPUserName"]);
            AzureFTPPassword = Convert.ToString(_config["AzureFTPPassword"]);
            AzureFTPBaseURL = Convert.ToString(_config["AzureFTPBaseURL"]);
        }
    }
}
