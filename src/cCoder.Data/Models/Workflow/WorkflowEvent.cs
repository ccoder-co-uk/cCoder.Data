using cCoder.Data.Models.Security;


namespace cCoder.Data.Models.Workflow;

public class WorkflowEvent
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string EventContext { get; set; }

    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }

    public Guid FlowId { get; set; }

    public virtual FlowDefinition Flow { get; set; }

    public string ExecuteAs { get; set; }

    public virtual User ExecuteAsUser { get; set; }
}
