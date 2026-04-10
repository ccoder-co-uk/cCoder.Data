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
        _ = builder.Entity<Calendar>(entity =>
        {
            entity.ToTable("Calendars", "Planning");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
        });
        _ = builder.Entity<CalendarEvent>(entity =>
        {
            entity.ToTable("Events", "Planning");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.HasOne(i => i.Calendar).WithMany(i => i.Events).HasForeignKey(i => i.CalendarId);
        });
        _ = builder.Entity<ScheduledTask>(entity =>
        {
            entity.ToTable("ScheduledTasks", "Planning");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired();
            entity.Ignore(i => i.Schedule);
            entity.HasOne(i => i.ExecuteAsUser).WithMany().HasForeignKey(i => i.ExecuteAs);
        });
    }

    private void ApplyPlanningFilters(ModelBuilder builder)
    {
        _ = builder.Entity<ScheduledTask>().HasQueryFilter(t =>
            t.App.Roles.Any(r => r.Privs.Contains("scheduledtask_read")));
        _ = builder.Entity<Calendar>().HasQueryFilter(c => c.App.Roles.Any(r => r.Privs.Contains("calendar_read")));
        _ = builder.Entity<CalendarEvent>().HasQueryFilter(e => e.Calendar != null);
    }
}






