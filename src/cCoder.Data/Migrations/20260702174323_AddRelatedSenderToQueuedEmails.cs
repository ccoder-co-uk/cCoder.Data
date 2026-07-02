using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cCoder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedSenderToQueuedEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MailSenderId",
                schema: "Mail",
                table: "QueuedEmails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueuedEmails_MailSenderId",
                schema: "Mail",
                table: "QueuedEmails",
                column: "MailSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueuedEmails_MailSenders_MailSenderId",
                schema: "Mail",
                table: "QueuedEmails",
                column: "MailSenderId",
                principalSchema: "Mail",
                principalTable: "MailSenders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueuedEmails_MailSenders_MailSenderId",
                schema: "Mail",
                table: "QueuedEmails");

            migrationBuilder.DropIndex(
                name: "IX_QueuedEmails_MailSenderId",
                schema: "Mail",
                table: "QueuedEmails");

            migrationBuilder.DropColumn(
                name: "MailSenderId",
                schema: "Mail",
                table: "QueuedEmails");
        }
    }
}
