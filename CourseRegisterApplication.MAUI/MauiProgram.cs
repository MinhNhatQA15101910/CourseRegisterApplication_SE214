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

            #region ViewModels
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<ChangePasswordViewModel>();

            #region Admin
            builder.Services.AddScoped<AdminAppShellViewModel>();
            builder.Services.AddScoped<AdminDashboardViewModel>();
            builder.Services.AddScoped<AdminAccountantAccountManagementViewModel>();
            builder.Services.AddScoped<StudentAccountManagementViewModel>();
            builder.Services.AddScoped<AddAdminAccountantAccountViewModel>();
            #endregion

            #region Student
            builder.Services.AddScoped<StudentAppShellViewModel>();
            builder.Services.AddScoped<StudentDashboardViewModel>();
            builder.Services.AddScoped<StudentInfomationViewModel>();
            #endregion

            #region Accountant
            builder.Services.AddScoped<AccountantAppShellViewModel>();
            builder.Services.AddScoped<AccountantDashboardViewModel>();
            builder.Services.AddScoped<DepartmentManagementViewModel>();
            builder.Services.AddScoped<AddUpdateDepartmentViewModel>();
            builder.Services.AddScoped<BranchManagementViewModel>();
            builder.Services.AddScoped<AddUpdateBranchViewModel>();
            builder.Services.AddScoped<ProvinceDistrictManagementViewModel>();
            builder.Services.AddScoped<AddUpdateProvinceViewModel>();
            builder.Services.AddScoped<AddUpdateDistrictViewModel>();
            builder.Services.AddScoped<StudentManagementViewModel>();
            builder.Services.AddScoped<AddUpdateStudentViewModel>();
            builder.Services.AddScoped<ConfirmCourseRegistrationViewModel>();
            builder.Services.AddScoped<ConfirmTuitionCollectionViewModel>();
            builder.Services.AddScoped<SubjectTypeManagementViewModel>();
            builder.Services.AddScoped<AvailableCourseManagementViewModel>();
            builder.Services.AddScoped<PriorityTypeManagementViewModel>();
            builder.Services.AddScoped<UnpayTuitionStudentsReportViewModel>();
            builder.Services.AddScoped<SubjectManagementViewModel>();
            #endregion
            #endregion

            #region Views
            builder.Services.AddScoped<NavigationTopBar>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddScoped<ChangePasswordPage>();

            #region Admin
            builder.Services.AddScoped<AdminAppShell>();
            builder.Services.AddScoped<AdminDashboardPage>();
            builder.Services.AddScoped<AdminAccountantAccountManagementPage>();
            builder.Services.AddScoped<StudentAccountManagementPage>();
            builder.Services.AddScoped<AddAdminAccountantAccountPage>();
            #endregion

            #region Accountant
            builder.Services.AddScoped<AccountantAppShell>();
            builder.Services.AddScoped<AddUpdateDepartmentPopup>();
            builder.Services.AddScoped<AccountantDashboardPage>();
            builder.Services.AddScoped<BranchManagementPage>();
            builder.Services.AddScoped<ConfirmCourseRegistrationPage>();
            builder.Services.AddScoped<SubjectTypeManagementPage>();
            builder.Services.AddScoped<DepartmentManagementPage>();
            builder.Services.AddScoped<AvailableCourseManagementPage>();
            builder.Services.AddScoped<PriorityTypeManagementPage>();
            builder.Services.AddScoped<UnpayTuitionStudentsReportPage>();
            builder.Services.AddScoped<CurriculumManagementPage>();
            builder.Services.AddScoped<SubjectManagementPage>();
            builder.Services.AddScoped<ConfirmTuitionCollectionPage>();
            builder.Services.AddScoped<BranchManagementPage>();
            builder.Services.AddScoped<AddUpdateBranchPopup>();
            builder.Services.AddScoped<ProvinceDistrictManagementPage>();
            builder.Services.AddScoped<AddUpdateProvincePopup>();
            builder.Services.AddScoped<AddUpdateDistrictPopup>();
            builder.Services.AddScoped<StudentManagementPage>();
            builder.Services.AddScoped<AddUpdateStudentPage>();
            #endregion

            #region Student
            builder.Services.AddScoped<StudentAppShell>();
            builder.Services.AddScoped<StudentDashboardPage>();
            builder.Services.AddScoped<StudentInfomationPage>();
            #endregion
            #endregion

            // Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICurriculumService, CurriculumService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IDistrictService, DistrictService>();
            builder.Services.AddScoped<IStudentPriorityTypeService, StudentPriorityTypeService>();
            builder.Services.AddScoped<IProvinceService, ProvinceService>();
            builder.Services.AddScoped<IDistrictService, DistrictService>();

			return builder.Build();
        }
    }
}