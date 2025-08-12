using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DWBServices.WebServices.TaxonomicServices.GbifSpecies
{
    public class GbifSpeciesWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public GbifSpeciesWebservice(HttpClient httpClient) : base(httpClient)
        {
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxResults)
        {
            GbifSpeciesTaxonomicSearchCriterias criterias = new GbifSpeciesTaxonomicSearchCriterias();
            queryRestrictions = Uri.EscapeDataString(queryRestrictions);
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxResults))
            {
                criterias.query = queryRestrictions;
                criterias.limit = maxResults.ToString();
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
            string settingValue = configuration["GbifSpecies:GbifSpecies_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.GBIFSpecies;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["GbifSpecies:GbifSpecies_ServiceUri"];
            return settingValue;
        }

        public override GbifSpeciesSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var GbifSpeciesModel = JsonSerializer.Deserialize<GbifSpeciesSearchResultItem>(jsonString);
                return GbifSpeciesModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override GbifSpeciesSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var GbifSpeciesModelResponse = JsonSerializer.Deserialize<GbifSpeciesSearchResult>(jsonString);
                if (GbifSpeciesModelResponse != null)
                {
                    GbifSpeciesModelResponse.ApiJsonResponse = jsonString;
                }

                return GbifSpeciesModelResponse;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override GbifSpeciesEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var gbifSpeciesModel = JsonSerializer.Deserialize<GbifSpeciesEntity>(jsonString);
                if (gbifSpeciesModel != null)
                {
                    gbifSpeciesModel.ApiJsonResponse = jsonString;
                }

                return gbifSpeciesModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override GbifSpeciesEntity GetEmptyDwbApiDetailModel()
        {
            var GbifSpeciesModel = new GbifSpeciesEntity();
            return GbifSpeciesModel;
        }
    }
}
