// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Security;

public class PageRole
{
    public int PageId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Page Page { get; set; }

    public virtual Role Role { get; set; }
}