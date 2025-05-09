using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class VisualMigratesFinalUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IDENTITY_ID",
                table: "Person",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDENTITY_ID",
                table: "Person");
        }
    }
}
