using System.Text.Json.Serialization;

namespace DWBServices.WebServices.GeoServices.ISOCountries
{
    public class IsoCountriesEntityRootobject
    {
        [JsonPropertyName("results")] public IsoCountriesEntity[] results { get; set; }
    }

    public class IsoCountriesEntity : GeoEntity
    {
        public string[] synonyms { get; set; }
        public string sourceTerminology { get; set; }
        public string uri { get; set; }
        public string httpwwwgeonamesorgontologyiso31662 { get; set; }
        public string IsPartOf { get; set; }
        public string isinscheme { get; set; }
        public string[] label { get; set; }
        public string[] type { get; set; }
        public string[] hasexactmatch { get; set; }

        public override GeoEntity? GetMappedApiEntityModel()
        {
            _URL = uri;
            _DisplayText = uri ?? string.Empty;
            LabelName = label != null && label.Length > 0 ? label[0] : string.Empty;
            Latitude = string.Empty;
            Longitude = string.Empty;
            ISOCountrycode = httpwwwgeonamesorgontologyiso31662 ?? string.Empty;
            return this;
        }
    }
}
