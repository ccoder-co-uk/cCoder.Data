// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;

namespace cCoder.Data.Models.Planning;

public class ScheduledTask
{
    // keys
    public int Id { get; set; }

    public int AppId { get; set; }

    public Guid FlowId { get; set; }

    public int? ExcludedEventsCalendarId { get; set; }
    public string ExcludedEventsName { get; set; }

    // details
    public string Name { get; set; }
    public string Description { get; set; }
    public string ExecutionArgs { get; set; }
    public long ScheduleInTicks { get; set; }

    public TimeSpan Schedule => TimeSpan.FromTicks(value:ScheduleInTicks);

    // users
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    public string ExecuteAs { get; set; }
    public virtual User ExecuteAsUser { get; set; }
    // points in time
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset? LastExecuted { get; set; }
    public DateTimeOffset? NextExecution { get; set; }

    public virtual App App { get; set; }

    public virtual FlowDefinition Flow { get; set; }

    public virtual Calendar ExcludedEventsCalendar { get; set; }

}