using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Security;

public class User
{
    public string Id { get; set; }

    public string DefaultCultureId { get; set; }

    public string DisplayName { get; set; }

    public string Email { get; set; }

    public bool IsActive { get; set; }

    public virtual Culture DefaultCulture { get; set; }

    public virtual ICollection<UserRole> Roles { get; set; }
}





