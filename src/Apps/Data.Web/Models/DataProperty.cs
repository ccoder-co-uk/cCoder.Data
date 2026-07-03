namespace Data.Web.Models;

public sealed class DataProperty
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public bool IsKey { get; set; }

    public bool IsNullable { get; set; }

    public bool CanCreate { get; set; }

    public bool CanUpdate { get; set; }

    public bool IsLongText { get; set; }
}
