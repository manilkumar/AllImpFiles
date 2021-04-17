using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UBMMedica.Asia.Common.Library.Config;

namespace MIMSV3SiteMapGenerator
{
    public static class ConnectionStringManager
    {

        private static Regex regexCountryNames = new Regex(@"^(?<CountryCode>.+)\[(?<CountryName>.+)\]$");

        public static IDictionary<string, string> GetCountryNames()
        {
            IDictionary<string, string> countryNames = new Dictionary<string, string>();
                
            string[] countries = GetCountries();
            foreach (string country in countries)
            {
                Match match = regexCountryNames.Match(country);
                string countryCode = match.Groups["CountryCode"].Value;
                string countryName = match.Groups["CountryName"].Value;
                countryNames.Add(countryCode, countryName);
            }
            return countryNames;
        }

        public static IList<string> GetCountryCodes()
        {
            IList<string> countryCodes = new List<string>();

            string[] countries = GetCountries();
            foreach (string country in countries)
            {
                Match match = regexCountryNames.Match(country);
                string countryCode = match.Groups["CountryCode"].Value;
                countryCodes.Add(countryCode);
            }
            return countryCodes;
        }

        private static string[] GetCountries()
        {
            string dbAvailability = AppSettings.COUNTRY_DB_AVAILABILITY;
            return dbAvailability.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
