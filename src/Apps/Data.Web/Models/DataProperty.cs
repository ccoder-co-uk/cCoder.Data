// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.Models;

public sealed class DataProperty
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool IsKey { get; set; }

    public bool IsNullable { get; set; }

    public bool CanCreate { get; set; }

    public bool CanUpdate { get; set; }

    public bool IsLongText { get; set; }
}