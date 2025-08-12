namespace DWBServices.WebServices.GeoServices
{
    /// <summary>
    /// Represents the base class for geographic search results returned by DWB web services.
    /// </summary>
    /// <remarks>
    /// This abstract class provides a structure for handling geographic search results, 
    /// including API responses and search result items specific to geographic domains. 
    /// It extends the <see cref="DwbSearchResult"/> class to include functionality 
    /// tailored to geographic data processing.
    /// </remarks>
    public abstract class GeoSearchResult : DwbSearchResult
    {
        public override GeoSearchResultItem[] DwbApiSearchResponse { get; }

    }
    /// <summary>
    /// Represents an abstract base class for geographic search result items in the DWB web services.
    /// </summary>
    /// <remarks>
    /// This class provides a foundation for specific types of geographic search result items, 
    /// such as those used in geonames, ISO countries, and other geographic services. 
    /// It includes properties like <see cref="Label"/> and <see cref="Synonyms"/>, 
    /// and defines an abstract method <see cref="GetMappedApiClientModel"/> for mapping to API client models.
    /// </remarks>
    public abstract class GeoSearchResultItem : DwbSearchResultItem
    {
        public string Label { get; set; }

        public string[] Synonyms { get; set; }

        public abstract GeoSearchResultItem? GetMappedApiClientModel();
    }
}
