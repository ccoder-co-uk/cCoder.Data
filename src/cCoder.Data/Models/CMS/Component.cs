namespace cCoder.Data.Models.CMS;

public class Component
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public int AppId { get; set; }

    public string ResourceKey { get; set; }
    public string Content { get; set; }
    public string Script { get; set; }

    public string Key { get; set; }
    public virtual App App { get; set; }
}





