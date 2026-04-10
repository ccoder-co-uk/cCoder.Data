namespace cCoder.Data.Models.Security;

public class Privilege
{
    public string Id { get; set; }

    public string Type { get; set; }

    public string Operation { get; set; }

    public string Description { get; set; }

    public bool PortalAdminsOnly { get; set; }
}





