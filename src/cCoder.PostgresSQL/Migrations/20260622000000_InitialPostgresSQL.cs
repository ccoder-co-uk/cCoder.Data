using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cCoder.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "postgres");

            // This is a baseline migration for PostgreSQL.
            // Existing SQL Server migrations will be skipped.
            // New PostgreSQL-specific changes should be added here.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
