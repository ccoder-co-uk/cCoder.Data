namespace cCoder.Data.Models.Logging;

public class LogDataItem
{
    public int Id { get; set; }

    public int LogEntryId { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public virtual LogEntry LogEntry { get; set; }
}





