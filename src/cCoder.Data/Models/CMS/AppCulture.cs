namespace cCoder.Data.Models.CMS;

public class AppCulture
{
    public int AppId { get; set; }

    public string CultureId { get; set; }

    public virtual App App { get; set; }

    public virtual Culture Culture { get; set; }
}


