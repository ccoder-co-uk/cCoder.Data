// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using Newtonsoft.Json;


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
        get => JsonConvert.DeserializeObject<dynamic>(
value:            DataJson,
settings:            cCoder.Data.Extensions.ObjectExtensions.GetJSONSettings());
        set => DataJson = value switch
        {
            null => "null",
            JsonElement jsonElement => jsonElement.GetRawText(),
            string json => json,
            _ => JsonConvert.SerializeObject(value)
        };
    }

    public virtual App App { get; set; }
}