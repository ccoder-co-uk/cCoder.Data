// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;

namespace cCoder.Data.Models.Security;

public static class RoleExtensions
{
    public static bool Allows(this Role role, User user, string privilege)
    {
        string normalizedPrivilege = privilege?.ToLowerInvariant() ?? string.Empty;

        return role != null
            && user?.Roles?.Any(predicate: userRole => userRole.RoleId == role.Id) == true
            && role.Privileges.Any(predicate: item => item == normalizedPrivilege);
    }
}