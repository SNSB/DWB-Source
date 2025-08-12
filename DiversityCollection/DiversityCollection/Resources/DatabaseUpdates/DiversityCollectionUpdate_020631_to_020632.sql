declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.31'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  New LocalisationSystem UTM  #################################################################################
--#####################################################################################################################

INSERT INTO LocalisationSystem
(LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, 
Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, DescriptionLocation2)
VALUES        (22, 9, 'UTM', 'UTM', 'UTM', 1, 122, 'UTM', 'East', 'East', 'North', 'North')

GO

-- UTM

INSERT INTO [dbo].[Entity]
           ([Entity])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22'
           ,'en-US'
           ,'General'
           ,'UTM'
           ,'UTM'
           ,'The UTM grid based on WGS84')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22'
           ,'de-DE'
           ,'General'
           ,'UTM'
           ,'UTM'
           ,'Das UTM-Koordinatensystem basieren auf WGS84')
GO


--East

INSERT INTO [dbo].[Entity]
           ([Entity])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.East')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.East'
           ,'en-US'
           ,'General'
           ,'East'
           ,'East'
           ,'The grid zone followed by the east value of the UTM coordinates')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.East'
           ,'de-DE'
           ,'General'
           ,'Ost'
           ,'Ost'
           ,'Das Planquadrat gefolgt vom Ostwert der UTM-Koordinaten')
GO



--North

INSERT INTO [dbo].[Entity]
           ([Entity])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.North')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.North'
           ,'en-US'
           ,'General'
           ,'North'
           ,'North'
           ,'The north value of the UTM coordinates')
GO

INSERT INTO [dbo].[EntityRepresentation]
           ([Entity]
           ,[LanguageCode]
           ,[EntityContext]
           ,[DisplayText]
           ,[Abbreviation]
           ,[Description])
     VALUES
           ('LocalisationSystem.LocalisationSystemID.22.North'
           ,'de-DE'
           ,'General'
           ,'Nord'
           ,'Nord'
           ,'Der Nordwert der UTM-Koordinaten')
GO



--#####################################################################################################################
--######  CollectionSpecimenID_CanEdit  ###############################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionSpecimenID_CanEdit]
AS
SELECT     P.CollectionSpecimenID
FROM         dbo.CollectionProject AS P INNER JOIN
                      dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID 
					  AND U.ReadOnly = 0
					  AND (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME())
GO

GRANT SELECT ON [dbo].[CollectionSpecimenID_CanEdit] TO [User]
GO

--#####################################################################################################################
--######   CollectionEventID_CanEdit    ###############################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[CollectionEventID_CanEdit]
AS
SELECT        S.CollectionEventID
FROM            dbo.CollectionSpecimen AS S INNER JOIN
                         dbo.CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID 
						 AND (NOT (S.CollectionEventID IS NULL)) INNER JOIN
                         dbo.ProjectUser AS U ON P.ProjectID = U.ProjectID
						 AND (U.LoginName = USER_NAME() OR U.LoginName = sUSER_sNAME()) 
						 AND (U.ReadOnly = 0) 
GO

GRANT SELECT ON [dbo].[CollectionEventID_CanEdit] TO [User]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.32'
END

GO

