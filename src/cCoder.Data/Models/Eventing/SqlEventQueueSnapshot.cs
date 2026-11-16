// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Eventing;

public sealed class SqlEventQueueSnapshot
{
    public string Name { get; set; }
    public long Queued { get; set; }
    public long Processing { get; set; }
    public long Failed { get; set; }
}