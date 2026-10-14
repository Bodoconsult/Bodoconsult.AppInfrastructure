
-- Perform a check of a catalog
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKCATALOG',
@LogToTable = 'Y'

-- Perform a check of a filegroup
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKFILEGROUP',
@FileGroups = 'AdventureWorks.Archive',
@LogToTable = 'Y'

-- Perform a check of all filegroups, except one
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKFILEGROUP',
@FileGroups = 'ALL_FILEGROUPS, -AdventureWorks.Archive',
@LogToTable = 'Y'