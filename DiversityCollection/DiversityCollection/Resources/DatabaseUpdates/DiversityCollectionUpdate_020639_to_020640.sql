declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.39'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   CollectionSpecimen_Core2: converting logging user from ID to Text    #######################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimen_Core2]
AS
SELECT        S.CollectionSpecimenID, S.Version, S.CollectionEventID, S.CollectionID, S.AccessionNumber, S.AccessionDate, S.AccessionDay, S.AccessionMonth, S.AccessionYear, S.AccessionDateSupplement, S.AccessionDateCategory, 
                         S.DepositorsName, S.DepositorsAgentURI, S.DepositorsAccessionNumber, S.LabelTitle, S.LabelType, S.LabelTranscriptionState, S.LabelTranscriptionNotes, S.ExsiccataURI, S.ExsiccataAbbreviation, S.OriginalNotes, 
                         S.AdditionalNotes, S.ReferenceTitle, S.ReferenceURI, S.Problems, S.DataWithholdingReason, CASE WHEN E.CollectionDate IS NULL AND E.CollectionYear IS NULL AND E.CollectionMonth IS NULL AND E.CollectionDay IS NULL 
                         THEN NULL ELSE CASE WHEN E.CollectionDate IS NULL THEN CASE WHEN E.CollectionYear IS NULL THEN '----' ELSE CAST(E.CollectionYear AS varchar) 
                         END + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CASE WHEN E.CollectionMonth IS NULL THEN '--' ELSE CAST(E.CollectionMonth AS varchar) END + '/' + CASE WHEN E.CollectionDay IS NULL 
                         THEN '--' ELSE CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(E.CollectionDay AS varchar) END ELSE CAST(year(E.CollectionDate) AS varchar) 
                         + '/' + CASE WHEN CollectionMonth < 10 THEN '0' ELSE '' END + CAST(CollectionMonth AS varchar) + '/' + CASE WHEN CollectionDay < 10 THEN '0' ELSE '' END + CAST(CollectionDay AS varchar) END END AS CollectionDate, 
                         E.LocalityDescription AS Locality, E.HabitatDescription AS Habitat, S.InternalNotes, S.ExternalDatasourceID, S.ExternalIdentifier, S.LogCreatedWhen, 
						 CASE WHEN UC.LoginName IS NULL THEN S.LogCreatedBy ELSE UC.LoginName END AS LogCreatedBy, S.LogUpdatedWhen, 
						 CASE WHEN UU.LoginName IS NULL THEN S.LogUpdatedBy ELSE UU.LoginName END AS LogUpdatedBy
FROM            CollectionSpecimen AS S INNER JOIN
                         CollectionSpecimenID_Available AS UA ON S.CollectionSpecimenID = UA.CollectionSpecimenID LEFT OUTER JOIN
                         UserProxy AS UU ON S.LogUpdatedBy = CAST(UU.ID AS NVARCHAR) LEFT OUTER JOIN
                         UserProxy AS UC ON S.LogCreatedBy = CAST(UC.ID AS NVARCHAR) LEFT OUTER JOIN
                         CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID
GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.40'
END

GO

