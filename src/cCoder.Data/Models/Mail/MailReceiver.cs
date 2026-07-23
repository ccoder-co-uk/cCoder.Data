// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Mail;

public class MailReceiver
{
    public Guid Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; }

    public string ProviderName { get; set; }

    public string User { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public bool EnableSSL { get; set; }

    public DateTimeOffset? LastReceivedOn { get; set; }

    public bool IsEnabled { get; set; }

    public virtual App App { get; set; }

    public virtual ICollection<ReceivedEmail> ReceivedEmails { get; set; }
}