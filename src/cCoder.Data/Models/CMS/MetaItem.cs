namespace cCoder.Data.Models.CMS;

public class MetaItem
{
    public int Id { get; set; }

    public string CultureId { get; set; }

    public string Context { get; set; }

    public string Type { get; set; }

    public string Operation { get; set; }

    public string Content { get; set; }

    public Culture Culture { get; set; }
}





