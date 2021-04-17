using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MIMSV3SiteMapGenerator.Urls;


namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public class MonographUrlFactory : IUrlArrayFactory
    {
        private string _country;
        private SqliteConnection _connection;
        private IDictionary<string, string> _monographAlias;
        private IDictionary<string, string> _brandAlias;
        private IList<Monograph> _brandMonographDict;

        public MonographUrlFactory(string countryCode, IConfiguration config)
        {
            _country = ConnectionStringManager.GetCountryNames()[countryCode];
            string connectionString = config.GetConnectionString(countryCode);
            _connection = new SqliteConnection(connectionString);
            string selectOne = string.Empty;

            if (AppSettings.ForCacheGeneration)
            {
                selectOne = "LIMIT 1";
            }

            string query = @"select * from (select distinct  m.Id, m.MonographName, b.BrandName as Name, c.Value as UrlType 
									from MONOGRAPH m
									join REL_PRODUCT_MONOGRAPH pm on m.Id = pm.MonographId
									join PRODUCT p on pm.ProductId = p.Id
									join BRAND b on p.BrandId = b.Id
									join CODETABLE c on m.MonographType = c.Id  {0})
                                    union									

                              select * from (select distinct  m.Id, m.MonographName, g.GenericName as Name, c.Value as UrlType 
																		from MONOGRAPH m
																		join rel_Generic_Monograph gm on m.Id = gm.MonographId
																		join Generic g on gm.GenericId = g.Id
																		join CODETABLE c on m.MonographType = c.Id {0})
																		
                                    Order by id";
            query = string.Format(query, selectOne);

            _connection.Open();
            _brandMonographDict = (IList<Monograph>)_connection.Query<Monograph>(query);

            //Added this function to sync with the mims.com logic
            GetMappingMonographOriginalNameToAlias();
            GetMappingBrandOriginalNameToAlias();
        }

        private void GetMappingBrandOriginalNameToAlias()
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

        private void GetMappingMonographOriginalNameToAlias()
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
            if (_brandMonographDict.Count > 0)
            {
                foreach (var item in _brandMonographDict)
                {
                    string brandName = item.Name;
                    string urlType = item.UrlType;
                    string monographName = item.MonographName;

                    if (_brandAlias.ContainsKey(brandName))
                        brandName = _brandAlias[brandName];

                    if (_monographAlias.ContainsKey(monographName))
                        monographName = _monographAlias[monographName];

                    if (!string.IsNullOrEmpty(brandName))
                    {
                        MonographUrl monographUrl = new MonographUrl
                        {
                            CountryName = _country,
                            BrandName = brandName,
                            MonographName = monographName,
                            UrlType = GetUrlType(urlType),
                            IsGeneric = CheckGeneric(urlType)
                        };
                        urlList.Add(monographUrl);
                    }
                }

                return urlList;
            }
            return null;
        }

        private MonoGraphUrlType GetUrlType(string typeString)
        {
            switch (typeString)
            {
                case "BRANDEDFULL":
                    return MonoGraphUrlType.FULL;
                case "BRANDEDBRIEF":
                    return MonoGraphUrlType.BRIEF;
                case "BRANDEDBASIC":
                    return MonoGraphUrlType.BASIC;
                case "GENERICFULL":
                    return MonoGraphUrlType.FULL;
                case "GENERICBRIEF":
                    return MonoGraphUrlType.BRIEF;
                case "BRANDEDVIDAL":
                    return MonoGraphUrlType.VIDAL;
                default:
                    return MonoGraphUrlType.FULL;
            }
        }

        private bool CheckGeneric(string typeString)
        {
            if (typeString.Contains("GENERIC"))
            {
                return true;
            }
            return false;
        }

        #region Old Code
        //        command.CommandText = @"select distinct m.Id, m.MonographName, b.BrandName, c.Value as UrlType 
        //									from MONOGRAPH m
        //									left outer join REL_PRODUCT_MONOGRAPH pm on m.Id = pm.MonographId
        //									left outer join PRODUCT p on pm.ProductId = p.Id
        //									left outer join BRAND b on p.BrandId = b.Id
        //									left outer join CODETABLE c on m.MonographType = c.Id
        //									order by m.Id";
        #endregion
    }
}
