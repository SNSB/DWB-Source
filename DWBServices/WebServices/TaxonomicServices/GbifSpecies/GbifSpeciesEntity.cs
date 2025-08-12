using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices.TaxonomicServices.GbifSpecies
{
    public class GbifSpeciesEntity : TaxonomicEntity
    {
        public int key { get; set; }
        public int nubKey { get; set; }
        public int nameKey { get; set; }
        public string taxonID { get; set; }
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
        public string datasetKey { get; set; }
        public int parentKey { get; set; }
        public string parent { get; set; }
        public string scientificName { get; set; }
        public string canonicalName { get; set; }
        public string authorship { get; set; }
        public string nameType { get; set; }
        public string rank { get; set; }
        public string origin { get; set; }
        public string taxonomicStatus { get; set; }
        public object[] nomenclaturalStatus { get; set; }
        public string remarks { get; set; }
        public int numDescendants { get; set; }
        public DateTime lastCrawled { get; set; }
        public DateTime lastInterpreted { get; set; }
        public object[] issues { get; set; }
        public string _class { get; set; }

        public override TaxonomicEntity? GetMappedApiEntityModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string DetailListPrefix = configuration["GbifSpecies:GbifSpecies_DetailUri"];
            _URL = DetailListPrefix + key;
            _DisplayText = scientificName ?? string.Empty;
            Taxon = scientificName ?? string.Empty;
            TaxonNameSinAuthor = string.Empty;
            AcceptedName = taxonomicStatus == "accepted" ? Taxon : string.Empty;
            Family = family ?? string.Empty; // TODO ??
            Order = order ?? string.Empty; // TODO
            Genus = genus ?? string.Empty;
            Epithet = string.Empty;
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
}
