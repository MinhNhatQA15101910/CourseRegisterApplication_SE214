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

namespace CourseRegisterApplication.MAUI;

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

        #region AdminViewModels
        builder.Services.AddScoped<AddManagerAccountViewModel>();
        builder.Services.AddScoped<ManagerAccountManagementViewModel>();
        builder.Services.AddScoped<AdminAppShellViewModel>();
        builder.Services.AddScoped<AdminDashboardViewModel>();
        builder.Services.AddScoped<StudentAccountManagementViewModel>();
        builder.Services.AddScoped<FilterManagerAccountViewModel>();
        builder.Services.AddScoped<FilterStudentAccountViewModel>();

        #endregion

        #region Student
        builder.Services.AddScoped<StudentAppShellViewModel>();
        builder.Services.AddScoped<StudentDashboardViewModel>(); 
        builder.Services.AddScoped<StudentInfomationViewModel>(); 
        builder.Services.AddScoped<CourseRegisTrationInfoViewModel>();
        builder.Services.AddScoped<CourseRegistrationViewModel>();
        builder.Services.AddScoped<TuitionInfoViewModel>();


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
        builder.Services.AddScoped<ConfirmTuitionCollectionViewModel>();
        builder.Services.AddScoped<SubjectTypeManagementViewModel>();
        builder.Services.AddScoped<AvailableCourseManagementViewModel>();
        builder.Services.AddScoped<PriorityTypeManagementViewModel>();
        builder.Services.AddScoped<UnpayTuitionStudentsReportViewModel>();
        builder.Services.AddScoped<SubjectManagementViewModel>();
        builder.Services.AddScoped<CourseConfirmationViewModel>();
        builder.Services.AddScoped<AddUpdateSubjectViewModel>();
        builder.Services.AddScoped<AddUpdateSubjectTypeViewModel>();
        #endregion
        #endregion

        #region Views
        builder.Services.AddTransient<NavigationTopBar>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddScoped<ChangePasswordPage>();

        #region Admin
        builder.Services.AddScoped<AdminAppShell>();
        builder.Services.AddScoped<AdminDashboardPage>();
        builder.Services.AddScoped<ManagerAccountManagementPage>();
        builder.Services.AddScoped<StudentAccountManagementPage>();
        builder.Services.AddScoped<AddManagerAccountPage>();
        builder.Services.AddScoped<FilterManagerAccountPopup>();
        builder.Services.AddScoped<FilterStudentAccountPopup>();
        #endregion

        #region Accountant
        builder.Services.AddScoped<AccountantAppShell>();
        builder.Services.AddScoped<AddUpdateDepartmentPopup>();
        builder.Services.AddScoped<AccountantDashboardPage>();
        builder.Services.AddScoped<BranchManagementPage>();
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
        builder.Services.AddScoped<CourseConfirmationPage>();
        builder.Services.AddScoped<AddUpdateSubjectPopup>();
        builder.Services.AddScoped<AddUpdateSubjectTypePopup>();
        #endregion

        #region Student
        builder.Services.AddScoped<StudentAppShell>();
        builder.Services.AddScoped<StudentDashboardPage>();
        builder.Services.AddScoped<StudentInfomationPage>(); 
        builder.Services.AddScoped<CourseRegistrationInfoPage>();
        builder.Services.AddScoped<CourseRegistrationPage>();
        builder.Services.AddScoped<PaymentPopup>();
        builder.Services.AddScoped<TuitionInfoPage>();




        #endregion
        #endregion

        #region Services
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddScoped<ITuitionFeeReceiptService, TuitionFeeReceiptService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IStudentService, StudentService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IBranchService, BranchService>();
        builder.Services.AddScoped<ICurriculumService, CurriculumService>();
        builder.Services.AddScoped<IStudentPriorityTypeService, StudentPriorityTypeService>();
        builder.Services.AddScoped<IProvinceService, ProvinceService>();
        builder.Services.AddScoped<IDistrictService, DistrictService>();
        builder.Services.AddScoped<IPriorityTypeService, PriorityTypeService>();
        builder.Services.AddScoped<ISemesterService, SemesterService>();
        builder.Services.AddScoped<ISubjectService, SubjectService>();
        builder.Services.AddScoped<IAvailableCourseService, AvailableCourseService>();
        builder.Services.AddScoped<ISubjectTypeService, SubjectTypeService>();
        builder.Services.AddScoped<ICourseRegistrationFormService, CourseRegistrationFormService>();
        builder.Services.AddScoped<ICourseRegistrationDetailService, CourseRegistrationDetailService>();
        #endregion

        return builder.Build();
    }
}