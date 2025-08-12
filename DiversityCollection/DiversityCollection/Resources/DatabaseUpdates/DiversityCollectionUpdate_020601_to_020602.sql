declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.01'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  StableIdentifier: Enforce user to enter a value    ##########################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[StableIdentifier] (@ProjectID int, @CollectionSpecimenID int, @IdentificationUnitID int, @SpecimenPartID int)
RETURNS varchar (500)
/*
Returns a stable identfier for a dataset.
Relies on an entry in ProjectProxy
*/
AS
BEGIN
declare @StableIdentifierBase varchar(500)
set @StableIdentifierBase = (SELECT case when [StableIdentifierBase] is null or [StableIdentifierBase] = ''
	then NULL -- dbo.BaseURL() -- former version used BaseURL as a default
	else [StableIdentifierBase]
	end
  FROM [dbo].[ProjectProxy]
p where p.ProjectID = @ProjectID)
if (@StableIdentifierBase not like '%/')
begin
	set @StableIdentifierBase = @StableIdentifierBase + '/'
end
declare @StableIdentifier varchar(500)
set @StableIdentifier = @StableIdentifierBase + cast(@CollectionSpecimenID as varchar) 
if (@IdentificationUnitID IS NOT NULL)
begin
	set @StableIdentifier = @StableIdentifier + '/' + cast(@IdentificationUnitID as varchar) 
	if (@SpecimenPartID IS NOT NULL)
	begin
		set @StableIdentifier = @StableIdentifier + '/' + cast(@SpecimenPartID as varchar) 
	end
end
return @StableIdentifier
END

GO

--#####################################################################################################################
--######  NextFreeAccNr: Bugfix    ####################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[NextFreeAccNr] (@AccessionNumber nvarchar(50))  
/*
returns next free accession number
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 06.03.2017
TEST:
select max(AccessionNumber) from CollectionSpecimen
select dbo.[NextFreeAccNr] ('0033933')
select dbo.[NextFreeAccNr] ('00041009')
select dbo.[NextFreeAccNr] ('M-00041009')
select dbo.[NextFreeAccNr] ('M-0014474')
select dbo.[NextFreeAccNr] ('ZSM_DIP-000')
select dbo.[NextFreeAccNr] ('1907/9')
select dbo.[NextFreeAccNr] ('ZSM-MA-9')
select dbo.[NextFreeAccNr] ('M-0013622')
select dbo.[NextFreeAccNr] ('M-0014900')
select dbo.[NextFreeAccNr] ('MB-FA-000001') 
select dbo.[NextFreeAccNr] ('MB-FA-000101') 
select dbo.[NextFreeAccNr] ('SMNS-B-PH-2017/1234') 
*/
RETURNS nvarchar (50)
AS
BEGIN 
declare @NextAcc nvarchar(50)
set @NextAcc = ''
declare @Start int
declare @Position tinyint
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (isnumeric(@AccessionNumber) = 1)
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
while (substring(@AccessionNumber, @Position, 1) LIKE '[0-9]')
begin
	set @Start = CAST(substring(@AccessionNumber, @Position, len(@AccessionNumber)) as int)
	set @Position = @Position - 1
	set @Prefix = substring(@AccessionNumber, 1, @Position)
end
end
if (@Start < 0) 
begin 
	set @Start = @Start * -1;
end
declare @Space nvarchar(1)
set @Space = ''
if (SUBSTRING(@AccessionNumber, @Position + 1, 1) = ' ')
begin
	set @Space = '_'
end
if (LEN(@Prefix) = LEN(@AccessionNumber))
begin
	set @Prefix = SUBSTRING(@Prefix, 1, len(@Prefix) - 1)
end
declare @T Table (ID int identity(1, 1),
	NumericPart int NULL,
    AccessionNumber nvarchar(50) NULL)
INSERT INTO @T (AccessionNumber)
SELECT AccessionNumber 
FROM CollectionSpecimen  
WHERE AccessionNumber LIKE @Prefix + '%'
AND AccessionNumber >= @AccessionNumber
if (select COUNT(*) from @T) = 0
begin
INSERT INTO @T (AccessionNumber)
SELECT @AccessionNumber 
end
UPDATE @T SET NumericPart = ID + @Start;
UPDATE @T SET AccessionNumber = @Prefix 
+ case when (len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) > 0 
then replicate('0', len(@AccessionNumber) - LEN(NumericPart)- LEN(@Prefix) - LEN(@Space)) else '' end
+ CAST(NumericPart as varchar);
set @NextAcc = (SELECT MIN(T.AccessionNumber) from @T T left outer join  CollectionSpecimen S on S.AccessionNumber = T.AccessionNumber
	where S.AccessionNumber is null)
return (@NextAcc)
END

GO



--#####################################################################################################################
--#####################################################################################################################
--######  Create Regulation tables    #################################################################################
--#####################################################################################################################
--#####################################################################################################################

--#####################################################################################################################
--######  RegulationType_Enum    ######################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'RegulationType_Enum') = 0
begin

CREATE TABLE [dbo].[RegulationType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_RegulationType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[RegulationType_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]

ALTER TABLE [dbo].[RegulationType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_RegulationType_Enum_RegulationType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[RegulationType_Enum] ([Code])

ALTER TABLE [dbo].[RegulationType_Enum] CHECK CONSTRAINT [FK_RegulationType_Enum_RegulationType_Enum]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'Code'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'Description'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The types of a Regulation, e.g. ABS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RegulationType_Enum'

end
GO

GRANT INSERT ON RegulationType_Enum TO [Administrator] 
GO
GRANT UPDATE ON RegulationType_Enum TO [Administrator] 
GO
GRANT SELECT ON RegulationType_Enum TO [User] 
GO

--#####################################################################################################################
--######  Regulation    ###############################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'Regulation') = 0
begin

CREATE TABLE [dbo].[Regulation](
	[RegulationID] [int] IDENTITY(1,1) NOT NULL,
	[ParentRegulationID] [int] NULL,
	[Regulation] [nvarchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[ProjectURI] [varchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Regulation] PRIMARY KEY CLUSTERED 
(
	[RegulationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [DF_Regulation_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]

ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [DF_Regulation_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]

ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [DF_Regulation_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [DF_Regulation_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]

ALTER TABLE [dbo].[Regulation] ADD  CONSTRAINT [DF_Regulation_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]

ALTER TABLE [dbo].[Regulation]  WITH CHECK ADD  CONSTRAINT [FK_Regulation_Regulation] FOREIGN KEY([ParentRegulationID])
REFERENCES [dbo].[Regulation] ([RegulationID])

ALTER TABLE [dbo].[Regulation] CHECK CONSTRAINT [FK_Regulation_Regulation]

ALTER TABLE [dbo].[Regulation]  WITH CHECK ADD  CONSTRAINT [FK_Regulation_RegulationType_Enum] FOREIGN KEY([Type])
REFERENCES [dbo].[RegulationType_Enum] ([Code])

ALTER TABLE [dbo].[Regulation] CHECK CONSTRAINT [FK_Regulation_RegulationType_Enum]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK, ID of the regulation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'RegulationID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The ID of the parent regulation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'ParentRegulationID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the regulation, e.g. the name of the file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'Regulation'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the regulation as defined in table RegulationType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'Type'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link to the module DiversityProjects where further informations about the regulation are stored' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'ProjectURI'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes about the regulation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'Notes'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Regulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

end
GO

GRANT INSERT ON Regulation TO [Administrator] 
GO
GRANT UPDATE ON Regulation TO [Administrator] 
GO
GRANT DELETE ON Regulation TO [Administrator] 
GO
GRANT SELECT ON Regulation TO [User] 
GO


--#####################################################################################################################
--######  Regulation_log    ###########################################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'Regulation_log') = 0
begin

CREATE TABLE [dbo].[Regulation_log](
	[RegulationID] [int] NULL,
	[ParentRegulationID] [int] NULL,
	[Regulation] [nvarchar](500) NULL,
	[Type] [nvarchar](50) NULL,
	[ProjectURI] [varchar](500) NULL,
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
 CONSTRAINT [PK_Regulation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


ALTER TABLE [dbo].[Regulation_log] ADD  CONSTRAINT [DF_Regulation_Log_LogState]  DEFAULT ('U') FOR [LogState]

ALTER TABLE [dbo].[Regulation_log] ADD  CONSTRAINT [DF_Regulation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]

ALTER TABLE [dbo].[Regulation_log] ADD  CONSTRAINT [DF_Regulation_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
end
GO


GRANT INSERT ON Regulation TO [Administrator] 
GO
GRANT SELECT ON Regulation TO [Administrator] 
GO



--#####################################################################################################################
--######  trgDelRegulation    #########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelRegulation] ON [dbo].[Regulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Regulation_Log (RegulationID, ParentRegulationID, Regulation, Type, ProjectURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.RegulationID, deleted.ParentRegulationID, deleted.Regulation, deleted.Type, deleted.ProjectURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######  trgUpdRegulation    #########################################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdRegulation] ON [dbo].[Regulation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Regulation_Log (RegulationID, ParentRegulationID, Regulation, Type, ProjectURI, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.RegulationID, deleted.ParentRegulationID, deleted.Regulation, deleted.Type, deleted.ProjectURI, deleted.Notes, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update Regulation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM Regulation, deleted 
where 1 = 1 
AND Regulation.RegulationID = deleted.RegulationID
GO


--#####################################################################################################################
--######   CollectionEventRegulation    ###############################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionEventRegulation') = 0
begin

CREATE TABLE [dbo].[CollectionEventRegulation](
	[CollectionEventID] [int] NOT NULL,
	[RegulationID] [int] NOT NULL,
	[Regulation] [nvarchar](500) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollectionEventRegulation] PRIMARY KEY CLUSTERED 
(
	[CollectionEventID] ASC,
	[RegulationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [DF_CollectionEventRegulation_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]

ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [DF__Collectio__LogCr__1394653D]  DEFAULT (suser_sname()) FOR [LogCreatedBy]

ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [DF_CollectionEventRegulation_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [DF__Collectio__LogUp__157CADAF]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]

ALTER TABLE [dbo].[CollectionEventRegulation] ADD  CONSTRAINT [DF__Collectio__RowGU__1670D1E8]  DEFAULT (newsequentialid()) FOR [RowGUID]

ALTER TABLE [dbo].[CollectionEventRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventRegulation_CollectionEvent] FOREIGN KEY([CollectionEventID])
REFERENCES [dbo].[CollectionEvent] ([CollectionEventID])

ALTER TABLE [dbo].[CollectionEventRegulation] CHECK CONSTRAINT [FK_CollectionEventRegulation_CollectionEvent]

ALTER TABLE [dbo].[CollectionEventRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventRegulation_Regulation] FOREIGN KEY([RegulationID])
REFERENCES [dbo].[Regulation] ([RegulationID])

ALTER TABLE [dbo].[CollectionEventRegulation] CHECK CONSTRAINT [FK_CollectionEventRegulation_Regulation]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of primay key, refers to unique ID for the table CollectionEvent (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'CollectionEventID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of primay key, refers to unique ID for the table Regulation (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'RegulationID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Regulation as defined in the table Regulation. Used to ensure, that user checked correct entry with authorized stuff' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'Regulation'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Regulation applied to a collection event' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventRegulation'
end
GO


GRANT INSERT ON CollectionEventRegulation TO [Editor] 
GO
GRANT UPDATE ON CollectionEventRegulation TO [Editor] 
GO
GRANT DELETE ON CollectionEventRegulation TO [Administrator] 
GO
GRANT SELECT ON CollectionEventRegulation TO [User] 
GO


--#####################################################################################################################
--######   CollectionEventRegulation_log    ###########################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionEventRegulation_log') = 0
begin

CREATE TABLE [dbo].[CollectionEventRegulation_log](
	[CollectionEventID] [int] NULL,
	[RegulationID] [int] NULL,
	[Regulation] [nvarchar](500) NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionEventRegulation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[CollectionEventRegulation_log] ADD  CONSTRAINT [DF_CollectionEventRegulation_Log_LogState]  DEFAULT ('U') FOR [LogState]

ALTER TABLE [dbo].[CollectionEventRegulation_log] ADD  CONSTRAINT [DF_CollectionEventRegulation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]

ALTER TABLE [dbo].[CollectionEventRegulation_log] ADD  CONSTRAINT [DF_CollectionEventRegulation_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]

end
GO


GRANT INSERT ON CollectionEventRegulation_log TO [Editor] 
GO
GRANT SELECT ON CollectionEventRegulation_log TO [Editor] 
GO

--#####################################################################################################################
--######   trgDelCollectionEventRegulation    #########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventRegulation_Log (CollectionEventID, RegulationID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.RegulationID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   trgUpdCollectionEventRegulation    #########################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionEventRegulation] ON [dbo].[CollectionEventRegulation] 
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
	if (select count(*) From inserted i, Regulation R where i.RegulationID = R.RegulationID and i.Regulation = R.Regulation) = 0
	 BEGIN
	 UPDATE R 
		 SET R.Regulation = NULL
		 FROM CollectionEventRegulation R, deleted d
		 WHERE R.CollectionEventID = d.CollectionEventID
		 AND R.RegulationID = d.RegulationID
	 END
	 else
	 BEGIN
		/* saving the original dataset in the logging table */ 
		INSERT INTO CollectionEventRegulation_Log (CollectionEventID, RegulationID, Regulation, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
		SELECT deleted.CollectionEventID, deleted.RegulationID, deleted.Regulation, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
		FROM DELETED

		/* updating the logging columns */
		Update CollectionEventRegulation
		set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
		FROM CollectionEventRegulation, deleted 
		where 1 = 1 
		AND CollectionEventRegulation.CollectionEventID = deleted.CollectionEventID
		AND CollectionEventRegulation.RegulationID = deleted.RegulationID
	 END
END 
GO

--#####################################################################################################################
--######   CollectionSpecimenPartRegulation    ########################################################################
--#####################################################################################################################


if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenPartRegulation') = 0
begin

CREATE TABLE [dbo].[CollectionSpecimenPartRegulation](
	[CollectionSpecimenID] [int] NOT NULL,
	[SpecimenPartID] [int] NOT NULL,
	[RegulationID] [int] NOT NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenPartRegulation] PRIMARY KEY CLUSTERED 
(
	[CollectionSpecimenID] ASC,
	[SpecimenPartID] ASC,
	[RegulationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_LogCreatedWhen]  DEFAULT (getdate()) FOR [LogCreatedWhen]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_LogCreatedBy]  DEFAULT (suser_sname()) FOR [LogCreatedBy]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenPartRegulation_CollectionSpecimenPart] FOREIGN KEY([CollectionSpecimenID], [SpecimenPartID])
REFERENCES [dbo].[CollectionSpecimenPart] ([CollectionSpecimenID], [SpecimenPartID])

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] CHECK CONSTRAINT [FK_CollectionSpecimenPartRegulation_CollectionSpecimenPart]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation]  WITH CHECK ADD  CONSTRAINT [FK_CollectionSpecimenPartRegulation_Regulation] FOREIGN KEY([RegulationID])
REFERENCES [dbo].[Regulation] ([RegulationID])

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation] CHECK CONSTRAINT [FK_CollectionSpecimenPartRegulation_Regulation]

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of primay key, refers to unique ID for the table CollectionSpecimen (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'CollectionSpecimenID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of primay key, refers to unique ID for the table CollectionSpecimenPart (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'SpecimenPartID'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of primay key, refers to unique ID for the table Regulation (= foreign key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'RegulationID'

EXEC sys.sp_addextendedproperty @name=N'MS_Regulation', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Regulation', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Regulation', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'

EXEC sys.sp_addextendedproperty @name=N'MS_Regulation', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Regulation applied to a collection specimen part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionSpecimenPartRegulation'
end
GO

GRANT INSERT ON CollectionSpecimenPartRegulation TO [Editor] 
GO
GRANT UPDATE ON CollectionSpecimenPartRegulation TO [Editor] 
GO
GRANT DELETE ON CollectionSpecimenPartRegulation TO [Editor] 
GO
GRANT SELECT ON CollectionSpecimenPartRegulation TO [User] 
GO

--#####################################################################################################################
--######   CollectionSpecimenPartRegulation_log    ####################################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CollectionSpecimenPartRegulation_log') = 0
begin

CREATE TABLE [dbo].[CollectionSpecimenPartRegulation_log](
	[CollectionSpecimenID] [int] NULL,
	[SpecimenPartID] [int] NULL,
	[RegulationID] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionSpecimenPartRegulation_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[CollectionSpecimenPartRegulation_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_Log_LogState]  DEFAULT ('U') FOR [LogState]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]

ALTER TABLE [dbo].[CollectionSpecimenPartRegulation_log] ADD  CONSTRAINT [DF_CollectionSpecimenPartRegulation_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]

end
GO


GRANT INSERT ON CollectionSpecimenPartRegulation_log TO [Editor] 
GO
GRANT SELECT ON CollectionSpecimenPartRegulation_log TO [Editor] 
GO


--#####################################################################################################################
--######   trgDelCollectionSpecimenPartRegulation    ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgDelCollectionSpecimenPartRegulation] ON [dbo].[CollectionSpecimenPartRegulation] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenPartRegulation_Log (CollectionSpecimenID, SpecimenPartID, RegulationID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.RegulationID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

--#####################################################################################################################
--######   trgUpdCollectionSpecimenPartRegulation    ##################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionSpecimenPartRegulation] ON [dbo].[CollectionSpecimenPartRegulation] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  3.0.8.9 */ 
/*  Date: 2/8/2017  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionSpecimenPartRegulation_Log (CollectionSpecimenID, SpecimenPartID, RegulationID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.SpecimenPartID, deleted.RegulationID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionSpecimenPartRegulation
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM CollectionSpecimenPartRegulation, deleted 
where 1 = 1 
AND CollectionSpecimenPartRegulation.CollectionSpecimenID = deleted.CollectionSpecimenID
AND CollectionSpecimenPartRegulation.RegulationID = deleted.RegulationID
AND CollectionSpecimenPartRegulation.SpecimenPartID = deleted.SpecimenPartID
GO


--#####################################################################################################################
--######   Data transfer to Regulation    #############################################################################
--#####################################################################################################################

-- Types
declare @Regulations TABLE ([Type] [nvarchar](50) Primary key)

INSERT @Regulations (Type)
SELECT [Type]
FROM ExternalIdentifierType
WHERE ParentType = 'Regulation'

declare @i int
set @i = (select count(*) from ExternalIdentifierType where ParentType IN (select [Type] from  @Regulations) and [Type] not in (select [Type] from  @Regulations))
while (@i > 0)
	begin
	INSERT @Regulations ([Type])
	SELECT [Type] 
	FROM ExternalIdentifierType
		where ParentType IN (select [Type] from  @Regulations) and [Type] not in (select [Type] from  @Regulations)
	set @i = (select count(*) from ExternalIdentifierType where ParentType IN (select [Type] from  @Regulations) and [Type] not in (select [Type] from  @Regulations))
end

INSERT INTO [dbo].[RegulationType_Enum]
			([Code]
			,[Description]
			,[DisplayText])
SELECT [Type],[Type],[Type]  
FROM @Regulations

if (select count(*) from [RegulationType_Enum] t where t.Code = 'ABS') = 0
begin
	INSERT INTO [dbo].[RegulationType_Enum]
				([Code]
				,[Description]
				,[DisplayText])
	VALUES('ABS','ABS','ABS')
end

-- Regulations
INSERT INTO [dbo].[Regulation]
			([Regulation]
			,[Type]
			,[ProjectURI])
SELECT [Identifier]
		,T.[Type]
		,max(E.[URL])
	FROM [dbo].[ExternalIdentifier] e, [dbo].[ExternalIdentifierType] T, [RegulationType_Enum] R
	where e.ReferencedTable = 'CollectionEvent'
and t.Type = e.Type
and R.Code = t.Type
group by t.Type, e.Identifier


--Events
INSERT INTO [dbo].[CollectionEventRegulation]
			([CollectionEventID]
			,[RegulationID]
			,[Regulation])
SELECT distinct e.ReferencedID,
R.RegulationID,
R.Regulation
FROM [dbo].[ExternalIdentifier] e, [Regulation] R, CollectionEvent CE, @Regulations rs
where e.ReferencedTable = 'CollectionEvent'
and r.Type = e.Type
and rs.Type = r.Type
and r.Regulation = e.Identifier
and CE.CollectionEventID = e.ReferencedID


-- Parts
INSERT INTO [dbo].[CollectionSpecimenPartRegulation]
			([CollectionSpecimenID]
			,SpecimenPartID
			,[RegulationID])
SELECT distinct P.CollectionSpecimenID,
e.ReferencedID,
R.RegulationID
FROM [dbo].[ExternalIdentifier] e, [Regulation] R, CollectionSpecimenPart P, @Regulations rs
where e.ReferencedTable = 'CollectionSpecimenPart'
and r.Type = e.Type
and rs.Type = r.Type
and r.Regulation = e.Identifier
and P.SpecimenPartID = e.ReferencedID


--remove old values

delete e
FROM [dbo].[ExternalIdentifier] e, @Regulations rs
where e.Type = rs.Type

delete e
FROM [dbo].[ExternalIdentifierType] e, @Regulations rs
where e.Type = rs.Type

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
RETURN '02.06.02'
END

GO

