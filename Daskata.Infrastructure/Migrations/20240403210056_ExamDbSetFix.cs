using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExamDbSetFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_UserID",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Exams",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_UserID",
                table: "Exams",
                newName: "IX_Exams_CreatedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Description of the exam",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Description of the exam");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_CreatedByUserId",
                table: "Exams",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_CreatedByUserId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Exams",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_CreatedByUserId",
                table: "Exams",
                newName: "IX_Exams_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Description of the exam",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Description of the exam");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_UserID",
                table: "Exams",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
