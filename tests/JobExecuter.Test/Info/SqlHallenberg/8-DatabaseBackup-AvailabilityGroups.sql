
-- Backup of AlwaysOn Availability Groups - Full copy_only
EXECUTE dbo.DatabaseBackup
@Databases = 'ola-ag-AlwaysOnTestDatabase',
@Directory = '\\OLA-FSW\Backup',
@BackupType = 'FULL',
@CopyOnly = 'Y',
@Verify = 'Y',
@LogToTable = 'Y'

-- Backup of AlwaysOn Availability Groups - Full copy_only,
-- overriding backup preference
EXECUTE dbo.DatabaseBackup
@Databases = 'ola-ag-AlwaysOnTestDatabase',
@Directory = '\\OLA-FSW\Backup',
@BackupType = 'FULL',
@CopyOnly = 'Y',
@Verify = 'Y',
@OverrideBackupPreference = 'Y',
@LogToTable = 'Y'

-- Backup of AlwaysOn Availability Groups - Full
EXECUTE dbo.DatabaseBackup
@Databases = 'ola-ag-AlwaysOnTestDatabase',
@Directory = '\\OLA-FSW\Backup',
@BackupType = 'FULL',
@CopyOnly = 'N',
@Verify = 'Y',
@LogToTable = 'Y'