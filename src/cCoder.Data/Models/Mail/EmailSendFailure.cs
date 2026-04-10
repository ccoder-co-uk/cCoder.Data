namespace cCoder.Data.Models.Mail;

public class EmailSendFailure
{
    public int Id { get; set; }

    public int EmailId { get; set; }

    public DateTimeOffset AttemptedOn { get; set; }

    public string FailureReason { get; set; }

    public virtual QueuedEmail Email { get; set; }
}





