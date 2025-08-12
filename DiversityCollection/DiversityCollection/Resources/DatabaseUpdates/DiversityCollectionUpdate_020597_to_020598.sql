declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.97'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollectionSpecimenProcessing  ##############################################################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenProcessing ALTER COLUMN ProcessingDate datetime NULL
GO

if (select count(*) from sys.default_constraints where name = 'DF_CollectionSpecimenPreparation_CollectorsSequence') > 0
begin
	ALTER TABLE [dbo].[CollectionSpecimenProcessing] DROP CONSTRAINT [DF_CollectionSpecimenPreparation_CollectorsSequence]
end
GO


--#####################################################################################################################
--######   CollectionSpecimenPartDescription  #########################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[CollectionSpecimenPartDescription] TO [User];
GO

GRANT DELETE ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GO

GRANT UPDATE ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GO

GRANT INSERT ON dbo.[CollectionSpecimenPartDescription] TO [Editor];
GO

GRANT INSERT ON dbo.[CollectionSpecimenPartDescription_log] TO [Editor];
GO

--#####################################################################################################################
--######  CacheAdmin inherits grants of CacheUser  ####################################################################
--#####################################################################################################################

EXEC sp_addrolemember N'CacheUser', N'CacheAdmin';
GO

EXEC sp_addrolemember N'CacheAdmin', N'Administrator';
GO




--#####################################################################################################################
--######   CollectionSpecimenID_AvailableReadOnly  ####################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[CollectionSpecimenID_AvailableReadOnly]
AS
SELECT        P.CollectionSpecimenID
FROM            dbo.CollectionProject AS P INNER JOIN
                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
WHERE        (U.LoginName = USER_NAME()) AND (U.ReadOnly = 1) AND (P.CollectionSpecimenID NOT IN
                             (SELECT        P.CollectionSpecimenID
                               FROM            dbo.CollectionProject AS P INNER JOIN
                                                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
                               WHERE        (U.LoginName = USER_NAME()) AND (U.ReadOnly = 0)))
GROUP BY P.CollectionSpecimenID

GO

--#####################################################################################################################
--######   setting the Client Version    ##############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.08.08' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.98'
END

GO

