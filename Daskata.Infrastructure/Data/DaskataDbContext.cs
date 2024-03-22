using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data
{
    public class DaskataDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
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
            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.User) 
                .WithMany(s => s.ExamAttempts) 
                .HasForeignKey(ea => ea.UserID)
                .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<ExamAttempt>()
                .HasOne(ea => ea.Exam)
                .WithMany()
                .HasForeignKey(ea => ea.ExamID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

