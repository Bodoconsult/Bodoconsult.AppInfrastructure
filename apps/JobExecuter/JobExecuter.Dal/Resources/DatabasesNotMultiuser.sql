select name as [Database], database_id as Id, 
	state_desc as [State],
	user_access_desc as [User access],
	compatibility_level as [Compatibility] ,
	collation_name as [Collation],
	recovery_model_desc [Recovery model]
from sys.databases 
where user_access<>0