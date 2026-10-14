
-- All databases
EXECUTE dbo.DatabaseBackup
@Databases = 'ALL_DATABASES',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- All system databases
EXECUTE dbo.DatabaseBackup
@Databases = 'SYSTEM_DATABASES',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- All user databases
EXECUTE dbo.DatabaseBackup
@Databases = 'USER_DATABASES',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- The database AdventureWorks
EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- All user databases, except the database AdventureWorks
EXECUTE dbo.DatabaseBackup
@Databases = 'USER_DATABASES, -AdventureWorks',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- All database that starts with AdventureWorks
EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks%',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'

-- All user databases, except those that have Test in the name
EXECUTE dbo.DatabaseBackup
@Databases = 'USER_DATABASES, -%Test%',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@LogToTable = 'Y'