// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;


namespace cCoder.Data;

public partial class CoreDataContext : DbContext
{
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
                string userName = string.IsNullOrWhiteSpace(value:AuthInfo.SSOUserId)
                    ? "Guest"
                    : AuthInfo.SSOUserId;
                user = GetUserInformation(userName:userName);
            }

            return user;
        }
    }

    private IEnumerable<int> AdminOf => User.Roles?
        .Where(predicate:r => r.Role.Privileges.Any(p => p == "app_admin"))
        .Select(selector:r => r.Role.AppId) ?? Array.Empty<int>();

    private IEnumerable<Guid> CurrentUserRoleIds =>
        User.Roles?.Select(selector:r => r.RoleId) ?? Array.Empty<Guid>();

    public CoreDataContext(
        ICoreAuthInfo auth,
        Config config,
        ILogger<CoreDataContext> log)
    {
        AuthInfo = auth;
        Config = config;
        this.log = log;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString:Config.ConnectionStrings["Core"]);
        optionsBuilder.ConfigureWarnings(warningsConfigurationBuilderAction:warnings => warnings.Ignore(eventIds:RelationalEventId.PendingModelChangesWarning));

        if (Config.LogSQL)
        {
            optionsBuilder.LogTo(action:message =>
            {
                if (message.Contains(value:"Executing") || message.Contains(value:"transaction"))
                    System.Diagnostics.Debug.WriteLine(message:message);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureModel(builder:modelBuilder);
        ApplyFilters(builder:modelBuilder);
        Seed(builder:modelBuilder);
        base.OnModelCreating(modelBuilder:modelBuilder);
    }

    private static void ConfigureModel(ModelBuilder builder)
    {
        builder.UseIdentityColumns();

        ConfigureCmsModel(builder:builder);
        ConfigureDmsModel(builder:builder);
        ConfigureLoggingModel(builder:builder);
        ConfigureMailModel(builder:builder);
        ConfigurePlanningModel(builder:builder);
        ConfigureWorkflowModel(builder:builder);

        IEnumerable<global::Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey> cascadingRelationships = builder.Model.GetEntityTypes()
            .SelectMany(selector:t => t.GetForeignKeys())
            .Where(predicate:fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (global::Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey relationship in cascadingRelationships)
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    private void ApplyFilters(ModelBuilder builder)
    {
        ApplyCmsFilters(builder:builder);
        ApplyDmsFilters(builder:builder);
        ApplyLoggingFilters(builder:builder);
        ApplyMailFilters(builder:builder);
        ApplyPlanningFilters(builder:builder);
        ApplyWorkflowFilters(builder:builder);
    }

    private void Seed(ModelBuilder builder)
    {
        _ = builder.Entity<Culture>().HasData(data:Data.Cultures.Known);
        _ = builder.Entity<Privilege>().HasData(data:GetAllPrivileges());
    }

    public virtual void SetAuth(ICoreAuthInfo auth)
    {
        AuthInfo = auth;
        user = null;
    }

    public void Migrate() =>
        Database.Migrate();

    private User GetUserInformation(string userName)
    {
        User loadedUser = Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefault(predicate:u => u.Id == userName);

        if (string.IsNullOrWhiteSpace(value:userName) || userName == "Guest" || loadedUser == null)
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
            .Where(predicate:userRole => userRole.UserId == loadedUser.Id)
            .Select(selector:userRole => userRole.RoleId)
            .Distinct()
            .ToArray();

        Role[] roles = Roles
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(predicate:role => roleIds.Contains(value:role.Id))
            .ToArray();

        loadedUser.Roles = roles.Select(selector:r => new UserRole
        {
            UserId = loadedUser.Id,
            RoleId = r.Id,
            Role = r
        }).ToArray();

        return loadedUser;
    }
}