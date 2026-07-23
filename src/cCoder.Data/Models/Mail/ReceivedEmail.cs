// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Mail;

/// <summary>
/// A received email persisted from a configured mailbox.
/// </summary>
public class ReceivedEmail : Email
{
    public DateTimeOffset ReceivedOn { get; set; }

    public string From { get; set; }

    public string MessageId { get; set; }

    public Guid? MailReceiverId { get; set; }

    public virtual MailReceiver MailReceiver { get; set; }
}