declare @data as Table (ID int, [name] varchar(max), [IntValue] varchar(max), [Value] varchar(max))
insert into @Data([ID], [Name], [IntValue], [Value])
EXEC xp_msver

insert into @Data([ID], [Name], [IntValue], [Value])
SELECT 0, 'Authentication mode', null, CASE SERVERPROPERTY('IsIntegratedSecurityOnly')   
WHEN 1 THEN 'Windows Authentication'   
WHEN 0 THEN 'Windows and SQL Server Authentication'   
END as [Authentication Mode]

select [Name], [Value]
from @Data
where [value] is not null