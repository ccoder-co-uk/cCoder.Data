// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<Layout> Layouts { get; set; }
    public virtual DbSet<App> Apps { get; set; }
    public virtual DbSet<Page> Pages { get; set; }
    public virtual DbSet<PageInfo> PageInfo { get; set; }
    public virtual DbSet<Content> Contents { get; set; }
    public virtual DbSet<Component> Components { get; set; }
    public virtual DbSet<Resource> Resources { get; set; }
    public virtual DbSet<Culture> Cultures { get; set; }
    public virtual DbSet<Template> Templates { get; set; }
    public virtual DbSet<Submission> Submissions { get; set; }
    public virtual DbSet<Script> Scripts { get; set; }
    public virtual DbSet<CommonObject> CommonObjects { get; set; }
    public virtual DbSet<Package> Packages { get; set; }
    public virtual DbSet<PackageItem> PackageItems { get; set; }
    public virtual DbSet<AppCulture> AppCultures { get; set; }
    public virtual DbSet<PageRole> PageRoles { get; set; }

    private static void ConfigureCmsModel(ModelBuilder builder)
    {
        _ = builder.Entity<App>(buildAction:entity =>
        {
            entity.ToTable(name:"Apps", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.DefaultCultureId).IsRequired();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(propertyExpression:i => i.Domain).IsRequired();
            entity.Property(propertyExpression:i => i.DefaultTheme).IsRequired();
            entity.Ignore(propertyExpression:i => i.Config);
        });

        _ = builder.Entity<Layout>(buildAction:entity =>
        {
            entity.ToTable(name:"Layouts", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
        });

        _ = builder.Entity<Page>(buildAction:entity =>
        {
            entity.ToTable(name:"Pages", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
        });

        _ = builder.Entity<PageInfo>(buildAction:entity =>
        {
            entity.ToTable(name:"PageInfo", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.CultureId).IsRequired();
            entity.Property(propertyExpression:i => i.Title).IsRequired();
        });

        _ = builder.Entity<Content>(buildAction:entity =>
        {
            entity.ToTable(name:"Contents", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.CultureId).IsRequired();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.HasOne(i => i.Page).WithMany(i => i.Contents).HasForeignKey(foreignKeyExpression:i => i.PageId);
        });

        _ = builder.Entity<Component>(buildAction:entity =>
        {
            entity.ToTable(name:"Components", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
        });

        _ = builder.Entity<Resource>(buildAction:entity =>
        {
            entity.ToTable(name:"Resources", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.Key).IsRequired();
            entity.Property(propertyExpression:i => i.Culture).IsRequired();
            entity.Property(propertyExpression:i => i.DisplayName).IsRequired();
            entity.Property(propertyExpression:i => i.ShortDisplayName).IsRequired();
        });

        _ = builder.Entity<Culture>(buildAction:entity =>
        {
            entity.ToTable(name:"Cultures", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedNever().IsRequired();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
        });

        _ = builder.Entity<Template>(buildAction:entity =>
        {
            entity.ToTable(name:"Templates", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
        });

        _ = builder.Entity<Submission>(buildAction:entity =>
        {
            entity.ToTable(name:"Submissions", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedNever();
            entity.Property(propertyExpression:i => i.DataJson).IsRequired();
            entity.Ignore(propertyExpression:i => i.Data);
        });

        _ = builder.Entity<Script>(buildAction:entity =>
        {
            entity.ToTable(name:"Scripts", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
        });

        _ = builder.Entity<CommonObject>(buildAction:entity =>
        {
            entity.ToTable(name:"CommonObjects", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).HasMaxLength(maxLength:350);
            entity.Property(propertyExpression:i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(maxLength:100);
            entity.Property(propertyExpression:i => i.Type).IsRequired();
            entity.Property(propertyExpression:i => i.Json).IsRequired();
        });

        _ = builder.Entity<Package>(buildAction:entity =>
        {
            entity.ToTable(name:"Packages", schema:"Packaging");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(maxLength:500);
            entity.Property(i => i.Category).IsRequired().HasMaxLength(maxLength:100);
            entity.Property(i => i.SourceApi).IsRequired().HasMaxLength(maxLength:200);
        });

        _ = builder.Entity<PackageItem>(buildAction:entity =>
        {
            entity.ToTable(name:"PackageItems", schema:"Packaging");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
        });

        _ = builder.Entity<AppCulture>().ToTable(name:"AppCultures", schema:"CMS");
        _ = builder.Entity<PageRole>().ToTable(name:"PageRoles", schema:"Security");

        _ = builder.Entity<Role>(buildAction:entity =>
        {
            entity.ToTable(name:"Roles", schema:"Security");
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Ignore(propertyExpression:r => r.Privileges);
        });

        _ = builder.Entity<User>(buildAction:entity =>
        {
            entity.ToTable(name:"Users", schema:"Security");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedNever();
            entity.Property(propertyExpression:i => i.DefaultCultureId).IsRequired();
            entity.Property(propertyExpression:i => i.DisplayName).IsRequired();
            entity.Property(propertyExpression:i => i.Email).IsRequired();
        });

        _ = builder.Entity<UserRole>().ToTable(name:"UserRoles", schema:"Security");

        _ = builder.Entity<MetaItem>(buildAction:entity =>
        {
            entity.ToTable(name:"MetaItems", schema:"CMS");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
        });

        _ = builder.Entity<Privilege>(buildAction:entity =>
        {
            entity.ToTable(name:"Privileges", schema:"Security");
            entity.Property(i => i.Id).ValueGeneratedNever().HasMaxLength(maxLength:200);
            entity.Property(i => i.Type).IsRequired().HasMaxLength(maxLength:50);
            entity.Property(i => i.Operation).IsRequired().HasMaxLength(maxLength:50);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(maxLength:500);
        });

        _ = builder.Entity<AppCulture>().HasKey(keyExpression:i => new { i.AppId, i.CultureId });
        _ = builder.Entity<UserRole>().HasKey(keyExpression:i => new { i.RoleId, i.UserId });
        _ = builder.Entity<PageRole>().HasKey(keyExpression:i => new { i.PageId, i.RoleId });
    }

    private void ApplyCmsFilters(ModelBuilder builder)
    {
        _ = builder.Entity<Role>().HasQueryFilter(filter:r => AdminOf.Contains(value:r.AppId)
            || CurrentUserRoleIds.Contains(value:r.Id));

        _ = builder.Entity<UserRole>().HasQueryFilter(filter:ur =>
            ur.UserId == AuthInfo.SSOUserId
            || AdminOf.Contains(value:ur.Role.AppId));

        _ = builder.Entity<User>().HasQueryFilter(filter:u => u.Roles.Any());

        _ = builder.Entity<PageRole>().HasQueryFilter(filter:pr =>
            CurrentUserRoleIds.Contains(value:pr.RoleId)
            && pr.Role != null
            && pr.Role.Privs.Contains(value:"pagerole_read"));

        _ = builder.Entity<Page>().HasQueryFilter(filter:p =>
            AdminOf.Contains(value:p.AppId)
            || p.Roles.Any(predicate:pr =>
                CurrentUserRoleIds.Contains(pr.RoleId)
                && pr.Role != null
                && pr.Role.Privs.Contains("page_read")));

        _ = builder.Entity<PageInfo>().HasQueryFilter(filter:i => i.Page != null);
        _ = builder.Entity<Content>().HasQueryFilter(filter:i => i.Page != null);

        _ = builder.Entity<Submission>().HasQueryFilter(filter:s => AdminOf.Contains(value:s.AppId)
            || s.App.Roles.Any(predicate:r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("submission_read")));
    }
}