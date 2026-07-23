// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Security;


namespace cCoder.Data.Models;

public interface IAmRoleSecured<TRole>
{
    ICollection<TRole> Roles { get; set; }

    bool UserCan(User user, string priv);
}