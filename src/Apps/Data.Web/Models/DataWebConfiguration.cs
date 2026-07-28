// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Security.Objects;

namespace Data.Web.Models;

public sealed class DataWebConfiguration
{
    public DataWebConfiguration()
    {
        Data = new DataConfiguration();
        Security = new SecurityConfiguration();
    }

    public DataConfiguration Data { get; set; }

    public SecurityConfiguration Security { get; set; }
}