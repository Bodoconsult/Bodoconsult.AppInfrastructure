
-- Mirroring and striping of backups

EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = '\\OLA\Backup1, \\OLA\Backup2',
@MirrorDirectory = '\\OLA\Backup3, \\OLA\Backup4',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@LogToTable = 'Y',
@CleanupTime = 24,
@MirrorCleanupTime = 48,
@NumberOfFiles = 4