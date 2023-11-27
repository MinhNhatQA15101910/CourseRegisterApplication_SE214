using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminDashboardPage : ContentPage
{
	public AdminDashboardPage(AdminDashboardViewModel adminDashboardViewModel)
	{
		InitializeComponent();
		BindingContext = adminDashboardViewModel;
    }

    private void SelectionMouseEnter(object sender, PointerEventArgs e)
    {
        (sender as Grid).BackgroundColor = Color.FromArgb("#D3D3D3");
    }

    private void SelectionMouseExit(object sender, PointerEventArgs e)
    {
        (sender as Grid).BackgroundColor = Color.FromArgb("#FFFFFF");
    }
}