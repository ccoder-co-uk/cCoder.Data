// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Security;

public static class AuthorizationExtensions
{
    public static bool IsAppAdmin(this App app, User user) =>
        user?.Roles?.Any(predicate:role =>
            role.Role?.AppId == app?.Id
            && role.Role.Allows(user:user, privilege:"app_admin")) ?? false;

    public static bool Allows(this Role role, User user, string privilege)
    {
        string normalizedPrivilege = privilege?.ToLowerInvariant() ?? string.Empty;

        return role != null
            && user?.Roles?.Any(predicate:userRole => userRole.RoleId == role.Id) == true
            && role.Privileges.Any(predicate:item => item == normalizedPrivilege);
    }

    public static bool IsAdminOfApp(this User user, int? appId) =>
        appId.HasValue
        && (user?.Roles?.Any(predicate:role =>
            role.Role?.AppId == appId.Value
            && role.Role.Allows(user:user, privilege:"app_admin")) ?? false);

    public static bool Can(this User user, int? appId, string operation)
    {
        string normalizedOperation = operation?.ToLowerInvariant() ?? string.Empty;

        return user != null
            && ((appId.HasValue && user.IsAdminOfApp(appId:appId.Value))
                || (user.Roles?.Any(predicate:role =>
                    (!appId.HasValue || role.Role?.AppId == appId.Value)
                    && (role.Role?.Privileges?.Contains(item:normalizedOperation) ?? false)) ?? false));
    }
}