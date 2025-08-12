declare @CurrentVersion varchar(10)
declare @ScriptVersion varchar(10)
set @CurrentVersion = (SELECT [dbo].[Version]())
set @CurrentVersion = (SELECT REPLACE(@CurrentVersion, '/', '.'))
set @ScriptVersion = '02.05.56'
IF (@CurrentVersion <> @ScriptVersion)
BEGIN
declare @Message nvarchar (199)
set @Message = 'WRONG VERION. Script is scheduled as update for version ' + @ScriptVersion + '. Current version = ' + @CurrentVersion
RAISERROR (@Message, 18, 1) 
END
GO

--#####################################################################################################################
--######   Image description: Transfer of non EXIF content into properties   #####################################################################################
--#####################################################################################################################

--delete from CollectionSpecimenImageProperty


-- temporary table for image descriptions
declare @TempTab table (ID int identity, CollectionSpecimenID int, URI varchar(255), XmlDescription nvarchar(max))

-- fill temporary table with to do content
insert into @TempTab (CollectionSpecimenID, URI, XmlDescription)
Select CollectionSpecimenID, URI , cast(I.Description as nvarchar(max))
from CollectionSpecimenImage I 
where cast(I.Description as nvarchar(max)) <> ''
AND cast(I.Description as nvarchar(max)) not like '<rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"><rdf:Description xmlns:et="http://ns.exiftool.ca%'

--select * from @TempTab

-- temporary table for the properties
declare @PropertyTab table(CollectionSpecimenID int, URI varchar(255), Property varchar(255), Description nvarchar(max))

-- temporary table for level in hierarchy of xml
declare @HierTab table (Node nvarchar(200), Display nvarchar(400), ID int identity);

-- variables
declare @ID int;
declare @CollectionSpecimenID int
declare @URI varchar(255)
declare @Property  varchar(255)
declare @XmlNode varchar(255)
declare @Description as nvarchar(max)
declare @Display varchar(499)
declare @XML as nvarchar(max)

-- walk trough the to do content in temp tab
set @ID = (select min(ID) from @TempTab)
while (select count(*) from @TempTab) > 0
begin
	-- reset internal tables
	delete H from @HierTab H
	delete from @PropertyTab
	-- set PK and XML
	set @CollectionSpecimenID = (select CollectionSpecimenID from @TempTab t where t.ID = @ID)
	set @URI = (select URI from @TempTab t where t.ID = @ID)
	set @XML = (select XmlDescription from @TempTab t where t.ID = @ID)
	-- consume xml
	while @XML <> ''
	begin
		set @XmlNode = (select SUBSTRING(@XML, 1, charindex('>', @XML))) 
		if @XmlNode like '%</%>' and @XmlNode not like '<%>'
		begin
			set @Description = SUBSTRING(@XmlNode, 1, charindex('<', @XML) - 1)
			set @Property = SUBSTRING(@XmlNode, charindex('<', @XML) + 2, 255)
			set @Property = SUBSTRING(@Property, 1, charindex('>', @Property) - 1)

			set @Display = (select h.Display from @HierTab h where h.ID = (select max(ID) from @HierTab))
			--select 'x'
			--select @CollectionSpecimenID, @URI, @Display, @Description
			insert into @PropertyTab(CollectionSpecimenID, URI, Property, Description)
			values (@CollectionSpecimenID, @URI, @Display, @Description)

			delete h from @HierTab h where H.ID = (select max(ID) from @HierTab) and H.Node = @Property
			--select * from @HierTab
		end
		else
		begin
			set @Property = SUBSTRING(@XmlNode, 2, charindex('>', @XML)-2)
			if @Property like '/%' set @Property = SUBSTRING(@Property, 2, 255)
			set @Description = ''
			if @XmlNode like '</%>'
			begin
				delete h from @HierTab h where H.ID = (select max(ID) from @HierTab) and H.Node = @Property
			end
		end
		if (@XmlNode like '<%>' and not @XmlNode like '</%>')
		begin
			set @Display = (select h.Display from @HierTab h where h.ID = (select max(ID) from @HierTab))
			if @Display is null set @Display = '' else set @Display = @Display + '.'
			set @Display = @Display + @Property
			insert into @HierTab (Node, Display) values (@Property, @Display)
		end
		--select @XmlNode, @Description, @Property
		set @XML = (select SUBSTRING(@XML, charindex('>', @XML) + 1, 8000))
		--select @XML
	end

	insert into [dbo].[CollectionSpecimenImageProperty] ([CollectionSpecimenID], [URI], [Property], [Description])
	select CollectionSpecimenID, URI, Property, Description
	from @PropertyTab PT
	WHERE NOT EXISTS 
	(select * from CollectionSpecimenImageProperty IP 
	WHERE IP.CollectionSpecimenID = PT.CollectionSpecimenID
	AND IP.URI = PT.URI
	AND IP.Property = PT.Property)


	delete t from @TempTab t
	where t.ID = @ID
	set @ID = (select min(ID) from @TempTab)
end

--select * from CollectionSpecimenImageProperty

GO
--#####################################################################################################################
--######   setting the Version   ######################################################################################
--#####################################################################################################################


ALTER FUNCTION [dbo].[Version] ()  
RETURNS nvarchar(8)
AS
BEGIN
RETURN '02.05.57'
END

GO

