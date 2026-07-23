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
            entity.ToTable(name:"LogEntries", schema:"Logging");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.AppId).IsRequired();
            entity.Property(propertyExpression:i => i.Level).IsRequired();
            entity.Property(propertyExpression:i => i.Message).IsRequired();
            entity.Property(propertyExpression:i => i.AppName).IsRequired();
            entity.Property(propertyExpression:i => i.TypeName).IsRequired();
        });

        _ = builder.Entity<LogDataItem>(buildAction:entity =>
        {
            entity.ToTable(name:"LogDataItems", schema:"Logging");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(propertyExpression:i => i.Value).IsRequired();
        });
    }

    private void ApplyLoggingFilters(ModelBuilder builder)
    {
        _ = builder.Entity<LogEntry>().HasQueryFilter(filter:log => AdminOf.Contains(value:log.AppId));
        _ = builder.Entity<LogDataItem>().HasQueryFilter(filter:item => item.LogEntry != null);
    }
}