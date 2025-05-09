using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class days : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_person",
                table: "person");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "person",
                newName: "EMAIL");

            migrationBuilder.RenameIndex(
                name: "IX_person_Email",
                table: "person",
                newName: "IX_person_EMAIL");

            migrationBuilder.AlterColumn<string>(
                name: "IDENTITY_ID",
                table: "person",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_person",
                table: "person",
                column: "PERSON_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_person",
                table: "person");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "person",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_person_EMAIL",
                table: "person",
                newName: "IX_person_Email");

            migrationBuilder.AlterColumn<string>(
                name: "IDENTITY_ID",
                table: "person",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_person",
                table: "person",
                column: "IDENTITY_ID");
        }
    }
}
