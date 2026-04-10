using cCoder.Data.Models.Security;


namespace cCoder.Data.Models.CMS;

public class Page
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public int AppId { get; set; }

    public int Order { get; set; }

    public bool ShowOnMenus { get; set; }

    public string Name { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public string Path { get; set; }

    public string ResourceKey { get; set; }

    public string Layout { get; set; }

    public virtual App App { get; set; }

    public virtual Page Parent { get; set; }

    public virtual ICollection<PageInfo> PageInfo { get; set; }

    public virtual ICollection<Page> Pages { get; set; }

    public virtual ICollection<Content> Contents { get; set; }

    public virtual ICollection<PageRole> Roles { get; set; }
}
