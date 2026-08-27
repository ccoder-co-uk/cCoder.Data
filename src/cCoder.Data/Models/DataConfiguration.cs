// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models;

public class DataConfiguration
{
    public DataConfiguration()
    {
        ConnectionString = string.Empty;
        AdminConnectionString = string.Empty;
    }

    public string ConnectionString { get; set; }

    public string AdminConnectionString { get; set; }

    public bool DebugInfo { get; set; }

    public bool LogSQL { get; set; }
}