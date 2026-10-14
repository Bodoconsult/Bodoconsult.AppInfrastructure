select name as UserName, type_desc as [login type] from sys.database_principals 
where type IN ('U', 'S', 'E', 'C', 'G', 'K', 'X') order by type_desc