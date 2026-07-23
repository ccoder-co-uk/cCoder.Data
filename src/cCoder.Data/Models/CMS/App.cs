// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Dynamic;
using System.Text.Json;
using cCoder.Data.Models.DMS;
using cCoder.Data.Models.Mail;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Newtonsoft.Json;


namespace cCoder.Data.Models.CMS;

public class App
{
    public int Id { get; set; }

    public string DefaultCultureId { get; set; }

    public string TenantId { get; set; }

    public string Name { get; set; }

    public string Domain { get; set; }

    public string DefaultTheme { get; set; }

    public string ConfigJson { get; set; }

    public dynamic Config
    {
        get => JsonConvert.DeserializeObject<ExpandoObject>(
value:            ConfigJson ?? "{}",
settings:            cCoder.Data.Extensions.ObjectExtensions.GetJSONSettings());
        set => ConfigJson = value switch
        {
            null => "{}",
            JsonElement jsonElement => jsonElement.GetRawText(),
            string json => json,
            _ => value.ToJson()
        };
    }

    public virtual ICollection<AppCulture> Cultures { get; set; }
    public virtual ICollection<Page> Pages { get; set; }
    public virtual ICollection<Component> Components { get; set; }
    public virtual ICollection<Script> Scripts { get; set; }
    public virtual ICollection<Role> Roles { get; set; }
    public virtual ICollection<Template> Templates { get; set; }
    public virtual ICollection<Resource> Resources { get; set; }
    public virtual ICollection<ScheduledTask> Tasks { get; set; }
    public virtual ICollection<Calendar> Calendars { get; set; }
    public virtual ICollection<Folder> Folders { get; set; }
    public virtual ICollection<Layout> Layouts { get; set; }
    public virtual ICollection<FlowDefinition> Flows { get; set; }
    public virtual ICollection<MailServer> MailServers { get; set; }
    public virtual ICollection<MailSender> MailSenders { get; set; }
    public virtual ICollection<MailReceiver> MailReceivers { get; set; }
    public virtual ICollection<QueuedEmail> MailQueue { get; set; }
    public virtual ICollection<SentEmail> SentMail { get; set; }
    public virtual ICollection<ReceivedEmail> ReceivedMail { get; set; }
}