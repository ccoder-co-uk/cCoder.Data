using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cCoder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApiMetadataPrivilege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "Privileges",
                columns: new[] { "Id", "Description", "Operation", "PortalAdminsOnly", "Type" },
                values: new object[] { "api_metadata_read", "Allows users to read API metadata.", "Read", false, "ApiMetadata" });

            migrationBuilder.Sql(
                sql: """
                    UPDATE [Security].[Roles]
                    SET [Privs] = CONCAT([Privs],
                        CASE WHEN NULLIF([Privs], '') IS NULL THEN '' ELSE ',' END,
                        'api_metadata_read')
                    WHERE CONCAT(',', [Privs], ',') LIKE '%,app_admin,%'
                      AND CONCAT(',', [Privs], ',') NOT LIKE '%,api_metadata_read,%';
                    """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: """
                    UPDATE [Security].[Roles]
                    SET [Privs] = (
                        SELECT STRING_AGG([value], ',')
                        FROM STRING_SPLIT([Privs], ',')
                        WHERE [value] <> 'api_metadata_read')
                    WHERE CONCAT(',', [Privs], ',') LIKE '%,api_metadata_read,%';
                    """);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Privileges",
                keyColumn: "Id",
                keyValue: "api_metadata_read");
        }
    }
}