using CommunityToolkit.Maui.Views;
using CourseRegisterApplication.MAUI.ContentViews;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminAccountantAccountManagementPage : ContentPage
{
	public AdminAccountantAccountManagementPage(AdminAccountantAccountManagementViewModel adminAccountantAccountManagementViewModel)
	{
		InitializeComponent();
        BindingContext = adminAccountantAccountManagementViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as AdminAccountantAccountManagementViewModel).GetAdminAccountantAccountCommand.Execute(null);
    }
	void OnTapOpenPopupFilter(object sender, TappedEventArgs args)
	{
		this.ShowPopup(new ManagerAccountFilterPopup());

	}
}