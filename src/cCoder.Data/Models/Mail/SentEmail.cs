namespace cCoder.Data.Models.Mail;

/// <summary>
/// A successfully sent email
/// </summary>
public class SentEmail : Email
{
    public DateTimeOffset SentOn { get; set; }

    public string From { get; set; }

    public Guid? MailSenderId { get; set; }

    public virtual MailSender MailSender { get; set; }
}





