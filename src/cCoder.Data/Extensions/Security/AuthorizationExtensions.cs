using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Security;

public static class AuthorizationExtensions
{
    public static bool IsAppAdmin(this App app, User user) =>
        user?.Roles?.Any(role =>
            role.Role?.AppId == app?.Id
            && role.Role.Allows(user, "app_admin")) ?? false;

    public static bool Allows(this Role role, User user, string privilege)
    {
        string normalizedPrivilege = privilege?.ToLowerInvariant() ?? string.Empty;

        return role != null
            && user?.Roles?.Any(userRole => userRole.RoleId == role.Id) == true
            && role.Privileges.Any(item => item == normalizedPrivilege);
    }

    public static bool IsAdminOfApp(this User user, int? appId) =>
        appId.HasValue
        && (user?.Roles?.Any(role =>
            role.Role?.AppId == appId.Value
            && role.Role.Allows(user, "app_admin")) ?? false);

    public static bool Can(this User user, int? appId, string operation)
    {
        string normalizedOperation = operation?.ToLowerInvariant() ?? string.Empty;

        return user != null
            && ((appId.HasValue && user.IsAdminOfApp(appId.Value))
                || (user.Roles?.Any(role =>
                    (!appId.HasValue || role.Role?.AppId == appId.Value)
                    && (role.Role?.Privileges?.Contains(normalizedOperation) ?? false)) ?? false));
    }
}
