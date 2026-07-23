// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Mail;

/// <summary>
/// An email that lives on the sending queue
/// </summary>
public class QueuedEmail : Email
{
    public virtual ICollection<EmailSendFailure> FailedSends { get; set; }

    public string MailServerName { get; set; }

    public Guid? MailSenderId { get; set; }

    public virtual MailSender MailSender { get; set; }
}