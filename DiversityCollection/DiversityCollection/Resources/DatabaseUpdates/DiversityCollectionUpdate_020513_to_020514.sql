--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK

declare @Version varchar(10)
set @Version = (SELECT [dbo].[Version]())
IF (SELECT [dbo].[Version]()) <> '02.05.12'
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version 02.05.12. Current version = ' + @Version
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######   [CollectionHierarchyAll]   ######################################################################################
--#####################################################################################################################



--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO

CREATE FUNCTION [dbo].[CollectionHierarchyAll] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayText] [varchar] (900) COLLATE Latin1_General_CI_AS NULL)

/*
Returns a table that lists all the collections including a display text with the whole hierarchy.
MW 02.01.2006
TEST:
SELECT * FROM DBO.CollectionHierarchyAll()
*/
AS
BEGIN

-- getting the TopIDs
INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
SELECT DISTINCT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder
, case when CollectionAcronym IS NULL OR CollectionAcronym = '' then CollectionName else CollectionAcronym end
FROM Collection
WHERE Collection.CollectionParentID IS NULL

declare @i int
set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))

-- getting the childs
while (@i > 0)
	begin
	
	INSERT @CollectionList (CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder, DisplayText)
	SELECT DISTINCT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, C.CollectionOwner
	, C.DisplayOrder, L.DisplayText + ' | ' + C.CollectionName
	FROM Collection C, @CollectionList L
	WHERE C.CollectionParentID = L.CollectionID
	AND C.CollectionID NOT IN (select CollectionID from  @CollectionList)

	set @i = (select count(*) from Collection where CollectionID not IN (select CollectionID from  @CollectionList))
end

   RETURN
END
GO


grant select on [dbo].[CollectionHierarchyAll] to DiversityCollectionUser
go


--#####################################################################################################################
--######   Umbau Entity   ######################################################################################
--#####################################################################################################################


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_UBT_Courses
--use DiversityCollection_UBT_DANECO
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK

GO


ALTER TABLE EntityUsage ADD Accessibility nvarchar(50) NULL
GO

ALTER TABLE EntityUsage ADD Determination nvarchar(50) NULL
GO

ALTER TABLE EntityUsage ADD Visibility nvarchar(50) NULL
GO


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO



CREATE TABLE [dbo].[EntityVisibility_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityVisibility_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the DiversityWorkbench may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityVisibility_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
/****** Object:  Table [dbo].[EntityDetermination_Enum]    Script Date: 09/23/2011 13:44:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDetermination_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityDetermination_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the DiversityWorkbench may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityDetermination_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
/****** Object:  Table [dbo].[EntityAccessibility_Enum]    Script Date: 09/23/2011 13:44:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityAccessibility_Enum](
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[DisplayOrder] [smallint] NULL,
	[DisplayEnable] [bit] NULL,
	[InternalNotes] [nvarchar](500) NULL,
	[ParentCode] [nvarchar](50) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityAccessibility_Enum] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A text code that uniquely identifies each object in the enumeration (primary key). This value may not be changed, because the DiversityWorkbench may depend upon it.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enumerated object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short abbreviated description of the object, displayed in the user interface' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The order in which the entries are displayed. The order may be changed at any time, but all values must be unique.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enumerated objects can be hidden from the user interface if this attribute is set to false (= unchecked check box)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'DisplayEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal development notes about usage, definition, etc. of an enumerated object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'InternalNotes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The code of the superior entry, if a hierarchy within the entries is necessary' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntityAccessibility_Enum', @level2type=N'COLUMN',@level2name=N'ParentCode'
GO
/****** Object:  Default [DF_EntityAccessibility_Enum_RowGUID]    Script Date: 09/23/2011 13:44:46 ******/
ALTER TABLE [dbo].[EntityAccessibility_Enum] ADD  CONSTRAINT [DF_EntityAccessibility_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
/****** Object:  Default [DF_EntityDetermination_Enum_RowGUID]    Script Date: 09/23/2011 13:44:46 ******/
ALTER TABLE [dbo].[EntityDetermination_Enum] ADD  CONSTRAINT [DF_EntityDetermination_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO
/****** Object:  Default [DF_EntityVisibility_Enum_RowGUID]    Script Date: 09/23/2011 13:44:46 ******/
ALTER TABLE [dbo].[EntityVisibility_Enum] ADD  CONSTRAINT [DF_EntityVisibility_Enum_RowGUID]  DEFAULT (newsequentialid()) FOR [RowGUID]
GO


grant select on [dbo].[EntityAccessibility_Enum] to DiversityCollectionUser
go
grant select on [dbo].[EntityDetermination_Enum] to DiversityCollectionUser
go
grant select on [dbo].[EntityVisibility_Enum] to DiversityCollectionUser
go


--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO



INSERT INTO [EntityAccessibility_Enum]
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode]
           ,[RowGUID])
SELECT [Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable]
      ,[InternalNotes]
      ,[ParentCode]
      ,[RowGUID]
  FROM [DiversityCollection].[dbo].[EntityAccessibility_Enum]
GO


INSERT INTO EntityDetermination_Enum
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode]
           ,[RowGUID])
SELECT [Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable]
      ,[InternalNotes]
      ,[ParentCode]
      ,[RowGUID]
  FROM [DiversityCollection].[dbo].EntityDetermination_Enum
GO


INSERT INTO EntityVisibility_Enum
           ([Code]
           ,[Description]
           ,[DisplayText]
           ,[DisplayOrder]
           ,[DisplayEnable]
           ,[InternalNotes]
           ,[ParentCode]
           ,[RowGUID])
SELECT [Code]
      ,[Description]
      ,[DisplayText]
      ,[DisplayOrder]
      ,[DisplayEnable]
      ,[InternalNotes]
      ,[ParentCode]
      ,[RowGUID]
  FROM [DiversityCollection].[dbo].EntityVisibility_Enum
GO


--#####################################################################################################################
--######   [EntityInformation_2]   ######################################################################################
--#####################################################################################################################




--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO


ALTER TABLE EntityUsage ADD Accessibility [nvarchar] (50) NULL
GO
ALTER TABLE EntityUsage ADD Determination [nvarchar] (50) NULL
GO
ALTER TABLE EntityUsage ADD Visibility [nvarchar] (50) NULL
GO

 


CREATE FUNCTION [dbo].[EntityInformation_2] (@Entity [varchar] (500), @Language nvarchar(50), @Context nvarchar(50))  
RETURNS @EntityList TABLE ([Entity] [varchar] (500) Primary key ,
	[DisplayGroup] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[Accessibility] [nvarchar](50) NULL,
	[Determination] [nvarchar](50) NULL,
	[Visibility] [nvarchar](50) NULL,
	[PresetValue] [nvarchar](500) NULL,
	[DoesExist] [bit] NULL,
	[DisplayTextOK] [bit] NULL,
	[AbbreviationOK] [bit] NULL,
	[DescriptionOK] [bit] NULL)
/*
Returns the information to an entity.
MW 26.09.2011
Test:
select * from [dbo].[EntityInformation_2] ('CollCircumstances_Enum.Code.bred', 'de-DE', 'General')
select * from [dbo].[EntityInformation_2] ('CollLabelType_Enum', 'de-DE', 'Observation.mobile')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', '', '')
select * from [dbo].[EntityInformation_2] ('EntityUsage.PresetValue', '', '')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('Identification.IdentificationDate', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'de-DE', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('Test', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('CollMaterialCategory_Enum.Code.drawing or photograph', 'de-DE', 'General')
*/

AS
BEGIN

if @Context = '' begin set @Context = 'General' end

-- fill the list with the basic data from the table Entity

insert @EntityList (Entity, [DisplayGroup]
	, [DoesExist], [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
SELECT TOP 1 [Entity]
      ,[DisplayGroup]
      ,1, 0, 0, 0
  FROM [Entity]
WHERE Entity = @Entity


-- if nothing is found, fill in the values according to the parameters of the function

if (select count(*) from @EntityList) = 0
begin
	insert @EntityList (Entity, DisplayText, Abbreviation, DoesExist, [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
	Values (@Entity, 
	case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 
	substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
	, 0, 0, 0, 0)

	declare @Table nvarchar(50)
	declare @Column nvarchar(50)
	if (@Entity not like '%.%')
	begin
	set @Table = @Entity
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value]))
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, default, NULL) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (@Entity like '%.%' and @Entity not like '%.%.%')
	begin
	set @Table = (select rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)))
	set @Column = (select rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)))
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value])) 
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, 'column', @Column) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (select count(*) from @EntityList where len([Description]) > 0) = 0
	begin
	update E set E.[Description] = case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) 
	+ case when @Entity like '%.%' then ' - ' + rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)) else '' end
	else @Entity end
	from @EntityList E 
	end
	
	if (@Entity like '%_Enum.Code.%')
	begin
	declare @EnumTable nvarchar(50)
	set @EnumTable = (select substring(@Entity, 1, charindex('.', @Entity) - 1))
	declare @Value nvarchar(50)
	set @Value = (select rtrim(substring(@Entity, charindex('.', @Entity) + 6, 255)))
	update E set E.[Description] = @Value, E.DisplayText = substring(@Value, 1, 50), E.Abbreviation = substring(@Value, 1, 20)
	from @EntityList E 
	end
	return
end


-- set the details for usage and representation of the entry

declare @ParentContext nvarchar(50)
declare @CurrentContext nvarchar(50)


-- set the Accessibility, Determination, Visibility and PresetValue if there is one --################################################################
update E set
E.Accessibility = U.Accessibility,
E.Determination = U.Determination, 
E.Visibility = U.Visibility, 
E.PresetValue = U.PresetValue
from dbo.EntityUsage U, @EntityList E
where U.Entity = @Entity
and U.EntityContext = @Context

-- search for usage information in parent datasets
set @CurrentContext = @Context
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.Accessibility = case when E.Accessibility is null then U.Accessibility else E.Accessibility end,
	E.Determination = case when E.Determination is null then U.Determination else E.Determination end,
	E.Visibility = case when E.Visibility is null then U.Visibility else E.Visibility end,
	E.PresetValue = case when E.PresetValue is null then U.PresetValue else E.PresetValue end
	from dbo.EntityUsage U, @EntityList E
	where U.Entity = @Entity
	and U.EntityContext = @ParentContext
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- set the representation values --################################################################
update E set E.[DisplayText] = R.DisplayText, 
E.Abbreviation = R.Abbreviation, 
E.[Description] = R.[Description],
E.[DisplayTextOK] = case when R.DisplayText is null or R.DisplayText = '' then 0 else 1 end, 
E.[AbbreviationOK] = case when R.Abbreviation is null or R.Abbreviation = '' then 0 else 1 end, 
E.[DescriptionOK] = case when R.[Description] is null or R.[Description] = '' then 0 else 1 end
from dbo.EntityRepresentation R, @EntityList E
where R.Entity = @Entity
and R.EntityContext = @Context
and R.LanguageCode = @Language

-- search for representation values in parent datasets in the same language if nothing is found
set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @Context)
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = @Language
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- search for representation values in parent datasets in the default language if nothing is found
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = 'en-US'
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end

update E set E.[DisplayText] = substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
from @EntityList E 
where E.DisplayText is null

update E set E.Abbreviation = substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
from @EntityList E 
where E.Abbreviation is null


   RETURN
END



GO


grant select on [EntityInformation_2] to DiversityCollectionUser
GO








--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO





ALTER FUNCTION [dbo].[EntityInformation_2] (@Entity [varchar] (500), @Language nvarchar(50), @Context nvarchar(50))  
RETURNS @EntityList TABLE ([Entity] [varchar] (500) Primary key ,
	[DisplayGroup] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[Accessibility] [nvarchar](50) NULL,
	[Determination] [nvarchar](50) NULL,
	[Visibility] [nvarchar](50) NULL,
	[PresetValue] [nvarchar](500) NULL,
	[UsageNotes] [nvarchar](4000) NULL,
	[DoesExist] [bit] NULL,
	[DisplayTextOK] [bit] NULL,
	[AbbreviationOK] [bit] NULL,
	[DescriptionOK] [bit] NULL)
/*
Returns the information to an entity.
MW 26.09.2011
Test:
select * from [dbo].[EntityInformation_2] ('CollCircumstances_Enum.Code.bred', 'de-DE', 'General')
select * from [dbo].[EntityInformation_2] ('CollLabelType_Enum', 'de-DE', 'Observation.mobile')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'de-DE', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', 'en-US', '')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen', '', '')
select * from [dbo].[EntityInformation_2] ('EntityUsage.PresetValue', '', '')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'Observation')
select * from [dbo].[EntityInformation_2] ('Identification.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('CollectionSpecimen.CollectionSpecimenID', 'en-US', 'General')
select * from [dbo].[EntityInformation_2] ('Identification.IdentificationDate', 'de-DE', 'Observation')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'de-DE', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('IdentificationUnit.TaxonomicGroup', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('Test', 'en-US', 'Observation.Mobile')
select * from [dbo].[EntityInformation_2] ('CollMaterialCategory_Enum.Code.drawing or photograph', 'de-DE', 'General')
*/

AS
BEGIN

if @Context = '' begin set @Context = 'General' end

-- fill the list with the basic data from the table Entity

insert @EntityList (Entity, [DisplayGroup]
	, [DoesExist], [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
SELECT TOP 1 [Entity]
      ,[DisplayGroup]
      ,1, 0, 0, 0
  FROM [Entity]
WHERE Entity = @Entity


-- if nothing is found, fill in the values according to the parameters of the function

if (select count(*) from @EntityList) = 0
begin
	insert @EntityList (Entity, DisplayText, Abbreviation, DoesExist, [DisplayTextOK], [AbbreviationOK], [DescriptionOK]) 
	Values (@Entity, 
	case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 
	substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
	, 0, 0, 0, 0)

	declare @Table nvarchar(50)
	declare @Column nvarchar(50)
	if (@Entity not like '%.%')
	begin
	set @Table = @Entity
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value]))
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, default, NULL) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (@Entity like '%.%' and @Entity not like '%.%.%')
	begin
	set @Table = (select rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)))
	set @Column = (select rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)))
	update E set E.[Description] = (SELECT max(CONVERT(nvarchar(MAX), [value])) 
	FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @Table, 'column', @Column) WHERE name =  'MS_Description')
	, E.[DescriptionOK] = 1
	from @EntityList E 
	end
	
	if (select count(*) from @EntityList where len([Description]) > 0) = 0
	begin
	update E set E.[Description] = case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) 
	+ case when @Entity like '%.%' then ' - ' + rtrim(substring(@Entity, 1, charindex('.', @Entity)-1)) else '' end
	else @Entity end
	from @EntityList E 
	end
	
	if (@Entity like '%_Enum.Code.%')
	begin
	declare @EnumTable nvarchar(50)
	set @EnumTable = (select substring(@Entity, 1, charindex('.', @Entity) - 1))
	declare @Value nvarchar(50)
	set @Value = (select rtrim(substring(@Entity, charindex('.', @Entity) + 6, 255)))
	update E set E.[Description] = @Value, E.DisplayText = substring(@Value, 1, 50), E.Abbreviation = substring(@Value, 1, 20)
	from @EntityList E 
	end
	return
end


-- set the details for usage and representation of the entry

declare @ParentContext nvarchar(50)
declare @CurrentContext nvarchar(50)


-- set the Accessibility, Determination, Visibility and PresetValue if there is one --################################################################
update E set
E.Accessibility = U.Accessibility,
E.Determination = U.Determination, 
E.Visibility = U.Visibility, 
E.PresetValue = U.PresetValue, 
E.UsageNotes = U.Notes
from dbo.EntityUsage U, @EntityList E
where U.Entity = @Entity
and U.EntityContext = @Context

-- search for usage information in parent datasets
set @CurrentContext = @Context
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.Accessibility = case when E.Accessibility is null then U.Accessibility else E.Accessibility end,
	E.Determination = case when E.Determination is null then U.Determination else E.Determination end,
	E.Visibility = case when E.Visibility is null then U.Visibility else E.Visibility end,
	E.PresetValue = case when E.PresetValue is null then U.PresetValue else E.PresetValue end,
	E.UsageNotes = case when E.UsageNotes is null then U.Notes else E.UsageNotes end
	from dbo.EntityUsage U, @EntityList E
	where U.Entity = @Entity
	and U.EntityContext = @ParentContext
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- set the representation values --################################################################
update E set E.[DisplayText] = R.DisplayText, 
E.Abbreviation = R.Abbreviation, 
E.[Description] = R.[Description],
E.[DisplayTextOK] = case when R.DisplayText is null or R.DisplayText = '' then 0 else 1 end, 
E.[AbbreviationOK] = case when R.Abbreviation is null or R.Abbreviation = '' then 0 else 1 end, 
E.[DescriptionOK] = case when R.[Description] is null or R.[Description] = '' then 0 else 1 end
from dbo.EntityRepresentation R, @EntityList E
where R.Entity = @Entity
and R.EntityContext = @Context
and R.LanguageCode = @Language

-- search for representation values in parent datasets in the same language if nothing is found
set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @Context)
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = @Language
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end


-- search for representation values in parent datasets in the default language if nothing is found
set @ParentContext = @Context
while not @ParentContext is null
begin
	update E set 
	E.[DisplayText] = case when E.DisplayText is null then R.DisplayText else E.DisplayText end, 
	E.Abbreviation = case when E.Abbreviation is null then R.Abbreviation else E.Abbreviation end, 
	E.[Description] = case when E.[Description] is null then R.[Description] else E.[Description] end,
	E.[DisplayTextOK] = case when E.[DisplayTextOK] = 0 and R.[DisplayText] <> '' then 1 else E.[DisplayTextOK] end, 
	E.[AbbreviationOK] = case when E.[AbbreviationOK] = 0 and R.[Abbreviation] <> '' then 1 else E.[AbbreviationOK] end, 
	E.[DescriptionOK] = case when E.[DescriptionOK] = 0 and R.[Description] <> '' then 1 else E.[DescriptionOK] end
	from dbo.EntityRepresentation R, @EntityList E
	where R.Entity = @Entity
	and R.EntityContext = @ParentContext
	and R.LanguageCode = 'en-US'
	
	set @CurrentContext = @ParentContext
	set @ParentContext = (select ParentCode from dbo.EntityContext_Enum where Code = @CurrentContext)
	
	-- avoid loops on itself
	if (@ParentContext = @CurrentContext) begin set @ParentContext = null end
end

update E set E.[DisplayText] = substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
from @EntityList E 
where E.DisplayText is null

update E set E.Abbreviation = substring(case when @Entity like '%.%' then rtrim(substring(@Entity, charindex('.', @Entity)+1, 50)) else @Entity end, 1, 20)
from @EntityList E 
where E.Abbreviation is null


   RETURN
END

GO


--#####################################################################################################################
--######   [TableDescription]    ######################################################################################
--#####################################################################################################################

--use DiversityCollection
--use DiversityCollection_CONC
--use DiversityCollection_BASE
--use DiversityCollection_BASETest
--use DiversityCollection_Monitoring
--use DiversityCollection_Test
--use DiversityCollection_TestPart
--use DiversityCollection_TestUpdate
--use DiversityCollection_Training
--use DiversityCollection_UBT
--use DiversityCollection_SMNK
--use DiversityCollection_ZFMK
GO

alter FUNCTION [dbo].[TableDescription] (@TableName [varchar] (500), @Language nvarchar(50), @Context nvarchar(50))  
RETURNS @ColumnList TABLE ([ColumnName] [varchar] (500) Primary key ,
	[DateType] [nvarchar](50) NULL,
	[DisplayGroup] [nvarchar](50) NULL,
	[DisplayText] [nvarchar](50) NULL,
	[Abbreviation] [nvarchar](20) NULL,
	[Description] [nvarchar](max) NULL,
	[Accessibility] [nvarchar](50) NULL,
	[Determination] [nvarchar](50) NULL,
	[Visibility] [nvarchar](50) NULL,
	[UsageNotes] [nvarchar](4000) NULL,
	[PresetValue] [nvarchar](500) NULL,
	[DefaultValue] [nvarchar](4000) NULL,
	[RelationTable] [nvarchar](50) NULL,
	[RelationColumn] [nvarchar](50) NULL,
	[Nullable] [varchar] (3) NULL)
/*
Returns the information to an entity.
MW 26.09.2011
Test:
select * from [dbo].[TableDescription] ('CollectionEventLocalisation', 'de-DE', 'Monitoring')
select * from [dbo].[TableDescription] ('CollectionEventLocalisation', 'en-US', 'Monitoring')
select * from [dbo].[TableDescription] ('CollectionEvent', 'en-US', 'Monitoring')
select * from [dbo].[TableDescription] ('Identification', 'en-US', 'Monitoring')
*/

AS
BEGIN

if @Context = '' begin set @Context = 'General' end

-- fill the list with the basic data from the table Entity

insert @ColumnList (ColumnName, [DateType], [Nullable]) 
select C.COLUMN_NAME
, C.DATA_TYPE + case when C.CHARACTER_MAXIMUM_LENGTH IS null then '' else
	case when C.CHARACTER_MAXIMUM_LENGTH = -1 
	then case when C.DATA_TYPE = 'geography' then '' else ' (MAX)' end 
	else ' (' + cast(C.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' end end
, C.IS_NULLABLE 
from INFORMATION_SCHEMA.COLUMNS C
where C.TABLE_NAME = @TableName
and C.COLUMN_NAME not like 'xx_%'
order by c.ORDINAL_POSITION

--SELECT TOP 1 [Entity]
--      ,[DisplayGroup]
--  FROM [Entity]
--WHERE Entity = @TableName

Declare @ColumnName nvarchar(50)
DECLARE ColumnCursor  CURSOR for
select ColumnName from @ColumnList
open ColumnCursor
FETCH next from ColumnCursor into @ColumnName
WHILE @@FETCH_STATUS = 0
	BEGIN
	update L set L.Abbreviation = E.Abbreviation
	, L.Description = e.Description
	, L.Accessibility = E.Accessibility
	, L.Visibility = E.Visibility
	, L.UsageNotes = e.UsageNotes
	, L.DisplayGroup = e.DisplayGroup
	, L.Determination = E.Determination
	, L.PresetValue = e.PresetValue
	from @ColumnList L, dbo.EntityInformation_2(@TableName + '.' + @ColumnName, @Language, @Context) E
	where L.ColumnName = @ColumnName
	FETCH NEXT FROM ColumnCursor into @ColumnName
	END
CLOSE ColumnCursor
DEALLOCATE ColumnCursor

update L SET L.DefaultValue = C.COLUMN_DEFAULT, L.RelationTable = P.TABLE_NAME, L.RelationColumn = P.COLUMN_NAME
FROM @ColumnList L, 
INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R 
INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME 
INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME 
RIGHT OUTER JOIN INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME 
AND F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG  
WHERE C.TABLE_NAME = @TableName and L.ColumnName = C.COLUMN_NAME

   RETURN
END

GO

GRANT SELECT ON [TableDescription] TO DiversityCollectionUser
GO

--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



/*

ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.13'
END

GO
*/
