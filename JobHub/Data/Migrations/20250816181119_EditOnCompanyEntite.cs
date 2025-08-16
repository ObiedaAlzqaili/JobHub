using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOnCompanyEntite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoBase64",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyLogoName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyLogoType",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoBase64",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
