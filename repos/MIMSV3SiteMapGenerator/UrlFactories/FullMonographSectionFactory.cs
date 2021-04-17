using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MIMSV3SiteMapGenerator.Urls;

namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public class FullMonographSectionFactory : IUrlArrayFactory
    {
        private string _country;
        private SqliteConnection _connection;
        private IDictionary<string, string> _monographAlias;
        private IList<object> _fullMonographDict;
        private IDictionary<string, string> _brandAlias;

        public FullMonographSectionFactory(string countryCode, IConfiguration config)
        {
            _country = ConnectionStringManager.GetCountryNames()[countryCode];
            string connectionString = config.GetConnectionString(countryCode);
            _connection = new SqliteConnection(connectionString);

            string selectOne = string.Empty;

            if (AppSettings.ForCacheGeneration)
            {
                selectOne = "LIMIT 1";
            }

            string query = @"select distinct m.Id, 
                                    m.MonographName, 
                                    b.BrandName as Name,
                                    m.Dosage,
                                    m.ContraIndication, 
                                    m.SpecialPrecaution, 
                                    m.AdverseReaction,
                                    m.Interaction,
                                    m.Description,
                                    m.Action,
                                    m.Indication,
                                    m.OverDosage,
                                    m.Warning,
                                    m.Precaution,
                                    m.Storage,
                                    m.SideEffect,
                                    m.Pharmacodynamics,
                                    m.UseInChildren,
                                    m.UseInElderly,
                                    m.UseInPregnancyLactation,
                                    m.Pharmacokinetics,
                                    m.PackingAndPrice,
                                    m.PatientCounsellingInformation
									from MONOGRAPH m
									join REL_PRODUCT_MONOGRAPH pm on m.Id = pm.MonographId
									join PRODUCT p on pm.ProductId = p.Id
									join BRAND b on p.BrandId = b.Id
									join CODETABLE c on m.MonographType = c.Id
                                    where m.MonographType = 6001						
									order by m.Id  {0}";
            query = string.Format(query, selectOne);

            _connection.Open();
            _fullMonographDict = (IList<object>)_connection.Query<object>(query);


            //Added this function to sync with the mims.com logic
            GetMappingMonographOriginalNameToAlias(countryCode);
            GetMappingBrandOriginalNameToAlias(countryCode);
        }

        private void GetMappingBrandOriginalNameToAlias(string countryCode)
        {
            _brandAlias = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            StringBuilder queryStr = new StringBuilder("select BrandName from Brand where ");
            IList<string> conditionList = Utility.getQueryAliasMappingCondition();

            for (int j = 0; j < conditionList.Count; j++)
            {
                if (j != 0)
                {
                    queryStr.Append(" or ");
                }

                queryStr.Append(" BrandName like ");
                queryStr.Append(conditionList[j]);
            }

            var brandData = _connection.Query<object>(queryStr.ToString());

            foreach (var brand in brandData)
            {
                var item = (IDictionary<string, object>)brand;
                string brandname = item["BrandName"].ToString();
                string brandnameAlias = Utility.MakeItURLSafe(brandname);
                _brandAlias[brandname] = brandnameAlias.ToLower();
            }

            queryStr = null;
            conditionList = null;

        }

        private void GetMappingMonographOriginalNameToAlias(string countryCode)
        {
            _monographAlias = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            StringBuilder queryStr = new StringBuilder("select m.MonographName, ct.Value from Monograph m inner join CodeTable ct on m.MonographType=ct.ID where ");
            IList<string> conditionList = Utility.getQueryAliasMappingCondition();

            for (int j = 0; j < conditionList.Count; j++)
            {
                if (j != 0)
                {
                    queryStr.Append(" or ");
                }

                queryStr.Append(" m.MonographName like ");
                queryStr.Append(conditionList[j]);
            }

            var monographData = _connection.Query<object>(queryStr.ToString());

            foreach (var brand in monographData)
            {
                var item = (IDictionary<string, object>)brand;
                string brandname = item["MonographName"].ToString();
                string brandnameAlias = Utility.MakeItURLSafe(brandname);
                _monographAlias[brandname] = brandnameAlias.ToLower();
            }

            queryStr = null;
            conditionList = null;
        }

        public List<IUrl> GetNext()
        {
            var urlList = new List<IUrl>();
            if (_fullMonographDict.Count > 0)
            {
                foreach (var data  in _fullMonographDict)
                {
                    var item = (IDictionary<string, object>)data;

                    string brandName = item["Name"].ToString();
                    var dosage = item["Dosage"] == null ? "" : item["Dosage"].ToString();
                    var contraIndication = item["ContraIndication"] == null ? "" : item["ContraIndication"].ToString();
                    var specialPrecaution = item["SpecialPrecaution"] == null ? "" : item["SpecialPrecaution"].ToString();
                    var adverseReaction = item["AdverseReaction"] == null ? "" : item["AdverseReaction"].ToString();
                    var interaction = item["Interaction"] == null ? "" : item["Interaction"].ToString();
                    var description = item["Description"] == null ? "" : item["Description"].ToString();
                    var action = item["Action"] == null ? "" : item["Action"].ToString();
                    var indication = item["Indication"] == null ? "" : item["Indication"].ToString();
                    var overDosage = item["OverDosage"] == null ? "" : item["OverDosage"].ToString();
                    var warning = item["Warning"] == null ? "" : item["Warning"].ToString();
                    var precaution = item["Precaution"] == null ? "" : item["Precaution"].ToString();
                    var storage = item["Storage"] == null ? "" : item["Storage"].ToString();
                    var sideEffect = item["SideEffect"] == null ? "" : item["SideEffect"].ToString();
                    var pharmacodynamics = item["Pharmacodynamics"] == null ? "" : item["Pharmacodynamics"].ToString();
                    var useInChildren = item["UseInChildren"] == null ? "" : item["UseInChildren"].ToString();
                    var useInElderly = item["UseInElderly"] == null ? "" : item["UseInElderly"].ToString();
                    var useInPregnancyLactation = item["UseInPregnancyLactation"] == null ? "" : item["UseInPregnancyLactation"].ToString();
                    var pharmacokinetics = item["Pharmacokinetics"] == null ? "" : item["Pharmacokinetics"].ToString();
                    var packingAndPrice = item["PackingAndPrice"] == null ? "" : item["PackingAndPrice"].ToString();
                    var patientCounsellingInformation = item["PatientCounsellingInformation"] == null ? "" : item["PatientCounsellingInformation"].ToString();

                    string _monographName = item["MonographName"].ToString();

                    if (_monographAlias.ContainsKey(_monographName))
                    {
                        _monographName = _monographAlias[_monographName];
                    }

                    if (_brandAlias.ContainsKey(brandName))
                    {
                        brandName = _brandAlias[brandName];
                    }

                    if (!string.IsNullOrEmpty(brandName))
                    {
                        if (!string.IsNullOrEmpty(indication))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "indications"
                            };

                            urlList.Add(fullMonoSectionUrl);
                        }

                        if (!string.IsNullOrEmpty(dosage) || !string.IsNullOrEmpty(overDosage))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "dosage"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }

                        if (!string.IsNullOrEmpty(contraIndication)
                            || !string.IsNullOrEmpty(warning)
                            || !string.IsNullOrEmpty(specialPrecaution)
                            || !string.IsNullOrEmpty(precaution)
                            || !string.IsNullOrEmpty(useInPregnancyLactation)
                            || !string.IsNullOrEmpty(useInChildren)
                            || !string.IsNullOrEmpty(useInElderly))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "special-precautions"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }

                        if (!string.IsNullOrEmpty(sideEffect) || !string.IsNullOrEmpty(adverseReaction))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "side-effects"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }

                        if (!string.IsNullOrEmpty(interaction))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "drug-interactions"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }

                        if (!string.IsNullOrEmpty(action) || !string.IsNullOrEmpty(pharmacodynamics) || !string.IsNullOrEmpty(pharmacokinetics))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "mechanism-of-action"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }


                        if (!string.IsNullOrEmpty(storage) || !string.IsNullOrEmpty(packingAndPrice))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "presentation-and-storage"
                            };
                            urlList.Add(fullMonoSectionUrl);
                        }


                        if (!string.IsNullOrEmpty(patientCounsellingInformation))
                        {
                            FullMonographSectionURL fullMonoSectionUrl = new FullMonographSectionURL
                            {
                                CountryName = _country,
                                BrandName = brandName,
                                MonographName = _monographName,
                                SectionName = "patient-counselling"
                            };

                            urlList.Add(fullMonoSectionUrl);
                        }

                        return urlList;
                    }
                }
            }

            _connection.Close();
            return null;
        }
    }
}
