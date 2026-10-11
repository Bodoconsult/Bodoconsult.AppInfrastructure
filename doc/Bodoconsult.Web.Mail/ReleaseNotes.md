Release notes for Bodoconsult.Web.Mail
==========================

With each release all the repository internal references are updated.

# 1.0.0

-	New package

# 1.0.10

NOT intended for public use

# 1.0.11

- 	Migration to repository https://github.com/Bodoconsult/Bodoconsult.AppInfrastructure

-	Refactoring for SMTP mailing and O365 Graph based mailing classes with new interface IMailer and implementations SmtpMailer, O365Mailer and FakeMailer

-	Mass mailing implemented with IMassMailHandler / MassMailHandler

-   IMailAccount, IO365MailAcccount, SmtpMailAccount and O365MailAccount moved to Bodoconsult.App.Abstractions.Interfaces


