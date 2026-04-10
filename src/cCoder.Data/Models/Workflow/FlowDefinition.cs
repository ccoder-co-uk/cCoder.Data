using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Workflow;

public class FlowDefinition
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public int AppId { get; set; }

    public string DefinitionJson { get; set; }

    public string ConfigJson { get; set; }

    public string ReportingComponentName { get; set; }

    public string InstanceReportingComponentName { get; set; }

    public virtual App App { get; set; }

    public virtual ICollection<FlowInstanceData> Instances { get; set; }
}






