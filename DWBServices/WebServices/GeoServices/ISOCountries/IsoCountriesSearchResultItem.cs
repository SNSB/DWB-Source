using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.GeoServices.ISOCountries
{
    public class IsoCountriesSearchResult : GeoSearchResult
    {
        [JsonPropertyName("results")]
        public IsoCountriesSearchResultItem[] Result { get; set; }
        public override IsoCountriesSearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                return Result.Select(item => item.GetMappedApiClientModel()).ToArray();
            }
        }
    }
    public class IsoCountriesSearchResultItem : GeoSearchResultItem
    {
        public string label { get; set; }
        public string uri { get; set; }
        public string[] synonyms { get; set; }
        public string sourceTerminology { get; set; }
        public string _internal { get; set; }
        public override IsoCountriesSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["IsoCountries:IsoCountries_DetailListPrefix"];
            this._URL = QueryListPrefix + uri;
            _DisplayText = label ?? string.Empty;
            Label = label ?? string.Empty;
            Synonyms = synonyms;

            return this;
        }
    }

    public class IsoCountriesGeoSearchCriterias : GeoSearchCriteria
    {
        public override string QueryParamString => $"search?query={query}&match_type={matchtype}&terminologies={terminologies}&first_hit={firsthit}&format={format}&internal_only={internalonly}";

        public string query = ""; // QueryRestrictions

        public string terminologies = "ISOCOUNTRIES";

        public string matchtype = "included";

        public string firsthit = "false";

        public string format = "json";

        public string internalonly = "false";

        public bool ValidateQueryRestrictions(string queryRestrictions, int offset, int maxPerPage)
        {
            // Regular expression to match valid URL characters
            string pattern = @"^[a-zA-Z0-9\-._~:/?#\[\]@!$&'()*+,;=%]*$";
            if (queryRestrictions == null || !Regex.IsMatch(queryRestrictions, pattern))
                return false;
            if (offset < 0 || maxPerPage <= 0)
                return false;
            return true;
        }

    }
}
