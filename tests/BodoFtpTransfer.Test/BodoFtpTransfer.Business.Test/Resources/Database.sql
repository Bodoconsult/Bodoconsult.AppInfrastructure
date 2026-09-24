CREATE TABLE [FtpFiles]
(
    [ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    [Path] NVARCHAR(900)  NOT NULL,
    [HashCode] NVARCHAR(255),
    [PathRemote] NVARCHAR(900),
    [Source] NVARCHAR(1),
    [HashCodeNew] NVARCHAR(255),
    [Type] NVARCHAR(1),
    [Size] INTEGER,
    [SizeRemote] INTEGER

);