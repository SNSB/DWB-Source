declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.26'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  CollTaskMetricAggregation_Enum  #############################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[CollTaskMetricAggregation_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_CollTaskMetricAggregation_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CollTaskMetricAggregation_Enum] ADD  DEFAULT (newsequentialid()) FOR [RowGUID]
GO

ALTER TABLE [dbo].[CollTaskMetricAggregation_Enum]  WITH CHECK ADD  CONSTRAINT [FK_CollTaskMetricAggregation_Enum_CollTaskMetricAggregation_Enum] FOREIGN KEY([ParentCode])
REFERENCES [dbo].[CollTaskMetricAggregation_Enum] ([Code])
GO

ALTER TABLE [dbo].[CollTaskMetricAggregation_Enum] CHECK CONSTRAINT [FK_CollTaskMetricAggregation_Enum_CollTaskMetricAggregation_Enum]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code which uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the application may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface, if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes on usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The aggregation applied for retrieval of values, e.g. avg, max etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollTaskMetricAggregation_Enum'
GO


GRANT SELECT ON [CollTaskMetricAggregation_Enum] TO [User]
GO
GRANT DELETE ON [CollTaskMetricAggregation_Enum] TO [Administrator] 
GO
GRANT UPDATE ON [CollTaskMetricAggregation_Enum] TO [Administrator]
GO
GRANT INSERT ON [CollTaskMetricAggregation_Enum] TO [Administrator]
GO


INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('none', 'no aggregation applied', 'none', 1)
GO

INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('max', 'the maximal value within a period', 'max', 1)
GO

INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('min', 'the minimal value within a period', 'min', 1)
GO

INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('avg', 'the average value of a period', 'avg', 1)
GO

INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('sum', 'the sum of values within a period', 'sum', 1)
GO

INSERT INTO [CollTaskMetricAggregation_Enum]  (Code, Description, DisplayText, DisplayEnable) VALUES ('range', 'the difference between the maximal and minimal value within a period', 'range', 1)
GO



--#####################################################################################################################
--######  CollectionTaskMetric - Add Aggregation  #####################################################################
--#####################################################################################################################

ALTER TABLE  [dbo].[CollectionTaskMetric] ADD [Aggregation] nvarchar(50) NOT NULL DEFAULT (N'none');
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Aggregation applied for retrieval of the value, e.g. max, avg etc.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionTaskMetric', @level2type=N'COLUMN',@level2name=N'Aggregation'
GO

ALTER TABLE [dbo].[CollectionTaskMetric_log] ADD [Aggregation] nvarchar(50) NULL;
GO


ALTER TABLE [dbo].[CollectionTaskMetric]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTaskMetric_CollTaskMetricAggregation_Enum] FOREIGN KEY([Aggregation])
REFERENCES [dbo].[CollTaskMetricAggregation_Enum] ([Code])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[CollectionTaskMetric] CHECK CONSTRAINT [FK_CollectionTaskMetric_CollTaskMetricAggregation_Enum]
GO



--#####################################################################################################################
--######  trgDelCollectionTaskMetric - new column Aggregation  ########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgDelCollectionTaskMetric] ON [dbo].[CollectionTaskMetric] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.202 */ 
/*  Date: 11/10/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskMetric_Log (CollectionTaskID, MetricDate, Aggregation, MetricValue, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.MetricDate, D.Aggregation, D.MetricValue, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  'D'
FROM DELETED D 

GO

--#####################################################################################################################
--######  trgUpdCollectionTaskMetric - new column Aggregation  ##########################################################
--#####################################################################################################################

ALTER TRIGGER [dbo].[trgUpdCollectionTaskMetric] ON [dbo].[CollectionTaskMetric] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityCollection  4.3.202 */ 
/*  Date: 11/10/2021  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO CollectionTaskMetric_Log (CollectionTaskID, MetricDate, Aggregation, MetricValue, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID, LogUser,  LogState) 
SELECT D.CollectionTaskID, D.MetricDate, D.Aggregation, D.MetricValue, D.LogInsertedBy, D.LogInsertedWhen, D.LogUpdatedBy, D.LogUpdatedWhen, D.RowGUID, cast(dbo.UserID() as varchar) ,  'U'
FROM DELETED D 


/* updating the logging columns */
Update T 
set T.LogUpdatedWhen = getdate(),  T.LogUpdatedBy = cast(dbo.UserID() as varchar) 
FROM CollectionTaskMetric T, deleted D where 1 = 1 
AND T.CollectionTaskID = D.CollectionTaskID
AND T.MetricDate = D.MetricDate

GO


--#####################################################################################################################
--######  CollectionTaskMetric - adapt PK for new column Aggregation  #################################################
--#####################################################################################################################

/****** Object:  Index [PK_CollectionTaskMetric]    Script Date: 02.02.2022 13:01:10 ******/
ALTER TABLE [dbo].[CollectionTaskMetric] DROP CONSTRAINT [PK_CollectionTaskMetric] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [PK_CollectionTaskMetric]    Script Date: 02.02.2022 13:01:11 ******/
ALTER TABLE [dbo].[CollectionTaskMetric] ADD  CONSTRAINT [PK_CollectionTaskMetric] PRIMARY KEY CLUSTERED 
(
	[CollectionTaskID] ASC,
	[MetricDate] ASC,
	[Aggregation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


--#####################################################################################################################
--######  TaskType_Enum - adding type Battery (Charge of a battery) and Bycatch   #####################################
--#####################################################################################################################

INSERT INTO [dbo].[TaskType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('Battery'
           ,'Charge of a battery in %'
           ,'Battery'
           ,1
           ,'Sensor')
GO

INSERT INTO [dbo].[TaskType_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayEnable]
           ,[ParentCode])
     VALUES
           ('Bycatch'
           ,'Bycatch in a trap'
           ,'Bycatch'
           ,1
           ,'IPM')
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.06.27'
END

GO

