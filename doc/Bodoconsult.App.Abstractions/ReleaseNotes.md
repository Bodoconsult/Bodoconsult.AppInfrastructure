Release notes for Bodoconsult.App.Abstractions
==========================

With each release all the repository internal references are updated.

# 1.0.5 

-	Extracted abstractions to Bodoconsult.App.Abstractions

# 1.0.6

-	Added some properties for I18N to IAppGlobals

# 1.0.7

-	Move package Bodoconsult.Typography to Bodoconsult.App.Abstractions

# 1.0.8

-	Added more interfaces and classes for typography to be used in Bodoconsult.Text, Bodoconsult.pdf etc.

-	Added IntExtensions and TypoColorExtensions classes

-	Enhanced MeasurementHelper class for new units like DIU, DXA, Twips etc.

# 1.0.9

-   New interface II18NBase as common base interface for I18N and I18NServer class to make ILocalesProviderPackage more flexible

-   Added properties BackgroundImagePath and WatermarkText to ITypoMetaData

-   Added extension methods for Memory<byte> and ReadOnlySequence<Byte> in classes MemoryExtensions and ReadOnlySequenceExtensions

# 1.0.10

NOT intended for public use

# 1.0.11

-   Better PDB handling

-   Added Args property to IAppStartParameter to store command line args

-   Added ErrorMailAddress property to IAppStartParameter to email address for sending mail to in case of exceptions

-   IMailAccount, IO365MailAcccount, SmtpMailAccount and O365MailAccount moved from Bodoconsult.Web.Mail to Bodoconsult.App.Abstractions.Interfaces