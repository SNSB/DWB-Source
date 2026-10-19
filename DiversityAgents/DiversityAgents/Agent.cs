using System;
using System.Collections.Generic;
using System.Text;
using DiversityWorkbench;

namespace DiversityAgents
{
    public class Agent : DiversityWorkbench.Agent
    {

        #region Parameter

        public static string SqlAgent = "SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName,  " +
            "GivenNamePostfix, InheritedNamePrefix, InheritedName, InheritedNamePostfix, Abbreviation, AgentType, AgentRole, " +
            "AgentGender, Description, OriginalSpelling, Notes,  " +
            "ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay,  " +
            "ValidUntilMonth, ValidUntilYear, ValidDateSupplement, PlaceOfBirth, PlaceOfDeath,  " +
            "SynonymToAgentID, SynonymisationType, RevisionLevel, DataWithholdingReason, IgnoreButKeepForReference, ContentLanguage " +
            "FROM Agent ";

        public static string SqlAgentContactInformation = "SELECT AgentID, DisplayOrder, AddressType, Country, City, " +
            "PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, " +
            "ValidFrom, ValidUntil, DataWithholdingReason " +
            "FROM AgentContactInformation ";

        public static string SqlAgentAddress = "SELECT AgentID, DisplayOrder, AddressType, Country, City, " +
            "PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, " +
            "ValidFrom, ValidUntil " +
            "FROM dbo.AgentAddress ";

        public static string SqlAgentKeyword = "SELECT AgentID, Keyword " +
            "FROM AgentKeyword ";

        public static string SqlAgentImage = "SELECT AgentID, URI, Description, Type, Sequence, DataWithholdingReason " +
            "FROM AgentImage ";

        public static string SqlAgentProject = "SELECT AgentID, ProjectID " +
            "FROM AgentProject ";

        public static string SqlExternalDataList = "SELECT AgentExternalID.AgentID, AgentExternalDatabase.ExternalDatabaseName, AgentExternalDatabase.ExternalDatabaseID, AgentExternalID.ExternalAgentURI " +
            "FROM AgentExternalID INNER JOIN AgentExternalDatabase ON AgentExternalID.ExternalDatabaseID = AgentExternalDatabase.ExternalDatabaseID " ;

        public static string SqlAgentReference = "SELECT AgentID, ReferenceTitle, ReferenceURI, ReferenceDetails, " +
            "ContainsImage, ContainsReferencelist, Notes " +
            "FROM AgentReference";

        public static string SqlAgentRelation = "SELECT AgentID, RelatedAgentID, RelationType, Notes, DataWithholdingReason " +
            "FROM AgentRelation";

        public static string SqlAgentIdentifier = "SELECT AgentID, Identifier, IdentifierURI, Type, Notes " +
            "FROM AgentIdentifier";

        public static string SqlAgentDescriptor = "SELECT AgentID, DescriptorID, Descriptor, URL, DescriptorType " +
            "FROM AgentDescriptor";

        public static string SqlAgentHierarchyOther = "SELECT AgentID, HierarchyID, AgentParentID, ValidFrom, ValidUntil, Notes " +
            "FROM AgentHierarchyOther";

        #endregion

        #region Construction
        public Agent(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Resources

        public static void CopyWorkbenchDirectory()
        {
            System.Collections.Generic.List<string> FoldersForTransfer = new List<string>();
            FoldersForTransfer.Add("Import");
            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.CopyWorkbenchDirectory(FoldersForTransfer);
        }

        #endregion



    }
}
