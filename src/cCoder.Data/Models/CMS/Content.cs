namespace cCoder.Data.Models.CMS;

public class Content
{
    public int Id { get; set; }

    public int PageId { get; set; }

    public string CultureId { get; set; }

    public string Name { get; set; }
    public string Html { get; set; }
    public virtual Culture Culture { get; set; }
    public virtual Page Page { get; set; }
}





