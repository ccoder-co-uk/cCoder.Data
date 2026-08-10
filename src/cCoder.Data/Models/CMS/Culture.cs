// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Security;


namespace cCoder.Data.Models.CMS;

public class Culture
{
    public string Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<AppCulture> Apps { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<PageInfo> PageInfos { get; set; }
    public virtual ICollection<Content> PageContents { get; set; }
    public virtual ICollection<MetaItem> MetaItems { get; set; }
}