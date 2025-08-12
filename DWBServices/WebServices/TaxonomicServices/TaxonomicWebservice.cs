namespace DWBServices.WebServices.TaxonomicServices
{
    /// <summary>
    /// Represents an abstract base class for taxonomic web services, providing functionality 
    /// to interact with taxonomic data through the DWB API. This class extends the 
    /// <see cref="DwbWebservice{TDwbSearchResult, TDwbSearchResultItem, TDwbEntity}"/> class 
    /// with specific implementations for taxonomic search results, search result items, 
    /// and taxonomic entities.
    /// </summary>
    /// <remarks>
    /// This class defines abstract methods that must be implemented by derived classes 
    /// to handle specific taxonomic web service operations, such as constructing API query URLs 
    /// and mapping API responses to domain models.
    /// </remarks>
    public abstract class TaxonomicWebservice(HttpClient httpClient) : DwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>(httpClient)
    {
        public abstract override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage);
        public abstract override TaxonomicSearchResultItem GetDwbApiSearchModel<T>(T tt);

        public abstract override TaxonomicSearchResult GetDwbApiSearchResultModel<T>(T tt);

        public abstract override TaxonomicEntity GetDwbApiDetailModel<T>(T tt);

        public abstract override TaxonomicEntity GetEmptyDwbApiDetailModel();

    }


}
