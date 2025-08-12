namespace DWBServices.WebServices.GeoServices
{
    /// <summary>
    /// Represents an abstract base class for geographical entities, providing common properties 
    /// such as location details, description, and ISO country code. 
    /// This class serves as a foundation for more specific geographical entity types.
    /// </summary>
    public abstract class GeoEntity : DwbEntity
    {
        public string LabelName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string ISOCountrycode { get; set; }
        


        public abstract override GeoEntity? GetMappedApiEntityModel();

    }
}
