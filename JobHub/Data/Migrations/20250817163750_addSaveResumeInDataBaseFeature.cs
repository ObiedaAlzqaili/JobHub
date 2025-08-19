using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSaveResumeInDataBaseFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileContentBase64",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileContentBase64",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Resumes");
        }
    }
}
