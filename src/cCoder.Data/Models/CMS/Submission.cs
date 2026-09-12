// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using cCoder.Data.Extensions;

namespace cCoder.Data.Models.CMS;

public class Submission
{
    public Guid Id { get; set; }

    public int AppId { get; set; }

    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset LastUpdatedOn { get; set; }

    public string SourceComponent { get; set; }

    public string State { get; set; }

    public string DataJson { get; set; }

    public dynamic Data
    {
        get => ObjectExtensions.FromJson<dynamic>(value: DataJson);
        set => DataJson = value switch
        {
            null => "null",
            JsonElement jsonElement => jsonElement.GetRawText(),
            string json => json,
            _ => ObjectExtensions.ToJsonUsingDefaultSettings(value: value)
        };
    }

    public virtual App App { get; set; }
}