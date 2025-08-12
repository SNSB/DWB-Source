namespace DWBServices.WebServices.TaxonomicServices
{
    /// <summary>
    /// Represents a base class for taxonomic entities, providing common properties and functionality 
    /// for handling taxonomic data such as taxonomy hierarchy, authorship, and classification details.
    /// </summary>
    /// <remarks>
    /// This abstract class serves as a foundation for specific taxonomic entity implementations, 
    /// such as those used in various taxonomic services (e.g., Catalogue of Life, Index Fungorum, Mycobank).
    /// It includes properties for taxonomic rank, hierarchy, authorship, and other related metadata.
    /// Derived classes must implement the <see cref="GetMappedApiEntityModel"/> method to provide
    /// a mapped API entity model specific to the implementation.
    /// </remarks>
    public abstract class TaxonomicEntity : DwbEntity
    {
        public string Taxon { get; set; }
        
        public string TaxonNameSinAuthor { get; set; }
        
        public string AcceptedName { get; set; }

        public string Family { get; set; }

        public string Order { get; set; }

        public string Genus { get; set; }

        public string Epithet { get; set; }

        public string Rank { get; set; }

        public string CommonNames { get; set; }

        public string Status { get; set; }

        public string Authors { get; set; }

        public string BasionymAuthors { get; set; }

        public string CombiningAuthors { get; set; }

        public string Kingdom { get; set; }

        public string Subkingdom { get; set; }

        public string Hierarchy { get; set; }

        public string Unranked { get; set; }

        public string Year { get; set; }

        public abstract override TaxonomicEntity? GetMappedApiEntityModel();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetPropertyToTaxonSynonymyMapping()
        {
            // Define the mapping between properties and the database columns of the cache database TaxonSynonymy
            return new Dictionary<string, string>
            {
                { "Taxon", "TaxonName" },
                { "TaxonNameSinAuthor", "TaxonNameSinAuthor" },
                { "Rank", "TaxonomicRank" },
                { "AcceptedName", "AcceptedName"},
                { "Genus", "GenusOrSupragenericName"}
            };
        }

    }
}
