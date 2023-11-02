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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            // Services
            builder.Services.AddSingleton<IUserService, UserService>();

            // Views
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<AdminMenuItem>();
            builder.Services.AddTransient<AdminFlyoutPage>();
            builder.Services.AddTransient<AdminDashboardPage>();

            // ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddTransient<AdminFlyoutItem>();

            return builder.Build();
        }
    }
}