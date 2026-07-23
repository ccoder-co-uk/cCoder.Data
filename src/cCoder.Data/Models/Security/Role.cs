// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Security;

public class Role
{
    public Guid Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Privs { get; set; }

    public App App { get; set; }

    public virtual ICollection<UserRole> Users { get; set; }

    public virtual ICollection<PageRole> Pages { get; set; }

    public virtual ICollection<FolderRole> Folders { get; set; }

    public virtual ICollection<string> Privileges { get => Privs.Split(separator:","); set => Privs = string.Join(separator:',', values:value); }
}