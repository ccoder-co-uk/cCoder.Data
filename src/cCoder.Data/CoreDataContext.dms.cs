// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

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
        _ = builder.Entity<Folder>(buildAction:entity =>
        {
            entity.ToTable(name:"Folders", schema:"DMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
        });

        _ = builder.Entity<File>(buildAction:entity =>
        {
            entity.ToTable(name:"Files", schema:"DMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(propertyExpression:i => i.Path).IsRequired();
            entity.Property(propertyExpression:i => i.MimeType).IsRequired();
            entity.Property(i => i.Size).HasMaxLength(maxLength:10);
            entity.HasOne(i => i.Folder).WithMany(i => i.Files).HasForeignKey(foreignKeyExpression:i => i.FolderId);
        });

        _ = builder.Entity<FileContent>(buildAction:entity =>
        {
            entity.ToTable(name:"FileContents", schema:"DMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Size).HasMaxLength(maxLength:10);
            entity.HasOne(i => i.File).WithMany(i => i.Contents).HasForeignKey(foreignKeyExpression:i => i.FileId);
        });

        _ = builder.Entity<FolderRole>().ToTable(name:"FolderRoles", schema:"Security");
        _ = builder.Entity<FolderRole>().HasKey(keyExpression:i => new { i.FolderId, i.RoleId });
    }

    private void ApplyDmsFilters(ModelBuilder builder)
    {
        _ = builder.Entity<FolderRole>().HasQueryFilter(filter:fr =>
            CurrentUserRoleIds.Contains(value:fr.RoleId)
            && fr.Role != null
            && fr.Role.Privs.Contains(value:"folder_read"));

        _ = builder.Entity<Folder>().HasQueryFilter(filter:f =>
            f.DeletedOn == null
            && (AdminOf.Contains(value:f.AppId)
                || f.Roles.Any(predicate:fr =>
                    CurrentUserRoleIds.Contains(fr.RoleId)
                    && fr.Role != null
                    && fr.Role.Privs.Contains("folder_read"))));

        _ = builder.Entity<File>().HasQueryFilter(filter:f =>
            f.DeletedOn == null
            && (AdminOf.Contains(value:f.Folder.AppId)
                || f.Folder.Roles.Any(predicate:fr =>
                    CurrentUserRoleIds.Contains(fr.RoleId)
                    && fr.Role != null
                    && fr.Role.Privs.Contains("file_read"))));

        _ = builder.Entity<FileContent>().HasQueryFilter(filter:i => i.File != null);
    }
}