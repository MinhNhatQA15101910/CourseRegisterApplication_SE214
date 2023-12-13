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
using CourseRegisterApplication.MAUI.Views.AccountantViews;

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
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<AdminAppShellViewModel>();
            builder.Services.AddScoped<AdminDashboardViewModel>();
            builder.Services.AddScoped<AdminAccountantAccountManagementViewModel>();
            builder.Services.AddScoped<ChangePasswordViewModel>();
            builder.Services.AddScoped<AddAdminAccountantAccountViewModel>();
            builder.Services.AddScoped<DepartmentManagementViewModel>();
            builder.Services.AddScoped<AddUpdateDepartmentViewModel>();
            builder.Services.AddScoped<StudentAccountManagementViewModel>();
            builder.Services.AddScoped<StudentManagementViewModel>();
			builder.Services.AddScoped<StudentAppShellViewModel>();
            builder.Services.AddScoped<AddStudentViewModel>();
			// ViewModels
			builder.Services.AddTransient<LoginViewModel>();
			builder.Services.AddTransient<AdminAppShellViewModel>();
			builder.Services.AddTransient<AdminDashboardViewModel>();
			builder.Services.AddTransient<AdminAccountantAccountManagementViewModel>();
			builder.Services.AddTransient<ChangePasswordViewModel>();
			builder.Services.AddTransient<AddAdminAccountantAccountViewModel>();
			builder.Services.AddTransient<StudentAppShellViewModel>();
			builder.Services.AddTransient<StudentDashboardViewModel>();
			builder.Services.AddTransient<StudentInfomationViewModel>();
			builder.Services.AddTransient<AccountantAppShellViewModel>();
			builder.Services.AddTransient<AccountantDashboardViewModel>();
			builder.Services.AddTransient<BranchManagementViewModel>();
			builder.Services.AddTransient<CourseManagementViewModel>();
			builder.Services.AddTransient<CreditsSubjectTypeViewModel>();
			builder.Services.AddTransient<DepantmentManagementViewModel>();
			builder.Services.AddTransient<OpenSubjectViewModel>();
			builder.Services.AddTransient<PriorityObjectViewModel>();
			builder.Services.AddTransient<ProvinceDistrictViewModel>();
			builder.Services.AddTransient<StatisticViewModel>();
			builder.Services.AddTransient<StudentManagermentViewModel>();
			builder.Services.AddTransient<StudyProgramViewModel>();
			builder.Services.AddTransient<SubjectManagementViewModel>();
			builder.Services.AddTransient<TuitionCollectionViewModel>();

			// Views
			builder.Services.AddTransient<LoginPage>();
			builder.Services.AddTransient<AdminAppShell>();
			builder.Services.AddTransient<StudentAppShell>();
			builder.Services.AddTransient<AccountantAppShell>();
			builder.Services.AddTransient<NavigationTopBar>();
			builder.Services.AddTransient<AdminDashboardPage>();
			builder.Services.AddTransient<StudentDashboardPage>();
			builder.Services.AddTransient<StudentInfomationPage>();
			builder.Services.AddTransient<AccountantDashboardPage>();
			builder.Services.AddTransient<BranchManagementPage>();
			builder.Services.AddTransient<CourseManagementPage>();
			builder.Services.AddTransient<CreditsSubjectTypePage>();
			builder.Services.AddTransient<DepantmentManagementPage>();
			builder.Services.AddTransient<OpenSubjectPage>();
			builder.Services.AddTransient<PriorityObjectPage>();
			builder.Services.AddTransient<ProvinceDistrictPage>();
			builder.Services.AddTransient<StatisticPage>();
			builder.Services.AddTransient<StudentManagementPage>();
			builder.Services.AddTransient<StudyProgramPage>();
			builder.Services.AddTransient<SubjectManagementPage>();
			builder.Services.AddTransient<TuitionCollectionPage>();
			builder.Services.AddTransient<ManagerAccountFilterPopup>();

			builder.Services.AddTransient<AdminAccountantAccountManagementPage>();
			builder.Services.AddTransient<ChangePasswordPage>();
			builder.Services.AddTransient<AddAdminAccountantAccountPage>();
            builder.Services.AddScoped<LoginPage>();
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