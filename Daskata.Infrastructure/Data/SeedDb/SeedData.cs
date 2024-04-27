using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data.SeedDb
{
    public class SeedData
    {
        private readonly DaskataDbContext _context;
        private readonly UserManager<UserProfile> _userManager;

        public SeedData(DaskataDbContext context, UserManager<UserProfile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            await SeedUsersAsync();
            await SeedExamsAsync();
        }

        private async Task SeedUsersAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                var hasher = new PasswordHasher<UserProfile>();

                var managerUser = new UserProfile()
                {
                    Id = Guid.Parse("c1f64c15-69d2-4b2b-8a75-efaae879b40e"),
                    UserName = "user333333",
                    Email = "no@email.xyz",
                    FirstName = "Иво",
                    LastName = "Директора"
                };
                var teacherUser = new UserProfile()
                {
                    Id = Guid.Parse("84ed7e36-8256-4e35-a74c-f72254c334cc"),
                    UserName = "user222222",
                    Email = "no@email.xyz",
                    FirstName = "Ани",
                    LastName = "Учителя",
                    CreatedByUserId = managerUser.Id
                };
                var studentUser = new UserProfile()
                {
                    Id = Guid.Parse("b736c6b1-cc6e-4a5e-99b7-81e4c6a7bce5"),
                    UserName = "user111111",
                    Email = "no@email.xyz",
                    FirstName = "Иван",
                    LastName = "Ученика",
                    CreatedByUserId = managerUser.Id
                };

                await _userManager.CreateAsync(managerUser, "manager123аА@");
                await _userManager.CreateAsync(teacherUser, "teacher123аА@");
                await _userManager.CreateAsync(studentUser, "student123аА@");
            }
        }

        private async Task SeedExamsAsync()
        {
            if (!await _context.Exams.AnyAsync())
            {
                var seededExams = GetSeedExams();

                foreach (var exam in seededExams)
                {
                    await _context.Exams.AddAsync(exam);
                }
                await _context.SaveChangesAsync();
            }
        }

        private List<Exam> GetSeedExams()
        {
            var teacherUserId = Guid.Parse("84ed7e36-8256-4e35-a74c-f72254c334cc");

            return new List<Exam>
    {
        new Exam
        {
            Id = Guid.NewGuid(),
            Title = "Основи на алгебрата",
            Description = "Изпит за оценка на знанията по алгебра за средно образование.",
            Duration = TimeSpan.FromMinutes(60),
            TotalPoints = 100,
            IsPublished = true,
            IsPublic = false,
            StudySubject = SubjectCategory.Mathematics,
            StudentGrade = GradeCategory.Grade8,
            TimesPassed = 0,
            CreationDate = DateTime.UtcNow.AddDays(-30),
            LastModifiedDate = DateTime.UtcNow.AddDays(-28),
            CreatedByUserId = teacherUserId,
            ExamUrl = "111111111"
        },
        new Exam
        {
            Id = Guid.NewGuid(),
            Title = "Световна литература",
            Description = "Подробен изпит по класическа и модерна световна литература.",
            Duration = TimeSpan.FromMinutes(90),
            TotalPoints = 120,
            IsPublished = true,
            IsPublic = true,
            StudySubject = SubjectCategory.Literature,
            StudentGrade = GradeCategory.Grade10,
            TimesPassed = 0,
            CreationDate = DateTime.UtcNow.AddDays(-25),
            LastModifiedDate = DateTime.UtcNow.AddDays(-20),
            CreatedByUserId = teacherUserId,
            ExamUrl = "222222222"
        },
        new Exam
        {
            Id = Guid.NewGuid(),
            Title = "Физика: Механика",
            Description = "Оценяване на знанията по механика в раздела на физиката.",
            Duration = TimeSpan.FromMinutes(45),
            TotalPoints = 80,
            IsPublished = false,
            IsPublic = false,
            StudySubject = SubjectCategory.Physics,
            StudentGrade = GradeCategory.Grade11,
            TimesPassed = 0,
            CreationDate = DateTime.UtcNow.AddDays(-20),
            LastModifiedDate = DateTime.UtcNow.AddDays(-18),
            CreatedByUserId = teacherUserId,
            ExamUrl = "333333333"
        },
        new Exam
        {
            Id = Guid.NewGuid(),
            Title = "История на България",
            Description = "Пълноценен тест, обхващащ ключови събития в историята на България.",
            Duration = TimeSpan.FromMinutes(60),
            TotalPoints = 100,
            IsPublished = true,
            IsPublic = true,
            StudySubject = SubjectCategory.History,
            StudentGrade = GradeCategory.Grade12,
            TimesPassed = 0,
            CreationDate = DateTime.UtcNow.AddDays(-15),
            LastModifiedDate = DateTime.UtcNow.AddDays(-10),
            CreatedByUserId = teacherUserId,
            ExamUrl = "444444444"
        },
        new Exam
        {
            Id = Guid.NewGuid(),
            Title = "Биология: Човешкото тяло",
            Description = "Тест за оценка на познанията относно човешките органи и системи.",
            Duration = TimeSpan.FromMinutes(75),
            TotalPoints = 110,
            IsPublished = false,
            IsPublic = true,
            StudySubject = SubjectCategory.Biology,
            StudentGrade = GradeCategory.Grade9,
            TimesPassed = 0,
            CreationDate = DateTime.UtcNow.AddDays(-10),
            LastModifiedDate = DateTime.UtcNow.AddDays(-5),
            CreatedByUserId = teacherUserId,
            ExamUrl = "555555555"
        }
    };
        }

    }
}
