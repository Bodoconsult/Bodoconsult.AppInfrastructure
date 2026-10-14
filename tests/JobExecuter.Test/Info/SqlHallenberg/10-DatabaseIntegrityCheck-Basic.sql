
-- Perform a check of a database
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKDB',
@LogToTable = 'Y'

-- Perform a check of a database with the option PHYSICAL_ONLY
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKDB',
@PhysicalOnly = 'Y',
@LogToTable = 'Y'

-- Perform a check of a database with the option NOINDEX
EXECUTE dbo.DatabaseIntegrityCheck
@Databases = 'AdventureWorks',
@CheckCommands = 'CHECKDB',
@NoIndex = 'Y',
@LogToTable = 'Y'

-- Logging to text output and table
SELECT *
FROM dbo.CommandLog
ORDER BY ID ASC