using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices.TaxonomicServices.GfbioTerminology;

public class GfbioTerminologyWebservice : TaxonomicWebservice,
    IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
{
    public GfbioTerminologyWebservice(HttpClient httpClient) : base(httpClient)
    {
        var BaseAddress = GetBaseAddress();
        httpClient.BaseAddress = new Uri(BaseAddress);
    }

    public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions,
        int offset, int maxPerPage)
    {
        var criterias = new GfbioTerminologyTaxonomicSearchCriterias();
        queryRestrictions = Uri.EscapeDataString(queryRestrictions);
        if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            criterias.query = queryRestrictions;
        else
            throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                nameof(queryRestrictions));

        return criterias.QueryParamString;
    }

    public override bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                     (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        //if (result)
        //{
        //    Uri uri = new Uri(url);
        //    string query = uri.Query;
        //    var queryParameters = HttpUtility.ParseQueryString(query);
        //    // Check if the "uri" parameter is missing or empty
        //    if (string.IsNullOrEmpty(queryParameters["GUID"]))
        //    {
        //        return false;
        //    }
        //}

        return result;
    }

    public override string GetBaseAddress()
    {
        // Access IConfiguration from the static DwbServiceProviderAccessor
        var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                            ?? throw new InvalidOperationException("IConfiguration service is not available.");
        var settingValue = configuration["GfbioTerminology:GfbioTerminology_BaseAddress"];
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
                            ?? throw new InvalidOperationException(
                                "DwbServiceProviderAccessor.Instance is not initialized.");
        var settingValue = configuration["GfbioTerminology:GfbioTerminology_ServiceUri"];
        return settingValue;
    }

    public override GfbioTerminologySearchResultItem GetDwbApiSearchModel<T>(T tt)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GfbioTerminologyModel = JsonSerializer.Deserialize<GfbioTerminologySearchResultItem>(jsonString);
            return GfbioTerminologyModel;
        }
        catch (Exception ex)
        {
            throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
        }
    }

    public override GfbioTerminologySearchResult GetDwbApiSearchResultModel<T>(T tt)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GfbioTerminologyModelResponse = JsonSerializer.Deserialize<GfbioTerminologySearchResult>(jsonString);
            if (GfbioTerminologyModelResponse != null) GfbioTerminologyModelResponse.ApiJsonResponse = jsonString;

            return GfbioTerminologyModelResponse;
        }
        catch (Exception ex)
        {
            throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
        }
    }

    public override GfbioTerminologyEntity GetDwbApiDetailModel<T>(T tt)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(tt);
            var GfbioTerminologyModel = JsonSerializer.Deserialize<GfbioTerminologyEntityRootobject>(jsonString);
            if (GfbioTerminologyModel != null && GfbioTerminologyModel.results != null &&
                GfbioTerminologyModel.results.Length > 0)
            {
                GfbioTerminologyModel.results[0].ApiJsonResponse = jsonString;
                return GfbioTerminologyModel.results[0];
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
        }
    }

    public override GfbioTerminologyEntity GetEmptyDwbApiDetailModel()
    {
        var GfbioTerminologyModel = new GfbioTerminologyEntity();
        return GfbioTerminologyModel;
    }
}