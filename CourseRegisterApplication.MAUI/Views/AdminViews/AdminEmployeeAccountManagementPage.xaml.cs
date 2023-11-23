using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminEmployeeAccountManagementPage : ContentPage
{
	public AdminEmployeeAccountManagementPage(AdminEmployeeAccountManagementViewModel adminEmployeeAccountManagementViewModel)
	{
		InitializeComponent();
        BindingContext = adminEmployeeAccountManagementViewModel;
    }
}