using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.CatalogueOfLife
{
    public class CoLSearchResult : TaxonomicSearchResult
    {
        [JsonPropertyName("result")]
        public CoLSearchResultItem[] Result { get; set; }

        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public bool empty { get; set; }
        public bool last { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                return Result.Select(item => item.GetMappedApiClientModel()).ToArray();
            }
        }

    }

    public class CoLSearchResultItem : TaxonomicSearchResultItem
    {
        public string id { get; set; }
        public Classification[] classification { get; set; }
        public Usage usage { get; set; }
        public int sectorDatasetKey { get; set; }
        public string sectorPublisherKey { get; set; }
        public string group { get; set; }


        public class Usage
        {
            public DateTime created { get; set; }
            public int createdBy { get; set; }
            public DateTime modified { get; set; }
            public int modifiedBy { get; set; }
            public int datasetKey { get; set; }
            public string id { get; set; }
            public int sectorKey { get; set; }
            public Name name { get; set; }
            public string status { get; set; }
            public string origin { get; set; }
            public string parentId { get; set; }
            public string label { get; set; }
            public string labelHtml { get; set; }
            public bool merged { get; set; }
        }

        public class Name
        {
            public DateTime created { get; set; }
            public int createdBy { get; set; }
            public DateTime modified { get; set; }
            public int modifiedBy { get; set; }
            public int datasetKey { get; set; }
            public string id { get; set; }
            public int sectorKey { get; set; }
            public string scientificName { get; set; }
            public string rank { get; set; }
            public string uninomial { get; set; }
            public string code { get; set; }
            public string origin { get; set; }
            public string type { get; set; }
            public bool merged { get; set; }

            public bool parsed { get; set; }
        }

        public class Classification
        {
            public string id { get; set; }
            public string name { get; set; }
            public string rank { get; set; }
            public string label { get; set; }
            public string labelHtml { get; set; }
        }

        public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["CoL:CoL_BaseAddress"];
            this._URL = QueryListPrefix + usage?.datasetKey + "/nameusage/" + id;
            _DisplayText = usage?.label ?? string.Empty;
            Taxon = usage?.label ?? string.Empty;
            Genus = string.Empty; // TODO ?
            Epithet = string.Empty; // TODO ?
            Rank = usage?.name?.rank.ToString() ?? string.Empty;
            CommonNames = usage?.name?.scientificName ?? string.Empty; // TODO
            Status = usage.status;
            Authors = usage.name.createdBy.ToString() ?? string.Empty; // TODO ?
            BasionymAuthors = string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = usage.created.Year.ToString();

            return this;
        }
    }

    public class CoLTaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        public override string QueryParamString => $"{datasetKey}/nameusage/search?content={content}&q={query}&fuzzy={fuzzy}&type={type}&offset={offset}&limit={maxPerPage}";

        public string datasetKey { get; set; } = "3LR"; // default latest release 3LR

        public string query = ""; // QueryRestrictions

        public string content = "SCIENTIFIC_NAME";

        public string fuzzy = "false";

        public string type = "PREFIX";

        public string offset = "0";

        public string maxPerPage = "50";


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
