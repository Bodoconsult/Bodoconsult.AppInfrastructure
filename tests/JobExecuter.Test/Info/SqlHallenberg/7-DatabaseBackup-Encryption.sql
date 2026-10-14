
-- Prerequisites
-- Create database master key (and back it up)
-- Create server certificate (and back it up)

-- Backup a database with SQL Server 2014 native encryption
EXECUTE dbo.DatabaseBackup
@Databases = 'AdventureWorks',
@Directory = 'C:\Backup',
@BackupType = 'FULL',
@Compress = 'Y',
@Encrypt = 'Y',
@EncryptionAlgorithm = 'AES_256',
@ServerCertificate = 'MyCertificate',
@LogToTable = 'Y'