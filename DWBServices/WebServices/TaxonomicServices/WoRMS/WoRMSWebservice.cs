using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DWBServices.WebServices.TaxonomicServices.WoRMS
{
    public class WoRMSWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public WoRMSWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            WoRMSTaxonomicSearchCriterias criterias = new WoRMSTaxonomicSearchCriterias();
            queryRestrictions = Uri.EscapeDataString(queryRestrictions);
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
            string settingValue = configuration["WoRMS:WoRMS_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.WoRMS;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["WoRMS:WoRMS_ServiceUri"];
            return settingValue;
        }

        public override WoRMSSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var WoRMSModel = JsonSerializer.Deserialize<WoRMSSearchResultItem>(jsonString);
                return WoRMSModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override WoRMSSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                WoRMSSearchResultItem[][] items = JsonSerializer.Deserialize<WoRMSSearchResultItem[][]>(jsonString);
                if (items != null)
                {
                    var result = new WoRMSSearchResult
                    {
                        Items = items
                    };
                    result.ApiJsonResponse = jsonString;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }

            throw new DataMappingException("The response json string could not be deserialized in WoRMSWebservice.GetDwbApiSearchResultModel()");
        }

        public override WoRMSEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var WoRMSModel = JsonSerializer.Deserialize<WoRMSEntity>(jsonString);
                if (WoRMSModel != null)
                {
                    WoRMSModel.ApiJsonResponse = jsonString;
                }

                return WoRMSModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override WoRMSEntity GetEmptyDwbApiDetailModel()
        {
            var WoRMSModel = new WoRMSEntity();
            return WoRMSModel;
        }
    }
}
