//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MIMSV3SiteMapGenerator.UrlFactories
//{
//    public class DiseasePortalUrlFactory : IUrlFactory
//    {
//        private string _country;
//        private SqlConnection _connection;
//        private SqlDataReader _reader;

//        public DiseasePortalUrlFactory(string countryCode)
//        {
//            _country = ConnectionStringManager.CountryNames[countryCode];
//            _connection = new SqlConnection(ConnectionStringManager.GetConnectionString(countryCode));

//            SqlCommand command = _connection.CreateCommand();
//            command.CommandText = @"
//select distinct Replace(s.Name, '/', '--') Specialty, 
//t.Name TreatmentGuideline,
//t.Id 
//from Specialty s
//	inner join rel_TreatmentGuideline_Specialty ts
//		on ts.SpecialtyId = s.Id
//	inner join TreatmentGuideline t
//		on ts.TreatmentGuidelineId = t.Id
//where s.LocalizationReference is null
//";

//            command.CommandTimeout = 300; //300 seconds

//            if (_connection.State == System.Data.ConnectionState.Closed)
//                _connection.Open();

//            _reader = command.ExecuteReader();
//        }



//        public IUrl GetNext()
//        {
//            while (_reader.Read())
//            {
//                string specialty = _reader["Specialty"].ToString();
//                string treatmentGuideline = _reader["TreatmentGuideline"].ToString();
//                string id = _reader["Id"].ToString();

//                if (!string.IsNullOrEmpty(id))
//                {
//                    DiseasePortalUrl diseasePortalUrl = new DiseasePortalUrl
//                    {
//                        CountryName = _country,
//                        Specialty = specialty,
//                        TreatmentGuideline = treatmentGuideline
//                    };
//                    return diseasePortalUrl;
//                }
//            }
//            _connection.Close();
//            return null;
//        }
//    }
//}
