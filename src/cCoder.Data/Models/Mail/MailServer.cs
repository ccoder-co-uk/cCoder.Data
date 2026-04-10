using cCoder.Data.Models.CMS;


namespace cCoder.Data.Models.Mail;

public class MailServer
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; }

    public string User { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public string FromEmail { get; set; }

    public int Port { get; set; }

    public bool EnableSSL { get; set; }

    public virtual App App { get; set; }
}





