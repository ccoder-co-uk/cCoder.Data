namespace cCoder.Data.Models.CMS;

public class PageInfo
{
    public int Id { get; set; }

    public int PageId { get; set; }

    public string CultureId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Keywords { get; set; }

    public virtual Page Page { get; set; }
    public virtual Culture Culture { get; set; }
}





