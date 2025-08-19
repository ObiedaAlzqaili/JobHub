using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOnResumeEntiteForAiFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnalysisResult",
                table: "Resumes",
                newName: "OriginalFileName");

            migrationBuilder.AddColumn<string>(
                name: "AiKeywords",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResumeId",
                table: "JobApplications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ResumeId",
                table: "JobApplications",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications",
                column: "ResumeId",
                principalTable: "Resumes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_ResumeId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "AiKeywords",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "Resumes",
                newName: "AnalysisResult");
        }
    }
}
