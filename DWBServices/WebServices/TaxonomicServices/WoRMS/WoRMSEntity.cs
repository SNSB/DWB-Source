using System.Text.Json.Serialization;
using static DWBServices.WebServices.TaxonomicServices.Mycobank.MycobankEntity;

namespace DWBServices.WebServices.TaxonomicServices.WoRMS
{
    public class WoRMSEntity : TaxonomicEntity
    {
        public int AphiaID { get; set; }
        public string url { get; set; }
        public string scientificname { get; set; }
        public string authority { get; set; }
        public string status { get; set; }
        public object unacceptreason { get; set; }
        public int? taxonRankID { get; set; }
        public string rank { get; set; }
        public int? valid_AphiaID { get; set; }
        public string valid_name { get; set; }
        public string valid_authority { get; set; }
        public int? parentNameUsageID { get; set; }
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
        public object isExtinct { get; set; }
        public string match_type { get; set; }
        public DateTime modified { get; set; }

        public override TaxonomicEntity? GetMappedApiEntityModel()
        {
            _URL = url ?? string.Empty;
            _DisplayText = scientificname ?? string.Empty;
            Taxon = scientificname ?? string.Empty;
            TaxonNameSinAuthor = string.Empty;
            AcceptedName = status == "accepted" ? Taxon : string.Empty;
            Family = family ?? string.Empty; // TODO ??
            Order = order ?? string.Empty; // TODO
            Genus = genus ?? string.Empty;
            Epithet = string.Empty;
            Rank = rank ?? string.Empty;
            CommonNames =string.Empty; // TODO
            Status = status ?? string.Empty;
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
}
