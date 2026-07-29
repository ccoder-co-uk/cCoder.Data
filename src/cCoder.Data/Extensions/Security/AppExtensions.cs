// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Security;

public static class AppExtensions
{
    public static bool IsAppAdmin(this App app, User user) =>
        user?.Roles?.Any(predicate: role =>
            role.Role?.AppId == app?.Id
            && role.Role.Allows(user: user, privilege: "app_admin")) ?? false;
}