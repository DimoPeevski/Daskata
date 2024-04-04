using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamUrlAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Description of the exam",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Description of the exam");

            migrationBuilder.AddColumn<string>(
                name: "ExamUrl",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Unique URL for the exam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamUrl",
                table: "Exams");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Description of the exam",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Description of the exam");
        }
    }
}
