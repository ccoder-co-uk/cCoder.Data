// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Logging;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<LogEntry> Logs { get; set; }
    public virtual DbSet<LogDataItem> LogData { get; set; }

    private static void ConfigureLoggingModel(ModelBuilder builder)
    {
        _ = builder.Entity<LogEntry>(buildAction:entity =>
        {
            entity.ToTable("LogEntries", "Logging");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.AppId).IsRequired();
            entity.Property(i => i.Level).IsRequired();
            entity.Property(i => i.Message).IsRequired();
            entity.Property(i => i.AppName).IsRequired();
            entity.Property(i => i.TypeName).IsRequired();
        });

        _ = builder.Entity<LogDataItem>(buildAction:entity =>
        {
            entity.ToTable("LogDataItems", "Logging");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired();
            entity.Property(i => i.Value).IsRequired();
        });
    }

    private void ApplyLoggingFilters(ModelBuilder builder)
    {
        _ = builder.Entity<LogEntry>().HasQueryFilter(filter:log => AdminOf.Contains(log.AppId));
        _ = builder.Entity<LogDataItem>().HasQueryFilter(filter:item => item.LogEntry != null);
    }
}