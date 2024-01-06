using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        private readonly ISemesterService _semesterService;
        private readonly ICourseRegistrationFormService _courseRegistrationFormService;

        public App(LoginPage page, ICourseRegistrationFormService courseRegistrationFormService, ISemesterService semesterService)
        {
            InitializeComponent();

            _courseRegistrationFormService = courseRegistrationFormService;
            _semesterService = semesterService;

            MainPage = page;
        }

        private async Task UpdateCourseRegistrationFormStateToExpired()
        {
            List<CourseRegistrationForm> courseRegistrationFormList = await _courseRegistrationFormService.GetAllCourseRegistrationForm();
            foreach (CourseRegistrationForm form in courseRegistrationFormList)
            {
                Semester semester = await _semesterService.GetSemesterById(form.SemesterId);
                if (form.State != CourseRegistrationFormState.Expired && semester.EndRegistrationDate < DateTime.Now)
                {
                    form.State = CourseRegistrationFormState.Expired;
                    await _courseRegistrationFormService.UpdateCourseRegistrationForm(form.Id, form);
                }
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await UpdateCourseRegistrationFormStateToExpired();
        }
    }
}