declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.06.09'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO



--#####################################################################################################################
--######  NextFreeAccNumber: New design including postfix for number   ################################################
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
select dbo.[NextFreeAccNumber] ('SMNK-ARA 08643', 1, 1)
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
		while (substring(@AccessionNumber, @EndPos, 1) NOT LIKE  '[0-9]')
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
		Delete T from @T T inner join CollectionSpecimen S on rtrim(ltrim(S.AccessionNumber)) =rtrim(ltrim( T.AccessionNumberGenerated))
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
--######  StableIdentifier: Bugfix using StableIdentifierBase if present    ###########################################
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
if (select count(*) from INFORMATION_SCHEMA.ROUTINES r where r.ROUTINE_TYPE = 'FUNCTION'
and r.ROUTINE_NAME = 'StableIdentifierBase') = 1
begin 
	set @StableIdentifierBase = (SELECT dbo.[StableIdentifierBase]())
end
else 
begin
	set @StableIdentifierBase = NULL
end
set @StableIdentifierBase = (SELECT case when [StableIdentifierBase] is null or [StableIdentifierBase] = ''
	then @StableIdentifierBase--NULL -- dbo.BaseURL() -- former version used BaseURL as a default
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
--######  Description of database roles  ##############################################################################
--#####################################################################################################################
begin try
EXEC sys.sp_addextendedproperty   
@name = N'MS_Description',   
@value = N'Database Role with reading accesss to transaction information.',  
@level0type = N'USER',  
@level0name = 'TransactionUser'; 
end try
begin catch
end catch
GO

--#####################################################################################################################
--######  CollectionEventParameterValue: adding missing defaults for logging columns   ################################
--#####################################################################################################################

alter table [dbo].[CollectionEventParameterValue] 
add constraint [Default_CollectionEventParameterValue_LogInsertedWhen]  DEFAULT (getdate())
for [LogInsertedWhen] ;


alter table [dbo].[CollectionEventParameterValue] 
add constraint [Default_CollectionEventParameterValue_LogUpdatedWhen]  DEFAULT (getdate())
for [LogUpdatedWhen] ;

alter table [dbo].[CollectionEventParameterValue] 
add constraint [Default_CollectionEventParameterValue_LogInsertedBy]  DEFAULT (suser_sname())
for  [LogInsertedBy] ;

alter table [dbo].[CollectionEventParameterValue] 
add constraint [Default_CollectionEventParameterValue_LogUpdatedBy]  DEFAULT (suser_sname())
for  [LogUpdatedBy] ;
GO

--#####################################################################################################################
--######  Cleaning table CollectionEventLocalisation from copies of active data   #####################################
--#####################################################################################################################


DELETE L
--select count(*)
--select *
from [dbo].[CollectionEventLocalisation_log] L, [dbo].[CollectionEventLocalisation] E
where L.CollectionEventID = E.CollectionEventID
and L.LocalisationSystemID = E.LocalisationSystemID
and L.Location1 = E.Location1
and (L.Location2 = E.Location2 or (L.Location2 is null and E.Location2 is null))
and (L.LocationAccuracy = E.LocationAccuracy or (L.LocationAccuracy is null and E.LocationAccuracy is null))
and (L.DirectionToLocation = E.DirectionToLocation or (L.DirectionToLocation is null and E.DirectionToLocation is null))
and (L.DeterminationDate = E.DeterminationDate or (L.DeterminationDate is null and E.DeterminationDate is null))
and (L.DirectionToLocation = E.DirectionToLocation or (L.DirectionToLocation is null and E.DirectionToLocation is null))
and (L.DistanceToLocation = E.DistanceToLocation or (L.DistanceToLocation is null and E.DistanceToLocation is null))
and (L.Geography.ToString() = E.Geography.ToString() or (L.Geography is null and E.Geography is null))
and (L.LocationNotes = E.LocationNotes or (L.LocationNotes is null and E.LocationNotes is null))
and (L.LogUpdatedBy = E.LogUpdatedBy or (L.LogUpdatedBy is null and E.LogUpdatedBy is null))
and (L.LogUpdatedWhen = E.LogUpdatedWhen or (L.LogUpdatedWhen is null and E.LogUpdatedWhen is null))
and (L.LogCreatedBy = E.LogCreatedBy or (L.LogCreatedBy is null and E.LogCreatedBy is null))
and (L.LogCreatedWhen = E.LogCreatedWhen or (L.LogCreatedWhen is null and E.LogCreatedWhen is null))

GO


--#####################################################################################################################
--######  Cleaning geometry in table CollectionSpecimenImageProperty  #################################################
--#####################################################################################################################

UPDATE P
   SET P.[ImageArea] = NULL
   --select P.[ImageArea].ToString(), * 
   FROM [dbo].[CollectionSpecimenImageProperty] AS P
 WHERE NOT P.[ImageArea] IS NULL

GO

--#####################################################################################################################
--######  Grants for CollectionEventRegulation  #######################################################################
--#####################################################################################################################

GRANT SELECT ON dbo.[CollectionEventRegulation] TO [User];
GO
GRANT INSERT ON dbo.[CollectionEventRegulation] TO [Editor];
GO
GRANT UPDATE ON dbo.[CollectionEventRegulation] TO [Editor];
GO
GRANT DELETE ON dbo.[CollectionEventRegulation] TO [Editor];
GO

GRANT SELECT ON dbo.[CollectionEventRegulation_log] TO [Editor];
GO
GRANT INSERT ON dbo.[CollectionEventRegulation_log] TO [Editor];
GO
GRANT DELETE ON dbo.[CollectionEventRegulation_log] TO [Administrator];
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
RETURN '02.06.10'
END

GO

