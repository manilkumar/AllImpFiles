using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MIMSV3SiteMapGenerator.Urls;

namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public class CompanyUrlFactory : IUrlArrayFactory
    {

        private string _country;
        private SqliteConnection _connection;
        private IDictionary<string, string> _companyAlias;
        private IList<CompanyUrl> _companyDict;

        public CompanyUrlFactory(string countryCode, IConfiguration config)
        {
            _country = ConnectionStringManager.GetCountryNames()[countryCode];
            string connectionString = config.GetConnectionString(countryCode);
            _connection = new SqliteConnection(connectionString);

            string query = @"select distinct Name as CompanyName from Company where Name is not null and LocalizationReference is null";

            _connection.Open();
            _companyDict = (IList<CompanyUrl>)_connection.Query<CompanyUrl>(query);
            //Added this function to sync with the mims.com logic
            GetMappingCompanyOriginalNameToAlias(countryCode);
        }

        private void GetMappingCompanyOriginalNameToAlias(string countryCode)
        {
            _companyAlias = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            StringBuilder queryStr = new StringBuilder("select Name as CompanyName from Company where ");
            IList<string> conditionList = Utility.getQueryAliasMappingCondition();

            for (int j = 0; j < conditionList.Count; j++)
            {
                if (j != 0)
                {
                    queryStr.Append(" or ");
                }

                queryStr.Append(" Name like ");
                queryStr.Append(conditionList[j]);
            }


            var companyData = _connection.Query<object>(queryStr.ToString());

            foreach (var company in companyData)
            {
                var item = (IDictionary<string, object>)company;
                string companyname = item["CompanyName"].ToString();
                string companynameAlias = Utility.MakeItURLSafe(companyname);
                _companyAlias[companyname] = companynameAlias.ToLower();
            }

            queryStr = null;
            conditionList = null;
        }


        public List<IUrl> GetNext()
        {
            var urlList = new List<IUrl>();
            if (_companyDict.Count > 0)
            {
                foreach (var item in _companyDict)
                {
                    string companyName = item.CompanyName;

                    if (_companyAlias.ContainsKey(companyName))
                    {
                        companyName = _companyAlias[companyName];
                    }

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        CompanyUrl monographUrl = new CompanyUrl
                        {
                            CountryName = _country,
                            CompanyName = companyName
                        };
                        urlList.Add(monographUrl);
                    }
                }

                return urlList;
            }
            return null;
        }
    }
}
