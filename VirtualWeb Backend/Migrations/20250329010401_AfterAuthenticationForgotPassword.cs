using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class AfterAuthenticationForgotPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "person",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "person",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_Email",
                table: "person",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_person_Email",
                table: "person");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "person");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "person");
        }
    }
}
