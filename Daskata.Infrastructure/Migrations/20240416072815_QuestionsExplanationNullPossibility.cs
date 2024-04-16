using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsExplanationNullPossibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Explanation or additional information for the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Explanation or additional information for the question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Explanation or additional information for the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Explanation or additional information for the question");
        }
    }
}
