// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;

namespace cCoder.Data.Models.Eventing;

public class EventMessageRecord
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string EventName { get; set; }

    public string Data { get; set; }

    public DateTimeOffset AvailableOn { get; set; }

    public DateTimeOffset? LeaseUntil { get; set; }

    public int Attempt { get; set; }
}