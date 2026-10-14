/*
http://www.practicalsqldba.com/2012/08/sql-server-list-logins-database-and.html
*/

CREATE TABLE #DatabaseRoleMemberShip 
   (
        Username VARCHAR(100),
        Rolename VARCHAR(100),
        Databasename VARCHAR(100),
		is_fixed_role bit
         
    )DECLARE @Cmd AS VARCHAR(MAX)DECLARE @PivotColumnHeaders VARCHAR(4000)           SET @Cmd = 'USE [?] ;insert into #DatabaseRoleMemberShip 
select u.name,r.name,''?'', R.is_fixed_role from sys.database_role_members RM inner join 
sys.database_principals U on U.principal_id=RM.member_principal_id
inner join sys.database_principals R on R.principal_id=RM.role_principal_id
where u.type<>''R'''EXEC sp_MSforeachdb @command1=@cmd

SELECT  @PivotColumnHeaders =                         
  COALESCE(@PivotColumnHeaders + ',[' + CAST(rolename AS VARCHAR(MAX)) + ']','[' + CAST(rolename AS VARCHAR(MAX))+ ']'                     
  )                     
  FROM (SELECT DISTINCT rolename FROM #DatabaseRoleMemberShip)a ORDER BY rolename  ASC


--SET @Cmd = 'select 
--databasename,username,'+@PivotColumnHeaders+'
--from 
--(
-- select   * from #DatabaseRoleMemberShip) as p
 
--pivot 
--(
-- count(rolename  )
--for rolename in ('+@PivotColumnHeaders+') )as pvt
--'


--EXECUTE(@Cmd )        



select username as [User name], Databasename as [Database], rolename as [Role]
from  #DatabaseRoleMemberShip 
order by username, Databasename, rolename

DROP TABLE #DatabaseRoleMemberShip 