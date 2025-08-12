using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices.TaxonomicServices.CatalogueOfLife
{
    public class CoLEntity : TaxonomicEntity
    {
        public DateTime created { get; set; }
        public int createdBy { get; set; }
        public DateTime modified { get; set; }
        public int modifiedBy { get; set; }
        public int datasetKey { get; set; }
        public string id { get; set; }
        public int verbatimKey { get; set; }
        public Name name { get; set; }
        public string status { get; set; }
        public string origin { get; set; }
        public string parentId { get; set; }
        public Accepted accepted { get; set; }
        public string label { get; set; }
        public string labelHtml { get; set; }

        public override TaxonomicEntity? GetMappedApiEntityModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string DetailListPrefix = configuration["CoL:CoL_DetailUri"];
            _URL = name?.link ?? DetailListPrefix + id;
            _DisplayText = label ?? string.Empty;
            Taxon = label ?? string.Empty;
            TaxonNameSinAuthor = name?.scientificName ?? string.Empty;
            AcceptedName = status == "accepted" ? Taxon : string.Empty;
            Family = string.Empty; // TODO ??
            Order = string.Empty; // TODO
            Genus = name?.genus ?? string.Empty;
            Epithet = name?.specificEpithet ?? string.Empty;
            Rank = name?.rank ?? string.Empty;
            CommonNames = string.Empty; // TODO
            Status = status;
            Authors = name?.authorship ?? string.Empty; // TODO ?string.Join(", ", commonNames) : string.Empty;
            BasionymAuthors = string.Join(", ", name?.basionymOrCombinationAuthorship?.authors ?? Enumerable.Empty<string>());
            CombiningAuthors = string.Join(", ", name?.combinationAuthorship?.authors ?? Enumerable.Empty<string>());
            Kingdom = string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = created.Year.ToString();

            return this;
        }
    }

    public class Name
    {
        public DateTime created { get; set; }
        public int createdBy { get; set; }
        public DateTime modified { get; set; }
        public int modifiedBy { get; set; }
        public int datasetKey { get; set; }
        public string id { get; set; }
        public int verbatimKey { get; set; }
        public int namesIndexId { get; set; }
        public string namesIndexType { get; set; }
        public string[] identifier { get; set; }
        public string scientificName { get; set; }
        public string authorship { get; set; }
        public string rank { get; set; }
        public string genus { get; set; }
        public string specificEpithet { get; set; }
        public Combinationauthorship combinationAuthorship { get; set; }
        public string code { get; set; }
        public string publishedInId { get; set; }
        public string origin { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public bool parsed { get; set; }
        public Basionymorcombinationauthorship basionymOrCombinationAuthorship { get; set; }
    }

    public class Combinationauthorship
    {
        public string[] authors { get; set; }
    }

    public class Basionymorcombinationauthorship
    {
        public string[] authors { get; set; }
    }

    public class Accepted
    {
        public DateTime created { get; set; }
        public int createdBy { get; set; }
        public DateTime modified { get; set; }
        public int modifiedBy { get; set; }
        public int datasetKey { get; set; }
        public string id { get; set; }
        public int verbatimKey { get; set; }
        public Name1 name { get; set; }
        public string status { get; set; }
        public string origin { get; set; }
        public string parentId { get; set; }
        public string link { get; set; }
        public string remarks { get; set; }
        public string label { get; set; }
        public string labelHtml { get; set; }
    }

    public class Name1
    {
        public DateTime created { get; set; }
        public int createdBy { get; set; }
        public DateTime modified { get; set; }
        public int modifiedBy { get; set; }
        public int datasetKey { get; set; }
        public string id { get; set; }
        public int verbatimKey { get; set; }
        public int namesIndexId { get; set; }
        public string namesIndexType { get; set; }
        public string[] identifier { get; set; }
        public string scientificName { get; set; }
        public string authorship { get; set; }
        public string rank { get; set; }
        public string genus { get; set; }
        public string specificEpithet { get; set; }
        public Combinationauthorship1 combinationAuthorship { get; set; }
        public Basionymauthorship basionymAuthorship { get; set; }
        public string code { get; set; }
        public string publishedInId { get; set; }
        public string origin { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public bool parsed { get; set; }
        public Basionymorcombinationauthorship1 basionymOrCombinationAuthorship { get; set; }
    }

    public class Combinationauthorship1
    {
        public string[] authors { get; set; }
        public string[] exAuthors { get; set; }
    }

    public class Basionymauthorship
    {
        public string[] authors { get; set; }
        public string[] exAuthors { get; set; }
    }

    public class Basionymorcombinationauthorship1
    {
        public string[] authors { get; set; }
        public string[] exAuthors { get; set; }
    }


    //public class CoLEntity : TaxonomicEntity
    //{
    //    public DateTime created { get; set; }
    //    public int createdBy { get; set; }
    //    public DateTime modified { get; set; }
    //    public int modifiedBy { get; set; }
    //    public int datasetKey { get; set; }
    //    public string id { get; set; }
    //    public int sectorKey { get; set; }
    //    public int verbatimKey { get; set; }
    //    public Name name { get; set; }
    //    public string status { get; set; }
    //    public string origin { get; set; }
    //    public string parentId { get; set; }
    //    public string namePhrase { get; set; }
    //    public string accordingTo { get; set; }
    //    public string accordingToId { get; set; }
    //    public string link { get; set; }
    //    public string remarks { get; set; }
    //    public string[] referenceIds { get; set; }
    //    public string label { get; set; }
    //    public string labelHtml { get; set; }
    //    public bool merged { get; set; }

    //    public override TaxonomicEntity? GetMappedApiEntityModel()
    //    {
    //        //var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
    //        //                    ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
    //        //string DetailListPrefix = configuration["CoL:CoL_DetailUri"];
    //        _URL = link;
    //        _DisplayText = label ?? string.Empty;
    //        Taxon = label ?? string.Empty;
    //        Family = string.Empty; // TODO ??
    //        Order = string.Empty; // TODO
    //        Genus = name?.genus ?? string.Empty;
    //        Epithet = name?.specificEpithet ?? string.Empty;
    //        Rank = name?.rank ?? string.Empty;
    //        CommonNames = name?.scientificName ?? string.Empty; // TODO
    //        Status = status;
    //        Authors = name?.authorship ?? string.Empty; // TODO ?
    //        BasionymAuthors = name?.basionymAuthorship?.ToString() ?? string.Empty; // TODO ?
    //        CombiningAuthors = name?.combinationAuthorship?.ToString() ?? string.Empty; // TODO ?
    //        Kingdom = string.Empty;
    //        Subkingdom = string.Empty;
    //        Hierarchy = string.Empty;
    //        Unranked = string.Empty;
    //        Year = created.Year.ToString();

    //        return this;
    //    }
    //}

    //public class Name
    //{
    //    public DateTime created { get; set; }
    //    public int createdBy { get; set; }
    //    public DateTime modified { get; set; }
    //    public int modifiedBy { get; set; }
    //    public int datasetKey { get; set; }
    //    public string id { get; set; }
    //    public int sectorKey { get; set; }
    //    public int verbatimKey { get; set; }
    //    public int namesIndexId { get; set; }
    //    public string namesIndexType { get; set; }
    //    public string scientificName { get; set; }
    //    public string authorship { get; set; }
    //    public string rank { get; set; }
    //    public string uninomial { get; set; }
    //    public string genus { get; set; }
    //    public string infragenericEpithet { get; set; }
    //    public string specificEpithet { get; set; }
    //    public string infraspecificEpithet { get; set; }
    //    public string cultivarEpithet { get; set; }
    //    public bool candidatus { get; set; }
    //    public string notho { get; set; }
    //    public Combinationauthorship combinationAuthorship { get; set; }
    //    public Basionymauthorship basionymAuthorship { get; set; }
    //    public string sanctioningAuthor { get; set; }
    //    public string code { get; set; }
    //    public string nomStatus { get; set; }
    //    public bool originalSpelling { get; set; }
    //    public string gender { get; set; }
    //    public string publishedInId { get; set; }
    //    public string publishedInPage { get; set; }
    //    public string publishedInPageLink { get; set; }
    //    public int publishedInYear { get; set; }
    //    public string origin { get; set; }
    //    public string type { get; set; }
    //    public string link { get; set; }
    //    public string nomenclaturalNote { get; set; }
    //    public string unparsed { get; set; }
    //    public string etymology { get; set; }
    //    public string remarks { get; set; }
    //    public string label { get; set; }
    //    public string labelHtml { get; set; }
    //    public bool merged { get; set; }
    //    public bool parsed { get; set; }
    //    public Basionymorcombinationauthorship basionymOrCombinationAuthorship { get; set; }
    //}

    //public class Combinationauthorship
    //{
    //    public string[] authors { get; set; }
    //    public string[] exAuthors { get; set; }
    //    public string year { get; set; }
    //}

    //public class Basionymauthorship
    //{
    //    public string[] authors { get; set; }
    //    public string[] exAuthors { get; set; }
    //    public string year { get; set; }
    //}

    //public class Basionymorcombinationauthorship
    //{
    //    public string[] authors { get; set; }
    //    public string[] exAuthors { get; set; }
    //    public string year { get; set; }
    //}

}