namespace DWBServices.WebServices.GeoServices
{
    /// <summary>
    /// Represents an abstract base class for geographic web services, providing functionality for interacting with 
    /// geographic data through the DWB API. This class extends the <see cref="DwbWebservice{TDwbSearchResult, TDwbSearchResultItem, TDwbEntity}"/> 
    /// and defines methods for constructing API queries and processing geographic data models.
    /// </summary>
    public abstract class GeoService(HttpClient httpClient) : DwbWebservice<GeoSearchResult, GeoSearchResultItem, GeoEntity>(httpClient)
    {
        public abstract override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage);
        public abstract override GeoSearchResultItem GetDwbApiSearchModel<T>(T tt);

        public abstract override GeoSearchResult GetDwbApiSearchResultModel<T>(T tt);

        public abstract override GeoEntity GetDwbApiDetailModel<T>(T tt);

        public abstract override GeoEntity GetEmptyDwbApiDetailModel();

    }

}
