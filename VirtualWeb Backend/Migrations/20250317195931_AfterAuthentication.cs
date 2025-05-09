using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class AfterAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDENTITY_ID",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "DOCUMENT_ID",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "STATUS_ID",
                table: "Applicant");

            migrationBuilder.AlterColumn<string>(
                name: "PERSON_ID",
                table: "NextOfKin",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NEXT_OF_KIN_IDENTITYID",
                table: "NextOfKin",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NEXT_OF_KIN_NAME",
                table: "NextOfKin",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APPLICATION_ID",
                table: "Document",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "APPLICANT_ID",
                table: "Application",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "DATEOFBIRTH",
                table: "Application",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FATHERID",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FATHERNAME",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FULLNAME",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GENDER",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOTHERID",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOTHERRNAME",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PHONENUMBER",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PROVINCE",
                table: "Application",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PERSON_ID",
                table: "Applicant",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                table: "Applicant",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NEXT_OF_KIN_IDENTITYID",
                table: "NextOfKin");

            migrationBuilder.DropColumn(
                name: "NEXT_OF_KIN_NAME",
                table: "NextOfKin");

            migrationBuilder.DropColumn(
                name: "DATEOFBIRTH",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FATHERID",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FATHERNAME",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FULLNAME",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "GENDER",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "MOTHERID",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "MOTHERRNAME",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PHONENUMBER",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "PROVINCE",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "Applicant");

            migrationBuilder.AlterColumn<int>(
                name: "PERSON_ID",
                table: "NextOfKin",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "APPLICATION_ID",
                table: "Document",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDENTITY_ID",
                table: "Document",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "APPLICANT_ID",
                table: "Application",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DOCUMENT_ID",
                table: "Application",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PERSON_ID",
                table: "Applicant",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "STATUS_ID",
                table: "Applicant",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
