using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices.TaxonomicServices.CatalogueOfLife
{
    public class CoLWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public CoLWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            // get datasetkey from current service
            var serviceInfoDictionary = DwbServiceEnums.TaxonomicServiceInfoDictionary();

            if (serviceInfoDictionary.TryGetValue(currentService, out var serviceInfo)) {
                CoLTaxonomicSearchCriterias criterias = new CoLTaxonomicSearchCriterias();
                queryRestrictions = Uri.EscapeDataString(queryRestrictions);
                if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
                {
                    criterias.datasetKey = serviceInfo.DatasetKey;
                    criterias.query = queryRestrictions;
                    criterias.offset = offset.ToString();
                    criterias.maxPerPage = maxPerPage.ToString();
                }
                else
                {
                    throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                        nameof(queryRestrictions));
                }
                return criterias.QueryParamString;
            } else
            {
                throw new ArgumentException($"The service {currentService} has no valid datasetKey");
            }
        }
        public override string GetBaseAddress()
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("IConfiguration service is not available.");
            string settingValue = configuration["CoL:CoL_BaseAddress"];
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
            if (currentService == DwbServiceEnums.DwbService.PoWo_WCVP)
            {
                return configuration["CoL:PoWo_ServiceUri"];
            }      
            string settingValue = configuration["CoL:CoL_ServiceUri"];
            return settingValue;
        }

        public override CoLSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var colModel = JsonSerializer.Deserialize<CoLSearchResultItem>(jsonString);

                var test = colModel?.GetMappedApiClientModel();
                return colModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override CoLSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var colModelResponse = JsonSerializer.Deserialize<CoLSearchResult>(jsonString);
                if (colModelResponse != null)
                {
                    colModelResponse.ApiJsonResponse = jsonString;
                }

                return colModelResponse;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override CoLEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var colModel = JsonSerializer.Deserialize<CoLEntity>(jsonString);
                if (colModel != null)
                {
                    colModel.ApiJsonResponse = jsonString;
                }

                return colModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override CoLEntity GetEmptyDwbApiDetailModel()
        {
            var colModel = new CoLEntity();
            return colModel;
        }
    }
}
