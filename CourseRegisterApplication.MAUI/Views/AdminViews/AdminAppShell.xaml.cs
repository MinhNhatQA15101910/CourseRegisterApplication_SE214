using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminAppShell : Shell
{
	public AdminAppShell(AdminAppShellViewModel adminAppShellViewModel)
	{
		InitializeComponent();
		BindingContext = adminAppShellViewModel;

		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
	}
}