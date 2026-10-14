
-- Backup to Azure Blob Storage

-- Prerequisites
-- Create storage account (Azure Portal)
-- Create container (Azure Portal)
-- Create credential (SSMS) -- CREATE CREDENTIAL olahallengren WITH IDENTITY= 'StorageAccount', SECRET = 'StorageAccessKey'

EXECUTE dbo.DatabaseBackup
@Databases = 'model',
@URL = 'https://olahallengren.blob.core.windows.net/backup',
@Credential = 'olahallengren',
@BackupType = 'FULL',
@Verify = 'Y',
@Compress = 'Y',
@CheckSum = 'Y',
@LogToTable = 'Y'