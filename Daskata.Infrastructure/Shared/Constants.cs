namespace Daskata.Infrastructure.Shared
{
    public static class Constants
    {
        //Constants uses in: UserProfile
        public const int UsernameLenghtMin = 3;
        public const int UsernameLenghtMax = 50;

        public const int EmailLenghtMin = 3;
        public const int EmailLenghtMax = 100;

        public const int FirstNameLenghtMin = 1;
        public const int FirstNameLenghtMax = 50;

        public const int LastNameLenghtMin = 1;
        public const int LastNameLenghtMax = 100;

        public const int PhoneNumberLenghtMax = 50;
        public const int AdditionalInfoLenghtMax = 500;


        //Constants uses in: UserRole
        public const int UserRoleBGNameLenghtMax = 50;


        //Constants uses in: Exam
        public const int ExamTitleLenghtMin = 3;
        public const int ExamTitleLenghtMax = 100;


        //Constants uses in: RegisterFormModel and LoginFormModel
        public const int PasswordLenghtMin = 6;
        public const int PasswordLenghtMax = 50;


        //Constants uses as: Error Messages
        public const string uniqueUserGeneratedFailMessage = "Квотата от 1 000 000 регистрирани потребители е достигната. Моля обърнете се към администратор.";
        public const string signInErrorMessage = "Някъде има грешка...";
    }
}

/*@if(User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                        {
                            < option value = "Admin" > Админ </ option >
                            < option value = "Manager" > Мениджър </ option >
                            < option value = "Teacher" > Учител </ option >
                            < option value = "Student" > Ученик </ option >
                        }
@if(User.Identity.IsAuthenticated && User.IsInRole("Manager"))
                        {
                            < option value = "Teacher" > Учител </ option >
                            < option value = "Student" > Ученик </ option >
                        }
@if(User.Identity.IsAuthenticated && User.IsInRole("Teacher"))
                        {
                            < option value = "Student" > Ученик </ option >
                        }*/