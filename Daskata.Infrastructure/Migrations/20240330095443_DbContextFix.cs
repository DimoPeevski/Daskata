using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbContextFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Role assigned to the user within the system (e.g., Admin, Manager, Teacher, Student)");
        }
    }
}
