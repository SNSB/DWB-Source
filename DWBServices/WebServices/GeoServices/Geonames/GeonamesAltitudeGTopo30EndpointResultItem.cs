using System.Text.RegularExpressions;

namespace DWBServices.WebServices.GeoServices.Geonames
{
    // altitude endpoints
    public class GeonamesAltitudeGTopo30SearchResult : GeoSearchResult
    {
        public GeonamesAltitudeGTopo30SearchResultItem geoname { get; set; }

        public override GeonamesAltitudeGTopo30SearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                return new[] { geoname }
                    .ToArray()
                    ?? Array.Empty<GeonamesAltitudeGTopo30SearchResultItem>();
            }
        }
    }
    public class GeonamesAltitudeGTopo30SearchResultItem : GeoSearchResultItem
    {
        public double lng { get; set; }
        public double lat { get; set; }
        public int gtopo30 { get; set; }


        public override GeonamesAltitudeGTopo30SearchResultItem? GetMappedApiClientModel()
        {
            return this;
        }
    }
    public class GeoNamesAltitudeGTopo30EndpointCriterias : GeoSearchCriteria
    {
        public override string QueryParamString => $"{queryfunction}lat={lat}&lng={lng}&username={username}&style=full";

        // findNearbyPlaceName?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".") + "&username=" + username + "&style=full";

        public string queryfunction = "gtopo30JSON?"; // default

        private string _lat;
        public string lat
        {
            get => _lat;
            set
            {
                if (!double.TryParse(value.Replace(",", "."), out _))
                {
                    throw new ArgumentException("Invalid latitude value.");
                }
                _lat = value.Replace(",", ".");
            }
        }
        private string _lng;
        public string lng
        {
            get => _lng;
            set
            {
                if (!double.TryParse(value.Replace(",", "."), out _))
                {
                    throw new ArgumentException("Invalid longitude value.");
                }
                _lng = value.Replace(",", ".");
            }
        }

        public string username { get; set; }
        public bool ValidateQueryRestrictions(string queryRestrictions, int offset, int maxPerPage)
        {
            // Regular expression to match valid URL characters
            string pattern = @"^[a-zA-Z0-9\-._~:/?#\[\]@!$&'()*+,;=%]*$";
            try
            {
                ParseQueryRestrictions(queryRestrictions, out queryfunction);
                if (string.IsNullOrEmpty(queryfunction))
                    return false;
            }
            catch (ArgumentException aex)
            {
                return false;
            }

            if (queryRestrictions == null || !Regex.IsMatch(queryRestrictions, pattern))
                return false;
            if (offset < 0 || maxPerPage <= 0)
                return false;
            return true;
        }
        private void ParseQueryRestrictions(string queryRestrictions, out string queryFunction)
        {
            int questionMarkIndex = queryRestrictions.IndexOf('?');
            if (questionMarkIndex == -1)
            {
                throw new ArgumentException("Invalid queryRestrictions format. Missing '?' separator.");
            }
            queryFunction = queryRestrictions.Substring(0, questionMarkIndex + 1); // Extract query function
            string queryParams = queryRestrictions.Substring(questionMarkIndex + 1); // Extract parameters
            Dictionary<string, string> parameters = queryParams
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(param => param.Split('='))
                .ToDictionary(pair => pair[0], pair => pair.Length > 1 ? pair[1] : string.Empty);
            // Assign parsed values to respective variables
            if (parameters.TryGetValue("lat", out var parsedLat))
            {
                lat = parsedLat.Replace(",", ".");
            }
            if (parameters.TryGetValue("lng", out var parsedLng))
            {
                lng = parsedLng.Replace(",", ".");
            }
            if (parameters.TryGetValue("username", out var parsedUsername))
            {
                username = parsedUsername;
            }
        }
    }
}
