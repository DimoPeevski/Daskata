using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<Role> Admins { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<UserExamResponse> UserExamResponses { get; set; }

        public DaskataDbContext(DbContextOptions<DaskataDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Role>(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.User)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(ea => ea.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>()
                .HasOne(uer => uer.ExamAttempt)
                .WithMany()
                .HasForeignKey(uer => uer.AttemptID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserExamResponse>()
                .HasOne(uer => uer.Question)
                .WithMany()
                .HasForeignKey(uer => uer.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserExamResponse>()
                .HasOne(uer => uer.Answer)
                .WithMany()
                .HasForeignKey(uer => uer.SelectedAnswerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
