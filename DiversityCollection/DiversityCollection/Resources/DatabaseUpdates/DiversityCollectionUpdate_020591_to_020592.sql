declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.91'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   Tippfehler                    ##############################################################################
--#####################################################################################################################

EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'Link to the source for further informations about the agent, e.g in the module DiversityAgents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionAgent', @level2type=N'COLUMN',@level2name=N'AgentURI'
GO


--#####################################################################################################################
--######   New views to allow access to read only datasets   ##########################################################
--#####################################################################################################################
--######   CollectionSpecimenID_AvailableReadOnly   ###################################################################
--#####################################################################################################################


CREATE VIEW [dbo].[CollectionSpecimenID_AvailableReadOnly]
AS
SELECT     P.CollectionSpecimenID
FROM         dbo.CollectionProject P INNER JOIN
                      dbo.ProjectUser U ON P.ProjectID = U.ProjectID
WHERE     (U.LoginName = USER_NAME() and U.ReadOnly = 1)
GROUP BY P.CollectionSpecimenID

GO

GRANT SELECT ON [CollectionSpecimenID_AvailableReadOnly] TO [User]
GO

--#####################################################################################################################
--######   CollectionSpecimenID_Available   ###########################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionSpecimenID_Available]
AS
SELECT     P.CollectionSpecimenID
FROM         dbo.CollectionProject AS P INNER JOIN
                      dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE     (U.LoginName = USER_NAME())
GROUP BY P.CollectionSpecimenID
UNION
SELECT     TOP (100) PERCENT S.CollectionSpecimenID
FROM         dbo.CollectionProject AS P RIGHT OUTER JOIN
                      dbo.CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID
GROUP BY S.CollectionSpecimenID, P.ProjectID
HAVING      (P.ProjectID IS NULL)
ORDER BY CollectionSpecimenID

GO

GRANT SELECT ON [CollectionSpecimenID_Available] TO [User]
GO


--#####################################################################################################################
--######   CollectionSpecimen_Core2   #################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionSpecimen_Core2]
AS
SELECT     S.CollectionSpecimenID, S.Version, S.CollectionEventID, S.CollectionID, S.AccessionNumber, S.AccessionDate, S.AccessionDay, S.AccessionMonth, S.AccessionYear, 
                      S.AccessionDateSupplement, S.AccessionDateCategory, S.DepositorsName, S.DepositorsAgentURI, S.DepositorsAccessionNumber, S.LabelTitle, S.LabelType, 
                      S.LabelTranscriptionState, S.LabelTranscriptionNotes, S.ExsiccataURI, S.ExsiccataAbbreviation, S.OriginalNotes, S.AdditionalNotes, S.ReferenceTitle, 
                      S.ReferenceURI, S.Problems, S.DataWithholdingReason, CASE WHEN E.CollectionDate IS NULL AND E.CollectionYear IS NULL AND E.CollectionMonth IS NULL AND 
                      E.CollectionDay IS NULL THEN NULL ELSE CASE WHEN E.CollectionDate IS NULL THEN CASE WHEN E.CollectionYear IS NULL 
                      THEN '----' ELSE CAST(E.CollectionYear AS varchar) END + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CASE WHEN E.CollectionMonth IS NULL 
                      THEN '--' ELSE CAST(E.CollectionMonth AS varchar) END + '/' + CASE WHEN E.CollectionDay IS NULL 
                      THEN '--' ELSE CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(E.CollectionDay AS varchar) END ELSE CAST(year(E.CollectionDate) AS varchar) 
                      + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(month(E.CollectionDate) AS varchar) 
                      + '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(day(E.CollectionDate) AS varchar) END END + CASE WHEN E.LocalityDescription IS NULL 
                      THEN '' ELSE '   ' + E.LocalityDescription END AS CollectionDate, E.LocalityDescription AS Locality, E.HabitatDescription AS Habitat, S.InternalNotes, 
                      S.ExternalDatasourceID, S.ExternalIdentifier, S.LogCreatedWhen, S.LogCreatedBy, S.LogUpdatedWhen, S.LogUpdatedBy
FROM         dbo.CollectionSpecimen AS S INNER JOIN
                      dbo.CollectionSpecimenID_Available AS UA ON S.CollectionSpecimenID = UA.CollectionSpecimenID LEFT OUTER JOIN
                      dbo.CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID

GO

GRANT SELECT ON CollectionSpecimen_Core2 TO [User]
GO




--#####################################################################################################################
--######   Enable settings in UserProxy  ##############################################################################
--#####################################################################################################################

GRANT UPDATE ON dbo.[UserProxy] TO [Editor];
GO



--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.07' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.90'
END

GO

