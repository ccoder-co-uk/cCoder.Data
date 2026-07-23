// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace Data.Web.Models;

public sealed class DataEntitySet
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string ClrType { get; set; }

    public string Table { get; set; }

    public string[] KeyProperties { get; set; }

    public DataProperty[] Properties { get; set; }
}