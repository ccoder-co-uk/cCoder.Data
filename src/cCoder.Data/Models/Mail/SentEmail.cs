namespace cCoder.Data.Models.Mail;

/// <summary>
/// A successfully sent email
/// </summary>
public class SentEmail : Email
{
    public DateTimeOffset SentOn { get; set; }

    public string From { get; set; }
}





