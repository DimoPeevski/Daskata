using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionTablesCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "ConnectedAt",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserConnections");

            migrationBuilder.AlterTable(
                name: "UserConnections",
                comment: "Established relationship connection between users",
                oldComment: "Mapping table to store user connections");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Unique identifier for the relationship",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Primary key for the user connection");

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishedDate",
                table: "UserConnections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date when the relationship was established");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstUserId",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The first user in the relationship");

            migrationBuilder.AddColumn<Guid>(
                name: "SecondUserId",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "The second user in the relationship");

            migrationBuilder.CreateTable(
                name: "ConnectionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the user connection request"),
                    RequestStatus = table.Column<int>(type: "int", nullable: false, comment: "Status of the connection request ('Pending', 'Accepted', 'Rejected', 'Blocked')"),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User ID who sent the connection request"),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User ID who received the connection request")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Connection request between users");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_FirstUserId",
                table: "UserConnections",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_SecondUserId",
                table: "UserConnections",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_FromUserId",
                table: "ConnectionRequests",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_ToUserId",
                table: "ConnectionRequests",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_FirstUserId",
                table: "UserConnections",
                column: "FirstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_SecondUserId",
                table: "UserConnections",
                column: "SecondUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_FirstUserId",
                table: "UserConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_SecondUserId",
                table: "UserConnections");

            migrationBuilder.DropTable(
                name: "ConnectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_FirstUserId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_SecondUserId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "EstablishedDate",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "FirstUserId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "SecondUserId",
                table: "UserConnections");

            migrationBuilder.AlterTable(
                name: "UserConnections",
                comment: "Mapping table to store user connections",
                oldComment: "Established relationship connection between users");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Primary key for the user connection",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Unique identifier for the relationship");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConnectedAt",
                table: "UserConnections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Timestamp when the connection was established");

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectionId",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Connection ID associated with the user");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserConnections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Foreign key referencing the user");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
