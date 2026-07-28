// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models;

public sealed class DataConfiguration
{
    public DataConfiguration() =>
        ConnectionString = string.Empty;

    public string ConnectionString { get; set; }

    public bool DebugInfo { get; set; }

    public bool LogSQL { get; set; }
}