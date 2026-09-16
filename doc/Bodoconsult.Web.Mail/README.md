# What does the library

Bodoconsult.Web.Mail library is intended for apps which have to send (but not receive) SMTP mails like console apps.

Mass mail handling features are in an experimental state or work is in progress.

# How to use the library

The source code contain a NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Send a simple mail with plain text via SMTP

``` csharp
    var account = new SmtpMailAccount
    {
        SmtpServer = "smtp.test.de",
        SmtpPassword = "test123!",
        SmtpAccountName = "test@test.de",
        MailAddressSender = "noreply@test.de",
        UseSecureConnection = true
    };
			
    var smtp = new SmtpMailer(account);
    smtp.Init();
    smtp.Logon();

    smtp.SendMail("to@test.de", "Testmail", "dgdgdgdgdgs sfsgdgs sshshsh");
```

## Send a HTML mail via SMTP

``` csharp
    var account = new SmtpMailAccount
    {
        SmtpServer = "smtp.test.de",
        SmtpPassword = "test123!",
        SmtpAccountName = "test@test.de",
        MailAddressSender = "noreply@test.de",
        UseSecureConnection = true
    };

    var msg = new MailMessage {IsBodyHtml = true};

    var add = new MailAddress(account.MailAddressSender);
    msg.From = add;

    add = new MailAddress(TestHelper.GetTestReceiver());
    msg.To.Add(add);

    msg.Subject = "Bodoconsult.Core.Web.Mail: test mail";
    msg.Body = "<p>ajHA SADad asd AS Ddad</p>";

    var smtp = new SmtpMailer(account);
    smtp.Init();
    smtp.Logon();

    smtp.SendMail(msg);
```

## Send a HTML mail via Office 365 (Graph)

``` csharp
    var account = new O365MailAccount
    {
        ClientId = ""),
        ClientSecret = "",
        Scope = "https://graph.microsoft.com/.default",
        Instance = "https://login.microsoftonline.com/{0}",
        Tenant = "",
        UserName = ""
    };

    var mailer = new O365Mailer(account);
    mailer.Init();
    mailer.Logon();

    mailer.SendMail("to@test.de", "Testmail", "dgdgdgdgdgs sfsgdgs sshshsh");
```

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

