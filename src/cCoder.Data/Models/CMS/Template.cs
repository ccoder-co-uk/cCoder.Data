namespace cCoder.Data.Models.CMS;

public class Template
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public string ResourceKey { get; set; }

    public string RawString { get; set; }

    public int AppId { get; set; }

    public virtual App App { get; set; }
}

