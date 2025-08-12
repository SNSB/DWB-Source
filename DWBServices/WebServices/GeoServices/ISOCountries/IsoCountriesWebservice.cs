using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DWBServices.WebServices.GeoServices.ISOCountries
{
    public class IsoCountriesWebservice : GeoService, IDwbWebservice<GeoSearchResult, GeoSearchResultItem, GeoEntity>
    {
        public IsoCountriesWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            IsoCountriesGeoSearchCriterias criterias = new IsoCountriesGeoSearchCriterias();
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
            string settingValue = configuration["IsoCountries:IsoCountries_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.CatalogueOfLife;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["IsoCountries:IsoCountries_ServiceUri"];
            return settingValue;
        }

        public override IsoCountriesSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IsoCountriesModel = JsonSerializer.Deserialize<IsoCountriesSearchResultItem>(jsonString);
            return IsoCountriesModel;
        }

        public override IsoCountriesSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IsoCountriesModelResponse = JsonSerializer.Deserialize<IsoCountriesSearchResult>(jsonString);
            if (IsoCountriesModelResponse != null)
            {
                IsoCountriesModelResponse.ApiJsonResponse = jsonString;
            }

            return IsoCountriesModelResponse;
        }

        public override IsoCountriesEntity GetDwbApiDetailModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IsoCountriesModel = JsonSerializer.Deserialize<IsoCountriesEntityRootobject>(jsonString);
            if (IsoCountriesModel != null && IsoCountriesModel.results != null && IsoCountriesModel.results.Length > 0)
            {
                IsoCountriesModel.results[0].ApiJsonResponse = jsonString;
                return IsoCountriesModel.results[0];
            }
            return null;
        }

        public override IsoCountriesEntity GetEmptyDwbApiDetailModel()
        {
            var IsoCountriesModel = new IsoCountriesEntity();
            return IsoCountriesModel;
        }
    }
}
