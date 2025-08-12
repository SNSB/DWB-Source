declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.92'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   ProjectListNotReadOnly  ####################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[ProjectListNotReadOnly]
AS
SELECT     TOP 100 PERCENT P.Project, U.ProjectID
FROM         dbo.ProjectUser U INNER JOIN
                      dbo.ProjectProxy P ON U.ProjectID = P.ProjectID
WHERE     U.LoginName = USER_NAME() AND (U.[ReadOnly] = 0 OR U.[ReadOnly] IS NULL)
GROUP BY P.Project, U.ProjectID
ORDER BY P.Project


GO

GRANT SELECT ON ProjectListNotReadOnly TO [User]
GO


--#####################################################################################################################
--######   CollectionEventID_AvailableNotReadOnly  ####################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionEventID_AvailableNotReadOnly]
AS
SELECT     S.CollectionEventID
FROM         dbo.CollectionSpecimen S INNER JOIN
                      dbo.CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN
                      dbo.ProjectUser U ON P.ProjectID = U.ProjectID
WHERE     (U.LoginName = USER_NAME() and U.ReadOnly = 0 and NOT (S.CollectionEventID IS NULL))
GROUP BY S.CollectionEventID

GO

grant select on [CollectionEventID_AvailableNotReadOnly] to [User]
go



--#####################################################################################################################
--######   CollectionEvent_Core2  #####################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionEvent_Core2]
AS
SELECT E.CollectionEventID, E.Version, E.SeriesID, E.CollectorsEventNumber, E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, 
	E.CollectionEndDay, E.CollectionEndMonth, E.CollectionEndYear, 
	E.CollectionDateSupplement, E.CollectionDateCategory, E.CollectionTime, E.CollectionTimeSpan, 
	E.LocalityDescription, E.LocalityVerbatim, E.HabitatDescription, E.ReferenceTitle, E.ReferenceDetails, 
	E.ReferenceURI, E.CollectingMethod, E.Notes, E.CountryCache, E.DataWithholdingReason, E.DataWithholdingReasonDate
FROM CollectionEvent AS E INNER JOIN
	CollectionEventID_AvailableNotReadOnly AS A ON E.CollectionEventID = A.CollectionEventID
GO

grant select on [CollectionEvent_Core2] to [User]
go


--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.06' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.93'
END

GO

