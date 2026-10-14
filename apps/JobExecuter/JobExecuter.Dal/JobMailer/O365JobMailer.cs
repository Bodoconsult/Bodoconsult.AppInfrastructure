// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Formatter;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Interfaces;

namespace JobExecuter.Dal.JobMailer;

/// <summary>
/// Sends mails via secure SMTP server
/// </summary>
public class O365JobMailer : IJobMailer
{
    private Bodoconsult.Web.Mail.Mailers.O365Mailer _mailer;

    private readonly O365MailAccount _o365MailAccount;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="o365MailAccount">Current mail account</param>
    public O365JobMailer(O365MailAccount o365MailAccount)
    {
        _o365MailAccount = o365MailAccount;
    }

    /// <summary>
    /// HTML template for mail body
    /// </summary>
    public string HtmlTemplate { get; set; }

    /// <summary>
    /// Load default template for HTML mails
    /// </summary>
    public void LoadDefaultHtmlTemplate()
    {
        HtmlTemplate = FileHelper.GetTemplate("HtmlMail");
    }

    /// <inheritdoc />
    /// <summary>
    /// Send a mail 
    /// </summary>
    /// <param name="mailTo">receiver of the mail</param>
    /// <param name="subject">subject of the mail</param>
    /// <param name="body">mail body</param>
    /// <param name="priority">true=high priority</param>
    /// <returns>true on success</returns>
    // ReSharper disable once RedundantAssignment
    public bool SendMail(string mailTo, string subject, IStructuredText body, bool priority)
    {
        try
        {
            if (_mailer == null)
            {
                _mailer = new Bodoconsult.Web.Mail.Mailers.O365Mailer(null);
                _mailer.LoadMailAccount(_o365MailAccount);
                _mailer.Init();
            }

            var f = new HtmlTextFormatter
            {
                StructuredText = body,
                Template = HtmlTemplate,
                Title = subject
            };
            var result = f.GetFormattedText();

#if DEBUG
            mailTo = "robert.leisner@bodoconsult.de";
#endif

            _mailer.SendMail(mailTo, subject, result);

            return true;
        }
        catch //(Exception ex)
        {
            return false;
        }
    }
    /// <summary>
    /// Send a test mail for checking the SMTP account
    /// </summary>
    /// <param name="email"></param>
    public void SendDummyMail(string email)
    {
        var text = new StructuredText();
        text.AddParagraph("This is an test mail sent by JobExcuter");

        SendMail(email, "Test mail", text, false);
    }
}