using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.ViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;
using CourseRegisterApplication.MAUI.Views;
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
			builder.Services.AddTransient<LoginViewModel>();
			builder.Services.AddTransient<AdminAppShellViewModel>();
			builder.Services.AddTransient<AdminDashboardViewModel>();
			builder.Services.AddTransient<AdminAccountantAccountManagementViewModel>();
			builder.Services.AddTransient<ChangePasswordViewModel>();
			builder.Services.AddTransient<AddAdminAccountantAccountViewModel>();
			builder.Services.AddTransient<StudentAppShellViewModel>();
			builder.Services.AddTransient<StudentDashboardViewModel>();
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

			builder.Services.AddTransient<AdminAccountantAccountManagementPage>();
			builder.Services.AddTransient<ChangePasswordPage>();
			builder.Services.AddTransient<AddAdminAccountantAccountPage>();

			// Services
			builder.Services.AddSingleton<HttpClient>();
			builder.Services.AddScoped<IUserService, UserService>();

			return builder.Build();
        }
    }
}