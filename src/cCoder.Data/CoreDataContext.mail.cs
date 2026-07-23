// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Mail;
using cCoder.Data.Models.Security;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Data;

public partial class CoreDataContext
{
    public virtual DbSet<MailServer> MailServers { get; set; }
    public virtual DbSet<MailSender> MailSenders { get; set; }
    public virtual DbSet<MailReceiver> MailReceivers { get; set; }
    public virtual DbSet<QueuedEmail> QueuedMail { get; set; }
    public virtual DbSet<SentEmail> SentMail { get; set; }
    public virtual DbSet<ReceivedEmail> ReceivedMail { get; set; }
    public virtual DbSet<EmailSendFailure> SendFailures { get; set; }

    private static void ConfigureMailModel(ModelBuilder builder)
    {
        _ = builder.Entity<MailServer>(buildAction:entity =>
        {
            entity.ToTable(name:"MailServers", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(propertyExpression:i => i.User).IsRequired();
            entity.Property(propertyExpression:i => i.Password).IsRequired();
            entity.Property(propertyExpression:i => i.Host).IsRequired();
        });

        _ = builder.Entity<MailSender>(buildAction:entity =>
        {
            entity.ToTable(name:"MailSenders", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(i => i.ProviderName).IsRequired().HasDefaultValue(value:"Smtp");
            entity.Property(propertyExpression:i => i.User).IsRequired();
            entity.Property(propertyExpression:i => i.Password).IsRequired();
            entity.Property(propertyExpression:i => i.Host).IsRequired();
        });

        _ = builder.Entity<MailReceiver>(buildAction:entity =>
        {
            entity.ToTable(name:"MailReceivers", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Name).IsRequired();
            entity.Property(propertyExpression:i => i.ProviderName).IsRequired();
            entity.Property(propertyExpression:i => i.User).IsRequired();
            entity.Property(propertyExpression:i => i.Password).IsRequired();
            entity.Property(propertyExpression:i => i.Host).IsRequired();
            entity.Property(i => i.IsEnabled).HasDefaultValue(value:true);
        });

        _ = builder.Entity<QueuedEmail>(buildAction:entity =>
        {
            entity.ToTable(name:"QueuedEmails", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Subject).IsRequired();
            entity.Property(propertyExpression:i => i.Content).IsRequired();
            entity.Property(propertyExpression:i => i.To).IsRequired();
            entity.Property(propertyExpression:i => i.MailServerName).IsRequired();
            entity.HasOne(i => i.SentBy).WithMany().HasForeignKey(foreignKeyExpression:i => i.SentByUserId);
            entity.HasOne(i => i.MailSender).WithMany(i => i.QueuedEmails).HasForeignKey(foreignKeyExpression:i => i.MailSenderId);
        });

        _ = builder.Entity<SentEmail>(buildAction:entity =>
        {
            entity.ToTable(name:"SentEmails", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Subject).IsRequired();
            entity.Property(propertyExpression:i => i.Content).IsRequired();
            entity.Property(propertyExpression:i => i.To).IsRequired();
            entity.Property(propertyExpression:i => i.From).IsRequired();
            entity.HasOne(i => i.SentBy).WithMany().HasForeignKey(foreignKeyExpression:i => i.SentByUserId);
            entity.HasOne(i => i.MailSender).WithMany(i => i.SentEmails).HasForeignKey(foreignKeyExpression:i => i.MailSenderId);
        });

        _ = builder.Entity<ReceivedEmail>(buildAction:entity =>
        {
            entity.ToTable(name:"ReceivedEmails", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.Subject).IsRequired();
            entity.Property(propertyExpression:i => i.Content).IsRequired();
            entity.Property(propertyExpression:i => i.To).IsRequired();
            entity.Property(propertyExpression:i => i.From).IsRequired();
            entity.Property(i => i.MessageId).IsRequired(required:false);
            entity.HasOne(i => i.SentBy).WithMany().HasForeignKey(foreignKeyExpression:i => i.SentByUserId);
            entity.HasOne(i => i.MailReceiver).WithMany(i => i.ReceivedEmails).HasForeignKey(foreignKeyExpression:i => i.MailReceiverId);
        });

        _ = builder.Entity<EmailSendFailure>(buildAction:entity =>
        {
            entity.ToTable(name:"EmailSendFailures", schema:"Mail");
            entity.Property(propertyExpression:i => i.Id).ValueGeneratedOnAdd();
            entity.Property(propertyExpression:i => i.FailureReason).IsRequired();
            entity.HasOne(i => i.Email).WithMany(i => i.FailedSends).HasForeignKey(foreignKeyExpression:i => i.EmailId);
        });

        _ = builder.Ignore<Email>();
    }

    private void ApplyMailFilters(ModelBuilder builder)
    {
        _ = builder.Entity<MailServer>().HasQueryFilter(filter:server => AdminOf.Contains(value:server.AppId));
        _ = builder.Entity<MailSender>().HasQueryFilter(filter:sender => AdminOf.Contains(value:sender.AppId));
        _ = builder.Entity<MailReceiver>().HasQueryFilter(filter:receiver => AdminOf.Contains(value:receiver.AppId));
        _ = builder.Entity<SentEmail>().HasQueryFilter(filter:mail => mail.SentByUserId == User.Id || AdminOf.Contains(value:mail.AppId));
        _ = builder.Entity<ReceivedEmail>().HasQueryFilter(filter:mail => mail.SentByUserId == User.Id || AdminOf.Contains(value:mail.AppId));
        _ = builder.Entity<QueuedEmail>().HasQueryFilter(filter:mail => mail.SentByUserId == User.Id || AdminOf.Contains(value:mail.AppId));
        _ = builder.Entity<EmailSendFailure>().HasQueryFilter(filter:mail => mail.Email != null);
    }
}