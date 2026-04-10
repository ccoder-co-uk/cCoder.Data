using cCoder.Data.Models.DMS;


namespace cCoder.Data.Models.Security;

public class FolderRole
{
    public Guid FolderId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Folder Folder { get; set; }

    public virtual Role Role { get; set; }
}





