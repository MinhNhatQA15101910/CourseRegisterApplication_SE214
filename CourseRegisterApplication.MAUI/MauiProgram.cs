using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.ViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;
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
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<AdminAppShellViewModel>();
            builder.Services.AddScoped<AdminDashboardViewModel>();
            builder.Services.AddScoped<AdminAccountantAccountManagementViewModel>();
            builder.Services.AddScoped<ChangePasswordViewModel>();
            builder.Services.AddScoped<AddAdminAccountantAccountViewModel>();
            builder.Services.AddScoped<DepartmentManagementViewModel>();
            builder.Services.AddScoped<AddUpdateDepartmentViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<AdminAppShellViewModel>();
            builder.Services.AddTransient<AdminDashboardViewModel>();
            builder.Services.AddTransient<AdminAccountantAccountManagementViewModel>();
            builder.Services.AddTransient<StudentAccountManagementViewModel>();
            builder.Services.AddTransient<ChangePasswordViewModel>();
            builder.Services.AddTransient<AddAdminAccountantAccountViewModel>();
			builder.Services.AddTransient<StudentAppShellViewModel>();

			// Views
			builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<AdminAppShell>();
			builder.Services.AddTransient<StudentAppShell>();
			builder.Services.AddTransient<NavigationTopBar>();
            builder.Services.AddTransient<AdminDashboardPage>();
            builder.Services.AddTransient<AdminAccountantAccountManagementPage>();
            builder.Services.AddTransient<StudentAccountManagementPage>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<AddAdminAccountantAccountPage>();
            // Views
            builder.Services.AddScoped<LoginPage>();
            builder.Services.AddScoped<AdminAppShell>();
            builder.Services.AddScoped<NavigationTopBar>();
            builder.Services.AddScoped<AdminDashboardPage>();
            builder.Services.AddScoped<AdminAccountantAccountManagementPage>();
            builder.Services.AddScoped<ChangePasswordPage>();
            builder.Services.AddScoped<AddAdminAccountantAccountPage>();
            builder.Services.AddScoped<DepartmentManagementPage>();
            builder.Services.AddScoped<AddUpdateDepartmentPopup>();

            // Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IBranchService, BranchService>();

            return builder.Build();
        }
    }
}