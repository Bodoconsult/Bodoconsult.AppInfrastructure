
-- Create new database
IF DB_ID('Database1') IS NOT NULL BEGIN DROP DATABASE Database1 END CREATE DATABASE Database1
GO

-- Perform a log backup
BACKUP LOG Database1 TO DISK = 'C:\Backup\Database1.trn' WITH INIT

-- Perform a differential backup
BACKUP DATABASE Database1 TO DISK = 'C:\Backup\Database1.trn' WITH DIFFERENTIAL, INIT

-- Perform a log backup with the option to change the backup type,
-- if the requested backup type cannot be performed
EXECUTE dbo.DatabaseBackup
@Databases = 'Database1',
@Directory = 'C:\Backup',
@BackupType = 'LOG',
@ChangeBackupType = 'Y',
@LogToTable = 'Y'

-- Check log backup status
SELECT last_log_backup_lsn
FROM sys.database_recovery_status
WHERE database_id = DB_ID('Database1')

-- Check differential backup status
SELECT differential_base_lsn
FROM sys.master_files
WHERE database_id = DB_ID('Database1')
AND [file_id] = 1