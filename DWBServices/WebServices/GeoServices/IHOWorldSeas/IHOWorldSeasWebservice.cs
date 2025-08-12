using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DWBServices.WebServices.GeoServices.IHOWorldSeas
{
    public class IHOWorldSeasWebservice : GeoService, IDwbWebservice<GeoSearchResult, GeoSearchResultItem, GeoEntity>
    {
        public IHOWorldSeasWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            IHOWorldSeasGeoSearchCriterias criterias = new IHOWorldSeasGeoSearchCriterias();
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
            string settingValue = configuration["IHOWorldSeas:IHOWorldSeas_BaseAddress"];
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
            string settingValue = configuration["IHOWorldSeas:IHOWorldSeas_ServiceUri"];
            return settingValue;
        }

        public override IHOWorldSeasSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IHOWorldSeasModel = JsonSerializer.Deserialize<IHOWorldSeasSearchResultItem>(jsonString);
            return IHOWorldSeasModel;
        }

        public override IHOWorldSeasSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IHOWorldSeasModelResponse = JsonSerializer.Deserialize<IHOWorldSeasSearchResult>(jsonString);
            if (IHOWorldSeasModelResponse != null)
            {
                IHOWorldSeasModelResponse.ApiJsonResponse = jsonString;
            }

            return IHOWorldSeasModelResponse;
        }

        public override IHOWorldSeasEntity GetDwbApiDetailModel<T>(T tt)
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var IHOWorldSeasModel = JsonSerializer.Deserialize<IHOWorldSeasEntityRootobject>(jsonString);
            if (IHOWorldSeasModel != null && IHOWorldSeasModel.results != null && IHOWorldSeasModel.results.Length > 0)
            {
                IHOWorldSeasModel.results[0].ApiJsonResponse = jsonString;
                return IHOWorldSeasModel.results[0];
            }
            return null;
        }

        public override IHOWorldSeasEntity GetEmptyDwbApiDetailModel()
        {
            var IHOWorldSeasModel = new IHOWorldSeasEntity();
            return IHOWorldSeasModel;
        }
    }
}
