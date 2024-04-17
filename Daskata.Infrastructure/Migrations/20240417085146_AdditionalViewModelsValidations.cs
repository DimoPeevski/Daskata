using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalViewModelsValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Type of the question (e.g., multiple choice, true/false)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Type of the question (e.g., multiple choice, true/false)");

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Explanation or additional information for the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Explanation or additional information for the question");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Text of the answer",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Text of the answer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Type of the question (e.g., multiple choice, true/false)",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Type of the question (e.g., multiple choice, true/false)");

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Explanation or additional information for the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Explanation or additional information for the question");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Text of the answer",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Text of the answer");
        }
    }
}
