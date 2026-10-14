
-- Perform a full backup with compression and checksums
-- Verify the backup (using RESTORE VERIFYONLY)
-- If the backup and verify was successful,
-- delete backups that are older than 24 hours (using xp_delete_file)

EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@CleanupTime = 24,
@LogToTable = 'Y'

-- Logging to text output and table
SELECT *
FROM dbo.CommandLog
ORDER BY ID ASC