using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class daysAf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "person",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_person_EMAIL",
                table: "person",
                newName: "IX_person_Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "person",
                newName: "EMAIL");

            migrationBuilder.RenameIndex(
                name: "IX_person_Email",
                table: "person",
                newName: "IX_person_EMAIL");
        }
    }
}
