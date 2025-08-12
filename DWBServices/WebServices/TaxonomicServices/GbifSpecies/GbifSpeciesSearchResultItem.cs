using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.GbifSpecies
{
    public class GbifSpeciesSearchResult : TaxonomicSearchResult
    {
        [JsonPropertyName("results")] public GbifSpeciesSearchResultItem[] Result { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public bool endOfRecords { get; set; }
        public int count { get; set; }
        public object[] facets { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get { return Result.Select(item => item.GetMappedApiClientModel()).ToArray(); }
        }
    }

    public class GbifSpeciesSearchResultItem : TaxonomicSearchResultItem
    {
        public int key { get; set; }
        public string datasetKey { get; set; }
        public int nubKey { get; set; }
        public int parentKey { get; set; }
        public string parent { get; set; }
        public string kingdom { get; set; }
        public string phylum { get; set; }
        public string order { get; set; }
        public string family { get; set; }
        public string genus { get; set; }
        public string species { get; set; }
        public int kingdomKey { get; set; }
        public int phylumKey { get; set; }
        public int classKey { get; set; }
        public int orderKey { get; set; }
        public int familyKey { get; set; }
        public int genusKey { get; set; }
        public int speciesKey { get; set; }
        public string scientificName { get; set; }
        public string canonicalName { get; set; }
        public string nameType { get; set; }
        public string taxonomicStatus { get; set; }
        public string rank { get; set; }
        public string origin { get; set; }
        public int numDescendants { get; set; }
        public int numOccurrences { get; set; }
        public string taxonID { get; set; }
        public string[] habitats { get; set; }
        public string[] nomenclaturalStatus { get; set; }
        public string[] threatStatuses { get; set; }
        public Description[] descriptions { get; set; }
        public Vernacularname[] vernacularNames { get; set; }
        public Higherclassificationmap higherClassificationMap { get; set; }
        public bool synonym { get; set; }
        public string _class { get; set; }
        public int nameKey { get; set; }
        public string authorship { get; set; }
        public int acceptedKey { get; set; }
        public string accepted { get; set; }
        public string constituentKey { get; set; }
        public int basionymKey { get; set; }
        public string basionym { get; set; }
        public string publishedIn { get; set; }
        public bool extinct { get; set; }

        public class Higherclassificationmap
        {
            public string _103504667 { get; set; }
            public string _103707375 { get; set; }
            public string _103720060 { get; set; }
            public string _103720070 { get; set; }
            public string _103724303 { get; set; }
            public string _103724314 { get; set; }
            public string _196630041 { get; set; }
            public string _196630043 { get; set; }
            public string _196630044 { get; set; }
            public string _196630190 { get; set; }
            public string _196630250 { get; set; }
            public string _196630252 { get; set; }
            public string _247430402 { get; set; }
            public string _178736636 { get; set; }
            public string _178736784 { get; set; }
            public string _178749155 { get; set; }
            public string _178752400 { get; set; }
            public string _178762910 { get; set; }
            public string _178763077 { get; set; }
            public string _252394563 { get; set; }
            public string _252394564 { get; set; }
            public string _252394565 { get; set; }
            public string _252394566 { get; set; }
            public string _252394585 { get; set; }
            public string _252394586 { get; set; }
            public string _165611957 { get; set; }
            public string _210386455 { get; set; }
            public string _195632352 { get; set; }
            public string _195632386 { get; set; }
            public string _195632612 { get; set; }
            public string _195632834 { get; set; }
            public string _195632914 { get; set; }
            public string _195632918 { get; set; }
            public string _159998387 { get; set; }
            public string _159999567 { get; set; }
            public string _160001514 { get; set; }
            public string _160009672 { get; set; }
            public string _160017785 { get; set; }
            public string _160017797 { get; set; }
            public string _221767405 { get; set; }
            public string _221768466 { get; set; }
            public string _221771010 { get; set; }
            public string _221771525 { get; set; }
            public string _221771544 { get; set; }
            public string _221771855 { get; set; }
            public string _178765772 { get; set; }
            public string _3087748 { get; set; }
            public string _103725338 { get; set; }
            public string _209385926 { get; set; }
            public string _209385927 { get; set; }
            public string _209385928 { get; set; }
            public string _209385960 { get; set; }
            public string _209386073 { get; set; }
            public string _209386074 { get; set; }
            public string _28987 { get; set; }
            public string _160776553 { get; set; }
            public string _160780279 { get; set; }
            public string _160780546 { get; set; }
            public string _160780696 { get; set; }
            public string _205177247 { get; set; }
            public string _205177255 { get; set; }
            public string _205799412 { get; set; }
            public string _711 { get; set; }
            public string _5 { get; set; }
            public string _34 { get; set; }
            public string _186 { get; set; }
            public string _1499 { get; set; }
            public string _4171 { get; set; }
            public string _6005964 { get; set; }
            public string _217768396 { get; set; }
            public string _217768426 { get; set; }
            public string _217768469 { get; set; }
            public string _217768503 { get; set; }
            public string _217768636 { get; set; }
            public string _217768641 { get; set; }
            public string _185474745 { get; set; }
            public string _185474748 { get; set; }
            public string _185474751 { get; set; }
            public string _211524678 { get; set; }
            public string _211531742 { get; set; }
            public string _211531750 { get; set; }
        }

        public class Description
        {
            public string description { get; set; }
        }

        public class Vernacularname
        {
            public string vernacularName { get; set; }
            public string language { get; set; }
        }

        public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException(
                                    "DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["GbifSpecies:GbifSpecies_DetailListPrefix"];
            this._URL = QueryListPrefix + key;
            _DisplayText = scientificName ?? string.Empty;
            Taxon = scientificName ?? string.Empty;
            Genus = genus ?? string.Empty; // TODO ?
            Epithet = string.Empty; // TODO ?
            Rank = rank ?? string.Empty;
            CommonNames = canonicalName ?? string.Empty; // TODO
            Status = taxonomicStatus ?? string.Empty;
            Authors = authorship ?? string.Empty; // TODO ?
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

    public class GbifSpeciesTaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        public override string QueryParamString => $"search?datasetKey={datasetkey}&q={query}&limit={limit}%offset={offset}";

        public string query = ""; // QueryRestrictions

        public string offset = "0";

        public string limit = "";

        public string datasetkey = "d7dddbf4-2cf0-4f39-9b2a-bb099caae36c"; // GBIF Backbone


        public bool ValidateQueryRestrictions(string queryRestrictions, int offset, int maxResult)
        {
            // Regular expression to match valid URL characters
            string pattern = @"^[a-zA-Z0-9\-._~:/?#\[\]@!$&'()*+,;=%]*$";
            if (queryRestrictions == null || !Regex.IsMatch(queryRestrictions, pattern))
                return false;
            if (offset < 0 || maxResult <= 0)
                return false;
            return true;
        }

    }
}
