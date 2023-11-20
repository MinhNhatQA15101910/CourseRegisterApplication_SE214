using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.IViewModels;
using CourseRegisterApplication.MAUI.IViews;
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
            builder.Services.AddTransient<ChangePasswordViewModel>();

            builder.Services.AddTransient<AddUpdateAccountViewModel>();
            builder.Services.AddTransient<AdminFlyoutItem>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ChangePasswordPage>();

            builder.Services.AddTransient<AddUpdateAccountPage>();
            builder.Services.AddTransient<AdminAppShell>();
            builder.Services.AddTransient<AdminDashboardPage>();
            builder.Services.AddTransient<AdminEmployeeAccountManagementPage>();
            builder.Services.AddTransient<AdminFlyoutPage>();
            builder.Services.AddTransient<AdminMenuItem>();
            builder.Services.AddTransient<StudentAccountManagementPage>();

            // Services
            builder.Services.AddScoped<IUserService, UserService>();

            return builder.Build();
        }
    }
}