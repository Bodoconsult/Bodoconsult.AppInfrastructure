select What, [Value]
from (
select 'MachineName' as What, serverproperty('MachineName') as [Value], 1 as SortId
UNION
select  'ServerInstanceName' as What, serverproperty('ServerName') as [Value], 2

UNION
select  'EditionInstalled', replace(cast(serverproperty('Edition')as varchar),'Edition',''), 3
UNION
select  'ProductBuildLevel',serverproperty('productVersion'), 4

UNION
select  'SPLevel',serverproperty('productLevel') , 5) as X
order by sortID