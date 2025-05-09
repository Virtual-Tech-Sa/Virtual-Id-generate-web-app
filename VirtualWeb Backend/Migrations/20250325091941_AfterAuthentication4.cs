using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class AfterAuthentication4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DATEOFBIRTH",
                table: "Application",
                newName: "DOB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "Application",
                newName: "DATEOFBIRTH");
        }
    }
}
