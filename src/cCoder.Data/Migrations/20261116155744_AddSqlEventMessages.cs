using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cCoder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSqlEventMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Eventing");

            migrationBuilder.CreateTable(
                name: "EventMessages",
                schema: "Eventing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LeaseUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Attempt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedEventMessages",
                schema: "Eventing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LeaseUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Attempt = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedEventMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventMessages_AvailableOn_CreatedOn_Id",
                schema: "Eventing",
                table: "EventMessages",
                columns: new[] { "AvailableOn", "CreatedOn", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMessages",
                schema: "Eventing");

            migrationBuilder.DropTable(
                name: "FailedEventMessages",
                schema: "Eventing");
        }
    }
}
