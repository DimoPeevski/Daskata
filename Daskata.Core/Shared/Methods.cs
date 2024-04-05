using System.Text;
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
    }
}
