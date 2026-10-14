
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.00'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--#####################################################################################################################
--######   Setting - new table, takes values from XML column ######################################################################################
--#####################################################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Setting](
	[SettingID] [int] IDENTITY(1,1) NOT NULL,
	[ParentSettingID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DisplayOrder] [int] NULL,
	[LogCreatedWhen] [datetime] NULL CONSTRAINT [DF_Setting_LogCreatedWhen]  DEFAULT (getdate()),
	[LogCreatedBy] [nvarchar](50) NULL CONSTRAINT [DF_Setting_LogCreatedBy]  DEFAULT (user_name()),
	[LogUpdatedWhen] [datetime] NULL CONSTRAINT [DF_Setting_LogUpdatedWhen]  DEFAULT (getdate()),
	[LogUpdatedBy] [nvarchar](50) NULL CONSTRAINT [DF_Setting_LogUpdatedBy]  DEFAULT (user_name()),
	[RowGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Setting_RowGUID]  DEFAULT (newid()),
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence in which the items should be shown in an interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogCreatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the creator of this data set' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogCreatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Point in time when this data set was updated last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the person to update this data set last' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO


GRANT SELECT ON [Setting] TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON [Setting] TO [DiversityWorkbenchAdministrator]
GO
GRANT UPDATE ON [Setting] TO [DiversityWorkbenchAdministrator]
GO
GRANT DELETE ON [Setting] TO [DiversityWorkbenchAdministrator]
GO







--#####################################################################################################################
--######   [Setting_log]  ######################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[Setting_log](
	[SettingID] [int] NULL,
	[ParentSettingID] [int] NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DisplayOrder] [int] NULL,
	[LogCreatedWhen] [datetime] NULL,
	[LogCreatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [datetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Setting_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Setting_log] ADD  CONSTRAINT [DF_Setting_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[Setting_log] ADD  CONSTRAINT [DF_Setting_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[Setting_log] ADD  CONSTRAINT [DF_Setting_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [Setting] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [Setting] TO [DiversityWorkbenchEditor]
GO






--#####################################################################################################################
--#####################################################################################################################
--######   [ProjectSetting]  ######################################################################################
--#####################################################################################################################
--#####################################################################################################################


CREATE TABLE [dbo].[ProjectSetting](
	[ProjectID] [int] NOT NULL,
	[SettingID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectSetting] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[SettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_Value]  DEFAULT ('') FOR [Value]
GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogInsertedBy]  DEFAULT (user_name()) FOR [LogInsertedBy]
GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogInsertedWhen]  DEFAULT (getdate()) FOR [LogInsertedWhen]
GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogUpdatedBy]  DEFAULT (user_name()) FOR [LogUpdatedBy]
GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_LogUpdatedWhen]  DEFAULT (getdate()) FOR [LogUpdatedWhen]
GO

ALTER TABLE [dbo].[ProjectSetting] ADD  CONSTRAINT [DF_ProjectSetting_RowGUID]  DEFAULT (newid()) FOR [RowGUID]
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part of PK, ID of the Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The value of the setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who first entered (typed or imported) the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogInsertedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were first entered (typed or imported) into this database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogInsertedWhen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of user who last updated the data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogUpdatedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and time when the data were last updated.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectSetting', @level2type=N'COLUMN',@level2name=N'LogUpdatedWhen'
GO



GRANT SELECT ON [ProjectSetting] TO [DiversityWorkbenchUser]
GO
GRANT INSERT ON [ProjectSetting] TO [DiversityWorkbenchEditor]
GO
GRANT UPDATE ON [ProjectSetting] TO [DiversityWorkbenchEditor]
GO
GRANT DELETE ON [ProjectSetting] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   [ProjectSetting_log]  ######################################################################################
--#####################################################################################################################

CREATE TABLE [dbo].[ProjectSetting_log](
	[ProjectID] [int] NULL,
	[SettingID] [int] NULL,
	[Value] [nvarchar](max) NULL,
	[LogInsertedBy] [nvarchar](50) NULL,
	[LogInsertedWhen] [smalldatetime] NULL,
	[LogUpdatedBy] [nvarchar](50) NULL,
	[LogUpdatedWhen] [smalldatetime] NULL,
	[RowGUID] [uniqueidentifier] NULL,
	[LogState] [char](1) NULL,
	[LogDate] [datetime] NOT NULL,
	[LogUser] [nvarchar](50) NULL,
	[LogID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ProjectSetting_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProjectSetting_log] ADD  CONSTRAINT [DF_ProjectSetting_Log_LogState]  DEFAULT ('U') FOR [LogState]
GO

ALTER TABLE [dbo].[ProjectSetting_log] ADD  CONSTRAINT [DF_ProjectSetting_Log_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

ALTER TABLE [dbo].[ProjectSetting_log] ADD  CONSTRAINT [DF_ProjectSetting_Log_LogUser]  DEFAULT (user_name()) FOR [LogUser]
GO


GRANT SELECT ON [ProjectSetting_log] TO [DiversityWorkbenchEditor]
GO
GRANT INSERT ON [ProjectSetting_log] TO [DiversityWorkbenchEditor]
GO


--#####################################################################################################################
--######   Transfer of XML settings into a new table structure   ######################################################################################
--#####################################################################################################################



-- adding a level column to Setting
if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'Setting' and C.COLUMN_NAME = 'xx_Level') = 0
begin
ALTER TABLE Setting ADD  xx_Level int NULL
end
go

-- cleaning the new tables
delete from ProjectSetting;
truncate table Setting;

-- declaration of parameters
declare @Node nvarchar(500)
declare @XmlNode nvarchar(500)
declare @XmlPreNode nvarchar(500)
declare @Parent nvarchar(200)
declare @Content nvarchar(500)
declare @ProjectID int
declare @SettingID int
declare @ParentSettingID int
declare @Level int


-- temporary table for ProjectIDs with settings
declare @TempTab table (ProjectID int)
insert into @TempTab Select ProjectID from Project P where cast(P.ProjectSettings as nvarchar(max)) <> ''

-- temporary table for level in hierarchy of xml
declare @HierTab table (SettingID int, Node nvarchar(200), ID int identity);

WHILE (SELECT COUNT(*) FROM @TempTab) > 0
BEGIN

SET @ProjectID = (SELECT MIN(ProjectID) FROM @TempTab)
DELETE T FROM @TempTab T WHERE T.ProjectID = @ProjectID
delete H from @HierTab H

-- Reset all parameters
set @Parent = ''
set @Node = ''
set @Content = ''
set @ParentSettingID = null
set @SettingID = null
set @Level = 0

declare @XML nvarchar(max)
set @XML = (select TOP 1 cast (ProjectSettings as nvarchar(max)) FROM [dbo].[Project]
P WHERE ProjectID = @ProjectID)


while len(@XML) > 0
begin
	-- reading the values from the xml into the parameters
	set @Node = SUBSTRING(@XML, 1, charindex('>', @XML, 1))
	set @XmlNode = @Node
	set @Content = ''
	if (@Node like '%</%>')
	begin
		set @Content = (select substring(@Node, 1, charindex('<', @Node)-1))
		if (@Content <> '')
		begin
			set @XmlNode = (select substring(@XmlNode, charindex('<', @Node), 300))
		end
	end
	set @Node = substring(@Node, charindex('<', @Node)+1, 255)
	set @Node = substring(@Node, 1, charindex('>', @Node) - 1)
	if (@Node like '/%') set @Node = substring(@Node, 2, 255)
	if (@Node like '%/') set @Node = substring(@Node, 1, len(@Node)-1)
	if (@XmlNode like '<%>' and @XmlNode not like '%</%>' and @XmlPreNode like '<%>' and @XmlPreNode not like '%<%/%>') 
		set @Level = @Level + 1
	if (@XmlNode like '</%>' and @Content = '') 
		set @Level = @Level - 1

	-- Parent
	if (@XmlNode = '<' + @Node + '>' and @XmlPreNode not like '</%>')
		set @Parent = substring(@XmlPreNode, 2, len(@XmlPreNode) - 2)
	if (@XmlNode = '</' + @Node + '>' and @XmlNode = '</' + @Parent + '>')
	begin
		set @Parent = (select min(P.DisplayText) from Setting S, Setting P where S.DisplayText = @Node and S.ParentSettingID = P.SettingID)
		set @ParentSettingID = (select P.SettingID from Setting S, Setting P where S.DisplayText = @Parent and S.ParentSettingID = P.SettingID and P.DisplayText = @Parent)
	end

	if (@XmlNode = '</' + @Node + '>')
	begin
		delete H from @HierTab AS H where H.ID = (select max(ID) from @HierTab) and H.Node = @Node
	end
	set @ParentSettingID = (select H.SettingID from @HierTab H where H.ID = (Select max(ID) from @HierTab))

	-- writing the data into the database
	if (select count(*) from Setting S, Setting P where S.DisplayText = @Node and S.xx_Level = @Level and S.ParentSettingID = P.SettingID and P.DisplayText = @Parent) = 0 and @XmlNode not like '</%>'
	begin
		
		insert into Setting (DisplayText, [Description], ParentSettingID, xx_Level) 
		values (cast(@Node as nvarchar(50)), @Node, @ParentSettingID, @Level)
		set @SettingID = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])

		if (@XmlNode not like '<%/%>')
			insert into @HierTab(SettingID, Node) values(@SettingID, @Node)
	end
	else
	begin
		if (@Content <> '')
		begin
			declare @i int
			set @i = @SettingID
			set @SettingID = (select MIN(S.SettingID) from Setting S, Setting P where S.DisplayText = @Node and S.xx_Level = @Level and S.ParentSettingID = P.SettingID and P.DisplayText = @Parent)
		end
	end


	set @XML = SUBSTRING(@XML, charindex('>', @XML, 1)+1, 8000)

	if (@Content <> '' and not @SettingID is null)
	begin
		if (select count(*) from [ProjectSetting] P where P.ProjectID = @ProjectID and P.SettingID = @SettingID) = 0
			insert into [dbo].[ProjectSetting] (ProjectID, SettingID, Value) values (@ProjectID, @SettingID, @Content);
	end
	
	-- Preparation for next loop
	if (@XmlPreNode is null)
		set @ParentSettingID = @SettingID
	else
	begin
		if (@XmlNode = '<' + @Node + '>' and not @XmlPreNode like '%/%' and not @XmlPreNode = '<' + @Node + '>')
		begin
			set @ParentSettingID = @SettingID
		end
	end

	set @XmlPreNode = @XmlNode
end
END

-- removing the level column from Setting
ALTER TABLE Setting DROP COLUMN xx_Level
go


--#####################################################################################################################
--######   Displayorder in Settings  ######################################################################################
--#####################################################################################################################

IF (select count(*) from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = 'Setting'
and C.COLUMN_NAME = 'DisplayOrder') = 0
begin
	alter table Setting add DisplayOrder int NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The sequence in which the settings should be displayed in a user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
end
GO

UPDATE S SET DisplayOrder = S.SettingID FROM  Setting S
GO




--#####################################################################################################################
--######   Setting the constraints after transfer of the data  ######################################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[Setting]  WITH CHECK ADD  CONSTRAINT [FK_Setting_Setting] FOREIGN KEY([ParentSettingID])
REFERENCES [dbo].[Setting] ([SettingID])
GO

ALTER TABLE [dbo].[Setting] CHECK CONSTRAINT [FK_Setting_Setting]
GO


--#####################################################################################################################
--######   Trigger for Setting  ######################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelSetting]    Script Date: 12.05.2015 11:33:43 ******/

CREATE TRIGGER [dbo].[trgDelSetting] ON [dbo].[Setting] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Setting_Log (SettingID, ParentSettingID, DisplayText, Description, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SettingID, deleted.ParentSettingID, deleted.DisplayText, deleted.Description, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO

/****** Object:  Trigger [dbo].[trgUpdSetting]    Script Date: 12.05.2015 11:34:04 ******/

CREATE TRIGGER [dbo].[trgUpdSetting] ON [dbo].[Setting] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO Setting_Log (SettingID, ParentSettingID, DisplayText, Description, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.SettingID, deleted.ParentSettingID, deleted.DisplayText, deleted.Description, deleted.DisplayOrder, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED

/* updating the logging columns */
Update Setting
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM Setting, deleted 
where 1 = 1 
AND Setting.SettingID = deleted.SettingID
GO



--#####################################################################################################################
--######   CONSTRAINTs for ProjectSetting  ######################################################################################
--#####################################################################################################################


ALTER TABLE [dbo].[ProjectSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectSetting_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectSetting] CHECK CONSTRAINT [FK_ProjectSetting_Project]
GO

ALTER TABLE [dbo].[ProjectSetting]  WITH CHECK ADD  CONSTRAINT [FK_ProjectSetting_Setting] FOREIGN KEY([SettingID])
REFERENCES [dbo].[Setting] ([SettingID])
GO

ALTER TABLE [dbo].[ProjectSetting] CHECK CONSTRAINT [FK_ProjectSetting_Setting]
GO


--#####################################################################################################################
--######   Trigger for ProjectSetting  ######################################################################################
--#####################################################################################################################

/****** Object:  Trigger [dbo].[trgDelProjectSetting]    Script Date: 12.05.2015 11:42:41 ******/

CREATE TRIGGER [dbo].[trgDelProjectSetting] ON [dbo].[ProjectSetting] 
FOR DELETE AS 

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectSetting_Log (ProjectID, SettingID, Value, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.SettingID, deleted.Value, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'D'
FROM DELETED

GO



/****** Object:  Trigger [dbo].[trgUpdProjectSetting]    Script Date: 12.05.2015 11:42:50 ******/

CREATE TRIGGER [dbo].[trgUpdProjectSetting] ON [dbo].[ProjectSetting] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  DiversityProjects  3.0.2.0 */ 
/*  Date: 12.05.2015  */ 


/* saving the original dataset in the logging table */ 
INSERT INTO ProjectSetting_Log (ProjectID, SettingID, Value, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, RowGUID,  LogState) 
SELECT deleted.ProjectID, deleted.SettingID, deleted.Value, deleted.LogInsertedBy, deleted.LogInsertedWhen, deleted.LogUpdatedBy, deleted.LogUpdatedWhen, deleted.RowGUID,  'U'
FROM DELETED


/* updating the logging columns */
Update ProjectSetting
set LogUpdatedWhen = getdate(), LogUpdatedBy = SUSER_NAME()
FROM ProjectSetting, deleted 
where 1 = 1 
AND ProjectSetting.ProjectID = deleted.ProjectID
AND ProjectSetting.SettingID = deleted.SettingID
GO



--#####################################################################################################################
--######    SettingHierarchyAll  ######################################################################################
--#####################################################################################################################

CREATE FUNCTION [dbo].[SettingHierarchyAll] ()  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL )
/*
Returns a table that lists all the settings including a display text with the whole hierarchy.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyAll()
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, DisplayText
	FROM Setting S
	WHERE S.ParentSettingID IS NULL
	ORDER BY DisplayOrder

	declare @i int
	set @i = (select count(*) from Setting where SettingID not IN (select SettingID from  @SettingList))
	while (@i > 0)
		begin
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
		SELECT DISTINCT S.SettingID, S.ParentSettingID, S.Description, S.DisplayText, S.DisplayOrder, L.Hierarchy + ' | ' + S.DisplayText
		FROM Setting S, @SettingList L
		WHERE S.ParentSettingID = L.SettingID
		AND S.SettingID NOT IN (select SettingID from  @SettingList)
		set @i = (select count(*) from Setting S where SettingID not IN (select SettingID from  @SettingList))
	end
	Declare @A int
    RETURN
END

GO

GRANT SELECT ON dbo.[SettingHierarchyAll] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   SettingHierarchyChildren  ######################################################################################
--#####################################################################################################################


CREATE FUNCTION [dbo].[SettingHierarchyChildren] (@SettingID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings depending on a setting
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyChildren(4)
*/
AS
BEGIN
	-- adding the direct children
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
	FROM Setting AS H WHERE H.ParentSettingID = @SettingID
	ORDER BY DisplayOrder

	-- temporary table to keep the missing IDs
	declare @ID table ([SettingID] [int] Primary key)

	-- getting the IDs of the depending datasets missing so far
	insert into @ID 
	select distinct S.SettingID 
	from  @SettingList P, Setting S 
	where P.SettingID = S.ParentSettingID
	and S.SettingID not in (select SettingID from @ID)

	declare @i int
	set @i = (select count(*) from @ID)
	while (@i > 0)
	begin
		-- insert the missing children
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
		SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
		FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID
		AND L.SettingID NOT IN (SELECT SettingID FROM @SettingList)
		ORDER BY DisplayOrder

		-- clear the temporary table
		DELETE I FROM @ID I

		-- search for still missing children
		insert into @ID 
		select distinct S.SettingID 
		from  @SettingList P, Setting S 
		where P.SettingID = S.ParentSettingID
		and S.SettingID not in (select SettingID from @SettingList)

		-- count still missing children
		set @i = (select count(*) from @ID)
	end

    RETURN
END


GO

GRANT SELECT ON dbo.[SettingHierarchyChildren] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######   SettingHierarchyNonProject  ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingHierarchyNonProject] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings including a display text with the whole hierarchy that are not included in a project so far.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyNonProject(3)
*/
AS
BEGIN
	-- insert all settings into the list
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	-- get all setting ID that are a parent of another setting
	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	DELETE L FROM @SettingList L 
	WHERE L.SettingID IN (SELECT P.SettingID FROM ProjectSetting P WHERE P.ProjectID = @ProjectID)
	AND L.SettingID NOT IN (select SettingID from @ID)

	RETURN
END

GO

GRANT SELECT ON dbo.[SettingHierarchyNonProject] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######   SettingHierarchyProject  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[SettingHierarchyProject] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings including a display text with the whole hierarchy.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProject(3)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	UPDATE L SET Value = P.Value 
	FROM @SettingList L, ProjectSetting P
	WHERE L.SettingID = P.SettingID
	AND P.ProjectID = @ProjectID

	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	declare @i int
	set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	while (@i > 0)
	begin
		DELETE L FROM @SettingList L
		where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID)
		DELETE I FROM @ID I
		insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null
		set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	end
    RETURN
END

GO


GRANT SELECT ON dbo.[SettingHierarchyProject] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######  SettingHierarchyProjectAll  ######################################################################################
--#####################################################################################################################




CREATE FUNCTION [dbo].[SettingHierarchyProjectAll] (@ProjectID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Hierarchy] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ProjectID] int NULL,
	[Project] nvarchar(200) NULL)
/*
Returns a table that lists all the settings for a project including those inherited from parent projects.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProjectAll(55)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder, Hierarchy
	FROM dbo.SettingHierarchyAll() AS H
	ORDER BY DisplayOrder

	DECLARE @ProjectParents table ([ProjectID] [int] Primary key,
	[Project] [nvarchar] (50) NULL,
	[HierarchyLevel] int,
	[DisplayOrder] [int] NULL)

	INSERT INTO @ProjectParents (ProjectID, Project, HierarchyLevel)
	SELECT ProjectID, Project, HierarchyLevel
	FROM dbo.ProjectParents(@ProjectID) P
	ORDER BY P.HierarchyLevel

	DECLARE @HierarchyLevel INT
	DECLARE @ParentProject int
	WHILE (SELECT COUNT(*) FROM @ProjectParents) > 0
	BEGIN
		SET @HierarchyLevel = (SELECT MIN(HierarchyLevel) FROM @ProjectParents)
		SET @ParentProject = (SELECT ProjectID FROM @ProjectParents WHERE HierarchyLevel = @HierarchyLevel)
		UPDATE L SET Value = S.Value, L.Project = PP.Project, L.ProjectID = PP.ProjectID
		FROM @SettingList L, ProjectSetting S, @ProjectParents PP
		WHERE L.SettingID = S.SettingID
		AND S.ProjectID = @ParentProject
		AND PP.ProjectID = @ParentProject
		AND L.ProjectID IS NULL
		DELETE PP FROM @ProjectParents PP WHERE PP.HierarchyLevel = @HierarchyLevel
	END

	declare @ID table ([SettingID] [int] Primary key)
	insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null

	declare @i int
	set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	while (@i > 0)
	begin
		DELETE L FROM @SettingList L
		where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID)
		DELETE I FROM @ID I
		insert into @ID select distinct P.ParentSettingID from  @SettingList P where not P.ParentSettingID is null
		set @i = (select count(*) from @SettingList L where L.Value IS NULL AND L.SettingID not IN (select SettingID from @ID))
	end
    RETURN
END

GO

GRANT SELECT ON dbo.[SettingHierarchyProjectAll] TO [DiversityWorkbenchUser]
GO


--#####################################################################################################################
--######   SettingHierarchyProjectChildren  ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingHierarchyProjectChildren] (@SettingID int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [int] NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings depending on a setting
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingHierarchyProjectChildren(3)
*/
AS
BEGIN
	-- adding the direct children
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
	FROM Setting AS H WHERE H.ParentSettingID = @SettingID

	-- temporary table to keep the missing IDs
	declare @ID table ([SettingID] [int] Primary key)

	-- getting the IDs of the depending datasets missing so far
	insert into @ID 
	select distinct S.SettingID 
	from  @SettingList P, Setting S 
	where P.SettingID = S.ParentSettingID
	and S.SettingID not in (select SettingID from @ID)


	declare @i int
	set @i = (select count(*) from @ID)
	while (@i > 0)
	begin
		INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, DisplayOrder)
		SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText, DisplayOrder
		FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID

		DELETE I FROM @ID I

		insert into @ID 
		select distinct S.SettingID 
		from  @SettingList P, Setting S 
		where P.SettingID = S.ParentSettingID
		and S.SettingID not in (select SettingID from @ID)

		set @i = (select count(*) from @ID)
	end

	--INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText)
	--SELECT DISTINCT S.SettingID, ParentSettingID, Description, DisplayText
	--FROM Setting AS S, @ID L WHERE S.SettingID = L.SettingID

    RETURN
END


GO

GRANT SELECT ON dbo.[SettingHierarchyProjectChildren] TO [DiversityWorkbenchUser]
GO



--#####################################################################################################################
--######   [SettingsForProject]   ######################################################################################
--######   for external use by cache databases   ######################################################################################
--#####################################################################################################################



CREATE FUNCTION [dbo].[SettingsForProject] (@ProjectID int, @SettingFilter nvarchar(255), @Spacer nvarchar(20), @DisplayType int)  
RETURNS @SettingList TABLE ([SettingID] [int] Primary key ,
	[ParentSettingID] [int] NULL ,
	[DisplayText] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ProjectSetting] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL)
/*
Returns a table that lists all the settings for a project including those inherited from parent projects.
MW 20.02.2015
TEST:
SELECT * FROM DBO.SettingsForProject(55, '% ABCD %', '', 0)
SELECT * FROM DBO.SettingsForProject(55, '%ABCD | %', '.', 1)
SELECT * FROM DBO.SettingsForProject(55, '%ABCD | %', '.', 2)
SELECT * FROM DBO.SettingsForProject(3, '%ABCD | %', '.', 2)
*/
AS
BEGIN
	INSERT @SettingList (SettingID, ParentSettingID, Description, DisplayText, ProjectSetting, Value)
	SELECT DISTINCT SettingID, ParentSettingID, Description, DisplayText, Hierarchy, Value
	FROM dbo.SettingHierarchyProjectAll(@ProjectID) AS H

	IF @SettingFilter <> ''
	BEGIN
		DELETE L FROM @SettingList L
		WHERE L.ProjectSetting NOT LIKE @SettingFilter
	END

	UPDATE L SET L.Value = ''
	FROM @SettingList L
	WHERE L.Value IS NULL

	IF @DisplayType = 1
	BEGIN
		UPDATE L SET L.ProjectSetting = LTRIM(SUBSTRING(L.ProjectSetting, PATINDEX(@SettingFilter, L.ProjectSetting) , 4000))
		FROM @SettingList L
	END

	IF @DisplayType = 2
	BEGIN
		UPDATE L SET L.ProjectSetting = LTRIM(SUBSTRING(L.ProjectSetting, PATINDEX(@SettingFilter, L.ProjectSetting) + LEN(REPLACE(@SettingFilter, '%', '')) , 4000))
		FROM @SettingList L
	END


	IF @Spacer <> ''
	BEGIN
		UPDATE L SET L.ProjectSetting = REPLACE(L.ProjectSetting, ' | ', @Spacer)
		FROM @SettingList L
	END
    RETURN
END


GO


GRANT SELECT ON DBO.SettingsForProject TO [DiversityWorkbenchUser]
GO








--#####################################################################################################################
--######   [VersionClient]   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) 
AS 
BEGIN RETURN '03.00.02.00' END

GO




--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.01'
END

GO

