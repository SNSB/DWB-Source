using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.PESI
{
    public class PESISearchResult : TaxonomicSearchResult
    {
        [JsonPropertyName("results")]
        public PESISearchResultItem[] Result { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                // return Result.Select(item => item.GetMappedApiClientModel()).ToArray();
                return Result
                    .Where(item => !string.IsNullOrEmpty(item.uri))
                    .Select(item => item.GetMappedApiClientModel())
                    .ToArray();
            }
        }
    }
    public class PESISearchResultItem : TaxonomicSearchResultItem
    {
        public string label { get; set; }
        public string uri { get; set; }
        public string kingdom { get; set; }
        public string rank { get; set; }
        public string status { get; set; }
        public string externalID { get; set; }
        public string sourceTerminology { get; set; }
        public string embeddedDatabase { get; set; }
        
        public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["PESI:PESI_DetailListPrefix"];
            this._URL = QueryListPrefix + uri;
            _DisplayText = label ?? string.Empty;
            Taxon = label ?? string.Empty;
            Genus = string.Empty; // TODO ?
            Epithet = string.Empty; // TODO ?
            Rank = rank ?? string.Empty;
            CommonNames = string.Empty; // TODO
            Status = status ?? string.Empty;
            Authors = string.Empty; // TODO ?
            BasionymAuthors = string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = kingdom ?? string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = string.Empty;

            return this;
        }
    }

    public class PESITaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        public override string QueryParamString => $"search?&query={query}&match_type={matchtype}&terminologies={terminologies}&first_hit={firsthit}&format={format}&internal_only={internalonly}";

        public string query = ""; // QueryRestrictions

        public string terminologies = "PESI";

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
