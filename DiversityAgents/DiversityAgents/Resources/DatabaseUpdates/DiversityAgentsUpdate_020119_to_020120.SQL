
declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.01.19'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######  ProjectUser: new column [ReadOnly] ##########################################################################
--#####################################################################################################################

ALTER TABLE ProjectUser ADD [ReadOnly]  [bit] NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the user has only read access to data of this project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectUser', @level2type=N'COLUMN',@level2name=N'ReadOnly'
GO

ALTER TABLE [dbo].[ProjectUser] ADD  CONSTRAINT [DF_ProjectUser_ReadOnly]  DEFAULT ((0)) FOR [ReadOnly]
GO


--#####################################################################################################################
--######   PublicIdentifier  ##########################################################################################
--#####################################################################################################################

CREATE VIEW [dbo].[PublicIdentifier]
AS
SELECT        I.AgentID, I.Identifier, I.IdentifierURI, I.Type, I.Notes, I.LogUpdatedWhen
FROM            dbo.AgentIdentifier AS I INNER JOIN
                         dbo.Agent AS A ON A.AgentID = I.AgentID
WHERE        (A.DataWithholdingReason = N'') OR (A.DataWithholdingReason IS NULL) 
GO

GRANT SELECT ON [dbo].[PublicIdentifier] TO [DiversityWorkbenchUser]
GO

--#####################################################################################################################
--######  AgentDescriptor - FK for DescriptorType if missing  #########################################################
--#####################################################################################################################

declare @i int
set @i = (select count(*)
from 
sys.foreign_key_columns as fk
inner join 
sys.tables as t on fk.parent_object_id = t.object_id
inner join 
sys.columns as c on fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id
where 
fk.referenced_object_id = (select object_id 
from sys.tables 
where name = 'AgentDescriptorType_Enum')
and t.name = 'AgentDescriptor'
and c.name = 'DescriptorType')

if (@i = 0)
begin
	ALTER TABLE [dbo].[AgentDescriptor]  WITH CHECK ADD  CONSTRAINT [FK_AgentDescriptor_AgentDescriptorType_Enum] FOREIGN KEY([DescriptorType])
	REFERENCES [dbo].[AgentDescriptorType_Enum] ([Code])


	ALTER TABLE [dbo].[AgentDescriptor] CHECK CONSTRAINT [FK_AgentDescriptor_AgentDescriptorType_Enum]
end
GO


--#####################################################################################################################
--######   setting the Client Version   ###############################################################################
--#####################################################################################################################

ALTER FUNCTION [dbo].[VersionClient] () 
RETURNS nvarchar(11) AS 
BEGIN 
RETURN '04.01.00' 
END

GO


--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################



ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.01.20'
END

GO

select [dbo].[Version] ()  

