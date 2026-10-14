
-- Striping of backups to multiple files and multiple network shares
EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = '\\OLA\Backup1, \\OLA\Backup2',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@NumberOfFiles = 4,
@CleanupTime = 24,
@LogToTable = 'Y'