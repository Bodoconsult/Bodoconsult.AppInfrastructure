
-- Perform a check of a catalog
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKCATALOG',
@LogToTable = 'Y'

-- Perform a check of the allocation structures
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKALLOC',
@LogToTable = 'Y'

-- Perform a check of a table
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKTABLE',
@Objects = 'AdventureWorks.Production.Product',
@LogToTable = 'Y'

-- Perform a check of all tables, except one
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKTABLE',
@Objects = 'ALL_OBJECTS, -AdventureWorks.Production.Product',
@LogToTable = 'Y'