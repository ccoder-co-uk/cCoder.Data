using cCoder.Data.Models.DMS;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;
using File = cCoder.Data.Models.DMS.File;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<Folder> Folders { get; set; }
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<FileContent> FileContents { get; set; }
    public virtual DbSet<FolderRole> FolderRoles { get; set; }

    private static void ConfigureDmsModel(ModelBuilder builder)
    {
        _ = builder.Entity<Folder>(entity =>
        {
            entity.ToTable("Folders", "DMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
        });
        _ = builder.Entity<File>(entity =>
        {
            entity.ToTable("Files", "DMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired();
            entity.Property(i => i.Path).IsRequired();
            entity.Property(i => i.MimeType).IsRequired();
            entity.Property(i => i.Size).HasMaxLength(10);
            entity.HasOne(i => i.Folder).WithMany(i => i.Files).HasForeignKey(i => i.FolderId);
        });
        _ = builder.Entity<FileContent>(entity =>
        {
            entity.ToTable("FileContents", "DMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Size).HasMaxLength(10);
            entity.HasOne(i => i.File).WithMany(i => i.Contents).HasForeignKey(i => i.FileId);
        });
        _ = builder.Entity<FolderRole>().ToTable("FolderRoles", "Security");
        _ = builder.Entity<FolderRole>().HasKey(i => new { i.FolderId, i.RoleId });
    }

    private void ApplyDmsFilters(ModelBuilder builder)
    {
        _ = builder.Entity<FolderRole>().HasQueryFilter(fr => fr.Role != null && fr.Role.Privs.Contains("folder_read"));
        _ = builder.Entity<Folder>().HasQueryFilter(f => f.DeletedOn == null && f.Roles.Any());
        _ = builder.Entity<File>().HasQueryFilter(f => f.DeletedOn == null && (AdminOf.Contains(f.Folder.AppId) || f.Folder != null));
        _ = builder.Entity<FileContent>().HasQueryFilter(i => i.File != null);
    }
}




