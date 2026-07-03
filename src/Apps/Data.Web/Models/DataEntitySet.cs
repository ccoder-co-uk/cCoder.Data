namespace Data.Web.Models;

public sealed class DataEntitySet
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string ClrType { get; set; } = string.Empty;

    public string Table { get; set; } = string.Empty;

    public string[] KeyProperties { get; set; } = [];

    public DataProperty[] Properties { get; set; } = [];
}
