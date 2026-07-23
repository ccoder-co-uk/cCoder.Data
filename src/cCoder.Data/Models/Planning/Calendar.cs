// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Planning;

public class Calendar
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual App App { get; set; }

    public virtual ICollection<CalendarEvent> Events { get; set; }
}