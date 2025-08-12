using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace DWBServices.WebServices.TaxonomicServices.Mycobank
{
    public class MycobankEntity : TaxonomicEntity
    {
            public Recorddetails RecordDetails { get; set; }
            public class Recorddetails
            {
                public Mycobank MycoBank { get; set; }
                public AuthorsM Authors { get; set; }
                public AuthorsAbbreviated Authorsabbreviated { get; set; }
                public CommentOnNameStatus Commentonnamestatus { get; set; }
                public Bibliography Bibliography { get; set; }
                public Classification Classification { get; set; }
                public Description Description { get; set; }
                public Etymology Etymology { get; set; }
                public Gender Gender { get; set; }
                [JsonPropertyName("Rank")]
                public RankM Rank { get; set; }
                public Synonymy Synonymy { get; set; }
                public TypeName Typename { get; set; }
                public NameType Nametype { get; set; }
                public TypeOfOrganism Typeoforganism { get; set; }
                public YearOfEffectivePublication Yearofeffectivepublication { get; set; }
                public Literature Literature { get; set; }
                public Protolog Protolog { get; set; }
                public TypeSpecimenOrExType Typespecimenorextype { get; set; }
                public DateEntered Dateentered { get; set; }
                public DatePublic Datepublic { get; set; }
            }

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

            public class AuthorsM
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
                public object[] ChildValue { get; set; }
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
                public Name1 Name { get; set; }
                public object Summary { get; set; }
                public int RecordId { get; set; }
                public object TargetFieldValue { get; set; }
            }

            public class Name1
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
                public object[] Value { get; set; }
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

            public class RankM
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
                public object[] TaxonSynonymsRecords { get; set; }
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
                public Name2 Name { get; set; }
                public Summary1 Summary { get; set; }
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

            public class Protolog
            {
                public object[] Value { get; set; }
                public int FieldType { get; set; }
                public bool IsEmpty { get; set; }
                public object FieldName { get; set; }
                public object CreationDate { get; set; }
                public object RecordId { get; set; }
                public object TargetFieldKey { get; set; }
            }

            public class TypeSpecimenOrExType
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
                public Name3 Name { get; set; }
                public Summary2 Summary { get; set; }
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


            public override TaxonomicEntity? GetMappedApiEntityModel()
            {
                var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                    ?? throw new InvalidOperationException(
                                        "DwbServiceProviderAccessor.Instance is not initialized.");
                string DetailUriPrefix = configuration["Mycobank:Mycobank_DetailUri"];
                _URL = DetailUriPrefix + RecordDetails?.Synonymy?.SynInfo?.SelectedRecord?.RecordId ?? string.Empty;
                _DisplayText = RecordDetails?.Synonymy?.SynInfo?.SelectedRecord?.RecordName?? string.Empty;
                Taxon = RecordDetails?.Synonymy?.SynInfo?.SelectedRecord?.RecordName ?? string.Empty;
                TaxonNameSinAuthor = string.Empty;
                AcceptedName = RecordDetails?.Commentonnamestatus?.Value == "accepted" ? Taxon : string.Empty;
                Family = string.Empty; // TODO ??
                Order = string.Empty; // TODO
                Genus = string.Empty;
                Epithet = string.Empty; // TODO
                Rank = RecordDetails?.Rank.Value ?? string.Empty;
                CommonNames = string.Empty; // TODO
                Status = RecordDetails?.Commentonnamestatus?.Value ?? string.Empty;
                Authors = RecordDetails?.Authors?.Value ?? string.Empty; // TODO ?
                BasionymAuthors = string.Empty; // TODO ?
                CombiningAuthors = string.Empty; // TODO ?
                Kingdom = string.Empty;
                Subkingdom = string.Empty;
                Hierarchy = string.Empty;
                Unranked = string.Empty;
                Year = RecordDetails?.Yearofeffectivepublication?.Value ?? string.Empty;

                return this;
            }
    }
}
