namespace Daskata.Infrastructure.Shared
{
    public static class Constants
    {
        //Constants used in: UserProfile
        public const int UsernameLenghtMin = 3;
        public const int UsernameLenghtMax = 50;

        public const int EmailLenghtMin = 3;
        public const int EmailLenghtMax = 100;

        public const int FirstNameLenghtMin = 0;
        public const int FirstNameLenghtMax = 50;

        public const int LastNameLenghtMin = 0;
        public const int LastNameLenghtMax = 100;

        public const int PhoneNumberLenghtMin = 0;
        public const int PhoneNumberLenghtMax = 50;

        public const int LocationLenghtMin = 0;
        public const int LocationLenghtMax = 50;

        public const int SchoolLenghtMin = 0;
        public const int SchoolLenghtMax = 50;

        public const int AdditionalInfoLenghtMin = 0;
        public const int AdditionalInfoLenghtMax = 500;


        //Constants used in: UserRole
        public const int UserRoleBGNameLenghtMin = 0;
        public const int UserRoleBGNameLenghtMax = 50;


        //Constants used in: Exam
        public const int ExamTitleLenghtMin = 3;
        public const int ExamTitleLenghtMax = 100;

        public const int ExamDescriptionLenghtMin = 0;
        public const int ExamDescriptionLenghtMax = 500;

        //Constants used in: Question
        public const int QuestionTextLenghtMin = 3;
        public const int QuestionTextLenghtMax = 500;

        public const int QuestionPointsMin = 1;
        public const int QuestionPointsMax = 200;


        //Constants used in: RegisterFormModel and LoginFormModel
        public const int PasswordLenghtMin = 6;
        public const int PasswordLenghtMax = 50;


        //Constants used in: ProfileController
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Teacher = "Teacher";
        public const string Student = "Student";


        //Constants used as: Error Messages
        public const string uniqueUserGeneratedFailMessage = "Квотата от 1 000 000 регистрирани потребители е достигната. Моля обърнете се към администратор.";
        public const string signInErrorMessage = "Някъде има грешка...";
    }
}
