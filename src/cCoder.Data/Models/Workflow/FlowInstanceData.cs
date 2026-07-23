// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Workflow;

public class FlowInstanceData
{
    public Guid Id { get; set; }

    public Guid FlowDefinitionId { get; set; }

    public string Name { get; set; }

    public byte[] ContextJson { get; set; }

    public string State { get; set; }

    public string ReportingComponentName { get; set; }

    public string Caller { get; set; }

    public string ContextString
    {
        get => ContextJson != null ? System.Text.Encoding.UTF8.GetString(bytes:ContextJson) : string.Empty;
        set => ContextJson = value != null ? System.Text.Encoding.UTF8.GetBytes(s:value) : null;
    }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset? End { get; set; }

    public virtual FlowDefinition FlowDefinition { get; set; }
}