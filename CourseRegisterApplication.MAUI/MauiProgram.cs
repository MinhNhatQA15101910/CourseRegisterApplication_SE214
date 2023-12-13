using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.ViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
using CourseRegisterApplication.MAUI.Views.AdminViews;
using CourseRegisterApplication.MAUI.Views.StudentViews;

namespace CourseRegisterApplication.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
					fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
					fonts.AddFont("Roboto-Thin.ttf", "RobotoThin");
					fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
				});

#if DEBUG
		builder.Logging.AddDebug();
#endif

            // ViewModels
            builder.Services.AddScoped<AddUpdateDepartmentViewModel>();
            builder.Services.AddScoped<StudentAccountManagementViewModel>();
            builder.Services.AddScoped<AddStudentViewModel>();
			builder.Services.AddScoped<LoginViewModel>();
			builder.Services.AddScoped<AdminAppShellViewModel>();
			builder.Services.AddScoped<AdminDashboardViewModel>();
			builder.Services.AddScoped<AdminAccountantAccountManagementViewModel>();
			builder.Services.AddScoped<ChangePasswordViewModel>();
			builder.Services.AddScoped<AddAdminAccountantAccountViewModel>();
			builder.Services.AddScoped<StudentAppShellViewModel>();
			builder.Services.AddScoped<StudentDashboardViewModel>();
			builder.Services.AddScoped<StudentInfomationViewModel>();
			builder.Services.AddScoped<AccountantAppShellViewModel>();
			builder.Services.AddScoped<AccountantDashboardViewModel>();
			builder.Services.AddScoped<BranchManagementViewModel>();
			builder.Services.AddScoped<CourseManagementViewModel>();
			builder.Services.AddScoped<CreditsSubjectTypeViewModel>();
			builder.Services.AddScoped<DepartmentManagementViewModel>();
			builder.Services.AddScoped<OpenSubjectViewModel>();
			builder.Services.AddScoped<PriorityObjectViewModel>();
			builder.Services.AddScoped<ProvinceDistrictViewModel>();
			builder.Services.AddScoped<StatisticViewModel>();
			builder.Services.AddScoped<StudentManagementViewModel>();
			builder.Services.AddScoped<StudyProgramViewModel>();
			builder.Services.AddScoped<SubjectManagementViewModel>();
			builder.Services.AddScoped<TuitionCollectionViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddScoped<AccountantAppShell>();
			builder.Services.AddScoped<StudentDashboardPage>();
			builder.Services.AddScoped<StudentInfomationPage>();
			builder.Services.AddScoped<AccountantDashboardPage>();
			builder.Services.AddScoped<BranchManagementPage>();
			builder.Services.AddScoped<CourseManagementPage>();
			builder.Services.AddScoped<CreditsSubjectTypePage>();
			builder.Services.AddScoped<DepantmentManagementPage>();
			builder.Services.AddScoped<OpenSubjectPage>();
			builder.Services.AddScoped<PriorityObjectPage>();
			builder.Services.AddScoped<ProvinceDistrictPage>();
			builder.Services.AddScoped<StatisticPage>();
			builder.Services.AddScoped<StudyProgramPage>();
			builder.Services.AddScoped<SubjectManagementPage>();
			builder.Services.AddScoped<TuitionCollectionPage>();
			builder.Services.AddScoped<ManagerAccountFilterPopup>();
			builder.Services.AddScoped<AdminAccountantAccountManagementPage>();
			builder.Services.AddScoped<ChangePasswordPage>();
			builder.Services.AddScoped<AddAdminAccountantAccountPage>();
            builder.Services.AddScoped<AdminAppShell>();
            builder.Services.AddScoped<StudentAppShell>();
            builder.Services.AddScoped<NavigationTopBar>();
            builder.Services.AddScoped<AdminDashboardPage>();
            builder.Services.AddScoped<AdminAccountantAccountManagementPage>();
            builder.Services.AddScoped<StudentAccountManagementPage>();
            builder.Services.AddScoped<ChangePasswordPage>();
            builder.Services.AddScoped<AddAdminAccountantAccountPage>();
            builder.Services.AddScoped<DepartmentManagementPage>();
            builder.Services.AddScoped<AddUpdateDepartmentPopup>();
            builder.Services.AddScoped<StudentManagementPage>();
            builder.Services.AddScoped<AddStudentPage>();

            // Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IDistrictService, DistrictService>();

			return builder.Build();
        }
    }
}