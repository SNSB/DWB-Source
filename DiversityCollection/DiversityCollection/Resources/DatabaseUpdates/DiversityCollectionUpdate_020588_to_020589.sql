declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.88'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO




--#####################################################################################################################
--######   ExternalIdentifier        ##################################################################################
--######   Views for linked table Transaction     #####################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[IdentifierTransaction]
AS
SELECT        ID, ReferencedID AS TransactionID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'Transaction')

GO

GRANT SELECT ON [IdentifierTransaction] TO [USER]
GO


--#####################################################################################################################
--######   Views for linked table CollectionEvent     #################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[IdentifierEvent]
AS
SELECT        ID, ReferencedID AS CollectionEventID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'CollectionEvent')

GO

GRANT SELECT ON [IdentifierEvent] TO [USER]
GO


--#####################################################################################################################
--######   Views for linked table CollectionSpecimen     ##############################################################
--#####################################################################################################################

CREATE VIEW [dbo].[IdentifierSpecimen]
AS
SELECT        ID, ReferencedID AS CollectionSpecimenID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'CollectionSpecimen')

GO

GRANT SELECT ON [IdentifierSpecimen] TO [USER]
GO


--#####################################################################################################################
--######   Views for linked table CollectionSpecimenPart     ##########################################################
--#####################################################################################################################

CREATE VIEW [dbo].[IdentifierPart]
AS
SELECT        ID, ReferencedID AS SpecimenPartID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'CollectionSpecimenPart')

GO

GRANT SELECT ON [IdentifierPart] TO [USER]
GO


--#####################################################################################################################
--######   Views for linked table IdentificationUnit     ##############################################################
--#####################################################################################################################

CREATE VIEW [dbo].[IdentifierUnit]
AS
SELECT        ID, ReferencedID AS IdentificationUnitID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
FROM            ExternalIdentifier
WHERE        (ReferencedTable = N'IdentificationUnit')

GO

GRANT SELECT ON [IdentifierUnit] TO [USER]
GO



--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.89'
END

GO

