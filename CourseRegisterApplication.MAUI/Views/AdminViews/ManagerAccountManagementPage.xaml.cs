using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class ManagerAccountManagementPage : ContentPage
{
	public ManagerAccountManagementPage(ManagerAccountManagementViewModel managerAccountManagementViewModel)
	{
		InitializeComponent();
        BindingContext = managerAccountManagementViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as ManagerAccountManagementViewModel).ResetAccountInformation();
        (BindingContext as ManagerAccountManagementViewModel).GetManagerAccountCommand.Execute(null);
    }
}