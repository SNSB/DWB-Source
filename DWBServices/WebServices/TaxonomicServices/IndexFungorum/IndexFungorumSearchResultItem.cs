using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.IndexFungorum
{
    public class IndexFungorumSearchResult : TaxonomicSearchResult
    {
        public IndexFungorumSearchResultItem[] IndexFungorum { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                return IndexFungorum.Select(item => item.GetMappedApiClientModel()).ToArray();
            }
        }
    }

    public class IndexFungorumSearchResultItem : TaxonomicSearchResultItem
    {
        public string NAME_x0020_OF_x0020_FUNGUS { get; set; }

        public string RECORD_x0020_NUMBER { get; set; } // record id for single items
        public string AUTHORS { get; set; }
        public string SPECIFIC_x0020_EPITHET { get; set; }
        public string INFRASPECIFIC_x0020_RANK { get; set; }

        public string YEAR_x0020_OF_x0020_PUBLICATION { get; set; }

        public string BASIONYM_x0020_RECORD_x0020_NUMBER { get; set; }
        public string PROTONYM_x0020_RECORD_x0020_NUMBER { get; set; }
        public string NAME_x0020_OF_x0020_FUNGUS_x0020_FUNDIC_x0020_RECORD_x0020_NUMBER { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string AddedDate { get; set; }
        public string UUID { get; set; }

        public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["IndexFungorum:IndexFungorum_BaseAddress"];
            string QueryListPrefix = settingValue + "NameByKeyRDF?NameLsid=";
            this._URL = QueryListPrefix + RECORD_x0020_NUMBER;
            _DisplayText = NAME_x0020_OF_x0020_FUNGUS + " " + AUTHORS ?? string.Empty;
            Taxon = NAME_x0020_OF_x0020_FUNGUS ?? string.Empty;
            Genus = string.Empty; // TODO ?
            Epithet = NAME_x0020_OF_x0020_FUNGUS ?? string.Empty; // TODO ?
            Rank = INFRASPECIFIC_x0020_RANK ?? string.Empty;
            CommonNames = string.Empty; // TODO
            Status = string.Empty;
            Authors = AUTHORS ?? string.Empty; // TODO ?
            BasionymAuthors = string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = YEAR_x0020_OF_x0020_PUBLICATION ?? string.Empty;

            return this;
        }
    }

    public class IndexFungorumTaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        public override string QueryParamString => $"NameSearch?SearchText={query}&AnywhereInText={anywhereInText}&MaxNumber={maxPerPage}";

        public string query = ""; // QueryRestrictions

        public string anywhereInText = "true";

        public string maxPerPage = "10";


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
