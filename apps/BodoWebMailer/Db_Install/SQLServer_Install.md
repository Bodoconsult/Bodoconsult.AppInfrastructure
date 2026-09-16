# Database installation for BodoWebMailer

BodoWebMailer is a basic app sending formatted emails stored in a database table to the requested receivers.

Currently BodoWebMailer is using a configurable Office365 mailbox to send mails (via Graph).

BodoWebMailer is not a service. It is a simple console app and therefore easy to start from TaskScheduler.

# Prerequisites

BodoWebMailer is currently using a SqlServer database installable on SqlServer Express 2019 and later as minimum requirement.

Create a fresh database BodoWebMailer on the SqlServer (Express) you want to use.

# Create the required database entities

You can find the following SQL commands in the file SQLSever_Install.sql in the folder DB_Install. Run the SQL in your database BodoWebMailer i.e. from SSMS to create the required entities in the database:

``` sql
USE [BodoWebMailer]

GO

GO

/****** Object:  DatabaseRole [MailSender]    Script Date: 08.09.2026 17:39:40 ******/
CREATE ROLE [MailSender]
GO

GO

/****** Object:  DatabaseRole [MailUser]    Script Date: 08.09.2026 17:40:05 ******/
CREATE ROLE [MailUser]
GO

USE [BodoWebMailer]
GO

/****** Object:  Table [dbo].[tMail]    Script Date: 08.09.2026 17:40:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tMail](
	[M_ID] [uniqueidentifier] NOT NULL,
	[M_From] [varchar](255) NULL,
	[M_To] [varchar](max) NOT NULL,
	[M_Subject] [varchar](255) NOT NULL,
	[M_Body] [varchar](max) NOT NULL,
	[M_Error] [bit] NOT NULL,
	[M_LogoPath] [varchar](900) NULL,
	[M_SignatureTemplate] [varchar](255) NULL,
	[M_Attachments] [varchar](max) NULL,
	[M_Archive] [bit] NOT NULL,
	[M_Queries] [varchar](max) NULL,
	[M_Zip] [bit] NOT NULL,
	[M_ZipPassword] [varchar](50) NULL,
 CONSTRAINT [PK_tMail] PRIMARY KEY CLUSTERED 
(
	[M_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF_tMail_M_ID]  DEFAULT (newid()) FOR [M_ID]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF_tMail_M_Error]  DEFAULT ((0)) FOR [M_Error]
GO

ALTER TABLE [dbo].[tMail] ADD  CONSTRAINT [DF__tMail__M_Archive__0BC6C43E]  DEFAULT ((0)) FOR [M_Archive]
GO

ALTER TABLE [dbo].[tMail] ADD  DEFAULT ((0)) FOR [M_Zip]
GO

GO

CREATE TABLE [dbo].[tMailArchive](
	[M_ID] [uniqueidentifier] NOT NULL,
	[M_From] [varchar](255) NULL,
	[M_To] [varchar](max) NOT NULL,
	[M_Subject] [varchar](255) NOT NULL,
	[M_Body] [varchar](max) NOT NULL,
	[M_LogoPath] [varchar](900) NULL,
	[M_SignatureTemplate] [varchar](255) NULL,
	[M_Date] [datetime] NOT NULL,
	[M_Attachments] [varchar](max) NULL,
	[M_Queries] [varchar](max) NULL,
	[M_Zip] [bit] NOT NULL,
	[M_ZipPassword] [varchar](50) NULL,
 CONSTRAINT [PK_tMailArchive] PRIMARY KEY CLUSTERED 
(
	[M_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  CONSTRAINT [DF_tMailArchive_M_ID]  DEFAULT (newid()) FOR [M_ID]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  CONSTRAINT [DF__tMailArch__M_Dat__145C0A3F]  DEFAULT (getdate()) FOR [M_Date]
GO

ALTER TABLE [dbo].[tMailArchive] ADD  DEFAULT ((0)) FOR [M_Zip]
GO

GO

create proc [dbo].[spMail_Delete]
@ID uniqueidentifier
/*
Delete mail after sending

© 2026 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

delete from tMail where M_ID=@ID


GO

GO


CREATE proc [dbo].[spMail_FetchAll]
/*
Alle Mails holen

© 2026 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

--begin try

SELECT [M_ID]
		, M_FROM
		, [M_To]
		, [M_Subject]
		, [M_Body]
		, [M_LogoPath]
		, [M_SignatureTemplate] 
		, M_Attachments
		, M_Archive
		, M_Queries
		, M_Zip
		, M_ZipPassword
FROM [dbo].[tMail]
where [M_Error]=0


--end try
--begin catch
--	declare @Mail varchar(255), @MailMsg varchar(max)

--	select @Mail=CAST(value as varchar(255)) from dbo.Settings where SKey = 'MailAdmin'

--	set @MailMsg= DB_NAME()+': '+ERROR_PROCEDURE()+': Row '+cast(ERROR_LINE() as varchar(max))+': '+ERROR_MESSAGE()
	
--	print @MailMsg
	
--	exec dbo.spMail_New @Mail, 'Database-Error',@MailMsg, null, null, null
	
--end catch

GO

GO

CREATE proc [dbo].[spMail_MoveToArchive]
@M_ID uniqueidentifier
/*
Move mails to archive

© 2026 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

--begin try


	INSERT INTO [dbo].[tMailArchive]
			   ([M_ID]
			   ,[M_From]
			   ,[M_To]
			   ,[M_Subject]
			   ,[M_Body]
			   ,[M_LogoPath]
			   ,[M_SignatureTemplate]
			   ,[M_Attachments]
			   , M_Queries
				, M_Zip
			, M_ZipPassword)
	SELECT [M_ID]
		  ,[M_From]
		  ,[M_To]
		  ,[M_Subject]
		  ,[M_Body]
		  ,[M_LogoPath]
		  ,[M_SignatureTemplate]
		  ,[M_Attachments]
		  , M_Queries
		, M_Zip
		, M_ZipPassword
	FROM [dbo].[tMail]
	where M_ID=@M_ID

	delete from tMail where M_ID=@M_ID

--end try
--begin catch
--	declare @Mail varchar(255), @MailMsg varchar(max)

--	select @Mail=CAST(value as varchar(255)) from dbo.Settings where SKey = 'MailAdmin'

--	set @MailMsg= DB_NAME()+': '+ERROR_PROCEDURE()+': Row '+cast(ERROR_LINE() as varchar(max))+': '+ERROR_MESSAGE()
	
--	print @MailMsg
	
--	exec dbo.spMail_New @Mail, 'Database-Error',@MailMsg, null, null, null
	
--end catch


GO

GO


create proc [dbo].[spMail_SetError]
@ID uniqueidentifier
/*
Set error flag for a mail if sending this mail has failed

© 2026 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

update tMail
set M_Error=1
where M_ID=@ID


GO

GO

create proc [dbo].[spMail_Test]
/*
Create a test mail

© 2026 Bodoconsult EDV-Dienstleistungen GmbH
*/
as


set nocount on
set transaction isolation level read uncommitted

	INSERT INTO [dbo].[tMail]
           ([M_From]
           ,[M_To]
           ,[M_Subject]
           ,[M_Body]
           ,[M_Error]
           ,[M_LogoPath]
           ,[M_SignatureTemplate]
           )
     VALUES
           ('noreply@test.de'
           ,'info@bodoconsult.de'
           ,'Testmail'
           ,'This is a test mail from BodoWebMailer'
           ,0
		   ,'c:\bodoconsult\Logos\BodoConsult.gif'
           ,'BodoConsultSignature')
GO

GRANT EXEC ON dbo.spMail_Delete TO MailSender

GO

GRANT EXEC ON dbo.spMail_FetchAll TO MailSender

GO

GRANT EXEC ON dbo.spMail_MoveToArchive To MailSender

GO

GRANT INSERT, UPDATE, DELETE, SELECT ON dbo.t_Mail TO MailSender

GO

GRANT INSERT ON dbo.t_Mail TO MailUser

GO

GRANT INSERT, UPDATE, DELETE, SELECT ON dbo.t_MailArchive TO MailSender

GO


GO

CREATE TABLE [dbo].[Settings](
	[S_ID] [uniqueidentifier] NOT NULL,
	[sKey] [varchar](255) NOT NULL,
	[Value] [varchar](max) NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY NONCLUSTERED 
(
	[S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_S_ID]  DEFAULT (newid()) FOR [S_ID]
GO

GO

GRANT SELECT ON dbo.Settings TO MailSender

GO
```

# Stored credentials to database

Now open a text editor and create the following JSON text in it for Office 365 usage:

``` csharp
{
	"$type": "Bodoconsult.Web.Mail.Models.O365MailAccount, Bodoconsult.Web.Mail",
	"Instance": "??encryptedValue??",
	"Tenant": "??encryptedValue??",
	"ClientId": "??encryptedValue??",
	"ClientSecret": "??encryptedValue??",
	"UserName": "??encryptedValue??",
	"Scope": "??encryptedValue??"
}
```

Set Instance to 'https://login.microsoftonline.com/{0}', Scope to 'https://graph.microsoft.com/.default' normally.

All value shvae to encrypted. Use command line command

``` cmd
BodoWebMailer /p 
```

to encrypt each required token.

Copy the JSON to the following SQl statement (replace the existing JSON in the SQL below)s:

``` sql
GO

INSERT INTO [dbo].[Settings]
           ([S_ID]
           ,[sKey]
           ,[Value]
           ,[Description])
     VALUES
           (newid()
           ,'MailAccount'
           ,'{
	"$type": "Bodoconsult.Web.Mail.Model.O365MailAccount, Bodoconsult.Web.Mail",
	"Instance": "Your instance encrypted",
	"Tenant": "Your tenant encrypted",
	"ClientId": "Your clientID encrypted",
	"ClientSecret": "Your client secret encrypted",
	"UserName": "Your username encrypted",
	"Scope": "Your scope encrypted"
}'
           ,'JSON string with mail acccount object')

GO           
```

Run the SQL statement

# Using SMTP

For using 

``` csharp
{
	"$type": "Bodoconsult.Web.Mail.Models.SmtpMailAccount, Bodoconsult.Web.Mail",
	"SmtpServer": "??encryptedValue??",
	"SmtpAccountName": "??encryptedValue??",
	"SmtpPassword": "??encryptedValue??",
	"MailAddressSender": "??encryptedValue??",
	"UseSecureConnection": true
}
```

