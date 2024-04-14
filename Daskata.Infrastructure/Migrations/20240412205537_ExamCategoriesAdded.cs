using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamCategoriesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indicates if the exam is public and visible for user's network");

            migrationBuilder.AddColumn<int>(
                name: "StudentGrade",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Indicates the grade level of the student");

            migrationBuilder.AddColumn<int>(
                name: "StudySubject",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Indicates the study subject of the exam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StudentGrade",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StudySubject",
                table: "Exams");
        }
    }
}
