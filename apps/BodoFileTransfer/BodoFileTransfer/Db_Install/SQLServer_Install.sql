USE [BodoFileTransfer]

GO

USE [BodoFileTransfer]
GO

/****** Object:  Table [dbo].[Settings]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Settings](
	[S_ID] [uniqueidentifier] NOT NULL,
	[sKey] [varchar](255) NOT NULL,
	[Value] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY NONCLUSTERED 
(
	[S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_Account]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_Account](
	[A_ID] [int] IDENTITY(1,1) NOT NULL,
	[A_GUID] [uniqueidentifier] NOT NULL,
	[A_Name] [varchar](255) NOT NULL,
	[A_Keyword] [varchar](255) NULL,
	[A_TransferTargetId] [int] NOT NULL,
	[A_InboxPath] [varchar](900) NULL,
	[A_ArchivePath] [varchar](900) NULL,
	[A_DocCounter] [int] NOT NULL,
	[A_ManualTransfer] [bit] NOT NULL,
 CONSTRAINT [PK_t_Account] PRIMARY KEY CLUSTERED 
(
	[A_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_Files]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_Files](
	[F_ID] [uniqueidentifier] NOT NULL,
	[F_Path] [varchar](900) NOT NULL,
	[F_Title] [varchar](255) NOT NULL,
	[F_Keyword] [varchar](255) NOT NULL,
	[F_AccountID] [int] NOT NULL,
	[F_DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_t_Files] PRIMARY KEY CLUSTERED 
(
	[F_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_FilesArchive]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_FilesArchive](
	[F_ID] [uniqueidentifier] NOT NULL,
	[F_Path] [varchar](900) NOT NULL,
	[F_Title] [varchar](255) NOT NULL,
	[F_Keyword] [varchar](255) NOT NULL,
	[F_AccountID] [int] NOT NULL,
	[F_Date] [datetime] NOT NULL,
	[F_DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_t_FilesArchive] PRIMARY KEY CLUSTERED 
(
	[F_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_ImapMail]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_ImapMail](
	[IM_ID] [int] IDENTITY(1,1) NOT NULL,
	[IM_GUID] [uniqueidentifier] NOT NULL,
	[IM_AccountId] [int] NOT NULL,
	[IM_Host] [varchar](255) NOT NULL,
	[IM_Port] [int] NOT NULL,
	[IM_UserName] [varchar](255) NOT NULL,
	[IM_Password] [varchar](255) NOT NULL,
	[IM_FileExtensionFilter] [varchar](255) NULL,
	[IM_SubjectFilter] [varchar](255) NULL,
	[IM_UseSsl] [bit] NOT NULL,
	[IM_Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_t_ImapMail] PRIMARY KEY CLUSTERED 
(
	[IM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_O365Mail]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_O365Mail](
	[O_ID] [int] IDENTITY(1,1) NOT NULL,
	[O_GUID] [uniqueidentifier] NOT NULL,
	[O_AccountId] [int] NOT NULL,
	[O_Instance] [varchar](255) NOT NULL,
	[O_Tenant] [varchar](255) NOT NULL,
	[O_ClientId] [varchar](255) NOT NULL,
	[O_ClientSecret] [varchar](255) NOT NULL,
	[O_Scope] [varchar](255) NOT NULL,
	[O_FileExtensionFilter] [varchar](255) NULL,
	[O_SubjectFilter] [varchar](255) NULL,
	[O_UserName] [varchar](255) NOT NULL,
	[O_Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_t_O365Mail] PRIMARY KEY CLUSTERED 
(
	[O_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_O365MailSender]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_O365MailSender](
	[OS_ID] [int] IDENTITY(1,1) NOT NULL,
	[OS_GUID] [uniqueidentifier] NOT NULL,
	[OS_Instance] [varchar](255) NOT NULL,
	[OS_Tenant] [varchar](255) NOT NULL,
	[OS_ClientId] [varchar](255) NOT NULL,
	[OS_ClientSecret] [varchar](255) NOT NULL,
	[OS_Scope] [varchar](255) NOT NULL,
	[OS_UserName] [varchar](255) NOT NULL,
	[OS_Name] [varchar](255) NOT NULL,
	[OS_NumberOfAttachments] [int] NOT NULL,
	[OS_SizeOfAttachments] [int] NOT NULL,
 CONSTRAINT [PK_t_O365MailSender] PRIMARY KEY CLUSTERED 
(
	[OS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_Trace]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_Trace](
	[T_ID] [uniqueidentifier] NOT NULL,
	[T_Date] [datetime] NOT NULL,
	[T_F_ID] [uniqueidentifier] NOT NULL,
	[T_Code] [int] NOT NULL,
	[T_Message] [nvarchar](max) NULL,
 CONSTRAINT [PK_t_Trace] PRIMARY KEY CLUSTERED 
(
	[T_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

/****** Object:  Table [dbo].[t_TransferTarget]    Script Date: 17.09.2026 16:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[t_TransferTarget](
	[TT_ID] [int] IDENTITY(1,1) NOT NULL,
	[TT_GUID] [uniqueidentifier] NOT NULL,
	[TT_Typ] [int] NOT NULL,
	[TT_MailAddress] [varchar](255) NULL,
	[TT_Password] [varchar](255) NULL,
	[TT_O365Sender_ID] [int] NOT NULL,
 CONSTRAINT [PK_tTransferTarget] PRIMARY KEY CLUSTERED 
(
	[TT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)
GO

GRANT DELETE ON [dbo].[t_TransferTarget] TO [BftUser] AS [dbo]
GO

GRANT INSERT ON [dbo].[t_TransferTarget] TO [BftUser] AS [dbo]
GO

GRANT SELECT ON [dbo].[t_TransferTarget] TO [BftUser] AS [dbo]
GO

GRANT UPDATE ON [dbo].[t_TransferTarget] TO [BftUser] AS [dbo]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_t_Account]    Script Date: 17.09.2026 16:00:59 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_t_Account] ON [dbo].[t_Account]
(
	[A_TransferTargetId] ASC,
	[A_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
GO

ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_S_ID]  DEFAULT (newid()) FOR [S_ID]
GO

ALTER TABLE [dbo].[t_Account] ADD  CONSTRAINT [DF_t_Account_A_GUID]  DEFAULT (newid()) FOR [A_GUID]
GO

ALTER TABLE [dbo].[t_Account] ADD  DEFAULT ((1)) FOR [A_TransferTargetId]
GO

ALTER TABLE [dbo].[t_Account] ADD  DEFAULT ((0)) FOR [A_DocCounter]
GO

ALTER TABLE [dbo].[t_Account] ADD  DEFAULT ((0)) FOR [A_ManualTransfer]
GO

ALTER TABLE [dbo].[t_Files] ADD  CONSTRAINT [DF_t_Files_F_ID]  DEFAULT (newid()) FOR [F_ID]
GO

ALTER TABLE [dbo].[t_Files] ADD  DEFAULT (getdate()) FOR [F_DateCreated]
GO

ALTER TABLE [dbo].[t_FilesArchive] ADD  CONSTRAINT [DF_t_FilesArchive_F_ID]  DEFAULT (newid()) FOR [F_ID]
GO

ALTER TABLE [dbo].[t_FilesArchive] ADD  DEFAULT (getdate()) FOR [F_Date]
GO

ALTER TABLE [dbo].[t_FilesArchive] ADD  DEFAULT (getdate()) FOR [F_DateCreated]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  CONSTRAINT [DF_t_ImapMail_IM_GUID]  DEFAULT (newid()) FOR [IM_GUID]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  CONSTRAINT [DF_t_ImapMail_IM_Port]  DEFAULT ((993)) FOR [IM_Port]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  DEFAULT ('.pdf') FOR [IM_FileExtensionFilter]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  DEFAULT ('Rechnung,Invoice') FOR [IM_SubjectFilter]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  DEFAULT ((1)) FOR [IM_UseSsl]
GO

ALTER TABLE [dbo].[t_ImapMail] ADD  DEFAULT ('Imap mail account') FOR [IM_Name]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  CONSTRAINT [DF_t_O365Mail_O_GUID]  DEFAULT (newid()) FOR [O_GUID]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  CONSTRAINT [DF_t_O365Mail_O_Instance]  DEFAULT ('https://login.microsoftonline.com/{0}') FOR [O_Instance]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  CONSTRAINT [DF_t_O365Mail_O_Scope]  DEFAULT ('https://graph.microsoft.com/.default') FOR [O_Scope]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  DEFAULT ('.pdf,.zip') FOR [O_FileExtensionFilter]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  DEFAULT ('Rechnung,Invoice') FOR [O_SubjectFilter]
GO

ALTER TABLE [dbo].[t_O365Mail] ADD  DEFAULT ('O365 mail account') FOR [O_Name]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  CONSTRAINT [DF_t_O365MailSender_OS_GUID]  DEFAULT (newid()) FOR [OS_GUID]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  CONSTRAINT [DF_t_O365MailSender_OS_Instance]  DEFAULT ('https://login.microsoftonline.com/{0}') FOR [OS_Instance]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  CONSTRAINT [DF_t_O365MailSender_OS_Scope]  DEFAULT ('https://graph.microsoft.com/.default') FOR [OS_Scope]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  DEFAULT ('O365 mail account') FOR [OS_Name]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  DEFAULT ((50)) FOR [OS_NumberOfAttachments]
GO

ALTER TABLE [dbo].[t_O365MailSender] ADD  DEFAULT ((20)) FOR [OS_SizeOfAttachments]
GO

ALTER TABLE [dbo].[t_Trace] ADD  CONSTRAINT [DF_t_Trace_T_ID]  DEFAULT (newid()) FOR [T_ID]
GO

ALTER TABLE [dbo].[t_Trace] ADD  CONSTRAINT [DF_t_Trace_T_Date]  DEFAULT (getdate()) FOR [T_Date]
GO

ALTER TABLE [dbo].[t_TransferTarget] ADD  CONSTRAINT [DF_tTransferTarget_TT_GUID]  DEFAULT (newid()) FOR [TT_GUID]
GO

ALTER TABLE [dbo].[t_TransferTarget] ADD  DEFAULT ((0)) FOR [TT_O365Sender_ID]
GO

GO

/****** Object:  StoredProcedure [dbo].[spAccount_GetMailData]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spAccount_GetMailData]
@AccountId int
as

select a.A_Name, a.A_Keyword, TT_mailAddress as MailAddress, TT_Password as [Password], TT_Typ, TT_O365Sender_ID
from t_account a inner join t_transfertarget t
	on A.A_transfertargetId=t.TT_ID
where A_ID=@AccountId

GO

GRANT EXECUTE ON [dbo].[spAccount_GetMailData] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spAccount_GetTransferAccounts]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spAccount_GetTransferAccounts]

AS
	select a.a_id
	from t_transfertarget tt inner join t_account a
		on tt.tt_id=a.a_transfertargetid
		inner join t_files f
		on a.a_id=f.F_AccountId
	where tt.TT_Typ=1
		AND isnull(a.A_ManualTransfer,0)=0
	group by a.a_id 
GO

GRANT EXECUTE ON [dbo].[spAccount_GetTransferAccounts] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spAccount_UpdateDocCounter]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spAccount_UpdateDocCounter]
@A_ID int
/*
Add one to the current document counter of the account

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

UPDATE T_Account
SET A_DocCounter=isnull(A_DocCounter,0)+1
WHERE A_ID=@A_ID

GO

GRANT EXECUTE ON [dbo].[spAccount_UpdateDocCounter] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spDbDocu_00_Overview]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spDbDocu_00_Overview]

/*


© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

declare @Data table (
	[What] varchar(255),
	[Value] int
)

insert into @Data(What, [Value])
select 'Number of transfer targets', count(*)
from t_TransferTarget


insert into @Data(What, [Value])
select '- Number of SMTP mail transfer targets', count(*)
from t_TransferTarget
where TT_Typ=1


insert into @Data(What, [Value])
select 'Number of accounts', count(*)
from t_account


insert into @Data(What, [Value])
select 'Workload files', count(*)
from t_files

insert into @Data(What, [Value])
select 'Processed files', count(*)
from t_filesArchive

select *
FROM @data 


GO

GRANT EXECUTE ON [dbo].[spDbDocu_00_Overview] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spDbDocu_01_Transfer_Targets]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spDbDocu_01_Transfer_Targets]

/*
DbDocumentation: transfer targets

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


SELECT [TT_ID] as[TargetId]
      , case when [TT_Typ]=1 then 'SMTP-Mail'
		else 'unknown' end
		as [Transfer type]

      ,[TT_MailAddress] as [Receiver mail address]
FROM [dbo].[t_TransferTarget]


GO

GRANT EXECUTE ON [dbo].[spDbDocu_01_Transfer_Targets] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spDbDocu_02_Accounts]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spDbDocu_02_Accounts]

/*
DbDocumentation: transfer targets

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


SELECT [A_ID] as [AcccountId]
      ,[A_Name] as [Account owner name]
      ,[A_Keyword] as [Keyword]
      ,[A_InboxPath] as [Inbox path]
      ,[A_ArchivePath] as [Archive path]
	  , [TT_ID] as[TargetId]
      , case when [TT_Typ]=1 then 'SMTP-Mail'
		else 'unknown' end
		as [Transfer type]
      ,[TT_MailAddress] as [Receiver mail address]
FROM dbo.t_Account a INNER JOIN [dbo].[t_TransferTarget] t
	ON a.A_TransferTargetId=t.TT_ID


GO

GRANT EXECUTE ON [dbo].[spDbDocu_02_Accounts] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spDbDocu_03_Imap_connectors]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spDbDocu_03_Imap_connectors]

/*
DbDocumentation: transfer targets

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


SELECT [IM_ID] as ImapConnectorId
	, 	  [A_ID] as [AcccountId]
      ,[A_Name] as [Account owner name]
      ,[A_InboxPath] as [Inbox path]
      ,[IM_Host] as ImapHost
      ,[IM_Port] as [Port]
	  , IM_UseSsl as [Use SSL]
      ,[IM_UserName] as [Username]
      ,[IM_FileExtensionFilter] as [File extension filter]
      ,[IM_SubjectFilter] as [Subject filter]

FROM dbo.t_Account a INNER JOIN [dbo].[t_ImapMail] t
	ON a.A_ID=t.IM_AccountId

GO

GRANT EXECUTE ON [dbo].[spDbDocu_03_Imap_connectors] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spDbDocu_04_File_Statistics]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spDbDocu_04_File_Statistics]

/*
File statistics per account this year, last year and last last year

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

declare @DV datetime, @DB datetime

declare @Data table (
	[Id] int,
	[Account] varchar(255),
	[CYear] int,
	[LYear] int,
	[LLYear] int
)


insert into @Data(Id, Account)
select A_ID, A_name
from t_account
order by A_name


set @DV = convert(datetime, cast(year(getdate()) as varchar(255))+'0101', 112)
set @DB = getdate()

update @Data
set CYear=isnull(c.Anzahl,0)
from @Data d left join (
select F_AccountID, count(*) as Anzahl
from t_FilesArchive
where F_Date>=@DV and F_Date<@DB
group by F_AccountID) as c
ON d.ID=c.F_AccountID


set @DB = @DV
set @DV = dateadd(yy, -1, @DB)


update @Data
set LYear=isnull(c.Anzahl,0)
from @Data d left join (
select F_AccountID, count(*) as Anzahl
from t_FilesArchive
where F_Date>=@DV and F_Date<@DB
group by F_AccountID) as c
ON d.ID=c.F_AccountID

set @DB = @DV
set @DV = dateadd(yy, -1, @DB)


update @Data
set LLYear=isnull(c.Anzahl,0)
from @Data d left join (
select F_AccountID, count(*) as Anzahl
from t_FilesArchive
where F_Date>=@DV and F_Date<@DB
group by F_AccountID) as c
ON d.ID=c.F_AccountID


select *
from @Data
order by account

GO

GRANT EXECUTE ON [dbo].[spDbDocu_04_File_Statistics] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFile_Delete]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spFile_Delete]
	@F_ID uniqueidentifier 
AS
	

	DELETE FROM T_Files WHERE F_ID=@F_ID;


GO

GRANT EXECUTE ON [dbo].[spFile_Delete] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_CheckIfExisting]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spFiles_CheckIfExisting]
@Path varchar(900), @AccountId int
/*
Check if a file already exists

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

declare @Exists bit
set @exists=1

set @Path = '%'+@Path

if not exists (select * from T_FilesArchive where F_AccountID=@AccountId AND  F_Path LIKE @Path)
	AND not exists (select * from T_Files where F_AccountID=@AccountId AND  F_Path LIKE @Path)
begin
	set @Exists=0
end

select @Exists as erg



GO

GRANT EXECUTE ON [dbo].[spFiles_CheckIfExisting] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_Delete]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spFiles_Delete]
@FileId uniqueidentifier
as

delete from t_Files where F_ID=@FileId

GO

GRANT EXECUTE ON [dbo].[spFiles_Delete] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_GetFile]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spFiles_GetFile]
@FileId uniqueidentifier
as


SELECT * 
FROM t_Files
WHERE F_ID=@FileId

GO

GRANT EXECUTE ON [dbo].[spFiles_GetFile] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_GetFilesForAccount]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[spFiles_GetFilesForAccount]
	@AccountId int
AS
	

	SELECT * 
	FROM T_Files 
	WHERE F_AccountID=@AccountId
	ORDER BY F_DateCreated DESC


GO

GRANT EXECUTE ON [dbo].[spFiles_GetFilesForAccount] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_GetFilesForAccountByDate]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[spFiles_GetFilesForAccountByDate]
	@AccountId int, @DV datetime, @DB datetime
AS
	

	SELECT * 
	FROM T_Files 
	WHERE F_AccountID=@AccountId
		AND F_DateCreated BETWEEN @DV AND @DB
	ORDER BY F_DateCreated DESC


GO

GRANT EXECUTE ON [dbo].[spFiles_GetFilesForAccountByDate] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFiles_New]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spFiles_New]
@FileId uniqueidentifier, @Path varchar(900), @Title varchar(255), @Keyword varchar(255), @AccountId int, @DateCreated datetime
/*
Add a new file

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

if not exists (select * from T_FilesArchive where F_Path=@Path)
	AND not exists (select * from T_Files where F_Path=@Path)
begin

	INSERT INTO [dbo].[t_Files]
			   ([F_ID]
			   ,[F_Path]
			   ,[F_Title]
			   ,[F_Keyword]
			   ,[F_AccountID]
			   ,[F_DateCreated])
	VALUES
			   (@FileId
			   ,@Path
			   ,@Title
			   ,@Keyword
			   ,@AccountID
			   ,@DateCreated)

	return 1

end

return 0



GO

GRANT EXECUTE ON [dbo].[spFiles_New] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchiv_ResendFiles]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spFilesArchiv_ResendFiles]
@Date datetime
/*
Resend emails starting with a certain date
*/
as

INSERT INTO [dbo].[t_Files]
           ([F_ID]
           ,[F_Path]
           ,[F_Title]
           ,[F_Keyword]
           ,[F_AccountID]
           ,[F_DateCreated])
SELECT [F_ID]
      ,[F_Path]
      ,[F_Title]
      ,[F_Keyword]
      ,[F_AccountID]
      ,[F_DateCreated]
  FROM [dbo].[t_FilesArchive]
WHERE F_Date> @Date

DELETE FROM [dbo].[t_FilesArchive]
WHERE F_Date> @Date

GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_Delete]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spFilesArchive_Delete]
	@F_ID uniqueidentifier 
AS
	

	DELETE FROM T_FilesArchive WHERE F_ID=@F_ID;


GO

GRANT EXECUTE ON [dbo].[spFilesArchive_Delete] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_GetFile]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spFilesArchive_GetFile]
@FileId uniqueidentifier
as


SELECT * 
FROM t_FilesArchive
WHERE F_ID=@FileId

GO

GRANT EXECUTE ON [dbo].[spFilesArchive_GetFile] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_GetFilesForAccount]    Script Date: 17.09.2026 16:01:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spFilesArchive_GetFilesForAccount]
	@AccountId int
AS
	

	SELECT * 
	FROM T_FilesArchive 
	WHERE F_AccountID=@AccountId
	ORDER BY F_DateCreated DESC


GO

GRANT EXECUTE ON [dbo].[spFilesArchive_GetFilesForAccount] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_GetFilesForAccountByDate]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[spFilesArchive_GetFilesForAccountByDate]
	@AccountId int, @DV datetime, @DB datetime
AS
	

	SELECT * 
	FROM T_FilesArchive 
	WHERE F_AccountID=@AccountId
		AND F_DateCreated BETWEEN @DV AND @DB
	ORDER BY F_DateCreated DESC


GO

GRANT EXECUTE ON [dbo].[spFilesArchive_GetFilesForAccountByDate] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_GetFilesForAccountForDelivery]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spFilesArchive_GetFilesForAccountForDelivery]
	@AccountId int
AS
	
	declare @D datetime

	set @D = GETDATE()

	set @D = DATEADD(m, -2, @D)

	select @D

	SELECT * 
	FROM T_FilesArchive 
	WHERE F_AccountID=@AccountId
		--AND (F_DATE IS NULL OR F_Date > @D)
	ORDER BY F_DateCreated DESC

GO

GRANT EXECUTE ON [dbo].[spFilesArchive_GetFilesForAccountForDelivery] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spFilesArchive_New]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spFilesArchive_New]
@FileId uniqueidentifier, @Path varchar(900), @Title varchar(255), @Keyword varchar(255), @AccountId int, @DateCreated datetime, @SendDate datetime
/*
Add a new file

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

if not exists (select * from T_FilesArchive where F_Path=@Path)
begin

	INSERT INTO [dbo].[t_FilesArchive]
			   ([F_ID]
			   ,[F_Path]
			   ,[F_Title]
			   ,[F_Keyword]
			   ,[F_AccountID]
			   ,[F_DateCreated]
			   ,[F_Date])
	VALUES
			   (@FileId
			   ,@Path
			   ,@Title
			   ,@Keyword
			   ,@AccountID
			   ,@DateCreated
			   , @SendDate)

	return 1

end

return 0

GO

GRANT EXECUTE ON [dbo].[spFilesArchive_New] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spImapMail_GetAllAccounts]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spImapMail_GetAllAccounts]

/*


© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


select i.*, a.*
from t_ImapMail i inner join t_account a
	on i.IM_AccountId=a.a_id
where ISNULL(a.A_InboxPath,'')<>''
GO

GRANT EXECUTE ON [dbo].[spImapMail_GetAllAccounts] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spInbox_GetAllInboxes]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spInbox_GetAllInboxes]

/*
Get all inboxes

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

select *
from t_account
where isnull(A_InboxPath, '')<>'' 
	AND isnull(A_ArchivePath, '')<>''

GO

GRANT EXECUTE ON [dbo].[spInbox_GetAllInboxes] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spMAIL_New]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE    procedure [dbo].[spMAIL_New]
@TO varchar(4000), @Subject varchar(255), @Msg text, @Q varchar(1000)=null, @Delimiter varchar(5)=null, @NoHeader bit=0, @Att varchar(max)=null, @Zip bit=0, @ZipPassword varchar(255)=null
as


	DECLARE @Mail varchar(255), @Signature varchar(255), @LogoPath varchar(900)

	select @Mail=cast([Value] as varchar(255)) FROM dbo.Settings WHERE sKey='SendMailAs'
	select @Signature=cast([Value] as varchar(255)) FROM dbo.Settings WHERE sKey='MailSignature'
	select @LogoPath = cast([value] as varchar(900)) from dbo.settings where skey='MailLogo'

	if @Mail is null SET @Mail='noreply@bodoconsult.de'

	insert into BodoWebMailer.dbo.tMail(M_From, M_To, M_Subject, M_Body, M_LogoPath, M_SignatureTemplate, M_Attachments, M_Archive, M_Zip, M_ZipPassword)
	select @Mail, @To, @Subject, @Msg, @LogoPath, @Signature, @Att, 0, @Zip, @ZipPassword

GO

GRANT EXECUTE ON [dbo].[spMAIL_New] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spMail_SendFilesForAccount]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spMail_SendFilesForAccount]
@AccountId int, @Fake bit = 0
/*
Sedn all files for an account via email

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

declare @Keyword varchar(255), @Name varchar(255), @Subject varchar(255), @Mail varchar(255), @Password varchar(255)
declare @Files varchar(max), @Body varchar(max), @Count int

select @Name=a.A_Name, @Keyword=a.A_Keyword, @Mail=TT_mailAddress, @Password=TT_Password
from t_account a inner join t_transfertarget t
	on A.A_transfertargetId=t.TT_ID
where A_ID=@AccountId and t.tt_typ=1

SET @Subject = 'Datenlieferung '+isnull(' '+@Keyword, '')+isnull(' '+@Name, '')

--print @Mail
--print @Subject


declare @data Table ( 
	F_ID uniqueidentifier
	, F_Path varchar(900)
	, F_Title varchar(255)
	, F_Keyword varchar(255)
	, F_DateCreated datetime)

INSERT INTO @Data(F_ID, F_Path, F_Title, F_Keyword, F_DateCreated)
SELECT F_ID, F_Path, F_Title, isnull(F_Keyword, 'Unbekannt') as F_Keyword, F_DateCreated
FROM t_files f 
WHERE F_AccountId=@AccountId
order by F_Keyword, F_Title

if not exists (select * FROM @data) return

declare @FID uniqueidentifier, @F_Path varchar(900), @F_Title varchar(255), @F_Keyword varchar(255), @Path varchar(255), @Part int 
declare @Number float, @MaxFiles int

set @MaxFiles=20

set @Number= ceiling((select count(*) FROM @data) /cast(@MaxFiles as float))


DECLARE FC CURSOR LOCAL READ_ONLY FOR
SELECT F_ID, F_Path, F_Title, F_Keyword
FROM @Data 
order by F_Keyword, F_Title

OPEN FC

SET @Count=1
SET @Part=1

set @Files = ''
set @Body='<h1>'+@Subject+' Teil '+cast(@Part as varchar(max))+'/'+cast(@Number as varchar(max))++ +'</h1>'+char(13)+char(10)

FETCH NEXT FROM FC
INTO @FID, @F_path, @F_Title, @F_Keyword



WHILE (@@FETCH_STATUS = 0)
BEGIN

	--print @FID

	set @Files = @Files + @F_Path+';'

	set @Path = RIGHT(@F_Path, CHARINDEX('\', REVERSE(@F_Path)) -1)

	set @Body = @Body+'<p>'+@F_Keyword+': '+@F_Title+' ('+@Path+')</p>'+char(13)+char(10)

    FETCH NEXT FROM FC
    INTO @FID, @F_path, @F_Title, @F_Keyword

	if @COUNT>19
	begin
		--print @Files

		if @Fake=1
		begin
			print @part

			print @Files

			print ''
			print ''
		end
		else
		begin
			exec dbo.spMail_New @Mail, @Subject, @Body, null, null, null, @Files, 1, @Password
		end
		
		
		SET @Part = @Part + 1

		set @Files = ''
		set @Body='<h1>'+@Subject+' Teil '+cast(@Part as varchar(max))+'/'+cast(@Number as varchar(max))++ +'</h1>'+char(13)+char(10)

		SET @Count=0

	end

	SET @Count=@Count+1
END

CLOSE FC

DEALLOCATE FC

-- Send remaining files
--print @Files
--print @Body

if @Fake=1
begin
	print @part

	print @Files

	print ''
	print ''

	return;
end
else
begin
	exec dbo.spMail_New @Mail, @Subject, @Body, null, null, null, @Files, 1, @Password

	-- Archive and delete files from inbox
	INSERT INTO [dbo].[t_FilesArchive]
			   ([F_ID]
			   ,[F_Path]
			   ,[F_Title]
			   ,[F_Keyword]
			   ,[F_AccountID]
			   ,[F_DateCreated])
	select [F_ID]
			   ,[F_Path]
			   ,[F_Title]
			   ,[F_Keyword]
			   ,@AccountId
			   ,[F_DateCreated]
	from @data

	delete from [dbo].[t_Files] where exists (select * from @Data d where d.F_ID=dbo.t_Files.F_ID)

end

GO

GRANT EXECUTE ON [dbo].[spMail_SendFilesForAccount] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spMail_SendmailsForTransferTargets]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spMail_SendmailsForTransferTargets]
@Fake bit = 0
/*
Send mails for targets with typ 1 (SMTP) 

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


declare @AccountId int



DECLARE UC CURSOR LOCAL READ_ONLY FOR
select a.a_id
from t_transfertarget tt inner join t_account a
	on tt.tt_id=a.a_transfertargetid
	inner join t_files f
	on a.a_id=f.F_AccountId
where tt.TT_Typ=1
	AND isnull(a.A_ManualTransfer,0)=0
group by a.a_id 

OPEN UC

FETCH NEXT FROM UC
INTO @AccountId

WHILE (@@FETCH_STATUS = 0)
BEGIN

	EXEC dbo.spMail_SendFilesForAccount @AccountId, @Fake

    FETCH NEXT FROM UC
    INTO @AccountId
END

CLOSE UC

DEALLOCATE UC


GO

GRANT EXECUTE ON [dbo].[spMail_SendmailsForTransferTargets] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spO365Mail_GetAllAccounts]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[spO365Mail_GetAllAccounts]

/*


© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


select i.*, a.*
from t_O365Mail i inner join t_account a
	on i.O_AccountId=a.a_id
where ISNULL(a.A_InboxPath,'')<>'' 
	--AND O_ID=2

GO

GRANT EXECUTE ON [dbo].[spO365Mail_GetAllAccounts] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spO365MailSender_GetAllAccounts]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE proc [dbo].[spO365MailSender_GetAllAccounts]

/*


© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted


select i.*
from t_O365MailSender i

GO

GRANT EXECUTE ON [dbo].[spO365MailSender_GetAllAccounts] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spTrace_GetTracesForFile]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create proc [dbo].[spTrace_GetTracesForFile]
@FileId uniqueidentifier
/*
Get all traces for a file

© 2019 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

select *
from t_trace
where T_F_ID=@FileID
order by T_Date desc

GO

GRANT EXECUTE ON [dbo].[spTrace_GetTracesForFile] TO [BftUser] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[spTraceAdd]    Script Date: 17.09.2026 16:01:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spTraceAdd]
	@T_ID uniqueidentifier,
	@F_ID uniqueidentifier, 
	@Code int,
	@Message varchar(max),
	@Date smalldatetime
AS
	

	INSERT INTO dbo.t_Trace(T_F_ID, T_code, T_message, T_date)
	VALUES (@F_ID, @Code, @Message, @Date)

GO

GRANT EXECUTE ON [dbo].[spTraceAdd] TO [BftUser] AS [dbo]
GO

