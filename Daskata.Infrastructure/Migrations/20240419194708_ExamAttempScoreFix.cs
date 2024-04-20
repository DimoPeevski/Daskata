using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamAttempScoreFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserID",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Exams_ExamID",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ExamAttempts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ExamID",
                table: "ExamAttempts",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempts_UserID",
                table: "ExamAttempts",
                newName: "IX_ExamAttempts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempts_ExamID",
                table: "ExamAttempts",
                newName: "IX_ExamAttempts_ExamId");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "ExamAttempts",
                type: "float",
                nullable: false,
                comment: "Score obtained in the exam attempt",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Score obtained in the exam attempt");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Exams_ExamId",
                table: "ExamAttempts",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Exams_ExamId",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ExamAttempts",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "ExamAttempts",
                newName: "ExamID");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempts",
                newName: "IX_ExamAttempts_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ExamAttempts_ExamId",
                table: "ExamAttempts",
                newName: "IX_ExamAttempts_ExamID");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "ExamAttempts",
                type: "int",
                nullable: false,
                comment: "Score obtained in the exam attempt",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Score obtained in the exam attempt");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserID",
                table: "ExamAttempts",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Exams_ExamID",
                table: "ExamAttempts",
                column: "ExamID",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
