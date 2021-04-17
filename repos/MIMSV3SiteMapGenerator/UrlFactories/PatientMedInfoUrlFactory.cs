using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MIMSV3SiteMapGenerator.Urls;

namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public class PatientMedInfoUrlFactory : IUrlArrayFactory
    {
        private string _country;
        private SqliteConnection _connection;
        private IList<PatientMedInfoUrl> _patientMedInfoDict;

        public PatientMedInfoUrlFactory(string countryCode, IConfiguration config)
        {
            _country = ConnectionStringManager.GetCountryNames()[countryCode];
            string connectionString = config.GetConnectionString(countryCode);
            _connection = new SqliteConnection(connectionString);

            string query = @"select  distinct pmi.Description from PatientMedInfo pmi inner join Generic g
		                                    on pmi.GenericId = g.Id
                                    where pmi.LocalizationReference is null";

            _connection.Open();
            _patientMedInfoDict = (IList<PatientMedInfoUrl>)_connection.Query<PatientMedInfoUrl>(query);
        }

        public List<IUrl> GetNext()
        {

            var urlList = new List<IUrl>();
            if (_patientMedInfoDict.Count > 0)
            {
                foreach (var item in _patientMedInfoDict)
                {
                    if (!string.IsNullOrEmpty(item.Description))
                    {
                        PatientMedInfoUrl patientMedInfoUrl = new PatientMedInfoUrl
                        {
                            CountryName = _country,
                            Description = item.Description
                        };
                        urlList.Add(patientMedInfoUrl);
                    }
                }

                return urlList;
            }
            return null;
        }
    }
}
