using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DWBServices.WebServices.GeoServices.GFBioTermGeonames
{
    public class GFBioTermGeonamesWebservice : GeoService, IDwbWebservice<GeoSearchResult, GeoSearchResultItem, GeoEntity>
    {
        public GFBioTermGeonamesWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            GFBioTermGeonamesGeoSearchCriterias criterias = new GFBioTermGeonamesGeoSearchCriterias();
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criterias.query = queryRestrictions;
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
            string settingValue = configuration["GFBioTermGeonames:GeoNames_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.GFBioTermGeonames;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["GFBioTermGeonames:GeoNames_ServiceUri"];
            return settingValue;
        }

        public override GFBioTermGeonamesSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModel = JsonSerializer.Deserialize<GFBioTermGeonamesSearchResultItem>(jsonString);
            return GeonamesModel;
        }

        public override GFBioTermGeonamesSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModelResponse = JsonSerializer.Deserialize<GFBioTermGeonamesSearchResult>(jsonString);
            if (GeonamesModelResponse != null)
            {
                GeonamesModelResponse.ApiJsonResponse = jsonString;
            }

            return GeonamesModelResponse;
        }

        public override GFBioTermGeonamesEntity GetDwbApiDetailModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GeonamesModel = JsonSerializer.Deserialize<GFBioTermGeonamesEntityRootobject>(jsonString);
            if (GeonamesModel != null && GeonamesModel.results != null && GeonamesModel.results.Length > 0)
            {
                GeonamesModel.results[0].ApiJsonResponse = jsonString;
                return GeonamesModel.results[0];
            }
            return null;
        }

        public override GFBioTermGeonamesEntity GetEmptyDwbApiDetailModel()
        {
            var GeonamesModel = new GFBioTermGeonamesEntity();
            return GeonamesModel;
        }
    }
}
