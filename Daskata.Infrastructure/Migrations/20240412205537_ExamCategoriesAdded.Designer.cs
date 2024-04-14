﻿// <auto-generated />
using System;
using Daskata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Daskata.Infrastructure.Migrations
{
    [DbContext(typeof(DaskataDbContext))]
    [Migration("20240412205537_ExamCategoriesAdded")]
    partial class ExamCategoriesAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the answer");

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Text of the answer");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit")
                        .HasComment("Indicates if the answer is correct");

                    b.Property<Guid>("QuestionID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated question");

                    b.HasKey("Id");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answers", t =>
                        {
                            t.HasComment("Question answer");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Exam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the exam");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the user who created the exam");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time when the exam was created");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Description of the exam");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time")
                        .HasComment("Duration of the exam in minutes");

                    b.Property<string>("ExamUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Unique URL for the exam");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit")
                        .HasComment("Indicates if the exam is public and visible for user's network");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit")
                        .HasComment("Indicates if the exam is published and available for students");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time when the exam was last modified");

                    b.Property<int>("StudentGrade")
                        .HasColumnType("int")
                        .HasComment("Indicates the grade level of the student");

                    b.Property<int>("StudySubject")
                        .HasColumnType("int")
                        .HasComment("Indicates the study subject of the exam");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Title of the exam");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int")
                        .HasComment("Total points available in the exam");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Exams", t =>
                        {
                            t.HasComment("Exam to be passed");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.ExamAttempt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the exam attempt");

                    b.Property<TimeSpan>("DurationTaken")
                        .HasColumnType("time")
                        .HasComment("Duration of the exam attempt in minutes");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2")
                        .HasComment("End time of the exam attempt");

                    b.Property<Guid>("ExamID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the exam attempted");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit")
                        .HasComment("Indicates if the exam attempt is completed");

                    b.Property<int>("Score")
                        .HasColumnType("int")
                        .HasComment("Score obtained in the exam attempt");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2")
                        .HasComment("Start time of the exam attempt");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the user who attempted the exam");

                    b.HasKey("Id");

                    b.HasIndex("ExamID");

                    b.HasIndex("UserID");

                    b.ToTable("ExamAttempts", t =>
                        {
                            t.HasComment("Exam attempted by user");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the question");

                    b.Property<Guid>("ExamID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated exam");

                    b.Property<string>("Explanation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Explanation or additional information for the question");

                    b.Property<bool>("IsMultipleCorrect")
                        .HasColumnType("bit")
                        .HasComment("Indicates if multiple correct answers are allowed");

                    b.Property<int>("Points")
                        .HasColumnType("int")
                        .HasComment("Points assigned to the question");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Text of the question");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Type of the question (e.g., multiple choice, true/false)");

                    b.HasKey("Id");

                    b.HasIndex("ExamID");

                    b.ToTable("Questions", t =>
                        {
                            t.HasComment("Question in an exam");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserExamResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the user's exam response");

                    b.Property<Guid>("AnswerID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the selected answer");

                    b.Property<Guid>("AttemptID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated exam attempt");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit")
                        .HasComment("Indicates if the user's response is correct");

                    b.Property<Guid>("QuestionID")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated question");

                    b.Property<Guid?>("UserProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AnswerID");

                    b.HasIndex("AttemptID");

                    b.HasIndex("QuestionID");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserExamResponses", t =>
                        {
                            t.HasComment("Records user responses to questions");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Additional information about the user");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Property to store the user ID of the creator");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("First name of the user");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasComment("Indicates whether the user account is active or deactivated");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time of the user's last login");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Last name of the user");

                    b.Property<string>("Location")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Information about user location");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("URL for the user's profile picture");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time when the user account was registered");

                    b.Property<string>("School")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Information about user school");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", null, t =>
                        {
                            t.HasComment("Represents individual users within the system");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BGName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Gets or sets the localized name of the role in Bulgarian");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", null, t =>
                        {
                            t.HasComment("Represents a custom user role in the application");
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("UserConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the relationship");

                    b.Property<DateTime>("EstablishedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date when the relationship was established");

                    b.Property<Guid>("FirstUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("The first user in the relationship");

                    b.Property<Guid>("SecondUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("The second user in the relationship");

                    b.HasKey("Id");

                    b.HasIndex("FirstUserId");

                    b.HasIndex("SecondUserId");

                    b.ToTable("UserConnections", t =>
                        {
                            t.HasComment("Established relationship connection between users");
                        });
                });

            modelBuilder.Entity("YourNamespace.ConnectionRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the user connection request");

                    b.Property<Guid>("FromUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("User ID who sent the connection request");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("int")
                        .HasComment("Status of the connection request ('Pending', 'Accepted', 'Rejected', 'Blocked')");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("User ID who received the connection request");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("ConnectionRequests", t =>
                        {
                            t.HasComment("Connection request between users");
                        });
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Answer", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Exam", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "User")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.ExamAttempt", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.Exam", "Exam")
                        .WithMany()
                        .HasForeignKey("ExamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "User")
                        .WithMany("ExamAttempts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Exam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Question", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.Exam", "Exam")
                        .WithMany("Questions")
                        .HasForeignKey("ExamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserExamResponse", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.ExamAttempt", "ExamAttempt")
                        .WithMany()
                        .HasForeignKey("AttemptID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", null)
                        .WithMany("UserExamResponses")
                        .HasForeignKey("UserProfileId");

                    b.Navigation("Answer");

                    b.Navigation("ExamAttempt");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserProfile", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserConnection", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FirstUser");

                    b.Navigation("SecondUser");
                });

            modelBuilder.Entity("YourNamespace.ConnectionRequest", b =>
                {
                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Daskata.Infrastructure.Data.Models.UserProfile", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Exam", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Daskata.Infrastructure.Data.Models.UserProfile", b =>
                {
                    b.Navigation("ExamAttempts");

                    b.Navigation("UserExamResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
