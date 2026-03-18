using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DWBServices.WebServices.GeoServices.Geonames
{
    public class GeonamesWebservice : GeoService, IDwbWebservice<GeoSearchResult, GeoSearchResultItem, GeoEntity>
    {
        public GeonamesWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        private string HelpMethod(string text, string help)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(help.PadRight(8).Substring(0, 8));
                byte[] cipherBytes = Convert.FromBase64String(text);
                using (var des = DES.Create())
                {
                    des.Key = keyBytes;
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    using (var decryptor = des.CreateDecryptor())
                    using (var ms = new MemoryStream(cipherBytes))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GeonamesWebservice: HelpMethod failed: {ex}");
                return "";
            }
        }
        
        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            GeonamesGeoSearchCriterias criterias = new GeonamesGeoSearchCriterias();
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criterias.username = HelpMethod(configuration["Geonames:id"], configuration["Geonames:version"]);
            }
            else
            {
                throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                    nameof(queryRestrictions));
            }

            return criterias.QueryParamString;
        }

        // special call to altitude endpoint
        public string DwbApiGeoNamesAltitudeSrtm3EndpointUrl(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            GeoNamesAltitudeSrtm3EndpointCriterias criterias = new GeoNamesAltitudeSrtm3EndpointCriterias();
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criterias.username = HelpMethod(configuration["Geonames:id"], configuration["Geonames:version"]);
            }
            else
            {
                throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                    nameof(queryRestrictions));
            }

            return criterias.QueryParamString;
        }

        public string DwbApiGeoNamesAltitudeGTopo30EndpointUrl(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            GeoNamesAltitudeGTopo30EndpointCriterias criterias = new GeoNamesAltitudeGTopo30EndpointCriterias();
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criterias.username = HelpMethod(configuration["Geonames:id"], configuration["Geonames:version"]);
            }
            else
            {
                throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                    nameof(queryRestrictions));
            }

            return criterias.QueryParamString;
        }
        public override string GetBaseAddress()
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("IConfiguration service is not available.");
            string settingValue = configuration["GeoNames:GeoNames_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.Geonames;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["GeoNames:GeoNames_ServiceUri"];
            return settingValue;
        }

        public override GeonamesSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModel = JsonSerializer.Deserialize<GeonamesSearchResultItem>(jsonString);
            return GeonamesModel;
        }

        public override GeonamesSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModelResponse = JsonSerializer.Deserialize<GeonamesSearchResult>(jsonString);
            if (GeonamesModelResponse != null)
            {
                GeonamesModelResponse.ApiJsonResponse = jsonString;
            }

            return GeonamesModelResponse;
        }

        //special method for altitude endpoint srmt3
        public GeonamesAltitudeSrmt3SearchResultItem GetAltitudeSrmt3SearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            Console.WriteLine($"Serialized JSON: {jsonString}");
            var GeonamesSrmt3ModelResponse = JsonSerializer.Deserialize<GeonamesAltitudeSrmt3SearchResultItem>(jsonString);
            return GeonamesSrmt3ModelResponse;
        }

        //special method for altitude endpoint gtopo30
        public GeonamesAltitudeGTopo30SearchResultItem GetAltitudeGTopo30SearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            Console.WriteLine($"Serialized JSON: {jsonString}");
            var GeonamesGTopo30ModelResponse = JsonSerializer.Deserialize<GeonamesAltitudeGTopo30SearchResultItem>(jsonString);
            return GeonamesGTopo30ModelResponse;
        }

        public override GeonamesEntity GetDwbApiDetailModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModel = JsonSerializer.Deserialize<GeonamesEntityRootobject>(jsonString);
            if (GeonamesModel != null && GeonamesModel.geonames != null && GeonamesModel.geonames.Length > 0)
            {
                GeonamesModel.geonames[0].ApiJsonResponse = jsonString;
                return GeonamesModel.geonames[0];
            }
            return null;
        }

        public override GeonamesEntity GetEmptyDwbApiDetailModel()
        {
            var GeonamesModel = new GeonamesEntity();
            return GeonamesModel;
        }
    }
}
