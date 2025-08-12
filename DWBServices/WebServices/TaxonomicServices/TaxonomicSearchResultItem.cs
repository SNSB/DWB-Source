namespace DWBServices.WebServices.TaxonomicServices
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TaxonomicSearchResult : DwbSearchResult
    {
        public override TaxonomicSearchResultItem[] DwbApiSearchResponse { get;}
        
    }
    /// <summary>
    /// Represents an abstract base class for items in the taxonomic search results of the DWB web services.
    /// </summary>
    /// <remarks>
    /// This class provides a structure for taxonomic search result items, including properties such as 
    /// taxonomic hierarchy, authorship, status, and other taxonomic details. It serves as a foundation 
    /// for specific implementations of taxonomic search result items in derived classes.
    /// </remarks>
    public abstract class TaxonomicSearchResultItem : DwbSearchResultItem
    {
        public string Taxon { get; set; }

        public string Genus { get; set; }

        public string Epithet { get; set; }

        public string Rank { get; set; }

        public string CommonNames { get; set; }

        public string Status { get; set; }

        public string Authors { get; set; }

        public string BasionymAuthors { get; set; }

        public string CombiningAuthors { get; set; }

        public string Kingdom { get; set; }

        public string Subkingdom { get; set; }

        public string Hierarchy { get; set; }

        public string Unranked { get; set; }

        public string Year { get; set; }

        /// <summary>
        /// Maps the current instance of <see cref="TaxonomicSearchResultItem"/> to a corresponding API client model.
        /// </summary>
        /// <remarks>
        /// This method is abstract and must be implemented by derived classes to provide the specific mapping logic
        /// for the respective taxonomic search result item.
        /// </remarks>
        /// <returns>
        /// A mapped instance of <see cref="TaxonomicSearchResultItem"/> or <c>null</c> if the mapping cannot be performed.
        /// </returns>
        public abstract TaxonomicSearchResultItem? GetMappedApiClientModel();
    }

}
