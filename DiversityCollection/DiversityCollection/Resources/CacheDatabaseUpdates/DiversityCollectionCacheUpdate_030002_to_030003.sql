
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '03.00.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   A B C D   ######################################################################################
--#####################################################################################################################

INSERT INTO [Interface]
           ([InterfaceName])
     VALUES
           ('ABCD')
GO


--#####################################################################################################################
--######   [ABCD_WGS84] etc. for Coordinates   ######################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ABCD_WGS84]
AS
SELECT     CollectionEventID, ProjectID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName,
                       ResponsibleAgentURI, RecordingMethod, Geography, AverageLatitudeCache, AverageLongitudeCache
FROM         CollectionEventLocalisation
WHERE     (LocalisationSystemID = 8)

GO

GRANT SELECT ON [ABCD_WGS84] TO [CacheUser]
GO



CREATE VIEW [dbo].[ABCD_GaussKrueger]
AS
SELECT     CollectionEventID, ProjectID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName,
                       ResponsibleAgentURI, RecordingMethod, Geography, AverageLatitudeCache, AverageLongitudeCache
FROM         dbo.CollectionEventLocalisation
WHERE     (LocalisationSystemID = 2)

GO

GRANT SELECT ON [ABCD_GaussKrueger] TO [CacheUser]
GO


CREATE VIEW [dbo].[ABCD_Greenwich]
AS
SELECT     CollectionEventID, ProjectID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName,
                       ResponsibleAgentURI, RecordingMethod, Geography, AverageLatitudeCache, AverageLongitudeCache
FROM         dbo.CollectionEventLocalisation
WHERE     (LocalisationSystemID = 6)

GO

GRANT SELECT ON [ABCD_Greenwich] TO [CacheUser]
GO



CREATE VIEW [dbo].[ABCD_Gazetteer]
AS
SELECT     CollectionEventID, ProjectID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName,
                       ResponsibleAgentURI, RecordingMethod, Geography, AverageLatitudeCache, AverageLongitudeCache
FROM         dbo.CollectionEventLocalisation
WHERE     (LocalisationSystemID = 7)

GO

GRANT SELECT ON [ABCD_Gazetteer] TO [CacheUser]
GO


CREATE VIEW [dbo].[ABCD_MTB]
AS
SELECT     CollectionEventID, ProjectID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, ResponsibleName,
                       ResponsibleAgentURI, RecordingMethod, Geography, AverageLatitudeCache, AverageLongitudeCache
FROM         dbo.CollectionEventLocalisation
WHERE     (LocalisationSystemID = 3)

GO

GRANT SELECT ON [ABCD_MTB] TO [CacheUser]
GO


--#####################################################################################################################
--######   Geological informations   ######################################################################################
--#####################################################################################################################



CREATE VIEW [dbo].[ABCD_Chronostratigraphy]
AS
SELECT     CollectionEventID, ProjectID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes
FROM         dbo.CollectionEventProperty
WHERE     (PropertyID = 20)

GO

GRANT SELECT ON [ABCD_Chronostratigraphy] TO [CacheUser]
GO



CREATE VIEW [dbo].[ABCD_Lithostratigraphy]
AS
SELECT     CollectionEventID, ProjectID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes
FROM         dbo.CollectionEventProperty
WHERE     (PropertyID = 30)

GO

GRANT SELECT ON [ABCD_Lithostratigraphy] TO [CacheUser]
GO


--#####################################################################################################################
--######   [ABCD_CollectionSpecimen]   ######################################################################################
--#####################################################################################################################



CREATE VIEW [dbo].[ABCD_CollectionSpecimen]
AS
SELECT     S.CollectionSpecimenID, S.ProjectID, S.Version, S.AccessionNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, 
                      E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache, S.LabelTranscriptionNotes, S.ExsiccataAbbreviation, S.OriginalNotes, 
                      CASE WHEN C_wgs.AverageLatitudeCache IS NULL THEN CASE WHEN C_gk.AverageLatitudeCache IS NULL THEN CASE WHEN C_gre.AverageLatitudeCache IS NULL 
                      THEN CASE WHEN C_gaz.AverageLatitudeCache IS NULL 
                      THEN C_mtb.AverageLatitudeCache ELSE C_gaz.AverageLatitudeCache END ELSE C_gre.AverageLatitudeCache END ELSE C_gk.AverageLatitudeCache END ELSE C_wgs.AverageLatitudeCache
                       END AS Latitude, CASE WHEN C_wgs.AverageLongitudeCache IS NULL THEN CASE WHEN C_gk.AverageLongitudeCache IS NULL 
                      THEN CASE WHEN C_gre.AverageLongitudeCache IS NULL THEN CASE WHEN C_gaz.AverageLongitudeCache IS NULL 
                      THEN C_mtb.AverageLongitudeCache ELSE C_gaz.AverageLongitudeCache END ELSE C_gre.AverageLongitudeCache END ELSE C_gk.AverageLongitudeCache END ELSE
                       C_wgs.AverageLongitudeCache END AS Longitude, CASE WHEN C_wgs.LocationAccuracy IS NULL THEN CASE WHEN C_gk.LocationAccuracy IS NULL 
                      THEN CASE WHEN C_gre.LocationAccuracy IS NULL THEN CASE WHEN C_gaz.LocationAccuracy IS NULL 
                      THEN C_mtb.LocationAccuracy ELSE C_gaz.LocationAccuracy END ELSE C_gre.LocationAccuracy END ELSE C_gk.LocationAccuracy END ELSE C_wgs.LocationAccuracy
                       END AS LocationAccuracy, CASE WHEN C_wgs.DistanceToLocation IS NULL THEN CASE WHEN C_gk.DistanceToLocation IS NULL 
                      THEN CASE WHEN C_gre.DistanceToLocation IS NULL THEN CASE WHEN C_gaz.DistanceToLocation IS NULL 
                      THEN C_mtb.DistanceToLocation ELSE C_gaz.DistanceToLocation END ELSE C_gre.DistanceToLocation END ELSE C_gk.DistanceToLocation END ELSE C_wgs.DistanceToLocation
                       END AS DistanceToLocation, CASE WHEN C_wgs.DirectionToLocation IS NULL THEN CASE WHEN C_gk.DirectionToLocation IS NULL 
                      THEN CASE WHEN C_gre.DirectionToLocation IS NULL THEN CASE WHEN C_gaz.DirectionToLocation IS NULL 
                      THEN C_mtb.DirectionToLocation ELSE C_gaz.DirectionToLocation END ELSE C_gre.DirectionToLocation END ELSE C_gk.DirectionToLocation END ELSE C_wgs.DirectionToLocation
                       END AS DirectionToLocation, E.CollectorsEventNumber, Lit.DisplayText AS Lithostratigraphy, Chr.DisplayText AS Chronostratigraphy, S.LogInsertedWhen
FROM         dbo.ABCD_Greenwich AS C_gre RIGHT OUTER JOIN
                      dbo.CollectionEvent AS E ON C_gre.CollectionEventID = E.CollectionEventID AND C_gre.ProjectID = E.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_Gazetteer AS C_gaz ON E.CollectionEventID = C_gaz.CollectionEventID AND E.ProjectID = C_gaz.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_MTB AS C_mtb ON E.CollectionEventID = C_mtb.CollectionEventID AND E.ProjectID = C_mtb.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_GaussKrueger AS C_gk ON E.CollectionEventID = C_gk.CollectionEventID AND E.ProjectID = C_gk.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_WGS84 AS C_wgs ON E.CollectionEventID = C_wgs.CollectionEventID AND E.ProjectID = C_wgs.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_Chronostratigraphy AS Chr ON E.CollectionEventID = Chr.CollectionEventID AND E.ProjectID = Chr.ProjectID LEFT OUTER JOIN
                      dbo.ABCD_Lithostratigraphy AS Lit ON E.CollectionEventID = Lit.CollectionEventID AND E.ProjectID = Lit.ProjectID RIGHT OUTER JOIN
                      dbo.CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID AND E.ProjectID = S.ProjectID

GO

GRANT SELECT ON [ABCD_CollectionSpecimen] TO [CacheUser]
GO

--INSERT INTO [InterfaceTable]
--           ([InterfaceName]
--           ,[TableName])
--     VALUES
--           ('ABCD'
--           ,'ABCD_CollectionSpecimen')
--GO


--#####################################################################################################################
--######   [ABCD_Metadata]   ######################################################################################
--#####################################################################################################################
/*
CREATE VIEW [dbo].[ABCD_Metadata]
AS
SELECT     ProjectID, 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/GUID)') AS nvarchar(500)) AS DatasetGUID, 
CAST(ProjectSettings.query('data(/settings/ABCD/TaxonomicGroup)') AS nvarchar(50)) AS [Scope.TaxonomicTerm], 
ProjectSettings.query('data(/settings/ABCD/RecordURI)') AS RecordURI, 
CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Name)') AS nvarchar(500)) AS [TechnicalContact.TechnicalContactName], 
CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Email)') AS nvarchar(500)) AS [TechnicalContact.TechnicalContactEmail], 
CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Phone)') AS nvarchar(500)) AS [TechnicalContact.TechnicalContactPhone], 
CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Address)') AS nvarchar(500)) AS [TechnicalContact.TechnicalContactAddress], 
CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Name)') AS nvarchar(500)) AS [ContentContact.ContentContactName], 
CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Email)') AS nvarchar(500)) AS [ContentContact.ContentContactEmail], 
CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Phone)') AS nvarchar(500)) AS [ContentContact.ContentContactPhone], 
CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Address)') AS nvarchar(500)) AS [ContentContact.ContentContactAddress], 
CAST(ProjectSettings.query('data(/settings/ABCD/OtherProviderUDDI)') AS nvarchar(500)) AS OtherProviderUDDI, 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Title)') AS nvarchar(500)) AS [Metadata.Description.Representation.Title], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Details)') AS nvarchar(500)) AS [Metadata.Description.Representation.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Coverage)') AS nvarchar(500)) AS [Metadata.Description.Representation.Coverage], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/URI)') AS nvarchar(500)) AS [Metadata.Description.Representation.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/IconURI)') AS nvarchar(500)) AS [Metadata.IconURI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/VersionMajor)') AS nvarchar(500)) AS [Metadata.Version.Major], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Creator)') AS nvarchar(500)) AS [Metadata.RevisionData.Creators], 
CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Contributors)') AS nvarchar(500)) AS [Metadata.RevisionData.Contributors], 
CAST(ProjectSettings.query('data(/settings/ABCD/Date/Created)') AS nvarchar(500)) AS [Metadata.RevisionData.DateCreated], 
CAST(ProjectSettings.query('data(/settings/ABCD/Date/Modified)') AS nvarchar(500)) AS [Metadata.RevisionData.DateModified], 
CAST(ProjectSettings.query('data(/settings/ABCD/Source/InstitutionID)') AS nvarchar(500)) AS [Unit.SourceInstitutionID], 
CAST(ProjectSettings.query('data(/settings/ABCD/Source/ID)') AS nvarchar(500)) AS [Unit.SourceID], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationName)') AS nvarchar(500)) AS [Metadata.Owner.Organization.Name], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationAbbrev)') AS nvarchar(500)) AS [Metadata.Owner.Organization.Representation.Abbreviation], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/ContactPerson)') AS nvarchar(500)) AS [Metadata.Owner.Person.FullName], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/ContactRole)') AS nvarchar(500)) AS [Metadata.Owner.Roles.Role], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Address)') AS nvarchar(500)) AS [Metadata.Owner.Addresses.Address], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Telephone)') AS nvarchar(500)) AS [Metadata.Owner.TelephoneNumbers.TelephoneNumber], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Email)') AS nvarchar(500)) AS [Metadata.Owner.EmailAddresses.EmailAddress], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/URI)') AS nvarchar(500)) AS [Metadata.Owner.URIs.URL], 
CAST(ProjectSettings.query('data(/settings/ABCD/Owner/LogoURI)') AS nvarchar(500)) AS [Metadata.Owner.LogoURI], 
CAST(ProjectSettings.query('data(/settings/ABCD/IPR/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/IPR/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/IPR/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.IPRDeclarations.IPRDeclaration.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Copyright/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.Copyrights.Copyright.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.TermsOfUseStatements.TermsOfUse.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.Disclaimers.Disclaimer.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.Disclaimers.Disclaimer.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.Disclaimers.Disclaimer.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/License/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.Licenses.License.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/License/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.Licenses.License.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/License/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.Licenses.License.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.Acknowledgements.Acknowledgement.Text],
CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.Acknowledgements.Acknowledgement.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.Acknowledgements.Acknowledgement.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/Citations/Text)') AS nvarchar(500)) AS [Metadata.IPRStatements.Citations.Citation.Text], 
CAST(ProjectSettings.query('data(/settings/ABCD/Citations/Details)') AS nvarchar(500)) AS [Metadata.IPRStatements.Citations.Citation.Details], 
CAST(ProjectSettings.query('data(/settings/ABCD/Citations/URI)') AS nvarchar(500)) AS [Metadata.IPRStatements.Citations.Citation.URI], 
CAST(ProjectSettings.query('data(/settings/ABCD/RecordBasis)') AS nvarchar(500)) AS [Metadata.RecordBasis], 
CAST(ProjectSettings.query('data(/settings/ABCD/KindOfUnit)') AS nvarchar(500)) AS [Metadata.KindOfUnit], 
CAST(ProjectSettings.query('data(/settings/ABCD/HigherTaxonRank)') AS nvarchar(500)) AS [Unit.Gathering.Synecology.AssociatedTaxa.TaxonIdentified.HigherTaxa.HigherTaxon.HigherTaxonRank], 
CAST(ProjectSettings.query('data(/settings/ABCD/RecordURI)') AS nvarchar(500)) AS [Metadata.RecordURI]
FROM         dbo.ProjectProxy

GO

GRANT SELECT ON [ABCD_Metadata] TO [CacheUser]
GO



INSERT INTO [InterfaceTable]
           ([InterfaceName]
           ,[TableName])
     VALUES
           ('ABCD'
           ,'ABCD_Metadata')
GO
*/


--#####################################################################################################################
--######   [ABCD_Datasets/Dataset]   ######################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[ABCD_Datasets/Dataset]
AS
SELECT     ProjectID, CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/GUID)') AS nvarchar(500)) AS DatasetGUID, 
  CAST(ProjectSettings.query('data(/settings/ABCD/TaxonomicGroup)') AS nvarchar(50)) AS [Metadata/Scope/TaxonomicTerms/TaxonomicTerm], 
  ProjectSettings.query('data(/settings/ABCD/RecordURI)') AS RecordURI, 
  CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Name)') AS nvarchar(500)) AS [TechnicalContacts/TechnicalContact/TechnicalContactName], CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Email)') AS nvarchar(500)) 
  AS [TechnicalContacts/TechnicalContact/TechnicalContactEmail], CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Phone)') AS nvarchar(500)) 
  AS [TechnicalContacts/TechnicalContact/TechnicalContactPhone], CAST(ProjectSettings.query('data(/settings/ABCD/TechnicalContact/Address)') AS nvarchar(500)) 
  AS [TechnicalContacts/TechnicalContact/TechnicalContactAddress], CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Name)') AS nvarchar(500)) 
  AS [ContentContacts/ContentContact/ContentContactName], CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Email)') AS nvarchar(500)) 
  AS [ContentContacts/ContentContact/ContentContactEmail], CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Phone)') AS nvarchar(500)) 
  AS [ContentContacts/ContentContact/ContentContactPhone], CAST(ProjectSettings.query('data(/settings/ABCD/ContentContact/Address)') AS nvarchar(500)) 
  AS [ContentContacts/ContentContact/ContentContactAddress], 
  CAST(ProjectSettings.query('data(/settings/ABCD/OtherProviderUDDI)') AS nvarchar(500)) AS [OtherProviders/OtherProvider], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Title)') AS nvarchar(500)) AS [Metadata/Description/Representation/Title], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Details)') AS nvarchar(500)) AS [Metadata/Description/Representation/Details], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Coverage)') AS nvarchar(500)) AS [Metadata/Description/Representation/Coverage], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/URI)') AS nvarchar(500)) AS [Metadata/Description/Representation/URI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/IconURI)') AS nvarchar(500)) AS [Metadata/IconURI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/VersionMajor)') AS nvarchar(500)) AS [Metadata/Version.Major], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Creator)') AS nvarchar(500)) AS [Metadata/RevisionData/Creators], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Dataset/Contributors)') AS nvarchar(500)) AS [Metadata/RevisionData/Contributors], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Date/Created)') AS nvarchar(500)) AS [Metadata/RevisionData/DateCreated], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Date/Modified)') AS nvarchar(500)) AS [Metadata/RevisionData/DateModified], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Source/InstitutionID)') AS nvarchar(500)) AS [Units/Unit/SourceInstitutionID], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Source/ID)') AS nvarchar(500)) AS [Units/Unit/SourceID], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationName)') AS nvarchar(500)) AS [Metadata/Owner/Organization/Name], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/OrganizationAbbrev)') AS nvarchar(500)) AS [Metadata/Owner/Organization/Representation/Abbreviation], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/ContactPerson)') AS nvarchar(500)) AS [Metadata/Owner/Person/FullName], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/ContactRole)') AS nvarchar(500)) AS [Metadata/Owner/Roles/Role], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Address)') AS nvarchar(500)) AS [Metadata/Owner/Addresses/Address], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Telephone)') AS nvarchar(500)) AS [Metadata/Owner/TelephoneNumbers/TelephoneNumber], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/Email)') AS nvarchar(500)) AS [Metadata/Owner/EmailAddresses/EmailAddress], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/URI)') AS nvarchar(500)) AS [Metadata/Owner/URIs/URL], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Owner/LogoURI)') AS nvarchar(500)) AS [Metadata/Owner/LogoURI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/IPR/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Text], 
  CAST(ProjectSettings.query('data(/settings/ABCD/IPR/Details)') AS nvarchar(500)) AS [Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/Details], 
  CAST(ProjectSettings.query('data(/settings/ABCD/IPR/URI)') AS nvarchar(500)) AS [Metadata/IPRStatements/IPRDeclarations/IPRDeclaration/URI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Copyright/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/Copyrights/Copyright/Text], 
  CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Text], 
  CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/Details)') AS nvarchar(500)) AS [Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/Details], 
  CAST(ProjectSettings.query('data(/settings/ABCD/TermsOfUse/URI)') AS nvarchar(500)) AS [Metadata/IPRStatements/TermsOfUseStatements/TermsOfUse/URI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/Disclaimers/Disclaimer/Text], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/Details)') AS nvarchar(500)) AS [Metadata/IPRStatements/Disclaimers/Disclaimer/Details], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Disclaimers/URI)') AS nvarchar(500)) AS [Metadata/IPRStatements/Disclaimers/Disclaimer/URI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/License/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/Licenses/License/Text], 
  CAST(ProjectSettings.query('data(/settings/ABCD/License/Details)') AS nvarchar(500)) AS [Metadata/IPRStatements/Licenses/License/Details], 
  CAST(ProjectSettings.query('data(/settings/ABCD/License/URI)') AS nvarchar(500)) AS [Metadata/IPRStatements/Licenses/License/URI], 
  CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Text)') AS nvarchar(500)) AS [Metadata/IPRStatements/Acknowledgements/Acknowledgement/Text],
   CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/Details)') AS nvarchar(500)) 
  AS [Metadata/IPRStatements/Acknowledgements/Acknowledgement/Details], CAST(ProjectSettings.query('data(/settings/ABCD/Acknowledgements/URI)') 
  AS nvarchar(500)) AS [Metadata/IPRStatements/Acknowledgements/Acknowledgement/URI], CAST(ProjectSettings.query('data(/settings/ABCD/Citations/Text)') 
  AS nvarchar(500)) AS [Metadata/IPRStatements/Citations/Citation/Text], CAST(ProjectSettings.query('data(/settings/ABCD/Citations/Details)') AS nvarchar(500)) 
  AS [Metadata/IPRStatements/Citations/Citation/Details], CAST(ProjectSettings.query('data(/settings/ABCD/Citations/URI)') AS nvarchar(500)) 
  AS [Metadata/IPRStatements/Citations/Citation/URI], CAST(ProjectSettings.query('data(/settings/ABCD/RecordBasis)') AS nvarchar(500)) AS [Metadata/RecordBasis], 
  CAST(ProjectSettings.query('data(/settings/ABCD/KindOfUnit)') AS nvarchar(500)) AS [Metadata/KindOfUnit], 
  CAST(ProjectSettings.query('data(/settings/ABCD/HigherTaxonRank)') AS nvarchar(500)) 
  AS [Units/Unit/Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonRank], 
  CAST(ProjectSettings.query('data(/settings/ABCD/RecordURI)') AS nvarchar(500)) AS [Metadata/RecordURI]
FROM dbo.ProjectProxy



GO


GRANT SELECT ON [ABCD_Datasets/Dataset] TO [CacheUser]
GO



INSERT INTO [InterfaceTable]
           ([InterfaceName]
           ,[TableName])
     VALUES
           ('ABCD'
           ,'ABCD_Datasets/Dataset')
GO



--#####################################################################################################################
--######   [ABCD_DataSets/DataSet/Units/Unit]   ######################################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[ABCD_DataSets/DataSet/Units/Unit]
AS
SELECT     UIP.ProjectID, U.CollectionSpecimenID, U.IdentificationUnitID, UIP.SpecimenPartID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID AS SubstrateID, 
      U.ExsiccataNumber, U.DisplayOrder, U.ColonisedSubstratePart, '' AS IdentificationQualifier, '' AS VernacularTerm, '' AS TaxonomicName, U.Notes, '' AS TypeStatus, 
      U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender, U.RelationType AS Relation, 
      I.LastValidTaxonName AS [Identifications/Identification/Result/TaxonIdentified/ScientificName/FullScientificNameString], 
      I.IdentificationQualifier AS [Identifications/Identification/Result/TaxonIdentified/ScientificName/IdentificationQualifier], 
      CASE WHEN I.TypeStatus <> '' THEN I.TaxonomicName ELSE '' END AS [SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypifiedName/FullScientificNameString],
       I.TypeStatus AS [SpecimenUnit/NomenclaturalTypeDesignations/NomenclaturalTypeDesignation/TypeStatus], REVERSE(SUBSTRING(REVERSE(dbo.BaseUrlSource()), 
      CHARINDEX('/', REVERSE(dbo.BaseUrlSource()), 2), 255)) AS [Associations/UnitAssociation/AssociatedUnitSourceInstitutionCode], 
      REPLACE(REVERSE(SUBSTRING(REVERSE(dbo.BaseUrlSource()), 1, CHARINDEX('/', REVERSE(dbo.BaseUrlSource()), 2))), '/', '') 
      AS [Associations/UnitAssociation/AssociatedUnitSourceName], 
      S.AccessionNumber + ' / ' + CAST(U.RelatedUnitID as varchar) + ' / ' + cast(UIP.SpecimenPartID as varchar) AS [Associations/UnitAssociation/AssociatedUnitID], 
      U.RelationType AS [Associations/UnitAssociation/AssociationType], RTRIM(RTRIM(U.RelationType) 
      + ' ' + CASE WHEN U.RelationType LIKE '% on' THEN '' ELSE 'on ' END + CASE WHEN A.LastIdentificationCache = '' THEN A.TaxonomicGroup ELSE A.LastIdentificationCache
       END) AS [Associations/UnitAssociation/Comment], 
      U.TaxonomicGroup AS [Gathering/Synecology/AssociatedTaxa/TaxonIdentified/HigherTaxa/HigherTaxon/HigherTaxonName], CAST(U.CollectionSpecimenID AS varchar)
       + '-' + CAST(U.IdentificationUnitID AS varchar) + '-' + CAST(UIP.SpecimenPartID AS varchar) AS UnitID, dbo.BaseUrlSource() + CAST(U.CollectionSpecimenID AS varchar)
       + '-' + CAST(U.IdentificationUnitID AS varchar) + '-' + CAST(UIP.SpecimenPartID AS varchar) AS UnitGUID, P.MaterialCategory
FROM         ABCD_Identification AS I INNER JOIN
      CollectionSpecimenPart AS P INNER JOIN
      IdentificationUnitInPart AS UIP ON P.CollectionSpecimenID = UIP.CollectionSpecimenID AND P.SpecimenPartID = UIP.SpecimenPartID AND 
      P.ProjectID = UIP.ProjectID RIGHT OUTER JOIN
      CollectionSpecimen AS S INNER JOIN
      IdentificationUnit AS U ON S.ProjectID = U.ProjectID AND S.CollectionSpecimenID = U.CollectionSpecimenID ON 
      UIP.CollectionSpecimenID = U.CollectionSpecimenID AND UIP.IdentificationUnitID = U.IdentificationUnitID AND UIP.ProjectID = U.ProjectID ON 
      I.CollectionSpecimenID = U.CollectionSpecimenID AND I.IdentificationUnitID = U.IdentificationUnitID LEFT OUTER JOIN
      IdentificationUnit AS A ON U.CollectionSpecimenID = A.CollectionSpecimenID AND U.RelatedUnitID = A.IdentificationUnitID AND U.ProjectID = A.ProjectID

GO



GRANT SELECT ON [ABCD_DataSets/DataSet/Units/Unit] TO [CacheUser]
GO


INSERT INTO [InterfaceTable]
           ([InterfaceName]
           ,[TableName])
     VALUES
           ('ABCD'
           ,'ABCD_DataSets/DataSet/Units/Unit')
GO


--#####################################################################################################################
--######   [Identification...]   ######################################################################################
--#####################################################################################################################
/*
Ziel: 
IdentificationUnitPartCache in DiversityCollectionCache
ist Tabelle die von Funktion gefüllt wird:
procRefreshIdentificationUnitPartCache

SELECT DISTINCT  U.CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, RTRIM(LastIdentificationCache), 
	TaxonomicGroup, RelatedUnitID, ExsiccataNumber, U.DisplayOrder, 
	ColonisedSubstratePart, '', '', '', CAST(Notes AS nvarchar(600)),  
	'' AS TypeStatus, RTRIM(''), RTRIM(''), '', '', 
	RTRIM('') AS GenusOrSupragenericName, RTRIM(''), RTRIM(''), RTRIM(''), RTRIM(''), 
	RTRIM('') AS LastValidTaxonIndex, '', '', FamilyCache, OrderCache, 
	LifeStage, Gender, RelationType, 
	'http://id.snsb.info/', 'Collection'
	, RelatedUnitID, RelationType
	, cast(U.CollectionSpecimenID as varchar) + '-' + cast(U.IdentificationUnitID as varchar) + '-' + cast(P.SpecimenPartID as varchar)
	, 'http://id.snsb.info/Collection/' + cast(U.CollectionSpecimenID as varchar) + '-' + cast(U.IdentificationUnitID as varchar) + '-' + cast(P.SpecimenPartID as varchar)
	, ''
	, CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber + ' / ' ELSE '' END + CAST(U.IdentificationUnitID AS varchar(90)) + ' / ' + cast(P.SpecimenPartID  AS varchar(90))
FROM DiversityCollection.dbo.IdentificationUnit U, dbo.IdentificationUnitInPart P, dbo.CollectionSpecimen S
where P.CollectionSpecimenID = U.CollectionSpecimenID and U.IdentificationUnitID = P.IdentificationUnitID
and S.CollectionSpecimenID = U.CollectionSpecimenID and S.CollectionSpecimenID = P.CollectionSpecimenID
and (U.TaxonomicGroup NOT IN ('unknown', 'other', 'soil'))

mit vorhandenen Tabellen loesbar


SELECT DISTINCT  U.CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, RTRIM(LastIdentificationCache), 
	TaxonomicGroup, RelatedUnitID, ExsiccataNumber, U.DisplayOrder, 
	ColonisedSubstratePart, '', '', '', CAST(SP.Notes AS nvarchar(600)),  
	'' AS TypeStatus, RTRIM(''), RTRIM(''), '', '', 
	RTRIM('') AS GenusOrSupragenericName, RTRIM(''), RTRIM(''), RTRIM(''), RTRIM(''), 
	RTRIM('') AS LastValidTaxonIndex, '', '', FamilyCache, OrderCache, 
	LifeStage, Gender, RelationType, 
	'http://id.snsb.info/', 'Collection'
	, RelatedUnitID, RelationType
	, cast(U.CollectionSpecimenID as varchar) + '-' + cast(U.IdentificationUnitID as varchar) + '-' + cast(P.SpecimenPartID as varchar)
	, 'http://id.snsb.info/Collection/' + cast(U.CollectionSpecimenID as varchar) + '-' + cast(U.IdentificationUnitID as varchar) + '-' + cast(P.SpecimenPartID as varchar)
	, ''
	, CAST(P.CollectionSpecimenID AS varchar) + ' / ' + CAST(U.IdentificationUnitID AS varchar(90)) + ' / ' + cast(SP.AccessionNumber  AS varchar(90))
FROM DiversityCollection.dbo.IdentificationUnit U, dbo.IdentificationUnitInPart P,
dbo.CollectionSpecimenPart SP, dbo.CollectionSpecimen_BSMweinzierl S
where P.CollectionSpecimenID = U.CollectionSpecimenID and U.IdentificationUnitID = P.IdentificationUnitID
and S.CollectionSpecimenID = U.CollectionSpecimenID and S.CollectionSpecimenID = P.CollectionSpecimenID
and (U.TaxonomicGroup NOT IN ('unknown', 'other', 'soil'))
and SP.CollectionSpecimenID = P.CollectionSpecimenID
and SP.SpecimenPartID = P.SpecimenPartID

Spezialloesung fuer Weinzierl - sollte durch eigene Sicht erfolgen

*/
CREATE VIEW [dbo].[ABCD_FirstIdentificationSequence]
AS
SELECT     CollectionSpecimenID, IdentificationUnitID, MIN(IdentificationSequence) AS IdentificationSequence
FROM         Identification
GROUP BY CollectionSpecimenID, IdentificationUnitID

GO

GRANT SELECT ON [ABCD_FirstIdentificationSequence] TO [CacheUser]
GO



CREATE VIEW [dbo].[ABCD_FirstIdentification]
AS
SELECT     I.CollectionSpecimenID, I.IdentificationUnitID, CASE WHEN I.TaxonomicName IS NULL OR
                      I.TaxonomicName = '' THEN I.VernacularTerm ELSE I.TaxonomicName END AS TaxonomicNameFirstEntry
FROM         Identification AS I INNER JOIN
                      ABCD_FirstIdentificationSequence F ON I.CollectionSpecimenID = F.CollectionSpecimenID AND 
                      I.IdentificationUnitID = F.IdentificationUnitID AND I.IdentificationSequence = F.IdentificationSequence

GO

GRANT SELECT ON [ABCD_FirstIdentification] TO [CacheUser]
GO




CREATE VIEW [dbo].[ABCD_LastIdentificationSequence]
AS
SELECT     CollectionSpecimenID, IdentificationUnitID, MAX(IdentificationSequence) AS IdentificationSequence
FROM         DiversityCollection.dbo.Identification
GROUP BY CollectionSpecimenID, IdentificationUnitID

GO

GRANT SELECT ON [ABCD_LastIdentificationSequence] TO [CacheUser]
GO




--#####################################################################################################################
--######   [IdentificationUnitCache]   ######################################################################################
--#####################################################################################################################
/*

CREATE TABLE [dbo].[IdentificationUnit_ABCD]( 
[CollectionSpecimenID] [int] NOT NULL, 
[IdentificationUnitID] [int] NOT NULL, 
[SpecimenPartID] [int] NOT NULL, 
[LastIdentificationCache] [nvarchar](255) NULL, 
[TaxonomicGroup] [nvarchar](50) NULL, 
[SubstrateID] [int] NULL, 
[ExsiccataNumber] [nvarchar](50) NULL, 
[DisplayOrder] [tinyint] NOT NULL, 
[ColonisedSubstratePart] [nvarchar](255) NULL, 
[IdentificationQualifier] [nvarchar](50) NULL, 
[VernacularTerm] [nvarchar](255) NULL, 
[TaxonomicName] [nvarchar](255) NULL, 
[Notes] [nvarchar](600) NULL, 
[TypeStatus] [nvarchar](50) NULL, 
[SynonymName] [nvarchar](255) NULL, 
[AcceptedName] [nvarchar](255) NULL, 
[AcceptedNameURI] [varchar](255) NULL, 
[SynNameURI] [varchar](255) NULL, 
[TaxonomicRank] [nvarchar](50) NULL, 
[GenusOrSupragenericName] [nvarchar](200) NULL, 
[TaxonNameSinAuthor] [nvarchar](255) NULL, 
[IdentificationSequenceFirstEntry] [smallint] NULL, 
[TaxonomicNameFirstEntry] [nvarchar](255) NULL, 
[LastValidIdentificationSequence] [smallint] NULL, 
[LastValidTaxonName] [nvarchar](255) NULL, 
[LastValidTaxonWithQualifier] [nvarchar](255) NULL, 
[LastValidTaxonSinAuthor] [nvarchar](255) NULL, 
[LastValidTaxonIndex] [nvarchar](255) NULL, 
[LastValidTaxonListOrder] [tinyint] NULL, 
[FamilyCache] [nvarchar](255) NULL, 
[OrderCache] [nvarchar](255) NULL, 
[LifeStage] [nvarchar](255) NULL, 
[Gender] [nvarchar](50) NULL, 
[Relation] [nvarchar](500) NULL, 
[UnitAssociation_AssociatedUnitSourceInstitutionCode] [nvarchar](50) NULL, 
[UnitAssociation_AssociatedUnitSourceName] [nvarchar](50) NULL, 
[UnitAssociation_AssociatedUnitID] [int] NULL, 
[UnitAssociation_AssociationType] [nvarchar](50) NULL, 
[UnitAssociation_Comment] [nvarchar](500) NULL, 
[UnitID] [nvarchar](90) NULL, 
[UnitGUID] [nvarchar](150) NULL, 
[AccNrUnitID] [nvarchar](90) NULL, 
[NomenclaturalTypeDesignation_TypifiedName] [nvarchar](500) NULL, 
[NomenclaturalTypeDesignation_TypeStatus] [nvarchar](50) NULL, 
[MaterialCategory] [nvarchar](50) NULL, 
[LogInsertedWhen] [smalldatetime] NULL, 
CONSTRAINT [PK_IdentificationUnit_ABCD] PRIMARY KEY CLUSTERED  
( 
[CollectionSpecimenID] ASC, 
[IdentificationUnitID] ASC, 
[SpecimenPartID] ASC, [ProjectID] ASC 
))
GO

ALTER TABLE [dbo].[IdentificationUnit_ABCD] ADD  CONSTRAINT [DF_IdentificationUnit_ABCD_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO


GRANT SELECT ON [IdentificationUnit_ABCD] TO [CacheUser]
GO

GRANT INSERT ON [IdentificationUnit_ABCD] TO [CacheAdministrator]
GO

GRANT UPDATE ON [IdentificationUnit_ABCD] TO [CacheAdministrator]
GO

GRANT DELETE ON [IdentificationUnit_ABCD] TO [CacheAdministrator]
GO
*/

--#####################################################################################################################
--######   [IdentificationCache]   ######################################################################################
--#####################################################################################################################

/*
CREATE TABLE [dbo].[Identification_ABCD]( 
 [CollectionSpecimenID] [int] NOT NULL, 
 [IdentificationUnitID] [int] NOT NULL,
 [IdentificationSequence] [int] NOT NULL,
 [IdentificationQualifier] [nvarchar](50) NULL, 
 [VernacularTerm] [nvarchar](255) NULL, 
 [TaxonomicName] [nvarchar](255) NULL, 
 [Notes] [nvarchar](max) NULL, 
 [TypeStatus] [nvarchar](50) NULL, 
 [SynonymName] [nvarchar](255) NULL, 
 [AcceptedName] [nvarchar](255) NULL, 
 [TaxonomicRank] [nvarchar](50) NULL, 
 [AcceptedNameURI] [varchar](255) NULL, 
 [GenusOrSupragenericName] [nvarchar](200) NULL, 
 [TaxonNameSinAuthor] [nvarchar](255) NULL, 
 [LastValidTaxonName] [nvarchar](255) NULL, 
 [LastValidTaxonWithQualifier] [nvarchar](255) NULL, 
 [LastValidTaxonNameSinAuthor] [nvarchar](255) NULL, 
 [LastValidTaxonIndex] [nvarchar](255) NULL, 
 [LastValidTaxonListOrder] [int] NOT NULL, 
 [ResponsibleName] [nvarchar](255) NULL
CONSTRAINT [PK_Identification_ABCD] PRIMARY KEY CLUSTERED  
( 
[CollectionSpecimenID] ASC, 
[IdentificationUnitID] ASC, 
[IdentificationSequence] ASC, [ProjectID] ASC 
))
GO

ALTER TABLE [dbo].[Identification_ABCD] ADD  CONSTRAINT [DF_Identification_ABCD_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO


GRANT SELECT ON [Identification_ABCD] TO [CacheUser]
GO

GRANT INSERT ON [Identification_ABCD] TO [CacheAdministrator]
GO

GRANT UPDATE ON [Identification_ABCD] TO [CacheAdministrator]
GO

GRANT DELETE ON [Identification_ABCD] TO [CacheAdministrator]
GO

*/
--#####################################################################################################################
--######   [CollectionSpecimenImageCache_ABCD]   ######################################################################################
--#####################################################################################################################
/*
CREATE TABLE [dbo].[CollectionSpecimenImageCache_ABCD]( 
[AccNrUnitID] [nvarchar](90) NOT NULL, 
[URI] [varchar] (255) NOT NULL, 
[MediaFormat] [nvarchar](50) NOT NULL, 
CONSTRAINT [PK_CollectionSpecimenImage_ABCD] PRIMARY KEY CLUSTERED  
( [AccNrUnitID] ASC, [URI] ASC, [ProjectID] ASC )) 
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the cached data from images and media.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenImage_ABCD'
GO


GRANT SELECT ON [CollectionSpecimenImage_ABCD] TO [CacheUser]
GO

GRANT INSERT ON [CollectionSpecimenImage_ABCD] TO [CacheAdministrator]
GO

GRANT UPDATE ON [CollectionSpecimenImage_ABCD] TO [CacheAdministrator]
GO

GRANT DELETE ON [CollectionSpecimenImage_ABCD] TO [CacheAdministrator]
GO
*/

--#####################################################################################################################
--######   [Identification_ABCD]   ######################################################################################
--#####################################################################################################################
/*
CREATE VIEW [dbo].[Identification_ABCD] 
AS 
SELECT I.CollectionSpecimenID, I.IdentificationUnitID, I.IdentificationQualifier, I.VernacularTerm, I.TaxonomicName, I.Notes, I.TypeStatus, T.SynonymName,  
  T.AcceptedName, T.TaxonomicRank, T.NameURI AS AcceptedNameURI, CASE WHEN T .GenusOrSupragenericName IS NULL  
  THEN CASE WHEN I.TaxonomicName IS NULL OR 
  len(rtrim(I.TaxonomicName)) = 0 THEN NULL ELSE CASE WHEN charindex(' ', I.TaxonomicName)  
  = 0 THEN I.TaxonomicName ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName))  
  END END ELSE T .GenusOrSupragenericName END  
AS GenusOrSupragenericName,  
 
CASE WHEN T .TaxonNameSinAuthor IS NULL  
  THEN CASE WHEN I.TaxonomicName IS NULL OR 
  len(rtrim(I.TaxonomicName)) = 0 THEN NULL ELSE CASE WHEN I.IdentificationQualifier IN ('sp.', 'sp. nov.', 'spp.') OR 
  T .TaxonomicRank = 'gen.' THEN CASE WHEN charindex(' ', I.TaxonomicName) = 0 THEN I.TaxonomicName + ' sp.' ELSE substring(I.TaxonomicName,  
  1, charindex(' ', I.TaxonomicName)) + 'sp.' END ELSE CASE WHEN charindex(' ', I.TaxonomicName)  
  = 0 THEN I.TaxonomicName + ' sp.' ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName) - 1) + substring(I.TaxonomicName,  
  charindex(' ', I.TaxonomicName), CASE WHEN charindex(' ', I.TaxonomicName, charindex(' ', I.TaxonomicName) + 1) > charindex(' ', I.TaxonomicName)  
  THEN charindex(' ', I.TaxonomicName, charindex(' ', I.TaxonomicName) + 1) - charindex(' ', I.TaxonomicName) ELSE len(I.TaxonomicName) END)  
  END END END ELSE T .TaxonNameSinAuthor END  
AS TaxonNameSinAuthor,  
 
CASE WHEN T.AcceptedName IS NULL  
  THEN CASE WHEN I.TaxonomicName IS NULL OR 
  I.TaxonomicName = '' THEN I.VernacularTerm ELSE I.TaxonomicName END ELSE T.AcceptedName END  
AS LastValidTaxonName,  
 
CASE WHEN T.AcceptedName IS NULL  
	THEN  
		CASE WHEN I.IdentificationQualifier IS NULL OR I.IdentificationQualifier = ''  
		THEN I.TaxonomicName  
		ELSE  
			CASE WHEN I.IdentificationQualifier = '?'  
			THEN I.IdentificationQualifier + ' ' + I.TaxonomicName 
			ELSE  
				CASE WHEN I.IdentificationQualifier IN ('sp.', 'sp. nov.', 'spp.')  
				THEN  
					CASE WHEN CHARINDEX(' ', I.TaxonomicName) = 0  
					THEN I.TaxonomicName  
					ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName) - 1)  
					END  
					+ ' ' + I.IdentificationQualifier  
				ELSE  
					CASE WHEN (I.IdentificationQualifier LIKE 'cf.%' OR I.IdentificationQualifier LIKE 'aff.%')  
					THEN  
						CASE WHEN I.IdentificationQualifier LIKE '% gen.'  
						THEN substring(I.IdentificationQualifier, 1, charindex('.', I.IdentificationQualifier)) + ' ' +  
							CASE WHEN charindex(' ', I.TaxonomicName) = 0 
							THEN I.TaxonomicName 
							ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName))  
							END 
						ELSE  
							CASE WHEN I.IdentificationQualifier LIKE '% sp.' OR I.IdentificationQualifier LIKE '% ssp.'  
							THEN rtrim(substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName)) + substring(I.IdentificationQualifier, 1, charindex('.', I.IdentificationQualifier)) + substring(I.TaxonomicName, charindex(' ', I.TaxonomicName), len(I.TaxonomicName)))  
							ELSE  
								CASE WHEN I.IdentificationQualifier LIKE '% var.' AND patindex('%var.%', I.TaxonomicName) > 0  
								THEN rtrim(substring(I.TaxonomicName, 1, patindex('%var.%', I.TaxonomicName) - 1) + I.IdentificationQualifier + substring(I.TaxonomicName, patindex('%var.%', I.TaxonomicName) + 4, len(I.TaxonomicName)))  
								ELSE  
									CASE WHEN I.IdentificationQualifier LIKE '% fm.' AND patindex('%fm.%', I.TaxonomicName) > 0  
									THEN rtrim(substring(I.TaxonomicName, 1, patindex('%fm.%', I.TaxonomicName) - 1) + I.IdentificationQualifier + substring(I.TaxonomicName, patindex('%fm.%', I.TaxonomicName) + 3, len(I.TaxonomicName)))  
									ELSE I.TaxonomicName  
									END  
								END  
							END  
						END  
					ELSE I.TaxonomicName  
					END  
				END  
			END  
		END  
	ELSE  
		CASE WHEN I.IdentificationQualifier IS NULL OR I.IdentificationQualifier = ''  
		THEN T.AcceptedName  
		ELSE  
			CASE WHEN I.IdentificationQualifier = '?'  
			THEN I.IdentificationQualifier + ' ' + T.AcceptedName 
			ELSE  
				CASE WHEN I.IdentificationQualifier IN ('sp.', 'sp. nov.', 'spp.')  
				THEN  
					CASE WHEN CHARINDEX(' ', T.AcceptedName) = 0  
					THEN T.AcceptedName  
					ELSE substring(T.AcceptedName, 1, charindex(' ', T.AcceptedName) - 1)  
					END + ' ' + I.IdentificationQualifier  
				ELSE  
					CASE WHEN (I.IdentificationQualifier LIKE 'cf.%' OR I.IdentificationQualifier LIKE 'aff.%')  
					THEN  
						CASE WHEN I.IdentificationQualifier LIKE '% gen.'  
						THEN substring(I.IdentificationQualifier, 1, charindex('.', I.IdentificationQualifier)) + ' ' + substring(T.AcceptedName, 1, charindex(' ', T.AcceptedName))  
						ELSE  
							CASE WHEN I.IdentificationQualifier LIKE '% sp.' OR I.IdentificationQualifier LIKE '% ssp.'  
							THEN rtrim(substring(T.AcceptedName, 1, charindex(' ', T.AcceptedName)) + substring(I.IdentificationQualifier, 1, charindex('.', I.IdentificationQualifier)) + substring(T.AcceptedName, charindex(' ', I.TaxonomicName), len(T.AcceptedName)))  
							ELSE  
								CASE WHEN I.IdentificationQualifier LIKE '% var.' AND patindex('%var.%', T.AcceptedName) > 0  
								THEN rtrim(substring(T.AcceptedName, 1, patindex('%var.%', T.AcceptedName) - 1) + I.IdentificationQualifier + substring(T.AcceptedName, patindex('%var.%', T.AcceptedName) + 4, len(T.AcceptedName)))  
								ELSE  
									CASE WHEN I.IdentificationQualifier LIKE '% fm.' AND patindex('%fm.%', T.AcceptedName) > 0  
									THEN rtrim(substring(T.AcceptedName, 1, patindex('%fm.%', T.AcceptedName) - 1) + I.IdentificationQualifier + substring(T.AcceptedName, patindex('%fm.%', T.AcceptedName) + 3, len(T.AcceptedName)))  
									ELSE T.AcceptedName  
								END  
							END  
						END  
					END  
				ELSE I.TaxonomicName  
				END  
			END  
		END  
	END  
END  
AS LastValidTaxonWithQualifier,  
 
CASE WHEN T .TaxonNameSinAuthor IS NULL  
THEN  
	CASE WHEN I.TaxonomicName IS NULL OR len(rtrim(I.TaxonomicName)) = 0  
	THEN NULL  
	ELSE  
			CASE WHEN charindex(' ', I.TaxonomicName) = 0  
			THEN I.TaxonomicName   
			ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName) - 1) + substring(I.TaxonomicName, charindex(' ', I.TaxonomicName),  
				CASE WHEN charindex(' ', I.TaxonomicName, charindex(' ', I.TaxonomicName) + 1) > charindex(' ', I.TaxonomicName)  
				THEN charindex(' ', I.TaxonomicName, charindex(' ', I.TaxonomicName) + 1) - charindex(' ', I.TaxonomicName)  
				ELSE len(I.TaxonomicName)  
				END)  
			END 
		END  
ELSE T .TaxonNameSinAuthor  
END 
AS LastValidTaxonNameSinAuthor, 
 
CASE WHEN T .AcceptedName IS NULL THEN CASE WHEN I.IdentificationQualifier IN ('sp.', 'cf. gen.') OR 
T .TaxonomicRank = 'gen.' THEN CASE WHEN CHARINDEX(' ', I.TaxonomicName)  
= 0 THEN I.TaxonomicName + ' sp.' ELSE substring(I.TaxonomicName, 1, charindex(' ', I.TaxonomicName) - 1)  
+ ' sp.' END ELSE I.TaxonomicName END ELSE CASE WHEN I.IdentificationQualifier IN ('sp.', 'cf. gen.') OR 
T .TaxonomicRank = 'gen.' THEN CASE WHEN CHARINDEX(' ', T .AcceptedName) = 0 THEN T .AcceptedName + ' sp.' ELSE substring(T .AcceptedName,  
1, charindex(' ', T .AcceptedName) - 1) + ' sp.' END ELSE T .AcceptedName END END  
AS LastValidTaxonIndex,  
 
CASE WHEN I.IdentificationQualifier IN ('sp.', 'cf. gen.') OR 
T .TaxonomicRank = 'gen.' THEN 1 ELSE 0 END  
AS LastValidTaxonListOrder,  
I.ResponsibleName 
FROM   IdentificationCache AS I INNER JOIN 
                      dbo.LastIdentification AS L ON I.CollectionSpecimenID = L.CollectionSpecimenID AND I.IdentificationUnitID = L.IdentificationUnitID AND  
                      I.IdentificationSequence = L.IdentificationSequence LEFT OUTER JOIN 
                      dbo.TaxonSynonymy AS T ON I.NameURI = T.SynNameURI   
GO

GRANT SELECT ON [Identification_ABCD] TO [CacheUser]
GO
**/

--#####################################################################################################################
--######   [IdentificationUnit_ABCD]   ######################################################################################
--#####################################################################################################################

/*
CREATE VIEW [dbo].[IdentificationUnit_ABCD] 
AS 
SELECT U.CollectionSpecimenID, U.IdentificationUnitID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID, U.RelationType, U.ExsiccataNumber, U.DisplayOrder,  
U.ColonisedSubstratePart, I.IdentificationQualifier, I.VernacularTerm, I.TaxonomicName, I.Notes, I.TypeStatus, I.SynonymName, I.AcceptedName, I.AcceptedNameURI,  
I.TaxonomicRank, I.GenusOrSupragenericName, I.TaxonNameSinAuthor, F.TaxonomicNameFirstEntry, I.LastValidTaxonName, I.LastValidTaxonWithQualifier,  
I.LastValidTaxonNameSinAuthor, I.LastValidTaxonIndex, I.LastValidTaxonListOrder, U.FamilyCache, U.OrderCache, U.LifeStage, U.Gender,  
CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber + ' / ' ELSE '' END + CAST(U.IdentificationUnitID AS varchar(90)) AS AccNrUnitID 
FROM  IdentificationUnitCache AS U INNER JOIN 
IdentificationCache AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID INNER JOIN 
dbo.IdentificationFirstEntry AS F ON U.CollectionSpecimenID = F.CollectionSpecimenID AND U.IdentificationUnitID = F.IdentificationUnitID INNER JOIN 
dbo.CollectionSpecimenCache AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN 
dbo.ProjectTaxonomicGroup AS T ON U.TaxonomicGroup = T.TaxonomicGroup INNER JOIN 
dbo.CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID AND T.ProjectID = P.ProjectID  

GRANT SELECT ON [IdentificationUnit_ABCD] TO [CacheUser]

CREATE VIEW [dbo].[Test_IdentificationUnit_AcceptedName_Duplicates_ABCD] 
AS SELECT CollectionSpecimenID, IdentificationUnitID, MIN(AcceptedName) AS AcceptedName, MAX(AcceptedName) AS _AcceptedName, MIN(AcceptedNameURI) 
AS AcceptedNameURI, MAX(AcceptedNameURI) AS _AcceptedNameURI 
FROM dbo.IdentificationUnit 
GROUP BY CollectionSpecimenID, IdentificationUnitID 
HAVING (COUNT(*) > 1) AND (MIN(AcceptedNameURI) <> MAX(AcceptedNameURI))

GRANT SELECT ON [Test_IdentificationUnit_AcceptedName_Duplicates_ABCD] TO [CacheUser]


-- STORAGE

CREATE VIEW [dbo].[CollectionStorage_ABCD] 
AS SELECT CollectionSpecimenID, MaterialCategory,  
CASE WHEN MaterialCategory IN ('drawing', 'icones', 'photogr. print', 'photogr. slide', 'machine observation')  
	THEN 'DrawingOrPhotograph'  
	ELSE  
		CASE WHEN MaterialCategory IN ('cultures')  
		THEN 'LivingSpecimen'  
		ELSE 'PreservedSpecimen' END  
	END AS RecordBasis,  
CASE WHEN MaterialCategory IN ('drawing', 'icones', 'photogr. print', 'photogr. slide', 'other specimen', 'cultures', 'vial', 'machine observation', 'DNA sample')  
	THEN NULL  
	ELSE  
		CASE WHEN MaterialCategory IN ('micr. slide', 'SEM table', 'TEM specimen')  
		THEN 'microscopic preparation'  
		ELSE  
			CASE WHEN MaterialCategory IN ('herbarium sheets')  
			THEN 'dried and pressed'  
			ELSE  
				CASE WHEN MaterialCategory IN ('cultures')  
				THEN 'no treatment'  
				ELSE 'dried' END  
			END  
		END  
	END AS PreparationType,  
CASE MaterialCategory  
WHEN 'specimen' THEN 0  
WHEN 'herbarium sheets' THEN 1  
WHEN 'drawing' THEN 2  
WHEN 'icones' THEN 3  
WHEN 'photogr. print' THEN 4  
WHEN 'photogr. slide' THEN 5  
WHEN 'cultures' THEN 6  
WHEN 'micr. slide' THEN 7  
WHEN 'SEM table' THEN 8  
WHEN 'TEM specimen' THEN 9  
WHEN 'cultures' THEN 10  
ELSE 100 END AS MaterialCategoryOrder,  
MAX(StorageLocation) AS StorageLocation 
FROM         CollectionSpecimenPart 
GROUP BY CollectionSpecimenID, MaterialCategory  

GRANT SELECT ON [CollectionStorage] TO [CacheUser]


-- Embargo

CREATE VIEW CollectionSpecimenEmbargo_ABCD AS SELECT S.CollectionSpecimenID 
FROM  DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimenTransaction AS S INNER JOIN 
                    DiversityWorkbench.Settings.DatabaseName + ".dbo.[Transaction] AS T ON S.TransactionID = T.TransactionID 
WHERE (T.TransactionType = N'embargo')  
AND GETDATE() BETWEEN T.BeginDate AND T.AgreedEndDate 
OR (T.BeginDate is null) AND (T.AgreedEndDate >= GETDATE()) 
OR (T.BeginDate <= GETDATE()) AND (T.AgreedEndDate is null)   

GRANT SELECT ON [CollectionSpecimenEmbargo_ABCD] TO [CacheUser]


-- PARTS

CREATE VIEW [dbo].[CollectionSpecimenPart_ABCD] 
AS 
SELECT  CollectionSpecimenID, SpecimenPartID, DerivedFromSpecimenPartID, PreparationMethod, PreparationDate, AccessionNumber, PartSublabel, CollectionID,  
MaterialCategory, StorageLocation, Stock, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID, DataWithholdingReason 
FROM  CollectionSpecimenPart AS P 
WHERE (DataWithholdingReason = N'') OR 
(DataWithholdingReason IS NULL)   

GRANT SELECT ON [CollectionSpecimenPart_ABCD] TO [CacheUser]


-- SPECIMEN

CREATE VIEW [dbo].[CollectionSpecimen_ABCD] 
AS 
SELECT S.CollectionSpecimenID, NULL AS Version, S.AccessionNumber, CASE WHEN 1 = 1 THEN CASE WHEN E.CollectionYear IS NULL THEN NULL  
ELSE CAST(E.CollectionYear AS VARCHAR) + CASE WHEN E.CollectionMonth IS NULL THEN '' ELSE + '-' + REPLICATE('0',  
2 - LEN(CAST(E.CollectionMonth AS VARCHAR))) + CAST(E.CollectionMonth AS VARCHAR) + CASE WHEN E.CollectionDay IS NULL THEN '' ELSE '-' + REPLICATE('0',  
2 - LEN(CAST(E.CollectionDay AS VARCHAR))) + CAST(E.CollectionDay AS VARCHAR) END END END ELSE NULL END AS CollectionDate, E.CollectionDay,  
E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache, S.LabelTranscriptionNotes,  
CASE WHEN S.ExsiccataAbbreviation LIKE '<%>' THEN NULL ELSE S.ExsiccataAbbreviation END AS ExsiccataAbbreviation, S.OriginalNotes,  
E.CollectorsEventNumber, E.DataWithholdingReason AS EventDataWithholdingReason, EC.Display AS Chronostratigraphy, EL.DisplayText AS Lithostratigraphy 
FROM dbo.CollectionEventLithostratigraphy AS EL RIGHT OUTER JOIN 
dbo.CollectionEvent AS E ON EL.CollectionEventID = E.CollectionEventID LEFT OUTER JOIN 
dbo.CollectionEventChronostratigraphy AS EC ON E.CollectionEventID = EC.CollectionEventID RIGHT OUTER JOIN 
CollectionSpecimen AS S INNER JOIN 
dbo.CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID ON E.CollectionEventID = S.CollectionEventID 
WHERE (S.DataWithholdingReason = '' OR 
S.DataWithholdingReason IS NULL) AND (S.CollectionSpecimenID NOT IN 
(SELECT CollectionSpecimenID 
FROM dbo.CollectionSpecimenEmbargo)) 
GROUP BY S.CollectionSpecimenID, S.AccessionNumber, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription,  
E.CountryCache, S.LabelTranscriptionNotes, S.OriginalNotes, E.CollectorsEventNumber, E.DataWithholdingReason, EC.Display, EL.DisplayText,  
S.ExsiccataAbbreviation  

GRANT SELECT ON [CollectionSpecimen_ABCD] TO [CacheUser]


CREATE VIEW [dbo].[CollectionSpecimen_Part_ABCD] 
AS 
SELECT     S.CollectionSpecimenID, NULL AS Version, SP.AccessionNumber, CASE WHEN 1 = 1 THEN CASE WHEN E.CollectionYear IS NULL THEN NULL  
ELSE CAST(E.CollectionYear AS VARCHAR) + CASE WHEN E.CollectionMonth IS NULL THEN '' ELSE + '-' + REPLICATE('0',  
2 - LEN(CAST(E.CollectionMonth AS VARCHAR))) + CAST(E.CollectionMonth AS VARCHAR) + CASE WHEN E.CollectionDay IS NULL THEN '' ELSE '-' + REPLICATE('0',  
2 - LEN(CAST(E.CollectionDay AS VARCHAR))) + CAST(E.CollectionDay AS VARCHAR) END END END ELSE NULL END AS CollectionDate, E.CollectionDay,  
E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache, S.LabelTranscriptionNotes,  
CASE WHEN S.ExsiccataAbbreviation LIKE '<%>' THEN NULL ELSE S.ExsiccataAbbreviation END AS ExsiccataAbbreviation, S.OriginalNotes,  
E.CollectorsEventNumber, E.DataWithholdingReason AS EventDataWithholdingReason, C.Display AS Chronostratigraphy, L.DisplayText AS Lithostratigraphy,  
P.ProjectID 
FROM         dbo.CollectionSpecimenPart AS SP INNER JOIN 
CollectionSpecimen AS S INNER JOIN 
dbo.CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID ON SP.CollectionSpecimenID = S.CollectionSpecimenID LEFT OUTER JOIN 
dbo.CollectionEventLithostratigraphy AS L RIGHT OUTER JOIN 
dbo.CollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID LEFT OUTER JOIN 
dbo.CollectionEventChronostratigraphy AS C ON E.CollectionEventID = C.CollectionEventID ON S.CollectionEventID = E.CollectionEventID 
WHERE     (S.DataWithholdingReason = '' OR 
S.DataWithholdingReason IS NULL) AND (SP.AccessionNumber <> N'') AND (S.CollectionSpecimenID NOT IN 
(SELECT     CollectionSpecimenID 
FROM          dbo.CollectionSpecimenEmbargo)) 
GROUP BY S.CollectionSpecimenID, E.CollectionDay, E.CollectionMonth, E.CollectionYear, E.CollectionDateSupplement, E.LocalityDescription, E.CountryCache,  
S.LabelTranscriptionNotes, S.OriginalNotes, E.CollectorsEventNumber, E.DataWithholdingReason, C.Display, L.DisplayText, S.ExsiccataAbbreviation,  
SP.AccessionNumber, P.ProjectID  

GRANT SELECT ON [CollectionSpecimen_Part_ABCD] TO [CacheUser]


CREATE  PROCEDURE [dbo].[CacheDB_procRefreshCoordinates_ABCD] @ProjectID int 
AS 
DECLARE @LocalisationSystemID int 
DECLARE @TempLocalisationSystem TABLE (OrderSequence int primary key, LocalisationSystemID int) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (1, 8) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (2, 6) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (3, 2) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (4, 9) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (5, 1) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (6, 3) 
	insert into @TempLocalisationSystem (OrderSequence, LocalisationSystemID) values (7, 7) 
while ((select count (*) from @TempLocalisationSystem) > 0)  
begin 
	set @LocalisationSystemID = (select min(LocalisationSystemID) from @TempLocalisationSystem where OrderSequence = (select min(OrderSequence) from @TempLocalisationSystem)) 
	UPDATE CC 
	SET CC.Latitude = CASE WHEN CC.Latitude IS NULL THEN L.AverageLatitudeCache ELSE CC.Latitude END, 
	CC.Longitude = CASE WHEN CC.Longitude IS NULL THEN L.AverageLongitudeCache ELSE CC.Longitude END 
	FROM CollectionSpecimenCache CC,  
	 DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimen S, 
	 DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionEvent E, 
	 DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionEventLocalisation L, 
	 DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P 
	WHERE CC.CollectionSpecimenID = S.CollectionSpecimenID 
	AND S.CollectionEventID = E.CollectionEventID 
	AND E.CollectionEventID = L.CollectionEventID 
	AND L.LocalisationSystemID = @LocalisationSystemID 
	AND S.CollectionSpecimenID = P.CollectionSpecimenID 
	AND P.ProjectID = @ProjectID 
	delete @TempLocalisationSystem where LocalisationSystemID = @LocalisationSystemID 
end  

GRANT EXEC ON [CacheDB_procRefreshCoordinates_ABCD] TO [CacheUser]

*/

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '03.00.03'
END

GO


