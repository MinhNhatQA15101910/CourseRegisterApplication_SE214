using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AccountantDashboardPage : ContentPage
{
	public AccountantDashboardPage(AccountantDashboardViewModel accountantDashboardViewModel)
	{
		InitializeComponent();
		BindingContext = accountantDashboardViewModel;
    }
}