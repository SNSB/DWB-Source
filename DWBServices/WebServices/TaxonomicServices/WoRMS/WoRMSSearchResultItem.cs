using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.WoRMS
{
    public class WoRMSSearchResult : TaxonomicSearchResult
    {
        public WoRMSSearchResultItem[][] Items { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get { return Items?[0]?.Select(item => item.GetMappedApiClientModel()).ToArray(); }
        }
    }

    public class WoRMSSearchResultItem : TaxonomicSearchResultItem
    {
        public int AphiaID { get; set; }
        public string url { get; set; }
        public string scientificname { get; set; }
        public string authority { get; set; }
        public string status { get; set; }
        public string unacceptreason { get; set; }
        public int taxonRankID { get; set; }
        public string rank { get; set; }
        public int? valid_AphiaID { get; set; }
        public string valid_name { get; set; }
        public string valid_authority { get; set; }
        public int parentNameUsageID { get; set; }
        public string kingdom { get; set; }
        public string phylum { get; set; }
        [JsonPropertyName("class")]
        public string _class { get; set; }
        public string order { get; set; }
        public string family { get; set; }
        public string genus { get; set; }
        public string citation { get; set; }
        public string lsid { get; set; }
        public int? isMarine { get; set; }
        public int? isBrackish { get; set; }
        public int? isFreshwater { get; set; }
        public int? isTerrestrial { get; set; }
        public int? isExtinct { get; set; }
        public string match_type { get; set; }
        public DateTime modified { get; set; }

    public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException(
                                    "DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["WoRMS:WoRMS_DetailListPrefix"];
            this._URL = QueryListPrefix + AphiaID;
            _DisplayText = string.IsNullOrEmpty(scientificname)
                ? string.Empty
                : scientificname + (string.IsNullOrEmpty(authority) ? string.Empty : " " + authority);
            Taxon = scientificname ?? string.Empty;
            Genus = genus ?? string.Empty; // TODO ?
            Epithet = string.Empty; // TODO ?
            Rank = rank ?? string.Empty;
            CommonNames = string.Empty; // TODO
            Status =status ?? string.Empty;
            Authors = authority ?? string.Empty; // TODO ?
            BasionymAuthors = string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = kingdom ?? string.Empty;
            Subkingdom = phylum ?? string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = modified.Year.ToString();

            return this;
        }
    }

    public class WoRMSTaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        public override string QueryParamString => $"AphiaRecordsByNames?scientificnames%5B%5D={query}&&like={like}&marine_only={marine_only}";

        public string query = ""; // QueryRestrictions

        public string offset = "0";

        public string limit = "20";

        private string like = "true";

        private string marine_only = "false";


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