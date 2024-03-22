using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for each user")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique username for authentication and identification"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Hashed representation of the user's password for security"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Email address of the user for communication and verification purposes"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "First name of the user"),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Last name of the user"),
                    Role = table.Column<int>(type: "int", nullable: false, comment: "Role assigned to the user within the system (e.g., Admin, Teacher, Student)"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the user account was registered"),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time of the user's last login"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates whether the user account is active or deactivated"),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "URL for the user's profile picture"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "User phone number"),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Additional information about the user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                },
                comment: "Represents individual users within the system");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the admin")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminID);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "App administator");

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the exam")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Title of the exam"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Description of the exam"),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false, comment: "Duration of the exam in minutes"),
                    TotalPoints = table.Column<int>(type: "int", nullable: false, comment: "otal points available in the exam"),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if the exam is published and available for students"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the exam was created"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the exam was last modified"),
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the user who created the exam")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamID);
                    table.ForeignKey(
                        name: "FK_Exams_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Exam to be passed");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the student")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "User with status - Student");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the teacher")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherID);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "User with status - Teacher");

            migrationBuilder.CreateTable(
                name: "ExamAttempts",
                columns: table => new
                {
                    AttemptID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the exam attempt")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Start time of the exam attempt"),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "End time of the exam attempt"),
                    DurationTaken = table.Column<int>(type: "int", nullable: false, comment: "Duration of the exam attempt in minutes"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if the exam attempt is completed"),
                    Score = table.Column<int>(type: "int", nullable: false, comment: "Score obtained in the exam attempt"),
                    ExamID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the exam attempted"),
                    UserID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the user who attempted the exam")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAttempts", x => x.AttemptID);
                    table.ForeignKey(
                        name: "FK_ExamAttempts_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAttempts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Exam attempted by user");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the question")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Text of the question"),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Type of the question (e.g., multiple choice, true/false)"),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Explanation or additional information for the question"),
                    Points = table.Column<int>(type: "int", nullable: false, comment: "Points assigned to the question"),
                    IsMultipleCorrect = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if multiple correct answers are allowed"),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, comment: "Order index for sorting questions within an exam"),
                    ExamID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated exam")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_Questions_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Question in an exam");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the answer")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Text of the answer"),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if the answer is correct"),
                    QuestionID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated question")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerID);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Question answer");

            migrationBuilder.CreateTable(
                name: "UserExamResponses",
                columns: table => new
                {
                    ResponseID = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the user's exam response")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if the user's response is correct"),
                    AttemptID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated exam attempt"),
                    QuestionID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the associated question"),
                    SelectedAnswerID = table.Column<int>(type: "int", nullable: false, comment: "Foreign key referencing the selected answer"),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExamResponses", x => x.ResponseID);
                    table.ForeignKey(
                        name: "FK_UserExamResponses_Answers_SelectedAnswerID",
                        column: x => x.SelectedAnswerID,
                        principalTable: "Answers",
                        principalColumn: "AnswerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExamResponses_ExamAttempts_AttemptID",
                        column: x => x.AttemptID,
                        principalTable: "ExamAttempts",
                        principalColumn: "AttemptID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExamResponses_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExamResponses_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                },
                comment: "Records user responses to questions");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserID",
                table: "Admins",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionID",
                table: "Answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_ExamID",
                table: "ExamAttempts",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_UserID",
                table: "ExamAttempts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UserID",
                table: "Exams",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamID",
                table: "Questions",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserID",
                table: "Students",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserID",
                table: "Teachers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExamResponses_AttemptID",
                table: "UserExamResponses",
                column: "AttemptID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamResponses_QuestionID",
                table: "UserExamResponses",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamResponses_SelectedAnswerID",
                table: "UserExamResponses",
                column: "SelectedAnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamResponses_UserID",
                table: "UserExamResponses",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "UserExamResponses");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "ExamAttempts");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
