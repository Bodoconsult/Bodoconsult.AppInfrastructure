EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'USER_DATABASES',
@CheckCommands = 'CHECKDB',
@PhysicalOnly = 'N'

GO

-- Perform a check of a catalog
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'USER_DATABASES',
@CheckCommands = 'CHECKCATALOG'

GO

-- Perform a check of the allocation structures
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'USER_DATABASES',
@CheckCommands = 'CHECKALLOC'

GO

-- Perform a check of a table
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'USER_DATABASES',
@CheckCommands = 'CHECKTABLE',
@Objects = 'ALL_OBJECTS'
