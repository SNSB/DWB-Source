declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.04'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######  Regulation - redesign, change of key etc.   #################################################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  Regulation: Add Status    ###################################################################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Regulation' and C.COLUMN_NAME = 'Status') = 0
begin
ALTER TABLE Regulation ADD [Status] nvarchar(500) NULL;
ALTER TABLE Regulation_log ADD [Status] nvarchar(500) NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The status of the permit, i.e. E-mail notification, signed agreement, other (please specify)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'Status'
end
GO

--#####################################################################################################################
--######  Regulation: Add ValidUntil    ###############################################################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Regulation' and C.COLUMN_NAME = 'ValidUntil') = 0
begin
ALTER TABLE Regulation ADD [ValidUntil] date NULL;
ALTER TABLE Regulation_log ADD [ValidUntil] date NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date until which the regulation is vaild' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'ValidUntil'
end
GO


--#####################################################################################################################
--######  Regulation: Add HierarchyOnly    ############################################################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Regulation' and C.COLUMN_NAME = 'HierarchyOnly') = 0
begin
ALTER TABLE Regulation ADD HierarchyOnly bit NULL DEFAULT 0;
ALTER TABLE Regulation_log ADD HierarchyOnly bit NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If this entry is not a regluation but for the organization of the hierarchy only' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'HierarchyOnly'
end
GO

UPDATE R 
SET R.HierarchyOnly = 0
FROM Regulation R


--#####################################################################################################################
--######  Regulation: Add ParentRegulation    #########################################################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'Regulation' and C.COLUMN_NAME = 'ParentRegulation') = 0
begin
ALTER TABLE Regulation ADD [ParentRegulation] nvarchar(400) NULL;
ALTER TABLE Regulation_log ADD [ParentRegulation] nvarchar(400) NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the parent regulation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'ParentRegulation'
end
GO


-- fill ParentRegulation
UPDATE R 
SET R.ParentRegulation = P.Regulation
FROM Regulation R, Regulation P
WHERE R.ParentRegulationID = P.RegulationID


--#####################################################################################################################
--######  CollectionSpecimenPartRegulation - add column Regulation    #################################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenPartRegulation' and C.COLUMN_NAME = 'Regulation') = 0
begin
ALTER TABLE CollectionSpecimenPartRegulation ADD [Regulation] nvarchar(400) NULL;
ALTER TABLE CollectionSpecimenPartRegulation_log ADD [Regulation] nvarchar(400) NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the regulation, refers to table Regulation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'Regulation'
end
GO

-- fill Regulation if present
if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenPartRegulation' and C.COLUMN_NAME = 'Regulation') = 1
begin
	UPDATE P 
	SET P.Regulation = R.Regulation
	FROM Regulation R, CollectionSpecimenPartRegulation P
	WHERE R.RegulationID = P.RegulationID
end
GO

--#####################################################################################################################
--######  CollectionEventRegulation - fill column Regulation    #######################################################
--#####################################################################################################################

UPDATE E 
SET E.Regulation = R.Regulation
FROM Regulation R, CollectionEventRegulation E
WHERE R.RegulationID = E.RegulationID

GO

--#####################################################################################################################
--######  CollectionSpecimenPartRegulation - add column [CollectionEventID]    ########################################
--#####################################################################################################################

if (Select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = 'CollectionSpecimenPartRegulation' and C.COLUMN_NAME = 'CollectionEventID') = 0
begin
ALTER TABLE CollectionSpecimenPartRegulation ADD [CollectionEventID] int NULL;
ALTER TABLE CollectionSpecimenPartRegulation_log ADD [CollectionEventID] int NULL;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the collection event during which the objects where collected, refers to table CollectionEvent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'CollectionEventID'
end
GO

-- fill Regulation
UPDATE P 
SET P.[CollectionEventID] = E.[CollectionEventID]
FROM CollectionSpecimen S, CollectionSpecimenPartRegulation P, CollectionEventRegulation E
WHERE S.CollectionSpecimenID = P.CollectionSpecimenID
AND E.CollectionEventID = S.CollectionEventID

DELETE P 
FROM CollectionSpecimenPartRegulation P
WHERE P.Regulation IS NULL
OR P.[CollectionEventID] IS NULL
GO

DELETE P 
FROM [dbo].[CollectionSpecimenPartRegulation] P left join [dbo].[CollectionEventRegulation] E
on P.CollectionEventID = e.CollectionEventID and p.Regulation = e.Regulation
where e.CollectionEventID is null

--#####################################################################################################################
--######  Remove all relevant keys that have to be adapted   ##########################################################
--#####################################################################################################################

--foreign keys
if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_CollectionEventSeriesRegulation_Regulation') > 0
begin
ALTER TABLE [dbo].[CollectionEventSeriesRegulation] DROP CONSTRAINT [FK_CollectionEventSeriesRegulation_Regulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_CollectionSpecimenPartRegulation_Regulation') > 0
begin
ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] DROP CONSTRAINT [FK_CollectionSpecimenPartRegulation_Regulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_CollectionSpecimenPartRegulation_CollectionEventRegulation') > 0
begin
ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] DROP CONSTRAINT [FK_CollectionSpecimenPartRegulation_CollectionEventRegulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_CollectionEventRegulation_Regulation') > 0
begin
ALTER TABLE [dbo].[CollectionEventRegulation] DROP CONSTRAINT [FK_CollectionEventRegulation_Regulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R where r.CONSTRAINT_NAME = 'FK_Regulation_Regulation') > 0
begin
ALTER TABLE [dbo].[Regulation] DROP CONSTRAINT [FK_Regulation_Regulation]
end
GO

--primary keys

if (select count(*) from INFORMATION_SCHEMA.TABLE_CONSTRAINTS P where P.CONSTRAINT_TYPE = 'PRIMARY KEY' and p.TABLE_NAME = 'CollectionSpecimenPartRegulation' and p.CONSTRAINT_NAME = 'PK_CollectionSpecimenPartRegulation') > 0
begin
ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] DROP CONSTRAINT [PK_CollectionSpecimenPartRegulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.TABLE_CONSTRAINTS P where P.CONSTRAINT_TYPE = 'PRIMARY KEY' and p.TABLE_NAME = 'CollectionEventRegulation' and p.CONSTRAINT_NAME = 'PK_CollectionEventRegulation') > 0
begin
ALTER TABLE [dbo].[CollectionEventRegulation] DROP CONSTRAINT [PK_CollectionEventRegulation]
end
GO

if (select count(*) from INFORMATION_SCHEMA.TABLE_CONSTRAINTS P where P.CONSTRAINT_TYPE = 'PRIMARY KEY' and p.TABLE_NAME = 'Regulation' and p.CONSTRAINT_NAME = 'PK_Regulation') > 0
begin
ALTER TABLE [dbo].[Regulation] DROP CONSTRAINT [PK_Regulation]
end
GO



--#####################################################################################################################
--######  Adding the new keys for table Regulation  ###################################################################
--#####################################################################################################################

ALTER TABLE Regulation ALTER COLUMN Regulation nvarchar(400) NOT NULL;
GO


ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [PK_Regulation] PRIMARY KEY CLUSTERED 
(
	[Regulation] ASC
)
GO


ALTER TABLE [dbo].[Regulation]  WITH CHECK ADD  CONSTRAINT [FK_Regulation_Regulation] FOREIGN KEY([ParentRegulation])
REFERENCES [dbo].[Regulation] ([Regulation])
GO

ALTER TABLE [dbo].[Regulation] CHECK CONSTRAINT [FK_Regulation_Regulation]
GO


--#####################################################################################################################
--######  Adding the new keys for table CollectionEventRegulation  ####################################################
--#####################################################################################################################

ALTER TABLE CollectionEventRegulation ALTER COLUMN [CollectionEventID] int NOT NULL;
GO

ALTER TABLE CollectionEventRegulation ALTER COLUMN [Regulation] nvarchar(400) NOT NULL;
GO

ALTER TABLE CollectionEventRegulation ALTER COLUMN [RegulationID] int NULL;
GO


ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [PK_CollectionEventRegulation] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[Regulation] ASC
)
GO


ALTER TABLE [dbo].[CollectionEventRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventRegulation_Regulation] FOREIGN KEY([Regulation])
REFERENCES [dbo].[Regulation] ([Regulation])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


ALTER TABLE [dbo].[CollectionEventRegulation] CHECK CONSTRAINT [FK_CollectionEventRegulation_Regulation]
GO


--#####################################################################################################################
--######  Adding the new keys for table CollectionSpecimenPartRegulation  #############################################
--#####################################################################################################################

ALTER TABLE CollectionSpecimenPartRegulation ALTER COLUMN [CollectionEventID] int NOT NULL;
GO

ALTER TABLE CollectionSpecimenPartRegulation ALTER COLUMN RegulationID int NULL;
GO

ALTER TABLE CollectionSpecimenPartRegulation ALTER COLUMN [Regulation] nvarchar(400) NOT NULL;
GO


ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [PK_CollectionSpecimenPartRegulation] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC,
	[CollectionEventID] ASC,
	[Regulation] ASC
)
GO


ALTER TABLE [dbo].[CollectionSpecimenPartRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenPartRegulation_CollectionEventRegulation] FOREIGN KEY([CollectionEventID], [Regulation])
REFERENCES [dbo].[CollectionEventRegulation] ([CollectionEventID], [Regulation])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] CHECK CONSTRAINT [FK_CollectionSpecimenPartRegulation_CollectionEventRegulation]
GO


--#####################################################################################################################
--######  trgDelRegulation    #########################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelRegulation] ON [dbo].[Regulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 

/* saving the original dataset in the logging table */ 
INSERT INTO Regulation_Log (Regulation, ParentRegulation, [Type], ProjectURI, Notes, [Status], HierarchyOnly, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Regulation, deleted.ParentRegulation, deleted.[Type], deleted.ProjectURI, deleted.Notes, deleted.[Status], deleted.HierarchyOnly, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######  trgUpdRegulation    #########################################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdRegulation] ON [dbo].[Regulation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Regulation_Log (Regulation, ParentRegulation, [Type], ProjectURI, Notes, [Status], HierarchyOnly, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.Regulation, deleted.ParentRegulation, deleted.[Type], deleted.ProjectURI, deleted.Notes, deleted.[Status], deleted.HierarchyOnly, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Regulation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM Regulation, deleted 
where 1 = 1 
AND Regulation.RegulationID = deleted.RegulationID

GO

--#####################################################################################################################
--######   trgDelCollectionEventRegulation    #########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventRegulation_Log (CollectionEventID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED


GO


--#####################################################################################################################
--######   trgUpdCollectionEventRegulation    #########################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


DECLARE @i int 
DECLARE @ID int
DECLARE @Version int

set @i = (select count(*) from deleted) 

if @i = 1 
BEGIN 
		/* saving the original dataset in the logging table */ 
		INSERT INTO CollectionEventRegulation_Log (CollectionEventID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
		SELECT deleted.CollectionEventID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
		FROM DELETED

		/* updating the logging columns */
		Update CollectionEventRegulation
		set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
		FROM CollectionEventRegulation, deleted 
		where 1 = 1 
		AND CollectionEventRegulation.CollectionEventID = deleted.CollectionEventID
		AND CollectionEventRegulation.Regulation = deleted.Regulation
	 END
GO


--#####################################################################################################################
--######   trgDelCollectionSpecimenPartRegulation    ##################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionSpecimenPartRegulation] ON [dbo].[CollectionSpecimenPartRegulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenPartRegulation_Log (CollectionSpecimenID, SpecimenPartID, CollectionEventID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.CollectionEventID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


--#####################################################################################################################
--######   trgUpdCollectionSpecimenPartRegulation    ##################################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionSpecimenPartRegulation] ON [dbo].[CollectionSpecimenPartRegulation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenPartRegulation_Log (CollectionSpecimenID, SpecimenPartID, CollectionEventID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.CollectionEventID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenPartRegulation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM CollectionSpecimenPartRegulation, deleted 
where 1 = 1 
AND CollectionSpecimenPartRegulation.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPartRegulation.Regulation = deleted.Regulation
AND CollectionSpecimenPartRegulation.SpecimenPartID = deleted.SpecimenPartID
AND CollectionSpecimenPartRegulation.CollectionEventID = deleted.CollectionEventID

GO



--#####################################################################################################################
--######   RegulationManager    #######################################################################################
--#####################################################################################################################

CREATE ROLE [RegulationManager]
GO

GRANT SELECT ON Regulation TO [RegulationManager]
GO
GRANT DELETE ON Regulation TO [RegulationManager]
GO
GRANT UPDATE ON Regulation TO [RegulationManager]
GO
GRANT INSERT ON Regulation TO [RegulationManager]
GO

GRANT DELETE ON CollectionEventRegulation TO [RegulationManager]
GO
GRANT UPDATE ON CollectionEventRegulation TO [RegulationManager]
GO
GRANT INSERT ON CollectionEventRegulation TO [RegulationManager]
GO

REVOKE UPDATE ON CollectionEventRegulation FROM [Editor]
GO
REVOKE INSERT ON CollectionEventRegulation FROM [Editor]
GO
--REVOKE SELECT ON Regulation FROM [User]
--GO


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
RETURN '02.06.05'
END

GO

