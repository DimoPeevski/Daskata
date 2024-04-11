using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourNamespace;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : IdentityDbContext<UserProfile, UserRole, Guid>
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserExamResponse> UserExamResponses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }

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

            modelBuilder.Entity<UserProfile>()
                .HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserExamResponse>(entity =>
            {
                entity.HasKey(e => e.Id);

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
