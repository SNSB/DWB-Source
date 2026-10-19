
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.02'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   AgentID_UserAvailable  ######################################################################################
--#####################################################################################################################

ALTER VIEW [dbo].[AgentID_UserAvailable]
AS
SELECT dbo.AgentProject.AgentID
FROM         dbo.AgentProject INNER JOIN
                      dbo.ProjectUser ON dbo.AgentProject.ProjectID = dbo.ProjectUser.ProjectID
WHERE     (dbo.ProjectUser.LoginName = USER_NAME())
UNION
SELECT AgentID FROM Agent 
WHERE AgentID NOT IN (SELECT AgentID FROM AgentProject)

GO




--#####################################################################################################################
--######   AgentNames   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[AgentNames] (@DisplayType nvarchar(50))  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentName] [nvarchar](200) NOT NULL)

/*
Returns a table that lists all the agents with their names according to the selected display type.
The types are documented in the table dbo.AgentNameDisplayType_Enum
MW 02.01.2006
Test:
select * from dbo.AgentNames ('Ii, G.') where AgentID in (24, 38, 41, 2760, 64, 94,149, 39427)

declare @AgentID int
set @AgentID = 25745--39466
select * from dbo.Agent where AgentID = @AgentID
select * from dbo.AgentNames ('Ii, G.') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('G. Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Ii, Gg') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Tt. Gg Ii') where AgentID in (@AgentID)
select * from dbo.AgentNames ('Tt. G. Ii') where AgentID in (@AgentID)

select * from dbo.Agent where AgentID = 39526
select * from dbo.Agent where AgentID in  (24, 38, 41, 2760, 64, 94,149)
select * from dbo.Agent where Givenname like '% % %'
select * from dbo.Agent where agentname like 'abel, g%'
select * from dbo.AgentNames ('Tt Gg Ii') order by AgentName
*/
AS
BEGIN
	INSERT @AgentList (AgentID, AgentName) 
	SELECT A.AgentID, 
	CASE WHEN A.AgentType <> 'Person' 
	THEN A.AgentName
	ELSE
	case when @DisplayType = 'Ii, G.'
	then 
		case when not A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
		+ case when A.InheritedName is null then '' else A.InheritedName end 
		+ case when not A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then A.InheritedNamePostfix + ' ' else '' end
		+ case when rtrim(A.GivenName) <> '' 
		then
			+ case when A.GivenName is null or rtrim(A.GivenName) = '' then ''  else ', ' end
				+ case when A.GivenName like '_._._._._._.' or A.GivenName like '_._._._._.' or A.GivenName like '_._._._.' or A.GivenName like '_._._.' or A.GivenName like '_._.' or A.GivenName like '_.' 
				or A.GivenName like '_. _. _.' or A.GivenName like '_. _.' 
				then A.GivenName 
				else
				case when A.GivenName like '%-%' then substring(A.GivenName, 1, 1) + '.' + '-' + substring(A.GivenName, charindex('-', A.GivenName) + 1, 1) + '.' 
				else
					case when not A.GivenName like '% %' 
					then substring(A.GivenName, 1, 1) + '.' 
					else
						case when not A.GivenName like '% % %' 
						then substring(A.GivenName, 1, 1) + '.' + substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '.' 
						else
							case when substring(A.GivenName, 1, 1) <> '(' then substring(A.GivenName, 1, 1) + '.'else '' end
							+ case when substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) <> '(' 
							then substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '.' 
							else '' end
							+ case when substring(substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255), charindex(' ', substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255)) + 1, 1) <> '(' 
							then substring(substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255), charindex(' ', substring(A.GivenName, charindex(' ', A.GivenName) + 1, 255)) + 1, 1) + '.'
							else '' end
						end
					end
				end
			end
		else ''
		end
		+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' 
			then case when rtrim(A.GivenName) <> '' then '' else ',' end + ' ' + A.GivenNamePostfix
			else '' end
	else
		case when @DisplayType = 'Ii, Gg'
		then
			ltrim(case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
			+ case when A.InheritedName is null then '' else ' ' + A.InheritedName end 
			+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
			+ case when A.GivenName is null then '' else ', ' + GivenName end
			+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then case when rtrim(A.GivenName) <> '' then '' else ',' end + ' ' + A.GivenNamePostfix else '' end)
		else
			case when @DisplayType = 'Gg Ii'
			then
				case when A.GivenName is null then '' else GivenName + ' ' end
				+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
				+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
				+ case when A.InheritedName is null then '' else A.InheritedName + ' ' end 
				+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
			else
				case when @DisplayType = 'G. Ii'
				then
					case when A.GivenName is null then '' else substring(A.GivenName, 1, 1) + '.' + ' ' end
					+ case when A.GivenName like '% %' and Not a.GivenName like '% (%' then substring(A.GivenName, charindex(' ', A.GivenName) + 1, 1) + '. ' else '' end
					+ case when A.GivenName like '% % %' and Not a.GivenName like '% % % %' and Not a.GivenName like '% % (%' then substring(reverse(A.GivenName), charindex(' ', reverse(A.GivenName)) - 1, 1) + '. 'else '' end
					+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
					+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
					+ case when A.InheritedName is null then '' else A.InheritedName + ' ' end 
					+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
				else
					case when @DisplayType = 'Tt. Gg Ii'
					then 
						case when A.AgentTitle is null then '' else A.AgentTitle + ' ' end
						+ case when A.GivenName is null then '' else A.GivenName + ' ' end
						+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
						+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
						+ case when A.InheritedName is null then '' else A.InheritedName end 
						+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
					else
						case when @DisplayType = 'Tt. G. Ii'
						then
							case when A.AgentTitle is null then '' else A.AgentTitle + ' ' end
							+ case when A.GivenName is null then '' else substring(A.GivenName, 1, 1) + '. ' end
							+ case when not A.GivenNamePostfix is null and A.GivenNamePostfix <> '' then A.GivenNamePostfix + ' ' else '' end
							+ case when A.InheritedNamePrefix is null and A.InheritedNamePrefix <> '' then A.InheritedNamePrefix + ' ' else '' end
							+ case when A.InheritedName is null then '' else A.InheritedName end
							+ case when A.InheritedNamePostfix is null and A.InheritedNamePostfix <> '' then ' ' + A.InheritedNamePostfix else '' end
						else
							case when @DisplayType = 'Abbr' and (not A.Abbreviation is null or A.Abbreviation = '')
							then A.Abbreviation
							else A.AgentName
							end
						end
					end
				end
			end
		end
	end
	END
	FROM dbo.Agent A

   RETURN
END

GO




--#####################################################################################################################
--######   AgentChildNodes   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[AgentChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   return
END

GO


ALTER FUNCTION [dbo].[AgentChildNodes] (@ID int)  
RETURNS @ItemList TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

/*
Returns a result set that lists all the items within a hierarchy starting at the topmost item related to the given item.
MW02.01.2006
*/
AS
BEGIN
   declare @ParentID int
   DECLARE @TempItem TABLE (
[AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)


	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
	SELECT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
	FROM Agent WHERE AgentParentID = @ID 

   DECLARE HierarchyCursor  CURSOR for
   select AgentID from @TempItem
   open HierarchyCursor
   FETCH next from HierarchyCursor into @ParentID
   WHILE @@FETCH_STATUS = 0
   BEGIN
	insert into @TempItem select AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason 
	from dbo.AgentChildNodes (@ParentID) where AgentID not in (select AgentID from @TempItem)
   	FETCH NEXT FROM HierarchyCursor into @ParentID
   END
   CLOSE HierarchyCursor
   DEALLOCATE HierarchyCursor
 INSERT @ItemList (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
   SELECT distinct AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM @TempItem ORDER BY AgentName
   RETURN
END

GO





--#####################################################################################################################
--######   AgentHierarchy   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[AgentHierarchy] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
*/
AS
BEGIN

-- getting the TopID
declare @TopID int
declare @i int
set @TopID = (select dbo.AgentTopID(@AgentID) )

-- get the ID's of the child nodes
DECLARE @TempItem TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL)

	INSERT @TempItem (AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason) 
	SELECT * FROM dbo.AgentChildNodes (@TopID)

-- copy the root node in the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM Agent
   WHERE Agent.AgentID = @TopID

   -- copy the child nodes into the result list
   INSERT @AgentList
   SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
	ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
	SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason
   FROM Agent
   WHERE Agent.AgentID in (select AgentID from @TempItem)
   ORDER BY AgentName

   RETURN
END






GO




--#####################################################################################################################
--######   AgentSuperiorList  ######################################################################################
--#####################################################################################################################




ALTER FUNCTION [dbo].[AgentSuperiorList] (@AgentID int)  
RETURNS @AgentList TABLE (
    [AgentID] [int] primary key,
	[AgentParentID] [int] NULL,
	[AgentName] [nvarchar](200) NOT NULL,
	[Version] [smallint] NOT NULL ,
	[AgentTitle] [nvarchar](50) NULL,
	[GivenName] [nvarchar](255) NULL,
	[InheritedName] [nvarchar](255) NULL,
	[Abbreviation] [nvarchar](50) NULL,
	[AgentType] [nvarchar](50) NULL,
	[AgentRole] [nvarchar](255) NULL,
	[Description] [nvarchar](1000) NULL,
	[Notes] [nvarchar](500) NULL,
	[ValidFromDate] [datetime] NULL,
	[ValidFromDay] [tinyint] NULL,
	[ValidFromMonth] [tinyint] NULL,
	[ValidFromYear] [smallint] NULL,
	[ValidUntilDate] [datetime] NULL,
	[ValidUntilDay] [tinyint] NULL,
	[ValidUntilMonth] [tinyint] NULL,
	[ValidUntilYear] [smallint] NULL,
	[ValidDateSupplement] [nvarchar](255) NULL,
	[SynonymToAgentID] [int] NULL,
	[SynonymisationType] [nvarchar](50) NULL,
	[RevisionLevel] [nvarchar](50) NULL,
	[PlaceOfBirth] [nvarchar](500) NULL,
	[PlaceOfDeath] [nvarchar](500) NULL,
	[DataWithholdingReason] [nvarchar](255) NULL,
	DisplayOrder int NOT NULL)

/*
Returns a table that lists all the Series related to the given Series.
MW 02.01.2006
Test:
select * from dbo.AgentSuperiorList(12665)
*/
AS
BEGIN
	declare @i int
	set @i = 1
	while (not @AgentID is null)
		begin

			INSERT @AgentList
			SELECT DISTINCT AgentID, AgentParentID, AgentName, Version, AgentTitle, GivenName, InheritedName, Abbreviation, AgentType, AgentRole, Description, Notes, 
			ValidFromDate, ValidFromDay, ValidFromMonth, ValidFromYear, ValidUntilDate, ValidUntilDay, ValidUntilMonth, ValidUntilYear, ValidDateSupplement, 
			SynonymToAgentID, SynonymisationType, RevisionLevel, PlaceOfBirth, PlaceOfDeath, DataWithholdingReason, @i
			FROM Agent
			WHERE Agent.AgentID = @AgentID

			set @AgentID = (select AgentParentID from Agent where AgentID = @AgentID)

			set @i = (select @i + 1)

		end

   RETURN
END

GO








--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################



declare @SQL nvarchar(4000)
set @SQL = 'CREATE FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN ''03.00.00.16'' 
END'; 


begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 2000)
exec sp_executesql @SQL
end catch


GO

GRANT EXEC ON dbo.VersionClient TO DiversityWorkbenchUser
GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################




ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.03'
END

GO

select [dbo].[Version] ()  

