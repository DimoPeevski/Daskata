using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : IdentityDbContext<UserProfile>
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserExamResponse> UserExamResponses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DaskataDbContext(DbContextOptions<DaskataDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.User)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(ea => ea.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.Exam)
                .WithMany()
                .HasForeignKey(ea => ea.ExamID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>(entity =>
            {
                entity.HasKey(e => e.ResponseID);

                entity.HasOne(d => d.Question)
                    .WithMany()
                    .HasForeignKey(d => d.QuestionID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Answer)
                    .WithMany()
                    .HasForeignKey(d => d.AnswerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ExamAttempt)
                    .WithMany()
                    .HasForeignKey(d => d.AttemptID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
