// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Eventing;
using Microsoft.EntityFrameworkCore;

namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<EventMessageRecord> EventMessages { get; set; }

    public virtual DbSet<FailedEventMessageRecord> FailedEventMessages { get; set; }

    private static void ConfigureEventingModel(ModelBuilder builder)
    {
        builder.Entity<EventMessageRecord>(entity =>
        {
            entity.ToTable("EventMessages", "Eventing");
            entity.HasKey(message => message.Id);
            entity.Property(message => message.Id).ValueGeneratedNever();
            entity.Property(message => message.EventName).IsRequired().HasMaxLength(100);
            entity.Property(message => message.Data).IsRequired().HasColumnType("nvarchar(max)");
            entity.HasIndex(message => new { message.AvailableOn, message.CreatedOn, message.Id });
        });

        builder.Entity<FailedEventMessageRecord>(entity =>
        {
            entity.ToTable("FailedEventMessages", "Eventing");
            entity.HasKey(message => message.Id);
            entity.Property(message => message.Id).ValueGeneratedNever();
            entity.Property(message => message.EventName).IsRequired().HasMaxLength(100);
            entity.Property(message => message.Data).IsRequired().HasColumnType("nvarchar(max)");
            entity.Property(message => message.FailureReason).IsRequired().HasColumnType("nvarchar(max)");
        });
    }
}
