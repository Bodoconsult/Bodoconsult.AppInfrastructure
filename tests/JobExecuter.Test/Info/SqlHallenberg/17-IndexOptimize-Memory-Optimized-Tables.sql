
-- Memory-optimized-tables
-- No auto-update-statistics for memory-optimized-tables
-- Statistics has to be updated with NORECOMPUTE and, RESAMPLE or FULLSCAN

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@UpdateStatistics = 'ALL',
@Indexes = 'AdventureWorks.dbo.Hekaton1',
@LogToTable = 'Y'

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@UpdateStatistics = 'ALL',
@Indexes = 'AdventureWorks.dbo.Hekaton1',
@StatisticsSample = 100,
@LogToTable = 'Y'