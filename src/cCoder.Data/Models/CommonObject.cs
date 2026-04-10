namespace cCoder.Data.Models;

public class CommonObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public int Version { get; set; }

    public string Key { get; set; }

    public string Type { get; set; }

    public string Json { get; set; }

    public string Culture { get; set; }
}





