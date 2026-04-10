using cCoder.Data.Models.Mail;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<MailServer> MailServers { get; set; }
    public virtual DbSet<QueuedEmail> QueuedMail { get; set; }
    public virtual DbSet<SentEmail> SentMail { get; set; }
    public virtual DbSet<EmailSendFailure> SendFailures { get; set; }

    private static void ConfigureMailModel(ModelBuilder builder)
    {
        _ = builder.Entity<MailServer>(entity =>
        {
            entity.ToTable("MailServers", "Mail");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Name).IsRequired();
            entity.Property(i => i.User).IsRequired();
            entity.Property(i => i.Password).IsRequired();
            entity.Property(i => i.Host).IsRequired();
        });
        _ = builder.Entity<QueuedEmail>(entity =>
        {
            entity.ToTable("QueuedEmails", "Mail");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Subject).IsRequired();
            entity.Property(i => i.Content).IsRequired();
            entity.Property(i => i.To).IsRequired();
            entity.Property(i => i.MailServerName).IsRequired();
            entity.HasOne(i => i.SentBy).WithMany().HasForeignKey(i => i.SentByUserId);
        });
        _ = builder.Entity<SentEmail>(entity =>
        {
            entity.ToTable("SentEmails", "Mail");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Subject).IsRequired();
            entity.Property(i => i.Content).IsRequired();
            entity.Property(i => i.To).IsRequired();
            entity.Property(i => i.From).IsRequired();
            entity.HasOne(i => i.SentBy).WithMany().HasForeignKey(i => i.SentByUserId);
        });
        _ = builder.Entity<EmailSendFailure>(entity =>
        {
            entity.ToTable("EmailSendFailures", "Mail");
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.FailureReason).IsRequired();
            entity.HasOne(i => i.Email).WithMany(i => i.FailedSends).HasForeignKey(i => i.EmailId);
        });
        _ = builder.Ignore<Email>();
    }

    private void ApplyMailFilters(ModelBuilder builder)
    {
        _ = builder.Entity<MailServer>().HasQueryFilter(server => AdminOf.Contains(server.AppId));
        _ = builder.Entity<SentEmail>().HasQueryFilter(mail => mail.SentByUserId == User.Id || AdminOf.Contains(mail.AppId));
        _ = builder.Entity<QueuedEmail>().HasQueryFilter(mail => mail.SentByUserId == User.Id || AdminOf.Contains(mail.AppId));
        _ = builder.Entity<EmailSendFailure>().HasQueryFilter(mail => mail.Email != null);
    }
}




