// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Security;

public class UserRole
{
    public Guid RoleId { get; set; }

    public string UserId { get; set; }

    public virtual User User { get; set; }

    public virtual Role Role { get; set; }
}