using CourseRegisterApplication_SE214.ViewModels.AdminViewModels;

namespace CourseRegisterApplication_SE214.Views.AdminViews;

public partial class AdminMenuItem : ContentPage
{
	public event EventHandler<Type> PageChanged;
	public AdminMenuItem()
	{
		InitializeComponent();
		BindingContext = new AdminFlyoutItem();
	}
	private void OnItemTapped(object sender, ItemTappedEventArgs e)
	{
		if (e.Item is MenuItems menuItem)
		{
			Type pageType = null;

			if (menuItem.Title == "Dashboard")
			{
				pageType = typeof(AdminDashboardPage);
			}
			else if (menuItem.Title == "Accountant account")
			{
				pageType = typeof(EmployeeAccountManagementPage);
			}
			else if (menuItem.Title == "Student account")
			{
				pageType = typeof(StudentAccountManagementPage);
			}
			else if (menuItem.Title == "Admin account")
			{
				pageType = typeof(AdminAccountManagementPage);
			}


			PageChanged?.Invoke(this, pageType);
		}
	}
}