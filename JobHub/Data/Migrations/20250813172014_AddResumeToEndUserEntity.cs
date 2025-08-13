using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeToEndUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeBase64",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeBase64",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResumeName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResumeType",
                table: "AspNetUsers");
        }
    }
}
