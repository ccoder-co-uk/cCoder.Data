// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.Models;

public sealed class DataRows
{
    public string EntitySet { get; set; } = string.Empty;

    public int Skip { get; set; }

    public int Take { get; set; }

    public Dictionary<string, object>[] Rows { get; set; } = [];
}