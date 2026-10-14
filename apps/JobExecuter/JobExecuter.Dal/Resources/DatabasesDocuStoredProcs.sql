select 'EXEC '+routine_schema+'.'+routine_name as Name 
from information_schema.routines 
where routine_type = 'PROCEDURE'
	AND routine_name LIKE '%spDbDocu_%'