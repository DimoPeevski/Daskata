using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamAttempScoreNameChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScorePercentage",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "ExamAttempts",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Score in percentages (Score/100) obtained in the exam attempt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<double>(
                name: "ScorePercentage",
                table: "ExamAttempts",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Score obtained in the exam attempt");
        }
    }
}
