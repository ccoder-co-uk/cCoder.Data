using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace cCoder.Core.Migrations
{
    /// <inheritdoc />
    public partial class RestorePrivilegeCatalogue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Privileges",
                columns: new[] { "Id", "Description", "Operation", "PortalAdminsOnly", "Type" },
                values: new object[,]
                {
                    { "privilege_create", "Allows users to Create Privileges.", "Create", false, "Privilege" },
                    { "privilege_delete", "Allows users to Delete Privileges.", "Delete", false, "Privilege" },
                    { "privilege_read", "Allows users to Read Privileges.", "Read", false, "Privilege" },
                    { "privilege_update", "Allows users to Update Privileges.", "Update", false, "Privilege" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "privilege_create");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "privilege_delete");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "privilege_read");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "privilege_update");
        }
    }
}
