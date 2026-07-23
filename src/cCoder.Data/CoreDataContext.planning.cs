// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Planning;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<Calendar> Calendars { get; set; }
    public virtual DbSet<CalendarEvent> Events { get; set; }
    public virtual DbSet<ScheduledTask> ScheduledTasks { get; set; }

    private static void ConfigurePlanningModel(ModelBuilder builder)
    {
        _ = builder.Entity<Calendar>(buildAction:entity =>
        {
            entity.ToTable(name:"Calendars", schema:"Planning");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
        });

        _ = builder.Entity<CalendarEvent>(buildAction:entity =>
        {
            entity.ToTable(name:"Events", schema:"Planning");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.HasOne(i => i.Calendar).WithMany(i => i.Events).HasForeignKey(foreignKeyExpression:i => i.CalendarId);
        });

        _ = builder.Entity<ScheduledTask>(buildAction:entity =>
        {
            entity.ToTable(name:"ScheduledTasks", schema:"Planning");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Ignore(propertyExpression:i => i.Schedule);
            entity.HasOne(i => i.ExecuteAsUser).WithMany().HasForeignKey(foreignKeyExpression:i => i.ExecuteAs);
        });
    }

    private void ApplyPlanningFilters(ModelBuilder builder)
    {
        _ = builder.Entity<ScheduledTask>().HasQueryFilter(filter:t =>
            AdminOf.Contains(value:t.AppId)
            || t.App.Roles.Any(predicate:r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("scheduledtask_read")));

        _ = builder.Entity<Calendar>().HasQueryFilter(filter:c =>
            AdminOf.Contains(value:c.AppId)
            || c.App.Roles.Any(predicate:r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("calendar_read")));

        _ = builder.Entity<CalendarEvent>().HasQueryFilter(filter:e => e.Calendar != null);
    }
}