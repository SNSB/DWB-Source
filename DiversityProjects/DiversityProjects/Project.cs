using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityProjects
{
    public class Project : DiversityWorkbench.Project
    {

        public static string SqlProject = "SELECT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, ProjectDescription, ProjectNotes, " +
            "ProjectVersion, ProjectURL, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol " +
            "FROM Project ";

        public static string SqlProjectFields = "ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, " +
            "ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol ";

        public static string SqlProjectAgent = "SELECT ProjectID, AgentName, AgentURI, AgentType, Notes, AgentSequence, ProjectAgentID " +
            "FROM ProjectAgent";

        public static string SqlProjectAgentRole = "SELECT ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID " +
            "FROM ProjectAgentRole";

        public static string SqlProjectResource = "SELECT ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes " +
            "FROM ProjectResource";

        public static string SqlProjectReference = "SELECT ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ReferenceType, IsCitation, URI, ReferenceCitation " +
            "FROM ProjectReference";

        public static string SqlProjectLicense = "SELECT LicenseID, ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, " +
            "LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, IPRHolderAgentURI, " +
            "CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes " +
            "FROM ProjectLicense";

        public static string SqlProjectIdentifier = "SELECT ProjectID, Identifier, Type, URL, Notes " +
            "FROM ProjectIdentifier";

        public static string SqlProjectDescriptor = "SELECT ID, ProjectID, Language, ElementID, [Content], ContentURI " +
            "FROM ProjectDescriptor";

        public static string SqlProjectArchive = "SELECT ProjectID, ArchiveID, ParentArchiveID, Format, MimeType, StorageLocation, ExternalStorageLocation, " +
            " FileSHA512Hash, FileSize, TransferDate, PublicationDate, DataWithholdingReason, Notes, Published " +
            "FROM ProjectArchive";

        public static string SqlProjectArchiveIdentifier = "SELECT ProjectID, ArchiveID, Identifier " +
            "FROM ProjectArchiveIdentifier";

        //public static string SqlProjectLabel = "SELECT ProjectID, Label " +
        //    "FROM ProjectLabel";

        //public static string SqlProjectKeyword = "SELECT ProjectID, Keyword " +
        //    "FROM ProjectKeyword";

        public static string SqlProjectIdentifierType = "SELECT Type, ParentType, URL, Description, InternalNotes " +
            "FROM ProjectIdentifierType";

        public static string SqlProjectSetting = "SELECT ProjectID, SettingID, Value " +
            "FROM ProjectSetting";

        public static string SqlSetting = "SELECT SettingID, ParentSettingID, DisplayText, Description " +
            "FROM Setting";

        //public static string SqlApplication = "SELECT Application FROM Application";
        //public static string SqlProjectApplication = "SELECT ProjectID, Application, SingleProject FROM ProjectApplication";

        #region Construction
        public Project(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        #endregion

    }
}
