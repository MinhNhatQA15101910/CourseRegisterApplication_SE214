using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminDashboardPage : ContentPage
{
	public AdminDashboardPage(AdminDashboardViewModel adminDashboardViewModel)
	{
		InitializeComponent();
		BindingContext = adminDashboardViewModel;
    }
}