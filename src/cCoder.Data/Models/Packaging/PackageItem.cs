// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Packaging;

public class PackageItem
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    /// <summary>
    /// Gets or sets the type used to interpret the data payload.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the JSON representation of the imported object.
    /// </summary>
    public string Data { get; set; }

    public virtual Package Package { get; set; }

}