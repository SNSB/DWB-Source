using System.Text.Json.Serialization;
using static DWBServices.WebServices.TaxonomicServices.Mycobank.MycobankEntity;

namespace DWBServices.WebServices.TaxonomicServices.PESI
{
    public class PESIEntityRootobject
    {
        [JsonPropertyName("results")]
        public PESIEntity[] results { get; set; }
    }
    public class PESIEntity : TaxonomicEntity
    {

        public string label { get; set; }
        public string uri { get; set; }
        public string kingdom { get; set; }
        public string rank { get; set; }
        public string status { get; set; }
        public string[] commonNames { get; set; }
        public string externalID { get; set; }
        public string sourceTerminology { get; set; }
        public string embeddedDatabase { get; set; }
        public string citation { get; set; }
        public string[] distribution { get; set; }

        public override TaxonomicEntity? GetMappedApiEntityModel()
        {
            _URL = uri;
            _DisplayText = label ?? string.Empty;
            Taxon = label ?? string.Empty;
            TaxonNameSinAuthor = string.Empty;
            AcceptedName = status == "accepted" ? Taxon : string.Empty;
            Family = string.Empty; // TODO ??
            Order = string.Empty; // TODO
            Genus = string.Empty;
            Epithet = string.Empty;
            Rank = rank ?? string.Empty;
            CommonNames = commonNames != null ? string.Join(", ", commonNames) : string.Empty; // TODO
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

}
