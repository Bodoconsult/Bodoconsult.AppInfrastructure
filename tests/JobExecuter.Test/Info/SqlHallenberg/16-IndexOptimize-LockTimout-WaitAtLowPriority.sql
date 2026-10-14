
-- Rebuild indexes with no lock timeout and with normal locks
EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationMedium = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@PageCountLevel = 0,
@Indexes = 'AdventureWorks.Production.Product.PK_Product_ProductID',
@LogToTable = 'Y'

-- Rebuild indexes with a 5 second lock timeout
EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationMedium = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@Indexes = 'AdventureWorks.Production.Product.PK_Product_ProductID',
@PageCountLevel = 0,
@LockTimeout = 5,
@LogToTable = 'Y'

-- Rebuild indexes with low priority locks, and abort after 1 minute (new feature in SQL Server 2014) 
EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationMedium = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@PageCountLevel = 0,
@Indexes = 'AdventureWorks.Production.Product.PK_Product_ProductID',
@WaitAtLowPriorityMaxDuration = 1,
@WaitAtLowPriorityAbortAfterWait = 'SELF',
@LogToTable = 'Y'

-- Rebuild indexes with low priority locks. After one minute shift to normal locks, and after another minute abort.
EXECUTE dbo.IndexOptimize
@Databases = 'AdventureWorks',
@FragmentationLow = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationMedium = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@PageCountLevel = 0,
@Indexes = 'AdventureWorks.Production.Product.PK_Product_ProductID',
@WaitAtLowPriorityMaxDuration = 1,
@WaitAtLowPriorityAbortAfterWait = 'NONE',
@LockTimeout = 60,
@LogToTable = 'Y'