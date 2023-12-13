﻿using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.ViewModels;
using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
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
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<AdminAppShellViewModel>();
            builder.Services.AddScoped<AdminDashboardViewModel>();
            builder.Services.AddScoped<AdminAccountantAccountManagementViewModel>();
            builder.Services.AddScoped<ChangePasswordViewModel>();
            builder.Services.AddScoped<AddAdminAccountantAccountViewModel>();
            builder.Services.AddScoped<DepartmentManagementViewModel>();
            builder.Services.AddScoped<AddUpdateDepartmentViewModel>();
            builder.Services.AddScoped<BranchManagementViewModel>();
            builder.Services.AddScoped<AddUpdateBranchViewModel>();
            builder.Services.AddScoped<ProvinceDistrictManagementViewModel>();
            builder.Services.AddScoped<AddUpdateProvinceViewModel>();
            builder.Services.AddScoped<AddUpdateDistrictViewModel>();

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
            builder.Services.AddScoped<BranchManagementPage>();
            builder.Services.AddScoped<AddUpdateBranchPopup>();
            builder.Services.AddScoped<ProvinceDistrictManagementPage>();
            builder.Services.AddScoped<AddUpdateProvincePopup>();
            builder.Services.AddScoped<AddUpdateDistrictPopup>();

            // Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICurriculumService, CurriculumService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IDistrictService, DistrictService>();
            builder.Services.AddScoped<IStudentPriorityTypeService, StudentPriorityTypeService>();

            return builder.Build();
        }
    }
}