using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AccountantAppShell : Shell
{
	public AccountantAppShell(AccountantAppShellViewModel accountantAppShellViewModel)
	{
		InitializeComponent();
		BindingContext = accountantAppShellViewModel;

		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
		Routing.RegisterRoute(nameof(UpdateCurriculumPage), typeof(UpdateCurriculumPage));
    }
}