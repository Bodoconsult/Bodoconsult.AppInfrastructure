select name as Role, type_desc as [Role type] from sys.database_principals 
where is_fixed_role=0 and type IN ('A', 'R') 
order by type_desc;