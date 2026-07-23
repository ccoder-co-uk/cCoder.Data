// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Security;


namespace cCoder.Data.Models.Mail;

public class Email
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string SentByUserId { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public string To { get; set; }

    public string CC { get; set; }

    public bool IsBodyHtml { get; set; }

    public virtual App App { get; set; }

    public virtual User SentBy { get; set; }
}