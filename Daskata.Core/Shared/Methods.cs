using System.Text;
using System.Text.RegularExpressions;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.Shared
{
    public static class Methods
    {
        public static string GetRegistrationMonthYearAsText(string registrationDate)
        {
            DateTime date = DateTime.Parse(registrationDate);
            string rawMonth = date.ToString("MMMM", new System.Globalization.CultureInfo("bg-BG"));
            string month = char.ToUpper(rawMonth[0]) + rawMonth.Substring(1);
            string year = date.Year.ToString();

            return $"{month} {year}";
        }

        public static string GetRegistrationMonthYearAsNumbers(string registrationDate)
        {
            DateTime date = DateTime.Parse(registrationDate);
            string formattedDate = date.ToString("dd.MM.yyyy");
            return formattedDate;
        }

        public static string FormatTimeSpanAsMinutes(TimeSpan timeSpan)
        {
            int totalMinutes = (int)timeSpan.TotalMinutes;
            return $"{totalMinutes} мин";
        }

        public static string GenerateExamUrl()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString().TrimEnd();
        }


        public static string TranslateRoleInBG(string roleName)
        {
            switch (roleName)
            {
                case Admin:
                    return "⭐⭐⭐⭐ Админ";
                case Manager:
                    return "⭐⭐⭐ Мениджър";
                case Teacher:
                    return "⭐⭐ Учител";
                case Student:
                    return "⭐ Ученик";
                default:
                    return roleName;
            }
        }

        public static string TranslateExamSubjectInBG(string subjectName)
        {
            switch (subjectName)
            {
                case "Mathematics":
                    return "Математика";
                case "BulgarianLanguage":
                    return "Български език";
                case "Literature":
                    return "Литература";
                case "History":
                    return "История";
                case "Biology":
                    return "Биология";
                case "Chemistry":
                    return "Химия";
                case "Physics":
                    return "Физика";
                case "Geography":
                    return "География";
                case "EnglishLanguage":
                    return "Английски език";
                default:
                    return "Неопределен";
            }
        }
        public static string TranslateQuestionsTypeBG(string subjectName)
        {
            switch (subjectName)
            {
                case "TrueFalse":
                    return "Правилно/Грешно";
                case "Multiple":
                    return "Посочи правилното";
                default:
                    return "Неопределен";
            }
        }

        public static string GradeNumberExtract(string grade)
        {
            var match = Regex.Match(grade, @"\d+");
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
