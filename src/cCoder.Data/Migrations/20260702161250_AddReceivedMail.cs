// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace cCoder.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivedMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MailSenderId",
                schema: "Mail",
                table: "SentEmails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MailReceivers",
                schema: "Mail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSSL = table.Column<bool>(type: "bit", nullable: false),
                    LastReceivedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(name:"PK_MailReceivers", columns:x => x.Id);

                    table.ForeignKey(
                        name: "FK_MailReceivers_Apps_AppId",
                        column: x => x.AppId,
                        principalSchema: "CMS",
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MailSenders",
                schema: "Mail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Smtp"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSSL = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(name:"PK_MailSenders", columns:x => x.Id);

                    table.ForeignKey(
                        name: "FK_MailSenders_Apps_AppId",
                        column: x => x.AppId,
                        principalSchema: "CMS",
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivedEmails",
                schema: "Mail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation(name:"SqlServer:Identity", value:"1, 1"),
                    ReceivedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    SentByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBodyHtml = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(name:"PK_ReceivedEmails", columns:x => x.Id);

                    table.ForeignKey(
                        name: "FK_ReceivedEmails_Apps_AppId",
                        column: x => x.AppId,
                        principalSchema: "CMS",
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_ReceivedEmails_MailReceivers_MailReceiverId",
                        column: x => x.MailReceiverId,
                        principalSchema: "Mail",
                        principalTable: "MailReceivers",
                        principalColumn: "Id");

                    table.ForeignKey(
                        name: "FK_ReceivedEmails_Users_SentByUserId",
                        column: x => x.SentByUserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Privileges",
                columns: new[] { "Id", "Description", "Operation", "PortalAdminsOnly", "Type" },
                values: new object[,]
                {
                    { "mailreceiver_create", "Allows users to Create MailReceivers.", "Create", false, "MailReceiver" },
                    { "mailreceiver_delete", "Allows users to Delete MailReceivers.", "Delete", false, "MailReceiver" },
                    { "mailreceiver_read", "Allows users to Read MailReceivers.", "Read", false, "MailReceiver" },
                    { "mailreceiver_update", "Allows users to Update MailReceivers.", "Update", false, "MailReceiver" },
                    { "mailsender_create", "Allows users to Create MailSenders.", "Create", false, "MailSender" },
                    { "mailsender_delete", "Allows users to Delete MailSenders.", "Delete", false, "MailSender" },
                    { "mailsender_read", "Allows users to Read MailSenders.", "Read", false, "MailSender" },
                    { "mailsender_update", "Allows users to Update MailSenders.", "Update", false, "MailSender" },
                    { "receivedemail_create", "Allows users to Create ReceivedEmails.", "Create", false, "ReceivedEmail" },
                    { "receivedemail_delete", "Allows users to Delete ReceivedEmails.", "Delete", false, "ReceivedEmail" },
                    { "receivedemail_read", "Allows users to Read ReceivedEmails.", "Read", false, "ReceivedEmail" },
                    { "receivedemail_update", "Allows users to Update ReceivedEmails.", "Update", false, "ReceivedEmail" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SentEmails_MailSenderId",
                schema: "Mail",
                table: "SentEmails",
                column: "MailSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MailReceivers_AppId",
                schema: "Mail",
                table: "MailReceivers",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_MailSenders_AppId",
                schema: "Mail",
                table: "MailSenders",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedEmails_AppId",
                schema: "Mail",
                table: "ReceivedEmails",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedEmails_MailReceiverId",
                schema: "Mail",
                table: "ReceivedEmails",
                column: "MailReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedEmails_SentByUserId",
                schema: "Mail",
                table: "ReceivedEmails",
                column: "SentByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SentEmails_MailSenders_MailSenderId",
                schema: "Mail",
                table: "SentEmails",
                column: "MailSenderId",
                principalSchema: "Mail",
                principalTable: "MailSenders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SentEmails_MailSenders_MailSenderId",
                schema: "Mail",
                table: "SentEmails");

            migrationBuilder.DropTable(
                name: "MailSenders",
                schema: "Mail");

            migrationBuilder.DropTable(
                name: "ReceivedEmails",
                schema: "Mail");

            migrationBuilder.DropTable(
                name: "MailReceivers",
                schema: "Mail");

            migrationBuilder.DropIndex(
                name: "IX_SentEmails_MailSenderId",
                schema: "Mail",
                table: "SentEmails");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailreceiver_create");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailreceiver_delete");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailreceiver_read");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailreceiver_update");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailsender_create");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailsender_delete");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailsender_read");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "mailsender_update");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "receivedemail_create");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "receivedemail_delete");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "receivedemail_read");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "receivedemail_update");

            migrationBuilder.DropColumn(
                name: "MailSenderId",
                schema: "Mail",
                table: "SentEmails");
        }
    }
}