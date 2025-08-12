using System.Text.Json.Serialization;

namespace DWBServices.WebServices.GeoServices.IHOWorldSeas
{
    public class IHOWorldSeasEntityRootobject
    {
        [JsonPropertyName("results")] public IHOWorldSeasEntity[] results { get; set; }
    }

    public class IHOWorldSeasEntity : GeoEntity
    {
        [JsonPropertyName("Minimum Latitude")]
        public string MinimumLatitude { get; set; }
        public string sourceTerminology { get; set; }
        public string uri { get; set; }
        [JsonPropertyName("Maximum Latitude")]
        public string MaximumLatitude { get; set; }
        public string FeatureClass { get; set; }
        [JsonPropertyName("Maximum Longitude")]
        public string MaximumLongitude { get; set; }
        [JsonPropertyName("Minimum Longitude")]
        public string MinimumLongitude { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string ParentFeature { get; set; }
        public string[] seeAlso { get; set; }

        public override GeoEntity? GetMappedApiEntityModel()
        {
            _URL = uri;
            _DisplayText = uri ?? string.Empty;
            LabelName = label ?? string.Empty;
            Latitude = "Min: " + MinimumLatitude + ", Max: " + MaximumLatitude;
            Longitude = "Min: " + MinimumLongitude + ", Max: " + MaximumLongitude ;
            ISOCountrycode = string.Empty;
            return this;
        }
    }
}
