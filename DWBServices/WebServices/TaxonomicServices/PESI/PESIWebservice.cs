using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Web;

namespace DWBServices.WebServices.TaxonomicServices.PESI
{
    public class PESIWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public PESIWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            PESITaxonomicSearchCriterias criterias = new PESITaxonomicSearchCriterias();
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
        public override bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                Uri uri = new Uri(url);
                string query = uri.Query;
                var queryParameters = HttpUtility.ParseQueryString(query);
                // Check if the "uri" parameter is missing or empty
                if (string.IsNullOrEmpty(queryParameters["uri"]))
                {
                    return false;
                }
            }

            return result;
        }
        
        public override string GetBaseAddress()
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("IConfiguration service is not available.");
            string settingValue = configuration["PESI:PESI_BaseAddress"];
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
            string settingValue = configuration["PESI:PESI_ServiceUri"];
            return settingValue;
        }

        public override PESISearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var pesiModel = JsonSerializer.Deserialize<PESISearchResultItem>(jsonString);
                return pesiModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override PESISearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var pesiModelResponse = JsonSerializer.Deserialize<PESISearchResult>(jsonString);
                if (pesiModelResponse != null)
                {
                    pesiModelResponse.ApiJsonResponse = jsonString;
                }

                return pesiModelResponse;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override PESIEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var pesiModel = JsonSerializer.Deserialize<PESIEntityRootobject>(jsonString);
                if (pesiModel != null && pesiModel.results != null && pesiModel.results.Length > 0)
                {
                    pesiModel.results[0].ApiJsonResponse = jsonString;
                    return pesiModel.results[0];
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override PESIEntity GetEmptyDwbApiDetailModel()
        {
            var pesiModel = new PESIEntity();
            return pesiModel;
        }
    }
}
