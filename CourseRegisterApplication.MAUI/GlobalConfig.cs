namespace CourseRegisterApplication.MAUI
{
    public static class GlobalConfig
	{
        #region Base URL
        public const string USER_BASE_URL = "https://localhost:7182/api/Users/";
        public const string DEPARTMENT_BASE_URL = "https://localhost:7182/api/Departments/";
        public const string BRANCH_BASE_URL = "https://localhost:7182/api/Branches/";
        public const string STUDENT_BASE_URL = "https://localhost:7182/api/Students/";
        public const string CURRICULUM_BASE_URL = "https://localhost:7182/api/Curriculums/";
        #endregion

        public static User CurrentUser { get; set; } = new User();
	}
}
