// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Data.Models.Planning;

public class CalendarEvent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Start { get; set; }
    public long DurationInTicks { get; set; }

    public int CalendarId { get; set; }
    public virtual Calendar Calendar { get; set; }
}