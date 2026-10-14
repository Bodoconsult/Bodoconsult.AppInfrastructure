
-- Three fragmentation groups; low, medium and high
-- Fragmentation thresholds by default 5% and 30%
-- Fragmentation is checked using sys.dm_db_index_physical_stats
-- Lets you define your preferred index maintenance for each group
-- Fallback in case your preferred index maintenance is not supported

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@PageCountLevel = 0,
@LogToTable = 'Y'

-- Logging to text output and table
SELECT *
FROM dbo.CommandLog
ORDER BY ID ASC