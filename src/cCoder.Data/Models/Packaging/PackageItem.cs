namespace cCoder.Data.Models.Packaging;

public class PackageItem
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    // used to determine T for data
    public string Type { get; set; }

    // JSON of the object being imported
    public string Data { get; set; }

    public virtual Package Package { get; set; }

}





