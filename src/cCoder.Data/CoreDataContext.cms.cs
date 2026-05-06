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
        _ = builder.Entity<App>(entity =>
        {
            entity.ToTable("Apps", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.DefaultCultureId).IsRequired();
            entity.Property(i => i.Name).IsRequired();
            entity.Property(i => i.Domain).IsRequired();
            entity.Property(i => i.DefaultTheme).IsRequired();
            entity.Ignore(i => i.Config);
        });
        _ = builder.Entity<Layout>(entity =>
        {
            entity.ToTable("Layouts", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });
        _ = builder.Entity<Page>(entity =>
        {
            entity.ToTable("Pages", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });
        _ = builder.Entity<PageInfo>(entity =>
        {
            entity.ToTable("PageInfo", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.CultureId).IsRequired();
            entity.Property(i => i.Title).IsRequired();
        });
        _ = builder.Entity<Content>(entity =>
        {
            entity.ToTable("Contents", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.CultureId).IsRequired();
            entity.Property(i => i.Name).IsRequired();
            entity.HasOne(i => i.Page).WithMany(i => i.Contents).HasForeignKey(i => i.PageId);
        });
        _ = builder.Entity<Component>(entity =>
        {
            entity.ToTable("Components", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });
        _ = builder.Entity<Resource>(entity =>
        {
            entity.ToTable("Resources", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
            entity.Property(i => i.Key).IsRequired();
            entity.Property(i => i.Culture).IsRequired();
            entity.Property(i => i.DisplayName).IsRequired();
            entity.Property(i => i.ShortDisplayName).IsRequired();
        });
        _ = builder.Entity<Culture>(entity =>
        {
            entity.ToTable("Cultures", "CMS");
            entity.Property(i => i.Id).ValueGeneratedNever().IsRequired();
            entity.Property(i => i.Name).IsRequired();
        });
        _ = builder.Entity<Template>(entity =>
        {
            entity.ToTable("Templates", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });
        _ = builder.Entity<Submission>(entity =>
        {
            entity.ToTable("Submissions", "CMS");
            entity.Property(i => i.Id).ValueGeneratedNever();
            entity.Property(i => i.DataJson).IsRequired();
            entity.Ignore(i => i.Data);
        });
        _ = builder.Entity<Script>(entity =>
        {
            entity.ToTable("Scripts", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });
        _ = builder.Entity<CommonObject>(entity =>
        {
            entity.ToTable("CommonObjects", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn).HasDefaultValue(InitialAuditDefault);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
            entity.Property(i => i.Type).IsRequired();
            entity.Property(i => i.Json).IsRequired();
        });
        _ = builder.Entity<Package>(entity =>
        {
            entity.ToTable("Packages", "Packaging");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(500);
            entity.Property(i => i.Category).IsRequired().HasMaxLength(100);
            entity.Property(i => i.SourceApi).IsRequired().HasMaxLength(200);
        });
        _ = builder.Entity<PackageItem>(entity =>
        {
            entity.ToTable("PackageItems", "Packaging");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
        });
        _ = builder.Entity<AppCulture>().ToTable("AppCultures", "CMS");
        _ = builder.Entity<PageRole>().ToTable("PageRoles", "Security");
        _ = builder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles", "Security");
            entity.Property(i => i.Name).IsRequired();
            entity.Ignore(r => r.Privileges);
        });
        _ = builder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "Security");
            entity.Property(i => i.Id).ValueGeneratedNever();
            entity.Property(i => i.DefaultCultureId).IsRequired();
            entity.Property(i => i.DisplayName).IsRequired();
            entity.Property(i => i.Email).IsRequired();
        });
        _ = builder.Entity<UserRole>().ToTable("UserRoles", "Security");
        _ = builder.Entity<MetaItem>(entity =>
        {
            entity.ToTable("MetaItems", "CMS");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
        });
        _ = builder.Entity<Privilege>(entity =>
        {
            entity.ToTable("Privileges", "Security");
            entity.Property(i => i.Id).ValueGeneratedNever().HasMaxLength(200);
            entity.Property(i => i.Type).IsRequired().HasMaxLength(50);
            entity.Property(i => i.Operation).IsRequired().HasMaxLength(50);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(500);
        });
        _ = builder.Entity<AppCulture>().HasKey(i => new { i.AppId, i.CultureId });
        _ = builder.Entity<UserRole>().HasKey(i => new { i.RoleId, i.UserId });
        _ = builder.Entity<PageRole>().HasKey(i => new { i.PageId, i.RoleId });
    }

    private void ApplyCmsFilters(ModelBuilder builder)
    {
        _ = builder.Entity<Role>().HasQueryFilter(r => AdminOf.Contains(r.AppId) 
            || CurrentUserRoleIds.Contains(r.Id));

        _ = builder.Entity<UserRole>().HasQueryFilter(ur =>
            ur.UserId == AuthInfo.SSOUserId
            || AdminOf.Contains(ur.Role.AppId));

        _ = builder.Entity<User>().HasQueryFilter(u => u.Roles.Any());

        _ = builder.Entity<PageRole>().HasQueryFilter(pr =>
            CurrentUserRoleIds.Contains(pr.RoleId)
            && pr.Role != null
            && pr.Role.Privs.Contains("pagerole_read"));

        _ = builder.Entity<Page>().HasQueryFilter(p =>
            AdminOf.Contains(p.AppId)
            || p.Roles.Any(pr =>
                CurrentUserRoleIds.Contains(pr.RoleId)
                && pr.Role != null
                && pr.Role.Privs.Contains("page_read")));

        _ = builder.Entity<PageInfo>().HasQueryFilter(i => i.Page != null);
        _ = builder.Entity<Content>().HasQueryFilter(i => i.Page != null);
        _ = builder.Entity<Submission>().HasQueryFilter(s => AdminOf.Contains(s.AppId) 
            || s.App.Roles.Any(r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("submission_read")));
    }
}