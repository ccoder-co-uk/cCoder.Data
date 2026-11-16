// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace cCoder.Data.Models.Eventing;

public sealed class SqlEventQueueReport
{
    public IReadOnlyCollection<SqlEventQueueSnapshot> Queues { get; set; }
}