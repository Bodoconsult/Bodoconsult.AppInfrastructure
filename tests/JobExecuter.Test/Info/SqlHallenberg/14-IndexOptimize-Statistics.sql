
-- Index maintenance and update of statistics
-- If an index is rebuilt, updating of statistics for that index is skipped

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@PageCountLevel = 0,
@UpdateStatistics = 'ALL',
@LogToTable = 'Y'