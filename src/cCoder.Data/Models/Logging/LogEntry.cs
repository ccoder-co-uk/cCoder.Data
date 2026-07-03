namespace cCoder.Data.Models.Logging;

public class LogEntry
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public int Level { get; set; }

    public string Message { get; set; }

    public string AppName { get; set; }

    public string TypeName { get; set; }

    public DateTime Date { get; set; }

    public virtual IEnumerable<LogDataItem> Data { get; set; }

    public LogEntry() { }

    public LogEntry(LoggingLevel level)
    {
        Level = (int)level;
    }
}

public enum LoggingLevel
{
    Error,
    Info,
    Warning,
    Debug
}





