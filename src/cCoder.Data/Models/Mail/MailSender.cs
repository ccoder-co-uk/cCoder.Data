using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Mail;

public class MailSender
{
    public Guid Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; }

    public string ProviderName { get; set; } = "Smtp";

    public string User { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public string FromEmail { get; set; }

    public int Port { get; set; }

    public bool EnableSSL { get; set; }

    public virtual App App { get; set; }

    public virtual ICollection<QueuedEmail> QueuedEmails { get; set; }

    public virtual ICollection<SentEmail> SentEmails { get; set; }
}
