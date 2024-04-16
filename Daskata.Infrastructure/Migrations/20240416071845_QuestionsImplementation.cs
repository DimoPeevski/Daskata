using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionID",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamID",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ExamID",
                table: "Questions",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ExamID",
                table: "Questions",
                newName: "IX_Questions_ExamId");

            migrationBuilder.RenameColumn(
                name: "QuestionID",
                table: "Answers",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionID",
                table: "Answers",
                newName: "IX_Answers_QuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "Text of the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Text of the question");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "Questions",
                newName: "ExamID");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ExamId",
                table: "Questions",
                newName: "IX_Questions_ExamID");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Answers",
                newName: "QuestionID");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                newName: "IX_Answers_QuestionID");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Text of the question",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "Text of the question");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionID",
                table: "Answers",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamID",
                table: "Questions",
                column: "ExamID",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
