using CommunityToolkit.Maui;
using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.ViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

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

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<AdminAppShell>();
            builder.Services.AddTransient<NavigationTopBar>();
            builder.Services.AddTransient<AdminDashboardPage>();
            builder.Services.AddTransient<AdminAccountantAccountManagementPage>();
            builder.Services.AddTransient<ChangePasswordPage>();

            // Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IUserService, UserService>();

            return builder.Build();
        }
    }
}