// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Security;

public static class AppExtensions
{
    public static bool IsAppAdmin(this App app, User user) =>
        user?.Roles?.Any(predicate: role =>
            role.Role is not null
            && role.Role.AppId == app?.Id
            && user.Roles.Any(predicate: userRole =>
                userRole.RoleId == role.Role.Id)
            && role.Role.Privileges.Any(predicate: privilege =>
                privilege == "app_admin")) ?? false;
}