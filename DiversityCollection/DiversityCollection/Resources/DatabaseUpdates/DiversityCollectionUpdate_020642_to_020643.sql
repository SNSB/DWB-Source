declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.42'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   CollEventSeriesDescriptorType_Enum - Enumeration for series descriptor types    ############################
--#####################################################################################################################


CREATE TABLE [dbo].[CollEventSeriesDescriptorType_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[ParentCode] [nvarchar](50) NULL,
	[ParentRelation] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[URL] [varchar](500) NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[Icon] [image] NULL,
	[ModuleName] [varchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollEventSeriesDescriptorType_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollEventSeriesDescriptorType_Enum] ADD  CONSTRAINT [DF__CollEventSeriesDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollEventSeriesDescriptorType_Enum]  WITH CHECK ADD  CONSTRAINT [FK_CollEventSeriesDescriptorType_Enum_CollEventSeriesDescriptorType_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[CollEventSeriesDescriptorType_Enum] ([Code])
GO

ALTER TABLE [dbo].[CollEventSeriesDescriptorType_Enum] CHECK CONSTRAINT [FK_CollEventSeriesDescriptorType_Enum_CollEventSeriesDescriptorType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relation to parent entry, e.g. part of' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ParentRelation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A link to further information about the enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A symbol representing this entry in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'Icon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the enumerated entry is related to a DiversityWorkbench module or a related webservice, the name of the DiversityWorkbench module, e.g. DiversityCollEventSeriess' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum', @level2type=N'COLUMN',@level2name=N'ModuleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The type of the Descriptors' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollEventSeriesDescriptorType_Enum'
GO



--#####################################################################################################################
--######   CollectionEventSeriesDescriptor - Descriptors for event series  ############################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollectionEventSeriesDescriptor](
	[SeriesID] [int] NOT NULL,
	[DescriptorID] [int] IDENTITY(1,1) NOT NULL,
	[Descriptor] [nvarchar](200) NOT NULL,
	[URL] [varchar](500) NULL,
	[DescriptorType] [nvarchar](50) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CollectionEventSeriesDescriptor] PRIMARY KEY CLUSTERED 
(
	[SeriesID] ASC,
	[DescriptorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_Descriptor]  DEFAULT ('') FOR [Descriptor]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_URL]  DEFAULT ('') FOR [URL]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_DescriptorType]  DEFAULT (N'Descriptor') FOR [DescriptorType]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF__CollectionEventSeriesDescriptor_LogInsertedBy]  DEFAULT (suser_sname()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF__CollectionEventSeriesDescriptor_LogUpdatedBy]  DEFAULT (suser_sname()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor]  WITH NOCHECK ADD  CONSTRAINT [FK_CollectionEventSeriesDescriptor_CollectionEventSeries] FOREIGN KEY([SeriesID])
REFERENCES [dbo].[CollectionEventSeries] ([SeriesID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] CHECK CONSTRAINT [FK_CollectionEventSeriesDescriptor_CollectionEventSeries]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_CollectionEventSeriesDescriptor_CollEventSeriesDescriptorType_Enum] FOREIGN KEY([DescriptorType])
REFERENCES [dbo].[CollEventSeriesDescriptorType_Enum] ([Code])
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] CHECK CONSTRAINT [FK_CollectionEventSeriesDescriptor_CollEventSeriesDescriptorType_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the CollectionEventSeries  (foreign key + part of primary key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'SeriesID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ID for the descriptor, Part of PK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'DescriptorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Descriptor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'Descriptor'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL of the Descriptor. In case of a module related Descriptor, the link to the module entry resp. the related webservice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of the Descriptor as described in table CollectionEventSeriesDescriptorType_Enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'DescriptorType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Descriptors for the CollectionEventSeries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeriesDescriptor'
GO


--#####################################################################################################################
--######   CollectionEventSeriesDescriptor_log  #######################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[CollectionEventSeriesDescriptor_log](
	[SeriesID] [int] NULL,
	[DescriptorID] [int] NULL,
	[Descriptor] [nvarchar](200) NULL,
	[URL] [varchar](500) NULL,
	[DescriptorType] [nvarchar](50) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionEventSeriesDescriptor_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor_log] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor_log] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor_log] ADD  CONSTRAINT [DF_CollectionEventSeriesDescriptor_Log_LogUser]  DEFAULT (CONVERT([varchar],[dbo].[UserID]())) FOR [LogUser]
GO



--#####################################################################################################################
--######   trgDelCollectionEventSeriesDescriptor  #####################################################################
--#####################################################################################################################


CREATE TRIGGER [dbo].[trgDelCollectionEventSeriesDescriptor] ON [dbo].[CollectionEventSeriesDescriptor] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.4.5 */ 
/*  Date: 8/7/2023  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesDescriptor_Log (Descriptor, DescriptorID, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SeriesID, URL, LogUser,  LogState) 
SELECT D.Descriptor, D.DescriptorID, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, D.SeriesID, D.URL, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ENABLE TRIGGER [trgDelCollectionEventSeriesDescriptor]
GO


--#####################################################################################################################
--######   trgUpdCollectionEventSeriesDescriptor  #####################################################################
--#####################################################################################################################

CREATE TRIGGER [dbo].[trgUpdCollectionEventSeriesDescriptor] ON [dbo].[CollectionEventSeriesDescriptor] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.4.5 */ 
/*  Date: 8/7/2023  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeriesDescriptor_Log (Descriptor, DescriptorID, DescriptorType, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, SeriesID, URL, LogUser,  LogState) 
SELECT D.Descriptor, D.DescriptorID, D.DescriptorType, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, D.SeriesID, D.URL, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionEventSeriesDescriptor T, deleted D where 1 = 1 
AND T.DescriptorID = D.DescriptorID
AND T.SeriesID = D.SeriesID
GO

ALTER TABLE [dbo].[CollectionEventSeriesDescriptor] ENABLE TRIGGER [trgUpdCollectionEventSeriesDescriptor]
GO




--#####################################################################################################################
--######  CollectionEventSeries: New column DateSupplement    #########################################################
--#####################################################################################################################

ALTER TABLE CollectionEventSeries ADD [DateSupplement] nvarchar(100) NULL;
ALTER TABLE CollectionEventSeries_log ADD [DateSupplement] nvarchar(100) NULL;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Verbal or additional collection date information, e.g. ''end of summer 1985'', ''first quarter'', ''1888-1892''. The end date, if the collection event series comprises a period. The time of the event, if necessary.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventSeries', @level2type=N'COLUMN',@level2name=N'DateSupplement'
GO



ALTER TRIGGER [dbo].[trgDelCollectionEventSeries] ON [dbo].[CollectionEventSeries] 
FOR DELETE AS 

/*  Date: 03.08.2023 - inclusion of DateSupplement  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeries_Log (SeriesID, SeriesParentID, Description, SeriesCode, Notes, Geography, DateStart, DateEnd, DateSupplement, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SeriesID, deleted.SeriesParentID, deleted.Description, deleted.SeriesCode, deleted.Notes, deleted.Geography, deleted.DateStart, deleted.DateEnd, deleted.DateSupplement, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED
GO



ALTER TRIGGER [dbo].[trgUpdCollectionEventSeries] ON [dbo].[CollectionEventSeries] 
FOR UPDATE AS

/*  Date: 03.08.2023 - inclusion of DateSupplement  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionEventSeries_Log (SeriesID, SeriesParentID, Description, SeriesCode, Notes, Geography, DateStart, DateEnd, DateSupplement, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SeriesID, deleted.SeriesParentID, deleted.Description, deleted.SeriesCode, deleted.Notes, deleted.Geography, deleted.DateStart, deleted.DateEnd, deleted.DateSupplement, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update CollectionEventSeries
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionEventSeries, deleted 
where 1 = 1 
AND CollectionEventSeries.SeriesID = deleted.SeriesID
GO



--#####################################################################################################################
--######   EventSeriesHierarchy - add DateSupplement    ###############################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[EventSeriesHierarchy] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test
SELECT * FROM  dbo.EventSeriesHierarchy(-1733)
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.EventSeriesTopID(@SeriesID) )

declare @List TABLE (SeriesID int primary key,
   SeriesParentID int NULL)
   
-- inserting the start values  
	INSERT @List (SeriesID) Values(@TopID)
	INSERT @List (SeriesID, SeriesParentID) SELECT SeriesID, SeriesParentID FROM dbo.EventSeriesChildNodes (@TopID)
	
-- getting the whole hierarchy	
	set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	while @i > 0
	begin
		INSERT @List (SeriesID, SeriesParentID) 
			SELECT E.SeriesID, E.SeriesParentID from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P)
		set @i = (select COUNT(*) from CollectionEventSeries E, @List L where L.SeriesID = E.SeriesParentID AND E.SeriesID NOT IN (Select P.SeriesID  from @List P))
	end

	INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes)
	SELECT E.SeriesID, E.SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes
	FROM CollectionEventSeries E, @List L where L.SeriesID = E.SeriesID
   
-- set the geography
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END
GO


--#####################################################################################################################
--######   EventSeriesSuperiorList - add DateSupplement    ############################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[EventSeriesSuperiorList] (@SeriesID int)  
RETURNS @EventSeriesList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar(50) NULL,
   Description nvarchar(500) NULL,
   Notes nvarchar(500) NULL ,
   [Geography] geography)

/*
Returns a table that lists all the Series above the given Series.
MW 02.01.2006
*/
AS
BEGIN

	while (not @SeriesID is null)
		begin
		INSERT @EventSeriesList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes)
		SELECT DISTINCT SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description], Notes
		FROM CollectionEventSeries
		WHERE CollectionEventSeries.SeriesID = @SeriesID
		AND CollectionEventSeries.SeriesID NOT IN (SELECT SeriesID FROM @EventSeriesList)
		set @SeriesID = (select SeriesParentID from CollectionEventSeries where SeriesID = @SeriesID)
		IF @SeriesID = (select SeriesParentID from CollectionEventSeries where SeriesID = @SeriesID)
			begin 
			set @SeriesID = null
			end 
		end
		
	UPDATE L SET [Geography] = E.[Geography]
	FROM @EventSeriesList L, CollectionEventSeries E
	WHERE E.SeriesID = L.SeriesID

   RETURN
END


GO



--#####################################################################################################################
--######   EventSeriesChildNodes - add DateSupplement   ###############################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[EventSeriesChildNodes] (@ID int)  
RETURNS @ItemList TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500) ,
   [Geography] geography)
/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW 19.10.2022
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (SeriesID int primary key,
   SeriesParentID int NULL,
   DateStart datetime NULL,
   DateEnd datetime NULL,
   DateSupplement nvarchar(100) NULL,
   SeriesCode nvarchar (50)  NULL ,
   Description nvarchar (500)  NULL ,
   Notes nvarchar (500)  ,
   [Geography] geography)

-- insert the first childs into the table
 INSERT @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography]) 
	SELECT SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography]
	FROM CollectionEventSeries WHERE SeriesParentID = @ID 

	declare @i int
	set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	while @i > 0
	begin
		insert into @TempItem (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes, [Geography])
		select S.SeriesID, S.SeriesParentID, S.DateStart, S.DateEnd, S.DateSupplement, S.SeriesCode, S.[Description],  S.Notes, S.[Geography]
		from @TempItem T, CollectionEventSeries S where S.SeriesParentID = T.SeriesID and S.SeriesID not in (select SeriesID from @TempItem)
		set @i = (select count(*) from @TempItem T, CollectionEventSeries C where C.SeriesParentID = T.SeriesID and C.SeriesID not in (select SeriesID from @TempItem))
	end


 INSERT @ItemList (SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes) 
   SELECT distinct SeriesID, SeriesParentID, DateStart, DateEnd, DateSupplement, SeriesCode, [Description],  Notes
   FROM @TempItem ORDER BY DateStart
 UPDATE L SET [Geography] = E.[Geography]
 FROM @ItemList L, CollectionEventSeries E
 WHERE E.SeriesID = L.SeriesID

   RETURN
END

GO




--#####################################################################################################################
--######   CollectionAgent - trgInsCollectionAgent adapted to synchron insert    ######################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgInsCollectionAgent] ON [dbo].[CollectionAgent] 
FOR INSERT AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
declare @i int 
/*
*/
set @i = (select count(*) from inserted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM inserted)
EXECUTE procSetVersionCollectionSpecimen @ID
-- adding milliseconds in case of same sequence in data to ensure unique sequence
declare @Count int
set @Count = (select count(*) from CollectionAgent a inner join inserted i on A.CollectionSpecimenID = i.CollectionSpecimenID and a.CollectorsSequence  = i.CollectorsSequence )
if (@Count > 1)
begin
	update a set a.CollectorsSequence = DATEADD(ms, @Count, i.CollectorsSequence) from CollectionAgent a inner join inserted i on A.CollectionSpecimenID = i.CollectionSpecimenID and a.CollectorsName = i.CollectorsName
end
end

GO

--#####################################################################################################################
--######   CollectionAgent - disable IX_CollectionAgentSequence to ensure insert  #####################################
--#####################################################################################################################

if (SELECT  COUNT(*) AS Anzahl FROM sys.indexes AS ind INNER JOIN sys.tables AS t ON ind.object_id = t.object_id WHERE (t.name = 'collectionagent') AND (ind.name = 'IX_CollectionAgentSequence')) > 0
begin
	ALTER INDEX IX_CollectionAgentSequence ON CollectionAgent DISABLE;
end
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.43'
END

GO

