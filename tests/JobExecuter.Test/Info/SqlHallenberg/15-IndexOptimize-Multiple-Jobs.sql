
-- Split the index maintenance work into multiple jobs

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@PageCountLevel = 0,
@Indexes = 'AdventureWorks.Production.Product',
@LogToTable = 'Y'

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@PageCountLevel = 0,
@Indexes = 'AdventureWorks.Person.Address',
@LogToTable = 'Y'

EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@PageCountLevel = 0,
@Indexes = 'ALL_INDEXES, -AdventureWorks.Production.Product, -AdventureWorks.Person.Address',
@LogToTable = 'Y'