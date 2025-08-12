namespace DWBServices.WebServices;

/// <summary>
/// Represents a generic interface for DWB web services, providing methods for interacting with 
/// various web service endpoints and handling data models specific to the service.
/// </summary>
/// <typeparam name="TDwbSearchResult">
/// The type representing the search result model returned by the web service.
/// </typeparam>
/// <typeparam name="TDwbSearchResultItem">
/// The type representing individual items within the search result model.
/// </typeparam>
/// <typeparam name="TDwbEntity">
/// The type representing the detailed entity model returned by the web service.
/// </typeparam>
public interface IDwbWebservice<out TDwbSearchResult, out TDwbSearchResultItem, out TDwbEntity>
    where TDwbSearchResult : class
    where TDwbSearchResultItem : class
    where TDwbEntity : class
{
    bool IsValidUrl(string url);

    string GetBaseAddress();

    string GetServiceUri(DwbServiceEnums.DwbService currentService);

    DwbServiceEnums.DwbService GetServiceName();
    
    Task<T> CallWebServiceAsync<T>(string url,
        string content,
        DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET);

    Task<T> CallWebServiceAsync<T>(
        string url, 
        DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET,
        HttpContent? content = null);

    string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage);
    TDwbSearchResultItem GetDwbApiSearchModel<T>(T tt);

    TDwbSearchResult GetDwbApiSearchResultModel<T>(T tt);
    
    TDwbEntity GetDwbApiDetailModel<T>(T tt);

    TDwbEntity GetEmptyDwbApiDetailModel();
}
