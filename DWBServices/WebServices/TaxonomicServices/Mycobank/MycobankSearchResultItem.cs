using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DWBServices.WebServices.TaxonomicServices.Mycobank
{
    
    public class MycobankSearchResult : TaxonomicSearchResult
    {
        public int TotalCount { get; set; }
        public Record[] Records { get; set; }

        public override TaxonomicSearchResultItem[] DwbApiSearchResponse
        {
            get
            {
                if (Records is null)
                {
                    return Array.Empty<TaxonomicSearchResultItem>();
                }

                return Records
                   .Select(record =>
                       record.RecordDetails?.GetMappedApiClientModel())
                   .Where(mappedItem => mappedItem != null)
                   .ToArray();

            }
        }
    }

    public class Record
    {
        public MycobankSearchResultItem RecordDetails { get; set; }
    }

    public class MycobankSearchResultItem : TaxonomicSearchResultItem
    {
        [JsonPropertyName("MycoBank #")]
        public Mycobank MycoBank { get; set; }
        [JsonPropertyName("Authors")]
        public new AuthorsClass Authors { get; set; }

        [JsonPropertyName("Authors (abbreviated)")]
        public AuthorsAbbreviated Authorsabbreviated { get; set; }

        [JsonPropertyName("Comment on name status")]
        public CommentOnNameStatus Commentonnamestatus { get; set; }
        [JsonPropertyName("Bibliography")]
        public Bibliography Bibliography1 { get; set; }
        [JsonPropertyName("Classification")]
        public Classification Classification1 { get; set; }
        [JsonPropertyName("Description")]
        public Description Description1 { get; set; }
        [JsonPropertyName("Etymology")]
        public Etymology Etymology1 { get; set; }
        [JsonPropertyName("Gender")]
        public Gender Gender1 { get; set; }
        [JsonPropertyName("Rank")]
        public new RankClass Rank { get; set; }
        [JsonPropertyName("Synonymy")]
        public Synonymy Synonymy1 { get; set; }
        [JsonPropertyName("Type name")]
        public TypeName Typename { get; set; }
        [JsonPropertyName("Name type")]
        public NameType Nametype { get; set; }
        [JsonPropertyName("Type of organism")]
        public TypeOfOrganism Typeoforganism { get; set; }
        [JsonPropertyName("Year of effective publication")]
        public YearOfEffectivePublication Yearofeffectivepublication { get; set; }
        [JsonPropertyName("Literature")]
        public Literature Literature1 { get; set; }
        [JsonPropertyName("Protolog")]
        public Protolog Protolog1 { get; set; }
        [JsonPropertyName("Type specimen or ex type")]
        public TypeSpecimenOrExType Typespecimenorextype { get; set; }
        [JsonPropertyName("Date entered")]
        public DateEntered Dateentered { get; set; }
        [JsonPropertyName("Date public")]
        public DatePublic Datepublic { get; set; }


        public class Mycobank
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class AuthorsClass
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class AuthorsAbbreviated
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class CommentOnNameStatus
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Bibliography
        {
            public Value[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Value
        {
            public int TemplateId { get; set; }
            public Name Name { get; set; }
            public Summary Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Summary
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Classification
        {
            public MycobankSearchResultItem[] ChildValue { get; set; }
            public Value1[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }


        public class Value1
        {
            public int TemplateId { get; set; }
            public Name2 Name { get; set; }
            public object Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name2
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Description
        {
            public Value2[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Value2
        {
            public int TemplateId { get; set; }
            public Name3 Name { get; set; }
            public Summary1 Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name3
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Summary1
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Etymology
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Gender
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class RankClass
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Synonymy
        {
            public int SynonymId { get; set; }
            public int ObligateSynonymId { get; set; }
            public object DesktopInfo { get; set; }
            public string DesktopInfoHtml { get; set; }
            public object OriginalSynFieldInfo { get; set; }
            public object NewSynFieldInfo { get; set; }
            public Syninfo SynInfo { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Syninfo
        {
            public Selectedrecord SelectedRecord { get; set; }
            public Currentnamerecord CurrentNameRecord { get; set; }
            public Taxonsynonymsrecord[] TaxonSynonymsRecords { get; set; }
            public Basionymrecord BasionymRecord { get; set; }
            public Obligatesynonymrecord[] ObligateSynonymRecords { get; set; }
        }

        public class Selectedrecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public object SecondLevelRecords { get; set; }
        }

        public class Currentnamerecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public object SecondLevelRecords { get; set; }
        }

        public class Basionymrecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public object SecondLevelRecords { get; set; }
        }

        public class Taxonsynonymsrecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public Secondlevelrecord[] SecondLevelRecords { get; set; }
        }

        public class Secondlevelrecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public object SecondLevelRecords { get; set; }
        }

        public class Obligatesynonymrecord
        {
            public int RecordId { get; set; }
            public string RecordName { get; set; }
            public string NameInfo { get; set; }
            public object SecondLevelRecords { get; set; }
        }

        public class TypeName
        {
            public object[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class NameType
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class TypeOfOrganism
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class YearOfEffectivePublication
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Literature
        {
            public Value3[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Value3
        {
            public int TemplateId { get; set; }
            public Name4 Name { get; set; }
            public Summary2 Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name4
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Summary2
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Protolog
        {
            public Value4[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Value4
        {
            public int TemplateId { get; set; }
            public Name5 Name { get; set; }
            public Summary3 Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name5
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Summary3
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class TypeSpecimenOrExType
        {
            public Value5[] Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Value5
        {
            public int TemplateId { get; set; }
            public Name6 Name { get; set; }
            public Summary4 Summary { get; set; }
            public int RecordId { get; set; }
            public object TargetFieldValue { get; set; }
        }

        public class Name6
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class Summary4
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public object FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class DateEntered
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

        public class DatePublic
        {
            public string Value { get; set; }
            public int FieldType { get; set; }
            public bool IsEmpty { get; set; }
            public string FieldName { get; set; }
            public object CreationDate { get; set; }
            public object RecordId { get; set; }
            public object TargetFieldKey { get; set; }
        }

 
        public override TaxonomicSearchResultItem? GetMappedApiClientModel()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException(
                                    "DwbServiceProviderAccessor.Instance is not initialized.");
            string QueryListPrefix = configuration["Mycobank:Mycobank_DetailListPrefix"];
            this._URL = QueryListPrefix + Synonymy1?.SynInfo?.SelectedRecord?.RecordId;
            _DisplayText = Synonymy1?.SynInfo?.SelectedRecord?.RecordName?.Trim() ?? string.Empty;
            Taxon = _DisplayText ?? string.Empty;
            Genus = string.Empty; // TODO ?
            Epithet = string.Empty; // TODO ?
            //Rank = string.Empty;
            CommonNames = string.Empty; // TODO
            Status = string.Empty;
            //Authors = string.Empty; // TODO ?
            BasionymAuthors = string.Empty; // TODO ?
            CombiningAuthors = string.Empty; // TODO ?
            Kingdom = string.Empty;
            Subkingdom = string.Empty;
            Hierarchy = string.Empty;
            Unranked = string.Empty;
            Year = string.Empty;

            return this;
        }
    }


    internal class MycobankTaxonomicSearchCriterias : TaxonomicSearchCriteria
    {
        // at the moment hardcoded values. could be changed to be dynamic in the fterue.
        public override string QueryParamString => $"{{\"Query\":[{{\"Index\":0,\"FieldName\":\"Taxon name\",\"Operation\":\"TextStartWith\",\"Value\":\"{query}\"}}],\"Expression\":\"Q0\",\"DisplayStart\":0,\"DisplayLength\":50}}";
        public string query = ""; // QueryRestrictions
        

        public bool ValidateQueryRestrictions(string queryRestrictions, int offset, int maxPerPage)
        {
            // Regular expression to match valid URL characters, for mycobank spaces like in Amanita cae are alloweid.
            string pattern = @"^[a-zA-Z0-9\-._~:/?#\[\]@!$&'()*+,;=%\s]*$";
            if (string.IsNullOrEmpty(queryRestrictions) || !Regex.IsMatch(queryRestrictions, pattern))
                return false;
            if (offset < 0 || maxPerPage <= 0)
                return false;
            return true;
        }

    }
}