
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.63'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Sorting for AccessionNumber of Part   ######################################################################################
--#####################################################################################################################


ALTER VIEW [dbo].[CollectionSpecimenPart_Core]
AS
SELECT        P.CollectionSpecimenID, P.SpecimenPartID, P.DerivedFromSpecimenPartID, P.PreparationMethod, P.PreparationDate, P.AccessionNumber, P.PartSublabel, 
                         P.CollectionID, P.MaterialCategory, P.StorageLocation, P.StorageContainer, P.Stock, P.StockUnit, P.Notes, P.AccessionNumber AS PartAccessionNumber
FROM            dbo.CollectionSpecimenID_UserAvailable AS U INNER JOIN
                         dbo.CollectionSpecimenPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID

GO




--#####################################################################################################################
--######   setting the Client Version   ######################################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '03.00.07.09' 
END

GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.64'
END

GO


