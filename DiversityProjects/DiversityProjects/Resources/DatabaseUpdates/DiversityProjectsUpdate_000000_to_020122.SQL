
/****** Object:  DatabaseRole [DiversityWorkbenchAdministrator]    Script Date: 07.04.2021 12:53:05 ******/
CREATE ROLE [DiversityWorkbenchAdministrator]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchEditor]    Script Date: 07.04.2021 12:53:05 ******/
CREATE ROLE [DiversityWorkbenchEditor]
GO
/****** Object:  DatabaseRole [DiversityWorkbenchUser]    Script Date: 07.04.2021 12:53:05 ******/
CREATE ROLE [DiversityWorkbenchUser]
GO
ALTER ROLE [DiversityWorkbenchEditor] ADD MEMBER [DiversityWorkbenchAdministrator]
GO
ALTER ROLE [DiversityWorkbenchUser] ADD MEMBER [DiversityWorkbenchEditor]
GO
/****** Object:  UserDefinedFunction [dbo].[ApplicationSearchItemPrimaryKeyColumn]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationSearchItemPrimaryKeyColumn] (@TableName varchar(50))  
/* returns the name of the first column of the primary key. Items in the database (e.g. Collection habe a IdentityColumn as primary key */
RETURNS nvarchar(300)
AS  
BEGIN 
DECLARE @ColumnName nvarchar(300)

set @ColumnName =
(
SELECT      MAX(c.Column_Name) 
FROM         INFORMATION_SCHEMA.TABLE_CONSTRAINTS t INNER JOIN
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c ON t.Constraint_Name = c.Constraint_Name
WHERE     (t.Constraint_Type = 'Primary key') 
AND (t.Table_Name = @TableName)
)

RETURN @ColumnName
END





GO
/****** Object:  UserDefinedFunction [dbo].[BaseURL]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE ON [dbo].[BaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[DiversityWorkbenchModule]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[DiversityWorkbenchModule] ()   RETURNS nvarchar(50) AS BEGIN RETURN 'DiversityProjects' END  ; 

GO
GRANT EXECUTE ON [dbo].[DiversityWorkbenchModule] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[Hierarchy]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Hierarchy] (@Table varchar(50), @ID int)  
RETURNS @ItemList TABLE (ItemID int primary key,
   ParentID int NULL,
   Caption nchar(500) NULL)
/*Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.*/
AS
BEGIN

declare @TopID int

set @TopID = (select dbo.HierarchyTopID(@Table, @ID))

   DECLARE @TempItem TABLE (ID int primary key,
   ParentID int NULL,
   Caption nchar(500) NULL)

if @Table = 'Project' 	begin  INSERT @TempItem (ID, ParentID, Caption) SELECT ProjectID, ProjectParentID, ProjectTitle FROM dbo.Project WHERE ProjectID = @TopID end

insert into @TempItem select distinct ItemID,ParentID, Caption from dbo.HierarchyChildNodes (@Table, @TopID)

   -- copy to the result of the function the required columns
 INSERT @ItemList
   SELECT ID,  ParentID,  Caption
   FROM @TempItem  /**/
   RETURN
END





GO
GRANT SELECT ON [dbo].[Hierarchy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[HierarchyChildNodes]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[HierarchyChildNodes] (@Table varchar(50), @ID int)  
RETURNS @ItemList TABLE (ItemID int primary key,
   ParentID int NULL,
   Caption nchar(500) NULL)
/*Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.*/

AS
BEGIN

   declare @ParentID int

   DECLARE @TempItem TABLE (ID int primary key,
   ParentID int NULL,
   Caption nchar(500) NULL)

   if @Table = 'Project' 	begin  INSERT @TempItem (ID, ParentID, Caption) SELECT ProjectID, ProjectParentID, ProjectTitle FROM dbo.Project WHERE ProjectParentID = @ID end

   DECLARE HierarchyCursor  CURSOR for
   select ID from @TempItem

   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID

   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select * from dbo.HierarchyChildNodes (@Table, @ParentID) where ItemID not in (select ID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor

 INSERT @ItemList
   SELECT distinct ID,  ParentID,  Caption
   FROM @TempItem

   RETURN
END




GO
GRANT SELECT ON [dbo].[HierarchyChildNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[HierarchyTopID]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[HierarchyTopID] (@Table nvarchar(50), @ID int)
RETURNS varchar (11)
/*Returns the top ID within the hierarchy for a given ID from the specified Table.*/
AS
BEGIN
declare @TopID int
declare @i int

if @Table = 'Project' begin set @TopID = (select ProjectParentID from dbo.Project where ProjectID = @ID) end

if @Table = 'Project' begin set @i = (select count(*) from dbo.Project where ProjectID = @ID and not ProjectParentID is null) end


if (@TopID = null )
	set @TopID =  @ID
else
	begin
	while (@i > 0)
		begin
		if @Table = 'Project' begin set @ID = (select ProjectParentID from dbo.Project where ProjectID = @ID) end

		if @Table = 'Project' begin set @i = (select count(*) from dbo.Project where ProjectID = @ID and not ProjectParentID is null) end
		end
	set @TopID = @ID
	end

return @TopID
END


GO
/****** Object:  UserDefinedFunction [dbo].[Metadata_ABCD]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE  FUNCTION [dbo].[Metadata_ABCD] (@ProjectID int)  
RETURNS @Metadata TABLE (
 "ProjectID" integer NOT NULL,
  "ProjectTitleCode" character varying(254),
  "TaxonomicGroup" character varying(254),
  "DatasetGUID" character varying(254),
  "TechnicalContactName" character varying(254),
  "TechnicalContactEmail" character varying(254),
  "TechnicalContactPhone" character varying(254),
  "TechnicalContactAddress" character varying(254),
  "ContentContactName" character varying(254),
  "ContentContactEmail" character varying(254),
  "ContentContactPhone" character varying(254),
  "ContentContactAddress" character varying(254),
  "OtherProviderUDDI" character varying(254),
  "DatasetTitle" character varying(254),
  "DatasetDetails" character varying(254),
  "DatasetCoverage" character varying(254),
  "DatasetURI" character varying(254),
  "DatasetIconURI" character varying(254),
  "DatasetVersionMajor" character varying(254),
  "DatasetCreators" character varying(254),
  "DatasetContributors" character varying(254),
  "DateCreated" character varying(254),
  "DateModified" character varying(254),
  "SourceID" character varying(254),
  "SourceInstitutionID" character varying(254),
  "OwnerOrganizationName" character varying(254),
  "OwnerOrganizationAbbrev" character varying(254),
  "OwnerContactPerson" character varying(254),
  "OwnerContactRole" character varying(254),
  "OwnerAddress" character varying(254),
  "OwnerTelephone" character varying(254),
  "OwnerEmail" character varying(254),
  "OwnerURI" character varying(254),
  "OwnerLogoURI" character varying(254),
  "IPRText" character varying(254),
  "IPRDetails" character varying(254),
  "IPRURI" character varying(254),
  "CopyrightText" character varying(254),
  "CopyrightDetails" character varying(254),
  "CopyrightURI" character varying(254),
  "TermsOfUseText" character varying(254),
  "TermsOfUseDetails" character varying(254),
  "TermsOfUseURI" character varying(254),
  "DisclaimersText" character varying(254),
  "DisclaimersDetails" character varying(254),
  "DisclaimersURI" character varying(254),
  "LicenseText" character varying(254),
  "LicensesDetails" character varying(254),
  "LicenseURI" character varying(254),
  "AcknowledgementsText" character varying(254),
  "AcknowledgementsDetails" character varying(254),
  "AcknowledgementsURI" character varying(254),
  "CitationsText" character varying(254),
  "CitationsDetails" character varying(254),
  "CitationsURI" character varying(254),
  "RecordBasis" character varying(254),
  "KindOfUnit" character varying(254),
  "HigherTaxonRank" character varying(254),
  "RecordURI" character varying(254)
)

/*
Returns a table that lists all the metadata related to the given project as stored in DiversityProjects.
MW 20.01.2010

Test:
select * from [dbo].[ProjectSettings_ABCD] (3)
select * from [dbo].[Metadata] (3)
select * from [dbo].[Metadata] (400)
*/
AS
BEGIN

INSERT INTO @Metadata (ProjectID) VALUES (@ProjectID)

declare @ProjectSettings TABLE (
	[ProjectSetting] [nvarchar] (500) NOT NULL,
	[Value] [nvarchar] (Max) NULL)

INSERT INTO @ProjectSettings (ProjectSetting, Value)	
SELECT ProjectSetting, Value FROM [dbo].[ProjectSettings_ABCD] (@ProjectID)

UPDATE @Metadata SET ProjectTitleCode = (SELECT Project FROM DiversityProjects.dbo.Project WHERE ProjectID = @ProjectID)
UPDATE @Metadata SET TaxonomicGroup = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Scope.TaxonomicTerm')
UPDATE @Metadata SET DatasetGUID = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'DatasetGUID')
UPDATE @Metadata SET TechnicalContactName = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'TechnicalContact.TechnicalContactName')
UPDATE @Metadata SET TechnicalContactEmail = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'TechnicalContact.TechnicalContactEmail')
UPDATE @Metadata SET TechnicalContactPhone = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'TechnicalContact.TechnicalContactPhone')
UPDATE @Metadata SET TechnicalContactAddress = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'TechnicalContact.TechnicalContactAddress')
UPDATE @Metadata SET ContentContactName = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'ContentContact.ContentContactName')
UPDATE @Metadata SET ContentContactEmail = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'ContentContact.ContentContactEmail')
UPDATE @Metadata SET ContentContactPhone = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'ContentContact.ContentContactPhone')
UPDATE @Metadata SET ContentContactAddress = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'ContentContact.ContentContactAddress')
UPDATE @Metadata SET OtherProviderUDDI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'OtherProviderUDDI')
UPDATE @Metadata SET DatasetTitle = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Description.Representation.Title')
UPDATE @Metadata SET DatasetDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Description.Representation.Details')
UPDATE @Metadata SET DatasetCoverage = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Description.Representation.Coverage')
UPDATE @Metadata SET DatasetURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Description.Representation.URI')
UPDATE @Metadata SET DatasetIconURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IconURI')
UPDATE @Metadata SET DatasetVersionMajor = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Version.Major')
UPDATE @Metadata SET DatasetCreators = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RevisionData.Creators')
UPDATE @Metadata SET DatasetContributors = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RevisionData.Contributors')
UPDATE @Metadata SET DateCreated = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RevisionData.DateCreated')
UPDATE @Metadata SET DateModified = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RevisionData.DateModified')
UPDATE @Metadata SET SourceID = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Unit.SourceID')
UPDATE @Metadata SET SourceInstitutionID = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Unit.SourceInstitutionID')
UPDATE @Metadata SET OwnerOrganizationName = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.Organization.Name')
UPDATE @Metadata SET OwnerOrganizationAbbrev = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.Organization.Representation.Abbreviation')
UPDATE @Metadata SET OwnerContactPerson = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.Person.FullName')
UPDATE @Metadata SET OwnerContactRole = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.Roles.Role')
UPDATE @Metadata SET OwnerAddress = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.Addresses.Address')
UPDATE @Metadata SET OwnerTelephone = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.TelephoneNumbers.TelephoneNumber')
UPDATE @Metadata SET OwnerEmail = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.EmailAddresses.EmailAddress')
UPDATE @Metadata SET OwnerURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.URIs.URL')
UPDATE @Metadata SET OwnerLogoURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.Owner.LogoURI')
UPDATE @Metadata SET IPRText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Text')
UPDATE @Metadata SET IPRDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Details')
UPDATE @Metadata SET IPRURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.URI')
UPDATE @Metadata SET CopyrightText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Copyrights.Copyright.Text')
UPDATE @Metadata SET CopyrightDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Copyrights.Copyright.Details')
UPDATE @Metadata SET CopyrightURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Copyrights.Copyright.URI')
UPDATE @Metadata SET TermsOfUseText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Text')
UPDATE @Metadata SET TermsOfUseDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Details')
UPDATE @Metadata SET TermsOfUseURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.URI')
UPDATE @Metadata SET DisclaimersText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Disclaimers.Disclaimer.Text')
UPDATE @Metadata SET DisclaimersDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Disclaimers.Disclaimer.Details')
UPDATE @Metadata SET DisclaimersURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Disclaimers.Disclaimer.URI')
UPDATE @Metadata SET LicenseText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Licenses.License.Text')
UPDATE @Metadata SET LicensesDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Licenses.License.Details')
UPDATE @Metadata SET LicenseURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Licenses.License.URI')
UPDATE @Metadata SET AcknowledgementsText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.Text')
UPDATE @Metadata SET AcknowledgementsDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.Details')
UPDATE @Metadata SET AcknowledgementsURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.URI')
UPDATE @Metadata SET CitationsText = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Citations.Citation.Text')
UPDATE @Metadata SET CitationsDetails = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Citations.Citation.Details')
UPDATE @Metadata SET CitationsURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.IPRStatements.Citations.Citation.URI')
UPDATE @Metadata SET RecordBasis = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RecordBasis')
UPDATE @Metadata SET KindOfUnit = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.KindOfUnit')
UPDATE @Metadata SET HigherTaxonRank = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Unit.Gathering.Synecology.AssociatedTaxa.TaxonIdentified.HigherTaxa.HigherTaxon.HigherTaxonRank')
UPDATE @Metadata SET RecordURI = (SELECT Value FROM @ProjectSettings WHERE ProjectSetting = 'Metadata.RecordURI')


-- set defaults for missing values

update @Metadata set DateCreated = 
(select convert(varchar(10), min(LogCreatedWhen), 104) 
from DiversityCollection.dbo.CollectionProject 
--from DiversityCollection_Monitoring.dbo.CollectionProject 
where ProjectID = @ProjectID)
from @Metadata where DateCreated = ''

update @Metadata set DateModified = 
(select convert(varchar(10), max(LogUpdatedWhen), 104) 
from DiversityCollection.dbo.CollectionProject 
--from DiversityCollection_Monitoring.dbo.CollectionProject 
where ProjectID = @ProjectID)
from @Metadata where DateModified = ''

update @Metadata set IPRText = '-' where IPRText = ''
update @Metadata set CopyrightText = '-' where CopyrightText = ''
update @Metadata set TermsOfUseText = '-' where TermsOfUseText = ''
update @Metadata set DisclaimersText = '-' where DisclaimersText = ''
update @Metadata set LicenseText = '-' where LicenseText = ''
update @Metadata set AcknowledgementsText = '-' where AcknowledgementsText = ''
update @Metadata set CitationsText = '-' where CitationsText = ''

DECLARE @UNDEFINED varchar(20)
set @UNDEFINED = '###### UNDEFINED ######'
update @Metadata set RecordBasis = @UNDEFINED where RecordBasis = '' or RecordBasis is null
UPDATE @Metadata SET ProjectTitleCode = @UNDEFINED where ProjectTitleCode = '' or ProjectTitleCode is null
UPDATE @Metadata SET TaxonomicGroup = @UNDEFINED where TaxonomicGroup = '' or TaxonomicGroup is null
UPDATE @Metadata SET DatasetGUID = @UNDEFINED where DatasetGUID = '' or DatasetGUID is null
UPDATE @Metadata SET TechnicalContactName = @UNDEFINED where TechnicalContactName = '' or TechnicalContactName is null
UPDATE @Metadata SET TechnicalContactEmail = @UNDEFINED where TechnicalContactEmail = '' or TechnicalContactEmail is null
UPDATE @Metadata SET TechnicalContactPhone = @UNDEFINED where TechnicalContactPhone = '' or TechnicalContactPhone is null
UPDATE @Metadata SET TechnicalContactAddress = @UNDEFINED where TechnicalContactAddress = '' or TechnicalContactAddress is null
UPDATE @Metadata SET ContentContactName = @UNDEFINED where ContentContactName = '' or ContentContactName is null
UPDATE @Metadata SET ContentContactEmail = @UNDEFINED where ContentContactEmail = '' or ContentContactEmail is null
UPDATE @Metadata SET ContentContactPhone = @UNDEFINED where ContentContactPhone = '' or ContentContactPhone is null
UPDATE @Metadata SET ContentContactAddress = @UNDEFINED where ContentContactAddress = '' or ContentContactAddress is null
UPDATE @Metadata SET OtherProviderUDDI = @UNDEFINED where OtherProviderUDDI = '' or OtherProviderUDDI is null
UPDATE @Metadata SET DatasetTitle = @UNDEFINED where DatasetTitle = '' or DatasetTitle is null
UPDATE @Metadata SET DatasetDetails = @UNDEFINED where DatasetDetails = '' or DatasetDetails is null
UPDATE @Metadata SET DatasetCoverage = @UNDEFINED where DatasetCoverage = '' or DatasetCoverage is null
UPDATE @Metadata SET DatasetURI = @UNDEFINED where DatasetURI = '' or DatasetURI is null
UPDATE @Metadata SET DatasetIconURI = @UNDEFINED where DatasetIconURI = '' or DatasetIconURI is null
UPDATE @Metadata SET DatasetVersionMajor = @UNDEFINED where DatasetVersionMajor = '' or DatasetVersionMajor is null
UPDATE @Metadata SET DatasetCreators = @UNDEFINED where DatasetCreators = '' or DatasetCreators is null
UPDATE @Metadata SET DatasetContributors = @UNDEFINED where DatasetContributors = '' or DatasetContributors is null
UPDATE @Metadata SET DateCreated = @UNDEFINED where DateCreated = '' or DateCreated is null
UPDATE @Metadata SET DateModified = @UNDEFINED where DateModified = '' or DateModified is null
UPDATE @Metadata SET SourceID = @UNDEFINED where SourceID = '' or SourceID is null
UPDATE @Metadata SET SourceInstitutionID = @UNDEFINED where SourceInstitutionID = '' or SourceInstitutionID is null
UPDATE @Metadata SET OwnerOrganizationName = @UNDEFINED where OwnerOrganizationName = '' or OwnerOrganizationName is null
UPDATE @Metadata SET OwnerOrganizationAbbrev = @UNDEFINED where OwnerOrganizationAbbrev = '' or OwnerOrganizationAbbrev is null
UPDATE @Metadata SET OwnerContactPerson = @UNDEFINED where OwnerContactPerson = '' or OwnerContactPerson is null
UPDATE @Metadata SET OwnerContactRole = @UNDEFINED where OwnerContactRole = '' or OwnerContactRole is null
UPDATE @Metadata SET OwnerAddress = @UNDEFINED where OwnerAddress = '' or OwnerAddress is null
UPDATE @Metadata SET OwnerTelephone = @UNDEFINED where OwnerTelephone = '' or OwnerTelephone is null
UPDATE @Metadata SET OwnerEmail = @UNDEFINED where OwnerEmail = '' or OwnerEmail is null
UPDATE @Metadata SET OwnerURI = @UNDEFINED where OwnerURI = '' or OwnerURI is null
UPDATE @Metadata SET OwnerLogoURI = @UNDEFINED where OwnerLogoURI = '' or OwnerLogoURI is null
UPDATE @Metadata SET IPRText = @UNDEFINED where IPRText = '' or IPRText is null
UPDATE @Metadata SET IPRDetails = @UNDEFINED where IPRDetails = '' or IPRDetails is null
UPDATE @Metadata SET IPRURI = @UNDEFINED where IPRURI = '' or IPRURI is null
UPDATE @Metadata SET CopyrightText = @UNDEFINED where CopyrightText = '' or CopyrightText is null
UPDATE @Metadata SET CopyrightDetails = @UNDEFINED where CopyrightDetails = '' or CopyrightDetails is null
UPDATE @Metadata SET CopyrightURI = @UNDEFINED where CopyrightURI = '' or CopyrightURI is null
UPDATE @Metadata SET TermsOfUseText = @UNDEFINED where TermsOfUseText = '' or TermsOfUseText is null
UPDATE @Metadata SET TermsOfUseDetails = @UNDEFINED where TermsOfUseDetails = '' or TermsOfUseDetails is null
UPDATE @Metadata SET TermsOfUseURI = @UNDEFINED where TermsOfUseURI = '' or TermsOfUseURI is null
UPDATE @Metadata SET DisclaimersText = @UNDEFINED where DisclaimersText = '' or DisclaimersText is null
UPDATE @Metadata SET DisclaimersDetails = @UNDEFINED where DisclaimersDetails = '' or DisclaimersDetails is null
UPDATE @Metadata SET DisclaimersURI = @UNDEFINED where DisclaimersURI = '' or DisclaimersURI is null
UPDATE @Metadata SET LicenseText = @UNDEFINED where LicenseText = '' or LicenseText is null
UPDATE @Metadata SET LicensesDetails = @UNDEFINED where LicensesDetails = '' or LicensesDetails is null
UPDATE @Metadata SET LicenseURI = @UNDEFINED where LicenseURI = '' or LicenseURI is null
UPDATE @Metadata SET AcknowledgementsText = @UNDEFINED where AcknowledgementsText = '' or AcknowledgementsText is null
UPDATE @Metadata SET AcknowledgementsDetails = @UNDEFINED where AcknowledgementsDetails = '' or AcknowledgementsDetails is null
UPDATE @Metadata SET AcknowledgementsURI = @UNDEFINED where AcknowledgementsURI = '' or AcknowledgementsURI is null
UPDATE @Metadata SET CitationsText = @UNDEFINED where CitationsText = '' or CitationsText is null
UPDATE @Metadata SET CitationsDetails = @UNDEFINED where CitationsDetails = '' or CitationsDetails is null
UPDATE @Metadata SET CitationsURI = @UNDEFINED where CitationsURI = '' or CitationsURI is null
UPDATE @Metadata SET RecordBasis = @UNDEFINED where RecordBasis = '' or RecordBasis is null
UPDATE @Metadata SET KindOfUnit = @UNDEFINED where KindOfUnit = '' or KindOfUnit is null
UPDATE @Metadata SET HigherTaxonRank = @UNDEFINED where HigherTaxonRank = '' or HigherTaxonRank is null
UPDATE @Metadata SET RecordURI = @UNDEFINED where RecordURI = '' or RecordURI is null




   RETURN
END





















GO
/****** Object:  UserDefinedFunction [dbo].[PrivacyConsentInfo]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   Function PrivacyConsentInfo providing common information with the DiversityWorkbench  ######################
--#####################################################################################################################

CREATE FUNCTION [dbo].[PrivacyConsentInfo] () 
RETURNS varchar (900) 
AS 
BEGIN return 'http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_Data_in_DWB_Software' 
END;
GO
GRANT ALTER ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[PrivacyConsentInfo] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectChildNodes]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  FUNCTION [dbo].[ProjectChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
declare @i int;
/*
   */
INSERT @TempItem (  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
	SELECT  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
	FROM Project WHERE ProjectParentID = @ID 
   DECLARE HierarchyCursor  CURSOR for
   select ProjectID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
	select  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
	from dbo.ProjectChildNodes (@ParentID) where ProjectID not in (select ProjectID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor 
 INSERT @ItemList ( ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol) 
   SELECT distinct  ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
   FROM @TempItem ORDER BY ProjectTitle
   RETURN
END


GO
GRANT SELECT ON [dbo].[ProjectChildNodes] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectHierarchy]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  FUNCTION [dbo].[ProjectHierarchy] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL, 
	[ProjectType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[InternalDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[PublicDescription] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescriptionType] [nvarchar](50) COLLATE Latin1_General_CI_AS  NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL
)
/*
Returns a table that lists all the items related to the given item.
SELECT * FROM dbo.ProjectHierarchy(3)
MW 02.01.2006
*/
AS
BEGIN
declare @TopID int
declare @i int
set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 
set @i = (select count(*) from Project where ProjectID = @ProjectID)
if (@TopID is null )
	set @TopID =  @ProjectID
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
   INSERT @ProjectList
   SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol
   FROM Project
   WHERE Project.ProjectID = @TopID

   INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol)
   SELECT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectVersion, ProjectURL, ProjectType, InternalDescription, PublicDescription, ProjectDescriptionType, EmbargoDate, CreateArchive, ArchiveProtocol 
	FROM dbo.ProjectChildNodes (@TopID)
   RETURN
END


GO
GRANT SELECT ON [dbo].[ProjectHierarchy] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectList]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  FUNCTION [dbo].[ProjectList] ()  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL)
/*
Returns a table that lists all the projects available for a user
MW 18.12.2012
*/
AS
BEGIN
IF (SELECT PERMISSIONS(OBJECT_ID('Project'))&16) = 16
	BEGIN
		INSERT INTO @ProjectList
		SELECT ProjectID FROM Project
	END
ELSE
	BEGIN
		INSERT INTO @ProjectList
		SELECT [ProjectID]
		FROM [ProjectUser]
		WHERE LoginName = USER_NAME()
	END
RETURN
END
GO
GRANT SELECT ON [dbo].[ProjectList] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectParents]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  FUNCTION [dbo].[ProjectParents] (@ProjectID int)  
RETURNS @ProjectList TABLE (
	[ProjectID] [int]  Primary key  NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProjectTitle] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ProjectDescription] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectNotes] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectCopyright] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectRights] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectLicenseURI] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectVersion] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProjectURL] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[HierarchyLevel] int
)
/*
Returns a table that lists all the parents of the given project.
SELECT * FROM dbo.ProjectParents(55)
MW 09.02.2015
*/
AS
BEGIN
--declare @TopID int
declare @i int
declare @HierarchyLevel int
set @HierarchyLevel = 1

INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, HierarchyLevel)
SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, @HierarchyLevel
FROM Project
WHERE Project.ProjectID = @ProjectID

set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
while (@i > 0)
begin
	set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
	set @HierarchyLevel = @HierarchyLevel + 1
	INSERT @ProjectList (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
	ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, HierarchyLevel)
	SELECT DISTINCT ProjectID, ProjectParentID, Project, ProjectTitle, ProjectDescription, 
		ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, @HierarchyLevel
	FROM Project
	WHERE Project.ProjectID = @ProjectID
	set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
end

RETURN
END


GO
GRANT SELECT ON [dbo].[ProjectParents] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectSettings]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  FUNCTION [dbo].[ProjectSettings] (@ProjectID int)  
RETURNS @SettingList TABLE (
	[ProjectSetting] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar](4000) COLLATE Latin1_General_CI_AS NULL
)

/*
Returns a table that lists all the settings related to the given item.
MW 02.10.2009

Test:
select * from [dbo].[ProjectSettings] (3)
*/
AS
BEGIN

declare @ProjectList TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL
)

-- Test	
/*
insert into @SettingList (ProjectSetting, Value)
select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000))
from Project P
where P.ProjectID = @ProjectID
and not P.ProjectSettings is null
and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')
*/
insert into @ProjectList(ProjectID) values (@ProjectID)

-- getting the superior IDs
declare @TopID int
declare @i int

set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 

set @i = (select count(*) from Project where ProjectID = @ProjectID)

if (@TopID is null )
	begin
	set @TopID =  @ProjectID
	insert into @ProjectList(ProjectID) values (@TopID)
	end
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		insert into @ProjectList(ProjectID) values (@ProjectID)
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
	
	
	
declare @ParentID int 

DECLARE ProjectCursor CURSOR for select [ProjectID] from @ProjectList order by ID	
open ProjectCursor 	
FETCH next from ProjectCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
	BEGIN 	
		
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/archive/server', cast(P.ProjectSettings.query('/settings/media/images/archive/server') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/archive/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/archive/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/server')
	
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')



		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/server', cast(P.ProjectSettings.query('/settings/media/images/preview/server') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/server')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/directory', cast(P.ProjectSettings.query('/settings/media/images/preview/directory') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/directory')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/width', cast(P.ProjectSettings.query('/settings/media/images/preview/width') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/width') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/width') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/width')



		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/server', cast(P.ProjectSettings.query('/settings/media/images/web/server') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/server')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/directory', cast(P.ProjectSettings.query('/settings/media/images/web/directory') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/directory')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/width', cast(P.ProjectSettings.query('/settings/media/images/web/width') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/width') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/width') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/width')

	FETCH NEXT FROM ProjectCursor into @ParentID 	

	END 
CLOSE ProjectCursor 
DEALLOCATE ProjectCursor     


   RETURN
END


















GO
GRANT SELECT ON [dbo].[ProjectSettings] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectSettings_ABCD]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE  FUNCTION [dbo].[ProjectSettings_ABCD] (@ProjectID int)  
RETURNS @SettingList TABLE (
	[ProjectSetting] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar](MAX) COLLATE Latin1_General_CI_AS NULL
)

/*
Returns a table that lists all the default settings related to the given item.
MW 02.10.2009

Test:
select * from [dbo].[ProjectSettings_ABCD] (904)
select * from [dbo].[ProjectSettings_ABCD] (3)
*/
AS
BEGIN

declare @ProjectList TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL
)

-- Test	
/*
insert into @SettingList (ProjectSetting, Value)
select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000))
from Project P
where P.ProjectID = @ProjectID
and not P.ProjectSettings is null
and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')
*/
insert into @ProjectList(ProjectID) values (@ProjectID)

-- getting the superior IDs
declare @TopID int
declare @i int

set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 

set @i = (select count(*) from Project where ProjectID = @ProjectID)

if (@TopID is null )
	begin
	set @TopID =  @ProjectID
	insert into @ProjectList(ProjectID) values (@TopID)
	end
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		insert into @ProjectList(ProjectID) values (@ProjectID)
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
	
	
	
declare @ParentID int 
declare @Node nvarchar(500)
declare @ABCD nvarchar(500)

DECLARE ProjectCursor CURSOR for select [ProjectID] from @ProjectList order by ID	
open ProjectCursor 	
FETCH next from ProjectCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
	BEGIN 	
		
		set @ABCD = 'DatasetGUID'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/GUID)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/GUID') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/GUID') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Scope.TaxonomicTerm'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TaxonomicGroup)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TaxonomicGroup') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TaxonomicGroup') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
	
		set @ABCD = 'RecordURI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/RecordURI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/RecordURI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/RecordURI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
		
-- TechnicalContact

		set @ABCD = 'TechnicalContact.TechnicalContactName'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Name)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Name') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Name') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'TechnicalContact.TechnicalContactEmail'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Email)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Email') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Email') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'TechnicalContact.TechnicalContactPhone'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Phone)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Phone') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Phone') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'TechnicalContact.TechnicalContactAddress'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Address)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Address') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TechnicalContact/Address') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
		
-- ContentContact

		set @ABCD = 'ContentContact.ContentContactName'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/ContentContact/Name)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/ContentContact/Name') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/ContentContact/Name') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'ContentContact.ContentContactEmail'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/ContentContact/Email)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/ContentContact/Email') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/ContentContact/Email') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'ContentContact.ContentContactPhone'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/ContentContact/Phone)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/ContentContact/Phone') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/ContentContact/Phone') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'ContentContact.ContentContactAddress'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/ContentContact/Address)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/ContentContact/Address') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/ContentContact/Address') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
		
-- OtherProviderUDDI

		set @ABCD = 'OtherProviderUDDI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/OtherProviderUDDI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/OtherProviderUDDI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/OtherProviderUDDI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
		
-- Representation

		set @ABCD = 'Metadata.Description.Representation.Title'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/Title)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/Title') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/Title') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Description.Representation.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Description.Representation.Coverage'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/Coverage)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/Coverage') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/Coverage') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Description.Representation.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IconURI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/IconURI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/IconURI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/IconURI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Version.Major'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/VersionMajor)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/VersionMajor') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/VersionMajor') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)
		
-- RevisionData

		set @ABCD = 'Metadata.RevisionData.Creators'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/Creator)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/Creator') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/Creator') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.RevisionData.Contributors'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Dataset/Contributors)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Dataset/Contributors') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Dataset/Contributors') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.RevisionData.DateCreated'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Date/Created)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Date/Created') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Date/Created') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.RevisionData.DateModified'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Date/Modified)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Date/Modified') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Date/Modified') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Unit.SourceInstitutionID'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Source/InstitutionID)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Source/InstitutionID') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Source/InstitutionID') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Unit.SourceID'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Source/ID)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Source/ID') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Source/ID') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- Owner

		set @ABCD = 'Metadata.Owner.Organization.Name'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationName)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/OrganizationName') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/OrganizationName') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.Organization.Representation.Abbreviation'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationAbbrev)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/OrganizationAbbrev') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/OrganizationAbbrev') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.Person.FullName'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/ContactPerson)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/ContactPerson') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/ContactPerson') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.Roles.Role'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/ContactRole)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/ContactRole') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/ContactRole') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.Addresses.Address'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/Address)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/Address') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/Address') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.TelephoneNumbers.TelephoneNumber'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/Telephone)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/Telephone') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/Telephone') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.EmailAddresses.EmailAddress'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/Email)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/Email') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/Email') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.URIs.URL'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.Owner.LogoURI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Owner/LogoURI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Owner/LogoURI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Owner/LogoURI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- IPRDeclaration

		set @ABCD = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/IPR/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/IPR/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/IPR/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/IPR/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/IPR/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/IPR/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/IPR/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/IPR/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/IPR/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- Copyright

		set @ABCD = 'Metadata.IPRStatements.Copyrights.Copyright.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Copyright/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Copyright/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Copyright/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Copyrights.Copyright.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Copyright/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Copyright/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Copyright/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Copyrights.Copyright.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Copyright/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Copyright/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Copyright/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- TermsOfUse

		set @ABCD = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TermsOfUse/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TermsOfUse/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TermsOfUse/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TermsOfUse/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/TermsOfUse/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/TermsOfUse/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/TermsOfUse/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- Disclaimer

		set @ABCD = 'Metadata.IPRStatements.Disclaimers.Disclaimer.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Disclaimers/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Disclaimers/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Disclaimers/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Disclaimers.Disclaimer.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Disclaimers/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Disclaimers/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Disclaimers/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Disclaimers.Disclaimer.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Disclaimers/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Disclaimers/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Disclaimers/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- License

		set @ABCD = 'Metadata.IPRStatements.Licenses.License.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/License/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/License/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/License/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Licenses.License.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/License/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/License/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/License/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Licenses.License.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/License/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/License/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/License/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- Acknowledgements
 
		set @ABCD = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Acknowledgements/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Acknowledgements/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Acknowledgements/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Acknowledgements/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Acknowledgements.Acknowledgement.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Acknowledgements/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Acknowledgements/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Acknowledgements/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- Citations
 
		set @ABCD = 'Metadata.IPRStatements.Citations.Citation.Text'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Citations/Text)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Citations/Text') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Citations/Text') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Citations.Citation.Details'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Citations/Details)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Citations/Details') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Citations/Details') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.IPRStatements.Citations.Citation.URI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/Citations/URI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/Citations/URI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/Citations/URI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

-- RecordBasis etc.
 
		set @ABCD = 'Metadata.RecordBasis'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/RecordBasis)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/RecordBasis') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/RecordBasis') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.KindOfUnit'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/KindOfUnit)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/KindOfUnit') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/KindOfUnit') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Unit.Gathering.Synecology.AssociatedTaxa.TaxonIdentified.HigherTaxa.HigherTaxon.HigherTaxonRank'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/HigherTaxonRank)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/HigherTaxonRank') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/HigherTaxonRank') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)

		set @ABCD = 'Metadata.RecordURI'
		insert into @SettingList (ProjectSetting, Value)
		select @ABCD, cast(P.ProjectSettings.query('data(/settings/ABCD/RecordURI)') as nvarchar(500))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/ABCD/RecordURI') is null
		and cast(P.ProjectSettings.query('/settings/ABCD/RecordURI') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = @ABCD)


	FETCH NEXT FROM ProjectCursor into @ParentID 	

	END 
CLOSE ProjectCursor 
DEALLOCATE ProjectCursor     


   RETURN
END





















GO
GRANT SELECT ON [dbo].[ProjectSettings_ABCD] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectSettings_Defaults]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  FUNCTION [dbo].[ProjectSettings_Defaults] (@ProjectID int)  
RETURNS @SettingList TABLE (
	[ProjectSetting] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar](4000) COLLATE Latin1_General_CI_AS NULL
)

/*
Returns a table that lists all the default settings related to the given item.
MW 02.10.2009

Test:
select * from [dbo].[ProjectSettings_Defaults] (904)
*/
AS
BEGIN

declare @ProjectList TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL
)

-- Test	
/*
insert into @SettingList (ProjectSetting, Value)
select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000))
from Project P
where P.ProjectID = @ProjectID
and not P.ProjectSettings is null
and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')
*/
insert into @ProjectList(ProjectID) values (@ProjectID)

-- getting the superior IDs
declare @TopID int
declare @i int

set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 

set @i = (select count(*) from Project where ProjectID = @ProjectID)

if (@TopID is null )
	begin
	set @TopID =  @ProjectID
	insert into @ProjectList(ProjectID) values (@TopID)
	end
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		insert into @ProjectList(ProjectID) values (@ProjectID)
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
	
	
	
declare @ParentID int 

DECLARE ProjectCursor CURSOR for select [ProjectID] from @ProjectList order by ID	
open ProjectCursor 	
FETCH next from ProjectCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
	BEGIN 	
		
		insert into @SettingList (ProjectSetting, Value)
		select 'CollectionSpecimenImage.ImageType', cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenImage/ImageType)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/default/CollectionSpecimenImage/ImageType') is null
		and cast(P.ProjectSettings.query('/settings/default/CollectionSpecimenImage/ImageType') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'CollectionSpecimenImage.ImageType')
	
		insert into @SettingList (ProjectSetting, Value)
		select 'CollectionSpecimenImage.Notes', cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenImage/Notes)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/default/CollectionSpecimenImage/Notes') is null
		and cast(P.ProjectSettings.query('/settings/default/CollectionSpecimenImage/Notes') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'CollectionSpecimenImage.Notes')
	
		insert into @SettingList (ProjectSetting, Value)
		select 'IdentificationUnit.TaxonomicGroup', cast(P.ProjectSettings.query('data(/settings/default/IdentificationUnit/TaxonomicGroup)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/default/IdentificationUnit/TaxonomicGroup') is null
		and cast(P.ProjectSettings.query('data(/settings/default/IdentificationUnit/TaxonomicGroup)') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'IdentificationUnit.TaxonomicGroup')

		insert into @SettingList (ProjectSetting, Value)
		select 'CollectionSpecimenPart.CollectionID', cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/CollectionID)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/CollectionID)') is null
		and cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/CollectionID)') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'CollectionSpecimenPart.CollectionID')

		insert into @SettingList (ProjectSetting, Value)
		select 'CollectionSpecimenPart.MaterialCategory', cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/MaterialCategory)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/MaterialCategory)') is null
		and cast(P.ProjectSettings.query('data(/settings/default/CollectionSpecimenPart/MaterialCategory)') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'CollectionSpecimenPart.MaterialCategory')

		insert into @SettingList (ProjectSetting, Value)
		select 'IdentificationUnitAnalysis.AnalysisID', cast(P.ProjectSettings.query('data(/settings/default/IdentificationUnitAnalysis/AnalysisID)') as nvarchar(50))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('data(/settings/default/IdentificationUnitAnalysis/AnalysisID)') is null
		and cast(P.ProjectSettings.query('data(/settings/default/IdentificationUnitAnalysis/AnalysisID)') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = 'IdentificationUnitAnalysis.AnalysisID')


	FETCH NEXT FROM ProjectCursor into @ParentID 	

	END 
CLOSE ProjectCursor 
DEALLOCATE ProjectCursor     


   RETURN
END



















GO
GRANT SELECT ON [dbo].[ProjectSettings_Defaults] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[ProjectSettings_Media]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  FUNCTION [dbo].[ProjectSettings_Media] (@ProjectID int)  
RETURNS @SettingList TABLE (
	[ProjectSetting] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar](4000) COLLATE Latin1_General_CI_AS NULL
)

/*
Returns a table that lists all the settings related to the given item.
MW 02.10.2009

Test:
select * from [dbo].[ProjectSettings_Media] (3)
select * from [dbo].[ProjectSettings_Media] (904)
*/
AS
BEGIN

declare @ProjectList TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL
)

-- Test	
/*
insert into @SettingList (ProjectSetting, Value)
select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000))
from Project P
where P.ProjectID = @ProjectID
and not P.ProjectSettings is null
and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')
*/
insert into @ProjectList(ProjectID) values (@ProjectID)

-- getting the superior IDs
declare @TopID int
declare @i int

set @TopID = (select ProjectParentID from Project where ProjectID = @ProjectID) 

set @i = (select count(*) from Project where ProjectID = @ProjectID)

if (@TopID is null )
	begin
	set @TopID =  @ProjectID
	insert into @ProjectList(ProjectID) values (@TopID)
	end
else	
	begin
	while (@i > 0)
		begin
		set @ProjectID = (select ProjectParentID from Project where ProjectID = @ProjectID and not ProjectParentID is null) 
		insert into @ProjectList(ProjectID) values (@ProjectID)
		set @i = (select count(*) from Project where ProjectID = @ProjectID and not ProjectParentID is null)
		end
	set @TopID = @ProjectID
	end
	
	
	
declare @ParentID int 

DECLARE ProjectCursor CURSOR for select [ProjectID] from @ProjectList order by ID	
open ProjectCursor 	
FETCH next from ProjectCursor into @ParentID 	
WHILE @@FETCH_STATUS = 0 	
	BEGIN 	
	
		--ARCHIVE
		
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/archive/server', cast(P.ProjectSettings.query('data(/settings/media/images/archive/server)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/archive/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/archive/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/server')
	
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/archive/directory', cast(P.ProjectSettings.query('data(/settings/media/images/archive/directory)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/archive/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/archive/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/archive/directory')


		--PREVIEW
		
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/server', cast(P.ProjectSettings.query('data(/settings/media/images/preview/server)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/server')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/directory', cast(P.ProjectSettings.query('data(/settings/media/images/preview/directory)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/directory')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/preview/width', cast(P.ProjectSettings.query('data(/settings/media/images/preview/width)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/preview/width') is null
		and cast(P.ProjectSettings.query('/settings/media/images/preview/width') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/preview/width')


		-- WEB
		
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/server', cast(P.ProjectSettings.query('data(/settings/media/images/web/server)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/server')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/directory', cast(P.ProjectSettings.query('data(/settings/media/images/web/directory)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/directory')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/web/width', cast(P.ProjectSettings.query('data(/settings/media/images/web/width)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/web/width') is null
		and cast(P.ProjectSettings.query('/settings/media/images/web/width') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/web/width')
		
		
		-- TRANSFER
		
		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/transfer/server', cast(P.ProjectSettings.query('data(/settings/media/images/transfer/server)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/transfer/server') is null
		and cast(P.ProjectSettings.query('/settings/media/images/transfer/server') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/transfer/server')

		insert into @SettingList (ProjectSetting, Value)
		select '/settings/media/images/transfer/directory', cast(P.ProjectSettings.query('data(/settings/media/images/transfer/directory)') as nvarchar(4000))
		from Project P
		where P.ProjectID = @ParentID
		and not P.ProjectSettings is null
		and not P.ProjectSettings.query('/settings/media/images/transfer/directory') is null
		and cast(P.ProjectSettings.query('/settings/media/images/transfer/directory') as nvarchar(4000)) <> ''
		and not exists (select * from @SettingList where ProjectSetting = '/settings/media/images/transfer/directory')

		
		

	FETCH NEXT FROM ProjectCursor into @ParentID 	

	END 
CLOSE ProjectCursor 
DEALLOCATE ProjectCursor     


   RETURN
END




















GO
GRANT SELECT ON [dbo].[ProjectSettings_Media] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuColumns]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuColumns] (@TableName varchar(50))  
RETURNS @MenuList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   DataType varchar(50) not null,
   ColumnLength int null,
   ColumnDescription nvarchar(1000) null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null,
   MenuTab varchar(50) null)
/*Returns a result set that lists all the Tables and Collections connected to the querried item.*/
AS
BEGIN
/* inserting the Data into the table */
-- table variable to hold accumulated results
   DECLARE @ItemList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   DataType varchar(50) not null,
   ColumnLength int null,
   ColumnDescription nvarchar(1000) null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null,
   MenuTab varchar(50) null)
insert into @ItemList (TableName, ColumnName, DataType, ColumnLength, MenuTab)
SELECT     TOP 100 PERCENT dbo.sysobjects.name AS TableName, dbo.syscolumns.name AS ColumnName, dbo.systypes.name AS DataType, 
                      	dbo.syscolumns.length, case when dbo.sysobjects.name like 'Collection%' and len(dbo.sysobjects.name) > 10  then RTRIM( substring(dbo.sysobjects.name, 11, 50)) else 
		case when dbo.sysobjects.name like 'Identification%' then  RTRIM( 'Identification ' + lower(substring(dbo.sysobjects.name, 15, 50)))  else dbo.sysobjects.name end end
FROM         dbo.syscolumns INNER JOIN
                      dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN
                      dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE     (dbo.sysobjects.xtype = 'U')
		AND (dbo.systypes.name <> N'sysname') 
		AND (NOT (dbo.syscolumns.name LIKE N'%ID')) 
		AND (NOT (dbo.syscolumns.name LIKE N'%Sequence')) 
		AND (dbo.sysobjects.name IN (SELECT Tablename FROM dbo.SearchMenuInferiorTables (@TableName)) 
			OR dbo.sysobjects.name = @TableName 
			OR dbo.sysobjects.name IN (SELECT Tablename FROM dbo.SearchMenuSuperiorTables (@TableName)) )
		AND dbo.systypes.name IN ('char', 'nchar', 'varchar', 'nvarchar', 'text', 'ntext','bit', 'tinyint', 'smallint', 'int', 'bigint', 'real', 'float', 'money','smallmoney','decimal', 'numeric', 'datetime', 'smalldatetime')
ORDER BY dbo.sysobjects.name, dbo.syscolumns.name
/* update of the descriptions */
DECLARE @Table as varchar(50)
DECLARE @OldTable as varchar(50)
DECLARE @Column as varchar(50)
DECLARE @Description as nvarchar(1000)
DECLARE @fktable_name nvarchar(257) /* 2*128 + 1 */
DECLARE @fktable_id	int
DECLARE @fkfull_table_name	nvarchar(257) /* 2*128 + 1 */
DECLARE DescriptionCursor  CURSOR for
select TableName, ColumnName from @ItemList
open DescriptionCursor
FETCH next from DescriptionCursor into @Table, @Column
SET @OldTable = (select @Table)
WHILE @@FETCH_STATUS = 0
BEGIN
   -- This is executed as long as the previous fetch succeeds.
	set @Description = (SELECT     max(CONVERT(nvarchar(1000), [value]))
	FROM         ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, 'column', @Column) [::fn_listextendedproperty_1]) 
	update @ItemList set ColumnDescription = @Description where TableName = @Table and ColumnName = @Column
	/* setting the foreign-key values for lookup tables */
	IF  @OldTable <>  @Table
	BEGIN
		SET @fkfull_table_name = (SELECT quotename(@Table))
		SET @fktable_id = (SELECT object_id(@fkfull_table_name))
		UPDATE @ItemList SET ForeignKeyTable = convert(sysname,o1.name), ForeignKeyColumn = convert(sysname,c1.name)
		from @ItemList a, sysreferences r,  sysindexes i,
			sysobjects o1, sysobjects o2,
			syscolumns c1, syscolumns c2
		where	r.rkeyindid = i.indid
			AND r.rkeyid = i.id
			AND r.fkeyid = @fktable_id
			AND o1.id = r.rkeyid
			AND o2.id = r.fkeyid
			AND c1.id = r.rkeyid
			AND c2.id = r.fkeyid
			AND c1.colid = r.rkey1
			AND c2.colid = r.fkey1
			and o1.name like '%_Enum'
			AND convert(sysname,c2.name) = a.ColumnName
		/*print @Table
		print @OldTable
		print @Column*/
	END
	set @OldTable = (select @Table)
	
   	FETCH NEXT FROM DescriptionCursor into @Table, @Column
END
CLOSE DescriptionCursor
DEALLOCATE DescriptionCursor
/* transferring the data */
INSERT @MenuList
SELECT TableName,  ColumnName, DataType, ColumnLength, ColumnDescription, ForeignKeyTable, ForeignKeyColumn, MenuTab
FROM @ItemList
order by TableName, ColumnName
return
END

GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuForeignKeys]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuForeignKeys] (@TableName varchar(50))  
RETURNS @MenuList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null)
/*Returns a result set that lists all the Tables and Foreign keys connected to the querried item.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @ItemList TABLE (
   TableName varchar(50) not null,
   ColumnName varchar(50) not null,
   ForeignKeyTable varchar(50) null,
   ForeignKeyColumn varchar(50) null)
/* inserting the Data into the table */
insert into @ItemList (TableName, ColumnName)
SELECT     TOP 100 PERCENT dbo.sysobjects.name AS TableName, dbo.syscolumns.name AS ColumnName
FROM         dbo.syscolumns INNER JOIN
                   dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN
                   dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype
WHERE     (dbo.sysobjects.xtype = 'U') AND (dbo.systypes.name <> N'sysname') 
	AND (NOT (dbo.syscolumns.name LIKE N'%Sequence')) 
	AND (
		dbo.sysobjects.name IN (SELECT Tablename FROM SearchMenuSuperiorTables ( @TableName) )
		OR dbo.sysobjects.name IN (SELECT Tablename FROM SearchMenuInferiorTables ( @TableName) )
		OR dbo.sysobjects.name = @TableName 
		)/*# ApplicationCollSearchMenuTables - Austauschen gegen Funktion !!! #*/
ORDER BY dbo.sysobjects.name, dbo.syscolumns.name
/* update of the descriptions */
DECLARE @Table as varchar(50)
DECLARE @OldTable as varchar(50)
DECLARE @Column as varchar(50)
--DECLARE @Description as nvarchar(1000)
DECLARE @fktable_name nvarchar(257) /* 2*128 + 1 */
DECLARE @fktable_id	int
DECLARE @fkfull_table_name	nvarchar(257) /* 2*128 + 1 */
DECLARE DescriptionCursor  CURSOR for
select TableName, ColumnName from @ItemList
open DescriptionCursor
FETCH next from DescriptionCursor into @Table, @Column
SET @OldTable = (select @Table)
WHILE @@FETCH_STATUS = 0
BEGIN
   -- This is executed as long as the previous fetch succeeds.
	/* setting the foreign-key values for lookup tables */
	IF  @OldTable <>  @Table
	BEGIN
		SET @fkfull_table_name = (SELECT quotename(@Table))
		SET @fktable_id = (SELECT object_id(@fkfull_table_name))
		UPDATE @ItemList SET ForeignKeyTable = convert(sysname,o1.name), ForeignKeyColumn = convert(sysname,c1.name)
		from @ItemList a, sysreferences r,  sysindexes i,
			sysobjects o1, sysobjects o2,
			syscolumns c1, syscolumns c2
		where	r.rkeyindid = i.indid
			AND r.rkeyid = i.id
			AND r.fkeyid = @fktable_id
			AND o1.id = r.rkeyid
			AND o2.id = r.fkeyid
			AND c1.id = r.rkeyid
			AND c2.id = r.fkeyid
			AND c1.colid = r.rkey1
			AND c2.colid = r.fkey1
			and o1.name not like '%_Enum'
			AND convert(sysname,c2.name) = a.ColumnName
	END
	set @OldTable = (select @Table)
	
   	FETCH NEXT FROM DescriptionCursor into @Table, @Column
END
CLOSE DescriptionCursor
DEALLOCATE DescriptionCursor
/* selecting the data */
DELETE FROM @ItemList  WHERE ForeignKeyTable IS NULL
/* transferring data to result table */
insert into @MenuList (TableName, ColumnName,  ForeignKeyTable, ForeignKeyColumn)
SELECT     TableName, ColumnName,  ForeignKeyTable, ForeignKeyColumn FROM @ItemList
WHERE ForeignKeyTable  NOT LIKE '[_]%'
AND ForeignKeyTable <> TableName
return
END
GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuInferiorTables]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuInferiorTables] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null)
/* inserting the Data into the table */
insert into @TempList (TableName)
select i.Table_name from information_Schema.Table_Constraints s,  information_Schema.Table_Constraints i, information_Schema.Referential_Constraints r
where s.Constraint_Type = 'Primary key'
and i.Constraint_name = r.Constraint_name
and r.Unique_constraint_name = s.Constraint_name
and i.Table_Name not like '%Enum'
and i.Table_Name not like '[_]%'
and i.Table_Name <> s.Table_Name
and s.Table_name = @TableName
DECLARE @Table as varchar(50)
DECLARE TableCursor  CURSOR for
select TableName from @TempList
open TableCursor
FETCH next from TableCursor into @Table
WHILE @@FETCH_STATUS = 0
BEGIN
   	-- This is executed as long as the previous fetch succeeds.
	insert into @TempList (TableName) select TableName from SearchMenuInferiorTables (@Table)
   	FETCH NEXT FROM TableCursor into @Table
END
CLOSE TableCursor
DEALLOCATE TableCursor
--insert into @TempList (TableName) values (@TableName)
/* transferring data to result table */
insert into @TableList (TableName)
SELECT   distinct  TableName FROM @TempList
return
END
GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuOrderColumn]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuOrderColumn] (@TableName varchar(50))  
RETURNS @ColumnList TABLE (
   ColumnName varchar(50) not null)
/*Returns a result set that lists all the Tables and Collections connected to the querried item.*/
AS
BEGIN
/* inserting the Data into the table */
INSERT INTO @ColumnList (ColumnName)
SELECT Column_Name FROM information_Schema.Columns 
WHERE Table_Name = @TableName
AND Data_Type IN ('char', 'nchar', 'varchar', 'nvarchar', 'text', 'ntext','bit', 'tinyint', 'smallint', 'int', 'bigint', 'real', 'float', 'money','smallmoney','decimal', 'numeric', 'datetime', 'smalldatetime')
union select ''
ORDER BY Column_Name
return
END


GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuSuperiorTables]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuSuperiorTables] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null)
/* inserting the Data into the table */
insert into @TempList (TableName) 
select i.Table_name from information_Schema.Table_Constraints s,  information_Schema.Table_Constraints i, information_Schema.Referential_Constraints r
where s.Constraint_Type = 'Foreign key'
and s.Constraint_name = r.Constraint_name
and r.Unique_constraint_name = i.Constraint_name
and i.Table_Name not like '%Enum'
and i.Table_Name not like '[_]%'
and i.Table_Name <> s.Table_Name
and s.Table_name = @TableName
DECLARE @Table as varchar(50)
DECLARE TableCursor  CURSOR for
select TableName from @TempList
open TableCursor
FETCH next from TableCursor into @Table
WHILE @@FETCH_STATUS = 0
BEGIN
   	-- This is executed as long as the previous fetch succeeds.
	insert into @TempList (TableName) select TableName from SearchMenuSuperiorTables (@Table)
   	FETCH NEXT FROM TableCursor into @Table
END
CLOSE TableCursor
DEALLOCATE TableCursor
--insert into @TempList (TableName) values (@TableName)
/* transferring data to result table */
insert into @TableList (TableName)
SELECT   distinct  TableName FROM @TempList
return
END
GO
/****** Object:  UserDefinedFunction [dbo].[SearchMenuTableTabs]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SearchMenuTableTabs] (@TableName varchar(50))  
RETURNS @TableList TABLE (
   TableName varchar(50),
   TableTab nvarchar(50) )
/*Returns a result set that lists all the Tables which are chiild tables to the querried table.*/
AS
BEGIN
-- table variable to hold accumulated results
   DECLARE @TempList TABLE (
   TableName varchar(50) not null,
   TableTab nvarchar(50) null)
/* inserting the Data into the table */
insert into @TempList (TableName) select TableName from SearchMenuInferiorTables (@TableName)
insert into @TempList (TableName) select TableName from SearchMenuSuperiorTables (@TableName)
insert into @TempList (TableName) values (@TableName)
update @TempList set TableTab = case when TableName like 'Collection%' and len(TableName) > 10  then RTRIM( substring(TableName, 11, 50)) else 
		case when TableName like 'Identification%' then  RTRIM( 'Identification ' + lower(substring(TableName, 15, 50)))  else TableName end end
/* transferring data to result table */
insert into @TableList (TableName, TableTab)
SELECT   distinct  TableName, TableTab FROM @TempList
return
END
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyAll]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######    SettingHierarchyAll  ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[SettingHierarchyAll] ()  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL )
/*
Returns a table that lists all the settings including a display text with the whole hierarchy.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyAll()
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, DisplayText
	FROM Setting S
	WHERE S.ParentSettingID IS NULL
	ORDER BY DisplayOrder

	declare @i int
	set @i = (select count(*) from Setting where SettingID not IN (select SettingID from  @SettingList))
	while (@i > 0)
		begin
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
		SELECT DISTINCT S.SettingID, S.ParentSettingID, S.Description, S.DisplayText, S.DisplayOrder, L.Hierarchy + ' | ' + S.DisplayText
		FROM Setting S, @SettingList L
		WHERE S.ParentSettingID = L.SettingID
		AND S.SettingID NOT IN (select SettingID from  @SettingList)
		set @i = (select count(*) from Setting S where SettingID not IN (select SettingID from  @SettingList))
	end
	Declare @A int
    RETURN
END

GO
GRANT SELECT ON [dbo].[SettingHierarchyAll] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyChildren]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   SettingHierarchyChildren  ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[SettingHierarchyChildren] (@SettingID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings depending on a setting
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyChildren(4)
*/
AS
BEGIN
	-- adding the direct children
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
	FROM Setting AS H WHERE H.ParentSettingID = @SettingID
	ORDER BY DisplayOrder

	-- temporary table to keep the missing IDs
	declare @ID table ([SettingID] [int] Primary key)

	-- getting the IDs of the depending datasets missing so far
	insert into @ID 
	select distinct S.SettingID 
	from  @SettingList P, Setting S 
	where P.SettingID = S.ParentSettingID
	and S.SettingID not in (select SettingID from @ID)

	declare @i int
	set @i = (select count(*) from @ID)
	while (@i > 0)
	begin
		-- insert the missing children
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
		SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
		FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID
		AND L.SettingID NOT IN (SELECT SettingID FROM @SettingList)
		ORDER BY DisplayOrder

		-- clear the temporary table
		DELETE I FROM @ID I

		-- search for still missing children
		insert into @ID 
		select distinct S.SettingID 
		from  @SettingList P, Setting S 
		where P.SettingID = S.ParentSettingID
		and S.SettingID not in (select SettingID from @SettingList)

		-- count still missing children
		set @i = (select count(*) from @ID)
	end

    RETURN
END


GO
GRANT SELECT ON [dbo].[SettingHierarchyChildren] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyNonProject]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   SettingHierarchyNonProject  ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingHierarchyNonProject] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings including a display text with the whole hierarchy that are not included in a project so far.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyNonProject(3)
*/
AS
BEGIN
	-- insert all settings into the list
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	-- get all setting ID that are a parent of another setting
	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	DELETE L FROM @SettingList L 
	WHERE L.SettingID IN (SELECT P.SettingID FROM ProjectSetting P WHERE P.ProjectID = @ProjectID)
	AND L.SettingID NOT IN (select SettingID from @ID)

	RETURN
END

GO
GRANT SELECT ON [dbo].[SettingHierarchyNonProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyProject]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   SettingHierarchyProject  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[SettingHierarchyProject] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings including a display text with the whole hierarchy.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProject(3)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	UPDATE L SET Value = P.Value 
	FROM @SettingList L, ProjectSetting P
	WHERE L.SettingID = P.SettingID
	AND P.ProjectID = @ProjectID

	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	declare @i int
	set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	while (@i > 0)
	begin
		DELETE L FROM @SettingList L
		where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID)
		DELETE I FROM @ID I
		insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null
		set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	end
    RETURN
END

GO
GRANT SELECT ON [dbo].[SettingHierarchyProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyProjectAll]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######  SettingHierarchyProjectAll  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[SettingHierarchyProjectAll] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectID] int NULL,
	[Project] nvarchar(200) NULL)
/*
Returns a table that lists all the settings for a project including those inherited from parent projects.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProjectAll(55)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	DECLARE @ProjectParents table ([ProjectID] [int] Primary key,
	[Project] [nvarchar] (50) NULL,
	[HierarchyLevel] int,
	[DisplayOrder] [int] NULL)

	INSERT INTO @ProjectParents (ProjectID, Project, HierarchyLevel)
	SELECT ProjectID, Project, HierarchyLevel
	FROM dbo.ProjectParents(@ProjectID) P
	ORDER BY P.HierarchyLevel

	DECLARE @HierarchyLevel INT
	DECLARE @ParentProject int
	WHILE (SELECT COUNT(*) FROM @ProjectParents) > 0
	BEGIN
		SET @HierarchyLevel = (SELECT MIN(HierarchyLevel) FROM @ProjectParents)
		SET @ParentProject = (SELECT ProjectID FROM @ProjectParents WHERE HierarchyLevel = @HierarchyLevel)
		UPDATE L SET Value = S.Value, L.Project = PP.Project, L.ProjectID = PP.ProjectID
		FROM @SettingList L, ProjectSetting S, @ProjectParents PP
		WHERE L.SettingID = S.SettingID
		AND S.ProjectID = @ParentProject
		AND PP.ProjectID = @ParentProject
		AND L.ProjectID IS NULL
		DELETE PP FROM @ProjectParents PP WHERE PP.HierarchyLevel = @HierarchyLevel
	END

	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	declare @i int
	set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	while (@i > 0)
	begin
		DELETE L FROM @SettingList L
		where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID)
		DELETE I FROM @ID I
		insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null
		set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	end
    RETURN
END

GO
GRANT SELECT ON [dbo].[SettingHierarchyProjectAll] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingHierarchyProjectChildren]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   SettingHierarchyProjectChildren  ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingHierarchyProjectChildren] (@SettingID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings depending on a setting
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProjectChildren(3)
*/
AS
BEGIN
	-- adding the direct children
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
	FROM Setting AS H WHERE H.ParentSettingID = @SettingID

	-- temporary table to keep the missing IDs
	declare @ID table ([SettingID] [int] Primary key)

	-- getting the IDs of the depending datasets missing so far
	insert into @ID 
	select distinct S.SettingID 
	from  @SettingList P, Setting S 
	where P.SettingID = S.ParentSettingID
	and S.SettingID not in (select SettingID from @ID)


	declare @i int
	set @i = (select count(*) from @ID)
	while (@i > 0)
	begin
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
		SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
		FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID

		DELETE I FROM @ID I

		insert into @ID 
		select distinct S.SettingID 
		from  @SettingList P, Setting S 
		where P.SettingID = S.ParentSettingID
		and S.SettingID not in (select SettingID from @ID)

		set @i = (select count(*) from @ID)
	end

	--INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText)
	--SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText
	--FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID

    RETURN
END


GO
GRANT SELECT ON [dbo].[SettingHierarchyProjectChildren] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[SettingsForProject]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   [SettingsForProject]   ######################################################################################
--######   for external use by cache databases   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingsForProject] (@ProjectID int, @SettingFilter nvarchar(255), @Spacer nvarchar(20), @DisplayType int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings for a project including those inherited from parent projects.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingsForProject(55, '% ABCD %', '', 0)
SELECT * FROM DBO.SettingsForProject(55, '%ABCD | %', '.', 1)
SELECT * FROM DBO.SettingsForProject(55, '%ABCD | %', '.', 2)
SELECT * FROM DBO.SettingsForProject(3, '%ABCD | %', '.', 2)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, ProjectSetting, Value)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, Hierarchy, Value
	FROM dbo.SettingHierarchyProjectAll(@ProjectID) AS H

	IF @SettingFilter <> ''
	BEGIN
		DELETE L FROM @SettingList L
		WHERE L.ProjectSetting NOT LIKE @SettingFilter
	END

	UPDATE L SET L.Value = ''
	FROM @SettingList L
	WHERE L.Value IS NULL

	IF @DisplayType = 1
	BEGIN
		UPDATE L SET L.ProjectSetting = LTRIM(SUBSTRING(L.ProjectSetting, PATINDEX(@SettingFilter, L.ProjectSetting) , 4000))
		FROM @SettingList L
	END

	IF @DisplayType = 2
	BEGIN
		UPDATE L SET L.ProjectSetting = LTRIM(SUBSTRING(L.ProjectSetting, PATINDEX(@SettingFilter, L.ProjectSetting) + LEN(REPLACE(@SettingFilter, '%', '')) , 4000))
		FROM @SettingList L
	END


	IF @Spacer <> ''
	BEGIN
		UPDATE L SET L.ProjectSetting = REPLACE(L.ProjectSetting, ' | ', @Spacer)
		FROM @SettingList L
	END
    RETURN
END


GO
GRANT SELECT ON [dbo].[SettingsForProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[StableIdentifier]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######  StableIdentifier   ##########################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[StableIdentifier] (@ProjectID int)
RETURNS varchar (500)
/*
Returns the stable identifier for the project.
Test
select dbo.StableIdentifier (1234)
*/
AS
BEGIN
declare @StableIdentifier varchar(500)
set @StableIdentifier = ''
set @StableIdentifier = (select dbo.StableIdentifierBase() + cast(@ProjectID as varchar) from Project P WHERE P.ProjectID = @ProjectID)
return @StableIdentifier
END

GO
GRANT EXECUTE ON [dbo].[StableIdentifier] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[UserID]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE FUNCTION [dbo].[UserID] () RETURNS int AS 
BEGIN  
declare @ID int;  
SET @ID = (SELECT MIN(ID) FROM UserProxy U WHERE U.LoginName = SUSER_SNAME()) 
RETURN @ID  
END 
GO
GRANT EXECUTE ON [dbo].[UserID] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[Version]    Script Date: 07.04.2021 12:53:06 ******/
GRANT EXECUTE ON [dbo].[Version] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[VersionClient]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   [VersionClient]   ##########################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.03.02' END
GO
GRANT EXECUTE ON [dbo].[VersionClient] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Language_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language_Enum](
	[DisplayOrder] [int] NOT NULL,
	[Code] [varchar](5) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Language_Enum] PRIMARY KEY CLUSTERED 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[Language_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](255) NOT NULL,
	[ProjectTitle] [nvarchar](200) NULL,
	[ProjectType] [nvarchar](50) NULL,
	[ProjectDescription] [nvarchar](2000) NULL,
	[ProjectDescriptionType] [nvarchar](50) NULL,
	[PublicDescription] [nvarchar](max) NULL,
	[InternalDescription] [nvarchar](max) NULL,
	[ProjectNotes] [nvarchar](1000) NULL,
	[ProjectVersion] [nvarchar](255) NULL,
	[ProjectURL] [varchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[ProjectRights] [nvarchar](255) NULL,
	[ProjectEditors] [nvarchar](255) NULL,
	[ProjectInstitution] [nvarchar](500) NULL,
	[ProjectSettings] [xml] NULL,
	[ProjectCopyright] [nvarchar](255) NULL,
	[ProjectLicenseURI] [nvarchar](255) NULL,
	[EmbargoDate] [date] NULL,
	[CreateArchive] [bit] NULL,
	[ArchiveProtocol] [nvarchar](max) NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[Project] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[Project] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Project] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Project] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Project_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_log](
	[ProjectID] [int] NULL,
	[ProjectParentID] [int] NULL,
	[Project] [nvarchar](255) NOT NULL,
	[ProjectTitle] [nvarchar](200) NULL,
	[ProjectDescription] [nvarchar](2000) NULL,
	[ProjectEditors] [nvarchar](255) NULL,
	[ProjectNotes] [nvarchar](1000) NULL,
	[ProjectCopyright] [nvarchar](255) NULL,
	[ProjectVersion] [nvarchar](255) NULL,
	[ProjectSettings] [xml] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectRights] [nvarchar](255) NULL,
	[ProjectLicenseURI] [nvarchar](255) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[ProjectURL] [varchar](255) NULL,
	[ProjectType] [nvarchar](50) NULL,
	[InternalDescription] [nvarchar](max) NULL,
	[PublicDescription] [nvarchar](max) NULL,
	[ProjectDescriptionType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Project_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[Project_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Project_log] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgent]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgent](
	[ProjectAgentID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[AgentName] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NOT NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](50) NULL,
	[AgentSequence] [int] NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgent] PRIMARY KEY CLUSTERED 
(
	[ProjectAgentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectAgent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectAgent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectAgent] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectAgent] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgent_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgent_log](
	[ProjectID] [int] NULL,
	[AgentName] [nvarchar](255) NULL,
	[AgentURI] [varchar](255) NULL,
	[AgentRole] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentSequence] [int] NULL,
	[ProjectAgentID] [int] NULL,
 CONSTRAINT [PK_ProjectAgent_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectAgent_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectAgent_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgentRole]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgentRole](
	[ProjectAgentID] [int] NOT NULL,
	[AgentRole] [nvarchar](50) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[AgentName] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgentRole] PRIMARY KEY CLUSTERED 
(
	[ProjectAgentID] ASC,
	[AgentRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectAgentRole] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectAgentRole] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectAgentRole] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectAgentRole] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgentRole_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgentRole_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgentRole_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectAgentRole_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgentRole_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgentRole_log](
	[ProjectID] [int] NULL,
	[AgentName] [nvarchar](255) NULL,
	[AgentURI] [varchar](255) NULL,
	[AgentRole] [nvarchar](50) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectAgentID] [int] NULL,
 CONSTRAINT [PK_ProjectAgentRole_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectAgentRole_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectAgentRole_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectAgentType_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectAgentType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectAgentType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectAgentType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectArchive]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchive](
	[ProjectID] [int] NOT NULL,
	[ArchiveID] [int] IDENTITY(1,1) NOT NULL,
	[ParentArchiveID] [int] NULL,
	[Format] [nvarchar](50) NULL,
	[MimeType] [nvarchar](50) NULL,
	[StorageLocation] [nvarchar](500) NULL,
	[ExternalStorageLocation] [nvarchar](500) NULL,
	[FileSHA512Hash] [nvarchar](529) NULL,
	[FileSize] [int] NULL,
	[TransferDate] [datetime] NULL,
	[PublicationDate] [date] NULL,
	[DataWithholdingReason] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Published] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchive] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ArchiveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectArchive_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchive_log](
	[ProjectID] [int] NULL,
	[ArchiveID] [int] NULL,
	[ParentArchiveID] [int] NULL,
	[Format] [nvarchar](50) NULL,
	[MimeType] [nvarchar](50) NULL,
	[StorageLocation] [nvarchar](500) NULL,
	[ExternalStorageLocation] [nvarchar](500) NULL,
	[FileSHA512Hash] [nvarchar](529) NULL,
	[FileSize] [int] NULL,
	[TransferDate] [datetime] NULL,
	[PublicationDate] [date] NULL,
	[DataWithholdingReason] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[Published] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectArchive_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectArchiveFormat_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchiveFormat_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveFormat_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectArchiveIdentifier]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchiveIdentifier](
	[ProjectID] [int] NOT NULL,
	[ArchiveID] [int] NOT NULL,
	[Identifier] [nvarchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveIdentifier] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ArchiveID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectArchiveIdentifier_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchiveIdentifier_log](
	[ProjectID] [int] NULL,
	[ArchiveID] [int] NULL,
	[Identifier] [nvarchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectArchiveIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectArchiveMimeType_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectArchiveMimeType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectArchiveMimeType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectDescriptor]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectDescriptor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Language] [varchar](5) NOT NULL,
	[ElementID] [int] NOT NULL,
	[Content] [nvarchar](255) NOT NULL,
	[ContentURI] [varchar](500) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectDescriptor] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectDescriptor_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectDescriptor_log](
	[ID] [int] NULL,
	[ProjectID] [int] NULL,
	[Language] [varchar](5) NULL,
	[ElementID] [int] NULL,
	[Content] [nvarchar](255) NULL,
	[ContentURI] [varchar](500) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectDescriptor_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectDescriptor_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectDescriptor_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectDescriptorElement]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectDescriptorElement](
	[ElementID] [int] NOT NULL,
	[ParentElementID] [int] NULL,
	[Abbreviation] [nvarchar](25) NOT NULL,
	[DisplayText] [nvarchar](80) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[URL] [varchar](500) NOT NULL,
	[InternalNotes] [nvarchar](1000) NOT NULL,
	[DisplayEnabled] [bit] NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [ProjectDescriptorElement_PK] PRIMARY KEY CLUSTERED 
(
	[ElementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectDescriptorElement] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectDescriptorElement_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectDescriptorElement_log](
	[ElementID] [int] NULL,
	[ParentElementID] [int] NULL,
	[Abbreviation] [nvarchar](25) NULL,
	[DisplayText] [nvarchar](80) NULL,
	[DisplayOrder] [int] NULL,
	[Description] [nvarchar](1000) NULL,
	[URL] [varchar](500) NULL,
	[InternalNotes] [nvarchar](1000) NULL,
	[DisplayEnabled] [bit] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectDescriptorElement_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectDescriptorElement_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectDescriptorElement_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectIdentifier]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectIdentifier](
	[ProjectID] [int] NOT NULL,
	[Identifier] [nvarchar](255) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectIdentifier] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectIdentifier] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectIdentifier_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectIdentifier_log](
	[ProjectID] [int] NULL,
	[Identifier] [nvarchar](255) NULL,
	[Type] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectIdentifier_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectIdentifier_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectIdentifier_log] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectIdentifierType]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectIdentifierType](
	[Type] [nvarchar](50) NOT NULL,
	[ParentType] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectIdentifierType] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectIdentifierType] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectIdentifierType_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectIdentifierType_log](
	[Type] [nvarchar](50) NULL,
	[ParentType] [nvarchar](50) NULL,
	[URL] [varchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectIdentifierType_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectIdentifierType_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectIdentifierType_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectKeyword]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectKeyword](
	[ProjectID] [int] NOT NULL,
	[Keyword] [nvarchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectKeyword] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[Keyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectKeyword] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectKeyword] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectKeyword_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectKeyword_log](
	[ProjectID] [int] NULL,
	[Keyword] [nvarchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectKeyword_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectKeyword_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectKeyword_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectLabel]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectLabel](
	[ProjectID] [int] NOT NULL,
	[Label] [nvarchar](255) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectLabel] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[Label] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectLabel] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectLabel] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectLabel] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectLabel] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectLabel_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectLabel_log](
	[ProjectID] [int] NULL,
	[Label] [nvarchar](255) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectLabel_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectLabel_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectLabel_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectLicense]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectLicense](
	[LicenseID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[DisplayText] [nvarchar](200) NULL,
	[LicenseURI] [varchar](500) NULL,
	[LicenseType] [nvarchar](500) NULL,
	[LicenseHolder] [nvarchar](500) NULL,
	[LicenseHolderAgentURI] [varchar](500) NULL,
	[LicenseYear] [nvarchar](50) NULL,
	[Context] [nvarchar](500) NULL,
	[LicenseDetails] [nvarchar](500) NULL,
	[IPR] [nvarchar](500) NULL,
	[IPRHolder] [nvarchar](500) NULL,
	[IPRHolderAgentURI] [varchar](500) NULL,
	[CopyrightStatement] [nvarchar](500) NULL,
	[CopyrightHolder] [nvarchar](500) NULL,
	[CopyrightHolderAgentURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectLicense] PRIMARY KEY CLUSTERED 
(
	[LicenseID] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectLicense] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT DELETE ON [dbo].[ProjectLicense] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectLicense] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectLicense] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectLicense] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectLicense_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectLicense_log](
	[LicenseID] [int] NULL,
	[ProjectID] [int] NULL,
	[DisplayText] [nvarchar](200) NULL,
	[LicenseURI] [varchar](500) NULL,
	[LicenseType] [nvarchar](500) NULL,
	[LicenseHolder] [nvarchar](500) NULL,
	[LicenseHolderAgentURI] [varchar](500) NULL,
	[LicenseYear] [nvarchar](50) NULL,
	[Context] [nvarchar](500) NULL,
	[LicenseDetails] [nvarchar](500) NULL,
	[IPR] [nvarchar](500) NULL,
	[IPRHolder] [nvarchar](500) NULL,
	[IPRHolderAgentURI] [varchar](500) NULL,
	[CopyrightStatement] [nvarchar](500) NULL,
	[CopyrightHolder] [nvarchar](500) NULL,
	[CopyrightHolderAgentURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectLicense_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectLicense_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectLicense_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectReference]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectReference](
	[ProjectID] [int] NOT NULL,
	[ReferenceTitle] [nvarchar](500) NOT NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[ReferenceType] [nvarchar](255) NULL,
	[IsCitation] [bit] NOT NULL,
	[ReferenceCitation] [nvarchar](500) NULL,
	[URI] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectReference] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReferenceTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectReference] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectReference] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectReference_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectReference_log](
	[ProjectID] [int] NULL,
	[ReferenceTitle] [nvarchar](500) NOT NULL,
	[ReferenceURI] [varchar](255) NULL,
	[ReferenceDetails] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceType] [nvarchar](255) NULL,
	[IsCitation] [bit] NULL,
	[URI] [nvarchar](500) NULL,
	[ReferenceCitation] [nvarchar](500) NULL,
 CONSTRAINT [PK_ProjectReference_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectReference_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectReference_log] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectResource]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectResource](
	[ProjectID] [int] NOT NULL,
	[URI] [varchar](255) NOT NULL,
	[SpecificRights] [nvarchar](500) NULL,
	[SpecificLicenseURI] [nvarchar](500) NULL,
	[ResourceURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectResource] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[URI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectResource] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectResource] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectResource] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectResource] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectResource_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectResource_log](
	[ProjectID] [int] NULL,
	[URI] [varchar](255) NULL,
	[SpecificRights] [nvarchar](500) NULL,
	[SpecificLicenseURI] [nvarchar](500) NULL,
	[ResourceURI] [varchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogInsertedWhen] [datetime] NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectResource_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectResource_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectResource_log] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectSetting]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectSetting](
	[ProjectID] [int] NOT NULL,
	[SettingID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectSetting] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[SettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectSetting] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectSetting] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectSetting] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectSetting] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectSetting_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectSetting_log](
	[ProjectID] [int] NULL,
	[SettingID] [int] NULL,
	[Value] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectSetting_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[ProjectSetting_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectSetting_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectType_Enum]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT SELECT ON [dbo].[ProjectType_Enum] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[ProjectUser]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectUser](
	[LoginName] [nvarchar](50) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_ProjectUser] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ProjectUser] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[ProjectUser] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[ProjectUser] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[SettingID] [int] IDENTITY(1,1) NOT NULL,
	[ParentSettingID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DisplayOrder] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[Setting] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[Setting] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Setting] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[Setting] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Setting] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Setting] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  Table [dbo].[Setting_log]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting_log](
	[SettingID] [int] NULL,
	[ParentSettingID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DisplayOrder] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Setting_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT INSERT ON [dbo].[Setting_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
GRANT SELECT ON [dbo].[Setting_log] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Table [dbo].[UserProxy]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProxy](
	[LoginName] [nvarchar](50) NOT NULL,
	[CombinedNameCache] [nvarchar](255) NOT NULL,
	[AgentURI] [varchar](255) NULL,
	[Queries] [xml] NULL,
	[Settings] [xml] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PrivacyConsent] [bit] NULL,
	[PrivacyConsentDate] [datetime] NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_UserProxy] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
GRANT DELETE ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT INSERT ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] TO [DiversityWorkbenchAdministrator] AS [dbo]
GO
GRANT SELECT ON [dbo].[UserProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsent]) TO [DiversityWorkbenchUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[UserProxy] ([PrivacyConsentDate]) TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[Project_Core]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   Project_Core: adding ProjectType for use in linked server  #################################################
--#####################################################################################################################


CREATE VIEW [dbo].[Project_Core]
AS
SELECT     P.ProjectID, P.ProjectParentID, P.Project, P.ProjectTitle, P.ProjectType, P.ProjectDescription, P.ProjectNotes, P.ProjectCopyright, P.ProjectRights, P.ProjectLicenseURI, 
                      P.ProjectVersion, P.ProjectURL, cast(P.ProjectSettings as nvarchar(max)) AS ProjectSettings, P.LogInsertedBy, P.LogInsertedWhen, P.LogUpdatedBy, P.LogUpdatedWhen, P.RowGUID
FROM         dbo.Project AS P INNER JOIN
                      dbo.ProjectList() AS L ON P.ProjectID = L.ProjectID

GO
GRANT SELECT ON [dbo].[Project_Core] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ProjectProxy]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectProxy]
AS
SELECT     ProjectID, Project, dbo.BaseURL() + CAST(ProjectID AS varchar) AS ProjectURI
FROM         dbo.Project_Core
GO
GRANT SELECT ON [dbo].[ProjectProxy] TO [DiversityWorkbenchUser] AS [dbo]
GO

/****** Object:  View [dbo].[ProjectAgent_Core]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectAgent_Core]
AS
SELECT     A.ProjectID, A.AgentName, A.AgentURI, A.AgentRole, A.Notes, A.LogInsertedBy, A.LogInsertedWhen, A.LogUpdatedBy, A.LogUpdatedWhen, A.RowGUID
FROM         dbo.ProjectAgent AS A INNER JOIN
                      dbo.ProjectList() AS L ON A.ProjectID = L.ProjectID
GO
GRANT SELECT ON [dbo].[ProjectAgent_Core] TO [DiversityWorkbenchUser] AS [dbo]
GO

/****** Object:  View [dbo].[ProjectDefinition]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectDefinition]
AS
SELECT     P.ProjectID, P.Project AS ProjectCode, P.ProjectTitle, PP.Project AS SuperiorProject, P.ProjectDescription, 
                       P.ProjectNotes, P.ProjectVersion
FROM         dbo.Project AS P LEFT OUTER JOIN
                      dbo.Project AS PP ON P.ProjectParentID = PP.ProjectID
GO
GRANT SELECT ON [dbo].[ProjectDefinition] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ProjectResource_Core]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProjectResource_Core]
AS
SELECT     R.ProjectID, R.URI, R.SpecificRights, R.SpecificLicenseURI, R.ResourceURI, R.Notes, R.LogInsertedWhen, R.LogInsertedBy, R.LogUpdatedWhen, R.LogUpdatedBy, 
                      R.RowGUID
FROM         dbo.ProjectResource AS R INNER JOIN
                      dbo.ProjectList() AS L ON R.ProjectID = L.ProjectID
GO
GRANT SELECT ON [dbo].[ProjectResource_Core] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewBaseURL]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Adding ViewBaseURL for linked server   #####################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewBaseURL]
	AS
	SELECT        dbo.BaseURL() AS BaseURL
GO
GRANT SELECT ON [dbo].[ViewBaseURL] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewDataCitation]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewDataCitation] AS
SELECT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
      ,[URI]
      ,[ReferenceCitation]
  FROM [dbo].[ProjectReference] R where R.IsCitation = 1
GO
GRANT SELECT ON [dbo].[ViewDataCitation] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewDiversityWorkbenchModule]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Adding ViewDiversityWorkbenchModule for linked server   ####################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ViewDiversityWorkbenchModule]
AS
SELECT dbo.DiversityWorkbenchModule() AS DiversityWorkbenchModule
GO
GRANT SELECT ON [dbo].[ViewDiversityWorkbenchModule] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewLiteratureReference]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   ViewLiteratureReference    #################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[ViewLiteratureReference] AS
SELECT [ProjectID]
      ,[ReferenceTitle]
      ,[ReferenceURI]
      ,[ReferenceDetails]
      ,[ReferenceType]
      ,[Notes]
      ,[URI]
      ,[ReferenceCitation]
  FROM [dbo].[ProjectReference] R where R.IsCitation = 0

GO
GRANT SELECT ON [dbo].[ViewLiteratureReference] TO [DiversityWorkbenchUser] AS [dbo]
GO
/****** Object:  View [dbo].[ViewProject]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewProject]
AS
SELECT [ProjectID]
      ,[ProjectParentID]
      ,[Project]
      ,[ProjectTitle]
      ,[ProjectDescription]
      ,[ProjectNotes]
      ,[ProjectVersion]
      ,[ProjectURL]
	  ,dbo.StableIdentifier(ProjectID) as StableIdentifier
	  ,ProjectType
	  ,InternalDescription
	  ,PublicDescription
	  ,ProjectDescriptionType
      ,[LogInsertedBy]
      ,[LogInsertedWhen]
      ,[LogUpdatedBy]
      ,[LogUpdatedWhen]
      ,[RowGUID]
  FROM [dbo].[Project]
GO
GRANT SELECT ON [dbo].[ViewProject] TO [DiversityWorkbenchUser] AS [dbo]
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (0, N'--', N'not language specific/not applicable')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (1, N'?', N'unknown (not checked)')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (2, N'en', N'English')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (3, N'de', N'German')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (4, N'es', N'Spanish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (5, N'fr', N'French')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (6, N'ru', N'Russian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (7, N'ja', N'Japanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (8, N'zh', N'Chinese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (9, N'pt', N'Portuguese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (10, N'it', N'Italian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (11, N'la', N'Latin')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (12, N'ar', N'Arabic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (13, N'aa', N'Afar')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (14, N'ab', N'Abkhazian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (15, N'af', N'Afrikaans')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (16, N'am', N'Amharic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (17, N'as', N'Assamese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (18, N'ay', N'Aymara')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (19, N'az', N'Azerbaijani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (20, N'ba', N'Bashkir')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (21, N'be', N'Byelorussian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (22, N'bg', N'Bulgarian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (23, N'bh', N'Bihari')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (24, N'bi', N'Bislama')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (25, N'bn', N'Bengali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (26, N'bo', N'Tibetan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (27, N'br', N'Breton')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (28, N'ca', N'Catalan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (29, N'co', N'Corsican')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (30, N'cs', N'Czech')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (31, N'cy', N'Welsh')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (32, N'da', N'Danish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (33, N'dz', N'Bhutani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (34, N'el', N'Greek')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (35, N'eo', N'Esperanto')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (36, N'et', N'Estonian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (37, N'eu', N'Basque')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (38, N'fa', N'Persian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (39, N'fi', N'Finnish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (40, N'fj', N'Fiji')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (41, N'fo', N'Faeroese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (42, N'fy', N'Frisian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (43, N'ga', N'Irish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (44, N'gd', N'Scots')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (45, N'gl', N'Galician')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (46, N'gn', N'Guarani')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (47, N'gu', N'Gujarati')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (48, N'ha', N'Hausa')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (49, N'hi', N'Hindi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (50, N'hr', N'Croatian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (51, N'hu', N'Hungarian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (52, N'hy', N'Armenian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (53, N'ia', N'Interlingua')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (54, N'ie', N'Interlingue')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (55, N'ik', N'Inupiak')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (56, N'in', N'Indonesian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (57, N'is', N'Icelandic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (58, N'iw', N'Hebrew')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (59, N'ji', N'Yiddish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (60, N'jw', N'Javanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (61, N'ka', N'Georgian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (62, N'kk', N'Kazakh')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (63, N'kl', N'Greenlandic')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (64, N'km', N'Cambodian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (65, N'kn', N'Kannada')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (66, N'ko', N'Korean')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (67, N'ks', N'Kashmiri')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (68, N'ku', N'Kurdish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (69, N'ky', N'Kirghiz')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (70, N'ln', N'Lingala')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (71, N'lo', N'Laothian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (72, N'lt', N'Lithuanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (73, N'lv', N'Latvian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (74, N'mg', N'Malagasy')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (75, N'mi', N'Maori')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (76, N'mk', N'Macedonian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (77, N'ml', N'Malayalam')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (78, N'mn', N'Mongolian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (79, N'mo', N'Moldavian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (80, N'mr', N'Marathi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (81, N'ms', N'Malay')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (82, N'mt', N'Maltese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (83, N'my', N'Burmese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (84, N'na', N'Nauru')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (85, N'ne', N'Nepali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (86, N'nl', N'Dutch')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (87, N'no', N'Norwegian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (88, N'oc', N'Occitan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (89, N'or', N'Oriya')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (90, N'pa', N'Punjabi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (91, N'pl', N'Polish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (92, N'ps', N'Pashto')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (93, N'qu', N'Quechua')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (94, N'rm', N'Rhaeto-Romance')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (95, N'rn', N'Kirundi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (96, N'ro', N'Romanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (97, N'rw', N'Kinyarwanda')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (98, N'sa', N'Sanskrit')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (99, N'sd', N'Sindhi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (100, N'sg', N'Sangro')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (101, N'sh', N'Serbo-Croatian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (102, N'si', N'Singhalese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (103, N'sk', N'Slovak')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (104, N'sl', N'Slovenian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (105, N'sm', N'Samoan')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (106, N'sn', N'Shona')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (107, N'so', N'Somali')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (108, N'sq', N'Albanian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (109, N'sr', N'Serbian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (110, N'ss', N'Siswati')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (111, N'st', N'Sesotho')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (112, N'su', N'Sundanese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (113, N'sv', N'Swedish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (114, N'sw', N'Swahili')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (115, N'ta', N'Tamil')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (116, N'te', N'Tegulu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (117, N'tg', N'Tajik')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (118, N'th', N'Thai')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (119, N'ti', N'Tigrinya')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (120, N'tk', N'Turkmen')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (121, N'tl', N'Tagalog')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (122, N'tn', N'Setswana')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (123, N'to', N'Tonga')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (124, N'tr', N'Turkish')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (125, N'ts', N'Tsonga')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (126, N'tt', N'Tatar')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (127, N'tw', N'Twi')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (128, N'uk', N'Ukrainian')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (129, N'ur', N'Urdu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (130, N'uz', N'Uzbek')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (131, N'vi', N'Vietnamese')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (132, N'vo', N'Volapuk')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (133, N'wo', N'Wolof')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (134, N'xh', N'Xhosa')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (135, N'yo', N'Yoruba')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (136, N'zu', N'Zulu')
GO
INSERT [dbo].[Language_Enum] ([DisplayOrder], [Code], [Description]) VALUES (1000, N'(?)!', N'unknown (checked but still unknown)')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Author', N'Main researcher involved in producing the dataset or data package of a project, or the author of the (data) publication (may be a corporate/institutional or personal name)', N'Author/Creator', NULL, 1, NULL, NULL, N'cb885291-8509-e711-8145-b3764018fbe5')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Content Contact', N'Person with knowledge of how to access, troubleshoot, or otherwise field issues related to the content of the dataset or data package in a project', N'Content Contact', NULL, 1, NULL, NULL, N'0bf79c7f-8958-ea11-81b4-fbb8b86e7a51')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Data Curator', N'Person tasked with curating, reviewing, enhancing, cleaning, or standardizing the content of a dataset or data package managed in a project or submitted for storage, use, and maintenance within a data repository', N'Data Curator', NULL, NULL, NULL, NULL, N'ca885291-8509-e711-8145-b3764018fbe5')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Data Editor', N'Person (or organisation with a staff of data managers) responsible for maintaining the finished dataset or data package', N'Data Editor/Data Manager', 1, 1, NULL, NULL, N'a70435b3-b0a0-e411-80ee-f3f738bb71d7')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Data Owner Contact', N'Representative for person or institution owning or managing property rights, including intellectual property rights over the dataset or data package of a project', N'Data Owner Contact', NULL, 1, NULL, NULL, N'b4e8d333-b4f8-e411-80f9-8abb48ed3198')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Other', N'Person or institution making a significant contribution to the development and/or maintenance of the dataset or data package of a project, but whose contribution does not "fit"', N'Other', NULL, 1, NULL, NULL, N'826a968d-2b7e-ea11-81b8-81bb1dc2e7cf')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Principal Investigator', N'Person leading an investigation, research study or research group', N'Principal Investigator', NULL, 1, NULL, NULL, N'b5e8d333-b4f8-e411-80f9-8abb48ed3198')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Publisher', N'Organisation publishing  datasets or data packages', N'Data Publisher', NULL, 1, NULL, NULL, N'60f92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Researcher', N'Person involved in analyzing data or the results of an experiment or formal study. May indicate an intern or assistant to one of the authors who helped with research but who was not so “key” as to be listed as an author', N'Researcher', NULL, 1, NULL, NULL, N'b6e8d333-b4f8-e411-80f9-8abb48ed3198')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Source Institution', N'Organisation with license rights over the datasets or data packages of a project (might be identical with the data owner)', N'Data Source Organisation', NULL, 1, NULL, NULL, N'0df79c7f-8958-ea11-81b4-fbb8b86e7a51')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Submitter', N'Person submitting a dataset or data package to a data repository or data publisher', N'Submitter', NULL, 1, NULL, NULL, N'7836e36a-d43b-e811-816a-f7fced133562')
GO
INSERT [dbo].[ProjectAgentRole_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Technical Contact', N'Person or institution (e.g., data repository) with knowledge of how to access or troubleshoot technical issues related to the datasets or data packages of a project', N'Technical Contact', NULL, 1, NULL, NULL, N'0cf79c7f-8958-ea11-81b4-fbb8b86e7a51')
GO
INSERT [dbo].[ProjectAgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Institution', N'Institution', N'Institution', NULL, 1, NULL, NULL, N'b8e8d333-b4f8-e411-80f9-8abb48ed3198')
GO
INSERT [dbo].[ProjectAgentType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Person', N'Person', N'Person', NULL, 1, NULL, NULL, N'b7e8d333-b4f8-e411-80f9-8abb48ed3198')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (0, NULL, N'Any', N'Any concept (unspecified)', 0, N'The most general base concept of all other concepts', N'', N'The ID=0 may be used in client software and should not be changed. All other IDs may be freely changed.', 1, N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'd5a97ecd-614c-e811-816d-bbea3fbbbfad')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (1, NULL, N'Keyword', N'Keyword', 200, N'Keywords are essentially unspecified concepts and may be used so in imported data. For new data entry, keywords should be used if no more appropriate concept is present.', N'', N'', 1, N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'd6a97ecd-614c-e811-816d-bbea3fbbbfad')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (2, NULL, N'Label', N'Label', 240, N'', N'', N'', 1, N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2018-04-30T12:33:00' AS SmallDateTime), N'd7a97ecd-614c-e811-816d-bbea3fbbbfad')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (3, NULL, N'Geographical name', N'Geographical name', 300, N'A geographical or topographical name from DiversityGazetteers or a related webservice', N'', N'', 1, N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'64b55e2e-35ef-e911-81a7-fd9f925f4d1d')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (4, NULL, N'Term', N'Scientific term', 400, N'A scientific term or parameter from DiversityScientificTerms or a related webservice', N'', N'', 1, N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'65b55e2e-35ef-e911-81a7-fd9f925f4d1d')
GO
INSERT [dbo].[ProjectDescriptorElement] ([ElementID], [ParentElementID], [Abbreviation], [DisplayText], [DisplayOrder], [Description], [URL], [InternalNotes], [DisplayEnabled], [LogInsertedBy], [LogInsertedWhen], [LogUpdatedBy], [LogUpdatedWhen], [RowGUID]) VALUES (5, NULL, N'Taxon', N'Taxon name', 500, N'A taxonomic name from DiversityTaxonNames or a related webservice', N'', N'', 1, N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'DWB-DEVEL\mweiss', CAST(N'2019-10-15T12:19:00' AS SmallDateTime), N'66b55e2e-35ef-e911-81a7-fd9f925f4d1d')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Agents', N'Project for the module DiversityAgents', N'Agents', NULL, 1, NULL, NULL, N'57f92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Collection', N'Project for the module DiversityCollection', N'Collection', NULL, 1, NULL, NULL, N'58f92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Descriptions', N'Project for the module DiversityDescriptions', N'Descriptions', NULL, 1, NULL, NULL, N'59f92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Gazetteer', N'Project for the module DiversityGazetteer', N'Gazetteer', NULL, 1, NULL, NULL, N'5af92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'References', N'Project for the module DiversityReferences', N'References', NULL, 1, NULL, NULL, N'5bf92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'Regulation', N'Regulation as used e.g. in DiversityCollection', N'Regulation', NULL, 1, NULL, NULL, N'5ff92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'SamplingPlots', N'Project for the module DiversitySamplingPlots', N'SamplingPlots', NULL, 1, NULL, NULL, N'5cf92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'ScientificTerms', N'Project for the module DiversityScientificTerms', N'ScientificTerms', NULL, 1, NULL, NULL, N'5df92ff2-79b9-e711-815b-d69ae55ab63f')
GO
INSERT [dbo].[ProjectType_Enum] ([Code], [Description], [DisplayText], [DisplayOrder], [DisplayEnable], [InternalNotes], [ParentCode], [RowGUID]) VALUES (N'TaxonNames', N'Project for the module DiversityTaxonNames', N'TaxonNames', NULL, 1, NULL, NULL, N'5ef92ff2-79b9-e711-815b-d69ae55ab63f')
GO
SET IDENTITY_INSERT [dbo].[UserProxy] ON 
;
INSERT [dbo].[UserProxy] ([LoginName], [CombinedNameCache], [AgentURI], [Queries], [Settings], [ID], [PrivacyConsent], [PrivacyConsentDate], [RowGUID]) VALUES (N'vsanz', N'vsanz', NULL, NULL, NULL, 1, NULL, NULL, N'd86dab5c-9071-e411-80e5-ba523a4f32c2')
;
SET IDENTITY_INSERT [dbo].[UserProxy] OFF
GO


SET IDENTITY_INSERT [dbo].[Project] ON 
;
INSERT INTO Project (ProjectID, Project, ProjectTitle, ProjectDescription)
VALUES (0, 'DiversityWorkbench', 'DiversityWorkbench', 'Default project for the DiversityWorkbench')
;
SET IDENTITY_INSERT [dbo].[Project] OFF
GO

/****** Object:  Index [ProjectDescriptor_PK]    Script Date: 07.04.2021 12:53:06 ******/
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [ProjectDescriptor_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF__Project__LogInse__62AFA012]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF__Project__LogUpda__63A3C44B]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Project_log] ADD  CONSTRAINT [DF_Project_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[Project_log] ADD  CONSTRAINT [DF_Project_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[Project_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_AgentURI]  DEFAULT ('') FOR [AgentURI]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF__ProjectAg__LogIn__658C0CBD]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF__ProjectAg__LogUp__668030F6]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectAgent] ADD  CONSTRAINT [DF_ProjectAgent_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectAgent_log] ADD  CONSTRAINT [DF_ProjectAgent_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectAgent_log] ADD  CONSTRAINT [DF_ProjectAgent_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectAgent_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF__ProjectAg__LogIn__3296789C]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF__ProjectAg__LogUp__347EC10E]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectAgentRole] ADD  CONSTRAINT [DF_ProjectAgentRole_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectAgentRole_Enum] ADD  CONSTRAINT [DF_ProjectAgentRole_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectAgentRole_log] ADD  CONSTRAINT [DF_ProjectAgentRole_Log_LogUser]  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectAgentType_Enum] ADD  CONSTRAINT [DF_ProjectAgentType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_TransferDate]  DEFAULT (getdate()) FOR [TransferDate]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_Published]  DEFAULT ((0)) FOR [Published]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectArchive] ADD  CONSTRAINT [DF_ProjectArchive_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectArchive_log] ADD  CONSTRAINT [DF_ProjectArchive_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectArchiveFormat_Enum] ADD  CONSTRAINT [DF_ProjectArchiveFormat_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier_log] ADD  CONSTRAINT [DF_ProjectArchiveIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectArchiveMimeType_Enum] ADD  CONSTRAINT [DF_ProjectArchiveMimeType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('en') FOR [Language]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ((0)) FOR [ElementID]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('') FOR [Content]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  DEFAULT ('') FOR [ContentURI]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptor] ADD  CONSTRAINT [DF_ProjectDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectDescriptor_log] ADD  CONSTRAINT [DF_ProjectDescriptor_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectDescriptor_log] ADD  CONSTRAINT [DF_ProjectDescriptor_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectDescriptor_log] ADD  CONSTRAINT [DF_ProjectDescriptor_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Eleme__2A563856]  DEFAULT ('') FOR [Abbreviation]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Eleme__2B4A5C8F]  DEFAULT ('') FOR [DisplayText]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Displ__2C3E80C8]  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Eleme__2D32A501]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Eleme__2E26C93A]  DEFAULT ('') FOR [URL]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF__ProjectDe__Inter__2F1AED73]  DEFAULT ('') FOR [InternalNotes]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF_ProjectDescriptorElement_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF_ProjectDescriptorElement_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF_ProjectDescriptorElement_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF_ProjectDescriptorElement_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ADD  CONSTRAINT [DF_ProjectDescriptorElement_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement_log] ADD  CONSTRAINT [DF_ProjectDescriptorElement_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement_log] ADD  CONSTRAINT [DF_ProjectDescriptorElement_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectDescriptorElement_log] ADD  CONSTRAINT [DF_ProjectDescriptorElement_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectIdentifier] ADD  CONSTRAINT [DF_ProjectIdentifier_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectIdentifier_log] ADD  CONSTRAINT [DF_ProjectIdentifier_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogUpdatedWhen_1]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_LogUpdatedBy_1]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ADD  CONSTRAINT [DF_ProjectIdentifierType_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectIdentifierType_log] ADD  CONSTRAINT [DF_ProjectIdentifierType_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectKeyword] ADD  CONSTRAINT [DF_ProjectKeyword_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectKeyword] ADD  CONSTRAINT [DF_ProjectKeyword_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectKeyword] ADD  CONSTRAINT [DF_ProjectKeyword_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectKeyword] ADD  CONSTRAINT [DF_ProjectKeyword_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectKeyword] ADD  CONSTRAINT [DF_ProjectKeyword_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectKeyword_log] ADD  CONSTRAINT [DF_ProjectKeyword_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectLabel] ADD  CONSTRAINT [DF_ProjectLabel_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectLabel] ADD  CONSTRAINT [DF_ProjectLabel_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectLabel] ADD  CONSTRAINT [DF_ProjectLabel_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectLabel] ADD  CONSTRAINT [DF_ProjectLabel_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectLabel] ADD  CONSTRAINT [DF_ProjectLabel_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectLabel_log] ADD  CONSTRAINT [DF_ProjectLabel_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectLabel_log] ADD  CONSTRAINT [DF_ProjectLabel_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectLabel_log] ADD  CONSTRAINT [DF_ProjectLabel_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectLicense] ADD  CONSTRAINT [DF_ProjectLicense_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[ProjectLicense] ADD  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[ProjectLicense] ADD  CONSTRAINT [DF_ProjectLicense_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectLicense] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectLicense] ADD  CONSTRAINT [DF_ProjectLicense_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectLicense_log] ADD  CONSTRAINT [DF_ProjectLicense_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectLicense_log] ADD  CONSTRAINT [DF_ProjectLicense_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectLicense_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_IsCitation]  DEFAULT ((0)) FOR [IsCitation]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF__ProjectRe__LogIn__6B44E613]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF__ProjectRe__LogUp__6C390A4C]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectReference] ADD  CONSTRAINT [DF_ProjectReference_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectReference_log] ADD  CONSTRAINT [DF_ProjectReference_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectReference_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectResource] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectResource] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectResource] ADD  CONSTRAINT [DF_ProjectResource_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectResource_log] ADD  CONSTRAINT [DF_ProjectResource_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectResource_log] ADD  CONSTRAINT [DF_ProjectResource_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectResource_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_Value]  DEFAULT ('') FOR [Value]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectSetting_log] ADD  CONSTRAINT [DF_ProjectSetting_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[ProjectSetting_log] ADD  CONSTRAINT [DF_ProjectSetting_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[ProjectSetting_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[ProjectType_Enum] ADD  CONSTRAINT [DF_ProjectType_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[ProjectUser] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Setting] ADD  CONSTRAINT [DF_Setting_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]
GO
ALTER TABLE [dbo].[Setting] ADD  DEFAULT (suser_sname()) FOR [LogCreatedBy]
GO
ALTER TABLE [dbo].[Setting] ADD  CONSTRAINT [DF_Setting_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO
ALTER TABLE [dbo].[Setting] ADD  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO
ALTER TABLE [dbo].[Setting] ADD  CONSTRAINT [DF_Setting_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Setting_log] ADD  CONSTRAINT [DF_Setting_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO
ALTER TABLE [dbo].[Setting_log] ADD  CONSTRAINT [DF_Setting_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[Setting_log] ADD  DEFAULT (suser_sname()) FOR [LogUser]
GO
ALTER TABLE [dbo].[UserProxy] ADD  CONSTRAINT [DF__UserProxy__RowGU__4B58D01C]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Project] FOREIGN KEY([ProjectParentID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Project]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_ProjectType_Enum] FOREIGN KEY([ProjectType])
REFERENCES [dbo].[ProjectType_Enum] ([Code])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_ProjectType_Enum]
GO
ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectAgent] CHECK CONSTRAINT [FK_ProjectAgent_Project]
GO
ALTER TABLE [dbo].[ProjectAgent]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgent_ProjectAgentType_Enum] FOREIGN KEY([AgentType])
REFERENCES [dbo].[ProjectAgentType_Enum] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProjectAgent] CHECK CONSTRAINT [FK_ProjectAgent_ProjectAgentType_Enum]
GO
ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgent] FOREIGN KEY([ProjectAgentID])
REFERENCES [dbo].[ProjectAgent] ([ProjectAgentID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgent]
GO
ALTER TABLE [dbo].[ProjectAgentRole]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum] FOREIGN KEY([AgentRole])
REFERENCES [dbo].[ProjectAgentRole_Enum] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProjectAgentRole] CHECK CONSTRAINT [FK_ProjectAgentRole_ProjectAgentRole_Enum]
GO
ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_Project]
GO
ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchive] FOREIGN KEY([ProjectID], [ParentArchiveID])
REFERENCES [dbo].[ProjectArchive] ([ProjectID], [ArchiveID])
GO
ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchive]
GO
ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchiveFormat_Enum] FOREIGN KEY([Format])
REFERENCES [dbo].[ProjectArchiveFormat_Enum] ([Code])
GO
ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchiveFormat_Enum]
GO
ALTER TABLE [dbo].[ProjectArchive]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchive_ProjectArchiveMimeType_Enum] FOREIGN KEY([MimeType])
REFERENCES [dbo].[ProjectArchiveMimeType_Enum] ([Code])
GO
ALTER TABLE [dbo].[ProjectArchive] CHECK CONSTRAINT [FK_ProjectArchive_ProjectArchiveMimeType_Enum]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectArchive] FOREIGN KEY([ProjectID], [ArchiveID])
REFERENCES [dbo].[ProjectArchive] ([ProjectID], [ArchiveID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] CHECK CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectArchive]
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectIdentifier] FOREIGN KEY([ProjectID], [Identifier])
REFERENCES [dbo].[ProjectIdentifier] ([ProjectID], [Identifier])
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] CHECK CONSTRAINT [FK_ProjectArchiveIdentifier_ProjectIdentifier]
GO
ALTER TABLE [dbo].[ProjectDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDescriptor_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[ProjectDescriptor] CHECK CONSTRAINT [FK_ProjectDescriptor_Project]
GO
ALTER TABLE [dbo].[ProjectDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_ProjectDescriptor_ProjectDescriptorElement] FOREIGN KEY([ElementID])
REFERENCES [dbo].[ProjectDescriptorElement] ([ElementID])
GO
ALTER TABLE [dbo].[ProjectDescriptor] CHECK CONSTRAINT [FK_ProjectDescriptor_ProjectDescriptorElement]
GO
ALTER TABLE [dbo].[ProjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectIdentifier_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectIdentifier] CHECK CONSTRAINT [FK_ProjectIdentifier_Project]
GO
ALTER TABLE [dbo].[ProjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ProjectIdentifier_ProjectIdentifierType] FOREIGN KEY([Type])
REFERENCES [dbo].[ProjectIdentifierType] ([Type])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProjectIdentifier] CHECK CONSTRAINT [FK_ProjectIdentifier_ProjectIdentifierType]
GO
ALTER TABLE [dbo].[ProjectKeyword]  WITH CHECK ADD  CONSTRAINT [FK_ProjectKeyword_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectKeyword] CHECK CONSTRAINT [FK_ProjectKeyword_Project]
GO
ALTER TABLE [dbo].[ProjectLabel]  WITH CHECK ADD  CONSTRAINT [FK_ProjectLabel_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectLabel] CHECK CONSTRAINT [FK_ProjectLabel_Project]
GO
ALTER TABLE [dbo].[ProjectLicense]  WITH CHECK ADD  CONSTRAINT [FK_ProjectLicense_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[ProjectLicense] CHECK CONSTRAINT [FK_ProjectLicense_Project]
GO
ALTER TABLE [dbo].[ProjectReference]  WITH CHECK ADD  CONSTRAINT [FK_ProjectReference_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectReference] CHECK CONSTRAINT [FK_ProjectReference_Project]
GO
ALTER TABLE [dbo].[ProjectResource]  WITH CHECK ADD  CONSTRAINT [FK_ProjectResource_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectResource] CHECK CONSTRAINT [FK_ProjectResource_Project]
GO
ALTER TABLE [dbo].[ProjectSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectSetting_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectSetting] CHECK CONSTRAINT [FK_ProjectSetting_Project]
GO
ALTER TABLE [dbo].[ProjectSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectSetting_Setting] FOREIGN KEY([SettingID])
REFERENCES [dbo].[Setting] ([SettingID])
GO
ALTER TABLE [dbo].[ProjectSetting] CHECK CONSTRAINT [FK_ProjectSetting_Setting]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH CHECK ADD  CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_Project]
GO
ALTER TABLE [dbo].[ProjectUser]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectUser_UserProxy] FOREIGN KEY([LoginName])
REFERENCES [dbo].[UserProxy] ([LoginName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_UserProxy]
GO
ALTER TABLE [dbo].[Setting]  WITH CHECK ADD  CONSTRAINT [FK_Setting_Setting] FOREIGN KEY([ParentSettingID])
REFERENCES [dbo].[Setting] ([SettingID])
GO
ALTER TABLE [dbo].[Setting] CHECK CONSTRAINT [FK_Setting_Setting]
GO
/****** Object:  StoredProcedure [dbo].[procInsertProjectCopy]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   [procInsertProjectCopy]   ######################################################################################
--#####################################################################################################################

CREATE PROCEDURE [dbo].[procInsertProjectCopy] 
	(@ProjectID int output,
	 @CopyID int,
	 @ProjectTitle nvarchar  (200)  )
AS
BEGIN TRANSACTION
-- Insert Project
INSERT INTO Project (ProjectParentID, Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectURL, ProjectVersion)
SELECT ProjectParentID, substring(@ProjectTitle, 1, 50), @ProjectTitle, ProjectDescription, ProjectNotes, ProjectURL, ProjectVersion 
FROM Project WHERE ProjectID = @CopyID
SELECT @ProjectID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

-- Insert agents
INSERT INTO ProjectAgent(ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes)
SELECT @ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes
FROM ProjectAgent WHERE ProjectID = @CopyID

-- Insert Licenses
INSERT INTO ProjectLicense
	  (ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, 
IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes)
SELECT @ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, 
IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes
FROM            ProjectLicense WHERE ProjectID = @CopyID

-- Insert References
INSERT INTO ProjectReference
(ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes)
SELECT        @ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes
FROM            ProjectReference WHERE ProjectID = @CopyID

-- Insert Resources
INSERT INTO ProjectResource
(ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes)
SELECT        @ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes
FROM            ProjectResource WHERE ProjectID = @CopyID

-- Insert Settings
INSERT INTO ProjectSetting
(ProjectID, SettingID, Value)
SELECT        @ProjectID, SettingID, Value
FROM            ProjectSetting WHERE ProjectID = @CopyID

-- Insert User
INSERT INTO [dbo].[ProjectUser]
           ([LoginName]
           ,[ProjectID])
SELECT  USER_NAME(),  @ProjectID

COMMIT
SELECT @ProjectID

GO
GRANT EXECUTE ON [dbo].[procInsertProjectCopy] TO [DiversityWorkbenchEditor] AS [dbo]
GO
/****** Object:  Trigger [dbo].[trgDelProject]    Script Date: 07.04.2021 12:53:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######  Adaption of triggers for table Project    ###################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProject] ON [dbo].[Project] 
FOR DELETE AS 
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectDescriptionType, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectDescriptionType, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[Project] ENABLE TRIGGER [trgDelProject]
GO
/****** Object:  Trigger [dbo].[trgUpdProject]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdProject] ON [dbo].[Project] 
FOR UPDATE AS
INSERT INTO Project_Log (ProjectID, ProjectParentID, Project, ProjectTitle, ProjectType, InternalDescription, ProjectDescription, PublicDescription, ProjectDescriptionType, ProjectNotes, ProjectCopyright, ProjectRights, ProjectLicenseURI, ProjectVersion, ProjectURL, ProjectSettings, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen,  LogState) 
SELECT deleted.ProjectID, deleted.ProjectParentID, deleted.Project, deleted.ProjectTitle, deleted.ProjectType, deleted.InternalDescription, deleted.ProjectDescription, deleted.PublicDescription, deleted.ProjectDescriptionType, deleted.ProjectNotes, deleted.ProjectCopyright, deleted.ProjectRights, deleted.ProjectLicenseURI, deleted.ProjectVersion, deleted.ProjectURL, deleted.ProjectSettings, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen,  'U'
FROM DELETED
Update Project
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Project, deleted 
where 1 = 1 
AND Project.ProjectID = deleted.ProjectID
GO
ALTER TABLE [dbo].[Project] ENABLE TRIGGER [trgUpdProject]
GO
/****** Object:  Trigger [dbo].[trgDelProjectAgent]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   trgDelProjectAgent   #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectAgent] ON [dbo].[ProjectAgent] 
FOR DELETE AS 
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectAgent] ENABLE TRIGGER [trgDelProjectAgent]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectAgent]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   trgUpdProjectAgent   #######################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectAgent] ON [dbo].[ProjectAgent] 
FOR UPDATE AS
INSERT INTO ProjectAgent_Log (ProjectID, AgentName, AgentURI, AgentRole, AgentType, Notes, AgentSequence, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.AgentType, deleted.Notes, deleted.AgentSequence, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED
Update ProjectAgent
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM ProjectAgent, deleted 
where 1 = 1 
AND ProjectAgent.ProjectAgentID = deleted.ProjectAgentID
GO
ALTER TABLE [dbo].[ProjectAgent] ENABLE TRIGGER [trgUpdProjectAgent]
GO
/****** Object:  Trigger [dbo].[trgDelProjectAgentRole]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   trgDelProjectAgentRole   ###################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectAgentRole] ENABLE TRIGGER [trgDelProjectAgentRole]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectAgentRole]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   trgUpdProjectAgentRole   ###################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectAgentRole] ON [dbo].[ProjectAgentRole] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.7 */ 
/*  Date: 03.01.2018  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectAgentRole_Log (ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.AgentName, deleted.AgentURI, deleted.AgentRole, deleted.ProjectAgentID, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectAgentRole
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectAgentRole, deleted 
where 1 = 1 
AND ProjectAgentRole.ProjectAgentID = deleted.ProjectAgentID
AND ProjectAgentRole.AgentRole = deleted.AgentRole
GO
ALTER TABLE [dbo].[ProjectAgentRole] ENABLE TRIGGER [trgUpdProjectAgentRole]
GO
/****** Object:  Trigger [dbo].[trgDelProjectArchive]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgDelProjectArchive   #####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectArchive] ON [dbo].[ProjectArchive] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchive_Log (ProjectID, ArchiveID, ParentArchiveID, Format, MimeType, StorageLocation, ExternalStorageLocation, FileSHA512Hash, FileSize, TransferDate, PublicationDate, DataWithholdingReason, Notes, Published, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.ParentArchiveID, D.Format, D.MimeType, D.StorageLocation, D.ExternalStorageLocation, D.FileSHA512Hash, D.FileSize, D.TransferDate, D.PublicationDate, D.DataWithholdingReason, D.Notes, D.Published, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO
ALTER TABLE [dbo].[ProjectArchive] ENABLE TRIGGER [trgDelProjectArchive]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectArchive]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgUpdProjectArchive   #####################################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgUpdProjectArchive] ON [dbo].[ProjectArchive] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchive_Log (ProjectID, ArchiveID, ParentArchiveID, Format, MimeType, StorageLocation, ExternalStorageLocation, FileSHA512Hash, FileSize, TransferDate, PublicationDate, DataWithholdingReason, Notes, Published, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.ParentArchiveID, D.Format, D.MimeType, D.StorageLocation, D.ExternalStorageLocation, D.FileSHA512Hash, D.FileSize, D.TransferDate, D.PublicationDate, D.DataWithholdingReason, D.Notes, D.Published, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM ProjectArchive T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ArchiveID = D.ArchiveID
AND T.ProjectID = D.ProjectID
GO
ALTER TABLE [dbo].[ProjectArchive] ENABLE TRIGGER [trgUpdProjectArchive]
GO
/****** Object:  Trigger [dbo].[trgDelProjectArchiveIdentifier]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgDelProjectArchiveIdentifier   ###########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectArchiveIdentifier] ON [dbo].[ProjectArchiveIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchiveIdentifier_Log (ProjectID, ArchiveID, Identifier, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.Identifier, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'D'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ENABLE TRIGGER [trgDelProjectArchiveIdentifier]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectArchiveIdentifier]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgUpdProjectArchiveIdentifier   ###########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectArchiveIdentifier] ON [dbo].[ProjectArchiveIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  4.1.11 */ 
/*  Date: 21.02.2020  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectArchiveIdentifier_Log (ProjectID, ArchiveID, Identifier, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.ProjectID, D.ArchiveID, D.Identifier, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(U.ID as varchar) ,  'U'
FROM DELETED D, UserProxy U 
 WHERE U.LoginName = suser_sname()


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = U.ID 
FROM ProjectArchiveIdentifier T, deleted D, UserProxy U 
where U.LoginName = SUSER_NAME() 
AND T.ArchiveID = D.ArchiveID
AND T.Identifier = D.Identifier
AND T.ProjectID = D.ProjectID
GO
ALTER TABLE [dbo].[ProjectArchiveIdentifier] ENABLE TRIGGER [trgUpdProjectArchiveIdentifier]
GO
/****** Object:  Trigger [dbo].[trgDelProjectDescriptor]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgDelProjectDescriptor  ###################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectDescriptor] ON [dbo].[ProjectDescriptor] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 27.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptor_Log (ID, ProjectID, Language, ElementID, Content, ContentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ProjectID, deleted.Language, deleted.ElementID, deleted.Content, deleted.ContentURI, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectDescriptor] ENABLE TRIGGER [trgDelProjectDescriptor]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectDescriptor]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgUpdProjectDescriptor   ##################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectDescriptor] ON [dbo].[ProjectDescriptor] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 27.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptor_Log (ID, ProjectID, Language, ElementID, Content, ContentURI, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ID, deleted.ProjectID, deleted.Language, deleted.ElementID, deleted.Content, deleted.ContentURI, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM ProjectDescriptor T, deleted D 
where 1 = 1 
AND T.ID = D.ID
GO
ALTER TABLE [dbo].[ProjectDescriptor] ENABLE TRIGGER [trgUpdProjectDescriptor]
GO
/****** Object:  Trigger [dbo].[trgDelProjectDescriptorElement]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgDelProjectDescriptor  ###################################################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgDelProjectDescriptorElement] ON [dbo].[ProjectDescriptorElement] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 30.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptorElement_Log (ElementID, ParentElementID, Abbreviation, DisplayText, DisplayOrder, Description, URL, InternalNotes, DisplayEnabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ElementID, deleted.ParentElementID, deleted.Abbreviation, deleted.DisplayText, deleted.DisplayOrder, deleted.Description, deleted.URL, deleted.InternalNotes, deleted.DisplayEnabled, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ENABLE TRIGGER [trgDelProjectDescriptorElement]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectDescriptorElement]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   trgUpdProjectDescriptorElement   ###########################################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgUpdProjectDescriptorElement] ON [dbo].[ProjectDescriptorElement] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.3.0 */ 
/*  Date: 30.04.2018  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectDescriptorElement_Log (ElementID, ParentElementID, Abbreviation, DisplayText, DisplayOrder, Description, URL, InternalNotes, DisplayEnabled, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ElementID, deleted.ParentElementID, deleted.Abbreviation, deleted.DisplayText, deleted.DisplayOrder, deleted.Description, deleted.URL, deleted.InternalNotes, deleted.DisplayEnabled, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  LogUpdatedBy = SUSER_NAME()
FROM ProjectDescriptorElement T, deleted D 
where 1 = 1 
AND T.ElementID = D.ElementID
GO
ALTER TABLE [dbo].[ProjectDescriptorElement] ENABLE TRIGGER [trgUpdProjectDescriptorElement]
GO
/****** Object:  Trigger [dbo].[trgDelProjectIdentifier]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--#####################################################################################################################
--######   ProjectIdentifier Trigger   ################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectIdentifier]    Script Date: 10.06.2016 15:13:23 ******/
CREATE TRIGGER [dbo].[trgDelProjectIdentifier] ON [dbo].[ProjectIdentifier] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifier_Log (ProjectID, Identifier, Type, URL, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Identifier, deleted.Type, deleted.URL, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[ProjectIdentifier] ENABLE TRIGGER [trgDelProjectIdentifier]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectIdentifier]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Trigger [dbo].[trgUpdProjectIdentifier]    Script Date: 10.06.2016 15:13:37 ******/
CREATE TRIGGER [dbo].[trgUpdProjectIdentifier] ON [dbo].[ProjectIdentifier] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifier_Log (ProjectID, Identifier, Type, URL, Notes, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Identifier, deleted.Type, deleted.URL, deleted.Notes, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectIdentifier
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectIdentifier, deleted 
where 1 = 1 
AND ProjectIdentifier.Identifier = deleted.Identifier
AND ProjectIdentifier.ProjectID = deleted.ProjectID
GO
ALTER TABLE [dbo].[ProjectIdentifier] ENABLE TRIGGER [trgUpdProjectIdentifier]
GO
/****** Object:  Trigger [dbo].[trgDelProjectIdentifierType]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   ProjectIdentifierType Trigger   ############################################################################
--#####################################################################################################################


/****** Object:  Trigger [dbo].[trgDelProjectIdentifierType]    Script Date: 10.06.2016 15:09:53 ******/
CREATE TRIGGER [dbo].[trgDelProjectIdentifierType] ON [dbo].[ProjectIdentifierType] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[ProjectIdentifierType] ENABLE TRIGGER [trgDelProjectIdentifierType]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectIdentifierType]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Trigger [dbo].[trgUpdProjectIdentifierType]    Script Date: 10.06.2016 15:10:31 ******/
CREATE TRIGGER [dbo].[trgUpdProjectIdentifierType] ON [dbo].[ProjectIdentifierType] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectIdentifierType_Log (Type, ParentType, URL, Description, InternalNotes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Type, deleted.ParentType, deleted.URL, deleted.Description, deleted.InternalNotes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectIdentifierType
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectIdentifierType, deleted 
where 1 = 1 
AND ProjectIdentifierType.Type = deleted.Type
GO
ALTER TABLE [dbo].[ProjectIdentifierType] ENABLE TRIGGER [trgUpdProjectIdentifierType]
GO
/****** Object:  Trigger [dbo].[trgDelProjectKeyword]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--#####################################################################################################################
--######   trgDelProjectKeyword    ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelProjectKeyword] ON [dbo].[ProjectKeyword] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 08.08.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectKeyword_Log (ProjectID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[ProjectKeyword] ENABLE TRIGGER [trgDelProjectKeyword]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectKeyword]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   trgUpdProjectKeyword    ####################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdProjectKeyword] ON [dbo].[ProjectKeyword] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 08.08.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectKeyword_Log (ProjectID, Keyword, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Keyword, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update ProjectKeyword
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectKeyword, deleted 
where 1 = 1 
AND ProjectKeyword.Keyword = deleted.Keyword
AND ProjectKeyword.ProjectID = deleted.ProjectID

GO
ALTER TABLE [dbo].[ProjectKeyword] ENABLE TRIGGER [trgUpdProjectKeyword]
GO
/****** Object:  Trigger [dbo].[trgDelProjectLabel]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelProjectLabel] ON [dbo].[ProjectLabel] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 25.08.2017  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLabel_Log (ProjectID, Label, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Label, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectLabel] ENABLE TRIGGER [trgDelProjectLabel]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectLabel]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdProjectLabel] ON [dbo].[ProjectLabel] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.5 */ 
/*  Date: 25.08.2017  */ 
/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLabel_Log (ProjectID, Label, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.Label, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED
/* updating the logging columns */
Update ProjectLabel
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectLabel, deleted 
where 1 = 1 
AND ProjectLabel.Label = deleted.Label
AND ProjectLabel.ProjectID = deleted.ProjectID
GO
ALTER TABLE [dbo].[ProjectLabel] ENABLE TRIGGER [trgUpdProjectLabel]
GO
/****** Object:  Trigger [dbo].[trgDelProjectLicense]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Trigger  ######################################################################################
--#####################################################################################################################



/****** Object:  Trigger [dbo].[trgDelProjectLicense]    Script Date: 12.05.2015 12:07:47 ******/

CREATE TRIGGER [dbo].[trgDelProjectLicense] ON [dbo].[ProjectLicense] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.1.1 */ 
/*  Date: 11.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLicense_Log (LicenseID, ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.LicenseID, deleted.ProjectID, deleted.DisplayText, deleted.LicenseURI, deleted.LicenseType, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.Context, deleted.LicenseDetails, deleted.IPR, deleted.IPRHolder, deleted.IPRHolderAgentURI, deleted.CopyrightStatement, deleted.CopyrightHolder, deleted.CopyrightHolderAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[ProjectLicense] ENABLE TRIGGER [trgDelProjectLicense]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectLicense]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  Trigger [dbo].[trgUpdProjectLicense]    Script Date: 12.05.2015 12:08:10 ******/
CREATE TRIGGER [dbo].[trgUpdProjectLicense] ON [dbo].[ProjectLicense] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.1.1 */ 
/*  Date: 11.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectLicense_Log (LicenseID, ProjectID, DisplayText, LicenseURI, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, Context, LicenseDetails, IPR, IPRHolder, IPRHolderAgentURI, CopyrightStatement, CopyrightHolder, CopyrightHolderAgentURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.LicenseID, deleted.ProjectID, deleted.DisplayText, deleted.LicenseURI, deleted.LicenseType, deleted.LicenseHolder, deleted.LicenseHolderAgentURI, deleted.LicenseYear, deleted.Context, deleted.LicenseDetails, deleted.IPR, deleted.IPRHolder, deleted.IPRHolderAgentURI, deleted.CopyrightStatement, deleted.CopyrightHolder, deleted.CopyrightHolderAgentURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectLicense
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM ProjectLicense, deleted 
where 1 = 1 
AND ProjectLicense.LicenseID = deleted.LicenseID
AND ProjectLicense.ProjectID = deleted.ProjectID
GO
ALTER TABLE [dbo].[ProjectLicense] ENABLE TRIGGER [trgUpdProjectLicense]
GO
/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######  Adaption of trigger of table ProjectReference   #############################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectReference]    Script Date: 10.06.2016 15:05:24 ******/
CREATE TRIGGER [dbo].[trgDelProjectReference] ON [dbo].[ProjectReference] 
FOR DELETE AS 
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 
/*  changed: 21.07.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, ReferenceCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.ReferenceCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectReference] ENABLE TRIGGER [trgDelProjectReference]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Trigger [dbo].[trgUpdProjectReference]    Script Date: 10.06.2016 15:05:54 ******/
CREATE TRIGGER [dbo].[trgUpdProjectReference] ON [dbo].[ProjectReference] 
FOR UPDATE AS
/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.2 */ 
/*  Date: 10.06.2016  */ 
/*  changed: 21.07.2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO ProjectReference_Log (ProjectID, ReferenceTitle, ReferenceURI, ReferenceDetails, ReferenceType, Notes, URI, IsCitation, ReferenceCitation, LogInsertedWhen, LogInsertedBy, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.ReferenceType, deleted.Notes, deleted.URI, deleted.IsCitation, deleted.ReferenceCitation, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update ProjectReference
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectReference, deleted 
where 1 = 1 
AND ProjectReference.ProjectID = deleted.ProjectID
AND ProjectReference.ReferenceTitle = deleted.ReferenceTitle
GO
ALTER TABLE [dbo].[ProjectReference] ENABLE TRIGGER [trgUpdProjectReference]
GO
/****** Object:  Trigger [dbo].[trgDelProjectResource]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgDelProjectResource] ON [dbo].[ProjectResource] 
FOR DELETE AS 
INSERT INTO ProjectResource_Log (ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.URI, deleted.SpecificRights, deleted.SpecificLicenseURI, deleted.ResourceURI, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO
ALTER TABLE [dbo].[ProjectResource] ENABLE TRIGGER [trgDelProjectResource]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectResource]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgUpdProjectResource] ON [dbo].[ProjectResource] 
FOR UPDATE AS
INSERT INTO ProjectResource_Log (ProjectID, URI, SpecificRights, SpecificLicenseURI, ResourceURI, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.URI, deleted.SpecificRights, deleted.SpecificLicenseURI, deleted.ResourceURI, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update ProjectResource
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM ProjectResource, deleted 
where 1 = 1 
AND ProjectResource.ProjectID = deleted.ProjectID
AND ProjectResource.URI = deleted.URI
GO
ALTER TABLE [dbo].[ProjectResource] ENABLE TRIGGER [trgUpdProjectResource]
GO
/****** Object:  Trigger [dbo].[trgDelProjectSetting]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Trigger for ProjectSetting  ######################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectSetting]    Script Date: 12.05.2015 11:42:41 ******/

CREATE TRIGGER [dbo].[trgDelProjectSetting] ON [dbo].[ProjectSetting] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectSetting_Log (ProjectID, SettingID, Value, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.SettingID, deleted.Value, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[ProjectSetting] ENABLE TRIGGER [trgDelProjectSetting]
GO
/****** Object:  Trigger [dbo].[trgUpdProjectSetting]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  Trigger [dbo].[trgUpdProjectSetting]    Script Date: 12.05.2015 11:42:50 ******/

CREATE TRIGGER [dbo].[trgUpdProjectSetting] ON [dbo].[ProjectSetting] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectSetting_Log (ProjectID, SettingID, Value, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.SettingID, deleted.Value, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectSetting
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectSetting, deleted 
where 1 = 1 
AND ProjectSetting.ProjectID = deleted.ProjectID
AND ProjectSetting.SettingID = deleted.SettingID
GO
ALTER TABLE [dbo].[ProjectSetting] ENABLE TRIGGER [trgUpdProjectSetting]
GO
/****** Object:  Trigger [dbo].[trgDelSetting]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--#####################################################################################################################
--######   Trigger for Setting  ######################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelSetting]    Script Date: 12.05.2015 11:33:43 ******/

CREATE TRIGGER [dbo].[trgDelSetting] ON [dbo].[Setting] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Setting_Log (SettingID, ParentSettingID, DisplayText, Description, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SettingID, deleted.ParentSettingID, deleted.DisplayText, deleted.Description, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO
ALTER TABLE [dbo].[Setting] ENABLE TRIGGER [trgDelSetting]
GO
/****** Object:  Trigger [dbo].[trgUpdSetting]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Trigger [dbo].[trgUpdSetting]    Script Date: 12.05.2015 11:34:04 ******/

CREATE TRIGGER [dbo].[trgUpdSetting] ON [dbo].[Setting] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Setting_Log (SettingID, ParentSettingID, DisplayText, Description, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SettingID, deleted.ParentSettingID, deleted.DisplayText, deleted.Description, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update Setting
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM Setting, deleted 
where 1 = 1 
AND Setting.SettingID = deleted.SettingID
GO
ALTER TABLE [dbo].[Setting] ENABLE TRIGGER [trgUpdSetting]
GO
/****** Object:  Trigger [dbo].[trgUpdUserProxy]    Script Date: 07.04.2021 12:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--#####################################################################################################################
--######   UserProxy - Trigger for setting PrivacyConsentDate   #######################################################
--#####################################################################################################################
CREATE TRIGGER [dbo].[trgUpdUserProxy] ON [dbo].[UserProxy] 
FOR UPDATE AS 
declare @PC bit 
if (select count(*) from deleted) = 1 
begin 
  set @PC = (select case when I.PrivacyConsent <> D.PrivacyConsent 
    or (I.PrivacyConsent is null and not D.PrivacyConsent is null) 
    or (not I.PrivacyConsent is null and D.PrivacyConsent is null) 
    then 1 else 0 end from inserted I, deleted D) 
  if (@PC = 1) 
  begin 
    UPDATE U SET PrivacyConsentDate = GETDATE()  
    FROM UserProxy U, deleted D 
    WHERE U.ID = D.ID 
  end 
end
GO
ALTER TABLE [dbo].[UserProxy] ENABLE TRIGGER [trgUpdUserProxy]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Copy project including all depending data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertProjectCopy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Output parameter, ID of the new project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertProjectCopy', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project that should be copied' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertProjectCopy', @level2type=N'PARAMETER',@level2name=N'@CopyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title of the new project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'procInsertProjectCopy', @level2type=N'PARAMETER',@level2name=N'@ProjectTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the basic address for accessing the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'BaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the name of the DiversityWorkbench module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'DiversityWorkbenchModule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'Hierarchy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Providing common information about the storage and processing of personal data within the DiversityWorkbench' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'PrivacyConsentInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectChildNodes', @level2type=N'PARAMETER',@level2name=N'@ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectChildNodes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectHierarchy', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the items related to the given item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectHierarchy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The list of Projects accessible for the current user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project as stored in table Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectParents', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the parents of the given project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'ProjectParents'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the settings including a display text with the whole hierarchy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyAll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyChildren', @level2type=N'PARAMETER',@level2name=N'@SettingID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the settings depending on a setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyChildren'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyNonProject', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the settings including a display text with the whole hierarchy that are not included in a project so far' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyNonProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Input parameter. Enter a valid ProjectID.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyProject', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Outdated. 
Replaced by SettingHierarchyProjectAll.
Returns the settings of a project including all settings inherited from superior projects' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Input parameter. Enter a valid ProjectID.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyProjectAll', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the settings for a project including those inherited from superior projects.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingHierarchyProjectAll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingsForProject', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A optional filter for the settings' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingsForProject', @level2type=N'PARAMETER',@level2name=N'@SettingFilter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional. A spacer placed between the ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingsForProject', @level2type=N'PARAMETER',@level2name=N'@Spacer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Formatting mode of the column ProjectSetting
1: Whole hierarchy
2: Remove leading value of @SettingFilter from hierarchy ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingsForProject', @level2type=N'PARAMETER',@level2name=N'@DisplayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns a table that lists all the settings for a project including those inherited from parent projects including formatting options.
Used for Metadata retrieval by DiversityCollection cache databases' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'SettingsForProject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'StableIdentifier', @level2type=N'PARAMETER',@level2name=N'@ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The stable identifier published for a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'StableIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the User as stored in table UserProxy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Version of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'Version'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Version of the client software compatible with the version of the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'VersionClient'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Database Role for administrative actions within the database.' , @level0type=N'USER',@level0name=N'DiversityWorkbenchAdministrator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Database Role with read/write accesss to the database.' , @level0type=N'USER',@level0name=N'DiversityWorkbenchEditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Database Role with reading accesss to the database.' , @level0type=N'USER',@level0name=N'DiversityWorkbenchUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the superior project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A short title for the project as displayed e.g. for selection from a  list in an user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Project'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An explicit title for the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text, describing characteristic features, e. g. the purpose and scope, of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'classification of the public description, e. g. abstract' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectDescriptionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Public description of the project if different from the description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'PublicDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal description of the project, not to be published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'InternalDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes regarding the project, not published on the web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Version number (refers to the database content of this project, not to application development)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL where e.g. the project is described' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The rights, e.g. the copyright of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectRights'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The editors of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectEditors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The institution responsible for the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectInstitution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings used within the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectSettings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Copyright statement for the data collected for and revised by this project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectCopyright'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The license under which the project is re-used or may be re-used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectLicenseURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date until which the data should not be published, made available to the public' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'EmbargoDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If an archive e.g. by a task schedule should be created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'CreateArchive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The protocol created during the last archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ArchiveProtocol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Information on the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent, Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'ProjectAgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given, Part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL linked to e.g. the module DiversityAgents within the Diversity Workbench, Part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the agent, e.g. person or institution  as defined in ProjectAgentType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the agent within the project as defined in ProjectAgentRole_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence of an agent within a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'AgentSequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the agent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The persons or institutions involved in the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the Agent, Part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'ProjectAgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the agent within the project as defined in ProjectAgentRole_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'An URL linked to e.g. the module DiversityAgents within the Diversity Workbench' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The role of the person or institution involved in the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentRole_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectAgentType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ArchiveID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the parent archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ParentArchiveID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The format of the archive,e.g. SDD, EML, ABCD etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Format'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The mime type of the archive, e.g. zip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'MimeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The path or URI of the archive file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'StorageLocation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The path or URI of the archive file as stored on an external server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'ExternalStorageLocation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The hash value generated from the file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'FileSHA512Hash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Size of the file in bytes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time when the data were transferred from the productive database (Last date and time when changes may have happend)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date when the archive is published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'PublicationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The reason for locking the data from publication. If empty data may be published' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'DataWithholdingReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the dataset is published and no further changes are allowed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'Published'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Archives of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveFormat_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Archive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'ArchiveID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier if archive is linked to an identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier of archives of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further information about this entry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectArchiveMimeType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal system generated primary key. Note that multiple values for a descriptor concept may be added (e.g. keywords)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project, foreign key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Language in which element content is expressed. Necessary even for numeric or date content (because expressed through string using language-specific conventions)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of a descriptor element concept (foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A name, state, or value text for the descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a conceptual ontological resource considered equivalent with the content, especially URIs for taxon names or keywords from ontologies.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'ContentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Object names, event names, keywords, etc., providing indexing information for a resource.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numeric identifier (primary key).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'ElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the parent element' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'ParentElementID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated name for descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Abbreviation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concise English label of a descriptor element definition.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in which elements are displayed independently of a resource collection (for order within a collection see ProjectDescriptorAssociation.DisplayOrder).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A free-form text that may be displayed in user interfaces as explanatory text.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a conceptual ontological resource considered equivalent with this descriptor element.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes and remarks. Although normally not published in public reports, this should not be used for truly confidential information.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Whether this DescriptorElement is to be displayed in the user interface.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'DisplayEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Examples of descriptor elements (= concepts for variables) are keyword, taxon name, pathogen name, host name, or host feature. Association with ResourceCollections is defined in ProjectDescriptorAssociation, relations in Res.DescriptorElementRelation.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectDescriptorElement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the agent. Only cached value where AgentURI is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Identifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the identifier as defined in table ExternalIdentifierType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL with further informations about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the identifier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'External identifiers for the project, e.g. a DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of project identifiers (primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The superior type of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'ParentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A URL providing with further informations about this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'URL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The description of this type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal notes about the type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of an external identier, e.g. DOI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectIdentifierType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The keyword related to the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'Keyword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Keywords for the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectKeyword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Label' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'Label'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data. This is the operator (or typist) name, which may be different from the person responsible.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when record was first entered (typed or imported) into this system.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Labels for the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLabel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the License, part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A display text as shown in the interface generated by the system or edited by the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URL of the license under which the project is re-used or may be re-used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of an official or legal permission to do or own a specified thing, e. g. Creative Common Licenses, Copyright Licenses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseHolder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information on the person or institution holding the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseHolderAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The year of the license declaration' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The context of the license, e.g. for publication' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'Context'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details of the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LicenseDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intellectual Property Rights; the rights given to persons for their intellectual property' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'IPR'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the Intellectual Property Rights' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'IPRHolder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information on the person or institution holding the ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'IPRHolderAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notice on rights held in and for the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'CopyrightStatement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The person or institution holding the copyright' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'CopyrightHolder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The link to a module containing futher information on the person or institution holding the copyright' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'CopyrightHolderAgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Licenses within a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectLicense'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the data publication of the project data package or literature reference publication where information on the project and/ or on project dataset(s) was published. Note this is only a cached value where ReferenceURI is present' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation or URI of literature reference where information on the project and/ or on project dataset(s) was published, e.g. referring to the module DiversityReferences' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the project data citation/ data package citation, e.g. versioning, or details within the literature reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the data citation or literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If a reference is the citation for the project data, i.e. the way the project data should be cited' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'IsCitation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of a reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the project data citation or literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The references related to the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectReference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The complete URI address of the resource. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The rights, e.g. the copyright of the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'SpecificRights'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The license under which the resource is re-used or may be re-used' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'SpecificLicenseURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URI of the image, e.g. as stored in the module DiversityResources. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'ResourceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the resource' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time when this dataset was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last time when this dataset was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who was the last to update this dataset' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Resources of the project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectResource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of the project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to ID of Settings, foreign key and part of primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'SettingID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Settings of a project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name which the user uses  for access the DivesityWorkbench, Microsoft domains, etc..' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the project to which the specimen belongs (Projects are defined in DiversityProjects)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The projects available for a user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID of the setting, primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'SettingID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the superior setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'ParentSettingID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the setting, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings used within the projects' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A login name of the user for access to the database server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The short name of the user, e.g. P. Smith. A cached value derived from an entry in the module DiversityAgents if AgentURI is filled.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'CombinedNameCache'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of a user in the remote module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Queries defined by the user to access the data in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Queries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The settings for the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'Settings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IS of the user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user consents the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The time and date when the user consented or refused the storage of his user name in the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy', @level2type=N'COLUMN',@level2name=N'PrivacyConsentDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The users with access to the database' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProxy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides all columus of table Project restricted to the projects accessible for a user as set in table ProjectUser with the exception of the dbo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Project_Core'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the project as used for communication among modules = BaseURL + ProjectID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectProxy', @level2type=N'COLUMN',@level2name=N'ProjectURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provides the ProjectID, the Project and the ProjectURI (= BaseURL + ProjectID) of table Project restricted to the projects accessible for a user as set in table ProjectUser with the exception of the dbo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ProjectProxy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of the function BaseURL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL', @level2type=N'COLUMN',@level2name=N'BaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of the function BaseURL provided via a view for usage e.g. by linked servers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewBaseURL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the data publication of the project data package. Note this is only a cached value where ReferenceURI is present.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation of the project data package, e.g. referring to the module DiversityReferences.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the project data citation/ data package citation, e.g. versioning' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the project data citation/ data package citation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the project data citation/ data package citation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of project data citation/ data package citation with information on the project dataset(s)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the project data/ data package should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The citation of published data from the project;  references of the data publication of the project data package' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataCitation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of the function DiversityWorkbenchModule' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDiversityWorkbenchModule', @level2type=N'COLUMN',@level2name=N'DiversityWorkbenchModule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Refers to the ID of table Project (= Foreign key and part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The title of the literature reference publication where information on the project and/ or on project dataset(s) was published. Note this is only a cached value where ReferenceURI is present.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of literature reference with information on the project and/ or on the project dataset(s), e.g. referring to the module DiversityReferences.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceURI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details within the literature reference, e.g. pages' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes on the literature reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'Notes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URI of the literature reference with information on the project dataset(s)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'URI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The way the literature reference should be cited; suggested citation style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference', @level2type=N'COLUMN',@level2name=N'ReferenceCitation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The references related to the project;  references of literature publications where information on the project and/ or on project dataset(s) is given' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewLiteratureReference'
GO
