
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '01.00.13'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO


--#####################################################################################################################
--######   procTransferCoordinates     #############################################################################
--#####################################################################################################################


declare @SQL nvarchar(max)
set @SQL = (select 'ALTER  PROCEDURE [dbo].[procTransferCoordinates] 
AS
-- setting the CollectionEventID to ease subsequent update
UPDATE C SET C.CollectionEventID = E.[CollectionEventID] 
FROM [dbo].[CollectionSpecimenCache] C, CollectionSpecimenEventID E
WHERE C.CollectionSpecimenID = E.CollectionSpecimenID

DECLARE @LocalisationSystemID int

-- the sequence of the LocalisationSystems that should be scanned to insert the coordiates
DECLARE @TempLocalisationSystem TABLE (OrderSequence int primary key, LocalisationSystemID int)
INSERT INTO @TempLocalisationSystem (OrderSequence, LocalisationSystemID)
SELECT [Sequence]
      ,[LocalisationSystemID]
  FROM [dbo].[EnumLocalisationSystem]

-- inserting the coordinates
while ((select count (*) from @TempLocalisationSystem) > 0) 
begin
	set @LocalisationSystemID = (select min(LocalisationSystemID) from @TempLocalisationSystem where OrderSequence = (select min(OrderSequence) from @TempLocalisationSystem))
	
	UPDATE CC
	SET CC.Latitude = CAST(L.AverageLatitudeCache AS float),
	CC.Longitude = L.AverageLongitudeCache
	FROM CollectionSpecimenCache CC, 
	dbo.CollectionEventLocalisation L
	WHERE CC.CollectionEventID = L.CollectionEventID
	AND L.LocalisationSystemID = @LocalisationSystemID
	AND CC.Latitude IS NULL
	AND L.AverageLatitudeCache <> 0
	
	delete @TempLocalisationSystem where LocalisationSystemID = @LocalisationSystemID

end')

set @SQL = (@SQL + '

-- reducing the precision of the coordinates for those project where a reduction is demanded
DECLARE @CoordinatePrecision TABLE(ProjectID int primary key, CoordinatePrecision tinyint)

-- getting all projects where the accuracy of the coordinates should be reduced
insert into @CoordinatePrecision (ProjectID, CoordinatePrecision)
select ProjectID, CoordinatePrecision from [dbo].[ProjectPublished] PP
where not PP.CoordinatePrecision is null

declare @ProjectID int
set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)
-- reducing the precision
while (select count(*) from @CoordinatePrecision) > 0
begin

	set @ProjectID = (select min(ProjectID) from @CoordinatePrecision)

	UPDATE CC
	SET CC.Latitude = round(CC.Latitude, CA.CoordinatePrecision),
	CC.Longitude = round(CC.Longitude, CA.CoordinatePrecision)
	FROM CollectionSpecimenCache CC, CollectionProject P, @CoordinatePrecision CA
	where CC.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = CA.ProjectID
	and (CC.Latitude <> 0 or CC.Longitude <> 0)
	and CA.ProjectID = @ProjectID

	delete CA from @CoordinatePrecision CA
	where CA.ProjectID = @ProjectID

end')

begin try
exec sp_executesql @SQL
end try
begin catch
set @SQL = 'ALTER ' + SUBSTRING(@SQL, 8, 4000)
exec sp_executesql @SQL
end catch



GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '01.00.14'
END

GO


