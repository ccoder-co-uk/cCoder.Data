// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Security;

public static class UserExtensions
{
    public static bool IsAdminOfApp(this User user, int? appId) =>
        appId.HasValue
        && (user?.Roles?.Any(predicate: role =>
            role.Role?.AppId == appId.Value
            && user.Roles.Any(predicate: userRole =>
                userRole.RoleId == role.Role.Id)
            && role.Role.Privileges.Any(predicate: privilege =>
                privilege == "app_admin")) ?? false);

    public static bool Can(this User user, int? appId, string operation)
    {
        string normalizedOperation = operation?.ToLowerInvariant() ?? string.Empty;

        return user != null
            && ((appId.HasValue && user.Roles?.Any(predicate: role =>
                role.Role?.AppId == appId.Value
                && user.Roles.Any(predicate: userRole =>
                    userRole.RoleId == role.Role.Id)
                && role.Role.Privileges.Any(predicate: privilege =>
                    privilege == "app_admin")) == true)
                || (user.Roles?.Any(predicate: role =>
                    (!appId.HasValue || role.Role?.AppId == appId.Value)
                    && (role.Role?.Privileges?.Contains(item: normalizedOperation) ?? false)) ?? false));
    }
}