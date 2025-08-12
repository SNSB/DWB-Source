using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices.TaxonomicServices.IndexFungorum
{
    public class IndexFungorumEntity : TaxonomicEntity
    {
        public TaxonName taxonName {  get; set; }

        public PublicationCitation publicationCitation { get; set; }

        public override TaxonomicEntity? GetMappedApiEntityModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["IndexFungorum:IndexFungorum_DetailListPrefix"];
            _URL = settingValue + taxonName?.rdf_about;
            _DisplayText = taxonName?.ns_Title ?? string.Empty;
            Taxon = taxonName?.ns_Title ?? string.Empty;
            TaxonNameSinAuthor = taxonName?.nameComplete ?? string.Empty;
            AcceptedName = string.Empty;
            Family = string.Empty; // TODO ??
            Order = string.Empty; // TODO
            Genus = string.Empty;
            Epithet = taxonName?.specificEpithet ?? string.Empty;
            Rank = taxonName?.rankString ?? string.Empty;
            CommonNames = string.Empty; // TODO
            Status = string.Empty;
            Authors = taxonName?.authorship ?? string.Empty; // TODO ?
            BasionymAuthors = taxonName?.basionymAuthorship?.ToString() ?? string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = taxonName?.year?.ToString() ?? string.Empty;

            return this;
        }
        public class TaxonName
        {
            public string rdf_about { get; set; } 
            public string ns_Title { get; set; } 
            public string owl_versionInfo { get; set; } 

            public string genusPart { get; set; }
            public string nameComplete
                { get; set; } 
            public string specificEpithet { get; set; } 

            public string infraspecificEpithet { get; set; }
            public string authorship { get; set; }

            public string basionymAuthorship { get; set; }

            public string combinationAuthorship { get; set; }
            public string year { get; set; } 
            public string microReference { get; set; }

            public string Common_publishedInCitation { get; set; }

            public string rank { get; set; }
            public string rankString { get; set; } 

            public string nomenclaturalCode { get; set; } 
        }

        public class PublicationCitation
        {
            public string rdf_nodeID { get; set; }
            public string year { get; set; } 
            public string title { get; set; } 
            public string volume   { get; set; }
            public string pages { get; set; } 

        }
    }
}
