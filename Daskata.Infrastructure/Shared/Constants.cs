﻿namespace Daskata.Infrastructure.Shared
{
    public static class Constants
    {
        //Constants uses in: User
        public const int UsernameLenghtMin = 3;
        public const int UsernameLenghtMax = 50;

        public const int EmailLenghtMin = 3;
        public const int EmailLenghtMax = 100;

        
        public const int FirstNameLenghtMax = 50;
        public const int LastNameLenghtMax = 100;

        public const int PhoneNumberLenghtMax = 50;
        public const int AdditionalInfoLenghtMax = 500;


        //Constants uses in: Exam
        public const int ExamTitleLenghtMin = 3;
        public const int ExamTitleLenghtMax = 100;


        //Constants uses in: RegisterFormModel
        public const int PasswordLenghtMin = 6;
        public const int PasswordLenghtMax = 50;
    }
}
