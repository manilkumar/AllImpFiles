using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MIMSV3SiteMapGenerator.Urls;


namespace MIMSV3SiteMapGenerator.UrlFactories
{
    public class DrugImageUrlFactory : IUrlArrayFactory
    {
        private string _country;
        private SqliteConnection _connection;
        private IDictionary<string, string> _productAlias;
        private IDictionary<string, string> _strengthAlias;
        private IList<DrugImageUrl> _drugImageDict;
        public DrugImageUrlFactory(string countryCode, IConfiguration config)
        {
            _country = ConnectionStringManager.GetCountryNames()[countryCode];
            string connectionString = config.GetConnectionString(countryCode);
            _connection = new SqliteConnection(connectionString);


            string query = @"select distinct d.Id as DrugImageId, d.Strength as ProductStrength, p.ProductName 
            						from DRUGIMAGE d
            						left outer join PRODUCT p on d.ProductId = p.Id
            						order by d.Id";

            _connection.Open();
            _drugImageDict = (IList<DrugImageUrl>)_connection.Query<DrugImageUrl>(query);

            //Added this function to sync with the mims.com logic
            GetMappingProductOriginalNameToAlias(countryCode);
            GetMappingStrengthOriginalNameToAlias(countryCode);
        }

        private void GetMappingProductOriginalNameToAlias(string countryCode)
        {
            _productAlias = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            StringBuilder queryStr = new StringBuilder("select ProductName from Product where ");
            IList<string> conditionList = Utility.getQueryAliasMappingCondition();

            for (int j = 0; j < conditionList.Count; j++)
            {
                if (j != 0)
                {
                    queryStr.Append(" or ");
                }

                queryStr.Append(" ProductName like ");
                queryStr.Append(conditionList[j]);
            }


            var productData = _connection.Query<object>(queryStr.ToString());

            foreach (var product in productData)
            {
                var item = (IDictionary<string, object>)product;
                string name = item["ProductName"].ToString();
                string productnameAlias = Utility.MakeItURLSafe(name);
                _productAlias[name] = productnameAlias.ToLower();
            }
            queryStr = null;
            conditionList = null;
        }

        private void GetMappingStrengthOriginalNameToAlias(string countryCode)
        {
            _strengthAlias = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            StringBuilder queryStr = new StringBuilder("select Strength as ProductStrength from DrugImage where ");
            IList<string> conditionList = Utility.getQueryAliasMappingCondition();

            for (int j = 0; j < conditionList.Count; j++)
            {
                if (j != 0)
                {
                    queryStr.Append(" or ");
                }

                queryStr.Append(" Strength like ");
                queryStr.Append(conditionList[j]);
            }



            var strengthData = _connection.Query<object>(queryStr.ToString());

            foreach (var strength in strengthData)
            {
                var item = (IDictionary<string, object>)strength;
                string name = item["ProductStrength"].ToString();
                string nameAlias = Utility.MakeItURLSafe(name);
                _productAlias[name] = nameAlias.ToLower();
            }
            queryStr = null;
            conditionList = null;
        }

        public List<IUrl> GetNext()
        {

            var urlList = new List<IUrl>();
            if (_drugImageDict.Count > 0)
            {
                foreach (var item in _drugImageDict)
                {
                    string _productName = item.ProductName;
                    string _strengthName = item.ProductStrength;



                    DrugImageUrl drugImageUrl = new DrugImageUrl
                    {
                        CountryName = _country,
                        ProductName = (_productAlias.ContainsKey(_productName)) ? _productAlias[_productName] : _productName,
                        ProductStrength = (_strengthAlias.ContainsKey(_strengthName)) ? _strengthAlias[_strengthName] : _strengthName,
                        DrugImageId = item.DrugImageId,
                    };

                    urlList.Add(drugImageUrl);
                }

                return urlList;
            }

            return null;
        }
    }
}
