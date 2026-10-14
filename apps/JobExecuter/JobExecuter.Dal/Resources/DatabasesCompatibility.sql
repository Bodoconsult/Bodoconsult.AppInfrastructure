DECLARE @ver nvarchar(128)
SET @ver = CAST(serverproperty('ProductVersion') AS nvarchar)

declare @I int

set @I = CHARINDEX('.', @ver)

SET @ver = replace(SUBSTRING(@ver, 1, CHARINDEX('.', @ver, @I+1) - 1), '.', '')

set @I = cast(@Ver as int)

select name as [Database], database_id as Id, 
	state_desc as [State],
	user_access_desc as [User access],
	compatibility_level as [Compatibility] ,
	collation_name as [Collation],
	recovery_model_desc [Recovery model]
from sys.databases 
where compatibility_level<@I
