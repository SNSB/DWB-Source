using System.Text.Json.Serialization;

namespace DWBServices.WebServices.GeoServices.GFBioTermGeonames
{
    public class GFBioTermGeonamesEntityRootobject
    {
        [JsonPropertyName("results")] public GFBioTermGeonamesEntity[] results { get; set; }
    }

    public class GFBioTermGeonamesEntity : GeoEntity
    {
        public string featurecode { get; set; }
        public string sourceTerminology { get; set; }
        public string uri { get; set; }
        public string level4administrativeparent { get; set; }
        public string latitude { get; set; }
        public string description { get; set; }
        public string DateModified { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string level3administrativeparent { get; set; }
        public string level1administrativeparent { get; set; }
        public string ISOcountrycode { get; set; }
        public string population { get; set; }
        public string parentcountry { get; set; }
        public string featureclass { get; set; }
        public string name { get; set; }
        public string nearbyfeatures { get; set; }
        public string level2administrativeparent { get; set; }
        public string parentfeature { get; set; }
        public string map { get; set; }
        public string longitude { get; set; }

        public override GeoEntity? GetMappedApiEntityModel()
        {
            _URL = uri;
            _DisplayText = label ?? string.Empty;
            LabelName = label ?? string.Empty;
            Latitude = latitude ?? string.Empty;
            Longitude = longitude ?? string.Empty;
            Description = description ?? string.Empty;
            ISOCountrycode = ISOcountrycode ?? string.Empty;
            return this;
        }
    }
}
