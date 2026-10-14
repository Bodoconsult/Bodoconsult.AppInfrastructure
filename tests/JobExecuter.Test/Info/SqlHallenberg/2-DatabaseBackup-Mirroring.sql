
-- Mirroring of backups - This will produce two identical backups
EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = '\\OLA\Backup1',
@MirrorDirectory = '\\OLA\Backup2',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@LogToTable = 'Y',
@CleanupTime = 24,
@MirrorCleanupTime = 48