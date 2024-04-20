using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourNamespace;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : IdentityDbContext<UserProfile, UserRole, Guid>
    {
        public DaskataDbContext(DbContextOptions<DaskataDbContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserExamResponse> UserExamResponses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureExamAttemptEntity(modelBuilder);
            ConfigureUserProfileEntity(modelBuilder);
            ConfigureConnectionRequestEntity(modelBuilder);
            ConfigureUserConnectionEntity(modelBuilder);
            ConfigureUserExamResponseEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureExamAttemptEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.User)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(ea => ea.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.Exam)
                .WithMany()
                .HasForeignKey(ea => ea.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureUserProfileEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureConnectionRequestEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConnectionRequest>()
                .HasOne(cr => cr.FromUser)
                .WithMany()
                .HasForeignKey(cr => cr.FromUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ConnectionRequest>()
                .HasOne(cr => cr.ToUser)
                .WithMany()
                .HasForeignKey(cr => cr.ToUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureUserConnectionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserConnection>()
                .HasOne(uc => uc.FirstUser)
                .WithMany()
                .HasForeignKey(uc => uc.FirstUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserConnection>()
                .HasOne(uc => uc.SecondUser)
                .WithMany()
                .HasForeignKey(uc => uc.SecondUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void ConfigureUserExamResponseEntity(ModelBuilder modelBuilder)
        {
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
        }
    }
}
