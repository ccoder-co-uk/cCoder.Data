// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Security.Models;

namespace Data.Web.Models;

public sealed class AppConfiguration
{
    public CoreDataConfiguration CoreData { get; set; }

    public SecurityConfiguration Security { get; set; }

    public SecurityDataConfiguration SecurityData { get; set; }
}