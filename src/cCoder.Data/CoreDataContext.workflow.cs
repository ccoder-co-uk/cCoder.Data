// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Workflow;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<WorkflowEvent> WorflowEvents { get; set; }
    public virtual DbSet<FlowDefinition> FlowDefinitions { get; set; }
    public virtual DbSet<FlowInstanceData> FlowInstances { get; set; }

    private static void ConfigureWorkflowModel(ModelBuilder builder)
    {
        _ = builder.Entity<FlowDefinition>(buildAction:entity =>
        {
            entity.ToTable("WorkFlows", "Workflow");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Description).HasMaxLength(350);
            entity.Property(i => i.LastUpdated);
            entity.Property(i => i.LastUpdatedBy).HasMaxLength(100);
            entity.Property(i => i.CreatedOn);
            entity.Property(i => i.CreatedBy).HasMaxLength(100);
        });

        _ = builder.Entity<FlowInstanceData>(buildAction:entity =>
        {
            entity.ToTable("FlowInstances", "Workflow");
            entity.Property(i => i.Id).ValueGeneratedNever();
            entity.Ignore(r => r.ContextString);
            entity.HasOne(i => i.FlowDefinition).WithMany(i => i.Instances).HasForeignKey(i => i.FlowDefinitionId);
        });

        _ = builder.Entity<WorkflowEvent>(buildAction:entity =>
        {
            entity.ToTable("WorkflowEvents", "Workflow");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.ExecuteAs).IsRequired();
            entity.HasOne(i => i.Flow).WithMany().HasForeignKey(i => i.FlowId);
            entity.HasOne(i => i.ExecuteAsUser).WithMany().HasForeignKey(i => i.ExecuteAs);
        });
    }

    private void ApplyWorkflowFilters(ModelBuilder builder)
    {
        _ = builder.Entity<FlowDefinition>().HasQueryFilter(filter:c =>
            AdminOf.Contains(c.AppId)
            || c.App.Roles.Any(r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("flowdefinition_read")));

        _ = builder.Entity<FlowInstanceData>().HasQueryFilter(filter:c => c.FlowDefinition != null);
        _ = builder.Entity<WorkflowEvent>().HasQueryFilter(filter:t => t.Flow != null);

        _ = builder.Entity<Calendar>().HasQueryFilter(filter:c =>
            AdminOf.Contains(c.AppId)
            || c.App.Roles.Any(r => CurrentUserRoleIds.Contains(r.Id) && r.Privs.Contains("calendar_read")));

        _ = builder.Entity<CalendarEvent>().HasQueryFilter(filter:e => e.Calendar != null);
    }
}