using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Security;

namespace cCoder.Data.Models.DMS;

public class Folder
{
    public Guid Id { get; set; }

    public int AppId { get; set; }

    public Guid? ParentId { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public App App { get; set; }

    public Folder Parent { get; set; }

    public ICollection<Folder> SubFolders { get; set; }

    public ICollection<File> Files { get; set; }

    public ICollection<FolderRole> Roles { get; set; }
}





