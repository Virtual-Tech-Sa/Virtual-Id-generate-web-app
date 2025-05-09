using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VID.Migrations
{
    /// <inheritdoc />
    public partial class VisualMigratesFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicant",
                columns: table => new
                {
                    APPLICANT_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PERSON_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_PHONE_NUMBER = table.Column<string>(type: "text", nullable: true),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant", x => x.APPLICANT_ID);
                });

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    APPLICATION_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    APPLICANT_ID = table.Column<int>(type: "integer", nullable: false),
                    DOCUMENT_ID = table.Column<int>(type: "integer", nullable: false),
                    NATIONALITY = table.Column<string>(type: "text", nullable: false),
                    CITIZENSHIP = table.Column<string>(type: "text", nullable: false),
                    STATUS = table.Column<string>(type: "text", nullable: true),
                    COUNTRYOFBIRTH = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.APPLICATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DOCUMENT_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    APPLICATION_ID = table.Column<int>(type: "integer", nullable: false),
                    IDENTITY_ID = table.Column<int>(type: "integer", nullable: true),
                    DOCUMENT_CODE = table.Column<string>(type: "text", nullable: true),
                    DOCUMENT_BIRTHCERTIFICATE = table.Column<byte[]>(type: "bytea", nullable: true),
                    DOCUMENT_IDENTITY_NUMBER_COPY = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DOCUMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "NextOfKin",
                columns: table => new
                {
                    NEXT_OF_KIN_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PERSON_ID = table.Column<int>(type: "integer", nullable: true),
                    RELATIONSHIP = table.Column<string>(type: "text", nullable: true),
                    NEXT_OF_KIN_PHONENUMBER = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextOfKin", x => x.NEXT_OF_KIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PERSON_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FIRSTNAME = table.Column<string>(type: "text", nullable: true),
                    SURNAME = table.Column<string>(type: "text", nullable: true),
                    DATEOFBIRTH = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    USER_PASSWORD = table.Column<string>(type: "text", nullable: true),
                    GENDER = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PERSON_ID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS_NAME = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.STATUS_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicant");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "NextOfKin");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
