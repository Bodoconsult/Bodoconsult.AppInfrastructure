DECLARE @productVersion nvarchar(128)
SELECT @productVersion = CONVERT(nvarchar(128), SERVERPROPERTY('ProductVersion'))
DECLARE @majorVersion tinyint
SELECT @majorVersion = CONVERT(tinyint, SUBSTRING(@productVersion, 0, CHARINDEX('.' , @productVersion)))
DECLARE @serverLevel tinyint
SET @serverLevel = @majorVersion * 10
select @serverLevel