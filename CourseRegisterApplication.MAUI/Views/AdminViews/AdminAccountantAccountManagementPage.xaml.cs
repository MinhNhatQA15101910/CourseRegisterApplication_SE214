using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminAccountantAccountManagementPage : ContentPage
{
	public AdminAccountantAccountManagementPage(AdminAccountantAccountManagementViewModel adminAccountantAccountManagementViewModel)
	{
		InitializeComponent();
        BindingContext = adminAccountantAccountManagementViewModel;
    }
}