using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;


namespace cCoder.Data;

public partial class CoreDataContext : DbContext
{
    private static readonly DateTimeOffset InitialAuditDefault = new(2021, 2, 19, 0, 0, 0, TimeSpan.Zero);

    public ICoreAuthInfo AuthInfo { get; set; }

    public Guid EventId { get; private set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    // Join entities
    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected Config Config { get; }

    private User user;

    private readonly ILogger log;

    public User User
    {
        get
        {
            if (user == null)
            {
                string userName = string.IsNullOrWhiteSpace(AuthInfo.SSOUserId)
                    ? "Guest"
                    : AuthInfo.SSOUserId;
                user = GetUserInformation(userName);
            }

            return user;
        }
    }

    private IEnumerable<int> AdminOf => User.Roles?
        .Where(r => r.Role.Privileges.Any(p => p == "app_admin"))
        .Select(r => r.Role.AppId) ?? Array.Empty<int>();

    private IEnumerable<Guid> CurrentUserRoleIds =>
        User.Roles?.Select(r => r.RoleId) ?? Array.Empty<Guid>();

    public CoreDataContext(
        DbContextOptions<CoreDataContext> options,
        ICoreAuthInfo auth,
        Config config,
        ILogger<CoreDataContext> log)
        : base(options)
    {
        AuthInfo = auth;
        Config = config;
        this.log = log;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        if (Config.LogSQL)
        {
            optionsBuilder.LogTo(message =>
            {
                if (message.Contains("Executing") || message.Contains("transaction"))
                    System.Diagnostics.Debug.WriteLine(message);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureModel(modelBuilder);
        ApplyFilters(modelBuilder);
        Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureModel(ModelBuilder builder)
    {
        ConfigureCmsModel(builder);
        ConfigureDmsModel(builder);
        ConfigureLoggingModel(builder);
        ConfigureMailModel(builder);
        ConfigurePlanningModel(builder);
        ConfigureWorkflowModel(builder);

        IEnumerable<global::Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey> cascadingRelationships = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (global::Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey relationship in cascadingRelationships)
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    private void ApplyFilters(ModelBuilder builder)
    {
        ApplyCmsFilters(builder);
        ApplyDmsFilters(builder);
        ApplyMailFilters(builder);
        ApplyPlanningFilters(builder);
        ApplyWorkflowFilters(builder);
    }

    private void Seed(ModelBuilder builder)
    {
        _ = builder.Entity<Culture>().HasData(Data.Cultures.Known);
        _ = builder.Entity<Privilege>().HasData(GetAllPrivileges());
    }

    public virtual void SetAuth(ICoreAuthInfo auth)
    {
        AuthInfo = auth;
        user = null;
    }

    public void Migrate() => Database.Migrate();

    private User GetUserInformation(string userName)
    {
        User loadedUser = Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == userName);

        if (string.IsNullOrWhiteSpace(userName) || userName == "Guest" || loadedUser == null)
        {
            loadedUser = new User
            {
                DefaultCultureId = string.Empty,
                IsActive = true,
                Id = "Guest",
                DisplayName = "Guest",
                Email = ""
            };
        }

        Guid[] roleIds = UserRoles
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(userRole => userRole.UserId == loadedUser.Id)
            .Select(userRole => userRole.RoleId)
            .Distinct()
            .ToArray();

        Role[] roles = Roles
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(role => roleIds.Contains(role.Id))
            .ToArray();

        loadedUser.Roles = roles.Select(r => new UserRole
        {
            UserId = loadedUser.Id,
            RoleId = r.Id,
            Role = r
        }).ToArray();

        return loadedUser;
    }
}
