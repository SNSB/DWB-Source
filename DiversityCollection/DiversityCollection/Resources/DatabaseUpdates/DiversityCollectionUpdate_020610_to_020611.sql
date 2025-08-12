declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.10'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  IdentificationUnit_log - adding RetrievalType if missing  ###################################################
--#####################################################################################################################

if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'IdentificationUnit_log' and C.COLUMN_NAME = 'RetrievalType') = 0
begin
	ALTER TABLE IdentificationUnit_log ADD RetrievalType nvarchar(50) NULL
end
GO


--#####################################################################################################################
--######  NextFreeAccNumber   #########################################################################################
--######  Bugfix in first while:  AND @EndPos > 0   ###################################################################
--######  removing matches in the database without trim to ensure index usage   #######################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[NextFreeAccNumber] (@AccessionNumber nvarchar(50), @IncludeSpecimen bit, @IncludePart bit)  
/*
returns next free accession number for parts or specimen
optionally including either parts or specimen
assumes that accession numbers have a pattern like M-0023423 or HAL 25345 or GLM3453
with a leading string and a numeric end
MW 05.09.2013
TEST:
select dbo.[NextFreeAccNumber] ('0033933', 1, 1)
select dbo.[NextFreeAccNumber] ('0033933', 0, 1)
select dbo.[NextFreeAccNumber] ('00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-00041009', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014474', 0, 1)
select dbo.[NextFreeAccNumber] ('ZSM_DIP-000', 1, 1)
select dbo.[NextFreeAccNumber] ('1907/9', 1, 1)
select dbo.[NextFreeAccNumber] ('ZSM-MA-9', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0013622', 1, 1)
select dbo.[NextFreeAccNumber] ('M-0014900', 1, 1)
select dbo.[NextFreeAccNumber] ('MB-FA-000001', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-FA-000101', 1, 1) 
select dbo.[NextFreeAccNumber] ('MB-006118', 1, 0)
select dbo.[NextFreeAccNumber] ('SMNS-B-PH-2017/942', 1, 1) 
select dbo.[NextFreeAccNumber] ('SMNS-B-PH-2017/02196', 1, 1) 
select dbo.[NextFreeAccNumber] ('SMNK-ARA 08643', 1, 1)
select dbo.[NextFreeAccNumber] ('Test', 1, 1)
*/
RETURNS nvarchar (50)
AS
BEGIN 
-- declaration of variables
declare @NextAcc nvarchar(50)			-- the result of the function
	set @NextAcc = null
declare @Start int						-- the numeric starting value
declare @NumericStartString nvarchar(50)-- the string containing the numeric part of the accession number
declare @NumericStartLength int			-- the length of the numeric part of the accession number
declare @EndString nvarchar(50)			-- an end of the accession number that is not numeric
	set @EndString = ''
declare @Position tinyint				-- the starting position of the numeric part 
declare @LastNumber int					-- the last numeric value that has been generated for testing										
declare @T Table (ID int identity(1, 1),-- temporary table keeping the numbers
	NumericPart int NULL,
    NumericString nvarchar(50) NULL,
    AccessionNumberGenerated nvarchar(50) NULL,
    AccessionNumberInCollection nvarchar(50) NULL)
declare @LastAccessionNumber nvarchar(50)-- the last AccessionNumber that has been generated for testing
	set @LastAccessionNumber = @AccessionNumber

-- getting the starting parameters of the accession number
declare @Prefix nvarchar(50)
set @Position = len(@AccessionNumber) 
if (@AccessionNumber NOT LIKE '%[^0-9]%')
begin
	set @Prefix = substring(@AccessionNumber, 1, len(@AccessionNumber) - len(cast(cast(@AccessionNumber as int) as varchar)))
	set @Start = cast(@AccessionNumber as int)
end
else
begin
	if (substring(reverse(@AccessionNumber), 1, 1) NOT LIKE  '[0-9]')
	begin
		set @EndString = substring(reverse(@AccessionNumber), 1, 1)
		declare @EndPos int
		set @EndPos = LEN(@AccessionNumber) - 1
		while (substring(@AccessionNumber, @EndPos, 1) NOT LIKE  '[0-9]' AND @EndPos > 0)
		begin
			set @EndString = substring(@AccessionNumber, @EndPos, 1) + @EndString
			set @EndPos = @EndPos - 1
		end
	end
	if(@EndString <> '')
	begin
		set @Position = @Position - len(@EndString)
	end
	while (substring(@AccessionNumber, @Position, 1) LIKE '[0-9]')
	begin
		set @NumericStartString = substring(@AccessionNumber, @Position, len(@AccessionNumber) - len(@EndString) - len(@Prefix) + 1)
		set @Start = CAST(@NumericStartString as int)
		set @Position = @Position - 1
		set @Prefix = substring(@AccessionNumber, 1, @Position)
	end
end

set @NumericStartLength = len(@NumericStartString)

-- getting leeding 0
declare @Leeding0 varchar(50)
declare @iLeed int
set @iLeed = @Position + 1
set @Leeding0 = ''
while (substring(@AccessionNumber, @iLeed, 1) = '0')
begin
	set @Leeding0 = @Leeding0 + '0'
	set @iLeed = @iLeed + 1
end

declare @LengthNumericPart int
set @LengthNumericPart = len(@NumericStartString)

-- walk through the existing numbers and find a not used number
while @NextAcc is null
begin
	-- filling the temporary table
	if (@IncludeSpecimen = 1)
	begin    
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT DISTINCT TOP 100  AccessionNumber 
		FROM CollectionSpecimen  
		WHERE AccessionNumber LIKE @Prefix + '%'
		AND AccessionNumber > @LastAccessionNumber
		AND isnumeric(SUBSTRING(AccessionNumber, len(@Prefix) + 1, 50)) = 1
	end
	if (@IncludePart = 1)
	begin    
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT DISTINCT TOP 100 AccessionNumber 
		FROM CollectionSpecimenPart  
		WHERE AccessionNumber LIKE @Prefix + '%'
		AND AccessionNumber > @LastAccessionNumber
		AND isnumeric(SUBSTRING(AccessionNumber, len(@Prefix) + 1, 50)) = 1
	end
	if (select COUNT(*) from @T) = 0
	begin
		INSERT INTO @T (AccessionNumberInCollection)
		SELECT @LastAccessionNumber 
	end

	-- setting the numbers
	UPDATE @T SET NumericPart = ID + @Start; 
	UPDATE @T SET NumericString = @Leeding0 + cast(NumericPart as varchar);

	-- if the with is restricted, shorten it
	if (@Leeding0 <> '')
	begin
		UPDATE @T SET NumericString = substring(NumericString, len(NumericString) - @LengthNumericPart + 1, @LengthNumericPart)
		WHERE len(NumericString) > @LengthNumericPart;
	end

	-- set the generated numbers
	UPDATE @T SET AccessionNumberGenerated = @Prefix + NumericString;

	-- start for the next circle
	set @LastAccessionNumber = (select max(AccessionNumberGenerated) from @T)
	set @LastNumber = (select max(NumericPart) from @T)

	-- remove those that are fitting
	if (select count(*) from @T) > 1
	begin
		delete from @T where AccessionNumberGenerated = rtrim(ltrim(AccessionNumberInCollection))
	end
	-- remove matches in the database
	if (select count(*) from @T) > 0
	begin
		Delete T from @T T inner join CollectionSpecimen S on S.AccessionNumber = T.AccessionNumberGenerated
		--Delete T from @T T inner join CollectionSpecimen S on rtrim(ltrim(S.AccessionNumber)) =rtrim(ltrim( T.AccessionNumberGenerated))
	end

	if (select count(*) from @T) > 0
	begin
		set @NextAcc = (select min(AccessionNumberGenerated) + @EndString from @T)
	end

	-- if nothing is left, exit the while loop
	if (select count(*) from CollectionSpecimen S where S.AccessionNumber > @LastAccessionNumber) = 0
	begin
		if (@NextAcc is null)
		begin
			set @NextAcc = ''
		end
	end

end

if (@NextAcc = '' AND not @LastNumber is null)
begin
	-- construction of the numeric part
	set @NextAcc = @Leeding0 + cast(@LastNumber + 1 as varchar)
	if (Len(@Leeding0) > 0 and len(@NextAcc) > @NumericStartLength)
		set @NextAcc = substring(@NextAcc, len(@NextAcc) - @NumericStartLength, 50)
	set @NextAcc = @Prefix + @NextAcc + @EndString
end

return (@NextAcc)
END

GO

--#####################################################################################################################
--######  CollectionManager - Delete entries together with collection  ################################################
--#####################################################################################################################

ALTER TABLE [dbo].[CollectionManager] DROP CONSTRAINT [FK_CollectionManager_Collection]
GO

ALTER TABLE [dbo].[CollectionManager]  WITH CHECK ADD  CONSTRAINT [FK_CollectionManager_Collection] FOREIGN KEY([AdministratingCollectionID])
REFERENCES [dbo].[Collection] ([CollectionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CollectionManager] CHECK CONSTRAINT [FK_CollectionManager_Collection]
GO


--#####################################################################################################################
--######  IdentificationUnit.NumberOfUnitsModifier -> nvarchar(100)  ##################################################
--#####################################################################################################################

alter TABLE [dbo].[IdentificationUnit] alter column [NumberOfUnitsModifier] nvarchar(100) null
GO

alter TABLE [dbo].[IdentificationUnit_log] alter column [NumberOfUnitsModifier] nvarchar(100) null
GO

--#####################################################################################################################
--######  UserCollectionList - bugfix  ################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[UserCollectionList] ()  
RETURNS @CollectionList TABLE ([CollectionID] [int] Primary key ,
	[CollectionParentID] [int] NULL ,
	[CollectionName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionAcronym] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactName] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL ,
	[AdministrativeContactAgentURI] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL ,
	[Location] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL ,
	[CollectionOwner]  [nvarchar]  (255) COLLATE Latin1_General_CI_AS NULL ,
	[DisplayOrder] [smallint])
/*
Returns a table that lists all the collections a User has access to, including the child collections.
MW 20.02.2012
Test:
select * from dbo.UserCollectionList()
*/
AS
BEGIN
	DECLARE @CollectionID INT
	DECLARE @TempAdminCollectionID TABLE (CollectionID int primary key)
	DECLARE @TempCollectionID TABLE (CollectionID int primary key)
	IF (SELECT COUNT(*) FROM CollectionUser WHERE (LoginName = USER_NAME())) > 0
	BEGIN
		IF (SELECT COUNT(*) FROM CollectionManager WHERE (LoginName = USER_NAME())) > 0
		BEGIN
			INSERT @TempCollectionID (CollectionID) 
				SELECT DISTINCT CollectionID FROM ManagerCollectionList()
		END
		INSERT @TempAdminCollectionID (CollectionID) 
		SELECT DISTINCT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME()) AND CollectionID NOT IN (SELECT CollectionID FROM @TempAdminCollectionID)

		INSERT @TempCollectionID (CollectionID) 
		SELECT CollectionID FROM CollectionUser WHERE (LoginName = USER_NAME())  AND CollectionID NOT IN (SELECT CollectionID FROM @TempCollectionID)

		DECLARE HierarchyCursor  CURSOR for
		select CollectionID from @TempAdminCollectionID
		open HierarchyCursor
		FETCH next from HierarchyCursor into @CollectionID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			insert into @TempCollectionID select CollectionID 
			from dbo.CollectionChildNodes (@CollectionID) where CollectionID not in (select CollectionID from @TempCollectionID)
			FETCH NEXT FROM HierarchyCursor into @CollectionID
		END
		CLOSE HierarchyCursor
		DEALLOCATE HierarchyCursor
	END
	IF (select COUNT(*) from @TempCollectionID) = 0
	BEGIN
		INSERT @TempCollectionID (CollectionID) 
			SELECT CollectionID FROM Collection
	END
	INSERT @CollectionList
	SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder FROM dbo.Collection
	WHERE CollectionID in (SELECT CollectionID FROM @TempCollectionID)
	RETURN
END
GO

--#####################################################################################################################
--######  CollectionEventParameterValue - adding Notes  ###############################################################
--#####################################################################################################################
if (select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollectionEventParameterValue' and C.COLUMN_NAME = 'Notes') = 0
begin
	ALTER TABLE CollectionEventParameterValue ADD Notes nvarchar(MAX) NULL
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notes concerning the value of the parameter ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollectionEventParameterValue', @level2type=N'COLUMN',@level2name=N'Notes'

	ALTER TABLE CollectionEventParameterValue_log ADD Notes nvarchar(MAX) NULL
end
GO

--Trigger for CollectionEventParameterValue
ALTER TRIGGER [dbo].[trgDelCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR DELETE AS 
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, MethodMarker, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'D'
FROM DELETED

GO


ALTER TRIGGER [dbo].[trgUpdCollectionEventParameterValue] ON [dbo].[CollectionEventParameterValue] 
FOR UPDATE AS
INSERT INTO CollectionEventParameterValue_Log (CollectionEventID, MethodID, ParameterID, Value, MethodMarker, Notes, LogInsertedWhen, LogInsertedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID,  LogState) 
SELECT deleted.CollectionEventID, deleted.MethodID, deleted.ParameterID, deleted.Value, deleted.MethodMarker, deleted.Notes, deleted.LogInsertedWhen, deleted.LogInsertedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, deleted.RowGUID,  'U'
FROM DELETED
Update CollectionEventParameterValue
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM CollectionEventParameterValue, deleted 
where 1 = 1 
AND CollectionEventParameterValue.CollectionEventID = deleted.CollectionEventID
AND CollectionEventParameterValue.MethodID = deleted.MethodID
AND CollectionEventParameterValue.ParameterID = deleted.ParameterID
AND CollectionEventParameterValue.MethodMarker = deleted.MethodMarker

GO



--#####################################################################################################################
--######  trgUpdIdentification - writing IdentificationDate  ##########################################################
--#####################################################################################################################


ALTER TRIGGER [dbo].[trgUpdIdentification] ON [dbo].[Identification] 
FOR UPDATE AS

/*  Created by DiversityWorkbench Administration.  */ 
/*  Administration  1.0.0.0 */ 
/*  Date: 01.09.2006  */ 

/* setting the version in the main table */ 
declare @i int 
set @i = (select count(*) from deleted) 
if @i = 1 
begin 
DECLARE @ID int
SET  @ID = (SELECT CollectionSpecimenID FROM deleted)
EXECUTE procSetVersionCollectionSpecimen @ID
end 

DECLARE @Version int
SET @Version = (SELECT Version FROM CollectionSpecimen WHERE CollectionSpecimenID = @ID)

/* updating the LastIdentificationCache in IdentificationUnit */
update IdentificationUnit
set LastIdentificationCache = 
case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end 
from IdentificationUnit, Identification a, inserted
where IdentificationUnit.CollectionSpecimenID = inserted.CollectionSpecimenID
and IdentificationUnit.IdentificationUnitID = inserted.IdentificationUnitID
and a.CollectionSpecimenID = inserted.CollectionSpecimenID
and a.IdentificationUnitID = inserted.IdentificationUnitID
and a.IdentificationSequence = 
(select max(b.IdentificationSequence) 
from Identification b
where b.CollectionSpecimenID = a.CollectionSpecimenID
and b.IdentificationUnitID = a.IdentificationUnitID
group by b.IdentificationUnitID, b.CollectionSpecimenID)
and (LastIdentificationCache is null or LastIdentificationCache <> case 
when a.TaxonomicName is null or  a.TaxonomicName = '' 
then 
	case 
	when a.VernacularTerm is null or  a.VernacularTerm = '' 
	then IdentificationUnit.TaxonomicGroup + ' [ ' + cast(a.IdentificationUnitID as nvarchar) + ' ]'
	else a.VernacularTerm
	end
else a.TaxonomicName
end )

/* updating the logging columns */
Update Identification
set LogUpdatedWhen = getdate(), LogUpdatedBy = SYSTEM_USER
FROM Identification, deleted 
where 1 = 1 
AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
AND Identification.IdentificationSequence = deleted.IdentificationSequence
AND Identification.IdentificationUnitID = deleted.IdentificationUnitID

/* updating the IdentificationDate */
declare @Date datetime
set @Date = NULL
begin try
	set @Date = (select convert(datetime, cast(Identification.[IdentificationYear] as varchar) + '-' + cast(Identification.[IdentificationMonth] as varchar) + '-' + cast(Identification.[IdentificationDay] as varchar), 120) FROM Identification, deleted 
	where 1 = 1 
	AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
	AND Identification.IdentificationSequence = deleted.IdentificationSequence
	AND Identification.IdentificationUnitID = deleted.IdentificationUnitID)
end try
begin catch
	set @Date = NULL
end catch

Update Identification
set IdentificationDate = @Date
FROM Identification, deleted 
where 1 = 1 
AND Identification.CollectionSpecimenID = deleted.CollectionSpecimenID
AND Identification.IdentificationSequence = deleted.IdentificationSequence
AND Identification.IdentificationUnitID = deleted.IdentificationUnitID


/* saving the original dataset in the logging table */ 
if (not @Version is null) 
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion,  LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TermUri, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy,  @Version,  'U'
FROM DELETED
end
else
begin
INSERT INTO Identification_Log (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, IdentificationDate, IdentificationDay, IdentificationMonth, IdentificationYear, IdentificationDateSupplement, IdentificationDateCategory, VernacularTerm, TermUri, TaxonomicName, NameURI, IdentificationCategory, IdentificationQualifier, TypeStatus, TypeNotes, ReferenceTitle, ReferenceURI, ReferenceDetails, Notes, ResponsibleName, ResponsibleAgentURI, RowGUID, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy,  LogVersion, LogState) 
SELECT deleted.CollectionSpecimenID, deleted.IdentificationUnitID, deleted.IdentificationSequence, deleted.IdentificationDate, deleted.IdentificationDay, deleted.IdentificationMonth, deleted.IdentificationYear, deleted.IdentificationDateSupplement, deleted.IdentificationDateCategory, deleted.VernacularTerm, deleted.TermUri, deleted.TaxonomicName, deleted.NameURI, deleted.IdentificationCategory, deleted.IdentificationQualifier, deleted.TypeStatus, deleted.TypeNotes, deleted.ReferenceTitle, deleted.ReferenceURI, deleted.ReferenceDetails, deleted.Notes, deleted.ResponsibleName, deleted.ResponsibleAgentURI, deleted.RowGUID, deleted.LogCreatedWhen, deleted.LogCreatedBy, deleted.LogUpdatedWhen, deleted.LogUpdatedBy, CollectionSpecimen.Version, 'U' 
FROM DELETED, CollectionSpecimen
WHERE deleted.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
end

GO

--#####################################################################################################################
--######  Description of database roles  ##############################################################################
--#####################################################################################################################
/*
EXEC sys.sp_addextendedproperty   
@name = N'MS_Description',   
@value = N'Database Role with reading accesss to transaction information.',  
@level0type = N'USER',  
@level0name = 'TransactionUser'; 

GO
*/
declare @SQL nvarchar(max)
set @SQL = (select 'EXEC sys.sp_addextendedproperty   
@name = N''MS_Description'',   
@value = N''Database Role with reading accesss to transaction information.'',  
@level0type = N''USER'',  
@level0name = ''TransactionUser'';')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = (select 'EXEC sys.sp_updateextendedproperty   
@name = N''MS_Description'',   
@value = N''Database Role with reading accesss to transaction information.'',  
@level0type = N''USER'',  
@level0name = ''TransactionUser'';')
exec sp_executesql @SQL
end catch

GO

--#####################################################################################################################
--######   change links to DiversityExsiccatae to new location    #####################################################
--#####################################################################################################################

UPDATE s set [ExsiccataURI] = replace([ExsiccataURI], 'http://id.snsb.info/Exsiccatae/', 'http://tnt.diversityworkbench.de/Exsiccatae/')
--SELECT [ExsiccataURI], replace([ExsiccataURI], 'http://id.snsb.info/Exsiccatae/', 'http://tnt.diversityworkbench.de/Exsiccatae/')
  FROM [dbo].[CollectionSpecimen]
s where s.ExsiccataURI like 'http://id.snsb.info/Exsiccatae/%'
GO




--#####################################################################################################################
--######   DeleteXmlAttribute    ######################################################################################
--#####################################################################################################################

--if (select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = 'PROCEDURE' and R.ROUTINE_NAME = 'DeleteXmlAttribute') = 0
--begin

	CREATE Procedure [dbo].[DeleteXmlAttribute] 
	@Table nvarchar(128), 
	@Column nvarchar(128), 
	@Path nvarchar(4000),  
	@Attribute nvarchar(128),
	@WhereClause nvarchar(4000)
	AS  
	/*
	Deleting a attribute of an XML node

	Test:
	EXEC dbo.DeleteXmlAttribute 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/TaxonomicGroup/fungus', 'Webservice', 'LoginName = USER_NAME()'
	SELECT [Settings] FROM [dbo].[UserProxy] WHERE LoginName = USER_NAME()

	*/
	BEGIN 
	declare @SQL nvarchar(max)
	-- try to insert the attribute
	set @SQL = (select 'SET ANSI_NULLS ON;
	DECLARE @Setting xml;
	SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
	set @Setting.modify(''delete (' + @Path + '/@' + @Attribute + ')[1]'');;
	update T set T.' + @Column + ' = @Setting 
	FROM ' + @Table + ' AS T  
	WHERE ' + @WhereClause)

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	end catch
	END
--end

GO


GRANT EXEC ON [dbo].[DeleteXmlAttribute] TO [User]
GO



--#####################################################################################################################
--######   DeleteXmlNode    ###########################################################################################
--#####################################################################################################################

--if (select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = 'PROCEDURE' and R.ROUTINE_NAME = 'DeleteXmlNode') = 0
--begin

	CREATE Procedure [dbo].[DeleteXmlNode] 
	@Table nvarchar(128), 
	@Column nvarchar(128), 
	@Path nvarchar(4000),  
	@WhereClause nvarchar(4000)
	AS  
	/*
	Deleting a XML node

	Test:
	EXEC dbo.DeleteXmlNode 'UserProxy', 'Settings', '/Settings/ModuleSource/Identification/TaxonomicGroup/fungus', 'LoginName = USER_NAME()'
	SELECT [Settings] FROM [dbo].[UserProxy] WHERE LoginName = USER_NAME()

	*/
	BEGIN 
	declare @SQL nvarchar(max)
	-- try to insert the attribute
	set @SQL = (select 'SET ANSI_NULLS ON;
	DECLARE @Setting xml;
	SET @Setting = (SELECT T.'+ @Column + ' FROM ' + @Table + ' AS T WHERE ' + @WhereClause + ');
	set @Setting.modify(''delete (' + @Path + ')[1]'');;
	update T set T.' + @Column + ' = @Setting 
	FROM ' + @Table + ' AS T  
	WHERE ' + @WhereClause)

	begin try
	exec sp_executesql @SQL
	end try
	begin catch
	end catch
	END
--end

GO


GRANT EXEC ON [dbo].[DeleteXmlNode] TO [User]
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
RETURN '02.06.11'
END

GO

